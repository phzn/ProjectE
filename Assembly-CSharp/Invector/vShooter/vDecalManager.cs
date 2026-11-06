using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000648 RID: 1608
	[vClassHeader("Decal Manager", true, "icon_v2", false, "", openClose = false)]
	public class vDecalManager : vMonoBehaviour
	{
		// Token: 0x060024E8 RID: 9448 RVA: 0x000CF563 File Offset: 0x000CD763
		public virtual void CreateDecal(RaycastHit hitInfo)
		{
			this.CreateDecal(hitInfo.collider.gameObject, hitInfo.point, hitInfo.normal);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000CF588 File Offset: 0x000CD788
		public virtual void CreateDecal(GameObject target, Vector3 position, Vector3 normal)
		{
			if (this.layermask == (this.layermask | (1 << target.layer)))
			{
				vDecalManager.DecalObject decal = this.GetDecal(target.tag);
				RaycastHit raycastHit;
				if (decal != null && Physics.SphereCast(new Ray(position + normal * 0.1f, -normal), 0.0001f, out raycastHit, 1f, this.layermask) && raycastHit.collider.gameObject == target)
				{
					Quaternion quaternion = Quaternion.LookRotation(raycastHit.normal, Vector3.up);
					Vector3 point = raycastHit.point;
					decal.CreateEffect(point, quaternion, base.gameObject, target);
				}
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000CF644 File Offset: 0x000CD844
		protected virtual vDecalManager.DecalObject GetDecal(string tag)
		{
			return this.decalObjects.Find((vDecalManager.DecalObject d) => d.tag.Equals(tag));
		}

		// Token: 0x040024AA RID: 9386
		public LayerMask layermask;

		// Token: 0x040024AB RID: 9387
		public List<vDecalManager.DecalObject> decalObjects;

		// Token: 0x02000649 RID: 1609
		[Serializable]
		public class DecalObject
		{
			// Token: 0x060024EC RID: 9452 RVA: 0x000CF678 File Offset: 0x000CD878
			public void CreateEffect(Vector3 position, Quaternion rotation, GameObject impactSender, GameObject impactReceiver)
			{
				this.impactEffect.DoImpactEffect(position, rotation, impactSender, impactReceiver);
				for (int i = 0; i < this.additionalEffects.Count; i++)
				{
					this.additionalEffects[i].DoImpactEffect(position, rotation, impactSender, impactReceiver);
				}
			}

			// Token: 0x040024AC RID: 9388
			public string tag;

			// Token: 0x040024AD RID: 9389
			[SerializeField]
			protected vImpactEffectBase impactEffect;

			// Token: 0x040024AE RID: 9390
			[SerializeField]
			protected List<vImpactEffectBase> additionalEffects;
		}
	}
}
