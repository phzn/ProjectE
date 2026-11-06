using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Profiling
{
	// Token: 0x02000007 RID: 7
	public readonly struct ProfilerMarker<[IsUnmanaged] TP1> where TP1 : struct, ValueType
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000020FA File Offset: 0x000002FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(string name, string param1Name)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000020FC File Offset: 0x000002FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(ProfilerCategory category, string name, string param1Name)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000020FE File Offset: 0x000002FE
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(TP1 p1)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002100 File Offset: 0x00000300
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End()
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002104 File Offset: 0x00000304
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker<TP1>.AutoScope Auto(TP1 p1)
		{
			return default(ProfilerMarker<TP1>.AutoScope);
		}

		// Token: 0x0200000E RID: 14
		public readonly struct AutoScope : IDisposable
		{
			// Token: 0x06000026 RID: 38 RVA: 0x000023E0 File Offset: 0x000005E0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal AutoScope(ProfilerMarker<TP1> marker, TP1 p1)
			{
			}

			// Token: 0x06000027 RID: 39 RVA: 0x000023E2 File Offset: 0x000005E2
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
			}
		}
	}
}
