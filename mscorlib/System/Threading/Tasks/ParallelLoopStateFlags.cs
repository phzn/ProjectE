using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200032B RID: 811
	internal class ParallelLoopStateFlags
	{
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x0007BB14 File Offset: 0x00079D14
		internal int LoopStateFlags
		{
			get
			{
				return this._loopStateFlags;
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0007BB20 File Offset: 0x00079D20
		internal bool AtomicLoopStateUpdate(int newState, int illegalStates)
		{
			int num = 0;
			return this.AtomicLoopStateUpdate(newState, illegalStates, ref num);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0007BB3C File Offset: 0x00079D3C
		internal bool AtomicLoopStateUpdate(int newState, int illegalStates, ref int oldState)
		{
			SpinWait spinWait = default(SpinWait);
			for (;;)
			{
				oldState = this._loopStateFlags;
				if ((oldState & illegalStates) != 0)
				{
					break;
				}
				if (Interlocked.CompareExchange(ref this._loopStateFlags, oldState | newState, oldState) == oldState)
				{
					return true;
				}
				spinWait.SpinOnce();
			}
			return false;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x0007BB82 File Offset: 0x00079D82
		internal void SetExceptional()
		{
			this.AtomicLoopStateUpdate(1, 0);
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x0007BB8D File Offset: 0x00079D8D
		internal void Stop()
		{
			if (!this.AtomicLoopStateUpdate(4, 2))
			{
				throw new InvalidOperationException("Stop was called after Break was called.");
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0007BBA4 File Offset: 0x00079DA4
		internal bool Cancel()
		{
			return this.AtomicLoopStateUpdate(8, 0);
		}

		// Token: 0x04001C38 RID: 7224
		internal const int ParallelLoopStateNone = 0;

		// Token: 0x04001C39 RID: 7225
		internal const int ParallelLoopStateExceptional = 1;

		// Token: 0x04001C3A RID: 7226
		internal const int ParallelLoopStateBroken = 2;

		// Token: 0x04001C3B RID: 7227
		internal const int ParallelLoopStateStopped = 4;

		// Token: 0x04001C3C RID: 7228
		internal const int ParallelLoopStateCanceled = 8;

		// Token: 0x04001C3D RID: 7229
		private volatile int _loopStateFlags;
	}
}
