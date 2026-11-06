using System;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004B RID: 75
	[AddComponentMenu("UI Toolkit/Panel Event Handler (UI Toolkit)")]
	public class PanelEventHandler : UIBehaviour, IPointerMoveHandler, IEventSystemHandler, IPointerUpHandler, IPointerDownHandler, ISubmitHandler, ICancelHandler, IMoveHandler, IScrollHandler, ISelectHandler, IDeselectHandler, IPointerExitHandler, IPointerEnterHandler, IRuntimePanelComponent, IPointerClickHandler
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00017561 File Offset: 0x00015761
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0001756C File Offset: 0x0001576C
		public IPanel panel
		{
			get
			{
				return this.m_Panel;
			}
			set
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)value;
				if (this.m_Panel != baseRuntimePanel)
				{
					this.UnregisterCallbacks();
					this.m_Panel = baseRuntimePanel;
					this.RegisterCallbacks();
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001759C File Offset: 0x0001579C
		private GameObject selectableGameObject
		{
			get
			{
				BaseRuntimePanel panel = this.m_Panel;
				if (panel == null)
				{
					return null;
				}
				return panel.selectableGameObject;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000175AF File Offset: 0x000157AF
		private EventSystem eventSystem
		{
			get
			{
				return UIElementsRuntimeUtility.activeEventSystem as EventSystem;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000175BB File Offset: 0x000157BB
		private bool isCurrentFocusedPanel
		{
			get
			{
				return this.m_Panel != null && this.eventSystem != null && this.eventSystem.currentSelectedGameObject == this.selectableGameObject;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000175EB File Offset: 0x000157EB
		private Focusable currentFocusedElement
		{
			get
			{
				BaseRuntimePanel panel = this.m_Panel;
				if (panel == null)
				{
					return null;
				}
				return panel.focusController.GetLeafFocusedElement();
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00017603 File Offset: 0x00015803
		protected override void OnEnable()
		{
			base.OnEnable();
			this.RegisterCallbacks();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00017611 File Offset: 0x00015811
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterCallbacks();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00017620 File Offset: 0x00015820
		private void RegisterCallbacks()
		{
			if (this.m_Panel != null)
			{
				this.m_Panel.destroyed += this.OnPanelDestroyed;
				this.m_Panel.visualTree.RegisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnElementFocus), TrickleDown.TrickleDown);
				this.m_Panel.visualTree.RegisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnElementBlur), TrickleDown.TrickleDown);
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00017688 File Offset: 0x00015888
		private void UnregisterCallbacks()
		{
			if (this.m_Panel != null)
			{
				this.m_Panel.destroyed -= this.OnPanelDestroyed;
				this.m_Panel.visualTree.UnregisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnElementFocus), TrickleDown.TrickleDown);
				this.m_Panel.visualTree.UnregisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnElementBlur), TrickleDown.TrickleDown);
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000176EE File Offset: 0x000158EE
		private void OnPanelDestroyed()
		{
			this.panel = null;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000176F7 File Offset: 0x000158F7
		private void OnElementFocus(FocusEvent e)
		{
			if (!this.m_Selecting && this.eventSystem != null)
			{
				this.eventSystem.SetSelectedGameObject(this.selectableGameObject);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00017720 File Offset: 0x00015920
		private void OnElementBlur(BlurEvent e)
		{
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00017724 File Offset: 0x00015924
		public void OnSelect(BaseEventData eventData)
		{
			this.m_Selecting = true;
			try
			{
				BaseRuntimePanel panel = this.m_Panel;
				if (panel != null)
				{
					panel.Focus();
				}
			}
			finally
			{
				this.m_Selecting = false;
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00017764 File Offset: 0x00015964
		public void OnDeselect(BaseEventData eventData)
		{
			BaseRuntimePanel panel = this.m_Panel;
			if (panel == null)
			{
				return;
			}
			panel.Blur();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00017778 File Offset: 0x00015978
		public void OnPointerMove(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Default))
			{
				return;
			}
			using (PointerMoveEvent pooled = PointerEventBase<PointerMoveEvent>.GetPooled(this.m_PointerEvent))
			{
				this.SendEvent(pooled, eventData);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000177D0 File Offset: 0x000159D0
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Up))
			{
				return;
			}
			using (PointerUpEvent pooled = PointerEventBase<PointerUpEvent>.GetPooled(this.m_PointerEvent))
			{
				this.SendEvent(pooled, eventData);
				if (pooled.pressedButtons == 0)
				{
					PointerDeviceState.SetPlayerPanelWithSoftPointerCapture(pooled.pointerId, null);
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001783C File Offset: 0x00015A3C
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Down))
			{
				return;
			}
			if (this.eventSystem != null)
			{
				this.eventSystem.SetSelectedGameObject(this.selectableGameObject);
			}
			using (PointerDownEvent pooled = PointerEventBase<PointerDownEvent>.GetPooled(this.m_PointerEvent))
			{
				this.SendEvent(pooled, eventData);
				PointerDeviceState.SetPlayerPanelWithSoftPointerCapture(pooled.pointerId, this.m_Panel);
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000178C4 File Offset: 0x00015AC4
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Default))
			{
				return;
			}
			if (eventData.pointerCurrentRaycast.gameObject == base.gameObject && eventData.pointerPressRaycast.gameObject != base.gameObject && this.m_PointerEvent.pointerId != PointerId.mousePointerId)
			{
				using (PointerCancelEvent pooled = PointerEventBase<PointerCancelEvent>.GetPooled(this.m_PointerEvent))
				{
					this.SendEvent(pooled, eventData);
					if (pooled.pressedButtons == 0)
					{
						PointerDeviceState.SetPlayerPanelWithSoftPointerCapture(pooled.pointerId, null);
					}
				}
			}
			this.m_Panel.PointerLeavesPanel(this.m_PointerEvent.pointerId, this.m_PointerEvent.position);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001799C File Offset: 0x00015B9C
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Default))
			{
				return;
			}
			this.m_Panel.PointerEntersPanel(this.m_PointerEvent.pointerId, this.m_PointerEvent.position);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000179E8 File Offset: 0x00015BE8
		public void OnPointerClick(PointerEventData eventData)
		{
			this.m_LastClickTime = Time.unscaledTime;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000179F8 File Offset: 0x00015BF8
		public void OnSubmit(BaseEventData eventData)
		{
			if (this.m_Panel == null)
			{
				return;
			}
			Focusable focusable = this.currentFocusedElement ?? this.m_Panel.visualTree;
			this.ProcessImguiEvents(focusable);
			using (NavigationSubmitEvent pooled = NavigationEventBase<NavigationSubmitEvent>.GetPooled(PanelEventHandler.s_Modifiers))
			{
				pooled.target = focusable;
				this.SendEvent(pooled, eventData);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00017A64 File Offset: 0x00015C64
		public void OnCancel(BaseEventData eventData)
		{
			if (this.m_Panel == null)
			{
				return;
			}
			Focusable focusable = this.currentFocusedElement ?? this.m_Panel.visualTree;
			this.ProcessImguiEvents(focusable);
			using (NavigationCancelEvent pooled = NavigationEventBase<NavigationCancelEvent>.GetPooled(PanelEventHandler.s_Modifiers))
			{
				pooled.target = focusable;
				this.SendEvent(pooled, eventData);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00017AD0 File Offset: 0x00015CD0
		public void OnMove(AxisEventData eventData)
		{
			if (this.m_Panel == null)
			{
				return;
			}
			Focusable focusable = this.currentFocusedElement ?? this.m_Panel.visualTree;
			this.ProcessImguiEvents(focusable);
			using (NavigationMoveEvent pooled = NavigationMoveEvent.GetPooled(eventData.moveVector, PanelEventHandler.s_Modifiers))
			{
				pooled.target = focusable;
				this.SendEvent(pooled, eventData);
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00017B40 File Offset: 0x00015D40
		public void OnScroll(PointerEventData eventData)
		{
			if (this.m_Panel == null || !this.ReadPointerData(this.m_PointerEvent, eventData, PanelEventHandler.PointerEventType.Default))
			{
				return;
			}
			Vector2 vector = eventData.scrollDelta;
			vector.y = -vector.y;
			vector /= 20f;
			using (WheelEvent pooled = WheelEvent.GetPooled(vector, this.m_PointerEvent))
			{
				this.SendEvent(pooled, eventData);
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00017BC0 File Offset: 0x00015DC0
		private void SendEvent(EventBase e, BaseEventData sourceEventData)
		{
			this.m_Panel.SendEvent(e, DispatchMode.Default);
			if (e.isPropagationStopped)
			{
				sourceEventData.Use();
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00017BDD File Offset: 0x00015DDD
		private void SendEvent(EventBase e, Event sourceEvent)
		{
			this.m_Panel.SendEvent(e, DispatchMode.Default);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00017BEC File Offset: 0x00015DEC
		internal void Update()
		{
			if (this.isCurrentFocusedPanel)
			{
				this.ProcessImguiEvents(this.currentFocusedElement ?? this.m_Panel.visualTree);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00017C11 File Offset: 0x00015E11
		private void LateUpdate()
		{
			this.ProcessImguiEvents(null);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00017C1C File Offset: 0x00015E1C
		private void ProcessImguiEvents(Focusable target)
		{
			bool flag = true;
			while (Event.PopEvent(this.m_Event))
			{
				if (this.m_Event.type != EventType.Ignore && this.m_Event.type != EventType.Repaint && this.m_Event.type != EventType.Layout)
				{
					PanelEventHandler.s_Modifiers = (flag ? this.m_Event.modifiers : (PanelEventHandler.s_Modifiers | this.m_Event.modifiers));
					flag = false;
					if (target != null)
					{
						this.ProcessKeyboardEvent(this.m_Event, target);
						if (this.eventSystem.sendNavigationEvents)
						{
							this.ProcessTabEvent(this.m_Event, target);
						}
					}
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00017CBA File Offset: 0x00015EBA
		private void ProcessKeyboardEvent(Event e, Focusable target)
		{
			if (e.type == EventType.KeyUp)
			{
				this.SendKeyUpEvent(e, target);
				return;
			}
			if (e.type == EventType.KeyDown)
			{
				this.SendKeyDownEvent(e, target);
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00017CDF File Offset: 0x00015EDF
		private void ProcessTabEvent(Event e, Focusable target)
		{
			if (e.ShouldSendNavigationMoveEventRuntime())
			{
				this.SendTabEvent(e, e.shift ? NavigationMoveEvent.Direction.Previous : NavigationMoveEvent.Direction.Next, target);
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00017D00 File Offset: 0x00015F00
		private void SendTabEvent(Event e, NavigationMoveEvent.Direction direction, Focusable target)
		{
			using (NavigationMoveEvent pooled = NavigationMoveEvent.GetPooled(direction, PanelEventHandler.s_Modifiers))
			{
				pooled.target = target;
				this.SendEvent(pooled, e);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00017D44 File Offset: 0x00015F44
		private void SendKeyUpEvent(Event e, Focusable target)
		{
			using (KeyUpEvent keyUpEvent = (KeyUpEvent)UIElementsRuntimeUtility.CreateEvent(e))
			{
				keyUpEvent.target = target;
				this.SendEvent(keyUpEvent, e);
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00017D88 File Offset: 0x00015F88
		private void SendKeyDownEvent(Event e, Focusable target)
		{
			using (KeyDownEvent keyDownEvent = (KeyDownEvent)UIElementsRuntimeUtility.CreateEvent(e))
			{
				keyDownEvent.target = target;
				this.SendEvent(keyDownEvent, e);
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00017DCC File Offset: 0x00015FCC
		private bool ReadPointerData(PanelEventHandler.PointerEvent pe, PointerEventData eventData, PanelEventHandler.PointerEventType eventType = PanelEventHandler.PointerEventType.Default)
		{
			if (this.eventSystem == null || this.eventSystem.currentInputModule == null)
			{
				return false;
			}
			pe.Read(this, eventData, eventType);
			Vector2 vector;
			Vector2 vector2;
			this.m_Panel.ScreenToPanel(pe.position, pe.deltaPosition, out vector, out vector2, true);
			pe.SetPosition(vector, vector2);
			return true;
		}

		// Token: 0x040001A5 RID: 421
		private BaseRuntimePanel m_Panel;

		// Token: 0x040001A6 RID: 422
		private readonly PanelEventHandler.PointerEvent m_PointerEvent = new PanelEventHandler.PointerEvent();

		// Token: 0x040001A7 RID: 423
		private float m_LastClickTime;

		// Token: 0x040001A8 RID: 424
		private bool m_Selecting;

		// Token: 0x040001A9 RID: 425
		private Event m_Event = new Event();

		// Token: 0x040001AA RID: 426
		private static EventModifiers s_Modifiers;

		// Token: 0x020000BF RID: 191
		private enum PointerEventType
		{
			// Token: 0x0400032F RID: 815
			Default,
			// Token: 0x04000330 RID: 816
			Down,
			// Token: 0x04000331 RID: 817
			Up
		}

		// Token: 0x020000C0 RID: 192
		private class PointerEvent : IPointerEvent
		{
			// Token: 0x170001DF RID: 479
			// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001C87E File Offset: 0x0001AA7E
			// (set) Token: 0x06000726 RID: 1830 RVA: 0x0001C886 File Offset: 0x0001AA86
			public int pointerId { get; private set; }

			// Token: 0x170001E0 RID: 480
			// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001C88F File Offset: 0x0001AA8F
			// (set) Token: 0x06000728 RID: 1832 RVA: 0x0001C897 File Offset: 0x0001AA97
			public string pointerType { get; private set; }

			// Token: 0x170001E1 RID: 481
			// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
			// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001C8A8 File Offset: 0x0001AAA8
			public bool isPrimary { get; private set; }

			// Token: 0x170001E2 RID: 482
			// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001C8B1 File Offset: 0x0001AAB1
			// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001C8B9 File Offset: 0x0001AAB9
			public int button { get; private set; }

			// Token: 0x170001E3 RID: 483
			// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001C8C2 File Offset: 0x0001AAC2
			// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001C8CA File Offset: 0x0001AACA
			public int pressedButtons { get; private set; }

			// Token: 0x170001E4 RID: 484
			// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001C8D3 File Offset: 0x0001AAD3
			// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001C8DB File Offset: 0x0001AADB
			public Vector3 position { get; private set; }

			// Token: 0x170001E5 RID: 485
			// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001C8E4 File Offset: 0x0001AAE4
			// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001C8EC File Offset: 0x0001AAEC
			public Vector3 localPosition { get; private set; }

			// Token: 0x170001E6 RID: 486
			// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001C8F5 File Offset: 0x0001AAF5
			// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001C8FD File Offset: 0x0001AAFD
			public Vector3 deltaPosition { get; private set; }

			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001C906 File Offset: 0x0001AB06
			// (set) Token: 0x06000736 RID: 1846 RVA: 0x0001C90E File Offset: 0x0001AB0E
			public float deltaTime { get; private set; }

			// Token: 0x170001E8 RID: 488
			// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001C917 File Offset: 0x0001AB17
			// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001C91F File Offset: 0x0001AB1F
			public int clickCount { get; private set; }

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001C928 File Offset: 0x0001AB28
			// (set) Token: 0x0600073A RID: 1850 RVA: 0x0001C930 File Offset: 0x0001AB30
			public float pressure { get; private set; }

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001C939 File Offset: 0x0001AB39
			// (set) Token: 0x0600073C RID: 1852 RVA: 0x0001C941 File Offset: 0x0001AB41
			public float tangentialPressure { get; private set; }

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x0600073D RID: 1853 RVA: 0x0001C94A File Offset: 0x0001AB4A
			// (set) Token: 0x0600073E RID: 1854 RVA: 0x0001C952 File Offset: 0x0001AB52
			public float altitudeAngle { get; private set; }

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001C95B File Offset: 0x0001AB5B
			// (set) Token: 0x06000740 RID: 1856 RVA: 0x0001C963 File Offset: 0x0001AB63
			public float azimuthAngle { get; private set; }

			// Token: 0x170001ED RID: 493
			// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001C96C File Offset: 0x0001AB6C
			// (set) Token: 0x06000742 RID: 1858 RVA: 0x0001C974 File Offset: 0x0001AB74
			public float twist { get; private set; }

			// Token: 0x170001EE RID: 494
			// (get) Token: 0x06000743 RID: 1859 RVA: 0x0001C97D File Offset: 0x0001AB7D
			// (set) Token: 0x06000744 RID: 1860 RVA: 0x0001C985 File Offset: 0x0001AB85
			public Vector2 tilt { get; private set; }

			// Token: 0x170001EF RID: 495
			// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001C98E File Offset: 0x0001AB8E
			// (set) Token: 0x06000746 RID: 1862 RVA: 0x0001C996 File Offset: 0x0001AB96
			public PenStatus penStatus { get; private set; }

			// Token: 0x170001F0 RID: 496
			// (get) Token: 0x06000747 RID: 1863 RVA: 0x0001C99F File Offset: 0x0001AB9F
			// (set) Token: 0x06000748 RID: 1864 RVA: 0x0001C9A7 File Offset: 0x0001ABA7
			public Vector2 radius { get; private set; }

			// Token: 0x170001F1 RID: 497
			// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001C9B0 File Offset: 0x0001ABB0
			// (set) Token: 0x0600074A RID: 1866 RVA: 0x0001C9B8 File Offset: 0x0001ABB8
			public Vector2 radiusVariance { get; private set; }

			// Token: 0x170001F2 RID: 498
			// (get) Token: 0x0600074B RID: 1867 RVA: 0x0001C9C1 File Offset: 0x0001ABC1
			// (set) Token: 0x0600074C RID: 1868 RVA: 0x0001C9C9 File Offset: 0x0001ABC9
			public EventModifiers modifiers { get; private set; }

			// Token: 0x170001F3 RID: 499
			// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001C9D2 File Offset: 0x0001ABD2
			public bool shiftKey
			{
				get
				{
					return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
				}
			}

			// Token: 0x170001F4 RID: 500
			// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001C9DF File Offset: 0x0001ABDF
			public bool ctrlKey
			{
				get
				{
					return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
				}
			}

			// Token: 0x170001F5 RID: 501
			// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001C9EC File Offset: 0x0001ABEC
			public bool commandKey
			{
				get
				{
					return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
				}
			}

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001C9F9 File Offset: 0x0001ABF9
			public bool altKey
			{
				get
				{
					return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
				}
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x06000751 RID: 1873 RVA: 0x0001CA06 File Offset: 0x0001AC06
			public bool actionKey
			{
				get
				{
					if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.OSXPlayer)
					{
						return this.ctrlKey;
					}
					return this.commandKey;
				}
			}

			// Token: 0x06000752 RID: 1874 RVA: 0x0001CA24 File Offset: 0x0001AC24
			public void Read(PanelEventHandler self, PointerEventData eventData, PanelEventHandler.PointerEventType eventType)
			{
				this.pointerId = self.eventSystem.currentInputModule.ConvertUIToolkitPointerId(eventData);
				this.pointerType = (PanelEventHandler.PointerEvent.<Read>g__InRange|90_0(this.pointerId, PointerId.touchPointerIdBase, PointerId.touchPointerCount) ? PointerType.touch : (PanelEventHandler.PointerEvent.<Read>g__InRange|90_0(this.pointerId, PointerId.penPointerIdBase, PointerId.penPointerCount) ? PointerType.pen : PointerType.mouse));
				this.isPrimary = this.pointerId == PointerId.mousePointerId || this.pointerId == PointerId.touchPointerIdBase || this.pointerId == PointerId.penPointerIdBase;
				int num = Screen.height;
				Vector3 relativeMousePositionForRaycast = MultipleDisplayUtilities.GetRelativeMousePositionForRaycast(eventData);
				int num2 = (int)relativeMousePositionForRaycast.z;
				if (num2 > 0 && num2 < Display.displays.Length)
				{
					num = Display.displays[num2].systemHeight;
				}
				Vector2 delta = eventData.delta;
				relativeMousePositionForRaycast.y = (float)num - relativeMousePositionForRaycast.y;
				delta.y = -delta.y;
				this.localPosition = (this.position = relativeMousePositionForRaycast);
				this.deltaPosition = delta;
				this.deltaTime = 0f;
				this.pressure = eventData.pressure;
				this.tangentialPressure = eventData.tangentialPressure;
				this.altitudeAngle = eventData.altitudeAngle;
				this.azimuthAngle = eventData.azimuthAngle;
				this.twist = eventData.twist;
				this.tilt = eventData.tilt;
				this.penStatus = eventData.penStatus;
				this.radius = eventData.radius;
				this.radiusVariance = eventData.radiusVariance;
				this.modifiers = PanelEventHandler.s_Modifiers;
				if (eventType == PanelEventHandler.PointerEventType.Default)
				{
					this.button = -1;
					this.clickCount = 0;
				}
				else
				{
					this.button = Mathf.Max(0, (int)eventData.button);
					this.clickCount = eventData.clickCount;
					if (eventType == PanelEventHandler.PointerEventType.Down)
					{
						if (Time.unscaledTime > self.m_LastClickTime + (float)ClickDetector.s_DoubleClickTime * 0.001f)
						{
							this.clickCount = 0;
						}
						int clickCount = this.clickCount;
						this.clickCount = clickCount + 1;
						PointerDeviceState.PressButton(this.pointerId, this.button);
					}
					else if (eventType == PanelEventHandler.PointerEventType.Up)
					{
						PointerDeviceState.ReleaseButton(this.pointerId, this.button);
					}
					this.clickCount = Mathf.Max(1, this.clickCount);
				}
				this.pressedButtons = PointerDeviceState.GetPressedButtons(this.pointerId);
			}

			// Token: 0x06000753 RID: 1875 RVA: 0x0001CC6C File Offset: 0x0001AE6C
			public void SetPosition(Vector3 positionOverride, Vector3 deltaOverride)
			{
				this.position = positionOverride;
				this.localPosition = positionOverride;
				this.deltaPosition = deltaOverride;
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x0001CC98 File Offset: 0x0001AE98
			[CompilerGenerated]
			internal static bool <Read>g__InRange|90_0(int i, int start, int count)
			{
				return i >= start && i < start + count;
			}
		}
	}
}
