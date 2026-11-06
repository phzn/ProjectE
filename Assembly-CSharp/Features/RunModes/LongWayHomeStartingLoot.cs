using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D19 RID: 3353
	public sealed class LongWayHomeStartingLoot : MonoBehaviour
	{
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060061DE RID: 25054 RVA: 0x0026F4C1 File Offset: 0x0026D6C1
		// (set) Token: 0x060061DF RID: 25055 RVA: 0x0026F4C9 File Offset: 0x0026D6C9
		public bool IsReady { get; private set; }

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060061E0 RID: 25056 RVA: 0x0026F4D2 File Offset: 0x0026D6D2
		// (set) Token: 0x060061E1 RID: 25057 RVA: 0x0026F4DA File Offset: 0x0026D6DA
		public string Error { get; private set; }

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060061E2 RID: 25058 RVA: 0x0026F4E3 File Offset: 0x0026D6E3
		public IReadOnlyList<GameObject> SpawnedPickups
		{
			get
			{
				return this._spawned;
			}
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x0026F4EB File Offset: 0x0026D6EB
		public IEnumerator Prepare(Transform player, Vector3 spawnPosition, Quaternion spawnRotation)
		{
			this.Clear();
			if (!RunModeService.IsLongWayHome)
			{
				this.IsReady = true;
				yield break;
			}
			this._policyGeneration = LongWayHomeLootPolicy.BeginRun();
			List<InventoryItemSO> list = new List<InventoryItemSO>();
			foreach (InventoryItemSO inventoryItemSO in Resources.FindObjectsOfTypeAll<InventoryItemSO>())
			{
				if (LongWayHomeLootPolicy.IsBodyPaintItem(inventoryItemSO))
				{
					list.Add(inventoryItemSO);
				}
			}
			LongWayHomeLootPolicy.CacheBodyPaintLoot(list);
			if (!RunModeService.LongWayHomeStartingLootEnabled)
			{
				this.IsReady = true;
				yield break;
			}
			if (player == null)
			{
				this.Error = "The player is unavailable for starting loot.";
				yield break;
			}
			InventoryDropController componentInChildren = player.GetComponentInChildren<InventoryDropController>(true);
			if (componentInChildren == null)
			{
				this.Error = "The item drop system is unavailable.";
				yield break;
			}
			list.AddRange(Resources.LoadAll<WearableItemSO>("Wardrobe"));
			Random random = new Random(Environment.TickCount ^ base.GetInstanceID());
			List<InventoryItemSO> list2 = LongWayHomeLootPolicy.SelectStartingItems(list, random.Next(1, 4), random);
			if (list2.Count == 0)
			{
				this.Error = "No eligible starting garment is available.";
				yield break;
			}
			Physics.SyncTransforms();
			GameObject gameObject = Resources.Load<GameObject>("LongWayHomeIntroRig");
			LongWayHomeIntroRig longWayHomeIntroRig = ((gameObject != null) ? gameObject.GetComponent<LongWayHomeIntroRig>() : null);
			foreach (Vector3 vector in LongWayHomeStartingLoot.PlacementOffsets)
			{
				if (!(longWayHomeIntroRig != null) || (vector - longWayHomeIntroRig.PhoneGroundOffset).sqrMagnitude >= 0.20249999f)
				{
					Vector3 vector2 = spawnPosition + spawnRotation * vector;
					Vector3 vector3;
					if (LongWayHomeStartingLoot.TryPlaceBesidePlayer(spawnPosition, vector2, player, out vector3))
					{
						bool flag = false;
						using (List<Vector3>.Enumerator enumerator = this._positions.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if ((enumerator.Current - vector3).sqrMagnitude < 0.20249999f)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							this._positions.Add(vector3);
							if (this._positions.Count >= list2.Count)
							{
								break;
							}
						}
					}
				}
			}
			if (this._positions.Count == 0)
			{
				this.Error = "There is no clear ground beside the wake-up pose for starting clothes.";
				yield break;
			}
			if (list2.Count > this._positions.Count)
			{
				list2.RemoveRange(this._positions.Count, list2.Count - this._positions.Count);
			}
			LongWayHomeLootPolicy.RegisterStartingItems(list2);
			for (int j = 0; j < list2.Count; j++)
			{
				InventoryItemSO inventoryItemSO2 = list2[j];
				GameObject gameObject2 = componentInChildren.SpawnDropAt(inventoryItemSO2, this._positions[j] + Vector3.up * 0.15f, inventoryItemSO2.rarity, inventoryItemSO2.PickVariantForTier(inventoryItemSO2.rarity), 1f, true, true);
				if (gameObject2 == null)
				{
					this.Clear();
					this.Error = "A starting item could not be placed.";
					yield break;
				}
				this._spawned.Add(gameObject2);
			}
			yield return null;
			this.IsReady = true;
			Debug.Log(string.Format("[LongWayHome] Placed {0} starting items beside Ciel.", this._spawned.Count));
			yield break;
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x0026F510 File Offset: 0x0026D710
		private static bool TryPlaceBesidePlayer(Vector3 origin, Vector3 desired, Transform player, out Vector3 ground)
		{
			ground = default(Vector3);
			RaycastHit raycastHit;
			if (!Physics.Raycast(desired + Vector3.up * 0.8f, Vector3.down, out raycastHit, 1.6f, -5, QueryTriggerInteraction.Ignore) || raycastHit.normal.y < 0.85f || Mathf.Abs(raycastHit.point.y - origin.y) > 0.35f || raycastHit.rigidbody != null)
			{
				return false;
			}
			ground = raycastHit.point;
			Collider[] array = Physics.OverlapSphere(ground + Vector3.up * 0.27f, 0.18f, -5, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].transform.IsChildOf(player))
				{
					return false;
				}
			}
			Vector3 vector = origin + Vector3.up * 0.45f;
			Vector3 vector2 = ground + Vector3.up * 0.45f;
			foreach (RaycastHit raycastHit2 in Physics.RaycastAll(vector, (vector2 - vector).normalized, Vector3.Distance(vector, vector2), -5, QueryTriggerInteraction.Ignore))
			{
				if (!raycastHit2.transform.IsChildOf(player))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x0026F670 File Offset: 0x0026D870
		public void Clear()
		{
			foreach (GameObject gameObject in this._spawned)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._spawned.Clear();
			this._positions.Clear();
			if (this._policyGeneration != -1)
			{
				LongWayHomeLootPolicy.EndRun(this._policyGeneration);
			}
			this._policyGeneration = -1;
			this.IsReady = false;
			this.Error = null;
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x0026F70C File Offset: 0x0026D90C
		private void OnDestroy()
		{
			this.Clear();
		}

		// Token: 0x04006415 RID: 25621
		private readonly List<GameObject> _spawned = new List<GameObject>(3);

		// Token: 0x04006416 RID: 25622
		private readonly List<Vector3> _positions = new List<Vector3>(3);

		// Token: 0x04006417 RID: 25623
		private int _policyGeneration = -1;

		// Token: 0x04006418 RID: 25624
		private const float MinimumPhoneClearance = 0.45f;

		// Token: 0x04006419 RID: 25625
		private static readonly Vector3[] PlacementOffsets = new Vector3[]
		{
			new Vector3(-0.9f, 0f, -0.55f),
			new Vector3(-1.15f, 0f, 0.1f),
			new Vector3(-0.85f, 0f, 0.7f),
			new Vector3(-1.5f, 0f, -0.65f),
			new Vector3(-1.5f, 0f, 0.65f),
			new Vector3(-0.6f, 0f, -1.35f),
			new Vector3(0.15f, 0f, -1.35f),
			new Vector3(-0.6f, 0f, 1.35f)
		};
	}
}
