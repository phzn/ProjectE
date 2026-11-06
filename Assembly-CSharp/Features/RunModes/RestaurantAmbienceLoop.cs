using System;
using System.Collections;
using Features.CielVoice;
using UnityEngine;
using UnityEngine.Audio;

namespace Features.RunModes
{
	// Token: 0x02000D6B RID: 3435
	[DisallowMultipleComponent]
	public sealed class RestaurantAmbienceLoop : MonoBehaviour
	{
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06006531 RID: 25905 RVA: 0x00285D93 File Offset: 0x00283F93
		public bool IsPlaying
		{
			get
			{
				return this._loop != null && this._loop.IsPlaying;
			}
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x00285DAA File Offset: 0x00283FAA
		private void Awake()
		{
			this.EnsureInitialized();
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x00285DB4 File Offset: 0x00283FB4
		public void Begin(float fadeSeconds = 0.6f)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.EnsureInitialized())
			{
				Debug.LogWarning("[RestaurantAmbienceLoop] Missing restaurant ambience clip.", this);
				return;
			}
			if (!this._loop.IsPlaying)
			{
				this._level = 0f;
				this._loop.Play();
			}
			this._stopAtSilence = false;
			this.FadeTo(1f, fadeSeconds);
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x00285E14 File Offset: 0x00284014
		public IEnumerator FadeOut(float seconds = 0.6f)
		{
			if (!this.IsPlaying)
			{
				yield break;
			}
			this._stopAtSilence = true;
			this.FadeTo(0f, seconds);
			while (this.IsPlaying && this._stopAtSilence)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x00285E2C File Offset: 0x0028402C
		public void StopImmediate()
		{
			this._level = (this._targetLevel = 0f);
			this._stopAtSilence = false;
			CrossfadeLoopPlayer loop = this._loop;
			if (loop == null)
			{
				return;
			}
			loop.Stop();
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x00285E64 File Offset: 0x00284064
		private void Update()
		{
			if (!this.IsPlaying)
			{
				return;
			}
			this._level = Mathf.MoveTowards(this._level, this._targetLevel, this._fadeRate * Time.unscaledDeltaTime);
			this._loop.Tick(this.backgroundVolume * this._level);
			if (this._stopAtSilence && this._level <= 0f)
			{
				this.StopImmediate();
			}
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x00285ED0 File Offset: 0x002840D0
		private void FadeTo(float level, float seconds)
		{
			this._targetLevel = level;
			this._fadeRate = ((seconds > 0f) ? (Mathf.Abs(level - this._level) / seconds) : 0f);
			if (seconds > 0f)
			{
				return;
			}
			this._level = level;
			this._loop.Tick(this.backgroundVolume * this._level);
			if (this._stopAtSilence && this._level <= 0f)
			{
				this.StopImmediate();
			}
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x00285F4C File Offset: 0x0028414C
		private bool EnsureInitialized()
		{
			if (this._loop != null)
			{
				return true;
			}
			if (this.clip == null)
			{
				return false;
			}
			this.sourceA = this.EnsureSource(this.sourceA, "Restaurant Ambience A");
			this.sourceB = this.EnsureSource(this.sourceB, "Restaurant Ambience B");
			float num = Mathf.Min(this.edgeTrim, this.clip.length * 0.1f);
			this._loop = new CrossfadeLoopPlayer(this.sourceA, this.sourceB, this.clip, this.loopCrossfade, num, this.clip.length - num);
			return true;
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x00285FF0 File Offset: 0x002841F0
		private AudioSource EnsureSource(AudioSource source, string childName)
		{
			if (source == null)
			{
				GameObject gameObject = new GameObject(childName);
				gameObject.transform.SetParent(base.transform, false);
				source = gameObject.AddComponent<AudioSource>();
			}
			source.playOnAwake = false;
			source.loop = false;
			source.spatialBlend = 0f;
			source.dopplerLevel = 0f;
			source.pitch = 1f;
			source.volume = 0f;
			source.outputAudioMixerGroup = this.outputMixer;
			return source;
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x0028606C File Offset: 0x0028426C
		private void OnDisable()
		{
			this.StopImmediate();
		}

		// Token: 0x04006791 RID: 26513
		[SerializeField]
		private AudioClip clip;

		// Token: 0x04006792 RID: 26514
		[SerializeField]
		private AudioSource sourceA;

		// Token: 0x04006793 RID: 26515
		[SerializeField]
		private AudioSource sourceB;

		// Token: 0x04006794 RID: 26516
		[SerializeField]
		private AudioMixerGroup outputMixer;

		// Token: 0x04006795 RID: 26517
		[SerializeField]
		[Range(0f, 1f)]
		private float backgroundVolume = 0.14f;

		// Token: 0x04006796 RID: 26518
		[Tooltip("Equal-power overlap hides the MP3 seam without a silent gap.")]
		[SerializeField]
		[Min(0.02f)]
		private float loopCrossfade = 1.5f;

		// Token: 0x04006797 RID: 26519
		[Tooltip("Skip the short encoder padding at both ends of the source recording.")]
		[SerializeField]
		[Min(0f)]
		private float edgeTrim = 0.04f;

		// Token: 0x04006798 RID: 26520
		private CrossfadeLoopPlayer _loop;

		// Token: 0x04006799 RID: 26521
		private float _level;

		// Token: 0x0400679A RID: 26522
		private float _targetLevel;

		// Token: 0x0400679B RID: 26523
		private float _fadeRate;

		// Token: 0x0400679C RID: 26524
		private bool _stopAtSilence;
	}
}
