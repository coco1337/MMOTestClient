using System;
using System.Net;
using ServerCore;
using UnityEngine;
using Google.Protobuf;

public class ServerSession : PacketSession
{
  public void Send(IMessage packet, PacketId msgId)
  {
    string msgName = packet.Descriptor.Name;
    var size = (ushort)packet.CalculateSize();
    var sendBuffer = new byte[size + 4];

    Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
    Array.Copy(BitConverter.GetBytes((ushort)(msgId)), 0, sendBuffer, 2, sizeof(ushort));
    Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
    Send(new ArraySegment<byte>(sendBuffer));
  }

  public override void OnConnected(EndPoint endPoint)
  {
    Log.Debug($"OnConnected : {endPoint}");
  }

  public override void OnDisconnected(EndPoint endPoint)
  {
    Log.Debug($"OnDisconnected : {endPoint}");
  }

  public override void OnRecvPacket(ArraySegment<byte> buffer)
  {
    PacketManager.Instance.OnRecvPacket(this, buffer);
  }

  public override void OnSend(int numOfBytes)
  {
    Log.Debug($"Transferred bytes : {numOfBytes}");
  }
}
