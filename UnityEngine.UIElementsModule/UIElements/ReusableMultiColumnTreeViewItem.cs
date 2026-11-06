using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004E RID: 78
	internal class ReusableMultiColumnTreeViewItem : ReusableTreeViewItem
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000BC4E File Offset: 0x00009E4E
		public override VisualElement rootElement
		{
			get
			{
				return base.bindableElement;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public override void Init(VisualElement item)
		{
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C27C File Offset: 0x0000A47C
		public void Init(VisualElement container, Columns columns)
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
					base.InitExpandHierarchy(visualElement, visualElement2);
					break;
				}
				num++;
			}
		}
	}
}
