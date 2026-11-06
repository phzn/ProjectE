using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Profiling
{
	// Token: 0x02000005 RID: 5
	public readonly struct ProfilerCounter<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020CF File Offset: 0x000002CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounter(ProfilerCategory category, string name, ProfilerMarkerDataUnit dataUnit)
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020D1 File Offset: 0x000002D1
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sample(T value)
		{
		}
	}
}
