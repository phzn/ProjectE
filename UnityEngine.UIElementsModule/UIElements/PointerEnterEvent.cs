using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000226 RID: 550
	[EventCategory(EventCategory.EnterLeave)]
	public sealed class PointerEnterEvent : PointerEventBase<PointerEnterEvent>
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x0003B241 File Offset: 0x00039441
		static PointerEnterEvent()
		{
			EventBase<PointerEnterEvent>.SetCreateFunction(() => new PointerEnterEvent());
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003B25A File Offset: 0x0003945A
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003B26B File Offset: 0x0003946B
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003B277 File Offset: 0x00039477
		public PointerEnterEvent()
		{
			this.LocalInit();
		}
	}
}
