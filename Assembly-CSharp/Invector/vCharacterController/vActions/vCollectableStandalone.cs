using System;
using System.Collections;
using Invector.vMelee;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000722 RID: 1826
	[vClassHeader("Collectable Standalone", "Use this component when your character doesn't have a ItemManager", openClose = false)]
	public class vCollectableStandalone : vTriggerGenericAction
	{
		// Token: 0x06002F50 RID: 12112 RVA: 0x0010B7B5 File Offset: 0x001099B5
		public override IEnumerator OnPressActionDelay(GameObject cc)
		{
			yield return base.StartCoroutine(base.OnPressActionDelay(cc));
			this.manager = cc.GetComponent<vCollectMeleeControl>();
			if (this.manager != null)
			{
				this.manager.HandleCollectableInput(this);
			}
			yield break;
		}

		// Token: 0x04002FBF RID: 12223
		[vEditorToolbar("Collectable", false, "", false, false)]
		public string targetEquipPoint;

		// Token: 0x04002FC0 RID: 12224
		public bool twoHandWeapon;

		// Token: 0x04002FC1 RID: 12225
		public GameObject weapon;

		// Token: 0x04002FC2 RID: 12226
		public Sprite weaponIcon;

		// Token: 0x04002FC3 RID: 12227
		public string weaponText;

		// Token: 0x04002FC4 RID: 12228
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnEquip;

		// Token: 0x04002FC5 RID: 12229
		public UnityEvent OnDrop;

		// Token: 0x04002FC6 RID: 12230
		private vCollectMeleeControl manager;
	}
}
