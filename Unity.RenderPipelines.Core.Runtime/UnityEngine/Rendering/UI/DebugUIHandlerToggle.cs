using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000135 RID: 309
	public class DebugUIHandlerToggle : DebugUIHandlerWidget
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x0002A8A8 File Offset: 0x00028AA8
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.BoolField>();
			this.nameLabel.text = this.m_Field.displayName;
			this.UpdateValueLabel();
			this.valueToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002A900 File Offset: 0x00028B00
		private void OnToggleValueChanged(bool value)
		{
			this.m_Field.SetValue(value);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002A90E File Offset: 0x00028B0E
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.checkmarkImage.color = this.colorSelected;
			return true;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002A933 File Offset: 0x00028B33
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.checkmarkImage.color = this.colorDefault;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002A958 File Offset: 0x00028B58
		public override void OnAction()
		{
			bool flag = !this.m_Field.GetValue();
			this.m_Field.SetValue(flag);
			this.UpdateValueLabel();
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002A986 File Offset: 0x00028B86
		protected internal virtual void UpdateValueLabel()
		{
			if (this.valueToggle != null)
			{
				this.valueToggle.isOn = this.m_Field.GetValue();
			}
		}

		// Token: 0x0400055D RID: 1373
		public Text nameLabel;

		// Token: 0x0400055E RID: 1374
		public Toggle valueToggle;

		// Token: 0x0400055F RID: 1375
		public Image checkmarkImage;

		// Token: 0x04000560 RID: 1376
		protected internal DebugUI.BoolField m_Field;
	}
}
