using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000200 RID: 512
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to hold a reference, that can then be used by other feedbacks to automatically set their target. It doesn't do anything when played.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Feedbacks/MMF Reference Holder")]
	public class MMF_ReferenceHolder : MMF_Feedback
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0003C564 File Offset: 0x0003A764
		public override float FeedbackDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0003C56B File Offset: 0x0003A76B
		public override bool DisplayFullHeaderColor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003C570 File Offset: 0x0003A770
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.ForceReferenceOnAll)
			{
				for (int i = 0; i < this.Owner.FeedbacksList.Count; i++)
				{
					if (this.Owner.FeedbacksList[i].HasAutomatedTargetAcquisition)
					{
						this.Owner.FeedbacksList[i].SetIndexInFeedbacksList(i);
						this.Owner.FeedbacksList[i].ForcedReferenceHolder = this;
						this.Owner.FeedbacksList[i].ForceAutomateTargetAcquisition();
					}
				}
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0003C603 File Offset: 0x0003A803
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
		}

		// Token: 0x04000FB0 RID: 4016
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FB1 RID: 4017
		[MMFInspectorGroup("References", true, 37, true, false)]
		[Tooltip("the game object to set as the target (or on which to look for a specific component as a target) of all feedbacks that may look at this reference holder for a target")]
		public GameObject GameObjectReference;

		// Token: 0x04000FB2 RID: 4018
		[Tooltip("whether or not to force this reference holder on all compatible feedbacks in the MMF Player's list")]
		public bool ForceReferenceOnAll;
	}
}
