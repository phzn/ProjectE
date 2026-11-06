using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200003D RID: 61
	internal interface IDefaultTreeViewController<T>
	{
		// Token: 0x06000299 RID: 665
		void SetRootItems(IList<TreeViewItemData<T>> items);

		// Token: 0x0600029A RID: 666
		void AddItem(in TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree = true);

		// Token: 0x0600029B RID: 667
		TreeViewItemData<T> GetTreeViewItemDataForId(int id);

		// Token: 0x0600029C RID: 668
		TreeViewItemData<T> GetTreeViewItemDataForIndex(int index);

		// Token: 0x0600029D RID: 669
		T GetDataForId(int id);

		// Token: 0x0600029E RID: 670
		T GetDataForIndex(int index);
	}
}
