using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200048E RID: 1166
	public class vDestroyOnTrigger : MonoBehaviour
	{
		// Token: 0x06001CA0 RID: 7328 RVA: 0x000B16E8 File Offset: 0x000AF8E8
		private void OnTriggerEnter(Collider other)
		{
			if (this.targsToDestroy.Contains(other.gameObject.tag))
			{
				Object.Destroy(other.gameObject, this.destroyDelayTime);
			}
		}

		// Token: 0x04001DEC RID: 7660
		public List<string> targsToDestroy;

		// Token: 0x04001DED RID: 7661
		public float destroyDelayTime;
	}
}
