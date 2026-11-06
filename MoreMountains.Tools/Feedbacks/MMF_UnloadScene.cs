using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000216 RID: 534
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you unload a scene by name or build index")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Scene/Unload Scene")]
	public class MMF_UnloadScene : MMF_Feedback
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x000415A0 File Offset: 0x0003F7A0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UnloadScene.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Method == MMF_UnloadScene.Methods.BuildIndex)
			{
				this._sceneToUnload = SceneManager.GetSceneByBuildIndex(this.BuildIndex);
			}
			else
			{
				this._sceneToUnload = SceneManager.GetSceneByName(this.SceneName);
			}
			Scene sceneToUnload = this._sceneToUnload;
			if (this._sceneToUnload.isLoaded)
			{
				SceneManager.UnloadSceneAsync(this._sceneToUnload);
				return;
			}
			if (this.OutputWarningsIfNeeded)
			{
				Debug.LogWarning("Unload Scene Feedback : you're trying to unload a scene that hasn't been loaded.");
			}
		}

		// Token: 0x040010FD RID: 4349
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010FE RID: 4350
		[MMFInspectorGroup("Unload Scene", true, 43, false, false)]
		[Tooltip("whether to unload a scene by build index or by name")]
		public MMF_UnloadScene.Methods Method = MMF_UnloadScene.Methods.SceneName;

		// Token: 0x040010FF RID: 4351
		[Tooltip("the build ID of the scene to unload, find it in your Build Settings")]
		[MMFEnumCondition("Method", new int[] { 0 })]
		public int BuildIndex;

		// Token: 0x04001100 RID: 4352
		[Tooltip("the name of the scene to unload")]
		[MMFEnumCondition("Method", new int[] { 1 })]
		public string SceneName = "";

		// Token: 0x04001101 RID: 4353
		[Tooltip("whether or not to output warnings if the scene doesn't exist or can't be loaded")]
		public bool OutputWarningsIfNeeded = true;

		// Token: 0x04001102 RID: 4354
		protected Scene _sceneToUnload;

		// Token: 0x02000503 RID: 1283
		public enum ColorModes
		{
			// Token: 0x04002370 RID: 9072
			Instant,
			// Token: 0x04002371 RID: 9073
			Gradient,
			// Token: 0x04002372 RID: 9074
			Interpolate
		}

		// Token: 0x02000504 RID: 1284
		public enum Methods
		{
			// Token: 0x04002374 RID: 9076
			BuildIndex,
			// Token: 0x04002375 RID: 9077
			SceneName
		}
	}
}
