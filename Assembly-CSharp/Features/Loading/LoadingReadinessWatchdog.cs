using System;
using UnityEngine;

namespace Features.Loading
{
	// Token: 0x020010D5 RID: 4309
	public sealed class LoadingReadinessWatchdog
	{
		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x0600828F RID: 33423 RVA: 0x00354FAF File Offset: 0x003531AF
		// (set) Token: 0x06008290 RID: 33424 RVA: 0x00354FB7 File Offset: 0x003531B7
		public float StalledSeconds { get; private set; }

		// Token: 0x06008291 RID: 33425 RVA: 0x00354FC0 File Offset: 0x003531C0
		public LoadingReadinessWatchdog(float stallTimeoutSeconds)
		{
			this._timeoutSeconds = Mathf.Max(0f, stallTimeoutSeconds);
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x00354FD9 File Offset: 0x003531D9
		public bool Tick(double realtimeSinceStartup, float progress, int workVersion = 0, int completedPhases = 0)
		{
			return this.Tick(realtimeSinceStartup, progress, workVersion, completedPhases, Time.frameCount);
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x00354FEC File Offset: 0x003531EC
		public bool Tick(double realtimeSinceStartup, float progress, int workVersion, int completedPhases, int observationFrame)
		{
			double num = (this._initialized ? Math.Max(0.0, realtimeSinceStartup - this._lastSampleRealtime) : 0.0);
			this._lastSampleRealtime = (this._initialized ? Math.Max(this._lastSampleRealtime, realtimeSinceStartup) : realtimeSinceStartup);
			bool flag = !this._initialized || progress > this._highestProgress + 1E-06f || workVersion > this._highestWorkVersion || (completedPhases & ~this._completedPhases) != 0;
			this._initialized = true;
			this._highestProgress = Mathf.Max(this._highestProgress, progress);
			this._highestWorkVersion = Mathf.Max(this._highestWorkVersion, workVersion);
			this._completedPhases |= completedPhases;
			this.StalledSeconds = (flag ? 0f : (this.StalledSeconds + (float)num));
			if (flag || this.StalledSeconds < this._timeoutSeconds)
			{
				this._expiredObservationFrame = null;
				return false;
			}
			if (this._expiredObservationFrame == null)
			{
				this._expiredObservationFrame = new int?(observationFrame);
				return false;
			}
			return observationFrame - this._expiredObservationFrame.Value >= 2;
		}

		// Token: 0x040085BD RID: 34237
		private readonly float _timeoutSeconds;

		// Token: 0x040085BE RID: 34238
		private bool _initialized;

		// Token: 0x040085BF RID: 34239
		private float _highestProgress;

		// Token: 0x040085C0 RID: 34240
		private int _highestWorkVersion;

		// Token: 0x040085C1 RID: 34241
		private int _completedPhases;

		// Token: 0x040085C2 RID: 34242
		private double _lastSampleRealtime;

		// Token: 0x040085C3 RID: 34243
		private int? _expiredObservationFrame;
	}
}
