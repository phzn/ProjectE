using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x02000670 RID: 1648
	internal class ContentDecodeStream : WebReadStream
	{
		// Token: 0x060033ED RID: 13293 RVA: 0x000B5160 File Offset: 0x000B3360
		public static ContentDecodeStream Create(WebOperation operation, Stream innerStream, ContentDecodeStream.Mode mode)
		{
			Stream stream;
			if (mode == ContentDecodeStream.Mode.GZip)
			{
				stream = new GZipStream(innerStream, CompressionMode.Decompress);
			}
			else
			{
				stream = new DeflateStream(innerStream, CompressionMode.Decompress);
			}
			return new ContentDecodeStream(operation, stream, innerStream);
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x000B518A File Offset: 0x000B338A
		private Stream OriginalInnerStream { get; }

		// Token: 0x060033EF RID: 13295 RVA: 0x000B5192 File Offset: 0x000B3392
		private ContentDecodeStream(WebOperation operation, Stream decodeStream, Stream originalInnerStream)
			: base(operation, decodeStream)
		{
			this.OriginalInnerStream = originalInnerStream;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000B51A3 File Offset: 0x000B33A3
		protected override Task<int> ProcessReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			return base.InnerStream.ReadAsync(buffer, offset, size, cancellationToken);
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000B51B8 File Offset: 0x000B33B8
		internal override Task FinishReading(CancellationToken cancellationToken)
		{
			WebReadStream webReadStream = this.OriginalInnerStream as WebReadStream;
			if (webReadStream != null)
			{
				return webReadStream.FinishReading(cancellationToken);
			}
			return Task.CompletedTask;
		}

		// Token: 0x02000671 RID: 1649
		internal enum Mode
		{
			// Token: 0x04001E71 RID: 7793
			GZip,
			// Token: 0x04001E72 RID: 7794
			Deflate
		}
	}
}
