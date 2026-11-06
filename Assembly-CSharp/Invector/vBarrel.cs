using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004E1 RID: 1249
	public class vBarrel : vHealthController
	{
		// Token: 0x06001E18 RID: 7704 RVA: 0x000B71D4 File Offset: 0x000B53D4
		private void OnCollisionEnter()
		{
			if (!this.referenceTransformUP)
			{
				return;
			}
			if (Vector3.Angle(this.referenceTransformUP.up, Vector3.up) > this.maxAngleUp && !this.isBarrelRoll)
			{
				this.isBarrelRoll = true;
				this.onBarrelRoll.Invoke();
			}
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000B7226 File Offset: 0x000B5426
		public override void TakeDamage(vDamage damage)
		{
			if (this.acceptableAttacks.Contains(damage.damageType))
			{
				base.TakeDamage(damage);
			}
		}

		// Token: 0x04001F16 RID: 7958
		public Transform referenceTransformUP;

		// Token: 0x04001F17 RID: 7959
		public float maxAngleUp = 90f;

		// Token: 0x04001F18 RID: 7960
		protected bool isBarrelRoll;

		// Token: 0x04001F19 RID: 7961
		public UnityEvent onBarrelRoll;

		// Token: 0x04001F1A RID: 7962
		[vEditorToolbar("Health", false, "", false, false)]
		public List<string> acceptableAttacks = new List<string> { "explosion", "projectile" };
	}
}
