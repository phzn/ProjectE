using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200014B RID: 331
	[Serializable]
	public class MMAdditiveSceneLoadingManagerSettings
	{
		// Token: 0x040008CE RID: 2254
		[Tooltip("the name of the MMSceneLoadingManager scene you want to use when in additive mode")]
		public string LoadingSceneName = "MMAdditiveLoadingScreen";

		// Token: 0x040008CF RID: 2255
		[Tooltip("when in additive loading mode, the thread priority to apply to the loading")]
		public ThreadPriority ThreadPriority = ThreadPriority.High;

		// Token: 0x040008D0 RID: 2256
		[Tooltip("whether or not to make additional sanity checks (better leave this to true)")]
		public bool SecureLoad = true;

		// Token: 0x040008D1 RID: 2257
		[Tooltip("when in additive loading mode, whether or not to interpolate the progress bar's progress")]
		public bool InterpolateProgress = true;

		// Token: 0x040008D2 RID: 2258
		[Tooltip("when in additive loading mode, when in additive loading mode, the duration (in seconds) of the delay before the entry fade")]
		public float BeforeEntryFadeDelay;

		// Token: 0x040008D3 RID: 2259
		[Tooltip("when in additive loading mode, the duration (in seconds) of the entry fade")]
		public float EntryFadeDuration = 0.25f;

		// Token: 0x040008D4 RID: 2260
		[Tooltip("when in additive loading mode, the duration (in seconds) of the delay before the entry fade")]
		public float AfterEntryFadeDelay = 0.1f;

		// Token: 0x040008D5 RID: 2261
		[Tooltip("when in additive loading mode, the duration (in seconds) of the delay before the exit fade")]
		public float BeforeExitFadeDelay = 0.25f;

		// Token: 0x040008D6 RID: 2262
		[Tooltip("when in additive loading mode, the duration (in seconds) of the exit fade")]
		public float ExitFadeDuration = 0.2f;

		// Token: 0x040008D7 RID: 2263
		[Tooltip("when in additive loading mode, when in additive loading mode, the tween to use to fade on entry")]
		public MMTweenType EntryFadeTween;

		// Token: 0x040008D8 RID: 2264
		[Tooltip("when in additive loading mode, the tween to use to fade on exit")]
		public MMTweenType ExitFadeTween;

		// Token: 0x040008D9 RID: 2265
		[Tooltip("when in additive loading mode, the speed at which the loader's progress bar should move")]
		public float ProgressBarSpeed = 5f;

		// Token: 0x040008DA RID: 2266
		[Tooltip("a list of progress intervals (values should be between 0 and 1) and their associated speeds, letting you have the bar progress less linearly")]
		public List<MMSceneLoadingSpeedInterval> SpeedIntervals;

		// Token: 0x040008DB RID: 2267
		[Tooltip("when in additive loading mode, the selective additive fade mode")]
		public MMAdditiveSceneLoadingManager.FadeModes FadeMode;

		// Token: 0x040008DC RID: 2268
		[Tooltip("the chosen way to unload scenes (none, only the active scene, all loaded scenes)")]
		public MMAdditiveSceneLoadingManagerSettings.UnloadMethods UnloadMethod = MMAdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes;

		// Token: 0x040008DD RID: 2269
		[Tooltip("the name of the anti spill scene to use when loading additively.If left empty, that scene will be automatically created, but you can specify any scene to use for that. Usually you'll want your own anti spill scene to be just an empty scene, but you can customize its lighting settings for example.")]
		public string AntiSpillSceneName = "";

		// Token: 0x02000449 RID: 1097
		public enum UnloadMethods
		{
			// Token: 0x04001FE5 RID: 8165
			None,
			// Token: 0x04001FE6 RID: 8166
			ActiveScene,
			// Token: 0x04001FE7 RID: 8167
			AllScenes
		}
	}
}
