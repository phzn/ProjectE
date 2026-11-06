using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000271 RID: 625
	[AddComponentMenu("")]
	[FeedbackHelp("A feedback used to pilot Vector3 springs")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Springs/Spring Vector3")]
	public class MMF_SpringVector3 : MMF_Feedback
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0004ED42 File Offset: 0x0004CF42
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x0004ED50 File Offset: 0x0004CF50
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0004ED59 File Offset: 0x0004CF59
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0004ED5C File Offset: 0x0004CF5C
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0004ED60 File Offset: 0x0004CF60
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector3.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector3Event.Trigger(this.Command, this.TargetSpring, this._eventChannelData, this.MoveToValue, this.BumpAmount, this.MoveToRandomValueMin, this.MoveToRandomValueMax, this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax, this.OverrideDamping, this.NewDamping, this.OverrideFrequency, this.NewFrequency);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0004EDF0 File Offset: 0x0004CFF0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector3.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector3Event.Trigger(SpringCommands.Stop, this.TargetSpring, this._eventChannelData, default(Vector3), default(Vector3), default(Vector3), default(Vector3), default(Vector3), default(Vector3), false, default(Vector3), false, default(Vector3));
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0004EE90 File Offset: 0x0004D090
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpringVector3.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector3Event.Trigger(SpringCommands.RestoreInitialValue, this.TargetSpring, this._eventChannelData, default(Vector3), default(Vector3), default(Vector3), default(Vector3), default(Vector3), default(Vector3), false, default(Vector3), false, default(Vector3));
		}

		// Token: 0x040014B2 RID: 5298
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014B3 RID: 5299
		[MMFInspectorGroup("Spring", true, 72, false, false)]
		[Tooltip("the Vector3 spring we want to pilot using this feedback. If you set one, only that spring will be targeted. If you don't, an event will be sent out to all springs matching the channel data info")]
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040014B4 RID: 5300
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x040014B5 RID: 5301
		[Tooltip("the command to use on that spring")]
		public SpringCommands Command = SpringCommands.Bump;

		// Token: 0x040014B6 RID: 5302
		[MMEnumCondition("Command", new int[] { 0, 1, 2, 4 })]
		[Tooltip("the new value this spring should move towards")]
		public Vector3 MoveToValue = new Vector3(2f, 2f, 2f);

		// Token: 0x040014B7 RID: 5303
		[Tooltip("the amount to add to the spring's current velocity to disturb it and make it bump")]
		[MMEnumCondition("Command", new int[] { 5 })]
		public Vector3 BumpAmount = new Vector3(75f, 75f, 75f);

		// Token: 0x040014B8 RID: 5304
		[Tooltip("the min values between which a random target x value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector3 MoveToRandomValueMin = new Vector3(-2f, -2f, -2f);

		// Token: 0x040014B9 RID: 5305
		[Tooltip("the min (x) and max (y) values between which a random target y value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector3 MoveToRandomValueMax = new Vector3(2f, 2f, 2f);

		// Token: 0x040014BA RID: 5306
		[Tooltip("the min (x) and max (y) values between which a random bump x value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector3 BumpAmountRandomValueMin = new Vector3(-20f, -20f, -20f);

		// Token: 0x040014BB RID: 5307
		[Tooltip("the min (x) and max (y) values between which a random bump y value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector3 BumpAmountRandomValueMax = new Vector3(20f, 20f, 20f);

		// Token: 0x040014BC RID: 5308
		[Header("Overrides")]
		[Tooltip("whether or not to override the current Damping value of the target spring(s) with the one specified below (NewDamping)")]
		public bool OverrideDamping;

		// Token: 0x040014BD RID: 5309
		[Tooltip("the new damping value to apply to the target spring(s) if OverrideDamping is true")]
		[MMFCondition("OverrideDamping", true)]
		public Vector3 NewDamping = new Vector3(0.8f, 0.8f, 0.8f);

		// Token: 0x040014BE RID: 5310
		[Tooltip("whether or not to override the current Frequency value of the target spring(s) with the one specified below (NewFrequency)")]
		public bool OverrideFrequency;

		// Token: 0x040014BF RID: 5311
		[Tooltip("the new frequency value to apply to the target spring(s) if OverrideFrequency is true")]
		[MMFCondition("OverrideFrequency", true)]
		public Vector3 NewFrequency = new Vector3(5f, 5f, 5f);

		// Token: 0x040014C0 RID: 5312
		protected MMChannelData _eventChannelData;
	}
}
