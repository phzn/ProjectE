using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200025B RID: 603
	public abstract class MMSpringColorComponent<T> : MMSpringComponentBase, MMEventListener<MMSpringColorEvent>, MMEventListenerBase where T : Component
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x0004AEBC File Offset: 0x000490BC
		public override bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this.ColorSpring.Velocity.r) + Mathf.Abs(this.ColorSpring.Velocity.g) + Mathf.Abs(this.ColorSpring.Velocity.b) + Mathf.Abs(this.ColorSpring.Velocity.a) + Mathf.Abs(this.ColorSpring.ColorSpring.Velocity) < this._velocityLowThreshold;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0004AF3E File Offset: 0x0004913E
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

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0004AF54 File Offset: 0x00049154
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x0004AF5C File Offset: 0x0004915C
		public virtual Color TargetColor { get; set; }

		// Token: 0x060010B1 RID: 4273 RVA: 0x0004AF65 File Offset: 0x00049165
		public virtual void MoveTo(Color newColor)
		{
			this.Activate();
			this.ColorSpring.MoveTo(newColor);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0004AF79 File Offset: 0x00049179
		public virtual void MoveToAdditive(Color newValue)
		{
			this.Activate();
			this.ColorSpring.MoveToAdditive(newValue);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0004AF8D File Offset: 0x0004918D
		public virtual void MoveToSubtractive(Color newValue)
		{
			this.Activate();
			this.ColorSpring.MoveToSubtractive(newValue);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0004AFA1 File Offset: 0x000491A1
		public virtual void MoveToRandom()
		{
			this.Activate();
			this.ColorSpring.MoveToRandom(this.MoveToRandomColorMin, this.MoveToRandomColorMax);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0004AFC0 File Offset: 0x000491C0
		public virtual void MoveToInstant(Vector4 newValue)
		{
			this.Activate();
			this.ColorSpring.MoveToInstant(newValue);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0004AFD9 File Offset: 0x000491D9
		public virtual void MoveToRandom(Color min, Color max)
		{
			this.Activate();
			this.ColorSpring.MoveToRandom(min, max);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0004AFEE File Offset: 0x000491EE
		public virtual void Bump(Color bumpColor)
		{
			this.Activate();
			this._bumping = true;
			this._bumpTargetColor = bumpColor;
			this._initialBumpColor = this.ColorSpring.CurrentValue;
			this.ColorSpring.Bump(bumpColor);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0004B021 File Offset: 0x00049221
		public virtual void BumpRandom()
		{
			this.Activate();
			this._bumpTargetColor = this._bumpTargetColor.MMRandomColor(this.BumpRandomColorMin, this.BumpRandomColorMax);
			this.Bump(this._bumpTargetColor);
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0004B052 File Offset: 0x00049252
		public virtual void BumpRandom(Color min, Color max)
		{
			this.Activate();
			this._bumpTargetColor = this._bumpTargetColor.MMRandomColor(min, max);
			this.Bump(this._bumpTargetColor);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0004B079 File Offset: 0x00049279
		public override void Stop()
		{
			base.Stop();
			base.enabled = false;
			this.GrabCurrentValue();
			this.ColorSpring.Stop();
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0004B0B4 File Offset: 0x000492B4
		public override void RestoreInitialValue()
		{
			this.ColorSpring.RestoreInitialValue();
			this.ApplyValue(this.ColorSpring.CurrentValue);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0004B0D2 File Offset: 0x000492D2
		public override void ResetInitialValue()
		{
			this.ColorSpring.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0004B0E0 File Offset: 0x000492E0
		protected override void UpdateSpringValue()
		{
			if (this._bumping)
			{
				float num = this.ColorSpring.ColorSpring.CurrentValue * this.BumpMultiplier;
				this.ColorSpring.UpdateSpringValue(this.DeltaTime);
				this._newBumpColor = Color.Lerp(this._initialBumpColor, this._bumpTargetColor, num);
				this.ApplyValue(this._newBumpColor);
				return;
			}
			this.ColorSpring.UpdateSpringValue(this.DeltaTime);
			this.ApplyValue(this.ColorSpring.CurrentValue);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0004B165 File Offset: 0x00049365
		public override void Finish()
		{
			this._bumping = false;
			this.ColorSpring.Finish();
			this.ApplyValue(this.ColorSpring.CurrentValue);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0004B18A File Offset: 0x0004938A
		protected override void Initialization()
		{
			base.Initialization();
			this.GrabCurrentValue();
			this.ColorSpring.SetInitialValue(this.ColorSpring.CurrentValue);
			this.ColorSpring.TargetValue = this.ColorSpring.CurrentValue;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0004B1C4 File Offset: 0x000493C4
		protected override void GrabCurrentValue()
		{
			base.GrabCurrentValue();
			this.ColorSpring.CurrentValue = this.TargetColor;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0004B1DD File Offset: 0x000493DD
		protected virtual void ApplyValue(Color newColor)
		{
			this.TargetColor = newColor;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004B1E8 File Offset: 0x000493E8
		public void OnMMEvent(MMSpringColorEvent springEvent)
		{
			bool flag = springEvent.ChannelData != null && MMChannel.Match(springEvent.ChannelData, this.ChannelMode, this.Channel, this.MMChannelDefinition);
			bool flag2 = springEvent.TargetSpring != null && springEvent.TargetSpring.Equals(this);
			if (!flag && !flag2)
			{
				return;
			}
			if (springEvent.OverrideDamping)
			{
				this.ColorSpring.SetDamping(springEvent.NewDamping);
			}
			if (springEvent.OverrideFrequency)
			{
				this.ColorSpring.SetFrequency(springEvent.NewFrequency);
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

		// Token: 0x060010C3 RID: 4291 RVA: 0x0004B334 File Offset: 0x00049534
		protected override void Awake()
		{
			if (this.Target == null)
			{
				this.Target = base.GetComponent<T>();
			}
			base.Awake();
			this.MMEventStartListening<MMSpringColorEvent>();
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0004B361 File Offset: 0x00049561
		protected void OnDestroy()
		{
			this.MMEventStopListening<MMSpringColorEvent>();
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0004B369 File Offset: 0x00049569
		protected override void TestMoveTo()
		{
			this.MoveTo(this.TestMoveToColor);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0004B377 File Offset: 0x00049577
		protected override void TestMoveToAdditive()
		{
			this.MoveToAdditive(this.TestMoveToColor);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0004B385 File Offset: 0x00049585
		protected override void TestMoveToSubtractive()
		{
			this.MoveToSubtractive(this.TestMoveToColor);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0004B393 File Offset: 0x00049593
		protected override void TestMoveToRandom()
		{
			this.MoveToRandom();
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0004B39B File Offset: 0x0004959B
		protected override void TestMoveToInstant()
		{
			this.MoveToInstant(this.TestMoveToColor);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0004B3AE File Offset: 0x000495AE
		protected override void TestBump()
		{
			this.Bump(this.TestBumpColor);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0004B3BC File Offset: 0x000495BC
		protected override void TestBumpRandom()
		{
			this.BumpRandom();
		}

		// Token: 0x040013B8 RID: 5048
		[MMInspectorGroup("Target", true, 17, false)]
		public T Target;

		// Token: 0x040013B9 RID: 5049
		[MMInspectorGroup("Channel & TimeScale", true, 16, true)]
		[Tooltip("whether this spring should run on scaled time (and be impacted by time scale changes) or unscaled time (and not be impacted by time scale changes)")]
		public MMSpringComponentBase.TimeScaleModes TimeScaleMode = MMSpringComponentBase.TimeScaleModes.Scaled;

		// Token: 0x040013BA RID: 5050
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x040013BB RID: 5051
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x040013BC RID: 5052
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x040013BD RID: 5053
		[MMInspectorGroup("Spring Settings", true, 18, false)]
		[Header("Spring")]
		[Tooltip("the spring definition driving all sub spring components for this color spring")]
		public MMSpringColor ColorSpring;

		// Token: 0x040013BE RID: 5054
		[Tooltip("the multiplier to apply when bumping this color spring (increase this if you're not getting enough of the bump color on bump)")]
		public float BumpMultiplier = 20f;

		// Token: 0x040013BF RID: 5055
		[MMInspectorGroup("Randomness", true, 12, true)]
		[Header("Move To Random")]
		[Tooltip("the min color from which to pick a random color in MoveToRandom mode")]
		public Color MoveToRandomColorMin = MMColors.LawnGreen;

		// Token: 0x040013C0 RID: 5056
		[Tooltip("the max color from which to pick a random color in MoveToRandom mode")]
		public Color MoveToRandomColorMax = MMColors.MediumSeaGreen;

		// Token: 0x040013C1 RID: 5057
		[Tooltip("the min color from which to pick a random color in BumpRandom mode")]
		public Color BumpRandomColorMin = MMColors.HotPink;

		// Token: 0x040013C2 RID: 5058
		[Tooltip("the max color from which to pick a random color in BumpRandom mode")]
		public Color BumpRandomColorMax = MMColors.Plum;

		// Token: 0x040013C3 RID: 5059
		[MMInspectorGroup("Test", true, 20, true)]
		[Tooltip("the value to move this spring to when interacting with any of the MoveTo debug buttons in its inspector")]
		public Color TestMoveToColor = MMColors.Aquamarine;

		// Token: 0x040013C4 RID: 5060
		[MMInspectorButtonBar(new string[] { "MoveTo", "MoveToAdditive", "MoveToSubtractive", "MoveToRandom", "MoveToInstant" }, new string[] { "TestMoveTo", "TestMoveToAdditive", "TestMoveToSubtractive", "TestMoveToRandom", "TestMoveToInstant" }, new bool[] { true, true, true, true, true }, new string[] { "main-call-to-action", "", "", "", "" })]
		public bool MoveToToolbar;

		// Token: 0x040013C5 RID: 5061
		[Tooltip("the amount by which to bump this spring when interacting with the Bump debug button in its inspector")]
		public Color TestBumpColor = MMColors.Orange;

		// Token: 0x040013C6 RID: 5062
		[MMInspectorButtonBar(new string[] { "Bump", "BumpRandom" }, new string[] { "TestBump", "TestBumpRandom" }, new bool[] { true, true }, new string[] { "main-call-to-action", "" })]
		public bool BumpToToolbar;

		// Token: 0x040013C7 RID: 5063
		[MMInspectorButtonBar(new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new string[] { "Stop", "Finish", "RestoreInitialValue", "ResetInitialValue" }, new bool[] { true, true, true, true }, new string[] { "", "", "", "" })]
		public bool OtherControlsToToolbar;

		// Token: 0x040013C9 RID: 5065
		protected bool _bumping;

		// Token: 0x040013CA RID: 5066
		protected Color _newBumpColor;

		// Token: 0x040013CB RID: 5067
		protected Color _bumpTargetColor;

		// Token: 0x040013CC RID: 5068
		protected Color _initialBumpColor;

		// Token: 0x040013CD RID: 5069
		protected Coroutine _coroutine;
	}
}
