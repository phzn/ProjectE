using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029F RID: 671
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will request the load of a new scene, using the method of your choice")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Scene/Load Scene")]
	public class MMF_LoadScene : MMF_Feedback
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x00051690 File Offset: 0x0004F890
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LoadScene.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.LoadingMode)
			{
			case MMF_LoadScene.LoadingModes.Direct:
				SceneManager.LoadScene(this.DestinationSceneName);
				return;
			case MMF_LoadScene.LoadingModes.MMSceneLoadingManager:
				MMSceneLoadingManager.LoadScene(this.DestinationSceneName, this.LoadingSceneName);
				return;
			case MMF_LoadScene.LoadingModes.MMAdditiveSceneLoadingManager:
				MMAdditiveSceneLoadingManager.LoadScene(this.DestinationSceneName, this.LoadingSceneName, this.Priority, this.SecureLoad, this.InterpolateProgress, this.BeforeEntryFadeDelay, this.EntryFadeDuration, this.AfterEntryFadeDelay, this.BeforeExitFadeDelay, this.ExitFadeDuration, this.EntryFadeTween, this.ExitFadeTween, this.ProgressInterpolationSpeed, this.FadeMode, this.UnloadMethod, this.AntiSpillSceneName, this.SpeedIntervals);
				return;
			case MMF_LoadScene.LoadingModes.DirectAdditive:
				SceneManager.LoadScene(this.DestinationSceneName, LoadSceneMode.Additive);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001565 RID: 5477
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001566 RID: 5478
		[MMFInspectorGroup("Scene Loading", true, 57, true, false)]
		[Tooltip("the name of the loading screen scene to use - HAS TO BE ADDED TO YOUR BUILD SETTINGS")]
		public string LoadingSceneName = "MMAdditiveLoadingScreen";

		// Token: 0x04001567 RID: 5479
		[Tooltip("the name of the destination scene - HAS TO BE ADDED TO YOUR BUILD SETTINGS")]
		public string DestinationSceneName = "";

		// Token: 0x04001568 RID: 5480
		[Header("Mode")]
		[Tooltip("the loading mode to use to load the destination scene : - direct : uses Unity's SceneManager API- MMSceneLoadingManager : the simple, original MM way of loading scenes- MMAdditiveSceneLoadingManager : a more advanced way of loading scenes, with (way) more options")]
		public MMF_LoadScene.LoadingModes LoadingMode = MMF_LoadScene.LoadingModes.MMAdditiveSceneLoadingManager;

		// Token: 0x04001569 RID: 5481
		[Header("Loading Scene Manager")]
		[Tooltip("the priority to use when loading the new scenes")]
		public ThreadPriority Priority = ThreadPriority.High;

		// Token: 0x0400156A RID: 5482
		[Tooltip("whether or not to perform extra checks to make sure the loading screen and destination scene are in the build settings")]
		public bool SecureLoad = true;

		// Token: 0x0400156B RID: 5483
		[Tooltip("the chosen way to unload scenes (none, only the active scene, all loaded scenes)")]
		[MMFEnumCondition("LoadingMode", new int[] { 2 })]
		public MMAdditiveSceneLoadingManagerSettings.UnloadMethods UnloadMethod = MMAdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes;

		// Token: 0x0400156C RID: 5484
		[Tooltip("the name of the anti spill scene to use when loading additively.If left empty, that scene will be automatically created, but you can specify any scene to use for that. Usually you'll want your own anti spill scene to be just an empty scene, but you can customize its lighting settings for example.")]
		[MMFEnumCondition("LoadingMode", new int[] { 2 })]
		public string AntiSpillSceneName = "";

		// Token: 0x0400156D RID: 5485
		[MMFInspectorGroup("Loading Scene Delays", true, 58, false, false)]
		[Tooltip("a delay (in seconds) to apply before the first fade plays")]
		public float BeforeEntryFadeDelay;

		// Token: 0x0400156E RID: 5486
		[Tooltip("the duration (in seconds) of the entry fade")]
		public float EntryFadeDuration = 0.2f;

		// Token: 0x0400156F RID: 5487
		[Tooltip("a delay (in seconds) to apply after the first fade plays")]
		public float AfterEntryFadeDelay;

		// Token: 0x04001570 RID: 5488
		[Tooltip("a delay (in seconds) to apply before the exit fade plays")]
		public float BeforeExitFadeDelay;

		// Token: 0x04001571 RID: 5489
		[Tooltip("the duration (in seconds) of the exit fade")]
		public float ExitFadeDuration = 0.2f;

		// Token: 0x04001572 RID: 5490
		[MMFInspectorGroup("Speed", true, 59, false, false)]
		[Tooltip("whether or not to interpolate progress (slower, but usually looks better and smoother)")]
		public bool InterpolateProgress = true;

		// Token: 0x04001573 RID: 5491
		[Tooltip("the speed at which the progress bar should move if interpolated")]
		public float ProgressInterpolationSpeed = 5f;

		// Token: 0x04001574 RID: 5492
		[Tooltip("a list of progress intervals (values should be between 0 and 1) and their associated speeds, letting you have the bar progress less linearly")]
		public List<MMSceneLoadingSpeedInterval> SpeedIntervals;

		// Token: 0x04001575 RID: 5493
		[MMFInspectorGroup("Transitions", true, 59, false, false)]
		[Tooltip("the order in which to play fades (really depends on the type of fader you have in your loading screen")]
		public MMAdditiveSceneLoadingManager.FadeModes FadeMode;

		// Token: 0x04001576 RID: 5494
		[Tooltip("the tween to use on the entry fade")]
		public MMTweenType EntryFadeTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04001577 RID: 5495
		[Tooltip("the tween to use on the exit fade")]
		public MMTweenType ExitFadeTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x02000538 RID: 1336
		public enum LoadingModes
		{
			// Token: 0x040023F3 RID: 9203
			Direct,
			// Token: 0x040023F4 RID: 9204
			MMSceneLoadingManager,
			// Token: 0x040023F5 RID: 9205
			MMAdditiveSceneLoadingManager,
			// Token: 0x040023F6 RID: 9206
			DirectAdditive
		}
	}
}
