using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Features.RunModes;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B83 RID: 2947
	public sealed class SpawnedVehicleLifetime : MonoBehaviour
	{
		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x060053F0 RID: 21488 RVA: 0x002093E5 File Offset: 0x002075E5
		public float LifetimeSeconds
		{
			get
			{
				return 85f;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x060053F1 RID: 21489 RVA: 0x002093EC File Offset: 0x002075EC
		public float RemainingSeconds
		{
			get
			{
				return this._remainingSeconds;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x060053F2 RID: 21490 RVA: 0x002093F4 File Offset: 0x002075F4
		public bool HasStarted
		{
			get
			{
				return this._hasStarted;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x060053F3 RID: 21491 RVA: 0x002093FC File Offset: 0x002075FC
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x060053F4 RID: 21492 RVA: 0x00209404 File Offset: 0x00207604
		public bool IsExpiring
		{
			get
			{
				return this._isExpiring;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x060053F5 RID: 21493 RVA: 0x0020940C File Offset: 0x0020760C
		public bool OwnsScreenHud
		{
			get
			{
				return VehicleLifetimeHUD.Instance != null && VehicleLifetimeHUD.Instance.OwnerInstanceId == base.GetInstanceID();
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x060053F6 RID: 21494 RVA: 0x0020942F File Offset: 0x0020762F
		public int ScreenHudOwnerInstanceId
		{
			get
			{
				if (!(VehicleLifetimeHUD.Instance != null))
				{
					return 0;
				}
				return VehicleLifetimeHUD.Instance.OwnerInstanceId;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x060053F7 RID: 21495 RVA: 0x0020944A File Offset: 0x0020764A
		public bool CanDestroySafely
		{
			get
			{
				return !this.IsVehicleMounted() && (this._lastKnownRider == null || !this._lastKnownRider.IsChildOf(base.transform));
			}
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x0020947A File Offset: 0x0020767A
		private void Awake()
		{
			this._remainingSeconds = 85f;
			this.CacheRideBindings();
			this.ResolvePresentation();
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00209494 File Offset: 0x00207694
		internal void Initialize(VehicleSpawnManager owner)
		{
			this.ReleaseScreenHud();
			this._owner = owner;
			this._expirySfx = ((owner != null) ? owner.VehicleExpirySfx : null);
			this._expirySfxVolume = ((owner != null) ? owner.VehicleExpirySfxVolume : 0f);
			this._expirySfxPitch = ((owner != null) ? owner.VehicleExpirySfxPitch : 1f);
			this._remainingSeconds = 85f;
			this._hasStarted = false;
			this._isPaused = false;
			this._isExpiring = false;
			this._lastKnownRider = null;
			this.CacheRideBindings();
			this.ResolvePresentation();
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x00209534 File Offset: 0x00207734
		private void Update()
		{
			if (this._isExpiring)
			{
				return;
			}
			bool flag = this.IsVehicleMounted();
			if (flag)
			{
				this.CaptureCurrentRider();
			}
			bool isPaused = GameplayDurationPause.IsPaused;
			bool flag2 = this.AdvanceCountdown(Time.deltaTime, flag, isPaused);
			if (RunModeService.UnlimitedVehicleDuration)
			{
				this._isPaused = true;
				this.ReleaseScreenHud();
				return;
			}
			if (!this._hasStarted)
			{
				return;
			}
			this._isPaused = !flag || isPaused;
			this.SyncScreenHud(flag, this._isPaused);
			if (flag2)
			{
				this.BeginExpiration();
			}
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x002095B0 File Offset: 0x002077B0
		private bool AdvanceCountdown(float deltaTime, bool mounted, bool durationPaused)
		{
			if (this._isExpiring)
			{
				return true;
			}
			if (RunModeService.UnlimitedVehicleDuration)
			{
				return false;
			}
			if (!this._hasStarted)
			{
				if (!mounted)
				{
					return false;
				}
				this._hasStarted = true;
				this._remainingSeconds = 85f;
			}
			if (mounted && !durationPaused && deltaTime > 0f)
			{
				this._remainingSeconds = Mathf.Max(0f, this._remainingSeconds - deltaTime);
			}
			return this._remainingSeconds <= 0f;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00209625 File Offset: 0x00207825
		private void BeginExpiration()
		{
			if (this._isExpiring)
			{
				return;
			}
			this._isExpiring = true;
			this._remainingSeconds = 0f;
			this._isPaused = false;
			this.ReleaseScreenHud();
			base.StartCoroutine(this.ExpireSafely());
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x0020965C File Offset: 0x0020785C
		private IEnumerator ExpireSafely()
		{
			float retrySeconds = 0f;
			bool warned = false;
			while (!this.CanDestroySafely)
			{
				this.CaptureCurrentRider();
				this.ForceDismount();
				yield return null;
				retrySeconds += Time.unscaledDeltaTime;
				if (!warned && retrySeconds >= 3f)
				{
					warned = true;
					Debug.LogWarning("[SpawnedVehicleLifetime] Waiting for a safe dismount before destroying '" + base.name + "'.", this);
				}
			}
			this.DisableRideInteractions();
			yield return null;
			while (!this.CanDestroySafely)
			{
				this.ForceDismount();
				yield return null;
			}
			this.PlayExpirySfx();
			if (this._owner != null)
			{
				this._owner.ExpireVehicleFromLifetime(base.gameObject);
				yield break;
			}
			base.gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				Object.DestroyImmediate(base.gameObject);
			}
			yield break;
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x0020966C File Offset: 0x0020786C
		private void PlayExpirySfx()
		{
			if (this._expirySfx == null || this._expirySfxVolume <= 0f)
			{
				return;
			}
			float num = Mathf.Clamp(this._expirySfxPitch, 0.5f, 2f);
			GameObject gameObject = new GameObject("Vehicle Expiry One Shot");
			gameObject.transform.position = base.transform.position;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.clip = this._expirySfx;
			audioSource.volume = Mathf.Clamp01(this._expirySfxVolume);
			audioSource.pitch = num;
			audioSource.spatialBlend = 1f;
			audioSource.Play();
			Object.Destroy(gameObject, SpawnedVehicleLifetime.CalculateExpiryOneShotLifetime(this._expirySfx.length, num));
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00209729 File Offset: 0x00207929
		private static float CalculateExpiryOneShotLifetime(float clipLength, float pitch)
		{
			return Mathf.Max(0f, clipLength) / Mathf.Max(0.01f, Mathf.Abs(pitch)) + 0.1f;
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00209750 File Offset: 0x00207950
		private bool IsVehicleMounted()
		{
			for (int i = 0; i < this._rideInteractables.Count; i++)
			{
				MonoBehaviour monoBehaviour = this._rideInteractables[i];
				if (monoBehaviour != null)
				{
					IInteractable interactable = monoBehaviour as IInteractable;
					if (interactable != null && interactable.IsBeingUsed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x002097A0 File Offset: 0x002079A0
		private void ForceDismount()
		{
			for (int i = 0; i < this._vehicleControllers.Length; i++)
			{
				vVehicleController vVehicleController = this._vehicleControllers[i];
				if (!(vVehicleController == null) && vVehicleController.IsBeingRidden)
				{
					this.RememberRider(vVehicleController.CurrentRider);
					vVehicleController.ForceDismount();
				}
			}
			for (int j = 0; j < this._bikeControllers.Length; j++)
			{
				vBikeController vBikeController = this._bikeControllers[j];
				if (!(vBikeController == null) && vBikeController.IsBeingRidden)
				{
					this.RememberRider(vBikeController.CurrentRider);
					vBikeController.ForceDismount();
				}
			}
			for (int k = 0; k < this._skateboards.Length; k++)
			{
				SkateboardInteractable skateboardInteractable = this._skateboards[k];
				if (!(skateboardInteractable == null) && skateboardInteractable.IsBeingUsed)
				{
					this.RememberRider(skateboardInteractable.CurrentRider);
					skateboardInteractable.ForceCrashDismount();
				}
			}
			for (int l = 0; l < this._rollerSkates.Length; l++)
			{
				SimpleRollerSkatesInteractable simpleRollerSkatesInteractable = this._rollerSkates[l];
				if (!(simpleRollerSkatesInteractable == null) && simpleRollerSkatesInteractable.IsBeingUsed)
				{
					this.RememberRider(simpleRollerSkatesInteractable.CurrentRider);
					simpleRollerSkatesInteractable.ForceCrashDismount();
				}
			}
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x002098C0 File Offset: 0x00207AC0
		private void CaptureCurrentRider()
		{
			for (int i = 0; i < this._vehicleControllers.Length; i++)
			{
				if (this._vehicleControllers[i] != null)
				{
					this.RememberRider(this._vehicleControllers[i].CurrentRider);
				}
			}
			for (int j = 0; j < this._bikeControllers.Length; j++)
			{
				if (this._bikeControllers[j] != null)
				{
					this.RememberRider(this._bikeControllers[j].CurrentRider);
				}
			}
			for (int k = 0; k < this._skateboards.Length; k++)
			{
				if (this._skateboards[k] != null)
				{
					this.RememberRider(this._skateboards[k].CurrentRider);
				}
			}
			for (int l = 0; l < this._rollerSkates.Length; l++)
			{
				if (this._rollerSkates[l] != null)
				{
					this.RememberRider(this._rollerSkates[l].CurrentRider);
				}
			}
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x002099A5 File Offset: 0x00207BA5
		private void RememberRider(Component rider)
		{
			if (rider != null)
			{
				this._lastKnownRider = rider.transform;
			}
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x002099BC File Offset: 0x00207BBC
		private void CacheRideBindings()
		{
			this._vehicleControllers = base.GetComponentsInChildren<vVehicleController>(true);
			this._bikeControllers = base.GetComponentsInChildren<vBikeController>(true);
			this._skateboards = base.GetComponentsInChildren<SkateboardInteractable>(true);
			this._rollerSkates = base.GetComponentsInChildren<SimpleRollerSkatesInteractable>(true);
			this._rideInteractables.Clear();
			foreach (MonoBehaviour monoBehaviour in base.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour != null && monoBehaviour is IInteractable)
				{
					this._rideInteractables.Add(monoBehaviour);
				}
			}
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00209A40 File Offset: 0x00207C40
		private void DisableRideInteractions()
		{
			for (int i = 0; i < this._rideInteractables.Count; i++)
			{
				MonoBehaviour monoBehaviour = this._rideInteractables[i];
				if (monoBehaviour != null)
				{
					monoBehaviour.enabled = false;
				}
			}
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00209A80 File Offset: 0x00207C80
		private void ResolvePresentation()
		{
			this._hudIcon = null;
			this._hudGlyph = this.ResolveGlyph();
			vVehicleInteractable[] componentsInChildren = base.GetComponentsInChildren<vVehicleInteractable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == null) && !(componentsInChildren[i].SourceItem == null))
				{
					this._hudIcon = componentsInChildren[i].SourceItem.icon;
					return;
				}
			}
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00209AE8 File Offset: 0x00207CE8
		private VehicleLifetimeGlyph ResolveGlyph()
		{
			if (this._skateboards != null && this._skateboards.Length != 0)
			{
				return VehicleLifetimeGlyph.Skateboard;
			}
			if (this._rollerSkates != null && this._rollerSkates.Length != 0)
			{
				return VehicleLifetimeGlyph.RollerSkates;
			}
			if (this._bikeControllers != null && this._bikeControllers.Length != 0)
			{
				return VehicleLifetimeGlyph.Bicycle;
			}
			if (this._vehicleControllers != null && this._vehicleControllers.Length != 0 && this._vehicleControllers[0] != null)
			{
				switch (this._vehicleControllers[0].Type)
				{
				case vVehicleController.VehicleType.Bicycle:
					return VehicleLifetimeGlyph.Bicycle;
				case vVehicleController.VehicleType.Scooter:
					return VehicleLifetimeGlyph.Scooter;
				case vVehicleController.VehicleType.KickScooter:
					return VehicleLifetimeGlyph.KickScooter;
				case vVehicleController.VehicleType.Motorbike:
					return VehicleLifetimeGlyph.Motorbike;
				case vVehicleController.VehicleType.RollerSkates:
					return VehicleLifetimeGlyph.RollerSkates;
				case vVehicleController.VehicleType.Unicycle:
					return VehicleLifetimeGlyph.Unicycle;
				}
			}
			return VehicleLifetimeGlyph.Generic;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x00209B8A File Offset: 0x00207D8A
		private void SyncScreenHud(bool mounted, bool paused)
		{
			if (!mounted || RunModeService.UnlimitedVehicleDuration)
			{
				this.ReleaseScreenHud();
				return;
			}
			VehicleLifetimeHUD orCreate = VehicleLifetimeHUD.GetOrCreate();
			orCreate.Claim(this, this._hudIcon, this._hudGlyph);
			orCreate.RefreshOwner(this, this._remainingSeconds, 85f, paused);
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x00209BC8 File Offset: 0x00207DC8
		private void ReleaseScreenHud()
		{
			VehicleLifetimeHUD instance = VehicleLifetimeHUD.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Release(this);
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x00209BDB File Offset: 0x00207DDB
		private void OnDisable()
		{
			this.ReleaseScreenHud();
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x00209BDB File Offset: 0x00207DDB
		private void OnDestroy()
		{
			this.ReleaseScreenHud();
		}

		// Token: 0x04005469 RID: 21609
		public const float DefaultLifetimeSeconds = 85f;

		// Token: 0x0400546A RID: 21610
		private readonly List<MonoBehaviour> _rideInteractables = new List<MonoBehaviour>(4);

		// Token: 0x0400546B RID: 21611
		private VehicleSpawnManager _owner;

		// Token: 0x0400546C RID: 21612
		private vVehicleController[] _vehicleControllers;

		// Token: 0x0400546D RID: 21613
		private vBikeController[] _bikeControllers;

		// Token: 0x0400546E RID: 21614
		private SkateboardInteractable[] _skateboards;

		// Token: 0x0400546F RID: 21615
		private SimpleRollerSkatesInteractable[] _rollerSkates;

		// Token: 0x04005470 RID: 21616
		private Transform _lastKnownRider;

		// Token: 0x04005471 RID: 21617
		private Sprite _hudIcon;

		// Token: 0x04005472 RID: 21618
		private VehicleLifetimeGlyph _hudGlyph;

		// Token: 0x04005473 RID: 21619
		private float _remainingSeconds = 85f;

		// Token: 0x04005474 RID: 21620
		private AudioClip _expirySfx;

		// Token: 0x04005475 RID: 21621
		private float _expirySfxVolume;

		// Token: 0x04005476 RID: 21622
		private float _expirySfxPitch = 1f;

		// Token: 0x04005477 RID: 21623
		private bool _hasStarted;

		// Token: 0x04005478 RID: 21624
		private bool _isPaused;

		// Token: 0x04005479 RID: 21625
		private bool _isExpiring;
	}
}
