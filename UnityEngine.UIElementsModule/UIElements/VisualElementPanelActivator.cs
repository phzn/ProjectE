using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200040B RID: 1035
	internal class VisualElementPanelActivator
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x0007D274 File Offset: 0x0007B474
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x0007D27C File Offset: 0x0007B47C
		public bool isActive { get; private set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x0007D285 File Offset: 0x0007B485
		// (set) Token: 0x06002114 RID: 8468 RVA: 0x0007D28D File Offset: 0x0007B48D
		public bool isDetaching { get; private set; }

		// Token: 0x06002115 RID: 8469 RVA: 0x0007D296 File Offset: 0x0007B496
		public VisualElementPanelActivator(IVisualElementPanelActivatable activatable)
		{
			this.m_Activatable = activatable;
			this.m_OnAttachToPanelCallback = new EventCallback<AttachToPanelEvent>(this.OnEnter);
			this.m_OnDetachFromPanelCallback = new EventCallback<DetachFromPanelEvent>(this.OnLeave);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0007D2CC File Offset: 0x0007B4CC
		public void SetActive(bool action)
		{
			bool flag = this.isActive != action;
			if (flag)
			{
				this.isActive = action;
				bool isActive = this.isActive;
				if (isActive)
				{
					this.m_Activatable.element.RegisterCallback<AttachToPanelEvent>(this.m_OnAttachToPanelCallback, TrickleDown.NoTrickleDown);
					this.m_Activatable.element.RegisterCallback<DetachFromPanelEvent>(this.m_OnDetachFromPanelCallback, TrickleDown.NoTrickleDown);
					this.SendActivation();
				}
				else
				{
					this.m_Activatable.element.UnregisterCallback<AttachToPanelEvent>(this.m_OnAttachToPanelCallback, TrickleDown.NoTrickleDown);
					this.m_Activatable.element.UnregisterCallback<DetachFromPanelEvent>(this.m_OnDetachFromPanelCallback, TrickleDown.NoTrickleDown);
					this.SendDeactivation();
				}
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0007D378 File Offset: 0x0007B578
		public void SendActivation()
		{
			bool flag = this.m_Activatable.CanBeActivated();
			if (flag)
			{
				this.m_Activatable.OnPanelActivate();
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0007D3A4 File Offset: 0x0007B5A4
		public void SendDeactivation()
		{
			bool flag = this.m_Activatable.CanBeActivated();
			if (flag)
			{
				this.m_Activatable.OnPanelDeactivate();
			}
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0007D3D0 File Offset: 0x0007B5D0
		private void OnEnter(AttachToPanelEvent evt)
		{
			bool isActive = this.isActive;
			if (isActive)
			{
				this.SendActivation();
			}
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0007D3F4 File Offset: 0x0007B5F4
		private void OnLeave(DetachFromPanelEvent evt)
		{
			bool isActive = this.isActive;
			if (isActive)
			{
				this.isDetaching = true;
				try
				{
					this.SendDeactivation();
				}
				finally
				{
					this.isDetaching = false;
				}
			}
		}

		// Token: 0x04000DFB RID: 3579
		private IVisualElementPanelActivatable m_Activatable;

		// Token: 0x04000DFE RID: 3582
		private EventCallback<AttachToPanelEvent> m_OnAttachToPanelCallback;

		// Token: 0x04000DFF RID: 3583
		private EventCallback<DetachFromPanelEvent> m_OnDetachFromPanelCallback;
	}
}
