using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000445 RID: 1093
	public class vUnderWaterTrigger : MonoBehaviour
	{
		// Token: 0x06001B73 RID: 7027 RVA: 0x000ACF2D File Offset: 0x000AB12D
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Water"))
			{
				this.waterEffect.SetActive(true);
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000ACF4D File Offset: 0x000AB14D
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Water"))
			{
				this.waterEffect.SetActive(false);
			}
		}

		// Token: 0x04001CA3 RID: 7331
		public GameObject waterEffect;
	}
}
