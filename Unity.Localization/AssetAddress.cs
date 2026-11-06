using System;

namespace UnityEngine.Localization
{
	// Token: 0x0200001F RID: 31
	internal static class AssetAddress
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x0000445E File Offset: 0x0000265E
		public static bool IsSubAsset(string address)
		{
			return address != null && address.EndsWith("]");
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004470 File Offset: 0x00002670
		public static string GetGuid(string address)
		{
			if (!AssetAddress.IsSubAsset(address))
			{
				return address;
			}
			int num = address.IndexOf("[");
			return address.Substring(0, num);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000449C File Offset: 0x0000269C
		public static string GetSubAssetName(string address)
		{
			if (!AssetAddress.IsSubAsset(address))
			{
				return null;
			}
			int num = address.IndexOf("[");
			int num2 = address.Length - num - 2;
			return address.Substring(num + 1, num2);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000044D4 File Offset: 0x000026D4
		public static string FormatAddress(string guid, string subAssetName)
		{
			return guid + "[" + subAssetName + "]";
		}

		// Token: 0x04000033 RID: 51
		private const string k_SubAssetEntryStartBracket = "[";

		// Token: 0x04000034 RID: 52
		private const string k_SubAssetEntryEndBracket = "]";
	}
}
