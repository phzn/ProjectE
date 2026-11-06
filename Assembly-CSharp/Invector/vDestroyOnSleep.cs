using System;
using System.Collections;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004DB RID: 1243
	public class vDestroyOnSleep : MonoBehaviour
	{
		// Token: 0x06001DF7 RID: 7671 RVA: 0x000B6E27 File Offset: 0x000B5027
		private IEnumerator Start()
		{
			Rigidbody rigdb = base.GetComponent<Rigidbody>();
			Collider collider = base.GetComponent<Collider>();
			yield return base.transform.parent.gameObject.activeSelf;
			while (!rigdb.IsSleeping())
			{
				yield return new WaitForSeconds(2f);
			}
			Object.Destroy(rigdb);
			if (collider)
			{
				Object.Destroy(collider);
			}
			yield break;
		}
	}
}
