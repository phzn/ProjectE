using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000260 RID: 608
	public abstract class MMSpringVector2Component<T> : MMSpringComponentBase, MMEventListener<MMSpringVector2Event>, MMEventListenerBase where T : Component
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0004B9C3 File Offset: 0x00049BC3
		public override bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this.SpringVector2.Velocity.x) + Mathf.Abs(this.SpringVector2.Velocity.y) < this._velocityLowThreshold;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0004B9F8 File Offset: 0x00049BF8
		public float DeltaTime
		{
			get
			{
				if (this.TimeScaleMode != MMSpringComponentBase.TimeScaleModes.Scaled)
				{
					return Time.unscaledDeltaTime;
				}
				return Time.deltaTime;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0004BA0E File Offset: 0x00049C0E
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x0004BA16 File Offset: 0x00049C16
		public virtual Vector2 TargetVector2 { get; set; }

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004BA1F File Offset: 0x00049C1F
		public virtual void MoveTo(Vector2 newValue)
		{
			this.Activate();
			this.SpringVector2.MoveTo(newValue);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0004BA33 File Offset: 0x00049C33
		public virtual void MoveToAdditive(Vector2 newValue)
		{
			this.Activate();
			this.SpringVector2.MoveToAdditive(newValue);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004BA47 File Offset: 0x00049C47
		public virtual void MoveToSubtractive(Vector2 newValue)
		{
			this.Activate();
			this.SpringVector2.MoveToSubtractive(newValue);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004BA5B File Offset: 0x00049C5B
		public virtual void MoveToRandom()
		{
			this.Activate();
			this.SpringVector2.MoveToRandom(this.MoveToRandomValueMin, this.MoveToRandomValueMax);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0004BA7A File Offset: 0x00049C7A
		public virtual void MoveToInstant(Vector2 newValue)
		{
			this.Activate();
			this.SpringVector2.MoveToInstant(newValue);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0004BA8E File Offset: 0x00049C8E
		public virtual void MoveToRandom(Vector2 min, Vector2 max)
		{
			this.Activate();
			this.SpringVector2.MoveToRandom(min, max);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0004BAA3 File Offset: 0x00049CA3
		public virtual void Bump(Vector2 bumpAmount)
		{
			this.Activate();
			this.SpringVector2.Bump(bumpAmount);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0004BAB7 File Offset: 0x00049CB7
		public virtual void BumpRandom()
		{
			this.Activate();
			this.SpringVector2.BumpRandom(this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004BAD6 File Offset: 0x00049CD6
		public virtual void BumpRandom(Vector2 min, Vector2 max)
		{
			this.Activate();
			this.SpringVector2.BumpRandom(min, max);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0004BAEB File Offset: 0x00049CEB
		public override void Stop()
		{
			base.Stop();
			base.enabled = false;
			this.GrabCurrentValue();
			this.SpringVector2.Stop();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0004BB0B File Offset: 0x00049D0B
		public override void RestoreInitialValue()
		{
			this.SpringVector2.RestoreInitialValue();
			this.ApplyValue(this.SpringVector2.CurrentValue);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0004BB29 File Offset: 0x00049D29
		public override void ResetInitialValue()
		{
			this.SpringVector2.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004BB36 File Offset: 0x00049D36
		protected override void UpdateSpringValue()
		{
			this.SpringVector2.UpdateSpringValue(this.DeltaTime);
			this.ApplyValue(this.SpringVector2.CurrentValue);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004BB5A File Offset: 0x00049D5A
		public override void Finish()
		{
			this.SpringVector2.Finish();
			this.ApplyValue(this.SpringVector2.CurrentValue);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0004BB78 File Offset: 0x00049D78
		protected override void Initialization()
		{
			base.Initialization();
			this.GrabCurrentValue();
			this.SpringVector2.SetInitialValue(this.SpringVector2.CurrentValue);
			this.SpringVector2.TargetValue = this.SpringVector2.CurrentValue;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0004BBB2 File Offset: 0x00049DB2
		protected virtual void ApplyValue(Vector2 newValue)
		{
			this.TargetVector2 = newValue;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0004BBBB File Offset: 0x00049DBB
		protected override void GrabCurrentValue()
		{
			base.GrabCurrentValue();
			this.SpringVector2.CurrentValue = this.TargetVector2;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0004BBD4 File Offset: 0x00049DD4
		public void OnMMEvent(MMSpringVector2Event springEvent)
		{
			bool flag = springEvent.ChannelData != null && MMChannel.Match(springEvent.ChannelData, this.ChannelMode, this.Channel, this.MMChannelDefinition);
			bool flag2 = springEvent.TargetSpring != null && springEvent.TargetSpring.Equals(this);
			if (!flag && !flag2)
			{
				return;
			}
			if (springEvent.OverrideDamping)
			{
				this.SpringVector2.SetDamping(springEvent.NewDamping);
			}
			if (springEvent.OverrideFrequency)
			{
				this.SpringVector2.SetFrequency(springEvent.NewFrequency);
			}
			switch (springEvent.Command)
			{
			case SpringCommands.MoveTo:
				this.MoveTo(springEvent.MoveToValue);
				return;
			case SpringCommands.MoveToAdditive:
				this.MoveToAdditive(springEvent.MoveToValue);
				return;
			case SpringCommands.MoveToSubtractive:
				this.MoveToSubtractive(springEvent.MoveToValue);
				return;
			case SpringCommands.MoveToRandom:
				this.MoveToRandom(springEvent.MoveToRandomValueMin, springEvent.MoveToRandomValueMax);
				return;
			case SpringCommands.MoveToInstant:
				this.MoveToInstant(springEvent.MoveToValue);
				return;
			case SpringCommands.Bump:
				this.Bump(springEvent.BumpAmount);
				return;
			case SpringCommands.BumpRandom:
				this.BumpRandom(springEvent.BumpAmountRandomValueMin, springEvent.BumpAmountRandomValueMax);
				return;
			case SpringCommands.Stop:
				this.Stop();
				return;
			case SpringCommands.Finish:
				this.Finish();
				return;
			case SpringCommands.RestoreInitialValue:
				this.RestoreInitialValue();
				return;
			case SpringCommands.ResetInitialValue:
				this.ResetInitialValue();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0004BD1B File Offset: 0x00049F1B
		protected override void Awake()
		{
			if (this.Target == null)
			{
				this.Target = base.GetComponent<T>();
			}
			base.Awake();
			this.MMEventStartListening<MMSpringVector2Event>();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0004BD48 File Offset: 0x00049F48
		protected void OnDestroy()
		{
			this.MMEventStopListening<MMSpringVector2Event>();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0004BD50 File Offset: 0x00049F50
		protected override void TestMoveTo()
		{
			this.MoveTo(this.TestMoveToValue);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0004BD5E File Offset: 0x00049F5E
		protected override void TestMoveToAdditive()
		{
			this.MoveToAdditive(this.TestMoveToValue);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0004BD6C File Offset: 0x00049F6C
		protected override void TestMoveToSubtractive()
		{
			this.MoveToSubtractive(this.TestMoveToValue);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0004BD7A File Offset: 0x00049F7A
		protected override void TestMoveToRandom()
		{
			this.MoveToRandom();
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0004BD82 File Offset: 0x00049F82
		protected override void TestMoveToInstant()
		{
			this.MoveToInstant(this.TestMoveToValue);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0004BD90 File Offset: 0x00049F90
		protected override void TestBump()
		{
			this.Bump(this.TestBumpAmount);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0004BD9E File Offset: 0x00049F9E
		protected override void TestBumpRandom()
		{
			this.BumpRandom();
		}

		// Token: 0x04001402 RID: 5122
		[MMInspectorGroup("Target", true, 17, false)]
		public T Target;

		// Token: 0x04001403 RID: 5123
		[MMInspectorGroup("Channel & TimeScale", true, 16, true)]
		[Tooltip("whether this spring should run on scaled time (and be impacted by time scale changes) or unscaled time (and not be impacted by time scale changes)")]
		public MMSpringComponentBase.TimeScaleModes TimeScaleMode = MMSpringComponentBase.TimeScaleModes.Scaled;

		// Token: 0x04001404 RID: 5124
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001405 RID: 5125
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001406 RID: 5126
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001407 RID: 5127
		[MMInspectorGroup("Spring Settings", true, 18, false)]
		[Header("SpringVector2")]
		public MMSpringVector2 SpringVector2;

		// Token: 0x04001408 RID: 5128
		[MMInspectorGroup("Randomness", true, 12, true)]
		[Header("Move To Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector2 MoveToRandomValueMin = new Vector2(-2f, -2f);

		// Token: 0x04001409 RID: 5129
		[Tooltip("the maximum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector2 MoveToRandomValueMax = new Vector2(2f, 2f);

		// Token: 0x0400140A RID: 5130
		[Header("Bump Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling BumpRandom()")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 BumpAmountRandomValueMin = new Vector2(-20f, -20f);

		// Token: 0x0400140B RID: 5131
		[Tooltip("the maximum vector from which to pick a random value when calling BumpRandom()")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 BumpAmountRandomValueMax = new Vector2(20f, 20f);

		// Token: 0x0400140C RID: 5132
		[MMInspectorGroup("Test", true, 20, true)]
		[Tooltip("the value to move this spring to when interacting with any of the MoveTo debug buttons in its inspector")]
		public Vector2 TestMoveToValue = new Vector2(2f, 2f);

		// Token: 0x0400140D RID: 5133
		[MMInspectorButtonBar(new string[] { "MoveTo", "MoveToAdditive", "MoveToSubtractive", "MoveToRandom", "MoveToInstant" }, new string[] { "TestMoveTo", "TestMoveToAdditive", "TestMoveToSubtractive", "TestMoveToRandom", "TestMoveToInstant" }, new bool[] { true, true, true, true, true }, new string[] { "main-call-to-action", "", "", "", "" })]
		public bool MoveToToolbar;

		// Token: 0x0400140E RID: 5134
		[Tooltip("the amount by which to bump this spring when interacting with the Bump debug button in its inspector")]
		public Vector2 TestBumpAmount = new Vector2(75f, 100f);

		// Token: 0x0400140F RID: 5135
		[MMInspectorButtonBar(new string[] { "Bump", "BumpRandom" }, new string[] { "TestBump", "TestBumpRandom" }, new bool[] { true, true }, new string[] { "main-call-to-action", "" })]
		public bool BumpToToolbar;

		// Token: 0x04001410 RID: 5136
		[MMInspectorButtonBar(new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new bool[] { true, true, true, true }, new string[] { "", "", "", "" })]
		public bool OtherControlsToToolbar;
	}
}
