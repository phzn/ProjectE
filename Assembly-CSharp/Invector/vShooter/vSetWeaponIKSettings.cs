using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x0200065B RID: 1627
	[vClassHeader("Set Weapon IK Settings", true, "icon_v2", false, "", openClose = false)]
	public class vSetWeaponIKSettings : vMonoBehaviour
	{
		// Token: 0x060025AB RID: 9643 RVA: 0x000D2344 File Offset: 0x000D0544
		private void Start()
		{
			if (this.setOnStart)
			{
				this.SetSettings(this.indexOfSetting);
			}
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000D235C File Offset: 0x000D055C
		private void CopyDefaultIK()
		{
			if (!this.weapon)
			{
				return;
			}
			if (this.defaultIsCreated)
			{
				return;
			}
			this.defaultIKSettings.freeIKOptions = this.weapon.freeIKOptions.Copy();
			this.defaultIKSettings.strafeIKOptions = this.weapon.freeIKOptions.Copy();
			this.defaultIKSettings.useIkAttacking = this.weapon.useIkAttacking;
			this.defaultIKSettings.useIKOnAiming = this.weapon.useIKOnAiming;
			this.defaultIKSettings.alignRightHandToAim = this.weapon.alignRightHandToAim;
			this.defaultIKSettings.alignRightUpperArmToAim = this.weapon.alignRightUpperArmToAim;
			this.defaultIKSettings.raycastAimTarget = this.weapon.raycastAimTarget;
			this.defaultIsCreated = true;
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000D242B File Offset: 0x000D062B
		public void ResetSettings()
		{
			if (this.defaultIsCreated && this.weapon)
			{
				this.ApplySettings(this.defaultIKSettings);
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000D2450 File Offset: 0x000D0650
		public void SetSettings(int index)
		{
			if (this.getWeaponOnSet)
			{
				vShooterWeapon componentInChildren = base.GetComponentInChildren<vShooterWeapon>();
				if (this.weapon != componentInChildren)
				{
					this.defaultIsCreated = false;
				}
			}
			if (!this.weapon)
			{
				return;
			}
			this.CopyDefaultIK();
			if (this.settings.Count > 0 && index >= 0 && index < this.settings.Count)
			{
				vSetWeaponIKSettings.IKSettings iksettings = this.settings[index];
				this.ApplySettings(iksettings);
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000D24CC File Offset: 0x000D06CC
		public void SetSettings(string name)
		{
			if (this.getWeaponOnSet)
			{
				vShooterWeapon componentInChildren = base.GetComponentInChildren<vShooterWeapon>();
				if (this.weapon != componentInChildren)
				{
					this.defaultIsCreated = false;
				}
			}
			if (!this.weapon)
			{
				return;
			}
			this.CopyDefaultIK();
			if (this.settings.Count > 0)
			{
				vSetWeaponIKSettings.IKSettings iksettings = this.settings.Find((vSetWeaponIKSettings.IKSettings s) => s.name.Equals(name));
				this.ApplySettings(iksettings);
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000D254C File Offset: 0x000D074C
		private void ApplySettings(vSetWeaponIKSettings.IKSettings settings)
		{
			if (settings == null)
			{
				return;
			}
			this.weapon.alignRightHandToAim = settings.alignRightHandToAim;
			this.weapon.alignRightUpperArmToAim = settings.alignRightUpperArmToAim;
			this.weapon.raycastAimTarget = settings.raycastAimTarget;
			this.weapon.useIkAttacking = settings.useIkAttacking;
			this.weapon.useIKOnAiming = settings.useIKOnAiming;
			this.weapon.freeIKOptions = settings.freeIKOptions;
			this.weapon.strafeIKOptions = settings.strafeIKOptions;
		}

		// Token: 0x04002560 RID: 9568
		public List<vSetWeaponIKSettings.IKSettings> settings;

		// Token: 0x04002561 RID: 9569
		[vHelpBox("It's recommended to attach this component in a Handler", vHelpBoxAttribute.MessageType.None)]
		[Tooltip("Auto get shooter weapon when set settings")]
		public bool getWeaponOnSet = true;

		// Token: 0x04002562 RID: 9570
		[vHideInInspector("getWeaponOnSet", false, invertValue = true)]
		public vShooterWeapon weapon;

		// Token: 0x04002563 RID: 9571
		public bool setOnStart;

		// Token: 0x04002564 RID: 9572
		[vHideInInspector("setOnStart", false)]
		public int indexOfSetting;

		// Token: 0x04002565 RID: 9573
		public vSetWeaponIKSettings.IKSettings defaultIKSettings;

		// Token: 0x04002566 RID: 9574
		private bool defaultIsCreated;

		// Token: 0x0200065C RID: 1628
		[Serializable]
		public class IKSettings
		{
			// Token: 0x04002567 RID: 9575
			public string name;

			// Token: 0x04002568 RID: 9576
			[Tooltip("IK will help the right hand to align where you actually is aiming")]
			public bool alignRightHandToAim = true;

			// Token: 0x04002569 RID: 9577
			[Tooltip("IK will help the right hand to align where you actually is aiming")]
			public bool alignRightUpperArmToAim = true;

			// Token: 0x0400256A RID: 9578
			public bool raycastAimTarget = true;

			// Token: 0x0400256B RID: 9579
			public vShooterWeapon.IKLocomotionOptions strafeIKOptions;

			// Token: 0x0400256C RID: 9580
			public vShooterWeapon.IKLocomotionOptions freeIKOptions;

			// Token: 0x0400256D RID: 9581
			[Tooltip("Left IK while attacking")]
			public bool useIkAttacking;

			// Token: 0x0400256E RID: 9582
			[Tooltip("Left IK while Shot")]
			public bool disableIkOnShot;

			// Token: 0x0400256F RID: 9583
			[Tooltip("Left IK while Aming")]
			public bool useIKOnAiming = true;
		}
	}
}
