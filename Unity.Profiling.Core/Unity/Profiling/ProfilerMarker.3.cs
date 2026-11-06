using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Profiling
{
	// Token: 0x02000009 RID: 9
	public readonly struct ProfilerMarker<[IsUnmanaged] TP1, [IsUnmanaged] TP2, [IsUnmanaged] TP3> where TP1 : struct, ValueType where TP2 : struct, ValueType where TP3 : struct, ValueType
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000213A File Offset: 0x0000033A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(string name, string param1Name, string param2Name, string param3Name)
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000213C File Offset: 0x0000033C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker(ProfilerCategory category, string name, string param1Name, string param2Name, string param3Name)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000213E File Offset: 0x0000033E
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(TP1 p1, TP2 p2, TP3 p3)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002140 File Offset: 0x00000340
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End()
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002144 File Offset: 0x00000344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ProfilerMarker<TP1, TP2, TP3>.AutoScope Auto(TP1 p1, TP2 p2, TP3 p3)
		{
			return default(ProfilerMarker<TP1, TP2, TP3>.AutoScope);
		}

		// Token: 0x02000010 RID: 16
		public readonly struct AutoScope : IDisposable
		{
			// Token: 0x0600002A RID: 42 RVA: 0x000023E8 File Offset: 0x000005E8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal AutoScope(ProfilerMarker<TP1, TP2, TP3> marker, TP1 p1, TP2 p2, TP3 p3)
			{
			}

			// Token: 0x0600002B RID: 43 RVA: 0x000023EA File Offset: 0x000005EA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
			}
		}
	}
}
