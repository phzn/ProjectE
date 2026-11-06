using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000165 RID: 357
	internal interface IDragAndDrop
	{
		// Token: 0x06000BAA RID: 2986
		void StartDrag(StartDragArgs args, Vector3 pointerPosition);

		// Token: 0x06000BAB RID: 2987
		void UpdateDrag(Vector3 pointerPosition);

		// Token: 0x06000BAC RID: 2988
		void AcceptDrag();

		// Token: 0x06000BAD RID: 2989
		void DragCleanup();

		// Token: 0x06000BAE RID: 2990
		void SetVisualMode(DragVisualMode visualMode);

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000BAF RID: 2991
		DragAndDropData data { get; }
	}
}
