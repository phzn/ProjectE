using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Profiling
{
	// Token: 0x02000008 RID: 8
	public readonly struct ProfilerMarker<[IsUnmanaged] TP1, [IsUnmanaged] TP2> where TP1 : struct, ValueType where TP2 : struct, ValueType
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000211A File Offset: 0x0000031A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(string name, string param1Name, string param2Name)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000211C File Offset: 0x0000031C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(ProfilerCategory category, string name, string param1Name, string param2Name)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000211E File Offset: 0x0000031E
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(TP1 p1, TP2 p2)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002120 File Offset: 0x00000320
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002124 File Offset: 0x00000324
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker<TP1, TP2>.AutoScope Auto(TP1 p1, TP2 p2)
		{
			return default(ProfilerMarker<TP1, TP2>.AutoScope);
		}

		// Token: 0x0200000F RID: 15
		public readonly struct AutoScope : IDisposable
		{
			// Token: 0x06000028 RID: 40 RVA: 0x000023E4 File Offset: 0x000005E4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal AutoScope(ProfilerMarker<TP1, TP2> marker, TP1 p1, TP2 p2)
			{
			}

			// Token: 0x06000029 RID: 41 RVA: 0x000023E6 File Offset: 0x000005E6
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
			}
		}
	}
}
