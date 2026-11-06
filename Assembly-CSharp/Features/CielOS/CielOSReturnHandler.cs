using System;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x0200138E RID: 5006
	public class CielOSReturnHandler : MonoBehaviour
	{
		// Token: 0x06009AF8 RID: 39672 RVA: 0x003F65A4 File Offset: 0x003F47A4
		private void Start()
		{
			if (!CielOSSession.ConsumeReopenRequest())
			{
				return;
			}
			CielOSController cielOSController = ((this.controller != null) ? this.controller : CielOSController.Instance);
			if (cielOSController != null)
			{
				cielOSController.Open();
			}
		}

		// Token: 0x04009FEC RID: 40940
		[Tooltip("Controller to reopen. If null, uses CielOSController.Instance.")]
		[SerializeField]
		private CielOSController controller;
	}
}
