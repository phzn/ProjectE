using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E4 RID: 484
	public class MouseDownEvent : MouseEventBase<MouseDownEvent>
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x00037DC8 File Offset: 0x00035FC8
		static MouseDownEvent()
		{
			EventBase<MouseDownEvent>.SetCreateFunction(() => new MouseDownEvent());
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00037DE1 File Offset: 0x00035FE1
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00037DF2 File Offset: 0x00035FF2
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00037DFE File Offset: 0x00035FFE
		public MouseDownEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00037E10 File Offset: 0x00036010
		public new static MouseDownEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
			}
			return MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00037E44 File Offset: 0x00036044
		private static MouseDownEvent MakeFromPointerEvent(IPointerEvent pointerEvent)
		{
			bool flag = pointerEvent != null && pointerEvent.button >= 0;
			if (flag)
			{
				PointerDeviceState.PressButton(PointerId.mousePointerId, pointerEvent.button);
			}
			return MouseEventBase<MouseDownEvent>.GetPooled(pointerEvent);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00037E88 File Offset: 0x00036088
		internal static MouseDownEvent GetPooled(PointerDownEvent pointerEvent)
		{
			return MouseDownEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00037EA0 File Offset: 0x000360A0
		internal static MouseDownEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseDownEvent.MakeFromPointerEvent(pointerEvent);
		}
	}
}
