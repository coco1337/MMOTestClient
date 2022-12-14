using ServerCore;
using System;
using System.Collections.Generic;
using Google.Protobuf;
using Protocol;

public enum PacketId : ushort
{
  PKT_CS_REGISTER_REQ = 1000,
  PKT_SC_REGISTER_RES = 1001,
  PKT_CS_LOGIN_REQ = 1002,
  PKT_SC_LOGIN_RES = 1003,
  PKT_CS_SEND_CHAT_REQ = 1004,
  PKT_SC_CHAT_NOTI = 1005,
  PKT_CS_SPAWN_REQ = 1006,
  PKT_SC_SPAWN_RES = 1007,
  PKT_SC_SPAWN_NOTI = 1008,
  PKT_SC_DESPAWN_NOTI = 1009,
  PKT_CS_MOVE_REQ = 1010,
  PKT_SC_MOVEDATA_NOTI = 1011,
}

public class PacketManager
{
  private static PacketManager instance = new PacketManager();
  public static PacketManager Instance => instance;

  PacketManager() => Register();

  Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
  Dictionary<ushort, Action<PacketSession, IMessage>> handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();

  public void Register()
  {
    onRecv.Add((ushort)PacketId.PKT_SC_REGISTER_RES, MakePacket<SC_REGISTER_RES>);
    handler.Add((ushort)PacketId.PKT_SC_REGISTER_RES, PacketHandler.SC_RegisterResHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_LOGIN_RES, MakePacket<SC_LOGIN_RES>);
    handler.Add((ushort)PacketId.PKT_SC_LOGIN_RES, PacketHandler.SC_LoginResHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_CHAT_NOTI, MakePacket<SC_CHAT_NOTI>);
    handler.Add((ushort)PacketId.PKT_SC_CHAT_NOTI, PacketHandler.SC_ChatNotiHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_SPAWN_RES, MakePacket<SC_SPAWN_RES>);
    handler.Add((ushort)PacketId.PKT_SC_SPAWN_RES, PacketHandler.SC_SpawnResHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_SPAWN_NOTI, MakePacket<SC_SPAWN_NOTI>);
    handler.Add((ushort)PacketId.PKT_SC_SPAWN_NOTI, PacketHandler.SC_SpawnNotiHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_MOVEDATA_NOTI, MakePacket<SC_MOVEDATA_NOTI>);
    handler.Add((ushort)PacketId.PKT_SC_MOVEDATA_NOTI, PacketHandler.SC_MovedataNotiHandler);
    onRecv.Add((ushort)PacketId.PKT_SC_DESPAWN_NOTI, MakePacket<SC_DESPAWN_NOTI>);
    handler.Add((ushort)PacketId.PKT_SC_DESPAWN_NOTI, PacketHandler.SC_DespawnNotiHandler);
  }

  public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
  {
    ushort count = 0;

    var size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
    count += 2;
    var id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
    count += 2;
    
    if (onRecv.TryGetValue(id, out var action))
      action.Invoke(session, buffer, id);
  }

  private void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
  {
    var pkt = new T();

    pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
    //if (handler.TryGetValue(id, out var action))
    //  action.Invoke(session, pkt);
    PacketQueue.Instance.Push(id, pkt);
  }

  public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
  {
    if (handler.TryGetValue(id, out var action)) return action;
    return null;
  }
}