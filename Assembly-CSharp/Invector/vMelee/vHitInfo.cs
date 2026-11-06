using System;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x02000510 RID: 1296
	public class vHitInfo
	{
		// Token: 0x06001F6E RID: 8046 RVA: 0x000BB453 File Offset: 0x000B9653
		public vHitInfo(vMeleeAttackObject attackObject, vHitBox hitBox, Collider targetCollider, Vector3 hitPoint)
		{
			this.attackObject = attackObject;
			this.hitBox = hitBox;
			this.targetCollider = targetCollider;
			this.hitPoint = hitPoint;
		}

		// Token: 0x04002024 RID: 8228
		public vMeleeAttackObject attackObject;

		// Token: 0x04002025 RID: 8229
		public vHitBox hitBox;

		// Token: 0x04002026 RID: 8230
		public Vector3 hitPoint;

		// Token: 0x04002027 RID: 8231
		public Collider targetCollider;

		// Token: 0x04002028 RID: 8232
		public bool targetIsBlocking;
	}
}
