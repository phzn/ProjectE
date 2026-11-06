using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;

namespace Unity.Entities.Content
{
	// Token: 0x020004A7 RID: 1191
	internal static class RuntimeContentManagerProfiler
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000654F3 File Offset: 0x000636F3
		private static ref bool s_Initialized
		{
			get
			{
				return RuntimeContentManagerProfiler.SharedInit.Ref.Data;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x000654FF File Offset: 0x000636FF
		public unsafe static Guid Guid
		{
			get
			{
				return *RuntimeContentManagerProfiler.SharedGuid.Ref.Data;
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00065510 File Offset: 0x00063710
		public unsafe static void Initialize()
		{
			if (*RuntimeContentManagerProfiler.s_Initialized)
			{
				return;
			}
			*RuntimeContentManagerProfiler.SharedGuid.Ref.Data = new Guid("db99cf5bb18a4a68898de7504dca6985");
			*RuntimeContentManagerProfiler.SharedLoadedObjectsCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Loaded Objects", false);
			*RuntimeContentManagerProfiler.SharedLoadedFilesCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Loaded Files", false);
			*RuntimeContentManagerProfiler.SharedLoadedArchivesCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Loaded Archives", false);
			*RuntimeContentManagerProfiler.SharedLoadObjectRequestsCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Object Load Requests", true);
			*RuntimeContentManagerProfiler.SharedReleaseObjectRequestsCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Object Release Requests", true);
			*RuntimeContentManagerProfiler.SharedObjectRefCountCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Object Ref Count", false);
			*RuntimeContentManagerProfiler.SharedProcessTimeCounter.Ref.Data = new RuntimeContentManagerProfiler.TimeCounter("Command Process Time");
			*RuntimeContentManagerProfiler.SharedProcessStartTime.Ref.Data = 0L;
			*RuntimeContentManagerProfiler.SharedLoadedScenesCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Loaded Scenes", false);
			*RuntimeContentManagerProfiler.SharedLoadSceneRequestsCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Scene Load Requests", true);
			*RuntimeContentManagerProfiler.SharedUnloadSceneRequestsCounter.Ref.Data = new RuntimeContentManagerProfiler.Counter("Scene Unload Requests", true);
			*RuntimeContentManagerProfiler.s_Initialized = true;
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x00065658 File Offset: 0x00063858
		internal unsafe static void Cleanup()
		{
			if (*RuntimeContentManagerProfiler.s_Initialized)
			{
				return;
			}
			RuntimeContentManagerProfiler.SharedLoadedObjectsCounter.Ref.Data.Value = 0L;
			RuntimeContentManagerProfiler.SharedLoadedFilesCounter.Ref.Data.Value = 0L;
			RuntimeContentManagerProfiler.SharedLoadedArchivesCounter.Ref.Data.Value = 0L;
			RuntimeContentManagerProfiler.SharedLoadedScenesCounter.Ref.Data.Value = 0L;
			*RuntimeContentManagerProfiler.s_Initialized = false;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000656B9 File Offset: 0x000638B9
		public unsafe static void EnterProcessCommands()
		{
			*RuntimeContentManagerProfiler.SharedProcessStartTime.Ref.Data = ProfilerUnsafeUtility.Timestamp;
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000656CC File Offset: 0x000638CC
		public unsafe static void ExitProcessCommands()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ProfilerUnsafeUtility.TimestampConversionRatio timestampToNanosecondsConversionRatio = ProfilerUnsafeUtility.TimestampToNanosecondsConversionRatio;
			long num = (ProfilerUnsafeUtility.Timestamp - *RuntimeContentManagerProfiler.SharedProcessStartTime.Ref.Data) * timestampToNanosecondsConversionRatio.Numerator / timestampToNanosecondsConversionRatio.Denominator;
			RuntimeContentManagerProfiler.SharedProcessTimeCounter.Ref.Data.Value += num;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00065720 File Offset: 0x00063920
		public unsafe static void RecordLoadSceneRequest()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadSceneRequestsCounter.Ref.Data;
			long num = data.Value;
			data.Value = num + 1L;
			ref RuntimeContentManagerProfiler.Counter data2 = ref RuntimeContentManagerProfiler.SharedLoadedScenesCounter.Ref.Data;
			num = data2.Value;
			data2.Value = num + 1L;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x0006576C File Offset: 0x0006396C
		public unsafe static void RecordUnloadSceneRequest()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedUnloadSceneRequestsCounter.Ref.Data;
			long num = data.Value;
			data.Value = num + 1L;
			ref RuntimeContentManagerProfiler.Counter data2 = ref RuntimeContentManagerProfiler.SharedLoadedScenesCounter.Ref.Data;
			num = data2.Value;
			data2.Value = num - 1L;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000657B8 File Offset: 0x000639B8
		public unsafe static void RecordLoadObjectRequest()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadObjectRequestsCounter.Ref.Data;
			long num = data.Value;
			data.Value = num + 1L;
			ref RuntimeContentManagerProfiler.Counter data2 = ref RuntimeContentManagerProfiler.SharedObjectRefCountCounter.Ref.Data;
			num = data2.Value;
			data2.Value = num + 1L;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00065804 File Offset: 0x00063A04
		internal unsafe static void RecordReleaseObjectRequest()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedReleaseObjectRequestsCounter.Ref.Data;
			long num = data.Value;
			data.Value = num + 1L;
			ref RuntimeContentManagerProfiler.Counter data2 = ref RuntimeContentManagerProfiler.SharedObjectRefCountCounter.Ref.Data;
			num = data2.Value;
			data2.Value = num - 1L;
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00065850 File Offset: 0x00063A50
		public unsafe static void RecordLoadObject()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedObjectsCounter.Ref.Data;
			long value = data.Value;
			data.Value = value + 1L;
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00065880 File Offset: 0x00063A80
		public unsafe static void RecordReleaseObject()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedObjectsCounter.Ref.Data;
			long value = data.Value;
			data.Value = value - 1L;
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000658B0 File Offset: 0x00063AB0
		public unsafe static void RecordLoadFile()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedFilesCounter.Ref.Data;
			long value = data.Value;
			data.Value = value + 1L;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000658E0 File Offset: 0x00063AE0
		public unsafe static void RecordUnloadFile()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedFilesCounter.Ref.Data;
			long value = data.Value;
			data.Value = value - 1L;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00065910 File Offset: 0x00063B10
		public unsafe static void RecordLoadArchive()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedArchivesCounter.Ref.Data;
			long value = data.Value;
			data.Value = value + 1L;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00065940 File Offset: 0x00063B40
		public unsafe static void RecordUnloadArchive()
		{
			if (!(*RuntimeContentManagerProfiler.s_Initialized))
			{
				return;
			}
			ref RuntimeContentManagerProfiler.Counter data = ref RuntimeContentManagerProfiler.SharedLoadedArchivesCounter.Ref.Data;
			long value = data.Value;
			data.Value = value - 1L;
		}

		// Token: 0x04000A5A RID: 2650
		internal const string k_CategoryName = "Runtime Content";

		// Token: 0x04000A5B RID: 2651
		internal const string k_LoadedObjectsCounterName = "Loaded Objects";

		// Token: 0x04000A5C RID: 2652
		internal const string k_LoadedScenesCounterName = "Loaded Scenes";

		// Token: 0x04000A5D RID: 2653
		internal const string k_LoadSceneRequestsCounterName = "Scene Load Requests";

		// Token: 0x04000A5E RID: 2654
		internal const string k_UnloadSceneRequestsCounterName = "Scene Unload Requests";

		// Token: 0x04000A5F RID: 2655
		internal const string k_LoadedFilesCounterName = "Loaded Files";

		// Token: 0x04000A60 RID: 2656
		internal const string k_LoadedArchivesCounterName = "Loaded Archives";

		// Token: 0x04000A61 RID: 2657
		internal const string k_LoadObjectRequestsCounterName = "Object Load Requests";

		// Token: 0x04000A62 RID: 2658
		internal const string k_ReleaseObjectRequestsCounterName = "Object Release Requests";

		// Token: 0x04000A63 RID: 2659
		internal const string k_ObjectRefsCounterName = "Object Ref Count";

		// Token: 0x04000A64 RID: 2660
		internal const string k_ProcessCommandsFrameTimeCounterName = "Command Process Time";

		// Token: 0x020004A8 RID: 1192
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_PROFILER")]
		public readonly struct Counter
		{
			// Token: 0x1700024D RID: 589
			// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x00065970 File Offset: 0x00063B70
			// (set) Token: 0x06001CFA RID: 7418 RVA: 0x0006597D File Offset: 0x00063B7D
			public long Value
			{
				get
				{
					return this.m_Counter.Value;
				}
				set
				{
					this.m_Counter.Value = value;
				}
			}

			// Token: 0x06001CFB RID: 7419 RVA: 0x0006598B File Offset: 0x00063B8B
			[ExcludeFromBurstCompatTesting("Takes managed string")]
			public Counter(string name, bool perFrame)
			{
				this.m_Counter = new ProfilerCounterValue<long>(ProfilerCategory.Loading, name, ProfilerMarkerDataUnit.Count, perFrame ? (ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush) : ProfilerCounterOptions.FlushOnEndOfFrame);
				this.m_Counter.Value = 0L;
			}

			// Token: 0x04000A65 RID: 2661
			private readonly ProfilerCounterValue<long> m_Counter;
		}

		// Token: 0x020004A9 RID: 1193
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_PROFILER")]
		public readonly struct TimeCounter
		{
			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06001CFC RID: 7420 RVA: 0x000659B3 File Offset: 0x00063BB3
			// (set) Token: 0x06001CFD RID: 7421 RVA: 0x000659C0 File Offset: 0x00063BC0
			public long Value
			{
				get
				{
					return this.m_Counter.Value;
				}
				set
				{
					this.m_Counter.Value = value;
				}
			}

			// Token: 0x06001CFE RID: 7422 RVA: 0x000659CE File Offset: 0x00063BCE
			[ExcludeFromBurstCompatTesting("Takes managed string")]
			public TimeCounter(string name)
			{
				this.m_Counter = new ProfilerCounterValue<long>(ProfilerCategory.Loading, name, ProfilerMarkerDataUnit.TimeNanoseconds, ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);
			}

			// Token: 0x04000A66 RID: 2662
			private readonly ProfilerCounterValue<long> m_Counter;
		}

		// Token: 0x020004AA RID: 1194
		private sealed class SharedGuid
		{
			// Token: 0x04000A67 RID: 2663
			internal static readonly SharedStatic<Guid> Ref = SharedStatic<Guid>.GetOrCreateUnsafe(0U, -1155307064413587928L, 0L);
		}

		// Token: 0x020004AB RID: 1195
		private sealed class SharedInit
		{
			// Token: 0x04000A68 RID: 2664
			internal static readonly SharedStatic<bool> Ref = SharedStatic<bool>.GetOrCreateUnsafe(0U, -4126830033592846115L, 0L);
		}

		// Token: 0x020004AC RID: 1196
		private sealed class SharedLoadedObjectsCounter
		{
			// Token: 0x04000A69 RID: 2665
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, 876604785633155754L, 0L);
		}

		// Token: 0x020004AD RID: 1197
		private sealed class SharedLoadedFilesCounter
		{
			// Token: 0x04000A6A RID: 2666
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, -8618209498389602759L, 0L);
		}

		// Token: 0x020004AE RID: 1198
		private sealed class SharedLoadedArchivesCounter
		{
			// Token: 0x04000A6B RID: 2667
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, -8103512047144967171L, 0L);
		}

		// Token: 0x020004AF RID: 1199
		private sealed class SharedLoadObjectRequestsCounter
		{
			// Token: 0x04000A6C RID: 2668
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, -7563482030588038630L, 0L);
		}

		// Token: 0x020004B0 RID: 1200
		private sealed class SharedReleaseObjectRequestsCounter
		{
			// Token: 0x04000A6D RID: 2669
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, 7829804822223170693L, 0L);
		}

		// Token: 0x020004B1 RID: 1201
		private sealed class SharedObjectRefCountCounter
		{
			// Token: 0x04000A6E RID: 2670
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, 7020701634031468652L, 0L);
		}

		// Token: 0x020004B2 RID: 1202
		private sealed class SharedProcessTimeCounter
		{
			// Token: 0x04000A6F RID: 2671
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.TimeCounter> Ref = SharedStatic<RuntimeContentManagerProfiler.TimeCounter>.GetOrCreateUnsafe(0U, 723109725950093045L, 0L);
		}

		// Token: 0x020004B3 RID: 1203
		private sealed class SharedProcessStartTime
		{
			// Token: 0x04000A70 RID: 2672
			internal static readonly SharedStatic<long> Ref = SharedStatic<long>.GetOrCreateUnsafe(0U, -4919119141719122583L, 0L);
		}

		// Token: 0x020004B4 RID: 1204
		private sealed class SharedLoadedScenesCounter
		{
			// Token: 0x04000A71 RID: 2673
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, -8191373118598409047L, 0L);
		}

		// Token: 0x020004B5 RID: 1205
		private sealed class SharedLoadSceneRequestsCounter
		{
			// Token: 0x04000A72 RID: 2674
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, 5367896169843424137L, 0L);
		}

		// Token: 0x020004B6 RID: 1206
		private sealed class SharedUnloadSceneRequestsCounter
		{
			// Token: 0x04000A73 RID: 2675
			internal static readonly SharedStatic<RuntimeContentManagerProfiler.Counter> Ref = SharedStatic<RuntimeContentManagerProfiler.Counter>.GetOrCreateUnsafe(0U, -4583447287418090470L, 0L);
		}
	}
}
