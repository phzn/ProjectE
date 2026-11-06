using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026F RID: 623
	[AddComponentMenu("")]
	[FeedbackHelp("A feedback used to pilot float springs")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Springs/Spring Float")]
	public class MMF_SpringFloat : MMF_Feedback
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0004E882 File Offset: 0x0004CA82
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x0004E890 File Offset: 0x0004CA90
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0004E899 File Offset: 0x0004CA99
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0004E89C File Offset: 0x0004CA9C
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0004E8A0 File Offset: 0x0004CAA0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringFloat.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringFloatEvent.Trigger(this.Command, this.TargetSpring, this._eventChannelData, this.MoveToValue, this.BumpAmount, this.MoveToRandomValue, this.BumpAmountRandomValue, this.OverrideDamping, this.NewDamping, this.OverrideFrequency, this.NewFrequency);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0004E924 File Offset: 0x0004CB24
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpringFloat.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringFloatEvent.Trigger(SpringCommands.Stop, this.TargetSpring, this._eventChannelData, 1f, 1f, default(Vector2), default(Vector2), false, 0.8f, false, 5f);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpringFloat.FeedbackTypeAuthorized)
			{
				return;
			}
			this._eventChannelData = ((this.TargetSpring == null) ? this.ChannelData : null);
			MMSpringFloatEvent.Trigger(SpringCommands.RestoreInitialValue, this.TargetSpring, this._eventChannelData, 1f, 1f, default(Vector2), default(Vector2), false, 0.8f, false, 5f);
		}

		// Token: 0x04001496 RID: 5270
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001497 RID: 5271
		[MMFInspectorGroup("Spring", true, 72, false, false)]
		[Tooltip("the spring we want to pilot using this feedback. If you set one, only that spring will be targeted. If you don't, an event will be sent out to all springs matching the channel data info")]
		public MMSpringComponentBase TargetSpring;

		// Token: 0x04001498 RID: 5272
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04001499 RID: 5273
		[Tooltip("the command to use on that spring")]
		public SpringCommands Command = SpringCommands.Bump;

		// Token: 0x0400149A RID: 5274
		[MMEnumCondition("Command", new int[] { 0, 1, 2, 4 })]
		[Tooltip("the new value this spring should move towards")]
		public float MoveToValue = 2f;

		// Token: 0x0400149B RID: 5275
		[Tooltip("the amount to add to the spring's current velocity to disturb it and make it bump")]
		[MMEnumCondition("Command", new int[] { 5 })]
		public float BumpAmount = 75f;

		// Token: 0x0400149C RID: 5276
		[Tooltip("a min and max values to pick a random value from to move the spring to when MoveToRandom is called")]
		[MMEnumCondition("Command", new int[] { 3 })]
		public Vector2 MoveToRandomValue = new Vector2(-2f, 2f);

		// Token: 0x0400149D RID: 5277
		[Tooltip("a min and max values to pick a random value from to add to the spring's velocity when BumpRandom is called")]
		[MMEnumCondition("Command", new int[] { 6 })]
		public Vector2 BumpAmountRandomValue = new Vector2(-50f, 50f);

		// Token: 0x0400149E RID: 5278
		[Header("Overrides")]
		[Tooltip("whether or not to override the current Damping value of the target spring(s) with the one specified below (NewDamping)")]
		public bool OverrideDamping;

		// Token: 0x0400149F RID: 5279
		[Tooltip("the new damping value to apply to the target spring(s) if OverrideDamping is true")]
		[MMFCondition("OverrideDamping", true)]
		public float NewDamping = 0.8f;

		// Token: 0x040014A0 RID: 5280
		[Tooltip("whether or not to override the current Frequency value of the target spring(s) with the one specified below (NewFrequency)")]
		public bool OverrideFrequency;

		// Token: 0x040014A1 RID: 5281
		[Tooltip("the new frequency value to apply to the target spring(s) if OverrideFrequency is true")]
		[MMFCondition("OverrideFrequency", true)]
		public float NewFrequency = 5f;

		// Token: 0x040014A2 RID: 5282
		protected MMChannelData _eventChannelData;
	}
}
