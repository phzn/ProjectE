using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vShooter
{
	// Token: 0x02000670 RID: 1648
	[vClassHeader("Shooter Weapon", true, "icon_v2", false, "", openClose = false)]
	public class vShooterWeapon : vShooterWeaponBase
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000D3EBE File Offset: 0x000D20BE
		// (set) Token: 0x060025F6 RID: 9718 RVA: 0x000D3EC6 File Offset: 0x000D20C6
		public virtual int clipSize
		{
			get
			{
				return this._clipSize;
			}
			set
			{
				this._clipSize = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000D3ECF File Offset: 0x000D20CF
		// (set) Token: 0x060025F8 RID: 9720 RVA: 0x000D3ED7 File Offset: 0x000D20D7
		public virtual bool IsEquipped { get; set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060025F9 RID: 9721 RVA: 0x000D3EE0 File Offset: 0x000D20E0
		// (set) Token: 0x060025FA RID: 9722 RVA: 0x000D3EE8 File Offset: 0x000D20E8
		protected virtual float _charge { get; set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x000D3EF1 File Offset: 0x000D20F1
		// (set) Token: 0x060025FC RID: 9724 RVA: 0x000D3EF9 File Offset: 0x000D20F9
		protected virtual Transform _handIKTargetOffset { get; set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x000D3F04 File Offset: 0x000D2104
		public virtual Transform handIKTargetOffset
		{
			get
			{
				if (this._handIKTargetOffset == null && this.handIKTarget != null)
				{
					this._handIKTargetOffset = new GameObject("Offset").transform;
					this._handIKTargetOffset.SetParent(this.handIKTarget);
					this._handIKTargetOffset.localPosition = Vector3.zero;
					this._handIKTargetOffset.localEulerAngles = Vector3.zero;
				}
				return this._handIKTargetOffset;
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000D3F79 File Offset: 0x000D2179
		protected virtual void OnDrawGizmos()
		{
			if (!Application.isPlaying && this.testShootEffect)
			{
				if (this.testTime <= 0f)
				{
					this.Shootest();
					return;
				}
				this.testTime -= Time.deltaTime;
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000D3FB0 File Offset: 0x000D21B0
		protected virtual void OnDisable()
		{
			this.onDisable.Invoke();
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000D3FBD File Offset: 0x000D21BD
		protected virtual void Start()
		{
			if (!this.reloadSource)
			{
				this.reloadSource = this.source;
			}
			this.SetScopeZoom(this.scopeZoom);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000D3FE4 File Offset: 0x000D21E4
		public virtual void Shootest()
		{
			this.testTime = this.shootFrequency;
			this.StartEmitters();
			this.lightOnShot.enabled = true;
			this.source.PlayOneShot(this.fireClip);
			base.Invoke("StopShootTest", 0.037f);
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000D4030 File Offset: 0x000D2230
		protected virtual void StopShootTest()
		{
			this.StopEmitters();
			this.lightOnShot.enabled = false;
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x000D4044 File Offset: 0x000D2244
		// (set) Token: 0x06002604 RID: 9732 RVA: 0x000D404C File Offset: 0x000D224C
		public virtual float powerCharge
		{
			get
			{
				return this._charge;
			}
			set
			{
				if (value != this._charge)
				{
					this._charge = value;
					this.onPowerChargerChanged.Invoke(this._charge);
					if (this._charge >= 1f)
					{
						this.onFullPower.Invoke();
					}
				}
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000D4087 File Offset: 0x000D2287
		public virtual void SetPrecision(float value)
		{
			this.cameraStability = Mathf.Clamp(value, 0f, 1f);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000D40A0 File Offset: 0x000D22A0
		public override bool HasAmmo()
		{
			if (this.checkAmmoHandle == null)
			{
				return this.ammo > 0;
			}
			bool flag = false;
			int num = 0;
			bool flag2 = this.checkAmmoHandle(ref flag, ref num);
			if (flag)
			{
				return flag2;
			}
			return this.ammo > 0;
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x000D40E4 File Offset: 0x000D22E4
		public virtual int ammoCount
		{
			get
			{
				if (this.checkAmmoHandle == null)
				{
					return this.ammo;
				}
				bool flag = false;
				int num = 0;
				this.checkAmmoHandle(ref flag, ref num);
				if (flag)
				{
					return num;
				}
				return this.ammo;
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000D4120 File Offset: 0x000D2320
		public virtual void AddAmmo(int value)
		{
			if (this.checkAmmoHandle == null || this.changeAmmoHandle == null)
			{
				this.ammo += value;
				return;
			}
			bool flag = false;
			int num = 0;
			this.checkAmmoHandle(ref flag, ref num);
			if (flag)
			{
				this.changeAmmoHandle(value);
				return;
			}
			this.ammo += value;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000D4180 File Offset: 0x000D2380
		public override void UseAmmo(int count = 1)
		{
			if (this.checkAmmoHandle == null || this.changeAmmoHandle == null)
			{
				this.ammo -= count;
				return;
			}
			bool flag = false;
			int num = 0;
			this.checkAmmoHandle(ref flag, ref num);
			if (flag)
			{
				this.changeAmmoHandle(-count);
				return;
			}
			this.ammo -= count;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000D41E0 File Offset: 0x000D23E0
		public virtual void ReloadEffect()
		{
			if (this.reloadSource && this.reloadClip)
			{
				this.reloadSource.Stop();
				this.reloadSource.PlayOneShot(this.reloadClip);
			}
			this.onReload.Invoke();
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000D4230 File Offset: 0x000D2430
		public virtual void FinishReloadEffect()
		{
			if (this.reloadSource && this.finishReloadClip)
			{
				this.reloadSource.Stop();
				this.reloadSource.PlayOneShot(this.finishReloadClip);
			}
			this.onFinishReload.Invoke();
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x000D427E File Offset: 0x000D247E
		protected override float damageMultiplier
		{
			get
			{
				if (!this.chargeWeapon)
				{
					return base.damageMultiplier;
				}
				return (float)Math.Round((double)(1f + Mathf.Lerp(0f, this.chargeDamageMultiplier, this._charge) + this.damageMultiplierMod), 1);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x000D42BA File Offset: 0x000D24BA
		protected override float velocityMultiplier
		{
			get
			{
				if (!this.chargeWeapon || !this.changeVelocityByCharge)
				{
					return base.velocityMultiplier;
				}
				return 1f + Mathf.Lerp(0f, this.chargeVelocityMultiplier, this._charge) + this.velocityMultiplierMod;
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000D42F8 File Offset: 0x000D24F8
		public virtual void SetScopeZoom(float value)
		{
			if (this.zoomScopeCamera)
			{
				float num = Mathf.Clamp(61f - value, 1f, 179f);
				this.zoomScopeCamera.fieldOfView = num;
			}
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000D4335 File Offset: 0x000D2535
		public virtual void SetActiveAim(bool value)
		{
			if (this.isAiming != value)
			{
				this.isAiming = value;
				if (this.isAiming)
				{
					this.onEnableAim.Invoke();
					return;
				}
				this.onDisableAim.Invoke();
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000D4366 File Offset: 0x000D2566
		public virtual void SetActiveScope(bool value)
		{
			if (this.usingScope != value)
			{
				this.usingScope = value;
				if (this.usingScope)
				{
					this.onEnableScope.Invoke();
					return;
				}
				this.onDisableScope.Invoke();
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000D4398 File Offset: 0x000D2598
		public virtual void SetScopeLookTarget(Vector3 point)
		{
			if (this.zoomScopeCamera)
			{
				Vector3 eulerAngles = Quaternion.LookRotation(point - this.zoomScopeCamera.transform.position, Vector3.up).eulerAngles;
				if (this.keepScopeCameraRotationZ)
				{
					eulerAngles.z = this.muzzle.transform.eulerAngles.z;
				}
				this.zoomScopeCamera.transform.eulerAngles = eulerAngles;
			}
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000D4410 File Offset: 0x000D2610
		public virtual void CancelReload()
		{
			if (this.reloadSource && this.reloadSource.isPlaying)
			{
				this.reloadSource.Stop();
			}
			this.onCancelReload.Invoke();
		}

		// Token: 0x040025C6 RID: 9670
		[vEditorToolbar("Weapon Settings", false, "", false, false)]
		public bool isLeftWeapon;

		// Token: 0x040025C7 RID: 9671
		[Tooltip("Hold Charge Input to charge")]
		public bool chargeWeapon;

		// Token: 0x040025C8 RID: 9672
		[vHideInInspector("chargeWeapon", false)]
		public bool autoShotOnFinishCharge;

		// Token: 0x040025C9 RID: 9673
		[vHideInInspector("chargeWeapon", false)]
		public float chargeSpeed = 0.1f;

		// Token: 0x040025CA RID: 9674
		[vHideInInspector("chargeWeapon", false)]
		public float chargeDamageMultiplier = 2f;

		// Token: 0x040025CB RID: 9675
		[vHideInInspector("chargeWeapon", false)]
		public bool changeVelocityByCharge = true;

		// Token: 0x040025CC RID: 9676
		[vHideInInspector("chargeWeapon", false)]
		public float chargeVelocityMultiplier = 2f;

		// Token: 0x040025CD RID: 9677
		[Tooltip("Change between automatic weapon or shot once")]
		[vHideInInspector("chargeWeapon", true)]
		public bool automaticWeapon;

		// Token: 0x040025CE RID: 9678
		[vEditorToolbar("Ammo", false, "", false, false)]
		public float reloadTime = 1f;

		// Token: 0x040025CF RID: 9679
		public bool reloadOneByOne;

		// Token: 0x040025D0 RID: 9680
		[SerializeField]
		[Tooltip("Max clip size of your weapon")]
		[FormerlySerializedAs("clipSize")]
		protected int _clipSize;

		// Token: 0x040025D1 RID: 9681
		[Tooltip("Check this to combine extra ammo with the current ammo, the Reload will not be used")]
		public bool dontUseReload;

		// Token: 0x040025D2 RID: 9682
		[vHideInInspector("dontUseReload", true)]
		[Tooltip("Automatically reload the weapon when it's empty")]
		public bool autoReload;

		// Token: 0x040025D3 RID: 9683
		[vHideInInspector("autoReload", false)]
		public vShooterWeapon.AutoReloadStyle autoReloadStyle;

		// Token: 0x040025D4 RID: 9684
		[Tooltip("Ammo ID - make sure your AmmoManager and ItemListData use the same ID")]
		[vHideInInspector("isInfinityAmmo", true)]
		public int ammoID;

		// Token: 0x040025D5 RID: 9685
		[vEditorToolbar("Weapon ID", false, "", false, false)]
		[Tooltip("What moveset the underbody will play")]
		public float moveSetID;

		// Token: 0x040025D6 RID: 9686
		[Tooltip("What moveset the uperbody will play")]
		public float upperBodyID;

		// Token: 0x040025D7 RID: 9687
		[Tooltip("What shot animation will trigger")]
		public float shotID;

		// Token: 0x040025D8 RID: 9688
		[Tooltip("What reload animation will play")]
		public int reloadID;

		// Token: 0x040025D9 RID: 9689
		[Tooltip("What equip animation will play")]
		public int equipID;

		// Token: 0x040025DA RID: 9690
		[vEditorToolbar("IK Options", false, "", false, false)]
		[Tooltip("IK will help the right hand to align where you actually is aiming")]
		public bool alignRightHandToAim = true;

		// Token: 0x040025DB RID: 9691
		[Tooltip("IK will help the right hand to align where you actually is aiming")]
		public bool alignRightUpperArmToAim = true;

		// Token: 0x040025DC RID: 9692
		public bool raycastAimTarget = true;

		// Token: 0x040025DD RID: 9693
		[Tooltip("Left IK on free locomotion")]
		public vShooterWeapon.IKLocomotionOptions freeIKOptions = new vShooterWeapon.IKLocomotionOptions();

		// Token: 0x040025DE RID: 9694
		[Tooltip("Left IK on strafe locomotion")]
		public vShooterWeapon.IKLocomotionOptions strafeIKOptions = new vShooterWeapon.IKLocomotionOptions();

		// Token: 0x040025DF RID: 9695
		[Tooltip("Left IK while attacking")]
		public bool useIkAttacking;

		// Token: 0x040025E0 RID: 9696
		[Tooltip("Left IK while Shot")]
		public bool disableIkOnShot;

		// Token: 0x040025E1 RID: 9697
		[Tooltip("Left IK while Aiming")]
		public bool useIKOnAiming = true;

		// Token: 0x040025E2 RID: 9698
		[Tooltip("Left IK Hand Target")]
		public Transform handIKTarget;

		// Token: 0x040025E3 RID: 9699
		[vEditorToolbar("Projectile", false, "", false, false)]
		[Tooltip("Assign the aimReference of your weapon")]
		public Transform aimReference;

		// Token: 0x040025E4 RID: 9700
		[vHelpBox("Only affects the camera from player, when player is aiming", vHelpBoxAttribute.MessageType.None)]
		[Tooltip("how much the camera will sway when aiming, 1 means no cameraSway and   means maxCameraSway from the ShooterManager")]
		[Range(0f, 1f)]
		[FormerlySerializedAs("precision")]
		public float cameraStability = 0.5f;

		// Token: 0x040025E5 RID: 9701
		[Tooltip("Creates a right recoil on the camera")]
		public float recoilRight = 1f;

		// Token: 0x040025E6 RID: 9702
		[Tooltip("Creates a left recoil on the camera")]
		public float recoilLeft = -1f;

		// Token: 0x040025E7 RID: 9703
		[Tooltip("Creates a up recoil on the camera")]
		public float recoilUp = 1f;

		// Token: 0x040025E8 RID: 9704
		[vEditorToolbar("Audio & VFX", false, "", false, false)]
		public AudioSource reloadSource;

		// Token: 0x040025E9 RID: 9705
		public AudioClip reloadClip;

		// Token: 0x040025EA RID: 9706
		public AudioClip finishReloadClip;

		// Token: 0x040025EB RID: 9707
		[vEditorToolbar("Scope View", false, "", false, false)]
		[vHelpBox("Third Person Controller Only", vHelpBoxAttribute.MessageType.Info)]
		public bool onlyUseScopeUIView;

		// Token: 0x040025EC RID: 9708
		[Tooltip("Check this bool to use an UI image for the scope, ex: snipers")]
		public bool useUI;

		// Token: 0x040025ED RID: 9709
		[Tooltip("You can create different Aim sprites and use for different weapons")]
		public int scopeID;

		// Token: 0x040025EE RID: 9710
		[Tooltip("The weight of shoot animation when using scope")]
		[Range(0f, 1f)]
		public float scopeShootAnimationWeight = 0.5f;

		// Token: 0x040025EF RID: 9711
		[Tooltip("change the FOV of the scope view\n **The calc is default value (60)-scopeZoom**")]
		[Range(-118f, 60f)]
		public float scopeZoom = 60f;

		// Token: 0x040025F0 RID: 9712
		[Tooltip("Change the FOV of the scope view Background\n **The calc is default value (60)-scopeZoom**")]
		[Range(-118f, 60f)]
		public float backGroundScopeZoom;

		// Token: 0x040025F1 RID: 9713
		[Tooltip("Used with the TPCamera to use a custom CameraState while aiming, if it's empty it will use the 'Aiming' CameraState.")]
		public string customAimCameraState;

		// Token: 0x040025F2 RID: 9714
		[Tooltip("Used with the TPCamera to use a custom CameraState while using scope view mode, if it's empty it will use the 'Aiming' CameraState.")]
		public string customScopeCameraState;

		// Token: 0x040025F3 RID: 9715
		[Tooltip("assign an empty transform with the pos/rot of your scope view")]
		public Transform scopeTarget;

		// Token: 0x040025F4 RID: 9716
		public Camera zoomScopeCamera;

		// Token: 0x040025F5 RID: 9717
		[vHelpBox("Keep Scope Camera Z is used to align z rotation of the zoomScopeCamera to z rotation of the weapon muzzle<color=red> (Projectile toolbar)</color>. if you want to align camera with Vector3.up in z rotation enable this.", vHelpBoxAttribute.MessageType.None)]
		public bool keepScopeCameraRotationZ = true;

		// Token: 0x040025F6 RID: 9718
		[HideInInspector]
		public bool isAiming;

		// Token: 0x040025F7 RID: 9719
		[HideInInspector]
		public bool usingScope;

		// Token: 0x040025F8 RID: 9720
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onReload;

		// Token: 0x040025F9 RID: 9721
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onCancelReload;

		// Token: 0x040025FA RID: 9722
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onFinishReload;

		// Token: 0x040025FB RID: 9723
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onFinishAmmo;

		// Token: 0x040025FC RID: 9724
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEnableAim;

		// Token: 0x040025FD RID: 9725
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onDisableAim;

		// Token: 0x040025FE RID: 9726
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEnableScope;

		// Token: 0x040025FF RID: 9727
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onDisableScope;

		// Token: 0x04002600 RID: 9728
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onFullPower;

		// Token: 0x04002601 RID: 9729
		[HideInInspector]
		public UnityEvent onDisable;

		// Token: 0x04002602 RID: 9730
		public vShooterWeapon.OnChangePowerCharger onPowerChargerChanged;

		// Token: 0x04002603 RID: 9731
		[HideInInspector]
		public Transform root;

		// Token: 0x04002604 RID: 9732
		[HideInInspector]
		public bool isSecundaryWeapon;

		// Token: 0x04002605 RID: 9733
		public vShooterWeapon.CheckAmmoHandle checkAmmoHandle;

		// Token: 0x04002606 RID: 9734
		public vShooterWeapon.ChangeAmmoHandle changeAmmoHandle;

		// Token: 0x0400260A RID: 9738
		[NonSerialized]
		private float testTime;

		// Token: 0x02000671 RID: 1649
		public enum AutoReloadStyle
		{
			// Token: 0x0400260C RID: 9740
			WhenAiming,
			// Token: 0x0400260D RID: 9741
			WhenShot,
			// Token: 0x0400260E RID: 9742
			WhenAmmoAvailable
		}

		// Token: 0x02000672 RID: 1650
		[Serializable]
		public class IKLocomotionOptions
		{
			// Token: 0x06002614 RID: 9748 RVA: 0x000D4508 File Offset: 0x000D2708
			public IKLocomotionOptions()
			{
				this.use = true;
				this.useOnIdle = true;
				this.useOnWalk = true;
				this.useOnRun = true;
				this.useOnSprint = true;
			}

			// Token: 0x0400260F RID: 9743
			public bool use = true;

			// Token: 0x04002610 RID: 9744
			[vHideInInspector("use", false)]
			public bool useOnIdle = true;

			// Token: 0x04002611 RID: 9745
			[vHideInInspector("use", false)]
			public bool useOnWalk = true;

			// Token: 0x04002612 RID: 9746
			[vHideInInspector("use", false)]
			public bool useOnRun = true;

			// Token: 0x04002613 RID: 9747
			[vHideInInspector("use", false)]
			public bool useOnSprint = true;
		}

		// Token: 0x02000673 RID: 1651
		[Serializable]
		public class OnChangePowerCharger : UnityEvent<float>
		{
		}

		// Token: 0x02000674 RID: 1652
		// (Invoke) Token: 0x06002617 RID: 9751
		public delegate bool CheckAmmoHandle(ref bool isValid, ref int totalAmmo);

		// Token: 0x02000675 RID: 1653
		// (Invoke) Token: 0x0600261B RID: 9755
		public delegate void ChangeAmmoHandle(int value);
	}
}
