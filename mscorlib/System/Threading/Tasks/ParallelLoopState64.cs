using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200032A RID: 810
	internal class ParallelLoopState64 : ParallelLoopState
	{
		// Token: 0x0600224A RID: 8778 RVA: 0x0007BAC0 File Offset: 0x00079CC0
		internal ParallelLoopState64(ParallelLoopStateFlags64 sharedParallelStateFlags)
			: base(sharedParallelStateFlags)
		{
			this._sharedParallelStateFlags = sharedParallelStateFlags;
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x0007BAD0 File Offset: 0x00079CD0
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x0007BAD8 File Offset: 0x00079CD8
		internal long CurrentIteration
		{
			get
			{
				return this._currentIteration;
			}
			set
			{
				this._currentIteration = value;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x0007BAE1 File Offset: 0x00079CE1
		internal override bool InternalShouldExitCurrentIteration
		{
			get
			{
				return this._sharedParallelStateFlags.ShouldExitLoop(this.CurrentIteration);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x0007BAF4 File Offset: 0x00079CF4
		internal override long? InternalLowestBreakIteration
		{
			get
			{
				return this._sharedParallelStateFlags.NullableLowestBreakIteration;
			}
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0007BB01 File Offset: 0x00079D01
		internal override void InternalBreak()
		{
			ParallelLoopState.Break(this.CurrentIteration, this._sharedParallelStateFlags);
		}

		// Token: 0x04001C36 RID: 7222
		private readonly ParallelLoopStateFlags64 _sharedParallelStateFlags;

		// Token: 0x04001C37 RID: 7223
		private long _currentIteration;
	}
}
