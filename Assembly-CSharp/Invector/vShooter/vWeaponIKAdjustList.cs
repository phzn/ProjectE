using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000664 RID: 1636
	[CreateAssetMenu(menuName = "Invector/Shooter/New Weapon IK Adjust List")]
	public class vWeaponIKAdjustList : ScriptableObject
	{
		// Token: 0x060025D1 RID: 9681 RVA: 0x000D2BBC File Offset: 0x000D0DBC
		public vWeaponIKAdjust GetWeaponIK(string category)
		{
			if (this.weaponIKAdjusts == null)
			{
				return null;
			}
			return this.weaponIKAdjusts.Find((vWeaponIKAdjust ik) => ik != null && ik.weaponCategories.Contains(category));
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000D2BF8 File Offset: 0x000D0DF8
		public void ReplaceWeaponIKAdjust(vWeaponIKAdjust currentIK, vWeaponIKAdjust newIK)
		{
			if (this.weaponIKAdjusts != null && this.weaponIKAdjusts.Contains(currentIK))
			{
				int num = this.IndexOfIK(currentIK);
				this.weaponIKAdjusts[num] = newIK;
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000D2C30 File Offset: 0x000D0E30
		public int IndexOfIK(vWeaponIKAdjust currentIK)
		{
			if (this.weaponIKAdjusts != null && this.weaponIKAdjusts.Contains(currentIK))
			{
				return this.weaponIKAdjusts.IndexOf(currentIK);
			}
			return -1;
		}

		// Token: 0x04002586 RID: 9606
		[vSeparator("Global Offsets for all Weapons Hand IK target", "")]
		public Vector3 ikTargetPositionOffsetR;

		// Token: 0x04002587 RID: 9607
		public Vector3 ikTargetRotationOffsetR;

		// Token: 0x04002588 RID: 9608
		public Vector3 ikTargetPositionOffsetL;

		// Token: 0x04002589 RID: 9609
		public Vector3 ikTargetRotationOffsetL;

		// Token: 0x0400258A RID: 9610
		[vSeparator("Offsets for specific Weapon categories", "")]
		public List<vWeaponIKAdjust> weaponIKAdjusts = new List<vWeaponIKAdjust>();
	}
}
