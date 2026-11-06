using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000481 RID: 1153
	internal abstract class HierarchyTraversal
	{
		// Token: 0x060023E3 RID: 9187 RVA: 0x000929AF File Offset: 0x00090BAF
		public virtual void Traverse(VisualElement element)
		{
			this.TraverseRecursive(element, 0);
		}

		// Token: 0x060023E4 RID: 9188
		public abstract void TraverseRecursive(VisualElement element, int depth);

		// Token: 0x060023E5 RID: 9189 RVA: 0x000929BC File Offset: 0x00090BBC
		protected void Recurse(VisualElement element, int depth)
		{
			int i = 0;
			while (i < element.hierarchy.childCount)
			{
				VisualElement visualElement = element.hierarchy[i];
				this.TraverseRecursive(visualElement, depth + 1);
				bool flag = visualElement.hierarchy.parent != element;
				if (!flag)
				{
					i++;
				}
			}
		}
	}
}
