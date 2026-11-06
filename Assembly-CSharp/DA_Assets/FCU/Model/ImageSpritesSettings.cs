using System;
using System.IO;
using DA_Assets.DAI;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000375 RID: 885
	[Serializable]
	public class ImageSpritesSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x00096A0A File Offset: 0x00094C0A
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00096A12 File Offset: 0x00094C12
		[SerializeProperty("proceduralCondition")]
		public ProceduralCondition ProceduralCondition
		{
			get
			{
				return this.proceduralCondition;
			}
			set
			{
				base.SetValue<ProceduralCondition>(ref this.proceduralCondition, value);
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00096A21 File Offset: 0x00094C21
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x00096A29 File Offset: 0x00094C29
		[SerializeProperty("svgCondition")]
		public SvgCondition SvgCondition
		{
			get
			{
				return this.svgCondition;
			}
			set
			{
				base.SetValue<SvgCondition>(ref this.svgCondition, value);
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00096A38 File Offset: 0x00094C38
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x00096A40 File Offset: 0x00094C40
		public ImageComponent ImageComponent
		{
			get
			{
				return this.imageComponent;
			}
			set
			{
				switch (value)
				{
				case ImageComponent.SubcShape:
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "SubcShape" }));
					value = ImageComponent.UnityImage;
					break;
				case ImageComponent.MPImage:
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "MPImage" }));
					value = ImageComponent.UnityImage;
					break;
				case ImageComponent.ProceduralImage:
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "ProceduralImage" }));
					value = ImageComponent.UnityImage;
					break;
				case ImageComponent.RoundedImage:
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "RoundedImage" }));
					value = ImageComponent.UnityImage;
					break;
				case ImageComponent.SvgImage:
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "SvgImage" }));
					value = ImageComponent.UnityImage;
					break;
				}
				base.SetValue<ImageComponent>(ref this.imageComponent, value);
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00096B20 File Offset: 0x00094D20
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x00096B28 File Offset: 0x00094D28
		[SerializeProperty("downloadOptions")]
		public SpriteDownloadOptions DownloadOptions
		{
			get
			{
				return this.downloadOptions;
			}
			set
			{
				base.SetValue<SpriteDownloadOptions>(ref this.downloadOptions, value);
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00096B37 File Offset: 0x00094D37
		// (set) Token: 0x06001712 RID: 5906 RVA: 0x00096B3F File Offset: 0x00094D3F
		public bool RedownloadSprites
		{
			get
			{
				return this.redownloadSprites;
			}
			set
			{
				base.SetValue<bool>(ref this.redownloadSprites, value);
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00096B4E File Offset: 0x00094D4E
		// (set) Token: 0x06001714 RID: 5908 RVA: 0x00096B56 File Offset: 0x00094D56
		public string SpritesPath
		{
			get
			{
				return this.spritesPath;
			}
			set
			{
				base.SetValue<string>(ref this.spritesPath, value);
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00096B65 File Offset: 0x00094D65
		// (set) Token: 0x06001716 RID: 5910 RVA: 0x00096B6D File Offset: 0x00094D6D
		public ImageFormat ImageFormat
		{
			get
			{
				return this.imageFormat;
			}
			set
			{
				if (value == ImageFormat.SVG)
				{
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "SVG" }));
					value = ImageFormat.PNG;
				}
				base.SetValue<ImageFormat>(ref this.imageFormat, value);
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x00096B9D File Offset: 0x00094D9D
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x00096BD7 File Offset: 0x00094DD7
		public float ImageScale
		{
			get
			{
				if (this.imageFormat == ImageFormat.SVG && this.imageScale != 1f)
				{
					DALogger.Log(FcuLocKey.log_svg_scale_1.Localize(Array.Empty<object>()));
					this.imageScale = 1f;
				}
				return this.imageScale;
			}
			set
			{
				base.SetValue<float>(ref this.imageScale, value);
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00096BE6 File Offset: 0x00094DE6
		// (set) Token: 0x0600171A RID: 5914 RVA: 0x00096BF0 File Offset: 0x00094DF0
		public Vector2Int MaxSpriteSize
		{
			get
			{
				return this.maxSpriteSize;
			}
			set
			{
				Vector2Int vector2Int = value;
				if (vector2Int.x > 4096)
				{
					vector2Int.x = 4096;
				}
				else if (vector2Int.x < 32)
				{
					vector2Int.x = 32;
				}
				if (vector2Int.y > 4096)
				{
					vector2Int.y = 4096;
				}
				else if (vector2Int.y < 32)
				{
					vector2Int.y = 32;
				}
				base.SetValue<Vector2Int>(ref this.maxSpriteSize, vector2Int);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00096C6C File Offset: 0x00094E6C
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x00096C74 File Offset: 0x00094E74
		public PreserveRatioMode PreserveRatioMode
		{
			get
			{
				return this.preserveRatioMode;
			}
			set
			{
				base.SetValue<PreserveRatioMode>(ref this.preserveRatioMode, value);
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00096C83 File Offset: 0x00094E83
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00096C8B File Offset: 0x00094E8B
		public float PixelsPerUnit
		{
			get
			{
				return this.pixelsPerUnit;
			}
			set
			{
				base.SetValue<float>(ref this.pixelsPerUnit, value);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00096C9A File Offset: 0x00094E9A
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x00096CA2 File Offset: 0x00094EA2
		public bool UseImageLinearMaterial
		{
			get
			{
				return this.useImageLinearMaterial;
			}
			set
			{
				base.SetValue<bool>(ref this.useImageLinearMaterial, value);
			}
		}

		// Token: 0x040018CA RID: 6346
		[SerializeField]
		private ProceduralCondition proceduralCondition = ProceduralCondition.Sprite | ProceduralCondition.RectangleNoRoundedCorners;

		// Token: 0x040018CB RID: 6347
		[Tooltip("The VectorGraphics 2.0.0 asset does not support displaying SVG images with effects.\r\n\r\nI also noticed that it doesn’t render objects that use an image as a fill in Figma.\r\n\r\nBy default, the asset will download objects that meet these conditions as PNG sprites and use the UI.Image component to draw them.  \r\n\r\nYou can disable these options, but these components may not display correctly in Unity.")]
		[SerializeField]
		private SvgCondition svgCondition = SvgCondition.ImageOrVideo | SvgCondition.AnyEffect;

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private ImageComponent imageComponent;

		// Token: 0x040018CD RID: 6349
		[SerializeField]
		private SpriteDownloadOptions downloadOptions = SpriteDownloadOptions.MultipleFills | SpriteDownloadOptions.UnsupportedGradients;

		// Token: 0x040018CE RID: 6350
		[SerializeField]
		private bool redownloadSprites;

		// Token: 0x040018CF RID: 6351
		[SerializeField]
		private string spritesPath = Path.Combine("Assets", "Sprites");

		// Token: 0x040018D0 RID: 6352
		[SerializeField]
		private ImageFormat imageFormat;

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		private float imageScale = 4f;

		// Token: 0x040018D2 RID: 6354
		public Vector2Int maxSpriteSize = new Vector2Int(4096, 4096);

		// Token: 0x040018D3 RID: 6355
		[SerializeField]
		private PreserveRatioMode preserveRatioMode;

		// Token: 0x040018D4 RID: 6356
		[SerializeField]
		private float pixelsPerUnit = 100f;

		// Token: 0x040018D5 RID: 6357
		[SerializeField]
		private bool useImageLinearMaterial = true;
	}
}
