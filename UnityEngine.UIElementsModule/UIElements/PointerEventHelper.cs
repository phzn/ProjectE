using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000218 RID: 536
	internal static class PointerEventHelper
	{
		// Token: 0x06000F95 RID: 3989 RVA: 0x000398CC File Offset: 0x00037ACC
		public static EventBase GetPooled(EventType eventType, Vector3 mousePosition, Vector2 delta, int button, int clickCount, EventModifiers modifiers)
		{
			bool flag = eventType == EventType.MouseDown && !PointerDeviceState.HasAdditionalPressedButtons(PointerId.mousePointerId, button);
			EventBase eventBase;
			if (flag)
			{
				eventBase = PointerEventBase<PointerDownEvent>.GetPooled(eventType, mousePosition, delta, button, clickCount, modifiers);
			}
			else
			{
				bool flag2 = eventType == EventType.MouseUp && !PointerDeviceState.HasAdditionalPressedButtons(PointerId.mousePointerId, button);
				if (flag2)
				{
					eventBase = PointerEventBase<PointerUpEvent>.GetPooled(eventType, mousePosition, delta, button, clickCount, modifiers);
				}
				else
				{
					eventBase = PointerEventBase<PointerMoveEvent>.GetPooled(eventType, mousePosition, delta, button, clickCount, modifiers);
				}
			}
			return eventBase;
		}
	}
}
