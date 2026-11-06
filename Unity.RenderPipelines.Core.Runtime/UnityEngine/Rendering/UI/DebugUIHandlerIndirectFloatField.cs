using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012A RID: 298
	public class DebugUIHandlerIndirectFloatField : DebugUIHandlerWidget
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x00029A8B File Offset: 0x00027C8B
		public void Init()
		{
			this.UpdateValueLabel();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00029A93 File Offset: 0x00027C93
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00029AB8 File Offset: 0x00027CB8
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00029ADC File Offset: 0x00027CDC
		public override void OnIncrement(bool fast)
		{
			this.ChangeValue(fast, 1f);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00029AEA File Offset: 0x00027CEA
		public override void OnDecrement(bool fast)
		{
			this.ChangeValue(fast, -1f);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00029AF8 File Offset: 0x00027CF8
		private void ChangeValue(bool fast, float multiplier)
		{
			float num = this.getter();
			num += this.incStepGetter() * (fast ? this.incStepMultGetter() : 1f) * multiplier;
			this.setter(num);
			this.UpdateValueLabel();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00029B4C File Offset: 0x00027D4C
		private void UpdateValueLabel()
		{
			if (this.valueLabel != null)
			{
				this.valueLabel.text = this.getter().ToString("N" + this.decimalsGetter().ToString());
			}
		}

		// Token: 0x04000530 RID: 1328
		public Text nameLabel;

		// Token: 0x04000531 RID: 1329
		public Text valueLabel;

		// Token: 0x04000532 RID: 1330
		public Func<float> getter;

		// Token: 0x04000533 RID: 1331
		public Action<float> setter;

		// Token: 0x04000534 RID: 1332
		public Func<float> incStepGetter;

		// Token: 0x04000535 RID: 1333
		public Func<float> incStepMultGetter;

		// Token: 0x04000536 RID: 1334
		public Func<float> decimalsGetter;
	}
}
