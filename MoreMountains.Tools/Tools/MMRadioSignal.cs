using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200012B RID: 299
	public abstract class MMRadioSignal : MonoBehaviour
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00021A43 File Offset: 0x0001FC43
		public virtual float Level
		{
			get
			{
				return this.CurrentLevel;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00021A4B File Offset: 0x0001FC4B
		public virtual float TimescaleTime
		{
			get
			{
				if (this.TimeScale != MMRadioSignal.TimeScales.Scaled)
				{
					return Time.unscaledTime;
				}
				return Time.time;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00021A61 File Offset: 0x0001FC61
		public virtual float TimescaleDeltaTime
		{
			get
			{
				if (this.TimeScale != MMRadioSignal.TimeScales.Scaled)
				{
					return Time.unscaledDeltaTime;
				}
				return Time.deltaTime;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00021A77 File Offset: 0x0001FC77
		protected virtual void Awake()
		{
			this.Initialization();
			if (this.PlayOnStart)
			{
				this.StartShaking();
			}
			base.enabled = this.PlayOnStart;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00021A99 File Offset: 0x0001FC99
		protected virtual void Initialization()
		{
			this.CurrentLevel = 0f;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00021AA6 File Offset: 0x0001FCA6
		public virtual void StartShaking()
		{
			if (this.Playing)
			{
				return;
			}
			base.enabled = true;
			this._shakeStartedTimestamp = this.TimescaleTime;
			this.Playing = true;
			this.ShakeStarts();
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00021AD1 File Offset: 0x0001FCD1
		protected virtual void ShakeStarts()
		{
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00021AD4 File Offset: 0x0001FCD4
		protected virtual void Update()
		{
			this.ProcessUpdate();
			if (this.SignalMode == MMRadioSignal.SignalModes.Driven)
			{
				this.ProcessDrivenMode();
			}
			else if (this.SignalMode == MMRadioSignal.SignalModes.Persistent)
			{
				this._signalTime += this.TimescaleDeltaTime;
				if (this._signalTime > this.Duration)
				{
					this._signalTime = 0f;
				}
				this.DriverTime = MMMaths.Remap(this._signalTime, 0f, this.Duration, 0f, 1f);
			}
			else
			{
				MMRadioSignal.SignalModes signalMode = this.SignalMode;
			}
			if (this.Playing || this.SignalMode == MMRadioSignal.SignalModes.Driven)
			{
				this.Shake();
			}
			if (this.SignalMode == MMRadioSignal.SignalModes.OneTime && this.Playing && this.TimescaleTime - this._shakeStartedTimestamp > this.Duration)
			{
				this.ShakeComplete();
			}
			if (this._levelLastFrame != this.Level)
			{
				this.ApplyLevel(this.Level);
			}
			this._levelLastFrame = this.Level;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00021BC4 File Offset: 0x0001FDC4
		public virtual void ApplyLevel(float level)
		{
			if (this.OnValueChange != null)
			{
				this.OnValueChange.Invoke(level);
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00021BDA File Offset: 0x0001FDDA
		protected virtual void ProcessDrivenMode()
		{
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00021BDC File Offset: 0x0001FDDC
		protected virtual void ProcessUpdate()
		{
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00021BDE File Offset: 0x0001FDDE
		protected virtual void Shake()
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00021BE0 File Offset: 0x0001FDE0
		public virtual float GraphValue(float time)
		{
			return 0f;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00021BE7 File Offset: 0x0001FDE7
		protected virtual void ShakeComplete()
		{
			this.Playing = false;
			base.enabled = false;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00021BF7 File Offset: 0x0001FDF7
		protected virtual void OnEnable()
		{
			this.StartShaking();
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00021BFF File Offset: 0x0001FDFF
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00021C01 File Offset: 0x0001FE01
		protected virtual void OnDisable()
		{
			if (this.Playing)
			{
				this.ShakeComplete();
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00021C11 File Offset: 0x0001FE11
		public virtual void Play()
		{
			base.enabled = true;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00021C1A File Offset: 0x0001FE1A
		public virtual void Stop()
		{
			this.ShakeComplete();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00021C24 File Offset: 0x0001FE24
		public virtual float ApplyBias(float t, float bias)
		{
			if (bias == 0.5f)
			{
				return t;
			}
			bias = MMMaths.Remap(bias, 0f, 1f, 1f, 0f);
			float num = bias * 2f - 1f;
			if (num < 0f)
			{
				t = 1f - Mathf.Pow(1f - t, Mathf.Max(1f + num, 0.01f));
			}
			else
			{
				t = Mathf.Pow(t, Mathf.Max(1f - num, 0.01f));
			}
			return t;
		}

		// Token: 0x04000860 RID: 2144
		[Header("Signal")]
		public MMRadioSignal.SignalModes SignalMode = MMRadioSignal.SignalModes.Persistent;

		// Token: 0x04000861 RID: 2145
		public MMRadioSignal.TimeScales TimeScale;

		// Token: 0x04000862 RID: 2146
		public float Duration = 2f;

		// Token: 0x04000863 RID: 2147
		public float GlobalMultiplier = 1f;

		// Token: 0x04000864 RID: 2148
		[MMReadOnly]
		public float CurrentLevel;

		// Token: 0x04000865 RID: 2149
		[Header("Play Settings")]
		[MMReadOnly]
		public bool Playing;

		// Token: 0x04000866 RID: 2150
		[Range(0f, 1f)]
		public float DriverTime;

		// Token: 0x04000867 RID: 2151
		public bool PlayOnStart = true;

		// Token: 0x04000868 RID: 2152
		public MMRadioSignalOnValueChange OnValueChange;

		// Token: 0x04000869 RID: 2153
		[Header("Debug")]
		[MMInspectorButton("StartShaking")]
		public bool StartShakingButton;

		// Token: 0x0400086A RID: 2154
		protected float _signalTime;

		// Token: 0x0400086B RID: 2155
		protected float _shakeStartedTimestamp;

		// Token: 0x0400086C RID: 2156
		protected float _levelLastFrame;

		// Token: 0x02000442 RID: 1090
		public enum SignalModes
		{
			// Token: 0x04001FCF RID: 8143
			OneTime,
			// Token: 0x04001FD0 RID: 8144
			Persistent,
			// Token: 0x04001FD1 RID: 8145
			Driven
		}

		// Token: 0x02000443 RID: 1091
		public enum TimeScales
		{
			// Token: 0x04001FD3 RID: 8147
			Unscaled,
			// Token: 0x04001FD4 RID: 8148
			Scaled
		}
	}
}
