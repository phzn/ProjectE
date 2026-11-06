using System;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000390 RID: 912
	public struct StrikePin
	{
		// Token: 0x060018DF RID: 6367 RVA: 0x00069AC0 File Offset: 0x00067CC0
		public void ResetPin()
		{
			this.Rb.transform.position = this.InitialPosition;
			this.Rb.transform.rotation = this.InitialRotation;
			this.Rb.velocity = Vector3.zero;
			this.Rb.angularVelocity = Vector3.zero;
		}

		// Token: 0x04001D07 RID: 7431
		public Rigidbody Rb;

		// Token: 0x04001D08 RID: 7432
		public Vector3 InitialPosition;

		// Token: 0x04001D09 RID: 7433
		public Quaternion InitialRotation;
	}
}
