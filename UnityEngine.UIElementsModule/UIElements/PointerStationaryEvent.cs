using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021E RID: 542
	public sealed class PointerStationaryEvent : PointerEventBase<PointerStationaryEvent>
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0003AF5B File Offset: 0x0003915B
		static PointerStationaryEvent()
		{
			EventBase<PointerStationaryEvent>.SetCreateFunction(() => new PointerStationaryEvent());
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003AF74 File Offset: 0x00039174
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003AF85 File Offset: 0x00039185
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003AFA0 File Offset: 0x000391A0
		public PointerStationaryEvent()
		{
			this.LocalInit();
		}
	}
}
