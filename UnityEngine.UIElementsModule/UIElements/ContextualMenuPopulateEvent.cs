using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001FA RID: 506
	public class ContextualMenuPopulateEvent : MouseEventBase<ContextualMenuPopulateEvent>
	{
		// Token: 0x06000EFF RID: 3839 RVA: 0x00038433 File Offset: 0x00036633
		static ContextualMenuPopulateEvent()
		{
			EventBase<ContextualMenuPopulateEvent>.SetCreateFunction(() => new ContextualMenuPopulateEvent());
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0003844C File Offset: 0x0003664C
		// (set) Token: 0x06000F01 RID: 3841 RVA: 0x00038454 File Offset: 0x00036654
		public DropdownMenu menu { get; private set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0003845D File Offset: 0x0003665D
		// (set) Token: 0x06000F03 RID: 3843 RVA: 0x00038465 File Offset: 0x00036665
		public EventBase triggerEvent { get; private set; }

		// Token: 0x06000F04 RID: 3844 RVA: 0x00038470 File Offset: 0x00036670
		public static ContextualMenuPopulateEvent GetPooled(EventBase triggerEvent, DropdownMenu menu, IEventHandler target, ContextualMenuManager menuManager)
		{
			ContextualMenuPopulateEvent pooled = EventBase<ContextualMenuPopulateEvent>.GetPooled(triggerEvent);
			bool flag = triggerEvent != null;
			if (flag)
			{
				triggerEvent.Acquire();
				pooled.triggerEvent = triggerEvent;
				IMouseEvent mouseEvent = triggerEvent as IMouseEvent;
				bool flag2 = mouseEvent != null;
				if (flag2)
				{
					pooled.modifiers = mouseEvent.modifiers;
					pooled.mousePosition = mouseEvent.mousePosition;
					pooled.localMousePosition = mouseEvent.mousePosition;
					pooled.mouseDelta = mouseEvent.mouseDelta;
					pooled.button = mouseEvent.button;
					pooled.clickCount = mouseEvent.clickCount;
				}
				else
				{
					IPointerEvent pointerEvent = triggerEvent as IPointerEvent;
					bool flag3 = pointerEvent != null;
					if (flag3)
					{
						pooled.modifiers = pointerEvent.modifiers;
						pooled.mousePosition = pointerEvent.position;
						pooled.localMousePosition = pointerEvent.position;
						pooled.mouseDelta = pointerEvent.deltaPosition;
						pooled.button = pointerEvent.button;
						pooled.clickCount = pointerEvent.clickCount;
					}
				}
				IMouseEventInternal mouseEventInternal = triggerEvent as IMouseEventInternal;
				bool flag4 = mouseEventInternal != null;
				if (flag4)
				{
					((IMouseEventInternal)pooled).triggeredByOS = mouseEventInternal.triggeredByOS;
				}
				else
				{
					IPointerEventInternal pointerEventInternal = triggerEvent as IPointerEventInternal;
					bool flag5 = pointerEventInternal != null;
					if (flag5)
					{
						((IMouseEventInternal)pooled).triggeredByOS = pointerEventInternal.triggeredByOS;
					}
				}
			}
			pooled.target = target;
			pooled.menu = menu;
			pooled.m_ContextualMenuManager = menuManager;
			return pooled;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000385E3 File Offset: 0x000367E3
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000385F4 File Offset: 0x000367F4
		private void LocalInit()
		{
			this.menu = null;
			this.m_ContextualMenuManager = null;
			bool flag = this.triggerEvent != null;
			if (flag)
			{
				this.triggerEvent.Dispose();
				this.triggerEvent = null;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00038634 File Offset: 0x00036834
		public ContextualMenuPopulateEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00038648 File Offset: 0x00036848
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = !base.isDefaultPrevented && this.m_ContextualMenuManager != null;
			if (flag)
			{
				this.menu.PrepareForDisplay(this.triggerEvent);
				this.m_ContextualMenuManager.DoDisplayMenu(this.menu, this.triggerEvent);
			}
			base.PostDispatch(panel);
		}

		// Token: 0x040006D4 RID: 1748
		private ContextualMenuManager m_ContextualMenuManager;
	}
}
