using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012C RID: 300
	public class DebugUIHandlerIntField : DebugUIHandlerWidget
	{
		// Token: 0x060008F5 RID: 2293 RVA: 0x00029C9A File Offset: 0x00027E9A
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.IntField>();
			this.nameLabel.text = this.m_Field.displayName;
			this.UpdateValueLabel();
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00029CCB File Offset: 0x00027ECB
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00029CF0 File Offset: 0x00027EF0
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00029D14 File Offset: 0x00027F14
		public override void OnIncrement(bool fast)
		{
			this.ChangeValue(fast, 1);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00029D1E File Offset: 0x00027F1E
		public override void OnDecrement(bool fast)
		{
			this.ChangeValue(fast, -1);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00029D28 File Offset: 0x00027F28
		private void ChangeValue(bool fast, int multiplier)
		{
			int num = this.m_Field.GetValue();
			num += this.m_Field.incStep * (fast ? this.m_Field.intStepMult : 1) * multiplier;
			this.m_Field.SetValue(num);
			this.UpdateValueLabel();
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00029D78 File Offset: 0x00027F78
		private void UpdateValueLabel()
		{
			if (this.valueLabel != null)
			{
				this.valueLabel.text = this.m_Field.GetValue().ToString("N0");
			}
		}

		// Token: 0x0400053D RID: 1341
		public Text nameLabel;

		// Token: 0x0400053E RID: 1342
		public Text valueLabel;

		// Token: 0x0400053F RID: 1343
		private DebugUI.IntField m_Field;
	}
}
