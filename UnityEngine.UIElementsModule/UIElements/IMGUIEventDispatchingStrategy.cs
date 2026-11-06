using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D1 RID: 465
	internal class IMGUIEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x00036994 File Offset: 0x00034B94
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is IMGUIEvent;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000369B0 File Offset: 0x00034BB0
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			bool flag = panel != null;
			if (flag)
			{
				EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
