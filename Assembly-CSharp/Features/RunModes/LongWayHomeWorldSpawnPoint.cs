using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D4B RID: 3403
	[DisallowMultipleComponent]
	public sealed class LongWayHomeWorldSpawnPoint : MonoBehaviour
	{
		// Token: 0x060063F6 RID: 25590 RVA: 0x00281334 File Offset: 0x0027F534
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.2f, 0.85f, 1f, 0.8f);
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.35f, 0.35f);
			Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward * 1.5f);
		}
	}
}
