using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F4 RID: 500
	[EventCategory(EventCategory.EnterLeaveWindow)]
	public class MouseLeaveWindowEvent : MouseEventBase<MouseLeaveWindowEvent>
	{
		// Token: 0x06000EEC RID: 3820 RVA: 0x000382FD File Offset: 0x000364FD
		static MouseLeaveWindowEvent()
		{
			EventBase<MouseLeaveWindowEvent>.SetCreateFunction(() => new MouseLeaveWindowEvent());
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00038316 File Offset: 0x00036516
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00038327 File Offset: 0x00036527
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Cancellable;
			((IMouseEventInternal)this).recomputeTopElementUnderMouse = false;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003833A File Offset: 0x0003653A
		public MouseLeaveWindowEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003834C File Offset: 0x0003654C
		public new static MouseLeaveWindowEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.ReleaseAllButtons(PointerId.mousePointerId);
			}
			return MouseEventBase<MouseLeaveWindowEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00038378 File Offset: 0x00036578
		protected internal override void PostDispatch(IPanel panel)
		{
			EventBase eventBase = ((IMouseEventInternal)this).sourcePointerEvent as EventBase;
			bool flag = eventBase == null;
			if (flag)
			{
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.CommitElementUnderPointers();
				}
			}
			base.PostDispatch(panel);
		}
	}
}
