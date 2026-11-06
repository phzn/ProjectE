using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x02000065 RID: 101
	public abstract class BaseTreeView : BaseVerticalCollectionView
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00011026 File Offset: 0x0000F226
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00011033 File Offset: 0x0000F233
		public new IList itemsSource
		{
			get
			{
				return this.viewController.itemsSource;
			}
			internal set
			{
				base.GetOrCreateViewController().itemsSource = value;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011042 File Offset: 0x0000F242
		public void SetRootItems<T>(IList<TreeViewItemData<T>> rootItems)
		{
			this.SetRootItemsInternal<T>(rootItems);
		}

		// Token: 0x0600044D RID: 1101
		internal abstract void SetRootItemsInternal<T>(IList<TreeViewItemData<T>> rootItems);

		// Token: 0x0600044E RID: 1102 RVA: 0x00011050 File Offset: 0x0000F250
		public IEnumerable<int> GetRootIds()
		{
			return this.viewController.GetRootItemIds();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00011070 File Offset: 0x0000F270
		public int GetTreeCount()
		{
			return this.viewController.GetTreeItemsCount();
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001108D File Offset: 0x0000F28D
		public new BaseTreeViewController viewController
		{
			get
			{
				return base.viewController as BaseTreeViewController;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001109A File Offset: 0x0000F29A
		private protected override void CreateVirtualizationController()
		{
			base.CreateVirtualizationController<ReusableTreeViewItem>();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000110A4 File Offset: 0x0000F2A4
		public override void SetViewController(CollectionViewController controller)
		{
			bool flag = this.viewController != null;
			if (flag)
			{
				this.viewController.itemIndexChanged -= this.OnItemIndexChanged;
			}
			base.SetViewController(controller);
			bool flag2 = this.viewController != null;
			if (flag2)
			{
				this.viewController.itemIndexChanged += this.OnItemIndexChanged;
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00011108 File Offset: 0x0000F308
		private void OnItemIndexChanged(int srcIndex, int dstIndex)
		{
			base.RefreshItems();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011112 File Offset: 0x0000F312
		internal override ICollectionDragAndDropController CreateDragAndDropController()
		{
			return new TreeViewReorderableDragAndDropController(this);
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0001111A File Offset: 0x0000F31A
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00011122 File Offset: 0x0000F322
		public bool autoExpand
		{
			get
			{
				return this.m_AutoExpand;
			}
			set
			{
				this.m_AutoExpand = value;
				BaseTreeViewController viewController = this.viewController;
				if (viewController != null)
				{
					viewController.RegenerateWrappers();
				}
				base.RefreshItems();
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00011145 File Offset: 0x0000F345
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0001114D File Offset: 0x0000F34D
		internal List<int> expandedItemIds
		{
			get
			{
				return this.m_ExpandedItemIds;
			}
			set
			{
				this.m_ExpandedItemIds = value;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00011156 File Offset: 0x0000F356
		public BaseTreeView()
			: this(-1)
		{
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011161 File Offset: 0x0000F361
		public BaseTreeView(int itemHeight)
			: base(null, (float)itemHeight)
		{
			this.m_ExpandedItemIds = new List<int>();
			base.AddToClassList(BaseTreeView.ussClassName);
			base.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnTreeViewPointerUp), TrickleDown.TrickleDown);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001119C File Offset: 0x0000F39C
		public int GetIdForIndex(int index)
		{
			return this.viewController.GetIdForIndex(index);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000111BC File Offset: 0x0000F3BC
		public int GetParentIdForIndex(int index)
		{
			return this.viewController.GetParentId(this.GetIdForIndex(index));
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000111E0 File Offset: 0x0000F3E0
		public IEnumerable<int> GetChildrenIdsForIndex(int index)
		{
			return this.viewController.GetChildrenIdsByIndex(index);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011200 File Offset: 0x0000F400
		public IEnumerable<TreeViewItemData<T>> GetSelectedItems<T>()
		{
			return this.GetSelectedItemsInternal<T>();
		}

		// Token: 0x0600045F RID: 1119
		private protected abstract IEnumerable<TreeViewItemData<T>> GetSelectedItemsInternal<T>();

		// Token: 0x06000460 RID: 1120 RVA: 0x00011218 File Offset: 0x0000F418
		public T GetItemDataForIndex<T>(int index)
		{
			return this.GetItemDataForIndexInternal<T>(index);
		}

		// Token: 0x06000461 RID: 1121
		private protected abstract T GetItemDataForIndexInternal<T>(int index);

		// Token: 0x06000462 RID: 1122 RVA: 0x00011234 File Offset: 0x0000F434
		public T GetItemDataForId<T>(int id)
		{
			return this.GetItemDataForIdInternal<T>(id);
		}

		// Token: 0x06000463 RID: 1123
		private protected abstract T GetItemDataForIdInternal<T>(int id);

		// Token: 0x06000464 RID: 1124 RVA: 0x0001124D File Offset: 0x0000F44D
		public void AddItem<T>(TreeViewItemData<T> item, int parentId = -1, int childIndex = -1, bool rebuildTree = true)
		{
			this.AddItemInternal<T>(item, parentId, childIndex, rebuildTree);
		}

		// Token: 0x06000465 RID: 1125
		private protected abstract void AddItemInternal<T>(TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree);

		// Token: 0x06000466 RID: 1126 RVA: 0x0001125C File Offset: 0x0000F45C
		public bool TryRemoveItem(int id)
		{
			bool flag = this.viewController.TryRemoveItem(id, true);
			bool flag2;
			if (flag)
			{
				base.RefreshItems();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001128C File Offset: 0x0000F48C
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			bool flag = this.viewController != null;
			if (flag)
			{
				this.viewController.RebuildTree();
				base.RefreshItems();
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000112C4 File Offset: 0x0000F4C4
		private protected override bool HandleItemNavigation(bool moveIn, bool altPressed)
		{
			int num = 1;
			bool flag = false;
			foreach (int num2 in base.selectedIds)
			{
				int indexForId = this.viewController.GetIndexForId(num2);
				bool flag2 = !this.viewController.HasChildrenByIndex(indexForId);
				if (flag2)
				{
					break;
				}
				bool flag3 = moveIn && !this.IsExpandedByIndex(indexForId);
				if (flag3)
				{
					this.ExpandItemByIndex(indexForId, altPressed);
					flag = true;
				}
				else
				{
					bool flag4 = !moveIn && this.IsExpandedByIndex(indexForId);
					if (flag4)
					{
						this.CollapseItemByIndex(indexForId, altPressed);
						flag = true;
					}
				}
			}
			bool flag5 = flag;
			bool flag6;
			if (flag5)
			{
				flag6 = true;
			}
			else
			{
				bool flag7 = !moveIn;
				if (flag7)
				{
					int idForIndex = this.viewController.GetIdForIndex(base.selectedIndex);
					int parentId = this.viewController.GetParentId(idForIndex);
					bool flag8 = parentId != -1;
					if (flag8)
					{
						this.SetSelectionById(parentId);
						base.ScrollToItemById(parentId);
						return true;
					}
					num = -1;
				}
				int num3 = base.selectedIndex;
				bool flag9;
				do
				{
					num3 += num;
					flag9 = this.viewController.HasChildrenByIndex(num3);
				}
				while (!flag9 && num3 >= 0 && num3 < this.itemsSource.Count);
				bool flag10 = flag9;
				if (flag10)
				{
					base.SetSelection(num3);
					base.ScrollToItem(num3);
					flag6 = true;
				}
				else
				{
					flag6 = false;
				}
			}
			return flag6;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011450 File Offset: 0x0000F650
		public void SetSelectionById(int id)
		{
			this.SetSelectionById(new int[] { id });
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00011464 File Offset: 0x0000F664
		public void SetSelectionById(IEnumerable<int> ids)
		{
			this.SetSelectionInternalById(ids, true);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00011470 File Offset: 0x0000F670
		public void SetSelectionByIdWithoutNotify(IEnumerable<int> ids)
		{
			this.SetSelectionInternalById(ids, false);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001147C File Offset: 0x0000F67C
		internal void SetSelectionInternalById(IEnumerable<int> ids, bool sendNotification)
		{
			bool flag = ids == null;
			if (!flag)
			{
				List<int> list = ids.Select<int, int>((int id) => this.GetItemIndex(id, true)).ToList<int>();
				base.SetSelectionInternal(list, sendNotification);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000114B8 File Offset: 0x0000F6B8
		public void AddToSelectionById(int id)
		{
			int itemIndex = this.GetItemIndex(id, true);
			base.Rebuild();
			base.AddToSelection(itemIndex);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000114E0 File Offset: 0x0000F6E0
		public void RemoveFromSelectionById(int id)
		{
			int itemIndex = this.GetItemIndex(id, false);
			base.RemoveFromSelection(itemIndex);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011500 File Offset: 0x0000F700
		private int GetItemIndex(int id, bool expand = false)
		{
			if (expand)
			{
				for (int num = this.viewController.GetParentId(id); num != -1; num = this.viewController.GetParentId(num))
				{
					bool flag = !this.m_ExpandedItemIds.Contains(num);
					if (flag)
					{
						this.viewController.ExpandItem(num, false, true);
					}
				}
			}
			return this.viewController.GetIndexForId(id);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011574 File Offset: 0x0000F774
		internal void CopyExpandedStates(int sourceId, int targetId)
		{
			bool flag = this.IsExpanded(sourceId);
			if (flag)
			{
				this.ExpandItem(targetId, false);
				bool flag2 = this.viewController.HasChildren(sourceId);
				if (flag2)
				{
					bool flag3 = this.viewController.GetChildrenIds(sourceId).Count<int>() != this.viewController.GetChildrenIds(targetId).Count<int>();
					if (flag3)
					{
						Debug.LogWarning("Source and target hierarchies are not the same");
					}
					else
					{
						for (int i = 0; i < this.viewController.GetChildrenIds(sourceId).Count<int>(); i++)
						{
							int num = this.viewController.GetChildrenIds(sourceId).ElementAt<int>(i);
							int num2 = this.viewController.GetChildrenIds(targetId).ElementAt<int>(i);
							this.CopyExpandedStates(num, num2);
						}
					}
				}
			}
			else
			{
				this.CollapseItem(targetId, false);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011650 File Offset: 0x0000F850
		public bool IsExpanded(int id)
		{
			return this.viewController.IsExpanded(id);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001166E File Offset: 0x0000F86E
		public void CollapseItem(int id, bool collapseAllChildren = false)
		{
			this.viewController.CollapseItem(id, collapseAllChildren);
			base.RefreshItems();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00011686 File Offset: 0x0000F886
		public void ExpandItem(int id, bool expandAllChildren = false)
		{
			this.viewController.ExpandItem(id, expandAllChildren, true);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00011698 File Offset: 0x0000F898
		public void ExpandRootItems()
		{
			foreach (int num in this.viewController.GetRootItemIds())
			{
				this.viewController.ExpandItem(num, false, false);
			}
			base.RefreshItems();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000116FC File Offset: 0x0000F8FC
		public void ExpandAll()
		{
			this.viewController.ExpandAll();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001170B File Offset: 0x0000F90B
		public void CollapseAll()
		{
			this.viewController.CollapseAll();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001171A File Offset: 0x0000F91A
		private void OnTreeViewPointerUp(PointerUpEvent evt)
		{
			base.scrollView.contentContainer.Focus();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00011730 File Offset: 0x0000F930
		private bool IsExpandedByIndex(int index)
		{
			return this.viewController.IsExpandedByIndex(index);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00011750 File Offset: 0x0000F950
		private void CollapseItemByIndex(int index, bool collapseAll)
		{
			bool flag = !this.viewController.HasChildrenByIndex(index);
			if (!flag)
			{
				this.viewController.CollapseItemByIndex(index, collapseAll);
				base.RefreshItems();
				base.SaveViewData();
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00011790 File Offset: 0x0000F990
		private void ExpandItemByIndex(int index, bool expandAll)
		{
			bool flag = !this.viewController.HasChildrenByIndex(index);
			if (!flag)
			{
				this.viewController.ExpandItemByIndex(index, expandAll, true);
				base.RefreshItems();
				base.SaveViewData();
			}
		}

		// Token: 0x0400019A RID: 410
		public new static readonly string ussClassName = "unity-tree-view";

		// Token: 0x0400019B RID: 411
		public new static readonly string itemUssClassName = BaseTreeView.ussClassName + "__item";

		// Token: 0x0400019C RID: 412
		public static readonly string itemToggleUssClassName = BaseTreeView.ussClassName + "__item-toggle";

		// Token: 0x0400019D RID: 413
		public static readonly string itemIndentsContainerUssClassName = BaseTreeView.ussClassName + "__item-indents";

		// Token: 0x0400019E RID: 414
		public static readonly string itemIndentUssClassName = BaseTreeView.ussClassName + "__item-indent";

		// Token: 0x0400019F RID: 415
		public static readonly string itemContentContainerUssClassName = BaseTreeView.ussClassName + "__item-content";

		// Token: 0x040001A0 RID: 416
		private bool m_AutoExpand;

		// Token: 0x040001A1 RID: 417
		[SerializeField]
		private List<int> m_ExpandedItemIds;

		// Token: 0x02000066 RID: 102
		public new class UxmlTraits : BaseVerticalCollectionView.UxmlTraits
		{
			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x0600047D RID: 1149 RVA: 0x00011858 File Offset: 0x0000FA58
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x0600047E RID: 1150 RVA: 0x00011878 File Offset: 0x0000FA78
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				BaseTreeView baseTreeView = (BaseTreeView)ve;
				baseTreeView.autoExpand = this.m_AutoExpand.GetValueFromBag(bag, cc);
			}

			// Token: 0x040001A2 RID: 418
			private readonly UxmlBoolAttributeDescription m_AutoExpand = new UxmlBoolAttributeDescription
			{
				name = "auto-expand",
				defaultValue = false
			};
		}
	}
}
