using System;

namespace UnityEngine.Networking
{
	// Token: 0x02000003 RID: 3
	public static class UnityWebRequestTexture
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000020FC File Offset: 0x000002FC
		public static UnityWebRequest GetTexture(string uri)
		{
			return UnityWebRequestTexture.GetTexture(uri, false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002118 File Offset: 0x00000318
		public static UnityWebRequest GetTexture(Uri uri)
		{
			return UnityWebRequestTexture.GetTexture(uri, false);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002134 File Offset: 0x00000334
		public static UnityWebRequest GetTexture(string uri, bool nonReadable)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000215C File Offset: 0x0000035C
		public static UnityWebRequest GetTexture(Uri uri, bool nonReadable)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
		}
	}
}
