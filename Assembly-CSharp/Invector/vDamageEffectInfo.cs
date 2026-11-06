using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200049D RID: 1181
	public class vDamageEffectInfo
	{
		// Token: 0x06001CFB RID: 7419 RVA: 0x000B2F41 File Offset: 0x000B1141
		public vDamageEffectInfo(Vector3 position, Quaternion rotation, string damageType = "", Transform receiver = null)
		{
			this.receiver = receiver;
			this.position = position;
			this.rotation = rotation;
			this.damageType = damageType;
		}

		// Token: 0x04001E13 RID: 7699
		public Transform receiver;

		// Token: 0x04001E14 RID: 7700
		public Vector3 position;

		// Token: 0x04001E15 RID: 7701
		public Quaternion rotation;

		// Token: 0x04001E16 RID: 7702
		public string damageType;
	}
}
