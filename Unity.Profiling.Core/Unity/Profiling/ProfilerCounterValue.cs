using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Profiling
{
	// Token: 0x02000006 RID: 6
	public readonly struct ProfilerCounterValue<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020D3 File Offset: 0x000002D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounterValue(string name)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020D5 File Offset: 0x000002D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounterValue(string name, ProfilerMarkerDataUnit dataUnit)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020D7 File Offset: 0x000002D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounterValue(string name, ProfilerMarkerDataUnit dataUnit, ProfilerCounterOptions counterOptions)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020D9 File Offset: 0x000002D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounterValue(ProfilerCategory category, string name, ProfilerMarkerDataUnit dataUnit)
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020DB File Offset: 0x000002DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerCounterValue(ProfilerCategory category, string name, ProfilerMarkerDataUnit dataUnit, ProfilerCounterOptions counterOptions)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020F6 File Offset: 0x000002F6
		public T Value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return default(T);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000020F8 File Offset: 0x000002F8
		[Conditional("ENABLE_PROFILER")]
		public void Sample()
		{
		}
	}
}
