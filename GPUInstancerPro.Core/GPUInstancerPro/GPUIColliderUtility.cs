using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000060 RID: 96
	public static class GPUIColliderUtility
	{
		// Token: 0x06000370 RID: 880 RVA: 0x000135E4 File Offset: 0x000117E4
		public static Collider CopyColliderValues(Collider source, Vector3 centerOffset, GameObject target)
		{
			if (source == null || target == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Source or Target is null.");
				return null;
			}
			Collider collider = null;
			BoxCollider boxCollider = source as BoxCollider;
			if (boxCollider != null)
			{
				BoxCollider boxCollider2 = target.AddComponent<BoxCollider>();
				GPUIColliderUtility.CopyBoxColliderValues(boxCollider, boxCollider2, centerOffset);
				collider = boxCollider2;
			}
			else
			{
				SphereCollider sphereCollider = source as SphereCollider;
				if (sphereCollider != null)
				{
					SphereCollider sphereCollider2 = target.AddComponent<SphereCollider>();
					GPUIColliderUtility.CopySphereColliderValues(sphereCollider, sphereCollider2, centerOffset);
					collider = sphereCollider2;
				}
				else
				{
					CapsuleCollider capsuleCollider = source as CapsuleCollider;
					if (capsuleCollider != null)
					{
						CapsuleCollider capsuleCollider2 = target.AddComponent<CapsuleCollider>();
						GPUIColliderUtility.CopyCapsuleColliderValues(capsuleCollider, capsuleCollider2, centerOffset);
						collider = capsuleCollider2;
					}
					else
					{
						MeshCollider meshCollider = source as MeshCollider;
						if (meshCollider != null)
						{
							MeshCollider meshCollider2 = target.AddComponent<MeshCollider>();
							GPUIColliderUtility.CopyMeshColliderValues(meshCollider, meshCollider2, centerOffset);
							collider = meshCollider2;
						}
					}
				}
			}
			return collider;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001369D File Offset: 0x0001189D
		public static void CopyBoxColliderValues(BoxCollider source, BoxCollider target, Vector3 offset)
		{
			target.center = source.center + offset;
			target.size = source.size;
			GPUIColliderUtility.CopyColliderValues(source, target);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000136C4 File Offset: 0x000118C4
		public static void CopySphereColliderValues(SphereCollider source, SphereCollider target, Vector3 offset)
		{
			target.center = source.center + offset;
			target.radius = source.radius;
			GPUIColliderUtility.CopyColliderValues(source, target);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000136EB File Offset: 0x000118EB
		public static void CopyCapsuleColliderValues(CapsuleCollider source, CapsuleCollider target, Vector3 offset)
		{
			target.center = source.center + offset;
			target.radius = source.radius;
			target.height = source.height;
			target.direction = source.direction;
			GPUIColliderUtility.CopyColliderValues(source, target);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001372A File Offset: 0x0001192A
		public static void CopyMeshColliderValues(MeshCollider source, MeshCollider target, Vector3 offset)
		{
			target.sharedMesh = source.sharedMesh;
			target.convex = source.convex;
			GPUIColliderUtility.CopyColliderValues(source, target);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001374B File Offset: 0x0001194B
		private static void CopyColliderValues(Collider source, Collider target)
		{
			target.isTrigger = source.isTrigger;
			target.contactOffset = source.contactOffset;
			target.sharedMaterial = source.sharedMaterial;
			target.excludeLayers = source.excludeLayers;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00013780 File Offset: 0x00011980
		public static void ReplaceOtherCollidersWithMeshColliders(GameObject parentGO, out List<Collider> disabledColliders, out List<MeshCollider> addedColliders, int layerMask)
		{
			disabledColliders = new List<Collider>(parentGO.GetComponentsInChildren<Collider>());
			for (int i = 0; i < disabledColliders.Count; i++)
			{
				Collider collider = disabledColliders[i];
				if (!(collider is MeshCollider) && collider.enabled && GPUIUtility.IsInLayer(layerMask, collider.gameObject.layer))
				{
					collider.enabled = false;
				}
				else
				{
					disabledColliders.RemoveAt(i);
					i--;
				}
			}
			addedColliders = new List<MeshCollider>();
			GPUIColliderUtility.AddMeshCollidersForEachMeshFilter(parentGO.transform, ref addedColliders, layerMask);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00013804 File Offset: 0x00011A04
		private static void AddMeshCollidersForEachMeshFilter(Transform parentTransform, ref List<MeshCollider> addedColliders, int layerMask)
		{
			MeshFilter meshFilter;
			if (!parentTransform.HasComponent<MeshCollider>() && GPUIUtility.IsInLayer(layerMask, parentTransform.gameObject.layer) && parentTransform.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.sharedMesh != null)
			{
				MeshCollider meshCollider = parentTransform.gameObject.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = meshFilter.sharedMesh;
				addedColliders.Add(meshCollider);
			}
			for (int i = 0; i < parentTransform.childCount; i++)
			{
				GPUIColliderUtility.AddMeshCollidersForEachMeshFilter(parentTransform.GetChild(i), ref addedColliders, layerMask);
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00013884 File Offset: 0x00011A84
		public static void RevertAddedMeshCollidersAndDisabledColliders(List<Collider> disabledColliders, List<MeshCollider> addedColliders)
		{
			foreach (MeshCollider meshCollider in addedColliders)
			{
				meshCollider.DestroyGeneric();
			}
			foreach (Collider collider in disabledColliders)
			{
				collider.enabled = true;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001390C File Offset: 0x00011B0C
		public static void CopyRigidbodySettings(Rigidbody source, Rigidbody target)
		{
			target.drag = source.drag;
			target.angularDrag = source.angularDrag;
			target.interpolation = source.interpolation;
			target.collisionDetectionMode = source.collisionDetectionMode;
			target.useGravity = source.useGravity;
			target.isKinematic = source.isKinematic;
			target.constraints = source.constraints;
			target.maxAngularVelocity = source.maxAngularVelocity;
			target.sleepThreshold = source.sleepThreshold;
		}
	}
}
