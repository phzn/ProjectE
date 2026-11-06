using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x0200066C RID: 1644
	internal class BufferedReadStream : WebReadStream
	{
		// Token: 0x060033DF RID: 13279 RVA: 0x000B4AE2 File Offset: 0x000B2CE2
		public BufferedReadStream(WebOperation operation, Stream innerStream, BufferOffsetSize readBuffer)
			: base(operation, innerStream)
		{
			this.readBuffer = readBuffer;
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000B4AF4 File Offset: 0x000B2CF4
		protected override async Task<int> ProcessReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			BufferOffsetSize bufferOffsetSize = this.readBuffer;
			int num = ((bufferOffsetSize != null) ? bufferOffsetSize.Size : 0);
			int num3;
			if (num > 0)
			{
				int num2 = ((num > size) ? size : num);
				Buffer.BlockCopy(this.readBuffer.Buffer, this.readBuffer.Offset, buffer, offset, num2);
				this.readBuffer.Offset += num2;
				this.readBuffer.Size -= num2;
				offset += num2;
				size -= num2;
				num3 = num2;
			}
			else if (base.InnerStream == null)
			{
				num3 = 0;
			}
			else
			{
				num3 = await base.InnerStream.ReadAsync(buffer, offset, size, cancellationToken).ConfigureAwait(false);
			}
			return num3;
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000B4B58 File Offset: 0x000B2D58
		internal bool TryReadFromBuffer(byte[] buffer, int offset, int size, out int result)
		{
			BufferOffsetSize bufferOffsetSize = this.readBuffer;
			int num = ((bufferOffsetSize != null) ? bufferOffsetSize.Size : 0);
			if (num <= 0)
			{
				result = 0;
				return base.InnerStream == null;
			}
			int num2 = ((num > size) ? size : num);
			Buffer.BlockCopy(this.readBuffer.Buffer, this.readBuffer.Offset, buffer, offset, num2);
			this.readBuffer.Offset += num2;
			this.readBuffer.Size -= num2;
			offset += num2;
			size -= num2;
			result = num2;
			return true;
		}

		// Token: 0x04001E5D RID: 7773
		private readonly BufferOffsetSize readBuffer;
	}
}
