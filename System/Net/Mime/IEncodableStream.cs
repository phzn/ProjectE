using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020007D4 RID: 2004
	internal interface IEncodableStream
	{
		// Token: 0x0600404B RID: 16459
		int DecodeBytes(byte[] buffer, int offset, int count);

		// Token: 0x0600404C RID: 16460
		int EncodeBytes(byte[] buffer, int offset, int count);

		// Token: 0x0600404D RID: 16461
		string GetEncodedString();

		// Token: 0x0600404E RID: 16462
		Stream GetStream();
	}
}
