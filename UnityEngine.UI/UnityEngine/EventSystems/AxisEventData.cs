using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200004D RID: 77
	public class AxisEventData : BaseEventData
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x000180AE File Offset: 0x000162AE
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x000180B6 File Offset: 0x000162B6
		public Vector2 moveVector { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000180BF File Offset: 0x000162BF
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x000180C7 File Offset: 0x000162C7
		public MoveDirection moveDir { get; set; }

		// Token: 0x06000532 RID: 1330 RVA: 0x000180D0 File Offset: 0x000162D0
		public AxisEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
			this.moveVector = Vector2.zero;
			this.moveDir = MoveDirection.None;
		}
	}
}
