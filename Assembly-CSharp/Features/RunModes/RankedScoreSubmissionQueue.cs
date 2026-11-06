using System;
using System.Collections.Generic;

namespace Features.RunModes
{
	// Token: 0x02000D6A RID: 3434
	public sealed class RankedScoreSubmissionQueue
	{
		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x0600651F RID: 25887 RVA: 0x00285963 File Offset: 0x00283B63
		// (set) Token: 0x06006520 RID: 25888 RVA: 0x0028596B File Offset: 0x00283B6B
		public RankedRunResult? InFlightResult { get; private set; }

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06006521 RID: 25889 RVA: 0x00285974 File Offset: 0x00283B74
		// (set) Token: 0x06006522 RID: 25890 RVA: 0x0028597C File Offset: 0x00283B7C
		public bool ResetRequested { get; private set; }

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06006523 RID: 25891 RVA: 0x00285985 File Offset: 0x00283B85
		// (set) Token: 0x06006524 RID: 25892 RVA: 0x0028598D File Offset: 0x00283B8D
		public bool IsResetInFlight { get; private set; }

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06006525 RID: 25893 RVA: 0x00285996 File Offset: 0x00283B96
		public int? ConfirmedBestDistanceMeters
		{
			get
			{
				return this._confirmedBestDistanceMeters;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06006526 RID: 25894 RVA: 0x0028599E File Offset: 0x00283B9E
		public bool HasPending
		{
			get
			{
				return this._pending != null;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06006527 RID: 25895 RVA: 0x002859AC File Offset: 0x00283BAC
		public RankedRunResult? BestUnconfirmedResult
		{
			get
			{
				if (this.ResetRequested)
				{
					return null;
				}
				if (this._pending == null)
				{
					return this.InFlightResult;
				}
				if (this.InFlightResult != null && this._pending.Value.DistanceMeters <= this.InFlightResult.Value.DistanceMeters)
				{
					return this.InFlightResult;
				}
				return this._pending;
			}
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x00285A28 File Offset: 0x00283C28
		public void Enqueue(RankedRunResult result)
		{
			if (this.ResetRequested || !result.IsValid)
			{
				return;
			}
			if (this._confirmedBestDistanceMeters != null && result.DistanceMeters <= this._confirmedBestDistanceMeters.Value)
			{
				return;
			}
			if (this.InFlightResult != null && result.DistanceMeters <= this.InFlightResult.Value.DistanceMeters)
			{
				return;
			}
			if (this._pending == null || result.DistanceMeters > this._pending.Value.DistanceMeters)
			{
				this._pending = new RankedRunResult?(result);
			}
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x00285AD0 File Offset: 0x00283CD0
		public bool TryBeginUpload(double now, out RankedRunResult result)
		{
			result = default(RankedRunResult);
			if (this.ResetRequested || this._pending == null || !this.CanBeginUpload(now))
			{
				return false;
			}
			result = this._pending.Value;
			this._pending = null;
			this.InFlightResult = new RankedRunResult?(result);
			this._attemptTimes.Enqueue(now);
			return true;
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x00285B40 File Offset: 0x00283D40
		public void CompleteUpload(bool success, double now)
		{
			if (this.InFlightResult == null)
			{
				return;
			}
			RankedRunResult value = this.InFlightResult.Value;
			this.InFlightResult = null;
			if (this.ResetRequested)
			{
				return;
			}
			if (success)
			{
				this._confirmedBestDistanceMeters = new int?((this._confirmedBestDistanceMeters != null) ? Math.Max(this._confirmedBestDistanceMeters.Value, value.DistanceMeters) : value.DistanceMeters);
				if (this._pending != null && this._pending.Value.DistanceMeters <= this._confirmedBestDistanceMeters.Value)
				{
					this._pending = null;
				}
				this._failures = 0;
				this._retryAfter = now;
				return;
			}
			this.Enqueue(value);
			this.ScheduleRetry(now);
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x00285C19 File Offset: 0x00283E19
		public void RequestReset()
		{
			if (this.ResetRequested)
			{
				return;
			}
			this.ResetRequested = true;
			this._pending = null;
			this._confirmedBestDistanceMeters = null;
			this._failures = 0;
			this._retryAfter = 0.0;
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x00285C59 File Offset: 0x00283E59
		public bool TryBeginResetUpload(double now)
		{
			if (!this.ResetRequested || !this.CanBeginUpload(now))
			{
				return false;
			}
			this.IsResetInFlight = true;
			this._attemptTimes.Enqueue(now);
			return true;
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x00285C82 File Offset: 0x00283E82
		public void CompleteResetUpload(bool success, double now)
		{
			if (!this.IsResetInFlight)
			{
				return;
			}
			this.IsResetInFlight = false;
			if (success)
			{
				this.ResetRequested = false;
				this._failures = 0;
				this._retryAfter = now;
				return;
			}
			this.ScheduleRetry(now);
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x00285CB4 File Offset: 0x00283EB4
		private bool CanBeginUpload(double now)
		{
			while (this._attemptTimes.Count > 0 && now - this._attemptTimes.Peek() >= 600.0)
			{
				this._attemptTimes.Dequeue();
			}
			return this.InFlightResult == null && !this.IsResetInFlight && now >= this._retryAfter && this._attemptTimes.Count < 10;
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00285D28 File Offset: 0x00283F28
		private void ScheduleRetry(double now)
		{
			this._failures = Math.Min(this._failures + 1, 4);
			this._retryAfter = now + Math.Min(120.0, 15.0 * Math.Pow(2.0, (double)(this._failures - 1)));
		}

		// Token: 0x04006789 RID: 26505
		private readonly Queue<double> _attemptTimes = new Queue<double>();

		// Token: 0x0400678A RID: 26506
		private RankedRunResult? _pending;

		// Token: 0x0400678B RID: 26507
		private int? _confirmedBestDistanceMeters;

		// Token: 0x0400678C RID: 26508
		private int _failures;

		// Token: 0x0400678D RID: 26509
		private double _retryAfter;
	}
}
