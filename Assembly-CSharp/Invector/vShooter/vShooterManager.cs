using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vItemManager;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x02000654 RID: 1620
	[vClassHeader("SHOOTER MANAGER", true, "icon_v2", false, "", iconName = "shooterIcon")]
	public class vShooterManager : vMonoBehaviour, IWeaponEquipmentListener
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x000D08FC File Offset: 0x000CEAFC
		// (set) Token: 0x0600253B RID: 9531 RVA: 0x000D0904 File Offset: 0x000CEB04
		public virtual vShooterWeapon rWeapon
		{
			get
			{
				return this._rWeapon;
			}
			set
			{
				this._rWeapon = value;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600253C RID: 9532 RVA: 0x000D090D File Offset: 0x000CEB0D
		// (set) Token: 0x0600253D RID: 9533 RVA: 0x000D0915 File Offset: 0x000CEB15
		public virtual vShooterWeapon lWeapon
		{
			get
			{
				return this._lWeapon;
			}
			set
			{
				this._lWeapon = value;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600253E RID: 9534 RVA: 0x000D091E File Offset: 0x000CEB1E
		// (set) Token: 0x0600253F RID: 9535 RVA: 0x000D0926 File Offset: 0x000CEB26
		public virtual vAmmoManager ammoManager { get; set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x000D092F File Offset: 0x000CEB2F
		// (set) Token: 0x06002541 RID: 9537 RVA: 0x000D0937 File Offset: 0x000CEB37
		public virtual vAmmoDisplay ammoDisplayR { get; set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x000D0940 File Offset: 0x000CEB40
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x000D0948 File Offset: 0x000CEB48
		public virtual vAmmoDisplay ammoDisplayL { get; set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x000D0951 File Offset: 0x000CEB51
		// (set) Token: 0x06002545 RID: 9541 RVA: 0x000D0959 File Offset: 0x000CEB59
		public virtual vShooterManager.AmmoHandle ammoHandle { get; set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x000D0962 File Offset: 0x000CEB62
		// (set) Token: 0x06002547 RID: 9543 RVA: 0x000D096A File Offset: 0x000CEB6A
		public virtual vThirdPersonCamera tpCamera { get; set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x000D0973 File Offset: 0x000CEB73
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x000D097B File Offset: 0x000CEB7B
		public virtual bool isReloadingWeapon { get; set; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x000D0984 File Offset: 0x000CEB84
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x000D098C File Offset: 0x000CEB8C
		protected virtual vWeaponIKAdjust currentWeaponIKAdjust { get; set; }

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000D0995 File Offset: 0x000CEB95
		// (set) Token: 0x0600254D RID: 9549 RVA: 0x000D099D File Offset: 0x000CEB9D
		protected virtual Animator animator { get; set; }

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000D09A6 File Offset: 0x000CEBA6
		// (set) Token: 0x0600254F RID: 9551 RVA: 0x000D09AE File Offset: 0x000CEBAE
		protected virtual bool usingThirdPersonController { get; set; }

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x000D09B7 File Offset: 0x000CEBB7
		// (set) Token: 0x06002551 RID: 9553 RVA: 0x000D09BF File Offset: 0x000CEBBF
		protected virtual bool cancelReload { get; set; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x000D09C8 File Offset: 0x000CEBC8
		// (set) Token: 0x06002553 RID: 9555 RVA: 0x000D09D0 File Offset: 0x000CEBD0
		protected virtual bool isReloading { get; set; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x000D09D9 File Offset: 0x000CEBD9
		// (set) Token: 0x06002555 RID: 9557 RVA: 0x000D09E1 File Offset: 0x000CEBE1
		protected virtual float hipfirePrecisionAngle { get; set; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x000D09EA File Offset: 0x000CEBEA
		// (set) Token: 0x06002557 RID: 9559 RVA: 0x000D09F2 File Offset: 0x000CEBF2
		protected virtual float hipfirePrecision { get; set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x000D09FB File Offset: 0x000CEBFB
		// (set) Token: 0x06002559 RID: 9561 RVA: 0x000D0A03 File Offset: 0x000CEC03
		protected virtual float reloadStartTime { get; set; }

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x000D0A0C File Offset: 0x000CEC0C
		// (set) Token: 0x0600255B RID: 9563 RVA: 0x000D0A14 File Offset: 0x000CEC14
		public virtual int ShotLayer { get; protected set; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000D0A1D File Offset: 0x000CEC1D
		public virtual int ExtraAmmo
		{
			get
			{
				return this.extraAmmo;
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000D0A28 File Offset: 0x000CEC28
		public virtual void Start()
		{
			this.animator = base.GetComponent<Animator>();
			if (this.applyRecoilToCamera)
			{
				this.tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			this.ammoManager = base.GetComponent<vAmmoManager>();
			if (this.ammoManager != null)
			{
				this.ammoManager.updateTotalAmmo = new vAmmoManager.OnUpdateTotalAmmo(this.AmmoManagerWasUpdated);
			}
			vThirdPersonController component = base.GetComponent<vThirdPersonController>();
			this.usingThirdPersonController = component;
			if (this.usingThirdPersonController && this.useCancelReload)
			{
				component.onReceiveDamage.AddListener(new UnityAction<vDamage>(this.CancelReload));
			}
			if (this.useAmmoDisplay)
			{
				this.GetAmmoDisplays();
			}
			if (this.animator)
			{
				Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.RightHand);
				Component boneTransform2 = this.animator.GetBoneTransform(HumanBodyBones.LeftHand);
				vShooterWeapon componentInChildren = boneTransform.GetComponentInChildren<vShooterWeapon>();
				vShooterWeapon componentInChildren2 = boneTransform2.GetComponentInChildren<vShooterWeapon>();
				this.ShotLayer = this.animator.GetLayerIndex("Shot");
				if (componentInChildren != null)
				{
					this.SetRightWeapon(componentInChildren.gameObject);
				}
				if (componentInChildren2 != null)
				{
					this.SetLeftWeapon(componentInChildren2.gameObject);
				}
			}
			if (!this.ignoreTags.Contains(base.gameObject.tag))
			{
				this.ignoreTags.Add(base.gameObject.tag);
			}
			if (this.useAmmoDisplay)
			{
				if (this.ammoDisplayR)
				{
					this.ammoDisplayR.UpdateDisplay("", "", 0);
				}
				if (this.ammoDisplayL)
				{
					this.ammoDisplayL.UpdateDisplay("", "", 0);
				}
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x000D0BC6 File Offset: 0x000CEDC6
		// (set) Token: 0x0600255F RID: 9567 RVA: 0x000D0BCE File Offset: 0x000CEDCE
		public virtual bool AllAmmoInfinity
		{
			get
			{
				return this.allAmmoInfinity;
			}
			set
			{
				this.allAmmoInfinity = value;
			}
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000D0BD8 File Offset: 0x000CEDD8
		public virtual void SetLeftWeapon(GameObject weapon)
		{
			if (weapon != null)
			{
				vShooterWeapon component = weapon.GetComponent<vShooterWeapon>();
				this.SetLeftWeapon(component);
				return;
			}
			if (this.lWeapon)
			{
				this.onUnequipWeapon.Invoke(this.lWeapon, true);
			}
			this.lWeapon = null;
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000D0C24 File Offset: 0x000CEE24
		protected virtual void SetLeftWeapon(vShooterWeapon weapon)
		{
			if (weapon)
			{
				this.lWeapon = weapon;
				this.lWeapon.IsEquipped = true;
				this.lWeapon.ignoreTags = this.ignoreTags;
				this.lWeapon.hitLayer = this.damageLayer;
				this.lWeapon.root = base.transform;
				this.lWeapon.onDisable.RemoveListener(new UnityAction(this.HideLeftAmmoDisplay));
				this.lWeapon.onDisable.AddListener(new UnityAction(this.HideLeftAmmoDisplay));
				this.lWeapon.onDestroy.RemoveListener(new UnityAction<GameObject>(this.OnDestroyWeapon));
				this.lWeapon.onDestroy.AddListener(new UnityAction<GameObject>(this.OnDestroyWeapon));
				this.CollectExtraAmmo(weapon);
				if (this.lWeapon.dontUseReload)
				{
					this.LoadAllAmmo(this.lWeapon);
				}
				if (this.usingThirdPersonController)
				{
					if (this.useAmmoDisplay && !this.ammoDisplayL)
					{
						this.GetAmmoDisplays();
					}
					if (this.useAmmoDisplay && this.ammoDisplayL)
					{
						this.ammoDisplayL.Show();
					}
					this.UpdateLeftAmmo();
				}
				this.UpdateWeaponIK();
				this.onEquipWeapon.Invoke(weapon, true);
				return;
			}
			if (this.lWeapon)
			{
				this.onUnequipWeapon.Invoke(this.lWeapon, true);
			}
			this.lWeapon = null;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000D0DA0 File Offset: 0x000CEFA0
		public virtual void SetRightWeapon(GameObject weapon)
		{
			if (weapon != null)
			{
				vShooterWeapon component = weapon.GetComponent<vShooterWeapon>();
				this.SetRightWeapon(component);
				return;
			}
			if (this.rWeapon)
			{
				this.onUnequipWeapon.Invoke(this.rWeapon, false);
			}
			this.rWeapon = null;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000D0DEC File Offset: 0x000CEFEC
		protected virtual void SetRightWeapon(vShooterWeapon weapon)
		{
			if (weapon)
			{
				this.rWeapon = weapon;
				this.rWeapon.IsEquipped = true;
				this.rWeapon.ignoreTags = this.ignoreTags;
				this.rWeapon.hitLayer = this.damageLayer;
				this.rWeapon.root = base.transform;
				this.rWeapon.onDisable.RemoveListener(new UnityAction(this.HideRightAmmoDisplay));
				this.rWeapon.onDisable.AddListener(new UnityAction(this.HideRightAmmoDisplay));
				this.rWeapon.onDestroy.RemoveListener(new UnityAction<GameObject>(this.OnDestroyWeapon));
				this.rWeapon.onDestroy.AddListener(new UnityAction<GameObject>(this.OnDestroyWeapon));
				if (this.rWeapon.dontUseReload)
				{
					this.LoadAllAmmo(this.rWeapon);
				}
				this.CollectExtraAmmo(weapon);
				if (this.usingThirdPersonController)
				{
					if (this.useAmmoDisplay && !this.ammoDisplayR)
					{
						this.GetAmmoDisplays();
					}
					if (this.useAmmoDisplay && this.ammoDisplayR)
					{
						this.ammoDisplayR.Show();
					}
					this.UpdateRightAmmo();
				}
				this.UpdateWeaponIK();
				this.onEquipWeapon.Invoke(weapon, false);
				return;
			}
			if (this.rWeapon)
			{
				this.onUnequipWeapon.Invoke(this.rWeapon, false);
			}
			this.rWeapon = null;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000D0F68 File Offset: 0x000CF168
		protected virtual void CollectExtraAmmo(vShooterWeapon weapon)
		{
			if (weapon.ammoCount > weapon.clipSize)
			{
				int num = weapon.ammo - weapon.clipSize;
				weapon.ammo -= num;
				if (this.ammoManager)
				{
					this.ammoManager.AddAmmo(weapon.ammoID, num);
				}
			}
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000D0FBE File Offset: 0x000CF1BE
		protected virtual void HideLeftAmmoDisplay()
		{
			this.HideAmmoDisplay(this.ammoDisplayL);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000D0FCC File Offset: 0x000CF1CC
		protected virtual void HideRightAmmoDisplay()
		{
			this.HideAmmoDisplay(this.ammoDisplayR);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000D0FDA File Offset: 0x000CF1DA
		protected virtual void HideAmmoDisplay(vAmmoDisplay ammoDisplay)
		{
			if (this.useAmmoDisplay && ammoDisplay)
			{
				ammoDisplay.UpdateDisplay("", "", 0);
				ammoDisplay.Hide();
			}
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000D1004 File Offset: 0x000CF204
		public virtual void OnDestroyWeapon(GameObject otherGameObject)
		{
			if (this.usingThirdPersonController)
			{
				vAmmoDisplay vAmmoDisplay = ((this.rWeapon != null && otherGameObject == this.rWeapon.gameObject) ? this.ammoDisplayR : ((this.lWeapon != null && otherGameObject == this.lWeapon.gameObject) ? this.ammoDisplayL : null));
				this.HideAmmoDisplay(vAmmoDisplay);
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000D1074 File Offset: 0x000CF274
		protected virtual void GetAmmoDisplays()
		{
			vAmmoDisplay[] array = Object.FindObjectsOfType<vAmmoDisplay>();
			if (array.Length != 0)
			{
				if (!this.ammoDisplayL)
				{
					this.ammoDisplayL = array.vToList<vAmmoDisplay>().Find((vAmmoDisplay d) => d.displayID == this.leftWeaponAmmoDisplayID);
				}
				if (!this.ammoDisplayR)
				{
					this.ammoDisplayR = array.vToList<vAmmoDisplay>().Find((vAmmoDisplay d) => d.displayID == this.rightWeaponAmmoDisplayID);
				}
			}
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000D10E0 File Offset: 0x000CF2E0
		public virtual int GetMoveSetID()
		{
			int num = 0;
			if (this.rWeapon && this.rWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.rWeapon.moveSetID;
			}
			else if (this.lWeapon && this.lWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.lWeapon.moveSetID;
			}
			return num;
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000D114C File Offset: 0x000CF34C
		public virtual int GetUpperBodyID()
		{
			int num = 0;
			if (this.rWeapon && this.rWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.rWeapon.upperBodyID;
			}
			else if (this.lWeapon && this.lWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.lWeapon.upperBodyID;
			}
			return num;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000D11B8 File Offset: 0x000CF3B8
		public virtual int GetShotID()
		{
			int num = 0;
			if (this.rWeapon && this.rWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.rWeapon.shotID;
			}
			else if (this.lWeapon && this.lWeapon.gameObject.activeInHierarchy)
			{
				num = (int)this.lWeapon.shotID;
			}
			return num;
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000D1224 File Offset: 0x000CF424
		public virtual int GetEquipID()
		{
			int num = 0;
			if (this.rWeapon && this.rWeapon.gameObject.activeInHierarchy)
			{
				num = this.rWeapon.equipID;
			}
			else if (this.lWeapon && this.lWeapon.gameObject.activeInHierarchy)
			{
				num = this.lWeapon.equipID;
			}
			return num;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000D128C File Offset: 0x000CF48C
		public virtual int GetReloadID()
		{
			int num = 0;
			if (this.rWeapon && this.rWeapon.gameObject.activeInHierarchy)
			{
				num = this.rWeapon.reloadID;
			}
			else if (this.lWeapon && this.lWeapon.gameObject.activeInHierarchy)
			{
				num = this.lWeapon.reloadID;
			}
			return num;
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000D12F4 File Offset: 0x000CF4F4
		public virtual float HipfireAimTime
		{
			get
			{
				return this.hipfireAimTime + (this.CurrentWeapon ? this.CurrentWeapon.shootFrequency : 0f);
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000D131C File Offset: 0x000CF51C
		public virtual bool WeaponHasLoadedAmmo()
		{
			return (this.CurrentWeapon ? this.CurrentWeapon.ammoCount : 0) > 0;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000D133C File Offset: 0x000CF53C
		public virtual bool WeaponHasUnloadedAmmo()
		{
			return this.extraAmmo > 0;
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x000D1347 File Offset: 0x000CF547
		public virtual bool isShooting
		{
			get
			{
				return this.CurrentWeapon && !this.CurrentWeapon.CanDoShot;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x000D1366 File Offset: 0x000CF566
		public virtual bool isShootingEmptyClip
		{
			get
			{
				return this.CurrentWeapon && !this.CurrentWeapon.CanDoEmptyClip;
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000D1388 File Offset: 0x000CF588
		public virtual void ReloadWeapon()
		{
			vShooterWeapon vShooterWeapon = (this.rWeapon ? this.rWeapon : this.lWeapon);
			if (!vShooterWeapon || !vShooterWeapon.gameObject.activeInHierarchy || this.isReloading)
			{
				return;
			}
			this.UpdateTotalAmmo();
			if (vShooterWeapon.ammoCount < vShooterWeapon.clipSize && (vShooterWeapon.isInfinityAmmo || this.AllAmmoInfinity || this.WeaponHasUnloadedAmmo()) && !vShooterWeapon.dontUseReload)
			{
				this.onStartReloadWeapon.Invoke(vShooterWeapon);
				if (this.animator)
				{
					this.animator.SetInteger(this.ReloadID, this.GetReloadID());
					this.animator.SetTrigger(this.Reload);
				}
				if (this.CurrentWeapon && this.CurrentWeapon.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.AddAmmoToWeapon(this.CurrentWeapon, this.CurrentWeapon.reloadTime));
				}
			}
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000D1487 File Offset: 0x000CF687
		protected virtual IEnumerator AddAmmoToWeapon(vShooterWeapon weapon, float delayTime)
		{
			this.isReloading = true;
			this.isReloadingWeapon = true;
			this.reloadStartTime = Time.time;
			if (weapon.ammoCount < weapon.clipSize && (weapon.isInfinityAmmo || this.AllAmmoInfinity || this.WeaponHasUnloadedAmmo()) && !weapon.dontUseReload && !this.cancelReload)
			{
				weapon.ReloadEffect();
				yield return new WaitForSeconds(delayTime);
				if (!this.cancelReload)
				{
					int num = (weapon.reloadOneByOne ? 1 : (weapon.clipSize - weapon.ammoCount));
					if (!weapon.reloadOneByOne && (weapon.isInfinityAmmo || this.AllAmmoInfinity))
					{
						weapon.AddAmmo(num);
					}
					else
					{
						if (!weapon.isInfinityAmmo && !this.AllAmmoInfinity && this.WeaponAmmo(weapon).count < num)
						{
							num = this.WeaponAmmo(weapon).count;
						}
						weapon.AddAmmo(num);
						this.WeaponAmmo(weapon).Use(num);
					}
					if (weapon.reloadOneByOne && weapon.ammoCount < weapon.clipSize && this.WeaponHasUnloadedAmmo())
					{
						if (!weapon.isInfinityAmmo && this.WeaponAmmo(weapon).count == 0)
						{
							weapon.FinishReloadEffect();
							this.isReloadingWeapon = false;
							this.onFinishReloadWeapon.Invoke(weapon);
						}
						else
						{
							this.isReloadingWeapon = true;
							if (!this.cancelReload)
							{
								this.animator.SetInteger(this.ReloadID, weapon.reloadID);
								this.animator.SetTrigger(this.Reload);
								base.StartCoroutine(this.AddAmmoToWeapon(weapon, delayTime));
							}
						}
					}
					else
					{
						weapon.FinishReloadEffect();
						this.isReloadingWeapon = false;
						this.onFinishReloadWeapon.Invoke(weapon);
					}
				}
				this.UpdateTotalAmmo();
			}
			this.isReloading = false;
			yield break;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000D14A4 File Offset: 0x000CF6A4
		public virtual void CancelReload()
		{
			if (this.isReloading)
			{
				base.StartCoroutine(this.CancelReloadRoutine());
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000D14BB File Offset: 0x000CF6BB
		public virtual void CancelReload(vDamage damage)
		{
			if (!this.ignoreReacionIDList.Contains(damage.reaction_id) && this.isReloading)
			{
				base.StartCoroutine(this.CancelReloadRoutine());
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000D14E5 File Offset: 0x000CF6E5
		protected virtual IEnumerator CancelReloadRoutine()
		{
			if (this.CurrentWeapon != null)
			{
				this.animator.SetTrigger("CancelReload");
				this.animator.ResetTrigger("Reload");
				this.cancelReload = true;
				base.StopCoroutine("AddAmmoToWeapon");
				if (this.CurrentWeapon)
				{
					this.CurrentWeapon.CancelReload();
				}
				yield return new WaitForSeconds(this.CurrentWeapon.reloadTime + 0.1f);
				this.cancelReload = false;
				if (this.isReloadingWeapon)
				{
					this.isReloadingWeapon = false;
					if (this.CurrentWeapon)
					{
						this.onFinishReloadWeapon.Invoke(this.CurrentWeapon);
					}
				}
				this.animator.ResetTrigger("CancelReload");
				this.UpdateTotalAmmo();
			}
			yield break;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000D14F4 File Offset: 0x000CF6F4
		public virtual void LoadAllAmmo(vShooterWeapon weapon)
		{
			if (!weapon)
			{
				return;
			}
			this.UpdateTotalAmmo();
			if (weapon.ammoCount < weapon.clipSize && (weapon.isInfinityAmmo || this.AllAmmoInfinity || this.WeaponHasUnloadedAmmo()))
			{
				int num = weapon.clipSize - weapon.ammoCount;
				if (weapon.isInfinityAmmo || this.AllAmmoInfinity)
				{
					weapon.AddAmmo(num);
				}
				else
				{
					if (this.WeaponAmmo(weapon).count < num)
					{
						num = this.WeaponAmmo(weapon).count;
					}
					weapon.AddAmmo(num);
					this.WeaponAmmo(weapon).Use(num);
				}
				weapon.onReload.Invoke();
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000D1598 File Offset: 0x000CF798
		public virtual vAmmo WeaponAmmo(vShooterWeapon weapon)
		{
			if (!weapon)
			{
				return null;
			}
			vAmmo vAmmo = new vAmmo();
			if (this.ammoManager && this.ammoManager.ammos != null && this.ammoManager.ammos.Count > 0)
			{
				vAmmo = this.ammoManager.GetAmmo(weapon.ammoID);
			}
			return vAmmo;
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x000D15F8 File Offset: 0x000CF7F8
		public virtual vShooterWeapon CurrentActiveWeapon
		{
			get
			{
				vShooterWeapon currentWeapon = this.CurrentWeapon;
				if (currentWeapon && currentWeapon.IsEquipped)
				{
					return currentWeapon;
				}
				return null;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x000D1620 File Offset: 0x000CF820
		public virtual vShooterWeapon CurrentWeapon
		{
			get
			{
				vShooterWeapon vShooterWeapon = (this.rWeapon ? this.rWeapon : (this.lWeapon ? this.lWeapon : null));
				if (!(vShooterWeapon != null))
				{
					return null;
				}
				return vShooterWeapon;
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000D1665 File Offset: 0x000CF865
		public virtual void SetIKAdjustList(vWeaponIKAdjustList weaponIKAdjustList)
		{
			this.weaponIKAdjustList = weaponIKAdjustList;
			if (this.CurrentWeapon)
			{
				this.currentWeaponIKAdjust = weaponIKAdjustList.GetWeaponIK(this.CurrentWeapon.weaponCategory);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x000D1692 File Offset: 0x000CF892
		public virtual vWeaponIKAdjust CurrentWeaponIK
		{
			get
			{
				return this.currentWeaponIKAdjust;
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000D169A File Offset: 0x000CF89A
		public virtual void UpdateWeaponIK()
		{
			if (this.weaponIKAdjustList && this.CurrentWeapon)
			{
				this.currentWeaponIKAdjust = this.weaponIKAdjustList.GetWeaponIK(this.CurrentWeapon.weaponCategory);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x000D16D2 File Offset: 0x000CF8D2
		public virtual bool IsLeftWeapon
		{
			get
			{
				if (!(this.rWeapon == null))
				{
					return this.rWeapon.isLeftWeapon;
				}
				return this.lWeapon;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000D16FC File Offset: 0x000CF8FC
		public virtual void AmmoManagerWasUpdated()
		{
			bool flag = true;
			if (this.CurrentWeapon && this.CurrentWeapon.dontUseReload)
			{
				this.LoadAllAmmo(this.CurrentWeapon);
				flag = false;
			}
			if (flag)
			{
				this.UpdateTotalAmmo();
			}
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000D173C File Offset: 0x000CF93C
		public virtual void UpdateTotalAmmo()
		{
			this.UpdateLeftAmmo();
			this.UpdateRightAmmo();
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000D174A File Offset: 0x000CF94A
		public virtual void UpdateLeftAmmo()
		{
			if (!this.lWeapon)
			{
				return;
			}
			this.UpdateTotalAmmo(this.lWeapon, ref this.extraAmmo, -1);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000D176D File Offset: 0x000CF96D
		public virtual void UpdateRightAmmo()
		{
			if (!this.rWeapon)
			{
				return;
			}
			this.UpdateTotalAmmo(this.rWeapon, ref this.extraAmmo, 1);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000D1790 File Offset: 0x000CF990
		protected virtual void UpdateTotalAmmo(vShooterWeapon weapon, ref int targetTotalAmmo, int displayId)
		{
			if (!weapon)
			{
				return;
			}
			int num = 0;
			if (weapon.isInfinityAmmo || this.AllAmmoInfinity)
			{
				num = 9999;
			}
			else
			{
				vAmmo vAmmo = this.WeaponAmmo(weapon);
				if (vAmmo != null)
				{
					num += vAmmo.count;
				}
			}
			targetTotalAmmo = num;
			this.UpdateAmmoDisplay(displayId);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000D17E0 File Offset: 0x000CF9E0
		protected virtual void UpdateAmmoDisplay(int displayId)
		{
			if (!this.useAmmoDisplay)
			{
				return;
			}
			vShooterWeapon vShooterWeapon = ((displayId == 1) ? this.rWeapon : this.lWeapon);
			if (!this.ammoDisplayR || !this.ammoDisplayL)
			{
				this.GetAmmoDisplays();
			}
			vAmmoDisplay vAmmoDisplay = ((displayId == 1) ? this.ammoDisplayR : this.ammoDisplayL);
			if (this.useAmmoDisplay && vAmmoDisplay)
			{
				string text = (vShooterWeapon.dontUseReload ? ((vShooterWeapon.isInfinityAmmo || this.AllAmmoInfinity) ? "∞" : (vShooterWeapon.ammoCount + this.extraAmmo).ToString("00")) : vShooterWeapon.ammoCount.ToString("00"));
				string text2 = ((vShooterWeapon.dontUseReload && (vShooterWeapon.isInfinityAmmo || this.AllAmmoInfinity)) ? "" : ((!vShooterWeapon.dontUseReload && (vShooterWeapon.isInfinityAmmo || this.AllAmmoInfinity)) ? "∞" : ((vShooterWeapon.dontUseReload && (!vShooterWeapon.isInfinityAmmo || !this.AllAmmoInfinity)) ? "" : this.extraAmmo.ToString("00"))));
				vAmmoDisplay.UpdateDisplay(text, text2, vShooterWeapon.ammoID);
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000D1920 File Offset: 0x000CFB20
		public virtual void Shoot(Vector3 aimPosition, bool applyHipfirePrecision = false, bool scopeViewMode = false)
		{
			if (this.isShooting)
			{
				return;
			}
			vShooterWeapon vShooterWeapon = (this.rWeapon ? this.rWeapon : this.lWeapon);
			if (!vShooterWeapon || !vShooterWeapon.gameObject.activeInHierarchy)
			{
				return;
			}
			if (vShooterWeapon.dontUseReload)
			{
				this.LoadAllAmmo(vShooterWeapon);
			}
			else if (vShooterWeapon.autoReload && vShooterWeapon.ammoCount <= 0 && this.WeaponHasUnloadedAmmo())
			{
				this.ReloadWeapon();
				return;
			}
			Vector3 vector = (applyHipfirePrecision ? (aimPosition + this.HipFirePrecision(aimPosition)) : aimPosition);
			bool sucessfulShot = false;
			vShooterWeapon.Shoot(vShooterWeapon.muzzle.position, vector, base.transform, delegate(bool sucessful)
			{
				sucessfulShot = sucessful;
			});
			if (sucessfulShot)
			{
				this.ApplyRecoil();
				this.onShot.Invoke(vShooterWeapon);
			}
			else
			{
				this.onEmptyClipShot.Invoke(vShooterWeapon);
			}
			this.UpdateAmmoDisplay(this.rWeapon ? 1 : (-1));
			if (vShooterWeapon.dontUseReload)
			{
				this.LoadAllAmmo(vShooterWeapon);
			}
			if (this.extraAmmo <= 0)
			{
				this.onFinishAmmo.Invoke(vShooterWeapon);
				vShooterWeapon.onFinishAmmo.Invoke();
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000D1A4C File Offset: 0x000CFC4C
		protected virtual Vector3 HipFirePrecision(Vector3 _aimPosition)
		{
			vShooterWeapon vShooterWeapon = (this.rWeapon ? this.rWeapon : this.lWeapon);
			if (!vShooterWeapon)
			{
				return Vector3.zero;
			}
			this.hipfirePrecisionAngle = (float)Random.Range(-1000, 1000);
			this.hipfirePrecision = Random.Range(-this.hipfireDispersion, this.hipfireDispersion);
			return (Quaternion.AngleAxis(this.hipfirePrecisionAngle, _aimPosition - vShooterWeapon.muzzle.position) * Vector3.up).normalized * this.hipfirePrecision;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000D1AEC File Offset: 0x000CFCEC
		public virtual void CameraSway()
		{
			vShooterWeapon vShooterWeapon = (this.rWeapon ? this.rWeapon : this.lWeapon);
			if (!vShooterWeapon)
			{
				return;
			}
			float num = Mathf.PerlinNoise(0f, Time.time * this.cameraSwaySpeed) - 0.5f;
			float num2 = Mathf.PerlinNoise(0f, Time.time * this.cameraSwaySpeed + 100f) - 0.5f;
			float num3 = this.cameraMaxSwayAmount * (1f - vShooterWeapon.cameraStability);
			if (num3 == 0f)
			{
				return;
			}
			num *= num3;
			num2 *= num3;
			float num4 = Mathf.PerlinNoise(0f, Time.time * this.cameraSwaySpeed) - 0.5f;
			float num5 = Mathf.PerlinNoise(0f, Time.time * this.cameraSwaySpeed + 100f) - 0.5f;
			num4 *= -(num3 * 0.25f);
			num5 *= num3 * 0.25f;
			if (this.tpCamera != null)
			{
				this.tpCamera.offsetMouse.x = num + num4;
				this.tpCamera.offsetMouse.y = num2 + num5;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x000D1C14 File Offset: 0x000CFE14
		protected virtual float cameraRecoilPower
		{
			get
			{
				return 1f - this.cameraRecoilStability;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x000D1C22 File Offset: 0x000CFE22
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x000D1C2A File Offset: 0x000CFE2A
		public virtual Vector3 recoilPositionOffset { get; protected set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x000D1C33 File Offset: 0x000CFE33
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x000D1C3B File Offset: 0x000CFE3B
		public virtual Vector3 recoilRotationOffset { get; protected set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600258F RID: 9615 RVA: 0x000D1C44 File Offset: 0x000CFE44
		// (set) Token: 0x06002590 RID: 9616 RVA: 0x000D1C4C File Offset: 0x000CFE4C
		public virtual float recoilSpineVerticalOffset { get; protected set; }

		// Token: 0x06002591 RID: 9617 RVA: 0x000D1C55 File Offset: 0x000CFE55
		public virtual void ApplyRecoil()
		{
			this.ApplyAnimationRecoil();
			this.ApplyCameraRecoil();
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000D1C64 File Offset: 0x000CFE64
		protected virtual void ApplyCameraRecoil()
		{
			if (!this.applyRecoilToCamera || this.CurrentWeapon == null || this.tpCamera == null)
			{
				return;
			}
			float num = Random.Range(this.CurrentWeapon.recoilLeft, this.CurrentWeapon.recoilRight) * this.cameraRecoilPower;
			float num2 = Random.Range(0f, this.CurrentWeapon.recoilUp) * this.cameraRecoilPower;
			if (this.tpCamera != null)
			{
				this.tpCamera.RotateCamera(num, num2);
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000D1CF1 File Offset: 0x000CFEF1
		protected virtual void ApplyAnimationRecoil()
		{
			if (this.animator)
			{
				this.animator.SetTrigger(this.IsShoot);
			}
		}

		// Token: 0x040024EF RID: 9455
		[vEditorToolbar("Melee Overrides", false, "", false, false)]
		[vHelpBox("Behaviour when Shooter Weapon is Disabled (equipped but disabled)", vHelpBoxAttribute.MessageType.None)]
		public bool canUseMeleeBlock_H = true;

		// Token: 0x040024F0 RID: 9456
		public bool canUseMeleeWeakAttack_H = true;

		// Token: 0x040024F1 RID: 9457
		public bool canUseMeleeStrongAttack_H = true;

		// Token: 0x040024F2 RID: 9458
		[vHelpBox("Behaviour when Shooter Weapon is Enabled (equipped and enabled)", vHelpBoxAttribute.MessageType.None)]
		public bool canUseMeleeBlock_E;

		// Token: 0x040024F3 RID: 9459
		public bool canUseMeleeWeakAttack_E = true;

		// Token: 0x040024F4 RID: 9460
		public bool canUseMeleeStrongAttack_E;

		// Token: 0x040024F5 RID: 9461
		public bool canUseMeleeAiming;

		// Token: 0x040024F6 RID: 9462
		[vEditorToolbar("Damage Layers", false, "", false, false)]
		[Tooltip("Layer to aim and apply damage")]
		public LayerMask damageLayer = 1;

		// Token: 0x040024F7 RID: 9463
		[Tooltip("Tags to ignore (auto add this gameObject tag to avoid damage your self)")]
		public vTagMask ignoreTags = new vTagMask(new string[] { "Player" });

		// Token: 0x040024F8 RID: 9464
		[Tooltip("Layer to block aim")]
		public LayerMask blockAimLayer = 1;

		// Token: 0x040024F9 RID: 9465
		[vEditorToolbar("Cancel Reload", false, "", false, false)]
		[vHelpBox("You can call the <b>CancelReload</b> method using events to interrupt the reload routine and animation, for example, when doing an Custom Action or receiving a specific hitReaction ID", vHelpBoxAttribute.MessageType.None)]
		[Tooltip("It will always automatically use the CancelReload")]
		public bool useCancelReload = true;

		// Token: 0x040024FA RID: 9466
		[Tooltip("This is a list of HitReaction ID that will be ignored by the CancelReload routine")]
		public List<int> ignoreReacionIDList = new List<int> { -1 };

		// Token: 0x040024FB RID: 9467
		[vEditorToolbar("Aim", false, "", false, false)]
		[Tooltip("The min distance that Hit point can be, the min distance will be used to indicate the target point in the crosshair")]
		public float maxAimHitPointIndicator = 30f;

		// Token: 0x040024FC RID: 9468
		[Tooltip("The min distance that Hit point can be, the min distance will be used to calculate the point using camera position and camera forward relative to muzzle position")]
		public float minAimHitPointDistance;

		// Token: 0x040024FD RID: 9469
		[Tooltip("If the Aim Hit point is behind weapon muzzle, the aim point will be the default point")]
		public bool ignoreBackAimPoint = true;

		// Token: 0x040024FE RID: 9470
		[Tooltip("Offset for check if aim point is behind weapon muzzle")]
		public float backAimPointOffset;

		// Token: 0x040024FF RID: 9471
		[vSeparator("Float Values", "")]
		public bool useCheckAim = true;

		// Token: 0x04002500 RID: 9472
		public float checkAimRadius = 0.1f;

		// Token: 0x04002501 RID: 9473
		public float checkAimOffsetZ;

		// Token: 0x04002502 RID: 9474
		public float checkAimOffsetSmooth = 2f;

		// Token: 0x04002503 RID: 9475
		[vSeparator("Standing", "")]
		public float checkAimStandingOffsetStartY;

		// Token: 0x04002504 RID: 9476
		public float checkAimStandingOffsetStartX = 0.2f;

		// Token: 0x04002505 RID: 9477
		public float checkAimStandingOffsetEndY;

		// Token: 0x04002506 RID: 9478
		public float checkAimStandingOffsetEndX;

		// Token: 0x04002507 RID: 9479
		[vSeparator("Crouching", "")]
		public float checkAimCrouchedOffsetStartY;

		// Token: 0x04002508 RID: 9480
		public float checkAimCrouchedOffsetStartX = 0.2f;

		// Token: 0x04002509 RID: 9481
		public float checkAimCrouchedOffsetEndY;

		// Token: 0x0400250A RID: 9482
		public float checkAimCrouchedOffsetEndX;

		// Token: 0x0400250B RID: 9483
		[vSeparator("Shooter Settings", "")]
		[Tooltip("The Aim stays active when reload, including animator parameter and camera state")]
		public bool keepAimingWhenReload;

		// Token: 0x0400250C RID: 9484
		[Tooltip("Check true to make the character always aim and walk on strafe mode")]
		public bool alwaysAiming;

		// Token: 0x0400250D RID: 9485
		public bool onlyWalkWhenAiming = true;

		// Token: 0x0400250E RID: 9486
		public bool useDefaultMovesetWhenNotAiming = true;

		// Token: 0x0400250F RID: 9487
		[vEditorToolbar("IK Adjust", false, "", false, false)]
		public float armIKSmoothIn = 10f;

		// Token: 0x04002510 RID: 9488
		[vEditorToolbar("IK Adjust", false, "", false, false)]
		public float armIKSmoothOut = 25f;

		// Token: 0x04002511 RID: 9489
		public AnimationCurve armIKCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002512 RID: 9490
		[Tooltip("Control the speed of the Animator Layer OnlyArms Weight")]
		public float onlyArmsSpeed = 25f;

		// Token: 0x04002513 RID: 9491
		[Tooltip("Alignment smooth method\nIf Yes. The smooth will be applied to the target alignment point (Aim Position) of the Arm \nIf NOT. the smooth will be applied to the additive rotation  based on target alignment point (Aim Position)")]
		public bool smoothIKAlignmentPoint;

		// Token: 0x04002514 RID: 9492
		[Tooltip("smooth of the right hand when correcting the aim")]
		public float smoothArmIKRotation = 30f;

		// Token: 0x04002515 RID: 9493
		[Tooltip("Limit the maxAngle for the arm Alignment")]
		public float maxVerticalAimAngle = 60f;

		// Token: 0x04002516 RID: 9494
		[Tooltip("Limit the maxAngle for the arm Alignment")]
		public float maxHorizontalAimAngle = 20f;

		// Token: 0x04002517 RID: 9495
		[Tooltip("smooth of the right arm when correcting the aim")]
		public float smoothArmWeight = 24f;

		// Token: 0x04002518 RID: 9496
		[Tooltip("Sync the weapon aim to the camera aim")]
		public bool raycastAimTarget = true;

		// Token: 0x04002519 RID: 9497
		[Tooltip("rotate arm ik to aim hit point, if false the arms will rotate to  camera forward distance 100")]
		public bool alignArmToHitPoint = true;

		// Token: 0x0400251A RID: 9498
		[Tooltip("Check this to use IK on the left hand")]
		public bool useLeftIK = true;

		// Token: 0x0400251B RID: 9499
		[Tooltip("Check this to use IK on the left hand")]
		public bool useRightIK = true;

		// Token: 0x0400251C RID: 9500
		[vSeparator("--- Start PlayMode to edit the IK Adjust ---", "")]
		public vWeaponIKAdjustList weaponIKAdjustList;

		// Token: 0x0400251D RID: 9501
		public float ikAdjustSmooth = 20f;

		// Token: 0x0400251E RID: 9502
		public AnimationCurve ikAdjustWeightCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400251F RID: 9503
		[vEditorToolbar("Ammo", false, "", false, false)]
		[SerializeField]
		protected bool allAmmoInfinity;

		// Token: 0x04002520 RID: 9504
		[Tooltip("Use the vAmmoDisplay to shot ammo count")]
		public bool useAmmoDisplay = true;

		// Token: 0x04002521 RID: 9505
		[Tooltip("ID to find ammoDisplay for leftWeapon")]
		public int leftWeaponAmmoDisplayID = -1;

		// Token: 0x04002522 RID: 9506
		[Tooltip("ID to find ammoDisplay for rightWeapon")]
		public int rightWeaponAmmoDisplayID = 1;

		// Token: 0x04002523 RID: 9507
		[vEditorToolbar("Recoil", false, "", false, false)]
		[Tooltip("Move camera angle when shot using recoil properties of weapon")]
		public bool applyRecoilToCamera = true;

		// Token: 0x04002524 RID: 9508
		[Tooltip("The camera recoil stability")]
		[Range(0f, 1f)]
		public float cameraRecoilStability;

		// Token: 0x04002525 RID: 9509
		[vEditorToolbar("LockOn", false, "", false, false)]
		[vSeparator("LockOn (need the shooter lockon component)", "")]
		[Tooltip("Allow the use of the LockOn or not")]
		public bool useLockOn;

		// Token: 0x04002526 RID: 9510
		[Tooltip("Allow the use of the LockOn only with a Melee Weapon")]
		public bool useLockOnMeleeOnly = true;

		// Token: 0x04002527 RID: 9511
		[vEditorToolbar("HipFire", false, "", false, false)]
		[vSeparator("HipFire Options", "")]
		[Tooltip("If enable, remember to change your weak attack input to other input - this allows shot without aim")]
		public bool hipfireShot;

		// Token: 0x04002528 RID: 9512
		[Tooltip("Precision of the weapon when shooting using hipfire (without aiming)")]
		public float hipfireDispersion = 0.5f;

		// Token: 0x04002529 RID: 9513
		[Tooltip("Time to keep aiming after shot")]
		[SerializeField]
		public float hipfireAimTime = 2f;

		// Token: 0x0400252A RID: 9514
		[vEditorToolbar("Camera Sway", false, "", false, false)]
		[vSeparator("Camera Sway Settings", "")]
		[Tooltip("Camera Sway movement while aiming")]
		public float cameraMaxSwayAmount = 2f;

		// Token: 0x0400252B RID: 9515
		[Tooltip("Camera Sway Speed while aiming")]
		public float cameraSwaySpeed = 0.5f;

		// Token: 0x0400252C RID: 9516
		[vEditorToolbar("Weapons", false, "", false, false)]
		[SerializeField]
		protected vShooterWeapon _rWeapon;

		// Token: 0x0400252D RID: 9517
		[vEditorToolbar("Weapons", false, "", false, false)]
		[SerializeField]
		protected vShooterWeapon _lWeapon;

		// Token: 0x0400252E RID: 9518
		public int reloadAnimatorLayer = 4;

		// Token: 0x0400252F RID: 9519
		public vShooterManager.WeaponEvent onEnableAim;

		// Token: 0x04002530 RID: 9520
		public vShooterManager.WeaponEvent onDisableAim;

		// Token: 0x04002531 RID: 9521
		public vShooterManager.WeaponEvent onEnableScopeView;

		// Token: 0x04002532 RID: 9522
		public vShooterManager.WeaponEvent onDisableScopeView;

		// Token: 0x04002533 RID: 9523
		public vShooterManager.WeaponEvent onShot;

		// Token: 0x04002534 RID: 9524
		public vShooterManager.WeaponEvent onEmptyClipShot;

		// Token: 0x04002535 RID: 9525
		public vShooterManager.WeaponEvent onFinishAmmo;

		// Token: 0x04002536 RID: 9526
		public vShooterManager.WeaponEvent onStartReloadWeapon;

		// Token: 0x04002537 RID: 9527
		public vShooterManager.WeaponEvent onFinishReloadWeapon;

		// Token: 0x04002538 RID: 9528
		public vShooterManager.EquipWeaponEvent onEquipWeapon;

		// Token: 0x04002539 RID: 9529
		public vShooterManager.EquipWeaponEvent onUnequipWeapon;

		// Token: 0x04002549 RID: 9545
		internal readonly int IsShoot = Animator.StringToHash("Shoot");

		// Token: 0x0400254A RID: 9546
		internal readonly int Reload = Animator.StringToHash("Reload");

		// Token: 0x0400254B RID: 9547
		internal readonly int ReloadID = Animator.StringToHash("ReloadID");

		// Token: 0x0400254C RID: 9548
		protected int extraAmmo;

		// Token: 0x0400254D RID: 9549
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool showCheckAimGizmos;

		// Token: 0x0400254E RID: 9550
		protected Vector3 targetPosition;

		// Token: 0x0400254F RID: 9551
		protected Vector3 currentPosition;

		// Token: 0x04002550 RID: 9552
		protected Vector3 currentRotation;

		// Token: 0x04002551 RID: 9553
		protected Vector3 targetRotation;

		// Token: 0x04002552 RID: 9554
		protected float targetSpinekickBack;

		// Token: 0x04002553 RID: 9555
		protected float currentspinekickBack;

		// Token: 0x02000655 RID: 1621
		[Serializable]
		public class WeaponEvent : UnityEvent<vShooterWeapon>
		{
		}

		// Token: 0x02000656 RID: 1622
		[Serializable]
		public class EquipWeaponEvent : UnityEvent<vShooterWeapon, bool>
		{
		}

		// Token: 0x02000657 RID: 1623
		// (Invoke) Token: 0x0600259A RID: 9626
		public delegate void AmmoHandle(int ammoID, ref int ammo);
	}
}
