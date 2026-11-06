using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200020A RID: 522
	[EventCategory(EventCategory.ChangePanel)]
	public abstract class PanelChangedEventBase<T> : EventBase<T>, IPanelChangedEvent where T : PanelChangedEventBase<T>, new()
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00039091 File Offset: 0x00037291
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x00039099 File Offset: 0x00037299
		public IPanel originPanel { get; private set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x000390A2 File Offset: 0x000372A2
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x000390AA File Offset: 0x000372AA
		public IPanel destinationPanel { get; private set; }

		// Token: 0x06000F47 RID: 3911 RVA: 0x000390B3 File Offset: 0x000372B3
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x000390C4 File Offset: 0x000372C4
		private void LocalInit()
		{
			this.originPanel = null;
			this.destinationPanel = null;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x000390D8 File Offset: 0x000372D8
		public static T GetPooled(IPanel originPanel, IPanel destinationPanel)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.originPanel = originPanel;
			pooled.destinationPanel = destinationPanel;
			return pooled;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0003910B File Offset: 0x0003730B
		protected PanelChangedEventBase()
		{
			this.LocalInit();
		}
	}
}
