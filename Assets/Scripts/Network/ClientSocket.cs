using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ClientSocket
{
  private static ManualResetEvent connectDone = new ManualResetEvent(false);
  private static ManualResetEvent sendDone = new ManualResetEvent(false);
  private static ManualResetEvent receiveDone = new ManualResetEvent(false);

  private static string response = string.Empty;

  public static void StartClient(string host, int port)
  {
    try
    {
      var ipHostInfo = Dns.GetHostEntry(host);
      var ipAddr = ipHostInfo.AddressList[0];
      var remoteEp = new IPEndPoint(ipAddr, port);

      var client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      client.BeginConnect(remoteEp, new AsyncCallback(ConnectCallback), client);
      connectDone.WaitOne();
    }
    catch (Exception e)
    {

    }
  }

  private static void ConnectCallback(IAsyncResult ar)
  {

  }

  private static void Receive(Socket client)
  {

  }

  private static void ReceiveCallback(IAsyncResult ar)
  {

  }

  private static void Send(Socket client)
  {

  }

  private static void SendCallback(IAsyncResult ar)
  {

  }
}