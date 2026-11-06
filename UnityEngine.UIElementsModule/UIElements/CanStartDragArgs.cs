using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200016D RID: 365
	internal readonly struct CanStartDragArgs
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002E6B4 File Offset: 0x0002C8B4
		internal CanStartDragArgs(VisualElement draggedElement, int id, IEnumerable<int> selectedIds)
		{
			this.draggedElement = draggedElement;
			this.id = id;
			this.selectedIds = selectedIds;
		}

		// Token: 0x0400058F RID: 1423
		public readonly VisualElement draggedElement;

		// Token: 0x04000590 RID: 1424
		public readonly int id;

		// Token: 0x04000591 RID: 1425
		public readonly IEnumerable<int> selectedIds;
	}
}
