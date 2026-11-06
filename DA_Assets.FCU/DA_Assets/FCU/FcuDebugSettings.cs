using System;
using System.Linq;
using DA_Assets.Tools;

namespace DA_Assets.FCU
{
	// Token: 0x0200000E RID: 14
	public class FcuDebugSettings
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00002D54 File Offset: 0x00000F54
		static FcuDebugSettings()
		{
			FcuDebugSettingsFlags[] array = new FcuDebugSettingsFlags[]
			{
				FcuDebugSettingsFlags.LogDefault,
				FcuDebugSettingsFlags.LogSetTag,
				FcuDebugSettingsFlags.LogIsDownloadable,
				FcuDebugSettingsFlags.LogTransform,
				FcuDebugSettingsFlags.LogGameObjectDrawer,
				FcuDebugSettingsFlags.LogComponentDrawer,
				FcuDebugSettingsFlags.LogHashGenerator
			};
			FcuDebugSettings.flags = (FcuDebugSettingsFlags)LocalPrefs.GetInt("FCU_DEBUG_FLAGS", (int)array.Aggregate<FcuDebugSettingsFlags>((FcuDebugSettingsFlags acc, FcuDebugSettingsFlags flag) => acc | flag));
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002D98 File Offset: 0x00000F98
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002D9F File Offset: 0x00000F9F
		public static FcuDebugSettingsFlags Settings
		{
			get
			{
				return FcuDebugSettings.flags;
			}
			set
			{
				if (FcuDebugSettings.flags != value)
				{
					FcuDebugSettings.flags = value;
					LocalPrefs.SetInt("FCU_DEBUG_FLAGS", (int)FcuDebugSettings.flags);
				}
			}
		}

		// Token: 0x0400004F RID: 79
		private const string FCU_DEBUG_PREFS_KEY = "FCU_DEBUG_FLAGS";

		// Token: 0x04000050 RID: 80
		private static FcuDebugSettingsFlags flags;
	}
}
