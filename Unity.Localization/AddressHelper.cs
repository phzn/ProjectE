using System;

namespace UnityEngine.Localization
{
	// Token: 0x0200001E RID: 30
	internal class AddressHelper
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x000043B4 File Offset: 0x000025B4
		public static string GetTableAddress(string tableName, LocaleIdentifier localeId)
		{
			return string.Format("{0}{1}{2}", tableName, '_', localeId.Code);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000043CF File Offset: 0x000025CF
		public static string GetSharedTableAddress(string tableName)
		{
			return tableName + " Shared Data";
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000043DC File Offset: 0x000025DC
		public static string FormatAssetLabel(LocaleIdentifier localeIdentifier)
		{
			return "Locale-" + localeIdentifier.Code;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000043EF File Offset: 0x000025EF
		public static bool IsLocaleLabel(string label)
		{
			return label.StartsWith("Locale-", StringComparison.InvariantCulture);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004400 File Offset: 0x00002600
		public static LocaleIdentifier LocaleLabelToId(string label)
		{
			return default(LocaleIdentifier);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004416 File Offset: 0x00002616
		public static bool TryGetLocaleLabelToId(string label, out LocaleIdentifier localeId)
		{
			if (!AddressHelper.IsLocaleLabel(label))
			{
				localeId = default(LocaleIdentifier);
				return false;
			}
			localeId = label.Substring("Locale-".Length, label.Length - "Locale-".Length);
			return true;
		}

		// Token: 0x04000031 RID: 49
		private const char k_Separator = '_';

		// Token: 0x04000032 RID: 50
		private const string k_AssetLabelPrefix = "Locale-";
	}
}
