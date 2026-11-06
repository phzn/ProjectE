using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000198 RID: 408
	internal class CommandEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x00031F2C File Offset: 0x0003012C
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is ICommandEvent;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00031F48 File Offset: 0x00030148
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			bool flag = panel != null;
			if (flag)
			{
				Focusable leafFocusedElement = panel.focusController.GetLeafFocusedElement();
				bool flag2 = leafFocusedElement != null;
				if (flag2)
				{
					bool isIMGUIContainer = leafFocusedElement.isIMGUIContainer;
					if (isIMGUIContainer)
					{
						IMGUIContainer imguicontainer = (IMGUIContainer)leafFocusedElement;
						bool flag3 = !evt.Skip(imguicontainer) && imguicontainer.SendEventToIMGUI(evt, true, true);
						if (flag3)
						{
							evt.StopPropagation();
							evt.PreventDefault();
						}
						bool flag4 = !evt.isPropagationStopped && evt.propagateToIMGUI;
						if (flag4)
						{
							evt.skipElements.Add(imguicontainer);
							EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
						}
					}
					else
					{
						evt.target = leafFocusedElement;
						EventDispatchUtilities.PropagateEvent(evt);
						bool flag5 = !evt.isPropagationStopped && evt.propagateToIMGUI;
						if (flag5)
						{
							EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
						}
					}
				}
				else
				{
					EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
				}
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
