using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	// Token: 0x02000003 RID: 3
	[Obsolete("MovieTexture is deprecated. Use VideoPlayer instead.", true)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerMovieTexture : DownloadHandler
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000020ED File Offset: 0x000002ED
		public DownloadHandlerMovieTexture()
		{
			DownloadHandlerMovieTexture.FeatureRemoved();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002100 File Offset: 0x00000300
		protected override byte[] GetData()
		{
			DownloadHandlerMovieTexture.FeatureRemoved();
			return null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002119 File Offset: 0x00000319
		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for movies");
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002128 File Offset: 0x00000328
		public MovieTexture movieTexture
		{
			get
			{
				DownloadHandlerMovieTexture.FeatureRemoved();
				return null;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002144 File Offset: 0x00000344
		public static MovieTexture GetContent(UnityWebRequest uwr)
		{
			DownloadHandlerMovieTexture.FeatureRemoved();
			return null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000215D File Offset: 0x0000035D
		private static void FeatureRemoved()
		{
			throw new Exception("Movie texture has been removed, use VideoPlayer instead");
		}
	}
}
