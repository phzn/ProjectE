using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036E RID: 878
	public class MemoryBinaryReader : BinaryReader, IDisposable
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x000585A1 File Offset: 0x000567A1
		// (set) Token: 0x0600178B RID: 6027 RVA: 0x000585A9 File Offset: 0x000567A9
		public long Position { get; set; }

		// Token: 0x0600178C RID: 6028 RVA: 0x000585B2 File Offset: 0x000567B2
		public unsafe MemoryBinaryReader(byte* content, long length)
		{
			this.content = content;
			this.length = length;
			this.Position = 0L;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00002287 File Offset: 0x00000487
		public void Dispose()
		{
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000585D0 File Offset: 0x000567D0
		public unsafe void ReadBytes(void* data, int bytes)
		{
			UnsafeUtility.MemCpy(data, (void*)(this.content + this.Position), (long)bytes);
			this.Position += (long)bytes;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000585F8 File Offset: 0x000567F8
		public static explicit operator BurstableMemoryBinaryReader(MemoryBinaryReader src)
		{
			return new BurstableMemoryBinaryReader
			{
				content = src.content,
				length = src.length,
				Position = src.Position
			};
		}

		// Token: 0x040008D5 RID: 2261
		private unsafe readonly byte* content;

		// Token: 0x040008D6 RID: 2262
		private readonly long length;
	}
}
