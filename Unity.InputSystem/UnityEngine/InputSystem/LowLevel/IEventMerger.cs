using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000CC RID: 204
	internal interface IEventMerger
	{
		// Token: 0x06000D4D RID: 3405
		bool MergeForward(InputEventPtr currentEventPtr, InputEventPtr nextEventPtr);
	}
}
