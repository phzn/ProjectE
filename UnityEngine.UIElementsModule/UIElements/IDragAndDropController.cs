using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x0200016B RID: 363
	internal interface IDragAndDropController<in TArgs>
	{
		// Token: 0x06000BCC RID: 3020
		bool CanStartDrag(IEnumerable<int> itemIds);

		// Token: 0x06000BCD RID: 3021
		StartDragArgs SetupDragAndDrop(IEnumerable<int> itemIds, bool skipText = false);

		// Token: 0x06000BCE RID: 3022
		DragVisualMode HandleDragAndDrop(TArgs args);

		// Token: 0x06000BCF RID: 3023
		void OnDrop(TArgs args);

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void DragCleanup()
		{
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void HandleAutoExpand(ReusableCollectionItem item, Vector2 pointerPosition)
		{
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002E6AD File Offset: 0x0002C8AD
		IEnumerable<int> GetSortedSelectedIds()
		{
			return Enumerable.Empty<int>();
		}
	}
}
