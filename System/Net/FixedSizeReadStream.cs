using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x02000683 RID: 1667
	internal class FixedSizeReadStream : WebReadStream
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x000B79EF File Offset: 0x000B5BEF
		public long ContentLength { get; }

		// Token: 0x0600348E RID: 13454 RVA: 0x000B79F7 File Offset: 0x000B5BF7
		public FixedSizeReadStream(WebOperation operation, Stream innerStream, long contentLength)
			: base(operation, innerStream)
		{
			this.ContentLength = contentLength;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000B7A08 File Offset: 0x000B5C08
		protected override async Task<int> ProcessReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			long num = this.ContentLength - this.position;
			int num2;
			if (num == 0L)
			{
				num2 = 0;
			}
			else
			{
				int num3 = (int)Math.Min(num, (long)size);
				int num4 = await base.InnerStream.ReadAsync(buffer, offset, num3, cancellationToken).ConfigureAwait(false);
				if (num4 <= 0)
				{
					num2 = num4;
				}
				else
				{
					this.position += (long)num4;
					num2 = num4;
				}
			}
			return num2;
		}

		// Token: 0x04001EA1 RID: 7841
		private long position;
	}
}
