using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200025E RID: 606
	public abstract class MMSpringFloatComponent<T> : MMSpringComponentBase, MMEventListener<MMSpringFloatEvent>, MMEventListenerBase where T : Component
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0004B4C3 File Offset: 0x000496C3
		public override bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this.FloatSpring.Velocity) < this._velocityLowThreshold;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0004B4DD File Offset: 0x000496DD
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0004B4F3 File Offset: 0x000496F3
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x0004B4FB File Offset: 0x000496FB
		public virtual float TargetFloat { get; set; }

		// Token: 0x060010D2 RID: 4306 RVA: 0x0004B504 File Offset: 0x00049704
		public virtual void MoveTo(float newValue)
		{
			this.Activate();
			this.FloatSpring.MoveTo(newValue);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0004B518 File Offset: 0x00049718
		public virtual void MoveToAdditive(float newValue)
		{
			this.Activate();
			this.FloatSpring.MoveToAdditive(newValue);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0004B52C File Offset: 0x0004972C
		public virtual void MoveToSubtractive(float newValue)
		{
			this.Activate();
			this.FloatSpring.MoveToSubtractive(newValue);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0004B540 File Offset: 0x00049740
		public virtual void MoveToRandom()
		{
			this.Activate();
			this.FloatSpring.MoveToRandom(this.MoveToRandomValue.x, this.MoveToRandomValue.y);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0004B569 File Offset: 0x00049769
		public virtual void MoveToInstant(float newValue)
		{
			this.Activate();
			this.FloatSpring.MoveToInstant(newValue);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0004B57D File Offset: 0x0004977D
		public virtual void MoveToRandom(float min, float max)
		{
			this.Activate();
			this.FloatSpring.MoveToRandom(min, max);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0004B592 File Offset: 0x00049792
		public virtual void Bump(float bumpAmount)
		{
			this.Activate();
			this.FloatSpring.Bump(bumpAmount);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0004B5A6 File Offset: 0x000497A6
		public virtual void BumpRandom()
		{
			this.Activate();
			this.FloatSpring.BumpRandom(this.BumpAmountRandomValue.x, this.BumpAmountRandomValue.y);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0004B5CF File Offset: 0x000497CF
		public virtual void BumpRandom(float min, float max)
		{
			this.Activate();
			this.FloatSpring.BumpRandom(min, max);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0004B5E4 File Offset: 0x000497E4
		public override void Stop()
		{
			base.Stop();
			base.enabled = false;
			this.GrabCurrentValue();
			this.FloatSpring.Stop();
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0004B604 File Offset: 0x00049804
		public override void RestoreInitialValue()
		{
			this.FloatSpring.RestoreInitialValue();
			this.ApplyValue(this.FloatSpring.CurrentValue);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0004B622 File Offset: 0x00049822
		public override void ResetInitialValue()
		{
			this.FloatSpring.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0004B62F File Offset: 0x0004982F
		protected override void UpdateSpringValue()
		{
			this.FloatSpring.UpdateSpringValue(this.DeltaTime);
			this.ApplyValue(this.FloatSpring.CurrentValue);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0004B653 File Offset: 0x00049853
		public override void Finish()
		{
			this.FloatSpring.Finish();
			this.ApplyValue(this.FloatSpring.CurrentValue);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0004B671 File Offset: 0x00049871
		protected override void Initialization()
		{
			base.Initialization();
			this.GrabCurrentValue();
			this.FloatSpring.SetInitialValue(this.FloatSpring.CurrentValue);
			this.FloatSpring.TargetValue = this.FloatSpring.CurrentValue;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0004B6AB File Offset: 0x000498AB
		protected virtual void ApplyValue(float newValue)
		{
			this.TargetFloat = newValue;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0004B6B4 File Offset: 0x000498B4
		protected override void GrabCurrentValue()
		{
			base.GrabCurrentValue();
			this.FloatSpring.CurrentValue = this.TargetFloat;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004B6D0 File Offset: 0x000498D0
		public void OnMMEvent(MMSpringFloatEvent springEvent)
		{
			bool flag = springEvent.ChannelData != null && MMChannel.Match(springEvent.ChannelData, this.ChannelMode, this.Channel, this.MMChannelDefinition);
			bool flag2 = springEvent.TargetSpring != null && springEvent.TargetSpring.Equals(this);
			if (!flag && !flag2)
			{
				return;
			}
			if (springEvent.OverrideDamping)
			{
				this.FloatSpring.Damping = springEvent.NewDamping;
			}
			if (springEvent.OverrideFrequency)
			{
				this.FloatSpring.Frequency = springEvent.NewFrequency;
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
				this.MoveToRandom(springEvent.MoveToRandomValue.x, springEvent.MoveToRandomValue.y);
				return;
			case SpringCommands.MoveToInstant:
				this.MoveToInstant(springEvent.MoveToValue);
				return;
			case SpringCommands.Bump:
				this.Bump(springEvent.BumpAmount);
				return;
			case SpringCommands.BumpRandom:
				this.BumpRandom(springEvent.BumpAmountRandomValue.x, springEvent.BumpAmountRandomValue.y);
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

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004B82B File Offset: 0x00049A2B
		protected override void Awake()
		{
			if (this.Target == null)
			{
				this.Target = base.GetComponent<T>();
			}
			base.Awake();
			this.MMEventStartListening<MMSpringFloatEvent>();
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0004B858 File Offset: 0x00049A58
		protected void OnDestroy()
		{
			this.MMEventStopListening<MMSpringFloatEvent>();
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0004B860 File Offset: 0x00049A60
		protected override void TestMoveTo()
		{
			this.MoveTo(this.TestMoveToValue);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0004B86E File Offset: 0x00049A6E
		protected override void TestMoveToAdditive()
		{
			this.MoveToAdditive(this.TestMoveToValue);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0004B87C File Offset: 0x00049A7C
		protected override void TestMoveToSubtractive()
		{
			this.MoveToSubtractive(this.TestMoveToValue);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0004B88A File Offset: 0x00049A8A
		protected override void TestMoveToRandom()
		{
			this.MoveToRandom();
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0004B892 File Offset: 0x00049A92
		protected override void TestMoveToInstant()
		{
			this.MoveToInstant(this.TestMoveToValue);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0004B8A0 File Offset: 0x00049AA0
		protected override void TestBump()
		{
			this.Bump(this.TestBumpAmount);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0004B8AE File Offset: 0x00049AAE
		protected override void TestBumpRandom()
		{
			this.BumpRandom();
		}

		// Token: 0x040013E6 RID: 5094
		[MMInspectorGroup("Target", true, 17, false)]
		public T Target;

		// Token: 0x040013E7 RID: 5095
		[MMInspectorGroup("Channel & TimeScale", true, 16, true)]
		[Tooltip("whether this spring should run on scaled time (and be impacted by time scale changes) or unscaled time (and not be impacted by time scale changes)")]
		public MMSpringComponentBase.TimeScaleModes TimeScaleMode = MMSpringComponentBase.TimeScaleModes.Scaled;

		// Token: 0x040013E8 RID: 5096
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x040013E9 RID: 5097
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x040013EA RID: 5098
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x040013EB RID: 5099
		[MMInspectorGroup("Spring Settings", true, 18, false)]
		public MMSpringFloat FloatSpring;

		// Token: 0x040013EC RID: 5100
		[MMInspectorGroup("Randomness", true, 12, true)]
		[Tooltip("the min (x) and max (y) values between which a random target value will be picked when calling MoveToRandom")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 MoveToRandomValue = new Vector2(-2f, 2f);

		// Token: 0x040013ED RID: 5101
		[Tooltip("the min (x) and max (y) values between which a random bump value will be picked when calling BumpRandom")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 BumpAmountRandomValue = new Vector2(20f, 100f);

		// Token: 0x040013EE RID: 5102
		[MMInspectorGroup("Test", true, 20, true)]
		[Tooltip("the value to move this spring to when interacting with any of the MoveTo debug buttons in its inspector")]
		public float TestMoveToValue = 2f;

		// Token: 0x040013EF RID: 5103
		[MMInspectorButtonBar(new string[] { "MoveTo", "MoveToAdditive", "MoveToSubtractive", "MoveToRandom", "MoveToInstant" }, new string[] { "TestMoveTo", "TestMoveToAdditive", "TestMoveToSubtractive", "TestMoveToRandom", "TestMoveToInstant" }, new bool[] { true, true, true, true, true }, new string[] { "main-call-to-action", "", "", "", "" })]
		public bool MoveToToolbar;

		// Token: 0x040013F0 RID: 5104
		[Tooltip("the amount by which to bump this spring when interacting with the Bump debug button in its inspector")]
		public float TestBumpAmount = 75f;

		// Token: 0x040013F1 RID: 5105
		[MMInspectorButtonBar(new string[] { "Bump", "BumpRandom" }, new string[] { "TestBump", "TestBumpRandom" }, new bool[] { true, true }, new string[] { "main-call-to-action", "" })]
		public bool BumpToToolbar;

		// Token: 0x040013F2 RID: 5106
		[MMInspectorButtonBar(new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new bool[] { true, true, true, true }, new string[] { "", "", "", "" })]
		public bool OtherControlsToToolbar;
	}
}
