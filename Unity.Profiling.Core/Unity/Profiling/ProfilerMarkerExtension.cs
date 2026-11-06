using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling.LowLevel.Unsafe;

namespace Unity.Profiling
{
	// Token: 0x0200000A RID: 10
	public static class ProfilerMarkerExtension
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000215C File Offset: 0x0000035C
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, int metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 2,
				Size = (uint)UnsafeUtility.SizeOf<int>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000021A4 File Offset: 0x000003A4
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, uint metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 3,
				Size = (uint)UnsafeUtility.SizeOf<uint>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000021EC File Offset: 0x000003EC
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, long metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 4,
				Size = (uint)UnsafeUtility.SizeOf<long>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002234 File Offset: 0x00000434
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, ulong metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 5,
				Size = (uint)UnsafeUtility.SizeOf<ulong>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000227C File Offset: 0x0000047C
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, float metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 6,
				Size = (uint)UnsafeUtility.SizeOf<float>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000022C4 File Offset: 0x000004C4
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, double metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 7,
				Size = (uint)UnsafeUtility.SizeOf<double>(),
				Ptr = (void*)(&metadata)
			};
			ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000230C File Offset: 0x0000050C
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Begin(this ProfilerMarker marker, string metadata)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 9
			};
			fixed (string text = metadata)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				profilerMarkerData.Size = (uint)((metadata.Length + 1) * 2);
				profilerMarkerData.Ptr = (void*)ptr;
				ProfilerUnsafeUtility.BeginSampleWithMetadata(marker.Handle, 1, (void*)(&profilerMarkerData));
			}
		}
	}
}
