using System;
using UnityEngine;

namespace Features.CielOS.Panel
{
	// Token: 0x020013E0 RID: 5088
	public static class CielOSQuickToolsPrefs
	{
		// Token: 0x06009D12 RID: 40210 RVA: 0x00407E40 File Offset: 0x00406040
		public static bool IsPinned(string appId, bool assetDefault)
		{
			if (string.IsNullOrEmpty(appId))
			{
				return assetDefault;
			}
			string text = "CielOS.QuickTools.Pinned." + appId;
			if (!PlayerPrefs.HasKey(text))
			{
				return assetDefault;
			}
			return PlayerPrefs.GetInt(text, assetDefault ? 1 : 0) != 0;
		}

		// Token: 0x06009D13 RID: 40211 RVA: 0x00407E7D File Offset: 0x0040607D
		public static bool HasPlayerOverride(string appId)
		{
			return !string.IsNullOrEmpty(appId) && PlayerPrefs.HasKey("CielOS.QuickTools.Pinned." + appId);
		}

		// Token: 0x06009D14 RID: 40212 RVA: 0x00407E99 File Offset: 0x00406099
		public static void SetPinned(string appId, bool pinned)
		{
			if (string.IsNullOrEmpty(appId))
			{
				return;
			}
			PlayerPrefs.SetInt("CielOS.QuickTools.Pinned." + appId, pinned ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06009D15 RID: 40213 RVA: 0x00407EC0 File Offset: 0x004060C0
		public static bool Toggle(string appId, bool assetDefault)
		{
			bool flag = !CielOSQuickToolsPrefs.IsPinned(appId, assetDefault);
			CielOSQuickToolsPrefs.SetPinned(appId, flag);
			return flag;
		}

		// Token: 0x06009D16 RID: 40214 RVA: 0x00407EE0 File Offset: 0x004060E0
		public static void ClearOverride(string appId)
		{
			if (string.IsNullOrEmpty(appId))
			{
				return;
			}
			PlayerPrefs.DeleteKey("CielOS.QuickTools.Pinned." + appId);
			PlayerPrefs.Save();
		}

		// Token: 0x0400A195 RID: 41365
		private const string KeyPrefix = "CielOS.QuickTools.Pinned.";
	}
}
