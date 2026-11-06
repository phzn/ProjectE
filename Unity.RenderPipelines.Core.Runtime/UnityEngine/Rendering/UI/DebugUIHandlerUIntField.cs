using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000137 RID: 311
	public class DebugUIHandlerUIntField : DebugUIHandlerWidget
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x0002AB6F File Offset: 0x00028D6F
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.UIntField>();
			this.nameLabel.text = this.m_Field.displayName;
			this.UpdateValueLabel();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002ABA0 File Offset: 0x00028DA0
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002ABC5 File Offset: 0x00028DC5
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002ABE9 File Offset: 0x00028DE9
		public override void OnIncrement(bool fast)
		{
			this.ChangeValue(fast, 1);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002ABF3 File Offset: 0x00028DF3
		public override void OnDecrement(bool fast)
		{
			this.ChangeValue(fast, -1);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002AC00 File Offset: 0x00028E00
		private void ChangeValue(bool fast, int multiplier)
		{
			long num = (long)((ulong)this.m_Field.GetValue());
			if (num == 0L && multiplier < 0)
			{
				return;
			}
			num += (long)((ulong)(this.m_Field.incStep * (fast ? this.m_Field.intStepMult : 1U)) * (ulong)((long)multiplier));
			this.m_Field.SetValue((uint)num);
			this.UpdateValueLabel();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002AC5C File Offset: 0x00028E5C
		private void UpdateValueLabel()
		{
			if (this.valueLabel != null)
			{
				this.valueLabel.text = this.m_Field.GetValue().ToString("N0");
			}
		}

		// Token: 0x04000563 RID: 1379
		public Text nameLabel;

		// Token: 0x04000564 RID: 1380
		public Text valueLabel;

		// Token: 0x04000565 RID: 1381
		private DebugUI.UIntField m_Field;
	}
}
