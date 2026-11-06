using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x0200066A RID: 1642
	[vClassHeader("Draw/Hide Shooter Melee Weapons", "This component works with vItemManager, vWeaponHolderManager and vShooterMeleeInput", useHelpBox = true)]
	public class vDrawHideShooterWeapons : vDrawHideMeleeWeapons
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x000D2DFE File Offset: 0x000D0FFE
		// (set) Token: 0x060025DE RID: 9694 RVA: 0x000D2E06 File Offset: 0x000D1006
		public virtual vShooterMeleeInput shooter { get; set; }

		// Token: 0x060025DF RID: 9695 RVA: 0x000D2E0F File Offset: 0x000D100F
		protected override void Start()
		{
			base.Start();
			this.shooter = base.GetComponent<vShooterMeleeInput>();
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000D2E24 File Offset: 0x000D1024
		protected override bool CanHideWeapons()
		{
			return (this.shooter && this.shooter.shooterManager && this.shooter.shooterManager.CurrentWeapon && (this.forceHide || (!this.shooter.IsAiming && !this.shooter.isReloading))) || (base.CanHideWeapons() && (this.forceHide || (!this.shooter.IsAiming && !this.shooter.isReloading)));
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000D2EBC File Offset: 0x000D10BC
		protected override bool CanDrawWeapons()
		{
			return (!this.forceHide && this.shooter && this.shooter.shooterManager && this.shooter.shooterManager.CurrentWeapon && !this.shooter.shooterManager.CurrentWeapon.gameObject.activeInHierarchy) || base.CanDrawWeapons();
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000D2F2C File Offset: 0x000D112C
		protected override GameObject RightWeaponObject(bool checkIsActve = false)
		{
			if (this.shooter && this.shooter.shooterManager && this.shooter.shooterManager.rWeapon && (!checkIsActve || this.shooter.shooterManager.rWeapon.gameObject.activeInHierarchy))
			{
				return this.shooter.shooterManager.rWeapon.gameObject;
			}
			return base.RightWeaponObject(checkIsActve);
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000D2FAC File Offset: 0x000D11AC
		protected override GameObject LeftWeaponObject(bool checkIsActve = false)
		{
			if (this.shooter && this.shooter.shooterManager && this.shooter.shooterManager.lWeapon && (!checkIsActve || this.shooter.shooterManager.lWeapon.gameObject.activeInHierarchy))
			{
				return this.shooter.shooterManager.lWeapon.gameObject;
			}
			return base.LeftWeaponObject(checkIsActve);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000D302B File Offset: 0x000D122B
		protected override void DrawRightWeapon(bool immediate = false)
		{
			base.DrawRightWeapon(immediate);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000D3034 File Offset: 0x000D1234
		protected override bool DrawWeaponsImmediateConditions()
		{
			if (this.shooter && this.shooter.shooterManager && this.shooter.shooterManager.CurrentWeapon)
			{
				return this.DrawShooterWeaponImmediateConditions();
			}
			return base.DrawWeaponsImmediateConditions();
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000D3084 File Offset: 0x000D1284
		protected virtual bool DrawShooterWeaponImmediateConditions()
		{
			return this.shooter && this.shooter.shooterManager && !this.shooter.cc.customAction && this.shooter.shooterManager.CurrentWeapon && !this.shooter.lockInput && (this.shooter.CurrentActiveWeapon == null && ((this.shooter.aimInput.GetButtonDown() && this.aim) || (this.shooter.shooterManager.hipfireShot && this.shooter.shotInput.GetButtonDown() && this.hipFire) || (this.shooter.shotInput.GetButtonDown() && this.shoot)));
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000D315E File Offset: 0x000D135E
		protected override void HandleInput()
		{
			base.HandleInput();
		}

		// Token: 0x040025A0 RID: 9632
		[vEditorToolbar("Shooter", false, "", false, false)]
		[Header("Draw Immediate Conditions")]
		public bool shoot;

		// Token: 0x040025A1 RID: 9633
		public bool aim = true;

		// Token: 0x040025A2 RID: 9634
		public bool hipFire = true;
	}
}
