using System;
using UnityEngine;

namespace AutoLOD.MeshDecimator
{
	// Token: 0x0200153F RID: 5439
	public class GlobalSmoothRotation : MonoBehaviour
	{
		// Token: 0x0600A869 RID: 43113 RVA: 0x00464798 File Offset: 0x00462998
		private void Update()
		{
			base.transform.Rotate(new Vector3(0f, 10f * Time.deltaTime, 0f), Space.World);
		}
	}
}
