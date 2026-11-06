using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000272 RID: 626
	[AddComponentMenu("")]
	[FeedbackHelp("A feedback used to pilot Vector4 springs")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Springs/Spring Vector4")]
	public class MMF_SpringVector4 : MMF_Feedback
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0004F01A File Offset: 0x0004D21A
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x0004F028 File Offset: 0x0004D228
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x0004F031 File Offset: 0x0004D231
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x0004F034 File Offset: 0x0004D234
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0004F038 File Offset: 0x0004D238
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector4.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector4Event.Trigger(this.Command, this.TargetSpring, this._eventChannelData, this.MoveToValue, this.BumpAmount, this.MoveToRandomValueMin, this.MoveToRandomValueMax, this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax, this.OverrideDamping, this.NewDamping, this.OverrideFrequency, this.NewFrequency);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0004F0C8 File Offset: 0x0004D2C8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector4.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector4Event.Trigger(SpringCommands.Stop, this.TargetSpring, this._eventChannelData, default(Vector4), default(Vector4), default(Vector4), default(Vector4), default(Vector4), default(Vector4), false, default(Vector4), false, default(Vector4));
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0004F168 File Offset: 0x0004D368
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpringVector4.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector4Event.Trigger(SpringCommands.RestoreInitialValue, this.TargetSpring, this._eventChannelData, default(Vector4), default(Vector4), default(Vector4), default(Vector4), default(Vector4), default(Vector4), false, default(Vector4), false, default(Vector4));
		}

		// Token: 0x040014C1 RID: 5313
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014C2 RID: 5314
		[MMFInspectorGroup("Spring", true, 72, false, false)]
		[Tooltip("the spring we want to pilot using this feedback. If you set one, only that spring will be targeted. If you don't, an event will be sent out to all springs matching the channel data info")]
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040014C3 RID: 5315
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x040014C4 RID: 5316
		[Tooltip("the command to use on that spring")]
		public SpringCommands Command = SpringCommands.Bump;

		// Token: 0x040014C5 RID: 5317
		[MMEnumCondition("Command", new int[] { 0, 1, 2, 4 })]
		[Tooltip("the new value this spring should move towards")]
		public Vector4 MoveToValue = new Vector4(2f, 2f, 2f, 2f);

		// Token: 0x040014C6 RID: 5318
		[Tooltip("the amount to add to the spring's current velocity to disturb it and make it bump")]
		[MMEnumCondition("Command", new int[] { 5 })]
		public Vector4 BumpAmount = new Vector4(75f, 75f, 75f, 75f);

		// Token: 0x040014C7 RID: 5319
		[Tooltip("the min values between which a random target x value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector4 MoveToRandomValueMin = new Vector4(-2f, -2f, -2f, -2f);

		// Token: 0x040014C8 RID: 5320
		[Tooltip("the min (x) and max (y) values between which a random target y value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector4 MoveToRandomValueMax = new Vector4(2f, 2f, 2f, 2f);

		// Token: 0x040014C9 RID: 5321
		[Tooltip("the min (x) and max (y) values between which a random bump x value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector4 BumpAmountRandomValueMin = new Vector4(-20f, -20f, -20f, -20f);

		// Token: 0x040014CA RID: 5322
		[Tooltip("the min (x) and max (y) values between which a random bump y value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector4 BumpAmountRandomValueMax = new Vector4(20f, 20f, 20f, 20f);

		// Token: 0x040014CB RID: 5323
		[Header("Overrides")]
		[Tooltip("whether or not to override the current Damping value of the target spring(s) with the one specified below (NewDamping)")]
		public bool OverrideDamping;

		// Token: 0x040014CC RID: 5324
		[Tooltip("the new damping value to apply to the target spring(s) if OverrideDamping is true")]
		[MMFCondition("OverrideDamping", true)]
		public Vector4 NewDamping = new Vector4(0.8f, 0.8f, 0.8f, 0.8f);

		// Token: 0x040014CD RID: 5325
		[Tooltip("whether or not to override the current Frequency value of the target spring(s) with the one specified below (NewFrequency)")]
		public bool OverrideFrequency;

		// Token: 0x040014CE RID: 5326
		[Tooltip("the new frequency value to apply to the target spring(s) if OverrideFrequency is true")]
		[MMFCondition("OverrideFrequency", true)]
		public Vector4 NewFrequency = new Vector4(5f, 5f, 5f, 5f);

		// Token: 0x040014CF RID: 5327
		protected MMChannelData _eventChannelData;
	}
}
