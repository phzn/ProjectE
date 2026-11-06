using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001EC RID: 492
	public class WheelEvent : MouseEventBase<WheelEvent>
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x000380A8 File Offset: 0x000362A8
		static WheelEvent()
		{
			EventBase<WheelEvent>.SetCreateFunction(() => new WheelEvent());
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x000380C1 File Offset: 0x000362C1
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x000380C9 File Offset: 0x000362C9
		public Vector3 delta { get; private set; }

		// Token: 0x06000ECD RID: 3789 RVA: 0x000380D4 File Offset: 0x000362D4
		public new static WheelEvent GetPooled(Event systemEvent)
		{
			WheelEvent pooled = MouseEventBase<WheelEvent>.GetPooled(systemEvent);
			pooled.imguiEvent = systemEvent;
			bool flag = systemEvent != null;
			if (flag)
			{
				pooled.delta = systemEvent.delta;
			}
			return pooled;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00038114 File Offset: 0x00036314
		internal static WheelEvent GetPooled(Vector3 delta, Vector3 mousePosition, EventModifiers modifiers = EventModifiers.None)
		{
			WheelEvent pooled = EventBase<WheelEvent>.GetPooled();
			pooled.delta = delta;
			pooled.mousePosition = mousePosition;
			pooled.modifiers = modifiers;
			return pooled;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0003814C File Offset: 0x0003634C
		internal static WheelEvent GetPooled(Vector3 delta, IPointerEvent pointerEvent)
		{
			WheelEvent pooled = MouseEventBase<WheelEvent>.GetPooled(pointerEvent);
			pooled.delta = delta;
			return pooled;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0003816E File Offset: 0x0003636E
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0003817F File Offset: 0x0003637F
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			this.delta = Vector3.zero;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00038197 File Offset: 0x00036397
		public WheelEvent()
		{
			this.LocalInit();
		}
	}
}
