using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000616 RID: 1558
	public class vTargetLookAt : MonoBehaviour
	{
		// Token: 0x060023DF RID: 9183 RVA: 0x000CA930 File Offset: 0x000C8B30
		private void Update()
		{
			if (!this.target)
			{
				return;
			}
			Vector3 vector = this.target.position + Vector3.up * this.offsetHeight - base.transform.position;
			Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
			if (!this.limitDistance || vector.magnitude > this.minDistanceToLook)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, this.smooth * Time.deltaTime);
			}
		}

		// Token: 0x040023D3 RID: 9171
		public Transform target;

		// Token: 0x040023D4 RID: 9172
		public float smooth;

		// Token: 0x040023D5 RID: 9173
		public float offsetHeight;

		// Token: 0x040023D6 RID: 9174
		public bool limitDistance;

		// Token: 0x040023D7 RID: 9175
		public float minDistanceToLook;
	}
}
