using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C5 RID: 453
	[EventCategory(EventCategory.Focus)]
	public abstract class FocusEventBase<T> : EventBase<T>, IFocusEvent where T : FocusEventBase<T>, new()
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00036224 File Offset: 0x00034424
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x0003622C File Offset: 0x0003442C
		public Focusable relatedTarget { get; private set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00036235 File Offset: 0x00034435
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x0003623D File Offset: 0x0003443D
		public FocusChangeDirection direction { get; private set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00036246 File Offset: 0x00034446
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x0003624E File Offset: 0x0003444E
		private protected FocusController focusController { protected get; private set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00036257 File Offset: 0x00034457
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x0003625F File Offset: 0x0003445F
		internal bool IsFocusDelegated { get; private set; }

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00036268 File Offset: 0x00034468
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00036279 File Offset: 0x00034479
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown;
			this.relatedTarget = null;
			this.direction = FocusChangeDirection.unspecified;
			this.focusController = null;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000362A0 File Offset: 0x000344A0
		public static T GetPooled(IEventHandler target, Focusable relatedTarget, FocusChangeDirection direction, FocusController focusController, bool bIsFocusDelegated = false)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.target = target;
			pooled.relatedTarget = relatedTarget;
			pooled.direction = direction;
			pooled.focusController = focusController;
			pooled.IsFocusDelegated = bIsFocusDelegated;
			return pooled;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000362FB File Offset: 0x000344FB
		protected FocusEventBase()
		{
			this.LocalInit();
		}
	}
}
