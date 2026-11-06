using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.AI;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.AI
{
	// Token: 0x02000007 RID: 7
	[StaticAccessor("NavMeshQueryBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Math/Matrix4x4.h")]
	[NativeHeader("Modules/AI/Public/NavMeshBindingTypes.h")]
	[NativeHeader("Modules/AI/NavMeshExperimental.bindings.h")]
	[NativeContainer]
	public struct NavMeshQuery : IDisposable
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000021A3 File Offset: 0x000003A3
		public NavMeshQuery(NavMeshWorld world, Allocator allocator, int pathNodePoolSize = 0)
		{
			this.m_NavMeshQuery = NavMeshQuery.Create(world, pathNodePoolSize);
			UnsafeUtility.LeakRecord(this.m_NavMeshQuery, LeakCategory.NavMeshQuery, 0);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021C1 File Offset: 0x000003C1
		public void Dispose()
		{
			UnsafeUtility.LeakErase(this.m_NavMeshQuery, LeakCategory.NavMeshQuery);
			NavMeshQuery.Destroy(this.m_NavMeshQuery);
			this.m_NavMeshQuery = IntPtr.Zero;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021E8 File Offset: 0x000003E8
		private static IntPtr Create(NavMeshWorld world, int nodePoolSize)
		{
			return NavMeshQuery.Create_Injected(ref world, nodePoolSize);
		}

		// Token: 0x06000013 RID: 19
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr navMeshQuery);

		// Token: 0x06000014 RID: 20 RVA: 0x000021F4 File Offset: 0x000003F4
		public unsafe PathQueryStatus BeginFindPath(NavMeshLocation start, NavMeshLocation end, int areaMask = -1, NativeArray<float> costs = default(NativeArray<float>))
		{
			void* ptr = ((costs.Length > 0) ? costs.GetUnsafePtr<float>() : null);
			return NavMeshQuery.BeginFindPath(this.m_NavMeshQuery, start, end, areaMask, ptr);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000222C File Offset: 0x0000042C
		public PathQueryStatus UpdateFindPath(int iterations, out int iterationsPerformed)
		{
			return NavMeshQuery.UpdateFindPath(this.m_NavMeshQuery, iterations, out iterationsPerformed);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000224C File Offset: 0x0000044C
		public PathQueryStatus EndFindPath(out int pathSize)
		{
			return NavMeshQuery.EndFindPath(this.m_NavMeshQuery, out pathSize);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000226C File Offset: 0x0000046C
		public int GetPathResult(NativeSlice<PolygonId> path)
		{
			return NavMeshQuery.GetPathResult(this.m_NavMeshQuery, path.GetUnsafePtr<PolygonId>(), path.Length);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002296 File Offset: 0x00000496
		[ThreadSafe]
		private unsafe static PathQueryStatus BeginFindPath(IntPtr navMeshQuery, NavMeshLocation start, NavMeshLocation end, int areaMask, void* costs)
		{
			return NavMeshQuery.BeginFindPath_Injected(navMeshQuery, ref start, ref end, areaMask, costs);
		}

		// Token: 0x06000019 RID: 25
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PathQueryStatus UpdateFindPath(IntPtr navMeshQuery, int iterations, out int iterationsPerformed);

		// Token: 0x0600001A RID: 26
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PathQueryStatus EndFindPath(IntPtr navMeshQuery, out int pathSize);

		// Token: 0x0600001B RID: 27
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int GetPathResult(IntPtr navMeshQuery, void* path, int maxPath);

		// Token: 0x0600001C RID: 28 RVA: 0x000022A5 File Offset: 0x000004A5
		[ThreadSafe]
		private static bool IsValidPolygon(IntPtr navMeshQuery, PolygonId polygon)
		{
			return NavMeshQuery.IsValidPolygon_Injected(navMeshQuery, ref polygon);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022B0 File Offset: 0x000004B0
		public bool IsValid(PolygonId polygon)
		{
			return polygon.polyRef != 0UL && NavMeshQuery.IsValidPolygon(this.m_NavMeshQuery, polygon);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022DC File Offset: 0x000004DC
		public bool IsValid(NavMeshLocation location)
		{
			return this.IsValid(location.polygon);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000022FB File Offset: 0x000004FB
		[ThreadSafe]
		private static int GetAgentTypeIdForPolygon(IntPtr navMeshQuery, PolygonId polygon)
		{
			return NavMeshQuery.GetAgentTypeIdForPolygon_Injected(navMeshQuery, ref polygon);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002308 File Offset: 0x00000508
		public int GetAgentTypeIdForPolygon(PolygonId polygon)
		{
			return NavMeshQuery.GetAgentTypeIdForPolygon(this.m_NavMeshQuery, polygon);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002326 File Offset: 0x00000526
		[ThreadSafe]
		private static bool IsPositionInPolygon(IntPtr navMeshQuery, Vector3 position, PolygonId polygon)
		{
			return NavMeshQuery.IsPositionInPolygon_Injected(navMeshQuery, ref position, ref polygon);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002332 File Offset: 0x00000532
		[ThreadSafe]
		private static PathQueryStatus GetClosestPointOnPoly(IntPtr navMeshQuery, PolygonId polygon, Vector3 position, out Vector3 nearest)
		{
			return NavMeshQuery.GetClosestPointOnPoly_Injected(navMeshQuery, ref polygon, ref position, out nearest);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002340 File Offset: 0x00000540
		public NavMeshLocation CreateLocation(Vector3 position, PolygonId polygon)
		{
			Vector3 vector;
			PathQueryStatus closestPointOnPoly = NavMeshQuery.GetClosestPointOnPoly(this.m_NavMeshQuery, polygon, position, out vector);
			return ((closestPointOnPoly & PathQueryStatus.Success) != (PathQueryStatus)0) ? new NavMeshLocation(vector, polygon) : default(NavMeshLocation);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002380 File Offset: 0x00000580
		[ThreadSafe]
		private static NavMeshLocation MapLocation(IntPtr navMeshQuery, Vector3 position, Vector3 extents, int agentTypeID, int areaMask = -1)
		{
			NavMeshLocation navMeshLocation;
			NavMeshQuery.MapLocation_Injected(navMeshQuery, ref position, ref extents, agentTypeID, areaMask, out navMeshLocation);
			return navMeshLocation;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023A0 File Offset: 0x000005A0
		public NavMeshLocation MapLocation(Vector3 position, Vector3 extents, int agentTypeID, int areaMask = -1)
		{
			return NavMeshQuery.MapLocation(this.m_NavMeshQuery, position, extents, agentTypeID, areaMask);
		}

		// Token: 0x06000026 RID: 38
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void MoveLocations(IntPtr navMeshQuery, void* locations, void* targets, void* areaMasks, int count);

		// Token: 0x06000027 RID: 39 RVA: 0x000023C2 File Offset: 0x000005C2
		public void MoveLocations(NativeSlice<NavMeshLocation> locations, NativeSlice<Vector3> targets, NativeSlice<int> areaMasks)
		{
			NavMeshQuery.MoveLocations(this.m_NavMeshQuery, locations.GetUnsafePtr<NavMeshLocation>(), targets.GetUnsafeReadOnlyPtr<Vector3>(), areaMasks.GetUnsafeReadOnlyPtr<int>(), locations.Length);
		}

		// Token: 0x06000028 RID: 40
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void MoveLocationsInSameAreas(IntPtr navMeshQuery, void* locations, void* targets, int count, int areaMask);

		// Token: 0x06000029 RID: 41 RVA: 0x000023EA File Offset: 0x000005EA
		public void MoveLocationsInSameAreas(NativeSlice<NavMeshLocation> locations, NativeSlice<Vector3> targets, int areaMask = -1)
		{
			NavMeshQuery.MoveLocationsInSameAreas(this.m_NavMeshQuery, locations.GetUnsafePtr<NavMeshLocation>(), targets.GetUnsafeReadOnlyPtr<Vector3>(), locations.Length, areaMask);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002410 File Offset: 0x00000610
		[ThreadSafe]
		private static NavMeshLocation MoveLocation(IntPtr navMeshQuery, NavMeshLocation location, Vector3 target, int areaMask)
		{
			NavMeshLocation navMeshLocation;
			NavMeshQuery.MoveLocation_Injected(navMeshQuery, ref location, ref target, areaMask, out navMeshLocation);
			return navMeshLocation;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000242C File Offset: 0x0000062C
		public NavMeshLocation MoveLocation(NavMeshLocation location, Vector3 target, int areaMask = -1)
		{
			return NavMeshQuery.MoveLocation(this.m_NavMeshQuery, location, target, areaMask);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000244C File Offset: 0x0000064C
		[ThreadSafe]
		private static bool GetPortalPoints(IntPtr navMeshQuery, PolygonId polygon, PolygonId neighbourPolygon, out Vector3 left, out Vector3 right)
		{
			return NavMeshQuery.GetPortalPoints_Injected(navMeshQuery, ref polygon, ref neighbourPolygon, out left, out right);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000245C File Offset: 0x0000065C
		public bool GetPortalPoints(PolygonId polygon, PolygonId neighbourPolygon, out Vector3 left, out Vector3 right)
		{
			return NavMeshQuery.GetPortalPoints(this.m_NavMeshQuery, polygon, neighbourPolygon, out left, out right);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002480 File Offset: 0x00000680
		[ThreadSafe]
		private static Matrix4x4 PolygonLocalToWorldMatrix(IntPtr navMeshQuery, PolygonId polygon)
		{
			Matrix4x4 matrix4x;
			NavMeshQuery.PolygonLocalToWorldMatrix_Injected(navMeshQuery, ref polygon, out matrix4x);
			return matrix4x;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002498 File Offset: 0x00000698
		public Matrix4x4 PolygonLocalToWorldMatrix(PolygonId polygon)
		{
			return NavMeshQuery.PolygonLocalToWorldMatrix(this.m_NavMeshQuery, polygon);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024B8 File Offset: 0x000006B8
		[ThreadSafe]
		private static Matrix4x4 PolygonWorldToLocalMatrix(IntPtr navMeshQuery, PolygonId polygon)
		{
			Matrix4x4 matrix4x;
			NavMeshQuery.PolygonWorldToLocalMatrix_Injected(navMeshQuery, ref polygon, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000024D0 File Offset: 0x000006D0
		public Matrix4x4 PolygonWorldToLocalMatrix(PolygonId polygon)
		{
			return NavMeshQuery.PolygonWorldToLocalMatrix(this.m_NavMeshQuery, polygon);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000024EE File Offset: 0x000006EE
		[ThreadSafe]
		private static NavMeshPolyTypes GetPolygonType(IntPtr navMeshQuery, PolygonId polygon)
		{
			return NavMeshQuery.GetPolygonType_Injected(navMeshQuery, ref polygon);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024F8 File Offset: 0x000006F8
		public NavMeshPolyTypes GetPolygonType(PolygonId polygon)
		{
			return NavMeshQuery.GetPolygonType(this.m_NavMeshQuery, polygon);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002518 File Offset: 0x00000718
		[ThreadSafe]
		private unsafe static PathQueryStatus Raycast(IntPtr navMeshQuery, NavMeshLocation start, Vector3 targetPosition, int areaMask, void* costs, out NavMeshHit hit, void* path, out int pathCount, int maxPath)
		{
			return NavMeshQuery.Raycast_Injected(navMeshQuery, ref start, ref targetPosition, areaMask, costs, out hit, path, out pathCount, maxPath);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000253C File Offset: 0x0000073C
		public unsafe PathQueryStatus Raycast(out NavMeshHit hit, NavMeshLocation start, Vector3 targetPosition, int areaMask = -1, NativeArray<float> costs = default(NativeArray<float>))
		{
			void* ptr = ((costs.Length == 32) ? costs.GetUnsafePtr<float>() : null);
			int num;
			PathQueryStatus pathQueryStatus = NavMeshQuery.Raycast(this.m_NavMeshQuery, start, targetPosition, areaMask, ptr, out hit, null, out num, 0);
			return pathQueryStatus & ~PathQueryStatus.BufferTooSmall;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002584 File Offset: 0x00000784
		public unsafe PathQueryStatus Raycast(out NavMeshHit hit, NativeSlice<PolygonId> path, out int pathCount, NavMeshLocation start, Vector3 targetPosition, int areaMask = -1, NativeArray<float> costs = default(NativeArray<float>))
		{
			void* ptr = ((costs.Length == 32) ? costs.GetUnsafePtr<float>() : null);
			void* ptr2 = ((path.Length > 0) ? path.GetUnsafePtr<PolygonId>() : null);
			int num = ((ptr2 != null) ? path.Length : 0);
			return NavMeshQuery.Raycast(this.m_NavMeshQuery, start, targetPosition, areaMask, ptr, out hit, ptr2, out pathCount, num);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000025EC File Offset: 0x000007EC
		[ThreadSafe]
		private unsafe static PathQueryStatus GetEdgesAndNeighbors(IntPtr navMeshQuery, PolygonId node, int maxVerts, int maxNei, void* verts, void* neighbors, void* edgeIndices, out int vertCount, out int neighborsCount)
		{
			return NavMeshQuery.GetEdgesAndNeighbors_Injected(navMeshQuery, ref node, maxVerts, maxNei, verts, neighbors, edgeIndices, out vertCount, out neighborsCount);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002610 File Offset: 0x00000810
		public unsafe PathQueryStatus GetEdgesAndNeighbors(PolygonId node, NativeSlice<Vector3> edgeVertices, NativeSlice<PolygonId> neighbors, NativeSlice<byte> edgeIndices, out int verticesCount, out int neighborsCount)
		{
			void* ptr = ((edgeVertices.Length > 0) ? edgeVertices.GetUnsafePtr<Vector3>() : null);
			void* ptr2 = ((neighbors.Length > 0) ? neighbors.GetUnsafePtr<PolygonId>() : null);
			void* ptr3 = ((edgeIndices.Length > 0) ? edgeIndices.GetUnsafePtr<byte>() : null);
			int length = edgeVertices.Length;
			int num = ((neighbors.Length > 0) ? neighbors.Length : edgeIndices.Length);
			return NavMeshQuery.GetEdgesAndNeighbors(this.m_NavMeshQuery, node, length, num, ptr, ptr2, ptr3, out verticesCount, out neighborsCount);
		}

		// Token: 0x06000039 RID: 57
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create_Injected(ref NavMeshWorld world, int nodePoolSize);

		// Token: 0x0600003A RID: 58
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern PathQueryStatus BeginFindPath_Injected(IntPtr navMeshQuery, ref NavMeshLocation start, ref NavMeshLocation end, int areaMask, void* costs);

		// Token: 0x0600003B RID: 59
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValidPolygon_Injected(IntPtr navMeshQuery, ref PolygonId polygon);

		// Token: 0x0600003C RID: 60
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAgentTypeIdForPolygon_Injected(IntPtr navMeshQuery, ref PolygonId polygon);

		// Token: 0x0600003D RID: 61
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPositionInPolygon_Injected(IntPtr navMeshQuery, ref Vector3 position, ref PolygonId polygon);

		// Token: 0x0600003E RID: 62
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PathQueryStatus GetClosestPointOnPoly_Injected(IntPtr navMeshQuery, ref PolygonId polygon, ref Vector3 position, out Vector3 nearest);

		// Token: 0x0600003F RID: 63
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MapLocation_Injected(IntPtr navMeshQuery, ref Vector3 position, ref Vector3 extents, int agentTypeID, int areaMask = -1, out NavMeshLocation ret);

		// Token: 0x06000040 RID: 64
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MoveLocation_Injected(IntPtr navMeshQuery, ref NavMeshLocation location, ref Vector3 target, int areaMask, out NavMeshLocation ret);

		// Token: 0x06000041 RID: 65
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPortalPoints_Injected(IntPtr navMeshQuery, ref PolygonId polygon, ref PolygonId neighbourPolygon, out Vector3 left, out Vector3 right);

		// Token: 0x06000042 RID: 66
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PolygonLocalToWorldMatrix_Injected(IntPtr navMeshQuery, ref PolygonId polygon, out Matrix4x4 ret);

		// Token: 0x06000043 RID: 67
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PolygonWorldToLocalMatrix_Injected(IntPtr navMeshQuery, ref PolygonId polygon, out Matrix4x4 ret);

		// Token: 0x06000044 RID: 68
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern NavMeshPolyTypes GetPolygonType_Injected(IntPtr navMeshQuery, ref PolygonId polygon);

		// Token: 0x06000045 RID: 69
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern PathQueryStatus Raycast_Injected(IntPtr navMeshQuery, ref NavMeshLocation start, ref Vector3 targetPosition, int areaMask, void* costs, out NavMeshHit hit, void* path, out int pathCount, int maxPath);

		// Token: 0x06000046 RID: 70
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern PathQueryStatus GetEdgesAndNeighbors_Injected(IntPtr navMeshQuery, ref PolygonId node, int maxVerts, int maxNei, void* verts, void* neighbors, void* edgeIndices, out int vertCount, out int neighborsCount);

		// Token: 0x04000014 RID: 20
		[NativeDisableUnsafePtrRestriction]
		internal IntPtr m_NavMeshQuery;
	}
}
