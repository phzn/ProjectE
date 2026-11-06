using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E6 RID: 486
	public class MouseUpEvent : MouseEventBase<MouseUpEvent>
	{
		// Token: 0x06000EB0 RID: 3760 RVA: 0x00037ECB File Offset: 0x000360CB
		static MouseUpEvent()
		{
			EventBase<MouseUpEvent>.SetCreateFunction(() => new MouseUpEvent());
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00037EE4 File Offset: 0x000360E4
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00037DF2 File Offset: 0x00035FF2
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00037EF5 File Offset: 0x000360F5
		public MouseUpEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00037F08 File Offset: 0x00036108
		public new static MouseUpEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.ReleaseButton(PointerId.mousePointerId, systemEvent.button);
			}
			return MouseEventBase<MouseUpEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00037F3C File Offset: 0x0003613C
		private static MouseUpEvent MakeFromPointerEvent(IPointerEvent pointerEvent)
		{
			bool flag = pointerEvent != null && pointerEvent.button >= 0;
			if (flag)
			{
				PointerDeviceState.ReleaseButton(PointerId.mousePointerId, pointerEvent.button);
			}
			return MouseEventBase<MouseUpEvent>.GetPooled(pointerEvent);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00037F80 File Offset: 0x00036180
		internal static MouseUpEvent GetPooled(PointerUpEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00037F98 File Offset: 0x00036198
		internal static MouseUpEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00037FB0 File Offset: 0x000361B0
		internal static MouseUpEvent GetPooled(PointerCancelEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}
	}
}
