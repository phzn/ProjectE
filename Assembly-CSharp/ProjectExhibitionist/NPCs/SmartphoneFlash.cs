using System;
using System.Collections;
using Features.NPC;
using UnityEngine;

namespace ProjectExhibitionist.NPCs
{
	// Token: 0x0200099A RID: 2458
	public class SmartphoneFlash : MonoBehaviour
	{
		// Token: 0x0600447D RID: 17533 RVA: 0x0019A862 File Offset: 0x00198A62
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.SmartphoneFlash_flashSound)
			{
				return this.flashSound;
			}
			throw new InvalidOperationException("Unsupported SmartphoneFlash POI binding: " + field.ToString());
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0019A88C File Offset: 0x00198A8C
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.SmartphoneFlash_flashSound)
			{
				this.flashSound = (AudioClip)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported SmartphoneFlash POI binding: " + field.ToString());
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600447F RID: 17535 RVA: 0x0019A8BC File Offset: 0x00198ABC
		// (remove) Token: 0x06004480 RID: 17536 RVA: 0x0019A8F0 File Offset: 0x00198AF0
		public static event Action<SmartphoneFlash> OnPhotoCaptured;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06004481 RID: 17537 RVA: 0x0019A924 File Offset: 0x00198B24
		// (remove) Token: 0x06004482 RID: 17538 RVA: 0x0019A958 File Offset: 0x00198B58
		public static event Action<SmartphoneFlash> OnVideoStarted;

		// Token: 0x06004483 RID: 17539 RVA: 0x0019A98B File Offset: 0x00198B8B
		private void Awake()
		{
			this.InitializeLight(this.spotLight, this.spotRange);
			this.InitializeLight(this.pointLight, this.pointRange);
			this.InitializeAudio();
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0019A9B8 File Offset: 0x00198BB8
		private void InitializeAudio()
		{
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
				if (this.audioSource == null)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
				}
			}
			this.audioSource.playOnAwake = false;
			this.audioSource.spatialBlend = 1f;
			this.audioSource.rolloffMode = AudioRolloffMode.Linear;
			this.audioSource.minDistance = 1f;
			this.audioSource.maxDistance = 15f;
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0019AA46 File Offset: 0x00198C46
		private void InitializeLight(Light light, float range)
		{
			if (light != null)
			{
				light.intensity = 0.01f;
				light.color = this.flashColor;
				light.range = range;
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0019AA70 File Offset: 0x00198C70
		public void PrewarmFlashShaders()
		{
			if (this.spotLight == null && this.pointLight == null)
			{
				return;
			}
			this.SetLightsIntensity(0.1f, 0.1f);
			this.SetLightsIntensity(0.01f, 0.01f);
			Debug.Log("[SmartphoneFlash] Shaders prewarmed.");
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0019AAC4 File Offset: 0x00198CC4
		public void TriggerFlash()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Action<SmartphoneFlash> onPhotoCaptured = SmartphoneFlash.OnPhotoCaptured;
			if (onPhotoCaptured != null)
			{
				onPhotoCaptured(this);
			}
			if (this.spotLight == null && this.pointLight == null)
			{
				Debug.LogWarning("[SmartphoneFlash] No lights assigned!");
				return;
			}
			if (this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
			}
			this.PlayFlashSound();
			if (this.useFlicker)
			{
				this.flashCoroutine = base.StartCoroutine(this.FlickerFlashRoutine());
				return;
			}
			this.flashCoroutine = base.StartCoroutine(this.SimpleFlashRoutine(1f));
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0019AB63 File Offset: 0x00198D63
		private void PlayFlashSound()
		{
			if (this.audioSource != null && this.flashSound != null)
			{
				this.audioSource.PlayOneShot(this.flashSound, this.flashVolume * this._volumeMultiplier);
			}
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0019AB9F File Offset: 0x00198D9F
		public void SetVolumeMultiplier(float multiplier)
		{
			this._volumeMultiplier = Mathf.Clamp01(multiplier);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0019ABAD File Offset: 0x00198DAD
		public void ResetVolumeMultiplier()
		{
			this._volumeMultiplier = 1f;
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600448B RID: 17547 RVA: 0x0019ABBA File Offset: 0x00198DBA
		public float VolumeMultiplier
		{
			get
			{
				return this._volumeMultiplier;
			}
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0019ABC4 File Offset: 0x00198DC4
		public void TriggerFlashWithMultiplier(float multiplier)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Action<SmartphoneFlash> onPhotoCaptured = SmartphoneFlash.OnPhotoCaptured;
			if (onPhotoCaptured != null)
			{
				onPhotoCaptured(this);
			}
			if (this.spotLight == null && this.pointLight == null)
			{
				Debug.LogWarning("[SmartphoneFlash] No lights assigned!");
				return;
			}
			if (this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
			}
			this.PlayFlashSound();
			this.flashCoroutine = base.StartCoroutine(this.SimpleFlashRoutine(multiplier));
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0019AC44 File Offset: 0x00198E44
		private void SetLightsIntensity(float spotValue, float pointValue)
		{
			if (this.spotLight != null)
			{
				this.spotLight.intensity = spotValue;
			}
			if (this.pointLight != null)
			{
				this.pointLight.intensity = pointValue;
			}
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0019AC7A File Offset: 0x00198E7A
		private IEnumerator SimpleFlashRoutine(float multiplier = 1f)
		{
			float currentSpotIntensity = this.spotIntensity * multiplier;
			float currentPointIntensity = this.pointIntensity * multiplier;
			this.SetLightsIntensity(currentSpotIntensity, currentPointIntensity);
			yield return SmartphoneFlash.WaitForSimulationSeconds(this.flashDuration);
			float fadeTime = this.flashDuration * 0.5f;
			float elapsed = 0f;
			while (elapsed < fadeTime)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				float num = elapsed / fadeTime;
				this.SetLightsIntensity(Mathf.Lerp(currentSpotIntensity, 0f, num), Mathf.Lerp(currentPointIntensity, 0f, num));
				yield return null;
			}
			this.SetLightsIntensity(0f, 0f);
			this.flashCoroutine = null;
			yield break;
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0019AC90 File Offset: 0x00198E90
		private IEnumerator FlickerFlashRoutine()
		{
			float flickerInterval = this.flashDuration / (float)(this.flickerCount * 2);
			int num;
			for (int i = 0; i < this.flickerCount; i = num + 1)
			{
				this.SetLightsIntensity(this.spotIntensity, this.pointIntensity);
				yield return SmartphoneFlash.WaitForSimulationSeconds(flickerInterval);
				if (i < this.flickerCount - 1)
				{
					this.SetLightsIntensity(0f, 0f);
					yield return SmartphoneFlash.WaitForSimulationSeconds(flickerInterval * 0.5f);
				}
				num = i;
			}
			float fadeTime = this.flashDuration * 0.3f;
			float elapsed = 0f;
			while (elapsed < fadeTime)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				float num2 = elapsed / fadeTime;
				this.SetLightsIntensity(Mathf.Lerp(this.spotIntensity, 0f, num2), Mathf.Lerp(this.pointIntensity, 0f, num2));
				yield return null;
			}
			this.SetLightsIntensity(0f, 0f);
			this.flashCoroutine = null;
			yield break;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0019AC9F File Offset: 0x00198E9F
		public void StopFlash()
		{
			if (this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
				this.flashCoroutine = null;
			}
			this.SetLightsIntensity(0f, 0f);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0019ACCC File Offset: 0x00198ECC
		public void StartVideoLight()
		{
			if (this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
				this.flashCoroutine = null;
			}
			this.isVideoRecording = true;
			this.SetLightsIntensity(this.videoSpotIntensity, this.videoPointIntensity);
			if (base.gameObject.activeInHierarchy)
			{
				Action<SmartphoneFlash> onVideoStarted = SmartphoneFlash.OnVideoStarted;
				if (onVideoStarted != null)
				{
					onVideoStarted(this);
				}
			}
			if (this.audioSource != null && this.videoStartSound != null)
			{
				this.audioSource.PlayOneShot(this.videoStartSound, this.videoVolume);
			}
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0019AD60 File Offset: 0x00198F60
		public void StopVideoLight()
		{
			this.isVideoRecording = false;
			if (this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
				this.flashCoroutine = null;
			}
			if (this.audioSource != null && this.videoStopSound != null)
			{
				this.audioSource.PlayOneShot(this.videoStopSound, this.videoVolume);
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.flashCoroutine = base.StartCoroutine(this.FadeOutRoutine(this.videoSpotIntensity, this.videoPointIntensity));
				return;
			}
			this.SetLightsIntensity(0f, 0f);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0019ADFE File Offset: 0x00198FFE
		private IEnumerator FadeOutRoutine(float fromSpot, float fromPoint)
		{
			float fadeTime = 0.3f;
			float elapsed = 0f;
			while (elapsed < fadeTime)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				float num = elapsed / fadeTime;
				this.SetLightsIntensity(Mathf.Lerp(fromSpot, 0f, num), Mathf.Lerp(fromPoint, 0f, num));
				yield return null;
			}
			this.SetLightsIntensity(0f, 0f);
			this.flashCoroutine = null;
			yield break;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0019AE1B File Offset: 0x0019901B
		private static IEnumerator WaitForSimulationSeconds(float seconds)
		{
			float elapsed = 0f;
			while (elapsed < seconds)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0019AE2A File Offset: 0x0019902A
		private void OnDisable()
		{
			this.StopFlash();
		}

		// Token: 0x04004426 RID: 17446
		[Header("Flash Lights")]
		[Tooltip("Main directional flash (Spot Light)")]
		[SerializeField]
		private Light spotLight;

		// Token: 0x04004427 RID: 17447
		[Tooltip("Ambient fill light (Point Light) for more realistic effect")]
		[SerializeField]
		private Light pointLight;

		// Token: 0x04004428 RID: 17448
		[Header("Spot Light Settings")]
		[SerializeField]
		private float spotIntensity = 8f;

		// Token: 0x04004429 RID: 17449
		[SerializeField]
		private float spotRange = 5f;

		// Token: 0x0400442A RID: 17450
		[Header("Point Light Settings")]
		[SerializeField]
		private float pointIntensity = 3f;

		// Token: 0x0400442B RID: 17451
		[SerializeField]
		private float pointRange = 2f;

		// Token: 0x0400442C RID: 17452
		[Header("Common Settings")]
		[SerializeField]
		private float flashDuration = 0.15f;

		// Token: 0x0400442D RID: 17453
		[SerializeField]
		private Color flashColor = Color.white;

		// Token: 0x0400442E RID: 17454
		[Header("Optional Effects")]
		[SerializeField]
		private bool useFlicker;

		// Token: 0x0400442F RID: 17455
		[SerializeField]
		private int flickerCount = 2;

		// Token: 0x04004430 RID: 17456
		[Header("Video Recording Light")]
		[SerializeField]
		private float videoSpotIntensity = 4f;

		// Token: 0x04004431 RID: 17457
		[SerializeField]
		private float videoPointIntensity = 1.5f;

		// Token: 0x04004432 RID: 17458
		[Header("Audio Settings")]
		[Tooltip("AudioSource for flash sounds (auto-created if null)")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004433 RID: 17459
		[Tooltip("Sound played when camera flash triggers")]
		[SerializeField]
		private AudioClip flashSound;

		// Token: 0x04004434 RID: 17460
		[Tooltip("Sound played when video recording starts")]
		[SerializeField]
		private AudioClip videoStartSound;

		// Token: 0x04004435 RID: 17461
		[Tooltip("Sound played when video recording stops")]
		[SerializeField]
		private AudioClip videoStopSound;

		// Token: 0x04004436 RID: 17462
		[Tooltip("Volume for flash sound")]
		[Range(0f, 1f)]
		[SerializeField]
		private float flashVolume = 1f;

		// Token: 0x04004437 RID: 17463
		[Tooltip("Volume for video sounds")]
		[Range(0f, 1f)]
		[SerializeField]
		private float videoVolume = 0.8f;

		// Token: 0x04004438 RID: 17464
		private Coroutine flashCoroutine;

		// Token: 0x04004439 RID: 17465
		private bool isVideoRecording;

		// Token: 0x0400443A RID: 17466
		private float _volumeMultiplier = 1f;

		// Token: 0x0400443B RID: 17467
		private float _originalFlashVolume;

		// Token: 0x0400443C RID: 17468
		private float _originalVideoVolume;
	}
}
