using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x02000017 RID: 23
	[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
	[NativeHeader("Modules/AI/NavMeshManager.h")]
	[MovedFrom("UnityEngine")]
	[StaticAccessor("NavMeshBindings", StaticAccessorType.DoubleColon)]
	public static class NavMesh
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00002FC4 File Offset: 0x000011C4
		[RequiredByNativeCode]
		private static void Internal_CallOnNavMeshPreUpdate()
		{
			bool flag = NavMesh.onPreUpdate != null;
			if (flag)
			{
				NavMesh.onPreUpdate();
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00002FE9 File Offset: 0x000011E9
		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int areaMask)
		{
			return NavMesh.Raycast_Injected(ref sourcePosition, ref targetPosition, out hit, areaMask);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00002FF8 File Offset: 0x000011F8
		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			path.ClearCorners();
			return NavMesh.CalculatePathInternal(sourcePosition, targetPosition, areaMask, path);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000301A File Offset: 0x0000121A
		private static bool CalculatePathInternal(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			return NavMesh.CalculatePathInternal_Injected(ref sourcePosition, ref targetPosition, areaMask, path);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003027 File Offset: 0x00001227
		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, int areaMask)
		{
			return NavMesh.FindClosestEdge_Injected(ref sourcePosition, out hit, areaMask);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003032 File Offset: 0x00001232
		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
		{
			return NavMesh.SamplePosition_Injected(ref sourcePosition, out hit, maxDistance, areaMask);
		}

		// Token: 0x06000131 RID: 305
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[NativeName("SetAreaCost")]
		[Obsolete("Use SetAreaCost instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLayerCost(int layer, float cost);

		// Token: 0x06000132 RID: 306
		[Obsolete("Use GetAreaCost instead.")]
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[NativeName("GetAreaCost")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetLayerCost(int layer);

		// Token: 0x06000133 RID: 307
		[Obsolete("Use GetAreaFromName instead.")]
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[NativeName("GetAreaFromName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNavMeshLayerFromName(string layerName);

		// Token: 0x06000134 RID: 308
		[NativeName("SetAreaCost")]
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAreaCost(int areaIndex, float cost);

		// Token: 0x06000135 RID: 309
		[NativeName("GetAreaCost")]
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAreaCost(int areaIndex);

		// Token: 0x06000136 RID: 310
		[NativeName("GetAreaFromName")]
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAreaFromName(string areaName);

		// Token: 0x06000137 RID: 311 RVA: 0x00003040 File Offset: 0x00001240
		public static NavMeshTriangulation CalculateTriangulation()
		{
			NavMeshTriangulation navMeshTriangulation;
			NavMesh.CalculateTriangulation_Injected(out navMeshTriangulation);
			return navMeshTriangulation;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003058 File Offset: 0x00001258
		[Obsolete("use NavMesh.CalculateTriangulation() instead.")]
		public static void Triangulate(out Vector3[] vertices, out int[] indices)
		{
			NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
			vertices = navMeshTriangulation.vertices;
			indices = navMeshTriangulation.indices;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000307C File Offset: 0x0000127C
		[Obsolete("AddOffMeshLinks has no effect and is deprecated.")]
		public static void AddOffMeshLinks()
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000307C File Offset: 0x0000127C
		[Obsolete("RestoreNavMesh has no effect and is deprecated.")]
		public static void RestoreNavMesh()
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013B RID: 315
		// (set) Token: 0x0600013C RID: 316
		[StaticAccessor("GetNavMeshManager()")]
		public static extern float avoidancePredictionTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600013D RID: 317
		// (set) Token: 0x0600013E RID: 318
		[StaticAccessor("GetNavMeshManager()")]
		public static extern int pathfindingIterationsPerFrame
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003080 File Offset: 0x00001280
		public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData)
		{
			bool flag = navMeshData == null;
			if (flag)
			{
				throw new ArgumentNullException("navMeshData");
			}
			return new NavMeshDataInstance
			{
				id = NavMesh.AddNavMeshDataInternal(navMeshData)
			};
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000030C0 File Offset: 0x000012C0
		public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
		{
			bool flag = navMeshData == null;
			if (flag)
			{
				throw new ArgumentNullException("navMeshData");
			}
			return new NavMeshDataInstance
			{
				id = NavMesh.AddNavMeshDataTransformedInternal(navMeshData, position, rotation)
			};
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003101 File Offset: 0x00001301
		public static void RemoveNavMeshData(NavMeshDataInstance handle)
		{
			NavMesh.RemoveNavMeshDataInternal(handle.id);
		}

		// Token: 0x06000142 RID: 322
		[StaticAccessor("GetNavMeshManager()")]
		[NativeName("IsValidSurfaceID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsValidNavMeshDataHandle(int handle);

		// Token: 0x06000143 RID: 323
		[StaticAccessor("GetNavMeshManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsValidLinkHandle(int handle);

		// Token: 0x06000144 RID: 324
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object InternalGetOwner(int dataID);

		// Token: 0x06000145 RID: 325
		[StaticAccessor("GetNavMeshManager()")]
		[NativeName("SetSurfaceUserID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalSetOwner(int dataID, int ownerID);

		// Token: 0x06000146 RID: 326
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object InternalGetLinkOwner(int linkID);

		// Token: 0x06000147 RID: 327
		[NativeName("SetLinkUserID")]
		[StaticAccessor("GetNavMeshManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalSetLinkOwner(int linkID, int ownerID);

		// Token: 0x06000148 RID: 328
		[NativeName("LoadData")]
		[StaticAccessor("GetNavMeshManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int AddNavMeshDataInternal(NavMeshData navMeshData);

		// Token: 0x06000149 RID: 329 RVA: 0x00003111 File Offset: 0x00001311
		[NativeName("LoadData")]
		[StaticAccessor("GetNavMeshManager()")]
		internal static int AddNavMeshDataTransformedInternal(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
		{
			return NavMesh.AddNavMeshDataTransformedInternal_Injected(navMeshData, ref position, ref rotation);
		}

		// Token: 0x0600014A RID: 330
		[NativeName("UnloadData")]
		[StaticAccessor("GetNavMeshManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveNavMeshDataInternal(int handle);

		// Token: 0x0600014B RID: 331 RVA: 0x00003120 File Offset: 0x00001320
		public static NavMeshLinkInstance AddLink(NavMeshLinkData link)
		{
			return new NavMeshLinkInstance
			{
				id = NavMesh.AddLinkInternal(link, Vector3.zero, Quaternion.identity)
			};
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00003154 File Offset: 0x00001354
		public static NavMeshLinkInstance AddLink(NavMeshLinkData link, Vector3 position, Quaternion rotation)
		{
			return new NavMeshLinkInstance
			{
				id = NavMesh.AddLinkInternal(link, position, rotation)
			};
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000317F File Offset: 0x0000137F
		public static void RemoveLink(NavMeshLinkInstance handle)
		{
			NavMesh.RemoveLinkInternal(handle.id);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000318F File Offset: 0x0000138F
		[NativeName("AddLink")]
		[StaticAccessor("GetNavMeshManager()")]
		internal static int AddLinkInternal(NavMeshLinkData link, Vector3 position, Quaternion rotation)
		{
			return NavMesh.AddLinkInternal_Injected(ref link, ref position, ref rotation);
		}

		// Token: 0x0600014F RID: 335
		[StaticAccessor("GetNavMeshManager()")]
		[NativeName("RemoveLink")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveLinkInternal(int handle);

		// Token: 0x06000150 RID: 336 RVA: 0x0000319C File Offset: 0x0000139C
		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, NavMeshQueryFilter filter)
		{
			return NavMesh.SamplePositionFilter(sourcePosition, out hit, maxDistance, filter.agentTypeID, filter.areaMask);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000031C4 File Offset: 0x000013C4
		private static bool SamplePositionFilter(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask)
		{
			return NavMesh.SamplePositionFilter_Injected(ref sourcePosition, out hit, maxDistance, type, mask);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000031D4 File Offset: 0x000013D4
		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, NavMeshQueryFilter filter)
		{
			return NavMesh.FindClosestEdgeFilter(sourcePosition, out hit, filter.agentTypeID, filter.areaMask);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000031FB File Offset: 0x000013FB
		private static bool FindClosestEdgeFilter(Vector3 sourcePosition, out NavMeshHit hit, int type, int mask)
		{
			return NavMesh.FindClosestEdgeFilter_Injected(ref sourcePosition, out hit, type, mask);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00003208 File Offset: 0x00001408
		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, NavMeshQueryFilter filter)
		{
			return NavMesh.RaycastFilter(sourcePosition, targetPosition, out hit, filter.agentTypeID, filter.areaMask);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003230 File Offset: 0x00001430
		private static bool RaycastFilter(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int type, int mask)
		{
			return NavMesh.RaycastFilter_Injected(ref sourcePosition, ref targetPosition, out hit, type, mask);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003240 File Offset: 0x00001440
		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter filter, NavMeshPath path)
		{
			path.ClearCorners();
			return NavMesh.CalculatePathFilterInternal(sourcePosition, targetPosition, path, filter.agentTypeID, filter.areaMask, filter.costs);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003276 File Offset: 0x00001476
		private static bool CalculatePathFilterInternal(Vector3 sourcePosition, Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs)
		{
			return NavMesh.CalculatePathFilterInternal_Injected(ref sourcePosition, ref targetPosition, path, type, mask, costs);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00003288 File Offset: 0x00001488
		[StaticAccessor("GetNavMeshProjectSettings()")]
		public static NavMeshBuildSettings CreateSettings()
		{
			NavMeshBuildSettings navMeshBuildSettings;
			NavMesh.CreateSettings_Injected(out navMeshBuildSettings);
			return navMeshBuildSettings;
		}

		// Token: 0x06000159 RID: 345
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveSettings(int agentTypeID);

		// Token: 0x0600015A RID: 346 RVA: 0x000032A0 File Offset: 0x000014A0
		public static NavMeshBuildSettings GetSettingsByID(int agentTypeID)
		{
			NavMeshBuildSettings navMeshBuildSettings;
			NavMesh.GetSettingsByID_Injected(agentTypeID, out navMeshBuildSettings);
			return navMeshBuildSettings;
		}

		// Token: 0x0600015B RID: 347
		[StaticAccessor("GetNavMeshProjectSettings()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetSettingsCount();

		// Token: 0x0600015C RID: 348 RVA: 0x000032B8 File Offset: 0x000014B8
		public static NavMeshBuildSettings GetSettingsByIndex(int index)
		{
			NavMeshBuildSettings navMeshBuildSettings;
			NavMesh.GetSettingsByIndex_Injected(index, out navMeshBuildSettings);
			return navMeshBuildSettings;
		}

		// Token: 0x0600015D RID: 349
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetSettingsNameFromID(int agentTypeID);

		// Token: 0x0600015E RID: 350
		[NativeName("CleanupAfterCarving")]
		[StaticAccessor("GetNavMeshManager()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveAllNavMeshData();

		// Token: 0x0600015F RID: 351
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Raycast_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int areaMask);

		// Token: 0x06000160 RID: 352
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CalculatePathInternal_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, int areaMask, NavMeshPath path);

		// Token: 0x06000161 RID: 353
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool FindClosestEdge_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, int areaMask);

		// Token: 0x06000162 RID: 354
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SamplePosition_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask);

		// Token: 0x06000163 RID: 355
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalculateTriangulation_Injected(out NavMeshTriangulation ret);

		// Token: 0x06000164 RID: 356
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddNavMeshDataTransformedInternal_Injected(NavMeshData navMeshData, ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06000165 RID: 357
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddLinkInternal_Injected(ref NavMeshLinkData link, ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06000166 RID: 358
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SamplePositionFilter_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask);

		// Token: 0x06000167 RID: 359
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool FindClosestEdgeFilter_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, int type, int mask);

		// Token: 0x06000168 RID: 360
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RaycastFilter_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int type, int mask);

		// Token: 0x06000169 RID: 361
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CalculatePathFilterInternal_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs);

		// Token: 0x0600016A RID: 362
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateSettings_Injected(out NavMeshBuildSettings ret);

		// Token: 0x0600016B RID: 363
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSettingsByID_Injected(int agentTypeID, out NavMeshBuildSettings ret);

		// Token: 0x0600016C RID: 364
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSettingsByIndex_Injected(int index, out NavMeshBuildSettings ret);

		// Token: 0x0400003D RID: 61
		public const int AllAreas = -1;

		// Token: 0x0400003E RID: 62
		public static NavMesh.OnNavMeshPreUpdate onPreUpdate;

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x0600016E RID: 366
		public delegate void OnNavMeshPreUpdate();
	}
}
