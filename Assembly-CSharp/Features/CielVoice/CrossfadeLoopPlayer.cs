using System;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200136A RID: 4970
	public class CrossfadeLoopPlayer
	{
		// Token: 0x060099E0 RID: 39392 RVA: 0x003F0470 File Offset: 0x003EE670
		public CrossfadeLoopPlayer(AudioSource a, AudioSource b, AudioClip clip, float overlapSeconds, float loopStartSeconds = 0f, float loopEndSeconds = 0f)
		{
			this._a = a;
			this._b = b;
			this._clip = clip;
			this._overlap = Mathf.Max(0.02f, overlapSeconds);
			this._loopStart = Mathf.Max(0f, loopStartSeconds);
			this._loopEnd = Mathf.Max(0f, loopEndSeconds);
			CrossfadeLoopPlayer.WarmClip(this._clip);
			this.ConfigureSource(this._a);
			this.ConfigureSource(this._b);
		}

		// Token: 0x060099E1 RID: 39393 RVA: 0x003F04F1 File Offset: 0x003EE6F1
		private void ConfigureSource(AudioSource s)
		{
			if (s == null)
			{
				return;
			}
			s.loop = false;
			s.playOnAwake = false;
			s.clip = this._clip;
			s.volume = 0f;
		}

		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x060099E2 RID: 39394 RVA: 0x003F0522 File Offset: 0x003EE722
		public bool IsPlaying
		{
			get
			{
				return this._playing;
			}
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x060099E3 RID: 39395 RVA: 0x003F052A File Offset: 0x003EE72A
		public float LoopStartSeconds
		{
			get
			{
				return this.ResolveLoopStart();
			}
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x060099E4 RID: 39396 RVA: 0x003F0532 File Offset: 0x003EE732
		public float LoopEndSeconds
		{
			get
			{
				return this.ResolveLoopEnd();
			}
		}

		// Token: 0x060099E5 RID: 39397 RVA: 0x003F053C File Offset: 0x003EE73C
		public void SetClip(AudioClip clip)
		{
			this._clip = clip;
			CrossfadeLoopPlayer.WarmClip(this._clip);
			if (this._a != null)
			{
				this._a.clip = clip;
			}
			if (this._b != null)
			{
				this._b.clip = clip;
			}
		}

		// Token: 0x060099E6 RID: 39398 RVA: 0x003F058F File Offset: 0x003EE78F
		private static void WarmClip(AudioClip clip)
		{
			if (clip != null && clip.loadState == AudioDataLoadState.Unloaded)
			{
				clip.LoadAudioData();
			}
		}

		// Token: 0x060099E7 RID: 39399 RVA: 0x003F05AC File Offset: 0x003EE7AC
		public void Play()
		{
			if (this._clip == null || this._a == null || this._b == null)
			{
				return;
			}
			if (this._playing)
			{
				return;
			}
			this._playing = true;
			this._bIsActive = false;
			this._a.clip = this._clip;
			this._b.Stop();
			this.SeekToLoopStart(this._a);
			this._a.Play();
			this.ScheduleSwap(this._a);
		}

		// Token: 0x060099E8 RID: 39400 RVA: 0x003F063A File Offset: 0x003EE83A
		public void Pause()
		{
			if (!this._playing)
			{
				return;
			}
			AudioSource a = this._a;
			if (a != null)
			{
				a.Pause();
			}
			AudioSource b = this._b;
			if (b != null)
			{
				b.Pause();
			}
			this._playing = false;
		}

		// Token: 0x060099E9 RID: 39401 RVA: 0x003F0670 File Offset: 0x003EE870
		public void Resume()
		{
			if (this._clip == null)
			{
				return;
			}
			if (this._playing)
			{
				return;
			}
			AudioSource a = this._a;
			if (a != null)
			{
				a.UnPause();
			}
			AudioSource b = this._b;
			if (b != null)
			{
				b.UnPause();
			}
			if ((this._a == null || !this._a.isPlaying) && (this._b == null || !this._b.isPlaying))
			{
				this.Play();
				return;
			}
			this._playing = true;
			this.ScheduleSwap(this._bIsActive ? this._b : this._a);
		}

		// Token: 0x060099EA RID: 39402 RVA: 0x003F0718 File Offset: 0x003EE918
		public void Stop()
		{
			AudioSource a = this._a;
			if (a != null)
			{
				a.Stop();
			}
			AudioSource b = this._b;
			if (b != null)
			{
				b.Stop();
			}
			if (this._a != null)
			{
				this._a.volume = 0f;
			}
			if (this._b != null)
			{
				this._b.volume = 0f;
			}
			this._playing = false;
		}

		// Token: 0x060099EB RID: 39403 RVA: 0x003F078C File Offset: 0x003EE98C
		private void ScheduleSwap(AudioSource active)
		{
			if (active == null || active.clip == null)
			{
				return;
			}
			double dspTime = AudioSettings.dspTime;
			double num = (double)(this.ResolveLoopEnd() - CrossfadeLoopPlayer.PlaybackTime(active));
			this._nextSwapDspTime = dspTime + (double)Mathf.Max(0f, (float)num - this.EffectiveOverlap());
		}

		// Token: 0x060099EC RID: 39404 RVA: 0x003F07E4 File Offset: 0x003EE9E4
		public void Tick(float targetVolume)
		{
			if (!this._playing || this._clip == null)
			{
				return;
			}
			AudioSource audioSource = (this._bIsActive ? this._b : this._a);
			AudioSource audioSource2 = (this._bIsActive ? this._a : this._b);
			if (audioSource == null || audioSource2 == null)
			{
				return;
			}
			if (AudioSettings.dspTime >= this._nextSwapDspTime && !audioSource2.isPlaying)
			{
				audioSource2.clip = this._clip;
				this.SeekToLoopStart(audioSource2);
				audioSource2.Play();
			}
			if (audioSource2.isPlaying)
			{
				float num = ((audioSource.clip != null) ? (this.ResolveLoopEnd() - CrossfadeLoopPlayer.PlaybackTime(audioSource)) : 0f);
				float num2 = Mathf.Clamp01(1f - num / this.EffectiveOverlap());
				float num3 = Mathf.Cos(num2 * 0.5f * 3.1415927f);
				float num4 = Mathf.Sin(num2 * 0.5f * 3.1415927f);
				audioSource.volume = targetVolume * num3;
				audioSource2.volume = targetVolume * num4;
				if (!audioSource.isPlaying || num <= 0f)
				{
					audioSource.Stop();
					audioSource.volume = 0f;
					this._bIsActive = !this._bIsActive;
					this.ScheduleSwap(this._bIsActive ? this._b : this._a);
					return;
				}
			}
			else
			{
				audioSource.volume = targetVolume;
				audioSource2.volume = 0f;
			}
		}

		// Token: 0x060099ED RID: 39405 RVA: 0x003F0958 File Offset: 0x003EEB58
		private float ResolveLoopStart()
		{
			if (this._clip == null || this._clip.length <= 0f)
			{
				return 0f;
			}
			return Mathf.Clamp(this._loopStart, 0f, Mathf.Max(0f, this._clip.length - 0.02f));
		}

		// Token: 0x060099EE RID: 39406 RVA: 0x003F09B8 File Offset: 0x003EEBB8
		private float ResolveLoopEnd()
		{
			if (this._clip == null || this._clip.length <= 0f)
			{
				return 0f;
			}
			float num = this.ResolveLoopStart();
			float num2 = ((this._loopEnd > 0f) ? this._loopEnd : this._clip.length);
			float num3 = Mathf.Min(num + 0.02f, this._clip.length);
			return Mathf.Clamp(num2, num3, this._clip.length);
		}

		// Token: 0x060099EF RID: 39407 RVA: 0x003F0A3C File Offset: 0x003EEC3C
		private float EffectiveOverlap()
		{
			float num = Mathf.Max(0.02f, this.ResolveLoopEnd() - this.ResolveLoopStart());
			return Mathf.Min(this._overlap, num * 0.5f);
		}

		// Token: 0x060099F0 RID: 39408 RVA: 0x003F0A74 File Offset: 0x003EEC74
		private static float PlaybackTime(AudioSource source)
		{
			if (source == null || source.clip == null || source.clip.frequency <= 0)
			{
				return 0f;
			}
			return (float)source.timeSamples / (float)source.clip.frequency;
		}

		// Token: 0x060099F1 RID: 39409 RVA: 0x003F0AC0 File Offset: 0x003EECC0
		private void SeekToLoopStart(AudioSource source)
		{
			if (source == null || this._clip == null || this._clip.samples <= 0)
			{
				return;
			}
			int num = Mathf.Clamp(Mathf.RoundToInt(this.ResolveLoopStart() * (float)this._clip.frequency), 0, this._clip.samples - 1);
			source.timeSamples = num;
		}

		// Token: 0x04009EEA RID: 40682
		private readonly AudioSource _a;

		// Token: 0x04009EEB RID: 40683
		private readonly AudioSource _b;

		// Token: 0x04009EEC RID: 40684
		private AudioClip _clip;

		// Token: 0x04009EED RID: 40685
		private readonly float _overlap;

		// Token: 0x04009EEE RID: 40686
		private readonly float _loopStart;

		// Token: 0x04009EEF RID: 40687
		private readonly float _loopEnd;

		// Token: 0x04009EF0 RID: 40688
		private bool _playing;

		// Token: 0x04009EF1 RID: 40689
		private bool _bIsActive;

		// Token: 0x04009EF2 RID: 40690
		private double _nextSwapDspTime;
	}
}
