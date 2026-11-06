using System;
using Invector.vCharacterController.vActions;
using Invector.vShooter;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vMelee
{
	// Token: 0x0200051D RID: 1309
	[vClassHeader("Collect Shooter Melee Control", "This component is used when you're character doesn't have a ItemManager to manage items, this will allow you to pickup 1 weapon at the time.")]
	public class vCollectShooterMeleeControl : vCollectMeleeControl
	{
		// Token: 0x06001FAC RID: 8108 RVA: 0x000BC2F7 File Offset: 0x000BA4F7
		protected override void Start()
		{
			base.Start();
			this.shooterManager = base.GetComponent<vShooterManager>();
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000BC30B File Offset: 0x000BA50B
		public override void HandleCollectableInput(vCollectableStandalone collectableStandAlone)
		{
			if (this.shooterManager && collectableStandAlone != null && collectableStandAlone.weapon != null)
			{
				this.EquipShooterWeapon(collectableStandAlone);
			}
			base.HandleCollectableInput(collectableStandAlone);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000BC340 File Offset: 0x000BA540
		protected virtual void EquipShooterWeapon(vCollectableStandalone collectable)
		{
			vShooterWeapon component = collectable.weapon.GetComponent<vShooterWeapon>();
			if (!component)
			{
				return;
			}
			if (component.isLeftWeapon)
			{
				Transform transform = this.GetEquipPoint(this.leftHandler, collectable.targetEquipPoint);
				if (transform)
				{
					collectable.weapon.transform.SetParent(transform);
					collectable.weapon.transform.localPosition = Vector3.zero;
					collectable.weapon.transform.localEulerAngles = Vector3.zero;
					if (this.leftWeapon && this.leftWeapon.gameObject != collectable.gameObject)
					{
						this.RemoveLeftWeapon();
					}
					this.shooterManager.SetLeftWeapon(component.gameObject);
					collectable.OnEquip.Invoke();
					this.leftWeapon = collectable;
					this.UpdateLeftDisplay(collectable);
					if (this.rightWeapon)
					{
						this.RemoveRightWeapon();
						return;
					}
				}
			}
			else
			{
				Transform transform = this.GetEquipPoint(this.rightHandler, collectable.targetEquipPoint);
				if (transform)
				{
					collectable.weapon.transform.SetParent(transform);
					collectable.weapon.transform.localPosition = Vector3.zero;
					collectable.weapon.transform.localEulerAngles = Vector3.zero;
					if (this.rightWeapon && this.rightWeapon.gameObject != collectable.gameObject)
					{
						this.RemoveRightWeapon();
					}
					this.shooterManager.SetRightWeapon(component.gameObject);
					collectable.OnEquip.Invoke();
					this.rightWeapon = collectable;
					this.UpdateRightDisplay(collectable);
					if (this.leftWeapon)
					{
						this.RemoveLeftWeapon();
					}
				}
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000BC4F5 File Offset: 0x000BA6F5
		public override void RemoveRightWeapon()
		{
			base.RemoveRightWeapon();
			if (this.shooterManager)
			{
				this.shooterManager.rWeapon = null;
			}
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000BC516 File Offset: 0x000BA716
		public override void RemoveLeftWeapon()
		{
			base.RemoveLeftWeapon();
			if (this.shooterManager)
			{
				this.shooterManager.lWeapon = null;
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000BC538 File Offset: 0x000BA738
		protected override void CheckIsEquipedWifhWeapon()
		{
			if (!this.wasUsingShooterWeapon && this.isUsingShooterWeapon)
			{
				this.onUnequipMeleeWeapon.Invoke();
				this.wasUsingMeleeWeapon = false;
				this.onEquipShooterWeapon.Invoke();
				this.wasUsingShooterWeapon = true;
			}
			else if (this.wasUsingShooterWeapon && !this.isUsingShooterWeapon)
			{
				this.onUnequipShooterWeapon.Invoke();
				this.wasUsingShooterWeapon = false;
			}
			if (!this.wasUsingShooterWeapon)
			{
				base.CheckIsEquipedWifhWeapon();
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x000BC5AB File Offset: 0x000BA7AB
		public virtual bool isUsingShooterWeapon
		{
			get
			{
				return this.shooterManager && this.shooterManager.CurrentActiveWeapon;
			}
		}

		// Token: 0x04002072 RID: 8306
		protected vShooterManager shooterManager;

		// Token: 0x04002073 RID: 8307
		[vEditorToolbar("Shooter Events", false, "", false, false)]
		public UnityEvent onEquipShooterWeapon;

		// Token: 0x04002074 RID: 8308
		[vEditorToolbar("Shooter Events", false, "", false, false)]
		public UnityEvent onUnequipShooterWeapon;

		// Token: 0x04002075 RID: 8309
		internal bool wasUsingShooterWeapon;
	}
}
