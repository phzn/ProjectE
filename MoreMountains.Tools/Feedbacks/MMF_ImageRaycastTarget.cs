using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001EE RID: 494
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you control the RaycastTarget parameter of a target image, turning it on or off on play")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Image RaycastTarget")]
	public class MMF_ImageRaycastTarget : MMF_Feedback
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x000374CD File Offset: 0x000356CD
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x000374D0 File Offset: 0x000356D0
		protected override void AutomateTargetAcquisition()
		{
			this.TargetImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x000374E0 File Offset: 0x000356E0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageRaycastTarget.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetImage == null)
			{
				return;
			}
			this._initialState = this.TargetImage.raycastTarget;
			this.TargetImage.raycastTarget = (this.NormalPlayDirection ? this.ShouldBeRaycastTarget : (!this.ShouldBeRaycastTarget));
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00037541 File Offset: 0x00035741
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ImageRaycastTarget.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetImage.raycastTarget = this._initialState;
		}

		// Token: 0x04000E7C RID: 3708
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E7D RID: 3709
		[MMFInspectorGroup("Image", true, 12, true, false)]
		[Tooltip("the target Image we want to control the RaycastTarget parameter on")]
		public Image TargetImage;

		// Token: 0x04000E7E RID: 3710
		[Tooltip("if this is true, when played, the target image will become a raycast target")]
		public bool ShouldBeRaycastTarget = true;

		// Token: 0x04000E7F RID: 3711
		protected bool _initialState;
	}
}
