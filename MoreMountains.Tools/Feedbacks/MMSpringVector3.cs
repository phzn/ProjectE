using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200026C RID: 620
	[Serializable]
	public class MMSpringVector3 : MMSpringDefinition<Vector3>
	{
		// Token: 0x060011C3 RID: 4547 RVA: 0x0004D895 File Offset: 0x0004BA95
		public MMSpringVector3()
		{
			this.SpringX = new MMSpringFloat();
			this.SpringY = new MMSpringFloat();
			this.SpringZ = new MMSpringFloat();
			this.UnifiedSpring = new MMSpringFloat();
			this.UnifiedSpring.UnifiedSpring = true;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004D8D8 File Offset: 0x0004BAD8
		public virtual void SetDamping(Vector3 newDamping)
		{
			this.UnifiedSpring.Damping = newDamping.x;
			this.SpringX.Damping = newDamping.x;
			this.SpringY.Damping = newDamping.y;
			this.SpringZ.Damping = newDamping.z;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004D92C File Offset: 0x0004BB2C
		public virtual void SetFrequency(Vector3 newFrequency)
		{
			this.UnifiedSpring.Frequency = newFrequency.x;
			this.SpringX.Frequency = newFrequency.x;
			this.SpringY.Frequency = newFrequency.y;
			this.SpringZ.Frequency = newFrequency.z;
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0004D980 File Offset: 0x0004BB80
		// (set) Token: 0x060011C7 RID: 4551 RVA: 0x0004D9D5 File Offset: 0x0004BBD5
		public override Vector3 CurrentValue
		{
			get
			{
				this._returnCurrentValue.x = this.SpringX.CurrentValue;
				this._returnCurrentValue.y = this.SpringY.CurrentValue;
				this._returnCurrentValue.z = this.SpringZ.CurrentValue;
				return this._returnCurrentValue;
			}
			set
			{
				this.SpringX.CurrentValue = value.x;
				this.SpringY.CurrentValue = value.y;
				this.SpringZ.CurrentValue = value.z;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x0004DA61 File Offset: 0x0004BC61
		public override Vector3 TargetValue
		{
			get
			{
				this._returnTargetValue.x = this.SpringX.TargetValue;
				this._returnTargetValue.y = this.SpringY.TargetValue;
				this._returnTargetValue.z = this.SpringZ.TargetValue;
				return this._returnTargetValue;
			}
			set
			{
				this.SpringX.TargetValue = value.x;
				this.SpringY.TargetValue = value.y;
				this.SpringZ.TargetValue = value.z;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x0004DA98 File Offset: 0x0004BC98
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x0004DAED File Offset: 0x0004BCED
		public override Vector3 Velocity
		{
			get
			{
				this._returnVelocity.x = this.SpringX.Velocity;
				this._returnVelocity.y = this.SpringY.Velocity;
				this._returnVelocity.z = this.SpringZ.Velocity;
				return this._returnVelocity;
			}
			set
			{
				this.SpringX.Velocity = value.x;
				this.SpringY.Velocity = value.y;
				this.SpringZ.Velocity = value.z;
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004DB24 File Offset: 0x0004BD24
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
			}
			this.SpringX.UpdateSpringValue(deltaTime);
			this.SpringY.UpdateSpringValue(deltaTime);
			this.SpringZ.UpdateSpringValue(deltaTime);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004DBE4 File Offset: 0x0004BDE4
		public override void MoveToInstant(Vector3 newValue)
		{
			this.SpringX.MoveToInstant(newValue.x);
			this.SpringY.MoveToInstant(newValue.y);
			this.SpringZ.MoveToInstant(newValue.z);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0004DC19 File Offset: 0x0004BE19
		public override void Stop()
		{
			this.SpringX.Stop();
			this.SpringY.Stop();
			this.SpringZ.Stop();
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0004DC3C File Offset: 0x0004BE3C
		public override void SetInitialValue(Vector3 newInitialValue)
		{
			this.SpringX.SetInitialValue(newInitialValue.x);
			this.SpringY.SetInitialValue(newInitialValue.y);
			this.SpringZ.SetInitialValue(newInitialValue.z);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0004DC71 File Offset: 0x0004BE71
		public override void RestoreInitialValue()
		{
			this.SpringX.RestoreInitialValue();
			this.SpringY.RestoreInitialValue();
			this.SpringZ.RestoreInitialValue();
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0004DC94 File Offset: 0x0004BE94
		public override void SetCurrentValueAsInitialValue()
		{
			this.SpringX.SetCurrentValueAsInitialValue();
			this.SpringY.SetCurrentValueAsInitialValue();
			this.SpringZ.SetCurrentValueAsInitialValue();
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0004DCB7 File Offset: 0x0004BEB7
		public override void MoveTo(Vector3 newValue)
		{
			this.SpringX.MoveTo(newValue.x);
			this.SpringY.MoveTo(newValue.y);
			this.SpringZ.MoveTo(newValue.z);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0004DCEC File Offset: 0x0004BEEC
		public override void MoveToAdditive(Vector3 newValue)
		{
			this.SpringX.MoveToAdditive(newValue.x);
			this.SpringY.MoveToAdditive(newValue.y);
			this.SpringZ.MoveToAdditive(newValue.z);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004DD21 File Offset: 0x0004BF21
		public override void MoveToSubtractive(Vector3 newValue)
		{
			this.SpringX.MoveToSubtractive(newValue.x);
			this.SpringY.MoveToSubtractive(newValue.y);
			this.SpringZ.MoveToSubtractive(newValue.z);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004DD58 File Offset: 0x0004BF58
		public override void MoveToRandom(Vector3 min, Vector3 max)
		{
			this.SpringX.MoveToRandom(min.x, max.x);
			this.SpringY.MoveToRandom(min.y, max.y);
			this.SpringZ.MoveToRandom(min.z, max.z);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004DDAA File Offset: 0x0004BFAA
		public override void Bump(Vector3 bumpAmount)
		{
			this.SpringX.Bump(bumpAmount.x);
			this.SpringY.Bump(bumpAmount.y);
			this.SpringZ.Bump(bumpAmount.z);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004DDE0 File Offset: 0x0004BFE0
		public override void BumpRandom(Vector3 min, Vector3 max)
		{
			this.SpringX.BumpRandom(min.x, max.x);
			this.SpringY.BumpRandom(min.y, max.y);
			this.SpringZ.BumpRandom(min.z, max.z);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0004DE32 File Offset: 0x0004C032
		public override void Finish()
		{
			this.SpringX.Finish();
			this.SpringY.Finish();
			this.SpringZ.Finish();
		}

		// Token: 0x04001476 RID: 5238
		public bool SeparateAxis;

		// Token: 0x04001477 RID: 5239
		public MMSpringFloat UnifiedSpring;

		// Token: 0x04001478 RID: 5240
		public MMSpringFloat SpringX;

		// Token: 0x04001479 RID: 5241
		public MMSpringFloat SpringY;

		// Token: 0x0400147A RID: 5242
		public MMSpringFloat SpringZ;

		// Token: 0x0400147B RID: 5243
		protected Vector3 _returnCurrentValue;

		// Token: 0x0400147C RID: 5244
		protected Vector3 _returnTargetValue;

		// Token: 0x0400147D RID: 5245
		protected Vector3 _returnVelocity;
	}
}
