using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B5 RID: 181
	public class GenericDropdownMenu : IGenericMenu
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00017A40 File Offset: 0x00015C40
		internal List<GenericDropdownMenu.MenuItem> items
		{
			get
			{
				return this.m_Items;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00017A48 File Offset: 0x00015C48
		internal VisualElement menuContainer
		{
			get
			{
				return this.m_MenuContainer;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00017A50 File Offset: 0x00015C50
		internal VisualElement outerContainer
		{
			get
			{
				return this.m_OuterContainer;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00017A58 File Offset: 0x00015C58
		internal ScrollView scrollView
		{
			get
			{
				return this.m_ScrollView;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00017A60 File Offset: 0x00015C60
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00017A68 File Offset: 0x00015C68
		internal bool isSingleSelectionDropdown { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00017A71 File Offset: 0x00015C71
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x00017A79 File Offset: 0x00015C79
		internal bool closeOnParentResize { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017A82 File Offset: 0x00015C82
		public VisualElement contentContainer
		{
			get
			{
				return this.m_ScrollView.contentContainer;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00017A90 File Offset: 0x00015C90
		public GenericDropdownMenu()
		{
			this.m_MenuContainer = new VisualElement();
			this.m_MenuContainer.AddToClassList(GenericDropdownMenu.ussClassName);
			this.m_OuterContainer = new VisualElement();
			this.m_OuterContainer.AddToClassList(GenericDropdownMenu.containerOuterUssClassName);
			this.m_MenuContainer.Add(this.m_OuterContainer);
			this.m_ScrollView = new ScrollView();
			this.m_ScrollView.AddToClassList(GenericDropdownMenu.containerInnerUssClassName);
			this.m_ScrollView.pickingMode = PickingMode.Position;
			this.m_ScrollView.contentContainer.focusable = true;
			this.m_ScrollView.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
			this.m_OuterContainer.hierarchy.Add(this.m_ScrollView);
			this.m_MenuContainer.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			this.m_MenuContainer.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
			this.isSingleSelectionDropdown = true;
			this.closeOnParentResize = true;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00017BA0 File Offset: 0x00015DA0
		private void OnAttachToPanel(AttachToPanelEvent evt)
		{
			bool flag = evt.destinationPanel == null;
			if (!flag)
			{
				this.contentContainer.AddManipulator(this.m_NavigationManipulator = new KeyboardNavigationManipulator(new Action<KeyboardNavigationOperation, EventBase>(this.Apply)));
				this.m_MenuContainer.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
				this.m_MenuContainer.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
				this.m_MenuContainer.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
				evt.destinationPanel.visualTree.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnParentResized), TrickleDown.NoTrickleDown);
				this.m_ScrollView.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnContainerGeometryChanged), TrickleDown.NoTrickleDown);
				this.m_ScrollView.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnFocusOut), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00017C84 File Offset: 0x00015E84
		private void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			bool flag = evt.originPanel == null;
			if (!flag)
			{
				this.contentContainer.RemoveManipulator(this.m_NavigationManipulator);
				this.m_MenuContainer.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
				this.m_MenuContainer.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
				this.m_MenuContainer.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
				evt.originPanel.visualTree.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnParentResized), TrickleDown.NoTrickleDown);
				this.m_ScrollView.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnContainerGeometryChanged), TrickleDown.NoTrickleDown);
				this.m_ScrollView.UnregisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnFocusOut), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00017D54 File Offset: 0x00015F54
		private void Hide(bool giveFocusBack = false)
		{
			this.m_MenuContainer.RemoveFromHierarchy();
			bool flag = this.m_TargetElement != null;
			if (flag)
			{
				this.m_TargetElement.pseudoStates ^= PseudoStates.Active;
				bool flag2 = giveFocusBack && this.m_TargetElement.canGrabFocus;
				if (flag2)
				{
					this.m_TargetElement.Focus();
				}
			}
			this.m_TargetElement = null;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00017DBC File Offset: 0x00015FBC
		private void Apply(KeyboardNavigationOperation op, EventBase sourceEvent)
		{
			bool flag = this.Apply(op);
			if (flag)
			{
				sourceEvent.StopPropagation();
				sourceEvent.PreventDefault();
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00017DE8 File Offset: 0x00015FE8
		private bool Apply(KeyboardNavigationOperation op)
		{
			GenericDropdownMenu.<>c__DisplayClass39_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.selectedIndex = this.GetSelectedIndex();
			switch (op)
			{
			case KeyboardNavigationOperation.Cancel:
				this.Hide(true);
				return true;
			case KeyboardNavigationOperation.Submit:
			{
				GenericDropdownMenu.MenuItem menuItem = ((CS$<>8__locals1.selectedIndex != -1) ? this.m_Items[CS$<>8__locals1.selectedIndex] : null);
				bool flag = CS$<>8__locals1.selectedIndex >= 0 && menuItem.element.enabledSelf;
				if (flag)
				{
					Action action = menuItem.action;
					if (action != null)
					{
						action();
					}
					Action<object> actionUserData = menuItem.actionUserData;
					if (actionUserData != null)
					{
						actionUserData(menuItem.element.userData);
					}
				}
				this.Hide(true);
				return true;
			}
			case KeyboardNavigationOperation.Previous:
				this.<Apply>g__UpdateSelectionUp|39_1((CS$<>8__locals1.selectedIndex < 0) ? (this.m_Items.Count - 1) : (CS$<>8__locals1.selectedIndex - 1), ref CS$<>8__locals1);
				return true;
			case KeyboardNavigationOperation.Next:
				this.<Apply>g__UpdateSelectionDown|39_0(CS$<>8__locals1.selectedIndex + 1, ref CS$<>8__locals1);
				return true;
			case KeyboardNavigationOperation.PageUp:
			case KeyboardNavigationOperation.Begin:
				this.<Apply>g__UpdateSelectionDown|39_0(0, ref CS$<>8__locals1);
				return true;
			case KeyboardNavigationOperation.PageDown:
			case KeyboardNavigationOperation.End:
				this.<Apply>g__UpdateSelectionUp|39_1(this.m_Items.Count - 1, ref CS$<>8__locals1);
				return true;
			}
			return false;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00017F44 File Offset: 0x00016144
		private void OnPointerDown(PointerDownEvent evt)
		{
			this.m_MousePosition = this.m_ScrollView.WorldToLocal(evt.position);
			this.UpdateSelection(evt.target as VisualElement);
			bool flag = evt.pointerId != PointerId.mousePointerId;
			if (flag)
			{
				this.m_MenuContainer.panel.PreventCompatibilityMouseEvents(evt.pointerId);
			}
			evt.StopPropagation();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00017FB4 File Offset: 0x000161B4
		private void OnPointerMove(PointerMoveEvent evt)
		{
			this.m_MousePosition = this.m_ScrollView.WorldToLocal(evt.position);
			this.UpdateSelection(evt.target as VisualElement);
			bool flag = evt.pointerId != PointerId.mousePointerId;
			if (flag)
			{
				this.m_MenuContainer.panel.PreventCompatibilityMouseEvents(evt.pointerId);
			}
			evt.StopPropagation();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00018024 File Offset: 0x00016224
		private void OnPointerUp(PointerUpEvent evt)
		{
			int selectedIndex = this.GetSelectedIndex();
			bool flag = selectedIndex != -1;
			if (flag)
			{
				GenericDropdownMenu.MenuItem menuItem = this.m_Items[selectedIndex];
				Action action = menuItem.action;
				if (action != null)
				{
					action();
				}
				Action<object> actionUserData = menuItem.actionUserData;
				if (actionUserData != null)
				{
					actionUserData(menuItem.element.userData);
				}
				bool isSingleSelectionDropdown = this.isSingleSelectionDropdown;
				if (isSingleSelectionDropdown)
				{
					this.Hide(true);
				}
			}
			bool flag2 = evt.pointerId != PointerId.mousePointerId;
			if (flag2)
			{
				this.m_MenuContainer.panel.PreventCompatibilityMouseEvents(evt.pointerId);
			}
			evt.StopPropagation();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000180CC File Offset: 0x000162CC
		private void OnFocusOut(FocusOutEvent evt)
		{
			bool flag = !this.m_ScrollView.ContainsPoint(this.m_MousePosition);
			if (flag)
			{
				this.Hide(false);
			}
			else
			{
				this.m_MenuContainer.schedule.Execute(new Action(this.contentContainer.Focus));
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00018124 File Offset: 0x00016324
		private void OnParentResized(GeometryChangedEvent evt)
		{
			bool closeOnParentResize = this.closeOnParentResize;
			if (closeOnParentResize)
			{
				this.Hide(true);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00018148 File Offset: 0x00016348
		private void UpdateSelection(VisualElement target)
		{
			bool flag = !this.m_ScrollView.ContainsPoint(this.m_MousePosition);
			if (flag)
			{
				int selectedIndex = this.GetSelectedIndex();
				bool flag2 = selectedIndex >= 0;
				if (flag2)
				{
					this.m_Items[selectedIndex].element.pseudoStates &= ~PseudoStates.Hover;
				}
			}
			else
			{
				bool flag3 = target == null;
				if (!flag3)
				{
					bool flag4 = (target.pseudoStates & PseudoStates.Hover) != PseudoStates.Hover;
					if (flag4)
					{
						int selectedIndex2 = this.GetSelectedIndex();
						bool flag5 = selectedIndex2 >= 0;
						if (flag5)
						{
							this.m_Items[selectedIndex2].element.pseudoStates &= ~PseudoStates.Hover;
						}
						target.pseudoStates |= PseudoStates.Hover;
					}
				}
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001820C File Offset: 0x0001640C
		private void ChangeSelectedIndex(int newIndex, int previousIndex)
		{
			bool flag = previousIndex >= 0 && previousIndex < this.m_Items.Count;
			if (flag)
			{
				this.m_Items[previousIndex].element.pseudoStates &= ~PseudoStates.Hover;
			}
			bool flag2 = newIndex >= 0 && newIndex < this.m_Items.Count;
			if (flag2)
			{
				this.m_Items[newIndex].element.pseudoStates |= PseudoStates.Hover;
				this.m_ScrollView.ScrollTo(this.m_Items[newIndex].element);
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000182AC File Offset: 0x000164AC
		private int GetSelectedIndex()
		{
			for (int i = 0; i < this.m_Items.Count; i++)
			{
				bool flag = (this.m_Items[i].element.pseudoStates & PseudoStates.Hover) == PseudoStates.Hover;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00018300 File Offset: 0x00016500
		public void AddItem(string itemName, bool isChecked, Action action)
		{
			GenericDropdownMenu.MenuItem menuItem = this.AddItem(itemName, isChecked, true, null);
			bool flag = menuItem != null;
			if (flag)
			{
				menuItem.action = action;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001832C File Offset: 0x0001652C
		public void AddItem(string itemName, bool isChecked, Action<object> action, object data)
		{
			GenericDropdownMenu.MenuItem menuItem = this.AddItem(itemName, isChecked, true, data);
			bool flag = menuItem != null;
			if (flag)
			{
				menuItem.actionUserData = action;
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00018357 File Offset: 0x00016557
		public void AddDisabledItem(string itemName, bool isChecked)
		{
			this.AddItem(itemName, isChecked, false, null);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00018368 File Offset: 0x00016568
		public void AddSeparator(string path)
		{
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList(GenericDropdownMenu.separatorUssClassName);
			visualElement.pickingMode = PickingMode.Ignore;
			this.m_ScrollView.Add(visualElement);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000183A0 File Offset: 0x000165A0
		private GenericDropdownMenu.MenuItem AddItem(string itemName, bool isChecked, bool isEnabled, object data = null)
		{
			bool flag = string.IsNullOrEmpty(itemName) || itemName.EndsWith("/");
			GenericDropdownMenu.MenuItem menuItem;
			if (flag)
			{
				this.AddSeparator(itemName);
				menuItem = null;
			}
			else
			{
				for (int i = 0; i < this.m_Items.Count; i++)
				{
					bool flag2 = itemName == this.m_Items[i].name;
					if (flag2)
					{
						return null;
					}
				}
				VisualElement visualElement = new VisualElement();
				visualElement.AddToClassList(GenericDropdownMenu.itemUssClassName);
				visualElement.SetEnabled(isEnabled);
				visualElement.userData = data;
				VisualElement visualElement2 = new VisualElement();
				visualElement2.AddToClassList(GenericDropdownMenu.checkmarkUssClassName);
				visualElement2.pickingMode = PickingMode.Ignore;
				visualElement.Add(visualElement2);
				if (isChecked)
				{
					visualElement.pseudoStates |= PseudoStates.Checked;
				}
				Label label = new Label(itemName);
				label.AddToClassList(GenericDropdownMenu.labelUssClassName);
				label.pickingMode = PickingMode.Ignore;
				visualElement.Add(label);
				this.m_ScrollView.Add(visualElement);
				GenericDropdownMenu.MenuItem menuItem2 = new GenericDropdownMenu.MenuItem
				{
					name = itemName,
					element = visualElement
				};
				this.m_Items.Add(menuItem2);
				menuItem = menuItem2;
			}
			return menuItem;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000184D8 File Offset: 0x000166D8
		internal void UpdateItem(string itemName, bool isChecked)
		{
			GenericDropdownMenu.MenuItem menuItem = this.m_Items.Find((GenericDropdownMenu.MenuItem x) => x.name == itemName);
			bool flag = menuItem == null;
			if (!flag)
			{
				if (isChecked)
				{
					menuItem.element.pseudoStates |= PseudoStates.Checked;
				}
				else
				{
					menuItem.element.pseudoStates &= ~PseudoStates.Checked;
				}
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001854C File Offset: 0x0001674C
		public void DropDown(Rect position, VisualElement targetElement = null, bool anchored = false)
		{
			bool flag = targetElement == null;
			if (flag)
			{
				Debug.LogError("VisualElement Generic Menu needs a target to find a root to attach to.");
			}
			else
			{
				this.m_TargetElement = targetElement;
				this.m_TargetElement.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnTargetElementDetachFromPanel), TrickleDown.NoTrickleDown);
				this.m_PanelRootVisualContainer = this.m_TargetElement.GetRootVisualContainer();
				bool flag2 = this.m_PanelRootVisualContainer == null;
				if (flag2)
				{
					Debug.LogError("Could not find rootVisualContainer...");
				}
				else
				{
					this.m_PanelRootVisualContainer.Add(this.m_MenuContainer);
					this.m_MenuContainer.style.left = this.m_PanelRootVisualContainer.layout.x;
					this.m_MenuContainer.style.top = this.m_PanelRootVisualContainer.layout.y;
					this.m_MenuContainer.style.width = this.m_PanelRootVisualContainer.layout.width;
					this.m_MenuContainer.style.height = this.m_PanelRootVisualContainer.layout.height;
					this.m_MenuContainer.style.fontSize = this.m_TargetElement.computedStyle.fontSize;
					this.m_MenuContainer.style.unityFont = this.m_TargetElement.computedStyle.unityFont;
					this.m_MenuContainer.style.unityFontDefinition = this.m_TargetElement.computedStyle.unityFontDefinition;
					Rect rect = this.m_PanelRootVisualContainer.WorldToLocal(position);
					this.m_OuterContainer.style.left = rect.x - this.m_PanelRootVisualContainer.layout.x;
					this.m_OuterContainer.style.top = rect.y + position.height - this.m_PanelRootVisualContainer.layout.y;
					this.m_DesiredRect = (anchored ? position : Rect.zero);
					this.m_MenuContainer.schedule.Execute(new Action(this.contentContainer.Focus));
					this.EnsureVisibilityInParent();
					bool flag3 = targetElement != null;
					if (flag3)
					{
						targetElement.pseudoStates |= PseudoStates.Active;
					}
				}
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000187B5 File Offset: 0x000169B5
		private void OnTargetElementDetachFromPanel(DetachFromPanelEvent evt)
		{
			this.Hide(false);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000187C0 File Offset: 0x000169C0
		private void OnContainerGeometryChanged(GeometryChangedEvent evt)
		{
			this.EnsureVisibilityInParent();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000187CC File Offset: 0x000169CC
		private void EnsureVisibilityInParent()
		{
			bool flag = this.m_PanelRootVisualContainer != null && !float.IsNaN(this.m_OuterContainer.layout.width) && !float.IsNaN(this.m_OuterContainer.layout.height);
			if (flag)
			{
				bool flag2 = this.m_DesiredRect == Rect.zero;
				if (flag2)
				{
					float num = Mathf.Min(this.m_OuterContainer.layout.x, this.m_PanelRootVisualContainer.layout.width - this.m_OuterContainer.layout.width);
					float num2 = Mathf.Min(this.m_OuterContainer.layout.y, Mathf.Max(0f, this.m_PanelRootVisualContainer.layout.height - this.m_OuterContainer.layout.height));
					this.m_OuterContainer.style.left = num;
					this.m_OuterContainer.style.top = num2;
				}
				this.m_OuterContainer.style.height = Mathf.Min(this.m_MenuContainer.layout.height - this.m_MenuContainer.layout.y - this.m_OuterContainer.layout.y, this.m_ScrollView.layout.height + this.m_OuterContainer.resolvedStyle.borderBottomWidth + this.m_OuterContainer.resolvedStyle.borderTopWidth);
				bool flag3 = this.m_DesiredRect != Rect.zero;
				if (flag3)
				{
					this.m_OuterContainer.style.width = this.m_DesiredRect.width;
				}
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00018A4C File Offset: 0x00016C4C
		[CompilerGenerated]
		private void <Apply>g__UpdateSelectionDown|39_0(int newIndex, ref GenericDropdownMenu.<>c__DisplayClass39_0 A_2)
		{
			while (newIndex < this.m_Items.Count)
			{
				bool enabledSelf = this.m_Items[newIndex].element.enabledSelf;
				if (enabledSelf)
				{
					this.ChangeSelectedIndex(newIndex, A_2.selectedIndex);
					break;
				}
				newIndex++;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00018AA0 File Offset: 0x00016CA0
		[CompilerGenerated]
		private void <Apply>g__UpdateSelectionUp|39_1(int newIndex, ref GenericDropdownMenu.<>c__DisplayClass39_0 A_2)
		{
			while (newIndex >= 0)
			{
				bool enabledSelf = this.m_Items[newIndex].element.enabledSelf;
				if (enabledSelf)
				{
					this.ChangeSelectedIndex(newIndex, A_2.selectedIndex);
					break;
				}
				newIndex--;
			}
		}

		// Token: 0x040002B4 RID: 692
		public static readonly string ussClassName = "unity-base-dropdown";

		// Token: 0x040002B5 RID: 693
		public static readonly string itemUssClassName = GenericDropdownMenu.ussClassName + "__item";

		// Token: 0x040002B6 RID: 694
		public static readonly string labelUssClassName = GenericDropdownMenu.ussClassName + "__label";

		// Token: 0x040002B7 RID: 695
		public static readonly string containerInnerUssClassName = GenericDropdownMenu.ussClassName + "__container-inner";

		// Token: 0x040002B8 RID: 696
		public static readonly string containerOuterUssClassName = GenericDropdownMenu.ussClassName + "__container-outer";

		// Token: 0x040002B9 RID: 697
		public static readonly string checkmarkUssClassName = GenericDropdownMenu.ussClassName + "__checkmark";

		// Token: 0x040002BA RID: 698
		public static readonly string separatorUssClassName = GenericDropdownMenu.ussClassName + "__separator";

		// Token: 0x040002BB RID: 699
		private List<GenericDropdownMenu.MenuItem> m_Items = new List<GenericDropdownMenu.MenuItem>();

		// Token: 0x040002BC RID: 700
		private VisualElement m_MenuContainer;

		// Token: 0x040002BD RID: 701
		private VisualElement m_OuterContainer;

		// Token: 0x040002BE RID: 702
		private ScrollView m_ScrollView;

		// Token: 0x040002BF RID: 703
		private VisualElement m_PanelRootVisualContainer;

		// Token: 0x040002C0 RID: 704
		private VisualElement m_TargetElement;

		// Token: 0x040002C1 RID: 705
		private Rect m_DesiredRect;

		// Token: 0x040002C2 RID: 706
		private KeyboardNavigationManipulator m_NavigationManipulator;

		// Token: 0x040002C5 RID: 709
		private Vector2 m_MousePosition;

		// Token: 0x020000B6 RID: 182
		internal class MenuItem
		{
			// Token: 0x040002C6 RID: 710
			public string name;

			// Token: 0x040002C7 RID: 711
			public VisualElement element;

			// Token: 0x040002C8 RID: 712
			public Action action;

			// Token: 0x040002C9 RID: 713
			public Action<object> actionUserData;
		}
	}
}
