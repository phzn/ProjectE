using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004C8 RID: 1224
	internal class ColumnResizer : PointerManipulator
	{
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x000A0765 File Offset: 0x0009E965
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x000A076D File Offset: 0x0009E96D
		public ColumnLayout columnLayout { get; set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x000A0776 File Offset: 0x0009E976
		// (set) Token: 0x06002653 RID: 9811 RVA: 0x000A077E File Offset: 0x0009E97E
		public bool preview { get; set; }

		// Token: 0x06002654 RID: 9812 RVA: 0x000A0788 File Offset: 0x0009E988
		public ColumnResizer(Column column)
		{
			this.m_Column = column;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			this.m_Active = false;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000A07CC File Offset: 0x0009E9CC
		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000A0840 File Offset: 0x0009EA40
		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000A08B4 File Offset: 0x0009EAB4
		private void OnKeyDown(KeyDownEvent e)
		{
			bool flag = e.keyCode == KeyCode.Escape && this.m_Resizing && this.preview;
			if (flag)
			{
				this.EndDragResize(0f, true);
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000A08F0 File Offset: 0x0009EAF0
		private void OnPointerDown(PointerDownEvent e)
		{
			bool active = this.m_Active;
			if (active)
			{
				e.StopImmediatePropagation();
			}
			else
			{
				bool flag = base.CanStartManipulation(e);
				if (flag)
				{
					VisualElement visualElement = e.currentTarget as VisualElement;
					this.m_Header = visualElement.GetFirstAncestorOfType<MultiColumnCollectionHeader>();
					this.preview = this.m_Column.collection.resizePreview;
					bool preview = this.preview;
					if (preview)
					{
						bool flag2 = this.m_PreviewElement == null;
						if (flag2)
						{
							this.m_PreviewElement = new MultiColumnHeaderColumnResizePreview();
						}
						ScrollView firstAncestorOfType = this.m_Header.GetFirstAncestorOfType<ScrollView>();
						VisualElement visualElement2 = ((firstAncestorOfType != null) ? firstAncestorOfType.parent : null) ?? this.m_Header.parent;
						visualElement2.hierarchy.Add(this.m_PreviewElement);
					}
					this.columnLayout = this.m_Header.columnLayout;
					this.m_Start = visualElement.ChangeCoordinatesTo(this.m_Header, e.localPosition);
					this.BeginDragResize(this.m_Start.x);
					this.m_Active = true;
					base.target.CaptureMouse();
					e.StopPropagation();
				}
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000A0A14 File Offset: 0x0009EC14
		private void OnPointerMove(PointerMoveEvent e)
		{
			bool flag = !this.m_Active || !base.target.HasPointerCapture(e.pointerId);
			if (!flag)
			{
				VisualElement visualElement = e.currentTarget as VisualElement;
				Vector2 vector = visualElement.ChangeCoordinatesTo(this.m_Header, e.localPosition);
				this.DragResize(vector.x);
				e.StopPropagation();
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000A0A80 File Offset: 0x0009EC80
		private void OnPointerUp(PointerUpEvent e)
		{
			bool flag = !this.m_Active || !base.target.HasPointerCapture(e.pointerId) || !base.CanStopManipulation(e);
			if (!flag)
			{
				VisualElement visualElement = e.currentTarget as VisualElement;
				Vector2 vector = visualElement.ChangeCoordinatesTo(this.m_Header, e.localPosition);
				this.EndDragResize(vector.x, false);
				this.m_Active = false;
				base.target.ReleasePointer(e.pointerId);
				e.StopPropagation();
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000A0B10 File Offset: 0x0009ED10
		private void BeginDragResize(float pos)
		{
			this.m_Resizing = true;
			ColumnLayout columnLayout = this.columnLayout;
			if (columnLayout != null)
			{
				columnLayout.BeginDragResize(this.m_Column, this.m_Start.x, this.preview);
			}
			bool preview = this.preview;
			if (preview)
			{
				this.UpdatePreviewPosition();
			}
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000A0B64 File Offset: 0x0009ED64
		private void DragResize(float pos)
		{
			bool flag = !this.m_Resizing;
			if (!flag)
			{
				ColumnLayout columnLayout = this.columnLayout;
				if (columnLayout != null)
				{
					columnLayout.DragResize(this.m_Column, pos);
				}
				bool preview = this.preview;
				if (preview)
				{
					this.UpdatePreviewPosition();
				}
			}
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000A0BAD File Offset: 0x0009EDAD
		private void UpdatePreviewPosition()
		{
			this.m_PreviewElement.style.left = this.columnLayout.GetDesiredPosition(this.m_Column) + this.columnLayout.GetDesiredWidth(this.m_Column);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000A0BEC File Offset: 0x0009EDEC
		private void EndDragResize(float pos, bool cancelled)
		{
			bool flag = !this.m_Resizing;
			if (!flag)
			{
				bool preview = this.preview;
				if (preview)
				{
					VisualElement previewElement = this.m_PreviewElement;
					if (previewElement != null)
					{
						previewElement.RemoveFromHierarchy();
					}
					this.m_PreviewElement = null;
				}
				ColumnLayout columnLayout = this.columnLayout;
				if (columnLayout != null)
				{
					columnLayout.EndDragResize(this.m_Column, cancelled);
				}
				this.m_Resizing = false;
			}
		}

		// Token: 0x04001268 RID: 4712
		private Vector2 m_Start;

		// Token: 0x04001269 RID: 4713
		protected bool m_Active;

		// Token: 0x0400126A RID: 4714
		private bool m_Resizing;

		// Token: 0x0400126B RID: 4715
		private MultiColumnCollectionHeader m_Header;

		// Token: 0x0400126C RID: 4716
		private Column m_Column;

		// Token: 0x0400126D RID: 4717
		private VisualElement m_PreviewElement;
	}
}
