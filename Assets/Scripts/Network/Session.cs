using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace ServerCore
{
  public abstract class PacketSession : Session
  {
    public static readonly int HeaderSize = 2;

    public sealed override int OnRecv(ArraySegment<byte> buffer)
    {
      int processLen = 0;
      int packetCount = 0;

      while (true)
      {
        if (buffer.Count < HeaderSize) break;

        var dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        if (buffer.Count < dataSize) break;

        OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
        packetCount++;

        processLen += dataSize;
        buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
      }

      if (packetCount > 1) Console.WriteLine($"패킷 모아 보내기 : {packetCount}");

      return processLen;
    }

    public abstract void OnRecvPacket(ArraySegment<byte> buffer);
  }

  public abstract class Session
  {
    Socket socket;
    int disconnected = 0;
    RecvBuffer recvBuffer = new RecvBuffer(65535);

    object _lock = new object();

    Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
    List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();
    SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
    SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();

    public abstract void OnConnected(EndPoint endPoint);
    public abstract int OnRecv(ArraySegment<byte> buffer);
    public abstract void OnSend(int numOfBytes);
    public abstract void OnDisconnected(EndPoint endPoint);

    private void Clear()
    {
      lock(this._lock)
      {
        this.sendQueue.Clear();
        this.pendingList.Clear();
      }
    }

    public void Start(Socket socket)
    {
      this.socket = socket;
      this.recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
      this.sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

      RegisterRecv();
    }

    public void Send(List<ArraySegment<byte>> sendBuffList)
    {
      if (sendBuffList.Count == 0) return;

      lock(this._lock)
      {
        foreach (var sendBuffer in sendBuffList) this.sendQueue.Enqueue(sendBuffer);

        if (this.pendingList.Count == 0) RegisterSend();
      }
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
      lock (this._lock)
      {
        this.sendQueue.Enqueue(sendBuff);
        if (this.pendingList.Count == 0) RegisterSend();
      }
    }

    public void Disconnect()
    {
      if (Interlocked.Exchange(ref this.disconnected, 1) == 1) return;

      OnDisconnected(this.socket.RemoteEndPoint);
      this.socket.Shutdown(SocketShutdown.Both);
      this.socket.Close();
      Clear();
    }

    #region Network
    private void RegisterSend()
    {
      if (this.disconnected == 1) return;

      while(this.sendQueue.Count > 0)
      {
        var buff = this.sendQueue.Dequeue();
        this.pendingList.Add(buff);
      }

      this.sendArgs.BufferList = this.pendingList;

      try
      {
        bool pending = this.socket.SendAsync(this.sendArgs);
        if (!pending) OnSendCompleted(null, this.sendArgs);
      } 
      catch (Exception e)
      {
        Debug.Log($"RegisterSend failed {e}");
      }
    }

    private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
    {
      lock (this._lock)
      {
        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
        {
          try
          {
            this.sendArgs.BufferList = null;
            this.pendingList.Clear();

            OnSend(this.sendArgs.BytesTransferred);

            if (this.sendQueue.Count > 0) RegisterSend();
          }
          catch (Exception e)
          {
            Debug.Log($"OnSendCompleted failed {e}");
          }
        }
        else
        {
          Disconnect();
        }
      }
    }

    private void RegisterRecv()
    {
      if (this.disconnected == 1) return;

      this.recvBuffer.Clean();
      var segment = this.recvBuffer.WriteSegment;
      this.recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

      try
      {
        bool pending = this.socket.ReceiveAsync(this.recvArgs);
        if (!pending) OnRecvCompleted(null, this.recvArgs);
      }
      catch (Exception e)
      {
        Debug.Log($"RegisterRecv failed {e}");
      }
    }

    private void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
    {
      if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
      {
        try
        {
          if (!this.recvBuffer.OnWrite(args.BytesTransferred))
          {
            Disconnect();
            return;
          }

          int processLen = OnRecv(this.recvBuffer.ReadSegment);
          if (processLen < 0 || this.recvBuffer.DataSize < processLen)
          {
            Disconnect();
            return;
          }

          if (!this.recvBuffer.OnRead(processLen))
          {
            Disconnect();
            return;
          }

          RegisterRecv();
        }
        catch (Exception e)
        {
          Debug.Log($"OnRecvCompleted failed {e}");
        }
      }
      else
      {
        Disconnect();
      }
    }
    #endregion
  }
}