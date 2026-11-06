using System;
using System.Collections.Generic;
using Features.Inventory.Wearable;
using Features.PubicHairMask;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007DC RID: 2012
	[DisallowMultipleComponent]
	public sealed class MCPubicHairPreviewMask : MonoBehaviour
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x0012F834 File Offset: 0x0012DA34
		private void OnEnable()
		{
			this._style = base.GetComponent<MCStyleCustomizationService>();
			CustomizationPreviewPhysicsGuard componentInParent = base.GetComponentInParent<CustomizationPreviewPhysicsGuard>(true);
			this._root = ((componentInParent != null) ? componentInParent.transform : base.transform);
			if (this._style == null)
			{
				return;
			}
			this._style.OnActiveStateChanged += this.MarkDirty;
			this._style.OnSlotVisibilityChanged += this.MarkDirty;
			this._style.OnPreviewClothingVisibilityChanged += this.MarkDirty;
			this._style.OnPhysicalSwapCompleted += this.OnPhysicalSwap;
			this._dirty = true;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0012F8E4 File Offset: 0x0012DAE4
		private void OnDisable()
		{
			if (this._style == null)
			{
				return;
			}
			this._style.OnActiveStateChanged -= this.MarkDirty;
			this._style.OnSlotVisibilityChanged -= this.MarkDirty;
			this._style.OnPreviewClothingVisibilityChanged -= this.MarkDirty;
			this._style.OnPhysicalSwapCompleted -= this.OnPhysicalSwap;
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x0012F95C File Offset: 0x0012DB5C
		private void MarkDirty()
		{
			this._dirty = true;
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x0012F965 File Offset: 0x0012DB65
		private void OnPhysicalSwap(MCStylePartDefinitionSO part, GameObject root)
		{
			this.MarkDirty();
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x0012F970 File Offset: 0x0012DB70
		private void LateUpdate()
		{
			if (!this._dirty || this._style == null)
			{
				return;
			}
			this._dirty = false;
			PubicHairStencilMask.ConfigureMaterials(this._root, this._style, this._processed, this._fur);
			float num = 1f;
			bool flag = false;
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this._style.PartDefinitions)
			{
				if (!(mcstylePartDefinitionSO == null) && !this._style.IsPartHiddenForPreview(mcstylePartDefinitionSO.partId))
				{
					WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(this._style.GetActivePreset(mcstylePartDefinitionSO.partId));
					if (!(mappedWearable == null) && mappedWearable.physicallyCoversBottom)
					{
						GameObject spawnedRoot = this._style.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
						if (!(spawnedRoot == null) && spawnedRoot.activeInHierarchy)
						{
							num = Mathf.Min(num, PubicHairStencilMask.FurFactorFor(mappedWearable, this._style.GetSlotSurfaceIndex(mcstylePartDefinitionSO.partId)));
							flag |= PubicHairStencilMask.BypassesStencil(mappedWearable);
						}
					}
				}
			}
			PubicHairStencilMask.ApplyFurSuppression(this._fur, num, flag && num >= 1f);
		}

		// Token: 0x04003596 RID: 13718
		private MCStyleCustomizationService _style;

		// Token: 0x04003597 RID: 13719
		private Transform _root;

		// Token: 0x04003598 RID: 13720
		private bool _dirty = true;

		// Token: 0x04003599 RID: 13721
		private readonly HashSet<int> _processed = new HashSet<int>();

		// Token: 0x0400359A RID: 13722
		private readonly Dictionary<Material, Vector4> _fur = new Dictionary<Material, Vector4>();
	}
}
