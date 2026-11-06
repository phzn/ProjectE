using System;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000645 RID: 1605
	public class vArrow : MonoBehaviour
	{
		// Token: 0x060024D7 RID: 9431 RVA: 0x000CF16C File Offset: 0x000CD36C
		private void Start()
		{
			if (!this.projectileControl)
			{
				this.projectileControl = base.GetComponent<vProjectileControl>();
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000CF188 File Offset: 0x000CD388
		public void OnDestroyProjectile(RaycastHit hit)
		{
			Transform transform = hit.transform.Find("ArrowParent");
			if (!transform)
			{
				transform = new GameObject("ArrowParent").transform;
				transform.position = hit.transform.position;
				transform.parent = hit.transform;
			}
			this.detachObject.parent = transform.transform;
			if (this.alignToNormal)
			{
				this.detachObject.rotation = Quaternion.LookRotation(-hit.normal);
			}
			this.detachObject.position = hit.point + base.transform.forward * this.penetration;
			if (this.debugPenetration)
			{
				Debug.DrawLine(hit.point, hit.point + base.transform.forward * this.penetration, Color.red, 10f);
			}
			if (this.projectileControl && this.raycastBackSide && this.penetration > 0f)
			{
				Vector3 vector = hit.point + base.transform.forward * this.penetration;
				Vector3 vector2 = hit.point + base.transform.forward * 0.001f;
				if (Physics.Linecast(vector, vector2, out hit, this.projectileControl.hitLayer))
				{
					this.projectileControl.onCastCollider.Invoke(hit);
				}
			}
		}

		// Token: 0x04002499 RID: 9369
		public vProjectileControl projectileControl;

		// Token: 0x0400249A RID: 9370
		public Transform detachObject;

		// Token: 0x0400249B RID: 9371
		[Tooltip("Use to raycast other side of the penetration")]
		public bool raycastBackSide = true;

		// Token: 0x0400249C RID: 9372
		public bool alignToNormal = true;

		// Token: 0x0400249D RID: 9373
		[HideInInspector]
		public float penetration;

		// Token: 0x0400249E RID: 9374
		public bool debugPenetration;
	}
}
