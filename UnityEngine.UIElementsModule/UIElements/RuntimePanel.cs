using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000254 RID: 596
	internal class RuntimePanel : BaseRuntimePanel, IRuntimePanel
	{
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x0003DDB8 File Offset: 0x0003BFB8
		public PanelSettings panelSettings
		{
			get
			{
				return this.m_PanelSettings;
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0003DDC0 File Offset: 0x0003BFC0
		public static RuntimePanel Create(ScriptableObject ownerObject)
		{
			return new RuntimePanel(ownerObject);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0003DDD8 File Offset: 0x0003BFD8
		private RuntimePanel(ScriptableObject ownerObject)
			: base(ownerObject, RuntimePanel.s_EventDispatcher)
		{
			this.focusController = new FocusController(new NavigateFocusRing(this.visualTree));
			this.m_PanelSettings = ownerObject as PanelSettings;
			base.name = ((this.m_PanelSettings != null) ? this.m_PanelSettings.name : "RuntimePanel");
			this.visualTree.RegisterCallback<FocusEvent, RuntimePanel>(delegate(FocusEvent e, RuntimePanel p)
			{
				p.OnElementFocus(e);
			}, this, TrickleDown.TrickleDown);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0003DE6C File Offset: 0x0003C06C
		public override void Update()
		{
			bool flag = this.m_PanelSettings != null;
			if (flag)
			{
				this.m_PanelSettings.ApplyPanelSettings();
			}
			base.Update();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0003DE9D File Offset: 0x0003C09D
		private void OnElementFocus(FocusEvent evt)
		{
			UIElementsRuntimeUtility.defaultEventSystem.OnFocusEvent(this, evt);
		}

		// Token: 0x0400078F RID: 1935
		internal static readonly EventDispatcher s_EventDispatcher = RuntimeEventDispatcher.Create();

		// Token: 0x04000790 RID: 1936
		private readonly PanelSettings m_PanelSettings;
	}
}
