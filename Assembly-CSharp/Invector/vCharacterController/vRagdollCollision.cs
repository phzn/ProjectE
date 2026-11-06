using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006B7 RID: 1719
	public class vRagdollCollision
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002A37 RID: 10807 RVA: 0x000ED39C File Offset: 0x000EB59C
		public GameObject Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000ED3A4 File Offset: 0x000EB5A4
		public Collision Collision
		{
			get
			{
				return this.collision;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000ED3AC File Offset: 0x000EB5AC
		public float ImpactForce
		{
			get
			{
				return this.impactForce;
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000ED3B4 File Offset: 0x000EB5B4
		public vRagdollCollision(GameObject sender, Collision collision)
		{
			this.sender = sender;
			this.collision = collision;
			this.impactForce = collision.relativeVelocity.magnitude;
		}

		// Token: 0x04002A4D RID: 10829
		private GameObject sender;

		// Token: 0x04002A4E RID: 10830
		private Collision collision;

		// Token: 0x04002A4F RID: 10831
		private float impactForce;
	}
}
