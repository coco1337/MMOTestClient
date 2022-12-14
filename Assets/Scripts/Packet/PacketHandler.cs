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

  public static void SC_SpawnResHandler(PacketSession session, IMessage packet)
  {
    var res = packet as SC_SPAWN_RES;
    Log.Debug("OnSpawnRes");

    RoomManager.Instance.HandleMessage(res);
  }

  public static void SC_SpawnNotiHandler(PacketSession session, IMessage packet)
  {
    var noti = packet as SC_SPAWN_NOTI;
    Log.Debug($"{noti.Player.Uid} spawn noti");

    RoomManager.Instance.HandleMessage(noti);
  }

  public static void SC_MovedataNotiHandler(PacketSession session, IMessage packet)
  {
    var noti = packet as SC_MOVEDATA_NOTI;
    if (noti == null || RoomManager.Instance == null)
    {
      return;
    }
    Log.Debug($"{noti.Uid} Move noti");
    RoomManager.Instance.HandleMessage(noti);
  }

  public static void SC_DespawnNotiHandler(PacketSession session, IMessage packet)
  {
    var noti = packet as SC_DESPAWN_NOTI;

    RoomManager.Instance.HandleMessage(noti);
  }
}

