using System;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A15 RID: 2581
	public sealed class RunSoakFrameTimeline
	{
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x0600496E RID: 18798 RVA: 0x001C26F1 File Offset: 0x001C08F1
		// (set) Token: 0x0600496F RID: 18799 RVA: 0x001C26F9 File Offset: 0x001C08F9
		public bool Invalid { get; private set; }

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004970 RID: 18800 RVA: 0x001C2702 File Offset: 0x001C0902
		// (set) Token: 0x06004971 RID: 18801 RVA: 0x001C270A File Offset: 0x001C090A
		public int MissingFrames { get; private set; }

		// Token: 0x06004972 RID: 18802 RVA: 0x001C2713 File Offset: 0x001C0913
		public RunSoakFrameTimeline(int creationFrame)
		{
			this._creationFrame = creationFrame;
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x001C272C File Offset: 0x001C092C
		public void CaptureLateState(int frame, int phase, int activeNpcs, float distance, int deferredStreamingBudgetActive = -1, double deferredStreamingBudgetMs = -1.0, double deferredStreamingElapsedMs = -1.0, int deferredStreamingBudgetDeferrals = -1)
		{
			if (frame <= this._creationFrame)
			{
				return;
			}
			if (this._hasPending && this._pending.sourceFrame != frame)
			{
				this.Invalid = true;
				this.MissingFrames++;
			}
			this._pending = new RunSoakFrameTimeline.State
			{
				sourceFrame = frame,
				phase = phase,
				activeNpcs = activeNpcs,
				distance = distance,
				deferredStreamingBudgetActive = deferredStreamingBudgetActive,
				deferredStreamingBudgetMs = deferredStreamingBudgetMs,
				deferredStreamingElapsedMs = deferredStreamingElapsedMs,
				deferredStreamingBudgetDeferrals = deferredStreamingBudgetDeferrals
			};
			this._hasPending = true;
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x001C27CC File Offset: 0x001C09CC
		public bool TryTakeCompleted(int currentFrame, out RunSoakFrameTimeline.State state)
		{
			state = default(RunSoakFrameTimeline.State);
			if (this.Invalid)
			{
				return false;
			}
			if (!this._hasPending)
			{
				int num = ((this._lastTaken >= 0) ? (this._lastTaken + 1) : (this._creationFrame + 1));
				if (currentFrame > num)
				{
					this.Invalid = true;
					this.MissingFrames += currentFrame - num;
				}
				return false;
			}
			if (this._pending.sourceFrame >= currentFrame)
			{
				return false;
			}
			if (this._pending.sourceFrame != currentFrame - 1)
			{
				this.Invalid = true;
				this.MissingFrames += currentFrame - this._pending.sourceFrame - 1;
				this._hasPending = false;
				return false;
			}
			if (this._lastTaken >= 0 && this._pending.sourceFrame != this._lastTaken + 1)
			{
				this.Invalid = true;
				this.MissingFrames += this._pending.sourceFrame - this._lastTaken - 1;
				this._hasPending = false;
				return false;
			}
			state = this._pending;
			this._lastTaken = state.sourceFrame;
			this._hasPending = false;
			return true;
		}

		// Token: 0x04004A14 RID: 18964
		private readonly int _creationFrame;

		// Token: 0x04004A15 RID: 18965
		private RunSoakFrameTimeline.State _pending;

		// Token: 0x04004A16 RID: 18966
		private bool _hasPending;

		// Token: 0x04004A17 RID: 18967
		private int _lastTaken = -1;

		// Token: 0x02000A16 RID: 2582
		public struct State
		{
			// Token: 0x04004A1A RID: 18970
			public int sourceFrame;

			// Token: 0x04004A1B RID: 18971
			public int phase;

			// Token: 0x04004A1C RID: 18972
			public int activeNpcs;

			// Token: 0x04004A1D RID: 18973
			public float distance;

			// Token: 0x04004A1E RID: 18974
			public int deferredStreamingBudgetActive;

			// Token: 0x04004A1F RID: 18975
			public int deferredStreamingBudgetDeferrals;

			// Token: 0x04004A20 RID: 18976
			public double deferredStreamingBudgetMs;

			// Token: 0x04004A21 RID: 18977
			public double deferredStreamingElapsedMs;
		}
	}
}
