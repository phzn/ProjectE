using System;
using System.Diagnostics;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004C6 RID: 1222
	internal class ColumnMover : PointerManipulator
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x0009FA8D File Offset: 0x0009DC8D
		// (set) Token: 0x0600262E RID: 9774 RVA: 0x0009FA95 File Offset: 0x0009DC95
		public ColumnLayout columnLayout { get; set; }

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x0009FA9E File Offset: 0x0009DC9E
		// (set) Token: 0x06002630 RID: 9776 RVA: 0x0009FAA8 File Offset: 0x0009DCA8
		public bool active
		{
			get
			{
				return this.m_Active;
			}
			set
			{
				bool flag = this.m_Active == value;
				if (!flag)
				{
					this.m_Active = value;
					Action<ColumnMover> action = this.activeChanged;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002631 RID: 9777 RVA: 0x0009FADF File Offset: 0x0009DCDF
		// (set) Token: 0x06002632 RID: 9778 RVA: 0x0009FAE8 File Offset: 0x0009DCE8
		public bool moving
		{
			get
			{
				return this.m_Moving;
			}
			set
			{
				bool flag = this.m_Moving == value;
				if (!flag)
				{
					this.m_Moving = value;
					Action<ColumnMover> action = this.movingChanged;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002633 RID: 9779 RVA: 0x0009FB20 File Offset: 0x0009DD20
		// (remove) Token: 0x06002634 RID: 9780 RVA: 0x0009FB58 File Offset: 0x0009DD58
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ColumnMover> activeChanged;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06002635 RID: 9781 RVA: 0x0009FB90 File Offset: 0x0009DD90
		// (remove) Token: 0x06002636 RID: 9782 RVA: 0x0009FBC8 File Offset: 0x0009DDC8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ColumnMover> movingChanged;

		// Token: 0x06002637 RID: 9783 RVA: 0x0009FC00 File Offset: 0x0009DE00
		public ColumnMover()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0009FC34 File Offset: 0x0009DE34
		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<MouseCaptureOutEvent>(new EventCallback<MouseCaptureOutEvent>(this.OnMouseCaptureOut), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCaptureOut), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0009FD3C File Offset: 0x0009DF3C
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
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0009FE44 File Offset: 0x0009E044
		protected void OnMouseDown(MouseDownEvent evt)
		{
			bool flag = base.CanStartManipulation(evt);
			if (flag)
			{
				this.ProcessDownEvent(evt, evt.localMousePosition, PointerId.mousePointerId);
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0009FE70 File Offset: 0x0009E070
		protected void OnMouseMove(MouseMoveEvent evt)
		{
			bool active = this.active;
			if (active)
			{
				this.ProcessMoveEvent(evt, evt.localMousePosition);
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0009FE98 File Offset: 0x0009E098
		protected void OnMouseUp(MouseUpEvent evt)
		{
			bool flag = this.active && base.CanStopManipulation(evt);
			if (flag)
			{
				this.ProcessUpEvent(evt, evt.localMousePosition, PointerId.mousePointerId);
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0009FED0 File Offset: 0x0009E0D0
		private void OnMouseCaptureOut(MouseCaptureOutEvent evt)
		{
			bool active = this.active;
			if (active)
			{
				this.ProcessCancelEvent(evt, PointerId.mousePointerId);
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0009FEF8 File Offset: 0x0009E0F8
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

		// Token: 0x0600263F RID: 9791 RVA: 0x0009FF68 File Offset: 0x0009E168
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

		// Token: 0x06002640 RID: 9792 RVA: 0x0009FFD0 File Offset: 0x0009E1D0
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

		// Token: 0x06002641 RID: 9793 RVA: 0x000A004C File Offset: 0x0009E24C
		private void OnPointerCancel(PointerCancelEvent evt)
		{
			bool flag = !this.active || !base.CanStopManipulation(evt);
			if (!flag)
			{
				bool flag2 = ColumnMover.IsNotMouseEvent(evt.pointerId);
				if (flag2)
				{
					this.ProcessCancelEvent(evt, evt.pointerId);
				}
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000A0094 File Offset: 0x0009E294
		private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
		{
			bool flag = !this.active;
			if (!flag)
			{
				bool flag2 = ColumnMover.IsNotMouseEvent(evt.pointerId);
				if (flag2)
				{
					this.ProcessCancelEvent(evt, evt.pointerId);
				}
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000A00D0 File Offset: 0x0009E2D0
		private static bool IsNotMouseEvent(int pointerId)
		{
			return pointerId != PointerId.mousePointerId;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000A00F0 File Offset: 0x0009E2F0
		protected void ProcessCancelEvent(EventBase evt, int pointerId)
		{
			this.active = false;
			base.target.ReleasePointer(pointerId);
			bool flag = !(evt is IPointerEvent);
			if (flag)
			{
				base.target.panel.ProcessPointerCapture(pointerId);
			}
			bool moving = this.moving;
			if (moving)
			{
				this.EndDragMove(true);
			}
			evt.StopPropagation();
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000A0150 File Offset: 0x0009E350
		private void OnKeyDown(KeyDownEvent e)
		{
			bool flag = e.keyCode == KeyCode.Escape && this.moving;
			if (flag)
			{
				this.EndDragMove(true);
			}
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000A0180 File Offset: 0x0009E380
		private void ProcessDownEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			bool active = this.active;
			if (active)
			{
				evt.StopImmediatePropagation();
			}
			else
			{
				base.target.CapturePointer(pointerId);
				bool flag = !(evt is IPointerEvent);
				if (flag)
				{
					base.target.panel.ProcessPointerCapture(pointerId);
				}
				VisualElement visualElement = evt.currentTarget as VisualElement;
				MultiColumnCollectionHeader firstAncestorOfType = visualElement.GetFirstAncestorOfType<MultiColumnCollectionHeader>();
				bool flag2 = !firstAncestorOfType.columns.reorderable;
				if (!flag2)
				{
					this.m_Header = firstAncestorOfType;
					Vector2 vector = visualElement.ChangeCoordinatesTo(this.m_Header, localPosition);
					this.columnLayout = this.m_Header.columnLayout;
					this.m_Cancelled = false;
					this.m_StartPos = vector.x;
					this.active = true;
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000A0248 File Offset: 0x0009E448
		private void ProcessMoveEvent(EventBase e, Vector2 localPosition)
		{
			bool cancelled = this.m_Cancelled;
			if (!cancelled)
			{
				VisualElement visualElement = e.currentTarget as VisualElement;
				Vector2 vector = visualElement.ChangeCoordinatesTo(this.m_Header, localPosition);
				bool flag = !this.moving && Mathf.Abs(this.m_StartPos - vector.x) > 5f;
				if (flag)
				{
					this.BeginDragMove(this.m_StartPos);
				}
				bool moving = this.moving;
				if (moving)
				{
					this.DragMove(vector.x);
				}
				e.StopPropagation();
			}
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000A02D8 File Offset: 0x0009E4D8
		private void ProcessUpEvent(EventBase evt, Vector2 localPosition, int pointerId)
		{
			this.active = false;
			base.target.ReleasePointer(pointerId);
			bool flag = !(evt is IPointerEvent);
			if (flag)
			{
				base.target.panel.ProcessPointerCapture(pointerId);
			}
			bool flag2 = this.moving || this.m_Cancelled;
			this.EndDragMove(false);
			bool flag3 = flag2;
			if (flag3)
			{
				evt.StopImmediatePropagation();
			}
			else
			{
				evt.StopPropagation();
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000A034C File Offset: 0x0009E54C
		private void BeginDragMove(float pos)
		{
			float num = 0f;
			Columns columns = this.columnLayout.columns;
			foreach (Column column in columns.visibleList)
			{
				num += this.columnLayout.GetDesiredWidth(column);
				bool flag = this.m_ColumnToMove == null;
				if (flag)
				{
					bool flag2 = num > pos;
					if (flag2)
					{
						this.m_ColumnToMove = column;
					}
				}
			}
			this.moving = true;
			this.m_LastPos = pos;
			this.m_PreviewElement = new MultiColumnHeaderColumnMovePreview();
			this.m_LocationPreviewElement = new MultiColumnHeaderColumnMoveLocationPreview();
			this.m_Header.hierarchy.Add(this.m_PreviewElement);
			ScrollView firstAncestorOfType = this.m_Header.GetFirstAncestorOfType<ScrollView>();
			VisualElement visualElement = ((firstAncestorOfType != null) ? firstAncestorOfType.parent : null) ?? this.m_Header;
			visualElement.hierarchy.Add(this.m_LocationPreviewElement);
			this.m_ColumnToMovePos = this.columnLayout.GetDesiredPosition(this.m_ColumnToMove);
			this.m_ColumnToMoveWidth = this.columnLayout.GetDesiredWidth(this.m_ColumnToMove);
			this.UpdateMoveLocation();
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000A048C File Offset: 0x0009E68C
		internal void DragMove(float pos)
		{
			this.m_LastPos = pos;
			this.UpdateMoveLocation();
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000A04A0 File Offset: 0x0009E6A0
		private void UpdatePreviewPosition()
		{
			this.m_PreviewElement.style.left = this.m_ColumnToMovePos + this.m_LastPos - this.m_StartPos;
			this.m_PreviewElement.style.width = this.m_ColumnToMoveWidth;
			bool flag = this.m_DestinationColumn != null;
			if (flag)
			{
				this.m_LocationPreviewElement.style.left = this.columnLayout.GetDesiredPosition(this.m_DestinationColumn) + ((!this.m_MoveBeforeDestination) ? this.columnLayout.GetDesiredWidth(this.m_DestinationColumn) : 0f);
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000A054C File Offset: 0x0009E74C
		private void UpdateMoveLocation()
		{
			float num = 0f;
			this.m_DestinationColumn = null;
			this.m_MoveBeforeDestination = false;
			foreach (Column column in this.columnLayout.columns.visibleList)
			{
				this.m_DestinationColumn = column;
				float desiredWidth = this.columnLayout.GetDesiredWidth(this.m_DestinationColumn);
				float num2 = num + desiredWidth / 2f;
				num += desiredWidth;
				bool flag = num > this.m_LastPos;
				if (flag)
				{
					this.m_MoveBeforeDestination = this.m_LastPos < num2;
					break;
				}
			}
			this.UpdatePreviewPosition();
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000A0608 File Offset: 0x0009E808
		private void EndDragMove(bool cancelled)
		{
			bool flag = !this.moving || this.m_Cancelled;
			if (!flag)
			{
				this.m_Cancelled = cancelled;
				bool flag2 = !cancelled;
				if (flag2)
				{
					int num = this.m_DestinationColumn.displayIndex;
					bool flag3 = !this.m_MoveBeforeDestination;
					if (flag3)
					{
						num++;
					}
					bool flag4 = this.m_ColumnToMove.displayIndex < num;
					if (flag4)
					{
						num--;
					}
					bool flag5 = this.m_ColumnToMove.displayIndex != num;
					if (flag5)
					{
						this.columnLayout.columns.ReorderDisplay(this.m_ColumnToMove.displayIndex, num);
					}
				}
				VisualElement previewElement = this.m_PreviewElement;
				if (previewElement != null)
				{
					previewElement.RemoveFromHierarchy();
				}
				this.m_PreviewElement = null;
				MultiColumnHeaderColumnMoveLocationPreview locationPreviewElement = this.m_LocationPreviewElement;
				if (locationPreviewElement != null)
				{
					locationPreviewElement.RemoveFromHierarchy();
				}
				this.m_LocationPreviewElement = null;
				this.m_ColumnToMove = null;
				this.moving = false;
			}
		}

		// Token: 0x04001255 RID: 4693
		private const float k_StartDragDistance = 5f;

		// Token: 0x04001256 RID: 4694
		private float m_StartPos;

		// Token: 0x04001257 RID: 4695
		private float m_LastPos;

		// Token: 0x04001258 RID: 4696
		private bool m_Active;

		// Token: 0x04001259 RID: 4697
		private bool m_Moving;

		// Token: 0x0400125A RID: 4698
		private bool m_Cancelled;

		// Token: 0x0400125B RID: 4699
		private MultiColumnCollectionHeader m_Header;

		// Token: 0x0400125C RID: 4700
		private VisualElement m_PreviewElement;

		// Token: 0x0400125D RID: 4701
		private MultiColumnHeaderColumnMoveLocationPreview m_LocationPreviewElement;

		// Token: 0x0400125E RID: 4702
		private Column m_ColumnToMove;

		// Token: 0x0400125F RID: 4703
		private float m_ColumnToMovePos;

		// Token: 0x04001260 RID: 4704
		private float m_ColumnToMoveWidth;

		// Token: 0x04001261 RID: 4705
		private Column m_DestinationColumn;

		// Token: 0x04001262 RID: 4706
		private bool m_MoveBeforeDestination;
	}
}
