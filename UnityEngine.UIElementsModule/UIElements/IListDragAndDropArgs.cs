using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000168 RID: 360
	internal interface IListDragAndDropArgs
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000BBA RID: 3002
		object target { get; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000BBB RID: 3003
		int insertAtIndex { get; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000BBC RID: 3004
		int parentId { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000BBD RID: 3005
		int childIndex { get; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000BBE RID: 3006
		DragAndDropData dragAndDropData { get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000BBF RID: 3007
		DragAndDropPosition dragAndDropPosition { get; }
	}
}
