using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200016E RID: 366
	internal readonly struct SetupDragAndDropArgs
	{
		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002E6CC File Offset: 0x0002C8CC
		internal SetupDragAndDropArgs(VisualElement draggedElement, IEnumerable<int> selectedIds, StartDragArgs startDragArgs)
		{
			this.draggedElement = draggedElement;
			this.selectedIds = selectedIds;
			this.startDragArgs = startDragArgs;
		}

		// Token: 0x04000592 RID: 1426
		public readonly VisualElement draggedElement;

		// Token: 0x04000593 RID: 1427
		public readonly IEnumerable<int> selectedIds;

		// Token: 0x04000594 RID: 1428
		public readonly StartDragArgs startDragArgs;
	}
}
