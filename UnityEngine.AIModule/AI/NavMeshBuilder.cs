using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI
{
	// Token: 0x02000008 RID: 8
	[NativeHeader("Modules/AI/Builder/NavMeshBuilder.bindings.h")]
	[StaticAccessor("NavMeshBuilderBindings", StaticAccessorType.DoubleColon)]
	public static class NavMeshBuilder
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000026A4 File Offset: 0x000008A4
		public static void CollectSources(Bounds includedWorldBounds, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, bool generateLinksByDefault, List<NavMeshBuildMarkup> markups, bool includeOnlyMarkedObjects, List<NavMeshBuildSource> results)
		{
			bool flag = markups == null;
			if (flag)
			{
				throw new ArgumentNullException("markups");
			}
			bool flag2 = results == null;
			if (flag2)
			{
				throw new ArgumentNullException("results");
			}
			includedWorldBounds.extents = Vector3.Max(includedWorldBounds.extents, 0.001f * Vector3.one);
			NavMeshBuildSource[] array = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, includedWorldBounds, null, true, geometry, defaultArea, generateLinksByDefault, markups.ToArray(), includeOnlyMarkedObjects);
			results.Clear();
			results.AddRange(array);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002725 File Offset: 0x00000925
		public static void CollectSources(Bounds includedWorldBounds, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
		{
			NavMeshBuilder.CollectSources(includedWorldBounds, includedLayerMask, geometry, defaultArea, false, markups, false, results);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002738 File Offset: 0x00000938
		public static void CollectSources(Transform root, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, bool generateLinksByDefault, List<NavMeshBuildMarkup> markups, bool includeOnlyMarkedObjects, List<NavMeshBuildSource> results)
		{
			bool flag = markups == null;
			if (flag)
			{
				throw new ArgumentNullException("markups");
			}
			bool flag2 = results == null;
			if (flag2)
			{
				throw new ArgumentNullException("results");
			}
			NavMeshBuildSource[] array = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, default(Bounds), root, false, geometry, defaultArea, generateLinksByDefault, markups.ToArray(), includeOnlyMarkedObjects);
			results.Clear();
			results.AddRange(array);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000279E File Offset: 0x0000099E
		public static void CollectSources(Transform root, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
		{
			NavMeshBuilder.CollectSources(root, includedLayerMask, geometry, defaultArea, false, markups, false, results);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000027B4 File Offset: 0x000009B4
		private static NavMeshBuildSource[] CollectSourcesInternal(int includedLayerMask, Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, bool generateLinksByDefault, NavMeshBuildMarkup[] markups, bool includeOnlyMarkedObjects)
		{
			return NavMeshBuilder.CollectSourcesInternal_Injected(includedLayerMask, ref includedWorldBounds, root, useBounds, geometry, defaultArea, generateLinksByDefault, markups, includeOnlyMarkedObjects);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000027D8 File Offset: 0x000009D8
		public static NavMeshData BuildNavMeshData(NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds, Vector3 position, Quaternion rotation)
		{
			bool flag = sources == null;
			if (flag)
			{
				throw new ArgumentNullException("sources");
			}
			NavMeshData navMeshData = new NavMeshData(buildSettings.agentTypeID)
			{
				position = position,
				rotation = rotation
			};
			NavMeshBuilder.UpdateNavMeshDataListInternal(navMeshData, buildSettings, sources, localBounds);
			return navMeshData;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002828 File Offset: 0x00000A28
		public static bool UpdateNavMeshData(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = sources == null;
			if (flag2)
			{
				throw new ArgumentNullException("sources");
			}
			return NavMeshBuilder.UpdateNavMeshDataListInternal(data, buildSettings, sources, localBounds);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000286C File Offset: 0x00000A6C
		private static bool UpdateNavMeshDataListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
		{
			return NavMeshBuilder.UpdateNavMeshDataListInternal_Injected(data, ref buildSettings, sources, ref localBounds);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000287C File Offset: 0x00000A7C
		public static AsyncOperation UpdateNavMeshDataAsync(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = sources == null;
			if (flag2)
			{
				throw new ArgumentNullException("sources");
			}
			return NavMeshBuilder.UpdateNavMeshDataAsyncListInternal(data, buildSettings, sources, localBounds);
		}

		// Token: 0x06000050 RID: 80
		[StaticAccessor("GetNavMeshManager().GetNavMeshBuildManager()", StaticAccessorType.Arrow)]
		[NativeMethod("Purge")]
		[NativeHeader("Modules/AI/NavMeshManager.h")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Cancel(NavMeshData data);

		// Token: 0x06000051 RID: 81 RVA: 0x000028C0 File Offset: 0x00000AC0
		private static AsyncOperation UpdateNavMeshDataAsyncListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
		{
			return NavMeshBuilder.UpdateNavMeshDataAsyncListInternal_Injected(data, ref buildSettings, sources, ref localBounds);
		}

		// Token: 0x06000052 RID: 82
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern NavMeshBuildSource[] CollectSourcesInternal_Injected(int includedLayerMask, ref Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, bool generateLinksByDefault, NavMeshBuildMarkup[] markups, bool includeOnlyMarkedObjects);

		// Token: 0x06000053 RID: 83
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UpdateNavMeshDataListInternal_Injected(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);

		// Token: 0x06000054 RID: 84
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOperation UpdateNavMeshDataAsyncListInternal_Injected(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);
	}
}
