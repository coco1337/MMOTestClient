using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

public class PacketMessage
{
  public ushort Id { get; set; }
  public IMessage Message { get; set; }
}

public sealed class PacketQueue
{
  public static PacketQueue Instance { get; } = new PacketQueue();

  Queue<PacketMessage> packetQueue = new Queue<PacketMessage>();
  object _lock = new object();

  public void Push(ushort id, IMessage packet)
  {
    lock (_lock)
      this.packetQueue.Enqueue(new PacketMessage() { Id = id, Message = packet });
  }

  public PacketMessage Pop()
  {
    lock (_lock)
    {
      if (this.packetQueue.Count == 0) return null;
      return this.packetQueue.Dequeue();
    }
  }

  public List<PacketMessage> PopAll()
  {
    var list = new List<PacketMessage>();

    lock (_lock)
    {
      while (this.packetQueue.Count > 0)
        list.Add(this.packetQueue.Dequeue());
    }

    return list;
  }
}

