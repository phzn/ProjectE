using System;
using System.Collections.Generic;
using Invector.IK;
using UnityEngine;
using UnityEngine.Serialization;

namespace Invector.vShooter
{
	// Token: 0x0200065E RID: 1630
	[CreateAssetMenu(menuName = "Invector/Shooter/New Weapon IK Adjust")]
	public class vWeaponIKAdjust : ScriptableObject
	{
		// Token: 0x060025B5 RID: 9653 RVA: 0x000D261A File Offset: 0x000D081A
		public void Awake()
		{
			this.AddDefaultStates();
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000D2624 File Offset: 0x000D0824
		public bool HasAllDefaultStates()
		{
			int i;
			int j;
			for (i = 0; i < vWeaponIKAdjust.defaultNames.Length; i = j + 1)
			{
				if (!this.ikAdjustsLeft.Exists((IKAdjust a) => a.name.Equals(vWeaponIKAdjust.defaultNames[i])))
				{
					return false;
				}
				if (!this.ikAdjustsRight.Exists((IKAdjust a) => a.name.Equals(vWeaponIKAdjust.defaultNames[i])))
				{
					return false;
				}
				j = i;
			}
			return true;
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000D2698 File Offset: 0x000D0898
		[ContextMenu("Add Default States")]
		public virtual void AddDefaultStates()
		{
			this.ApplyCorretlyName();
			this.AddIKAdjust(this.standingRight.Copy(), false);
			this.AddIKAdjust(this.standingAimingRight.Copy(), false);
			this.AddIKAdjust(this.crouchingRight.Copy(), false);
			this.AddIKAdjust(this.crouchingAimingRight.Copy(), false);
			this.AddIKAdjust(this.standingLeft.Copy(), true);
			this.AddIKAdjust(this.standingAimingLeft.Copy(), true);
			this.AddIKAdjust(this.crouchingLeft.Copy(), true);
			this.AddIKAdjust(this.crouchingAimingLeft.Copy(), true);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000D273C File Offset: 0x000D093C
		public virtual void AddIKAdjust(string name, bool isLeftWeapon = false)
		{
			List<IKAdjust> list = (isLeftWeapon ? this.ikAdjustsLeft : this.ikAdjustsRight);
			if (!list.Exists((IKAdjust a) => a.name.Equals(name)))
			{
				list.Add(new IKAdjust(name));
			}
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000D2790 File Offset: 0x000D0990
		public virtual void AddIKAdjust(IKAdjust adjust, bool isLeftWeapon = false)
		{
			if (adjust == null)
			{
				return;
			}
			List<IKAdjust> list = (isLeftWeapon ? this.ikAdjustsLeft : this.ikAdjustsRight);
			if (!list.Exists((IKAdjust a) => a.name.Equals(adjust.name)))
			{
				list.Add(adjust);
			}
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x000D27E8 File Offset: 0x000D09E8
		public virtual IKAdjust CreateIKAdjust(string name, bool isLeftWeapon = false)
		{
			List<IKAdjust> list = (isLeftWeapon ? this.ikAdjustsLeft : this.ikAdjustsRight);
			if (!list.Exists((IKAdjust a) => a.name.Equals(name)))
			{
				IKAdjust ikadjust = new IKAdjust(name);
				list.Add(ikadjust);
				return ikadjust;
			}
			return this.GetIKAdjust(name, isLeftWeapon);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000D284C File Offset: 0x000D0A4C
		public virtual string GetDefaultStateName(vIShooterIKController controller)
		{
			bool isAiming = controller.IsAiming;
			bool isCrouching = controller.IsCrouching;
			if (!isAiming)
			{
				if (!isCrouching)
				{
					return "Standing";
				}
				return "Crouching";
			}
			else
			{
				if (!isCrouching)
				{
					return "StandingAiming";
				}
				return "CrouchingAiming";
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000D2888 File Offset: 0x000D0A88
		public virtual IKAdjust GetIKAdjust(bool isAming, bool isCrouching, bool isLeftWeapon)
		{
			if (isAming)
			{
				if (isCrouching)
				{
					if (!isLeftWeapon)
					{
						return this.crouchingAimingRight;
					}
					return this.crouchingAimingLeft;
				}
				else
				{
					if (!isLeftWeapon)
					{
						return this.standingAimingRight;
					}
					return this.standingAimingLeft;
				}
			}
			else if (isCrouching)
			{
				if (!isLeftWeapon)
				{
					return this.crouchingRight;
				}
				return this.crouchingLeft;
			}
			else
			{
				if (!isLeftWeapon)
				{
					return this.standingRight;
				}
				return this.standingLeft;
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000D28E1 File Offset: 0x000D0AE1
		[ContextMenu("Reset Standing")]
		public virtual void ResetStanding()
		{
			this.standingLeft = new IKAdjust("StandingLeft");
			this.standingRight = new IKAdjust("StandingRight");
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000D2903 File Offset: 0x000D0B03
		[ContextMenu("Reset Standing Aiming")]
		public virtual void ResetStandingAiming()
		{
			this.standingAimingLeft = new IKAdjust("StandingAimingLeft");
			this.standingAimingRight = new IKAdjust("StandingAimingRight");
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000D2925 File Offset: 0x000D0B25
		[ContextMenu("Reset Crouching")]
		public virtual void ResetCrouching()
		{
			this.crouchingLeft = new IKAdjust("CrouchingLeft");
			this.crouchingRight = new IKAdjust("CrouchingRight");
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000D2947 File Offset: 0x000D0B47
		[ContextMenu("Reset Crouching Aiming")]
		public virtual void ResetCrouchingAiming()
		{
			this.crouchingAimingLeft = new IKAdjust("CrouchingAimingLeft");
			this.crouchingAimingRight = new IKAdjust("CrouchingAimingRight");
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000D296C File Offset: 0x000D0B6C
		[ContextMenu("Reset Default Adjust Names")]
		public virtual void ApplyCorretlyName()
		{
			this.standingRight.name = "Standing";
			this.standingAimingRight.name = "StandingAiming";
			this.standingLeft.name = "Standing";
			this.standingAimingLeft.name = "StandingAiming";
			this.crouchingRight.name = "Crouching";
			this.crouchingAimingRight.name = "CrouchingAiming";
			this.crouchingLeft.name = "Crouching";
			this.crouchingAimingLeft.name = "CrouchingAiming";
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000D29F9 File Offset: 0x000D0BF9
		[ContextMenu("Reset ALL")]
		public virtual void Reset()
		{
			this.ResetStanding();
			this.ResetStandingAiming();
			this.ResetCrouching();
			this.ResetCrouchingAiming();
			this.ApplyCorretlyName();
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000D2A1C File Offset: 0x000D0C1C
		public virtual IKAdjust GetIKAdjust(string name, bool isLeftWeapon)
		{
			return (isLeftWeapon ? this.ikAdjustsLeft : this.ikAdjustsRight).Find((IKAdjust ik) => ik.name.Equals(name));
		}

		// Token: 0x04002571 RID: 9585
		public const string StandingState = "Standing";

		// Token: 0x04002572 RID: 9586
		public const string StandingAimingState = "StandingAiming";

		// Token: 0x04002573 RID: 9587
		public const string CrouchingState = "Crouching";

		// Token: 0x04002574 RID: 9588
		public const string CrouchingAimingState = "CrouchingAiming";

		// Token: 0x04002575 RID: 9589
		public static string[] defaultNames = new string[] { "Standing", "StandingAiming", "Crouching", "CrouchingAiming" };

		// Token: 0x04002576 RID: 9590
		public List<string> weaponCategories = new List<string> { "HandGun", "Pistol" };

		// Token: 0x04002577 RID: 9591
		public List<IKAdjust> ikAdjustsLeft = new List<IKAdjust>();

		// Token: 0x04002578 RID: 9592
		public List<IKAdjust> ikAdjustsRight = new List<IKAdjust>();

		// Token: 0x04002579 RID: 9593
		[vSeparator("<color=yellow><size=15>The fields below will be removed in the future.</size></color>\n<color=green>The old states settings will be automatically added to IKAdjustsLeft and IKAdjustsRight</color>\n<color=white><size=10> If for some reason the default States is not present in the lists, Right Click in this Inspector Header and click in Add Default States</size></color>", "")]
		[FormerlySerializedAs("standing")]
		public IKAdjust standingRight = new IKAdjust("StandingRight");

		// Token: 0x0400257A RID: 9594
		[FormerlySerializedAs("standingAiming")]
		public IKAdjust standingAimingRight = new IKAdjust("StandingAimingRight");

		// Token: 0x0400257B RID: 9595
		public IKAdjust standingLeft = new IKAdjust("StandingLeft");

		// Token: 0x0400257C RID: 9596
		public IKAdjust standingAimingLeft = new IKAdjust("StandingAimingLeft");

		// Token: 0x0400257D RID: 9597
		[FormerlySerializedAs("crouching")]
		public IKAdjust crouchingRight = new IKAdjust("CrouchingRight");

		// Token: 0x0400257E RID: 9598
		[FormerlySerializedAs("crouchingAiming")]
		public IKAdjust crouchingAimingRight = new IKAdjust("CrouchingAimingRight");

		// Token: 0x0400257F RID: 9599
		public IKAdjust crouchingLeft = new IKAdjust("CrouchingLeft");

		// Token: 0x04002580 RID: 9600
		public IKAdjust crouchingAimingLeft = new IKAdjust("CrouchingAimingLeft");
	}
}
