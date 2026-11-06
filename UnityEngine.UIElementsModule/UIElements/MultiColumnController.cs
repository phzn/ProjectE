using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UIElements.Internal;

namespace UnityEngine.UIElements
{
	// Token: 0x020000FB RID: 251
	public class MultiColumnController : IDisposable
	{
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060008A0 RID: 2208 RVA: 0x00021C08 File Offset: 0x0001FE08
		// (remove) Token: 0x060008A1 RID: 2209 RVA: 0x00021C40 File Offset: 0x0001FE40
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action columnSortingChanged;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060008A2 RID: 2210 RVA: 0x00021C78 File Offset: 0x0001FE78
		// (remove) Token: 0x060008A3 RID: 2211 RVA: 0x00021CB0 File Offset: 0x0001FEB0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ContextualMenuPopulateEvent, Column> headerContextMenuPopulateEvent;

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00021CE5 File Offset: 0x0001FEE5
		internal MultiColumnCollectionHeader header
		{
			get
			{
				return this.m_MultiColumnHeader;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00021CF0 File Offset: 0x0001FEF0
		public MultiColumnController(Columns columns, SortColumnDescriptions sortDescriptions, List<SortColumnDescription> sortedColumns)
		{
			this.m_MultiColumnHeader = new MultiColumnCollectionHeader(columns, sortDescriptions, sortedColumns)
			{
				viewDataKey = MultiColumnController.k_HeaderViewDataKey
			};
			this.m_MultiColumnHeader.columnSortingChanged += this.OnColumnSortingChanged;
			this.m_MultiColumnHeader.contextMenuPopulateEvent += this.OnContextMenuPopulateEvent;
			this.m_MultiColumnHeader.columnResized += this.OnColumnResized;
			this.m_MultiColumnHeader.viewDataRestored += this.OnViewDataRestored;
			this.m_MultiColumnHeader.columns.columnAdded += this.OnColumnAdded;
			this.m_MultiColumnHeader.columns.columnRemoved += this.OnColumnRemoved;
			this.m_MultiColumnHeader.columns.columnReordered += this.OnColumnReordered;
			this.m_MultiColumnHeader.columns.columnChanged += this.OnColumnsChanged;
			this.m_MultiColumnHeader.columns.changed += this.OnColumnChanged;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00021E10 File Offset: 0x00020010
		private static void BindCellItem<T>(VisualElement ve, int rowIndex, Column column, T item)
		{
			bool flag = column.bindCell != null;
			if (flag)
			{
				column.bindCell(ve, rowIndex);
			}
			else
			{
				MultiColumnController.DefaultBindCellItem<T>(ve, column, item);
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00021E48 File Offset: 0x00020048
		private static void UnbindCellItem(VisualElement ve, int rowIndex, Column column)
		{
			Action<VisualElement, int> unbindCell = column.unbindCell;
			if (unbindCell != null)
			{
				unbindCell(ve, rowIndex);
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00021E60 File Offset: 0x00020060
		private static VisualElement DefaultMakeCellItem()
		{
			Label label = new Label();
			label.AddToClassList(MultiColumnController.cellLabelUssClassName);
			return label;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00021E88 File Offset: 0x00020088
		private static void DefaultBindCellItem<T>(VisualElement ve, Column column, T item)
		{
			Label label = ve as Label;
			bool flag = label != null;
			if (flag)
			{
				label.text = item.ToString();
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00021EBC File Offset: 0x000200BC
		public VisualElement MakeItem()
		{
			VisualElement visualElement = new VisualElement
			{
				name = MultiColumnController.rowContainerUssClassName
			};
			visualElement.AddToClassList(MultiColumnController.rowContainerUssClassName);
			foreach (Column column in this.m_MultiColumnHeader.columns.visibleList)
			{
				VisualElement visualElement2 = new VisualElement();
				visualElement2.AddToClassList(MultiColumnController.cellUssClassName);
				Func<VisualElement> makeCell = column.makeCell;
				VisualElement visualElement3 = ((makeCell != null) ? makeCell() : null) ?? MultiColumnController.DefaultMakeCellItem();
				visualElement2.SetProperty(MultiColumnController.bindableElementPropertyName, visualElement3);
				visualElement2.Add(visualElement3);
				visualElement.Add(visualElement2);
			}
			return visualElement;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00021F84 File Offset: 0x00020184
		public void BindItem<T>(VisualElement element, int index, T item)
		{
			int num = 0;
			foreach (Column column in this.m_MultiColumnHeader.columns.visibleList)
			{
				MultiColumnCollectionHeader.ColumnData columnData;
				bool flag = !this.m_MultiColumnHeader.columnDataMap.TryGetValue(column, out columnData);
				if (!flag)
				{
					VisualElement visualElement = element[num++];
					VisualElement visualElement2 = visualElement.GetProperty(MultiColumnController.bindableElementPropertyName) as VisualElement;
					MultiColumnController.BindCellItem<T>(visualElement2, index, column, item);
					visualElement.style.width = columnData.control.resolvedStyle.width;
					visualElement.SetProperty(MultiColumnController.k_BoundColumnVePropertyName, column);
				}
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0002205C File Offset: 0x0002025C
		public void UnbindItem(VisualElement element, int index)
		{
			foreach (VisualElement visualElement in element.Children())
			{
				Column column = visualElement.GetProperty(MultiColumnController.k_BoundColumnVePropertyName) as Column;
				bool flag = column == null;
				if (!flag)
				{
					VisualElement visualElement2 = visualElement.GetProperty(MultiColumnController.bindableElementPropertyName) as VisualElement;
					MultiColumnController.UnbindCellItem(visualElement2, index, column);
				}
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000220E0 File Offset: 0x000202E0
		public void DestroyItem(VisualElement element)
		{
			foreach (VisualElement visualElement in element.Children())
			{
				Column column = visualElement.GetProperty(MultiColumnController.k_BoundColumnVePropertyName) as Column;
				bool flag = column == null;
				if (!flag)
				{
					VisualElement visualElement2 = visualElement.GetProperty(MultiColumnController.bindableElementPropertyName) as VisualElement;
					Action<VisualElement> destroyCell = column.destroyCell;
					if (destroyCell != null)
					{
						destroyCell(visualElement2);
					}
					visualElement.SetProperty(MultiColumnController.k_BoundColumnVePropertyName, null);
				}
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002217C File Offset: 0x0002037C
		public void PrepareView(BaseVerticalCollectionView collectionView)
		{
			bool flag = this.m_View != null;
			if (flag)
			{
				Debug.LogWarning("Trying to initialize multi column view more than once. This shouldn't happen.");
			}
			else
			{
				this.m_View = collectionView;
				this.m_HeaderContainer = new VisualElement
				{
					name = MultiColumnController.headerContainerUssClassName
				};
				this.m_HeaderContainer.AddToClassList(MultiColumnController.headerContainerUssClassName);
				this.m_HeaderContainer.viewDataKey = MultiColumnController.k_HeaderContainerViewDataKey;
				collectionView.scrollView.hierarchy.Insert(0, this.m_HeaderContainer);
				this.m_HeaderContainer.Add(this.m_MultiColumnHeader);
				this.m_View.scrollView.horizontalScroller.valueChanged += this.OnHorizontalScrollerValueChanged;
				this.m_View.scrollView.contentViewport.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnViewportGeometryChanged), TrickleDown.NoTrickleDown);
				this.m_MultiColumnHeader.columnContainer.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnColumnContainerGeometryChanged), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00022278 File Offset: 0x00020478
		public void Dispose()
		{
			bool flag = this.m_View != null;
			if (flag)
			{
				this.m_View.scrollView.horizontalScroller.valueChanged -= this.OnHorizontalScrollerValueChanged;
				this.m_View.scrollView.contentViewport.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnViewportGeometryChanged), TrickleDown.NoTrickleDown);
				this.m_View = null;
			}
			this.m_MultiColumnHeader.columnContainer.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnColumnContainerGeometryChanged), TrickleDown.NoTrickleDown);
			this.m_MultiColumnHeader.columnSortingChanged -= this.OnColumnSortingChanged;
			this.m_MultiColumnHeader.contextMenuPopulateEvent -= this.OnContextMenuPopulateEvent;
			this.m_MultiColumnHeader.columnResized -= this.OnColumnResized;
			this.m_MultiColumnHeader.viewDataRestored -= this.OnViewDataRestored;
			this.m_MultiColumnHeader.columns.columnAdded -= this.OnColumnAdded;
			this.m_MultiColumnHeader.columns.columnRemoved -= this.OnColumnRemoved;
			this.m_MultiColumnHeader.columns.columnReordered -= this.OnColumnReordered;
			this.m_MultiColumnHeader.columns.columnChanged -= this.OnColumnsChanged;
			this.m_MultiColumnHeader.columns.changed -= this.OnColumnChanged;
			this.m_MultiColumnHeader.RemoveFromHierarchy();
			this.m_MultiColumnHeader.Dispose();
			this.m_MultiColumnHeader = null;
			this.m_HeaderContainer.RemoveFromHierarchy();
			this.m_HeaderContainer = null;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00022422 File Offset: 0x00020622
		private void OnHorizontalScrollerValueChanged(float v)
		{
			this.m_MultiColumnHeader.ScrollHorizontally(v);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00022434 File Offset: 0x00020634
		private void OnViewportGeometryChanged(GeometryChangedEvent evt)
		{
			float num = this.m_MultiColumnHeader.resolvedStyle.paddingLeft + this.m_MultiColumnHeader.resolvedStyle.paddingRight;
			this.m_MultiColumnHeader.style.maxWidth = evt.newRect.width - num;
			this.m_MultiColumnHeader.style.maxWidth = evt.newRect.width - num;
			this.UpdateContentContainer(this.m_View);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000224BD File Offset: 0x000206BD
		private void OnColumnContainerGeometryChanged(GeometryChangedEvent evt)
		{
			this.UpdateContentContainer(this.m_View);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000224D0 File Offset: 0x000206D0
		private void UpdateContentContainer(BaseVerticalCollectionView collectionView)
		{
			float width = this.m_MultiColumnHeader.columnContainer.layout.width;
			float num = Mathf.Max(width, collectionView.scrollView.contentViewport.resolvedStyle.width);
			collectionView.scrollView.contentContainer.style.width = num;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002252F File Offset: 0x0002072F
		private void OnColumnSortingChanged()
		{
			Action action = this.columnSortingChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00022543 File Offset: 0x00020743
		private void OnContextMenuPopulateEvent(ContextualMenuPopulateEvent evt, Column column)
		{
			Action<ContextualMenuPopulateEvent, Column> action = this.headerContextMenuPopulateEvent;
			if (action != null)
			{
				action(evt, column);
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002255C File Offset: 0x0002075C
		private void OnColumnResized(int index, float width)
		{
			foreach (ReusableCollectionItem reusableCollectionItem in this.m_View.activeItems)
			{
				reusableCollectionItem.bindableElement.ElementAt(index).style.width = width;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000225CC File Offset: 0x000207CC
		private void OnColumnAdded(Column column, int index)
		{
			this.m_View.Rebuild();
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000225CC File Offset: 0x000207CC
		private void OnColumnRemoved(Column column)
		{
			this.m_View.Rebuild();
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000225DC File Offset: 0x000207DC
		private void OnColumnReordered(Column column, int from, int to)
		{
			bool isApplyingViewState = this.m_MultiColumnHeader.isApplyingViewState;
			if (!isApplyingViewState)
			{
				this.m_View.Rebuild();
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00022608 File Offset: 0x00020808
		private void OnColumnsChanged(Column column, ColumnDataType type)
		{
			bool isApplyingViewState = this.m_MultiColumnHeader.isApplyingViewState;
			if (!isApplyingViewState)
			{
				bool flag = type == ColumnDataType.Visibility;
				if (flag)
				{
					this.m_View.ScheduleRebuild();
				}
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002263C File Offset: 0x0002083C
		private void OnColumnChanged(ColumnsDataType type)
		{
			bool isApplyingViewState = this.m_MultiColumnHeader.isApplyingViewState;
			if (!isApplyingViewState)
			{
				bool flag = type == ColumnsDataType.PrimaryColumn;
				if (flag)
				{
					this.m_View.ScheduleRebuild();
				}
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000225CC File Offset: 0x000207CC
		private void OnViewDataRestored()
		{
			this.m_View.Rebuild();
		}

		// Token: 0x040003E7 RID: 999
		private static readonly PropertyName k_BoundColumnVePropertyName = "__unity-multi-column-bound-column";

		// Token: 0x040003E8 RID: 1000
		internal static readonly PropertyName bindableElementPropertyName = "__unity-multi-column-bindable-element";

		// Token: 0x040003E9 RID: 1001
		internal static readonly string baseUssClassName = "unity-multi-column-view";

		// Token: 0x040003EA RID: 1002
		private static readonly string k_HeaderContainerViewDataKey = "unity-multi-column-header-container";

		// Token: 0x040003EB RID: 1003
		public static readonly string headerContainerUssClassName = MultiColumnController.baseUssClassName + "__header-container";

		// Token: 0x040003EC RID: 1004
		public static readonly string rowContainerUssClassName = MultiColumnController.baseUssClassName + "__row-container";

		// Token: 0x040003ED RID: 1005
		public static readonly string cellUssClassName = MultiColumnController.baseUssClassName + "__cell";

		// Token: 0x040003EE RID: 1006
		public static readonly string cellLabelUssClassName = MultiColumnController.cellUssClassName + "__label";

		// Token: 0x040003EF RID: 1007
		private static readonly string k_HeaderViewDataKey = "Header";

		// Token: 0x040003F2 RID: 1010
		private BaseVerticalCollectionView m_View;

		// Token: 0x040003F3 RID: 1011
		private VisualElement m_HeaderContainer;

		// Token: 0x040003F4 RID: 1012
		private MultiColumnCollectionHeader m_MultiColumnHeader;
	}
}
