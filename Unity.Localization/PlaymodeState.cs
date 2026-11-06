using System;

namespace UnityEngine.Localization
{
	// Token: 0x0200001D RID: 29
	internal static class PlaymodeState
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004397 File Offset: 0x00002597
		public static bool IsChangingPlayMode
		{
			get
			{
				return PlaymodeState.IsPlayingOrWillChangePlaymode && !PlaymodeState.IsPlaying;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000043AA File Offset: 0x000025AA
		public static bool IsPlayingOrWillChangePlaymode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000043AD File Offset: 0x000025AD
		public static bool IsPlaying
		{
			get
			{
				return Application.isPlaying;
			}
		}
	}
}
