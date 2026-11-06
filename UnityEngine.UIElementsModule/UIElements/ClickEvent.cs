using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000224 RID: 548
	public sealed class ClickEvent : PointerEventBase<ClickEvent>
	{
		// Token: 0x06001002 RID: 4098 RVA: 0x0003B1CF File Offset: 0x000393CF
		static ClickEvent()
		{
			EventBase<ClickEvent>.SetCreateFunction(() => new ClickEvent());
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0003B1E8 File Offset: 0x000393E8
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00037DF2 File Offset: 0x00035FF2
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0003B1F9 File Offset: 0x000393F9
		public ClickEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003B20C File Offset: 0x0003940C
		internal static ClickEvent GetPooled(PointerUpEvent pointerEvent, int clickCount)
		{
			ClickEvent pooled = PointerEventBase<ClickEvent>.GetPooled(pointerEvent);
			pooled.clickCount = clickCount;
			return pooled;
		}
	}
}
