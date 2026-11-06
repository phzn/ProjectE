using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x02000025 RID: 37
	internal class DynamicAtlas : AtlasBase
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00003D8A File Offset: 0x00001F8A
		internal bool isInitialized
		{
			get
			{
				return this.m_PointPage != null || this.m_BilinearPage != null;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00003DA0 File Offset: 0x00001FA0
		protected override void OnAssignedToPanel(IPanel panel)
		{
			base.OnAssignedToPanel(panel);
			this.m_Panels.Add(panel);
			bool flag = this.m_Panels.Count == 1;
			if (flag)
			{
				this.m_ColorSpace = QualitySettings.activeColorSpace;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00003DE0 File Offset: 0x00001FE0
		protected override void OnRemovedFromPanel(IPanel panel)
		{
			this.m_Panels.Remove(panel);
			bool flag = this.m_Panels.Count == 0 && this.isInitialized;
			if (flag)
			{
				this.DestroyPages();
			}
			base.OnRemovedFromPanel(panel);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00003E24 File Offset: 0x00002024
		public override void Reset()
		{
			bool isInitialized = this.isInitialized;
			if (isInitialized)
			{
				this.DestroyPages();
				int i = 0;
				int count = this.m_Panels.Count;
				while (i < count)
				{
					AtlasBase.RepaintTexturedElements(this.m_Panels[i]);
					i++;
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003E74 File Offset: 0x00002074
		private void InitPages()
		{
			int num = Mathf.Max(this.m_MaxSubTextureSize, 1);
			num = Mathf.NextPowerOfTwo(num);
			int num2 = Mathf.Max(this.m_MaxAtlasSize, 1);
			num2 = Mathf.NextPowerOfTwo(num2);
			num2 = Mathf.Min(num2, SystemInfo.maxRenderTextureSize);
			int num3 = Mathf.Max(this.m_MinAtlasSize, 1);
			num3 = Mathf.NextPowerOfTwo(num3);
			num3 = Mathf.Min(num3, num2);
			Vector2Int vector2Int = new Vector2Int(num3, num3);
			Vector2Int vector2Int2 = new Vector2Int(num2, num2);
			this.m_PointPage = new DynamicAtlasPage(RenderTextureFormat.ARGB32, FilterMode.Point, vector2Int, vector2Int2);
			this.m_BilinearPage = new DynamicAtlasPage(RenderTextureFormat.ARGB32, FilterMode.Bilinear, vector2Int, vector2Int2);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003F04 File Offset: 0x00002104
		private void DestroyPages()
		{
			this.m_PointPage.Dispose();
			this.m_PointPage = null;
			this.m_BilinearPage.Dispose();
			this.m_BilinearPage = null;
			this.m_Database.Clear();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003F3C File Offset: 0x0000213C
		public override bool TryGetAtlas(VisualElement ve, Texture2D src, out TextureId atlas, out RectInt atlasRect)
		{
			bool flag = this.m_Panels.Count == 0 || src == null;
			bool flag2;
			if (flag)
			{
				atlas = TextureId.invalid;
				atlasRect = default(RectInt);
				flag2 = false;
			}
			else
			{
				bool flag3 = !this.isInitialized;
				if (flag3)
				{
					this.InitPages();
				}
				DynamicAtlas.TextureInfo textureInfo;
				bool flag4 = this.m_Database.TryGetValue(src, out textureInfo);
				if (flag4)
				{
					atlas = textureInfo.page.textureId;
					atlasRect = textureInfo.rect;
					textureInfo.counter++;
					flag2 = true;
				}
				else
				{
					Allocator2D.Alloc2D alloc2D;
					bool flag5 = this.IsTextureValid(src, FilterMode.Bilinear) && this.m_BilinearPage.TryAdd(src, out alloc2D, out atlasRect);
					if (flag5)
					{
						textureInfo = DynamicAtlas.TextureInfo.pool.Get();
						textureInfo.alloc = alloc2D;
						textureInfo.counter = 1;
						textureInfo.page = this.m_BilinearPage;
						textureInfo.rect = atlasRect;
						this.m_Database[src] = textureInfo;
						atlas = this.m_BilinearPage.textureId;
						flag2 = true;
					}
					else
					{
						bool flag6 = this.IsTextureValid(src, FilterMode.Point) && this.m_PointPage.TryAdd(src, out alloc2D, out atlasRect);
						if (flag6)
						{
							textureInfo = DynamicAtlas.TextureInfo.pool.Get();
							textureInfo.alloc = alloc2D;
							textureInfo.counter = 1;
							textureInfo.page = this.m_PointPage;
							textureInfo.rect = atlasRect;
							this.m_Database[src] = textureInfo;
							atlas = this.m_PointPage.textureId;
							flag2 = true;
						}
						else
						{
							atlas = TextureId.invalid;
							atlasRect = default(RectInt);
							flag2 = false;
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000040EC File Offset: 0x000022EC
		public override void ReturnAtlas(VisualElement ve, Texture2D src, TextureId atlas)
		{
			DynamicAtlas.TextureInfo textureInfo;
			bool flag = this.m_Database.TryGetValue(src, out textureInfo);
			if (flag)
			{
				textureInfo.counter--;
				bool flag2 = textureInfo.counter == 0;
				if (flag2)
				{
					textureInfo.page.Remove(textureInfo.alloc);
					this.m_Database.Remove(src);
					DynamicAtlas.TextureInfo.pool.Return(textureInfo);
				}
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004158 File Offset: 0x00002358
		protected override void OnUpdateDynamicTextures(IPanel panel)
		{
			bool flag = this.m_PointPage != null;
			if (flag)
			{
				this.m_PointPage.Commit();
				base.SetDynamicTexture(this.m_PointPage.textureId, this.m_PointPage.atlas);
			}
			bool flag2 = this.m_BilinearPage != null;
			if (flag2)
			{
				this.m_BilinearPage.Commit();
				base.SetDynamicTexture(this.m_BilinearPage.textureId, this.m_BilinearPage.atlas);
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000041D8 File Offset: 0x000023D8
		internal static bool IsTextureFormatSupported(TextureFormat format)
		{
			switch (format)
			{
			case TextureFormat.Alpha8:
			case TextureFormat.ARGB4444:
			case TextureFormat.RGB24:
			case TextureFormat.RGBA32:
			case TextureFormat.ARGB32:
			case TextureFormat.RGB565:
			case TextureFormat.R16:
			case TextureFormat.DXT1:
			case TextureFormat.DXT5:
			case TextureFormat.RGBA4444:
			case TextureFormat.BGRA32:
			case TextureFormat.BC7:
			case TextureFormat.BC4:
			case TextureFormat.BC5:
			case TextureFormat.DXT1Crunched:
			case TextureFormat.DXT5Crunched:
			case TextureFormat.PVRTC_RGB2:
			case TextureFormat.PVRTC_RGBA2:
			case TextureFormat.PVRTC_RGB4:
			case TextureFormat.PVRTC_RGBA4:
			case TextureFormat.ETC_RGB4:
			case TextureFormat.EAC_R:
			case TextureFormat.EAC_R_SIGNED:
			case TextureFormat.EAC_RG:
			case TextureFormat.EAC_RG_SIGNED:
			case TextureFormat.ETC2_RGB:
			case TextureFormat.ETC2_RGBA1:
			case TextureFormat.ETC2_RGBA8:
			case TextureFormat.ASTC_4x4:
			case TextureFormat.ASTC_5x5:
			case TextureFormat.ASTC_6x6:
			case TextureFormat.ASTC_8x8:
			case TextureFormat.ASTC_10x10:
			case TextureFormat.ASTC_12x12:
			case TextureFormat.ASTC_RGBA_4x4:
			case TextureFormat.ASTC_RGBA_5x5:
			case TextureFormat.ASTC_RGBA_6x6:
			case TextureFormat.ASTC_RGBA_8x8:
			case TextureFormat.ASTC_RGBA_10x10:
			case TextureFormat.ASTC_RGBA_12x12:
			case TextureFormat.ETC_RGB4_3DS:
			case TextureFormat.ETC_RGBA8_3DS:
			case TextureFormat.RG16:
			case TextureFormat.R8:
			case TextureFormat.ETC_RGB4Crunched:
			case TextureFormat.ETC2_RGBA8Crunched:
				return true;
			case TextureFormat.RHalf:
			case TextureFormat.RGHalf:
			case TextureFormat.RGBAHalf:
			case TextureFormat.RFloat:
			case TextureFormat.RGFloat:
			case TextureFormat.RGBAFloat:
			case TextureFormat.YUY2:
			case TextureFormat.RGB9e5Float:
			case TextureFormat.BC6H:
			case TextureFormat.ASTC_HDR_4x4:
			case TextureFormat.ASTC_HDR_5x5:
			case TextureFormat.ASTC_HDR_6x6:
			case TextureFormat.ASTC_HDR_8x8:
			case TextureFormat.ASTC_HDR_10x10:
			case TextureFormat.ASTC_HDR_12x12:
			case TextureFormat.RG32:
			case TextureFormat.RGB48:
			case TextureFormat.RGBA64:
				return false;
			}
			return false;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000432C File Offset: 0x0000252C
		public virtual bool IsTextureValid(Texture2D texture, FilterMode atlasFilterMode)
		{
			DynamicAtlasFilters activeFilters = this.m_ActiveFilters;
			bool flag = this.m_CustomFilter != null && !this.m_CustomFilter(texture, ref activeFilters);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = (activeFilters & DynamicAtlasFilters.Readability) > DynamicAtlasFilters.None;
				bool flag4 = (activeFilters & DynamicAtlasFilters.Size) > DynamicAtlasFilters.None;
				bool flag5 = (activeFilters & DynamicAtlasFilters.Format) > DynamicAtlasFilters.None;
				bool flag6 = (activeFilters & DynamicAtlasFilters.ColorSpace) > DynamicAtlasFilters.None;
				bool flag7 = (activeFilters & DynamicAtlasFilters.FilterMode) > DynamicAtlasFilters.None;
				bool flag8 = flag3 && texture.isReadable;
				if (flag8)
				{
					flag2 = false;
				}
				else
				{
					bool flag9 = flag4 && (texture.width > this.maxSubTextureSize || texture.height > this.maxSubTextureSize);
					if (flag9)
					{
						flag2 = false;
					}
					else
					{
						bool flag10 = flag5 && !DynamicAtlas.IsTextureFormatSupported(texture.format);
						if (flag10)
						{
							flag2 = false;
						}
						else
						{
							bool flag11 = flag6 && this.m_ColorSpace == ColorSpace.Linear && texture.activeTextureColorSpace > ColorSpace.Gamma;
							if (flag11)
							{
								flag2 = false;
							}
							else
							{
								bool flag12 = flag7 && texture.filterMode != atlasFilterMode;
								flag2 = !flag12;
							}
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00004444 File Offset: 0x00002644
		public void SetDirty(Texture2D tex)
		{
			bool flag = tex == null;
			if (!flag)
			{
				DynamicAtlas.TextureInfo textureInfo;
				bool flag2 = this.m_Database.TryGetValue(tex, out textureInfo);
				if (flag2)
				{
					textureInfo.page.Update(tex, textureInfo.rect);
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004484 File Offset: 0x00002684
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000449C File Offset: 0x0000269C
		public int minAtlasSize
		{
			get
			{
				return this.m_MinAtlasSize;
			}
			set
			{
				bool flag = this.m_MinAtlasSize == value;
				if (!flag)
				{
					this.m_MinAtlasSize = value;
					this.Reset();
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000044C8 File Offset: 0x000026C8
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000044E0 File Offset: 0x000026E0
		public int maxAtlasSize
		{
			get
			{
				return this.m_MaxAtlasSize;
			}
			set
			{
				bool flag = this.m_MaxAtlasSize == value;
				if (!flag)
				{
					this.m_MaxAtlasSize = value;
					this.Reset();
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000450B File Offset: 0x0000270B
		public static DynamicAtlasFilters defaultFilters
		{
			get
			{
				return DynamicAtlasFilters.Readability | DynamicAtlasFilters.Size | DynamicAtlasFilters.Format | DynamicAtlasFilters.ColorSpace | DynamicAtlasFilters.FilterMode;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00004510 File Offset: 0x00002710
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00004528 File Offset: 0x00002728
		public DynamicAtlasFilters activeFilters
		{
			get
			{
				return this.m_ActiveFilters;
			}
			set
			{
				bool flag = this.m_ActiveFilters == value;
				if (!flag)
				{
					this.m_ActiveFilters = value;
					this.Reset();
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00004554 File Offset: 0x00002754
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000456C File Offset: 0x0000276C
		public int maxSubTextureSize
		{
			get
			{
				return this.m_MaxSubTextureSize;
			}
			set
			{
				bool flag = this.m_MaxSubTextureSize == value;
				if (!flag)
				{
					this.m_MaxSubTextureSize = value;
					this.Reset();
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00004598 File Offset: 0x00002798
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000045B0 File Offset: 0x000027B0
		public DynamicAtlasCustomFilter customFilter
		{
			get
			{
				return this.m_CustomFilter;
			}
			set
			{
				bool flag = this.m_CustomFilter == value;
				if (!flag)
				{
					this.m_CustomFilter = value;
					this.Reset();
				}
			}
		}

		// Token: 0x04000065 RID: 101
		private Dictionary<Texture, DynamicAtlas.TextureInfo> m_Database = new Dictionary<Texture, DynamicAtlas.TextureInfo>();

		// Token: 0x04000066 RID: 102
		private DynamicAtlasPage m_PointPage;

		// Token: 0x04000067 RID: 103
		private DynamicAtlasPage m_BilinearPage;

		// Token: 0x04000068 RID: 104
		private ColorSpace m_ColorSpace;

		// Token: 0x04000069 RID: 105
		private List<IPanel> m_Panels = new List<IPanel>(1);

		// Token: 0x0400006A RID: 106
		private int m_MinAtlasSize = 64;

		// Token: 0x0400006B RID: 107
		private int m_MaxAtlasSize = 4096;

		// Token: 0x0400006C RID: 108
		private int m_MaxSubTextureSize = 64;

		// Token: 0x0400006D RID: 109
		private DynamicAtlasFilters m_ActiveFilters = DynamicAtlas.defaultFilters;

		// Token: 0x0400006E RID: 110
		private DynamicAtlasCustomFilter m_CustomFilter;

		// Token: 0x02000026 RID: 38
		private class TextureInfo : LinkedPoolItem<DynamicAtlas.TextureInfo>
		{
			// Token: 0x06000186 RID: 390 RVA: 0x00004631 File Offset: 0x00002831
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static DynamicAtlas.TextureInfo Create()
			{
				return new DynamicAtlas.TextureInfo();
			}

			// Token: 0x06000187 RID: 391 RVA: 0x00004638 File Offset: 0x00002838
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void Reset(DynamicAtlas.TextureInfo info)
			{
				info.page = null;
				info.counter = 0;
				info.alloc = default(Allocator2D.Alloc2D);
				info.rect = default(RectInt);
			}

			// Token: 0x0400006F RID: 111
			public DynamicAtlasPage page;

			// Token: 0x04000070 RID: 112
			public int counter;

			// Token: 0x04000071 RID: 113
			public Allocator2D.Alloc2D alloc;

			// Token: 0x04000072 RID: 114
			public RectInt rect;

			// Token: 0x04000073 RID: 115
			public static readonly LinkedPool<DynamicAtlas.TextureInfo> pool = new LinkedPool<DynamicAtlas.TextureInfo>(new Func<DynamicAtlas.TextureInfo>(DynamicAtlas.TextureInfo.Create), new Action<DynamicAtlas.TextureInfo>(DynamicAtlas.TextureInfo.Reset), 1024);
		}
	}
}
