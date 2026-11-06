using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000034 RID: 52
	internal class ClickDetector
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000063F5 File Offset: 0x000045F5
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000063FC File Offset: 0x000045FC
		internal static int s_DoubleClickTime { get; set; } = -1;

		// Token: 0x06000208 RID: 520 RVA: 0x00006404 File Offset: 0x00004604
		public ClickDetector()
		{
			this.m_ClickStatus = new List<ClickDetector.ButtonClickStatus>(PointerId.maxPointers);
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				this.m_ClickStatus.Add(new ClickDetector.ButtonClickStatus());
			}
			bool flag = ClickDetector.s_DoubleClickTime == -1;
			if (flag)
			{
				ClickDetector.s_DoubleClickTime = Event.GetDoubleClickTime();
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000646C File Offset: 0x0000466C
		private void StartClickTracking(EventBase evt)
		{
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag = pointerEvent == null;
			if (!flag)
			{
				ClickDetector.ButtonClickStatus buttonClickStatus = this.m_ClickStatus[pointerEvent.pointerId];
				VisualElement visualElement = evt.target as VisualElement;
				bool flag2 = visualElement != buttonClickStatus.m_Target;
				if (flag2)
				{
					buttonClickStatus.Reset();
				}
				buttonClickStatus.m_Target = visualElement;
				bool flag3 = evt.timestamp - buttonClickStatus.m_LastPointerDownTime > (long)ClickDetector.s_DoubleClickTime;
				if (flag3)
				{
					buttonClickStatus.m_ClickCount = 1;
				}
				else
				{
					buttonClickStatus.m_ClickCount++;
				}
				buttonClickStatus.m_LastPointerDownTime = evt.timestamp;
				buttonClickStatus.m_PointerDownPosition = pointerEvent.position;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00006520 File Offset: 0x00004720
		private void SendClickEvent(EventBase evt)
		{
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag = pointerEvent == null;
			if (!flag)
			{
				ClickDetector.ButtonClickStatus buttonClickStatus = this.m_ClickStatus[pointerEvent.pointerId];
				VisualElement visualElement = evt.target as VisualElement;
				bool flag2 = visualElement != null && ClickDetector.ContainsPointer(visualElement, pointerEvent.position);
				if (flag2)
				{
					bool flag3 = buttonClickStatus.m_Target != null && buttonClickStatus.m_ClickCount > 0;
					if (flag3)
					{
						VisualElement visualElement2 = buttonClickStatus.m_Target.FindCommonAncestor(evt.target as VisualElement);
						bool flag4 = visualElement2 != null;
						if (flag4)
						{
							using (ClickEvent pooled = ClickEvent.GetPooled(evt as PointerUpEvent, buttonClickStatus.m_ClickCount))
							{
								pooled.target = visualElement2;
								visualElement2.SendEvent(pooled);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000660C File Offset: 0x0000480C
		private void CancelClickTracking(EventBase evt)
		{
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag = pointerEvent == null;
			if (!flag)
			{
				ClickDetector.ButtonClickStatus buttonClickStatus = this.m_ClickStatus[pointerEvent.pointerId];
				buttonClickStatus.Reset();
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00006648 File Offset: 0x00004848
		public void ProcessEvent(EventBase evt)
		{
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag = pointerEvent == null;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<PointerDownEvent>.TypeId() && pointerEvent.button == 0;
				if (flag2)
				{
					this.StartClickTracking(evt);
				}
				else
				{
					bool flag3 = evt.eventTypeId == EventBase<PointerMoveEvent>.TypeId();
					if (flag3)
					{
						bool flag4 = pointerEvent.button == 0 && (pointerEvent.pressedButtons & 1) == 1;
						if (flag4)
						{
							this.StartClickTracking(evt);
						}
						else
						{
							bool flag5 = pointerEvent.button == 0 && (pointerEvent.pressedButtons & 1) == 0;
							if (flag5)
							{
								this.SendClickEvent(evt);
							}
							else
							{
								ClickDetector.ButtonClickStatus buttonClickStatus = this.m_ClickStatus[pointerEvent.pointerId];
								bool flag6 = buttonClickStatus.m_Target != null;
								if (flag6)
								{
									buttonClickStatus.m_LastPointerDownTime = 0L;
								}
							}
						}
					}
					else
					{
						bool flag7 = evt.eventTypeId == EventBase<PointerCancelEvent>.TypeId();
						if (flag7)
						{
							this.CancelClickTracking(evt);
						}
						else
						{
							bool flag8 = evt.eventTypeId == EventBase<PointerUpEvent>.TypeId() && pointerEvent.button == 0;
							if (flag8)
							{
								this.SendClickEvent(evt);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00006778 File Offset: 0x00004978
		private static bool ContainsPointer(VisualElement element, Vector2 position)
		{
			bool flag = !element.worldBound.Contains(position) || element.panel == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				VisualElement visualElement = element.panel.Pick(position);
				flag2 = element == visualElement || element.Contains(visualElement);
			}
			return flag2;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000067CC File Offset: 0x000049CC
		internal void Cleanup(List<VisualElement> elements)
		{
			foreach (ClickDetector.ButtonClickStatus buttonClickStatus in this.m_ClickStatus)
			{
				bool flag = buttonClickStatus.m_Target == null;
				if (!flag)
				{
					bool flag2 = elements.Contains(buttonClickStatus.m_Target);
					if (flag2)
					{
						buttonClickStatus.Reset();
					}
				}
			}
		}

		// Token: 0x040000A2 RID: 162
		private List<ClickDetector.ButtonClickStatus> m_ClickStatus;

		// Token: 0x02000035 RID: 53
		private class ButtonClickStatus
		{
			// Token: 0x06000210 RID: 528 RVA: 0x0000684C File Offset: 0x00004A4C
			public void Reset()
			{
				this.m_Target = null;
				this.m_ClickCount = 0;
				this.m_LastPointerDownTime = 0L;
				this.m_PointerDownPosition = Vector3.zero;
			}

			// Token: 0x040000A4 RID: 164
			public VisualElement m_Target;

			// Token: 0x040000A5 RID: 165
			public Vector3 m_PointerDownPosition;

			// Token: 0x040000A6 RID: 166
			public long m_LastPointerDownTime;

			// Token: 0x040000A7 RID: 167
			public int m_ClickCount;
		}
	}
}
