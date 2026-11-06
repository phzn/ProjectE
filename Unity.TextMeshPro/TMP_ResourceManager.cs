using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000049 RID: 73
	public class TMP_ResourceManager
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00024A5D File Offset: 0x00022C5D
		internal static TMP_Settings GetTextSettings()
		{
			if (TMP_ResourceManager.s_TextSettings == null)
			{
				TMP_ResourceManager.s_TextSettings = Resources.Load<TMP_Settings>("TextSettings");
			}
			return TMP_ResourceManager.s_TextSettings;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00024A80 File Offset: 0x00022C80
		public static void AddFontAsset(TMP_FontAsset fontAsset)
		{
			int hashCode = fontAsset.hashCode;
			if (TMP_ResourceManager.s_FontAssetReferenceLookup.ContainsKey(hashCode))
			{
				return;
			}
			TMP_ResourceManager.s_FontAssetReferences.Add(fontAsset);
			TMP_ResourceManager.s_FontAssetReferenceLookup.Add(hashCode, fontAsset);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00024AB9 File Offset: 0x00022CB9
		public static bool TryGetFontAsset(int hashcode, out TMP_FontAsset fontAsset)
		{
			fontAsset = null;
			return TMP_ResourceManager.s_FontAssetReferenceLookup.TryGetValue(hashcode, out fontAsset);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00024ACC File Offset: 0x00022CCC
		internal static void RebuildFontAssetCache(int instanceID)
		{
			for (int i = 0; i < TMP_ResourceManager.s_FontAssetReferences.Count; i++)
			{
				TMP_FontAsset tmp_FontAsset = TMP_ResourceManager.s_FontAssetReferences[i];
				if (tmp_FontAsset.FallbackSearchQueryLookup.Contains(instanceID))
				{
					tmp_FontAsset.ReadFontAssetDefinition();
				}
			}
		}

		// Token: 0x0400028E RID: 654
		private static readonly TMP_ResourceManager s_instance = new TMP_ResourceManager();

		// Token: 0x0400028F RID: 655
		private static TMP_Settings s_TextSettings;

		// Token: 0x04000290 RID: 656
		private static readonly List<TMP_FontAsset> s_FontAssetReferences = new List<TMP_FontAsset>();

		// Token: 0x04000291 RID: 657
		private static readonly Dictionary<int, TMP_FontAsset> s_FontAssetReferenceLookup = new Dictionary<int, TMP_FontAsset>();
	}
}
