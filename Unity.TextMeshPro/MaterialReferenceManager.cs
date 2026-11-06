using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000008 RID: 8
	public class MaterialReferenceManager
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002389 File Offset: 0x00000589
		public static MaterialReferenceManager instance
		{
			get
			{
				if (MaterialReferenceManager.s_Instance == null)
				{
					MaterialReferenceManager.s_Instance = new MaterialReferenceManager();
				}
				return MaterialReferenceManager.s_Instance;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023A1 File Offset: 0x000005A1
		public static void AddFontAsset(TMP_FontAsset fontAsset)
		{
			MaterialReferenceManager.instance.AddFontAssetInternal(fontAsset);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023AE File Offset: 0x000005AE
		private void AddFontAssetInternal(TMP_FontAsset fontAsset)
		{
			if (this.m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode))
			{
				return;
			}
			this.m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset);
			this.m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023ED File Offset: 0x000005ED
		public static void AddSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(spriteAsset);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023FA File Offset: 0x000005FA
		private void AddSpriteAssetInternal(TMP_SpriteAsset spriteAsset)
		{
			if (this.m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode))
			{
				return;
			}
			this.m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset);
			this.m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002439 File Offset: 0x00000639
		public static void AddSpriteAsset(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(hashCode, spriteAsset);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002447 File Offset: 0x00000647
		private void AddSpriteAssetInternal(int hashCode, TMP_SpriteAsset spriteAsset)
		{
			if (this.m_SpriteAssetReferenceLookup.ContainsKey(hashCode))
			{
				return;
			}
			this.m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset);
			this.m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material);
			if (spriteAsset.hashCode == 0)
			{
				spriteAsset.hashCode = hashCode;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002486 File Offset: 0x00000686
		public static void AddFontMaterial(int hashCode, Material material)
		{
			MaterialReferenceManager.instance.AddFontMaterialInternal(hashCode, material);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002494 File Offset: 0x00000694
		private void AddFontMaterialInternal(int hashCode, Material material)
		{
			this.m_FontMaterialReferenceLookup.Add(hashCode, material);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024A3 File Offset: 0x000006A3
		public static void AddColorGradientPreset(int hashCode, TMP_ColorGradient spriteAsset)
		{
			MaterialReferenceManager.instance.AddColorGradientPreset_Internal(hashCode, spriteAsset);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024B1 File Offset: 0x000006B1
		private void AddColorGradientPreset_Internal(int hashCode, TMP_ColorGradient spriteAsset)
		{
			if (this.m_ColorGradientReferenceLookup.ContainsKey(hashCode))
			{
				return;
			}
			this.m_ColorGradientReferenceLookup.Add(hashCode, spriteAsset);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024CF File Offset: 0x000006CF
		public bool Contains(TMP_FontAsset font)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(font.hashCode);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000024E2 File Offset: 0x000006E2
		public bool Contains(TMP_SpriteAsset sprite)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000024F5 File Offset: 0x000006F5
		public static bool TryGetFontAsset(int hashCode, out TMP_FontAsset fontAsset)
		{
			return MaterialReferenceManager.instance.TryGetFontAssetInternal(hashCode, out fontAsset);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002503 File Offset: 0x00000703
		private bool TryGetFontAssetInternal(int hashCode, out TMP_FontAsset fontAsset)
		{
			fontAsset = null;
			return this.m_FontAssetReferenceLookup.TryGetValue(hashCode, out fontAsset);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002515 File Offset: 0x00000715
		public static bool TryGetSpriteAsset(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			return MaterialReferenceManager.instance.TryGetSpriteAssetInternal(hashCode, out spriteAsset);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002523 File Offset: 0x00000723
		private bool TryGetSpriteAssetInternal(int hashCode, out TMP_SpriteAsset spriteAsset)
		{
			spriteAsset = null;
			return this.m_SpriteAssetReferenceLookup.TryGetValue(hashCode, out spriteAsset);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002535 File Offset: 0x00000735
		public static bool TryGetColorGradientPreset(int hashCode, out TMP_ColorGradient gradientPreset)
		{
			return MaterialReferenceManager.instance.TryGetColorGradientPresetInternal(hashCode, out gradientPreset);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002543 File Offset: 0x00000743
		private bool TryGetColorGradientPresetInternal(int hashCode, out TMP_ColorGradient gradientPreset)
		{
			gradientPreset = null;
			return this.m_ColorGradientReferenceLookup.TryGetValue(hashCode, out gradientPreset);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002555 File Offset: 0x00000755
		public static bool TryGetMaterial(int hashCode, out Material material)
		{
			return MaterialReferenceManager.instance.TryGetMaterialInternal(hashCode, out material);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002563 File Offset: 0x00000763
		private bool TryGetMaterialInternal(int hashCode, out Material material)
		{
			material = null;
			return this.m_FontMaterialReferenceLookup.TryGetValue(hashCode, out material);
		}

		// Token: 0x04000009 RID: 9
		private static MaterialReferenceManager s_Instance;

		// Token: 0x0400000A RID: 10
		private Dictionary<int, Material> m_FontMaterialReferenceLookup = new Dictionary<int, Material>();

		// Token: 0x0400000B RID: 11
		private Dictionary<int, TMP_FontAsset> m_FontAssetReferenceLookup = new Dictionary<int, TMP_FontAsset>();

		// Token: 0x0400000C RID: 12
		private Dictionary<int, TMP_SpriteAsset> m_SpriteAssetReferenceLookup = new Dictionary<int, TMP_SpriteAsset>();

		// Token: 0x0400000D RID: 13
		private Dictionary<int, TMP_ColorGradient> m_ColorGradientReferenceLookup = new Dictionary<int, TMP_ColorGradient>();
	}
}
