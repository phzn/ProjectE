using System;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x02000162 RID: 354
	internal abstract class DragEventsProcessor
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0002E02C File Offset: 0x0002C22C
		internal bool isRegistered
		{
			get
			{
				return this.m_IsRegistered;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002E034 File Offset: 0x0002C234
		internal DragEventsProcessor.DragState dragState
		{
			get
			{
				return this.m_DragState;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0000C927 File Offset: 0x0000AB27
		protected virtual bool supportsDragEvents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0002E03C File Offset: 0x0002C23C
		private bool useDragEvents
		{
			get
			{
				return this.isEditorContext && this.supportsDragEvents;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0002E04F File Offset: 0x0002C24F
		protected IDragAndDrop dragAndDrop
		{
			get
			{
				return DragAndDropUtility.GetDragAndDrop(this.m_Target.panel);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0002E064 File Offset: 0x0002C264
		internal virtual bool isEditorContext
		{
			get
			{
				Assert.IsNotNull<VisualElement>(this.m_Target);
				Assert.IsNotNull<VisualElement>(this.m_Target.parent);
				return this.m_Target.panel.contextType == ContextType.Editor;
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002E0A8 File Offset: 0x0002C2A8
		internal DragEventsProcessor(VisualElement target)
		{
			this.m_Target = target;
			this.m_Target.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.RegisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			this.m_Target.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.UnregisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			this.RegisterCallbacksFromTarget();
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002E0FD File Offset: 0x0002C2FD
		private void RegisterCallbacksFromTarget(AttachToPanelEvent evt)
		{
			this.RegisterCallbacksFromTarget();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002E108 File Offset: 0x0002C308
		private void RegisterCallbacksFromTarget()
		{
			bool isRegistered = this.m_IsRegistered;
			if (!isRegistered)
			{
				this.m_IsRegistered = true;
				this.m_Target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUpEvent), TrickleDown.TrickleDown);
				this.m_Target.RegisterCallback<PointerLeaveEvent>(new EventCallback<PointerLeaveEvent>(this.OnPointerLeaveEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMoveEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancelEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCapturedOut), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002E1C2 File Offset: 0x0002C3C2
		private void UnregisterCallbacksFromTarget(DetachFromPanelEvent evt)
		{
			this.UnregisterCallbacksFromTarget(false);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002E1D0 File Offset: 0x0002C3D0
		internal void UnregisterCallbacksFromTarget(bool unregisterPanelEvents = false)
		{
			this.m_IsRegistered = false;
			this.m_Target.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUpEvent), TrickleDown.TrickleDown);
			this.m_Target.UnregisterCallback<PointerLeaveEvent>(new EventCallback<PointerLeaveEvent>(this.OnPointerLeaveEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMoveEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancelEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCapturedOut), TrickleDown.NoTrickleDown);
			if (unregisterPanelEvents)
			{
				this.m_Target.UnregisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.RegisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
				this.m_Target.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.UnregisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000B9E RID: 2974
		protected abstract bool CanStartDrag(Vector3 pointerPosition);

		// Token: 0x06000B9F RID: 2975
		protected internal abstract StartDragArgs StartDrag(Vector3 pointerPosition);

		// Token: 0x06000BA0 RID: 2976
		protected internal abstract void UpdateDrag(Vector3 pointerPosition);

		// Token: 0x06000BA1 RID: 2977
		protected internal abstract void OnDrop(Vector3 pointerPosition);

		// Token: 0x06000BA2 RID: 2978
		protected abstract void ClearDragAndDropUI(bool dragCancelled);

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002E2B4 File Offset: 0x0002C4B4
		private void OnPointerDownEvent(PointerDownEvent evt)
		{
			bool flag;
			if (evt.button == 0)
			{
				VisualElement visualElement = evt.leafTarget as VisualElement;
				flag = visualElement != null && visualElement.isIMGUIContainer;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.m_DragState = DragEventsProcessor.DragState.None;
			}
			else
			{
				bool flag3 = this.CanStartDrag(evt.position);
				if (flag3)
				{
					this.m_DragState = DragEventsProcessor.DragState.CanStartDrag;
					this.m_Start = evt.position;
				}
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002E318 File Offset: 0x0002C518
		internal void OnPointerUpEvent(PointerUpEvent evt)
		{
			bool flag = !this.useDragEvents && this.m_DragState == DragEventsProcessor.DragState.Dragging;
			if (flag)
			{
				DragEventsProcessor dragEventsProcessor = this.GetDropTarget(evt.position) ?? this;
				dragEventsProcessor.UpdateDrag(evt.position);
				dragEventsProcessor.OnDrop(evt.position);
				dragEventsProcessor.ClearDragAndDropUI(false);
				evt.StopPropagation();
			}
			this.m_Target.ReleasePointer(evt.pointerId);
			this.ClearDragAndDropUI(this.m_DragState == DragEventsProcessor.DragState.Dragging);
			this.dragAndDrop.DragCleanup();
			this.m_DragState = DragEventsProcessor.DragState.None;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002E3B5 File Offset: 0x0002C5B5
		private void OnPointerLeaveEvent(PointerLeaveEvent evt)
		{
			this.ClearDragAndDropUI(false);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002E3C0 File Offset: 0x0002C5C0
		private void OnPointerCancelEvent(PointerCancelEvent evt)
		{
			bool flag = !this.useDragEvents;
			if (flag)
			{
				this.ClearDragAndDropUI(true);
			}
			this.m_Target.ReleasePointer(evt.pointerId);
			this.ClearDragAndDropUI(this.m_DragState == DragEventsProcessor.DragState.Dragging);
			this.dragAndDrop.DragCleanup();
			this.m_DragState = DragEventsProcessor.DragState.None;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002E418 File Offset: 0x0002C618
		private void OnPointerCapturedOut(PointerCaptureOutEvent evt)
		{
			bool flag = !this.useDragEvents;
			if (flag)
			{
				this.ClearDragAndDropUI(true);
			}
			this.ClearDragAndDropUI(this.m_DragState == DragEventsProcessor.DragState.Dragging);
			this.dragAndDrop.DragCleanup();
			this.m_DragState = DragEventsProcessor.DragState.None;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002E460 File Offset: 0x0002C660
		private void OnPointerMoveEvent(PointerMoveEvent evt)
		{
			bool isHandledByDraggable = evt.isHandledByDraggable;
			if (!isHandledByDraggable)
			{
				bool flag = !this.useDragEvents && this.m_DragState == DragEventsProcessor.DragState.Dragging;
				if (flag)
				{
					DragEventsProcessor dragEventsProcessor = this.GetDropTarget(evt.position) ?? this;
					dragEventsProcessor.UpdateDrag(evt.position);
				}
				else
				{
					bool flag2 = this.m_DragState != DragEventsProcessor.DragState.CanStartDrag;
					if (!flag2)
					{
						bool flag3 = (this.m_Start - evt.position).sqrMagnitude >= 100f;
						if (flag3)
						{
							StartDragArgs startDragArgs = this.StartDrag(this.m_Start);
							bool flag4 = startDragArgs.visualMode == DragVisualMode.Rejected;
							if (flag4)
							{
								this.m_DragState = DragEventsProcessor.DragState.None;
							}
							else
							{
								bool flag5 = !this.useDragEvents;
								if (flag5)
								{
									bool supportsDragEvents = this.supportsDragEvents;
									if (supportsDragEvents)
									{
										this.dragAndDrop.StartDrag(startDragArgs, evt.position);
									}
								}
								else
								{
									bool flag6 = Event.current != null && Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseDrag;
									if (flag6)
									{
										return;
									}
									this.dragAndDrop.StartDrag(startDragArgs, evt.position);
								}
								this.m_DragState = DragEventsProcessor.DragState.Dragging;
								this.m_Target.CapturePointer(evt.pointerId);
								evt.isHandledByDraggable = true;
								evt.StopPropagation();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002E5D4 File Offset: 0x0002C7D4
		private DragEventsProcessor GetDropTarget(Vector2 position)
		{
			DragEventsProcessor dragEventsProcessor = null;
			bool flag = this.m_Target.worldBound.Contains(position);
			if (flag)
			{
				dragEventsProcessor = this;
			}
			else
			{
				bool supportsDragEvents = this.supportsDragEvents;
				if (supportsDragEvents)
				{
					VisualElement visualElement = this.m_Target.elementPanel.Pick(position);
					BaseVerticalCollectionView baseVerticalCollectionView = ((visualElement != null) ? visualElement.GetFirstOfType<BaseVerticalCollectionView>() : null);
					dragEventsProcessor = ((baseVerticalCollectionView != null) ? baseVerticalCollectionView.dragger : null);
				}
			}
			return dragEventsProcessor;
		}

		// Token: 0x04000577 RID: 1399
		private bool m_IsRegistered;

		// Token: 0x04000578 RID: 1400
		private DragEventsProcessor.DragState m_DragState;

		// Token: 0x04000579 RID: 1401
		private Vector3 m_Start;

		// Token: 0x0400057A RID: 1402
		protected readonly VisualElement m_Target;

		// Token: 0x02000163 RID: 355
		internal enum DragState
		{
			// Token: 0x0400057C RID: 1404
			None,
			// Token: 0x0400057D RID: 1405
			CanStartDrag,
			// Token: 0x0400057E RID: 1406
			Dragging
		}
	}
}
