using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200018A RID: 394
	[EventCategory(EventCategory.Pointer)]
	public abstract class PointerCaptureEventBase<T> : EventBase<T>, IPointerCaptureEvent, IPointerCaptureEventInternal where T : PointerCaptureEventBase<T>, new()
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00031CA4 File Offset: 0x0002FEA4
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x00031CAC File Offset: 0x0002FEAC
		public IEventHandler relatedTarget { get; private set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00031CB5 File Offset: 0x0002FEB5
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x00031CBD File Offset: 0x0002FEBD
		public int pointerId { get; private set; }

		// Token: 0x06000C66 RID: 3174 RVA: 0x00031CC6 File Offset: 0x0002FEC6
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00031CD7 File Offset: 0x0002FED7
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.relatedTarget = null;
			this.pointerId = PointerId.invalidPointerId;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00031CF8 File Offset: 0x0002FEF8
		public static T GetPooled(IEventHandler target, IEventHandler relatedTarget, int pointerId)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.target = target;
			pooled.relatedTarget = relatedTarget;
			pooled.pointerId = pointerId;
			return pooled;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00031D38 File Offset: 0x0002FF38
		protected PointerCaptureEventBase()
		{
			this.LocalInit();
		}
	}
}
