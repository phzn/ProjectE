using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000758 RID: 1880
	public class MudSplatOnImpact : MonoBehaviour
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x00112800 File Offset: 0x00110A00
		public void OnCollisionEnter(Collision collision)
		{
			Vector3 point = collision.contacts[0].point;
			Vector3 normal = collision.contacts[0].normal;
			normal.Normalize();
			this.MudClump.SetActive(false);
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.isKinematic = true;
			component.useGravity = false;
			base.GetComponent<Collider>().enabled = false;
			this.Decal.SetActive(true);
			this.SetDecalMaterial(this.DecalMaterial);
			base.transform.position = point + normal;
			base.transform.LookAt(point);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x0011289C File Offset: 0x00110A9C
		public void SetDecalMaterial(Material material)
		{
			this.DecalMaterial = material;
			DecalProjector decalProjector;
			if (this.Decal.TryGetComponent<DecalProjector>(out decalProjector))
			{
				decalProjector.material = material;
			}
		}

		// Token: 0x04003115 RID: 12565
		public GameObject MudClump;

		// Token: 0x04003116 RID: 12566
		public GameObject Decal;

		// Token: 0x04003117 RID: 12567
		public Material DecalMaterial;
	}
}
