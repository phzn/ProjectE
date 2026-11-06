using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004BB RID: 1211
	public class PointerUpLinkTagEvent : PointerEventBase<PointerUpLinkTagEvent>
	{
		// Token: 0x06002593 RID: 9619 RVA: 0x0009E8EA File Offset: 0x0009CAEA
		static PointerUpLinkTagEvent()
		{
			EventBase<PointerUpLinkTagEvent>.SetCreateFunction(() => new PointerUpLinkTagEvent());
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x0009E903 File Offset: 0x0009CB03
		// (set) Token: 0x06002595 RID: 9621 RVA: 0x0009E90B File Offset: 0x0009CB0B
		public string linkID { get; private set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x0009E914 File Offset: 0x0009CB14
		// (set) Token: 0x06002597 RID: 9623 RVA: 0x0009E91C File Offset: 0x0009CB1C
		public string linkText { get; private set; }

		// Token: 0x06002598 RID: 9624 RVA: 0x0009E925 File Offset: 0x0009CB25
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00036336 File Offset: 0x00034536
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x0009E938 File Offset: 0x0009CB38
		public static PointerUpLinkTagEvent GetPooled(IPointerEvent evt, string linkID, string linkText)
		{
			PointerUpLinkTagEvent pooled = PointerEventBase<PointerUpLinkTagEvent>.GetPooled(evt);
			pooled.linkID = linkID;
			pooled.linkText = linkText;
			return pooled;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x0009E962 File Offset: 0x0009CB62
		public PointerUpLinkTagEvent()
		{
			this.LocalInit();
		}
	}
}
