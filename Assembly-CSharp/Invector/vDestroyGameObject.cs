using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x02000494 RID: 1172
	[vClassHeader("Destroy GameObject", true, "icon_v2", false, "", openClose = false)]
	public class vDestroyGameObject : vMonoBehaviour
	{
		// Token: 0x06001CBD RID: 7357 RVA: 0x000B1F5A File Offset: 0x000B015A
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(this.delay);
			this.onDestroy.Invoke();
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x04001E01 RID: 7681
		public float delay;

		// Token: 0x04001E02 RID: 7682
		public UnityEvent onDestroy;
	}
}
