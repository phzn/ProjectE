using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A1 RID: 417
	internal class ElementUnderPointer
	{
		// Token: 0x06000CAA RID: 3242 RVA: 0x00032258 File Offset: 0x00030458
		internal VisualElement GetTopElementUnderPointer(int pointerId, out Vector2 pickPosition, out bool isTemporary)
		{
			pickPosition = this.m_PickingPointerPositions[pointerId];
			isTemporary = this.m_IsPickingPointerTemporaries[pointerId];
			return this.m_PendingTopElementUnderPointer[pointerId];
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00032290 File Offset: 0x00030490
		internal VisualElement GetTopElementUnderPointer(int pointerId)
		{
			return this.m_PendingTopElementUnderPointer[pointerId];
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000322AC File Offset: 0x000304AC
		internal void SetElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, Vector2 pointerPos)
		{
			Debug.Assert(pointerId >= 0);
			VisualElement visualElement = this.m_TopElementUnderPointer[pointerId];
			this.m_IsPickingPointerTemporaries[pointerId] = false;
			this.m_PickingPointerPositions[pointerId] = pointerPos;
			bool flag = newElementUnderPointer == visualElement;
			if (!flag)
			{
				this.m_PendingTopElementUnderPointer[pointerId] = newElementUnderPointer;
				this.m_TriggerPointerEvent[pointerId] = null;
				this.m_TriggerMouseEvent[pointerId] = null;
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003230C File Offset: 0x0003050C
		private Vector2 GetEventPointerPosition(EventBase triggerEvent)
		{
			IPointerEvent pointerEvent = triggerEvent as IPointerEvent;
			bool flag = pointerEvent != null;
			Vector2 vector;
			if (flag)
			{
				vector = new Vector2(pointerEvent.position.x, pointerEvent.position.y);
			}
			else
			{
				IMouseEvent mouseEvent = triggerEvent as IMouseEvent;
				bool flag2 = mouseEvent != null;
				if (flag2)
				{
					vector = mouseEvent.mousePosition;
				}
				else
				{
					vector = new Vector2(float.MinValue, float.MinValue);
				}
			}
			return vector;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00032376 File Offset: 0x00030576
		internal void SetTemporaryElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, EventBase triggerEvent)
		{
			this.SetElementUnderPointer(newElementUnderPointer, pointerId, triggerEvent, true);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00032384 File Offset: 0x00030584
		internal void SetElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, EventBase triggerEvent)
		{
			this.SetElementUnderPointer(newElementUnderPointer, pointerId, triggerEvent, false);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00032394 File Offset: 0x00030594
		private void SetElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, EventBase triggerEvent, bool temporary)
		{
			Debug.Assert(pointerId >= 0);
			this.m_IsPickingPointerTemporaries[pointerId] = temporary;
			this.m_PickingPointerPositions[pointerId] = this.GetEventPointerPosition(triggerEvent);
			VisualElement visualElement = this.m_TopElementUnderPointer[pointerId];
			bool flag = newElementUnderPointer == visualElement;
			if (!flag)
			{
				this.m_PendingTopElementUnderPointer[pointerId] = newElementUnderPointer;
				bool flag2 = this.m_TriggerPointerEvent[pointerId] == null && triggerEvent is IPointerEvent;
				if (flag2)
				{
					this.m_TriggerPointerEvent[pointerId] = triggerEvent as IPointerEvent;
				}
				bool flag3 = this.m_TriggerMouseEvent[pointerId] == null && triggerEvent is IMouseEvent;
				if (flag3)
				{
					this.m_TriggerMouseEvent[pointerId] = triggerEvent as IMouseEvent;
				}
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00032440 File Offset: 0x00030640
		internal void CommitElementUnderPointers(EventDispatcher dispatcher, ContextType contextType)
		{
			for (int i = 0; i < this.m_TopElementUnderPointer.Length; i++)
			{
				IPointerEvent pointerEvent = this.m_TriggerPointerEvent[i];
				VisualElement visualElement = this.m_TopElementUnderPointer[i];
				VisualElement visualElement2 = this.m_PendingTopElementUnderPointer[i];
				bool flag = visualElement2 == visualElement;
				if (flag)
				{
					bool flag2 = pointerEvent != null;
					if (flag2)
					{
						Vector3 position = pointerEvent.position;
						this.m_PickingPointerPositions[i] = new Vector2(position.x, position.y);
					}
					else
					{
						bool flag3 = this.m_TriggerMouseEvent[i] != null;
						if (flag3)
						{
							this.m_PickingPointerPositions[i] = this.m_TriggerMouseEvent[i].mousePosition;
						}
					}
				}
				else
				{
					this.m_TopElementUnderPointer[i] = visualElement2;
					bool flag4 = pointerEvent == null && this.m_TriggerMouseEvent[i] == null;
					if (flag4)
					{
						using (new EventDispatcherGate(dispatcher))
						{
							Vector2 pointerPosition = PointerDeviceState.GetPointerPosition(i, contextType);
							PointerEventsHelper.SendOverOut(visualElement, visualElement2, null, pointerPosition, i);
							PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, null, pointerPosition, i);
							this.m_PickingPointerPositions[i] = pointerPosition;
							bool flag5 = i == PointerId.mousePointerId;
							if (flag5)
							{
								MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, null, pointerPosition);
								MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, null, pointerPosition);
							}
						}
					}
					bool flag6 = pointerEvent != null;
					if (flag6)
					{
						Vector3 position2 = pointerEvent.position;
						this.m_PickingPointerPositions[i] = new Vector2(position2.x, position2.y);
						EventBase eventBase = pointerEvent as EventBase;
						bool flag7 = eventBase != null && (eventBase.eventTypeId == EventBase<PointerMoveEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerDownEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerUpEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerCancelEvent>.TypeId());
						if (flag7)
						{
							using (new EventDispatcherGate(dispatcher))
							{
								PointerEventsHelper.SendOverOut(visualElement, visualElement2, pointerEvent, position2, i);
								PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, pointerEvent, position2, i);
							}
						}
					}
					this.m_TriggerPointerEvent[i] = null;
					IMouseEvent mouseEvent = this.m_TriggerMouseEvent[i];
					bool flag8 = mouseEvent != null;
					if (flag8)
					{
						Vector2 mousePosition = mouseEvent.mousePosition;
						this.m_PickingPointerPositions[i] = mousePosition;
						EventBase eventBase2 = mouseEvent as EventBase;
						bool flag9 = eventBase2 != null;
						if (flag9)
						{
							bool flag10 = eventBase2.eventTypeId == EventBase<MouseMoveEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseDownEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseUpEvent>.TypeId() || eventBase2.eventTypeId == EventBase<WheelEvent>.TypeId();
							if (flag10)
							{
								using (new EventDispatcherGate(dispatcher))
								{
									MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, mouseEvent, mousePosition);
									MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
								}
							}
							else
							{
								bool flag11 = eventBase2.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId();
								if (flag11)
								{
									using (new EventDispatcherGate(dispatcher))
									{
										PointerEventsHelper.SendOverOut(visualElement, visualElement2, null, mousePosition, i);
										PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, null, mousePosition, i);
										bool flag12 = i == PointerId.mousePointerId;
										if (flag12)
										{
											MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, mouseEvent, mousePosition);
											MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
										}
									}
								}
							}
						}
						this.m_TriggerMouseEvent[i] = null;
					}
				}
			}
		}

		// Token: 0x040005FE RID: 1534
		private VisualElement[] m_PendingTopElementUnderPointer = new VisualElement[PointerId.maxPointers];

		// Token: 0x040005FF RID: 1535
		private VisualElement[] m_TopElementUnderPointer = new VisualElement[PointerId.maxPointers];

		// Token: 0x04000600 RID: 1536
		private IPointerEvent[] m_TriggerPointerEvent = new IPointerEvent[PointerId.maxPointers];

		// Token: 0x04000601 RID: 1537
		private IMouseEvent[] m_TriggerMouseEvent = new IMouseEvent[PointerId.maxPointers];

		// Token: 0x04000602 RID: 1538
		private Vector2[] m_PickingPointerPositions = new Vector2[PointerId.maxPointers];

		// Token: 0x04000603 RID: 1539
		private bool[] m_IsPickingPointerTemporaries = new bool[PointerId.maxPointers];
	}
}
