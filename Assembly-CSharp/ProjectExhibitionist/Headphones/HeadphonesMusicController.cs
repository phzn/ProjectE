using System;
using Features.CielOS;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D5 RID: 2517
	[DisallowMultipleComponent]
	public sealed class HeadphonesMusicController : MonoBehaviour
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x001AC741 File Offset: 0x001AA941
		public CielOSAudioService AudioService
		{
			get
			{
				if (!(this._audioService != null))
				{
					return CielOSAudioService.Instance;
				}
				return this._audioService;
			}
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x001AC760 File Offset: 0x001AA960
		public static HeadphonesMusicController GetOrAdd(GameObject playerRoot)
		{
			if (playerRoot == null)
			{
				return null;
			}
			HeadphonesMusicController headphonesMusicController = playerRoot.GetComponentInChildren<HeadphonesMusicController>(true);
			if (headphonesMusicController == null)
			{
				headphonesMusicController = playerRoot.AddComponent<HeadphonesMusicController>();
			}
			return headphonesMusicController;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x001AC791 File Offset: 0x001AA991
		private void OnEnable()
		{
			HeadphonesEquipment.WearStateChanged += this.HandleWearStateChanged;
			this.SetWornState(HeadphonesEquipment.IsWorn, true);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x001AC7B0 File Offset: 0x001AA9B0
		private void OnDisable()
		{
			HeadphonesEquipment.WearStateChanged -= this.HandleWearStateChanged;
			this.ReleaseRuntimeOwnership();
			this._worn = false;
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x001AC7D0 File Offset: 0x001AA9D0
		private void Update()
		{
			bool isWorn = HeadphonesEquipment.IsWorn;
			if (isWorn != this._worn)
			{
				this.SetWornState(isWorn, false);
			}
			if (!this._worn)
			{
				return;
			}
			this.EnsureRuntimeOwnership(false);
			if (TextInputFocus.BlocksGameplayInput || CielOSController.AnyOpen || AudioListener.pause || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			CielOSAudioService audioService = this.AudioService;
			if (audioService == null)
			{
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.F1))
			{
				audioService.SkipPrevious();
			}
			if (GameplayInput.GetKeyDown(KeyCode.F2))
			{
				audioService.TogglePlayPause();
			}
			if (GameplayInput.GetKeyDown(KeyCode.F3))
			{
				audioService.Next();
			}
			if (GameplayInput.GetKeyDown(KeyCode.PageUp))
			{
				audioService.Volume += this.EffectiveVolumeStep;
			}
			if (GameplayInput.GetKeyDown(KeyCode.PageDown))
			{
				audioService.Volume -= this.EffectiveVolumeStep;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060046E2 RID: 18146 RVA: 0x001AC8A8 File Offset: 0x001AAAA8
		private float EffectiveVolumeStep
		{
			get
			{
				if (this.volumeStep <= 0f)
				{
					return 0.05f;
				}
				return this.volumeStep;
			}
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x001AC8C3 File Offset: 0x001AAAC3
		private void HandleWearStateChanged(bool isWorn)
		{
			this.SetWornState(isWorn, false);
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x001AC8D0 File Offset: 0x001AAAD0
		private void SetWornState(bool isWorn, bool force)
		{
			if (!force && this._worn == isWorn)
			{
				return;
			}
			bool worn = this._worn;
			this._worn = isWorn;
			if (this._worn)
			{
				this.EnsureRuntimeOwnership(true);
				return;
			}
			if (worn && this._audioService != null)
			{
				this._audioService.Stop();
			}
			this.ReleaseRuntimeOwnership();
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x001AC92C File Offset: 0x001AAB2C
		private void EnsureRuntimeOwnership(bool rescan)
		{
			CielOSAudioService instance = CielOSAudioService.Instance;
			if (this._audioService == null || instance == null || this._audioService != instance)
			{
				this.ReleaseAudioRetention();
				this._audioService = CielOSAudioService.Ensure(null);
				rescan = true;
			}
			if (this._audioService != null && !this._retentionHeld)
			{
				this._audioService.AcquirePlaybackRetention(this);
				this._retentionHeld = true;
			}
			if (rescan && this._audioService != null)
			{
				this._audioService.Rescan();
			}
			this.EnsureClothingInputBlock(rescan);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x001AC9C8 File Offset: 0x001AABC8
		private void EnsureClothingInputBlock(bool force)
		{
			if (this._clothingInput == null)
			{
				if (!force && Time.unscaledTime < this._nextClothingResolveTime)
				{
					return;
				}
				this._nextClothingResolveTime = Time.unscaledTime + 1f;
				this._clothingBlockHeld = false;
				this._clothingInput = this.ResolveClothingInput();
			}
			if (this._clothingInput == null || this._clothingBlockHeld)
			{
				return;
			}
			this._clothingInput.AcquireClothingToggleInputBlock(this);
			this._clothingBlockHeld = true;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x001ACA44 File Offset: 0x001AAC44
		private vThirdPersonInput ResolveClothingInput()
		{
			vThirdPersonInput vThirdPersonInput = base.GetComponentInParent<vThirdPersonInput>();
			if (vThirdPersonInput == null)
			{
				vThirdPersonInput = base.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (vThirdPersonInput != null)
			{
				return vThirdPersonInput;
			}
			HeadphonesEquipment headphonesEquipment = HeadphonesEquipment.Current;
			if (headphonesEquipment != null)
			{
				vThirdPersonInput = headphonesEquipment.GetComponentInParent<vThirdPersonInput>();
				if (vThirdPersonInput == null)
				{
					vThirdPersonInput = headphonesEquipment.GetComponentInChildren<vThirdPersonInput>(true);
				}
				if (vThirdPersonInput != null)
				{
					return vThirdPersonInput;
				}
			}
			return null;
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x001ACAA7 File Offset: 0x001AACA7
		private void ReleaseRuntimeOwnership()
		{
			this.ReleaseClothingInputBlock();
			this.ReleaseAudioRetention();
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x001ACAB5 File Offset: 0x001AACB5
		private void ReleaseAudioRetention()
		{
			if (this._retentionHeld && this._audioService != null)
			{
				this._audioService.ReleasePlaybackRetention(this);
			}
			this._retentionHeld = false;
			this._audioService = null;
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x001ACAE7 File Offset: 0x001AACE7
		private void ReleaseClothingInputBlock()
		{
			if (this._clothingBlockHeld && this._clothingInput != null)
			{
				this._clothingInput.ReleaseClothingToggleInputBlock(this);
			}
			this._clothingBlockHeld = false;
			this._clothingInput = null;
		}

		// Token: 0x04004668 RID: 18024
		public const KeyCode PreviousKey = KeyCode.F1;

		// Token: 0x04004669 RID: 18025
		public const KeyCode PlayPauseKey = KeyCode.F2;

		// Token: 0x0400466A RID: 18026
		public const KeyCode NextKey = KeyCode.F3;

		// Token: 0x0400466B RID: 18027
		public const KeyCode VolumeUpKey = KeyCode.PageUp;

		// Token: 0x0400466C RID: 18028
		public const KeyCode VolumeDownKey = KeyCode.PageDown;

		// Token: 0x0400466D RID: 18029
		public const float DefaultVolumeStep = 0.05f;

		// Token: 0x0400466E RID: 18030
		private const float ClothingResolveRetryInterval = 1f;

		// Token: 0x0400466F RID: 18031
		[SerializeField]
		[Range(0.01f, 0.25f)]
		private float volumeStep = 0.05f;

		// Token: 0x04004670 RID: 18032
		private CielOSAudioService _audioService;

		// Token: 0x04004671 RID: 18033
		private vThirdPersonInput _clothingInput;

		// Token: 0x04004672 RID: 18034
		private bool _retentionHeld;

		// Token: 0x04004673 RID: 18035
		private bool _clothingBlockHeld;

		// Token: 0x04004674 RID: 18036
		private bool _worn;

		// Token: 0x04004675 RID: 18037
		private float _nextClothingResolveTime;
	}
}
