using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200006B RID: 107
	public abstract class PointerInputModule : BaseInputModule
	{
		// Token: 0x06000628 RID: 1576 RVA: 0x00019E12 File Offset: 0x00018012
		protected bool GetPointerData(int id, out PointerEventData data, bool create)
		{
			if (!this.m_PointerData.TryGetValue(id, out data) && create)
			{
				data = new PointerEventData(base.eventSystem)
				{
					pointerId = id
				};
				this.m_PointerData.Add(id, data);
				return true;
			}
			return false;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00019E4D File Offset: 0x0001804D
		protected void RemovePointerData(PointerEventData data)
		{
			this.m_PointerData.Remove(data.pointerId);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00019E64 File Offset: 0x00018064
		protected PointerEventData GetTouchPointerEventData(Touch input, out bool pressed, out bool released)
		{
			PointerEventData pointerEventData;
			bool pointerData = this.GetPointerData(input.fingerId, out pointerEventData, true);
			pointerEventData.Reset();
			pressed = pointerData || input.phase == TouchPhase.Began;
			released = input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended;
			if (pointerData)
			{
				pointerEventData.position = input.position;
			}
			if (pressed)
			{
				pointerEventData.delta = Vector2.zero;
			}
			else
			{
				pointerEventData.delta = input.position - pointerEventData.position;
			}
			pointerEventData.position = input.position;
			pointerEventData.button = PointerEventData.InputButton.Left;
			if (input.phase == TouchPhase.Canceled)
			{
				pointerEventData.pointerCurrentRaycast = default(RaycastResult);
			}
			else
			{
				base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
				RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
				pointerEventData.pointerCurrentRaycast = raycastResult;
				this.m_RaycastResultCache.Clear();
			}
			pointerEventData.pressure = input.pressure;
			pointerEventData.altitudeAngle = input.altitudeAngle;
			pointerEventData.azimuthAngle = input.azimuthAngle;
			pointerEventData.radius = Vector2.one * input.radius;
			pointerEventData.radiusVariance = Vector2.one * input.radiusVariance;
			return pointerEventData;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00019FA4 File Offset: 0x000181A4
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
			to.pressure = from.pressure;
			to.tangentialPressure = from.tangentialPressure;
			to.altitudeAngle = from.altitudeAngle;
			to.azimuthAngle = from.azimuthAngle;
			to.twist = from.twist;
			to.radius = from.radius;
			to.radiusVariance = from.radiusVariance;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001A044 File Offset: 0x00018244
		protected PointerEventData.FramePressState StateForMouseButton(int buttonId)
		{
			bool mouseButtonDown = base.input.GetMouseButtonDown(buttonId);
			bool mouseButtonUp = base.input.GetMouseButtonUp(buttonId);
			if (mouseButtonDown && mouseButtonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (mouseButtonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (mouseButtonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001A07D File Offset: 0x0001827D
		protected virtual PointerInputModule.MouseState GetMousePointerEventData()
		{
			return this.GetMousePointerEventData(0);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001A088 File Offset: 0x00018288
		protected virtual PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = this.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			if (pointerData)
			{
				pointerEventData.position = base.input.mousePosition;
			}
			Vector2 mousePosition = base.input.mousePosition;
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				pointerEventData.position = new Vector2(-1f, -1f);
				pointerEventData.delta = Vector2.zero;
			}
			else
			{
				pointerEventData.delta = mousePosition - pointerEventData.position;
				pointerEventData.position = mousePosition;
			}
			pointerEventData.scrollDelta = base.input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = raycastResult;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			this.GetPointerData(-2, out pointerEventData2, true);
			pointerEventData2.Reset();
			this.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			this.GetPointerData(-3, out pointerEventData3, true);
			pointerEventData3.Reset();
			this.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, this.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, this.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001A1D8 File Offset: 0x000183D8
		protected PointerEventData GetLastPointerEventData(int id)
		{
			PointerEventData pointerEventData;
			this.GetPointerData(id, out pointerEventData, false);
			return pointerEventData;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001A1F4 File Offset: 0x000183F4
		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001A220 File Offset: 0x00018420
		protected virtual void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject gameObject = ((Cursor.lockState == CursorLockMode.Locked) ? null : pointerEvent.pointerCurrentRaycast.gameObject);
			base.HandlePointerExitAndEnter(pointerEvent, gameObject);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001A250 File Offset: 0x00018450
		protected virtual void ProcessDrag(PointerEventData pointerEvent)
		{
			if (!pointerEvent.IsPointerMoving() || Cursor.lockState == CursorLockMode.Locked || pointerEvent.pointerDrag == null)
			{
				return;
			}
			if (!pointerEvent.dragging && PointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001A318 File Offset: 0x00018518
		public override bool IsPointerOverGameObject(int pointerId)
		{
			PointerEventData lastPointerEventData = this.GetLastPointerEventData(pointerId);
			return lastPointerEventData != null && lastPointerEventData.pointerEnter != null;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001A340 File Offset: 0x00018540
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (PointerEventData pointerEventData in this.m_PointerData.Values)
			{
				base.HandlePointerExitAndEnter(pointerEventData, null);
			}
			this.m_PointerData.Clear();
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001A3B8 File Offset: 0x000185B8
		public override string ToString()
		{
			string text = "<b>Pointer Input Module of type: </b>";
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder(text + ((type != null) ? type.ToString() : null));
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, PointerEventData> keyValuePair in this.m_PointerData)
			{
				if (keyValuePair.Value != null)
				{
					stringBuilder.AppendLine("<B>Pointer:</b> " + keyValuePair.Key.ToString());
					stringBuilder.AppendLine(keyValuePair.Value.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A470 File Offset: 0x00018670
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x04000210 RID: 528
		public const int kMouseLeftId = -1;

		// Token: 0x04000211 RID: 529
		public const int kMouseRightId = -2;

		// Token: 0x04000212 RID: 530
		public const int kMouseMiddleId = -3;

		// Token: 0x04000213 RID: 531
		public const int kFakeTouchesId = -4;

		// Token: 0x04000214 RID: 532
		protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData>();

		// Token: 0x04000215 RID: 533
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x020000C8 RID: 200
		protected class ButtonState
		{
			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001CCDD File Offset: 0x0001AEDD
			// (set) Token: 0x0600075F RID: 1887 RVA: 0x0001CCE5 File Offset: 0x0001AEE5
			public PointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001CCEE File Offset: 0x0001AEEE
			// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001CCF6 File Offset: 0x0001AEF6
			public PointerEventData.InputButton button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}

			// Token: 0x04000355 RID: 853
			private PointerEventData.InputButton m_Button;

			// Token: 0x04000356 RID: 854
			private PointerInputModule.MouseButtonEventData m_EventData;
		}

		// Token: 0x020000C9 RID: 201
		protected class MouseState
		{
			// Token: 0x06000763 RID: 1891 RVA: 0x0001CD08 File Offset: 0x0001AF08
			public bool AnyPressesThisFrame()
			{
				int count = this.m_TrackedButtons.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x0001CD48 File Offset: 0x0001AF48
			public bool AnyReleasesThisFrame()
			{
				int count = this.m_TrackedButtons.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x0001CD88 File Offset: 0x0001AF88
			public PointerInputModule.ButtonState GetButtonState(PointerEventData.InputButton button)
			{
				PointerInputModule.ButtonState buttonState = null;
				int count = this.m_TrackedButtons.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.m_TrackedButtons[i].button == button)
					{
						buttonState = this.m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new PointerInputModule.ButtonState
					{
						button = button,
						eventData = new PointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			// Token: 0x06000766 RID: 1894 RVA: 0x0001CDFA File Offset: 0x0001AFFA
			public void SetButtonState(PointerEventData.InputButton button, PointerEventData.FramePressState stateForMouseButton, PointerEventData data)
			{
				PointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}

			// Token: 0x04000357 RID: 855
			private List<PointerInputModule.ButtonState> m_TrackedButtons = new List<PointerInputModule.ButtonState>();
		}

		// Token: 0x020000CA RID: 202
		public class MouseButtonEventData
		{
			// Token: 0x06000768 RID: 1896 RVA: 0x0001CE2D File Offset: 0x0001B02D
			public bool PressedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x06000769 RID: 1897 RVA: 0x0001CE42 File Offset: 0x0001B042
			public bool ReleasedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x04000358 RID: 856
			public PointerEventData.FramePressState buttonState;

			// Token: 0x04000359 RID: 857
			public PointerEventData buttonData;
		}
	}
}
