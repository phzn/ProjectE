using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007D2 RID: 2002
	internal class EncodedStreamFactory
	{
		// Token: 0x0600403D RID: 16445 RVA: 0x000DB8CE File Offset: 0x000D9ACE
		internal IEncodableStream GetEncoder(TransferEncoding encoding, Stream stream)
		{
			if (encoding == TransferEncoding.Base64)
			{
				return new Base64Stream(stream, new Base64WriteStateInfo());
			}
			if (encoding == TransferEncoding.QuotedPrintable)
			{
				return new QuotedPrintableStream(stream, true);
			}
			if (encoding == TransferEncoding.SevenBit || encoding == TransferEncoding.EightBit)
			{
				return new EightBitStream(stream);
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x000DB900 File Offset: 0x000D9B00
		internal IEncodableStream GetEncoderForHeader(Encoding encoding, bool useBase64Encoding, int headerTextLength)
		{
			byte[] array = this.CreateHeader(encoding, useBase64Encoding);
			byte[] array2 = this.CreateFooter();
			if (useBase64Encoding)
			{
				return new Base64Stream((Base64WriteStateInfo)new Base64WriteStateInfo(1024, array, array2, 70, headerTextLength));
			}
			return new QEncodedStream(new WriteStateInfoBase(1024, array, array2, 70, headerTextLength));
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x000DB94E File Offset: 0x000D9B4E
		protected byte[] CreateHeader(Encoding encoding, bool useBase64Encoding)
		{
			return Encoding.ASCII.GetBytes("=?" + encoding.HeaderName + "?" + (useBase64Encoding ? "B?" : "Q?"));
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x000DB97E File Offset: 0x000D9B7E
		protected byte[] CreateFooter()
		{
			return new byte[] { 63, 61 };
		}

		// Token: 0x04002674 RID: 9844
		internal const int DefaultMaxLineLength = 70;

		// Token: 0x04002675 RID: 9845
		private const int InitialBufferSize = 1024;
	}
}
