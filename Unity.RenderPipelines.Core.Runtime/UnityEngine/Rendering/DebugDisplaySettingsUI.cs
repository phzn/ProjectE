using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005E RID: 94
	public class DebugDisplaySettingsUI : IDebugData
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000CEAD File Offset: 0x0000B0AD
		private void Reset()
		{
			if (this.m_Settings != null)
			{
				this.m_Settings.Reset();
				this.UnregisterDebug();
				this.RegisterDebug(this.m_Settings);
				DebugManager.instance.RefreshEditor();
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
		public void RegisterDebug(IDebugDisplaySettings settings)
		{
			DebugManager debugManager = DebugManager.instance;
			List<IDebugDisplaySettingsPanelDisposable> panels = new List<IDebugDisplaySettingsPanelDisposable>();
			debugManager.RegisterData(this);
			this.m_Settings = settings;
			this.m_DisposablePanels = panels;
			Action<IDebugDisplaySettingsData> action = delegate(IDebugDisplaySettingsData data)
			{
				IDebugDisplaySettingsPanelDisposable debugDisplaySettingsPanelDisposable = data.CreatePanel();
				DebugUI.Widget[] widgets = debugDisplaySettingsPanelDisposable.Widgets;
				DebugManager debugManager2 = debugManager;
				string panelName = debugDisplaySettingsPanelDisposable.PanelName;
				bool flag = true;
				DebugDisplaySettingsPanel debugDisplaySettingsPanel = debugDisplaySettingsPanelDisposable as DebugDisplaySettingsPanel;
				DebugUI.Panel panel = debugManager2.GetPanel(panelName, flag, (debugDisplaySettingsPanel != null) ? debugDisplaySettingsPanel.Order : 0, false);
				ObservableList<DebugUI.Widget> children = panel.children;
				panel.flags = debugDisplaySettingsPanelDisposable.Flags;
				panels.Add(debugDisplaySettingsPanelDisposable);
				children.Add(widgets);
			};
			this.m_Settings.ForEach(action);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000CF44 File Offset: 0x0000B144
		public void UnregisterDebug()
		{
			DebugManager instance = DebugManager.instance;
			foreach (IDebugDisplaySettingsPanelDisposable debugDisplaySettingsPanelDisposable in this.m_DisposablePanels)
			{
				DebugUI.Widget[] widgets = debugDisplaySettingsPanelDisposable.Widgets;
				string panelName = debugDisplaySettingsPanelDisposable.PanelName;
				ObservableList<DebugUI.Widget> children = instance.GetPanel(panelName, true, 0, false).children;
				debugDisplaySettingsPanelDisposable.Dispose();
				children.Remove(widgets);
			}
			this.m_DisposablePanels = null;
			instance.UnregisterData(this);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		public Action GetReset()
		{
			return new Action(this.Reset);
		}

		// Token: 0x040001A8 RID: 424
		private IEnumerable<IDebugDisplaySettingsPanelDisposable> m_DisposablePanels;

		// Token: 0x040001A9 RID: 425
		private IDebugDisplaySettings m_Settings;
	}
}
