using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004D RID: 77
	internal class ReusableMultiColumnListViewItem : ReusableListViewItem
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000BC4E File Offset: 0x00009E4E
		public override VisualElement rootElement
		{
			get
			{
				return base.bindableElement;
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public override void Init(VisualElement item)
		{
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public void Init(VisualElement container, Columns columns, bool usesAnimatedDrag)
		{
			int num = 0;
			base.bindableElement = container;
			foreach (Column column in columns.visibleList)
			{
				bool flag = columns.IsPrimary(column);
				if (flag)
				{
					VisualElement visualElement = container[num];
					VisualElement visualElement2 = visualElement.GetProperty(MultiColumnController.bindableElementPropertyName) as VisualElement;
					base.UpdateHierarchy(visualElement, visualElement2, usesAnimatedDrag);
					break;
				}
				num++;
			}
		}
	}
}
