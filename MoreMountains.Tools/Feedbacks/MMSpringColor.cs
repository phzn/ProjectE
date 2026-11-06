using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000266 RID: 614
	[Serializable]
	public class MMSpringColor : MMSpringDefinition<Color>
	{
		// Token: 0x06001154 RID: 4436 RVA: 0x0004C9EB File Offset: 0x0004ABEB
		public MMSpringColor()
		{
			this.SpringR = new MMSpringFloat();
			this.SpringG = new MMSpringFloat();
			this.SpringB = new MMSpringFloat();
			this.SpringA = new MMSpringFloat();
			this.ColorSpring = new MMSpringFloat();
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0004CA2A File Offset: 0x0004AC2A
		public virtual void SetDamping(float newDamping)
		{
			this.ColorSpring.Damping = newDamping;
			this.SpringR.Damping = newDamping;
			this.SpringG.Damping = newDamping;
			this.SpringB.Damping = newDamping;
			this.SpringA.Damping = newDamping;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0004CA68 File Offset: 0x0004AC68
		public virtual void SetFrequency(float newFrequency)
		{
			this.ColorSpring.Frequency = newFrequency;
			this.SpringR.Frequency = newFrequency;
			this.SpringG.Frequency = newFrequency;
			this.SpringB.Frequency = newFrequency;
			this.SpringA.Frequency = newFrequency;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0004CAA8 File Offset: 0x0004ACA8
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x0004CB14 File Offset: 0x0004AD14
		public override Color CurrentValue
		{
			get
			{
				this._returnCurrentValue.r = this.SpringR.CurrentValue;
				this._returnCurrentValue.g = this.SpringG.CurrentValue;
				this._returnCurrentValue.b = this.SpringB.CurrentValue;
				this._returnCurrentValue.a = this.SpringA.CurrentValue;
				return this._returnCurrentValue;
			}
			set
			{
				this.SpringR.CurrentValue = value.r;
				this.SpringG.CurrentValue = value.g;
				this.SpringB.CurrentValue = value.b;
				this.SpringA.CurrentValue = value.a;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x0004CB68 File Offset: 0x0004AD68
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x0004CBD4 File Offset: 0x0004ADD4
		public override Color TargetValue
		{
			get
			{
				this._returnTargetValue.r = this.SpringR.TargetValue;
				this._returnTargetValue.g = this.SpringG.TargetValue;
				this._returnTargetValue.b = this.SpringB.TargetValue;
				this._returnTargetValue.a = this.SpringA.TargetValue;
				return this._returnTargetValue;
			}
			set
			{
				this.SpringR.TargetValue = value.r;
				this.SpringG.TargetValue = value.g;
				this.SpringB.TargetValue = value.b;
				this.SpringA.TargetValue = value.a;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0004CC28 File Offset: 0x0004AE28
		// (set) Token: 0x0600115C RID: 4444 RVA: 0x0004CC94 File Offset: 0x0004AE94
		public override Color Velocity
		{
			get
			{
				this._returnVelocity.r = this.SpringR.Velocity;
				this._returnVelocity.g = this.SpringG.Velocity;
				this._returnVelocity.b = this.SpringB.Velocity;
				this._returnVelocity.a = this.SpringA.Velocity;
				return this._returnVelocity;
			}
			set
			{
				this.SpringR.Velocity = value.r;
				this.SpringG.Velocity = value.g;
				this.SpringB.Velocity = value.b;
				this.SpringA.Velocity = value.a;
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0004CCE8 File Offset: 0x0004AEE8
		public override void UpdateSpringValue(float deltaTime)
		{
			this.SpringR.Damping = this.ColorSpring.Damping;
			this.SpringG.Damping = this.ColorSpring.Damping;
			this.SpringB.Damping = this.ColorSpring.Damping;
			this.SpringA.Damping = this.ColorSpring.Damping;
			this.SpringR.Frequency = this.ColorSpring.Frequency;
			this.SpringG.Frequency = this.ColorSpring.Frequency;
			this.SpringB.Frequency = this.ColorSpring.Frequency;
			this.SpringA.Frequency = this.ColorSpring.Frequency;
			this.SpringR.UpdateSpringValue(deltaTime);
			this.SpringG.UpdateSpringValue(deltaTime);
			this.SpringB.UpdateSpringValue(deltaTime);
			this.SpringA.UpdateSpringValue(deltaTime);
			this.ColorSpring.UpdateSpringValue(deltaTime);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0004CDE4 File Offset: 0x0004AFE4
		public override void MoveToInstant(Color newValue)
		{
			this.SpringR.MoveToInstant(newValue.r);
			this.SpringG.MoveToInstant(newValue.g);
			this.SpringB.MoveToInstant(newValue.b);
			this.SpringA.MoveToInstant(newValue.a);
			this.ColorSpring.MoveToInstant(newValue.Sum());
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0004CE46 File Offset: 0x0004B046
		public override void Stop()
		{
			this.SpringR.Stop();
			this.SpringG.Stop();
			this.SpringB.Stop();
			this.SpringA.Stop();
			this.ColorSpring.Stop();
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0004CE80 File Offset: 0x0004B080
		public override void SetInitialValue(Color newInitialValue)
		{
			this.SpringR.SetInitialValue(newInitialValue.r);
			this.SpringG.SetInitialValue(newInitialValue.g);
			this.SpringB.SetInitialValue(newInitialValue.b);
			this.SpringA.SetInitialValue(newInitialValue.a);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004CED1 File Offset: 0x0004B0D1
		public override void RestoreInitialValue()
		{
			this.SpringR.RestoreInitialValue();
			this.SpringG.RestoreInitialValue();
			this.SpringB.RestoreInitialValue();
			this.SpringA.RestoreInitialValue();
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004CEFF File Offset: 0x0004B0FF
		public override void SetCurrentValueAsInitialValue()
		{
			this.SpringR.SetCurrentValueAsInitialValue();
			this.SpringG.SetCurrentValueAsInitialValue();
			this.SpringB.SetCurrentValueAsInitialValue();
			this.SpringA.SetCurrentValueAsInitialValue();
			this.ColorSpring.SetCurrentValueAsInitialValue();
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004CF38 File Offset: 0x0004B138
		public override void MoveTo(Color newValue)
		{
			this.SpringR.MoveTo(newValue.r);
			this.SpringG.MoveTo(newValue.g);
			this.SpringB.MoveTo(newValue.b);
			this.SpringA.MoveTo(newValue.a);
			this.ColorSpring.MoveTo(newValue.Sum());
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0004CF9C File Offset: 0x0004B19C
		public override void MoveToAdditive(Color newValue)
		{
			this.SpringR.MoveToAdditive(newValue.r);
			this.SpringG.MoveToAdditive(newValue.g);
			this.SpringB.MoveToAdditive(newValue.b);
			this.SpringA.MoveToAdditive(newValue.a);
			this.ColorSpring.MoveToAdditive(newValue.Sum());
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004D000 File Offset: 0x0004B200
		public override void MoveToSubtractive(Color newValue)
		{
			this.SpringR.MoveToSubtractive(newValue.r);
			this.SpringG.MoveToSubtractive(newValue.g);
			this.SpringB.MoveToSubtractive(newValue.b);
			this.SpringA.MoveToSubtractive(newValue.a);
			this.ColorSpring.MoveToSubtractive(newValue.Sum());
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0004D064 File Offset: 0x0004B264
		public override void MoveToRandom(Color min, Color max)
		{
			this.SpringR.MoveToRandom(min.r, max.r);
			this.SpringG.MoveToRandom(min.g, max.g);
			this.SpringB.MoveToRandom(min.b, max.b);
			this.SpringA.MoveToRandom(min.a, max.a);
			this.ColorSpring.MoveToRandom(min.Sum(), max.Sum());
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0004D0E4 File Offset: 0x0004B2E4
		public override void Bump(Color bumpAmount)
		{
			this.ColorSpring.Bump(bumpAmount.Sum());
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004D0F7 File Offset: 0x0004B2F7
		public override void BumpRandom(Color min, Color max)
		{
			this.ColorSpring.BumpRandom(min.Sum(), max.Sum());
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004D110 File Offset: 0x0004B310
		public override void Finish()
		{
			this.SpringR.Finish();
			this.SpringG.Finish();
			this.SpringB.Finish();
			this.SpringA.Finish();
			this.ColorSpring.Finish();
		}

		// Token: 0x04001456 RID: 5206
		public MMSpringFloat ColorSpring;

		// Token: 0x04001457 RID: 5207
		public MMSpringFloat SpringR;

		// Token: 0x04001458 RID: 5208
		public MMSpringFloat SpringG;

		// Token: 0x04001459 RID: 5209
		public MMSpringFloat SpringB;

		// Token: 0x0400145A RID: 5210
		public MMSpringFloat SpringA;

		// Token: 0x0400145B RID: 5211
		protected Color _returnCurrentValue;

		// Token: 0x0400145C RID: 5212
		protected Color _returnTargetValue;

		// Token: 0x0400145D RID: 5213
		protected Color _returnVelocity;
	}
}
