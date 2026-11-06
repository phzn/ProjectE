using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006A5 RID: 1701
	internal class MonoChunkStream : WebReadStream
	{
		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x000BF2C1 File Offset: 0x000BD4C1
		protected WebHeaderCollection Headers { get; }

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x000BF2C9 File Offset: 0x000BD4C9
		protected MonoChunkParser Decoder { get; }

		// Token: 0x06003676 RID: 13942 RVA: 0x000BF2D1 File Offset: 0x000BD4D1
		public MonoChunkStream(WebOperation operation, Stream innerStream, WebHeaderCollection headers)
			: base(operation, innerStream)
		{
			this.Headers = headers;
			this.Decoder = new MonoChunkParser(headers);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000BF2F0 File Offset: 0x000BD4F0
		protected override async Task<int> ProcessReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			int num;
			if (this.Decoder.DataAvailable)
			{
				num = this.Decoder.Read(buffer, offset, size);
			}
			else
			{
				int num2 = 0;
				byte[] moreBytes = null;
				while (num2 == 0 && this.Decoder.WantMore)
				{
					int num3 = this.Decoder.ChunkLeft;
					if (num3 <= 0)
					{
						num3 = 1024;
					}
					else if (num3 > 16384)
					{
						num3 = 16384;
					}
					if (moreBytes == null || moreBytes.Length < num3)
					{
						moreBytes = new byte[num3];
					}
					num2 = await base.InnerStream.ReadAsync(moreBytes, 0, num3, cancellationToken).ConfigureAwait(false);
					if (num2 <= 0)
					{
						return num2;
					}
					this.Decoder.Write(moreBytes, 0, num2);
					num2 = this.Decoder.Read(buffer, offset, size);
				}
				num = num2;
			}
			return num;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000BF354 File Offset: 0x000BD554
		internal override async Task FinishReading(CancellationToken cancellationToken)
		{
			await base.FinishReading(cancellationToken).ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			if (this.Decoder.DataAvailable)
			{
				MonoChunkStream.ThrowExpectingChunkTrailer();
			}
			while (this.Decoder.WantMore)
			{
				byte[] buffer = new byte[256];
				int num = await base.InnerStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
				if (num <= 0)
				{
					MonoChunkStream.ThrowExpectingChunkTrailer();
				}
				this.Decoder.Write(buffer, 0, num);
				if (this.Decoder.Read(buffer, 0, 1) != 0)
				{
					MonoChunkStream.ThrowExpectingChunkTrailer();
				}
				buffer = null;
			}
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000BF39F File Offset: 0x000BD59F
		private static void ThrowExpectingChunkTrailer()
		{
			throw new WebException("Expecting chunk trailer.", null, WebExceptionStatus.ServerProtocolViolation, null);
		}
	}
}
