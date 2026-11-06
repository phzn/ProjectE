using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000293 RID: 659
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you broadcast a float value to the MMRadio system.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("GameObject/Broadcast")]
	public class MMF_Broadcast : MMF_FeedbackBase
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0004FE7D File Offset: 0x0004E07D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0004FE80 File Offset: 0x0004E080
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._proxy = this.Owner.gameObject.AddComponent<MMF_BroadcastProxy>();
			this._proxy.Channel = this.Channel;
			this.PrepareTargets();
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0004FEB8 File Offset: 0x0004E0B8
		protected override void FillTargets()
		{
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget.Target = new MMPropertyReceiver
			{
				TargetObject = this.Owner.gameObject,
				TargetComponent = this._proxy,
				TargetPropertyName = "ThisLevel",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.Curve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantChange;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x040014E2 RID: 5346
		[Header("Level")]
		[Tooltip("the curve to tween the intensity on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType Curve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040014E3 RID: 5347
		[Tooltip("the value to remap the intensity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x040014E4 RID: 5348
		[Tooltip("the value to remap the intensity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x040014E5 RID: 5349
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantChange;

		// Token: 0x040014E6 RID: 5350
		protected MMF_BroadcastProxy _proxy;
	}
}
