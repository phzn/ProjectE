using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026E RID: 622
	[AddComponentMenu("")]
	[FeedbackHelp("A feedback used to pilot color springs")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Springs/Spring Color")]
	public class MMF_SpringColor : MMF_Feedback
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x0004E633 File Offset: 0x0004C833
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x0004E641 File Offset: 0x0004C841
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.DeclaredDuration);
			}
			set
			{
				this.DeclaredDuration = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0004E64A File Offset: 0x0004C84A
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0004E64D File Offset: 0x0004C84D
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004E650 File Offset: 0x0004C850
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringColor.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringColorEvent.Trigger(this.Command, this.TargetSpring, this._eventChannelData, this.MoveToColor, this.BumpColor, this.MoveToRandomColorMin, this.MoveToRandomColorMax, this.BumpRandomColorMin, this.BumpRandomColorMax, this.OverrideDamping, this.NewDamping, this.OverrideFrequency, this.NewFrequency);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004E6E0 File Offset: 0x0004C8E0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringColor.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringColorEvent.Trigger(SpringCommands.Stop, this.TargetSpring, this._eventChannelData, default(Color), default(Color), default(Color), default(Color), default(Color), default(Color), false, 0f, false, 0f);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004E778 File Offset: 0x0004C978
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpringColor.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringColorEvent.Trigger(SpringCommands.RestoreInitialValue, this.TargetSpring, this._eventChannelData, default(Color), default(Color), default(Color), default(Color), default(Color), default(Color), false, 0f, false, 0f);
		}

		// Token: 0x04001487 RID: 5255
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001488 RID: 5256
		[MMFInspectorGroup("Spring", true, 72, false, false)]
		[Tooltip("the Color spring we want to pilot using this feedback. If you set one, only that spring will be targeted. If you don't, an event will be sent out to all springs matching the channel data info")]
		public MMSpringComponentBase TargetSpring;

		// Token: 0x04001489 RID: 5257
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x0400148A RID: 5258
		[Tooltip("the command to use on that spring")]
		public SpringCommands Command = SpringCommands.Bump;

		// Token: 0x0400148B RID: 5259
		[MMEnumCondition("Command", new int[] { 0, 1, 2, 4 })]
		[Tooltip("the new color this spring should move towards")]
		public Color MoveToColor = MMColors.Aquamarine;

		// Token: 0x0400148C RID: 5260
		[Tooltip("the color to add to the spring's current velocity to disturb it and make it bump")]
		[MMEnumCondition("Command", new int[] { 5 })]
		public Color BumpColor = MMColors.Orange;

		// Token: 0x0400148D RID: 5261
		[Tooltip("the min color from which to pick a random color in MoveToRandom mode")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Color MoveToRandomColorMin = MMColors.LawnGreen;

		// Token: 0x0400148E RID: 5262
		[Tooltip("the max color from which to pick a random color in MoveToRandom mode")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Color MoveToRandomColorMax = MMColors.MediumSeaGreen;

		// Token: 0x0400148F RID: 5263
		[Tooltip("the min color from which to pick a random color in BumpRandom mode")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Color BumpRandomColorMin = MMColors.HotPink;

		// Token: 0x04001490 RID: 5264
		[Tooltip("the max color from which to pick a random color in BumpRandom mode")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Color BumpRandomColorMax = MMColors.Plum;

		// Token: 0x04001491 RID: 5265
		[Header("Overrides")]
		[Tooltip("whether or not to override the current Damping value of the target spring(s) with the one specified below (NewDamping)")]
		public bool OverrideDamping;

		// Token: 0x04001492 RID: 5266
		[Tooltip("the new damping value to apply to the target spring(s) if OverrideDamping is true")]
		[MMFCondition("OverrideDamping", true)]
		public float NewDamping = 0.8f;

		// Token: 0x04001493 RID: 5267
		[Tooltip("whether or not to override the current Frequency value of the target spring(s) with the one specified below (NewFrequency)")]
		public bool OverrideFrequency;

		// Token: 0x04001494 RID: 5268
		[Tooltip("the new frequency value to apply to the target spring(s) if OverrideFrequency is true")]
		[MMFCondition("OverrideFrequency", true)]
		public float NewFrequency = 5f;

		// Token: 0x04001495 RID: 5269
		protected MMChannelData _eventChannelData;
	}
}
