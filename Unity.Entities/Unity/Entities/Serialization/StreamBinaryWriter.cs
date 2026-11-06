using System;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036C RID: 876
	internal class StreamBinaryWriter : BinaryWriter, IDisposable
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0005843B File Offset: 0x0005663B
		// (set) Token: 0x0600177D RID: 6013 RVA: 0x00058448 File Offset: 0x00056648
		public long Position
		{
			get
			{
				return this.stream.Position;
			}
			set
			{
				this.stream.Position = value;
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00058456 File Offset: 0x00056656
		public StreamBinaryWriter(string fileName, int bufferSize = 65536)
		{
			this.stream = File.Open(fileName, FileMode.Create, FileAccess.Write);
			this.buffer = new byte[bufferSize];
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00058478 File Offset: 0x00056678
		public void Dispose()
		{
			this.stream.Dispose();
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00058488 File Offset: 0x00056688
		public unsafe void WriteBytes(void* data, int bytes)
		{
			int num = bytes;
			int num2 = this.buffer.Length;
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			while (num != 0)
			{
				int num3 = Math.Min(num, num2);
				UnsafeUtility.MemCpy((void*)ptr, data, (long)num3);
				this.stream.Write(this.buffer, 0, num3);
				data = (void*)((byte*)data + num3);
				num -= num3;
			}
			array = null;
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x000584F6 File Offset: 0x000566F6
		public long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x040008D1 RID: 2257
		private Stream stream;

		// Token: 0x040008D2 RID: 2258
		private byte[] buffer;
	}
}
