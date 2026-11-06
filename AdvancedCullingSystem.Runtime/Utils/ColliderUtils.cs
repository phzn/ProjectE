using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Utils
{
	// Token: 0x0200000A RID: 10
	public static class ColliderUtils
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public static Collider Duplicate(Collider original)
		{
			if (original == null)
			{
				throw new ArgumentNullException("ColliderUtils::Duplicate 'original' collider is null");
			}
			Collider collider = (Collider)new GameObject("SC_Collider").AddComponent(original.GetType());
			ColliderUtils.CopyColliderProperties(original, collider);
			return collider;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B1C File Offset: 0x00000D1C
		public static void CopyColliderProperties(Collider original, Collider copy)
		{
			copy.isTrigger = original.isTrigger;
			copy.sharedMaterial = original.sharedMaterial;
			if (original is BoxCollider)
			{
				ColliderUtils.CopyBoxColliderProperties((BoxCollider)original, (BoxCollider)copy);
				return;
			}
			if (original is SphereCollider)
			{
				ColliderUtils.CopySphereColliderProperties((SphereCollider)original, (SphereCollider)copy);
				return;
			}
			if (original is CapsuleCollider)
			{
				ColliderUtils.CopyCapsuleColliderProperties((CapsuleCollider)original, (CapsuleCollider)copy);
				return;
			}
			if (original is MeshCollider)
			{
				ColliderUtils.CopyMeshColliderProperties((MeshCollider)original, (MeshCollider)copy);
				return;
			}
			if (original is TerrainCollider)
			{
				ColliderUtils.CopyTerrainColliderProperties((TerrainCollider)original, (TerrainCollider)copy);
				return;
			}
			Debug.Log(string.Format("ColliderUtils::CopyColliderProperties {0} type not implemented", original.GetType()));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BD8 File Offset: 0x00000DD8
		private static void CopyBoxColliderProperties(BoxCollider original, BoxCollider copy)
		{
			copy.center = original.center;
			copy.size = original.size;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002BF2 File Offset: 0x00000DF2
		private static void CopySphereColliderProperties(SphereCollider original, SphereCollider copy)
		{
			copy.center = original.center;
			copy.radius = original.radius;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C0C File Offset: 0x00000E0C
		private static void CopyCapsuleColliderProperties(CapsuleCollider original, CapsuleCollider copy)
		{
			copy.center = original.center;
			copy.height = original.height;
			copy.radius = original.radius;
			copy.direction = original.direction;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C3E File Offset: 0x00000E3E
		private static void CopyMeshColliderProperties(MeshCollider original, MeshCollider copy)
		{
			copy.sharedMesh = original.sharedMesh;
			copy.convex = original.convex;
			copy.cookingOptions = original.cookingOptions;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C64 File Offset: 0x00000E64
		private static void CopyTerrainColliderProperties(TerrainCollider original, TerrainCollider copy)
		{
			copy.terrainData = original.terrainData;
		}
	}
}
