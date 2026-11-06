using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004B5 RID: 1205
	[EventCategory(EventCategory.PointerMove)]
	public class PointerMoveLinkTagEvent : PointerEventBase<PointerMoveLinkTagEvent>
	{
		// Token: 0x06002573 RID: 9587 RVA: 0x0009E74A File Offset: 0x0009C94A
		static PointerMoveLinkTagEvent()
		{
			EventBase<PointerMoveLinkTagEvent>.SetCreateFunction(() => new PointerMoveLinkTagEvent());
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x0009E763 File Offset: 0x0009C963
		// (set) Token: 0x06002575 RID: 9589 RVA: 0x0009E76B File Offset: 0x0009C96B
		public string linkID { get; private set; }

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x0009E774 File Offset: 0x0009C974
		// (set) Token: 0x06002577 RID: 9591 RVA: 0x0009E77C File Offset: 0x0009C97C
		public string linkText { get; private set; }

		// Token: 0x06002578 RID: 9592 RVA: 0x0009E785 File Offset: 0x0009C985
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x0009E798 File Offset: 0x0009C998
		public static PointerMoveLinkTagEvent GetPooled(IPointerEvent evt, string linkID, string linkText)
		{
			PointerMoveLinkTagEvent pooled = PointerEventBase<PointerMoveLinkTagEvent>.GetPooled(evt);
			pooled.linkID = linkID;
			pooled.linkText = linkText;
			return pooled;
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0009E7C2 File Offset: 0x0009C9C2
		public PointerMoveLinkTagEvent()
		{
			this.LocalInit();
		}
	}
}
