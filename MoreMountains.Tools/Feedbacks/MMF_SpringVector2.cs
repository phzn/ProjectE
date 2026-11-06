using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000270 RID: 624
	[AddComponentMenu("")]
	[FeedbackHelp("A feedback used to pilot Vector2 springs")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Springs/Spring Vector2")]
	public class MMF_SpringVector2 : MMF_Feedback
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x0004EA90 File Offset: 0x0004CC90
		// (set) Token: 0x06001202 RID: 4610 RVA: 0x0004EA9E File Offset: 0x0004CC9E
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x0004EAA7 File Offset: 0x0004CCA7
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0004EAAA File Offset: 0x0004CCAA
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0004EAB0 File Offset: 0x0004CCB0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector2.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector2Event.Trigger(this.Command, this.TargetSpring, this._eventChannelData, this.MoveToValue, this.BumpAmount, this.MoveToRandomValueMin, this.MoveToRandomValueMax, this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax, this.OverrideDamping, this.NewDamping, this.OverrideFrequency, this.NewFrequency);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004EB40 File Offset: 0x0004CD40
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringVector2.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector2Event.Trigger(SpringCommands.Stop, this.TargetSpring, this._eventChannelData, default(Vector2), default(Vector2), default(Vector2), default(Vector2), default(Vector2), default(Vector2), false, default(Vector2), false, default(Vector2));
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004EBE0 File Offset: 0x0004CDE0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpringVector2.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringVector2Event.Trigger(SpringCommands.RestoreInitialValue, this.TargetSpring, this._eventChannelData, default(Vector2), default(Vector2), default(Vector2), default(Vector2), default(Vector2), default(Vector2), false, default(Vector2), false, default(Vector2));
		}

		// Token: 0x040014A3 RID: 5283
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014A4 RID: 5284
		[MMFInspectorGroup("Spring", true, 72, false, false)]
		[Tooltip("the Vector2 spring we want to pilot using this feedback. If you set one, only that spring will be targeted. If you don't, an event will be sent out to all springs matching the channel data info")]
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040014A5 RID: 5285
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x040014A6 RID: 5286
		[Tooltip("the command to use on that spring")]
		public SpringCommands Command = SpringCommands.Bump;

		// Token: 0x040014A7 RID: 5287
		[MMEnumCondition("Command", new int[] { 0, 1, 2, 4 })]
		[Tooltip("the new value this spring should move towards")]
		public Vector2 MoveToValue = new Vector2(2f, 2f);

		// Token: 0x040014A8 RID: 5288
		[Tooltip("the amount to add to the spring's current velocity to disturb it and make it bump")]
		[MMEnumCondition("Command", new int[] { 5 })]
		public Vector2 BumpAmount = new Vector2(75f, 75f);

		// Token: 0x040014A9 RID: 5289
		[Tooltip("the min values between which a random target x value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector2 MoveToRandomValueMin = new Vector2(-2f, -2f);

		// Token: 0x040014AA RID: 5290
		[Tooltip("the min (x) and max (y) values between which a random target y value will be picked when calling MoveToRandom")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector2 MoveToRandomValueMax = new Vector2(2f, 2f);

		// Token: 0x040014AB RID: 5291
		[Tooltip("the min (x) and max (y) values between which a random bump x value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector2 BumpAmountRandomValueMin = new Vector2(-20f, -20f);

		// Token: 0x040014AC RID: 5292
		[Tooltip("the min (x) and max (y) values between which a random bump y value will be picked when calling BumpRandom")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector2 BumpAmountRandomValueMax = new Vector2(20f, 20f);

		// Token: 0x040014AD RID: 5293
		[Header("Overrides")]
		[Tooltip("whether or not to override the current Damping value of the target spring(s) with the one specified below (NewDamping)")]
		public bool OverrideDamping;

		// Token: 0x040014AE RID: 5294
		[Tooltip("the new damping value to apply to the target spring(s) if OverrideDamping is true")]
		[MMFCondition("OverrideDamping", true)]
		public Vector2 NewDamping = new Vector2(0.8f, 0.8f);

		// Token: 0x040014AF RID: 5295
		[Tooltip("whether or not to override the current Frequency value of the target spring(s) with the one specified below (NewFrequency)")]
		public bool OverrideFrequency;

		// Token: 0x040014B0 RID: 5296
		[Tooltip("the new frequency value to apply to the target spring(s) if OverrideFrequency is true")]
		[MMFCondition("OverrideFrequency", true)]
		public Vector2 NewFrequency = new Vector2(5f, 5f);

		// Token: 0x040014B1 RID: 5297
		protected MMChannelData _eventChannelData;
	}
}
