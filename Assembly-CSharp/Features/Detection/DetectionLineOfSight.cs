using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001271 RID: 4721
	public static class DetectionLineOfSight
	{
		// Token: 0x060092D3 RID: 37587 RVA: 0x003C4118 File Offset: 0x003C2318
		public static bool IsBlocked(Vector3 origin, Vector3 target, int obstacleMask, Transform observer, Transform player, out RaycastHit blocker, Transform interactionOccluder = null)
		{
			Vector3 vector = target - origin;
			float magnitude = vector.magnitude;
			blocker = default(RaycastHit);
			if (magnitude <= 0.001f || obstacleMask == 0)
			{
				return false;
			}
			Vector3 vector2 = vector / magnitude;
			return DetectionLineOfSight.Cast(origin, vector2, magnitude, obstacleMask, observer, player, interactionOccluder, out blocker) || DetectionLineOfSight.Cast(target, -vector2, magnitude, obstacleMask, observer, player, interactionOccluder, out blocker);
		}

		// Token: 0x060092D4 RID: 37588 RVA: 0x003C417C File Offset: 0x003C237C
		private static bool Cast(Vector3 origin, Vector3 direction, float distance, int mask, Transform observer, Transform player, Transform interactionOccluder, out RaycastHit blocker)
		{
			int num = Physics.RaycastNonAlloc(origin, direction, DetectionLineOfSight.Hits, distance, mask, QueryTriggerInteraction.Ignore);
			blocker = default(RaycastHit);
			bool flag = false;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = DetectionLineOfSight.Hits[i];
				if (!(raycastHit.collider == null) && !DetectionLineOfSight.BelongsTo(raycastHit.collider.transform, observer) && !DetectionLineOfSight.BelongsTo(raycastHit.collider.transform, player) && !DetectionLineOfSight.BelongsTo(raycastHit.collider.transform, interactionOccluder) && raycastHit.distance < num2)
				{
					blocker = raycastHit;
					num2 = raycastHit.distance;
					flag = true;
				}
			}
			return flag || num == DetectionLineOfSight.Hits.Length;
		}

		// Token: 0x060092D5 RID: 37589 RVA: 0x001043DD File Offset: 0x001025DD
		private static bool BelongsTo(Transform collider, Transform actor)
		{
			return actor != null && (collider == actor || collider.IsChildOf(actor));
		}

		// Token: 0x04009770 RID: 38768
		private static readonly RaycastHit[] Hits = new RaycastHit[32];
	}
}
