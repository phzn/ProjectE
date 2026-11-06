using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Utils
{
	// Token: 0x020005FF RID: 1535
	[RequireComponent(typeof(BoxCollider))]
	public class vCheckpointExample : MonoBehaviour
	{
		// Token: 0x06002377 RID: 9079 RVA: 0x000C9A58 File Offset: 0x000C7C58
		private void Start()
		{
			this.gm = base.GetComponentInParent<vGameController>();
			base.GetComponent<BoxCollider>().isTrigger = true;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000C9A74 File Offset: 0x000C7C74
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vHUDController.instance.ShowText("Checkpoint reached!");
				this.gm.spawnPoint = base.gameObject.transform;
				this.onTriggerEnter.Invoke();
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0400238A RID: 9098
		private vGameController gm;

		// Token: 0x0400238B RID: 9099
		public UnityEvent onTriggerEnter;
	}
}
