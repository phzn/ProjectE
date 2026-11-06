using System;
using System.Collections.Generic;
using Features.Detection;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PoolWaterTools
{
	// Token: 0x02000728 RID: 1832
	public static class PoolWaterRecovery
	{
		// Token: 0x06002F6E RID: 12142 RVA: 0x0010C398 File Offset: 0x0010A598
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetCache()
		{
			PoolWaterRecovery.DeepVolumes.Clear();
			PoolWaterRecovery.AllVolumes.Clear();
			PoolWaterRecovery._nextVolumeRefresh = 0f;
			PoolWaterRecovery._cachedSceneSignature = -1;
			PoolWaterRecovery._cachedWaterPlayer = null;
			PoolWaterRecovery._cachedPlayerSwimming = null;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x0010C3CC File Offset: 0x0010A5CC
		public static float SurfaceY(Collider water)
		{
			ShallowPoolWater shallowPoolWater = ((water != null) ? water.GetComponent<ShallowPoolWater>() : null);
			if (shallowPoolWater != null)
			{
				return shallowPoolWater.SurfaceY;
			}
			if (!(water != null))
			{
				return 0f;
			}
			return water.transform.position.y;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x0010C41C File Offset: 0x0010A61C
		public static bool ContainsHorizontal(Collider water, Vector3 position, float margin = 0f)
		{
			if (water == null)
			{
				return false;
			}
			BoxCollider boxCollider = water as BoxCollider;
			if (boxCollider != null)
			{
				Vector3 vector = boxCollider.transform.InverseTransformPoint(position) - boxCollider.center;
				Vector3 lossyScale = boxCollider.transform.lossyScale;
				return Mathf.Abs(vector.x) <= boxCollider.size.x * 0.5f + margin / Mathf.Max(0.001f, Mathf.Abs(lossyScale.x)) && Mathf.Abs(vector.z) <= boxCollider.size.z * 0.5f + margin / Mathf.Max(0.001f, Mathf.Abs(lossyScale.z));
			}
			Bounds bounds = water.bounds;
			return position.x >= bounds.min.x - margin && position.x <= bounds.max.x + margin && position.z >= bounds.min.z - margin && position.z <= bounds.max.z + margin;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x0010C53C File Offset: 0x0010A73C
		public static bool TryFindWater(Vector3 feet, out Collider water, float aboveSurfaceTolerance = 0.35f, bool includeShallow = false)
		{
			foreach (Collider collider in PoolWaterRecovery.WaterVolumes(includeShallow))
			{
				if (PoolWaterRecovery.IsUsable(collider) && feet.y <= PoolWaterRecovery.SurfaceY(collider) + aboveSurfaceTolerance && feet.y >= collider.bounds.min.y - 2f && PoolWaterRecovery.ContainsHorizontal(collider, feet, 0f))
				{
					water = collider;
					return true;
				}
			}
			water = null;
			return false;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x0010C5DC File Offset: 0x0010A7DC
		public static bool IsPlayerInWater(Transform player)
		{
			if (player == null)
			{
				return false;
			}
			vThirdPersonController componentInParent = player.GetComponentInParent<vThirdPersonController>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : player);
			if (PoolWaterRecovery._cachedWaterPlayer != transform || PoolWaterRecovery._cachedPlayerSwimming == null)
			{
				PoolWaterRecovery._cachedWaterPlayer = transform;
				PoolWaterRecovery._cachedPlayerSwimming = transform.GetComponentInChildren<vSwimming>(true) ?? transform.GetComponentInParent<vSwimming>();
			}
			vSwimming cachedPlayerSwimming = PoolWaterRecovery._cachedPlayerSwimming;
			Collider collider;
			return (cachedPlayerSwimming != null && (cachedPlayerSwimming.isSwimming || cachedPlayerSwimming.isUnderWater)) || PoolWaterRecovery.TryFindWater(transform.position, out collider, 0.045f, true);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x0010C678 File Offset: 0x0010A878
		public static bool TryFindClimbExit(Collider water, Vector3 from, Transform actor, out PoolClimbPoint climb, out Vector3 deck)
		{
			climb = null;
			deck = from;
			if (water == null)
			{
				return false;
			}
			List<Collider> list = PoolWaterRecovery.WaterVolumes(true);
			float num = float.PositiveInfinity;
			foreach (PoolClimbPoint poolClimbPoint in Object.FindObjectsOfType<PoolClimbPoint>())
			{
				Vector3 vector;
				if (!(poolClimbPoint.gameObject.scene != water.gameObject.scene) && PoolWaterRecovery.ContainsHorizontal(water, poolClimbPoint.SwimApproachPosition, 0.5f) && PoolWaterRecovery.TryDeckCandidate(water, poolClimbPoint.StandPosition, actor, 0.4f, list, out vector, PoolWaterRecovery.SampleDeckHeight(water, poolClimbPoint.StandPosition, actor, list)))
				{
					float sqrMagnitude = (poolClimbPoint.SwimApproachPosition - from).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						climb = poolClimbPoint;
						deck = vector;
					}
				}
			}
			return climb != null;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x0010C758 File Offset: 0x0010A958
		public static bool TryFindDryDeck(Collider water, Vector3 origin, Transform actor, out Vector3 deck, float clearance = 0.8f)
		{
			deck = origin;
			if (water == null)
			{
				return false;
			}
			Physics.SyncTransforms();
			List<Collider> list = PoolWaterRecovery.WaterVolumes(true);
			float positiveInfinity = float.PositiveInfinity;
			foreach (PoolClimbPoint poolClimbPoint in Object.FindObjectsOfType<PoolClimbPoint>())
			{
				if (!(poolClimbPoint.gameObject.scene != water.gameObject.scene) && PoolWaterRecovery.ContainsHorizontal(water, poolClimbPoint.SwimApproachPosition, 0.5f))
				{
					PoolWaterRecovery.ConsiderDeck(water, poolClimbPoint.StandPosition, origin, actor, clearance, list, ref positiveInfinity, ref deck, PoolWaterRecovery.SampleDeckHeight(water, poolClimbPoint.StandPosition, actor, list));
				}
			}
			if (!float.IsPositiveInfinity(positiveInfinity))
			{
				return true;
			}
			Bounds bounds = water.bounds;
			float num = PoolWaterRecovery.SurfaceY(water);
			List<float> list2 = new List<float>();
			foreach (PoolClimbPoint poolClimbPoint2 in Object.FindObjectsOfType<PoolClimbPoint>())
			{
				if (poolClimbPoint2.gameObject.scene == water.gameObject.scene && PoolWaterRecovery.ContainsHorizontal(water, poolClimbPoint2.SwimApproachPosition, 0.5f))
				{
					list2.Add(PoolWaterRecovery.SampleDeckHeight(water, poolClimbPoint2.StandPosition, actor, list));
				}
			}
			if (list2.Count > 0)
			{
				list2.Sort();
				num = list2[list2.Count / 2];
			}
			for (float num2 = clearance + 0.15f; num2 <= clearance + 2.2f; num2 += 0.5f)
			{
				for (int j = 0; j < 4; j++)
				{
					int num3 = Mathf.CeilToInt(((j < 2) ? bounds.size.z : bounds.size.x) / 1.5f);
					for (int k = 0; k <= num3; k++)
					{
						float num4 = ((num3 == 0) ? 0.5f : ((float)k / (float)num3));
						Vector3 vector = new Vector3((j < 2) ? ((j == 0) ? (bounds.min.x - num2) : (bounds.max.x + num2)) : Mathf.Lerp(bounds.min.x, bounds.max.x, num4), num, (j >= 2) ? ((j == 2) ? (bounds.min.z - num2) : (bounds.max.z + num2)) : Mathf.Lerp(bounds.min.z, bounds.max.z, num4));
						PoolWaterRecovery.ConsiderDeck(water, vector, origin, actor, clearance, list, ref positiveInfinity, ref deck, num);
					}
				}
			}
			return !float.IsPositiveInfinity(positiveInfinity);
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x0010C9F8 File Offset: 0x0010ABF8
		private static void ConsiderDeck(Collider water, Vector3 candidate, Vector3 origin, Transform actor, float clearance, List<Collider> volumes, ref float best, ref Vector3 deck, float deckY)
		{
			Vector3 vector;
			if (!PoolWaterRecovery.TryDeckCandidate(water, candidate, actor, clearance, volumes, out vector, deckY))
			{
				return;
			}
			float sqrMagnitude = (vector - origin).sqrMagnitude;
			if (sqrMagnitude >= best)
			{
				return;
			}
			best = sqrMagnitude;
			deck = vector;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x0010CA3C File Offset: 0x0010AC3C
		private static bool TryDeckCandidate(Collider water, Vector3 candidate, Transform actor, float clearance, List<Collider> volumes, out Vector3 ground, float expectedDeckY)
		{
			ground = candidate;
			float num = PoolWaterRecovery.SurfaceY(water);
			if (Mathf.Abs(expectedDeckY - num) > 1.5f)
			{
				return false;
			}
			candidate.y = num + 1.5f;
			RaycastHit[] array = Physics.RaycastAll(candidate, Vector3.down, 2f, -1, QueryTriggerInteraction.Ignore);
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit raycastHit = array2[i];
				if (!(actor != null) || (!(raycastHit.transform == actor) && !raycastHit.transform.IsChildOf(actor)))
				{
					if (raycastHit.normal.y < 0.65f)
					{
						return false;
					}
					if (Mathf.Abs(raycastHit.point.y - expectedDeckY) > 0.15f)
					{
						return false;
					}
					ground = raycastHit.point + Vector3.up * 0.06f;
					foreach (Collider collider in volumes)
					{
						if (PoolWaterRecovery.IsUsable(collider) && PoolWaterRecovery.ContainsHorizontal(collider, ground, clearance))
						{
							return false;
						}
					}
					return DetectionEncounterPlacement.HasStandingClearance(actor, ground, clearance);
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x0010CBBC File Offset: 0x0010ADBC
		private static float SampleDeckHeight(Collider water, Vector3 anchor, Transform actor, List<Collider> volumes)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < 9; i++)
			{
				float num2 = (float)i * 3.1415927f * 0.25f;
				Vector3 vector = anchor + ((i == 8) ? Vector3.zero : (new Vector3(Mathf.Cos(num2), 0f, Mathf.Sin(num2)) * 1.6f));
				bool flag = false;
				foreach (Collider collider in volumes)
				{
					flag |= PoolWaterRecovery.IsUsable(collider) && PoolWaterRecovery.ContainsHorizontal(collider, vector, 0.1f);
				}
				if (!flag)
				{
					vector.y = PoolWaterRecovery.SurfaceY(water) + 1.5f;
					RaycastHit[] array = Physics.RaycastAll(vector, Vector3.down, 2f, -1, QueryTriggerInteraction.Ignore);
					Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
					RaycastHit[] array2 = array;
					int j = 0;
					while (j < array2.Length)
					{
						RaycastHit raycastHit = array2[j];
						if (!(actor != null) || (!(raycastHit.transform == actor) && !raycastHit.transform.IsChildOf(actor)))
						{
							if (raycastHit.normal.y >= 0.65f)
							{
								num = Mathf.Min(num, raycastHit.point.y);
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
				}
			}
			if (!float.IsPositiveInfinity(num))
			{
				return num;
			}
			return anchor.y;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x0010CD50 File Offset: 0x0010AF50
		private static List<Collider> WaterVolumes(bool includeShallow)
		{
			int sceneCount = SceneManager.sceneCount;
			int num = sceneCount;
			for (int i = 0; i < sceneCount; i++)
			{
				num = (num * 397) ^ SceneManager.GetSceneAt(i).handle;
			}
			if (Application.isPlaying && PoolWaterRecovery._cachedSceneSignature == num && Time.unscaledTime < PoolWaterRecovery._nextVolumeRefresh)
			{
				if (!includeShallow)
				{
					return PoolWaterRecovery.DeepVolumes;
				}
				return PoolWaterRecovery.AllVolumes;
			}
			else
			{
				PoolWaterRecovery._cachedSceneSignature = num;
				PoolWaterRecovery._nextVolumeRefresh = Time.unscaledTime + 0.5f;
				PoolWaterRecovery.DeepVolumes.Clear();
				PoolWaterRecovery.AllVolumes.Clear();
				GameObject[] array = GameObject.FindGameObjectsWithTag("Water");
				for (int j = 0; j < array.Length; j++)
				{
					foreach (Collider collider in array[j].GetComponents<Collider>())
					{
						if (collider.enabled && collider.isTrigger)
						{
							PoolWaterRecovery.DeepVolumes.Add(collider);
						}
					}
				}
				PoolWaterRecovery.AllVolumes.AddRange(PoolWaterRecovery.DeepVolumes);
				if (Application.isPlaying)
				{
					ShallowPoolWater.AppendRecoveryVolumes(PoolWaterRecovery.AllVolumes);
				}
				else
				{
					foreach (ShallowPoolWater shallowPoolWater in Object.FindObjectsOfType<ShallowPoolWater>())
					{
						Collider collider2 = ((shallowPoolWater.WaterVolume != null) ? shallowPoolWater.WaterVolume : shallowPoolWater.GetComponent<Collider>());
						if (PoolWaterRecovery.IsUsable(collider2) && !PoolWaterRecovery.AllVolumes.Contains(collider2))
						{
							PoolWaterRecovery.AllVolumes.Add(collider2);
						}
					}
				}
				if (!includeShallow)
				{
					return PoolWaterRecovery.DeepVolumes;
				}
				return PoolWaterRecovery.AllVolumes;
			}
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x0010CED4 File Offset: 0x0010B0D4
		private static bool IsUsable(Collider collider)
		{
			return collider != null && collider.enabled && collider.gameObject.activeInHierarchy;
		}

		// Token: 0x04002FEB RID: 12267
		private static readonly List<Collider> DeepVolumes = new List<Collider>();

		// Token: 0x04002FEC RID: 12268
		private static readonly List<Collider> AllVolumes = new List<Collider>();

		// Token: 0x04002FED RID: 12269
		private static float _nextVolumeRefresh;

		// Token: 0x04002FEE RID: 12270
		private static int _cachedSceneSignature = -1;

		// Token: 0x04002FEF RID: 12271
		private static Transform _cachedWaterPlayer;

		// Token: 0x04002FF0 RID: 12272
		private static vSwimming _cachedPlayerSwimming;
	}
}
