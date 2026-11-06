using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.InsertableToys
{
	// Token: 0x02001178 RID: 4472
	public sealed class InsertableToyPickupSource : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate, IInteractionHandoffCandidate
	{
		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x0600889F RID: 34975 RVA: 0x0037E78F File Offset: 0x0037C98F
		public InsertableToyItemSO ItemData
		{
			get
			{
				return this.itemData;
			}
		}

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x060088A0 RID: 34976 RVA: 0x0037E797 File Offset: 0x0037C997
		public InteractionConfigSO Config
		{
			get
			{
				return this._runtimeConfig;
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x060088A1 RID: 34977 RVA: 0x0037E79F File Offset: 0x0037C99F
		public bool IsBeingUsed
		{
			get
			{
				return this._pickupInProgress;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x060088A2 RID: 34978 RVA: 0x0037E7A7 File Offset: 0x0037C9A7
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x060088A3 RID: 34979 RVA: 0x0037E7AF File Offset: 0x0037C9AF
		public bool IsCandidateEligible
		{
			get
			{
				return this._inOwnRange && this.CanInteract(this._player);
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x060088A4 RID: 34980 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x060088A5 RID: 34981 RVA: 0x0037E7C8 File Offset: 0x0037C9C8
		public bool CanPrioritizePickup
		{
			get
			{
				return this._inventory != null && this._inventory.CanAcceptWorldPickup(this.itemData, null, -1, true);
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x060088A6 RID: 34982 RVA: 0x0037E804 File Offset: 0x0037CA04
		public bool CanPrepareInteraction
		{
			get
			{
				return this.itemData != null && this.itemData.category == InventoryCategory.Item && this._inventory != null && this._inventory.CanPrepareWorldPickupSwap(this.itemData, this._inventory.ActiveSlotIndex, this._inventory.GetActiveSlotItem());
			}
		}

		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x060088A7 RID: 34983 RVA: 0x0037E863 File Offset: 0x0037CA63
		private bool CanQuickSwap
		{
			get
			{
				return this._inventory != null && this._inventory.CanSwapWorldPickupIntoSelectedSlot(this.itemData, this._inventory.ActiveSlotIndex, this._inventory.GetActiveSlotItem());
			}
		}

		// Token: 0x060088A8 RID: 34984 RVA: 0x0037E89C File Offset: 0x0037CA9C
		public void Configure(InsertableToyItemSO item)
		{
			if (item == null)
			{
				return;
			}
			this.itemData = item;
			this.EnsureRuntimeConfig();
			this.RebuildStock();
			this.ResolvePlayerAndInventory();
			this.EnsurePrompt();
			this.RefreshPromptPlacement();
			this.SetPromptVisible(false);
		}

		// Token: 0x060088A9 RID: 34985 RVA: 0x0037E8D4 File Offset: 0x0037CAD4
		private void Awake()
		{
			this.RebuildStock();
		}

		// Token: 0x060088AA RID: 34986 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x060088AB RID: 34987 RVA: 0x0037E8DC File Offset: 0x0037CADC
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._holdInput.Reset();
			this.SetPromptVisible(false);
		}

		// Token: 0x060088AC RID: 34988 RVA: 0x0037E8F8 File Offset: 0x0037CAF8
		private void OnDestroy()
		{
			InteractionCandidateRegistry.Unregister(this);
			if (this._ownsPrompt && this._prompt != null)
			{
				Object.Destroy(this._prompt.gameObject);
			}
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x060088AD RID: 34989 RVA: 0x0037E94A File Offset: 0x0037CB4A
		private void Start()
		{
			this.ResolvePlayerAndInventory();
			this.EnsureRuntimeConfig();
			this.EnsurePrompt();
			this.RefreshPromptPlacement();
			this.SetPromptVisible(false);
		}

		// Token: 0x060088AE RID: 34990 RVA: 0x0037E96C File Offset: 0x0037CB6C
		private void Update()
		{
			if (this.itemData == null)
			{
				this.SetPromptVisible(false);
				return;
			}
			if ((this._player == null || this._inventory == null) && Time.unscaledTime >= this._nextResolveTime)
			{
				this._nextResolveTime = Time.unscaledTime + 0.5f;
				this.ResolvePlayerAndInventory();
			}
			if (Time.unscaledTime >= this._nextStockRefreshTime)
			{
				this._nextStockRefreshTime = Time.unscaledTime + 1f;
				if (this.ComputeStockHierarchySignature() != this._stockHierarchySignature)
				{
					this.RebuildStock();
					this.RefreshPromptPlacement();
				}
			}
			this.RefreshCandidateDistance();
			bool flag = !this._pickupInProgress && this._inOwnRange && this.CanInteract(this._player) && InteractionCandidateRegistry.IsWinner(this);
			string text = "Pick Up" + ((flag && this.CanQuickSwap) ? "  (Hold: Swap)" : "");
			bool flag2 = this._runtimeConfig != null && this._runtimeConfig.mainActionName != text;
			if (this._runtimeConfig != null)
			{
				this._runtimeConfig.mainActionName = text;
			}
			this.SetPromptVisible(flag);
			if (flag && flag2 && this._prompt != null)
			{
				this._prompt.ShowForHandoff(this._runtimeConfig, this);
			}
			if (!flag)
			{
				this._holdInput.Reset();
				return;
			}
			WorldPickupInputAction worldPickupInputAction = this._holdInput.Step(this.interactInput.GetButtonDown(), this.interactInput.GetButtonUp(), Time.deltaTime, 0.4f, this.CanQuickSwap, this._inventory.ActiveSlotIndex, this._inventory.GetActiveSlotItem());
			if (worldPickupInputAction == WorldPickupInputAction.Tap)
			{
				this.TryPickup();
				return;
			}
			if (worldPickupInputAction == WorldPickupInputAction.Hold)
			{
				this.BeginPickup(this._holdInput.CapturedSlot, this._holdInput.CapturedItem);
			}
		}

		// Token: 0x060088AF RID: 34991 RVA: 0x0037EB42 File Offset: 0x0037CD42
		public bool CanInteract(GameObject player)
		{
			return !this._pickupInProgress && this.itemData != null && this._inventory != null && this.FindNearestAvailableStock() != null;
		}

		// Token: 0x060088B0 RID: 34992 RVA: 0x0037EB73 File Offset: 0x0037CD73
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player) || !InteractionCandidateRegistry.IsWinner(this))
			{
				return;
			}
			this.TryPickup();
		}

		// Token: 0x060088B1 RID: 34993 RVA: 0x0037EB8D File Offset: 0x0037CD8D
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x060088B2 RID: 34994 RVA: 0x0037EB96 File Offset: 0x0037CD96
		private void TryPickup()
		{
			this.BeginPickup(-1, null);
		}

		// Token: 0x060088B3 RID: 34995 RVA: 0x0037EBA0 File Offset: 0x0037CDA0
		private void BeginPickup(int swapSlot, InventoryItemSO expectedItem)
		{
			InsertableToyPickupSource.<>c__DisplayClass49_0 CS$<>8__locals1 = new InsertableToyPickupSource.<>c__DisplayClass49_0();
			CS$<>8__locals1.<>4__this = this;
			if (!this.CanInteract(this._player) || !InteractionCandidateRegistry.IsWinner(this) || (swapSlot >= 0 && !this._inventory.CanSwapWorldPickupIntoSelectedSlot(this.itemData, swapSlot, expectedItem)) || (swapSlot >= 0 && !this._inventory.TryPrepareWorldPickupSwap(this.itemData, swapSlot, expectedItem)) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			CS$<>8__locals1.selected = this.FindNearestAvailableStock();
			if (CS$<>8__locals1.selected == null)
			{
				return;
			}
			this._pickupInProgress = true;
			this._swapSlot = swapSlot;
			this._swapExpectedItem = expectedItem;
			this._holdInput.Reset();
			InsertableToyPickupSource.<>c__DisplayClass49_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._pickupSequence + 1;
			this._pickupSequence = num;
			CS$<>8__locals2.sequence = num;
			this.SetPromptVisible(false);
			CS$<>8__locals1.pickupVisual = this.CreatePickupVisual(CS$<>8__locals1.selected.Transform);
			if (this.itemData.ConsumeOneSourceChild)
			{
				InsertableToyPickupSource.SetStockAvailable(CS$<>8__locals1.selected, false);
			}
			if (this._inventory != null && CS$<>8__locals1.pickupVisual != null)
			{
				this._inventory.PlayPickupAnimation(this.itemData, CS$<>8__locals1.pickupVisual, delegate
				{
					CS$<>8__locals1.<>4__this.CompletePickup(CS$<>8__locals1.sequence, CS$<>8__locals1.selected, CS$<>8__locals1.pickupVisual);
				});
				base.StartCoroutine(this.PickupCompletionFailsafe(CS$<>8__locals1.sequence, CS$<>8__locals1.selected, CS$<>8__locals1.pickupVisual));
				return;
			}
			this.CompletePickup(CS$<>8__locals1.sequence, CS$<>8__locals1.selected, CS$<>8__locals1.pickupVisual);
		}

		// Token: 0x060088B4 RID: 34996 RVA: 0x0037ED04 File Offset: 0x0037CF04
		private void CompletePickup(int sequence, InsertableToyPickupSource.StockEntry selected, Transform pickupVisual)
		{
			if (!this._pickupInProgress || sequence != this._pickupSequence)
			{
				return;
			}
			int swapSlot = this._swapSlot;
			InventoryItemSO swapExpectedItem = this._swapExpectedItem;
			this._pickupInProgress = false;
			this._swapSlot = -1;
			this._swapExpectedItem = null;
			if ((!(this._inventory != null) || !(this.itemData != null) || !((swapSlot >= 0) ? this._inventory.TrySwapWorldPickupIntoSelectedSlot(this.itemData, this.itemData.rarity, -1, 1f, true, swapSlot, swapExpectedItem, true) : this._inventory.AddItem(this.itemData))) && this.itemData != null && this.itemData.ConsumeOneSourceChild && selected != null && selected.Transform != null)
			{
				InsertableToyPickupSource.SetStockAvailable(selected, true);
			}
			if (pickupVisual != null)
			{
				Object.Destroy(pickupVisual.gameObject);
			}
			this.RefreshCandidateDistance();
			this.RefreshPromptPlacement();
		}

		// Token: 0x060088B5 RID: 34997 RVA: 0x0037EDF7 File Offset: 0x0037CFF7
		private IEnumerator PickupCompletionFailsafe(int sequence, InsertableToyPickupSource.StockEntry selected, Transform pickupVisual)
		{
			yield return new WaitForSecondsRealtime(1f);
			if (this._pickupInProgress && sequence == this._pickupSequence)
			{
				this.CompletePickup(sequence, selected, pickupVisual);
			}
			yield break;
		}

		// Token: 0x060088B6 RID: 34998 RVA: 0x0037EE1C File Offset: 0x0037D01C
		private Transform CreatePickupVisual(Transform source)
		{
			if (source == null)
			{
				return null;
			}
			InsertableToyPickupSource.StockEntry stockEntry = this.FindStockEntry(source);
			GameObject gameObject;
			if (stockEntry != null && stockEntry.IsSourceRoot && this.itemData != null && this.itemData.floorPrefab != null)
			{
				gameObject = Object.Instantiate<GameObject>(this.itemData.floorPrefab, source.position, source.rotation);
				gameObject.transform.localScale = source.lossyScale;
			}
			else
			{
				gameObject = Object.Instantiate<GameObject>(source.gameObject, source.parent);
			}
			gameObject.name = source.name + "_PickupVisual";
			Transform transform = gameObject.transform;
			transform.SetParent(null, true);
			Collider[] componentsInChildren = gameObject.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			foreach (Rigidbody rigidbody in gameObject.GetComponentsInChildren<Rigidbody>(true))
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.detectCollisions = false;
				rigidbody.isKinematic = true;
			}
			foreach (MonoBehaviour monoBehaviour in gameObject.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour is IInteractable || monoBehaviour is IInteractionCandidate)
				{
					monoBehaviour.enabled = false;
				}
			}
			return transform;
		}

		// Token: 0x060088B7 RID: 34999 RVA: 0x0037EF78 File Offset: 0x0037D178
		private void ResolvePlayerAndInventory()
		{
			this._inventory = PlayerInventoryManager.Instance;
			if (this._inventory == null)
			{
				this._player = null;
				return;
			}
			vThirdPersonController componentInParent = this._inventory.GetComponentInParent<vThirdPersonController>();
			this._player = ((componentInParent != null) ? componentInParent.gameObject : this._inventory.transform.root.gameObject);
		}

		// Token: 0x060088B8 RID: 35000 RVA: 0x0037EFE0 File Offset: 0x0037D1E0
		private void EnsureRuntimeConfig()
		{
			if (this.itemData == null)
			{
				return;
			}
			if (this._runtimeConfig == null)
			{
				this._runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._runtimeConfig.hideFlags = HideFlags.HideAndDontSave;
			}
			this._runtimeConfig.objectName = (string.IsNullOrWhiteSpace(this.itemData.itemName) ? "Toy" : this.itemData.itemName);
			this._runtimeConfig.objectNameColor = Color.white;
			this._runtimeConfig.mainActionName = "Pick Up";
			this._runtimeConfig.mainKeyDisplayLabel = "E";
			this._runtimeConfig.secondaryActionName = string.Empty;
			this._runtimeConfig.secondaryKeyDisplayLabel = string.Empty;
		}

		// Token: 0x060088B9 RID: 35001 RVA: 0x0037F0A4 File Offset: 0x0037D2A4
		private void EnsurePrompt()
		{
			if (this._prompt != null || this.itemData == null || this.itemData.PickupPromptPrefab == null)
			{
				return;
			}
			this._prompt = Object.Instantiate<InteractablePromptUI>(this.itemData.PickupPromptPrefab);
			this._prompt.name = "InsertableToyPickupPrompt";
			this._prompt.transform.SetParent(base.transform, true);
			this._ownsPrompt = true;
			this._prompt.ShowForHandoff(this._runtimeConfig, this);
			this._prompt.Hide();
		}

		// Token: 0x060088BA RID: 35002 RVA: 0x0037F144 File Offset: 0x0037D344
		private void SetPromptVisible(bool visible)
		{
			bool flag = false;
			if (this._prompt == null && visible)
			{
				this.EnsureRuntimeConfig();
				this.EnsurePrompt();
				this.RefreshPromptPlacement();
				flag = this._prompt != null;
			}
			if (this._prompt == null || (!flag && this._promptVisible == visible))
			{
				return;
			}
			this._promptVisible = visible;
			if (visible)
			{
				this._prompt.ShowForHandoff(this._runtimeConfig, this);
				return;
			}
			this._prompt.Hide();
		}

		// Token: 0x060088BB RID: 35003 RVA: 0x0037F1C8 File Offset: 0x0037D3C8
		private void RebuildStock()
		{
			this._stock.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (!(child == null) && (!(this._prompt != null) || !(child == this._prompt.transform)))
				{
					Renderer[] componentsInChildren = child.GetComponentsInChildren<Renderer>(true);
					if (componentsInChildren != null && componentsInChildren.Length != 0)
					{
						this._stock.Add(new InsertableToyPickupSource.StockEntry
						{
							Transform = child,
							Renderers = componentsInChildren
						});
					}
				}
			}
			if (this._stock.Count == 0)
			{
				Renderer[] componentsInChildren2 = base.GetComponentsInChildren<Renderer>(true);
				if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
				{
					Collider[] componentsInChildren3 = base.GetComponentsInChildren<Collider>(true);
					InsertableToyPickupSource.StockEntry stockEntry = new InsertableToyPickupSource.StockEntry
					{
						Transform = base.transform,
						Renderers = componentsInChildren2,
						Colliders = componentsInChildren3,
						RendererEnabledBaseline = new bool[componentsInChildren2.Length],
						ColliderEnabledBaseline = new bool[componentsInChildren3.Length],
						IsSourceRoot = true
					};
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						stockEntry.RendererEnabledBaseline[j] = componentsInChildren2[j] != null && componentsInChildren2[j].enabled;
					}
					for (int k = 0; k < componentsInChildren3.Length; k++)
					{
						stockEntry.ColliderEnabledBaseline[k] = componentsInChildren3[k] != null && componentsInChildren3[k].enabled;
					}
					this._stock.Add(stockEntry);
				}
			}
			this._stockHierarchySignature = this.ComputeStockHierarchySignature();
		}

		// Token: 0x060088BC RID: 35004 RVA: 0x0037F354 File Offset: 0x0037D554
		private int ComputeStockHierarchySignature()
		{
			int num = base.transform.hierarchyCount;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (!(this._prompt != null) || !(child == this._prompt.transform))
				{
					num = (num * 397) ^ ((child != null) ? child.GetInstanceID() : 0);
					num = (num * 397) ^ ((child != null && child.gameObject.activeSelf) ? 1 : 0);
				}
			}
			return num;
		}

		// Token: 0x060088BD RID: 35005 RVA: 0x0037F3F0 File Offset: 0x0037D5F0
		private InsertableToyPickupSource.StockEntry FindStockEntry(Transform stockTransform)
		{
			for (int i = 0; i < this._stock.Count; i++)
			{
				InsertableToyPickupSource.StockEntry stockEntry = this._stock[i];
				if (stockEntry != null && stockEntry.Transform == stockTransform)
				{
					return stockEntry;
				}
			}
			return null;
		}

		// Token: 0x060088BE RID: 35006 RVA: 0x0037F434 File Offset: 0x0037D634
		private static bool IsStockAvailable(InsertableToyPickupSource.StockEntry entry)
		{
			if (entry == null || entry.Transform == null || entry.Consumed || !entry.Transform.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (!entry.IsSourceRoot)
			{
				return true;
			}
			int num = 0;
			while (entry.Renderers != null && num < entry.Renderers.Length)
			{
				Renderer renderer = entry.Renderers[num];
				if (renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x060088BF RID: 35007 RVA: 0x0037F4C0 File Offset: 0x0037D6C0
		private static void SetStockAvailable(InsertableToyPickupSource.StockEntry entry, bool available)
		{
			if (entry == null || entry.Transform == null)
			{
				return;
			}
			entry.Consumed = !available;
			if (!entry.IsSourceRoot)
			{
				entry.Transform.gameObject.SetActive(available);
				return;
			}
			int num = 0;
			while (entry.Renderers != null && num < entry.Renderers.Length)
			{
				Renderer renderer = entry.Renderers[num];
				if (!(renderer == null))
				{
					bool flag = entry.RendererEnabledBaseline != null && num < entry.RendererEnabledBaseline.Length && entry.RendererEnabledBaseline[num];
					renderer.enabled = available && flag;
				}
				num++;
			}
			int num2 = 0;
			while (entry.Colliders != null && num2 < entry.Colliders.Length)
			{
				Collider collider = entry.Colliders[num2];
				if (!(collider == null))
				{
					bool flag2 = entry.ColliderEnabledBaseline != null && num2 < entry.ColliderEnabledBaseline.Length && entry.ColliderEnabledBaseline[num2];
					collider.enabled = available && flag2;
				}
				num2++;
			}
		}

		// Token: 0x060088C0 RID: 35008 RVA: 0x0037F5B4 File Offset: 0x0037D7B4
		private InsertableToyPickupSource.StockEntry FindNearestAvailableStock()
		{
			InsertableToyPickupSource.StockEntry stockEntry = null;
			float num = float.MaxValue;
			Vector3 vector = ((this._player != null) ? this._player.transform.position : base.transform.position);
			for (int i = 0; i < this._stock.Count; i++)
			{
				InsertableToyPickupSource.StockEntry stockEntry2 = this._stock[i];
				if (InsertableToyPickupSource.IsStockAvailable(stockEntry2))
				{
					float num2 = InsertableToyPickupSource.DistanceToEntrySqr(stockEntry2, vector);
					if (num2 < num)
					{
						num = num2;
						stockEntry = stockEntry2;
					}
				}
			}
			return stockEntry;
		}

		// Token: 0x060088C1 RID: 35009 RVA: 0x0037F638 File Offset: 0x0037D838
		private void RefreshCandidateDistance()
		{
			if (this._player == null)
			{
				this._sqrDistance = float.MaxValue;
				this._inOwnRange = false;
				return;
			}
			InsertableToyPickupSource.StockEntry stockEntry = this.FindNearestAvailableStock();
			if (stockEntry == null)
			{
				this._sqrDistance = float.MaxValue;
				this._inOwnRange = false;
				return;
			}
			this._sqrDistance = InsertableToyPickupSource.DistanceToEntrySqr(stockEntry, this._player.transform.position);
			float num = ((this.itemData != null) ? this.itemData.SourceDetectionRadius : 1.35f);
			this._inOwnRange = this._sqrDistance <= num * num;
		}

		// Token: 0x060088C2 RID: 35010 RVA: 0x0037F6D4 File Offset: 0x0037D8D4
		private static float DistanceToEntrySqr(InsertableToyPickupSource.StockEntry entry, Vector3 position)
		{
			float num = (entry.Transform.position - position).sqrMagnitude;
			Renderer[] renderers = entry.Renderers;
			int num2 = 0;
			while (renderers != null && num2 < renderers.Length)
			{
				Renderer renderer = renderers[num2];
				if (!(renderer == null) && renderer.gameObject.activeInHierarchy)
				{
					float sqrMagnitude = (renderer.bounds.ClosestPoint(position) - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
					}
				}
				num2++;
			}
			return num;
		}

		// Token: 0x060088C3 RID: 35011 RVA: 0x0037F75C File Offset: 0x0037D95C
		private void RefreshPromptPlacement()
		{
			if (this._prompt == null)
			{
				return;
			}
			bool flag = false;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this._stock.Count; i++)
			{
				InsertableToyPickupSource.StockEntry stockEntry = this._stock[i];
				if (InsertableToyPickupSource.IsStockAvailable(stockEntry))
				{
					Renderer[] renderers = stockEntry.Renderers;
					int num = 0;
					while (renderers != null && num < renderers.Length)
					{
						Renderer renderer = renderers[num];
						if (!(renderer == null) && renderer.gameObject.activeInHierarchy)
						{
							if (!flag)
							{
								bounds = renderer.bounds;
								flag = true;
							}
							else
							{
								bounds.Encapsulate(renderer.bounds);
							}
						}
						num++;
					}
				}
			}
			Vector3 vector = (flag ? bounds.center : base.transform.position);
			float num2 = ((this.itemData != null) ? this.itemData.SourcePromptHeight : 0.75f);
			this._prompt.transform.position = vector + Vector3.up * num2;
		}

		// Token: 0x04008B51 RID: 35665
		[SerializeField]
		private InsertableToyItemSO itemData;

		// Token: 0x04008B52 RID: 35666
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04008B53 RID: 35667
		private readonly List<InsertableToyPickupSource.StockEntry> _stock = new List<InsertableToyPickupSource.StockEntry>(12);

		// Token: 0x04008B54 RID: 35668
		private PlayerInventoryManager _inventory;

		// Token: 0x04008B55 RID: 35669
		private GameObject _player;

		// Token: 0x04008B56 RID: 35670
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x04008B57 RID: 35671
		private InteractablePromptUI _prompt;

		// Token: 0x04008B58 RID: 35672
		private bool _ownsPrompt;

		// Token: 0x04008B59 RID: 35673
		private bool _promptVisible;

		// Token: 0x04008B5A RID: 35674
		private bool _inOwnRange;

		// Token: 0x04008B5B RID: 35675
		private bool _pickupInProgress;

		// Token: 0x04008B5C RID: 35676
		private float _sqrDistance = float.MaxValue;

		// Token: 0x04008B5D RID: 35677
		private float _nextResolveTime;

		// Token: 0x04008B5E RID: 35678
		private float _nextStockRefreshTime;

		// Token: 0x04008B5F RID: 35679
		private int _stockHierarchySignature;

		// Token: 0x04008B60 RID: 35680
		private int _pickupSequence;

		// Token: 0x04008B61 RID: 35681
		private readonly WorldPickupHoldInput _holdInput = new WorldPickupHoldInput();

		// Token: 0x04008B62 RID: 35682
		private int _swapSlot = -1;

		// Token: 0x04008B63 RID: 35683
		private InventoryItemSO _swapExpectedItem;

		// Token: 0x02001179 RID: 4473
		private sealed class StockEntry
		{
			// Token: 0x04008B64 RID: 35684
			public Transform Transform;

			// Token: 0x04008B65 RID: 35685
			public Renderer[] Renderers;

			// Token: 0x04008B66 RID: 35686
			public Collider[] Colliders;

			// Token: 0x04008B67 RID: 35687
			public bool[] RendererEnabledBaseline;

			// Token: 0x04008B68 RID: 35688
			public bool[] ColliderEnabledBaseline;

			// Token: 0x04008B69 RID: 35689
			public bool IsSourceRoot;

			// Token: 0x04008B6A RID: 35690
			public bool Consumed;
		}
	}
}
