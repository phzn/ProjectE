using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010FD RID: 4349
	public static class ItemVariantLook
	{
		// Token: 0x06008389 RID: 33673 RVA: 0x0035A934 File Offset: 0x00358B34
		public static MCSurfaceVariantSO ResolveVariant(InventoryItemSO item, int variantIndex)
		{
			if (variantIndex < 0)
			{
				return null;
			}
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO == null || wearableItemSO.mcStylePreset == null)
			{
				return null;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!wearableItemSO.mcStylePreset.TryGetSurfaceVariant(variantIndex, out mcsurfaceVariantSO))
			{
				return null;
			}
			return mcsurfaceVariantSO;
		}

		// Token: 0x0600838A RID: 33674 RVA: 0x0035A97C File Offset: 0x00358B7C
		public static Sprite ResolveIcon(InventoryItemSO item, int variantIndex)
		{
			MCSurfaceVariantSO mcsurfaceVariantSO = ItemVariantLook.ResolveVariant(item, variantIndex);
			if (mcsurfaceVariantSO != null && mcsurfaceVariantSO.previewIcon != null)
			{
				return mcsurfaceVariantSO.previewIcon;
			}
			if (!(item != null))
			{
				return null;
			}
			return item.icon;
		}

		// Token: 0x0600838B RID: 33675 RVA: 0x0035A9C0 File Offset: 0x00358BC0
		public static void ApplyToWorldInstance(GameObject root, InventoryItemSO item, int variantIndex)
		{
			ItemVariantLook.ApplyVariantToRenderers(root, ItemVariantLook.ResolveVariant(item, variantIndex));
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x0035A9D0 File Offset: 0x00358BD0
		public static void ApplyVariantToRenderers(GameObject root, MCSurfaceVariantSO variant)
		{
			if (root == null || variant == null)
			{
				return;
			}
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				if ((renderer is MeshRenderer || renderer is SkinnedMeshRenderer) && !(renderer.GetComponentInParent<LootRarityAura>() != null))
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					if (sharedMaterials != null && sharedMaterials.Length != 0)
					{
						Material[] array = new Material[sharedMaterials.Length];
						for (int j = 0; j < sharedMaterials.Length; j++)
						{
							array[j] = ((sharedMaterials[j] != null) ? new Material(sharedMaterials[j]) : null);
						}
						ItemVariantLook.ApplyMaterialSwaps(renderer, array, variant);
						ItemVariantLook.ApplyTextureOverrides(array, variant);
						renderer.sharedMaterials = array;
					}
				}
			}
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x0035AA90 File Offset: 0x00358C90
		private static void ApplyMaterialSwaps(Renderer renderer, Material[] materials, MCSurfaceVariantSO variant)
		{
			MCSurfaceVariantSO.MaterialTargetBinding[] materialBindings = variant.materialBindings;
			if (materialBindings == null || materialBindings.Length == 0)
			{
				return;
			}
			foreach (MCSurfaceVariantSO.MaterialTargetBinding materialTargetBinding in materialBindings)
			{
				if (materialTargetBinding != null && !(materialTargetBinding.material == null) && (string.IsNullOrWhiteSpace(materialTargetBinding.rendererName) || string.Equals(renderer.name, materialTargetBinding.rendererName, StringComparison.Ordinal)))
				{
					if (materialTargetBinding.materialIndex >= 0 && materialTargetBinding.materialIndex < materials.Length)
					{
						materials[materialTargetBinding.materialIndex] = new Material(materialTargetBinding.material);
					}
					else
					{
						for (int j = 0; j < materials.Length; j++)
						{
							materials[j] = new Material(materialTargetBinding.material);
						}
					}
				}
			}
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x0035AB38 File Offset: 0x00358D38
		private static void ApplyTextureOverrides(Material[] materials, MCSurfaceVariantSO variant)
		{
			MCSurfaceVariantSO.TextureTargetBinding[] textureBindings = variant.textureBindings;
			if (textureBindings == null || textureBindings.Length == 0)
			{
				for (int i = 0; i < materials.Length; i++)
				{
					ItemVariantLook.ApplyTextureIfPresent(materials[i], variant.baseMapPropertyName, variant.baseMap);
					ItemVariantLook.ApplyTextureIfPresent(materials[i], variant.normalMapPropertyName, variant.normalMap);
					ItemVariantLook.ApplyTextureIfPresent(materials[i], variant.maskMapPropertyName, variant.maskMap);
				}
				return;
			}
			foreach (MCSurfaceVariantSO.TextureTargetBinding textureTargetBinding in textureBindings)
			{
				if (textureTargetBinding != null && textureTargetBinding.HasAnyTexture)
				{
					string text = (string.IsNullOrEmpty(textureTargetBinding.baseMapPropertyName) ? variant.baseMapPropertyName : textureTargetBinding.baseMapPropertyName);
					string text2 = (string.IsNullOrEmpty(textureTargetBinding.normalMapPropertyName) ? variant.normalMapPropertyName : textureTargetBinding.normalMapPropertyName);
					string text3 = (string.IsNullOrEmpty(textureTargetBinding.maskMapPropertyName) ? variant.maskMapPropertyName : textureTargetBinding.maskMapPropertyName);
					if (textureTargetBinding.materialIndex >= 0 && textureTargetBinding.materialIndex < materials.Length)
					{
						ItemVariantLook.ApplyTextureIfPresent(materials[textureTargetBinding.materialIndex], text, textureTargetBinding.baseMap);
						ItemVariantLook.ApplyTextureIfPresent(materials[textureTargetBinding.materialIndex], text2, textureTargetBinding.normalMap);
						ItemVariantLook.ApplyTextureIfPresent(materials[textureTargetBinding.materialIndex], text3, textureTargetBinding.maskMap);
					}
					else
					{
						for (int k = 0; k < materials.Length; k++)
						{
							ItemVariantLook.ApplyTextureIfPresent(materials[k], text, textureTargetBinding.baseMap);
							ItemVariantLook.ApplyTextureIfPresent(materials[k], text2, textureTargetBinding.normalMap);
							ItemVariantLook.ApplyTextureIfPresent(materials[k], text3, textureTargetBinding.maskMap);
						}
					}
				}
			}
		}

		// Token: 0x0600838F RID: 33679 RVA: 0x0035ACBE File Offset: 0x00358EBE
		private static void ApplyTextureIfPresent(Material material, string propertyName, Texture texture)
		{
			if (material == null || texture == null || string.IsNullOrEmpty(propertyName))
			{
				return;
			}
			if (!material.HasProperty(propertyName))
			{
				return;
			}
			material.SetTexture(propertyName, texture);
		}
	}
}
