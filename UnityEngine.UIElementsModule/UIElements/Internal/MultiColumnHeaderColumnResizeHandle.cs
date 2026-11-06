using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004D3 RID: 1235
	internal class MultiColumnHeaderColumnResizeHandle : VisualElement
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x000A3205 File Offset: 0x000A1405
		public VisualElement dragArea { get; }

		// Token: 0x060026C6 RID: 9926 RVA: 0x000A3210 File Offset: 0x000A1410
		public MultiColumnHeaderColumnResizeHandle()
		{
			base.AddToClassList(MultiColumnHeaderColumnResizeHandle.ussClassName);
			this.dragArea = new VisualElement
			{
				focusable = true
			};
			this.dragArea.AddToClassList(MultiColumnHeaderColumnResizeHandle.dragAreaUssClassName);
			base.Add(this.dragArea);
		}

		// Token: 0x040012B3 RID: 4787
		public static readonly string ussClassName = MultiColumnCollectionHeader.ussClassName + "__column-resize-handle";

		// Token: 0x040012B4 RID: 4788
		public static readonly string dragAreaUssClassName = MultiColumnHeaderColumnResizeHandle.ussClassName + "__drag-area";
	}
}
