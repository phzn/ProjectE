using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001DC RID: 476
	[EventCategory(EventCategory.Geometry)]
	public class GeometryChangedEvent : EventBase<GeometryChangedEvent>
	{
		// Token: 0x06000E53 RID: 3667 RVA: 0x0003710D File Offset: 0x0003530D
		static GeometryChangedEvent()
		{
			EventBase<GeometryChangedEvent>.SetCreateFunction(() => new GeometryChangedEvent());
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00037128 File Offset: 0x00035328
		public static GeometryChangedEvent GetPooled(Rect oldRect, Rect newRect)
		{
			GeometryChangedEvent pooled = EventBase<GeometryChangedEvent>.GetPooled();
			pooled.oldRect = oldRect;
			pooled.newRect = newRect;
			return pooled;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00037151 File Offset: 0x00035351
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00037162 File Offset: 0x00035362
		private void LocalInit()
		{
			this.oldRect = Rect.zero;
			this.newRect = Rect.zero;
			this.layoutPass = 0;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00037185 File Offset: 0x00035385
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0003718D File Offset: 0x0003538D
		public Rect oldRect { get; private set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00037196 File Offset: 0x00035396
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0003719E File Offset: 0x0003539E
		public Rect newRect { get; private set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x000371A7 File Offset: 0x000353A7
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x000371AF File Offset: 0x000353AF
		internal int layoutPass { get; set; }

		// Token: 0x06000E5D RID: 3677 RVA: 0x000371B8 File Offset: 0x000353B8
		public GeometryChangedEvent()
		{
			this.LocalInit();
		}
	}
}
