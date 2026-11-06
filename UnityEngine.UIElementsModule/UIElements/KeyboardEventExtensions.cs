using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D9 RID: 473
	internal static class KeyboardEventExtensions
	{
		// Token: 0x06000E4C RID: 3660 RVA: 0x00037070 File Offset: 0x00035270
		internal static bool ShouldSendNavigationMoveEvent(this KeyDownEvent e)
		{
			return e.keyCode == KeyCode.Tab && !e.ctrlKey && !e.altKey && !e.commandKey && !e.functionKey;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000370B0 File Offset: 0x000352B0
		internal static bool ShouldSendNavigationMoveEventRuntime(this Event e)
		{
			return e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab;
		}
	}
}
