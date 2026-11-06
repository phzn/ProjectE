using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000262 RID: 610
	public abstract class MMSpringVector3Component<T> : MMSpringComponentBase, MMEventListener<MMSpringVector3Event>, MMEventListenerBase where T : Component
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x0004BEF0 File Offset: 0x0004A0F0
		public override bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this.SpringVector3.Velocity.x) + Mathf.Abs(this.SpringVector3.Velocity.y) + Mathf.Abs(this.SpringVector3.Velocity.z) < this._velocityLowThreshold;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0004BF46 File Offset: 0x0004A146
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x0004BF5C File Offset: 0x0004A15C
		// (set) Token: 0x06001113 RID: 4371 RVA: 0x0004BF64 File Offset: 0x0004A164
		public virtual Vector3 TargetVector3 { get; set; }

		// Token: 0x06001114 RID: 4372 RVA: 0x0004BF6D File Offset: 0x0004A16D
		public virtual void MoveTo(Vector3 newValue)
		{
			this.Activate();
			this.SpringVector3.MoveTo(newValue);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0004BF81 File Offset: 0x0004A181
		public virtual void MoveToAdditive(Vector3 newValue)
		{
			this.Activate();
			this.SpringVector3.MoveToAdditive(newValue);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0004BF95 File Offset: 0x0004A195
		public virtual void MoveToSubtractive(Vector3 newValue)
		{
			this.Activate();
			this.SpringVector3.MoveToSubtractive(newValue);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0004BFA9 File Offset: 0x0004A1A9
		public virtual void MoveToRandom()
		{
			this.Activate();
			this.SpringVector3.MoveToRandom(this.MoveToRandomValueMin, this.MoveToRandomValueMax);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0004BFC8 File Offset: 0x0004A1C8
		public virtual void MoveToInstant(Vector3 newValue)
		{
			this.Activate();
			this.SpringVector3.MoveToInstant(newValue);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0004BFDC File Offset: 0x0004A1DC
		public virtual void MoveToRandom(Vector3 min, Vector3 max)
		{
			this.Activate();
			this.SpringVector3.MoveToRandom(min, max);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004BFF1 File Offset: 0x0004A1F1
		public virtual void Bump(Vector3 bumpAmount)
		{
			this.Activate();
			this.SpringVector3.Bump(bumpAmount);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0004C005 File Offset: 0x0004A205
		public virtual void BumpRandom()
		{
			this.Activate();
			this.SpringVector3.BumpRandom(this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0004C024 File Offset: 0x0004A224
		public virtual void BumpRandom(Vector3 min, Vector3 max)
		{
			this.Activate();
			this.SpringVector3.BumpRandom(min, max);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0004C039 File Offset: 0x0004A239
		public override void Stop()
		{
			base.Stop();
			base.enabled = false;
			this.GrabCurrentValue();
			this.SpringVector3.Stop();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0004C059 File Offset: 0x0004A259
		public override void RestoreInitialValue()
		{
			this.SpringVector3.RestoreInitialValue();
			this.ApplyValue(this.SpringVector3.CurrentValue);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0004C077 File Offset: 0x0004A277
		public override void ResetInitialValue()
		{
			this.SpringVector3.SetCurrentValueAsInitialValue();
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0004C084 File Offset: 0x0004A284
		protected override void UpdateSpringValue()
		{
			this.SpringVector3.UpdateSpringValue(this.DeltaTime);
			this.ApplyValue(this.SpringVector3.CurrentValue);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0004C0A8 File Offset: 0x0004A2A8
		public override void Finish()
		{
			this.SpringVector3.Finish();
			this.ApplyValue(this.SpringVector3.CurrentValue);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0004C0C6 File Offset: 0x0004A2C6
		protected override void Initialization()
		{
			base.Initialization();
			this.GrabCurrentValue();
			this.SpringVector3.SetInitialValue(this.SpringVector3.CurrentValue);
			this.SpringVector3.TargetValue = this.SpringVector3.CurrentValue;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004C100 File Offset: 0x0004A300
		protected virtual void ApplyValue(Vector3 newValue)
		{
			this.TargetVector3 = newValue;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0004C109 File Offset: 0x0004A309
		protected override void GrabCurrentValue()
		{
			base.GrabCurrentValue();
			this.SpringVector3.CurrentValue = this.TargetVector3;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0004C124 File Offset: 0x0004A324
		public void OnMMEvent(MMSpringVector3Event springEvent)
		{
			bool flag = springEvent.ChannelData != null && MMChannel.Match(springEvent.ChannelData, this.ChannelMode, this.Channel, this.MMChannelDefinition);
			bool flag2 = springEvent.TargetSpring != null && springEvent.TargetSpring.Equals(this);
			if (!flag && !flag2)
			{
				return;
			}
			if (springEvent.OverrideDamping)
			{
				this.SpringVector3.SetDamping(springEvent.NewDamping);
			}
			if (springEvent.OverrideFrequency)
			{
				this.SpringVector3.SetFrequency(springEvent.NewFrequency);
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

		// Token: 0x06001126 RID: 4390 RVA: 0x0004C26B File Offset: 0x0004A46B
		protected override void Awake()
		{
			if (this.Target == null)
			{
				this.Target = base.GetComponent<T>();
			}
			base.Awake();
			this.MMEventStartListening<MMSpringVector3Event>();
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0004C298 File Offset: 0x0004A498
		protected void OnDestroy()
		{
			this.MMEventStopListening<MMSpringVector3Event>();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0004C2A0 File Offset: 0x0004A4A0
		protected override void TestMoveTo()
		{
			this.MoveTo(this.TestMoveToValue);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0004C2AE File Offset: 0x0004A4AE
		protected override void TestMoveToAdditive()
		{
			this.MoveToAdditive(this.TestMoveToValue);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0004C2BC File Offset: 0x0004A4BC
		protected override void TestMoveToSubtractive()
		{
			this.MoveToSubtractive(this.TestMoveToValue);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004C2CA File Offset: 0x0004A4CA
		protected override void TestMoveToRandom()
		{
			this.MoveToRandom();
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004C2D2 File Offset: 0x0004A4D2
		protected override void TestMoveToInstant()
		{
			this.MoveToInstant(this.TestMoveToValue);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
		protected override void TestBump()
		{
			this.Bump(this.TestBumpAmount);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0004C2EE File Offset: 0x0004A4EE
		protected override void TestBumpRandom()
		{
			this.BumpRandom();
		}

		// Token: 0x04001420 RID: 5152
		[MMInspectorGroup("Target", true, 17, false)]
		public T Target;

		// Token: 0x04001421 RID: 5153
		[MMInspectorGroup("Channel & TimeScale", true, 16, true)]
		[Tooltip("whether this spring should run on scaled time (and be impacted by time scale changes) or unscaled time (and not be impacted by time scale changes)")]
		public MMSpringComponentBase.TimeScaleModes TimeScaleMode = MMSpringComponentBase.TimeScaleModes.Scaled;

		// Token: 0x04001422 RID: 5154
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001423 RID: 5155
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001424 RID: 5156
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001425 RID: 5157
		[MMInspectorGroup("Spring Settings", true, 18, false)]
		[Header("SpringVector3")]
		public MMSpringVector3 SpringVector3;

		// Token: 0x04001426 RID: 5158
		[MMInspectorGroup("Randomness", true, 12, true)]
		[Header("Move To Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector3 MoveToRandomValueMin = new Vector3(-2f, -2f, -2f);

		// Token: 0x04001427 RID: 5159
		[Tooltip("the maximum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector3 MoveToRandomValueMax = new Vector3(2f, 2f, 2f);

		// Token: 0x04001428 RID: 5160
		[Header("Bump Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling BumpRandom()")]
		public Vector3 BumpAmountRandomValueMin = new Vector3(-20f, 20f, -20f);

		// Token: 0x04001429 RID: 5161
		[Tooltip("the maximum vector from which to pick a random value when calling BumpRandom()")]
		public Vector3 BumpAmountRandomValueMax = new Vector3(20f, 20f, 20f);

		// Token: 0x0400142A RID: 5162
		[MMInspectorGroup("Test", true, 20, true)]
		[Tooltip("the value to move this spring to when interacting with any of the MoveTo debug buttons in its inspector")]
		public Vector3 TestMoveToValue = new Vector3(2f, 2f, 2f);

		// Token: 0x0400142B RID: 5163
		[MMInspectorButtonBar(new string[] { "MoveTo", "MoveToAdditive", "MoveToSubtractive", "MoveToRandom", "MoveToInstant" }, new string[] { "TestMoveTo", "TestMoveToAdditive", "TestMoveToSubtractive", "TestMoveToRandom", "TestMoveToInstant" }, new bool[] { true, true, true, true, true }, new string[] { "main-call-to-action", "", "", "", "" })]
		public bool MoveToToolbar;

		// Token: 0x0400142C RID: 5164
		[Tooltip("the amount by which to bump this spring when interacting with the Bump debug button in its inspector")]
		public Vector3 TestBumpAmount = new Vector3(75f, 100f, 50f);

		// Token: 0x0400142D RID: 5165
		[MMInspectorButtonBar(new string[] { "Bump", "BumpRandom" }, new string[] { "TestBump", "TestBumpRandom" }, new bool[] { true, true }, new string[] { "main-call-to-action", "" })]
		public bool BumpToToolbar;

		// Token: 0x0400142E RID: 5166
		[MMInspectorButtonBar(new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new bool[] { true, true, true, true }, new string[] { "", "", "", "" })]
		public bool OtherControlsToToolbar;
	}
}
