using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C46 RID: 3142
	[DisallowMultipleComponent]
	public sealed class StadiumParachuteLaunchPoint : MonoBehaviour
	{
		// Token: 0x06005ABD RID: 23229 RVA: 0x00238B44 File Offset: 0x00236D44
		public bool ContainsApproach(Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(position);
			return base.isActiveAndEnabled && Mathf.Abs(vector.x) <= this.width * 0.5f && vector.z >= -this.approachDepth && vector.z <= 0.65f && Mathf.Abs(vector.y) <= 0.35f;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x00238BB4 File Offset: 0x00236DB4
		public bool ContainsDeparture(Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(position);
			return base.isActiveAndEnabled && vector.z >= 0f && vector.x * vector.x + vector.z * vector.z <= this.departureDistance * this.departureDistance && vector.y <= 0.35f && vector.y >= -this.maximumDrop;
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x00238C30 File Offset: 0x00236E30
		public bool CrossesOpening(Vector3 previous, Vector3 position)
		{
			Vector3 vector = base.transform.InverseTransformPoint(previous);
			Vector3 vector2 = base.transform.InverseTransformPoint(position);
			if (!base.isActiveAndEnabled || vector.z > 0f || vector2.z < 0f || vector2.z <= vector.z)
			{
				return false;
			}
			Vector3 vector3 = Vector3.Lerp(vector, vector2, -vector.z / (vector2.z - vector.z));
			return Mathf.Abs(vector3.x) <= this.width * 0.5f && vector3.y <= 0.35f;
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x00238CD4 File Offset: 0x00236ED4
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(new Vector3(0f, 0.9f, -this.approachDepth * 0.5f), new Vector3(this.width, 1.8f, this.approachDepth));
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(Vector3.zero, Vector3.forward * this.departureDistance);
		}

		// Token: 0x04005BB2 RID: 23474
		[Min(0.5f)]
		public float width = 1.2f;

		// Token: 0x04005BB3 RID: 23475
		[Min(0.5f)]
		public float approachDepth = 2.5f;

		// Token: 0x04005BB4 RID: 23476
		[Min(1f)]
		public float departureDistance = 10f;

		// Token: 0x04005BB5 RID: 23477
		[Min(1f)]
		public float maximumDrop = 10f;

		// Token: 0x04005BB6 RID: 23478
		public StadiumParachuteGate gate;
	}
}
