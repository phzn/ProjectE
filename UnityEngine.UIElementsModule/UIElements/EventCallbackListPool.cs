using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020001AF RID: 431
	internal class EventCallbackListPool
	{
		// Token: 0x06000D22 RID: 3362 RVA: 0x000335FC File Offset: 0x000317FC
		public EventCallbackList Get(EventCallbackList initializer)
		{
			bool flag = this.m_Stack.Count == 0;
			EventCallbackList eventCallbackList;
			if (flag)
			{
				bool flag2 = initializer != null;
				if (flag2)
				{
					eventCallbackList = new EventCallbackList(initializer);
				}
				else
				{
					eventCallbackList = new EventCallbackList();
				}
			}
			else
			{
				eventCallbackList = this.m_Stack.Pop();
				bool flag3 = initializer != null;
				if (flag3)
				{
					eventCallbackList.AddRange(initializer);
				}
			}
			return eventCallbackList;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003365C File Offset: 0x0003185C
		public void Release(EventCallbackList element)
		{
			element.Clear();
			this.m_Stack.Push(element);
		}

		// Token: 0x0400063D RID: 1597
		private readonly Stack<EventCallbackList> m_Stack = new Stack<EventCallbackList>();
	}
}
