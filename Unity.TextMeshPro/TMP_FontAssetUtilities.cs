using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000039 RID: 57
	public class TMP_FontAssetUtilities
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0001CA74 File Offset: 0x0001AC74
		public static TMP_FontAssetUtilities instance
		{
			get
			{
				return TMP_FontAssetUtilities.s_Instance;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0001CA7B File Offset: 0x0001AC7B
		public static TMP_Character GetCharacterFromFontAsset(uint unicode, TMP_FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
		{
			if (includeFallbacks)
			{
				if (TMP_FontAssetUtilities.k_SearchedAssets == null)
				{
					TMP_FontAssetUtilities.k_SearchedAssets = new HashSet<int>();
				}
				else
				{
					TMP_FontAssetUtilities.k_SearchedAssets.Clear();
				}
			}
			return TMP_FontAssetUtilities.GetCharacterFromFontAsset_Internal(unicode, sourceFontAsset, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0001CAAC File Offset: 0x0001ACAC
		private static TMP_Character GetCharacterFromFontAsset_Internal(uint unicode, TMP_FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
		{
			isAlternativeTypeface = false;
			TMP_Character tmp_Character = null;
			bool flag = (fontStyle & FontStyles.Italic) == FontStyles.Italic;
			if (flag || fontWeight != FontWeight.Regular)
			{
				TMP_FontWeightPair[] fontWeightTable = sourceFontAsset.fontWeightTable;
				int num = 4;
				if (fontWeight <= FontWeight.Regular)
				{
					if (fontWeight <= FontWeight.ExtraLight)
					{
						if (fontWeight != FontWeight.Thin)
						{
							if (fontWeight == FontWeight.ExtraLight)
							{
								num = 2;
							}
						}
						else
						{
							num = 1;
						}
					}
					else if (fontWeight != FontWeight.Light)
					{
						if (fontWeight == FontWeight.Regular)
						{
							num = 4;
						}
					}
					else
					{
						num = 3;
					}
				}
				else if (fontWeight <= FontWeight.SemiBold)
				{
					if (fontWeight != FontWeight.Medium)
					{
						if (fontWeight == FontWeight.SemiBold)
						{
							num = 6;
						}
					}
					else
					{
						num = 5;
					}
				}
				else if (fontWeight != FontWeight.Bold)
				{
					if (fontWeight != FontWeight.Heavy)
					{
						if (fontWeight == FontWeight.Black)
						{
							num = 9;
						}
					}
					else
					{
						num = 8;
					}
				}
				else
				{
					num = 7;
				}
				TMP_FontAsset tmp_FontAsset = (flag ? fontWeightTable[num].italicTypeface : fontWeightTable[num].regularTypeface);
				if (tmp_FontAsset != null)
				{
					if (tmp_FontAsset.characterLookupTable.TryGetValue(unicode, out tmp_Character))
					{
						isAlternativeTypeface = true;
						return tmp_Character;
					}
					if (tmp_FontAsset.atlasPopulationMode == AtlasPopulationMode.Dynamic && tmp_FontAsset.TryAddCharacterInternal(unicode, out tmp_Character))
					{
						isAlternativeTypeface = true;
						return tmp_Character;
					}
				}
			}
			if (sourceFontAsset.characterLookupTable.TryGetValue(unicode, out tmp_Character))
			{
				return tmp_Character;
			}
			if (sourceFontAsset.atlasPopulationMode == AtlasPopulationMode.Dynamic && sourceFontAsset.TryAddCharacterInternal(unicode, out tmp_Character))
			{
				return tmp_Character;
			}
			if (tmp_Character == null && includeFallbacks && sourceFontAsset.fallbackFontAssetTable != null)
			{
				List<TMP_FontAsset> fallbackFontAssetTable = sourceFontAsset.fallbackFontAssetTable;
				int count = fallbackFontAssetTable.Count;
				if (count == 0)
				{
					return null;
				}
				for (int i = 0; i < count; i++)
				{
					TMP_FontAsset tmp_FontAsset2 = fallbackFontAssetTable[i];
					if (!(tmp_FontAsset2 == null))
					{
						int instanceID = tmp_FontAsset2.instanceID;
						if (TMP_FontAssetUtilities.k_SearchedAssets.Add(instanceID))
						{
							sourceFontAsset.FallbackSearchQueryLookup.Add(instanceID);
							tmp_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset_Internal(unicode, tmp_FontAsset2, true, fontStyle, fontWeight, out isAlternativeTypeface);
							if (tmp_Character != null)
							{
								return tmp_Character;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0001CC80 File Offset: 0x0001AE80
		public static TMP_Character GetCharacterFromFontAssets(uint unicode, TMP_FontAsset sourceFontAsset, List<TMP_FontAsset> fontAssets, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
		{
			isAlternativeTypeface = false;
			if (fontAssets == null || fontAssets.Count == 0)
			{
				return null;
			}
			if (includeFallbacks)
			{
				if (TMP_FontAssetUtilities.k_SearchedAssets == null)
				{
					TMP_FontAssetUtilities.k_SearchedAssets = new HashSet<int>();
				}
				else
				{
					TMP_FontAssetUtilities.k_SearchedAssets.Clear();
				}
			}
			int count = fontAssets.Count;
			for (int i = 0; i < count; i++)
			{
				TMP_FontAsset tmp_FontAsset = fontAssets[i];
				if (!(tmp_FontAsset == null))
				{
					sourceFontAsset.FallbackSearchQueryLookup.Add(tmp_FontAsset.instanceID);
					TMP_Character characterFromFontAsset_Internal = TMP_FontAssetUtilities.GetCharacterFromFontAsset_Internal(unicode, tmp_FontAsset, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface);
					if (characterFromFontAsset_Internal != null)
					{
						return characterFromFontAsset_Internal;
					}
				}
			}
			return null;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		public static TMP_SpriteCharacter GetSpriteCharacterFromSpriteAsset(uint unicode, TMP_SpriteAsset spriteAsset, bool includeFallbacks)
		{
			if (spriteAsset == null)
			{
				return null;
			}
			TMP_SpriteCharacter spriteCharacterFromSpriteAsset_Internal;
			if (spriteAsset.spriteCharacterLookupTable.TryGetValue(unicode, out spriteCharacterFromSpriteAsset_Internal))
			{
				return spriteCharacterFromSpriteAsset_Internal;
			}
			if (includeFallbacks)
			{
				if (TMP_FontAssetUtilities.k_SearchedAssets == null)
				{
					TMP_FontAssetUtilities.k_SearchedAssets = new HashSet<int>();
				}
				else
				{
					TMP_FontAssetUtilities.k_SearchedAssets.Clear();
				}
				TMP_FontAssetUtilities.k_SearchedAssets.Add(spriteAsset.instanceID);
				List<TMP_SpriteAsset> fallbackSpriteAssets = spriteAsset.fallbackSpriteAssets;
				if (fallbackSpriteAssets != null && fallbackSpriteAssets.Count > 0)
				{
					int count = fallbackSpriteAssets.Count;
					for (int i = 0; i < count; i++)
					{
						TMP_SpriteAsset tmp_SpriteAsset = fallbackSpriteAssets[i];
						if (!(tmp_SpriteAsset == null))
						{
							int instanceID = tmp_SpriteAsset.instanceID;
							if (TMP_FontAssetUtilities.k_SearchedAssets.Add(instanceID))
							{
								spriteCharacterFromSpriteAsset_Internal = TMP_FontAssetUtilities.GetSpriteCharacterFromSpriteAsset_Internal(unicode, tmp_SpriteAsset, true);
								if (spriteCharacterFromSpriteAsset_Internal != null)
								{
									return spriteCharacterFromSpriteAsset_Internal;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0001CDCC File Offset: 0x0001AFCC
		private static TMP_SpriteCharacter GetSpriteCharacterFromSpriteAsset_Internal(uint unicode, TMP_SpriteAsset spriteAsset, bool includeFallbacks)
		{
			TMP_SpriteCharacter spriteCharacterFromSpriteAsset_Internal;
			if (spriteAsset.spriteCharacterLookupTable.TryGetValue(unicode, out spriteCharacterFromSpriteAsset_Internal))
			{
				return spriteCharacterFromSpriteAsset_Internal;
			}
			if (includeFallbacks)
			{
				List<TMP_SpriteAsset> fallbackSpriteAssets = spriteAsset.fallbackSpriteAssets;
				if (fallbackSpriteAssets != null && fallbackSpriteAssets.Count > 0)
				{
					int count = fallbackSpriteAssets.Count;
					for (int i = 0; i < count; i++)
					{
						TMP_SpriteAsset tmp_SpriteAsset = fallbackSpriteAssets[i];
						if (!(tmp_SpriteAsset == null))
						{
							int instanceID = tmp_SpriteAsset.instanceID;
							if (TMP_FontAssetUtilities.k_SearchedAssets.Add(instanceID))
							{
								spriteCharacterFromSpriteAsset_Internal = TMP_FontAssetUtilities.GetSpriteCharacterFromSpriteAsset_Internal(unicode, tmp_SpriteAsset, true);
								if (spriteCharacterFromSpriteAsset_Internal != null)
								{
									return spriteCharacterFromSpriteAsset_Internal;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040001EB RID: 491
		private static readonly TMP_FontAssetUtilities s_Instance = new TMP_FontAssetUtilities();

		// Token: 0x040001EC RID: 492
		private static HashSet<int> k_SearchedAssets;

		// Token: 0x040001ED RID: 493
		private static bool k_IsFontEngineInitialized;
	}
}
