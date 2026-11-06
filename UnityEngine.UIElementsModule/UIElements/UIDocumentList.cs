using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000257 RID: 599
	internal class UIDocumentList
	{
		// Token: 0x06001128 RID: 4392 RVA: 0x0003DFFC File Offset: 0x0003C1FC
		internal void RemoveFromListAndFromVisualTree(UIDocument uiDocument)
		{
			this.m_AttachedUIDocuments.Remove(uiDocument);
			VisualElement rootVisualElement = uiDocument.rootVisualElement;
			if (rootVisualElement != null)
			{
				rootVisualElement.RemoveFromHierarchy();
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0003E020 File Offset: 0x0003C220
		internal void AddToListAndToVisualTree(UIDocument uiDocument, VisualElement visualTree, int firstInsertIndex = 0)
		{
			int num = 0;
			foreach (UIDocument uidocument in this.m_AttachedUIDocuments)
			{
				bool flag = uiDocument.sortingOrder > uidocument.sortingOrder;
				if (flag)
				{
					num++;
				}
				else
				{
					bool flag2 = uiDocument.sortingOrder < uidocument.sortingOrder;
					if (flag2)
					{
						break;
					}
					bool flag3 = uiDocument.m_UIDocumentCreationIndex > uidocument.m_UIDocumentCreationIndex;
					if (!flag3)
					{
						break;
					}
					num++;
				}
			}
			bool flag4 = num < this.m_AttachedUIDocuments.Count;
			if (flag4)
			{
				this.m_AttachedUIDocuments.Insert(num, uiDocument);
				bool flag5 = visualTree == null || uiDocument.rootVisualElement == null;
				if (flag5)
				{
					return;
				}
				bool flag6 = num > 0;
				if (flag6)
				{
					VisualElement visualElement = null;
					int num2 = 1;
					while (visualElement == null && num - num2 >= 0)
					{
						UIDocument uidocument2 = this.m_AttachedUIDocuments[num - num2++];
						visualElement = uidocument2.rootVisualElement;
					}
					bool flag7 = visualElement != null;
					if (flag7)
					{
						num = visualTree.IndexOf(visualElement) + 1;
					}
				}
				bool flag8 = num > visualTree.childCount;
				if (flag8)
				{
					num = visualTree.childCount;
				}
			}
			else
			{
				this.m_AttachedUIDocuments.Add(uiDocument);
			}
			bool flag9 = visualTree == null || uiDocument.rootVisualElement == null;
			if (!flag9)
			{
				int num3 = firstInsertIndex + num;
				bool flag10 = num3 < visualTree.childCount;
				if (flag10)
				{
					visualTree.Insert(num3, uiDocument.rootVisualElement);
				}
				else
				{
					visualTree.Add(uiDocument.rootVisualElement);
				}
			}
		}

		// Token: 0x04000793 RID: 1939
		internal List<UIDocument> m_AttachedUIDocuments = new List<UIDocument>();
	}
}
