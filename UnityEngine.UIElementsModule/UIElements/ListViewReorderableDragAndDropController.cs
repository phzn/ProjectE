using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000176 RID: 374
	internal class ListViewReorderableDragAndDropController : BaseReorderableDragAndDropController
	{
		// Token: 0x06000C11 RID: 3089 RVA: 0x0003049E File Offset: 0x0002E69E
		public ListViewReorderableDragAndDropController(BaseListView view)
			: base(view)
		{
			this.m_ListView = view;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000304B0 File Offset: 0x0002E6B0
		public override DragVisualMode HandleDragAndDrop(IListDragAndDropArgs args)
		{
			bool flag = args.dragAndDropPosition == DragAndDropPosition.OverItem || !this.enableReordering;
			DragVisualMode dragVisualMode;
			if (flag)
			{
				dragVisualMode = DragVisualMode.Rejected;
			}
			else
			{
				dragVisualMode = ((args.dragAndDropData.source == this.m_ListView) ? DragVisualMode.Move : DragVisualMode.Rejected);
			}
			return dragVisualMode;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000304F8 File Offset: 0x0002E6F8
		public override void OnDrop(IListDragAndDropArgs args)
		{
			int insertAtIndex = args.insertAtIndex;
			int num = 0;
			int num2 = 0;
			for (int i = this.m_SortedSelectedIds.Count - 1; i >= 0; i--)
			{
				int num3 = this.m_SortedSelectedIds[i];
				int num4 = this.m_View.viewController.GetIndexForId(num3);
				bool flag = num4 < 0;
				if (!flag)
				{
					int num5 = insertAtIndex - num;
					bool flag2 = num4 >= insertAtIndex;
					if (flag2)
					{
						num4 += num2;
						num2++;
					}
					else
					{
						bool flag3 = num4 < num5;
						if (flag3)
						{
							num++;
							num5--;
						}
					}
					this.m_ListView.viewController.Move(num4, num5);
				}
			}
			bool flag4 = this.m_ListView.selectionType > SelectionType.None;
			if (flag4)
			{
				List<int> list = new List<int>();
				for (int j = 0; j < this.m_SortedSelectedIds.Count; j++)
				{
					list.Add(insertAtIndex - num + j);
				}
				this.m_ListView.SetSelectionWithoutNotify(list);
			}
			else
			{
				this.m_ListView.ClearSelection();
			}
		}

		// Token: 0x040005B3 RID: 1459
		protected readonly BaseListView m_ListView;
	}
}
