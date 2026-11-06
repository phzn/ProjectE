using System;

namespace System.Net
{
	// Token: 0x02000692 RID: 1682
	internal class HttpRequestCreator : IWebRequestCreate
	{
		// Token: 0x06003567 RID: 13671 RVA: 0x0000219B File Offset: 0x0000039B
		internal HttpRequestCreator()
		{
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000BAC06 File Offset: 0x000B8E06
		public WebRequest Create(Uri uri)
		{
			return new HttpWebRequest(uri);
		}
	}
}
