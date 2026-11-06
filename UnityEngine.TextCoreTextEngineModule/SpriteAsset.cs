using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000019 RID: 25
	[HelpURL("https://docs.unity3d.com/2022.3/Documentation/Manual/UIE-sprite.html")]
	[ExcludeFromPreset]
	public class SpriteAsset : TextAsset
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000079D4 File Offset: 0x00005BD4
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000079EC File Offset: 0x00005BEC
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000079F8 File Offset: 0x00005BF8
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007A10 File Offset: 0x00005C10
		public Texture spriteSheet
		{
			get
			{
				return this.m_SpriteAtlasTexture;
			}
			internal set
			{
				this.m_SpriteAtlasTexture = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00007A1C File Offset: 0x00005C1C
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00007A48 File Offset: 0x00005C48
		public List<SpriteCharacter> spriteCharacterTable
		{
			get
			{
				bool flag = this.m_GlyphIndexLookup == null;
				if (flag)
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00007A54 File Offset: 0x00005C54
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00007A80 File Offset: 0x00005C80
		public Dictionary<uint, SpriteCharacter> spriteCharacterLookupTable
		{
			get
			{
				bool flag = this.m_SpriteCharacterLookup == null;
				if (flag)
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007A8C File Offset: 0x00005C8C
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00007AA4 File Offset: 0x00005CA4
		public List<SpriteGlyph> spriteGlyphTable
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

		// Token: 0x060000E8 RID: 232 RVA: 0x00002F2F File Offset: 0x0000112F
		private void Awake()
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007AB0 File Offset: 0x00005CB0
		public void UpdateLookupTables()
		{
			bool flag = this.m_GlyphIndexLookup == null;
			if (flag)
			{
				this.m_GlyphIndexLookup = new Dictionary<uint, int>();
			}
			else
			{
				this.m_GlyphIndexLookup.Clear();
			}
			bool flag2 = this.m_SpriteGlyphLookup == null;
			if (flag2)
			{
				this.m_SpriteGlyphLookup = new Dictionary<uint, SpriteGlyph>();
			}
			else
			{
				this.m_SpriteGlyphLookup.Clear();
			}
			for (int i = 0; i < this.m_SpriteGlyphTable.Count; i++)
			{
				SpriteGlyph spriteGlyph = this.m_SpriteGlyphTable[i];
				uint index = spriteGlyph.index;
				bool flag3 = !this.m_GlyphIndexLookup.ContainsKey(index);
				if (flag3)
				{
					this.m_GlyphIndexLookup.Add(index, i);
				}
				bool flag4 = !this.m_SpriteGlyphLookup.ContainsKey(index);
				if (flag4)
				{
					this.m_SpriteGlyphLookup.Add(index, spriteGlyph);
				}
			}
			bool flag5 = this.m_NameLookup == null;
			if (flag5)
			{
				this.m_NameLookup = new Dictionary<int, int>();
			}
			else
			{
				this.m_NameLookup.Clear();
			}
			bool flag6 = this.m_SpriteCharacterLookup == null;
			if (flag6)
			{
				this.m_SpriteCharacterLookup = new Dictionary<uint, SpriteCharacter>();
			}
			else
			{
				this.m_SpriteCharacterLookup.Clear();
			}
			for (int j = 0; j < this.m_SpriteCharacterTable.Count; j++)
			{
				SpriteCharacter spriteCharacter = this.m_SpriteCharacterTable[j];
				bool flag7 = spriteCharacter == null;
				if (!flag7)
				{
					uint glyphIndex = spriteCharacter.glyphIndex;
					bool flag8 = !this.m_SpriteGlyphLookup.ContainsKey(glyphIndex);
					if (!flag8)
					{
						spriteCharacter.glyph = this.m_SpriteGlyphLookup[glyphIndex];
						spriteCharacter.textAsset = this;
						int hashCodeCaseInSensitive = TextUtilities.GetHashCodeCaseInSensitive(this.m_SpriteCharacterTable[j].name);
						bool flag9 = !this.m_NameLookup.ContainsKey(hashCodeCaseInSensitive);
						if (flag9)
						{
							this.m_NameLookup.Add(hashCodeCaseInSensitive, j);
						}
						uint unicode = this.m_SpriteCharacterTable[j].unicode;
						bool flag10 = unicode != 65534U && !this.m_SpriteCharacterLookup.ContainsKey(unicode);
						if (flag10)
						{
							this.m_SpriteCharacterLookup.Add(unicode, spriteCharacter);
						}
					}
				}
			}
			this.m_IsSpriteAssetLookupTablesDirty = false;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007CEC File Offset: 0x00005EEC
		public int GetSpriteIndexFromHashcode(int hashCode)
		{
			bool flag = this.m_NameLookup == null;
			if (flag)
			{
				this.UpdateLookupTables();
			}
			int num;
			bool flag2 = this.m_NameLookup.TryGetValue(hashCode, out num);
			int num2;
			if (flag2)
			{
				num2 = num;
			}
			else
			{
				num2 = -1;
			}
			return num2;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007D2C File Offset: 0x00005F2C
		public int GetSpriteIndexFromUnicode(uint unicode)
		{
			bool flag = this.m_SpriteCharacterLookup == null;
			if (flag)
			{
				this.UpdateLookupTables();
			}
			SpriteCharacter spriteCharacter;
			bool flag2 = this.m_SpriteCharacterLookup.TryGetValue(unicode, out spriteCharacter);
			int num;
			if (flag2)
			{
				num = (int)spriteCharacter.glyphIndex;
			}
			else
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007D70 File Offset: 0x00005F70
		public int GetSpriteIndexFromName(string name)
		{
			bool flag = this.m_NameLookup == null;
			if (flag)
			{
				this.UpdateLookupTables();
			}
			int hashCodeCaseInSensitive = TextUtilities.GetHashCodeCaseInSensitive(name);
			return this.GetSpriteIndexFromHashcode(hashCodeCaseInSensitive);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007DA4 File Offset: 0x00005FA4
		public static SpriteAsset SearchForSpriteByUnicode(SpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			bool flag = spriteAsset == null;
			SpriteAsset spriteAsset2;
			if (flag)
			{
				spriteIndex = -1;
				spriteAsset2 = null;
			}
			else
			{
				spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
				bool flag2 = spriteIndex != -1;
				if (flag2)
				{
					spriteAsset2 = spriteAsset;
				}
				else
				{
					bool flag3 = SpriteAsset.k_searchedSpriteAssets == null;
					if (flag3)
					{
						SpriteAsset.k_searchedSpriteAssets = new HashSet<int>();
					}
					else
					{
						SpriteAsset.k_searchedSpriteAssets.Clear();
					}
					int instanceID = spriteAsset.GetInstanceID();
					SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
					bool flag4 = includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0;
					if (flag4)
					{
						spriteAsset2 = SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, true, out spriteIndex);
					}
					else
					{
						spriteIndex = -1;
						spriteAsset2 = null;
					}
				}
			}
			return spriteAsset2;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007E54 File Offset: 0x00006054
		private static SpriteAsset SearchForSpriteByUnicodeInternal(List<SpriteAsset> spriteAssets, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			for (int i = 0; i < spriteAssets.Count; i++)
			{
				SpriteAsset spriteAsset = spriteAssets[i];
				bool flag = spriteAsset == null;
				if (!flag)
				{
					int instanceID = spriteAsset.GetInstanceID();
					bool flag2 = !SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
					if (!flag2)
					{
						spriteAsset = SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset, unicode, includeFallbacks, out spriteIndex);
						bool flag3 = spriteAsset != null;
						if (flag3)
						{
							return spriteAsset;
						}
					}
				}
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007ED4 File Offset: 0x000060D4
		private static SpriteAsset SearchForSpriteByUnicodeInternal(SpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
		{
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			bool flag = spriteIndex != -1;
			SpriteAsset spriteAsset2;
			if (flag)
			{
				spriteAsset2 = spriteAsset;
			}
			else
			{
				bool flag2 = includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0;
				if (flag2)
				{
					spriteAsset2 = SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, true, out spriteIndex);
				}
				else
				{
					spriteIndex = -1;
					spriteAsset2 = null;
				}
			}
			return spriteAsset2;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007F34 File Offset: 0x00006134
		public static SpriteAsset SearchForSpriteByHashCode(SpriteAsset spriteAsset, int hashCode, bool includeFallbacks, out int spriteIndex, TextSettings textSettings = null)
		{
			bool flag = spriteAsset == null;
			SpriteAsset spriteAsset2;
			if (flag)
			{
				spriteIndex = -1;
				spriteAsset2 = null;
			}
			else
			{
				spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
				bool flag2 = spriteIndex != -1;
				if (flag2)
				{
					spriteAsset2 = spriteAsset;
				}
				else
				{
					bool flag3 = SpriteAsset.k_searchedSpriteAssets == null;
					if (flag3)
					{
						SpriteAsset.k_searchedSpriteAssets = new HashSet<int>();
					}
					else
					{
						SpriteAsset.k_searchedSpriteAssets.Clear();
					}
					int instanceID = spriteAsset.instanceID;
					SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
					bool flag4 = includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0;
					if (flag4)
					{
						SpriteAsset spriteAsset3 = SpriteAsset.SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, true, out spriteIndex);
						bool flag5 = spriteIndex != -1;
						if (flag5)
						{
							return spriteAsset3;
						}
					}
					bool flag6 = textSettings == null;
					if (flag6)
					{
						spriteIndex = -1;
						spriteAsset2 = null;
					}
					else
					{
						bool flag7 = includeFallbacks && textSettings.defaultSpriteAsset != null;
						if (flag7)
						{
							SpriteAsset spriteAsset3 = SpriteAsset.SearchForSpriteByHashCodeInternal(textSettings.defaultSpriteAsset, hashCode, true, out spriteIndex);
							bool flag8 = spriteIndex != -1;
							if (flag8)
							{
								return spriteAsset3;
							}
						}
						SpriteAsset.k_searchedSpriteAssets.Clear();
						uint missingSpriteCharacterUnicode = textSettings.missingSpriteCharacterUnicode;
						spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(missingSpriteCharacterUnicode);
						bool flag9 = spriteIndex != -1;
						if (flag9)
						{
							spriteAsset2 = spriteAsset;
						}
						else
						{
							SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
							bool flag10 = includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0;
							if (flag10)
							{
								SpriteAsset spriteAsset3 = SpriteAsset.SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, missingSpriteCharacterUnicode, true, out spriteIndex);
								bool flag11 = spriteIndex != -1;
								if (flag11)
								{
									return spriteAsset3;
								}
							}
							bool flag12 = includeFallbacks && textSettings.defaultSpriteAsset != null;
							if (flag12)
							{
								SpriteAsset spriteAsset3 = SpriteAsset.SearchForSpriteByUnicodeInternal(textSettings.defaultSpriteAsset, missingSpriteCharacterUnicode, true, out spriteIndex);
								bool flag13 = spriteIndex != -1;
								if (flag13)
								{
									return spriteAsset3;
								}
							}
							spriteIndex = -1;
							spriteAsset2 = null;
						}
					}
				}
			}
			return spriteAsset2;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00008120 File Offset: 0x00006320
		private static SpriteAsset SearchForSpriteByHashCodeInternal(List<SpriteAsset> spriteAssets, int hashCode, bool searchFallbacks, out int spriteIndex)
		{
			for (int i = 0; i < spriteAssets.Count; i++)
			{
				SpriteAsset spriteAsset = spriteAssets[i];
				bool flag = spriteAsset == null;
				if (!flag)
				{
					int instanceID = spriteAsset.instanceID;
					bool flag2 = !SpriteAsset.k_searchedSpriteAssets.Add(instanceID);
					if (!flag2)
					{
						spriteAsset = SpriteAsset.SearchForSpriteByHashCodeInternal(spriteAsset, hashCode, searchFallbacks, out spriteIndex);
						bool flag3 = spriteAsset != null;
						if (flag3)
						{
							return spriteAsset;
						}
					}
				}
			}
			spriteIndex = -1;
			return null;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000081A0 File Offset: 0x000063A0
		private static SpriteAsset SearchForSpriteByHashCodeInternal(SpriteAsset spriteAsset, int hashCode, bool searchFallbacks, out int spriteIndex)
		{
			spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
			bool flag = spriteIndex != -1;
			SpriteAsset spriteAsset2;
			if (flag)
			{
				spriteAsset2 = spriteAsset;
			}
			else
			{
				bool flag2 = searchFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0;
				if (flag2)
				{
					spriteAsset2 = SpriteAsset.SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, true, out spriteIndex);
				}
				else
				{
					spriteIndex = -1;
					spriteAsset2 = null;
				}
			}
			return spriteAsset2;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00008200 File Offset: 0x00006400
		public void SortGlyphTable()
		{
			bool flag = this.m_SpriteGlyphTable == null || this.m_SpriteGlyphTable.Count == 0;
			if (!flag)
			{
				this.m_SpriteGlyphTable = this.m_SpriteGlyphTable.OrderBy<SpriteGlyph, uint>((SpriteGlyph item) => item.index).ToList<SpriteGlyph>();
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00008264 File Offset: 0x00006464
		internal void SortCharacterTable()
		{
			bool flag = this.m_SpriteCharacterTable != null && this.m_SpriteCharacterTable.Count > 0;
			if (flag)
			{
				this.m_SpriteCharacterTable = this.m_SpriteCharacterTable.OrderBy<SpriteCharacter, uint>((SpriteCharacter c) => c.unicode).ToList<SpriteCharacter>();
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000082C4 File Offset: 0x000064C4
		internal void SortGlyphAndCharacterTables()
		{
			this.SortGlyphTable();
			this.SortCharacterTable();
		}

		// Token: 0x040000B8 RID: 184
		internal Dictionary<int, int> m_NameLookup;

		// Token: 0x040000B9 RID: 185
		internal Dictionary<uint, int> m_GlyphIndexLookup;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		internal FaceInfo m_FaceInfo;

		// Token: 0x040000BB RID: 187
		[FormerlySerializedAs("spriteSheet")]
		[SerializeField]
		internal Texture m_SpriteAtlasTexture;

		// Token: 0x040000BC RID: 188
		[SerializeField]
		private List<SpriteCharacter> m_SpriteCharacterTable = new List<SpriteCharacter>();

		// Token: 0x040000BD RID: 189
		internal Dictionary<uint, SpriteCharacter> m_SpriteCharacterLookup;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		private List<SpriteGlyph> m_SpriteGlyphTable = new List<SpriteGlyph>();

		// Token: 0x040000BF RID: 191
		internal Dictionary<uint, SpriteGlyph> m_SpriteGlyphLookup;

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		public List<SpriteAsset> fallbackSpriteAssets;

		// Token: 0x040000C1 RID: 193
		internal bool m_IsSpriteAssetLookupTablesDirty = false;

		// Token: 0x040000C2 RID: 194
		private static HashSet<int> k_searchedSpriteAssets;
	}
}
