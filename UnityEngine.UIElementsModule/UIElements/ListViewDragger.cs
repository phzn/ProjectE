using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x02000172 RID: 370
	internal class ListViewDragger : DragEventsProcessor
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x0002E809 File Offset: 0x0002CA09
		protected BaseVerticalCollectionView targetView
		{
			get
			{
				return this.m_Target as BaseVerticalCollectionView;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0002E816 File Offset: 0x0002CA16
		protected ScrollView targetScrollView
		{
			get
			{
				return this.targetView.scrollView;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0002E823 File Offset: 0x0002CA23
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0002E82B File Offset: 0x0002CA2B
		public ICollectionDragAndDropController dragAndDropController { get; set; }

		// Token: 0x06000BED RID: 3053 RVA: 0x0002E834 File Offset: 0x0002CA34
		public ListViewDragger(BaseVerticalCollectionView listView)
			: base(listView)
		{
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002E858 File Offset: 0x0002CA58
		protected override bool CanStartDrag(Vector3 pointerPosition)
		{
			bool flag = this.dragAndDropController == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = !this.targetScrollView.contentContainer.worldBound.Contains(pointerPosition);
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					ReusableCollectionItem recycledItem = this.GetRecycledItem(pointerPosition);
					bool flag4 = recycledItem != null && this.targetView.HasCanStartDrag();
					if (flag4)
					{
						IEnumerable<int> enumerable2;
						if (!this.targetView.selectedIds.Any<int>())
						{
							IEnumerable<int> enumerable = new int[] { recycledItem.id };
							enumerable2 = enumerable;
						}
						else
						{
							enumerable2 = this.targetView.selectedIds;
						}
						IEnumerable<int> enumerable3 = enumerable2;
						flag2 = this.targetView.RaiseCanStartDrag(recycledItem, enumerable3);
					}
					else
					{
						bool flag5 = this.targetView.selectedIds.Any<int>();
						if (flag5)
						{
							flag2 = this.dragAndDropController.CanStartDrag(this.targetView.selectedIds);
						}
						else
						{
							flag2 = recycledItem != null && this.dragAndDropController.CanStartDrag(new int[] { recycledItem.id });
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0002E95C File Offset: 0x0002CB5C
		protected internal override StartDragArgs StartDrag(Vector3 pointerPosition)
		{
			ReusableCollectionItem recycledItem = this.GetRecycledItem(pointerPosition);
			bool flag = recycledItem != null;
			IEnumerable<int> enumerable;
			if (flag)
			{
				bool flag2 = !this.targetView.selectedIndices.Contains(recycledItem.index);
				if (flag2)
				{
					this.targetView.SetSelection(recycledItem.index);
				}
				enumerable = this.targetView.selectedIds;
			}
			else
			{
				enumerable = (this.targetView.selectedIds.Any<int>() ? this.targetView.selectedIds : Enumerable.Empty<int>());
			}
			StartDragArgs startDragArgs = this.dragAndDropController.SetupDragAndDrop(enumerable, false);
			startDragArgs = this.targetView.RaiseSetupDragAndDrop(recycledItem, this.dragAndDropController.GetSortedSelectedIds(), startDragArgs);
			startDragArgs.SetGenericData("__unity-drag-and-drop__source-view", this.targetView);
			return startDragArgs;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002EA28 File Offset: 0x0002CC28
		protected internal override void UpdateDrag(Vector3 pointerPosition)
		{
			ListViewDragger.DragPosition dragPosition = default(ListViewDragger.DragPosition);
			DragVisualMode visualMode = this.GetVisualMode(pointerPosition, ref dragPosition);
			bool flag = visualMode == DragVisualMode.Rejected;
			if (flag)
			{
				this.ClearDragAndDropUI(false);
			}
			else
			{
				this.HandleDragAndScroll(pointerPosition);
				this.HandleAutoExpansion(pointerPosition);
				this.ApplyDragAndDropUI(dragPosition);
			}
			base.dragAndDrop.SetVisualMode(visualMode);
			base.dragAndDrop.UpdateDrag(pointerPosition);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002EA9C File Offset: 0x0002CC9C
		private DragVisualMode GetVisualMode(Vector3 pointerPosition, ref ListViewDragger.DragPosition dragPosition)
		{
			bool flag = this.dragAndDropController == null;
			DragVisualMode dragVisualMode;
			if (flag)
			{
				dragVisualMode = DragVisualMode.Rejected;
			}
			else
			{
				bool flag2 = this.TryGetDragPosition(pointerPosition, ref dragPosition);
				DragAndDropArgs dragAndDropArgs = this.MakeDragAndDropArgs(dragPosition);
				DragVisualMode dragVisualMode2 = this.targetView.RaiseHandleDragAndDrop(pointerPosition, dragAndDropArgs);
				bool flag3 = dragVisualMode2 > DragVisualMode.None;
				if (flag3)
				{
					dragVisualMode = dragVisualMode2;
				}
				else
				{
					dragVisualMode = (flag2 ? this.dragAndDropController.HandleDragAndDrop(dragAndDropArgs) : DragVisualMode.Rejected);
				}
			}
			return dragVisualMode;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002EB18 File Offset: 0x0002CD18
		protected internal override void OnDrop(Vector3 pointerPosition)
		{
			ListViewDragger.DragPosition dragPosition = default(ListViewDragger.DragPosition);
			bool flag = !this.TryGetDragPosition(pointerPosition, ref dragPosition);
			if (!flag)
			{
				DragAndDropArgs dragAndDropArgs = this.MakeDragAndDropArgs(dragPosition);
				DragVisualMode dragVisualMode = this.targetView.RaiseDrop(pointerPosition, dragAndDropArgs);
				bool flag2 = dragVisualMode > DragVisualMode.None;
				if (flag2)
				{
					bool flag3 = dragVisualMode != DragVisualMode.Rejected;
					if (flag3)
					{
						base.dragAndDrop.AcceptDrag();
					}
					else
					{
						this.dragAndDropController.DragCleanup();
					}
				}
				else
				{
					bool flag4 = this.dragAndDropController.HandleDragAndDrop(dragAndDropArgs) != DragVisualMode.Rejected;
					if (flag4)
					{
						this.dragAndDropController.OnDrop(dragAndDropArgs);
						base.dragAndDrop.AcceptDrag();
					}
					else
					{
						this.dragAndDropController.DragCleanup();
					}
				}
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
		internal void HandleDragAndScroll(Vector2 pointerPosition)
		{
			bool flag = pointerPosition.y < this.targetScrollView.worldBound.yMin + 5f;
			bool flag2 = pointerPosition.y > this.targetScrollView.worldBound.yMax - 5f;
			bool flag3 = flag || flag2;
			if (flag3)
			{
				Vector2 vector = this.targetScrollView.scrollOffset + (flag ? Vector2.down : Vector2.up) * 20f;
				vector.y = Mathf.Clamp(vector.y, 0f, Mathf.Max(0f, this.targetScrollView.contentContainer.worldBound.height - this.targetScrollView.contentViewport.worldBound.height));
				this.targetScrollView.scrollOffset = vector;
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002ECD0 File Offset: 0x0002CED0
		private void HandleAutoExpansion(Vector2 pointerPosition)
		{
			ReusableCollectionItem recycledItem = this.GetRecycledItem(pointerPosition);
			bool flag = recycledItem == null;
			if (!flag)
			{
				this.dragAndDropController.HandleAutoExpand(recycledItem, pointerPosition);
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002ED04 File Offset: 0x0002CF04
		private void ApplyDragAndDropUI(ListViewDragger.DragPosition dragPosition)
		{
			bool flag = this.m_LastDragPosition.Equals(dragPosition);
			if (!flag)
			{
				bool flag2 = this.m_DragHoverBar == null;
				if (flag2)
				{
					this.m_DragHoverBar = new VisualElement();
					this.m_DragHoverBar.AddToClassList(BaseVerticalCollectionView.dragHoverBarUssClassName);
					this.m_DragHoverBar.style.width = this.targetView.localBound.width;
					this.m_DragHoverBar.style.visibility = Visibility.Hidden;
					this.m_DragHoverBar.pickingMode = PickingMode.Ignore;
					this.targetView.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.<ApplyDragAndDropUI>g__GeometryChangedCallback|27_0), TrickleDown.NoTrickleDown);
					this.targetScrollView.contentViewport.Add(this.m_DragHoverBar);
				}
				bool flag3 = this.m_DragHoverItemMarker == null && this.targetView is BaseTreeView;
				if (flag3)
				{
					this.m_DragHoverItemMarker = new VisualElement();
					this.m_DragHoverItemMarker.AddToClassList(BaseVerticalCollectionView.dragHoverMarkerUssClassName);
					this.m_DragHoverItemMarker.style.visibility = Visibility.Hidden;
					this.m_DragHoverItemMarker.pickingMode = PickingMode.Ignore;
					this.m_DragHoverBar.Add(this.m_DragHoverItemMarker);
					this.m_DragHoverSiblingMarker = new VisualElement();
					this.m_DragHoverSiblingMarker.AddToClassList(BaseVerticalCollectionView.dragHoverMarkerUssClassName);
					this.m_DragHoverSiblingMarker.style.visibility = Visibility.Hidden;
					this.m_DragHoverSiblingMarker.pickingMode = PickingMode.Ignore;
					this.targetScrollView.contentViewport.Add(this.m_DragHoverSiblingMarker);
				}
				this.ClearDragAndDropUI(false);
				this.m_LastDragPosition = dragPosition;
				switch (dragPosition.dropPosition)
				{
				case DragAndDropPosition.OverItem:
					dragPosition.recycledItem.rootElement.AddToClassList(BaseVerticalCollectionView.itemDragHoverUssClassName);
					break;
				case DragAndDropPosition.BetweenItems:
				{
					bool flag4 = dragPosition.insertAtIndex == 0;
					if (flag4)
					{
						this.PlaceHoverBarAt(0f, -1f, -1f);
					}
					else
					{
						ReusableCollectionItem recycledItemFromIndex = this.targetView.GetRecycledItemFromIndex(dragPosition.insertAtIndex - 1);
						ReusableCollectionItem recycledItemFromIndex2 = this.targetView.GetRecycledItemFromIndex(dragPosition.insertAtIndex);
						this.PlaceHoverBarAtElement(recycledItemFromIndex ?? recycledItemFromIndex2);
					}
					break;
				}
				case DragAndDropPosition.OutsideItems:
				{
					ReusableCollectionItem recycledItemFromIndex3 = this.targetView.GetRecycledItemFromIndex(this.targetView.itemsSource.Count - 1);
					bool flag5 = recycledItemFromIndex3 != null;
					if (flag5)
					{
						this.PlaceHoverBarAtElement(recycledItemFromIndex3);
					}
					else
					{
						this.PlaceHoverBarAt(0f, -1f, -1f);
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException("dropPosition", dragPosition.dropPosition, "Unsupported dropPosition value");
				}
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002EFB8 File Offset: 0x0002D1B8
		protected virtual bool TryGetDragPosition(Vector2 pointerPosition, ref ListViewDragger.DragPosition dragPosition)
		{
			ReusableCollectionItem recycledItem = this.GetRecycledItem(pointerPosition);
			bool flag = recycledItem == null;
			bool flag3;
			if (flag)
			{
				bool flag2 = !this.targetView.worldBound.Contains(pointerPosition);
				if (flag2)
				{
					flag3 = false;
				}
				else
				{
					dragPosition.dropPosition = DragAndDropPosition.OutsideItems;
					bool flag4 = pointerPosition.y >= this.targetScrollView.contentContainer.worldBound.yMax;
					if (flag4)
					{
						dragPosition.insertAtIndex = this.targetView.itemsSource.Count;
					}
					else
					{
						dragPosition.insertAtIndex = 0;
					}
					this.HandleTreePosition(pointerPosition, ref dragPosition);
					flag3 = true;
				}
			}
			else
			{
				bool flag5 = recycledItem.rootElement.worldBound.yMax - pointerPosition.y < 5f;
				if (flag5)
				{
					dragPosition.insertAtIndex = recycledItem.index + 1;
					dragPosition.dropPosition = DragAndDropPosition.BetweenItems;
				}
				else
				{
					bool flag6 = pointerPosition.y - recycledItem.rootElement.worldBound.yMin > 5f;
					if (flag6)
					{
						Vector2 scrollOffset = this.targetScrollView.scrollOffset;
						this.targetView.ScrollToItem(recycledItem.index);
						bool flag7 = !Mathf.Approximately(scrollOffset.x, this.targetScrollView.scrollOffset.x) || !Mathf.Approximately(scrollOffset.y, this.targetScrollView.scrollOffset.y);
						if (flag7)
						{
							return this.TryGetDragPosition(pointerPosition, ref dragPosition);
						}
						dragPosition.recycledItem = recycledItem;
						dragPosition.insertAtIndex = recycledItem.index;
						dragPosition.dropPosition = DragAndDropPosition.OverItem;
					}
					else
					{
						dragPosition.insertAtIndex = recycledItem.index;
						dragPosition.dropPosition = DragAndDropPosition.BetweenItems;
					}
				}
				this.HandleTreePosition(pointerPosition, ref dragPosition);
				flag3 = true;
			}
			return flag3;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002F184 File Offset: 0x0002D384
		private void HandleTreePosition(Vector2 pointerPosition, ref ListViewDragger.DragPosition dragPosition)
		{
			dragPosition.parentId = -1;
			dragPosition.childIndex = -1;
			this.m_LeftIndentation = -1f;
			this.m_SiblingBottom = -1f;
			BaseTreeView baseTreeView = this.targetView as BaseTreeView;
			bool flag = baseTreeView == null;
			if (!flag)
			{
				bool flag2 = dragPosition.insertAtIndex < 0;
				if (!flag2)
				{
					BaseTreeViewController viewController = baseTreeView.viewController;
					bool flag3 = dragPosition.dropPosition == DragAndDropPosition.OverItem;
					if (flag3)
					{
						dragPosition.parentId = viewController.GetIdForIndex(dragPosition.insertAtIndex);
						dragPosition.childIndex = -1;
					}
					else
					{
						bool flag4 = dragPosition.insertAtIndex <= 0;
						if (flag4)
						{
							dragPosition.childIndex = 0;
						}
						else
						{
							this.HandleSiblingInsertionAtAvailableDepthsAndChangeTargetIfNeeded(ref dragPosition, pointerPosition);
						}
					}
				}
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002F238 File Offset: 0x0002D438
		private void HandleSiblingInsertionAtAvailableDepthsAndChangeTargetIfNeeded(ref ListViewDragger.DragPosition dragPosition, Vector2 pointerPosition)
		{
			BaseTreeView baseTreeView = this.targetView as BaseTreeView;
			bool flag = baseTreeView == null;
			if (!flag)
			{
				BaseTreeViewController viewController = baseTreeView.viewController;
				int insertAtIndex = dragPosition.insertAtIndex;
				int idForIndex = viewController.GetIdForIndex(insertAtIndex);
				int num;
				int num2;
				this.GetPreviousAndNextItemsIgnoringDraggedItems(dragPosition.insertAtIndex, out num, out num2);
				bool flag2 = num == -1;
				if (!flag2)
				{
					bool flag3 = viewController.HasChildren(num) && baseTreeView.IsExpanded(num);
					int indentationDepth = viewController.GetIndentationDepth(num);
					int indentationDepth2 = viewController.GetIndentationDepth(num2);
					int num3 = ((num2 != -1) ? indentationDepth2 : 0);
					int num4 = viewController.GetIndentationDepth(num) + (flag3 ? 1 : 0);
					int num5 = num;
					float num6 = 15f;
					float num7 = 15f;
					VisualElement rootElementForId = baseTreeView.GetRootElementForId(num);
					bool flag4 = indentationDepth > 0 && rootElementForId != null;
					if (flag4)
					{
						VisualElement visualElement = rootElementForId.Q(BaseTreeView.itemIndentUssClassName, null);
						VisualElement visualElement2 = rootElementForId.Q(BaseTreeView.itemToggleUssClassName, null);
						num6 = visualElement2.layout.width;
						num7 = visualElement.layout.width / (float)indentationDepth;
					}
					else
					{
						int indentationDepth3 = baseTreeView.viewController.GetIndentationDepth(idForIndex);
						bool flag5 = indentationDepth3 > 0;
						if (flag5)
						{
							VisualElement rootElementForId2 = baseTreeView.GetRootElementForId(idForIndex);
							VisualElement visualElement3 = rootElementForId2.Q(BaseTreeView.itemIndentUssClassName, null);
							VisualElement visualElement4 = rootElementForId2.Q(BaseTreeView.itemToggleUssClassName, null);
							num6 = visualElement4.layout.width;
							num7 = visualElement3.layout.width / (float)indentationDepth3;
						}
					}
					bool flag6 = num4 <= num3;
					if (flag6)
					{
						this.m_LeftIndentation = num6 + num7 * (float)num3;
						bool flag7 = flag3;
						if (flag7)
						{
							dragPosition.parentId = num;
							dragPosition.childIndex = 0;
						}
						else
						{
							dragPosition.parentId = viewController.GetParentId(num);
							bool flag8 = viewController.GetParentId(num2) == viewController.GetIdForIndex(dragPosition.insertAtIndex);
							if (flag8)
							{
								dragPosition.childIndex = viewController.GetChildIndexForId(num) + 1;
							}
							else
							{
								dragPosition.childIndex = viewController.GetChildIndexForId(num2);
							}
						}
					}
					else
					{
						Vector2 vector = baseTreeView.scrollView.contentContainer.WorldToLocal(pointerPosition);
						int num8 = Mathf.FloorToInt((vector.x - num6) / num7);
						bool flag9 = num8 >= num4;
						if (flag9)
						{
							this.m_LeftIndentation = num6 + num7 * (float)num4;
							bool flag10 = flag3;
							if (flag10)
							{
								dragPosition.parentId = num;
								dragPosition.childIndex = 0;
							}
							else
							{
								dragPosition.parentId = viewController.GetParentId(num);
								dragPosition.childIndex = viewController.GetChildIndexForId(num) + 1;
							}
						}
						else
						{
							int i;
							for (i = viewController.GetIndentationDepth(num5); i > num3; i--)
							{
								bool flag11 = i == num8;
								if (flag11)
								{
									break;
								}
								num5 = viewController.GetParentId(num5);
							}
							bool flag12 = num5 != idForIndex;
							bool flag13 = flag12;
							if (flag13)
							{
								VisualElement rootElementForId3 = baseTreeView.GetRootElementForId(num5);
								bool flag14 = rootElementForId3 != null;
								if (flag14)
								{
									VisualElement contentViewport = this.targetScrollView.contentViewport;
									Rect rect = contentViewport.WorldToLocal(rootElementForId3.worldBound);
									bool flag15 = contentViewport.localBound.yMin < rect.yMax && rect.yMax < contentViewport.localBound.yMax;
									if (flag15)
									{
										this.m_SiblingBottom = rect.yMax;
									}
								}
							}
							dragPosition.parentId = viewController.GetParentId(num5);
							dragPosition.childIndex = viewController.GetChildIndexForId(num5) + 1;
							this.m_LeftIndentation = num6 + num7 * (float)i;
						}
					}
				}
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002F5DC File Offset: 0x0002D7DC
		private void GetPreviousAndNextItemsIgnoringDraggedItems(int insertAtIndex, out int previousItemId, out int nextItemId)
		{
			previousItemId = (nextItemId = -1);
			int i = insertAtIndex - 1;
			int j = insertAtIndex;
			while (i >= 0)
			{
				int idForIndex = this.targetView.viewController.GetIdForIndex(i);
				bool flag = !this.dragAndDropController.GetSortedSelectedIds().Contains(idForIndex);
				if (flag)
				{
					previousItemId = idForIndex;
					break;
				}
				i--;
			}
			while (j < this.targetView.itemsSource.Count)
			{
				int idForIndex2 = this.targetView.viewController.GetIdForIndex(j);
				bool flag2 = !this.dragAndDropController.GetSortedSelectedIds().Contains(idForIndex2);
				if (flag2)
				{
					nextItemId = idForIndex2;
					break;
				}
				j++;
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002F698 File Offset: 0x0002D898
		protected DragAndDropArgs MakeDragAndDropArgs(ListViewDragger.DragPosition dragPosition)
		{
			object obj = null;
			ReusableCollectionItem recycledItem = dragPosition.recycledItem;
			bool flag = recycledItem != null;
			if (flag)
			{
				obj = this.targetView.viewController.GetItemForIndex(recycledItem.index);
			}
			return new DragAndDropArgs
			{
				target = obj,
				insertAtIndex = dragPosition.insertAtIndex,
				parentId = dragPosition.parentId,
				childIndex = dragPosition.childIndex,
				dragAndDropPosition = dragPosition.dropPosition,
				dragAndDropData = DragAndDropUtility.GetDragAndDrop(this.m_Target.panel).data
			};
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0002F73C File Offset: 0x0002D93C
		private float GetHoverBarTopPosition(ReusableCollectionItem item)
		{
			VisualElement contentViewport = this.targetScrollView.contentViewport;
			return Mathf.Min(contentViewport.WorldToLocal(item.rootElement.worldBound).yMax, contentViewport.localBound.yMax - 2f);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002F790 File Offset: 0x0002D990
		private void PlaceHoverBarAtElement(ReusableCollectionItem item)
		{
			this.PlaceHoverBarAt(this.GetHoverBarTopPosition(item), this.m_LeftIndentation, this.m_SiblingBottom);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002F7B0 File Offset: 0x0002D9B0
		private void PlaceHoverBarAt(float top, float indentationPadding = -1f, float siblingBottom = -1f)
		{
			this.m_DragHoverBar.style.top = top;
			this.m_DragHoverBar.style.visibility = Visibility.Visible;
			bool flag = this.m_DragHoverItemMarker != null;
			if (flag)
			{
				this.m_DragHoverItemMarker.style.visibility = Visibility.Visible;
			}
			bool flag2 = indentationPadding >= 0f;
			if (flag2)
			{
				this.m_DragHoverBar.style.marginLeft = indentationPadding;
				this.m_DragHoverBar.style.width = this.targetView.localBound.width - indentationPadding;
				bool flag3 = siblingBottom > 0f && this.m_DragHoverSiblingMarker != null;
				if (flag3)
				{
					this.m_DragHoverSiblingMarker.style.top = siblingBottom;
					this.m_DragHoverSiblingMarker.style.visibility = Visibility.Visible;
					this.m_DragHoverSiblingMarker.style.marginLeft = indentationPadding;
				}
			}
			else
			{
				this.m_DragHoverBar.style.marginLeft = 0f;
				this.m_DragHoverBar.style.width = this.targetView.localBound.width;
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002F910 File Offset: 0x0002DB10
		protected override void ClearDragAndDropUI(bool dragCancelled)
		{
			if (dragCancelled)
			{
				this.dragAndDropController.DragCleanup();
			}
			this.targetView.elementPanel.cursorManager.ResetCursor();
			this.m_LastDragPosition = default(ListViewDragger.DragPosition);
			foreach (ReusableCollectionItem reusableCollectionItem in this.targetView.activeItems)
			{
				reusableCollectionItem.rootElement.RemoveFromClassList(BaseVerticalCollectionView.itemDragHoverUssClassName);
			}
			bool flag = this.m_DragHoverBar != null;
			if (flag)
			{
				this.m_DragHoverBar.style.visibility = Visibility.Hidden;
			}
			bool flag2 = this.m_DragHoverItemMarker != null;
			if (flag2)
			{
				this.m_DragHoverItemMarker.style.visibility = Visibility.Hidden;
			}
			bool flag3 = this.m_DragHoverSiblingMarker != null;
			if (flag3)
			{
				this.m_DragHoverSiblingMarker.style.visibility = Visibility.Hidden;
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002FA18 File Offset: 0x0002DC18
		protected ReusableCollectionItem GetRecycledItem(Vector3 pointerPosition)
		{
			foreach (ReusableCollectionItem reusableCollectionItem in this.targetView.activeItems)
			{
				bool flag = reusableCollectionItem.rootElement.worldBound.Contains(pointerPosition);
				if (flag)
				{
					return reusableCollectionItem;
				}
			}
			return null;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0002FA8C File Offset: 0x0002DC8C
		[CompilerGenerated]
		private void <ApplyDragAndDropUI>g__GeometryChangedCallback|27_0(GeometryChangedEvent e)
		{
			this.m_DragHoverBar.style.width = this.targetView.localBound.width;
		}

		// Token: 0x0400059B RID: 1435
		private ListViewDragger.DragPosition m_LastDragPosition;

		// Token: 0x0400059C RID: 1436
		private VisualElement m_DragHoverBar;

		// Token: 0x0400059D RID: 1437
		private VisualElement m_DragHoverItemMarker;

		// Token: 0x0400059E RID: 1438
		private VisualElement m_DragHoverSiblingMarker;

		// Token: 0x0400059F RID: 1439
		private float m_LeftIndentation = -1f;

		// Token: 0x040005A0 RID: 1440
		private float m_SiblingBottom = -1f;

		// Token: 0x040005A1 RID: 1441
		private const int k_AutoScrollAreaSize = 5;

		// Token: 0x040005A2 RID: 1442
		private const int k_BetweenElementsAreaSize = 5;

		// Token: 0x040005A3 RID: 1443
		private const int k_PanSpeed = 20;

		// Token: 0x040005A4 RID: 1444
		private const int k_DragHoverBarHeight = 2;

		// Token: 0x02000173 RID: 371
		internal struct DragPosition : IEquatable<ListViewDragger.DragPosition>
		{
			// Token: 0x06000C01 RID: 3073 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
			public bool Equals(ListViewDragger.DragPosition other)
			{
				return this.insertAtIndex == other.insertAtIndex && this.parentId == other.parentId && this.childIndex == other.childIndex && object.Equals(this.recycledItem, other.recycledItem) && this.dropPosition == other.dropPosition;
			}

			// Token: 0x06000C02 RID: 3074 RVA: 0x0002FB24 File Offset: 0x0002DD24
			public override bool Equals(object obj)
			{
				bool flag;
				if (obj is ListViewDragger.DragPosition)
				{
					ListViewDragger.DragPosition dragPosition = (ListViewDragger.DragPosition)obj;
					flag = this.Equals(dragPosition);
				}
				else
				{
					flag = false;
				}
				return flag;
			}

			// Token: 0x06000C03 RID: 3075 RVA: 0x0002FB50 File Offset: 0x0002DD50
			public override int GetHashCode()
			{
				int num = this.insertAtIndex;
				num = (num * 397) ^ this.parentId;
				num = (num * 397) ^ this.childIndex;
				int num2 = num * 397;
				ReusableCollectionItem reusableCollectionItem = this.recycledItem;
				num = num2 ^ ((reusableCollectionItem != null) ? reusableCollectionItem.GetHashCode() : 0);
				return (num * 397) ^ (int)this.dropPosition;
			}

			// Token: 0x040005A6 RID: 1446
			public int insertAtIndex;

			// Token: 0x040005A7 RID: 1447
			public int parentId;

			// Token: 0x040005A8 RID: 1448
			public int childIndex;

			// Token: 0x040005A9 RID: 1449
			public ReusableCollectionItem recycledItem;

			// Token: 0x040005AA RID: 1450
			public DragAndDropPosition dropPosition;
		}
	}
}
