using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000259 RID: 601
	internal static class UIDocumentHierarchyUtil
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x0003E9F8 File Offset: 0x0003CBF8
		internal static int FindHierarchicalSortedIndex(SortedDictionary<UIDocumentHierarchicalIndex, UIDocument> children, UIDocument child)
		{
			int num = 0;
			foreach (UIDocument uidocument in children.Values)
			{
				bool flag = uidocument == child;
				if (flag)
				{
					return num;
				}
				bool flag2 = uidocument.rootVisualElement != null && uidocument.rootVisualElement.parent != null;
				if (flag2)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0003EA88 File Offset: 0x0003CC88
		internal static void SetHierarchicalIndex(Transform childTransform, Transform directParentTransform, Transform mainParentTransform, out UIDocumentHierarchicalIndex hierarchicalIndex)
		{
			bool flag = mainParentTransform == null || childTransform == null;
			if (flag)
			{
				hierarchicalIndex.pathToParent = null;
			}
			else
			{
				bool flag2 = directParentTransform == mainParentTransform;
				if (flag2)
				{
					hierarchicalIndex.pathToParent = new int[] { childTransform.GetSiblingIndex() };
				}
				else
				{
					List<int> list = new List<int>();
					while (mainParentTransform != childTransform && childTransform != null)
					{
						list.Add(childTransform.GetSiblingIndex());
						childTransform = childTransform.parent;
					}
					list.Reverse();
					hierarchicalIndex.pathToParent = list.ToArray();
				}
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0003EB28 File Offset: 0x0003CD28
		internal static void SetGlobalIndex(Transform objectTransform, Transform directParentTransform, out UIDocumentHierarchicalIndex globalIndex)
		{
			bool flag = objectTransform == null;
			if (flag)
			{
				globalIndex.pathToParent = null;
			}
			else
			{
				bool flag2 = directParentTransform == null;
				if (flag2)
				{
					globalIndex.pathToParent = new int[] { objectTransform.GetSiblingIndex() };
				}
				else
				{
					List<int> list = new List<int> { objectTransform.GetSiblingIndex() };
					while (directParentTransform != null)
					{
						list.Add(directParentTransform.GetSiblingIndex());
						directParentTransform = directParentTransform.parent;
					}
					list.Reverse();
					globalIndex.pathToParent = list.ToArray();
				}
			}
		}

		// Token: 0x040007A2 RID: 1954
		internal static UIDocumentHierarchicalIndexComparer indexComparer = new UIDocumentHierarchicalIndexComparer();
	}
}
