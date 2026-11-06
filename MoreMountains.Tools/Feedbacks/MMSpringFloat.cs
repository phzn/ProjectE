using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026A RID: 618
	[Serializable]
	public class MMSpringFloat : MMSpringDefinition<float>
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x0004D1F4 File Offset: 0x0004B3F4
		// (set) Token: 0x06001196 RID: 4502 RVA: 0x0004D1FC File Offset: 0x0004B3FC
		[MMInspectorGroup("Debug", true, 19, true)]
		[Tooltip("the current value of this spring")]
		public override float CurrentValue
		{
			get
			{
				return this._returnCurrentValue;
			}
			set
			{
				this._actualCurrentValue = value;
				this._returnCurrentValue = value;
				this.UpdateSpringDebug();
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x0004D212 File Offset: 0x0004B412
		// (set) Token: 0x06001198 RID: 4504 RVA: 0x0004D21A File Offset: 0x0004B41A
		[Tooltip("the value towards which this spring is trending, and that it'll reach once it stops oscillating")]
		public override float TargetValue
		{
			get
			{
				return this._targetValue;
			}
			set
			{
				this._targetValue = this.ClampSettings.GetTargetValue(value, this.InitialValue);
				this.UpdateSpringDebug();
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x0004D23A File Offset: 0x0004B43A
		// (set) Token: 0x0600119A RID: 4506 RVA: 0x0004D242 File Offset: 0x0004B442
		[Tooltip("the current velocity of the spring")]
		public override float Velocity
		{
			get
			{
				return this._velocity;
			}
			set
			{
				this._velocity = value;
				this.UpdateSpringDebug();
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x0004D251 File Offset: 0x0004B451
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x0004D259 File Offset: 0x0004B459
		public float InitialValue { get; protected set; }

		// Token: 0x0600119D RID: 4509 RVA: 0x0004D264 File Offset: 0x0004B464
		public override void UpdateSpringValue(float deltaTime)
		{
			MMMaths.Spring(ref this._actualCurrentValue, this.TargetValue, ref this._velocity, this.Damping, this.Frequency, deltaTime);
			this._returnCurrentValue = this._actualCurrentValue;
			if (this.ClampSettings.ClampNeeded)
			{
				this.HandleClampMode();
			}
			this.UpdateSpringDebug();
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004D2BC File Offset: 0x0004B4BC
		protected virtual void HandleClampMode()
		{
			float num = (this.ClampSettings.ClampMinInitial ? this.InitialValue : this.ClampSettings.ClampMinValue);
			float num2 = (this.ClampSettings.ClampMaxInitial ? this.InitialValue : this.ClampSettings.ClampMaxValue);
			if (this.ClampSettings.ClampMin && this._actualCurrentValue < num)
			{
				if (this.ClampSettings.ClampMinBounce)
				{
					this._returnCurrentValue = Mathf.Abs(this._actualCurrentValue - num) + num;
				}
				else
				{
					this._returnCurrentValue = Mathf.Max(this._actualCurrentValue, num);
				}
			}
			if (this.ClampSettings.ClampMax && this._actualCurrentValue > num2)
			{
				if (this.ClampSettings.ClampMaxBounce)
				{
					this._returnCurrentValue = num2 - (this._actualCurrentValue - num2);
					return;
				}
				this._returnCurrentValue = Mathf.Min(this._actualCurrentValue, num2);
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004D39D File Offset: 0x0004B59D
		protected virtual void UpdateSpringDebug()
		{
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0004D39F File Offset: 0x0004B59F
		public override void MoveToInstant(float newValue)
		{
			this._actualCurrentValue = newValue;
			this._returnCurrentValue = newValue;
			this.TargetValue = newValue;
			this.Velocity = 0f;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004D3C1 File Offset: 0x0004B5C1
		public override void Stop()
		{
			this.Velocity = 0f;
			this.TargetValue = this._actualCurrentValue;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004D3DA File Offset: 0x0004B5DA
		public override void SetInitialValue(float newInitialValue)
		{
			this.InitialValue = newInitialValue;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004D3E3 File Offset: 0x0004B5E3
		public override void RestoreInitialValue()
		{
			this._actualCurrentValue = this.InitialValue;
			this._returnCurrentValue = this.InitialValue;
			this.TargetValue = this._actualCurrentValue;
			this.UpdateSpringDebug();
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004D40F File Offset: 0x0004B60F
		public override void SetCurrentValueAsInitialValue()
		{
			this.InitialValue = this._actualCurrentValue;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004D41D File Offset: 0x0004B61D
		public override void MoveTo(float newValue)
		{
			this.TargetValue = newValue;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004D426 File Offset: 0x0004B626
		public override void MoveToAdditive(float newValue)
		{
			this.TargetValue += newValue;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004D436 File Offset: 0x0004B636
		public override void MoveToSubtractive(float newValue)
		{
			this.TargetValue -= newValue;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0004D446 File Offset: 0x0004B646
		public override void MoveToRandom(float min, float max)
		{
			this.TargetValue = Random.Range(min, max);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0004D455 File Offset: 0x0004B655
		public override void Bump(float bumpAmount)
		{
			this.Velocity += bumpAmount;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004D465 File Offset: 0x0004B665
		public override void BumpRandom(float min, float max)
		{
			this.Velocity += Random.Range(min, max);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004D47B File Offset: 0x0004B67B
		public override void Finish()
		{
			this.Velocity = 0f;
			this._actualCurrentValue = this.TargetValue;
			this._returnCurrentValue = this.TargetValue;
			this.UpdateSpringDebug();
		}

		// Token: 0x04001462 RID: 5218
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float Damping = 0.4f;

		// Token: 0x04001463 RID: 5219
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float Frequency = 6f;

		// Token: 0x04001464 RID: 5220
		public MMSpringClampSettings ClampSettings;

		// Token: 0x04001466 RID: 5222
		public MMSpringDebug SpringDebug = new MMSpringDebug();

		// Token: 0x04001467 RID: 5223
		[MMHidden]
		public bool UnifiedSpring;

		// Token: 0x04001468 RID: 5224
		[MMHidden]
		public float CurrentValueDisplay;

		// Token: 0x04001469 RID: 5225
		[MMHidden]
		public float TargetValueDisplay;

		// Token: 0x0400146A RID: 5226
		[MMHidden]
		public float VelocityDisplay;

		// Token: 0x0400146B RID: 5227
		protected float _actualCurrentValue;

		// Token: 0x0400146C RID: 5228
		protected float _returnCurrentValue;

		// Token: 0x0400146D RID: 5229
		protected float _targetValue;

		// Token: 0x0400146E RID: 5230
		protected float _velocity;
	}
}
