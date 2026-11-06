using System;

namespace Features.Loading
{
	// Token: 0x020010D4 RID: 4308
	public sealed class LoadingFrameStability
	{
		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x06008289 RID: 33417 RVA: 0x00354E52 File Offset: 0x00353052
		// (set) Token: 0x0600828A RID: 33418 RVA: 0x00354E5A File Offset: 0x0035305A
		public bool TimedOut { get; private set; }

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x0600828B RID: 33419 RVA: 0x00354E63 File Offset: 0x00353063
		// (set) Token: 0x0600828C RID: 33420 RVA: 0x00354E6B File Offset: 0x0035306B
		public double ElapsedSeconds { get; private set; }

		// Token: 0x0600828D RID: 33421 RVA: 0x00354E74 File Offset: 0x00353074
		public LoadingFrameStability(double minimumSeconds, double maximumSeconds, double stableWindowSeconds, double maximumFrameSeconds)
		{
			this._minimumSeconds = Math.Max(0.0, minimumSeconds);
			this._maximumSeconds = Math.Max(this._minimumSeconds, maximumSeconds);
			this._stableWindowSeconds = Math.Max(0.0, stableWindowSeconds);
			this._maximumFrameSeconds = Math.Max(0.001, maximumFrameSeconds);
		}

		// Token: 0x0600828E RID: 33422 RVA: 0x00354EE8 File Offset: 0x003530E8
		public bool Tick(double realtime, double frameSeconds, bool presentationReady)
		{
			if (double.IsNaN(this._startedAt))
			{
				this._lastSample = realtime;
				this._startedAt = realtime;
				return false;
			}
			if (realtime <= this._lastSample)
			{
				return false;
			}
			double num = realtime - this._lastSample;
			this._lastSample = realtime;
			this.ElapsedSeconds = realtime - this._startedAt;
			this._stableSeconds = ((presentationReady && frameSeconds > 0.0 && frameSeconds <= this._maximumFrameSeconds) ? (this._stableSeconds + Math.Min(num, frameSeconds)) : 0.0);
			if (this.ElapsedSeconds >= this._minimumSeconds && this._stableSeconds >= this._stableWindowSeconds)
			{
				return true;
			}
			this.TimedOut = this.ElapsedSeconds >= this._maximumSeconds;
			return this.TimedOut;
		}

		// Token: 0x040085B4 RID: 34228
		private readonly double _minimumSeconds;

		// Token: 0x040085B5 RID: 34229
		private readonly double _maximumSeconds;

		// Token: 0x040085B6 RID: 34230
		private readonly double _stableWindowSeconds;

		// Token: 0x040085B7 RID: 34231
		private readonly double _maximumFrameSeconds;

		// Token: 0x040085B8 RID: 34232
		private double _startedAt = double.NaN;

		// Token: 0x040085B9 RID: 34233
		private double _lastSample;

		// Token: 0x040085BA RID: 34234
		private double _stableSeconds;
	}
}
