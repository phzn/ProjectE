using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000374 RID: 884
	[AddComponentMenu("")]
	public class BounceRocks : MonoBehaviour
	{
		// Token: 0x0600184D RID: 6221 RVA: 0x00067280 File Offset: 0x00065480
		public virtual void Bounce()
		{
			foreach (Rigidbody rigidbody in this.Rocks)
			{
				this._force = MMMaths.RandomVector3(this.MinForce, this.MaxForce);
				this._torque = MMMaths.RandomVector3(this.MinTorque, this.MaxTorque);
				rigidbody.AddForce(this._force, ForceMode.Impulse);
				rigidbody.AddTorque(this._torque, ForceMode.Impulse);
			}
		}

		// Token: 0x04001C2B RID: 7211
		public List<Rigidbody> Rocks;

		// Token: 0x04001C2C RID: 7212
		public Vector3 MinForce;

		// Token: 0x04001C2D RID: 7213
		public Vector3 MaxForce;

		// Token: 0x04001C2E RID: 7214
		public Vector3 MinTorque;

		// Token: 0x04001C2F RID: 7215
		public Vector3 MaxTorque;

		// Token: 0x04001C30 RID: 7216
		protected Vector3 _force;

		// Token: 0x04001C31 RID: 7217
		protected Vector3 _torque;
	}
}
