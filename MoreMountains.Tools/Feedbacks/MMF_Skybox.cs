using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020D RID: 525
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the scene's skybox on play, replacing it with another one, either a specific one, or one picked at random among multiple skyboxes.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Skybox")]
	public class MMF_Skybox : MMF_Feedback
	{
		// Token: 0x06000E3B RID: 3643 RVA: 0x0003F664 File Offset: 0x0003D864
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Skybox.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialSkybox = RenderSettings.skybox;
			if (this.Mode == MMF_Skybox.Modes.Single)
			{
				RenderSettings.skybox = this.SingleSkybox;
				return;
			}
			if (this.Mode == MMF_Skybox.Modes.Random)
			{
				RenderSettings.skybox = this.RandomSkyboxes[Random.Range(0, this.RandomSkyboxes.Length)];
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0003F6C3 File Offset: 0x0003D8C3
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Skybox.FeedbackTypeAuthorized)
			{
				return;
			}
			RenderSettings.skybox = this._initialSkybox;
		}

		// Token: 0x0400107F RID: 4223
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001080 RID: 4224
		[MMFInspectorGroup("Skybox", true, 65, false, false)]
		public MMF_Skybox.Modes Mode;

		// Token: 0x04001081 RID: 4225
		public Material SingleSkybox;

		// Token: 0x04001082 RID: 4226
		public Material[] RandomSkyboxes;

		// Token: 0x04001083 RID: 4227
		protected Material _initialSkybox;

		// Token: 0x020004EE RID: 1262
		public enum Modes
		{
			// Token: 0x0400230B RID: 8971
			Single,
			// Token: 0x0400230C RID: 8972
			Random
		}
	}
}
