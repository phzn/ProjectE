using System;
using UnityEngine.UIElements.Experimental;

namespace UnityEngine.UIElements
{
	// Token: 0x02000175 RID: 373
	internal class ListViewDraggerAnimated : ListViewDragger
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0002FC8C File Offset: 0x0002DE8C
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0002FC94 File Offset: 0x0002DE94
		public bool isDragging { get; private set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0002FC9D File Offset: 0x0002DE9D
		public ReusableCollectionItem draggedItem
		{
			get
			{
				return this.m_Item;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0000960A File Offset: 0x0000780A
		protected override bool supportsDragEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002FCA5 File Offset: 0x0002DEA5
		public ListViewDraggerAnimated(BaseVerticalCollectionView listView)
			: base(listView)
		{
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002FCB0 File Offset: 0x0002DEB0
		protected internal override StartDragArgs StartDrag(Vector3 pointerPosition)
		{
			base.targetView.ClearSelection();
			ReusableCollectionItem recycledItem = base.GetRecycledItem(pointerPosition);
			bool flag = recycledItem == null;
			StartDragArgs startDragArgs;
			if (flag)
			{
				startDragArgs = new StartDragArgs(string.Empty, DragVisualMode.Rejected);
			}
			else
			{
				base.targetView.SetSelection(recycledItem.index);
				this.isDragging = true;
				this.m_Item = recycledItem;
				base.targetView.virtualizationController.StartDragItem(this.m_Item);
				float y = this.m_Item.rootElement.layout.y;
				this.m_SelectionHeight = this.m_Item.rootElement.layout.height;
				this.m_Item.rootElement.style.position = Position.Absolute;
				this.m_Item.rootElement.style.height = this.m_Item.rootElement.layout.height;
				this.m_Item.rootElement.style.width = this.m_Item.rootElement.layout.width;
				this.m_Item.rootElement.style.top = y;
				this.m_DragStartIndex = this.m_Item.index;
				this.m_CurrentIndex = this.m_DragStartIndex;
				this.m_CurrentPointerPosition = pointerPosition;
				this.m_LocalOffsetOnStart = base.targetScrollView.contentContainer.WorldToLocal(pointerPosition).y - y;
				ReusableCollectionItem recycledItemFromIndex = base.targetView.GetRecycledItemFromIndex(this.m_CurrentIndex + 1);
				bool flag2 = recycledItemFromIndex != null;
				if (flag2)
				{
					this.m_OffsetItem = recycledItemFromIndex;
					this.Animate(this.m_OffsetItem, this.m_SelectionHeight);
					this.m_OffsetItem.rootElement.style.paddingTop = this.m_SelectionHeight;
					bool flag3 = base.targetView.virtualizationMethod == CollectionVirtualizationMethod.FixedHeight;
					if (flag3)
					{
						this.m_OffsetItem.rootElement.style.height = base.targetView.fixedItemHeight + this.m_SelectionHeight;
					}
				}
				startDragArgs = base.dragAndDropController.SetupDragAndDrop(new int[] { this.m_Item.index }, true);
			}
			return startDragArgs;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002FF08 File Offset: 0x0002E108
		protected internal override void UpdateDrag(Vector3 pointerPosition)
		{
			bool flag = this.m_Item == null;
			if (!flag)
			{
				base.HandleDragAndScroll(pointerPosition);
				this.m_CurrentPointerPosition = pointerPosition;
				Vector2 vector = base.targetScrollView.contentContainer.WorldToLocal(this.m_CurrentPointerPosition);
				Rect layout = this.m_Item.rootElement.layout;
				float height = base.targetScrollView.contentContainer.layout.height;
				layout.y = Mathf.Clamp(vector.y - this.m_LocalOffsetOnStart, 0f, height - this.m_SelectionHeight);
				float num = base.targetScrollView.contentContainer.resolvedStyle.paddingTop;
				this.m_CurrentIndex = -1;
				foreach (ReusableCollectionItem reusableCollectionItem in base.targetView.activeItems)
				{
					bool flag2 = reusableCollectionItem.index < 0 || (reusableCollectionItem.rootElement.style.display == DisplayStyle.None && !reusableCollectionItem.isDragGhost);
					if (!flag2)
					{
						bool flag3 = reusableCollectionItem.index == this.m_Item.index && reusableCollectionItem.index < base.targetView.itemsSource.Count - 1;
						if (flag3)
						{
							float expectedItemHeight = base.targetView.virtualizationController.GetExpectedItemHeight(reusableCollectionItem.index + 1);
							bool flag4 = layout.y <= num + expectedItemHeight * 0.5f;
							if (flag4)
							{
								this.m_CurrentIndex = reusableCollectionItem.index;
							}
						}
						else
						{
							float expectedItemHeight2 = base.targetView.virtualizationController.GetExpectedItemHeight(reusableCollectionItem.index);
							bool flag5 = layout.y <= num + expectedItemHeight2 * 0.5f;
							if (flag5)
							{
								bool flag6 = this.m_CurrentIndex == -1;
								if (flag6)
								{
									this.m_CurrentIndex = reusableCollectionItem.index;
								}
								bool flag7 = this.m_OffsetItem == reusableCollectionItem;
								if (flag7)
								{
									break;
								}
								this.Animate(this.m_OffsetItem, 0f);
								this.Animate(reusableCollectionItem, this.m_SelectionHeight);
								this.m_OffsetItem = reusableCollectionItem;
								break;
							}
							else
							{
								num += expectedItemHeight2;
							}
						}
					}
				}
				bool flag8 = this.m_CurrentIndex == -1;
				if (flag8)
				{
					this.m_CurrentIndex = base.targetView.itemsSource.Count;
					this.Animate(this.m_OffsetItem, 0f);
					this.m_OffsetItem = null;
				}
				this.m_Item.rootElement.layout = layout;
				this.m_Item.rootElement.BringToFront();
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000301E4 File Offset: 0x0002E3E4
		private void Animate(ReusableCollectionItem element, float paddingTop)
		{
			bool flag = element == null;
			if (!flag)
			{
				bool flag2 = element.animator != null;
				if (flag2)
				{
					bool flag3 = (element.animator.isRunning && element.animator.to.paddingTop == paddingTop) || (!element.animator.isRunning && element.rootElement.style.paddingTop == paddingTop);
					if (flag3)
					{
						return;
					}
				}
				ValueAnimation<StyleValues> animator = element.animator;
				if (animator != null)
				{
					animator.Stop();
				}
				ValueAnimation<StyleValues> animator2 = element.animator;
				if (animator2 != null)
				{
					animator2.Recycle();
				}
				StyleValues styleValues = ((base.targetView.virtualizationMethod == CollectionVirtualizationMethod.FixedHeight) ? new StyleValues
				{
					paddingTop = paddingTop,
					height = base.targetView.ResolveItemHeight(-1f) + paddingTop
				} : new StyleValues
				{
					paddingTop = paddingTop
				});
				element.animator = element.rootElement.experimental.animation.Start(styleValues, 500);
				element.animator.KeepAlive();
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0003030C File Offset: 0x0002E50C
		protected internal override void OnDrop(Vector3 pointerPosition)
		{
			bool flag = this.m_Item == null;
			if (!flag)
			{
				this.isDragging = false;
				this.m_Item.rootElement.ClearManualLayout();
				base.targetView.virtualizationController.EndDrag(this.m_CurrentIndex);
				bool flag2 = this.m_OffsetItem != null;
				if (flag2)
				{
					ValueAnimation<StyleValues> animator = this.m_OffsetItem.animator;
					if (animator != null)
					{
						animator.Stop();
					}
					ValueAnimation<StyleValues> animator2 = this.m_OffsetItem.animator;
					if (animator2 != null)
					{
						animator2.Recycle();
					}
					this.m_OffsetItem.animator = null;
					this.m_OffsetItem.rootElement.style.paddingTop = 0f;
					bool flag3 = base.targetView.virtualizationMethod == CollectionVirtualizationMethod.FixedHeight;
					if (flag3)
					{
						this.m_OffsetItem.rootElement.style.height = base.targetView.ResolveItemHeight(-1f);
					}
				}
				ListViewDragger.DragPosition dragPosition = new ListViewDragger.DragPosition
				{
					recycledItem = this.m_Item,
					insertAtIndex = this.m_CurrentIndex,
					dropPosition = DragAndDropPosition.BetweenItems
				};
				DragAndDropArgs dragAndDropArgs = base.MakeDragAndDropArgs(dragPosition);
				base.dragAndDropController.OnDrop(dragAndDropArgs);
				base.dragAndDrop.AcceptDrag();
				this.m_Item = null;
				this.m_OffsetItem = null;
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected override void ClearDragAndDropUI(bool dragCancelled)
		{
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0003046C File Offset: 0x0002E66C
		protected override bool TryGetDragPosition(Vector2 pointerPosition, ref ListViewDragger.DragPosition dragPosition)
		{
			dragPosition.recycledItem = this.m_Item;
			dragPosition.insertAtIndex = this.m_CurrentIndex;
			dragPosition.dropPosition = DragAndDropPosition.BetweenItems;
			return true;
		}

		// Token: 0x040005AB RID: 1451
		private int m_DragStartIndex;

		// Token: 0x040005AC RID: 1452
		private int m_CurrentIndex;

		// Token: 0x040005AD RID: 1453
		private float m_SelectionHeight;

		// Token: 0x040005AE RID: 1454
		private float m_LocalOffsetOnStart;

		// Token: 0x040005AF RID: 1455
		private Vector3 m_CurrentPointerPosition;

		// Token: 0x040005B0 RID: 1456
		private ReusableCollectionItem m_Item;

		// Token: 0x040005B1 RID: 1457
		private ReusableCollectionItem m_OffsetItem;
	}
}
