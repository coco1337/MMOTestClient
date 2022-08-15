using System;

namespace ServerCore
{
  public sealed class RecvBuffer
  {
    ArraySegment<byte> buffer;
    int readPos;
    int writePos;

    public RecvBuffer(int bufferSize)
    {
      this.buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
    }

    public int DataSize => this.writePos - this.readPos;
    public int FreeSize => this.buffer.Count - this.writePos;
    public ArraySegment<byte> ReadSegment => new ArraySegment<byte>(this.buffer.Array, this.buffer.Offset + this.readPos, DataSize);
    public ArraySegment<byte> WriteSegment => new ArraySegment<byte>(this.buffer.Array, this.buffer.Offset + this.writePos, FreeSize);

    public void Clean()
    {
      int dataSize = DataSize;
      if (dataSize == 0)
      {
        this.readPos = this.writePos = 0;
      } 
      else
      {
        Array.Copy(this.buffer.Array, this.buffer.Offset + this.readPos, this.buffer.Array, this.buffer.Offset, dataSize);
        this.readPos = 0;
        this.writePos = dataSize;
      }
    }

    public bool OnRead(int numOfBytes)
    {
      if (numOfBytes > DataSize) return false;

      this.readPos += numOfBytes;
      return true;
    }

    public bool OnWrite(int numOfBytes)
    {
      if (numOfBytes > FreeSize) return false;

      this.writePos += numOfBytes;
      return true;
    }
  }
}
