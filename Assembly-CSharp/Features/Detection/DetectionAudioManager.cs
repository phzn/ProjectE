using System;
using System.Collections;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001264 RID: 4708
	public class DetectionAudioManager : MonoBehaviour
	{
		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x0600927B RID: 37499 RVA: 0x003C1B97 File Offset: 0x003BFD97
		// (set) Token: 0x0600927C RID: 37500 RVA: 0x003C1B9E File Offset: 0x003BFD9E
		public static DetectionAudioManager Instance { get; private set; }

		// Token: 0x0600927D RID: 37501 RVA: 0x003C1BA6 File Offset: 0x003BFDA6
		private void Awake()
		{
			if (DetectionAudioManager.Instance == null)
			{
				DetectionAudioManager.Instance = this;
				this.InitializeAudioSources();
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x0600927E RID: 37502 RVA: 0x003C1BD0 File Offset: 0x003BFDD0
		private void InitializeAudioSources()
		{
			if (this.ambientSource == null)
			{
				GameObject gameObject = new GameObject("DetectionAmbientSource");
				gameObject.transform.SetParent(base.transform);
				this.ambientSource = gameObject.AddComponent<AudioSource>();
			}
			this.ambientSource.loop = true;
			this.ambientSource.playOnAwake = false;
			this.ambientSource.spatialBlend = 0f;
			this.ambientSource.volume = 0f;
			if (this.stingerSource == null)
			{
				GameObject gameObject2 = new GameObject("DetectionStingerSource");
				gameObject2.transform.SetParent(base.transform);
				this.stingerSource = gameObject2.AddComponent<AudioSource>();
			}
			this.stingerSource.loop = false;
			this.stingerSource.playOnAwake = false;
			this.stingerSource.spatialBlend = 0f;
			if (this.heartbeatSource == null && this.enableHeartbeat)
			{
				GameObject gameObject3 = new GameObject("DetectionHeartbeatSource");
				gameObject3.transform.SetParent(base.transform);
				this.heartbeatSource = gameObject3.AddComponent<AudioSource>();
			}
			if (this.heartbeatSource != null)
			{
				this.heartbeatSource.loop = true;
				this.heartbeatSource.playOnAwake = false;
				this.heartbeatSource.spatialBlend = 0f;
				this.heartbeatSource.volume = 0f;
			}
		}

		// Token: 0x0600927F RID: 37503 RVA: 0x003C1D2D File Offset: 0x003BFF2D
		public void OnDetectionStarted()
		{
			if (this._isDetectionActive)
			{
				return;
			}
			this._isDetectionActive = true;
			this.PlayStinger(this.detectionStinger);
			this.StartAmbientAudio();
			Debug.Log("[DetectionAudioManager] Detection audio started");
		}

		// Token: 0x06009280 RID: 37504 RVA: 0x003C1D5B File Offset: 0x003BFF5B
		public void OnPlayerChoice()
		{
			this.PlayStinger(this.choiceStinger);
		}

		// Token: 0x06009281 RID: 37505 RVA: 0x003C1D69 File Offset: 0x003BFF69
		public void OnDetectionEnded()
		{
			if (!this._isDetectionActive)
			{
				return;
			}
			this._isDetectionActive = false;
			this.PlayStinger(this.exitStinger);
			this.StopAmbientAudio();
			this.StopHeartbeat();
			Debug.Log("[DetectionAudioManager] Detection audio ended");
		}

		// Token: 0x06009282 RID: 37506 RVA: 0x003C1D9D File Offset: 0x003BFF9D
		private void PlayStinger(AudioClip clip)
		{
			if (this.stingerSource != null && clip != null)
			{
				this.stingerSource.PlayOneShot(clip, this.stingerVolume);
			}
		}

		// Token: 0x06009283 RID: 37507 RVA: 0x003C1DC8 File Offset: 0x003BFFC8
		private void StartAmbientAudio()
		{
			if (this.ambientSource == null || this.detectionAmbient == null)
			{
				return;
			}
			if (this._ambientFadeCoroutine != null)
			{
				base.StopCoroutine(this._ambientFadeCoroutine);
			}
			this.ambientSource.clip = this.detectionAmbient;
			this.ambientSource.Play();
			this._ambientFadeCoroutine = base.StartCoroutine(this.FadeAudioSource(this.ambientSource, this.ambientVolume, this.ambientFadeInDuration));
		}

		// Token: 0x06009284 RID: 37508 RVA: 0x003C1E48 File Offset: 0x003C0048
		private void StopAmbientAudio()
		{
			if (this.ambientSource == null)
			{
				return;
			}
			if (this._ambientFadeCoroutine != null)
			{
				base.StopCoroutine(this._ambientFadeCoroutine);
			}
			this._ambientFadeCoroutine = base.StartCoroutine(this.FadeOutAndStop(this.ambientSource, this.ambientFadeOutDuration));
		}

		// Token: 0x06009285 RID: 37509 RVA: 0x003C1E98 File Offset: 0x003C0098
		private void StartHeartbeat()
		{
			if (this.heartbeatSource == null || this.heartbeatClip == null)
			{
				return;
			}
			this.heartbeatSource.clip = this.heartbeatClip;
			this.heartbeatSource.volume = this.heartbeatVolume;
			this.heartbeatSource.Play();
			if (this.scaleHeartbeatWithShame)
			{
				this._heartbeatCoroutine = base.StartCoroutine(this.UpdateHeartbeatVolume());
			}
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x003C1F0C File Offset: 0x003C010C
		private void StopHeartbeat()
		{
			if (this._heartbeatCoroutine != null)
			{
				base.StopCoroutine(this._heartbeatCoroutine);
				this._heartbeatCoroutine = null;
			}
			if (this.heartbeatSource != null)
			{
				base.StartCoroutine(this.FadeOutAndStop(this.heartbeatSource, 0.5f));
			}
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x003C1F5A File Offset: 0x003C015A
		private IEnumerator UpdateHeartbeatVolume()
		{
			while (this._isDetectionActive && this.heartbeatSource != null)
			{
				float num = 0f;
				if (PlayerEmotionController.Instance != null)
				{
					num = PlayerEmotionController.Instance.ShameLevel;
				}
				float num2 = 1f + num / 100f;
				this.heartbeatSource.volume = this.heartbeatVolume * num2;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}

		// Token: 0x06009288 RID: 37512 RVA: 0x003C1F69 File Offset: 0x003C0169
		private IEnumerator FadeAudioSource(AudioSource source, float targetVolume, float duration)
		{
			float startVolume = source.volume;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
				yield return null;
			}
			source.volume = targetVolume;
			yield break;
		}

		// Token: 0x06009289 RID: 37513 RVA: 0x003C1F86 File Offset: 0x003C0186
		private IEnumerator FadeOutAndStop(AudioSource source, float duration)
		{
			float startVolume = source.volume;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
				yield return null;
			}
			source.volume = 0f;
			source.Stop();
			yield break;
		}

		// Token: 0x0600928A RID: 37514 RVA: 0x003C1F9C File Offset: 0x003C019C
		public void FadeAmbientTo(float targetVolume, float duration)
		{
			if (this.ambientSource == null)
			{
				return;
			}
			if (this._ambientFadeCoroutine != null)
			{
				base.StopCoroutine(this._ambientFadeCoroutine);
			}
			this._ambientFadeCoroutine = base.StartCoroutine(this.FadeAudioSource(this.ambientSource, targetVolume, duration));
		}

		// Token: 0x0600928B RID: 37515 RVA: 0x003C1FDB File Offset: 0x003C01DB
		public void RestoreAmbientVolume(float duration)
		{
			this.FadeAmbientTo(this.ambientVolume, duration);
		}

		// Token: 0x0600928C RID: 37516 RVA: 0x003C1FEA File Offset: 0x003C01EA
		public void SetAmbientClip(AudioClip clip)
		{
			this.detectionAmbient = clip;
		}

		// Token: 0x0600928D RID: 37517 RVA: 0x003C1FF3 File Offset: 0x003C01F3
		public void SetDetectionStinger(AudioClip clip)
		{
			this.detectionStinger = clip;
		}

		// Token: 0x0600928E RID: 37518 RVA: 0x003C1FFC File Offset: 0x003C01FC
		public void FadeHeartbeatTo(float targetVolume, float duration)
		{
			if (this.heartbeatSource == null)
			{
				return;
			}
			if (this._heartbeatFadeCoroutine != null)
			{
				base.StopCoroutine(this._heartbeatFadeCoroutine);
			}
			if (this._heartbeatCoroutine != null)
			{
				base.StopCoroutine(this._heartbeatCoroutine);
				this._heartbeatCoroutine = null;
			}
			this._heartbeatFadeCoroutine = base.StartCoroutine(this.FadeAudioSource(this.heartbeatSource, targetVolume, duration));
		}

		// Token: 0x0600928F RID: 37519 RVA: 0x003C2064 File Offset: 0x003C0264
		public void RestoreHeartbeatVolume(float duration)
		{
			if (this.heartbeatSource == null)
			{
				return;
			}
			if (this._heartbeatFadeCoroutine != null)
			{
				base.StopCoroutine(this._heartbeatFadeCoroutine);
			}
			this._heartbeatFadeCoroutine = base.StartCoroutine(this.FadeAudioSource(this.heartbeatSource, this.heartbeatVolume, duration));
			if (this.scaleHeartbeatWithShame && this._isDetectionActive && this._heartbeatCoroutine == null)
			{
				this._heartbeatCoroutine = base.StartCoroutine(this.UpdateHeartbeatVolume());
			}
		}

		// Token: 0x06009290 RID: 37520 RVA: 0x003C20DD File Offset: 0x003C02DD
		private void OnDestroy()
		{
			if (DetectionAudioManager.Instance == this)
			{
				DetectionAudioManager.Instance = null;
			}
		}

		// Token: 0x04009701 RID: 38657
		[Header("Audio Sources")]
		[Tooltip("AudioSource for ambient/background music during detection")]
		[SerializeField]
		private AudioSource ambientSource;

		// Token: 0x04009702 RID: 38658
		[Tooltip("AudioSource for stinger/impact sounds")]
		[SerializeField]
		private AudioSource stingerSource;

		// Token: 0x04009703 RID: 38659
		[Header("Ambient Audio")]
		[Tooltip("Background ambient track to play during detection (loops)")]
		[SerializeField]
		private AudioClip detectionAmbient;

		// Token: 0x04009704 RID: 38660
		[Tooltip("Volume for ambient audio")]
		[Range(0f, 1f)]
		[SerializeField]
		private float ambientVolume = 0.5f;

		// Token: 0x04009705 RID: 38661
		[Tooltip("Fade in duration for ambient audio")]
		[SerializeField]
		private float ambientFadeInDuration = 0.5f;

		// Token: 0x04009706 RID: 38662
		[Tooltip("Fade out duration for ambient audio")]
		[SerializeField]
		private float ambientFadeOutDuration = 1f;

		// Token: 0x04009707 RID: 38663
		[Header("Stinger Audio")]
		[Tooltip("Sound played at the moment of detection (dramatic hit)")]
		[SerializeField]
		private AudioClip detectionStinger;

		// Token: 0x04009708 RID: 38664
		[Tooltip("Sound played when player makes a choice")]
		[SerializeField]
		private AudioClip choiceStinger;

		// Token: 0x04009709 RID: 38665
		[Tooltip("Sound played when detection sequence ends")]
		[SerializeField]
		private AudioClip exitStinger;

		// Token: 0x0400970A RID: 38666
		[Tooltip("Volume for stinger sounds")]
		[Range(0f, 1f)]
		[SerializeField]
		private float stingerVolume = 0.8f;

		// Token: 0x0400970B RID: 38667
		[Header("Heartbeat Effect")]
		[Tooltip("[DEPRECATO] L'heartbeat della detection e' stato rimosso: ora il battito e' gestito dal sistema dedicato (HeartbeatController / OrgasmHeartbeatDriver del MasturbationSystem), che lo pilota in base ai BPM. Questo flag resta a false e l'heartbeat qui non parte piu'. I metodi FadeHeartbeatTo/RestoreHeartbeatVolume restano no-op per compatibilita' (TVStudioController li chiama ancora).")]
		[SerializeField]
		private bool enableHeartbeat;

		// Token: 0x0400970C RID: 38668
		[Tooltip("Heartbeat audio clip")]
		[SerializeField]
		private AudioClip heartbeatClip;

		// Token: 0x0400970D RID: 38669
		[Tooltip("AudioSource for heartbeat (separate for looping)")]
		[SerializeField]
		private AudioSource heartbeatSource;

		// Token: 0x0400970E RID: 38670
		[Tooltip("Base heartbeat volume")]
		[Range(0f, 1f)]
		[SerializeField]
		private float heartbeatVolume = 0.4f;

		// Token: 0x0400970F RID: 38671
		[Tooltip("Heartbeat volume increases with shame level")]
		[SerializeField]
		private bool scaleHeartbeatWithShame = true;

		// Token: 0x04009710 RID: 38672
		private Coroutine _ambientFadeCoroutine;

		// Token: 0x04009711 RID: 38673
		private Coroutine _heartbeatCoroutine;

		// Token: 0x04009712 RID: 38674
		private Coroutine _heartbeatFadeCoroutine;

		// Token: 0x04009713 RID: 38675
		private bool _isDetectionActive;
	}
}
