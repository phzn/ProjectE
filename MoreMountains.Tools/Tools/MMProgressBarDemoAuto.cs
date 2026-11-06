using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000051 RID: 81
	public class MMProgressBarDemoAuto : MonoBehaviour
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x00008DD4 File Offset: 0x00006FD4
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008DDC File Offset: 0x00006FDC
		protected virtual void Initialization()
		{
			this._progressBar = base.GetComponent<MMProgressBar>();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008DEC File Offset: 0x00006FEC
		protected virtual void Update()
		{
			if (this.TestMode == MMProgressBarDemoAuto.TestModes.Permanent)
			{
				this._progressBar.UpdateBar(this.CurrentValue, this.MinValue, this.MaxValue);
				this.CurrentValue += this.Speed * Time.deltaTime * this._direction;
				if (this.CurrentValue <= this.MinValue || this.CurrentValue >= this.MaxValue)
				{
					this._direction *= -1f;
				}
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008E6C File Offset: 0x0000706C
		protected virtual void OneTime()
		{
			this._progressBar.UpdateBar(this.OneTimeNewValue, this.OneTimeMinValue, this.OneTimeMaxValue);
		}

		// Token: 0x0400026B RID: 619
		public MMProgressBarDemoAuto.TestModes TestMode;

		// Token: 0x0400026C RID: 620
		[MMEnumCondition("TestMode", new int[] { 0 })]
		public float CurrentValue;

		// Token: 0x0400026D RID: 621
		[MMEnumCondition("TestMode", new int[] { 0 })]
		public float MinValue;

		// Token: 0x0400026E RID: 622
		[MMEnumCondition("TestMode", new int[] { 0 })]
		public float MaxValue = 100f;

		// Token: 0x0400026F RID: 623
		[MMEnumCondition("TestMode", new int[] { 0 })]
		public float Speed = 1f;

		// Token: 0x04000270 RID: 624
		[MMEnumCondition("TestMode", new int[] { 1 })]
		public float OneTimeNewValue;

		// Token: 0x04000271 RID: 625
		[MMEnumCondition("TestMode", new int[] { 1 })]
		public float OneTimeMinValue;

		// Token: 0x04000272 RID: 626
		[MMEnumCondition("TestMode", new int[] { 1 })]
		public float OneTimeMaxValue;

		// Token: 0x04000273 RID: 627
		[MMEnumCondition("TestMode", new int[] { 1 })]
		[MMInspectorButton("OneTime")]
		public bool OneTimeButton;

		// Token: 0x04000274 RID: 628
		protected float _direction = 1f;

		// Token: 0x04000275 RID: 629
		protected MMProgressBar _progressBar;

		// Token: 0x020003C7 RID: 967
		public enum TestModes
		{
			// Token: 0x04001DE3 RID: 7651
			Permanent,
			// Token: 0x04001DE4 RID: 7652
			OneTime
		}
	}
}
