using System;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x0200066B RID: 1643
	[RequireComponent(typeof(LineRenderer))]
	public class vLaserSight : MonoBehaviour
	{
		// Token: 0x060025E9 RID: 9705 RVA: 0x000D317C File Offset: 0x000D137C
		private void Start()
		{
			this.line = base.GetComponent<LineRenderer>();
			this.ray = default(Ray);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000D3198 File Offset: 0x000D1398
		private void LateUpdate()
		{
			this.ray.origin = base.transform.position;
			this.ray.direction = base.transform.forward.normalized;
			Vector3 zero = Vector3.zero;
			if (Physics.Raycast(this.ray, out this.hit, this.maxDistance, this.layerMask))
			{
				zero.z = base.transform.InverseTransformPoint(this.hit.point).z - this.aimSpriteOffset;
				this.line.SetPosition(1, zero);
				this.aimSprite.transform.rotation = Quaternion.LookRotation(this.hit.normal);
			}
			else
			{
				zero.z = Vector3.Distance(base.transform.position, this.ray.GetPoint(this.maxDistance - this.aimSpriteOffset));
				this.line.SetPosition(1, zero);
				this.aimSprite.transform.localEulerAngles = Vector3.zero;
			}
			this.aimSprite.transform.localPosition = zero;
		}

		// Token: 0x040025A3 RID: 9635
		public LayerMask layerMask;

		// Token: 0x040025A4 RID: 9636
		public GameObject aimSprite;

		// Token: 0x040025A5 RID: 9637
		public float aimSpriteOffset;

		// Token: 0x040025A6 RID: 9638
		public float maxDistance;

		// Token: 0x040025A7 RID: 9639
		private Ray ray;

		// Token: 0x040025A8 RID: 9640
		private RaycastHit hit;

		// Token: 0x040025A9 RID: 9641
		private LineRenderer line;
	}
}
