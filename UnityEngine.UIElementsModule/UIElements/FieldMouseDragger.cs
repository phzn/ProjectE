using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000244 RID: 580
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class FieldMouseDragger<T> : BaseFieldMouseDragger
	{
		// Token: 0x06001071 RID: 4209 RVA: 0x0003B9FE File Offset: 0x00039BFE
		public FieldMouseDragger(IValueField<T> drivenField)
		{
			this.m_DrivenField = drivenField;
			this.m_DragElement = null;
			this.m_DragHotZone = new Rect(0f, 0f, -1f, -1f);
			this.dragging = false;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0003BA3D File Offset: 0x00039C3D
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x0003BA45 File Offset: 0x00039C45
		public bool dragging { get; set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0003BA4E File Offset: 0x00039C4E
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x0003BA56 File Offset: 0x00039C56
		public T startValue { get; set; }

		// Token: 0x06001076 RID: 4214 RVA: 0x0003BA60 File Offset: 0x00039C60
		public sealed override void SetDragZone(VisualElement dragElement, Rect hotZone)
		{
			bool flag = this.m_DragElement != null;
			if (flag)
			{
				this.m_DragElement.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.UpdateValueOnPointerDown), TrickleDown.NoTrickleDown);
				this.m_DragElement.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.UpdateValueOnPointerUp), TrickleDown.NoTrickleDown);
				this.m_DragElement.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.UpdateValueOnKeyDown), TrickleDown.NoTrickleDown);
			}
			this.m_DragElement = dragElement;
			this.m_DragHotZone = hotZone;
			bool flag2 = this.m_DragElement != null;
			if (flag2)
			{
				this.dragging = false;
				this.m_DragElement.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.UpdateValueOnPointerDown), TrickleDown.NoTrickleDown);
				this.m_DragElement.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.UpdateValueOnPointerUp), TrickleDown.NoTrickleDown);
				this.m_DragElement.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.UpdateValueOnKeyDown), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0003BB38 File Offset: 0x00039D38
		private bool CanStartDrag(int button, Vector2 localPosition)
		{
			return button == 0 && (this.m_DragHotZone.width < 0f || this.m_DragHotZone.height < 0f || this.m_DragHotZone.Contains(this.m_DragElement.WorldToLocal(localPosition)));
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0003BB90 File Offset: 0x00039D90
		private void UpdateValueOnPointerDown(PointerDownEvent evt)
		{
			bool flag = this.CanStartDrag(evt.button, evt.localPosition);
			if (flag)
			{
				bool flag2 = evt.pointerType == PointerType.mouse;
				if (flag2)
				{
					this.m_DragElement.CaptureMouse();
					this.ProcessDownEvent(evt);
				}
				else
				{
					bool flag3 = this.m_DragElement.panel.contextType == ContextType.Editor;
					if (flag3)
					{
						evt.PreventDefault();
						this.m_DragElement.CapturePointer(evt.pointerId);
						this.ProcessDownEvent(evt);
					}
				}
			}
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0003BC24 File Offset: 0x00039E24
		private void ProcessDownEvent(EventBase evt)
		{
			evt.StopPropagation();
			this.dragging = true;
			this.m_DragElement.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.UpdateValueOnPointerMove), TrickleDown.NoTrickleDown);
			this.startValue = this.m_DrivenField.value;
			this.m_DrivenField.StartDragging();
			BaseVisualElementPanel baseVisualElementPanel = this.m_DragElement.panel as BaseVisualElementPanel;
			if (baseVisualElementPanel != null)
			{
				UIElementsBridge uiElementsBridge = baseVisualElementPanel.uiElementsBridge;
				if (uiElementsBridge != null)
				{
					uiElementsBridge.SetWantsMouseJumping(1);
				}
			}
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0003BCA0 File Offset: 0x00039EA0
		private void UpdateValueOnPointerMove(PointerMoveEvent evt)
		{
			this.ProcessMoveEvent(evt.shiftKey, evt.altKey, evt.deltaPosition);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0003BCC4 File Offset: 0x00039EC4
		private void ProcessMoveEvent(bool shiftKey, bool altKey, Vector2 deltaPosition)
		{
			bool dragging = this.dragging;
			if (dragging)
			{
				DeltaSpeed deltaSpeed = (shiftKey ? DeltaSpeed.Fast : (altKey ? DeltaSpeed.Slow : DeltaSpeed.Normal));
				this.m_DrivenField.ApplyInputDeviceDelta(deltaPosition, deltaSpeed, this.startValue);
			}
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0003BD05 File Offset: 0x00039F05
		private void UpdateValueOnPointerUp(PointerUpEvent evt)
		{
			this.ProcessUpEvent(evt, evt.pointerId);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0003BD18 File Offset: 0x00039F18
		private void ProcessUpEvent(EventBase evt, int pointerId)
		{
			bool dragging = this.dragging;
			if (dragging)
			{
				this.dragging = false;
				this.m_DragElement.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.UpdateValueOnPointerMove), TrickleDown.NoTrickleDown);
				this.m_DragElement.ReleasePointer(pointerId);
				bool flag = evt is IMouseEvent;
				if (flag)
				{
					this.m_DragElement.panel.ProcessPointerCapture(PointerId.mousePointerId);
				}
				BaseVisualElementPanel baseVisualElementPanel = this.m_DragElement.panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					UIElementsBridge uiElementsBridge = baseVisualElementPanel.uiElementsBridge;
					if (uiElementsBridge != null)
					{
						uiElementsBridge.SetWantsMouseJumping(0);
					}
				}
				this.m_DrivenField.StopDragging();
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0003BDBC File Offset: 0x00039FBC
		private void UpdateValueOnKeyDown(KeyDownEvent evt)
		{
			bool flag = this.dragging && evt.keyCode == KeyCode.Escape;
			if (flag)
			{
				this.dragging = false;
				this.m_DrivenField.value = this.startValue;
				this.m_DrivenField.StopDragging();
				VisualElement visualElement = evt.target as VisualElement;
				IPanel panel = ((visualElement != null) ? visualElement.panel : null);
				panel.ReleasePointer(PointerId.mousePointerId);
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					UIElementsBridge uiElementsBridge = baseVisualElementPanel.uiElementsBridge;
					if (uiElementsBridge != null)
					{
						uiElementsBridge.SetWantsMouseJumping(0);
					}
				}
			}
		}

		// Token: 0x0400073F RID: 1855
		private readonly IValueField<T> m_DrivenField;

		// Token: 0x04000740 RID: 1856
		private VisualElement m_DragElement;

		// Token: 0x04000741 RID: 1857
		private Rect m_DragHotZone;
	}
}
