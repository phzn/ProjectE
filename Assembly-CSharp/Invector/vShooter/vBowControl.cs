using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x02000646 RID: 1606
	[vClassHeader("v Bow Control", true, "icon_v2", false, "")]
	[RequireComponent(typeof(vShooterWeapon))]
	public class vBowControl : vMonoBehaviour
	{
		// Token: 0x060024DA RID: 9434 RVA: 0x000CF32A File Offset: 0x000CD52A
		private void Start()
		{
			this.weapon = base.GetComponent<vShooterWeapon>();
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000CF344 File Offset: 0x000CD544
		public void EnableArrow()
		{
			this.OnEnableArrow.Invoke();
			if (this.animator)
			{
				this.animator.SetFloat("PowerCharger", 0f);
			}
			if (this.animator)
			{
				this.animator.ResetTrigger("Shot");
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000CF39C File Offset: 0x000CD59C
		public void DisableArrow()
		{
			this.OnDisableArrow.Invoke();
			if (this.animator)
			{
				this.animator.ResetTrigger("Shot");
			}
			if (this.animator)
			{
				this.animator.SetFloat("PowerCharger", 0f);
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000CF3F3 File Offset: 0x000CD5F3
		public void OnChangePowerCharger(float charger)
		{
			if (this.animator)
			{
				this.animator.SetFloat("PowerCharger", charger);
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000CF414 File Offset: 0x000CD614
		public void Shot()
		{
			if (!this.weapon)
			{
				return;
			}
			if (this.animator)
			{
				this.animator.SetFloat("PowerCharger", 0f);
			}
			if (this.animator)
			{
				this.animator.SetTrigger("Shot");
			}
			base.StartCoroutine(this.ShootEffect());
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000CF47C File Offset: 0x000CD67C
		public void OnInstantiateProjectile(vProjectileControl pCtrl)
		{
			if (!this.weapon)
			{
				return;
			}
			vArrow component = pCtrl.GetComponent<vArrow>();
			if (component)
			{
				component.penetration = Mathf.Lerp(this.minPenetration, this.maxPenetration, this.weapon.powerCharge);
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000CF4C8 File Offset: 0x000CD6C8
		private IEnumerator ShootEffect()
		{
			yield return new WaitForSeconds(this.delayToSpringAfterShot);
			if (this.weapon.isAiming && this.weapon.ammoCount > 0)
			{
				this.OnFinishShot.Invoke();
			}
			yield break;
		}

		// Token: 0x0400249F RID: 9375
		private vShooterWeapon weapon;

		// Token: 0x040024A0 RID: 9376
		private Animator animator;

		// Token: 0x040024A1 RID: 9377
		public float delayToSpringAfterShot;

		// Token: 0x040024A2 RID: 9378
		public float minPenetration;

		// Token: 0x040024A3 RID: 9379
		public float maxPenetration;

		// Token: 0x040024A4 RID: 9380
		public UnityEvent OnFinishShot;

		// Token: 0x040024A5 RID: 9381
		public UnityEvent OnEnableArrow;

		// Token: 0x040024A6 RID: 9382
		public UnityEvent OnDisableArrow;
	}
}
