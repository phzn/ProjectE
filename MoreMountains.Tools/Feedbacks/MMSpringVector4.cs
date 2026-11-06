using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000264 RID: 612
	public abstract class MMSpringVector4<T> : MMSpringComponentBase, MMEventListener<MMSpringVector4Event>, MMEventListenerBase where T : Component
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0004C460 File Offset: 0x0004A660
		public override bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this.SpringVector4.SpringX.Velocity) + Mathf.Abs(this.SpringVector4.SpringY.Velocity) + Mathf.Abs(this.SpringVector4.SpringZ.Velocity) + Mathf.Abs(this.SpringVector4.SpringW.Velocity) < this._velocityLowThreshold;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x0004C4CC File Offset: 0x0004A6CC
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

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x0004C4E2 File Offset: 0x0004A6E2
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x0004C4EA File Offset: 0x0004A6EA
		public virtual Vector4 TargetVector4 { get; set; }

		// Token: 0x06001135 RID: 4405 RVA: 0x0004C4F3 File Offset: 0x0004A6F3
		public virtual void MoveTo(Vector4 newValue)
		{
			this.Activate();
			this.SpringVector4.MoveTo(newValue);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0004C507 File Offset: 0x0004A707
		public virtual void MoveToAdditive(Vector4 newValue)
		{
			this.Activate();
			this.SpringVector4.MoveToAdditive(newValue);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0004C51B File Offset: 0x0004A71B
		public virtual void MoveToSubtractive(Vector4 newValue)
		{
			this.Activate();
			this.SpringVector4.MoveToSubtractive(newValue);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0004C52F File Offset: 0x0004A72F
		public virtual void MoveToRandom()
		{
			this.Activate();
			this.SpringVector4.MoveToRandom(this.MoveToRandomValueMin, this.MoveToRandomValueMax);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0004C54E File Offset: 0x0004A74E
		public virtual void MoveToInstant(Vector4 newValue)
		{
			this.Activate();
			this.SpringVector4.MoveToInstant(newValue);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0004C562 File Offset: 0x0004A762
		public virtual void MoveToRandom(Vector4 min, Vector4 max)
		{
			this.Activate();
			this.SpringVector4.MoveToRandom(min, max);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0004C577 File Offset: 0x0004A777
		public virtual void Bump(Vector4 bumpAmount)
		{
			this.Activate();
			this.SpringVector4.Bump(bumpAmount);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0004C58B File Offset: 0x0004A78B
		public virtual void BumpRandom()
		{
			this.Activate();
			this.SpringVector4.BumpRandom(this.BumpAmountRandomValueMin, this.BumpAmountRandomValueMax);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0004C5B4 File Offset: 0x0004A7B4
		public virtual void BumpRandom(Vector4 min, Vector4 max)
		{
			this.Activate();
			this.SpringVector4.BumpRandom(min, max);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0004C5C9 File Offset: 0x0004A7C9
		public override void Stop()
		{
			base.Stop();
			base.enabled = false;
			this.GrabCurrentValue();
			this.SpringVector4.Stop();
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0004C5E9 File Offset: 0x0004A7E9
		public override void RestoreInitialValue()
		{
			this.SpringVector4.RestoreInitialValue();
			this.ApplyValue(this.SpringVector4.CurrentValue);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0004C607 File Offset: 0x0004A807
		public override void ResetInitialValue()
		{
			this.SpringVector4.RestoreInitialValue();
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0004C614 File Offset: 0x0004A814
		protected override void UpdateSpringValue()
		{
			this.SpringVector4.UpdateSpringValue(this.DeltaTime);
			this.ApplyValue(this.SpringVector4.CurrentValue);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0004C638 File Offset: 0x0004A838
		public override void Finish()
		{
			this.SpringVector4.Finish();
			this.ApplyValue(this.SpringVector4.CurrentValue);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0004C656 File Offset: 0x0004A856
		protected override void Initialization()
		{
			base.Initialization();
			this.GrabCurrentValue();
			this.SpringVector4.SetInitialValue(this.SpringVector4.CurrentValue);
			this.SpringVector4.TargetValue = this.SpringVector4.CurrentValue;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0004C690 File Offset: 0x0004A890
		protected virtual void ApplyValue(Vector4 newValue)
		{
			this.TargetVector4 = newValue;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0004C699 File Offset: 0x0004A899
		protected override void GrabCurrentValue()
		{
			base.GrabCurrentValue();
			this.SpringVector4.CurrentValue = this.TargetVector4;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0004C6B4 File Offset: 0x0004A8B4
		public void OnMMEvent(MMSpringVector4Event springEvent)
		{
			bool flag = springEvent.ChannelData != null && MMChannel.Match(springEvent.ChannelData, this.ChannelMode, this.Channel, this.MMChannelDefinition);
			bool flag2 = springEvent.TargetSpring != null && springEvent.TargetSpring.Equals(this);
			if (!flag && !flag2)
			{
				return;
			}
			if (springEvent.OverrideDamping)
			{
				this.SpringVector4.SetDamping(springEvent.NewDamping);
			}
			if (springEvent.OverrideFrequency)
			{
				this.SpringVector4.SetFrequency(springEvent.NewFrequency);
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

		// Token: 0x06001147 RID: 4423 RVA: 0x0004C7FB File Offset: 0x0004A9FB
		protected override void Awake()
		{
			if (this.Target == null)
			{
				this.Target = base.GetComponent<T>();
			}
			base.Awake();
			this.MMEventStartListening<MMSpringVector4Event>();
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0004C828 File Offset: 0x0004AA28
		protected void OnDestroy()
		{
			this.MMEventStopListening<MMSpringVector4Event>();
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0004C830 File Offset: 0x0004AA30
		protected override void TestMoveTo()
		{
			this.MoveTo(this.TestMoveToValue);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0004C83E File Offset: 0x0004AA3E
		protected override void TestMoveToAdditive()
		{
			this.MoveToAdditive(this.TestMoveToValue);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0004C84C File Offset: 0x0004AA4C
		protected override void TestMoveToSubtractive()
		{
			this.MoveToSubtractive(this.TestMoveToValue);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0004C85A File Offset: 0x0004AA5A
		protected override void TestMoveToRandom()
		{
			this.MoveToRandom();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0004C862 File Offset: 0x0004AA62
		protected override void TestMoveToInstant()
		{
			this.MoveToInstant(this.TestMoveToValue);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0004C870 File Offset: 0x0004AA70
		protected override void TestBump()
		{
			this.Bump(this.TestBumpAmount);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0004C87E File Offset: 0x0004AA7E
		protected override void TestBumpRandom()
		{
			this.BumpRandom();
		}

		// Token: 0x0400143E RID: 5182
		[MMInspectorGroup("Target", true, 17, false)]
		public T Target;

		// Token: 0x0400143F RID: 5183
		[MMInspectorGroup("Channel & TimeScale", true, 16, true)]
		[Tooltip("whether this spring should run on scaled time (and be impacted by time scale changes) or unscaled time (and not be impacted by time scale changes)")]
		public MMSpringComponentBase.TimeScaleModes TimeScaleMode = MMSpringComponentBase.TimeScaleModes.Scaled;

		// Token: 0x04001440 RID: 5184
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001441 RID: 5185
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001442 RID: 5186
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001443 RID: 5187
		[MMInspectorGroup("Spring Settings", true, 18, false)]
		[Header("SpringVector4")]
		public MMSpringVector4 SpringVector4;

		// Token: 0x04001444 RID: 5188
		[MMInspectorGroup("Randomness", true, 12, true)]
		[Header("Move To Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector4 MoveToRandomValueMin = new Vector4(-2f, -2f, -2f, -2f);

		// Token: 0x04001445 RID: 5189
		[Tooltip("the maximum vector from which to pick a random value when calling MoveToRandom()")]
		public Vector4 MoveToRandomValueMax = new Vector4(2f, 2f, 2f, 2f);

		// Token: 0x04001446 RID: 5190
		[Header("Bump Random")]
		[Tooltip("the minimum vector from which to pick a random value when calling BumpRandom()")]
		public Vector2 BumpAmountRandomValueMin = new Vector4(-20f, -20f, -20f, -20f);

		// Token: 0x04001447 RID: 5191
		[Tooltip("the maximum vector from which to pick a random value when calling BumpRandom()")]
		public Vector2 BumpAmountRandomValueMax = new Vector4(20f, 20f, 20f, 20f);

		// Token: 0x04001448 RID: 5192
		[MMInspectorGroup("Test", true, 20, true)]
		[Tooltip("the value to move this spring to when interacting with any of the MoveTo debug buttons in its inspector")]
		public Vector4 TestMoveToValue = new Vector4(2f, 2f, 2f, 2f);

		// Token: 0x04001449 RID: 5193
		[MMInspectorButtonBar(new string[] { "MoveTo", "MoveToAdditive", "MoveToSubtractive", "MoveToRandom", "MoveToInstant" }, new string[] { "TestMoveTo", "TestMoveToAdditive", "TestMoveToSubtractive", "TestMoveToRandom", "TestMoveToInstant" }, new bool[] { true, true, true, true, true }, new string[] { "main-call-to-action", "", "", "", "" })]
		public bool MoveToToolbar;

		// Token: 0x0400144A RID: 5194
		[Tooltip("the amount by which to bump this spring when interacting with the Bump debug button in its inspector")]
		public Vector4 TestBumpAmount = new Vector4(75f, 100f, 50f, 25f);

		// Token: 0x0400144B RID: 5195
		[MMInspectorButtonBar(new string[] { "Bump", "BumpRandom" }, new string[] { "TestBump", "TestBumpRandom" }, new bool[] { true, true }, new string[] { "main-call-to-action", "" })]
		public bool BumpToToolbar;

		// Token: 0x0400144C RID: 5196
		[MMInspectorButtonBar(new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new bool[] { true, true, true, true }, new string[] { "", "", "", "" })]
		public bool OtherControlsToToolbar;
	}
}
