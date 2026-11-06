using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200075C RID: 1884
	[RequireComponent(typeof(Rigidbody))]
	public class RandomForceOnEnable : MonoBehaviour
	{
		// Token: 0x060030AF RID: 12463 RVA: 0x00112CB0 File Offset: 0x00110EB0
		private void OnEnable()
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.AddForce(new Vector3((float)Random.Range(-500, 500), 0f, (float)Random.Range(-500, 500)));
			component.AddRelativeTorque(new Vector3((float)Random.Range(-30, 30), (float)Random.Range(-30, 30), (float)Random.Range(-30, 30)));
		}
	}
}
