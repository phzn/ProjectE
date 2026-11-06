using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F2 RID: 498
	[EventCategory(EventCategory.EnterLeaveWindow)]
	public class MouseEnterWindowEvent : MouseEventBase<MouseEnterWindowEvent>
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x00038263 File Offset: 0x00036463
		static MouseEnterWindowEvent()
		{
			EventBase<MouseEnterWindowEvent>.SetCreateFunction(() => new MouseEnterWindowEvent());
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0003827C File Offset: 0x0003647C
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0003828D File Offset: 0x0003648D
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Cancellable;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00038298 File Offset: 0x00036498
		public MouseEnterWindowEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x000382AC File Offset: 0x000364AC
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
