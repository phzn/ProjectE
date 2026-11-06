using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200048E RID: 1166
	internal struct ImageSource
	{
		// Token: 0x06002469 RID: 9321 RVA: 0x00097620 File Offset: 0x00095820
		public bool IsNull()
		{
			return this.texture == null && this.sprite == null && this.vectorImage == null && this.renderTexture == null;
		}

		// Token: 0x04001181 RID: 4481
		public Texture2D texture;

		// Token: 0x04001182 RID: 4482
		public Sprite sprite;

		// Token: 0x04001183 RID: 4483
		public VectorImage vectorImage;

		// Token: 0x04001184 RID: 4484
		public RenderTexture renderTexture;
	}
}
