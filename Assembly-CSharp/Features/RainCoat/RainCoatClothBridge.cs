using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.DebugTools;
using Features.Detection;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Features.RainCoat
{
	// Token: 0x02000DBD RID: 3517
	[DefaultExecutionOrder(20002)]
	public class RainCoatClothBridge : MonoBehaviour
	{
		// Token: 0x060067B2 RID: 26546 RVA: 0x0029733C File Offset: 0x0029553C
		private void Awake()
		{
			this.ResolveRefs();
		}

		// Token: 0x060067B3 RID: 26547 RVA: 0x00297344 File Offset: 0x00295544
		private void ResolveRefs()
		{
			if (this.flashController == null || this.hoodController == null)
			{
				foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
				{
					if (armsFlashController.Config != null)
					{
						if (this.flashController == null && armsFlashController.Config.flashLayerName == this.flashLayerName)
						{
							this.flashController = armsFlashController;
						}
						else if (this.hoodController == null && armsFlashController.Config.flashLayerName == this.hoodLayerName)
						{
							this.hoodController = armsFlashController;
						}
					}
				}
			}
			if (this.openSwap == null)
			{
				this.openSwap = base.GetComponent<RainCoatOpenSwap>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<RainCoatActionsUI>();
			}
			this._animator = base.GetComponentInParent<Animator>();
			Transform transform = ((this._animator != null) ? this._animator.transform : base.transform.root);
			if (this._wearController == null)
			{
				this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			}
			if (this._inventory == null)
			{
				this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			}
			if (this._styleService == null)
			{
				this._styleService = base.GetComponentInParent<MCStyleCustomizationService>(true) ?? transform.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x002974C0 File Offset: 0x002956C0
		private void OnEnable()
		{
			this.ResolveRefs();
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn += this.HandleItemWorn;
				this._wearController.OnItemUnworn += this.HandleItemUnworn;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveClothChanged += this.HandleActiveClothChanged;
			}
			if (this.flashController != null)
			{
				this.flashController.OnOpened.AddListener(new UnityAction(this.HandleFlashOpened));
			}
			this.SubscribeHairChanges();
			this.RestoreWornHairState();
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x0029756C File Offset: 0x0029576C
		private void OnDisable()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn -= this.HandleItemWorn;
				this._wearController.OnItemUnworn -= this.HandleItemUnworn;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveClothChanged -= this.HandleActiveClothChanged;
			}
			if (this.flashController != null)
			{
				this.flashController.OnOpened.RemoveListener(new UnityAction(this.HandleFlashOpened));
			}
			if (this._styleService != null && this._hairStyleSubscribed)
			{
				this._styleService.OnPhysicalSwapCompleted -= this.HandleHairPhysicalChanged;
				this._styleService.OnPhysicalRemoved -= this.HandleHairPhysicalRemoved;
			}
			this._hairStyleSubscribed = false;
			this._hairTargetsDirty = true;
			this._hairTargets.Clear();
			this._hoodTargets.Clear();
			this._hasHoodDownHair = false;
			this._lastHairHoodDown = float.NaN;
			this.SetStealthApplied(false);
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x00297688 File Offset: 0x00295888
		private void HandleFlashOpened()
		{
			if (this.hoodController == null || this.hoodController.IsOpen)
			{
				return;
			}
			Animator animator = this.hoodController.EnsureDetailAnimator();
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				if (animator.gameObject.activeInHierarchy && !animator.enabled)
				{
					animator.enabled = true;
				}
				if (animator.isActiveAndEnabled && this.hoodController.Config != null)
				{
					animator.SetTrigger(this.hoodController.Config.startTrigger);
				}
			}
			this.hoodController.SetOpenLatch(true);
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x00297728 File Offset: 0x00295928
		private void Update()
		{
			if (!this._enabled || this.hoodController == null)
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this.flashController != null && this.flashController.IsOpen)
			{
				return;
			}
			if (this.hoodInput != null && this.hoodInput.GetButtonDown())
			{
				this.hoodController.Toggle();
			}
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x00297790 File Offset: 0x00295990
		private void LateUpdate()
		{
			if (this._worn && this._hasHoodDownHair)
			{
				this.ApplyHairFitting(true, false);
			}
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x002977AC File Offset: 0x002959AC
		private void Start()
		{
			this.ResolveRefs();
			this.SubscribeHairChanges();
			this.RestoreWornHairState();
			bool flag = this._worn && this.IsSelected();
			this._enabled = !flag;
			this.SetMechanicsEnabled(flag);
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x002977F0 File Offset: 0x002959F0
		private bool IsOurItem(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.rainCoatClothItem != null)
			{
				return item == this.rainCoatClothItem;
			}
			return !string.IsNullOrEmpty(this.rainCoatItemId) && item.itemId == this.rainCoatItemId;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x00297844 File Offset: 0x00295A44
		private void HandleItemWorn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = true;
			if (this.flashController != null)
			{
				this.flashController.SetRestored();
			}
			if (this.hoodController != null)
			{
				this.hoodController.SetRestored();
			}
			if (this.openSwap != null)
			{
				this.openSwap.OnGarmentRestored();
			}
			this.CacheHoodRenderers();
			this.SetHairHidden(true);
			this.SetStealthApplied(true);
			this.ApplyState();
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x002978C8 File Offset: 0x00295AC8
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = false;
			if (this.flashController != null)
			{
				this.flashController.SetRemoved();
			}
			if (this.hoodController != null)
			{
				this.hoodController.SetRemoved();
			}
			this.SetHairHidden(false);
			this.SetStealthApplied(false);
			this.ApplyState();
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x0029792C File Offset: 0x00295B2C
		private void SetStealthApplied(bool on)
		{
			if (on == this._stealthApplied)
			{
				return;
			}
			if (this._stealthActor == null)
			{
				if (this._animator == null)
				{
					this._animator = base.GetComponentInParent<Animator>();
				}
				Transform transform = ((this._animator != null) ? this._animator.transform : base.transform.root);
				this._stealthActor = transform.GetComponentInChildren<StealthActor>(true) ?? transform.GetComponentInParent<StealthActor>();
				if (this._stealthActor == null)
				{
					Debug.LogWarning("[RainCoatClothBridge] Nessuno StealthActor sul player: niente bonus stealth del raincoat.", this);
					return;
				}
			}
			if (this._stealthModifier == null)
			{
				this._stealthModifier = new StealthModifier
				{
					SourceName = "RainCoat",
					VisibilityMultiplier = this.wornVisibilityMultiplier,
					NoiseMultiplier = this.wornNoiseMultiplier
				};
			}
			this._stealthModifier.VisibilityMultiplier = this.wornVisibilityMultiplier;
			this._stealthModifier.NoiseMultiplier = this.wornNoiseMultiplier;
			if (on)
			{
				this._stealthActor.AddModifier(this._stealthModifier);
			}
			else
			{
				this._stealthActor.RemoveModifier(this._stealthModifier);
			}
			this._stealthApplied = on;
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x00297A49 File Offset: 0x00295C49
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x00297A54 File Offset: 0x00295C54
		private bool IsSelected()
		{
			if (this._inventory == null)
			{
				return false;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			return this.IsOurItem(wearableItemSO);
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x00297A89 File Offset: 0x00295C89
		private void ApplyState()
		{
			this.SetMechanicsEnabled(this._worn && this.IsSelected());
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x00297AA4 File Offset: 0x00295CA4
		private void SetMechanicsEnabled(bool on)
		{
			if (on == this._enabled)
			{
				return;
			}
			this._enabled = on;
			if (this.actionsUI != null)
			{
				this.actionsUI.enabled = on;
			}
			bool flag = this.flashController != null && this.flashController.IsOpen;
			if (this.flashController != null)
			{
				this.flashController.enabled = on;
			}
			if (this.hoodController != null)
			{
				this.hoodController.enabled = on;
			}
			if (!on && flag && this.openSwap != null)
			{
				this.openSwap.OnFlashClosed();
			}
			if (this.openSwap != null)
			{
				this.openSwap.enabled = this._worn;
			}
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x00297B6D File Offset: 0x00295D6D
		private void SetHairHidden(bool hidden)
		{
			if (!this.ResolveHairRenderers())
			{
				if (hidden)
				{
					Debug.LogWarning("[RainCoatClothBridge] Nessun renderer capelli con blendshape '" + this.hideHairBlendshapeName + "' trovato sotto il rig.", this);
				}
				return;
			}
			this.ApplyHairFitting(hidden, true);
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x00297BA0 File Offset: 0x00295DA0
		private void ApplyHairFitting(bool worn, bool force = false)
		{
			float num = ((worn && this._hasHoodDownHair) ? this.ReadHoodDownWeight() : 0f);
			if (!force && Mathf.Approximately(num, this._lastHairHoodDown))
			{
				return;
			}
			this._lastHairHoodDown = num;
			foreach (ValueTuple<SkinnedMeshRenderer, Mesh, int, int> valueTuple in this._hairTargets)
			{
				if (!(valueTuple.Item1 == null) && !(valueTuple.Item1.sharedMesh != valueTuple.Item2))
				{
					float num2 = ((worn && valueTuple.Item4 >= 0) ? num : 0f);
					valueTuple.Item1.SetBlendShapeWeight(valueTuple.Item3, worn ? (100f - num2) : 0f);
					if (valueTuple.Item4 >= 0)
					{
						valueTuple.Item1.SetBlendShapeWeight(valueTuple.Item4, num2);
					}
				}
			}
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x00297C9C File Offset: 0x00295E9C
		private float ReadHoodDownWeight()
		{
			if (this.openSwap != null && this.openSwap.IsOpenCoatVisible)
			{
				return 100f;
			}
			float num = 0f;
			for (int i = 0; i < this._hoodTargets.Count; i++)
			{
				ValueTuple<SkinnedMeshRenderer, Mesh, int> valueTuple = this._hoodTargets[i];
				if (!(valueTuple.Item1 == null))
				{
					Mesh sharedMesh = valueTuple.Item1.sharedMesh;
					if (sharedMesh != valueTuple.Item2)
					{
						valueTuple.Item2 = sharedMesh;
						valueTuple.Item3 = ((sharedMesh != null) ? sharedMesh.GetBlendShapeIndex("RainCoat_Hood_Down") : (-1));
						this._hoodTargets[i] = valueTuple;
					}
					if (sharedMesh != null && valueTuple.Item3 >= 0)
					{
						num = Mathf.Max(num, valueTuple.Item1.GetBlendShapeWeight(valueTuple.Item3));
					}
				}
			}
			return Mathf.Clamp(num, 0f, 100f);
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x00297D90 File Offset: 0x00295F90
		private void CacheHoodRenderers()
		{
			this._hoodTargets.Clear();
			this._lastHairHoodDown = float.NaN;
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
			}
			if (this._animator == null)
			{
				return;
			}
			string text = ((this.rainCoatClothItem != null && !string.IsNullOrEmpty(this.rainCoatClothItem.mcStylePartId)) ? this.rainCoatClothItem.mcStylePartId : "Outfit.Swimsuit");
			GameObject gameObject = ((this._styleService != null) ? this._styleService.GetSpawnedRoot(text) : null);
			Component component = ((gameObject != null) ? gameObject.transform : this._animator.transform);
			this._hairSearch.Clear();
			component.GetComponentsInChildren<SkinnedMeshRenderer>(true, this._hairSearch);
			string text2 = ((this.flashController != null && this.flashController.Config != null) ? this.flashController.Config.wornObjectName : "RainCoat_A");
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._hairSearch)
			{
				if (!(skinnedMeshRenderer.sharedMesh == null) && !(skinnedMeshRenderer.name != text2))
				{
					int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("RainCoat_Hood_Down");
					if (blendShapeIndex >= 0)
					{
						this._hoodTargets.Add(new ValueTuple<SkinnedMeshRenderer, Mesh, int>(skinnedMeshRenderer, skinnedMeshRenderer.sharedMesh, blendShapeIndex));
					}
				}
			}
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x00297F24 File Offset: 0x00296124
		private bool ResolveHairRenderers()
		{
			if (!this._hairTargetsDirty && this._hairTargets.Count > 0)
			{
				bool flag = true;
				foreach (ValueTuple<SkinnedMeshRenderer, Mesh, int, int> valueTuple in this._hairTargets)
				{
					if (valueTuple.Item1 == null || valueTuple.Item1.sharedMesh != valueTuple.Item2)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return true;
				}
			}
			this._hairTargets.Clear();
			this._hasHoodDownHair = false;
			this._lastHairHoodDown = float.NaN;
			this._hairSearch.Clear();
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
			}
			if (this._animator == null)
			{
				return false;
			}
			GameObject gameObject = ((this._styleService != null) ? this._styleService.GetSpawnedRoot("Head.Hair") : null);
			((gameObject != null) ? gameObject.transform : this._animator.transform).GetComponentsInChildren<SkinnedMeshRenderer>(true, this._hairSearch);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._hairSearch)
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.hideHairBlendshapeName);
					if (blendShapeIndex >= 0)
					{
						int blendShapeIndex2 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.hoodDownHairBlendshapeName);
						this._hairTargets.Add(new ValueTuple<SkinnedMeshRenderer, Mesh, int, int>(skinnedMeshRenderer, skinnedMeshRenderer.sharedMesh, blendShapeIndex, blendShapeIndex2));
						this._hasHoodDownHair |= blendShapeIndex2 >= 0;
					}
				}
			}
			this._hairTargetsDirty = false;
			return this._hairTargets.Count > 0;
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x00298114 File Offset: 0x00296314
		private void SubscribeHairChanges()
		{
			if (this._hairStyleSubscribed || this._styleService == null)
			{
				return;
			}
			this._styleService.OnPhysicalSwapCompleted += this.HandleHairPhysicalChanged;
			this._styleService.OnPhysicalRemoved += this.HandleHairPhysicalRemoved;
			this._hairStyleSubscribed = true;
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x00298170 File Offset: 0x00296370
		private void RestoreWornHairState()
		{
			if (this._wearController == null)
			{
				return;
			}
			this._worn = ((this.rainCoatClothItem != null) ? this._wearController.IsWorn(this.rainCoatClothItem) : this._wearController.IsWorn(this.rainCoatItemId, false));
			this._hairTargetsDirty = true;
			if (this._worn)
			{
				this.CacheHoodRenderers();
				this.SetHairHidden(true);
			}
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x002981E4 File Offset: 0x002963E4
		private void HandleHairPhysicalChanged(MCStylePartDefinitionSO definition, GameObject hair)
		{
			if (definition == null)
			{
				return;
			}
			if (definition.partId != "Head.Hair")
			{
				string text = ((this.rainCoatClothItem != null && !string.IsNullOrEmpty(this.rainCoatClothItem.mcStylePartId)) ? this.rainCoatClothItem.mcStylePartId : "Outfit.Swimsuit");
				if (definition.partId == text)
				{
					this.CacheHoodRenderers();
					if (this._worn)
					{
						this.SetHairHidden(true);
					}
				}
				return;
			}
			this._hairTargetsDirty = true;
			if (this._worn)
			{
				this.SetHairHidden(true);
			}
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x0029827A File Offset: 0x0029647A
		private void HandleHairPhysicalRemoved(MCStylePartDefinitionSO definition, GameObject hair)
		{
			if (definition == null || definition.partId != "Head.Hair")
			{
				return;
			}
			this._hairTargets.Clear();
			this._hasHoodDownHair = false;
			this._hairTargetsDirty = true;
		}

		// Token: 0x04006A30 RID: 27184
		[Header("Identity")]
		[Tooltip("Il WearableItemSO del cloth Raincoat (Cloth_RainCoat).")]
		[SerializeField]
		private WearableItemSO rainCoatClothItem;

		// Token: 0x04006A31 RID: 27185
		[SerializeField]
		private string rainCoatItemId = "cloth_raincoat";

		// Token: 0x04006A32 RID: 27186
		[Header("Mechanics (auto-found on this GameObject if empty)")]
		[Tooltip("Flash del coat (variante ClothFlashArms con stato di hold). Identificato fra i controller su questo GO da Config.flashLayerName == flashLayerName.")]
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x04006A33 RID: 27187
		[SerializeField]
		private string flashLayerName = "RainCoatFlash";

		// Token: 0x04006A34 RID: 27188
		[Tooltip("Toggle del cappuccio (variante ClothFlashArms transiente).")]
		[SerializeField]
		private ArmsFlashController hoodController;

		// Token: 0x04006A35 RID: 27189
		[SerializeField]
		private string hoodLayerName = "RainCoatHood";

		// Token: 0x04006A36 RID: 27190
		[SerializeField]
		private RainCoatOpenSwap openSwap;

		// Token: 0x04006A37 RID: 27191
		[SerializeField]
		private RainCoatActionsUI actionsUI;

		// Token: 0x04006A38 RID: 27192
		[Header("Hair")]
		[Tooltip("Fitting dei capelli con cappuccio alzato; anche fitting classico quando manca il morph HoodDown.")]
		[SerializeField]
		private string hideHairBlendshapeName = "RainCoat_HideHair";

		// Token: 0x04006A39 RID: 27193
		[Tooltip("Fitting delle lunghezze, con sommità libera, quando il cappuccio è abbassato. I capelli senza questo morph mantengono il comportamento classico.")]
		[SerializeField]
		private string hoodDownHairBlendshapeName = "RainCoat_HoodDown_HairFit";

		// Token: 0x04006A3A RID: 27194
		[Header("Input")]
		[Tooltip("Toggle cappuccio (il bridge fa da coordinator: il controller hood ha listenForOwnInput=false). Tasto secondario dei vestiti, come towel slip/dance.")]
		[SerializeField]
		private GenericInput hoodInput = new GenericInput("N", "X", "X");

		// Token: 0x04006A3B RID: 27195
		[Header("Stealth (raincoat INDOSSATO = spotting molto più lento)")]
		[Tooltip("Moltiplicatore di VISIBILITÀ applicato allo StealthActor del player mentre il raincoat è INDOSSATO (a prescindere dalla selezione). Entra dritto nella formula di detection visiva del SensoryManager (rate ∝ FinalVisibility): 0.15 = gli NPC ti spottano all'85% più lentamente. Stesso pattern dei Socks in PlayerStealthBridge.")]
		[Range(0.05f, 1f)]
		[SerializeField]
		private float wornVisibilityMultiplier = 0.15f;

		// Token: 0x04006A3C RID: 27196
		[Tooltip("Moltiplicatore del raggio di RUMORE mentre indossato (1 = invariato).")]
		[Range(0.05f, 2f)]
		[SerializeField]
		private float wornNoiseMultiplier = 1f;

		// Token: 0x04006A3D RID: 27197
		private WearableUseController _wearController;

		// Token: 0x04006A3E RID: 27198
		private PlayerInventoryManager _inventory;

		// Token: 0x04006A3F RID: 27199
		private Animator _animator;

		// Token: 0x04006A40 RID: 27200
		private MCStyleCustomizationService _styleService;

		// Token: 0x04006A41 RID: 27201
		private bool _hairStyleSubscribed;

		// Token: 0x04006A42 RID: 27202
		private bool _hairTargetsDirty = true;

		// Token: 0x04006A43 RID: 27203
		private readonly List<SkinnedMeshRenderer> _hairSearch = new List<SkinnedMeshRenderer>();

		// Token: 0x04006A44 RID: 27204
		[TupleElementNames(new string[] { "renderer", "mesh", "index", "downIndex" })]
		private readonly List<ValueTuple<SkinnedMeshRenderer, Mesh, int, int>> _hairTargets = new List<ValueTuple<SkinnedMeshRenderer, Mesh, int, int>>();

		// Token: 0x04006A45 RID: 27205
		[TupleElementNames(new string[] { "renderer", "mesh", "index" })]
		private readonly List<ValueTuple<SkinnedMeshRenderer, Mesh, int>> _hoodTargets = new List<ValueTuple<SkinnedMeshRenderer, Mesh, int>>();

		// Token: 0x04006A46 RID: 27206
		private bool _hasHoodDownHair;

		// Token: 0x04006A47 RID: 27207
		private float _lastHairHoodDown = float.NaN;

		// Token: 0x04006A48 RID: 27208
		private bool _worn;

		// Token: 0x04006A49 RID: 27209
		private bool _enabled;

		// Token: 0x04006A4A RID: 27210
		private StealthActor _stealthActor;

		// Token: 0x04006A4B RID: 27211
		private StealthModifier _stealthModifier;

		// Token: 0x04006A4C RID: 27212
		private bool _stealthApplied;
	}
}
