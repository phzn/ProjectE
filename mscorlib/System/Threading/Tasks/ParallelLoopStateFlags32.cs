using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200032C RID: 812
	internal class ParallelLoopStateFlags32 : ParallelLoopStateFlags
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0007BBAE File Offset: 0x00079DAE
		internal int LowestBreakIteration
		{
			get
			{
				return this._lowestBreakIteration;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x0007BBB8 File Offset: 0x00079DB8
		internal long? NullableLowestBreakIteration
		{
			get
			{
				if (this._lowestBreakIteration == 2147483647)
				{
					return null;
				}
				long num = (long)this._lowestBreakIteration;
				if (IntPtr.Size >= 8)
				{
					return new long?(num);
				}
				return new long?(Interlocked.Read(ref num));
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x0007BC04 File Offset: 0x00079E04
		internal bool ShouldExitLoop(int CallerIteration)
		{
			int loopStateFlags = base.LoopStateFlags;
			return loopStateFlags != 0 && ((loopStateFlags & 13) != 0 || ((loopStateFlags & 2) != 0 && CallerIteration > this.LowestBreakIteration));
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0007BC38 File Offset: 0x00079E38
		internal bool ShouldExitLoop()
		{
			int loopStateFlags = base.LoopStateFlags;
			return loopStateFlags != 0 && (loopStateFlags & 9) != 0;
		}

		// Token: 0x04001C3E RID: 7230
		internal volatile int _lowestBreakIteration = int.MaxValue;
	}
}
