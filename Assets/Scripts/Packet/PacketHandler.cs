using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using ServerCore;
using Protocol;
using UnityEngine;

public sealed class PacketHandler
{
  public static void SC_RegisterResHandler(PacketSession session, IMessage packet)
  {
    var res = packet as SC_REGISTER_RES;

    MainSceneUIManager.Instance.HandleMessage(res);
  }

  public static void SC_LoginResHandler(PacketSession session, IMessage packet)
  {
    var res = packet as SC_LOGIN_RES;

    MainSceneUIManager.Instance.HandleMessage(res);
  }

  public static void SC_ChatNotiHandler(PacketSession session, IMessage packet)
  {
    var res = packet as SC_CHAT_NOTI;

    RoomUIManager.Instance.HandleMessage(res);
  }
}

