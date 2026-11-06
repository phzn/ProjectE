using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x02000033 RID: 51
	public class Clickable : PointerManipulator
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001E2 RID: 482 RVA: 0x00005920 File Offset: 0x00003B20
		// (remove) Token: 0x060001E3 RID: 483 RVA: 0x00005958 File Offset: 0x00003B58
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<EventBase> clickedWithEventInfo;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001E4 RID: 484 RVA: 0x00005990 File Offset: 0x00003B90
		// (remove) Token: 0x060001E5 RID: 485 RVA: 0x000059C8 File Offset: 0x00003BC8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action clicked;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000059FD File Offset: 0x00003BFD
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00005A05 File Offset: 0x00003C05
		protected bool active { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00005A0E File Offset: 0x00003C0E
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00005A16 File Offset: 0x00003C16
		public Vector2 lastMousePosition { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00005A1F File Offset: 0x00003C1F
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00005A28 File Offset: 0x00003C28
		internal bool acceptClicksIfDisabled
		{
			get
			{
				return this.m_AcceptClicksIfDisabled;
			}
			set
			{
				bool flag = this.m_AcceptClicksIfDisabled == value;
				if (!flag)
				{
					this.UnregisterCallbacksFromTarget();
					this.m_AcceptClicksIfDisabled = value;
					this.RegisterCallbacksOnTarget();
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00005A5A File Offset: 0x00003C5A
		private InvokePolicy invokePolicy
		{
			get
			{
				return this.acceptClicksIfDisabled ? InvokePolicy.IncludeDisabled : InvokePolicy.Default;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00005A68 File Offset: 0x00003C68
		public Clickable(Action handler, long delay, long interval)
			: this(handler)
		{
			this.m_Delay = delay;
			this.m_Interval = interval;
			this.active = false;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00005A8C File Offset: 0x00003C8C
		public Clickable(Action<EventBase> handler)
		{
			this.m_ActivePointerId = -1;
			base..ctor();
			this.clickedWithEventInfo = handler;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00005AD0 File Offset: 0x00003CD0
		public Clickable(Action handler)
		{
			this.m_ActivePointerId = -1;
			base..ctor();
			this.clicked = handler;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			this.active = false;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00005B1C File Offset: 0x00003D1C
		private void OnTimer(TimerState timerState)
		{
			bool flag = (this.clicked != null || this.clickedWithEventInfo != null) && this.IsRepeatable();
			if (flag)
			{
				bool flag2 = this.ContainsPointer(this.m_ActivePointerId) && (base.target.enabledInHierarchy || this.acceptClicksIfDisabled);
				if (flag2)
				{
					this.Invoke(null);
					base.target.pseudoStates |= PseudoStates.Active;
				}
				else
				{
					base.target.pseudoStates &= ~PseudoStates.Active;
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00005BAC File Offset: 0x00003DAC
		private bool IsRepeatable()
		{
			return this.m_Delay > 0L || this.m_Interval > 0L;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00005BD8 File Offset: 0x00003DD8
		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), this.invokePolicy, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), this.invokePolicy, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseCaptureOutEvent>(new EventCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), this.invokePolicy, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), this.invokePolicy, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCaptureOut), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00005CE4 File Offset: 0x00003EE4
		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<MouseCaptureOutEvent>(new EventCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCaptureOut), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00005DD4 File Offset: 0x00003FD4
		protected void OnMouseDown(MouseDownEvent evt)
		{
			bool flag = base.CanStartManipulation(evt);
			if (flag)
			{
				this.ProcessDownEvent(evt, evt.localMousePosition, PointerId.mousePointerId);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00005E00 File Offset: 0x00004000
		protected void OnMouseMove(MouseMoveEvent evt)
		{
			bool active = this.active;
			if (active)
			{
				this.ProcessMoveEvent(evt, evt.localMousePosition);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00005E28 File Offset: 0x00004028
		protected void OnMouseUp(MouseUpEvent evt)
		{
			bool flag = this.active && base.CanStopManipulation(evt);
			if (flag)
			{
				this.ProcessUpEvent(evt, evt.localMousePosition, PointerId.mousePointerId);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00005E60 File Offset: 0x00004060
		private void OnMouseCaptureOut(MouseCaptureOutEvent evt)
		{
			bool active = this.active;
			if (active)
			{
				this.ProcessCancelEvent(evt, PointerId.mousePointerId);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00005E88 File Offset: 0x00004088
		private void OnPointerDown(PointerDownEvent evt)
		{
			bool flag = !base.CanStartManipulation(evt);
			if (!flag)
			{
				bool flag2 = evt.pointerId != PointerId.mousePointerId;
				if (flag2)
				{
					this.ProcessDownEvent(evt, evt.localPosition, evt.pointerId);
					base.target.panel.PreventCompatibilityMouseEvents(evt.pointerId);
				}
				else
				{
					evt.StopImmediatePropagation();
				}
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00005EF8 File Offset: 0x000040F8
		private void OnPointerMove(PointerMoveEvent evt)
		{
			bool flag = !this.active;
			if (!flag)
			{
				bool flag2 = evt.pointerId != PointerId.mousePointerId;
				if (flag2)
				{
					this.ProcessMoveEvent(evt, evt.localPosition);
					base.target.panel.PreventCompatibilityMouseEvents(evt.pointerId);
				}
				else
				{
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00005F60 File Offset: 0x00004160
		private void OnPointerUp(PointerUpEvent evt)
		{
			bool flag = !this.active || !base.CanStopManipulation(evt);
			if (!flag)
			{
				bool flag2 = evt.pointerId != PointerId.mousePointerId;
				if (flag2)
				{
					this.ProcessUpEvent(evt, evt.localPosition, evt.pointerId);
					base.target.panel.PreventCompatibilityMouseEvents(evt.pointerId);
				}
				else
				{
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00005FDC File Offset: 0x000041DC
		private void OnPointerCancel(PointerCancelEvent evt)
		{
			bool flag = !this.active || !base.CanStopManipulation(evt);
			if (!flag)
			{
				bool flag2 = Clickable.IsNotMouseEvent(evt.pointerId);
				if (flag2)
				{
					this.ProcessCancelEvent(evt, evt.pointerId);
				}
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00006024 File Offset: 0x00004224
		private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
		{
			bool flag = !this.active;
			if (!flag)
			{
				bool flag2 = Clickable.IsNotMouseEvent(evt.pointerId);
				if (flag2)
				{
					this.ProcessCancelEvent(evt, evt.pointerId);
				}
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00006060 File Offset: 0x00004260
		private bool ContainsPointer(int pointerId)
		{
			VisualElement topElementUnderPointer = base.target.elementPanel.GetTopElementUnderPointer(pointerId);
			return base.target == topElementUnderPointer || base.target.Contains(topElementUnderPointer);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000609C File Offset: 0x0000429C
		private static bool IsNotMouseEvent(int pointerId)
		{
			return pointerId != PointerId.mousePointerId;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000060B9 File Offset: 0x000042B9
		protected void Invoke(EventBase evt)
		{
			Action action = this.clicked;
			if (action != null)
			{
				action();
			}
			Action<EventBase> action2 = this.clickedWithEventInfo;
			if (action2 != null)
			{
				action2(evt);
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000060E4 File Offset: 0x000042E4
		internal void SimulateSingleClick(EventBase evt, int delayMs = 100)
		{
			base.target.pseudoStates |= PseudoStates.Active;
			base.target.schedule.Execute(delegate
			{
				base.target.pseudoStates &= ~PseudoStates.Active;
			}).ExecuteLater((long)delayMs);
			this.Invoke(evt);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00006134 File Offset: 0x00004334
		protected virtual void ProcessDownEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			this.active = true;
			this.m_ActivePointerId = pointerId;
			base.target.CapturePointer(pointerId);
			bool flag = !(evt is IPointerEvent);
			if (flag)
			{
				base.target.panel.ProcessPointerCapture(pointerId);
			}
			this.lastMousePosition = localPosition;
			bool flag2 = this.IsRepeatable();
			if (flag2)
			{
				bool flag3 = this.ContainsPointer(pointerId) && (base.target.enabledInHierarchy || this.acceptClicksIfDisabled);
				if (flag3)
				{
					this.Invoke(evt);
				}
				bool flag4 = this.m_Repeater == null;
				if (flag4)
				{
					this.m_Repeater = base.target.schedule.Execute(new Action<TimerState>(this.OnTimer)).Every(this.m_Interval).StartingIn(this.m_Delay);
				}
				else
				{
					this.m_Repeater.ExecuteLater(this.m_Delay);
				}
			}
			base.target.pseudoStates |= PseudoStates.Active;
			evt.StopImmediatePropagation();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006240 File Offset: 0x00004440
		protected virtual void ProcessMoveEvent(EventBase evt, Vector2 localPosition)
		{
			this.lastMousePosition = localPosition;
			bool flag = this.ContainsPointer(this.m_ActivePointerId);
			if (flag)
			{
				base.target.pseudoStates |= PseudoStates.Active;
			}
			else
			{
				base.target.pseudoStates &= ~PseudoStates.Active;
			}
			evt.StopPropagation();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000629C File Offset: 0x0000449C
		protected virtual void ProcessUpEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			this.active = false;
			this.m_ActivePointerId = -1;
			base.target.ReleasePointer(pointerId);
			bool flag = !(evt is IPointerEvent);
			if (flag)
			{
				base.target.panel.ProcessPointerCapture(pointerId);
			}
			base.target.pseudoStates &= ~PseudoStates.Active;
			bool flag2 = this.IsRepeatable();
			if (flag2)
			{
				IVisualElementScheduledItem repeater = this.m_Repeater;
				if (repeater != null)
				{
					repeater.Pause();
				}
			}
			else
			{
				bool flag3 = this.ContainsPointer(pointerId) && (base.target.enabledInHierarchy || this.acceptClicksIfDisabled);
				if (flag3)
				{
					this.Invoke(evt);
				}
			}
			evt.StopPropagation();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00006358 File Offset: 0x00004558
		protected virtual void ProcessCancelEvent(EventBase evt, int pointerId)
		{
			this.active = false;
			this.m_ActivePointerId = -1;
			base.target.ReleasePointer(pointerId);
			bool flag = !(evt is IPointerEvent);
			if (flag)
			{
				base.target.panel.ProcessPointerCapture(pointerId);
			}
			base.target.pseudoStates &= ~PseudoStates.Active;
			bool flag2 = this.IsRepeatable();
			if (flag2)
			{
				IVisualElementScheduledItem repeater = this.m_Repeater;
				if (repeater != null)
				{
					repeater.Pause();
				}
			}
			evt.StopPropagation();
		}

		// Token: 0x0400009B RID: 155
		private readonly long m_Delay;

		// Token: 0x0400009C RID: 156
		private readonly long m_Interval;

		// Token: 0x0400009F RID: 159
		private int m_ActivePointerId;

		// Token: 0x040000A0 RID: 160
		private bool m_AcceptClicksIfDisabled;

		// Token: 0x040000A1 RID: 161
		private IVisualElementScheduledItem m_Repeater;
	}
}
