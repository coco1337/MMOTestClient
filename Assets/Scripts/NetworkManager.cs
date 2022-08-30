using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using Protocol;
using ServerCore;

public sealed class NetworkManager : MonoBehaviour
{
  public static NetworkManager Instance;
  public static ulong Uid { get; private set; }

  private ServerSession session = new ServerSession();

  public void SetUid(ulong uid) => Uid = uid;
  
  public void Send(IMessage packet, PacketId msgId)
  {
    this.session.Send(packet, msgId);
  }

  private void Awake()
  {
    DontDestroyOnLoad(this);
  }

  private void Start()
  {
    var host = Dns.GetHostName();
    var ipHost = Dns.GetHostEntry(host);
    var ipAddr = ipHost.AddressList[0];
    var endPoint = new IPEndPoint(ipAddr, 8888);

    var connector = new Connector();

    connector.Connect(endPoint, () => { return this.session; }, 1);

    Instance ??= this;
  }

  private void Update()
  {
    var list = PacketQueue.Instance.PopAll();
    foreach (var packet in list)
    {
      var handler = PacketManager.Instance.GetPacketHandler(packet.Id);
      if (handler != null) handler.Invoke(this.session, packet.Message);
    }
  }

  private void OnApplicationQuit()
  {
    session.Disconnect();
  }
}
