using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036F RID: 879
	[GenerateTestsForBurstCompatibility]
	public struct BurstableMemoryBinaryReader : BinaryReader, IDisposable
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x00058635 File Offset: 0x00056835
		// (set) Token: 0x06001791 RID: 6033 RVA: 0x0005863D File Offset: 0x0005683D
		public long Position { readonly get; set; }

		// Token: 0x06001792 RID: 6034 RVA: 0x00058646 File Offset: 0x00056846
		public unsafe BurstableMemoryBinaryReader(byte* content, long length)
		{
			this.content = content;
			this.length = length;
			this.Position = 0L;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00002287 File Offset: 0x00000487
		public void Dispose()
		{
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0005865E File Offset: 0x0005685E
		public unsafe byte ReadByte()
		{
			byte b = this.content[this.Position];
			this.Position += 1L;
			return b;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0005867E File Offset: 0x0005687E
		public unsafe int ReadInt()
		{
			int num = *(int*)(this.content + this.Position);
			this.Position += 4L;
			return num;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0005869E File Offset: 0x0005689E
		public unsafe ulong ReadULong()
		{
			ulong num = (ulong)(*(long*)(this.content + this.Position));
			this.Position += 8L;
			return num;
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000586BE File Offset: 0x000568BE
		public unsafe void ReadBytes(void* data, int bytes)
		{
			UnsafeUtility.MemCpy(data, (void*)(this.content + this.Position), (long)bytes);
			this.Position += (long)bytes;
		}

		// Token: 0x040008D8 RID: 2264
		internal unsafe byte* content;

		// Token: 0x040008D9 RID: 2265
		internal long length;
	}
}
