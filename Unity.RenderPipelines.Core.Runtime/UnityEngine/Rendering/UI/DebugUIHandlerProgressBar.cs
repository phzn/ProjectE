using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000133 RID: 307
	public class DebugUIHandlerProgressBar : DebugUIHandlerWidget
	{
		// Token: 0x06000921 RID: 2337 RVA: 0x0002A52A File Offset: 0x0002872A
		protected override void OnEnable()
		{
			this.m_Timer = 0f;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002A537 File Offset: 0x00028737
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Value = base.CastWidget<DebugUI.ProgressBarValue>();
			this.nameLabel.text = this.m_Value.displayName;
			this.UpdateValue();
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002A568 File Offset: 0x00028768
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002A57C File Offset: 0x0002877C
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002A590 File Offset: 0x00028790
		private void Update()
		{
			if (this.m_Timer >= this.m_Value.refreshRate)
			{
				this.UpdateValue();
				this.m_Timer -= this.m_Value.refreshRate;
			}
			this.m_Timer += Time.deltaTime;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002A5E0 File Offset: 0x000287E0
		private void UpdateValue()
		{
			float num = (float)this.m_Value.GetValue();
			this.valueLabel.text = this.m_Value.FormatString(num);
			Vector3 localScale = this.progressBarRect.localScale;
			localScale.x = num;
			this.progressBarRect.localScale = localScale;
		}

		// Token: 0x04000557 RID: 1367
		public Text nameLabel;

		// Token: 0x04000558 RID: 1368
		public Text valueLabel;

		// Token: 0x04000559 RID: 1369
		public RectTransform progressBarRect;

		// Token: 0x0400055A RID: 1370
		private DebugUI.ProgressBarValue m_Value;

		// Token: 0x0400055B RID: 1371
		private float m_Timer;
	}
}
