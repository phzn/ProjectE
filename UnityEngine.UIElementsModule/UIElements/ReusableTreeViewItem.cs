using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004F RID: 79
	internal class ReusableTreeViewItem : ReusableCollectionItem
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000C315 File Offset: 0x0000A515
		public override VisualElement rootElement
		{
			get
			{
				return this.m_Container ?? base.bindableElement;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000352 RID: 850 RVA: 0x0000C328 File Offset: 0x0000A528
		// (remove) Token: 0x06000353 RID: 851 RVA: 0x0000C360 File Offset: 0x0000A560
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<PointerUpEvent> onPointerUp;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000354 RID: 852 RVA: 0x0000C398 File Offset: 0x0000A598
		// (remove) Token: 0x06000355 RID: 853 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ChangeEvent<bool>> onToggleValueChanged;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000C405 File Offset: 0x0000A605
		internal float indentWidth
		{
			get
			{
				return this.m_IndentWidth;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000C40D File Offset: 0x0000A60D
		public ReusableTreeViewItem()
		{
			this.m_PointerUpCallback = new EventCallback<PointerUpEvent>(this.OnPointerUp);
			this.m_ToggleValueChangedCallback = new EventCallback<ChangeEvent<bool>>(this.OnToggleValueChanged);
			this.m_ToggleGeometryChangedCallback = new EventCallback<GeometryChangedEvent>(this.OnToggleGeometryChanged);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000C450 File Offset: 0x0000A650
		public override void Init(VisualElement item)
		{
			base.Init(item);
			VisualElement visualElement = new VisualElement
			{
				name = BaseTreeView.itemUssClassName
			};
			visualElement.AddToClassList(BaseTreeView.itemUssClassName);
			this.InitExpandHierarchy(visualElement, item);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000C490 File Offset: 0x0000A690
		protected void InitExpandHierarchy(VisualElement root, VisualElement item)
		{
			this.m_Container = root;
			this.m_Container.style.flexDirection = FlexDirection.Row;
			this.m_IndentElement = new VisualElement
			{
				name = BaseTreeView.itemIndentUssClassName,
				style = 
				{
					flexDirection = FlexDirection.Row
				}
			};
			this.m_Container.hierarchy.Add(this.m_IndentElement);
			this.m_Toggle = new Toggle
			{
				name = BaseTreeView.itemToggleUssClassName,
				userData = this
			};
			this.m_Toggle.AddToClassList(Foldout.toggleUssClassName);
			this.m_Toggle.AddToClassList(BaseTreeView.itemToggleUssClassName);
			this.m_Toggle.visualInput.AddToClassList(Foldout.inputUssClassName);
			this.m_Checkmark = this.m_Toggle.visualInput.Q(null, Toggle.checkmarkUssClassName);
			this.m_Checkmark.AddToClassList(Foldout.checkmarkUssClassName);
			this.m_Container.hierarchy.Add(this.m_Toggle);
			this.m_BindableContainer = new VisualElement
			{
				name = BaseTreeView.itemContentContainerUssClassName,
				style = 
				{
					flexGrow = 1f
				}
			};
			this.m_BindableContainer.AddToClassList(BaseTreeView.itemContentContainerUssClassName);
			this.m_Container.Add(this.m_BindableContainer);
			this.m_BindableContainer.Add(item);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		public override void PreAttachElement()
		{
			base.PreAttachElement();
			this.rootElement.AddToClassList(BaseTreeView.itemUssClassName);
			VisualElement container = this.m_Container;
			if (container != null)
			{
				container.RegisterCallback<PointerUpEvent>(this.m_PointerUpCallback, TrickleDown.NoTrickleDown);
			}
			Toggle toggle = this.m_Toggle;
			if (toggle != null)
			{
				toggle.visualInput.Q(null, Toggle.checkmarkUssClassName).RegisterCallback<GeometryChangedEvent>(this.m_ToggleGeometryChangedCallback, TrickleDown.NoTrickleDown);
			}
			Toggle toggle2 = this.m_Toggle;
			if (toggle2 != null)
			{
				toggle2.RegisterValueChangedCallback<bool>(this.m_ToggleValueChangedCallback);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C67C File Offset: 0x0000A87C
		public override void DetachElement()
		{
			base.DetachElement();
			this.rootElement.RemoveFromClassList(BaseTreeView.itemUssClassName);
			VisualElement container = this.m_Container;
			if (container != null)
			{
				container.UnregisterCallback<PointerUpEvent>(this.m_PointerUpCallback, TrickleDown.NoTrickleDown);
			}
			Toggle toggle = this.m_Toggle;
			if (toggle != null)
			{
				toggle.visualInput.Q(null, Toggle.checkmarkUssClassName).UnregisterCallback<GeometryChangedEvent>(this.m_ToggleGeometryChangedCallback, TrickleDown.NoTrickleDown);
			}
			Toggle toggle2 = this.m_Toggle;
			if (toggle2 != null)
			{
				toggle2.UnregisterValueChangedCallback<bool>(this.m_ToggleValueChangedCallback);
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000C6FC File Offset: 0x0000A8FC
		public void Indent(int depth)
		{
			bool flag = this.m_IndentElement == null;
			if (!flag)
			{
				this.m_Depth = depth;
				this.UpdateIndentLayout();
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C727 File Offset: 0x0000A927
		public void SetExpandedWithoutNotify(bool expanded)
		{
			Toggle toggle = this.m_Toggle;
			if (toggle != null)
			{
				toggle.SetValueWithoutNotify(expanded);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C740 File Offset: 0x0000A940
		public void SetToggleVisibility(bool visible)
		{
			bool flag = this.m_Toggle != null;
			if (flag)
			{
				this.m_Toggle.visible = visible;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C768 File Offset: 0x0000A968
		private void OnToggleGeometryChanged(GeometryChangedEvent evt)
		{
			float num = this.m_Checkmark.resolvedStyle.width + this.m_Checkmark.resolvedStyle.marginLeft + this.m_Checkmark.resolvedStyle.marginRight;
			bool flag = Math.Abs(num - this.m_IndentWidth) < float.Epsilon;
			if (!flag)
			{
				this.m_IndentWidth = num;
				this.UpdateIndentLayout();
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		private void UpdateIndentLayout()
		{
			this.m_IndentElement.style.width = this.m_IndentWidth * (float)this.m_Depth;
			this.m_IndentElement.EnableInClassList(BaseTreeView.itemIndentUssClassName, this.m_Depth > 0);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C820 File Offset: 0x0000AA20
		private void OnPointerUp(PointerUpEvent evt)
		{
			Action<PointerUpEvent> action = this.onPointerUp;
			if (action != null)
			{
				action(evt);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C836 File Offset: 0x0000AA36
		private void OnToggleValueChanged(ChangeEvent<bool> evt)
		{
			Action<ChangeEvent<bool>> action = this.onToggleValueChanged;
			if (action != null)
			{
				action(evt);
			}
		}

		// Token: 0x040000FA RID: 250
		private Toggle m_Toggle;

		// Token: 0x040000FB RID: 251
		private VisualElement m_Container;

		// Token: 0x040000FC RID: 252
		private VisualElement m_IndentElement;

		// Token: 0x040000FD RID: 253
		private VisualElement m_BindableContainer;

		// Token: 0x040000FE RID: 254
		private VisualElement m_Checkmark;

		// Token: 0x04000101 RID: 257
		private int m_Depth;

		// Token: 0x04000102 RID: 258
		private float m_IndentWidth;

		// Token: 0x04000103 RID: 259
		private EventCallback<PointerUpEvent> m_PointerUpCallback;

		// Token: 0x04000104 RID: 260
		private EventCallback<ChangeEvent<bool>> m_ToggleValueChangedCallback;

		// Token: 0x04000105 RID: 261
		private EventCallback<GeometryChangedEvent> m_ToggleGeometryChangedCallback;
	}
}
