using System;
using UnityEngine.EventSystems;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x02000086 RID: 134
	internal class ExtendedAxisEventData : AxisEventData, INavigationEventData
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0003B1F6 File Offset: 0x000393F6
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x0003B1FE File Offset: 0x000393FE
		public InputDevice device { get; set; }

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003B207 File Offset: 0x00039407
		public ExtendedAxisEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003B210 File Offset: 0x00039410
		public override string ToString()
		{
			return string.Format("MoveDir: {0}\nMoveVector: {1}", base.moveDir, base.moveVector);
		}
	}
}
