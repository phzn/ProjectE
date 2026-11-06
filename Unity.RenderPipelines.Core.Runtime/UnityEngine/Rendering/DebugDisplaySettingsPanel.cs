using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005C RID: 92
	public abstract class DebugDisplaySettingsPanel : IDebugDisplaySettingsPanelDisposable, IDebugDisplaySettingsPanel, IDisposable
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000CDCB File Offset: 0x0000AFCB
		public virtual string PanelName
		{
			get
			{
				DisplayInfoAttribute displayInfo = this.m_DisplayInfo;
				return ((displayInfo != null) ? displayInfo.name : null) ?? string.Empty;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		public virtual int Order
		{
			get
			{
				DisplayInfoAttribute displayInfo = this.m_DisplayInfo;
				if (displayInfo == null)
				{
					return 0;
				}
				return displayInfo.order;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000CDFB File Offset: 0x0000AFFB
		public DebugUI.Widget[] Widgets
		{
			get
			{
				return this.m_Widgets.ToArray();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000CE08 File Offset: 0x0000B008
		public virtual DebugUI.Flags Flags
		{
			get
			{
				return DebugUI.Flags.None;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000CE0B File Offset: 0x0000B00B
		protected void AddWidget(DebugUI.Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}
			this.m_Widgets.Add(widget);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000CE27 File Offset: 0x0000B027
		protected void Clear()
		{
			this.m_Widgets.Clear();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000CE34 File Offset: 0x0000B034
		public void Dispose()
		{
			this.Clear();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000CE3C File Offset: 0x0000B03C
		protected DebugDisplaySettingsPanel()
		{
			this.m_DisplayInfo = base.GetType().GetCustomAttribute<DisplayInfoAttribute>();
			if (this.m_DisplayInfo == null)
			{
				Debug.Log(string.Format("Type {0} should specify the attribute {1}", base.GetType(), "DisplayInfoAttribute"));
			}
		}

		// Token: 0x040001A5 RID: 421
		private readonly List<DebugUI.Widget> m_Widgets = new List<DebugUI.Widget>();

		// Token: 0x040001A6 RID: 422
		private readonly DisplayInfoAttribute m_DisplayInfo;
	}
}
