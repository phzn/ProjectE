using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E8 RID: 488
	[EventCategory(EventCategory.PointerMove)]
	public class MouseMoveEvent : MouseEventBase<MouseMoveEvent>
	{
		// Token: 0x06000EBC RID: 3772 RVA: 0x00037FDB File Offset: 0x000361DB
		static MouseMoveEvent()
		{
			EventBase<MouseMoveEvent>.SetCreateFunction(() => new MouseMoveEvent());
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00037FF4 File Offset: 0x000361F4
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00038005 File Offset: 0x00036205
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00038010 File Offset: 0x00036210
		public MouseMoveEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00038024 File Offset: 0x00036224
		public new static MouseMoveEvent GetPooled(Event systemEvent)
		{
			MouseMoveEvent pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
			pooled.button = 0;
			return pooled;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00038048 File Offset: 0x00036248
		internal static MouseMoveEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseEventBase<MouseMoveEvent>.GetPooled(pointerEvent);
		}
	}
}
