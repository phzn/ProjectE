using System;
using Invector.vShooter;

// Token: 0x020000EF RID: 239
public class vAutoShotWeapon : vShooterWeaponBase
{
	// Token: 0x0600094B RID: 2379 RVA: 0x0004384E File Offset: 0x00041A4E
	protected virtual void Update()
	{
		if (base.enabled)
		{
			this.Shoot();
		}
	}
}
