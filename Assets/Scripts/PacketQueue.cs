using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

public sealed class PacketQueue
{
  public static PacketQueue Instance { get; } = new PacketQueue();

  Queue<IMessage> packetQueue = new Queue<IMessage>();
  object _lock = new object();

  public void Push(IMessage packet)
  {
    lock(_lock)
      this.packetQueue.Enqueue(packet);
  }

  public IMessage Pop()
  {
    lock (_lock)
    {
      if (this.packetQueue.Count == 0) return null;
      return this.packetQueue.Dequeue();
    }
  }

  public List<IMessage> PopAll()
  {
    var list = new List<IMessage>();

    lock (_lock)
    {
      while (this.packetQueue.Count > 0)
        list.Add(this.packetQueue.Dequeue());
    }

    return list;
  }
}

