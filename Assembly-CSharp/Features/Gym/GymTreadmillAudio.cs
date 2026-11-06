using System;
using UnityEngine;

namespace Features.Gym
{
	// Token: 0x020011B4 RID: 4532
	[DisallowMultipleComponent]
	public sealed class GymTreadmillAudio : MonoBehaviour
	{
		// Token: 0x06008B07 RID: 35591 RVA: 0x0038D1A0 File Offset: 0x0038B3A0
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			GymTreadmillAudio.s_clip = null;
			GymTreadmillAudio.s_clipSearched = false;
			GymTreadmillAudio.s_playerActiveCount = 0;
		}

		// Token: 0x06008B08 RID: 35592 RVA: 0x0038D1B4 File Offset: 0x0038B3B4
		internal static void Register(GymTreadmillBeltScroller scroller)
		{
			if (scroller != null && scroller.GetComponent<GymTreadmillAudio>() == null)
			{
				scroller.gameObject.AddComponent<GymTreadmillAudio>();
			}
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x0038D1D9 File Offset: 0x0038B3D9
		private void Awake()
		{
			this._scroller = base.GetComponent<GymTreadmillBeltScroller>();
			this._station = base.GetComponent<GymExerciseStation>();
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x0038D1F3 File Offset: 0x0038B3F3
		private void OnDisable()
		{
			this.ReleasePlayerActiveFlag();
			if (this._source != null && this._source.isPlaying)
			{
				this._source.Stop();
				this._source.volume = 0f;
			}
		}

		// Token: 0x06008B0B RID: 35595 RVA: 0x0038D234 File Offset: 0x0038B434
		private void Update()
		{
			if (this._scroller == null)
			{
				base.enabled = false;
				return;
			}
			bool flag = this._station != null && this._station.IsExerciseActive;
			if (flag != this._countedPlayerActive)
			{
				if (flag)
				{
					GymTreadmillAudio.s_playerActiveCount++;
				}
				else
				{
					this.ReleasePlayerActiveFlag();
				}
				this._countedPlayerActive = flag;
			}
			float num = this.ComputeTargetVolume(flag);
			this.UpdateSource(num, flag);
		}

		// Token: 0x06008B0C RID: 35596 RVA: 0x0038D2AB File Offset: 0x0038B4AB
		private float ComputeTargetVolume(bool playerOnThis)
		{
			if (!this._scroller.IsRunning)
			{
				return 0f;
			}
			if (playerOnThis)
			{
				return 0.16f;
			}
			if (GymTreadmillAudio.s_playerActiveCount > 0)
			{
				return 0f;
			}
			return 0.11f;
		}

		// Token: 0x06008B0D RID: 35597 RVA: 0x0038D2DC File Offset: 0x0038B4DC
		private void UpdateSource(float targetVolume, bool playerOnThis)
		{
			if (targetVolume <= 0f && (this._source == null || !this._source.isPlaying))
			{
				return;
			}
			if (!this.EnsureSource())
			{
				return;
			}
			float num = ((targetVolume > this._source.volume) ? (0.16f / Mathf.Max(0.01f, 0.7f)) : (0.16f / Mathf.Max(0.01f, 0.45f)));
			this._source.volume = Mathf.MoveTowards(this._source.volume, targetVolume, num * Time.deltaTime);
			float num2 = Mathf.InverseLerp(0.8f, 1.5f, this._scroller.CurrentSpeedMultiplier);
			float num3 = (playerOnThis ? Mathf.Lerp(0.94f, 1.12f, num2) : 1f);
			this._source.pitch = Mathf.MoveTowards(this._source.pitch, num3, 0.5f * Time.deltaTime);
			if (targetVolume > 0f && !this._source.isPlaying)
			{
				this._source.time = Random.value * Mathf.Max(0.01f, this._source.clip.length - 0.05f);
				this._source.Play();
				return;
			}
			if (targetVolume <= 0f && this._source.volume <= 0.001f && this._source.isPlaying)
			{
				this._source.Stop();
			}
		}

		// Token: 0x06008B0E RID: 35598 RVA: 0x0038D458 File Offset: 0x0038B658
		private bool EnsureSource()
		{
			if (this._source != null)
			{
				return true;
			}
			if (!GymTreadmillAudio.s_clipSearched)
			{
				GymTreadmillAudio.s_clipSearched = true;
				GymTreadmillAudio.s_clip = Resources.Load<AudioClip>("Gym/TreadmillLoop");
				if (GymTreadmillAudio.s_clip == null)
				{
					Debug.LogWarning("[GymTreadmillAudio] Loop clip missing at Resources/Gym/TreadmillLoop. Run Tools/Gym/Build Treadmill Loop Audio.");
				}
			}
			if (GymTreadmillAudio.s_clip == null)
			{
				base.enabled = false;
				return false;
			}
			GameObject gameObject = new GameObject("TreadmillLoopAudio");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = Vector3.up * 0.5f;
			this._source = gameObject.AddComponent<AudioSource>();
			this._source.clip = GymTreadmillAudio.s_clip;
			this._source.loop = true;
			this._source.playOnAwake = false;
			this._source.volume = 0f;
			this._source.pitch = 1f;
			this._source.spatialBlend = 1f;
			this._source.dopplerLevel = 0f;
			this._source.spread = 45f;
			this._source.rolloffMode = AudioRolloffMode.Linear;
			this._source.minDistance = 1.4f;
			this._source.maxDistance = 10f;
			return true;
		}

		// Token: 0x06008B0F RID: 35599 RVA: 0x0038D5A6 File Offset: 0x0038B7A6
		private void ReleasePlayerActiveFlag()
		{
			if (!this._countedPlayerActive)
			{
				return;
			}
			this._countedPlayerActive = false;
			GymTreadmillAudio.s_playerActiveCount = Mathf.Max(0, GymTreadmillAudio.s_playerActiveCount - 1);
		}

		// Token: 0x04008DE3 RID: 36323
		private const string ClipResourcePath = "Gym/TreadmillLoop";

		// Token: 0x04008DE4 RID: 36324
		private const float PlayerVolume = 0.16f;

		// Token: 0x04008DE5 RID: 36325
		private const float AmbientVolume = 0.11f;

		// Token: 0x04008DE6 RID: 36326
		private const float FadeInSeconds = 0.7f;

		// Token: 0x04008DE7 RID: 36327
		private const float FadeOutSeconds = 0.45f;

		// Token: 0x04008DE8 RID: 36328
		private const float MinDistance = 1.4f;

		// Token: 0x04008DE9 RID: 36329
		private const float MaxDistance = 10f;

		// Token: 0x04008DEA RID: 36330
		private const float MinSpeedMultiplier = 0.8f;

		// Token: 0x04008DEB RID: 36331
		private const float MaxSpeedMultiplier = 1.5f;

		// Token: 0x04008DEC RID: 36332
		private const float MinPitch = 0.94f;

		// Token: 0x04008DED RID: 36333
		private const float MaxPitch = 1.12f;

		// Token: 0x04008DEE RID: 36334
		private static AudioClip s_clip;

		// Token: 0x04008DEF RID: 36335
		private static bool s_clipSearched;

		// Token: 0x04008DF0 RID: 36336
		private static int s_playerActiveCount;

		// Token: 0x04008DF1 RID: 36337
		private GymTreadmillBeltScroller _scroller;

		// Token: 0x04008DF2 RID: 36338
		private GymExerciseStation _station;

		// Token: 0x04008DF3 RID: 36339
		private AudioSource _source;

		// Token: 0x04008DF4 RID: 36340
		private bool _countedPlayerActive;
	}
}
