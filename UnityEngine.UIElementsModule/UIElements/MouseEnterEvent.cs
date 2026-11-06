using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001EE RID: 494
	[EventCategory(EventCategory.EnterLeave)]
	public class MouseEnterEvent : MouseEventBase<MouseEnterEvent>
	{
		// Token: 0x06000ED6 RID: 3798 RVA: 0x000381BB File Offset: 0x000363BB
		static MouseEnterEvent()
		{
			EventBase<MouseEnterEvent>.SetCreateFunction(() => new MouseEnterEvent());
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000381D4 File Offset: 0x000363D4
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000381E5 File Offset: 0x000363E5
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000381F1 File Offset: 0x000363F1
		public MouseEnterEvent()
		{
			this.LocalInit();
		}
	}
}
