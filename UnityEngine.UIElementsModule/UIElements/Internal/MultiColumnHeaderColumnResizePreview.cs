using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004C7 RID: 1223
	internal class MultiColumnHeaderColumnResizePreview : VisualElement
	{
		// Token: 0x0600264E RID: 9806 RVA: 0x000A06F0 File Offset: 0x0009E8F0
		public MultiColumnHeaderColumnResizePreview()
		{
			base.AddToClassList(MultiColumnHeaderColumnResizePreview.ussClassName);
			base.pickingMode = PickingMode.Ignore;
			VisualElement visualElement = new VisualElement
			{
				pickingMode = PickingMode.Ignore
			};
			visualElement.AddToClassList(MultiColumnHeaderColumnResizePreview.visualUssClassName);
			base.Add(visualElement);
		}

		// Token: 0x04001266 RID: 4710
		public static readonly string ussClassName = MultiColumnHeaderColumn.ussClassName + "__resize-preview";

		// Token: 0x04001267 RID: 4711
		public static readonly string visualUssClassName = MultiColumnHeaderColumnResizePreview.ussClassName + "__visual";
	}
}
