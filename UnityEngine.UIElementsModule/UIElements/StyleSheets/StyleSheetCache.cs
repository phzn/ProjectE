using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000497 RID: 1175
	internal static class StyleSheetCache
	{
		// Token: 0x060024C6 RID: 9414 RVA: 0x0009A5B1 File Offset: 0x000987B1
		internal static void ClearCaches()
		{
			StyleSheetCache.s_RulePropertyIdsCache.Clear();
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x0009A5C0 File Offset: 0x000987C0
		internal static StylePropertyId[] GetPropertyIds(StyleSheet sheet, int ruleIndex)
		{
			StyleSheetCache.SheetHandleKey sheetHandleKey = new StyleSheetCache.SheetHandleKey(sheet, ruleIndex);
			StylePropertyId[] array;
			bool flag = !StyleSheetCache.s_RulePropertyIdsCache.TryGetValue(sheetHandleKey, out array);
			if (flag)
			{
				StyleRule styleRule = sheet.rules[ruleIndex];
				array = new StylePropertyId[styleRule.properties.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = StyleSheetCache.GetPropertyId(styleRule, i);
				}
				StyleSheetCache.s_RulePropertyIdsCache.Add(sheetHandleKey, array);
			}
			return array;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0009A640 File Offset: 0x00098840
		internal static StylePropertyId[] GetPropertyIds(StyleRule rule)
		{
			StylePropertyId[] array = new StylePropertyId[rule.properties.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = StyleSheetCache.GetPropertyId(rule, i);
			}
			return array;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x0009A680 File Offset: 0x00098880
		private static StylePropertyId GetPropertyId(StyleRule rule, int index)
		{
			StyleProperty styleProperty = rule.properties[index];
			string name = styleProperty.name;
			StylePropertyId stylePropertyId;
			bool flag = !StylePropertyUtil.s_NameToId.TryGetValue(name, out stylePropertyId);
			if (flag)
			{
				stylePropertyId = (styleProperty.isCustomProperty ? StylePropertyId.Custom : StylePropertyId.Unknown);
			}
			return stylePropertyId;
		}

		// Token: 0x040011AE RID: 4526
		private static StyleSheetCache.SheetHandleKeyComparer s_Comparer = new StyleSheetCache.SheetHandleKeyComparer();

		// Token: 0x040011AF RID: 4527
		private static Dictionary<StyleSheetCache.SheetHandleKey, StylePropertyId[]> s_RulePropertyIdsCache = new Dictionary<StyleSheetCache.SheetHandleKey, StylePropertyId[]>(StyleSheetCache.s_Comparer);

		// Token: 0x02000498 RID: 1176
		private struct SheetHandleKey
		{
			// Token: 0x060024CB RID: 9419 RVA: 0x0009A6E3 File Offset: 0x000988E3
			public SheetHandleKey(StyleSheet sheet, int index)
			{
				this.sheetInstanceID = sheet.GetInstanceID();
				this.index = index;
			}

			// Token: 0x040011B0 RID: 4528
			public readonly int sheetInstanceID;

			// Token: 0x040011B1 RID: 4529
			public readonly int index;
		}

		// Token: 0x02000499 RID: 1177
		private class SheetHandleKeyComparer : IEqualityComparer<StyleSheetCache.SheetHandleKey>
		{
			// Token: 0x060024CC RID: 9420 RVA: 0x0009A6FC File Offset: 0x000988FC
			public bool Equals(StyleSheetCache.SheetHandleKey x, StyleSheetCache.SheetHandleKey y)
			{
				return x.sheetInstanceID == y.sheetInstanceID && x.index == y.index;
			}

			// Token: 0x060024CD RID: 9421 RVA: 0x0009A730 File Offset: 0x00098930
			public int GetHashCode(StyleSheetCache.SheetHandleKey key)
			{
				return key.sheetInstanceID.GetHashCode() ^ key.index.GetHashCode();
			}
		}
	}
}
