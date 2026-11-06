using System;
using System.IO;

namespace Unity.Serialization.Json
{
	// Token: 0x0200006E RID: 110
	internal class SyncBlockReader : BlockStreamReader
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000B388 File Offset: 0x00009588
		public SyncBlockReader(Stream input, int bufferSize, bool leaveInputOpen)
			: base(input, bufferSize, leaveInputOpen)
		{
			int num = bufferSize / 2;
			this.m_Buffer = new ManagedCharBuffer(num);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B3B0 File Offset: 0x000095B0
		public override BlockInfo GetNextBlock()
		{
			int num = this.Reader.ReadBlock(this.m_Buffer.GetManagedArray(), 0, this.m_Buffer.GetManagedArray().Length);
			return new BlockInfo
			{
				Block = new UnsafeBuffer<char>(this.m_Buffer.GetUnsafePtr(), num),
				IsFinal = (num != this.m_Buffer.GetManagedArray().Length)
			};
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B41D File Offset: 0x0000961D
		public override void Dispose()
		{
			base.Dispose();
			this.m_Buffer.Dispose();
		}

		// Token: 0x04000150 RID: 336
		private ManagedCharBuffer m_Buffer;
	}
}
