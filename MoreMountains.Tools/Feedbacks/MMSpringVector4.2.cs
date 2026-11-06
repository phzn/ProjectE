using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026D RID: 621
	[Serializable]
	public class MMSpringVector4 : MMSpringDefinition<Vector4>
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x0004DE58 File Offset: 0x0004C058
		public MMSpringVector4()
		{
			this.SpringX = new MMSpringFloat();
			this.SpringY = new MMSpringFloat();
			this.SpringZ = new MMSpringFloat();
			this.SpringW = new MMSpringFloat();
			this.UnifiedSpring = new MMSpringFloat();
			this.UnifiedSpring.UnifiedSpring = true;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0004DEB0 File Offset: 0x0004C0B0
		public virtual void SetDamping(Vector4 newDamping)
		{
			this.UnifiedSpring.Damping = newDamping.x;
			this.SpringX.Damping = newDamping.x;
			this.SpringY.Damping = newDamping.y;
			this.SpringZ.Damping = newDamping.z;
			this.SpringW.Damping = newDamping.w;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004DF14 File Offset: 0x0004C114
		public virtual void SetFrequency(Vector4 newFrequency)
		{
			this.UnifiedSpring.Frequency = newFrequency.x;
			this.SpringX.Frequency = newFrequency.x;
			this.SpringY.Frequency = newFrequency.y;
			this.SpringZ.Frequency = newFrequency.z;
			this.SpringW.Frequency = newFrequency.w;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x0004DF78 File Offset: 0x0004C178
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x0004DFE4 File Offset: 0x0004C1E4
		public override Vector4 CurrentValue
		{
			get
			{
				this._returnCurrentValue.x = this.SpringX.CurrentValue;
				this._returnCurrentValue.y = this.SpringY.CurrentValue;
				this._returnCurrentValue.z = this.SpringZ.CurrentValue;
				this._returnCurrentValue.w = this.SpringW.CurrentValue;
				return this._returnCurrentValue;
			}
			set
			{
				this.SpringX.CurrentValue = value.x;
				this.SpringY.CurrentValue = value.y;
				this.SpringZ.CurrentValue = value.z;
				this.SpringW.CurrentValue = value.w;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x0004E038 File Offset: 0x0004C238
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x0004E0A4 File Offset: 0x0004C2A4
		public override Vector4 TargetValue
		{
			get
			{
				this._returnTargetValue.x = this.SpringX.TargetValue;
				this._returnTargetValue.y = this.SpringY.TargetValue;
				this._returnTargetValue.z = this.SpringZ.TargetValue;
				this._returnTargetValue.w = this.SpringW.TargetValue;
				return this._returnTargetValue;
			}
			set
			{
				this.SpringX.TargetValue = value.x;
				this.SpringY.TargetValue = value.y;
				this.SpringZ.TargetValue = value.z;
				this.SpringW.TargetValue = value.w;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x0004E0F8 File Offset: 0x0004C2F8
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x0004E164 File Offset: 0x0004C364
		public override Vector4 Velocity
		{
			get
			{
				this._returnVelocity.x = this.SpringX.Velocity;
				this._returnVelocity.y = this.SpringY.Velocity;
				this._returnVelocity.z = this.SpringZ.Velocity;
				this._returnVelocity.w = this.SpringW.Velocity;
				return this._returnVelocity;
			}
			set
			{
				this.SpringX.Velocity = value.x;
				this.SpringY.Velocity = value.y;
				this.SpringZ.Velocity = value.z;
				this.SpringW.Velocity = value.w;
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0004E1B8 File Offset: 0x0004C3B8
		public override void UpdateSpringValue(float deltaTime)
		{
			if (!this.SeparateAxis)
			{
				this.SpringX.Damping = this.UnifiedSpring.Damping;
				this.SpringX.Frequency = this.UnifiedSpring.Frequency;
				this.SpringY.Damping = this.UnifiedSpring.Damping;
				this.SpringY.Frequency = this.UnifiedSpring.Frequency;
				this.SpringZ.Damping = this.UnifiedSpring.Damping;
				this.SpringZ.Frequency = this.UnifiedSpring.Frequency;
				this.SpringW.Damping = this.UnifiedSpring.Damping;
				this.SpringW.Frequency = this.UnifiedSpring.Frequency;
			}
			this.SpringX.UpdateSpringValue(deltaTime);
			this.SpringY.UpdateSpringValue(deltaTime);
			this.SpringZ.UpdateSpringValue(deltaTime);
			this.SpringW.UpdateSpringValue(deltaTime);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004E2B0 File Offset: 0x0004C4B0
		public override void MoveToInstant(Vector4 newValue)
		{
			this.SpringX.MoveToInstant(newValue.x);
			this.SpringY.MoveToInstant(newValue.y);
			this.SpringZ.MoveToInstant(newValue.z);
			this.SpringW.MoveToInstant(newValue.w);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004E301 File Offset: 0x0004C501
		public override void Stop()
		{
			this.SpringX.Stop();
			this.SpringY.Stop();
			this.SpringZ.Stop();
			this.SpringW.Stop();
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004E330 File Offset: 0x0004C530
		public override void SetInitialValue(Vector4 newInitialValue)
		{
			this.SpringX.SetInitialValue(newInitialValue.x);
			this.SpringY.SetInitialValue(newInitialValue.y);
			this.SpringZ.SetInitialValue(newInitialValue.z);
			this.SpringW.SetInitialValue(newInitialValue.w);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0004E381 File Offset: 0x0004C581
		public override void RestoreInitialValue()
		{
			this.SpringX.RestoreInitialValue();
			this.SpringY.RestoreInitialValue();
			this.SpringZ.RestoreInitialValue();
			this.SpringW.RestoreInitialValue();
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004E3AF File Offset: 0x0004C5AF
		public override void SetCurrentValueAsInitialValue()
		{
			this.SpringX.SetCurrentValueAsInitialValue();
			this.SpringY.SetCurrentValueAsInitialValue();
			this.SpringZ.SetCurrentValueAsInitialValue();
			this.SpringW.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004E3E0 File Offset: 0x0004C5E0
		public override void MoveTo(Vector4 newValue)
		{
			this.SpringX.MoveTo(newValue.x);
			this.SpringY.MoveTo(newValue.y);
			this.SpringZ.MoveTo(newValue.z);
			this.SpringW.MoveTo(newValue.w);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004E434 File Offset: 0x0004C634
		public override void MoveToAdditive(Vector4 newValue)
		{
			this.SpringX.MoveToAdditive(newValue.x);
			this.SpringY.MoveToAdditive(newValue.y);
			this.SpringZ.MoveToAdditive(newValue.z);
			this.SpringW.MoveToAdditive(newValue.w);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004E488 File Offset: 0x0004C688
		public override void MoveToSubtractive(Vector4 newValue)
		{
			this.SpringX.MoveToSubtractive(newValue.x);
			this.SpringY.MoveToSubtractive(newValue.y);
			this.SpringZ.MoveToSubtractive(newValue.z);
			this.SpringW.MoveToSubtractive(newValue.w);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004E4DC File Offset: 0x0004C6DC
		public override void MoveToRandom(Vector4 min, Vector4 max)
		{
			this.SpringX.MoveToRandom(min.x, max.x);
			this.SpringY.MoveToRandom(min.y, max.y);
			this.SpringZ.MoveToRandom(min.z, max.z);
			this.SpringW.MoveToRandom(min.w, max.w);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004E548 File Offset: 0x0004C748
		public override void Bump(Vector4 bumpAmount)
		{
			this.SpringX.Bump(bumpAmount.x);
			this.SpringY.Bump(bumpAmount.y);
			this.SpringZ.Bump(bumpAmount.z);
			this.SpringW.Bump(bumpAmount.w);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004E59C File Offset: 0x0004C79C
		public override void BumpRandom(Vector4 min, Vector4 max)
		{
			this.SpringX.BumpRandom(min.x, max.x);
			this.SpringY.BumpRandom(min.y, max.y);
			this.SpringZ.BumpRandom(min.z, max.z);
			this.SpringW.BumpRandom(min.w, max.w);
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0004E605 File Offset: 0x0004C805
		public override void Finish()
		{
			this.SpringX.Finish();
			this.SpringY.Finish();
			this.SpringZ.Finish();
			this.SpringW.Finish();
		}

		// Token: 0x0400147E RID: 5246
		public bool SeparateAxis;

		// Token: 0x0400147F RID: 5247
		public MMSpringFloat UnifiedSpring;

		// Token: 0x04001480 RID: 5248
		public MMSpringFloat SpringX;

		// Token: 0x04001481 RID: 5249
		public MMSpringFloat SpringY;

		// Token: 0x04001482 RID: 5250
		public MMSpringFloat SpringZ;

		// Token: 0x04001483 RID: 5251
		public MMSpringFloat SpringW;

		// Token: 0x04001484 RID: 5252
		protected Vector4 _returnCurrentValue;

		// Token: 0x04001485 RID: 5253
		protected Vector4 _returnTargetValue;

		// Token: 0x04001486 RID: 5254
		protected Vector4 _returnVelocity;
	}
}
