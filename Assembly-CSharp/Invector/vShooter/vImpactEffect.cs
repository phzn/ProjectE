using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000651 RID: 1617
	[CreateAssetMenu(menuName = "Invector/Shooter/Impact Effects/New ImpactEffect", fileName = "ImpactEffect@")]
	public class vImpactEffect : vImpactEffectBase
	{
		// Token: 0x0600252E RID: 9518 RVA: 0x000D05C8 File Offset: 0x000CE7C8
		protected virtual GameObject GetRandomObject(List<GameObject> referenceList)
		{
			if (referenceList.Count > 1)
			{
				int num = Random.Range(0, referenceList.Count);
				return referenceList[num];
			}
			if (referenceList.Count == 1)
			{
				return referenceList[0];
			}
			return null;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000D0605 File Offset: 0x000CE805
		protected virtual GameObject CreateDecal(Vector3 position, Quaternion rotation)
		{
			return this.CreateInstance(this.GetRandomObject(this.decals), position, rotation);
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000D061B File Offset: 0x000CE81B
		protected virtual GameObject CreateHitEffect(Vector3 position, Quaternion rotation)
		{
			return this.CreateInstance(this.GetRandomObject(this.hitEffects), position, rotation);
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000D0631 File Offset: 0x000CE831
		protected GameObject CreateInstance(GameObject target, Vector3 position, Quaternion rotation)
		{
			if (target == null)
			{
				return null;
			}
			return Object.Instantiate<GameObject>(target, position, rotation);
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000D0648 File Offset: 0x000CE848
		public override void DoImpactEffect(Vector3 position, Quaternion rotation, GameObject sender, GameObject receiver)
		{
			GameObject gameObject = this.CreateInstance(this.GetRandomObject(this.decals), position, rotation);
			gameObject.transform.Rotate(Vector3.forward, (float)Random.Range(0, 360), Space.Self);
			GameObject gameObject2 = this.CreateInstance(this.GetRandomObject(this.hitEffects), position, rotation);
			if (gameObject && receiver)
			{
				gameObject.transform.SetParent(receiver.transform, true);
			}
			if (gameObject2)
			{
				gameObject2.transform.SetParent(vObjectContainer.root, true);
			}
		}

		// Token: 0x040024EC RID: 9452
		public List<GameObject> decals;

		// Token: 0x040024ED RID: 9453
		public List<GameObject> hitEffects;
	}
}
