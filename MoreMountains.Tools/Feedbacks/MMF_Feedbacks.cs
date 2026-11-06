using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E5 RID: 485
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to trigger a target MMFeedbacks, or any MMFeedbacks on the specified Channel within a certain range. You'll need an MMFeedbacksShaker on them.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Feedbacks/Feedbacks Player")]
	public class MMF_Feedbacks : MMF_Feedback
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00035F98 File Offset: 0x00034198
		public override float FeedbackDuration
		{
			get
			{
				if (this.TargetFeedbacks == this.Owner)
				{
					return 0f;
				}
				if (this.Mode == MMF_Feedbacks.Modes.PlayTargetFeedbacks && this.TargetFeedbacks != null)
				{
					return this.TargetFeedbacks.TotalDuration;
				}
				return 0f;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00035FE6 File Offset: 0x000341E6
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00035FE9 File Offset: 0x000341E9
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.EventOriginTransform == null)
			{
				this.EventOriginTransform = owner.transform;
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0003600C File Offset: 0x0003420C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.TargetFeedbacks == this.Owner)
			{
				return;
			}
			if (!this.Active || !MMF_Feedbacks.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Mode == MMF_Feedbacks.Modes.PlayFeedbacksInArea)
			{
				MMFeedbacksShakeEvent.Trigger(this.ChannelData, this.OnlyTriggerPlayersInRange, this.EventRange, this.EventOriginTransform.position);
				return;
			}
			if (this.Mode == MMF_Feedbacks.Modes.PlayTargetFeedbacks)
			{
				MMFeedbacks targetFeedbacks = this.TargetFeedbacks;
				if (targetFeedbacks == null)
				{
					return;
				}
				targetFeedbacks.PlayFeedbacks(position, feedbacksIntensity, false);
			}
		}

		// Token: 0x04000E2C RID: 3628
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E2D RID: 3629
		[MMFInspectorGroup("Feedbacks", true, 79, false, false)]
		[Tooltip("the selected mode for this feedback")]
		public MMF_Feedbacks.Modes Mode;

		// Token: 0x04000E2E RID: 3630
		[MMFEnumCondition("Mode", new int[] { 1 })]
		[Tooltip("a specific MMFeedbacks / MMF_Player to play")]
		public MMFeedbacks TargetFeedbacks;

		// Token: 0x04000E2F RID: 3631
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[Tooltip("whether or not to use a range")]
		public bool OnlyTriggerPlayersInRange;

		// Token: 0x04000E30 RID: 3632
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[Tooltip("the range of the event, in units")]
		public float EventRange = 100f;

		// Token: 0x04000E31 RID: 3633
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[Tooltip("the transform to use to broadcast the event as origin point")]
		public Transform EventOriginTransform;

		// Token: 0x020004B3 RID: 1203
		public enum Modes
		{
			// Token: 0x040021E9 RID: 8681
			PlayFeedbacksInArea,
			// Token: 0x040021EA RID: 8682
			PlayTargetFeedbacks
		}
	}
}
