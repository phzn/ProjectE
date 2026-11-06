using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005B RID: 91
	internal class DebugDisplayStats : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00014AC6 File Offset: 0x00012CC6
		public void UpdateFrameTiming()
		{
			this.m_DebugFrameTiming.UpdateFrameTiming();
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00014AD3 File Offset: 0x00012CD3
		public bool AreAnySettingsActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00014AD6 File Offset: 0x00012CD6
		public bool IsPostProcessingAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00014AD9 File Offset: 0x00012CD9
		public bool IsLightingActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00014ADC File Offset: 0x00012CDC
		public bool TryGetScreenClearColor(ref Color _)
		{
			return false;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00014ADF File Offset: 0x00012CDF
		public IDebugDisplaySettingsPanelDisposable CreatePanel()
		{
			return new DebugDisplayStats.StatsPanel(this.m_DebugFrameTiming);
		}

		// Token: 0x0400027B RID: 635
		private DebugFrameTiming m_DebugFrameTiming = new DebugFrameTiming();

		// Token: 0x02000179 RID: 377
		[DisplayInfo(name = "Display Stats", order = -2147483648)]
		private class StatsPanel : DebugDisplaySettingsPanel
		{
			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000489C8 File Offset: 0x00046BC8
			public override DebugUI.Flags Flags
			{
				get
				{
					return DebugUI.Flags.RuntimeOnly;
				}
			}

			// Token: 0x06000A92 RID: 2706 RVA: 0x000489CC File Offset: 0x00046BCC
			public StatsPanel(DebugFrameTiming frameTiming)
			{
				List<DebugUI.Widget> list = new List<DebugUI.Widget>();
				frameTiming.RegisterDebugUI(list);
				foreach (DebugUI.Widget widget in list)
				{
					base.AddWidget(widget);
				}
			}
		}
	}
}
