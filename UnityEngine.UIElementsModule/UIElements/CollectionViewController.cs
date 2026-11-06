using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x02000039 RID: 57
	public abstract class CollectionViewController : IDisposable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000257 RID: 599 RVA: 0x000080F8 File Offset: 0x000062F8
		// (remove) Token: 0x06000258 RID: 600 RVA: 0x00008130 File Offset: 0x00006330
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action itemsSourceChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000259 RID: 601 RVA: 0x00008168 File Offset: 0x00006368
		// (remove) Token: 0x0600025A RID: 602 RVA: 0x000081A0 File Offset: 0x000063A0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int, int> itemIndexChanged;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600025B RID: 603 RVA: 0x000081D5 File Offset: 0x000063D5
		// (set) Token: 0x0600025C RID: 604 RVA: 0x000081E0 File Offset: 0x000063E0
		public virtual IList itemsSource
		{
			get
			{
				return this.m_ItemsSource;
			}
			set
			{
				bool flag = this.m_ItemsSource == value;
				if (!flag)
				{
					this.m_ItemsSource = value;
					bool flag2 = this.m_View.GetProperty("__unity-collection-view-internal-binding") == null;
					if (flag2)
					{
						this.m_View.RefreshItems();
					}
					this.RaiseItemsSourceChanged();
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00008235 File Offset: 0x00006435
		protected void SetItemsSourceWithoutNotify(IList source)
		{
			this.m_ItemsSource = source;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000823F File Offset: 0x0000643F
		protected BaseVerticalCollectionView view
		{
			get
			{
				return this.m_View;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00008247 File Offset: 0x00006447
		public void SetView(BaseVerticalCollectionView collectionView)
		{
			this.m_View = collectionView;
			this.PrepareView();
			Assert.IsNotNull<BaseVerticalCollectionView>(this.m_View, "View must not be null.");
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void PrepareView()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00008269 File Offset: 0x00006469
		public virtual void Dispose()
		{
			this.itemsSourceChanged = null;
			this.itemIndexChanged = null;
			this.m_View = null;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00008284 File Offset: 0x00006484
		public virtual int GetItemsCount()
		{
			IList itemsSource = this.m_ItemsSource;
			return (itemsSource != null) ? itemsSource.Count : 0;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000082A8 File Offset: 0x000064A8
		internal virtual int GetItemsMinCount()
		{
			return this.GetItemsCount();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000082B0 File Offset: 0x000064B0
		public virtual int GetIndexForId(int id)
		{
			return id;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000082C4 File Offset: 0x000064C4
		public virtual int GetIdForIndex(int index)
		{
			return index;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000082D8 File Offset: 0x000064D8
		public virtual object GetItemForIndex(int index)
		{
			bool flag = this.m_ItemsSource == null;
			object obj;
			if (flag)
			{
				obj = null;
			}
			else
			{
				bool flag2 = index < 0 || index >= this.m_ItemsSource.Count;
				if (flag2)
				{
					obj = null;
				}
				else
				{
					obj = this.m_ItemsSource[index];
				}
			}
			return obj;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00008328 File Offset: 0x00006528
		internal virtual object GetItemForId(int id)
		{
			bool flag = this.m_ItemsSource == null;
			object obj;
			if (flag)
			{
				obj = null;
			}
			else
			{
				int indexForId = this.GetIndexForId(id);
				bool flag2 = indexForId < 0 || indexForId >= this.m_ItemsSource.Count;
				if (flag2)
				{
					obj = null;
				}
				else
				{
					obj = this.m_ItemsSource[indexForId];
				}
			}
			return obj;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000837F File Offset: 0x0000657F
		internal virtual void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			reusableItem.Init(this.MakeItem());
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00008390 File Offset: 0x00006590
		internal virtual void InvokeBindItem(ReusableCollectionItem reusableItem, int index)
		{
			this.BindItem(reusableItem.bindableElement, index);
			reusableItem.SetSelected(this.m_View.selectedIndices.Contains(index));
			reusableItem.rootElement.pseudoStates &= ~PseudoStates.Hover;
			reusableItem.index = index;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000083E1 File Offset: 0x000065E1
		internal virtual void InvokeUnbindItem(ReusableCollectionItem reusableItem, int index)
		{
			this.UnbindItem(reusableItem.bindableElement, index);
			reusableItem.index = -1;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000083FA File Offset: 0x000065FA
		internal virtual void InvokeDestroyItem(ReusableCollectionItem reusableItem)
		{
			this.DestroyItem(reusableItem.bindableElement);
		}

		// Token: 0x0600026C RID: 620
		protected abstract VisualElement MakeItem();

		// Token: 0x0600026D RID: 621
		protected abstract void BindItem(VisualElement element, int index);

		// Token: 0x0600026E RID: 622
		protected abstract void UnbindItem(VisualElement element, int index);

		// Token: 0x0600026F RID: 623
		protected abstract void DestroyItem(VisualElement element);

		// Token: 0x06000270 RID: 624 RVA: 0x0000840A File Offset: 0x0000660A
		protected void RaiseItemsSourceChanged()
		{
			Action action = this.itemsSourceChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000841F File Offset: 0x0000661F
		protected void RaiseItemIndexChanged(int srcIndex, int dstIndex)
		{
			Action<int, int> action = this.itemIndexChanged;
			if (action != null)
			{
				action(srcIndex, dstIndex);
			}
		}

		// Token: 0x040000B4 RID: 180
		private BaseVerticalCollectionView m_View;

		// Token: 0x040000B5 RID: 181
		private IList m_ItemsSource;
	}
}
