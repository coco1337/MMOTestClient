using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using ServerCore;

public sealed class NetworkManager : MonoBehaviour
{
  ServerSession session = new ServerSession();
  
  public void Send(ArraySegment<byte> sendBuff)
  {
    this.session.Send(sendBuff);
  }

  private void Start()
  {
    var host = Dns.GetHostName();
    var ipHost = Dns.GetHostEntry(host);
    var ipAddr = ipHost.AddressList[0];
    var endPoint = new IPEndPoint(ipAddr, 7777);

    var connector = new Connector();

    connector.Connect(endPoint, () => { return this.session; }, 1);
  }

  private void Update()
  {
    var list = PacketQueue.Instance.PopAll();
    foreach (var packet in list)
    {
      // PacketManager.Instance.HandlePacket(this.session, packet);
    }
  }
}
