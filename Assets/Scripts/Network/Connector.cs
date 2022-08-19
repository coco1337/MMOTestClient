using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerCore
{
  public class Connector
  {
    Func<Session> sessionFactory;

    public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 1)
    {
      for (int i = 0; i < count; ++i)
      {
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        this.sessionFactory = sessionFactory;

        var args = new SocketAsyncEventArgs()
        {
          RemoteEndPoint = endPoint,
          UserToken = socket,
        };

        args.Completed += OnConnectCompleted;

        RegisterConnect(args);
      }
    }

    private void RegisterConnect(SocketAsyncEventArgs args)
    {
      var socket = args.UserToken as Socket;
      if (socket == null) return;

      bool pending = socket.ConnectAsync(args);
      if (!pending) OnConnectCompleted(null, args);
    }

    private void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
    {
      if (args.SocketError == SocketError.Success)
      {
        var session = this.sessionFactory.Invoke();
        session.Start(args.ConnectSocket);
        session.OnConnected(args.RemoteEndPoint);
      }
      else
      {
        Console.WriteLine($"OnConnectCompleted failed : {args.SocketError}");
      }
    }
  }
}
