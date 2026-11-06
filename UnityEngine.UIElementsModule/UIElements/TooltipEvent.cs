using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000233 RID: 563
	[EventCategory(EventCategory.Tooltip)]
	public class TooltipEvent : EventBase<TooltipEvent>
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x0003B5CD File Offset: 0x000397CD
		static TooltipEvent()
		{
			EventBase<TooltipEvent>.SetCreateFunction(() => new TooltipEvent());
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0003B5E6 File Offset: 0x000397E6
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x0003B5EE File Offset: 0x000397EE
		public string tooltip { get; set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003B5F7 File Offset: 0x000397F7
		// (set) Token: 0x06001035 RID: 4149 RVA: 0x0003B5FF File Offset: 0x000397FF
		public Rect rect { get; set; }

		// Token: 0x06001036 RID: 4150 RVA: 0x0003B608 File Offset: 0x00039808
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003B61C File Offset: 0x0003981C
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.rect = default(Rect);
			this.tooltip = string.Empty;
			base.ignoreCompositeRoots = true;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003B658 File Offset: 0x00039858
		internal static TooltipEvent GetPooled(string tooltip, Rect rect)
		{
			TooltipEvent pooled = EventBase<TooltipEvent>.GetPooled();
			pooled.tooltip = tooltip;
			pooled.rect = rect;
			return pooled;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003B681 File Offset: 0x00039881
		public TooltipEvent()
		{
			this.LocalInit();
		}
	}
}
