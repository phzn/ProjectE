using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000213 RID: 531
	internal class PointerEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000F6E RID: 3950 RVA: 0x0003964C File Offset: 0x0003784C
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is IPointerEvent;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00039667 File Offset: 0x00037867
		public virtual void DispatchEvent(EventBase evt, IPanel panel)
		{
			PointerEventDispatchingStrategy.SetBestTargetForEvent(evt, panel);
			PointerEventDispatchingStrategy.SendEventToTarget(evt, panel);
			evt.stopDispatch = true;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00039684 File Offset: 0x00037884
		private static void SendEventToTarget(EventBase evt, IPanel panel)
		{
			VisualElement visualElement = evt.target as VisualElement;
			bool flag = visualElement != null && visualElement.panel == panel;
			if (flag)
			{
				EventDispatchUtilities.PropagateEvent(evt);
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000396BC File Offset: 0x000378BC
		private static void SetBestTargetForEvent(EventBase evt, IPanel panel)
		{
			VisualElement visualElement;
			PointerEventDispatchingStrategy.UpdateElementUnderPointer(evt, panel, out visualElement);
			bool flag = evt.target == null && visualElement != null;
			if (flag)
			{
				evt.propagateToIMGUI = false;
				evt.target = visualElement;
			}
			else
			{
				bool flag2 = evt.target == null && visualElement == null;
				if (flag2)
				{
					bool flag3 = panel != null && panel.contextType == ContextType.Editor && evt.eventTypeId == EventBase<PointerUpEvent>.TypeId();
					if (flag3)
					{
						Panel panel2 = panel as Panel;
						evt.target = ((panel2 != null) ? panel2.rootIMGUIContainer : null);
					}
					else
					{
						evt.target = ((panel != null) ? panel.visualTree : null);
					}
				}
				else
				{
					bool flag4 = evt.target != null;
					if (flag4)
					{
						evt.propagateToIMGUI = false;
					}
				}
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0003977C File Offset: 0x0003797C
		private static void UpdateElementUnderPointer(EventBase evt, IPanel panel, out VisualElement elementUnderPointer)
		{
			IPointerEvent pointerEvent = evt as IPointerEvent;
			BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
			IPointerEventInternal pointerEventInternal = evt as IPointerEventInternal;
			elementUnderPointer = ((pointerEventInternal == null || pointerEventInternal.recomputeTopElementUnderPointer) ? ((baseVisualElementPanel != null) ? baseVisualElementPanel.RecomputeTopElementUnderPointer(pointerEvent.pointerId, pointerEvent.position, evt) : null) : ((baseVisualElementPanel != null) ? baseVisualElementPanel.GetTopElementUnderPointer(pointerEvent.pointerId) : null));
		}
	}
}
