using System;
using UnityEngine.EventSystems;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x0200008A RID: 138
	internal class ExtendedSubmitCancelEventData : BaseEventData, INavigationEventData
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0003B724 File Offset: 0x00039924
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0003B72C File Offset: 0x0003992C
		public InputDevice device { get; set; }

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003B735 File Offset: 0x00039935
		public ExtendedSubmitCancelEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
		}
	}
}
