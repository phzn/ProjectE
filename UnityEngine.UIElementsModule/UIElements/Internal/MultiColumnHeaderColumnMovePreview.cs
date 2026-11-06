using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004C4 RID: 1220
	internal class MultiColumnHeaderColumnMovePreview : VisualElement
	{
		// Token: 0x06002629 RID: 9769 RVA: 0x0009F9E4 File Offset: 0x0009DBE4
		public MultiColumnHeaderColumnMovePreview()
		{
			base.AddToClassList(MultiColumnHeaderColumnMovePreview.ussClassName);
			base.pickingMode = PickingMode.Ignore;
		}

		// Token: 0x04001252 RID: 4690
		public static readonly string ussClassName = MultiColumnHeaderColumn.ussClassName + "__move-preview";
	}
}
