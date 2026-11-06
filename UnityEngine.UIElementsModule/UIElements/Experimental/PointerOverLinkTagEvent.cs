using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004B3 RID: 1203
	[EventCategory(EventCategory.EnterLeave)]
	public class PointerOverLinkTagEvent : PointerEventBase<PointerOverLinkTagEvent>
	{
		// Token: 0x06002567 RID: 9575 RVA: 0x0009E6AD File Offset: 0x0009C8AD
		static PointerOverLinkTagEvent()
		{
			EventBase<PointerOverLinkTagEvent>.SetCreateFunction(() => new PointerOverLinkTagEvent());
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x0009E6C6 File Offset: 0x0009C8C6
		// (set) Token: 0x06002569 RID: 9577 RVA: 0x0009E6CE File Offset: 0x0009C8CE
		public string linkID { get; private set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600256A RID: 9578 RVA: 0x0009E6D7 File Offset: 0x0009C8D7
		// (set) Token: 0x0600256B RID: 9579 RVA: 0x0009E6DF File Offset: 0x0009C8DF
		public string linkText { get; private set; }

		// Token: 0x0600256C RID: 9580 RVA: 0x0009E6E8 File Offset: 0x0009C8E8
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x0009E6FC File Offset: 0x0009C8FC
		public static PointerOverLinkTagEvent GetPooled(IPointerEvent evt, string linkID, string linkText)
		{
			PointerOverLinkTagEvent pooled = PointerEventBase<PointerOverLinkTagEvent>.GetPooled(evt);
			pooled.linkID = linkID;
			pooled.linkText = linkText;
			return pooled;
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x0009E726 File Offset: 0x0009C926
		public PointerOverLinkTagEvent()
		{
			this.LocalInit();
		}
	}
}
