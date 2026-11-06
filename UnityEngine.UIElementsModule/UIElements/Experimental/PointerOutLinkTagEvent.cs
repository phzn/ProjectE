using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004B7 RID: 1207
	[EventCategory(EventCategory.EnterLeave)]
	public class PointerOutLinkTagEvent : PointerEventBase<PointerOutLinkTagEvent>
	{
		// Token: 0x0600257F RID: 9599 RVA: 0x0009E7E6 File Offset: 0x0009C9E6
		static PointerOutLinkTagEvent()
		{
			EventBase<PointerOutLinkTagEvent>.SetCreateFunction(() => new PointerOutLinkTagEvent());
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x0009E7FF File Offset: 0x0009C9FF
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x0009E810 File Offset: 0x0009CA10
		public static PointerOutLinkTagEvent GetPooled(IPointerEvent evt, string linkID)
		{
			return PointerEventBase<PointerOutLinkTagEvent>.GetPooled(evt);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0009E82A File Offset: 0x0009CA2A
		public PointerOutLinkTagEvent()
		{
			this.LocalInit();
		}
	}
}
