using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000016 RID: 22
	internal class MaterialReferenceManager
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00006EC8 File Offset: 0x000050C8
		public static MaterialReferenceManager instance
		{
			get
			{
				bool flag = MaterialReferenceManager.s_Instance == null;
				if (flag)
				{
					MaterialReferenceManager.s_Instance = new MaterialReferenceManager();
				}
				return MaterialReferenceManager.s_Instance;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006EF5 File Offset: 0x000050F5
		public static void AddFontAsset(FontAsset fontAsset)
		{
			MaterialReferenceManager.instance.AddFontAssetInternal(fontAsset);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006F04 File Offset: 0x00005104
		private void AddFontAssetInternal(FontAsset fontAsset)
		{
			bool flag = this.m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode);
			if (!flag)
			{
				this.m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset);
				this.m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006F54 File Offset: 0x00005154
		public static void AddSpriteAsset(SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(spriteAsset);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006F64 File Offset: 0x00005164
		private void AddSpriteAssetInternal(SpriteAsset spriteAsset)
		{
			bool flag = this.m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode);
			if (!flag)
			{
				this.m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset);
				this.m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006FB4 File Offset: 0x000051B4
		public static void AddSpriteAsset(int hashCode, SpriteAsset spriteAsset)
		{
			MaterialReferenceManager.instance.AddSpriteAssetInternal(hashCode, spriteAsset);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006FC4 File Offset: 0x000051C4
		private void AddSpriteAssetInternal(int hashCode, SpriteAsset spriteAsset)
		{
			bool flag = this.m_SpriteAssetReferenceLookup.ContainsKey(hashCode);
			if (!flag)
			{
				this.m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset);
				this.m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00007005 File Offset: 0x00005205
		public static void AddFontMaterial(int hashCode, Material material)
		{
			MaterialReferenceManager.instance.AddFontMaterialInternal(hashCode, material);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007015 File Offset: 0x00005215
		private void AddFontMaterialInternal(int hashCode, Material material)
		{
			this.m_FontMaterialReferenceLookup.Add(hashCode, material);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00007026 File Offset: 0x00005226
		public static void AddColorGradientPreset(int hashCode, TextColorGradient spriteAsset)
		{
			MaterialReferenceManager.instance.AddColorGradientPreset_Internal(hashCode, spriteAsset);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007038 File Offset: 0x00005238
		private void AddColorGradientPreset_Internal(int hashCode, TextColorGradient spriteAsset)
		{
			bool flag = this.m_ColorGradientReferenceLookup.ContainsKey(hashCode);
			if (!flag)
			{
				this.m_ColorGradientReferenceLookup.Add(hashCode, spriteAsset);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00007068 File Offset: 0x00005268
		public bool Contains(FontAsset font)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(font.hashCode);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000708C File Offset: 0x0000528C
		public bool Contains(SpriteAsset sprite)
		{
			return this.m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000070B0 File Offset: 0x000052B0
		public static bool TryGetFontAsset(int hashCode, out FontAsset fontAsset)
		{
			return MaterialReferenceManager.instance.TryGetFontAssetInternal(hashCode, out fontAsset);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000070D0 File Offset: 0x000052D0
		private bool TryGetFontAssetInternal(int hashCode, out FontAsset fontAsset)
		{
			fontAsset = null;
			return this.m_FontAssetReferenceLookup.TryGetValue(hashCode, out fontAsset);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000070F4 File Offset: 0x000052F4
		public static bool TryGetSpriteAsset(int hashCode, out SpriteAsset spriteAsset)
		{
			return MaterialReferenceManager.instance.TryGetSpriteAssetInternal(hashCode, out spriteAsset);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007114 File Offset: 0x00005314
		private bool TryGetSpriteAssetInternal(int hashCode, out SpriteAsset spriteAsset)
		{
			spriteAsset = null;
			return this.m_SpriteAssetReferenceLookup.TryGetValue(hashCode, out spriteAsset);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007138 File Offset: 0x00005338
		public static bool TryGetColorGradientPreset(int hashCode, out TextColorGradient gradientPreset)
		{
			return MaterialReferenceManager.instance.TryGetColorGradientPresetInternal(hashCode, out gradientPreset);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007158 File Offset: 0x00005358
		private bool TryGetColorGradientPresetInternal(int hashCode, out TextColorGradient gradientPreset)
		{
			gradientPreset = null;
			return this.m_ColorGradientReferenceLookup.TryGetValue(hashCode, out gradientPreset);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000717C File Offset: 0x0000537C
		public static bool TryGetMaterial(int hashCode, out Material material)
		{
			return MaterialReferenceManager.instance.TryGetMaterialInternal(hashCode, out material);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000719C File Offset: 0x0000539C
		private bool TryGetMaterialInternal(int hashCode, out Material material)
		{
			material = null;
			return this.m_FontMaterialReferenceLookup.TryGetValue(hashCode, out material);
		}

		// Token: 0x040000A3 RID: 163
		private static MaterialReferenceManager s_Instance;

		// Token: 0x040000A4 RID: 164
		private Dictionary<int, Material> m_FontMaterialReferenceLookup = new Dictionary<int, Material>();

		// Token: 0x040000A5 RID: 165
		private Dictionary<int, FontAsset> m_FontAssetReferenceLookup = new Dictionary<int, FontAsset>();

		// Token: 0x040000A6 RID: 166
		private Dictionary<int, SpriteAsset> m_SpriteAssetReferenceLookup = new Dictionary<int, SpriteAsset>();

		// Token: 0x040000A7 RID: 167
		private Dictionary<int, TextColorGradient> m_ColorGradientReferenceLookup = new Dictionary<int, TextColorGradient>();
	}
}
