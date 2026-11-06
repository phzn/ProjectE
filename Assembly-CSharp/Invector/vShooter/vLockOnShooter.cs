using System;
using Invector.vCharacterController;

namespace Invector.vShooter
{
	// Token: 0x02000653 RID: 1619
	[vClassHeader("Shooter Lock-On", true, "icon_v2", false, "")]
	public class vLockOnShooter : vLockOn
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x000D06E1 File Offset: 0x000CE8E1
		protected override void Start()
		{
			base.Start();
			this.shooterMelee = this.tpInput as vShooterMeleeInput;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000D06FC File Offset: 0x000CE8FC
		protected override void UpdateLockOn()
		{
			if (this.shooterMelee == null || this.shooterMelee.shooterManager == null || (this.shooterMelee.shooterManager.useLockOn && this.shooterMelee.shooterManager.rWeapon != null) || (this.shooterMelee.shooterManager.useLockOnMeleeOnly && this.shooterMelee.shooterManager.rWeapon == null))
			{
				base.UpdateLockOn();
				return;
			}
			if (this.isLockingOn && this.shooterMelee.shooterManager.rWeapon != null)
			{
				this.isLockingOn = false;
				this.LockOn(false);
				this.StopLockOn();
				base.aimImage.transform.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000D07D0 File Offset: 0x000CE9D0
		protected override void LockOnInput()
		{
			if (this.tpInput.tpCamera == null || this.tpInput.cc == null)
			{
				return;
			}
			if (this.lockOnInput.GetButtonDown() && !this.tpInput.cc.customAction)
			{
				this.isLockingOn = !this.isLockingOn;
				this.LockOn(this.isLockingOn);
			}
			else if (this.isLockingOn && this.tpInput.tpCamera.lockTarget == null)
			{
				this.isLockingOn = false;
				this.LockOn(false);
			}
			if (this.strafeWhileLockOn && !this.tpInput.cc.locomotionType.Equals(vThirdPersonMotor.LocomotionType.OnlyStrafe))
			{
				if (this.shooterMelee.isAimingByInput || (this.strafeWhileLockOn && this.isLockingOn && this.tpInput.tpCamera.lockTarget != null))
				{
					this.tpInput.cc.lockInStrafe = true;
					return;
				}
				this.tpInput.cc.lockInStrafe = false;
			}
		}

		// Token: 0x040024EE RID: 9454
		protected vShooterMeleeInput shooterMelee;
	}
}
