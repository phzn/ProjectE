using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F0 RID: 496
	[EventCategory(EventCategory.EnterLeave)]
	public class MouseLeaveEvent : MouseEventBase<MouseLeaveEvent>
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x00038215 File Offset: 0x00036415
		static MouseLeaveEvent()
		{
			EventBase<MouseLeaveEvent>.SetCreateFunction(() => new MouseLeaveEvent());
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0003822E File Offset: 0x0003642E
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000381E5 File Offset: 0x000363E5
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0003823F File Offset: 0x0003643F
		public MouseLeaveEvent()
		{
			this.LocalInit();
		}
	}
}
