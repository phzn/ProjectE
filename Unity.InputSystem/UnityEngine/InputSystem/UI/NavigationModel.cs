using System;
using UnityEngine.EventSystems;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x0200008E RID: 142
	internal struct NavigationModel
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x0003E0EC File Offset: 0x0003C2EC
		public void Reset()
		{
			this.move = Vector2.zero;
		}

		// Token: 0x04000420 RID: 1056
		public Vector2 move;

		// Token: 0x04000421 RID: 1057
		public int consecutiveMoveCount;

		// Token: 0x04000422 RID: 1058
		public MoveDirection lastMoveDirection;

		// Token: 0x04000423 RID: 1059
		public float lastMoveTime;

		// Token: 0x04000424 RID: 1060
		public AxisEventData eventData;

		// Token: 0x04000425 RID: 1061
		public InputDevice device;
	}
}
