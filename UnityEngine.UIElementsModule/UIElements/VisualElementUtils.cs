using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200040D RID: 1037
	internal static class VisualElementUtils
	{
		// Token: 0x06002128 RID: 8488 RVA: 0x0007D7AC File Offset: 0x0007B9AC
		public static string GetUniqueName(string nameBase)
		{
			string text = nameBase;
			int num = 2;
			while (VisualElementUtils.s_usedNames.Contains(text))
			{
				text = nameBase + num.ToString();
				num++;
			}
			VisualElementUtils.s_usedNames.Add(text);
			return text;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0007D7F4 File Offset: 0x0007B9F4
		internal static int GetFoldoutDepth(this VisualElement element)
		{
			int num = 0;
			bool flag = element.parent != null;
			if (flag)
			{
				for (VisualElement visualElement = element.parent; visualElement != null; visualElement = visualElement.parent)
				{
					bool flag2 = VisualElementUtils.s_FoldoutType.IsAssignableFrom(visualElement.GetType());
					if (flag2)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0007D850 File Offset: 0x0007BA50
		internal static void AssignInspectorStyleIfNecessary(this VisualElement element, string classNameToEnable)
		{
			VisualElement firstAncestorWhere = element.GetFirstAncestorWhere((VisualElement i) => i.ClassListContains(VisualElementUtils.s_InspectorElementUssClassName));
			element.EnableInClassList(classNameToEnable, firstAncestorWhere != null);
		}

		// Token: 0x04000E01 RID: 3585
		private static readonly HashSet<string> s_usedNames = new HashSet<string>();

		// Token: 0x04000E02 RID: 3586
		private static readonly Type s_FoldoutType = typeof(Foldout);

		// Token: 0x04000E03 RID: 3587
		private static readonly string s_InspectorElementUssClassName = "unity-inspector-element";
	}
}
