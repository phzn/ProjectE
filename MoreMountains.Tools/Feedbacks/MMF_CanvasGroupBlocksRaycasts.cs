using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DE RID: 478
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you turn the BlocksRaycast parameter of a target CanvasGroup on or off on play")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/CanvasGroup BlocksRaycasts")]
	public class MMF_CanvasGroupBlocksRaycasts : MMF_Feedback
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003522E File Offset: 0x0003342E
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00035231 File Offset: 0x00033431
		protected override void AutomateTargetAcquisition()
		{
			this.TargetCanvasGroup = this.FindAutomatedTarget<CanvasGroup>();
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00035240 File Offset: 0x00033440
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CanvasGroupBlocksRaycasts.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetCanvasGroup == null)
			{
				return;
			}
			this._initialState = this.TargetCanvasGroup.blocksRaycasts;
			this.TargetCanvasGroup.blocksRaycasts = this.ShouldBlockRaycasts;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0003528E File Offset: 0x0003348E
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CanvasGroupBlocksRaycasts.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetCanvasGroup.blocksRaycasts = this._initialState;
		}

		// Token: 0x04000DE1 RID: 3553
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DE2 RID: 3554
		[MMFInspectorGroup("Block Raycasts", true, 54, true, false)]
		[Tooltip("the target canvas group we want to control the BlocksRaycasts parameter on")]
		public CanvasGroup TargetCanvasGroup;

		// Token: 0x04000DE3 RID: 3555
		[Tooltip("if this is true, on play, the target canvas group will block raycasts, if false it won't")]
		public bool ShouldBlockRaycasts = true;

		// Token: 0x04000DE4 RID: 3556
		protected bool _initialState;
	}
}
