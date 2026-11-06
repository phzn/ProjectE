using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200048C RID: 1164
	[Serializable]
	internal struct ScalableImage
	{
		// Token: 0x06002468 RID: 9320 RVA: 0x000975DC File Offset: 0x000957DC
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", new object[] { "normalImage", this.normalImage, "highResolutionImage", this.highResolutionImage });
		}

		// Token: 0x0400117D RID: 4477
		public Texture2D normalImage;

		// Token: 0x0400117E RID: 4478
		public Texture2D highResolutionImage;
	}
}
