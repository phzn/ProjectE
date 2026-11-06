using System;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014B8 RID: 5304
	public sealed class StreamerArtworkCatalog : ScriptableObject
	{
		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x0600A332 RID: 41778 RVA: 0x0043CA8C File Offset: 0x0043AC8C
		public static StreamerArtworkCatalog Instance
		{
			get
			{
				if (!(StreamerArtworkCatalog._instance != null))
				{
					return StreamerArtworkCatalog._instance = Resources.Load<StreamerArtworkCatalog>("StreamerArtworkCatalog");
				}
				return StreamerArtworkCatalog._instance;
			}
		}

		// Token: 0x0600A333 RID: 41779 RVA: 0x0043CAB4 File Offset: 0x0043ACB4
		public bool TryGetSafe(Sprite original, out Sprite safe)
		{
			foreach (StreamerArtworkCatalog.Entry entry in this.entries)
			{
				if (entry.original == original && original != null)
				{
					safe = entry.safe;
					return safe != null;
				}
			}
			safe = null;
			return false;
		}

		// Token: 0x0600A334 RID: 41780 RVA: 0x0043CB0C File Offset: 0x0043AD0C
		public bool TryGetSafe(Texture original, out Texture safe)
		{
			foreach (StreamerArtworkCatalog.TextureEntry textureEntry in this.textures)
			{
				if (textureEntry.original == original && original != null)
				{
					safe = textureEntry.safe;
					return safe != null;
				}
			}
			safe = null;
			return false;
		}

		// Token: 0x0400A6F7 RID: 42743
		public StreamerArtworkCatalog.Entry[] entries = Array.Empty<StreamerArtworkCatalog.Entry>();

		// Token: 0x0400A6F8 RID: 42744
		public StreamerArtworkCatalog.TextureEntry[] textures = Array.Empty<StreamerArtworkCatalog.TextureEntry>();

		// Token: 0x0400A6F9 RID: 42745
		private static StreamerArtworkCatalog _instance;

		// Token: 0x020014B9 RID: 5305
		[Serializable]
		public struct Entry
		{
			// Token: 0x0400A6FA RID: 42746
			public Sprite original;

			// Token: 0x0400A6FB RID: 42747
			public Sprite safe;
		}

		// Token: 0x020014BA RID: 5306
		[Serializable]
		public struct TextureEntry
		{
			// Token: 0x0400A6FC RID: 42748
			public Texture original;

			// Token: 0x0400A6FD RID: 42749
			public Texture safe;
		}
	}
}
