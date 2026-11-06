using System;
using System.IO;
using System.Text;

namespace Unity.Serialization.Json
{
	// Token: 0x0200006C RID: 108
	internal abstract class BlockStreamReader : IUnsafeStreamBlockReader, IDisposable
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000B15B File Offset: 0x0000935B
		protected BlockStreamReader(Stream input, int bufferSize, bool leaveInputOpen)
		{
			this.Reader = new StreamReader(input, Encoding.UTF8, false, bufferSize, leaveInputOpen);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000B177 File Offset: 0x00009377
		public virtual void Dispose()
		{
			this.Reader.Dispose();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B184 File Offset: 0x00009384
		public virtual void Reset()
		{
			this.Reader.DiscardBufferedData();
		}

		// Token: 0x0600028A RID: 650
		public abstract BlockInfo GetNextBlock();

		// Token: 0x0400014C RID: 332
		protected readonly StreamReader Reader;
	}
}
