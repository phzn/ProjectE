using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x02000050 RID: 80
	internal abstract class VerticalVirtualizationController<T> : CollectionVirtualizationController where T : ReusableCollectionItem, new()
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000C84C File Offset: 0x0000AA4C
		public override IEnumerable<ReusableCollectionItem> activeItems
		{
			get
			{
				return this.m_ActiveItems;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000C854 File Offset: 0x0000AA54
		internal int itemsCount
		{
			get
			{
				return this.m_CollectionView.itemsSource.Count;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C866 File Offset: 0x0000AA66
		protected virtual bool VisibleItemPredicate(T i)
		{
			return i.rootElement.style.display == DisplayStyle.Flex;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000C888 File Offset: 0x0000AA88
		internal T firstVisibleItem
		{
			get
			{
				return this.m_ActiveItems.FirstOrDefault<T>(this.m_VisibleItemPredicateDelegate);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000C89B File Offset: 0x0000AA9B
		internal T lastVisibleItem
		{
			get
			{
				return this.m_ActiveItems.LastOrDefault<T>(this.m_VisibleItemPredicateDelegate);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000C8AE File Offset: 0x0000AAAE
		public override int visibleItemCount
		{
			get
			{
				return this.m_ActiveItems.Count<T>(this.m_VisibleItemPredicateDelegate);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000C8C1 File Offset: 0x0000AAC1
		protected SerializedVirtualizationData serializedData
		{
			get
			{
				return this.m_CollectionView.serializedVirtualizationData;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000C8CE File Offset: 0x0000AACE
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000C90C File Offset: 0x0000AB0C
		public override int firstVisibleIndex
		{
			get
			{
				return Mathf.Min(this.serializedData.firstVisibleIndex, (this.m_CollectionView.viewController != null) ? (this.m_CollectionView.viewController.GetItemsCount() - 1) : this.serializedData.firstVisibleIndex);
			}
			protected set
			{
				this.serializedData.firstVisibleIndex = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000C91A File Offset: 0x0000AB1A
		protected float lastHeight
		{
			get
			{
				return this.m_CollectionView.lastHeight;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000C927 File Offset: 0x0000AB27
		protected virtual bool alwaysRebindOnRefresh
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C92C File Offset: 0x0000AB2C
		protected VerticalVirtualizationController(BaseVerticalCollectionView collectionView)
			: base(collectionView.scrollView)
		{
			this.m_CollectionView = collectionView;
			this.m_ActiveItems = new List<T>();
			this.m_VisibleItemPredicateDelegate = new Func<T, bool>(this.VisibleItemPredicate);
			this.m_ScrollView.contentContainer.disableClipping = false;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		public override void Refresh(bool rebuild)
		{
			bool flag = this.m_CollectionView.HasValidDataAndBindings();
			for (int i = 0; i < this.m_ActiveItems.Count; i++)
			{
				int num = this.firstVisibleIndex + i;
				T t = this.m_ActiveItems[i];
				bool flag2 = t.rootElement.style.display == DisplayStyle.Flex;
				if (rebuild)
				{
					bool flag3 = flag;
					if (flag3)
					{
						this.m_CollectionView.viewController.InvokeUnbindItem(t, t.index);
					}
					this.m_Pool.Release(t);
				}
				else
				{
					bool flag4 = this.m_CollectionView.itemsSource != null && num >= 0 && num < this.itemsCount;
					if (flag4)
					{
						bool flag5 = !flag;
						if (!flag5)
						{
							bool flag6 = flag2 || this.alwaysRebindOnRefresh;
							if (flag6)
							{
								bool flag7 = t.index != -1;
								if (flag7)
								{
									this.m_CollectionView.viewController.InvokeUnbindItem(t, t.index);
								}
								this.Setup(t, num);
							}
						}
					}
					else
					{
						bool flag8 = flag2;
						if (flag8)
						{
							this.ReleaseItem(i--);
						}
					}
				}
			}
			if (rebuild)
			{
				this.m_Pool.Clear();
				this.m_ActiveItems.Clear();
				this.m_ScrollView.Clear();
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000CB98 File Offset: 0x0000AD98
		protected void Setup(T recycledItem, int newIndex)
		{
			bool isDragGhost = recycledItem.isDragGhost;
			bool flag = this.GetDraggedIndex() == newIndex;
			if (flag)
			{
				bool flag2 = recycledItem.index != -1;
				if (flag2)
				{
					this.m_CollectionView.viewController.InvokeUnbindItem(recycledItem, recycledItem.index);
				}
				recycledItem.SetDragGhost(true);
				recycledItem.index = this.m_DraggedItem.index;
				recycledItem.rootElement.style.display = DisplayStyle.Flex;
			}
			else
			{
				bool flag3 = isDragGhost;
				if (flag3)
				{
					recycledItem.SetDragGhost(false);
				}
				bool flag4 = newIndex >= this.itemsCount;
				if (flag4)
				{
					recycledItem.rootElement.style.display = DisplayStyle.None;
					bool flag5 = recycledItem.index >= 0 && recycledItem.index < this.itemsCount;
					if (flag5)
					{
						this.m_CollectionView.viewController.InvokeUnbindItem(recycledItem, recycledItem.index);
						recycledItem.index = -1;
					}
				}
				else
				{
					recycledItem.rootElement.style.display = DisplayStyle.Flex;
					int idForIndex = this.m_CollectionView.viewController.GetIdForIndex(newIndex);
					bool flag6 = recycledItem.index == newIndex && recycledItem.id == idForIndex;
					if (!flag6)
					{
						bool flag7 = this.m_CollectionView.showAlternatingRowBackgrounds != AlternatingRowBackground.None && newIndex % 2 == 1;
						recycledItem.rootElement.EnableInClassList(BaseVerticalCollectionView.itemAlternativeBackgroundUssClassName, flag7);
						int index = recycledItem.index;
						bool flag8 = recycledItem.index != -1;
						if (flag8)
						{
							this.m_CollectionView.viewController.InvokeUnbindItem(recycledItem, recycledItem.index);
						}
						recycledItem.index = newIndex;
						recycledItem.id = idForIndex;
						int num = newIndex - this.firstVisibleIndex;
						bool flag9 = num >= this.m_ScrollView.contentContainer.childCount;
						if (flag9)
						{
							recycledItem.rootElement.BringToFront();
						}
						else
						{
							bool flag10 = num >= 0;
							if (flag10)
							{
								recycledItem.rootElement.PlaceBehind(this.m_ScrollView.contentContainer[num]);
							}
							else
							{
								recycledItem.rootElement.SendToBack();
							}
						}
						this.m_CollectionView.viewController.InvokeBindItem(recycledItem, newIndex);
						this.HandleFocus(recycledItem, index);
					}
				}
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000CE7C File Offset: 0x0000B07C
		public override void OnFocus(VisualElement leafTarget)
		{
			bool flag = leafTarget == this.m_ScrollView.contentContainer;
			if (!flag)
			{
				this.m_LastFocusedElementTreeChildIndexes.Clear();
				bool flag2 = this.m_ScrollView.contentContainer.FindElementInTree(leafTarget, this.m_LastFocusedElementTreeChildIndexes);
				if (flag2)
				{
					VisualElement visualElement = this.m_ScrollView.contentContainer[this.m_LastFocusedElementTreeChildIndexes[0]];
					foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
					{
						bool flag3 = reusableCollectionItem.rootElement == visualElement;
						if (flag3)
						{
							this.m_LastFocusedElementIndex = reusableCollectionItem.index;
							break;
						}
					}
					this.m_LastFocusedElementTreeChildIndexes.RemoveAt(0);
				}
				else
				{
					this.m_LastFocusedElementIndex = -1;
				}
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000CF60 File Offset: 0x0000B160
		public override void OnBlur(VisualElement willFocus)
		{
			bool flag = willFocus == null || willFocus != this.m_ScrollView.contentContainer;
			if (flag)
			{
				this.m_LastFocusedElementTreeChildIndexes.Clear();
				this.m_LastFocusedElementIndex = -1;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		private void HandleFocus(ReusableCollectionItem recycledItem, int previousIndex)
		{
			bool flag = this.m_LastFocusedElementIndex == -1;
			if (!flag)
			{
				bool flag2 = this.m_LastFocusedElementIndex == recycledItem.index;
				if (flag2)
				{
					VisualElement visualElement = recycledItem.rootElement.ElementAtTreePath(this.m_LastFocusedElementTreeChildIndexes);
					if (visualElement != null)
					{
						visualElement.Focus();
					}
				}
				else
				{
					bool flag3 = this.m_LastFocusedElementIndex != previousIndex;
					if (flag3)
					{
						VisualElement visualElement2 = recycledItem.rootElement.ElementAtTreePath(this.m_LastFocusedElementTreeChildIndexes);
						if (visualElement2 != null)
						{
							visualElement2.Blur();
						}
					}
					else
					{
						this.m_ScrollView.contentContainer.Focus();
					}
				}
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000D030 File Offset: 0x0000B230
		public override void UpdateBackground()
		{
			float num;
			bool flag = this.m_CollectionView.showAlternatingRowBackgrounds != AlternatingRowBackground.All || (num = this.m_ScrollView.contentViewport.resolvedStyle.height - this.GetExpectedContentHeight()) <= 0f;
			if (flag)
			{
				VisualElement emptyRows = this.m_EmptyRows;
				if (emptyRows != null)
				{
					emptyRows.RemoveFromHierarchy();
				}
			}
			else
			{
				bool flag2 = this.lastVisibleItem == null;
				if (!flag2)
				{
					bool flag3 = this.m_EmptyRows == null;
					if (flag3)
					{
						this.m_EmptyRows = new VisualElement
						{
							classList = { BaseVerticalCollectionView.backgroundFillUssClassName }
						};
					}
					bool flag4 = this.m_EmptyRows.parent == null;
					if (flag4)
					{
						this.m_ScrollView.contentViewport.Add(this.m_EmptyRows);
					}
					float expectedItemHeight = this.GetExpectedItemHeight(-1);
					int num2 = Mathf.FloorToInt(num / expectedItemHeight) + 1;
					bool flag5 = num2 > this.m_EmptyRows.childCount;
					if (flag5)
					{
						int num3 = num2 - this.m_EmptyRows.childCount;
						for (int i = 0; i < num3; i++)
						{
							VisualElement visualElement = new VisualElement();
							visualElement.style.flexShrink = 0f;
							this.m_EmptyRows.Add(visualElement);
						}
					}
					T t = this.lastVisibleItem;
					int num4 = ((t != null) ? t.index : (-1));
					int childCount = this.m_EmptyRows.hierarchy.childCount;
					for (int j = 0; j < childCount; j++)
					{
						VisualElement visualElement2 = this.m_EmptyRows.hierarchy[j];
						num4++;
						visualElement2.style.height = expectedItemHeight;
						visualElement2.EnableInClassList(BaseVerticalCollectionView.itemAlternativeBackgroundUssClassName, num4 % 2 == 1);
					}
				}
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000D20C File Offset: 0x0000B40C
		internal override void StartDragItem(ReusableCollectionItem item)
		{
			this.m_DraggedItem = item as T;
			int num = this.m_ActiveItems.IndexOf(this.m_DraggedItem);
			this.m_ActiveItems.RemoveAt(num);
			T orMakeItemAtIndex = this.GetOrMakeItemAtIndex(num, num);
			this.Setup(orMakeItemAtIndex, this.m_DraggedItem.index);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000D26C File Offset: 0x0000B46C
		internal override void EndDrag(int dropIndex)
		{
			ReusableCollectionItem recycledItemFromIndex = this.m_CollectionView.GetRecycledItemFromIndex(dropIndex);
			int num = ((recycledItemFromIndex != null) ? this.m_ScrollView.IndexOf(recycledItemFromIndex.rootElement) : this.m_ActiveItems.Count);
			this.m_ScrollView.Insert(num, this.m_DraggedItem.rootElement);
			this.m_ActiveItems.Insert(num, this.m_DraggedItem);
			for (int i = 0; i < this.m_ActiveItems.Count; i++)
			{
				T t = this.m_ActiveItems[i];
				bool isDragGhost = t.isDragGhost;
				if (isDragGhost)
				{
					t.index = -1;
					this.ReleaseItem(i);
					i--;
				}
			}
			bool flag = Math.Min(dropIndex, this.itemsCount - 1) != this.m_DraggedItem.index;
			if (flag)
			{
				bool flag2 = this.lastVisibleItem != null;
				if (flag2)
				{
					this.lastVisibleItem.rootElement.style.display = DisplayStyle.None;
				}
				bool flag3 = this.m_DraggedItem.index < dropIndex;
				if (flag3)
				{
					this.m_CollectionView.viewController.InvokeUnbindItem(this.m_DraggedItem, this.m_DraggedItem.index);
					this.m_DraggedItem.index = -1;
				}
				else
				{
					bool flag4 = recycledItemFromIndex != null;
					if (flag4)
					{
						this.m_CollectionView.viewController.InvokeUnbindItem(recycledItemFromIndex, recycledItemFromIndex.index);
						recycledItemFromIndex.index = -1;
					}
				}
			}
			this.m_DraggedItem = default(T);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000D428 File Offset: 0x0000B628
		internal virtual T GetOrMakeItemAtIndex(int activeItemIndex = -1, int scrollViewIndex = -1)
		{
			T t = this.m_Pool.Get();
			bool flag = t.rootElement == null;
			if (flag)
			{
				this.m_CollectionView.viewController.InvokeMakeItem(t);
				t.onDestroy += this.OnDestroyItem;
			}
			t.PreAttachElement();
			bool flag2 = activeItemIndex == -1;
			if (flag2)
			{
				this.m_ActiveItems.Add(t);
			}
			else
			{
				this.m_ActiveItems.Insert(activeItemIndex, t);
			}
			bool flag3 = scrollViewIndex == -1;
			if (flag3)
			{
				this.m_ScrollView.Add(t.rootElement);
			}
			else
			{
				this.m_ScrollView.Insert(scrollViewIndex, t.rootElement);
			}
			return t;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000D500 File Offset: 0x0000B700
		internal virtual void ReleaseItem(int activeItemsIndex)
		{
			T t = this.m_ActiveItems[activeItemsIndex];
			bool flag = t.index != -1;
			if (flag)
			{
				this.m_CollectionView.viewController.InvokeUnbindItem(t, t.index);
			}
			this.m_Pool.Release(t);
			this.m_ActiveItems.Remove(t);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000D56E File Offset: 0x0000B76E
		private void OnDestroyItem(ReusableCollectionItem item)
		{
			this.m_CollectionView.viewController.InvokeDestroyItem(item);
			item.onDestroy -= this.OnDestroyItem;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000D598 File Offset: 0x0000B798
		protected int GetDraggedIndex()
		{
			ListViewDraggerAnimated listViewDraggerAnimated = this.m_CollectionView.dragger as ListViewDraggerAnimated;
			bool flag = listViewDraggerAnimated != null && listViewDraggerAnimated.isDragging;
			int num;
			if (flag)
			{
				num = listViewDraggerAnimated.draggedItem.index;
			}
			else
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x04000106 RID: 262
		private readonly ObjectPool<T> m_Pool = new ObjectPool<T>(() => new T(), null, delegate(T i)
		{
			i.DetachElement();
		}, delegate(T i)
		{
			i.DestroyElement();
		}, true, 10, 10000);

		// Token: 0x04000107 RID: 263
		protected BaseVerticalCollectionView m_CollectionView;

		// Token: 0x04000108 RID: 264
		protected const int k_ExtraVisibleItems = 2;

		// Token: 0x04000109 RID: 265
		protected List<T> m_ActiveItems;

		// Token: 0x0400010A RID: 266
		protected T m_DraggedItem;

		// Token: 0x0400010B RID: 267
		private int m_LastFocusedElementIndex = -1;

		// Token: 0x0400010C RID: 268
		private List<int> m_LastFocusedElementTreeChildIndexes = new List<int>();

		// Token: 0x0400010D RID: 269
		protected readonly Func<T, bool> m_VisibleItemPredicateDelegate;

		// Token: 0x0400010E RID: 270
		protected List<T> m_ScrollInsertionList = new List<T>();

		// Token: 0x0400010F RID: 271
		private VisualElement m_EmptyRows;
	}
}
