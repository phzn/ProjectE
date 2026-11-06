using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x02000177 RID: 375
	internal class TreeViewReorderableDragAndDropController : BaseReorderableDragAndDropController
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x00030622 File Offset: 0x0002E822
		public TreeViewReorderableDragAndDropController(BaseTreeView view)
			: base(view)
		{
			this.m_TreeView = view;
			this.m_ExpandDropItemCallback = new Action(this.ExpandDropItem);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00030654 File Offset: 0x0002E854
		protected override int CompareId(int id1, int id2)
		{
			bool flag = id1 == id2;
			int num;
			if (flag)
			{
				num = id1.CompareTo(id2);
			}
			else
			{
				int num2 = id1;
				int num3 = id2;
				List<int> list;
				using (CollectionPool<List<int>, int>.Get(out list))
				{
					while (num2 != -1)
					{
						list.Add(num2);
						num2 = this.m_TreeView.viewController.GetParentId(num2);
					}
					List<int> list2;
					using (CollectionPool<List<int>, int>.Get(out list2))
					{
						while (num3 != -1)
						{
							list2.Add(num3);
							num3 = this.m_TreeView.viewController.GetParentId(num3);
						}
						list.Add(-1);
						list2.Add(-1);
						int i = 0;
						while (i < list.Count)
						{
							int num4 = list[i];
							int num5 = list2.IndexOf(num4);
							bool flag2 = num5 >= 0;
							if (flag2)
							{
								bool flag3 = i == 0;
								if (flag3)
								{
									return -1;
								}
								int num6 = ((i > 0) ? list[i - 1] : id1);
								int num7 = ((num5 > 0) ? list2[num5 - 1] : id2);
								int childIndexForId = this.m_TreeView.viewController.GetChildIndexForId(num6);
								int childIndexForId2 = this.m_TreeView.viewController.GetChildIndexForId(num7);
								return childIndexForId.CompareTo(childIndexForId2);
							}
							else
							{
								i++;
							}
						}
						throw new ArgumentOutOfRangeException("[UI Toolkit] Trying to reorder ids that are not in the same tree.");
					}
				}
			}
			return num;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00030810 File Offset: 0x0002EA10
		public override StartDragArgs SetupDragAndDrop(IEnumerable<int> itemIds, bool skipText = false)
		{
			StartDragArgs startDragArgs = base.SetupDragAndDrop(itemIds, skipText);
			this.m_DropData.draggedIds = base.GetSortedSelectedIds().ToArray<int>();
			return startDragArgs;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00030844 File Offset: 0x0002EA44
		public override DragVisualMode HandleDragAndDrop(IListDragAndDropArgs args)
		{
			bool flag = !this.enableReordering;
			DragVisualMode dragVisualMode;
			if (flag)
			{
				dragVisualMode = DragVisualMode.Rejected;
			}
			else
			{
				dragVisualMode = ((args.dragAndDropData.source == this.m_TreeView) ? DragVisualMode.Move : DragVisualMode.Rejected);
			}
			return dragVisualMode;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00030880 File Offset: 0x0002EA80
		public override void OnDrop(IListDragAndDropArgs args)
		{
			int parentId = args.parentId;
			int childIndex = args.childIndex;
			int num = 0;
			bool flag = args.dragAndDropPosition == DragAndDropPosition.OverItem || (parentId == -1 && childIndex == -1);
			List<ValueTuple<int, int>> list;
			using (CollectionPool<List<ValueTuple<int, int>>, ValueTuple<int, int>>.Get(out list))
			{
				foreach (int num2 in this.m_DropData.draggedIds)
				{
					int parentId2 = this.m_TreeView.viewController.GetParentId(num2);
					int childIndexForId = this.m_TreeView.viewController.GetChildIndexForId(num2);
					list.Add(new ValueTuple<int, int>(parentId2, childIndexForId));
					bool flag2 = flag;
					if (flag2)
					{
						this.m_TreeView.viewController.Move(num2, parentId, -1, false);
					}
					else
					{
						int num3 = childIndex + num;
						bool flag3 = parentId2 != parentId || childIndexForId >= childIndex;
						if (flag3)
						{
							num++;
						}
						this.m_TreeView.viewController.Move(num2, parentId, num3, false);
					}
				}
				bool flag4 = args.dragAndDropPosition == DragAndDropPosition.OverItem;
				if (flag4)
				{
					this.m_TreeView.viewController.ExpandItem(parentId, false, false);
				}
				IVisualElementScheduledItem expandDropItemScheduledItem = this.m_ExpandDropItemScheduledItem;
				if (expandDropItemScheduledItem != null)
				{
					expandDropItemScheduledItem.Pause();
				}
				this.m_TreeView.viewController.RebuildTree();
				this.m_TreeView.RefreshItems();
				for (int j = 0; j < this.m_DropData.draggedIds.Length; j++)
				{
					int num4 = this.m_DropData.draggedIds[j];
					ValueTuple<int, int> valueTuple = list[j];
					int parentId3 = this.m_TreeView.viewController.GetParentId(num4);
					int childIndexForId2 = this.m_TreeView.viewController.GetChildIndexForId(num4);
					bool flag5 = valueTuple.Item1 == parentId3 && valueTuple.Item2 == childIndexForId2;
					if (!flag5)
					{
						this.m_TreeView.viewController.RaiseItemParentChanged(num4, parentId);
					}
				}
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00030AA4 File Offset: 0x0002ECA4
		public override void DragCleanup()
		{
			bool flag = this.m_DropData != null;
			if (flag)
			{
				bool flag2 = this.m_DropData.expandedIdsBeforeDrag != null;
				if (flag2)
				{
					this.RestoreExpanded(new List<int>(this.m_DropData.expandedIdsBeforeDrag));
				}
				this.m_DropData = new TreeViewReorderableDragAndDropController.DropData();
			}
			IVisualElementScheduledItem expandDropItemScheduledItem = this.m_ExpandDropItemScheduledItem;
			if (expandDropItemScheduledItem != null)
			{
				expandDropItemScheduledItem.Pause();
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00030B0C File Offset: 0x0002ED0C
		private void RestoreExpanded(List<int> ids)
		{
			foreach (int num in this.m_TreeView.viewController.GetAllItemIds(null))
			{
				bool flag = !ids.Contains(num);
				if (flag)
				{
					this.m_TreeView.CollapseItem(num, false);
				}
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00030B80 File Offset: 0x0002ED80
		public override void HandleAutoExpand(ReusableCollectionItem item, Vector2 pointerPosition)
		{
			int id = item.id;
			Rect worldBound = item.bindableElement.worldBound;
			Rect rect = new Rect(worldBound.x, worldBound.y + 4f, worldBound.width, worldBound.height - 8f);
			bool flag = rect.Contains(pointerPosition);
			Vector2 vector = this.m_DropData.expandItemBeginPosition - pointerPosition;
			bool flag2 = id != this.m_DropData.lastItemId || !flag || vector.sqrMagnitude >= 100f;
			if (flag2)
			{
				this.m_DropData.lastItemId = id;
				this.m_DropData.expandItemBeginTimerMs = (float)Panel.TimeSinceStartupMs();
				this.m_DropData.expandItemBeginPosition = pointerPosition;
				this.DelayExpandDropItem();
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00030C4C File Offset: 0x0002EE4C
		private void DelayExpandDropItem()
		{
			bool flag = this.m_ExpandDropItemScheduledItem == null;
			if (flag)
			{
				this.m_ExpandDropItemScheduledItem = this.m_TreeView.schedule.Execute(this.m_ExpandDropItemCallback).Every(10L);
			}
			else
			{
				this.m_ExpandDropItemScheduledItem.Pause();
				this.m_ExpandDropItemScheduledItem.Resume();
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00030CAC File Offset: 0x0002EEAC
		private void ExpandDropItem()
		{
			bool flag = (float)Panel.TimeSinceStartupMs() - this.m_DropData.expandItemBeginTimerMs > 700f;
			bool flag2 = flag;
			int lastItemId = this.m_DropData.lastItemId;
			bool flag3 = this.m_TreeView.viewController.Exists(lastItemId) && flag2;
			if (flag3)
			{
				bool flag4 = this.m_TreeView.viewController.HasChildren(lastItemId);
				bool flag5 = this.m_TreeView.IsExpanded(lastItemId);
				bool flag6 = !flag4 || flag5;
				if (!flag6)
				{
					TreeViewReorderableDragAndDropController.DropData dropData = this.m_DropData;
					if (dropData.expandedIdsBeforeDrag == null)
					{
						dropData.expandedIdsBeforeDrag = this.m_TreeView.expandedItemIds.ToArray();
					}
					this.m_DropData.expandItemBeginTimerMs = (float)Panel.TimeSinceStartupMs();
					this.m_DropData.lastItemId = 0;
					this.m_TreeView.ExpandItem(lastItemId, false);
				}
			}
		}

		// Token: 0x040005B4 RID: 1460
		private const long k_ExpandUpdateIntervalMs = 10L;

		// Token: 0x040005B5 RID: 1461
		private const float k_DropExpandTimeoutMs = 700f;

		// Token: 0x040005B6 RID: 1462
		private const float k_DropDeltaPosition = 100f;

		// Token: 0x040005B7 RID: 1463
		private const float k_HalfDropBetweenHeight = 4f;

		// Token: 0x040005B8 RID: 1464
		protected TreeViewReorderableDragAndDropController.DropData m_DropData = new TreeViewReorderableDragAndDropController.DropData();

		// Token: 0x040005B9 RID: 1465
		protected readonly BaseTreeView m_TreeView;

		// Token: 0x040005BA RID: 1466
		private IVisualElementScheduledItem m_ExpandDropItemScheduledItem;

		// Token: 0x040005BB RID: 1467
		private Action m_ExpandDropItemCallback;

		// Token: 0x02000178 RID: 376
		protected class DropData
		{
			// Token: 0x040005BC RID: 1468
			public int[] expandedIdsBeforeDrag;

			// Token: 0x040005BD RID: 1469
			public int[] draggedIds;

			// Token: 0x040005BE RID: 1470
			public int lastItemId = -1;

			// Token: 0x040005BF RID: 1471
			public float expandItemBeginTimerMs;

			// Token: 0x040005C0 RID: 1472
			public Vector2 expandItemBeginPosition;
		}
	}
}
