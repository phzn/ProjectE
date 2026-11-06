using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x0200005B RID: 91
	public abstract class BaseListView : BaseVerticalCollectionView
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
		public bool showBoundCollectionSize
		{
			get
			{
				return this.m_ShowBoundCollectionSize;
			}
			set
			{
				bool flag = this.m_ShowBoundCollectionSize == value;
				if (!flag)
				{
					this.m_ShowBoundCollectionSize = value;
					this.SetupArraySizeField();
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000E3E3 File Offset: 0x0000C5E3
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public bool showFoldoutHeader
		{
			get
			{
				return this.m_ShowFoldoutHeader;
			}
			set
			{
				bool flag = this.m_ShowFoldoutHeader == value;
				if (!flag)
				{
					this.m_ShowFoldoutHeader = value;
					base.EnableInClassList(BaseListView.listViewWithHeaderUssClassName, value);
					bool showFoldoutHeader = this.m_ShowFoldoutHeader;
					if (showFoldoutHeader)
					{
						bool flag2 = this.m_Foldout != null;
						if (flag2)
						{
							return;
						}
						this.m_Foldout = new Foldout
						{
							name = BaseListView.foldoutHeaderUssClassName,
							text = this.m_HeaderTitle
						};
						this.m_Foldout.toggle.tabIndex = 10;
						this.m_Foldout.toggle.m_Clickable.acceptClicksIfDisabled = true;
						this.m_Foldout.AddToClassList(BaseListView.foldoutHeaderUssClassName);
						this.m_Foldout.tabIndex = 1;
						base.hierarchy.Add(this.m_Foldout);
						this.m_Foldout.Add(base.scrollView);
					}
					else
					{
						bool flag3 = this.m_Foldout != null;
						if (flag3)
						{
							Foldout foldout = this.m_Foldout;
							if (foldout != null)
							{
								foldout.RemoveFromHierarchy();
							}
							this.m_Foldout = null;
							base.hierarchy.Add(base.scrollView);
						}
					}
					this.SetupArraySizeField();
					this.UpdateListViewLabel();
					bool showAddRemoveFooter = this.showAddRemoveFooter;
					if (showAddRemoveFooter)
					{
						this.EnableFooter(true);
					}
				}
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000E53C File Offset: 0x0000C73C
		internal void SetupArraySizeField()
		{
			bool flag = !this.showBoundCollectionSize || (!this.showFoldoutHeader && base.GetProperty("__unity-collection-view-internal-binding") == null);
			if (flag)
			{
				TextField arraySizeField = this.m_ArraySizeField;
				if (arraySizeField != null)
				{
					arraySizeField.RemoveFromHierarchy();
				}
			}
			else
			{
				bool flag2 = this.m_ArraySizeField == null;
				if (flag2)
				{
					this.m_ArraySizeField = new TextField
					{
						name = BaseListView.arraySizeFieldUssClassName,
						tabIndex = 20
					};
					this.m_ArraySizeField.AddToClassList(BaseListView.arraySizeFieldUssClassName);
					this.m_ArraySizeField.RegisterValueChangedCallback<string>(new EventCallback<ChangeEvent<string>>(this.OnArraySizeFieldChanged));
					this.m_ArraySizeField.isDelayed = true;
					this.m_ArraySizeField.focusable = true;
				}
				this.m_ArraySizeField.EnableInClassList(BaseListView.arraySizeFieldWithFooterUssClassName, this.showAddRemoveFooter);
				this.m_ArraySizeField.EnableInClassList(BaseListView.arraySizeFieldWithHeaderUssClassName, this.showFoldoutHeader);
				bool showFoldoutHeader = this.showFoldoutHeader;
				if (showFoldoutHeader)
				{
					this.m_ArraySizeField.label = string.Empty;
					base.hierarchy.Add(this.m_ArraySizeField);
				}
				else
				{
					this.m_ArraySizeField.label = BaseListView.k_SizeFieldLabel;
					base.hierarchy.Insert(0, this.m_ArraySizeField);
				}
				this.UpdateArraySizeField();
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000E693 File Offset: 0x0000C893
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0000E69C File Offset: 0x0000C89C
		public string headerTitle
		{
			get
			{
				return this.m_HeaderTitle;
			}
			set
			{
				this.m_HeaderTitle = value;
				bool flag = this.m_Foldout != null;
				if (flag)
				{
					this.m_Foldout.text = this.m_HeaderTitle;
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000E6DB File Offset: 0x0000C8DB
		public bool showAddRemoveFooter
		{
			get
			{
				return this.m_Footer != null;
			}
			set
			{
				this.EnableFooter(value);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000E6E5 File Offset: 0x0000C8E5
		internal Foldout headerFoldout
		{
			get
			{
				return this.m_Foldout;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000E6F0 File Offset: 0x0000C8F0
		private void EnableFooter(bool enabled)
		{
			base.EnableInClassList(BaseListView.listViewWithFooterUssClassName, enabled);
			base.scrollView.EnableInClassList(BaseListView.scrollViewWithFooterUssClassName, enabled);
			bool flag = this.m_ArraySizeField != null;
			if (flag)
			{
				this.m_ArraySizeField.EnableInClassList(BaseListView.arraySizeFieldWithFooterUssClassName, enabled);
			}
			if (enabled)
			{
				bool flag2 = this.m_Footer == null;
				if (flag2)
				{
					this.m_Footer = new VisualElement
					{
						name = BaseListView.footerUssClassName
					};
					this.m_Footer.AddToClassList(BaseListView.footerUssClassName);
					this.m_AddButton = new Button(new Action(this.OnAddClicked))
					{
						name = BaseListView.footerAddButtonName,
						text = "+"
					};
					this.m_Footer.Add(this.m_AddButton);
					this.m_RemoveButton = new Button(new Action(this.OnRemoveClicked))
					{
						name = BaseListView.footerRemoveButtonName,
						text = "-"
					};
					this.m_Footer.Add(this.m_RemoveButton);
				}
				bool flag3 = this.m_Foldout != null;
				if (flag3)
				{
					this.m_Foldout.contentContainer.Add(this.m_Footer);
				}
				else
				{
					base.hierarchy.Add(this.m_Footer);
				}
			}
			else
			{
				Button removeButton = this.m_RemoveButton;
				if (removeButton != null)
				{
					removeButton.RemoveFromHierarchy();
				}
				Button addButton = this.m_AddButton;
				if (addButton != null)
				{
					addButton.RemoveFromHierarchy();
				}
				VisualElement footer = this.m_Footer;
				if (footer != null)
				{
					footer.RemoveFromHierarchy();
				}
				this.m_RemoveButton = null;
				this.m_AddButton = null;
				this.m_Footer = null;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060003BF RID: 959 RVA: 0x0000E890 File Offset: 0x0000CA90
		// (remove) Token: 0x060003C0 RID: 960 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<int>> itemsAdded;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060003C1 RID: 961 RVA: 0x0000E900 File Offset: 0x0000CB00
		// (remove) Token: 0x060003C2 RID: 962 RVA: 0x0000E938 File Offset: 0x0000CB38
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<int>> itemsRemoved;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060003C3 RID: 963 RVA: 0x0000E970 File Offset: 0x0000CB70
		// (remove) Token: 0x060003C4 RID: 964 RVA: 0x0000E9A8 File Offset: 0x0000CBA8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action itemsSourceSizeChanged;

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E9DD File Offset: 0x0000CBDD
		private void AddItems(int itemCount)
		{
			this.viewController.AddItems(itemCount);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E9ED File Offset: 0x0000CBED
		private void RemoveItems(List<int> indices)
		{
			this.viewController.RemoveItems(indices);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000EA00 File Offset: 0x0000CC00
		private void OnArraySizeFieldChanged(ChangeEvent<string> evt)
		{
			bool flag = this.m_ArraySizeField.showMixedValue && BaseField<string>.mixedValueString == evt.newValue;
			if (!flag)
			{
				int num;
				bool flag2 = !int.TryParse(evt.newValue, out num) || num < 0;
				if (flag2)
				{
					this.m_ArraySizeField.SetValueWithoutNotify(evt.previousValue);
				}
				else
				{
					int itemsCount = this.viewController.GetItemsCount();
					bool flag3 = itemsCount == 0 && num == this.viewController.GetItemsMinCount();
					if (!flag3)
					{
						bool flag4 = num > itemsCount;
						if (flag4)
						{
							this.viewController.AddItems(num - itemsCount);
						}
						else
						{
							bool flag5 = num < itemsCount;
							if (flag5)
							{
								this.viewController.RemoveItems(itemsCount - num);
							}
							else
							{
								bool flag6 = num == 0;
								if (flag6)
								{
									this.viewController.ClearItems();
									this.m_IsOverMultiEditLimit = false;
								}
							}
						}
						this.UpdateListViewLabel();
					}
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		internal void UpdateArraySizeField()
		{
			bool flag = !this.HasValidDataAndBindings() || this.m_ArraySizeField == null;
			if (!flag)
			{
				bool flag2 = !this.m_ArraySizeField.showMixedValue;
				if (flag2)
				{
					this.m_ArraySizeField.SetValueWithoutNotify(this.viewController.GetItemsMinCount().ToString());
				}
				VisualElement footer = this.footer;
				if (footer != null)
				{
					footer.SetEnabled(!this.m_IsOverMultiEditLimit);
				}
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000EB68 File Offset: 0x0000CD68
		internal void UpdateListViewLabel()
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = base.itemsSource.Count == 0;
				bool isOverMultiEditLimit = this.m_IsOverMultiEditLimit;
				if (isOverMultiEditLimit)
				{
					if (this.m_ListViewLabel == null)
					{
						this.m_ListViewLabel = new Label();
					}
					this.m_ListViewLabel.text = this.m_MaxMultiEditStr;
					base.scrollView.contentViewport.Add(this.m_ListViewLabel);
				}
				else
				{
					bool flag3 = flag2;
					if (flag3)
					{
						if (this.m_ListViewLabel == null)
						{
							this.m_ListViewLabel = new Label();
						}
						this.m_ListViewLabel.text = BaseListView.k_EmptyListStr;
						base.scrollView.contentViewport.Add(this.m_ListViewLabel);
					}
					else
					{
						Label listViewLabel = this.m_ListViewLabel;
						if (listViewLabel != null)
						{
							listViewLabel.RemoveFromHierarchy();
						}
						this.m_ListViewLabel = null;
					}
				}
				Label listViewLabel2 = this.m_ListViewLabel;
				if (listViewLabel2 != null)
				{
					listViewLabel2.EnableInClassList(BaseListView.emptyLabelUssClassName, flag2);
				}
				Label listViewLabel3 = this.m_ListViewLabel;
				if (listViewLabel3 != null)
				{
					listViewLabel3.EnableInClassList(BaseListView.overMaxMultiEditLimitClassName, this.m_IsOverMultiEditLimit);
				}
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000EC78 File Offset: 0x0000CE78
		private void OnAddClicked()
		{
			this.AddItems(1);
			bool flag = base.binding == null;
			if (flag)
			{
				base.SetSelection(base.itemsSource.Count - 1);
				base.ScrollToItem(-1);
			}
			else
			{
				base.schedule.Execute(delegate
				{
					base.SetSelection(base.itemsSource.Count - 1);
					base.ScrollToItem(-1);
				}).ExecuteLater(100L);
			}
			bool flag2 = this.HasValidDataAndBindings() && this.m_ArraySizeField != null;
			if (flag2)
			{
				this.m_ArraySizeField.showMixedValue = false;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000ED04 File Offset: 0x0000CF04
		private void OnRemoveClicked()
		{
			bool flag = base.selectedIndices.Any<int>();
			if (flag)
			{
				this.viewController.RemoveItems(base.selectedIndices.ToList<int>());
				base.ClearSelection();
			}
			else
			{
				bool flag2 = base.itemsSource.Count > 0;
				if (flag2)
				{
					int num = base.itemsSource.Count - 1;
					this.viewController.RemoveItem(num);
				}
			}
			bool flag3 = this.HasValidDataAndBindings() && this.m_ArraySizeField != null;
			if (flag3)
			{
				this.m_ArraySizeField.showMixedValue = false;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000ED97 File Offset: 0x0000CF97
		internal TextField arraySizeField
		{
			get
			{
				return this.m_ArraySizeField;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000ED9F File Offset: 0x0000CF9F
		internal void SetOverMaxMultiEditLimit(bool isOverLimit, int maxMultiEditCount)
		{
			this.m_IsOverMultiEditLimit = isOverLimit;
			this.m_MaxMultiEditCount = maxMultiEditCount;
			this.m_MaxMultiEditStr = string.Format("This field cannot display arrays with more than {0} elements when multiple objects are selected.", this.m_MaxMultiEditCount);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000EDCB File Offset: 0x0000CFCB
		internal VisualElement footer
		{
			get
			{
				return this.m_Footer;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000EDD3 File Offset: 0x0000CFD3
		public new BaseListViewController viewController
		{
			get
			{
				return base.viewController as BaseListViewController;
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		private protected override void CreateVirtualizationController()
		{
			base.CreateVirtualizationController<ReusableListViewItem>();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public override void SetViewController(CollectionViewController controller)
		{
			if (this.m_ItemAddedCallback == null)
			{
				this.m_ItemAddedCallback = new Action<IEnumerable<int>>(this.OnItemAdded);
			}
			if (this.m_ItemRemovedCallback == null)
			{
				this.m_ItemRemovedCallback = new Action<IEnumerable<int>>(this.OnItemsRemoved);
			}
			if (this.m_ItemsSourceSizeChangedCallback == null)
			{
				this.m_ItemsSourceSizeChangedCallback = new Action(this.OnItemsSourceSizeChanged);
			}
			bool flag = this.viewController != null;
			if (flag)
			{
				this.viewController.itemsAdded -= this.m_ItemAddedCallback;
				this.viewController.itemsRemoved -= this.m_ItemRemovedCallback;
				this.viewController.itemsSourceSizeChanged -= this.m_ItemsSourceSizeChangedCallback;
			}
			base.SetViewController(controller);
			bool flag2 = this.viewController != null;
			if (flag2)
			{
				this.viewController.itemsAdded += this.m_ItemAddedCallback;
				this.viewController.itemsRemoved += this.m_ItemRemovedCallback;
				this.viewController.itemsSourceSizeChanged += this.m_ItemsSourceSizeChangedCallback;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000EEDA File Offset: 0x0000D0DA
		private void OnItemAdded(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsAdded;
			if (action != null)
			{
				action(indices);
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		private void OnItemsRemoved(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsRemoved;
			if (action != null)
			{
				action(indices);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000EF08 File Offset: 0x0000D108
		private void OnItemsSourceSizeChanged()
		{
			bool flag = base.GetProperty("__unity-collection-view-internal-binding") == null;
			if (flag)
			{
				base.RefreshItems();
			}
			Action action = this.itemsSourceSizeChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060003D5 RID: 981 RVA: 0x0000EF48 File Offset: 0x0000D148
		// (remove) Token: 0x060003D6 RID: 982 RVA: 0x0000EF80 File Offset: 0x0000D180
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action reorderModeChanged;

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000EFB5 File Offset: 0x0000D1B5
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		public ListViewReorderMode reorderMode
		{
			get
			{
				return this.m_ReorderMode;
			}
			set
			{
				bool flag = value != this.m_ReorderMode;
				if (flag)
				{
					this.m_ReorderMode = value;
					base.InitializeDragAndDropController(base.reorderable);
					Action action = this.reorderModeChanged;
					if (action != null)
					{
						action();
					}
					base.Rebuild();
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000F010 File Offset: 0x0000D210
		internal override ListViewDragger CreateDragger()
		{
			bool flag = this.m_ReorderMode == ListViewReorderMode.Simple;
			ListViewDragger listViewDragger;
			if (flag)
			{
				listViewDragger = new ListViewDragger(this);
			}
			else
			{
				listViewDragger = new ListViewDraggerAnimated(this);
			}
			return listViewDragger;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000F03E File Offset: 0x0000D23E
		internal override ICollectionDragAndDropController CreateDragAndDropController()
		{
			return new ListViewReorderableDragAndDropController(this);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000F046 File Offset: 0x0000D246
		public BaseListView()
		{
			base.AddToClassList(BaseListView.ussClassName);
			base.pickingMode = PickingMode.Ignore;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000F06B File Offset: 0x0000D26B
		public BaseListView(IList itemsSource, float itemHeight = -1f)
			: base(itemsSource, itemHeight)
		{
			base.AddToClassList(BaseListView.ussClassName);
			base.pickingMode = PickingMode.Ignore;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000F092 File Offset: 0x0000D292
		private protected override void PostRefresh()
		{
			this.UpdateArraySizeField();
			this.UpdateListViewLabel();
			base.PostRefresh();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		private protected override bool HandleItemNavigation(bool moveIn, bool altPressed)
		{
			bool flag = false;
			foreach (int num in base.selectedIndices)
			{
				foreach (ReusableCollectionItem reusableCollectionItem in base.activeItems)
				{
					bool flag2 = reusableCollectionItem.index == num && base.GetProperty("__unity-collection-view-internal-binding") != null;
					if (flag2)
					{
						Foldout foldout = reusableCollectionItem.bindableElement.Q<Foldout>(null, null);
						bool flag3 = foldout != null;
						if (flag3)
						{
							foldout.value = moveIn;
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x04000131 RID: 305
		private static readonly string k_SizeFieldLabel = "Size";

		// Token: 0x04000132 RID: 306
		private const int k_FoldoutTabIndex = 10;

		// Token: 0x04000133 RID: 307
		private const int k_ArraySizeFieldTabIndex = 20;

		// Token: 0x04000134 RID: 308
		private bool m_ShowBoundCollectionSize = true;

		// Token: 0x04000135 RID: 309
		private bool m_ShowFoldoutHeader;

		// Token: 0x04000136 RID: 310
		private string m_HeaderTitle;

		// Token: 0x0400013A RID: 314
		private Label m_ListViewLabel;

		// Token: 0x0400013B RID: 315
		private Foldout m_Foldout;

		// Token: 0x0400013C RID: 316
		private TextField m_ArraySizeField;

		// Token: 0x0400013D RID: 317
		private bool m_IsOverMultiEditLimit;

		// Token: 0x0400013E RID: 318
		private int m_MaxMultiEditCount;

		// Token: 0x0400013F RID: 319
		private VisualElement m_Footer;

		// Token: 0x04000140 RID: 320
		private Button m_AddButton;

		// Token: 0x04000141 RID: 321
		private Button m_RemoveButton;

		// Token: 0x04000142 RID: 322
		private Action<IEnumerable<int>> m_ItemAddedCallback;

		// Token: 0x04000143 RID: 323
		private Action<IEnumerable<int>> m_ItemRemovedCallback;

		// Token: 0x04000144 RID: 324
		private Action m_ItemsSourceSizeChangedCallback;

		// Token: 0x04000145 RID: 325
		private ListViewReorderMode m_ReorderMode;

		// Token: 0x04000147 RID: 327
		public new static readonly string ussClassName = "unity-list-view";

		// Token: 0x04000148 RID: 328
		public new static readonly string itemUssClassName = BaseListView.ussClassName + "__item";

		// Token: 0x04000149 RID: 329
		public static readonly string emptyLabelUssClassName = BaseListView.ussClassName + "__empty-label";

		// Token: 0x0400014A RID: 330
		public static readonly string overMaxMultiEditLimitClassName = BaseListView.ussClassName + "__over-max-multi-edit-limit-label";

		// Token: 0x0400014B RID: 331
		public static readonly string reorderableUssClassName = BaseListView.ussClassName + "__reorderable";

		// Token: 0x0400014C RID: 332
		public static readonly string reorderableItemUssClassName = BaseListView.reorderableUssClassName + "-item";

		// Token: 0x0400014D RID: 333
		public static readonly string reorderableItemContainerUssClassName = BaseListView.reorderableItemUssClassName + "__container";

		// Token: 0x0400014E RID: 334
		public static readonly string reorderableItemHandleUssClassName = BaseListView.reorderableUssClassName + "-handle";

		// Token: 0x0400014F RID: 335
		public static readonly string reorderableItemHandleBarUssClassName = BaseListView.reorderableItemHandleUssClassName + "-bar";

		// Token: 0x04000150 RID: 336
		public static readonly string footerUssClassName = BaseListView.ussClassName + "__footer";

		// Token: 0x04000151 RID: 337
		public static readonly string foldoutHeaderUssClassName = BaseListView.ussClassName + "__foldout-header";

		// Token: 0x04000152 RID: 338
		public static readonly string arraySizeFieldUssClassName = BaseListView.ussClassName + "__size-field";

		// Token: 0x04000153 RID: 339
		public static readonly string arraySizeFieldWithHeaderUssClassName = BaseListView.arraySizeFieldUssClassName + "--with-header";

		// Token: 0x04000154 RID: 340
		public static readonly string arraySizeFieldWithFooterUssClassName = BaseListView.arraySizeFieldUssClassName + "--with-footer";

		// Token: 0x04000155 RID: 341
		public static readonly string listViewWithHeaderUssClassName = BaseListView.ussClassName + "--with-header";

		// Token: 0x04000156 RID: 342
		public static readonly string listViewWithFooterUssClassName = BaseListView.ussClassName + "--with-footer";

		// Token: 0x04000157 RID: 343
		public static readonly string scrollViewWithFooterUssClassName = BaseListView.ussClassName + "__scroll-view--with-footer";

		// Token: 0x04000158 RID: 344
		public static readonly string footerAddButtonName = BaseListView.ussClassName + "__add-button";

		// Token: 0x04000159 RID: 345
		public static readonly string footerRemoveButtonName = BaseListView.ussClassName + "__remove-button";

		// Token: 0x0400015A RID: 346
		private string m_MaxMultiEditStr;

		// Token: 0x0400015B RID: 347
		private static readonly string k_EmptyListStr = "List is empty";

		// Token: 0x0200005C RID: 92
		public new class UxmlTraits : BaseVerticalCollectionView.UxmlTraits
		{
			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000F344 File Offset: 0x0000D544
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x060003E2 RID: 994 RVA: 0x0000F364 File Offset: 0x0000D564
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				BaseListView baseListView = (BaseListView)ve;
				baseListView.reorderMode = this.m_ReorderMode.GetValueFromBag(bag, cc);
				baseListView.showFoldoutHeader = this.m_ShowFoldoutHeader.GetValueFromBag(bag, cc);
				baseListView.headerTitle = this.m_HeaderTitle.GetValueFromBag(bag, cc);
				baseListView.showAddRemoveFooter = this.m_ShowAddRemoveFooter.GetValueFromBag(bag, cc);
				baseListView.showBoundCollectionSize = this.m_ShowBoundCollectionSize.GetValueFromBag(bag, cc);
			}

			// Token: 0x060003E3 RID: 995 RVA: 0x0000F3E8 File Offset: 0x0000D5E8
			protected UxmlTraits()
			{
				this.m_PickingMode.defaultValue = PickingMode.Ignore;
			}

			// Token: 0x0400015C RID: 348
			private readonly UxmlBoolAttributeDescription m_ShowFoldoutHeader = new UxmlBoolAttributeDescription
			{
				name = "show-foldout-header",
				defaultValue = false
			};

			// Token: 0x0400015D RID: 349
			private readonly UxmlStringAttributeDescription m_HeaderTitle = new UxmlStringAttributeDescription
			{
				name = "header-title",
				defaultValue = string.Empty
			};

			// Token: 0x0400015E RID: 350
			private readonly UxmlBoolAttributeDescription m_ShowAddRemoveFooter = new UxmlBoolAttributeDescription
			{
				name = "show-add-remove-footer",
				defaultValue = false
			};

			// Token: 0x0400015F RID: 351
			private readonly UxmlEnumAttributeDescription<ListViewReorderMode> m_ReorderMode = new UxmlEnumAttributeDescription<ListViewReorderMode>
			{
				name = "reorder-mode",
				defaultValue = ListViewReorderMode.Simple
			};

			// Token: 0x04000160 RID: 352
			private readonly UxmlBoolAttributeDescription m_ShowBoundCollectionSize = new UxmlBoolAttributeDescription
			{
				name = "show-bound-collection-size",
				defaultValue = true
			};
		}
	}
}
