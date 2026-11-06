using System;
using Features.CielOS.WindowManager;
using UnityEngine;

namespace Features.CielOS.Panel
{
	// Token: 0x020013DD RID: 5085
	public class CielOSPowerHandler : MonoBehaviour
	{
		// Token: 0x06009D04 RID: 40196 RVA: 0x0040722E File Offset: 0x0040542E
		private void OnEnable()
		{
			if (this.panel != null)
			{
				this.panel.PowerPressed += this.OnPowerPressed;
			}
		}

		// Token: 0x06009D05 RID: 40197 RVA: 0x00407255 File Offset: 0x00405455
		private void OnDisable()
		{
			if (this.panel != null)
			{
				this.panel.PowerPressed -= this.OnPowerPressed;
			}
		}

		// Token: 0x06009D06 RID: 40198 RVA: 0x0040727C File Offset: 0x0040547C
		private void OnPowerPressed()
		{
			if (this.windowManager != null)
			{
				this.windowManager.CloseAll();
			}
			CielOSController cielOSController = ((this.controller != null) ? this.controller : CielOSController.Instance);
			if (cielOSController != null)
			{
				cielOSController.Close();
			}
		}

		// Token: 0x0400A183 RID: 41347
		[SerializeField]
		private CielOSPanel panel;

		// Token: 0x0400A184 RID: 41348
		[SerializeField]
		private CielOSController controller;

		// Token: 0x0400A185 RID: 41349
		[SerializeField]
		private CielOSWindowManager windowManager;
	}
}
