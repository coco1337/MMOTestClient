using ServerCore;
using System;
using System.Collections.Generic;
using Google.Protobuf;
using Protocol;

public class PacketManager
{
  private static PacketManager instance = new PacketManager();
  public static PacketManager Instance => instance;

  PacketManager() => Register();

  Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
  Dictionary<ushort, Action<PacketSession, IMessage>> handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();

  public void Register()
  {
    onRecv.Add((ushort)1000, MakePacket<SC_TEST>);
    handler.Add((ushort)1000, PacketHandler.SC_TestHandler);
    onRecv.Add((ushort)1001, MakePacket<CS_TEST>);
    handler.Add((ushort)1001, PacketHandler.CS_TestHandler);
    onRecv.Add((ushort)1002, MakePacket<CS_LOGIN>);
    handler.Add((ushort)1002, PacketHandler.CS_LoginHandler);
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
    if (handler.TryGetValue(id, out var action))
      action.Invoke(session, pkt);
  }

  public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
  {
    if (handler.TryGetValue(id, out var action)) return action;
    return null;
  }
}