using System;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200027B RID: 635
	public struct GlobalScratchpad
	{
		// Token: 0x06001109 RID: 4361 RVA: 0x00048530 File Offset: 0x00046730
		public unsafe static void Initialize()
		{
			if (*GlobalScratchpad.IsInstalled.Data == 0)
			{
				int threadIndexCount = JobsUtility.ThreadIndexCount;
				GlobalScratchpad.Pad.Data.Initialize(threadIndexCount, 32768, true, (int)AllocatorManager.GlobalAllocatorBaseIndex);
				*GlobalScratchpad.IsInstalled.Data = 1;
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00048578 File Offset: 0x00046778
		public unsafe static ref ScratchpadAllocator GetAllocator()
		{
			if (*GlobalScratchpad.IsInstalled.Data == 0)
			{
				throw new InvalidOperationException("GlobalScratchpad is not initialized.");
			}
			int num = Interlocked.Increment(GlobalScratchpad.ThreadIndex.Data);
			num = (num - 1) % (int)AllocatorManager.NumGlobalScratchAllocators;
			return GlobalScratchpad.Pad.Data.GetScratchpadAllocator(num);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x000485C8 File Offset: 0x000467C8
		public unsafe static void Rewind()
		{
			if (*GlobalScratchpad.IsInstalled.Data == 0)
			{
				throw new InvalidOperationException("GlobalScratchpad is not initialized.");
			}
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Rewind GlobalScratchpad from a Job.");
			}
			Interlocked.Exchange(GlobalScratchpad.ThreadIndex.Data, 0);
			GlobalScratchpad.Pad.Data.Rewind();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00048620 File Offset: 0x00046820
		[ExcludeFromBurstCompatTesting("Accesses managed delegate")]
		internal unsafe static void Dispose()
		{
			if (*GlobalScratchpad.IsInstalled.Data == 0)
			{
				throw new InvalidOperationException("GlobalScratchpad is not initialized.");
			}
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("You cannot Dispose a Scratchpad from a Job.");
			}
			GlobalScratchpad.Pad.Data.Dispose();
			*GlobalScratchpad.IsInstalled.Data = 0;
		}

		// Token: 0x04000687 RID: 1671
		internal const ushort BlockSize = 32768;

		// Token: 0x04000688 RID: 1672
		internal static readonly SharedStatic<byte> IsInstalled = SharedStatic<byte>.GetOrCreatePartiallyUnsafeWithHashCode<byte>(0U, -9131820293153653849L);

		// Token: 0x04000689 RID: 1673
		internal static readonly SharedStatic<int> ThreadIndex = SharedStatic<int>.GetOrCreatePartiallyUnsafeWithHashCode<int>(0U, -9131820293153653849L);

		// Token: 0x0400068A RID: 1674
		internal static readonly SharedStatic<Scratchpad> Pad = SharedStatic<Scratchpad>.GetOrCreateUnsafe(0U, -9131820293153653849L, 9960927003105844L);
	}
}
