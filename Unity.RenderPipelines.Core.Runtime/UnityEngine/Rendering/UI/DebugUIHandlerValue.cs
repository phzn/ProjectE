using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000138 RID: 312
	public class DebugUIHandlerValue : DebugUIHandlerWidget
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x0002ACA2 File Offset: 0x00028EA2
		protected override void OnEnable()
		{
			this.m_Timer = 0f;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002ACAF File Offset: 0x00028EAF
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Value>();
			this.nameLabel.text = this.m_Field.displayName;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002ACDA File Offset: 0x00028EDA
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002ACFF File Offset: 0x00028EFF
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002AD24 File Offset: 0x00028F24
		private void Update()
		{
			if (this.m_Timer >= this.m_Field.refreshRate)
			{
				object value = this.m_Field.GetValue();
				this.valueLabel.text = this.m_Field.FormatString(value);
				if (value is float)
				{
					this.valueLabel.color = (((float)value == 0f) ? DebugUIHandlerValue.k_ZeroColor : this.colorDefault);
				}
				this.m_Timer -= this.m_Field.refreshRate;
			}
			this.m_Timer += Time.deltaTime;
		}

		// Token: 0x04000566 RID: 1382
		public Text nameLabel;

		// Token: 0x04000567 RID: 1383
		public Text valueLabel;

		// Token: 0x04000568 RID: 1384
		private DebugUI.Value m_Field;

		// Token: 0x04000569 RID: 1385
		protected internal float m_Timer;

		// Token: 0x0400056A RID: 1386
		private static readonly Color k_ZeroColor = Color.gray;
	}
}
