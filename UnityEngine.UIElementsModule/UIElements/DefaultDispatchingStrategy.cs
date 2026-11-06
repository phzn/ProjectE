using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A0 RID: 416
	internal class DefaultDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003219C File Offset: 0x0003039C
		public bool CanDispatchEvent(EventBase evt)
		{
			return !(evt is IMGUIEvent);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000321BC File Offset: 0x000303BC
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			VisualElement visualElement = evt.target as VisualElement;
			bool flag = visualElement != null && visualElement.panel == panel;
			if (flag)
			{
				evt.propagateToIMGUI = visualElement.isIMGUIContainer;
				EventDispatchUtilities.PropagateEvent(evt);
			}
			else
			{
				bool flag2 = !evt.isPropagationStopped && panel != null;
				if (flag2)
				{
					bool flag3 = evt.propagateToIMGUI || evt.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId();
					if (flag3)
					{
						EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
					}
				}
			}
			evt.stopDispatch = true;
		}
	}
}
