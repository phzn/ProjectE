using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vShooter
{
	// Token: 0x02000676 RID: 1654
	[vClassHeader("Shooter Weapon", true, "icon_v2", false, "", openClose = false)]
	public class vShooterWeaponBase : vMonoBehaviour
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000D4561 File Offset: 0x000D2761
		// (set) Token: 0x0600261F RID: 9759 RVA: 0x000D4569 File Offset: 0x000D2769
		public virtual float shootFrequency
		{
			get
			{
				return this._shootFrequency;
			}
			set
			{
				this._shootFrequency = value;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000D4572 File Offset: 0x000D2772
		// (set) Token: 0x06002621 RID: 9761 RVA: 0x000D457A File Offset: 0x000D277A
		public virtual int ammo
		{
			get
			{
				return this._ammo;
			}
			set
			{
				this._ammo = value;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x000D4583 File Offset: 0x000D2783
		// (set) Token: 0x06002623 RID: 9763 RVA: 0x000D458B File Offset: 0x000D278B
		public virtual float minDamageDistance
		{
			get
			{
				return this._minDamageDistance;
			}
			set
			{
				this._minDamageDistance = value;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x000D4594 File Offset: 0x000D2794
		// (set) Token: 0x06002625 RID: 9765 RVA: 0x000D459C File Offset: 0x000D279C
		public virtual float maxDamageDistance
		{
			get
			{
				return this._maxDamageDistance;
			}
			set
			{
				this._maxDamageDistance = value;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x000D45A5 File Offset: 0x000D27A5
		// (set) Token: 0x06002627 RID: 9767 RVA: 0x000D45AD File Offset: 0x000D27AD
		public virtual int minDamage
		{
			get
			{
				return this._minDamage;
			}
			set
			{
				this._minDamage = value;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x000D45B6 File Offset: 0x000D27B6
		// (set) Token: 0x06002629 RID: 9769 RVA: 0x000D45BE File Offset: 0x000D27BE
		public virtual int maxDamage
		{
			get
			{
				return this._maxDamage;
			}
			set
			{
				this._maxDamage = value;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x000D45C7 File Offset: 0x000D27C7
		// (set) Token: 0x0600262B RID: 9771 RVA: 0x000D45CF File Offset: 0x000D27CF
		protected virtual float _nextShootTime { get; set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x000D45D8 File Offset: 0x000D27D8
		// (set) Token: 0x0600262D RID: 9773 RVA: 0x000D45E0 File Offset: 0x000D27E0
		protected virtual float _nextEmptyClipTime { get; set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x000D45E9 File Offset: 0x000D27E9
		// (set) Token: 0x0600262F RID: 9775 RVA: 0x000D45F1 File Offset: 0x000D27F1
		protected virtual Transform sender { get; set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x000D45FA File Offset: 0x000D27FA
		// (set) Token: 0x06002631 RID: 9777 RVA: 0x000D4602 File Offset: 0x000D2802
		public virtual float velocityMultiplierMod { get; set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000D460B File Offset: 0x000D280B
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x000D4613 File Offset: 0x000D2813
		public virtual float damageMultiplierMod { get; set; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000D461C File Offset: 0x000D281C
		public virtual string weaponName
		{
			get
			{
				return base.gameObject.name.Replace("(Clone)", string.Empty);
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000D4638 File Offset: 0x000D2838
		public virtual void Shoot()
		{
			this.Shoot(this.muzzle.position + this.muzzle.forward * 100f, null, null);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000D4667 File Offset: 0x000D2867
		public virtual void Shoot(Transform _sender = null, UnityAction<bool> successfulShot = null)
		{
			this.Shoot(this.muzzle.position + this.muzzle.forward * 100f, _sender, successfulShot);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000D4696 File Offset: 0x000D2896
		public virtual void Shoot(Vector3 aimPosition, Transform _sender = null, UnityAction<bool> successfulShot = null)
		{
			this.Shoot(this.muzzle.position, aimPosition, _sender, successfulShot);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000D46AC File Offset: 0x000D28AC
		public virtual void Shoot(Vector3 startPoint, Vector3 endPoint, Transform _sender = null, UnityAction<bool> successfulShot = null)
		{
			if (this.HasAmmo())
			{
				if (!this.CanDoShot)
				{
					return;
				}
				this.UseAmmo(1);
				this.sender = ((_sender != null) ? _sender : base.transform);
				this.HandleShot(startPoint, endPoint);
				if (successfulShot != null)
				{
					successfulShot(true);
				}
				this._nextShootTime = Time.time + this.shootFrequency;
				this._nextEmptyClipTime = this._nextShootTime;
				return;
			}
			else
			{
				if (!this.CanDoEmptyClip)
				{
					return;
				}
				this.EmptyClipEffect();
				if (successfulShot != null)
				{
					successfulShot(false);
				}
				this._nextEmptyClipTime = Time.time + this.shootFrequency;
				return;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x000D4749 File Offset: 0x000D2949
		public virtual bool CanDoShot
		{
			get
			{
				return this._nextShootTime < Time.time;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x000D4758 File Offset: 0x000D2958
		public virtual bool CanDoEmptyClip
		{
			get
			{
				return this._nextEmptyClipTime < Time.time;
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000D4767 File Offset: 0x000D2967
		public virtual void UseAmmo(int count = 1)
		{
			if (this.ammo <= 0)
			{
				return;
			}
			this.ammo -= count;
			if (this.ammo <= 0)
			{
				this.ammo = 0;
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000D4791 File Offset: 0x000D2991
		public virtual bool HasAmmo()
		{
			return this.isInfinityAmmo || this.ammo > 0;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000D47A6 File Offset: 0x000D29A6
		protected virtual void OnDestroy()
		{
			this.onDestroy.Invoke(base.gameObject);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000D47B9 File Offset: 0x000D29B9
		private void OnApplicationQuit()
		{
			this.onDestroy.RemoveAllListeners();
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000D47C6 File Offset: 0x000D29C6
		protected virtual void HandleShot(Vector3 startPoint, Vector3 endPoint)
		{
			this.ShootBullet(startPoint, endPoint);
			this.ShotEffect();
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000D47D6 File Offset: 0x000D29D6
		public virtual Vector3 Dispersion(Vector3 aim, float dispersion)
		{
			if (!this.quadDispersion)
			{
				return this.CircleDispersion(aim, dispersion);
			}
			return this.QuadDispersion(aim, dispersion);
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000D47F1 File Offset: 0x000D29F1
		public virtual Vector3 CircleDispersion(Vector3 aim, float dispersion)
		{
			aim = Quaternion.Euler(Random.insideUnitSphere * dispersion) * aim;
			return aim;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000D480C File Offset: 0x000D2A0C
		public virtual Vector3 QuadDispersion(Vector3 aim, float dispersion)
		{
			aim = Quaternion.Euler(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion)) * aim;
			return aim.normalized;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000D483C File Offset: 0x000D2A3C
		protected virtual void ShootBullet(Vector3 startPoint, Vector3 endPoint)
		{
			Vector3 vector = endPoint - startPoint;
			Quaternion quaternion = Quaternion.LookRotation(vector);
			if (this.dispersion > 0f && this.projectile)
			{
				for (int i = 0; i < this.projectilesPerShot; i++)
				{
					Vector3 vector2 = this.Dispersion(vector.normalized, this.dispersion);
					Quaternion quaternion2 = Quaternion.LookRotation(vector2);
					GameObject gameObject = Object.Instantiate<GameObject>(this.projectile, startPoint, quaternion2);
					vProjectileControl vProjectileControl = gameObject.GetComponent<vProjectileControl>();
					if (vProjectileControl.debugTrajetory && i == 0)
					{
						startPoint.DebugPoint(Color.red, 10f, 0.1f);
						Debug.DrawLine(startPoint, endPoint, Color.red, 10f);
						endPoint.DebugPoint(Color.red, 10f, 0.1f);
					}
					vProjectileControl.shooterTransform = this.sender;
					vProjectileControl.ignoreTags = this.ignoreTags;
					vProjectileControl.hitLayer = this.hitLayer;
					vProjectileControl.damage.sender = this.sender;
					vProjectileControl.startPosition = gameObject.transform.position;
					vProjectileControl.damageByDistance = this.damageByDistance;
					vProjectileControl.maxDamage = (int)((float)(this.maxDamage / this.projectilesPerShot) * this.damageMultiplier);
					vProjectileControl.minDamage = (int)((float)(this.minDamage / this.projectilesPerShot) * this.damageMultiplier);
					vProjectileControl.minDamageDistance = this.minDamageDistance;
					vProjectileControl.maxDamageDistance = this.maxDamageDistance;
					this.onInstantiateProjectile.Invoke(vProjectileControl);
					float num = this.velocity * this.velocityMultiplier;
					this.ApplyForceToBullet(gameObject, vector2, num);
					vProjectileControl = this.CreateProjectileData(endPoint, num, vector2, vProjectileControl);
				}
				return;
			}
			if (this.projectilesPerShot > 0 && this.projectile)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.projectile, startPoint, quaternion);
				vProjectileControl component = gameObject.GetComponent<vProjectileControl>();
				if (component.debugTrajetory)
				{
					startPoint.DebugPoint(Color.red, 10f, 0.1f);
					Debug.DrawLine(startPoint, endPoint, Color.red, 10f);
					endPoint.DebugPoint(Color.red, 10f, 0.1f);
				}
				component.shooterTransform = this.sender;
				component.ignoreTags = this.ignoreTags;
				component.hitLayer = this.hitLayer;
				component.damage.sender = this.sender;
				component.startPosition = gameObject.transform.position;
				component.damageByDistance = this.damageByDistance;
				component.maxDamage = (int)((float)(this.maxDamage / this.projectilesPerShot) * this.damageMultiplier);
				component.minDamage = (int)((float)(this.minDamage / this.projectilesPerShot) * this.damageMultiplier);
				component.minDamageDistance = this.minDamageDistance;
				component.maxDamageDistance = this.maxDamageDistance;
				this.onInstantiateProjectile.Invoke(component);
				float num = this.velocity * this.velocityMultiplier;
				this.ApplyForceToBullet(gameObject, vector, num);
			}
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000D4B3E File Offset: 0x000D2D3E
		protected virtual vProjectileControl CreateProjectileData(Vector3 aimPosition, float velocityChanged, Vector3 dispersionDir, vProjectileControl pCtrl)
		{
			pCtrl.instantiateData = new vProjectileInstantiateData
			{
				aimPos = aimPosition,
				dir = dispersionDir,
				vel = velocityChanged
			};
			return pCtrl;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000D4B64 File Offset: 0x000D2D64
		protected virtual void ApplyForceToBullet(GameObject bulletObject, Vector3 direction, float velocityChanged)
		{
			try
			{
				Rigidbody component = bulletObject.GetComponent<Rigidbody>();
				component.mass /= (float)this.projectilesPerShot;
				component.AddForce(direction.normalized * velocityChanged, ForceMode.VelocityChange);
			}
			catch
			{
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x000D4BB4 File Offset: 0x000D2DB4
		protected virtual float damageMultiplier
		{
			get
			{
				return 1f + this.damageMultiplierMod;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000D4BC2 File Offset: 0x000D2DC2
		protected virtual float velocityMultiplier
		{
			get
			{
				return 1f + this.velocityMultiplierMod;
			}
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000D4BD0 File Offset: 0x000D2DD0
		protected virtual void ShotEffect()
		{
			this.onShot.Invoke();
			base.StopCoroutine(this.LightOnShoot(0f));
			if (this.source && this.fireClip)
			{
				this.source.PlayOneShot(this.fireClip);
			}
			base.StartCoroutine(this.LightOnShoot(0.037f));
			this.StartEmitters();
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000D4C3C File Offset: 0x000D2E3C
		protected virtual void StopSound()
		{
			if (this.source)
			{
				this.source.Stop();
			}
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000D4C56 File Offset: 0x000D2E56
		protected virtual IEnumerator LightOnShoot(float time = 0f)
		{
			if (this.lightOnShot)
			{
				this.lightOnShot.enabled = true;
				yield return new WaitForSeconds(time);
				this.lightOnShot.enabled = false;
			}
			yield break;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000D4C6C File Offset: 0x000D2E6C
		protected virtual void StartEmitters()
		{
			if (this.emittShurykenParticle != null)
			{
				ParticleSystem[] array = this.emittShurykenParticle;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Emit(1);
				}
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000D4CA0 File Offset: 0x000D2EA0
		protected virtual void StopEmitters()
		{
			if (this.emittShurykenParticle != null)
			{
				ParticleSystem[] array = this.emittShurykenParticle;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Stop();
				}
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000D4CD2 File Offset: 0x000D2ED2
		protected virtual void EmptyClipEffect()
		{
			if (this.source && this.emptyClip)
			{
				this.source.PlayOneShot(this.emptyClip);
			}
			this.onEmptyClip.Invoke();
		}

		// Token: 0x04002614 RID: 9748
		[vEditorToolbar("Weapon Settings", false, "", false, false)]
		[Tooltip("The category of the weapon\n Used to the IK offset system. \nExample: HandGun, Pistol, Machine-Gun")]
		public string weaponCategory = "MyCategory";

		// Token: 0x04002615 RID: 9749
		[SerializeField]
		[Tooltip("Frequency of shots")]
		[FormerlySerializedAs("shootFrequency")]
		protected float _shootFrequency;

		// Token: 0x04002616 RID: 9750
		[vEditorToolbar("Ammo", false, "", false, false)]
		[Tooltip("Unlimited ammo")]
		public bool isInfinityAmmo;

		// Token: 0x04002617 RID: 9751
		[Tooltip("Starting ammo")]
		[SerializeField]
		[vHideInInspector("isInfinityAmmo", true)]
		[FormerlySerializedAs("ammo")]
		protected int _ammo;

		// Token: 0x04002618 RID: 9752
		[vEditorToolbar("Layer & Tag", false, "", false, false)]
		public List<string> ignoreTags = new List<string>();

		// Token: 0x04002619 RID: 9753
		public LayerMask hitLayer = 1;

		// Token: 0x0400261A RID: 9754
		[vEditorToolbar("Projectile", false, "", false, false)]
		[Tooltip("Prefab of the projectile")]
		public GameObject projectile;

		// Token: 0x0400261B RID: 9755
		[Tooltip("Assign the muzzle of your weapon")]
		public Transform muzzle;

		// Token: 0x0400261C RID: 9756
		[Tooltip("How many projectiles will spawn per shot")]
		[Range(1f, 20f)]
		public int projectilesPerShot = 1;

		// Token: 0x0400261D RID: 9757
		[Range(0f, 90f)]
		[Tooltip("how much dispersion the weapon have")]
		public float dispersion;

		// Token: 0x0400261E RID: 9758
		[vToggleOption("DispersionShape", "Circle", "Quad")]
		public bool quadDispersion;

		// Token: 0x0400261F RID: 9759
		[Range(0f, 1000f)]
		[Tooltip("Velocity of your projectile")]
		public float velocity = 380f;

		// Token: 0x04002620 RID: 9760
		[vHelpBox("If you're using the ItemManager attribute 'Damage' on your item, the damage will be always maxDamage, ignoring the distance or minDamage", vHelpBoxAttribute.MessageType.Info)]
		[Tooltip("Check this to calculate damage automatically based on distance using min and max damage, higher distance less damage, less distance more damage")]
		public bool damageByDistance = true;

		// Token: 0x04002621 RID: 9761
		[Tooltip("Min distance to apply damage, used to evaluate the damage between minDamage and maxDamage")]
		[SerializeField]
		[vHideInInspector("damageByDistance", false)]
		[FormerlySerializedAs("minDamageDistance")]
		protected float _minDamageDistance = 8f;

		// Token: 0x04002622 RID: 9762
		[Tooltip("Max distance to apply damage, used to evaluate the damage between minDamage and maxDamage")]
		[SerializeField]
		[vHideInInspector("damageByDistance", false)]
		[FormerlySerializedAs("maxDamageDistance")]
		protected float _maxDamageDistance = 50f;

		// Token: 0x04002623 RID: 9763
		[vHideInInspector("damageByDistance", false)]
		[SerializeField]
		[Tooltip("Minimum damage caused by the shot, regardless the distance")]
		[FormerlySerializedAs("minDamage")]
		protected int _minDamage;

		// Token: 0x04002624 RID: 9764
		[SerializeField]
		[Tooltip("Maximum damage caused by the close shot")]
		[FormerlySerializedAs("maxDamage")]
		protected int _maxDamage;

		// Token: 0x04002625 RID: 9765
		[vEditorToolbar("Audio & VFX", false, "", false, false)]
		[Header("Audio")]
		public AudioSource source;

		// Token: 0x04002626 RID: 9766
		public AudioClip fireClip;

		// Token: 0x04002627 RID: 9767
		public AudioClip emptyClip;

		// Token: 0x04002628 RID: 9768
		[Header("Effects")]
		public bool testShootEffect;

		// Token: 0x04002629 RID: 9769
		public Light lightOnShot;

		// Token: 0x0400262A RID: 9770
		[SerializeField]
		public ParticleSystem[] emittShurykenParticle;

		// Token: 0x0400262B RID: 9771
		[HideInInspector]
		public vShooterWeaponBase.OnDestroyEvent onDestroy;

		// Token: 0x0400262C RID: 9772
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onShot;

		// Token: 0x0400262D RID: 9773
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEmptyClip;

		// Token: 0x0400262E RID: 9774
		public vShooterWeaponBase.OnInstantiateProjectile onInstantiateProjectile;

		// Token: 0x02000677 RID: 1655
		[Serializable]
		public class OnDestroyEvent : UnityEvent<GameObject>
		{
		}

		// Token: 0x02000678 RID: 1656
		[Serializable]
		public class OnInstantiateProjectile : UnityEvent<vProjectileControl>
		{
		}
	}
}
