using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x02000031 RID: 49
	internal class ClampedDragger<T> : Clickable where T : IComparable<T>
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001D3 RID: 467 RVA: 0x000056F8 File Offset: 0x000038F8
		// (remove) Token: 0x060001D4 RID: 468 RVA: 0x00005730 File Offset: 0x00003930
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action dragging;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001D5 RID: 469 RVA: 0x00005768 File Offset: 0x00003968
		// (remove) Token: 0x060001D6 RID: 470 RVA: 0x000057A0 File Offset: 0x000039A0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action draggingEnded;

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000057D5 File Offset: 0x000039D5
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x000057DD File Offset: 0x000039DD
		public ClampedDragger<T>.DragDirection dragDirection { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000057E6 File Offset: 0x000039E6
		// (set) Token: 0x060001DA RID: 474 RVA: 0x000057EE File Offset: 0x000039EE
		private BaseSlider<T> slider { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000057F7 File Offset: 0x000039F7
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000057FF File Offset: 0x000039FF
		public Vector2 startMousePosition { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00005808 File Offset: 0x00003A08
		public Vector2 delta
		{
			get
			{
				return base.lastMousePosition - this.startMousePosition;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000581B File Offset: 0x00003A1B
		public ClampedDragger(BaseSlider<T> slider, Action clickHandler, Action dragHandler)
			: base(clickHandler, 250L, 30L)
		{
			this.dragDirection = ClampedDragger<T>.DragDirection.None;
			this.slider = slider;
			this.dragging += dragHandler;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00005847 File Offset: 0x00003A47
		protected override void ProcessDownEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			this.startMousePosition = localPosition;
			this.dragDirection = ClampedDragger<T>.DragDirection.None;
			base.ProcessDownEvent(evt, localPosition, pointerId);
			Action action = this.dragging;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00005876 File Offset: 0x00003A76
		protected override void ProcessUpEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			base.ProcessUpEvent(evt, localPosition, pointerId);
			Action action = this.draggingEnded;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00005898 File Offset: 0x00003A98
		protected override void ProcessMoveEvent(EventBase evt, Vector2 localPosition)
		{
			base.ProcessMoveEvent(evt, localPosition);
			bool flag = this.dragDirection == ClampedDragger<T>.DragDirection.None;
			if (flag)
			{
				this.dragDirection = ClampedDragger<T>.DragDirection.Free;
			}
			bool flag2 = this.dragDirection == ClampedDragger<T>.DragDirection.Free;
			if (flag2)
			{
				bool flag3 = evt.eventTypeId == EventBase<PointerMoveEvent>.TypeId();
				if (flag3)
				{
					PointerMoveEvent pointerMoveEvent = (PointerMoveEvent)evt;
					bool flag4 = pointerMoveEvent.pointerId != PointerId.mousePointerId;
					if (flag4)
					{
						pointerMoveEvent.isHandledByDraggable = true;
					}
				}
				Action action = this.dragging;
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x02000032 RID: 50
		[Flags]
		public enum DragDirection
		{
			// Token: 0x04000095 RID: 149
			None = 0,
			// Token: 0x04000096 RID: 150
			LowToHigh = 1,
			// Token: 0x04000097 RID: 151
			HighToLow = 2,
			// Token: 0x04000098 RID: 152
			Free = 4
		}
	}
}
