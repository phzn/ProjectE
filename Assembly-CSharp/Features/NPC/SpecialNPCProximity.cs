using System;
using System.Collections.Generic;
using Features.Detection;
using Features.GameOver;
using Features.Police;
using Pathfinding;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000F03 RID: 3843
	public static class SpecialNPCProximity
	{
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06007344 RID: 29508 RVA: 0x002F0324 File Offset: 0x002EE524
		public static bool PopulationChangesBlocked
		{
			get
			{
				return NPCSimulationPause.IsPaused || DetectionTransitionGate.IsBlocked || (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive) || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver) || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI);
			}
		}

		// Token: 0x06007345 RID: 29509 RVA: 0x002F0394 File Offset: 0x002EE594
		public static bool CanRecycleGroup(NPCSenses leader, IReadOnlyList<NPCSenses> members, Transform player, float distance)
		{
			if (player == null)
			{
				return false;
			}
			float num = Mathf.Max(0f, distance) * Mathf.Max(0f, distance);
			if (SpecialNPCProximity.MustRetainMember(leader, player, num))
			{
				return false;
			}
			if (members != null)
			{
				for (int i = 0; i < members.Count; i++)
				{
					if (SpecialNPCProximity.MustRetainMember(members[i], player, num))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007346 RID: 29510 RVA: 0x002F03F8 File Offset: 0x002EE5F8
		private static bool MustRetainMember(NPCSenses member, Transform player, float distanceSqr)
		{
			if (member == null || !member.gameObject.activeInHierarchy)
			{
				return false;
			}
			Vector3 vector = member.transform.position - player.position;
			vector.y = 0f;
			return vector.sqrMagnitude <= distanceSqr || !SpecialNPCProximity.IsHiddenFromCamera(member.transform.position, player, member.transform);
		}

		// Token: 0x06007347 RID: 29511 RVA: 0x002F0468 File Offset: 0x002EE668
		public static bool TryResolveSpawnPosition(Vector3 candidate, Vector3 playerPosition, out Vector3 resolvedPosition)
		{
			resolvedPosition = candidate;
			if (AstarPath.active == null)
			{
				return false;
			}
			NNInfo nearest = AstarPath.active.GetNearest(playerPosition, NearestNodeConstraint.Walkable);
			NNInfo nearest2 = AstarPath.active.GetNearest(candidate, NearestNodeConstraint.Walkable);
			if (nearest.node == null || nearest2.node == null || !nearest2.node.Walkable || nearest.node.Area != nearest2.node.Area)
			{
				return false;
			}
			Vector3 vector = nearest2.position - candidate;
			if (Mathf.Abs(vector.y) > 2f)
			{
				return false;
			}
			vector.y = 0f;
			if (vector.sqrMagnitude > 2.25f)
			{
				return false;
			}
			resolvedPosition = nearest2.position;
			return true;
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x002F0530 File Offset: 0x002EE730
		public static bool IsHiddenFromCamera(Vector3 position, Transform player, Transform actor = null)
		{
			Camera main = Camera.main;
			return main == null || (SpecialNPCProximity.IsSampleHidden(main, position + Vector3.up * 0.2f, player, actor) && SpecialNPCProximity.IsSampleHidden(main, position + Vector3.up * 1.05f, player, actor) && SpecialNPCProximity.IsSampleHidden(main, position + Vector3.up * 1.75f, player, actor));
		}

		// Token: 0x06007349 RID: 29513 RVA: 0x002F05AC File Offset: 0x002EE7AC
		private static bool IsSampleHidden(Camera camera, Vector3 target, Transform player, Transform actor)
		{
			Vector3 vector = camera.WorldToViewportPoint(target);
			if (vector.z <= 0f || vector.x < -0.05f || vector.x > 1.05f || vector.y < -0.05f || vector.y > 1.05f)
			{
				return true;
			}
			Vector3 vector2 = target - camera.transform.position;
			float magnitude = vector2.magnitude;
			if (magnitude <= 0.25f)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(camera.transform.position, vector2 / magnitude, SpecialNPCProximity.OcclusionHits, magnitude - 0.2f, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = SpecialNPCProximity.OcclusionHits[i].collider;
				if (!(collider == null) && !SpecialNPCProximity.IsWithin(collider.transform, player) && !SpecialNPCProximity.IsWithin(collider.transform, actor) && !SpecialNPCProximity.IsWithin(collider.transform, camera.transform) && !(collider.GetComponentInParent<NPCSenses>() != null) && !SpecialNPCProximity.IsTransparent(collider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x001043DD File Offset: 0x001025DD
		private static bool IsWithin(Transform candidate, Transform root)
		{
			return root != null && (candidate == root || candidate.IsChildOf(root));
		}

		// Token: 0x0600734B RID: 29515 RVA: 0x002F06C8 File Offset: 0x002EE8C8
		private static bool IsTransparent(Collider collider)
		{
			Transform transform = collider.transform;
			int num = 0;
			while (transform != null && num < 4)
			{
				if (SpecialNPCProximity.IsGlassName(transform.name))
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			Renderer component = collider.GetComponent<Renderer>();
			Material material = ((component != null) ? component.sharedMaterial : null);
			return material != null && (material.renderQueue >= 3000 || SpecialNPCProximity.IsGlassName(material.name));
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x002F0748 File Offset: 0x002EE948
		private static bool IsGlassName(string value)
		{
			return value.IndexOf("glass", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("window", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("vetr", StringComparison.OrdinalIgnoreCase) >= 0 || value.IndexOf("crystal", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04007689 RID: 30345
		private static readonly RaycastHit[] OcclusionHits = new RaycastHit[32];
	}
}
