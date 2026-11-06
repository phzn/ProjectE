using System;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008E5 RID: 2277
	public class MaskClothBridge : MonoBehaviour
	{
		// Token: 0x06003D6B RID: 15723 RVA: 0x0016D49F File Offset: 0x0016B69F
		private void Awake()
		{
			this.ResolveRefs();
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0016D4A8 File Offset: 0x0016B6A8
		private void ResolveRefs()
		{
			if (this.maskToggle == null)
			{
				this.maskToggle = base.GetComponent<MaskToggleController>();
			}
			if (this.maskToggle == null)
			{
				this.maskToggle = base.GetComponentInParent<MaskToggleController>();
			}
			if (this.actionsUI == null)
			{
				this.actionsUI = base.GetComponent<MaskActionsUI>();
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			if (this._wearController == null)
			{
				this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			}
			if (this._inventory == null)
			{
				this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0016D570 File Offset: 0x0016B770
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
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x0016D5E4 File Offset: 0x0016B7E4
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
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x0016D652 File Offset: 0x0016B852
		private void Start()
		{
			this._enabled = false;
			this.ApplyState();
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0016D664 File Offset: 0x0016B864
		private bool IsOurItem(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (this.maskClothItem != null)
			{
				return item == this.maskClothItem;
			}
			return !string.IsNullOrEmpty(this.maskItemId) && item.itemId == this.maskItemId;
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x0016D6B7 File Offset: 0x0016B8B7
		private void HandleItemWorn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = true;
			this.ApplyState();
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0016D6D0 File Offset: 0x0016B8D0
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (!this.IsOurItem(item))
			{
				return;
			}
			this._worn = false;
			this.ApplyState();
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0016D6E9 File Offset: 0x0016B8E9
		private void HandleActiveClothChanged(int _)
		{
			this.ApplyState();
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0016D6F4 File Offset: 0x0016B8F4
		private bool IsSelected()
		{
			if (this._inventory == null)
			{
				return false;
			}
			WearableItemSO wearableItemSO = this._inventory.GetActiveCloth() as WearableItemSO;
			return this.IsOurItem(wearableItemSO);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0016D72C File Offset: 0x0016B92C
		private void ApplyState()
		{
			if (this.maskToggle == null)
			{
				return;
			}
			bool flag = this._worn && this.IsSelected();
			if (flag)
			{
				if (!this._enabled)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = this.FindMaskRenderer();
					this.maskToggle.RebindMaskRenderer(skinnedMeshRenderer);
					this.maskToggle.enabled = true;
				}
				this.maskToggle.InputEnabled = true;
				if (this.actionsUI != null)
				{
					this.actionsUI.enabled = true;
				}
			}
			else if (this._worn)
			{
				this.maskToggle.InputEnabled = false;
				if (this.actionsUI != null)
				{
					this.actionsUI.enabled = false;
				}
			}
			else
			{
				if (this.actionsUI != null)
				{
					this.actionsUI.enabled = false;
				}
				this.maskToggle.SetMaskAvailable(false);
				this.maskToggle.enabled = false;
			}
			this._enabled = flag;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0016D818 File Offset: 0x0016BA18
		private SkinnedMeshRenderer FindMaskRenderer()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			if (transform == null)
			{
				return null;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in transform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.sharedMesh == null) && skinnedMeshRenderer2.gameObject.activeInHierarchy)
				{
					if (skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(this.downBlendshapeName) >= 0)
					{
						return skinnedMeshRenderer2;
					}
					if (skinnedMeshRenderer == null && this.NameMatches(skinnedMeshRenderer2.gameObject.name))
					{
						skinnedMeshRenderer = skinnedMeshRenderer2;
					}
				}
			}
			return skinnedMeshRenderer;
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0016D8D6 File Offset: 0x0016BAD6
		private bool NameMatches(string n)
		{
			return !string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(this.maskObjectName) && (n == this.maskObjectName || n.StartsWith(this.maskObjectName));
		}

		// Token: 0x04003D76 RID: 15734
		[Header("Identity")]
		[Tooltip("Il WearableItemSO del cloth Mask (Cloth_Mask).")]
		[SerializeField]
		private WearableItemSO maskClothItem;

		// Token: 0x04003D77 RID: 15735
		[SerializeField]
		private string maskItemId = "cloth_mask";

		// Token: 0x04003D78 RID: 15736
		[Header("Mechanics (auto-found on this GameObject if empty)")]
		[SerializeField]
		private MaskToggleController maskToggle;

		// Token: 0x04003D79 RID: 15737
		[SerializeField]
		private MaskActionsUI actionsUI;

		// Token: 0x04003D7A RID: 15738
		[Header("Mask mesh lookup")]
		[Tooltip("Nome del blendshape di abbassamento sulla mask (deve combaciare con MaskToggleController.downBlendshapeName).")]
		[SerializeField]
		private string downBlendshapeName = "DOWN";

		// Token: 0x04003D7B RID: 15739
		[Tooltip("Nome (o prefisso) del GameObject mesh della mask dinamica sotto il player. Fallback se la ricerca per blendshape non bastasse.")]
		[SerializeField]
		private string maskObjectName = "Mask";

		// Token: 0x04003D7C RID: 15740
		private WearableUseController _wearController;

		// Token: 0x04003D7D RID: 15741
		private PlayerInventoryManager _inventory;

		// Token: 0x04003D7E RID: 15742
		private bool _worn;

		// Token: 0x04003D7F RID: 15743
		private bool _enabled;
	}
}
