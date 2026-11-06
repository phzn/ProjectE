using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004B9 RID: 1209
	public sealed class PointerDownLinkTagEvent : PointerEventBase<PointerDownLinkTagEvent>
	{
		// Token: 0x06002587 RID: 9607 RVA: 0x0009E84E File Offset: 0x0009CA4E
		static PointerDownLinkTagEvent()
		{
			EventBase<PointerDownLinkTagEvent>.SetCreateFunction(() => new PointerDownLinkTagEvent());
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x0009E867 File Offset: 0x0009CA67
		// (set) Token: 0x06002589 RID: 9609 RVA: 0x0009E86F File Offset: 0x0009CA6F
		public string linkID { get; private set; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x0009E878 File Offset: 0x0009CA78
		// (set) Token: 0x0600258B RID: 9611 RVA: 0x0009E880 File Offset: 0x0009CA80
		public string linkText { get; private set; }

		// Token: 0x0600258C RID: 9612 RVA: 0x0009E889 File Offset: 0x0009CA89
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0009E89C File Offset: 0x0009CA9C
		public static PointerDownLinkTagEvent GetPooled(IPointerEvent evt, string linkID, string linkText)
		{
			PointerDownLinkTagEvent pooled = PointerEventBase<PointerDownLinkTagEvent>.GetPooled(evt);
			pooled.linkID = linkID;
			pooled.linkText = linkText;
			return pooled;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0009E8C6 File Offset: 0x0009CAC6
		public PointerDownLinkTagEvent()
		{
			this.LocalInit();
		}
	}
}
