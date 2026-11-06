using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000707 RID: 1799
	public static class VehicleDismountSafety
	{
		// Token: 0x06002E3D RID: 11837 RVA: 0x00103D84 File Offset: 0x00101F84
		public static Quaternion UprightRotation(Transform vehicle, Transform rider)
		{
			Vector3 vector = ((vehicle != null) ? Vector3.ProjectOnPlane(vehicle.forward, Vector3.up) : Vector3.zero);
			if (vector.sqrMagnitude < 0.001f && rider != null)
			{
				vector = Vector3.ProjectOnPlane(rider.forward, Vector3.up);
			}
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x00103E00 File Offset: 0x00102000
		public static Vector3 FindSafeExitPosition(Transform vehicle, Transform rider, CapsuleCollider capsule, float sideDistance = 1.15f)
		{
			Quaternion quaternion = VehicleDismountSafety.UprightRotation(vehicle, rider);
			Vector3 vector = quaternion * Vector3.forward;
			Vector3 vector2 = quaternion * Vector3.right;
			Vector3 vector3 = ((vehicle != null) ? vehicle.position : rider.position);
			Vector3[] array = new Vector3[]
			{
				vector3 - vector2 * sideDistance,
				vector3 + vector2 * sideDistance,
				vector3 - vector2 * (sideDistance + 0.4f),
				vector3 + vector2 * (sideDistance + 0.4f),
				vector3 - vector * (sideDistance + 0.2f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector4 = VehicleDismountSafety.SnapRootToGround(array[i], rider, capsule, vehicle);
				if (capsule == null || VehicleDismountSafety.IsCapsuleClear(vector4, quaternion, rider, capsule, vehicle))
				{
					return vector4;
				}
			}
			return VehicleDismountSafety.SnapRootToGround(array[array.Length - 1], rider, capsule, vehicle);
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x00103F18 File Offset: 0x00102118
		public static void RestoreRiderPhysics(Transform rider, Transform originalParent, Vector3 worldPosition, Quaternion worldRotation, Vector3 originalLocalScale, CapsuleCollider capsule, bool restoreCapsuleEnabled, Rigidbody body, bool restoreKinematic, Scene originalScene = default(Scene))
		{
			if (rider == null)
			{
				return;
			}
			if (capsule != null)
			{
				capsule.enabled = false;
			}
			if (body != null)
			{
				if (body.isKinematic && !restoreKinematic)
				{
					body.isKinematic = false;
				}
				if (!body.isKinematic)
				{
					body.velocity = Vector3.zero;
					body.angularVelocity = Vector3.zero;
				}
			}
			object obj = originalScene.IsValid() && originalScene.isLoaded;
			Transform transform = ((originalParent != null) ? originalParent : null);
			object obj2 = obj;
			if (obj2 != null && transform != null && transform.gameObject.scene != originalScene)
			{
				transform = null;
			}
			rider.SetParent(transform, true);
			if (obj2 != null && rider.gameObject.scene != originalScene)
			{
				rider.SetParent(null, true);
				SceneManager.MoveGameObjectToScene(rider.gameObject, originalScene);
			}
			rider.position = worldPosition;
			rider.rotation = VehicleDismountSafety.UprightOnly(worldRotation);
			rider.localScale = originalLocalScale;
			if (body != null)
			{
				body.position = worldPosition;
				body.rotation = rider.rotation;
			}
			Physics.SyncTransforms();
			if (body != null)
			{
				body.isKinematic = restoreKinematic;
				if (!body.isKinematic)
				{
					body.velocity = Vector3.zero;
					body.angularVelocity = Vector3.zero;
				}
			}
			if (capsule != null)
			{
				capsule.enabled = restoreCapsuleEnabled;
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x00104080 File Offset: 0x00102280
		private static Quaternion UprightOnly(Quaternion rotation)
		{
			Vector3 vector = Vector3.ProjectOnPlane(rotation * Vector3.forward, Vector3.up);
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x001040C8 File Offset: 0x001022C8
		private static Vector3 SnapRootToGround(Vector3 rootPosition, Transform rider, CapsuleCollider capsule, Transform vehicle)
		{
			float capsuleBottomOffset = VehicleDismountSafety.GetCapsuleBottomOffset(rider, capsule);
			RaycastHit[] array = Physics.RaycastAll(rootPosition + Vector3.up * 1.5f, Vector3.down, 3f, -1, QueryTriggerInteraction.Ignore);
			float num = float.NegativeInfinity;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = array[i].transform;
				if (!(transform == null) && !VehicleDismountSafety.IsPartOf(transform, rider) && !VehicleDismountSafety.IsPartOf(transform, vehicle) && array[i].normal.y >= 0.45f)
				{
					num = Mathf.Max(num, array[i].point.y);
				}
			}
			if (!float.IsNegativeInfinity(num))
			{
				rootPosition.y = num - capsuleBottomOffset + 0.025f;
			}
			return rootPosition;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x00104190 File Offset: 0x00102390
		private static bool IsCapsuleClear(Vector3 rootPosition, Quaternion rootRotation, Transform rider, CapsuleCollider capsule, Transform vehicle)
		{
			Vector3 vector;
			Vector3 vector2;
			float num;
			VehicleDismountSafety.GetWorldCapsule(rootPosition, rootRotation, rider, capsule, out vector, out vector2, out num);
			Collider[] array = Physics.OverlapCapsule(vector, vector2, Mathf.Max(0.01f, num - 0.025f), -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = ((array[i] != null) ? array[i].transform : null);
				if (transform != null && !VehicleDismountSafety.IsPartOf(transform, rider) && !VehicleDismountSafety.IsPartOf(transform, vehicle))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x00104218 File Offset: 0x00102418
		private static void GetWorldCapsule(Vector3 rootPosition, Quaternion rootRotation, Transform rider, CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB, out float radius)
		{
			Vector3 vector = ((rider != null) ? rider.lossyScale : Vector3.one);
			Vector3 vector2 = ((capsule.direction == 0) ? Vector3.right : ((capsule.direction == 2) ? Vector3.forward : Vector3.up));
			Vector3 normalized = (rootRotation * vector2).normalized;
			float num = ((capsule.direction == 0) ? Mathf.Abs(vector.x) : ((capsule.direction == 2) ? Mathf.Abs(vector.z) : Mathf.Abs(vector.y)));
			float num2 = ((capsule.direction == 0) ? Mathf.Max(Mathf.Abs(vector.y), Mathf.Abs(vector.z)) : ((capsule.direction == 2) ? Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y)) : Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.z))));
			radius = capsule.radius * num2;
			float num3 = Mathf.Max(0f, capsule.height * num * 0.5f - radius);
			Vector3 vector3 = rootPosition + rootRotation * Vector3.Scale(capsule.center, vector);
			pointA = vector3 + normalized * num3;
			pointB = vector3 - normalized * num3;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x00104384 File Offset: 0x00102584
		private static float GetCapsuleBottomOffset(Transform rider, CapsuleCollider capsule)
		{
			if (rider == null || capsule == null || capsule.direction != 1)
			{
				return 0f;
			}
			float num = Mathf.Abs(rider.lossyScale.y);
			return (capsule.center.y - capsule.height * 0.5f) * num;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x001043DD File Offset: 0x001025DD
		private static bool IsPartOf(Transform candidate, Transform root)
		{
			return root != null && (candidate == root || candidate.IsChildOf(root));
		}

		// Token: 0x04002E85 RID: 11909
		private const float GroundProbeHeight = 1.5f;

		// Token: 0x04002E86 RID: 11910
		private const float GroundProbeDistance = 3f;

		// Token: 0x04002E87 RID: 11911
		private const float GroundClearance = 0.025f;

		// Token: 0x04002E88 RID: 11912
		private const float MinimumGroundNormalY = 0.45f;
	}
}
