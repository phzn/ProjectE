using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000049 RID: 73
	internal class TextResourceManager
	{
		// Token: 0x0600020E RID: 526 RVA: 0x000229CC File Offset: 0x00020BCC
		internal static void AddFontAsset(FontAsset fontAsset)
		{
			int instanceID = fontAsset.instanceID;
			bool flag = !TextResourceManager.s_FontAssetReferences.ContainsKey(instanceID);
			if (flag)
			{
				TextResourceManager.FontAssetRef fontAssetRef = new TextResourceManager.FontAssetRef(fontAsset.hashCode, fontAsset.familyNameHashCode, fontAsset.styleNameHashCode, fontAsset);
				TextResourceManager.s_FontAssetReferences.Add(instanceID, fontAssetRef);
				bool flag2 = !TextResourceManager.s_FontAssetNameReferenceLookup.ContainsKey(fontAssetRef.nameHashCode);
				if (flag2)
				{
					TextResourceManager.s_FontAssetNameReferenceLookup.Add(fontAssetRef.nameHashCode, fontAsset);
				}
				bool flag3 = !TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.ContainsKey(fontAssetRef.familyNameAndStyleHashCode);
				if (flag3)
				{
					TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.Add(fontAssetRef.familyNameAndStyleHashCode, fontAsset);
				}
			}
			else
			{
				TextResourceManager.FontAssetRef fontAssetRef2 = TextResourceManager.s_FontAssetReferences[instanceID];
				bool flag4 = fontAssetRef2.nameHashCode == fontAsset.hashCode && fontAssetRef2.familyNameHashCode == fontAsset.familyNameHashCode && fontAssetRef2.styleNameHashCode == fontAsset.styleNameHashCode;
				if (!flag4)
				{
					bool flag5 = fontAssetRef2.nameHashCode != fontAsset.hashCode;
					if (flag5)
					{
						TextResourceManager.s_FontAssetNameReferenceLookup.Remove(fontAssetRef2.nameHashCode);
						fontAssetRef2.nameHashCode = fontAsset.hashCode;
						bool flag6 = !TextResourceManager.s_FontAssetNameReferenceLookup.ContainsKey(fontAssetRef2.nameHashCode);
						if (flag6)
						{
							TextResourceManager.s_FontAssetNameReferenceLookup.Add(fontAssetRef2.nameHashCode, fontAsset);
						}
					}
					bool flag7 = fontAssetRef2.familyNameHashCode != fontAsset.familyNameHashCode || fontAssetRef2.styleNameHashCode != fontAsset.styleNameHashCode;
					if (flag7)
					{
						TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.Remove(fontAssetRef2.familyNameAndStyleHashCode);
						fontAssetRef2.familyNameHashCode = fontAsset.familyNameHashCode;
						fontAssetRef2.styleNameHashCode = fontAsset.styleNameHashCode;
						fontAssetRef2.familyNameAndStyleHashCode = ((long)fontAsset.styleNameHashCode << 32) | (long)((ulong)fontAsset.familyNameHashCode);
						bool flag8 = !TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.ContainsKey(fontAssetRef2.familyNameAndStyleHashCode);
						if (flag8)
						{
							TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.Add(fontAssetRef2.familyNameAndStyleHashCode, fontAsset);
						}
					}
					TextResourceManager.s_FontAssetReferences[instanceID] = fontAssetRef2;
				}
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00022BD4 File Offset: 0x00020DD4
		public static void RemoveFontAsset(FontAsset fontAsset)
		{
			int instanceID = fontAsset.instanceID;
			TextResourceManager.FontAssetRef fontAssetRef;
			bool flag = TextResourceManager.s_FontAssetReferences.TryGetValue(instanceID, out fontAssetRef);
			if (flag)
			{
				TextResourceManager.s_FontAssetNameReferenceLookup.Remove(fontAssetRef.nameHashCode);
				TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.Remove(fontAssetRef.familyNameAndStyleHashCode);
				TextResourceManager.s_FontAssetReferences.Remove(instanceID);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00022C2C File Offset: 0x00020E2C
		internal static bool TryGetFontAssetByName(int nameHashcode, out FontAsset fontAsset)
		{
			fontAsset = null;
			return TextResourceManager.s_FontAssetNameReferenceLookup.TryGetValue(nameHashcode, out fontAsset);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00022C50 File Offset: 0x00020E50
		internal static bool TryGetFontAssetByFamilyName(int familyNameHashCode, int styleNameHashCode, out FontAsset fontAsset)
		{
			fontAsset = null;
			bool flag = styleNameHashCode == 0;
			if (flag)
			{
				styleNameHashCode = TextResourceManager.k_RegularStyleHashCode;
			}
			long num = ((long)styleNameHashCode << 32) | (long)((ulong)familyNameHashCode);
			return TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.TryGetValue(num, out fontAsset);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00022C8C File Offset: 0x00020E8C
		internal static void RebuildFontAssetCache()
		{
			foreach (KeyValuePair<int, TextResourceManager.FontAssetRef> keyValuePair in TextResourceManager.s_FontAssetReferences)
			{
				TextResourceManager.FontAssetRef value = keyValuePair.Value;
				FontAsset fontAsset = value.fontAsset;
				bool flag = fontAsset == null;
				if (flag)
				{
					TextResourceManager.s_FontAssetNameReferenceLookup.Remove(value.nameHashCode);
					TextResourceManager.s_FontAssetFamilyNameAndStyleReferenceLookup.Remove(value.familyNameAndStyleHashCode);
					TextResourceManager.s_FontAssetRemovalList.Add(keyValuePair.Key);
				}
				else
				{
					fontAsset.InitializeCharacterLookupDictionary();
					fontAsset.AddSynthesizedCharactersAndFaceMetrics();
				}
			}
			for (int i = 0; i < TextResourceManager.s_FontAssetRemovalList.Count; i++)
			{
				TextResourceManager.s_FontAssetReferences.Remove(TextResourceManager.s_FontAssetRemovalList[i]);
			}
			TextResourceManager.s_FontAssetRemovalList.Clear();
			TextEventManager.ON_FONT_PROPERTY_CHANGED(true, null);
		}

		// Token: 0x0400039A RID: 922
		private static readonly Dictionary<int, TextResourceManager.FontAssetRef> s_FontAssetReferences = new Dictionary<int, TextResourceManager.FontAssetRef>();

		// Token: 0x0400039B RID: 923
		private static readonly Dictionary<int, FontAsset> s_FontAssetNameReferenceLookup = new Dictionary<int, FontAsset>();

		// Token: 0x0400039C RID: 924
		private static readonly Dictionary<long, FontAsset> s_FontAssetFamilyNameAndStyleReferenceLookup = new Dictionary<long, FontAsset>();

		// Token: 0x0400039D RID: 925
		private static readonly List<int> s_FontAssetRemovalList = new List<int>(16);

		// Token: 0x0400039E RID: 926
		private static readonly int k_RegularStyleHashCode = TextUtilities.GetHashCodeCaseInSensitive("Regular");

		// Token: 0x0200004A RID: 74
		private struct FontAssetRef
		{
			// Token: 0x06000215 RID: 533 RVA: 0x00022DC3 File Offset: 0x00020FC3
			public FontAssetRef(int nameHashCode, int familyNameHashCode, int styleNameHashCode, FontAsset fontAsset)
			{
				this.nameHashCode = ((nameHashCode != 0) ? nameHashCode : familyNameHashCode);
				this.familyNameHashCode = familyNameHashCode;
				this.styleNameHashCode = styleNameHashCode;
				this.familyNameAndStyleHashCode = ((long)styleNameHashCode << 32) | (long)((ulong)familyNameHashCode);
				this.fontAsset = fontAsset;
			}

			// Token: 0x0400039F RID: 927
			public int nameHashCode;

			// Token: 0x040003A0 RID: 928
			public int familyNameHashCode;

			// Token: 0x040003A1 RID: 929
			public int styleNameHashCode;

			// Token: 0x040003A2 RID: 930
			public long familyNameAndStyleHashCode;

			// Token: 0x040003A3 RID: 931
			public readonly FontAsset fontAsset;
		}
	}
}
