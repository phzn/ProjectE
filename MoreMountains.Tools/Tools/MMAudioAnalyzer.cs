using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x020000B4 RID: 180
	[AddComponentMenu("More Mountains/Tools/Audio/MMAudioAnalyzer")]
	public class MMAudioAnalyzer : MonoBehaviour
	{
		// Token: 0x06000385 RID: 901 RVA: 0x00011A80 File Offset: 0x0000FC80
		public virtual void FindPeaks()
		{
			float num = 0f;
			while (num < this.TargetAudioSource.clip.length)
			{
				this.TargetAudioSource.time = num;
				this.TargetAudioSource.GetSpectrumData(this.RawSpectrum, 0, this.Window);
				num += this.SampleInterval;
				this.ComputeBandLevels();
				PeaksSaver.Peaks = this.BandPeaks;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00011AE6 File Offset: 0x0000FCE6
		public virtual void PastePeaks()
		{
			this.BandPeaks = PeaksSaver.Peaks;
			PeaksSaver.Peaks = null;
			this.PeaksPasted = true;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011B00 File Offset: 0x0000FD00
		public virtual void ClearPeaks()
		{
			this.BandPeaks = null;
			PeaksSaver.Peaks = null;
			this.PeaksPasted = false;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00011B16 File Offset: 0x0000FD16
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00011B20 File Offset: 0x0000FD20
		public virtual void Initialization()
		{
			this._cachedNumberOfBands = this.NumberOfBands;
			this.RawSpectrum = new float[this.SpectrumSamples];
			this.BandLevels = new float[this._cachedNumberOfBands];
			this.BufferedBandLevels = new float[this._cachedNumberOfBands];
			if (this.BandPeaks == null || this.BandPeaks.Length == 0)
			{
				this.BandPeaks = new float[this._cachedNumberOfBands];
				this.PeaksPasted = false;
			}
			if (this.BandPeaks.Length != this.BandLevels.Length)
			{
				this.BandPeaks = new float[this._cachedNumberOfBands];
				this.PeaksPasted = false;
			}
			this.LastPeaksAt = new float[this._cachedNumberOfBands];
			this.NormalizedBandLevels = new float[this._cachedNumberOfBands];
			this.NormalizedBufferedBandLevels = new float[this._cachedNumberOfBands];
			if (this.Mode == MMAudioAnalyzer.Modes.AudioSource && this.TargetAudioSource == null)
			{
				Debug.LogError(base.name + " : this MMAudioAnalyzer needs a target audio source to operate.");
				return;
			}
			if (this.Mode == MMAudioAnalyzer.Modes.Microphone)
			{
				GameObject gameObject = new GameObject("Microphone");
				SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
				gameObject.transform.SetParent(base.gameObject.transform);
				this.TargetAudioSource = gameObject.AddComponent<AudioSource>();
				this._microphoneStartedAt = Time.time;
			}
			this.Active = true;
			this._sampleIntervalWaitForSeconds = new WaitForSeconds(this.SampleInterval);
			base.StartCoroutine(this.Analyze());
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00011C96 File Offset: 0x0000FE96
		protected virtual void Update()
		{
			this.HandleBuffer();
			this.ComputeAmplitudes();
			this.HandleBeats();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00011CAA File Offset: 0x0000FEAA
		protected virtual IEnumerator Analyze()
		{
			for (;;)
			{
				switch (this.Mode)
				{
				case MMAudioAnalyzer.Modes.Global:
					AudioListener.GetSpectrumData(this.RawSpectrum, 0, this.Window);
					break;
				case MMAudioAnalyzer.Modes.AudioSource:
					this.TargetAudioSource.GetSpectrumData(this.RawSpectrum, 0, this.Window);
					break;
				case MMAudioAnalyzer.Modes.Microphone:
				{
					int num = 0;
					if ((float)num / 24000f > 0.03f)
					{
						if (!this.TargetAudioSource.isPlaying)
						{
							this.TargetAudioSource.timeSamples = (int)((float)num - 720f);
							this.TargetAudioSource.Play();
						}
						this._microphoneStartedAt = Time.time;
					}
					AudioListener.GetSpectrumData(this.RawSpectrum, 0, this.Window);
					break;
				}
				}
				this.ComputeBandLevels();
				yield return this._sampleIntervalWaitForSeconds;
			}
			yield break;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00011CBC File Offset: 0x0000FEBC
		protected virtual void HandleBuffer()
		{
			for (int i = 0; i < this.BandLevels.Length; i++)
			{
				this.BufferedBandLevels[i] = Mathf.Max(this.BufferedBandLevels[i] * Mathf.Exp(-this.BufferSpeed * Time.deltaTime), this.BandLevels[i]);
				this.NormalizedBandLevels[i] = this.BandLevels[i] / this.BandPeaks[i];
				this.NormalizedBufferedBandLevels[i] = this.BufferedBandLevels[i] / this.BandPeaks[i];
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00011D40 File Offset: 0x0000FF40
		protected virtual void ComputeBandLevels()
		{
			float num = Mathf.Log((float)this.RawSpectrum.Length);
			int num2 = 0;
			for (int i = 0; i < this.BandLevels.Length; i++)
			{
				float num3 = 0f;
				float num4 = Mathf.Exp(num / (float)this.BandLevels.Length * (float)(i + 1));
				float num5 = 1f / (num4 - (float)num2);
				float num6 = 0f;
				while ((float)num2 < num4)
				{
					num6 += this.RawSpectrum[num2];
					num3 = num6;
					num2++;
				}
				this.BandLevels[i] = Mathf.Sqrt(num5 * num3);
				if (this.BandLevels[i] > this.BandPeaks[i])
				{
					this.BandPeaks[i] = this.BandLevels[i];
					this.LastPeaksAt[i] = Time.time;
				}
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00011E04 File Offset: 0x00010004
		protected virtual void ComputeAmplitudes()
		{
			this.Amplitude = 0f;
			this.BufferedAmplitude = 0f;
			this.NormalizedAmplitude = 0f;
			this.NormalizedBufferedAmplitude = 0f;
			for (int i = 0; i < this._cachedNumberOfBands; i++)
			{
				this.Amplitude += this.BandLevels[i];
				this.BufferedAmplitude += this.BufferedBandLevels[i];
				this.NormalizedAmplitude += this.NormalizedBandLevels[i];
				this.NormalizedBufferedAmplitude += this.NormalizedBufferedBandLevels[i];
			}
			this.Amplitude /= (float)this._cachedNumberOfBands;
			this.BufferedAmplitude /= (float)this._cachedNumberOfBands;
			this.NormalizedAmplitude /= (float)this._cachedNumberOfBands;
			this.NormalizedBufferedAmplitude /= (float)this._cachedNumberOfBands;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00011EF4 File Offset: 0x000100F4
		protected virtual void HandleBeats()
		{
			if (this.Beats.Length == 0)
			{
				return;
			}
			foreach (Beat beat in this.Beats)
			{
				float num = 0f;
				beat.BeatThisFrame = false;
				switch (beat.Mode)
				{
				case Beat.Modes.Raw:
					num = this.BandLevels[beat.BandID];
					break;
				case Beat.Modes.Normalized:
					num = this.NormalizedBandLevels[beat.BandID];
					break;
				case Beat.Modes.BufferedRaw:
					num = this.BufferedBandLevels[beat.BandID];
					break;
				case Beat.Modes.BufferedNormalized:
					num = this.NormalizedBufferedBandLevels[beat.BandID];
					break;
				case Beat.Modes.Amplitude:
					num = this.Amplitude;
					break;
				case Beat.Modes.NormalizedAmplitude:
					num = this.NormalizedAmplitude;
					break;
				case Beat.Modes.AmplitudeBuffered:
					num = this.BufferedAmplitude;
					break;
				case Beat.Modes.NormalizedAmplitudeBuffered:
					num = this.NormalizedBufferedAmplitude;
					break;
				}
				if (beat.BeatValueMode == Beat.BeatValueModes.Live)
				{
					beat.CurrentValue = num;
				}
				else
				{
					if (beat._previousValue > beat.Threshold && num <= beat.Threshold && Time.time - beat._lastBeatAt > beat.MinimumTimeBetweenBeats)
					{
						this.OnBeat(beat, num);
					}
					if (beat._previousValue <= beat.Threshold && num > beat.Threshold && Time.time - beat._lastBeatAt > beat.MinimumTimeBetweenBeats)
					{
						this.OnBeat(beat, num);
					}
					beat._previousValue = num;
				}
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001204C File Offset: 0x0001024C
		protected virtual void OnBeat(Beat beat, float rawValue)
		{
			beat._lastBeatAt = Time.time;
			beat.BeatThisFrame = true;
			if (beat.OnBeat != null)
			{
				beat.OnBeat.Invoke();
			}
			MMBeatEvent.Trigger(beat.Name, beat.CurrentValue);
			base.StartCoroutine(this.RemapBeat(beat));
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001209D File Offset: 0x0001029D
		protected virtual IEnumerator RemapBeat(Beat beat)
		{
			float remapStartedAt = Time.time;
			while (Time.time - remapStartedAt < beat.RemappedAttack + beat.RemappedDecay)
			{
				if (Time.time - remapStartedAt < beat.RemappedAttack)
				{
					beat.CurrentValue = Mathf.Lerp(0f, 1f, (Time.time - remapStartedAt) / beat.RemappedAttack);
				}
				if (Time.time - remapStartedAt > beat.RemappedAttack)
				{
					beat.CurrentValue = Mathf.Lerp(1f, 0f, (Time.time - remapStartedAt - beat.RemappedAttack) / beat.RemappedDecay);
				}
				yield return null;
			}
			beat.CurrentValue = 0f;
			yield break;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000120AC File Offset: 0x000102AC
		protected virtual void OnValidate()
		{
			if (this.Beats == null || this.Beats.Length == 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < this.Beats.Length; i++)
			{
				if (num >= this._cachedNumberOfBands)
				{
					num = 0;
				}
				this.Beats[i].InitializeIfNeeded(i, num);
				num++;
			}
		}

		// Token: 0x040004F4 RID: 1268
		[Header("Source")]
		[MMInformation("This component lets you pick an audio source (either global : the whole scene's audio, a unique source, or the microphone), and will cut it into chunks that you can then use to emit beat events, that other objects can consume and act upon. The sample interval is the frequency at which sound will be analyzed, the amount of spectrum samples will determine the accuracy of the sampling, the window defines the method used to reduce leakage, and the number of bands will determine in how many bands you want to cut the sound. The more bands, the more levers you'll have to play with afterwards.In general, for all of these settings, higher values mean better quality and lower performance. The buffer speed determines how fast buffered band levels readjust.", MMInformationAttribute.InformationType.Info, false)]
		[MMReadOnlyWhenPlaying]
		public MMAudioAnalyzer.Modes Mode;

		// Token: 0x040004F5 RID: 1269
		[MMEnumCondition("Mode", new int[] { 1 })]
		[MMReadOnlyWhenPlaying]
		public AudioSource TargetAudioSource;

		// Token: 0x040004F6 RID: 1270
		[MMEnumCondition("Mode", new int[] { 2 })]
		public int MicrophoneID;

		// Token: 0x040004F7 RID: 1271
		[Header("Sampling")]
		[MMReadOnlyWhenPlaying]
		public float SampleInterval = 0.02f;

		// Token: 0x040004F8 RID: 1272
		[MMDropdown(new object[]
		{
			2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,
			2048, 4096, 8192
		})]
		[MMReadOnlyWhenPlaying]
		public int SpectrumSamples = 1024;

		// Token: 0x040004F9 RID: 1273
		[MMReadOnlyWhenPlaying]
		public FFTWindow Window;

		// Token: 0x040004FA RID: 1274
		[Range(1f, 64f)]
		[MMReadOnlyWhenPlaying]
		public int NumberOfBands = 8;

		// Token: 0x040004FB RID: 1275
		public float BufferSpeed = 2f;

		// Token: 0x040004FC RID: 1276
		[Header("Beat Events")]
		public Beat[] Beats;

		// Token: 0x040004FD RID: 1277
		[HideInInspector]
		public float[] RawSpectrum;

		// Token: 0x040004FE RID: 1278
		[HideInInspector]
		public float[] BandLevels;

		// Token: 0x040004FF RID: 1279
		[HideInInspector]
		public float[] BufferedBandLevels;

		// Token: 0x04000500 RID: 1280
		[HideInInspector]
		public float[] BandPeaks;

		// Token: 0x04000501 RID: 1281
		[HideInInspector]
		public float[] LastPeaksAt;

		// Token: 0x04000502 RID: 1282
		[HideInInspector]
		public float[] NormalizedBandLevels;

		// Token: 0x04000503 RID: 1283
		[HideInInspector]
		public float[] NormalizedBufferedBandLevels;

		// Token: 0x04000504 RID: 1284
		[HideInInspector]
		public float Amplitude;

		// Token: 0x04000505 RID: 1285
		[HideInInspector]
		public float NormalizedAmplitude;

		// Token: 0x04000506 RID: 1286
		[HideInInspector]
		public float BufferedAmplitude;

		// Token: 0x04000507 RID: 1287
		[HideInInspector]
		public float NormalizedBufferedAmplitude;

		// Token: 0x04000508 RID: 1288
		[HideInInspector]
		public bool Active;

		// Token: 0x04000509 RID: 1289
		[HideInInspector]
		public bool PeaksPasted;

		// Token: 0x0400050A RID: 1290
		protected const int _microphoneDuration = 5;

		// Token: 0x0400050B RID: 1291
		protected string _microphone;

		// Token: 0x0400050C RID: 1292
		protected float _microphoneStartedAt;

		// Token: 0x0400050D RID: 1293
		protected const float _microphoneDelay = 0.03f;

		// Token: 0x0400050E RID: 1294
		protected const float _microphoneFrequency = 24000f;

		// Token: 0x0400050F RID: 1295
		protected WaitForSeconds _sampleIntervalWaitForSeconds;

		// Token: 0x04000510 RID: 1296
		protected int _cachedNumberOfBands;

		// Token: 0x020003FB RID: 1019
		public enum Modes
		{
			// Token: 0x04001EB8 RID: 7864
			Global,
			// Token: 0x04001EB9 RID: 7865
			AudioSource,
			// Token: 0x04001EBA RID: 7866
			Microphone
		}
	}
}
