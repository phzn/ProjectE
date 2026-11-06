using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000174 RID: 372
	internal static class ListViewDraggerExtension
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		public static ReusableCollectionItem GetRecycledItemFromId(this BaseVerticalCollectionView listView, int id)
		{
			foreach (ReusableCollectionItem reusableCollectionItem in listView.activeItems)
			{
				bool flag = reusableCollectionItem.id.Equals(id);
				if (flag)
				{
					return reusableCollectionItem;
				}
			}
			return null;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002FC20 File Offset: 0x0002DE20
		public static ReusableCollectionItem GetRecycledItemFromIndex(this BaseVerticalCollectionView listView, int index)
		{
			foreach (ReusableCollectionItem reusableCollectionItem in listView.activeItems)
			{
				bool flag = reusableCollectionItem.index.Equals(index);
				if (flag)
				{
					return reusableCollectionItem;
				}
			}
			return null;
		}
	}
}
