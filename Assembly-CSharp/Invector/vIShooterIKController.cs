using System;
using Invector.IK;
using Invector.vShooter;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004EA RID: 1258
	public interface vIShooterIKController
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001E47 RID: 7751
		GameObject gameObject { get; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001E48 RID: 7752
		vIKSolver LeftIK { get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001E49 RID: 7753
		vIKSolver RightIK { get; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001E4A RID: 7754
		// (set) Token: 0x06001E4B RID: 7755
		vWeaponIKAdjustList WeaponIKAdjustList { get; set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001E4C RID: 7756
		vWeaponIKAdjust CurrentWeaponIK { get; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001E4D RID: 7757
		IKAdjust CurrentIKAdjust { get; }

		// Token: 0x06001E4E RID: 7758
		void UpdateWeaponIK();

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001E4F RID: 7759
		// (set) Token: 0x06001E50 RID: 7760
		bool LockAiming { get; set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001E51 RID: 7761
		// (set) Token: 0x06001E52 RID: 7762
		bool LockHipFireAiming { get; set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001E53 RID: 7763
		vShooterWeapon CurrentActiveWeapon { get; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001E54 RID: 7764
		// (set) Token: 0x06001E55 RID: 7765
		bool EditingIKGlobalOffset { get; set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001E56 RID: 7766
		bool IsAiming { get; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001E57 RID: 7767
		// (set) Token: 0x06001E58 RID: 7768
		bool IsCrouching { get; set; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001E59 RID: 7769
		bool IsLeftWeapon { get; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001E5A RID: 7770
		Vector3 AimPosition { get; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001E5B RID: 7771
		string CurrentIKAdjustState { get; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001E5C RID: 7772
		string CurrentIKAdjustStateWithTag { get; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001E5D RID: 7773
		bool IsUsingCustomIKAdjust { get; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001E5E RID: 7774
		bool IsIgnoreIK { get; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001E5F RID: 7775
		bool IsSupportHandIKEnabled { get; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001E60 RID: 7776
		string CustomIKAdjustState { get; }

		// Token: 0x06001E61 RID: 7777
		void SetCustomIKAdjustState(string value);

		// Token: 0x06001E62 RID: 7778
		void ResetCustomIKAdjustState();

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06001E63 RID: 7779
		// (remove) Token: 0x06001E64 RID: 7780
		event IKUpdateEvent onStartUpdateIK;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06001E65 RID: 7781
		// (remove) Token: 0x06001E66 RID: 7782
		event IKUpdateEvent onFinishUpdateIK;
	}
}
