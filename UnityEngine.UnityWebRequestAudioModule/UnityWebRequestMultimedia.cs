using System;

namespace UnityEngine.Networking
{
	// Token: 0x02000004 RID: 4
	public static class UnityWebRequestMultimedia
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000216C File Offset: 0x0000036C
		public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerAudioClip(uri, audioType), null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002194 File Offset: 0x00000394
		public static UnityWebRequest GetAudioClip(Uri uri, AudioType audioType)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerAudioClip(uri, audioType), null);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021BC File Offset: 0x000003BC
		[Obsolete("MovieTexture is deprecated. Use VideoPlayer instead.", true)]
		public static UnityWebRequest GetMovieTexture(string uri)
		{
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021D0 File Offset: 0x000003D0
		[Obsolete("MovieTexture is deprecated. Use VideoPlayer instead.", true)]
		public static UnityWebRequest GetMovieTexture(Uri uri)
		{
			return null;
		}
	}
}
