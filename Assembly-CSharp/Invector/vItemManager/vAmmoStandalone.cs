using System;
using System.Collections;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005F8 RID: 1528
	[vClassHeader("vAmmoStandalone", true, "icon_v2", false, "")]
	public class vAmmoStandalone : vTriggerGenericAction
	{
		// Token: 0x06002356 RID: 9046 RVA: 0x000C9581 File Offset: 0x000C7781
		public override IEnumerator OnPressActionDelay(GameObject cc)
		{
			yield return base.StartCoroutine(base.OnPressActionDelay(cc));
			this.ammoManager = cc.gameObject.GetComponent<vAmmoManager>();
			if (this.ammoManager != null)
			{
				this.ammoManager.AddAmmo(this.weaponName, this.ammoID, this.ammoAmount);
			}
			yield break;
		}

		// Token: 0x0400236F RID: 9071
		[Header("Ammo Standalone Options")]
		[Tooltip("Use the same name as in the AmmoManager")]
		public string weaponName;

		// Token: 0x04002370 RID: 9072
		public int ammoID;

		// Token: 0x04002371 RID: 9073
		public int ammoAmount;

		// Token: 0x04002372 RID: 9074
		private vAmmoManager ammoManager;
	}
}
