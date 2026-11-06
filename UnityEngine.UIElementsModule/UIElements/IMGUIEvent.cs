using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000241 RID: 577
	[EventCategory(EventCategory.IMGUI)]
	public class IMGUIEvent : EventBase<IMGUIEvent>
	{
		// Token: 0x06001066 RID: 4198 RVA: 0x0003B968 File Offset: 0x00039B68
		static IMGUIEvent()
		{
			EventBase<IMGUIEvent>.SetCreateFunction(() => new IMGUIEvent());
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0003B984 File Offset: 0x00039B84
		public static IMGUIEvent GetPooled(Event systemEvent)
		{
			IMGUIEvent pooled = EventBase<IMGUIEvent>.GetPooled();
			pooled.imguiEvent = systemEvent;
			return pooled;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0003B9A5 File Offset: 0x00039BA5
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00038005 File Offset: 0x00036205
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0003B9B6 File Offset: 0x00039BB6
		public IMGUIEvent()
		{
			this.LocalInit();
		}
	}
}
