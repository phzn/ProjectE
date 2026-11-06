using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020B RID: 523
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you change the parent of a transform.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Set Parent")]
	public class MMF_SetParent : MMF_Feedback
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0003F24F File Offset: 0x0003D44F
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003F252 File Offset: 0x0003D452
		protected override void AutomateTargetAcquisition()
		{
			this.ObjectToParent = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0003F260 File Offset: 0x0003D460
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SetParent.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.ObjectToParent == null)
			{
				Debug.LogWarning("No object to parent was set for " + this.Owner.name);
				return;
			}
			this.ObjectToParent.SetParent(this.NewParent, this.WorldPositionStays);
		}

		// Token: 0x0400106A RID: 4202
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400106B RID: 4203
		[MMFInspectorGroup("Parenting", true, 12, true, false)]
		[Tooltip("the object we want to change the parent of")]
		public Transform ObjectToParent;

		// Token: 0x0400106C RID: 4204
		[Tooltip("the object ObjectToParent should now be parented to after playing this feedback")]
		public Transform NewParent;

		// Token: 0x0400106D RID: 4205
		[Tooltip("if true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before")]
		public bool WorldPositionStays = true;
	}
}
