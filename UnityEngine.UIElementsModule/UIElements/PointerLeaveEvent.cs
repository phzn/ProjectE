using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000228 RID: 552
	[EventCategory(EventCategory.EnterLeave)]
	public sealed class PointerLeaveEvent : PointerEventBase<PointerLeaveEvent>
	{
		// Token: 0x06001011 RID: 4113 RVA: 0x0003B29B File Offset: 0x0003949B
		static PointerLeaveEvent()
		{
			EventBase<PointerLeaveEvent>.SetCreateFunction(() => new PointerLeaveEvent());
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003B2B4 File Offset: 0x000394B4
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003B26B File Offset: 0x0003946B
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003B2C5 File Offset: 0x000394C5
		public PointerLeaveEvent()
		{
			this.LocalInit();
		}
	}
}
