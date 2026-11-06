using System;
using MoreMountains.Tools;
using UnityEngine.Events;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000267 RID: 615
	[MMRequiresConstantRepaintOnlyWhenPlaying]
	public abstract class MMSpringComponentBase : MMMonoBehaviour
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x0004D149 File Offset: 0x0004B349
		public virtual bool LowVelocity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0004D14C File Offset: 0x0004B34C
		public virtual void SetVelocityLowThreshold(float threshold)
		{
			this._velocityLowThreshold = threshold;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0004D155 File Offset: 0x0004B355
		protected virtual void Awake()
		{
			this.Initialization();
			base.enabled = false;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0004D164 File Offset: 0x0004B364
		protected virtual void Update()
		{
			this.UpdateSpringValue();
			this.SelfDisable();
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0004D172 File Offset: 0x0004B372
		protected virtual void Activate()
		{
			base.enabled = true;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0004D17B File Offset: 0x0004B37B
		protected virtual void SelfDisable()
		{
			if (this.LowVelocity)
			{
				if (this.OnEquilibriumReached != null)
				{
					this.OnEquilibriumReached.Invoke();
				}
				this.Finish();
				base.enabled = false;
			}
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0004D1A5 File Offset: 0x0004B3A5
		public virtual void Stop()
		{
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0004D1A7 File Offset: 0x0004B3A7
		public virtual void Finish()
		{
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0004D1A9 File Offset: 0x0004B3A9
		public virtual void RestoreInitialValue()
		{
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0004D1AB File Offset: 0x0004B3AB
		public virtual void ResetInitialValue()
		{
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0004D1AD File Offset: 0x0004B3AD
		protected virtual void Initialization()
		{
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0004D1AF File Offset: 0x0004B3AF
		protected virtual void GrabCurrentValue()
		{
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0004D1B1 File Offset: 0x0004B3B1
		protected virtual void UpdateSpringValue()
		{
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0004D1B3 File Offset: 0x0004B3B3
		protected virtual void TestMoveTo()
		{
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0004D1B5 File Offset: 0x0004B3B5
		protected virtual void TestMoveToAdditive()
		{
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004D1B7 File Offset: 0x0004B3B7
		protected virtual void TestMoveToSubtractive()
		{
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004D1B9 File Offset: 0x0004B3B9
		protected virtual void TestMoveToRandom()
		{
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0004D1BB File Offset: 0x0004B3BB
		protected virtual void TestMoveToInstant()
		{
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0004D1BD File Offset: 0x0004B3BD
		protected virtual void TestBump()
		{
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0004D1BF File Offset: 0x0004B3BF
		protected virtual void TestBumpRandom()
		{
		}

		// Token: 0x0400145E RID: 5214
		[MMInspectorGroup("Events", true, 16, true)]
		public UnityEvent OnEquilibriumReached;

		// Token: 0x0400145F RID: 5215
		protected float _velocityLowThreshold = 0.001f;

		// Token: 0x02000527 RID: 1319
		public enum TimeScaleModes
		{
			// Token: 0x040023A9 RID: 9129
			Unscaled,
			// Token: 0x040023AA RID: 9130
			Scaled
		}
	}
}
