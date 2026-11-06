using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026B RID: 619
	[Serializable]
	public class MMSpringVector2 : MMSpringDefinition<Vector2>
	{
		// Token: 0x060011AD RID: 4525 RVA: 0x0004D4CF File Offset: 0x0004B6CF
		public MMSpringVector2()
		{
			this.SpringX = new MMSpringFloat();
			this.SpringY = new MMSpringFloat();
			this.UnifiedSpring = new MMSpringFloat();
			this.UnifiedSpring.UnifiedSpring = true;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0004D504 File Offset: 0x0004B704
		public virtual void SetDamping(Vector2 newDamping)
		{
			this.UnifiedSpring.Damping = newDamping.x;
			this.SpringX.Damping = newDamping.x;
			this.SpringY.Damping = newDamping.y;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0004D539 File Offset: 0x0004B739
		public virtual void SetFrequency(Vector2 newFrequency)
		{
			this.UnifiedSpring.Frequency = newFrequency.x;
			this.SpringX.Frequency = newFrequency.x;
			this.SpringY.Frequency = newFrequency.y;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0004D56E File Offset: 0x0004B76E
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x0004D5A2 File Offset: 0x0004B7A2
		public override Vector2 CurrentValue
		{
			get
			{
				this._returnCurrentValue.x = this.SpringX.CurrentValue;
				this._returnCurrentValue.y = this.SpringY.CurrentValue;
				return this._returnCurrentValue;
			}
			set
			{
				this.SpringX.CurrentValue = value.x;
				this.SpringY.CurrentValue = value.y;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0004D5C6 File Offset: 0x0004B7C6
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x0004D5FA File Offset: 0x0004B7FA
		public override Vector2 TargetValue
		{
			get
			{
				this._returnTargetValue.x = this.SpringX.TargetValue;
				this._returnTargetValue.y = this.SpringY.TargetValue;
				return this._returnTargetValue;
			}
			set
			{
				this.SpringX.TargetValue = value.x;
				this.SpringY.TargetValue = value.y;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0004D61E File Offset: 0x0004B81E
		// (set) Token: 0x060011B5 RID: 4533 RVA: 0x0004D652 File Offset: 0x0004B852
		public override Vector2 Velocity
		{
			get
			{
				this._returnVelocity.x = this.SpringX.Velocity;
				this._returnVelocity.y = this.SpringY.Velocity;
				return this._returnVelocity;
			}
			set
			{
				this.SpringX.Velocity = value.x;
				this.SpringY.Velocity = value.y;
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004D678 File Offset: 0x0004B878
		public override void UpdateSpringValue(float deltaTime)
		{
			if (!this.SeparateAxis)
			{
				this.SpringX.Damping = this.UnifiedSpring.Damping;
				this.SpringX.Frequency = this.UnifiedSpring.Frequency;
				this.SpringY.Damping = this.UnifiedSpring.Damping;
				this.SpringY.Frequency = this.UnifiedSpring.Frequency;
			}
			this.SpringX.UpdateSpringValue(deltaTime);
			this.SpringY.UpdateSpringValue(deltaTime);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004D6FD File Offset: 0x0004B8FD
		public override void MoveToInstant(Vector2 newValue)
		{
			this.SpringX.MoveToInstant(newValue.x);
			this.SpringY.MoveToInstant(newValue.y);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004D721 File Offset: 0x0004B921
		public override void Stop()
		{
			this.SpringX.Stop();
			this.SpringY.Stop();
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004D739 File Offset: 0x0004B939
		public override void SetInitialValue(Vector2 newInitialValue)
		{
			this.SpringX.SetInitialValue(newInitialValue.x);
			this.SpringY.SetInitialValue(newInitialValue.y);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0004D75D File Offset: 0x0004B95D
		public override void RestoreInitialValue()
		{
			this.SpringX.RestoreInitialValue();
			this.SpringY.RestoreInitialValue();
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004D775 File Offset: 0x0004B975
		public override void SetCurrentValueAsInitialValue()
		{
			this.SpringX.SetCurrentValueAsInitialValue();
			this.SpringY.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0004D78D File Offset: 0x0004B98D
		public override void MoveTo(Vector2 newValue)
		{
			this.SpringX.MoveTo(newValue.x);
			this.SpringY.MoveTo(newValue.y);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004D7B1 File Offset: 0x0004B9B1
		public override void MoveToAdditive(Vector2 newValue)
		{
			this.SpringX.MoveToAdditive(newValue.x);
			this.SpringY.MoveToAdditive(newValue.y);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004D7D5 File Offset: 0x0004B9D5
		public override void MoveToSubtractive(Vector2 newValue)
		{
			this.SpringX.MoveToSubtractive(newValue.x);
			this.SpringY.MoveToSubtractive(newValue.y);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004D7F9 File Offset: 0x0004B9F9
		public override void MoveToRandom(Vector2 min, Vector2 max)
		{
			this.SpringX.MoveToRandom(min.x, max.x);
			this.SpringY.MoveToRandom(min.y, max.y);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004D829 File Offset: 0x0004BA29
		public override void Bump(Vector2 bumpAmount)
		{
			this.SpringX.Bump(bumpAmount.x);
			this.SpringY.Bump(bumpAmount.y);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004D84D File Offset: 0x0004BA4D
		public override void BumpRandom(Vector2 min, Vector2 max)
		{
			this.SpringX.BumpRandom(min.x, max.x);
			this.SpringY.BumpRandom(min.y, max.y);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004D87D File Offset: 0x0004BA7D
		public override void Finish()
		{
			this.SpringX.Finish();
			this.SpringY.Finish();
		}

		// Token: 0x0400146F RID: 5231
		public bool SeparateAxis;

		// Token: 0x04001470 RID: 5232
		public MMSpringFloat UnifiedSpring;

		// Token: 0x04001471 RID: 5233
		public MMSpringFloat SpringX;

		// Token: 0x04001472 RID: 5234
		public MMSpringFloat SpringY;

		// Token: 0x04001473 RID: 5235
		protected Vector2 _returnCurrentValue;

		// Token: 0x04001474 RID: 5236
		protected Vector2 _returnTargetValue;

		// Token: 0x04001475 RID: 5237
		protected Vector2 _returnVelocity;
	}
}
