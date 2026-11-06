using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004B0 RID: 1200
	public class vRotateObject : MonoBehaviour
	{
		// Token: 0x06001D3B RID: 7483 RVA: 0x000B3AC8 File Offset: 0x000B1CC8
		private void Update()
		{
			base.transform.Rotate(this.rotationSpeed * Time.deltaTime, Space.Self);
		}

		// Token: 0x04001E3F RID: 7743
		public Vector3 rotationSpeed;
	}
}
