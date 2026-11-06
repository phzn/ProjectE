using System;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200092E RID: 2350
	[DefaultExecutionOrder(32000)]
	public class SubwayPlatformCarry : MonoBehaviour
	{
		// Token: 0x06003FB3 RID: 16307 RVA: 0x0017A2A4 File Offset: 0x001784A4
		private void FixedUpdate()
		{
			if (this.Velocity == Vector3.zero)
			{
				return;
			}
			Vector3 vector = this.Velocity * Time.fixedDeltaTime;
			if (this.TrainRb != null)
			{
				this.TrainRb.MovePosition(this.TrainRb.position + vector);
			}
			else
			{
				base.transform.position += vector;
			}
			if (this.PlayerRb != null)
			{
				Vector3 velocity = this.PlayerRb.velocity;
				this.PlayerRb.velocity = new Vector3(velocity.x + this.Velocity.x, velocity.y, velocity.z + this.Velocity.z);
			}
		}

		// Token: 0x04003F90 RID: 16272
		[NonSerialized]
		public Rigidbody TrainRb;

		// Token: 0x04003F91 RID: 16273
		[NonSerialized]
		public Rigidbody PlayerRb;

		// Token: 0x04003F92 RID: 16274
		[NonSerialized]
		public Vector3 Velocity;
	}
}
