using System;
using System.Collections.Generic;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.NipplePiercings;
using Features.Rank;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007D5 RID: 2005
	public static class CustomizationWardrobeDiscoveryPolicy
	{
		// Token: 0x0600352B RID: 13611 RVA: 0x0012EC5E File Offset: 0x0012CE5E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetCache()
		{
			CustomizationWardrobeDiscoveryPolicy.SetSceneUnloadHook(true);
			CustomizationWardrobeDiscoveryPolicy.InvalidateRuntimeCache();
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0012EC6B File Offset: 0x0012CE6B
		private static void SetSceneUnloadHook(bool enabled)
		{
			SceneManager.sceneUnloaded -= CustomizationWardrobeDiscoveryPolicy.HandleSceneUnloaded;
			if (enabled)
			{
				SceneManager.sceneUnloaded += CustomizationWardrobeDiscoveryPolicy.HandleSceneUnloaded;
			}
			CustomizationWardrobeDiscoveryPolicy._sceneUnloadHookRegistered = enabled;
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x0012EC98 File Offset: 0x0012CE98
		private static void HandleSceneUnloaded(Scene scene)
		{
			if (CustomizationWardrobeDiscoveryPolicy._wearablesByPreset == CustomizationWardrobeDiscoveryPolicy._generatedCacheOwner)
			{
				CustomizationWardrobeDiscoveryPolicy.InvalidateRuntimeCache();
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x0012ECAB File Offset: 0x0012CEAB
		public static void InvalidateRuntimeCache()
		{
			CustomizationWardrobeDiscoveryPolicy._wearablesByPreset = null;
			CustomizationWardrobeDiscoveryPolicy._generatedCacheOwner = null;
			CustomizationWardrobeDiscoveryPolicy.IncompleteMappings.Clear();
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x0012ECC4 File Offset: 0x0012CEC4
		public static WearableItemSO GetMappedWearable(MCStylePresetSO preset)
		{
			List<WearableItemSO> list;
			bool flag;
			if (!CustomizationWardrobeDiscoveryPolicy.TryGetMappedWearables(preset, out list, out flag))
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0012ED08 File Offset: 0x0012CF08
		public static WardrobeDiscoveryResult Evaluate(MCStylePresetSO preset)
		{
			if (preset != null && (preset.isVaulted || preset.gameplayOnly || !CollarAvailabilityPolicy.IsPresetAvailable(preset) || !NipplePiercingRankPolicy.IsPresetAvailable(preset) || !LeadChokerAvailabilityPolicy.IsPresetAvailable(preset) || !HalloweenWardrobePolicy.IsPresetAvailable(preset)))
			{
				return new WardrobeDiscoveryResult(false, null);
			}
			List<WearableItemSO> list;
			bool flag;
			if (!CustomizationWardrobeDiscoveryPolicy.TryGetMappedWearables(preset, out list, out flag))
			{
				return new WardrobeDiscoveryResult(flag, null);
			}
			bool isAdminRankActive = RankProgressionService.IsAdminRankActive;
			WearableItemSO wearableItemSO = null;
			for (int i = 0; i < list.Count; i++)
			{
				WearableItemSO wearableItemSO2 = list[i];
				if (!(wearableItemSO2 == null))
				{
					if (CustomizationWardrobeDiscoveryPolicy.HasCustomizationOnlySurface(wearableItemSO2, preset))
					{
						return new WardrobeDiscoveryResult(true, wearableItemSO2);
					}
					wearableItemSO = wearableItemSO ?? wearableItemSO2;
					if (isAdminRankActive || HalloweenWardrobePolicy.IsGranted(wearableItemSO2) || CollectionUnlockRegistry.IsItemUnlocked(wearableItemSO2))
					{
						return new WardrobeDiscoveryResult(true, wearableItemSO2);
					}
				}
			}
			return new WardrobeDiscoveryResult(false, wearableItemSO);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x0012EDDC File Offset: 0x0012CFDC
		public static bool IsSurfaceDiscovered(MCStylePresetSO preset, int surfaceIndex)
		{
			if (preset == null || preset.isVaulted || preset.gameplayOnly || !CollarAvailabilityPolicy.IsPresetAvailable(preset) || !NipplePiercingRankPolicy.IsPresetAvailable(preset) || !LeadChokerAvailabilityPolicy.IsPresetAvailable(preset) || !HalloweenWardrobePolicy.IsPresetAvailable(preset) || surfaceIndex < 0 || surfaceIndex >= preset.SurfaceCount)
			{
				return false;
			}
			if (RankProgressionService.IsAdminRankActive || HalloweenWardrobePolicy.IsGranted(preset))
			{
				return true;
			}
			List<WearableItemSO> list;
			bool flag;
			if (!CustomizationWardrobeDiscoveryPolicy.TryGetMappedWearables(preset, out list, out flag))
			{
				return flag;
			}
			for (int i = 0; i < list.Count; i++)
			{
				WearableItemSO wearableItemSO = list[i];
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (!(wearableItemSO == null) && preset.TryGetSurfaceVariant(surfaceIndex, out mcsurfaceVariantSO) && !(mcsurfaceVariantSO == null))
				{
					if (!wearableItemSO.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
					{
						return true;
					}
					if (CollectionUnlockRegistry.IsVariantUnlocked(wearableItemSO, surfaceIndex))
					{
						return true;
					}
					if (surfaceIndex == preset.ResolveDefaultSurfaceIndex() && CollectionUnlockRegistry.IsVariantUnlocked(wearableItemSO, -1))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x0012EEB4 File Offset: 0x0012D0B4
		private static bool HasCustomizationOnlySurface(WearableItemSO wearable, MCStylePresetSO preset)
		{
			if (wearable == null || preset == null)
			{
				return false;
			}
			if (preset.surfaceVariants == null || preset.surfaceVariants.Count == 0)
			{
				return !wearable.showInGameplayInventory;
			}
			for (int i = 0; i < preset.surfaceVariants.Count; i++)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO = preset.surfaceVariants[i];
				if (mcsurfaceVariantSO != null && !wearable.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x0012EF2C File Offset: 0x0012D12C
		public static List<int> GetDiscoveredSurfaceIndices(MCStylePresetSO preset)
		{
			List<int> list = new List<int>();
			if (preset == null || preset.surfaceVariants == null)
			{
				return list;
			}
			for (int i = 0; i < preset.surfaceVariants.Count; i++)
			{
				if (preset.surfaceVariants[i] != null && CustomizationWardrobeDiscoveryPolicy.IsSurfaceDiscovered(preset, i))
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x0012EF90 File Offset: 0x0012D190
		public static int ResolveFirstDiscoveredSurfaceIndex(MCStylePresetSO preset)
		{
			List<int> discoveredSurfaceIndices = CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(preset);
			if (discoveredSurfaceIndices.Count <= 0)
			{
				return -1;
			}
			return discoveredSurfaceIndices[0];
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0012EFB6 File Offset: 0x0012D1B6
		public static int ResolvePreferredDiscoveredSurfaceIndex(MCStylePresetSO preset, int preferredSurfaceIndex)
		{
			if (CustomizationWardrobeDiscoveryPolicy.IsSurfaceDiscovered(preset, preferredSurfaceIndex))
			{
				return preferredSurfaceIndex;
			}
			if (preset != null && CustomizationWardrobeDiscoveryPolicy.IsSurfaceDiscovered(preset, preset.ResolveDefaultSurfaceIndex()))
			{
				return preset.ResolveDefaultSurfaceIndex();
			}
			return CustomizationWardrobeDiscoveryPolicy.ResolveFirstDiscoveredSurfaceIndex(preset);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x0012EFE8 File Offset: 0x0012D1E8
		private static bool TryGetMappedWearables(MCStylePresetSO preset, out List<WearableItemSO> wearables, out bool mappingComplete)
		{
			wearables = null;
			mappingComplete = true;
			if (preset == null)
			{
				return false;
			}
			if (CustomizationWardrobeDiscoveryPolicy._wearablesByPreset == null)
			{
				if (!CustomizationWardrobeDiscoveryPolicy._sceneUnloadHookRegistered)
				{
					CustomizationWardrobeDiscoveryPolicy.SetSceneUnloadHook(true);
				}
				CustomizationWardrobeDiscoveryPolicy._wearablesByPreset = new Dictionary<MCStylePresetSO, List<WearableItemSO>>();
				CustomizationWardrobeDiscoveryPolicy._generatedCacheOwner = CustomizationWardrobeDiscoveryPolicy._wearablesByPreset;
				CustomizationWardrobeDiscoveryPolicy.IncompleteMappings.Clear();
			}
			if (CustomizationWardrobeDiscoveryPolicy._wearablesByPreset.TryGetValue(preset, out wearables))
			{
				mappingComplete = CustomizationWardrobeDiscoveryPolicy._wearablesByPreset != CustomizationWardrobeDiscoveryPolicy._generatedCacheOwner || !CustomizationWardrobeDiscoveryPolicy.IncompleteMappings.Contains(preset);
				return mappingComplete && wearables != null && wearables.Count > 0;
			}
			if (CustomizationWardrobeDiscoveryPolicy._wearablesByPreset != CustomizationWardrobeDiscoveryPolicy._generatedCacheOwner)
			{
				return false;
			}
			WardrobeMetadataIndex wardrobeMetadataIndex = WardrobeMetadataIndex.Load();
			if (wardrobeMetadataIndex != null && wardrobeMetadataIndex.GetPresetCandidates(preset).Count == 0)
			{
				return false;
			}
			mappingComplete = wardrobeMetadataIndex != null && wardrobeMetadataIndex.TryLoadMappedWearables(preset, out wearables, null);
			if (!mappingComplete || (wearables != null && wearables.Count > 0))
			{
				CustomizationWardrobeDiscoveryPolicy._wearablesByPreset[preset] = wearables ?? new List<WearableItemSO>();
			}
			if (!mappingComplete)
			{
				CustomizationWardrobeDiscoveryPolicy.IncompleteMappings.Add(preset);
			}
			return mappingComplete && wearables != null && wearables.Count > 0;
		}

		// Token: 0x04003574 RID: 13684
		private static Dictionary<MCStylePresetSO, List<WearableItemSO>> _wearablesByPreset;

		// Token: 0x04003575 RID: 13685
		private static Dictionary<MCStylePresetSO, List<WearableItemSO>> _generatedCacheOwner;

		// Token: 0x04003576 RID: 13686
		private static readonly HashSet<MCStylePresetSO> IncompleteMappings = new HashSet<MCStylePresetSO>();

		// Token: 0x04003577 RID: 13687
		private static bool _sceneUnloadHookRegistered;
	}
}
