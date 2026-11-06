using System;
using PoolWaterTools;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200126D RID: 4717
	public static class DetectionEncounterPlacement
	{
		// Token: 0x060092B4 RID: 37556 RVA: 0x003C2E30 File Offset: 0x003C1030
		public static bool TryFindStandingPosition(Transform player, Vector3 origin, Vector3 preferredDirection, out Vector3 position, float clearanceRadius = 0.8f)
		{
			position = origin;
			if (player == null)
			{
				return false;
			}
			Physics.SyncTransforms();
			float num = Mathf.Clamp(clearanceRadius, 0.3f, 0.9f);
			if (DetectionEncounterPlacement.TryCandidate(player, origin, origin, num, out position))
			{
				return true;
			}
			preferredDirection.y = 0f;
			if (preferredDirection.sqrMagnitude < 0.001f)
			{
				preferredDirection = player.forward;
			}
			preferredDirection.Normalize();
			for (float num2 = 0.5f; num2 <= 4f; num2 += 0.5f)
			{
				for (int i = 0; i < 16; i++)
				{
					float num3 = (float)((i + 1) / 2) * 22.5f * ((i % 2 == 0) ? (-1f) : 1f);
					Vector3 vector = origin + Quaternion.Euler(0f, num3, 0f) * preferredDirection * num2;
					if (DetectionEncounterPlacement.TryCandidate(player, origin, vector, num, out position))
					{
						return true;
					}
				}
			}
			if (num > 0.35f)
			{
				return DetectionEncounterPlacement.TryFindStandingPosition(player, origin, preferredDirection, out position, 0.35f);
			}
			position = origin;
			return false;
		}

		// Token: 0x060092B5 RID: 37557 RVA: 0x003C2F34 File Offset: 0x003C1134
		private static bool TryCandidate(Transform player, Vector3 origin, Vector3 candidate, float radius, out Vector3 position)
		{
			position = origin;
			Vector3 vector = candidate - origin;
			RaycastHit[] array = Physics.RaycastAll(origin + Vector3.up * 0.9f, vector.normalized, vector.magnitude, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < array.Length; i++)
			{
				if (!DetectionEncounterPlacement.IsPlayerCollider(array[i].collider, player))
				{
					return false;
				}
			}
			RaycastHit[] array2 = Physics.RaycastAll(candidate + Vector3.up * 0.6f, Vector3.down, 1.6f, -1, QueryTriggerInteraction.Ignore);
			Array.Sort<RaycastHit>(array2, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			int j = 0;
			while (j < array2.Length)
			{
				RaycastHit raycastHit = array2[j];
				if (!DetectionEncounterPlacement.IsPlayerCollider(raycastHit.collider, player))
				{
					if (raycastHit.normal.y < 0.65f)
					{
						return false;
					}
					Vector3 vector2 = raycastHit.point + Vector3.up * 0.06f;
					Collider collider;
					if (PoolWaterRecovery.TryFindWater(vector2, out collider, 0.35f, true))
					{
						return false;
					}
					if (!DetectionEncounterPlacement.HasStandingClearance(player, vector2, radius))
					{
						return false;
					}
					position = vector2;
					return true;
				}
				else
				{
					j++;
				}
			}
			return false;
		}

		// Token: 0x060092B6 RID: 37558 RVA: 0x003C3078 File Offset: 0x003C1278
		public static bool HasStandingClearance(Transform player, Vector3 feet, float radius = 0.35f)
		{
			radius = Mathf.Clamp(radius, 0.3f, 0.9f);
			Collider[] array = Physics.OverlapCapsule(feet + Vector3.up * radius, feet + Vector3.up * (1.8f - radius), radius, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < array.Length; i++)
			{
				if (!DetectionEncounterPlacement.IsPlayerCollider(array[i], player))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060092B7 RID: 37559 RVA: 0x001D8F84 File Offset: 0x001D7184
		private static bool IsPlayerCollider(Collider collider, Transform player)
		{
			return collider == null || (player != null && (collider.transform == player || collider.transform.IsChildOf(player)));
		}

		// Token: 0x04009747 RID: 38727
		private const float StandingHeight = 1.8f;

		// Token: 0x04009748 RID: 38728
		private const float FeetClearance = 0.06f;
	}
}
