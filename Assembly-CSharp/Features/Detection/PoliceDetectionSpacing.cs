using System;
using Features.NPC;
using Pathfinding;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012DA RID: 4826
	public static class PoliceDetectionSpacing
	{
		// Token: 0x0600958C RID: 38284 RVA: 0x003D60EC File Offset: 0x003D42EC
		public unsafe static bool TrySeparate(NPCSenses officer, Transform player, float preferredDistance = 1.5f)
		{
			if (officer == null || player == null || preferredDistance <= 0f || float.IsNaN(preferredDistance) || float.IsInfinity(preferredDistance) || NPCSimulationPause.IsPaused || AstarPath.active == null)
			{
				return false;
			}
			Transform transform = officer.transform;
			Vector3 position = transform.position;
			Vector3 position2 = player.position;
			Vector3 vector = Vector3.ProjectOnPlane(position - position2, Vector3.up);
			if (vector.magnitude >= preferredDistance - 0.02f)
			{
				return false;
			}
			FollowerEntity component = officer.GetComponent<FollowerEntity>();
			AStarNPCController component2 = officer.GetComponent<AStarNPCController>();
			if (component == null || !component.isActiveAndEnabled || !component.entityExists || component.isTraversingOffMeshLink || (component2 != null && component2.IsManualMode))
			{
				return false;
			}
			PathRequestSettings pathRequestSettings = *component.pathfindingSettings;
			NearestNodeConstraint nearestNodeConstraint = pathRequestSettings.ToNearestNodeConstraint();
			nearestNodeConstraint.maxDistance = new float?(0.35f);
			NNInfo nearest = AstarPath.active.GetNearest(position, nearestNodeConstraint);
			if (nearest.node != null && nearest.node.Walkable)
			{
				IRaycastableGraph raycastableGraph = nearest.node.Graph as IRaycastableGraph;
				if (raycastableGraph != null && PoliceDetectionSpacing.IsSmallNavCorrection(position, nearest.position))
				{
					nearestNodeConstraint.area = (int)nearest.node.Area;
					nearestNodeConstraint.graphMask = GraphMask.FromGraphIndex(nearest.node.GraphIndex);
					TraversalConstraint traversalConstraint = pathRequestSettings.ToTraversalConstraint();
					if (vector.sqrMagnitude < 0.0001f)
					{
						vector = Vector3.ProjectOnPlane(-transform.forward, Vector3.up);
					}
					if (vector.sqrMagnitude < 0.0001f)
					{
						vector = Vector3.back;
					}
					vector.Normalize();
					Vector3 vector2;
					float num;
					float num2;
					PoliceDetectionSpacing.GetCapsule(transform, component, out vector2, out num, out num2);
					PhysicsScene physicsScene = transform.gameObject.scene.GetPhysicsScene();
					if (!physicsScene.IsValid())
					{
						return false;
					}
					Physics.SyncTransforms();
					float num3 = Mathf.Min(preferredDistance, 1.3f);
					for (int i = 0; i < 2; i++)
					{
						float num4 = ((i == 0) ? preferredDistance : num3);
						if (i != 0 && Mathf.Approximately(num4, preferredDistance))
						{
							break;
						}
						for (int j = 0; j < PoliceDetectionSpacing.RetreatAngles.Length; j++)
						{
							Vector3 vector3 = Quaternion.AngleAxis(PoliceDetectionSpacing.RetreatAngles[j], Vector3.up) * vector;
							Vector3 vector4 = position2 + vector3 * num4;
							vector4.y = position.y;
							NNInfo nearest2 = AstarPath.active.GetNearest(vector4, nearestNodeConstraint);
							if (nearest2.node != null && nearest2.node.GraphIndex == nearest.node.GraphIndex && nearest2.node.Area == nearest.node.Area && PoliceDetectionSpacing.IsSmallNavCorrection(vector4, nearest2.position))
							{
								Vector3 position3 = nearest2.position;
								position3.y += position.y - nearest.position.y;
								GraphHitInfo graphHitInfo;
								if (PoliceDetectionSpacing.IsRetreatCandidate(position, position2, position3, num3) && !raycastableGraph.Linecast(nearest.position, nearest2.position, nearest.node, out graphHitInfo, ref traversalConstraint, null) && PoliceDetectionSpacing.HasClearRetreat(physicsScene, transform, player, position, position3, vector2, num, num2))
								{
									bool isStopped = component.isStopped;
									component.Teleport(position3, true);
									if (!component.updatePosition)
									{
										transform.position = component.position;
									}
									component.isStopped = isStopped;
									Physics.SyncTransforms();
									return true;
								}
							}
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600958D RID: 38285 RVA: 0x003D6478 File Offset: 0x003D4678
		public static bool IsRetreatCandidate(Vector3 origin, Vector3 player, Vector3 candidate, float minimumDistance)
		{
			if (Mathf.Abs(candidate.y - origin.y) > 0.2f)
			{
				return false;
			}
			Vector3 vector = Vector3.ProjectOnPlane(origin - player, Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(candidate - player, Vector3.up);
			Vector3 vector3 = vector2 - vector;
			return vector2.magnitude >= minimumDistance - 0.001f && vector2.magnitude > vector.magnitude + 0.02f && vector3.sqrMagnitude <= 4f && Vector3.Dot(vector3, vector) >= -0.0001f;
		}

		// Token: 0x0600958E RID: 38286 RVA: 0x003D6514 File Offset: 0x003D4714
		public static bool HasClearRetreat(PhysicsScene physicsScene, Transform actor, Transform player, Vector3 origin, Vector3 candidate, Vector3 centerOffset, float radius, float height)
		{
			if (!physicsScene.IsValid() || actor == null || player == null || radius <= 0f)
			{
				return false;
			}
			float num = Mathf.Max(0f, height * 0.5f - radius);
			Vector3 vector = centerOffset - Vector3.up * num + Vector3.up * 0.03f;
			Vector3 vector2 = centerOffset + Vector3.up * num;
			if (!PoliceDetectionSpacing.IsCapsuleClear(physicsScene, origin + vector, origin + vector2, radius, actor, player) || !PoliceDetectionSpacing.IsCapsuleClear(physicsScene, candidate + vector, candidate + vector2, radius, actor, null))
			{
				return false;
			}
			Vector3 vector3 = candidate - origin;
			float magnitude = vector3.magnitude;
			if (magnitude <= 0.001f)
			{
				return false;
			}
			int num2 = physicsScene.CapsuleCast(origin + vector, origin + vector2, radius, vector3 / magnitude, PoliceDetectionSpacing.Hits, magnitude, -1, QueryTriggerInteraction.Ignore);
			if (num2 >= PoliceDetectionSpacing.Hits.Length)
			{
				return false;
			}
			for (int i = 0; i < num2; i++)
			{
				Collider collider = PoliceDetectionSpacing.Hits[i].collider;
				if (collider != null && !PoliceDetectionSpacing.BelongsTo(collider, actor) && !PoliceDetectionSpacing.BelongsTo(collider, player))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600958F RID: 38287 RVA: 0x003D6668 File Offset: 0x003D4868
		private static bool IsCapsuleClear(PhysicsScene physicsScene, Vector3 bottom, Vector3 top, float radius, Transform actor, Transform ignoredPlayer)
		{
			int num = physicsScene.OverlapCapsule(bottom, top, radius, PoliceDetectionSpacing.Overlaps, -1, QueryTriggerInteraction.Ignore);
			if (num >= PoliceDetectionSpacing.Overlaps.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				Collider collider = PoliceDetectionSpacing.Overlaps[i];
				if (collider != null && !PoliceDetectionSpacing.BelongsTo(collider, actor) && !PoliceDetectionSpacing.BelongsTo(collider, ignoredPlayer))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009590 RID: 38288 RVA: 0x003D66C7 File Offset: 0x003D48C7
		private static bool BelongsTo(Collider collider, Transform root)
		{
			return root != null && (collider.transform == root || collider.transform.IsChildOf(root));
		}

		// Token: 0x06009591 RID: 38289 RVA: 0x003D66F0 File Offset: 0x003D48F0
		private static bool IsSmallNavCorrection(Vector3 requested, Vector3 snapped)
		{
			Vector3 vector = snapped - requested;
			return Mathf.Abs(vector.y) <= 0.2f && vector.x * vector.x + vector.z * vector.z <= 0.040000003f;
		}

		// Token: 0x06009592 RID: 38290 RVA: 0x003D6740 File Offset: 0x003D4940
		private static void GetCapsule(Transform actor, FollowerEntity follower, out Vector3 centerOffset, out float radius, out float height)
		{
			Vector3 lossyScale = actor.lossyScale;
			radius = Mathf.Max(0.1f, follower.radius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z)));
			height = Mathf.Max(radius * 2f, follower.height * Mathf.Abs(lossyScale.y));
			centerOffset = Vector3.up * (height * 0.5f);
			CapsuleCollider component = actor.GetComponent<CapsuleCollider>();
			if (component != null && component.direction == 1)
			{
				Vector3 vector = actor.TransformPoint(component.center) - actor.position;
				float num = component.radius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
				float num2 = Mathf.Max(num * 2f, component.height * Mathf.Abs(lossyScale.y));
				float num3 = Mathf.Min(0f, vector.y - num2 * 0.5f);
				float num4 = Mathf.Max(height, vector.y + num2 * 0.5f);
				radius = Mathf.Max(radius, num + new Vector2(vector.x, vector.z).magnitude);
				height = num4 - num3;
				centerOffset = Vector3.up * ((num3 + num4) * 0.5f);
			}
			radius += 0.03f;
			height = Mathf.Max(height, radius * 2f);
		}

		// Token: 0x04009AB1 RID: 39601
		private const float MinimumUsefulDistance = 1.3f;

		// Token: 0x04009AB2 RID: 39602
		private const float MaximumMovement = 2f;

		// Token: 0x04009AB3 RID: 39603
		private const float MaximumNavSnap = 0.2f;

		// Token: 0x04009AB4 RID: 39604
		private const float MaximumHeightChange = 0.2f;

		// Token: 0x04009AB5 RID: 39605
		private const float Skin = 0.03f;

		// Token: 0x04009AB6 RID: 39606
		private static readonly float[] RetreatAngles = new float[] { 0f, 15f, -15f, 30f, -30f };

		// Token: 0x04009AB7 RID: 39607
		private static readonly Collider[] Overlaps = new Collider[64];

		// Token: 0x04009AB8 RID: 39608
		private static readonly RaycastHit[] Hits = new RaycastHit[64];
	}
}
