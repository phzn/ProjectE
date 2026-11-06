using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x02000642 RID: 1602
	[vClassHeader("PowerCharge Projectile", true, "icon_v2", false, "", openClose = false)]
	public class vPowerChargeProjectileControl : vMonoBehaviour
	{
		// Token: 0x060024D1 RID: 9425 RVA: 0x000CF086 File Offset: 0x000CD286
		private void Start()
		{
			this.weapon = base.GetComponent<vShooterWeapon>();
			if (this.weapon)
			{
				this.weapon.onPowerChargerChanged.AddListener(new UnityAction<float>(this.OnChangerPower));
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000CF0C0 File Offset: 0x000CD2C0
		public void OnChangerPower(float value)
		{
			if (value <= 0f)
			{
				return;
			}
			if (this.weapon)
			{
				vPowerChargeProjectileControl.vProjectilePerPower vProjectilePerPower = this.projectiles.Find((vPowerChargeProjectileControl.vProjectilePerPower projectile) => value >= projectile.min && value <= projectile.max);
				if (vProjectilePerPower != null && vProjectilePerPower.projectile && this.lastProjectilePerPower != vProjectilePerPower)
				{
					this.lastProjectilePerPower = vProjectilePerPower;
					this.weapon.projectile = vProjectilePerPower.projectile;
					vProjectilePerPower.OnValidatePower.Invoke();
				}
			}
		}

		// Token: 0x04002491 RID: 9361
		public List<vPowerChargeProjectileControl.vProjectilePerPower> projectiles;

		// Token: 0x04002492 RID: 9362
		private vShooterWeapon weapon;

		// Token: 0x04002493 RID: 9363
		private vPowerChargeProjectileControl.vProjectilePerPower lastProjectilePerPower;

		// Token: 0x02000643 RID: 1603
		[Serializable]
		public class vProjectilePerPower
		{
			// Token: 0x04002494 RID: 9364
			public float min;

			// Token: 0x04002495 RID: 9365
			public float max;

			// Token: 0x04002496 RID: 9366
			public GameObject projectile;

			// Token: 0x04002497 RID: 9367
			[Tooltip("Called when power is between min and max value")]
			public UnityEvent OnValidatePower;
		}
	}
}
