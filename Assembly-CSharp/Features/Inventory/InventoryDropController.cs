using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory.Placement;
using Features.RunModes;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Inventory
{
	// Token: 0x020010F5 RID: 4341
	public class InventoryDropController : MonoBehaviour
	{
		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x06008358 RID: 33624 RVA: 0x00358F44 File Offset: 0x00357144
		// (set) Token: 0x06008359 RID: 33625 RVA: 0x00358F4B File Offset: 0x0035714B
		public static InventoryDropController Instance { get; private set; }

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x0600835A RID: 33626 RVA: 0x00358F53 File Offset: 0x00357153
		public bool CanDropFromInventory
		{
			get
			{
				return WallCoverActivityLock.ActiveController == null;
			}
		}

		// Token: 0x0600835B RID: 33627 RVA: 0x00358F60 File Offset: 0x00357160
		private void Awake()
		{
			if (InventoryDropController.Instance == null)
			{
				InventoryDropController.Instance = this;
				if (this.inventory == null)
				{
					this.inventory = base.GetComponent<PlayerInventoryManager>();
				}
				if (this.player == null)
				{
					this.player = base.transform;
				}
				return;
			}
			Object.Destroy(this);
		}

		// Token: 0x0600835C RID: 33628 RVA: 0x00358FC0 File Offset: 0x003571C0
		public void DropItemSlot(int slotIndex)
		{
			if (!this.CanDropFromInventory || this.inventory == null)
			{
				return;
			}
			InventoryItemSO itemAt = this.inventory.GetItemAt(slotIndex);
			if (itemAt == null)
			{
				return;
			}
			ItemRarity itemRarityAt = this.inventory.GetItemRarityAt(slotIndex);
			int itemVariantAt = this.inventory.GetItemVariantAt(slotIndex);
			float itemDurability = this.inventory.GetItemDurability01(slotIndex);
			if (this.SpawnDrop(itemAt, itemRarityAt, itemVariantAt, itemDurability))
			{
				if (itemAt is IInventoryWholeStackWorldProvider)
				{
					this.inventory.DropEntireItemStack(slotIndex);
					return;
				}
				this.inventory.DropItem(slotIndex);
			}
		}

		// Token: 0x0600835D RID: 33629 RVA: 0x00359050 File Offset: 0x00357250
		public void DropClothSlot(int slotIndex)
		{
			if (!this.CanDropFromInventory || this.inventory == null)
			{
				return;
			}
			InventoryItemSO clothAt = this.inventory.GetClothAt(slotIndex);
			if (clothAt == null)
			{
				return;
			}
			ItemRarity clothRarityAt = this.inventory.GetClothRarityAt(slotIndex);
			int clothVariantAt = this.inventory.GetClothVariantAt(slotIndex);
			float num = this.ClothDurability01(slotIndex);
			if (this.SpawnDrop(clothAt, clothRarityAt, clothVariantAt, num))
			{
				this.inventory.RemoveCloth(slotIndex);
			}
		}

		// Token: 0x0600835E RID: 33630 RVA: 0x003590C8 File Offset: 0x003572C8
		private float ClothDurability01(int slotIndex)
		{
			float clothDurabilityBase = this.inventory.GetClothDurabilityBase(slotIndex);
			if (clothDurabilityBase <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.inventory.GetClothDurability(slotIndex) / clothDurabilityBase);
		}

		// Token: 0x0600835F RID: 33631 RVA: 0x00359104 File Offset: 0x00357304
		public void DropItemSlotTowards(int slotIndex, Vector2 screenPos)
		{
			if (!this.CanDropFromInventory || this.inventory == null)
			{
				return;
			}
			InventoryItemSO itemAt = this.inventory.GetItemAt(slotIndex);
			if (itemAt == null)
			{
				return;
			}
			ItemRarity itemRarityAt = this.inventory.GetItemRarityAt(slotIndex);
			int itemVariantAt = this.inventory.GetItemVariantAt(slotIndex);
			float itemDurability = this.inventory.GetItemDurability01(slotIndex);
			Vector3 vector = this.ResolveCursorDropSpot(screenPos);
			if (this.SpawnDropAt(itemAt, vector + Vector3.up * this.spawnHeight, itemRarityAt, itemVariantAt, itemDurability, false, false) != null)
			{
				if (itemAt is IInventoryWholeStackWorldProvider)
				{
					this.inventory.DropEntireItemStack(slotIndex);
					return;
				}
				this.inventory.DropItem(slotIndex);
			}
		}

		// Token: 0x06008360 RID: 33632 RVA: 0x003591BC File Offset: 0x003573BC
		public void DropClothSlotTowards(int slotIndex, Vector2 screenPos)
		{
			if (!this.CanDropFromInventory || this.inventory == null)
			{
				return;
			}
			InventoryItemSO clothAt = this.inventory.GetClothAt(slotIndex);
			if (clothAt == null)
			{
				return;
			}
			ItemRarity clothRarityAt = this.inventory.GetClothRarityAt(slotIndex);
			int clothVariantAt = this.inventory.GetClothVariantAt(slotIndex);
			float num = this.ClothDurability01(slotIndex);
			Vector3 vector = this.ResolveCursorDropSpot(screenPos);
			if (this.SpawnDropAt(clothAt, vector + Vector3.up * this.spawnHeight, clothRarityAt, clothVariantAt, num, false, false) != null)
			{
				this.inventory.RemoveCloth(slotIndex);
			}
		}

		// Token: 0x06008361 RID: 33633 RVA: 0x0035925C File Offset: 0x0035745C
		public Vector3 ResolveCursorDropSpot(Vector2 screenPos)
		{
			Vector3 vector = this.player.position + this.player.forward * this.dropDistance;
			Camera main = Camera.main;
			if (main != null)
			{
				Ray ray = main.ScreenPointToRay(screenPos);
				RaycastHit raycastHit;
				Vector3 vector2;
				float num;
				if (Physics.Raycast(ray, out raycastHit, 60f, this.groundMask, QueryTriggerInteraction.Ignore))
				{
					vector2 = raycastHit.point;
				}
				else if (new Plane(Vector3.up, this.player.position).Raycast(ray, out num))
				{
					vector2 = ray.GetPoint(num);
				}
				else
				{
					vector2 = vector;
				}
				Vector3 vector3 = vector2 - this.player.position;
				vector3.y = 0f;
				if (vector3.sqrMagnitude > 0.0001f)
				{
					float num2 = Mathf.Clamp(vector3.magnitude, this.dropDistance, this.maxCursorDropDistance);
					vector = this.player.position + vector3.normalized * num2;
					RaycastHit raycastHit2;
					if (Physics.Linecast(this.player.position + Vector3.up * 0.9f, vector + Vector3.up * 0.9f, out raycastHit2, this.groundMask, QueryTriggerInteraction.Ignore))
					{
						float num3 = Mathf.Max(0.3f, raycastHit2.distance - 0.3f);
						vector = this.player.position + vector3.normalized * Mathf.Min(num2, num3);
					}
				}
			}
			return this.ConstrainHomeDropSpot(this.ApplyScatter(vector));
		}

		// Token: 0x06008362 RID: 33634 RVA: 0x00359404 File Offset: 0x00357604
		private Vector3 ApplyScatter(Vector3 spot)
		{
			if (this.dropScatterRadius <= 0f)
			{
				return spot;
			}
			Vector2 vector = Random.insideUnitCircle * this.dropScatterRadius;
			return spot + new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06008363 RID: 33635 RVA: 0x0035944D File Offset: 0x0035764D
		public bool SpawnDrop(InventoryItemSO item)
		{
			return this.SpawnDrop(item, (item != null) ? item.rarity : ItemRarity.Common, -1, 1f);
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x00359470 File Offset: 0x00357670
		public bool SpawnDrop(InventoryItemSO item, ItemRarity instanceRarity, int variantIndex, float durability01 = 1f)
		{
			if (!this.CanDropFromInventory || item == null || this.player == null)
			{
				return false;
			}
			Vector3 vector = this.ConstrainHomeDropSpot(this.ApplyScatter(this.player.position + this.player.forward * this.dropDistance));
			return this.SpawnDropAt(item, vector + Vector3.up * this.spawnHeight, instanceRarity, variantIndex, durability01, false, false) != null;
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x003594FC File Offset: 0x003576FC
		public bool CanStageSwapDrop(PlayerInventoryManager owner, InventoryItemSO item, int quantity)
		{
			if (owner == null || this.inventory != owner || this.player == null || !this.CanDropFromInventory || item == null || quantity < 1 || !LongWayHomeLootPolicy.CanSpawnDrop(item, false, false))
			{
				return false;
			}
			if (item is IInventoryWorldInstanceProvider && !(item is IInventoryWholeStackWorldProvider))
			{
				return false;
			}
			IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = item as IInventoryWholeStackWorldProvider;
			return inventoryWholeStackWorldProvider == null || inventoryWholeStackWorldProvider.InventoryStackQuantity == quantity;
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x00359578 File Offset: 0x00357778
		public bool TryStageSwapDrops(PlayerInventoryManager owner, InventoryItemSO item, int quantity, ItemRarity rarity, int variantIndex, float durability01, out List<GameObject> staged)
		{
			staged = new List<GameObject>();
			if (!this.CanStageSwapDrop(owner, item, quantity))
			{
				return false;
			}
			int num = ((item is IInventoryWholeStackWorldProvider) ? 1 : quantity);
			bool flag;
			try
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = this.SpawnSwapDrop(item, rarity, variantIndex, durability01);
					if (gameObject == null)
					{
						InventoryDropController.DiscardSwapDrops(staged);
						return false;
					}
					gameObject.SetActive(false);
					staged.Add(gameObject);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				InventoryDropController.DiscardSwapDrops(staged);
				Debug.LogWarning("[InventoryDrop] Swap drop could not be prepared: " + ex.Message, this);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x00359620 File Offset: 0x00357820
		protected virtual GameObject SpawnSwapDrop(InventoryItemSO item, ItemRarity rarity, int variantIndex, float durability01)
		{
			Vector3 vector = this.ConstrainHomeDropSpot(this.ApplyScatter(this.player.position + this.player.forward * this.dropDistance));
			return this.SpawnDropAt(item, vector + Vector3.up * this.spawnHeight, rarity, variantIndex, durability01, false, false);
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x00359684 File Offset: 0x00357884
		public static void DiscardSwapDrops(List<GameObject> staged)
		{
			if (staged == null)
			{
				return;
			}
			foreach (GameObject gameObject in staged)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject);
					}
					else
					{
						Object.DestroyImmediate(gameObject);
					}
				}
			}
			staged.Clear();
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x003596FC File Offset: 0x003578FC
		public static void ActivateSwapDrops(List<GameObject> staged)
		{
			foreach (GameObject gameObject in staged)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600836A RID: 33642 RVA: 0x00359754 File Offset: 0x00357954
		private Vector3 ConstrainHomeDropSpot(Vector3 proposed)
		{
			if (!string.Equals(SceneManager.GetActiveScene().name, "CielHome", StringComparison.Ordinal) || this.player == null)
			{
				return proposed;
			}
			this.ResolveHomeFloorRoot();
			if (this._homeFloorRoot == null)
			{
				if (!this._warnedMissingHomeFloor)
				{
					this._warnedMissingHomeFloor = true;
					Debug.LogWarning("[InventoryDrop] CielHome floor boundary missing; falling back to the player's position.", this);
				}
				return this.player.position;
			}
			proposed.y = this.player.position.y;
			if (this.IsSafeHomeDropSpot(proposed))
			{
				return this.FindNearestClearHomeDropSpot(proposed);
			}
			Vector3 position = this.player.position;
			position.y = proposed.y;
			if (!this.IsSafeHomeDropSpot(position))
			{
				return position;
			}
			Vector3 vector = position;
			float num = 0f;
			float num2 = 1f;
			for (int i = 1; i <= 24; i++)
			{
				float num3 = (float)i / 24f;
				Vector3 vector2 = Vector3.Lerp(position, proposed, num3);
				if (!this.IsSafeHomeDropSpot(vector2))
				{
					num2 = num3;
					break;
				}
				vector = vector2;
				num = num3;
			}
			for (int j = 0; j < 6; j++)
			{
				float num4 = (num + num2) * 0.5f;
				Vector3 vector3 = Vector3.Lerp(position, proposed, num4);
				if (this.IsSafeHomeDropSpot(vector3))
				{
					vector = vector3;
					num = num4;
				}
				else
				{
					num2 = num4;
				}
			}
			return this.FindNearestClearHomeDropSpot(vector);
		}

		// Token: 0x0600836B RID: 33643 RVA: 0x003598A0 File Offset: 0x00357AA0
		private Vector3 FindNearestClearHomeDropSpot(Vector3 desired)
		{
			if (this.IsHomeDropSpotClear(desired))
			{
				return desired;
			}
			Vector3 vector = this.player.position - desired;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = this.player.forward;
			}
			vector.Normalize();
			float num = Mathf.Atan2(vector.z, vector.x);
			for (int i = 1; i <= 6; i++)
			{
				float num2 = (float)i * 0.24f;
				for (int j = 0; j < 16; j++)
				{
					float num3 = num + (float)j * 3.1415927f * 2f / 16f;
					Vector3 vector2 = desired + new Vector3(Mathf.Cos(num3), 0f, Mathf.Sin(num3)) * num2;
					if (this.IsSafeHomeDropSpot(vector2) && this.IsHomeDropSpotClear(vector2))
					{
						return vector2;
					}
				}
			}
			Vector3 position = this.player.position;
			position.y = desired.y;
			return position;
		}

		// Token: 0x0600836C RID: 33644 RVA: 0x003599A4 File Offset: 0x00357BA4
		private bool IsHomeDropSpotClear(Vector3 point)
		{
			RaycastHit raycastHit;
			if (!this.TryGetHomeFloorHit(point + Vector3.up * 3f, 8f, out raycastHit))
			{
				return false;
			}
			float num = Mathf.Max(0.1f, this.homeObstacleClearance);
			Vector3 vector = new Vector3(point.x, raycastHit.point.y + num + 0.05f, point.z);
			Vector3 vector2 = new Vector3(point.x, Mathf.Max(vector.y, raycastHit.point.y + 0.58f), point.z);
			foreach (Collider collider in Physics.OverlapCapsule(vector, vector2, num, -1, QueryTriggerInteraction.Ignore))
			{
				if (!(collider == null))
				{
					Transform transform = collider.transform;
					if (!(transform == this._homeFloorRoot) && !transform.IsChildOf(this._homeFloorRoot) && (!(this.player != null) || (!(transform == this.player) && !transform.IsChildOf(this.player) && !this.player.IsChildOf(transform))))
					{
						Ray ray = new Ray(new Vector3(point.x, raycastHit.point.y + 1f, point.z), Vector3.down);
						RaycastHit raycastHit2;
						if (!collider.Raycast(ray, out raycastHit2, 1.25f) || Vector3.Dot(raycastHit2.normal, Vector3.up) <= 0.65f || raycastHit2.point.y > raycastHit.point.y + 0.15f)
						{
							return false;
						}
					}
				}
			}
			foreach (Collider collider2 in Physics.OverlapSphere(new Vector3(point.x, raycastHit.point.y + 0.08f, point.z), num, -1, QueryTriggerInteraction.Ignore))
			{
				if (!(collider2 == null) && !(collider2.GetComponentInParent<InventoryPickup>() == null))
				{
					Transform transform2 = collider2.transform;
					if (!(this.player != null) || (!(transform2 == this.player) && !transform2.IsChildOf(this.player) && !this.player.IsChildOf(transform2)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x00359C08 File Offset: 0x00357E08
		private bool IsSafeHomeDropSpot(Vector3 point)
		{
			if (!this.HasHomeFloorBelow(point))
			{
				return false;
			}
			float num = Mathf.Max(0.05f, this.homeBoundaryInset);
			return this.HasHomeFloorBelow(point + Vector3.right * num) && this.HasHomeFloorBelow(point - Vector3.right * num) && this.HasHomeFloorBelow(point + Vector3.forward * num) && this.HasHomeFloorBelow(point - Vector3.forward * num);
		}

		// Token: 0x0600836E RID: 33646 RVA: 0x00359C98 File Offset: 0x00357E98
		private bool HasHomeFloorBelow(Vector3 point)
		{
			RaycastHit raycastHit;
			return this.TryGetHomeFloorHit(point + Vector3.up * 3f, 8f, out raycastHit);
		}

		// Token: 0x0600836F RID: 33647 RVA: 0x00359CC8 File Offset: 0x00357EC8
		private bool TryGetHomeFloorHit(Vector3 origin, float maxDistance, out RaycastHit floorHit)
		{
			floorHit = default(RaycastHit);
			this.ResolveHomeFloorRoot();
			if (this._homeFloorRoot == null)
			{
				return false;
			}
			RaycastHit[] array = Physics.RaycastAll(origin, Vector3.down, maxDistance, -1, QueryTriggerInteraction.Ignore);
			float num = float.PositiveInfinity;
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = ((array[i].collider != null) ? array[i].collider.transform : null);
				if (transform != null && (transform == this._homeFloorRoot || transform.IsChildOf(this._homeFloorRoot)) && array[i].distance < num)
				{
					floorHit = array[i];
					num = array[i].distance;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06008370 RID: 33648 RVA: 0x00359D9C File Offset: 0x00357F9C
		private bool TryResolveDropGround(Vector3 origin, out RaycastHit hit)
		{
			if (string.Equals(SceneManager.GetActiveScene().name, "CielHome", StringComparison.Ordinal))
			{
				this.ResolveHomeFloorRoot();
				if (this._homeFloorRoot != null)
				{
					return this.TryGetHomeFloorHit(origin, 60f, out hit);
				}
			}
			return Physics.Raycast(origin, Vector3.down, out hit, 60f, this.groundMask, QueryTriggerInteraction.Ignore);
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x00359E04 File Offset: 0x00358004
		private void ResolveHomeFloorRoot()
		{
			if (this._homeFloorRoot != null || this._homeFloorResolveAttempted)
			{
				return;
			}
			this._homeFloorResolveAttempted = true;
			foreach (Transform transform in Object.FindObjectsOfType<Transform>(true))
			{
				if (!(transform == null) && !(transform.name != "FloorCorrectiveColliders"))
				{
					Transform transform2 = transform.parent;
					while (transform2 != null && transform2.name != "CielHouse")
					{
						transform2 = transform2.parent;
					}
					if (!(transform2 == null))
					{
						this._homeFloorRoot = transform;
						return;
					}
				}
			}
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x00359E9F File Offset: 0x0035809F
		public GameObject SpawnDropAt(InventoryItemSO item, Vector3 spawnPos)
		{
			return this.SpawnDropAt(item, spawnPos, (item != null) ? item.rarity : ItemRarity.Common, -1, 1f, true, false);
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x00359EC4 File Offset: 0x003580C4
		public GameObject SpawnDropAt(InventoryItemSO item, Vector3 spawnPos, ItemRarity instanceRarity, int variantIndex, float durability01 = 1f, bool registerAsDiscovery = true, bool longWayHomeStarter = false)
		{
			if (item == null)
			{
				return null;
			}
			if (!LongWayHomeLootPolicy.CanSpawnDrop(item, registerAsDiscovery, longWayHomeStarter))
			{
				return null;
			}
			GameObject gameObject = ((item.floorPrefab != null) ? item.floorPrefab : ((item is IInventoryWorldInstanceProvider) ? item.wallPrefab : null));
			GameObject gameObject2 = ((gameObject != null) ? Object.Instantiate<GameObject>(gameObject, spawnPos, gameObject.transform.rotation) : ((this.genericFloorFallback != null) ? Object.Instantiate<GameObject>(this.genericFloorFallback, spawnPos, this.genericFloorFallback.transform.rotation) : this.CreateFallback(item, spawnPos)));
			GameObject gameObject3;
			try
			{
				InventoryItemSO inventoryItemSO = item;
				IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = item as IInventoryWholeStackWorldProvider;
				IInventoryWorldInstanceProvider inventoryWorldInstanceProvider = ((inventoryWholeStackWorldProvider == null) ? (item as IInventoryWorldInstanceProvider) : null);
				if (inventoryWholeStackWorldProvider != null)
				{
					inventoryItemSO = inventoryWholeStackWorldProvider.PrepareWholeStackWorldInstance(gameObject2);
					if (inventoryItemSO == null)
					{
						Object.Destroy(gameObject2);
						return null;
					}
				}
				else if (inventoryWorldInstanceProvider != null)
				{
					inventoryItemSO = inventoryWorldInstanceProvider.PrepareWorldInstance(gameObject2);
					if (inventoryItemSO == null)
					{
						Object.Destroy(gameObject2);
						return null;
					}
				}
				else
				{
					IInventoryInstanceVisualProvider inventoryInstanceVisualProvider = item as IInventoryInstanceVisualProvider;
					if (inventoryInstanceVisualProvider != null)
					{
						inventoryInstanceVisualProvider.ConfigureWorldInstance(gameObject2);
					}
				}
				this.EnsurePromptUI(gameObject2);
				ItemVariantLook.ApplyToWorldInstance(gameObject2, item, variantIndex);
				InventoryPickup inventoryPickup = gameObject2.GetComponentInChildren<InventoryPickup>();
				if (inventoryPickup == null)
				{
					inventoryPickup = gameObject2.AddComponent<InventoryPickup>();
				}
				foreach (MonoBehaviour monoBehaviour in gameObject2.GetComponentsInChildren<MonoBehaviour>(true))
				{
					if (monoBehaviour != inventoryPickup && monoBehaviour is IInteractionPickupCandidate)
					{
						monoBehaviour.enabled = false;
					}
				}
				inventoryPickup.enabled = true;
				inventoryPickup.Configure(inventoryItemSO, this.pickupConfig, inventoryItemSO.pickupSFX, new ItemRarity?(instanceRarity), variantIndex, durability01, registerAsDiscovery);
				if (longWayHomeStarter && RunModeService.IsLongWayHome)
				{
					LongWayHomeLootPolicy.RegisterStarterPickup(inventoryPickup);
				}
				if (inventoryWorldInstanceProvider != null)
				{
					inventoryWorldInstanceProvider.CommitWorldInstance();
				}
				Transform transform = gameObject2.transform;
				PhotoBoothPrint component = gameObject2.GetComponent<PhotoBoothPrint>();
				if (component != null)
				{
					transform = component.PivotTransform;
				}
				IInventoryPlacementPoseProvider inventoryPlacementPoseProvider = inventoryItemSO as IInventoryPlacementPoseProvider;
				if (inventoryPlacementPoseProvider != null)
				{
					Vector3 vector = ((this.player != null) ? Vector3.ProjectOnPlane(this.player.forward, Vector3.up) : Vector3.forward);
					if (vector.sqrMagnitude < 0.0001f)
					{
						vector = Vector3.forward;
					}
					Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
					transform.rotation = inventoryPlacementPoseProvider.AdjustPlacementRotation(quaternion, false);
				}
				if (Application.isPlaying)
				{
					base.StartCoroutine(this.FallToGround(transform, item.dropSFX));
				}
				this._worldDrops.Add(gameObject2);
				gameObject3 = gameObject2;
			}
			catch
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject2);
					}
					else
					{
						Object.DestroyImmediate(gameObject2);
					}
				}
				throw;
			}
			return gameObject3;
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x0035A18C File Offset: 0x0035838C
		public int ClearWorldDrops()
		{
			int num = 0;
			for (int i = 0; i < this._worldDrops.Count; i++)
			{
				GameObject gameObject = this._worldDrops[i];
				if (!(gameObject == null))
				{
					Object.Destroy(gameObject);
					num++;
				}
			}
			this._worldDrops.Clear();
			return num;
		}

		// Token: 0x06008375 RID: 33653 RVA: 0x0035A1DD File Offset: 0x003583DD
		private IEnumerator FallToGround(Transform t, AudioClip dropSFX)
		{
			if (t == null)
			{
				yield break;
			}
			Renderer rend = this.GetItemRenderer(t);
			float minY = this.player.position.y - 5f;
			float downwardSpeed = Mathf.Max(0.1f, this.fallSpeed);
			while (t != null)
			{
				Vector3 position = t.position;
				Vector3 vector = position + Vector3.up * 0.5f;
				downwardSpeed = Mathf.Min(Mathf.Max(0.5f, this.maximumFallSpeed), downwardSpeed + Mathf.Max(0.1f, this.fallAcceleration) * Time.deltaTime);
				RaycastHit raycastHit;
				if (this.TryResolveDropGround(vector, out raycastHit))
				{
					float num = ((rend != null) ? (position.y - rend.bounds.min.y) : 0f);
					float num2 = raycastHit.point.y + num + this.dropHeight;
					if (position.y <= num2 + 0.0005f)
					{
						t.position = new Vector3(position.x, num2, position.z);
						break;
					}
					float num3 = Mathf.Max(num2, position.y - downwardSpeed * Time.deltaTime);
					t.position = new Vector3(position.x, num3, position.z);
				}
				else
				{
					t.position = position - Vector3.up * downwardSpeed * Time.deltaTime;
					if (position.y < minY)
					{
						break;
					}
				}
				yield return null;
			}
			if (dropSFX != null && t != null)
			{
				AudioSource.PlayClipAtPoint(dropSFX, t.position);
			}
			yield break;
		}

		// Token: 0x06008376 RID: 33654 RVA: 0x0035A1FC File Offset: 0x003583FC
		private Renderer GetItemRenderer(Transform root)
		{
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
				{
					return renderer;
				}
			}
			return null;
		}

		// Token: 0x06008377 RID: 33655 RVA: 0x0035A238 File Offset: 0x00358438
		private GameObject CreateFallback(InventoryItemSO item, Vector3 pos)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.name = "Dropped_" + item.itemName;
			gameObject.transform.position = pos;
			gameObject.transform.localScale = Vector3.one * this.fallbackSize;
			return gameObject;
		}

		// Token: 0x06008378 RID: 33656 RVA: 0x0035A288 File Offset: 0x00358488
		private void EnsurePromptUI(GameObject world)
		{
			if (world == null)
			{
				return;
			}
			if (world.GetComponentInChildren<InteractablePromptUI>(true) != null)
			{
				return;
			}
			InteractablePromptUI interactablePromptUI = this.promptUIPrefab;
			if (interactablePromptUI == null)
			{
				foreach (InteractablePromptUI interactablePromptUI2 in Object.FindObjectsOfType<InteractablePromptUI>(true))
				{
					if (!(interactablePromptUI2 == null) && !interactablePromptUI2.transform.IsChildOf(world.transform))
					{
						interactablePromptUI = interactablePromptUI2;
						if (interactablePromptUI2.name.IndexOf("InteractPrompt", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							break;
						}
					}
				}
			}
			if (interactablePromptUI == null)
			{
				return;
			}
			Vector3 lossyScale = interactablePromptUI.transform.lossyScale;
			InteractablePromptUI interactablePromptUI3 = Object.Instantiate<InteractablePromptUI>(interactablePromptUI, world.transform);
			interactablePromptUI3.name = "DroppedItem_InteractPrompt";
			Renderer itemRenderer = this.GetItemRenderer(world.transform);
			Bounds bounds = ((itemRenderer != null) ? itemRenderer.bounds : new Bounds(world.transform.position, Vector3.one * 0.1f));
			interactablePromptUI3.transform.position = bounds.center + Vector3.up * Mathf.Max(0.14f, bounds.extents.y + this.promptUIHeight);
			interactablePromptUI3.transform.rotation = Quaternion.identity;
			InventoryDropController.SetWorldScale(interactablePromptUI3.transform, lossyScale);
			interactablePromptUI3.gameObject.SetActive(false);
		}

		// Token: 0x06008379 RID: 33657 RVA: 0x0035A3E8 File Offset: 0x003585E8
		private static void SetWorldScale(Transform target, Vector3 worldScale)
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(worldScale.x / Mathf.Max(0.0001f, Mathf.Abs(vector.x)), worldScale.y / Mathf.Max(0.0001f, Mathf.Abs(vector.y)), worldScale.z / Mathf.Max(0.0001f, Mathf.Abs(vector.z)));
		}

		// Token: 0x0400864D RID: 34381
		[Header("References")]
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x0400864E RID: 34382
		[SerializeField]
		private Transform player;

		// Token: 0x0400864F RID: 34383
		[Tooltip("Optional interaction config applied to dropped pickups (the 'Press E to pick up' prompt).")]
		[SerializeField]
		private InteractionConfigSO pickupConfig;

		// Token: 0x04008650 RID: 34384
		[Header("Drop Placement")]
		[Tooltip("How far in front of the player the item is dropped (small = right at the player's feet).")]
		[SerializeField]
		private float dropDistance = 0.6f;

		// Token: 0x04008651 RID: 34385
		[Tooltip("Furthest distance from the player a CURSOR-directed drop can land (drag out of the panel and release: the item lands toward the release point, clamped to this radius).")]
		[SerializeField]
		private float maxCursorDropDistance = 3f;

		// Token: 0x04008652 RID: 34386
		[Tooltip("Random horizontal scatter around the drop spot so items dropped on the same point never stack exactly on top of each other.")]
		[SerializeField]
		private float dropScatterRadius = 0.25f;

		// Token: 0x04008653 RID: 34387
		[Tooltip("How high above the drop spot the item spawns before falling (Fortnite-style toss).")]
		[SerializeField]
		private float spawnHeight = 1.2f;

		// Token: 0x04008654 RID: 34388
		[Tooltip("Final resting gap above the ground once it lands.")]
		[SerializeField]
		private float dropHeight = 0.03f;

		// Token: 0x04008655 RID: 34389
		[Tooltip("Layers considered 'ground' for the fall. MUST exclude the player layer or items will land on the player's own collider and float. Set to your environment/ground layers.")]
		[SerializeField]
		private LayerMask groundMask = -1;

		// Token: 0x04008656 RID: 34390
		[Tooltip("Fall speed (units/sec) for the drop-and-settle animation. Lower = slower, softer toss.")]
		[SerializeField]
		private float fallSpeed = 0.8f;

		// Token: 0x04008657 RID: 34391
		[Tooltip("Downward acceleration used by the Fortnite-style drop fall.")]
		[SerializeField]
		[Min(0.1f)]
		private float fallAcceleration = 14f;

		// Token: 0x04008658 RID: 34392
		[Tooltip("Terminal downward speed for dropped inventory objects.")]
		[SerializeField]
		[Min(0.5f)]
		private float maximumFallSpeed = 7f;

		// Token: 0x04008659 RID: 34393
		[Tooltip("Generic prefab spawned when a dropped item has no floorPrefab of its own (e.g. a folded-clothes bundle). If left null, a small primitive is used as a last resort.")]
		[SerializeField]
		private GameObject genericFloorFallback;

		// Token: 0x0400865A RID: 34394
		[Tooltip("Last-resort primitive size when neither floorPrefab nor genericFloorFallback exist.")]
		[SerializeField]
		private float fallbackSize = 0.3f;

		// Token: 0x0400865B RID: 34395
		[Tooltip("Prompt UI prefab (the 'Press E' world label) added to any dropped object that doesn't already carry one - so items without a fully-authored floorPrefab are still pick-up-able. Assign Assets/Features/Interaction/Prefabs/InteractPromptUI.prefab.")]
		[SerializeField]
		private InteractablePromptUI promptUIPrefab;

		// Token: 0x0400865C RID: 34396
		[Tooltip("Local height offset for an auto-added prompt UI, so it floats above the dropped object.")]
		[SerializeField]
		private float promptUIHeight = 0.4f;

		// Token: 0x0400865D RID: 34397
		[Header("CielHome containment")]
		[Tooltip("Horizontal clearance kept from the edge of CielHome's authored interior floor. This prevents even a scattered drop from crossing a wall or balcony pane.")]
		[SerializeField]
		[Min(0.05f)]
		private float homeBoundaryInset = 0.3f;

		// Token: 0x0400865E RID: 34398
		[Tooltip("Clear radius reserved around a home drop so it cannot settle under furniture or inside another pickup.")]
		[SerializeField]
		[Min(0.1f)]
		private float homeObstacleClearance = 0.28f;

		// Token: 0x0400865F RID: 34399
		private const string HomeSceneName = "CielHome";

		// Token: 0x04008660 RID: 34400
		private const string HomeFloorRootName = "FloorCorrectiveColliders";

		// Token: 0x04008661 RID: 34401
		private Transform _homeFloorRoot;

		// Token: 0x04008662 RID: 34402
		private bool _homeFloorResolveAttempted;

		// Token: 0x04008663 RID: 34403
		private bool _warnedMissingHomeFloor;

		// Token: 0x04008664 RID: 34404
		private readonly List<GameObject> _worldDrops = new List<GameObject>(64);
	}
}
