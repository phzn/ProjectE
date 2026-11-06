using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200047F RID: 1151
	public class vHealthItem : MonoBehaviour
	{
		// Token: 0x06001C52 RID: 7250 RVA: 0x000B0680 File Offset: 0x000AE880
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(this.tagFilter))
			{
				vHealthController component = other.GetComponent<vHealthController>();
				if (component != null && component.currentHealth < (float)component.maxHealth)
				{
					component.AddHealth((int)this.value);
					Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x04001DA5 RID: 7589
		[Tooltip("How much health will be recovery")]
		public float value;

		// Token: 0x04001DA6 RID: 7590
		public string tagFilter = "Player";
	}
}
