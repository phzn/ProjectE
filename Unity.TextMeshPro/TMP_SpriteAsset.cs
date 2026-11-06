using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore;

namespace TMPro
{
	// Token: 0x02000054 RID: 84
	[ExcludeFromPreset]
	public class TMP_SpriteAsset : TMP_Asset
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00025F13 File Offset: 0x00024113
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00025F1B File Offset: 0x0002411B
		public string version
		{
			get
			{
				return this.m_Version;
			}
			internal set
			{
				this.m_Version = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00025F24 File Offset: 0x00024124
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00025F2C File Offset: 0x0002412C
		public FaceInfo faceInfo
		{
			get
			{
				return this.m_FaceInfo;
			}
			internal set
			{
				this.m_FaceInfo = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00025F35 File Offset: 0x00024135
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00025F4B File Offset: 0x0002414B
		public List<TMP_SpriteCharacter> spriteCharacterTable
		{
			get
			{
				if (this.m_GlyphIndexLookup == null)
				{
					this.UpdateLookupTables();
				}
				return this.m_SpriteCharacterTable;
			}
			internal set
			{
				this.m_SpriteCharacterTable = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00025F54 File Offset: 0x00024154
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00025F6A File Offset: 0x0002416A
		public Dictionary<uint, TMP_SpriteCharacter> spriteCharacterLookupTable
		{
			get
			{
				if (this.m_SpriteCharacterLookup == null)
				{
					this.UpdateLookupTables();
				}
				return this.m_SpriteCharacterLookup;
			}
			internal set
			{
				this.m_SpriteCharacterLookup = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00025F73 File Offset: 0x00024173
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00025F7B File Offset: 0x0002417B
		public List<TMP_SpriteGlyph> spriteGlyphTable
		{
			get
			{
				return this.m_SpriteGlyphTable;
			}
			internal set
			{
				this.m_SpriteGlyphTable = value;
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00025F84 File Offset: 0x00024184
		private void Awake()
		{
			if (this.material != null && string.IsNullOrEmpty(this.m_Version))
			{
				this.UpgradeSpriteAsset();
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00025FA7 File Offset: 0x000241A7
		private Material GetDefaultSpriteMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			Material material = new Material(Shader.Find("TextMeshPro/Sprite"));
			material.SetTexture(ShaderUtilities.ID_MainTex, this.spriteSheet);
			material.hideFlags = HideFlags.HideInHierarchy;
			return material;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00025FD8 File Offset: 0x000241D8
		public void UpdateLookupTables()
		{
			if (this.material != null && string.IsNullOrEmpty(this.m_Version))
			{
				this.UpgradeSpriteAsset();
			}
			if (this.m_GlyphIndexLookup == null)
			{
				this.m_GlyphIndexLookup = new Dictionary<uint, int>();
			}
			else
			{
				this.m_GlyphIndexLookup.Clear();
			}
			if (this.m_SpriteGlyphLookup == null)
			{
				this.m_SpriteGlyphLookup = new Dictionary<uint, TMP_SpriteGlyph>();
			}
			else
			{
				this.m_SpriteGlyphLookup.Clear();
			}
			for (int i = 0; i < this.m_SpriteGlyphTable.Count; i++)
			{
				TMP_SpriteGlyph tmp_SpriteGlyph = this.m_SpriteGlyphTable[i];
				uint index = tmp_SpriteGlyph.index;
				if (!this.m_GlyphIndexLookup.ContainsKey(index))
				{
					this.m_GlyphIndexLookup.Add(index, i);
				}
				if (!this.m_SpriteGlyphLookup.ContainsKey(index))
				{
					this.m_SpriteGlyphLookup.Add(index, tmp_SpriteGlyph);
				}
			}
			if (this.m_NameLookup == null)
			{
				this.m_NameLookup = new Dictionary<int, int>();
			}
			else
			{
				this.m_NameLookup.Clear();
			}
			if (this.m_SpriteCharacterLookup == null)
			{
				this.m_SpriteCharacterLookup = new Dictionary<uint, TMP_SpriteCharacter>();
			}
			else
			{
				this.m_SpriteCharacterLookup.Clear();
			}
			for (int j = 0; j < this.m_SpriteCharacterTable.Count; j++)
			{
				TMP_SpriteCharacter tmp_SpriteCharacter = this.m_SpriteCharacterTable[j];
				if (tmp_SpriteCharacter != null)
				{
					uint glyphIndex = tmp_SpriteCharacter.glyphIndex;
					if (this.m_SpriteGlyphLookup.ContainsKey(glyphIndex))
					{
						tmp_SpriteCharacter.glyph = this.m_SpriteGlyphLookup[glyphIndex];
						tmp_SpriteCharacter.textAsset = this;
						int hashCode = this.m_SpriteCharacterTable[j].hashCode;
						if (!this.m_NameLookup.ContainsKey(hashCode))
						{
							this.m_NameLookup.Add(hashCode, j);
						}
						uint unicode = this.m_SpriteCharacterTable[j].unicode;
						if (unicode != 65534U && !this.m_SpriteCharacterLookup.ContainsKey(unicode))
						{
							this.m_SpriteCharacterLookup.Add(unicode, tmp_SpriteCharacter);
						}
					}
				}
			}
			this.m_IsSpriteAssetLookupTablesDirty = false;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000261C0 File Offset: 0x000243C0
		public int GetSpriteIndexFromHashcode(int hashCode)
		{
			if (this.m_NameLookup == null)
			{
				this.UpdateLookupTables();
			}
			int num;
			if (this.m_NameLookup.TryGetValue(hashCode, out num))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000261F0 File Offset: 0x000243F0
		public int GetSpriteIndexFromUnicode(uint unicode)
		{
			if (this.m_SpriteCharacterLookup == null)
			{
				this.UpdateLookupTables();
			}
			TMP_SpriteCharacter tmp_SpriteCharacter;
			if (this.m_SpriteCharacterLookup.TryGetValue(unicode, out tmp_SpriteCharacter))
			{
				return (int)tmp_SpriteCharacter.glyphIndex;
			}
			return -1;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00026224 File Offset: 0x00024424
		public int GetSpriteIndexFromName(string name)
		{
			if (this.m_NameLookup == null)
			{
				this.UpdateLookupTables();
			}
			int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(name);
			return this.GetSpriteIndexFromHashcode(simpleHashCode);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00026250 File Offset: 0x00024450
		public static TMP_SpriteAsset SearchForSpriteByUnicode(TMP_SpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			if (spriteAsset == null)
			{
				spriteIndex = -1;
				return null;
			}
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (TMP_SpriteAsset.k_searchedSpriteAssets == null)
			{
				TMP_SpriteAsset.k_searchedSpriteAssets = new HashSet<int>();
			}
			else
			{
				TMP_SpriteAsset.k_searchedSpriteAssets.Clear();
			}
			int instanceID = spriteAsset.GetInstanceID();
			TMP_SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
			if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				return TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, true, out spriteIndex);
			}
			if (includeFallbacks && TMP_Settings.defaultSpriteAsset != null)
			{
				return TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(TMP_Settings.defaultSpriteAsset, unicode, true, out spriteIndex);
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000262F8 File Offset: 0x000244F8
		private static TMP_SpriteAsset SearchForSpriteByUnicodeInternal(List<TMP_SpriteAsset> spriteAssets, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			for (int i = 0; i < spriteAssets.Count; i++)
			{
				TMP_SpriteAsset tmp_SpriteAsset = spriteAssets[i];
				if (!(tmp_SpriteAsset == null))
				{
					int instanceID = tmp_SpriteAsset.GetInstanceID();
					if (TMP_SpriteAsset.k_searchedSpriteAssets.Add(instanceID))
					{
						tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(tmp_SpriteAsset, unicode, includeFallbacks, out spriteIndex);
						if (tmp_SpriteAsset != null)
						{
							return tmp_SpriteAsset;
						}
					}
				}
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00026354 File Offset: 0x00024554
		private static TMP_SpriteAsset SearchForSpriteByUnicodeInternal(TMP_SpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				return TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, true, out spriteIndex);
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00026394 File Offset: 0x00024594
		public static TMP_SpriteAsset SearchForSpriteByHashCode(TMP_SpriteAsset spriteAsset, int hashCode, bool includeFallbacks, out int spriteIndex)
		{
			if (spriteAsset == null)
			{
				spriteIndex = -1;
				return null;
			}
			spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (TMP_SpriteAsset.k_searchedSpriteAssets == null)
			{
				TMP_SpriteAsset.k_searchedSpriteAssets = new HashSet<int>();
			}
			else
			{
				TMP_SpriteAsset.k_searchedSpriteAssets.Clear();
			}
			int instanceID = spriteAsset.instanceID;
			TMP_SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
			if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, true, out spriteIndex);
				if (spriteIndex != -1)
				{
					return tmp_SpriteAsset;
				}
			}
			if (includeFallbacks && TMP_Settings.defaultSpriteAsset != null)
			{
				TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCodeInternal(TMP_Settings.defaultSpriteAsset, hashCode, true, out spriteIndex);
				if (spriteIndex != -1)
				{
					return tmp_SpriteAsset;
				}
			}
			TMP_SpriteAsset.k_searchedSpriteAssets.Clear();
			uint missingCharacterSpriteUnicode = TMP_Settings.missingCharacterSpriteUnicode;
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(missingCharacterSpriteUnicode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			TMP_SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
			if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, missingCharacterSpriteUnicode, true, out spriteIndex);
				if (spriteIndex != -1)
				{
					return tmp_SpriteAsset;
				}
			}
			if (includeFallbacks && TMP_Settings.defaultSpriteAsset != null)
			{
				TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByUnicodeInternal(TMP_Settings.defaultSpriteAsset, missingCharacterSpriteUnicode, true, out spriteIndex);
				if (spriteIndex != -1)
				{
					return tmp_SpriteAsset;
				}
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000264C8 File Offset: 0x000246C8
		private static TMP_SpriteAsset SearchForSpriteByHashCodeInternal(List<TMP_SpriteAsset> spriteAssets, int hashCode, bool searchFallbacks, out int spriteIndex)
		{
			for (int i = 0; i < spriteAssets.Count; i++)
			{
				TMP_SpriteAsset tmp_SpriteAsset = spriteAssets[i];
				if (!(tmp_SpriteAsset == null))
				{
					int instanceID = tmp_SpriteAsset.instanceID;
					if (TMP_SpriteAsset.k_searchedSpriteAssets.Add(instanceID))
					{
						tmp_SpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCodeInternal(tmp_SpriteAsset, hashCode, searchFallbacks, out spriteIndex);
						if (tmp_SpriteAsset != null)
						{
							return tmp_SpriteAsset;
						}
					}
				}
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00026524 File Offset: 0x00024724
		private static TMP_SpriteAsset SearchForSpriteByHashCodeInternal(TMP_SpriteAsset spriteAsset, int hashCode, bool searchFallbacks, out int spriteIndex)
		{
			spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (searchFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				return TMP_SpriteAsset.SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, true, out spriteIndex);
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00026564 File Offset: 0x00024764
		public void SortGlyphTable()
		{
			if (this.m_SpriteGlyphTable == null || this.m_SpriteGlyphTable.Count == 0)
			{
				return;
			}
			this.m_SpriteGlyphTable = this.m_SpriteGlyphTable.OrderBy<TMP_SpriteGlyph, uint>((TMP_SpriteGlyph item) => item.index).ToList<TMP_SpriteGlyph>();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000265BC File Offset: 0x000247BC
		internal void SortCharacterTable()
		{
			if (this.m_SpriteCharacterTable != null && this.m_SpriteCharacterTable.Count > 0)
			{
				this.m_SpriteCharacterTable = this.m_SpriteCharacterTable.OrderBy<TMP_SpriteCharacter, uint>((TMP_SpriteCharacter c) => c.unicode).ToList<TMP_SpriteCharacter>();
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00026614 File Offset: 0x00024814
		internal void SortGlyphAndCharacterTables()
		{
			this.SortGlyphTable();
			this.SortCharacterTable();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00026624 File Offset: 0x00024824
		private void UpgradeSpriteAsset()
		{
			this.m_Version = "1.1.0";
			Debug.Log(string.Concat(new string[] { "Upgrading sprite asset [", base.name, "] to version ", this.m_Version, "." }), this);
			this.m_SpriteCharacterTable.Clear();
			this.m_SpriteGlyphTable.Clear();
			for (int i = 0; i < this.spriteInfoList.Count; i++)
			{
				TMP_Sprite tmp_Sprite = this.spriteInfoList[i];
				TMP_SpriteGlyph tmp_SpriteGlyph = new TMP_SpriteGlyph();
				tmp_SpriteGlyph.index = (uint)i;
				tmp_SpriteGlyph.sprite = tmp_Sprite.sprite;
				tmp_SpriteGlyph.metrics = new GlyphMetrics(tmp_Sprite.width, tmp_Sprite.height, tmp_Sprite.xOffset, tmp_Sprite.yOffset, tmp_Sprite.xAdvance);
				tmp_SpriteGlyph.glyphRect = new GlyphRect((int)tmp_Sprite.x, (int)tmp_Sprite.y, (int)tmp_Sprite.width, (int)tmp_Sprite.height);
				tmp_SpriteGlyph.scale = 1f;
				tmp_SpriteGlyph.atlasIndex = 0;
				this.m_SpriteGlyphTable.Add(tmp_SpriteGlyph);
				TMP_SpriteCharacter tmp_SpriteCharacter = new TMP_SpriteCharacter();
				tmp_SpriteCharacter.glyph = tmp_SpriteGlyph;
				tmp_SpriteCharacter.unicode = (uint)((tmp_Sprite.unicode == 0) ? 65534 : tmp_Sprite.unicode);
				tmp_SpriteCharacter.name = tmp_Sprite.name;
				tmp_SpriteCharacter.scale = tmp_Sprite.scale;
				this.m_SpriteCharacterTable.Add(tmp_SpriteCharacter);
			}
			this.UpdateLookupTables();
		}

		// Token: 0x0400039A RID: 922
		internal Dictionary<int, int> m_NameLookup;

		// Token: 0x0400039B RID: 923
		internal Dictionary<uint, int> m_GlyphIndexLookup;

		// Token: 0x0400039C RID: 924
		[SerializeField]
		private string m_Version;

		// Token: 0x0400039D RID: 925
		[SerializeField]
		internal FaceInfo m_FaceInfo;

		// Token: 0x0400039E RID: 926
		public Texture spriteSheet;

		// Token: 0x0400039F RID: 927
		[SerializeField]
		private List<TMP_SpriteCharacter> m_SpriteCharacterTable = new List<TMP_SpriteCharacter>();

		// Token: 0x040003A0 RID: 928
		internal Dictionary<uint, TMP_SpriteCharacter> m_SpriteCharacterLookup;

		// Token: 0x040003A1 RID: 929
		[SerializeField]
		private List<TMP_SpriteGlyph> m_SpriteGlyphTable = new List<TMP_SpriteGlyph>();

		// Token: 0x040003A2 RID: 930
		internal Dictionary<uint, TMP_SpriteGlyph> m_SpriteGlyphLookup;

		// Token: 0x040003A3 RID: 931
		public List<TMP_Sprite> spriteInfoList;

		// Token: 0x040003A4 RID: 932
		[SerializeField]
		public List<TMP_SpriteAsset> fallbackSpriteAssets;

		// Token: 0x040003A5 RID: 933
		internal bool m_IsSpriteAssetLookupTablesDirty;

		// Token: 0x040003A6 RID: 934
		private static HashSet<int> k_searchedSpriteAssets;
	}
}
