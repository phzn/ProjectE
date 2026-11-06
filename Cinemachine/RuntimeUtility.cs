using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200004A RID: 74
	[DocumentationSorting(DocumentationSortingAttribute.Level.Undoc)]
	public static class RuntimeUtility
	{
		// Token: 0x06000343 RID: 835 RVA: 0x00014854 File Offset: 0x00012A54
		public static void DestroyObject(Object obj)
		{
			if (obj != null)
			{
				Object.Destroy(obj);
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014865 File Offset: 0x00012A65
		public static bool IsPrefab(GameObject gameObject)
		{
			return false;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00014868 File Offset: 0x00012A68
		public static bool RaycastIgnoreTag(Ray ray, out RaycastHit hitInfo, float rayLength, int layerMask, in string ignoreTag)
		{
			if (ignoreTag.Length == 0)
			{
				if (Physics.Raycast(ray, out hitInfo, rayLength, layerMask, QueryTriggerInteraction.Ignore))
				{
					return true;
				}
			}
			else
			{
				int num = -1;
				int num2 = Physics.RaycastNonAlloc(ray, RuntimeUtility.s_HitBuffer, rayLength, layerMask, QueryTriggerInteraction.Ignore);
				for (int i = 0; i < num2; i++)
				{
					if (!RuntimeUtility.s_HitBuffer[i].collider.CompareTag(ignoreTag) && (num < 0 || RuntimeUtility.s_HitBuffer[i].distance < RuntimeUtility.s_HitBuffer[num].distance))
					{
						num = i;
					}
				}
				if (num >= 0)
				{
					hitInfo = RuntimeUtility.s_HitBuffer[num];
					if (num2 == RuntimeUtility.s_HitBuffer.Length)
					{
						RuntimeUtility.s_HitBuffer = new RaycastHit[RuntimeUtility.s_HitBuffer.Length * 2];
					}
					return true;
				}
			}
			hitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001492C File Offset: 0x00012B2C
		public static bool SphereCastIgnoreTag(Vector3 rayStart, float radius, Vector3 dir, out RaycastHit hitInfo, float rayLength, int layerMask, in string ignoreTag)
		{
			int num = -1;
			int num2 = 0;
			float num3 = 0f;
			int num4 = Physics.SphereCastNonAlloc(rayStart, radius, dir, RuntimeUtility.s_HitBuffer, rayLength, layerMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num4; i++)
			{
				RaycastHit raycastHit = RuntimeUtility.s_HitBuffer[i];
				if (ignoreTag.Length <= 0 || !raycastHit.collider.CompareTag(ignoreTag))
				{
					if (raycastHit.distance == 0f && raycastHit.normal == -dir)
					{
						SphereCollider scratchCollider = RuntimeUtility.GetScratchCollider();
						scratchCollider.radius = radius;
						Collider collider = raycastHit.collider;
						Vector3 vector;
						float num5;
						if (!Physics.ComputePenetration(scratchCollider, rayStart, Quaternion.identity, collider, collider.transform.position, collider.transform.rotation, out vector, out num5))
						{
							goto IL_0148;
						}
						raycastHit.point = rayStart + vector * (num5 - radius);
						raycastHit.distance = num5 - radius;
						raycastHit.normal = vector;
						RuntimeUtility.s_HitBuffer[i] = raycastHit;
						if (raycastHit.distance < -0.0001f)
						{
							num3 += raycastHit.distance;
							if (RuntimeUtility.s_PenetrationIndexBuffer.Length > num2 + 1)
							{
								RuntimeUtility.s_PenetrationIndexBuffer[num2++] = i;
							}
						}
					}
					if (num < 0 || raycastHit.distance < RuntimeUtility.s_HitBuffer[num].distance)
					{
						num = i;
					}
				}
				IL_0148:;
			}
			if (num2 > 1)
			{
				hitInfo = default(RaycastHit);
				for (int j = 0; j < num2; j++)
				{
					RaycastHit raycastHit2 = RuntimeUtility.s_HitBuffer[RuntimeUtility.s_PenetrationIndexBuffer[j]];
					float num6 = raycastHit2.distance / num3;
					hitInfo.point += raycastHit2.point * num6;
					hitInfo.distance += raycastHit2.distance * num6;
					hitInfo.normal += raycastHit2.normal * num6;
				}
				hitInfo.normal = hitInfo.normal.normalized;
				return true;
			}
			if (num >= 0)
			{
				hitInfo = RuntimeUtility.s_HitBuffer[num];
				if (num4 == RuntimeUtility.s_HitBuffer.Length)
				{
					RuntimeUtility.s_HitBuffer = new RaycastHit[RuntimeUtility.s_HitBuffer.Length * 2];
				}
				return true;
			}
			hitInfo = default(RaycastHit);
			return false;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00014B74 File Offset: 0x00012D74
		internal static SphereCollider GetScratchCollider()
		{
			if (RuntimeUtility.s_ScratchColliderGameObject == null)
			{
				RuntimeUtility.s_ScratchColliderGameObject = new GameObject("Cinemachine Scratch Collider");
				RuntimeUtility.s_ScratchColliderGameObject.hideFlags = HideFlags.HideAndDontSave;
				RuntimeUtility.s_ScratchColliderGameObject.transform.position = Vector3.zero;
				RuntimeUtility.s_ScratchColliderGameObject.SetActive(true);
				RuntimeUtility.s_ScratchCollider = RuntimeUtility.s_ScratchColliderGameObject.AddComponent<SphereCollider>();
				RuntimeUtility.s_ScratchCollider.isTrigger = true;
				Rigidbody rigidbody = RuntimeUtility.s_ScratchColliderGameObject.AddComponent<Rigidbody>();
				rigidbody.detectCollisions = false;
				rigidbody.isKinematic = true;
			}
			return RuntimeUtility.s_ScratchCollider;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00014C00 File Offset: 0x00012E00
		internal static void DestroyScratchCollider()
		{
			if (RuntimeUtility.s_ScratchColliderGameObject != null)
			{
				RuntimeUtility.s_ScratchColliderGameObject.SetActive(false);
				RuntimeUtility.DestroyObject(RuntimeUtility.s_ScratchColliderGameObject.GetComponent<Rigidbody>());
			}
			RuntimeUtility.DestroyObject(RuntimeUtility.s_ScratchCollider);
			RuntimeUtility.DestroyObject(RuntimeUtility.s_ScratchColliderGameObject);
			RuntimeUtility.s_ScratchColliderGameObject = null;
			RuntimeUtility.s_ScratchCollider = null;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00014C54 File Offset: 0x00012E54
		public static AnimationCurve NormalizeCurve(AnimationCurve curve, bool normalizeX, bool normalizeY)
		{
			if (!normalizeX && !normalizeY)
			{
				return curve;
			}
			Keyframe[] keys = curve.keys;
			if (keys.Length != 0)
			{
				float num = keys[0].time;
				float num2 = num;
				float num3 = keys[0].value;
				float num4 = num3;
				for (int i = 0; i < keys.Length; i++)
				{
					num = Mathf.Min(num, keys[i].time);
					num2 = Mathf.Max(num2, keys[i].time);
					num3 = Mathf.Min(num3, keys[i].value);
					num4 = Mathf.Max(num4, keys[i].value);
				}
				float num5 = num2 - num;
				float num6 = ((num5 < 0.0001f) ? 1f : (1f / num5));
				num5 = num4 - num3;
				float num7 = ((num5 < 1f) ? 1f : (1f / num5));
				float num8 = 0f;
				if (num5 < 1f)
				{
					if (num3 > 0f && num3 + num5 <= 1f)
					{
						num8 = num3;
					}
					else
					{
						num8 = 1f - num5;
					}
				}
				for (int j = 0; j < keys.Length; j++)
				{
					if (normalizeX)
					{
						keys[j].time = (keys[j].time - num) * num6;
					}
					if (normalizeY)
					{
						keys[j].value = (keys[j].value - num3) * num7 + num8;
					}
				}
				curve.keys = keys;
			}
			return curve;
		}

		// Token: 0x0400022C RID: 556
		private static RaycastHit[] s_HitBuffer = new RaycastHit[16];

		// Token: 0x0400022D RID: 557
		private static int[] s_PenetrationIndexBuffer = new int[16];

		// Token: 0x0400022E RID: 558
		private static SphereCollider s_ScratchCollider;

		// Token: 0x0400022F RID: 559
		private static GameObject s_ScratchColliderGameObject;
	}
}
