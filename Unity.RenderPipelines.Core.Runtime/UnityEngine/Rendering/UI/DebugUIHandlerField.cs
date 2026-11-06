using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000125 RID: 293
	public abstract class DebugUIHandlerField<T> : DebugUIHandlerWidget where T : DebugUI.Widget
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x000293B0 File Offset: 0x000275B0
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<T>();
			this.nameLabel.text = this.m_Field.displayName;
			this.UpdateValueLabel();
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000293E8 File Offset: 0x000275E8
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			if (this.nextButtonText != null)
			{
				this.nextButtonText.color = this.colorSelected;
			}
			if (this.previousButtonText != null)
			{
				this.previousButtonText.color = this.colorSelected;
			}
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00029458 File Offset: 0x00027658
		public override void OnDeselection()
		{
			if (this.nextButtonText != null)
			{
				this.nextButtonText.color = this.colorDefault;
			}
			if (this.previousButtonText != null)
			{
				this.previousButtonText.color = this.colorDefault;
			}
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000294C5 File Offset: 0x000276C5
		public override void OnAction()
		{
			this.OnIncrement(false);
		}

		// Token: 0x060008CA RID: 2250
		public abstract void UpdateValueLabel();

		// Token: 0x060008CB RID: 2251 RVA: 0x000294CE File Offset: 0x000276CE
		protected void SetLabelText(string text)
		{
			if (text.Length > 26)
			{
				text = text.Substring(0, 23) + "...";
			}
			this.valueLabel.text = text;
		}

		// Token: 0x0400051D RID: 1309
		public Text nextButtonText;

		// Token: 0x0400051E RID: 1310
		public Text previousButtonText;

		// Token: 0x0400051F RID: 1311
		public Text nameLabel;

		// Token: 0x04000520 RID: 1312
		public Text valueLabel;

		// Token: 0x04000521 RID: 1313
		protected internal T m_Field;
	}
}
