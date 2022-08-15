using System;
using System.Threading;

namespace ServerCore
{
  public class SendBufferHelper
  {
    public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => { return null; });
    public static int ChunkSize = 65535;
    public static ArraySegment<byte> Close(int usedSize) => CurrentBuffer.Value.Close(usedSize);
    public static ArraySegment<byte> Open(int reserveSize)
    {
      if (CurrentBuffer.Value == null) CurrentBuffer.Value = new SendBuffer(ChunkSize);
      if (CurrentBuffer.Value.FreeSize < reserveSize) CurrentBuffer.Value = new SendBuffer(ChunkSize);

      return CurrentBuffer.Value.Open(reserveSize);
    }
  }

  public sealed class SendBuffer
  {
    byte[] buffer;
    int usedSize = 0;

    public int FreeSize => this.buffer.Length - usedSize;
    public SendBuffer(int chunkSize) => this.buffer = new byte[chunkSize];
    public ArraySegment<byte> Open(int reserveSize) => new ArraySegment<byte>(this.buffer, this.usedSize, reserveSize);

    public ArraySegment<byte> Close(int usedSize)
    {
      var segment = new ArraySegment<byte>(this.buffer, this.usedSize, usedSize);
      this.usedSize += usedSize;
      return segment;
    }
  }
}
