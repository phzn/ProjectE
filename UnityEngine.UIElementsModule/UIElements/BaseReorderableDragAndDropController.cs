using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200015F RID: 351
	internal abstract class BaseReorderableDragAndDropController : ICollectionDragAndDropController, IDragAndDropController<IListDragAndDropArgs>, IReorderable
	{
		// Token: 0x06000B7A RID: 2938 RVA: 0x0002DBDF File Offset: 0x0002BDDF
		public IEnumerable<int> GetSortedSelectedIds()
		{
			return this.m_SortedSelectedIds;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002DBE7 File Offset: 0x0002BDE7
		protected BaseReorderableDragAndDropController(BaseVerticalCollectionView view)
		{
			this.m_View = view;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002DC0A File Offset: 0x0002BE0A
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0002DC12 File Offset: 0x0002BE12
		public virtual bool enableReordering { get; set; } = true;

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002DC1C File Offset: 0x0002BE1C
		public virtual bool CanStartDrag(IEnumerable<int> itemIds)
		{
			return this.enableReordering;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002DC34 File Offset: 0x0002BE34
		public virtual StartDragArgs SetupDragAndDrop(IEnumerable<int> itemIds, bool skipText = false)
		{
			this.m_SortedSelectedIds.Clear();
			string text = string.Empty;
			bool flag = itemIds != null;
			if (flag)
			{
				foreach (int num in itemIds)
				{
					this.m_SortedSelectedIds.Add(num);
					bool flag2 = skipText;
					if (!flag2)
					{
						bool flag3 = string.IsNullOrEmpty(text);
						if (flag3)
						{
							ReusableCollectionItem recycledItemFromId = this.m_View.GetRecycledItemFromId(num);
							Label label = ((recycledItemFromId != null) ? recycledItemFromId.rootElement.Q<Label>(null, null) : null);
							text = ((label != null) ? label.text : string.Format("Item {0}", num));
						}
						else
						{
							text = "<Multiple>";
							skipText = true;
						}
					}
				}
			}
			this.m_SortedSelectedIds.Sort(new Comparison<int>(this.CompareId));
			return new StartDragArgs(text, DragVisualMode.Move);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002DD34 File Offset: 0x0002BF34
		protected virtual int CompareId(int id1, int id2)
		{
			return id1.CompareTo(id2);
		}

		// Token: 0x06000B81 RID: 2945
		public abstract DragVisualMode HandleDragAndDrop(IListDragAndDropArgs args);

		// Token: 0x06000B82 RID: 2946
		public abstract void OnDrop(IListDragAndDropArgs args);

		// Token: 0x06000B83 RID: 2947 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void DragCleanup()
		{
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void HandleAutoExpand(ReusableCollectionItem item, Vector2 pointerPosition)
		{
		}

		// Token: 0x0400056D RID: 1389
		protected readonly BaseVerticalCollectionView m_View;

		// Token: 0x0400056E RID: 1390
		protected List<int> m_SortedSelectedIds = new List<int>();
	}
}
