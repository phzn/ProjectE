using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020001B0 RID: 432
	internal class EventCallbackList
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00033687 File Offset: 0x00031887
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x0003368F File Offset: 0x0003188F
		public int trickleDownCallbackCount { get; private set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00033698 File Offset: 0x00031898
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x000336A0 File Offset: 0x000318A0
		public int bubbleUpCallbackCount { get; private set; }

		// Token: 0x06000D29 RID: 3369 RVA: 0x000336A9 File Offset: 0x000318A9
		public EventCallbackList()
		{
			this.m_List = new List<EventCallbackFunctorBase>();
			this.trickleDownCallbackCount = 0;
			this.bubbleUpCallbackCount = 0;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000336CE File Offset: 0x000318CE
		public EventCallbackList(EventCallbackList source)
		{
			this.m_List = new List<EventCallbackFunctorBase>(source.m_List);
			this.trickleDownCallbackCount = 0;
			this.bubbleUpCallbackCount = 0;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000336FC File Offset: 0x000318FC
		public bool Contains(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			return this.Find(eventTypeId, callback, phase) != null;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0003371C File Offset: 0x0003191C
		public EventCallbackFunctorBase Find(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			for (int i = 0; i < this.m_List.Count; i++)
			{
				bool flag = this.m_List[i].IsEquivalentTo(eventTypeId, callback, phase);
				if (flag)
				{
					return this.m_List[i];
				}
			}
			return null;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00033774 File Offset: 0x00031974
		public bool Remove(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			for (int i = 0; i < this.m_List.Count; i++)
			{
				bool flag = this.m_List[i].IsEquivalentTo(eventTypeId, callback, phase);
				if (flag)
				{
					this.m_List.RemoveAt(i);
					bool flag2 = phase == CallbackPhase.TrickleDownAndTarget;
					if (flag2)
					{
						int num = this.trickleDownCallbackCount;
						this.trickleDownCallbackCount = num - 1;
					}
					else
					{
						bool flag3 = phase == CallbackPhase.TargetAndBubbleUp;
						if (flag3)
						{
							int num = this.bubbleUpCallbackCount;
							this.bubbleUpCallbackCount = num - 1;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003380C File Offset: 0x00031A0C
		public void Add(EventCallbackFunctorBase item)
		{
			this.m_List.Add(item);
			bool flag = item.phase == CallbackPhase.TrickleDownAndTarget;
			if (flag)
			{
				int num = this.trickleDownCallbackCount;
				this.trickleDownCallbackCount = num + 1;
			}
			else
			{
				bool flag2 = item.phase == CallbackPhase.TargetAndBubbleUp;
				if (flag2)
				{
					int num = this.bubbleUpCallbackCount;
					this.bubbleUpCallbackCount = num + 1;
				}
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003386C File Offset: 0x00031A6C
		public void AddRange(EventCallbackList list)
		{
			this.m_List.AddRange(list.m_List);
			foreach (EventCallbackFunctorBase eventCallbackFunctorBase in list.m_List)
			{
				bool flag = eventCallbackFunctorBase.phase == CallbackPhase.TrickleDownAndTarget;
				if (flag)
				{
					int num = this.trickleDownCallbackCount;
					this.trickleDownCallbackCount = num + 1;
				}
				else
				{
					bool flag2 = eventCallbackFunctorBase.phase == CallbackPhase.TargetAndBubbleUp;
					if (flag2)
					{
						int num = this.bubbleUpCallbackCount;
						this.bubbleUpCallbackCount = num + 1;
					}
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x00033914 File Offset: 0x00031B14
		public int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x170002AD RID: 685
		public EventCallbackFunctorBase this[int i]
		{
			get
			{
				return this.m_List[i];
			}
			set
			{
				this.m_List[i] = value;
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00033963 File Offset: 0x00031B63
		public void Clear()
		{
			this.m_List.Clear();
			this.trickleDownCallbackCount = 0;
			this.bubbleUpCallbackCount = 0;
		}

		// Token: 0x0400063E RID: 1598
		private List<EventCallbackFunctorBase> m_List;
	}
}
