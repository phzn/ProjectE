using System;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E0 RID: 480
	internal class MouseEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000E64 RID: 3684 RVA: 0x00037430 File Offset: 0x00035630
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is IMouseEvent;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0003744C File Offset: 0x0003564C
		public void DispatchEvent(EventBase evt, IPanel iPanel)
		{
			bool flag = iPanel != null;
			if (flag)
			{
				Assert.IsTrue(iPanel is BaseVisualElementPanel);
				BaseVisualElementPanel baseVisualElementPanel = (BaseVisualElementPanel)iPanel;
				MouseEventDispatchingStrategy.SetBestTargetForEvent(evt, baseVisualElementPanel);
				MouseEventDispatchingStrategy.SendEventToTarget(evt, baseVisualElementPanel);
			}
			evt.stopDispatch = true;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00037494 File Offset: 0x00035694
		private static bool SendEventToTarget(EventBase evt, BaseVisualElementPanel panel)
		{
			return MouseEventDispatchingStrategy.SendEventToRegularTarget(evt, panel) || MouseEventDispatchingStrategy.SendEventToIMGUIContainer(evt, panel);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000374BC File Offset: 0x000356BC
		private static bool SendEventToRegularTarget(EventBase evt, BaseVisualElementPanel panel)
		{
			VisualElement visualElement = evt.target as VisualElement;
			bool flag = visualElement == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = visualElement.panel == panel;
				if (flag3)
				{
					EventDispatchUtilities.PropagateEvent(evt);
				}
				flag2 = MouseEventDispatchingStrategy.IsDone(evt);
			}
			return flag2;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00037504 File Offset: 0x00035704
		private static bool SendEventToIMGUIContainer(EventBase evt, BaseVisualElementPanel panel)
		{
			bool flag = evt.imguiEvent == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				IMGUIContainer rootIMGUIContainer = panel.rootIMGUIContainer;
				bool flag3 = rootIMGUIContainer == null;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					bool flag4 = evt.propagateToIMGUI || evt.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId();
					if (flag4)
					{
						evt.skipElements.Add(evt.target);
						EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
					}
					flag2 = MouseEventDispatchingStrategy.IsDone(evt);
				}
			}
			return flag2;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0003758C File Offset: 0x0003578C
		private static void SetBestTargetForEvent(EventBase evt, BaseVisualElementPanel panel)
		{
			VisualElement visualElement;
			MouseEventDispatchingStrategy.UpdateElementUnderMouse(evt, panel, out visualElement);
			bool flag = evt.target != null;
			if (flag)
			{
				evt.propagateToIMGUI = false;
			}
			else
			{
				bool flag2 = visualElement != null;
				if (flag2)
				{
					evt.propagateToIMGUI = false;
					evt.target = visualElement;
				}
				else
				{
					evt.target = ((panel != null) ? panel.visualTree : null);
				}
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000375F0 File Offset: 0x000357F0
		private static void UpdateElementUnderMouse(EventBase evt, BaseVisualElementPanel panel, out VisualElement elementUnderMouse)
		{
			IMouseEventInternal mouseEventInternal = evt as IMouseEventInternal;
			elementUnderMouse = ((mouseEventInternal == null || mouseEventInternal.recomputeTopElementUnderMouse) ? panel.RecomputeTopElementUnderPointer(PointerId.mousePointerId, ((IMouseEvent)evt).mousePosition, evt) : panel.GetTopElementUnderPointer(PointerId.mousePointerId));
			bool flag = evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId() && (evt as MouseLeaveWindowEvent).pressedButtons == 0;
			if (flag)
			{
				panel.ClearCachedElementUnderPointer(PointerId.mousePointerId, evt);
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0003766C File Offset: 0x0003586C
		private static bool IsDone(EventBase evt)
		{
			Event imguiEvent = evt.imguiEvent;
			bool flag = imguiEvent != null && imguiEvent.rawType == EventType.Used;
			if (flag)
			{
				evt.StopPropagation();
			}
			return evt.isPropagationStopped;
		}
	}
}
