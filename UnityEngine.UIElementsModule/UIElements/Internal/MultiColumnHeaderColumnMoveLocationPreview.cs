using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004C5 RID: 1221
	internal class MultiColumnHeaderColumnMoveLocationPreview : VisualElement
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x0009FA18 File Offset: 0x0009DC18
		public MultiColumnHeaderColumnMoveLocationPreview()
		{
			base.AddToClassList(MultiColumnHeaderColumnMoveLocationPreview.ussClassName);
			base.pickingMode = PickingMode.Ignore;
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList(MultiColumnHeaderColumnMoveLocationPreview.visualUssClassName);
			visualElement.pickingMode = PickingMode.Ignore;
			base.Add(visualElement);
		}

		// Token: 0x04001253 RID: 4691
		public static readonly string ussClassName = MultiColumnHeaderColumn.ussClassName + "__move-location-preview";

		// Token: 0x04001254 RID: 4692
		public static readonly string visualUssClassName = MultiColumnHeaderColumnMoveLocationPreview.ussClassName + "__visual";
	}
}
