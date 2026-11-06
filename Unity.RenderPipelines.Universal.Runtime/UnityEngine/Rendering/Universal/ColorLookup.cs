using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000092 RID: 146
	[VolumeComponentMenuForRenderPipeline("Post-processing/Color Lookup", new Type[] { typeof(UniversalRenderPipeline) })]
	[Serializable]
	public sealed class ColorLookup : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x0001FA03 File Offset: 0x0001DC03
		public bool IsActive()
		{
			return this.contribution.value > 0f && this.ValidateLUT();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001FA1F File Offset: 0x0001DC1F
		public bool IsTileCompatible()
		{
			return true;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001FA24 File Offset: 0x0001DC24
		public bool ValidateLUT()
		{
			UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
			if (asset == null || this.texture.value == null)
			{
				return false;
			}
			int colorGradingLutSize = asset.colorGradingLutSize;
			if (this.texture.value.height != colorGradingLutSize)
			{
				return false;
			}
			bool flag = false;
			Texture value = this.texture.value;
			Texture2D texture2D = value as Texture2D;
			if (texture2D == null)
			{
				RenderTexture renderTexture = value as RenderTexture;
				if (renderTexture != null)
				{
					flag |= renderTexture.dimension == TextureDimension.Tex2D && renderTexture.width == colorGradingLutSize * colorGradingLutSize && !renderTexture.sRGB;
				}
			}
			else
			{
				flag |= texture2D.width == colorGradingLutSize * colorGradingLutSize && !GraphicsFormatUtility.IsSRGBFormat(texture2D.graphicsFormat);
			}
			return flag;
		}

		// Token: 0x040003EC RID: 1004
		[Tooltip("A 2D Lookup Texture (LUT) to use for color grading.")]
		public TextureParameter texture = new TextureParameter(null, false);

		// Token: 0x040003ED RID: 1005
		[Tooltip("How much of the lookup texture will contribute to the color grading effect.")]
		public ClampedFloatParameter contribution = new ClampedFloatParameter(0f, 0f, 1f, false);
	}
}
