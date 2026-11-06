using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000126 RID: 294
	public class DebugUIHandlerFloatField : DebugUIHandlerWidget
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x00029503 File Offset: 0x00027703
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.FloatField>();
			this.nameLabel.text = this.m_Field.displayName;
			this.UpdateValueLabel();
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00029534 File Offset: 0x00027734
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00029559 File Offset: 0x00027759
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002957D File Offset: 0x0002777D
		public override void OnIncrement(bool fast)
		{
			this.ChangeValue(fast, 1f);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002958B File Offset: 0x0002778B
		public override void OnDecrement(bool fast)
		{
			this.ChangeValue(fast, -1f);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002959C File Offset: 0x0002779C
		private void ChangeValue(bool fast, float multiplier)
		{
			float num = this.m_Field.GetValue();
			num += this.m_Field.incStep * (fast ? this.m_Field.incStepMult : 1f) * multiplier;
			this.m_Field.SetValue(num);
			this.UpdateValueLabel();
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000295F0 File Offset: 0x000277F0
		private void UpdateValueLabel()
		{
			this.valueLabel.text = this.m_Field.GetValue().ToString("N" + this.m_Field.decimals.ToString());
		}

		// Token: 0x04000522 RID: 1314
		public Text nameLabel;

		// Token: 0x04000523 RID: 1315
		public Text valueLabel;

		// Token: 0x04000524 RID: 1316
		private DebugUI.FloatField m_Field;
	}
}
