using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001FE RID: 510
	internal class NavigationEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x00038C50 File Offset: 0x00036E50
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is INavigationEvent;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00038C6C File Offset: 0x00036E6C
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			bool flag = panel != null;
			if (flag)
			{
				if (evt.target == null)
				{
					evt.target = panel.focusController.GetLeafFocusedElement() ?? panel.visualTree;
				}
				EventDispatchUtilities.PropagateEvent(evt);
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
