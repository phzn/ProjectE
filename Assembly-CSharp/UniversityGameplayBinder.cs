using System;
using System.Collections.Generic;
using System.Globalization;
using Bozo.AnimeCharacters;
using Dinopunch.LivingMud;
using Features.Audio;
using Features.Bush;
using Features.ClothingLoot;
using Features.Detection;
using Features.DumpsterHide;
using Features.HideFlash;
using Features.InsertableToys;
using Features.InsertableToys.Vegetables;
using Features.Inventory;
using Features.LockerHide;
using Features.MapGeneration.Scripts;
using Features.NPC;
using Features.NPC.Audio;
using Features.NPC.StateMachine;
using Features.PlayerEmotes;
using Features.Stadium;
using Features.TrashcanLoot;
using Features.Vehicles.Spawning;
using Features.WantedPoster;
using PoolWaterTools;
using ProjectExhibitionist;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.NPCs;
using ProjectExhibitionist.Subway;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;

// Token: 0x020000A9 RID: 169
public static class UniversityGameplayBinder
{
	// Token: 0x060006CB RID: 1739 RVA: 0x0002F264 File Offset: 0x0002D464
	public static Type ExpectedComponentType(UniversityGameplayField field)
	{
		switch (field)
		{
		case UniversityGameplayField.StreamedWorldPropSlot_viewPrefab:
			return typeof(StreamedWorldPropSlot);
		case UniversityGameplayField.BushConcealment_enterSound:
			return typeof(BushConcealment);
		case UniversityGameplayField.VehicleSpawnPoint_spawnProfile:
			return typeof(VehicleSpawnPoint);
		case UniversityGameplayField.DumpsterHideInteractable_enterConfig:
			return typeof(DumpsterHideInteractable);
		case UniversityGameplayField.DumpsterHideInteractable_hiddenHudConfig:
			return typeof(DumpsterHideInteractable);
		case UniversityGameplayField.DumpsterHideInteractable_lidCloseSound:
			return typeof(DumpsterHideInteractable);
		case UniversityGameplayField.DumpsterHideInteractable_lidOpenSound:
			return typeof(DumpsterHideInteractable);
		case UniversityGameplayField.HideFlashPickup_interactionConfig:
			return typeof(HideFlashPickup);
		case UniversityGameplayField.HideFlashPickup_itemData:
			return typeof(HideFlashPickup);
		case UniversityGameplayField.TrashcanLootInteractable_ejectSFX:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_lootBodyPaintDecal:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_lootProfile:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_progressUiPrefab:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_promptConfig:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_rummageHudConfig:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.TrashcanLootInteractable_searchLoopSFX:
			return typeof(TrashcanLootInteractable);
		case UniversityGameplayField.WasteCanHideInteractable_hiddenHudConfig:
			return typeof(WasteCanHideInteractable);
		case UniversityGameplayField.WantedPosterPickup_interactionConfig:
			return typeof(WantedPosterPickup);
		case UniversityGameplayField.WantedPosterPickup_itemData:
			return typeof(WantedPosterPickup);
		case UniversityGameplayField.LockerHideInteractable_doorCloseSound:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_doorOpenSound:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_enterConfig:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_hiddenHudConfig:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_lockerEnterAnimation:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_lockerExitAnimation:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_lockerIdleAnimation:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_peeAudioClip:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.LockerHideInteractable_peeEffectPrefab:
			return typeof(LockerHideInteractable);
		case UniversityGameplayField.Outfit_LinkedColorSets_linkedType:
			return typeof(Outfit);
		case UniversityGameplayField.Outfit_OutfitIcon:
			return typeof(Outfit);
		case UniversityGameplayField.Outfit_Type:
			return typeof(Outfit);
		case UniversityGameplayField.MudPuddleInteractable_interactionConfig:
			return typeof(MudPuddleInteractable);
		case UniversityGameplayField.Door_txt:
			return typeof(Door);
		case UniversityGameplayField.FootstepController_footstepClips:
			return typeof(FootstepController);
		case UniversityGameplayField.ClothingLootTableInteractable_interactionConfig:
			return typeof(ClothingLootTableInteractable);
		case UniversityGameplayField.ClothingLootTableInteractable_lootPool:
			return typeof(ClothingLootTableInteractable);
		case UniversityGameplayField.ClothingLootTableInteractable_promptPrefab:
			return typeof(ClothingLootTableInteractable);
		case UniversityGameplayField.NPCSenses_ReactionProfile:
			return typeof(NPCSenses);
		case UniversityGameplayField.DumpsterHideInteractable_hideBodyPaintDecal:
			return typeof(DumpsterHideInteractable);
		case UniversityGameplayField.VegetableTheftSource_interactionConfig:
			return typeof(VegetableTheftSource);
		case UniversityGameplayField.VegetableTheftSource_promptPrefab:
			return typeof(VegetableTheftSource);
		case UniversityGameplayField.VegetableTheftSource_vegetables:
			return typeof(VegetableTheftSource);
		case UniversityGameplayField.KombiniSlidingDoor_closeClip:
			return typeof(KombiniSlidingDoor);
		case UniversityGameplayField.KombiniSlidingDoor_openClip:
			return typeof(KombiniSlidingDoor);
		case UniversityGameplayField.NPCReactionAudioPlayer_audioProfile:
			return typeof(NPCReactionAudioPlayer);
		case UniversityGameplayField.NPCWanderController_actionRegistry:
			return typeof(NPCWanderController);
		case UniversityGameplayField.NPCAlertHandler_actionRegistry:
			return typeof(NPCAlertHandler);
		case UniversityGameplayField.NPCAlertHandler_alertAnimationProfile:
			return typeof(NPCAlertHandler);
		case UniversityGameplayField.StaticPhoneNPC_encounterController:
			return typeof(StaticPhoneNPC);
		case UniversityGameplayField.PoiEmoteZone_profile:
			return typeof(PoiEmoteZone);
		case UniversityGameplayField.StadiumShellEnvironment_daySignMaterial:
			return typeof(StadiumShellEnvironment);
		case UniversityGameplayField.StadiumShellEnvironment_nightSignMaterial:
			return typeof(StadiumShellEnvironment);
		case UniversityGameplayField.NPCEquipmentManager_actionRegistry:
			return typeof(NPCEquipmentManager);
		case UniversityGameplayField.NPCEquipmentManager_sharedConfig:
			return typeof(NPCEquipmentManager);
		case UniversityGameplayField.ShallowPoolWater_footstepSplash:
			return typeof(ShallowPoolWater);
		case UniversityGameplayField.ShallowPoolWater_footstepVariations:
			return typeof(ShallowPoolWater);
		case UniversityGameplayField.ShallowPoolWater_ripplePrefab:
			return typeof(ShallowPoolWater);
		case UniversityGameplayField.ShallowPoolWater_splashPrefab:
			return typeof(ShallowPoolWater);
		case UniversityGameplayField.CoverWallInteractable_interactionConfig:
			return typeof(CoverWallInteractable);
		case UniversityGameplayField.CoverWallSetup_interactionConfig:
			return typeof(CoverWallSetup);
		case UniversityGameplayField.SmartphoneFlash_flashSound:
			return typeof(SmartphoneFlash);
		case UniversityGameplayField.SubwayRideDirector_arriveConfig:
			return typeof(SubwayRideDirector);
		case UniversityGameplayField.TextMeshProUGUI_fontAsset:
		case UniversityGameplayField.TextMeshProUGUI_sharedMaterial:
			return typeof(TextMeshProUGUI);
		case UniversityGameplayField.BenchSeat_animations:
			return typeof(BenchSeat);
		case UniversityGameplayField.BenchInteractable_promptPrefab:
		case UniversityGameplayField.BenchInteractable_sitConfig:
		case UniversityGameplayField.BenchInteractable_standConfig:
			return typeof(BenchInteractable);
		default:
		{
			string text = "Unknown gameplay field: ";
			int num = (int)field;
			throw UniversityGameplayBinder.Invalid(text + num.ToString());
		}
		}
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002F674 File Offset: 0x0002D874
	public static Type ExpectedAssetType(UniversityGameplayField field)
	{
		switch (field)
		{
		case UniversityGameplayField.StreamedWorldPropSlot_viewPrefab:
			return typeof(GameObject);
		case UniversityGameplayField.BushConcealment_enterSound:
			return typeof(AudioClip);
		case UniversityGameplayField.VehicleSpawnPoint_spawnProfile:
			return typeof(VehicleSpawnProfile);
		case UniversityGameplayField.DumpsterHideInteractable_enterConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.DumpsterHideInteractable_hiddenHudConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.DumpsterHideInteractable_lidCloseSound:
			return typeof(AudioClip);
		case UniversityGameplayField.DumpsterHideInteractable_lidOpenSound:
			return typeof(AudioClip);
		case UniversityGameplayField.HideFlashPickup_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.HideFlashPickup_itemData:
			return typeof(HideFlashItemSO);
		case UniversityGameplayField.TrashcanLootInteractable_ejectSFX:
			return typeof(AudioClip);
		case UniversityGameplayField.TrashcanLootInteractable_lootBodyPaintDecal:
			return typeof(SkinDecal);
		case UniversityGameplayField.TrashcanLootInteractable_lootProfile:
			return typeof(TrashcanLootProfileSO);
		case UniversityGameplayField.TrashcanLootInteractable_progressUiPrefab:
			return typeof(TrashcanLootProgressUI);
		case UniversityGameplayField.TrashcanLootInteractable_promptConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.TrashcanLootInteractable_rummageHudConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.TrashcanLootInteractable_searchLoopSFX:
			return typeof(AudioClip);
		case UniversityGameplayField.WasteCanHideInteractable_hiddenHudConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.WantedPosterPickup_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.WantedPosterPickup_itemData:
			return typeof(InventoryItemSO);
		case UniversityGameplayField.LockerHideInteractable_doorCloseSound:
			return typeof(AudioClip);
		case UniversityGameplayField.LockerHideInteractable_doorOpenSound:
			return typeof(AudioClip);
		case UniversityGameplayField.LockerHideInteractable_enterConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.LockerHideInteractable_hiddenHudConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.LockerHideInteractable_lockerEnterAnimation:
			return typeof(AnimationClip);
		case UniversityGameplayField.LockerHideInteractable_lockerExitAnimation:
			return typeof(AnimationClip);
		case UniversityGameplayField.LockerHideInteractable_lockerIdleAnimation:
			return typeof(AnimationClip);
		case UniversityGameplayField.LockerHideInteractable_peeAudioClip:
			return typeof(AudioClip);
		case UniversityGameplayField.LockerHideInteractable_peeEffectPrefab:
			return typeof(GameObject);
		case UniversityGameplayField.Outfit_LinkedColorSets_linkedType:
			return typeof(OutfitType);
		case UniversityGameplayField.Outfit_OutfitIcon:
			return typeof(Sprite);
		case UniversityGameplayField.Outfit_Type:
			return typeof(OutfitType);
		case UniversityGameplayField.MudPuddleInteractable_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.Door_txt:
			return typeof(Text);
		case UniversityGameplayField.FootstepController_footstepClips:
			return typeof(AudioClip);
		case UniversityGameplayField.ClothingLootTableInteractable_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.ClothingLootTableInteractable_lootPool:
			return typeof(ClothingLootPoolSO);
		case UniversityGameplayField.ClothingLootTableInteractable_promptPrefab:
			return typeof(InteractablePromptUI);
		case UniversityGameplayField.NPCSenses_ReactionProfile:
			return typeof(NPCReactionProfileSO);
		case UniversityGameplayField.DumpsterHideInteractable_hideBodyPaintDecal:
			return typeof(SkinDecal);
		case UniversityGameplayField.VegetableTheftSource_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.VegetableTheftSource_promptPrefab:
			return typeof(InteractablePromptUI);
		case UniversityGameplayField.VegetableTheftSource_vegetables:
			return typeof(InsertableToyItemSO);
		case UniversityGameplayField.KombiniSlidingDoor_closeClip:
			return typeof(AudioClip);
		case UniversityGameplayField.KombiniSlidingDoor_openClip:
			return typeof(AudioClip);
		case UniversityGameplayField.NPCReactionAudioPlayer_audioProfile:
			return typeof(NPCReactionAudioProfileSO);
		case UniversityGameplayField.NPCWanderController_actionRegistry:
			return typeof(NPCActionRegistry);
		case UniversityGameplayField.NPCAlertHandler_actionRegistry:
			return typeof(NPCActionRegistry);
		case UniversityGameplayField.NPCAlertHandler_alertAnimationProfile:
			return typeof(AlertAnimationProfileSO);
		case UniversityGameplayField.StaticPhoneNPC_encounterController:
			return typeof(RuntimeAnimatorController);
		case UniversityGameplayField.PoiEmoteZone_profile:
			return typeof(PoiEmoteProfileSO);
		case UniversityGameplayField.StadiumShellEnvironment_daySignMaterial:
			return typeof(Material);
		case UniversityGameplayField.StadiumShellEnvironment_nightSignMaterial:
			return typeof(Material);
		case UniversityGameplayField.NPCEquipmentManager_actionRegistry:
			return typeof(NPCActionRegistry);
		case UniversityGameplayField.NPCEquipmentManager_sharedConfig:
			return typeof(NPCEquipmentConfig);
		case UniversityGameplayField.ShallowPoolWater_footstepSplash:
			return typeof(AudioClip);
		case UniversityGameplayField.ShallowPoolWater_footstepVariations:
			return typeof(AudioClip);
		case UniversityGameplayField.ShallowPoolWater_ripplePrefab:
			return typeof(GameObject);
		case UniversityGameplayField.ShallowPoolWater_splashPrefab:
			return typeof(GameObject);
		case UniversityGameplayField.CoverWallInteractable_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.CoverWallSetup_interactionConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.SmartphoneFlash_flashSound:
			return typeof(AudioClip);
		case UniversityGameplayField.SubwayRideDirector_arriveConfig:
			return typeof(InteractionConfigSO);
		case UniversityGameplayField.TextMeshProUGUI_fontAsset:
			return typeof(TMP_FontAsset);
		case UniversityGameplayField.TextMeshProUGUI_sharedMaterial:
			return typeof(Material);
		case UniversityGameplayField.BenchSeat_animations:
			return typeof(BenchAnimationSet);
		case UniversityGameplayField.BenchInteractable_promptPrefab:
			return typeof(InteractablePromptUI);
		case UniversityGameplayField.BenchInteractable_sitConfig:
		case UniversityGameplayField.BenchInteractable_standConfig:
			return typeof(InteractionConfigSO);
		default:
		{
			string text = "Unknown gameplay field: ";
			int num = (int)field;
			throw UniversityGameplayBinder.Invalid(text + num.ToString());
		}
		}
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0002FA98 File Offset: 0x0002DC98
	public static string SerializedFieldName(UniversityGameplayField field)
	{
		switch (field)
		{
		case UniversityGameplayField.StreamedWorldPropSlot_viewPrefab:
			return "viewPrefab";
		case UniversityGameplayField.BushConcealment_enterSound:
			return "enterSound";
		case UniversityGameplayField.VehicleSpawnPoint_spawnProfile:
			return "spawnProfile";
		case UniversityGameplayField.DumpsterHideInteractable_enterConfig:
			return "enterConfig";
		case UniversityGameplayField.DumpsterHideInteractable_hiddenHudConfig:
			return "hiddenHudConfig";
		case UniversityGameplayField.DumpsterHideInteractable_lidCloseSound:
			return "lidCloseSound";
		case UniversityGameplayField.DumpsterHideInteractable_lidOpenSound:
			return "lidOpenSound";
		case UniversityGameplayField.HideFlashPickup_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.HideFlashPickup_itemData:
			return "itemData";
		case UniversityGameplayField.TrashcanLootInteractable_ejectSFX:
			return "ejectSFX";
		case UniversityGameplayField.TrashcanLootInteractable_lootBodyPaintDecal:
			return "lootBodyPaintDecal";
		case UniversityGameplayField.TrashcanLootInteractable_lootProfile:
			return "lootProfile";
		case UniversityGameplayField.TrashcanLootInteractable_progressUiPrefab:
			return "progressUiPrefab";
		case UniversityGameplayField.TrashcanLootInteractable_promptConfig:
			return "promptConfig";
		case UniversityGameplayField.TrashcanLootInteractable_rummageHudConfig:
			return "rummageHudConfig";
		case UniversityGameplayField.TrashcanLootInteractable_searchLoopSFX:
			return "searchLoopSFX";
		case UniversityGameplayField.WasteCanHideInteractable_hiddenHudConfig:
			return "hiddenHudConfig";
		case UniversityGameplayField.WantedPosterPickup_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.WantedPosterPickup_itemData:
			return "itemData";
		case UniversityGameplayField.LockerHideInteractable_doorCloseSound:
			return "doorCloseSound";
		case UniversityGameplayField.LockerHideInteractable_doorOpenSound:
			return "doorOpenSound";
		case UniversityGameplayField.LockerHideInteractable_enterConfig:
			return "enterConfig";
		case UniversityGameplayField.LockerHideInteractable_hiddenHudConfig:
			return "hiddenHudConfig";
		case UniversityGameplayField.LockerHideInteractable_lockerEnterAnimation:
			return "lockerEnterAnimation";
		case UniversityGameplayField.LockerHideInteractable_lockerExitAnimation:
			return "lockerExitAnimation";
		case UniversityGameplayField.LockerHideInteractable_lockerIdleAnimation:
			return "lockerIdleAnimation";
		case UniversityGameplayField.LockerHideInteractable_peeAudioClip:
			return "peeAudioClip";
		case UniversityGameplayField.LockerHideInteractable_peeEffectPrefab:
			return "peeEffectPrefab";
		case UniversityGameplayField.Outfit_LinkedColorSets_linkedType:
			return "LinkedColorSets";
		case UniversityGameplayField.Outfit_OutfitIcon:
			return "OutfitIcon";
		case UniversityGameplayField.Outfit_Type:
			return "Type";
		case UniversityGameplayField.MudPuddleInteractable_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.Door_txt:
			return "txt";
		case UniversityGameplayField.FootstepController_footstepClips:
			return "footstepClips";
		case UniversityGameplayField.ClothingLootTableInteractable_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.ClothingLootTableInteractable_lootPool:
			return "lootPool";
		case UniversityGameplayField.ClothingLootTableInteractable_promptPrefab:
			return "promptPrefab";
		case UniversityGameplayField.NPCSenses_ReactionProfile:
			return "ReactionProfile";
		case UniversityGameplayField.DumpsterHideInteractable_hideBodyPaintDecal:
			return "hideBodyPaintDecal";
		case UniversityGameplayField.VegetableTheftSource_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.VegetableTheftSource_promptPrefab:
			return "promptPrefab";
		case UniversityGameplayField.VegetableTheftSource_vegetables:
			return "vegetables";
		case UniversityGameplayField.KombiniSlidingDoor_closeClip:
			return "closeClip";
		case UniversityGameplayField.KombiniSlidingDoor_openClip:
			return "openClip";
		case UniversityGameplayField.NPCReactionAudioPlayer_audioProfile:
			return "audioProfile";
		case UniversityGameplayField.NPCWanderController_actionRegistry:
			return "actionRegistry";
		case UniversityGameplayField.NPCAlertHandler_actionRegistry:
			return "actionRegistry";
		case UniversityGameplayField.NPCAlertHandler_alertAnimationProfile:
			return "alertAnimationProfile";
		case UniversityGameplayField.StaticPhoneNPC_encounterController:
			return "encounterController";
		case UniversityGameplayField.PoiEmoteZone_profile:
			return "profile";
		case UniversityGameplayField.StadiumShellEnvironment_daySignMaterial:
			return "daySignMaterial";
		case UniversityGameplayField.StadiumShellEnvironment_nightSignMaterial:
			return "nightSignMaterial";
		case UniversityGameplayField.NPCEquipmentManager_actionRegistry:
			return "actionRegistry";
		case UniversityGameplayField.NPCEquipmentManager_sharedConfig:
			return "sharedConfig";
		case UniversityGameplayField.ShallowPoolWater_footstepSplash:
			return "footstepSplash";
		case UniversityGameplayField.ShallowPoolWater_footstepVariations:
			return "footstepVariations";
		case UniversityGameplayField.ShallowPoolWater_ripplePrefab:
			return "ripplePrefab";
		case UniversityGameplayField.ShallowPoolWater_splashPrefab:
			return "splashPrefab";
		case UniversityGameplayField.CoverWallInteractable_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.CoverWallSetup_interactionConfig:
			return "interactionConfig";
		case UniversityGameplayField.SmartphoneFlash_flashSound:
			return "flashSound";
		case UniversityGameplayField.SubwayRideDirector_arriveConfig:
			return "arriveConfig";
		case UniversityGameplayField.TextMeshProUGUI_fontAsset:
			return "m_fontAsset";
		case UniversityGameplayField.TextMeshProUGUI_sharedMaterial:
			return "m_sharedMaterial";
		case UniversityGameplayField.BenchSeat_animations:
			return "animations";
		case UniversityGameplayField.BenchInteractable_promptPrefab:
			return "promptPrefab";
		case UniversityGameplayField.BenchInteractable_sitConfig:
			return "sitConfig";
		case UniversityGameplayField.BenchInteractable_standConfig:
			return "standConfig";
		default:
		{
			string text = "Unknown gameplay field: ";
			int num = (int)field;
			throw UniversityGameplayBinder.Invalid(text + num.ToString());
		}
		}
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0002FD74 File Offset: 0x0002DF74
	public static bool TryDescribeProperty(Type componentType, string serializedPropertyPath, out UniversityGameplayField field, out int elementIndex)
	{
		field = (UniversityGameplayField)0;
		elementIndex = -1;
		if (componentType == null || string.IsNullOrEmpty(serializedPropertyPath))
		{
			return false;
		}
		for (int i = 1; i <= 68; i++)
		{
			UniversityGameplayField universityGameplayField = (UniversityGameplayField)i;
			if (!(UniversityGameplayBinder.ExpectedComponentType(universityGameplayField) != componentType))
			{
				string text = UniversityGameplayBinder.SerializedFieldName(universityGameplayField);
				if (!UniversityGameplayBinder.IsIndexedField(universityGameplayField))
				{
					if (string.Equals(text, serializedPropertyPath, StringComparison.Ordinal))
					{
						field = universityGameplayField;
						return true;
					}
				}
				else
				{
					string text2 = text + ".Array.data[";
					string text3 = ((universityGameplayField == UniversityGameplayField.Outfit_LinkedColorSets_linkedType) ? "].linkedType" : "]");
					if (serializedPropertyPath.StartsWith(text2, StringComparison.Ordinal) && serializedPropertyPath.EndsWith(text3, StringComparison.Ordinal))
					{
						int num = serializedPropertyPath.Length - text2.Length - text3.Length;
						if (num > 0)
						{
							string text4 = serializedPropertyPath.Substring(text2.Length, num);
							int num2;
							if (int.TryParse(text4, NumberStyles.None, CultureInfo.InvariantCulture, out num2) && num2 >= 0 && !(num2.ToString(CultureInfo.InvariantCulture) != text4))
							{
								field = universityGameplayField;
								elementIndex = num2;
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002FE76 File Offset: 0x0002E076
	public static bool IsIndexedField(UniversityGameplayField field)
	{
		return field == UniversityGameplayField.Outfit_LinkedColorSets_linkedType || field == UniversityGameplayField.FootstepController_footstepClips || field == UniversityGameplayField.VegetableTheftSource_vegetables || field == UniversityGameplayField.ShallowPoolWater_footstepVariations;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002FE8E File Offset: 0x0002E08E
	public static bool IsFontBindingField(UniversityGameplayField field)
	{
		return field == UniversityGameplayField.TextMeshProUGUI_fontAsset || field == UniversityGameplayField.TextMeshProUGUI_sharedMaterial;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002FE9C File Offset: 0x0002E09C
	public static void ValidateSharedFontAssets(TMP_FontAsset font, Material material)
	{
		if (font == null || material == null || material.shader == null)
		{
			throw UniversityGameplayBinder.Invalid("A shared UGUI font binding requires both original font and material.");
		}
		if (font.atlasPopulationMode != AtlasPopulationMode.Static)
		{
			throw UniversityGameplayBinder.Invalid("Dynamic font atlases cannot enter the canonical UGUI font binding path.");
		}
		Texture2D[] atlasTextures = font.atlasTextures;
		if (atlasTextures == null || atlasTextures.Length == 0 || atlasTextures[0] == null || font.material == null || !material.HasProperty("_MainTex") || material.GetTexture("_MainTex") != atlasTextures[0])
		{
			throw UniversityGameplayBinder.Invalid("Shared UGUI font and material must refer to the same existing font atlas.");
		}
		bool flag = false;
		bool flag2 = false;
		if (font.characterTable != null)
		{
			foreach (TMP_Character tmp_Character in font.characterTable)
			{
				if (tmp_Character != null && (tmp_Character.unicode == 95U || tmp_Character.unicode == 8230U))
				{
					bool flag3 = false;
					if (font.glyphTable != null)
					{
						foreach (Glyph glyph in font.glyphTable)
						{
							if (glyph != null && glyph.index == tmp_Character.glyphIndex)
							{
								flag3 = true;
								break;
							}
						}
					}
					if (flag3 && tmp_Character.unicode == 95U)
					{
						flag = true;
					}
					if (flag3 && tmp_Character.unicode == 8230U)
					{
						flag2 = true;
					}
				}
			}
		}
		if (!flag || !flag2)
		{
			throw UniversityGameplayBinder.Invalid("Shared UGUI font must already contain the underscore and ellipsis glyphs.");
		}
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00030048 File Offset: 0x0002E248
	public static int GetElementCount(Component component, UniversityGameplayField field)
	{
		UniversityGameplayBinder.ValidateComponent(component, field);
		if (!UniversityGameplayBinder.IsIndexedField(field))
		{
			return -1;
		}
		Outfit outfit = component as Outfit;
		if (outfit != null)
		{
			return outfit.GetPoiGameplayElementCount(field);
		}
		FootstepController footstepController = component as FootstepController;
		if (footstepController != null)
		{
			return footstepController.GetPoiGameplayElementCount(field);
		}
		VegetableTheftSource vegetableTheftSource = component as VegetableTheftSource;
		if (vegetableTheftSource != null)
		{
			return vegetableTheftSource.GetPoiGameplayElementCount(field);
		}
		ShallowPoolWater shallowPoolWater = component as ShallowPoolWater;
		if (shallowPoolWater == null)
		{
			throw UniversityGameplayBinder.Invalid("Unsupported indexed gameplay component.");
		}
		return shallowPoolWater.GetPoiGameplayElementCount(field);
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x000300BC File Offset: 0x0002E2BC
	public static string SerializedPropertyPath(UniversityGameplayField field, int elementIndex = -1)
	{
		string text = UniversityGameplayBinder.SerializedFieldName(field);
		if (!UniversityGameplayBinder.IsIndexedField(field))
		{
			if (elementIndex != -1)
			{
				throw UniversityGameplayBinder.Invalid("Scalar binding cannot have an element index.");
			}
			return text;
		}
		else
		{
			if (elementIndex < 0)
			{
				throw UniversityGameplayBinder.Invalid("Indexed binding requires a non-negative index.");
			}
			return string.Concat(new string[]
			{
				text,
				".Array.data[",
				elementIndex.ToString(CultureInfo.InvariantCulture),
				"]",
				(field == UniversityGameplayField.Outfit_LinkedColorSets_linkedType) ? ".linkedType" : string.Empty
			});
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0003013C File Offset: 0x0002E33C
	private static void ValidateElementAddress(Component component, UniversityGameplayField field, int elementIndex)
	{
		if (!UniversityGameplayBinder.IsIndexedField(field))
		{
			if (elementIndex != -1)
			{
				throw UniversityGameplayBinder.Invalid("Scalar binding cannot have an element index: " + field.ToString());
			}
			return;
		}
		else
		{
			int elementCount = UniversityGameplayBinder.GetElementCount(component, field);
			if (elementIndex < 0 || elementIndex >= elementCount)
			{
				throw UniversityGameplayBinder.Invalid("Gameplay collection index is out of range: " + field.ToString());
			}
			return;
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x000301A0 File Offset: 0x0002E3A0
	public static Object ReadReference(Component component, UniversityGameplayField field, int elementIndex = -1)
	{
		UniversityGameplayBinder.ValidateComponent(component, field);
		UniversityGameplayBinder.ValidateElementAddress(component, field, elementIndex);
		TextMeshProUGUI textMeshProUGUI = component as TextMeshProUGUI;
		if (textMeshProUGUI == null)
		{
			BenchSeat benchSeat = component as BenchSeat;
			if (benchSeat != null)
			{
				return benchSeat.animations;
			}
			BenchInteractable benchInteractable = component as BenchInteractable;
			if (benchInteractable != null)
			{
				return benchInteractable.ReadPoiGameplayReference(field, elementIndex);
			}
			StreamedWorldPropSlot streamedWorldPropSlot = component as StreamedWorldPropSlot;
			if (streamedWorldPropSlot != null)
			{
				return streamedWorldPropSlot.ReadUniversityGameplayReference(field);
			}
			BushConcealment bushConcealment = component as BushConcealment;
			if (bushConcealment != null)
			{
				return bushConcealment.ReadUniversityGameplayReference(field);
			}
			VehicleSpawnPoint vehicleSpawnPoint = component as VehicleSpawnPoint;
			if (vehicleSpawnPoint != null)
			{
				return vehicleSpawnPoint.ReadUniversityGameplayReference(field);
			}
			DumpsterHideInteractable dumpsterHideInteractable = component as DumpsterHideInteractable;
			if (dumpsterHideInteractable != null)
			{
				return dumpsterHideInteractable.ReadUniversityGameplayReference(field);
			}
			HideFlashPickup hideFlashPickup = component as HideFlashPickup;
			if (hideFlashPickup != null)
			{
				return hideFlashPickup.ReadUniversityGameplayReference(field);
			}
			TrashcanLootInteractable trashcanLootInteractable = component as TrashcanLootInteractable;
			if (trashcanLootInteractable != null)
			{
				return trashcanLootInteractable.ReadUniversityGameplayReference(field);
			}
			WasteCanHideInteractable wasteCanHideInteractable = component as WasteCanHideInteractable;
			if (wasteCanHideInteractable != null)
			{
				return wasteCanHideInteractable.ReadUniversityGameplayReference(field);
			}
			WantedPosterPickup wantedPosterPickup = component as WantedPosterPickup;
			if (wantedPosterPickup != null)
			{
				return wantedPosterPickup.ReadUniversityGameplayReference(field);
			}
			LockerHideInteractable lockerHideInteractable = component as LockerHideInteractable;
			if (lockerHideInteractable != null)
			{
				return lockerHideInteractable.ReadUniversityGameplayReference(field);
			}
			Outfit outfit = component as Outfit;
			if (outfit != null)
			{
				return outfit.ReadPoiGameplayReference(field, elementIndex);
			}
			MudPuddleInteractable mudPuddleInteractable = component as MudPuddleInteractable;
			if (mudPuddleInteractable != null)
			{
				return mudPuddleInteractable.ReadPoiGameplayReference(field, elementIndex);
			}
			Door door = component as Door;
			if (door != null)
			{
				return door.ReadPoiGameplayReference(field, elementIndex);
			}
			FootstepController footstepController = component as FootstepController;
			if (footstepController != null)
			{
				return footstepController.ReadPoiGameplayReference(field, elementIndex);
			}
			ClothingLootTableInteractable clothingLootTableInteractable = component as ClothingLootTableInteractable;
			if (clothingLootTableInteractable != null)
			{
				return clothingLootTableInteractable.ReadPoiGameplayReference(field, elementIndex);
			}
			NPCSenses npcsenses = component as NPCSenses;
			if (npcsenses != null)
			{
				return npcsenses.ReadPoiGameplayReference(field, elementIndex);
			}
			VegetableTheftSource vegetableTheftSource = component as VegetableTheftSource;
			if (vegetableTheftSource != null)
			{
				return vegetableTheftSource.ReadPoiGameplayReference(field, elementIndex);
			}
			KombiniSlidingDoor kombiniSlidingDoor = component as KombiniSlidingDoor;
			if (kombiniSlidingDoor != null)
			{
				return kombiniSlidingDoor.ReadPoiGameplayReference(field, elementIndex);
			}
			NPCReactionAudioPlayer npcreactionAudioPlayer = component as NPCReactionAudioPlayer;
			if (npcreactionAudioPlayer != null)
			{
				return npcreactionAudioPlayer.ReadPoiGameplayReference(field, elementIndex);
			}
			NPCWanderController npcwanderController = component as NPCWanderController;
			if (npcwanderController != null)
			{
				return npcwanderController.ReadPoiGameplayReference(field, elementIndex);
			}
			NPCAlertHandler npcalertHandler = component as NPCAlertHandler;
			if (npcalertHandler != null)
			{
				return npcalertHandler.ReadPoiGameplayReference(field, elementIndex);
			}
			StaticPhoneNPC staticPhoneNPC = component as StaticPhoneNPC;
			if (staticPhoneNPC != null)
			{
				return staticPhoneNPC.ReadPoiGameplayReference(field, elementIndex);
			}
			PoiEmoteZone poiEmoteZone = component as PoiEmoteZone;
			if (poiEmoteZone != null)
			{
				return poiEmoteZone.ReadPoiGameplayReference(field, elementIndex);
			}
			StadiumShellEnvironment stadiumShellEnvironment = component as StadiumShellEnvironment;
			if (stadiumShellEnvironment != null)
			{
				return stadiumShellEnvironment.ReadPoiGameplayReference(field, elementIndex);
			}
			NPCEquipmentManager npcequipmentManager = component as NPCEquipmentManager;
			if (npcequipmentManager != null)
			{
				return npcequipmentManager.ReadPoiGameplayReference(field, elementIndex);
			}
			ShallowPoolWater shallowPoolWater = component as ShallowPoolWater;
			if (shallowPoolWater != null)
			{
				return shallowPoolWater.ReadPoiGameplayReference(field, elementIndex);
			}
			CoverWallInteractable coverWallInteractable = component as CoverWallInteractable;
			if (coverWallInteractable != null)
			{
				return coverWallInteractable.ReadPoiGameplayReference(field, elementIndex);
			}
			CoverWallSetup coverWallSetup = component as CoverWallSetup;
			if (coverWallSetup != null)
			{
				return coverWallSetup.ReadPoiGameplayReference(field, elementIndex);
			}
			SmartphoneFlash smartphoneFlash = component as SmartphoneFlash;
			if (smartphoneFlash != null)
			{
				return smartphoneFlash.ReadPoiGameplayReference(field, elementIndex);
			}
			SubwayRideDirector subwayRideDirector = component as SubwayRideDirector;
			if (subwayRideDirector == null)
			{
				throw UniversityGameplayBinder.Invalid("Unsupported gameplay component.");
			}
			return subwayRideDirector.ReadPoiGameplayReference(field, elementIndex);
		}
		else
		{
			if (field != UniversityGameplayField.TextMeshProUGUI_fontAsset)
			{
				return textMeshProUGUI.fontSharedMaterial;
			}
			return textMeshProUGUI.font;
		}
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x000304E0 File Offset: 0x0002E6E0
	public static void WriteReference(Component component, UniversityGameplayField field, Object asset, int elementIndex = -1)
	{
		UniversityGameplayBinder.ValidateComponent(component, field);
		UniversityGameplayBinder.ValidateElementAddress(component, field, elementIndex);
		if (asset != null && !UniversityGameplayBinder.ExpectedAssetType(field).IsInstanceOfType(asset))
		{
			throw UniversityGameplayBinder.Invalid("Asset type does not match " + field.ToString() + ".");
		}
		if (UniversityGameplayBinder.IsFontBindingField(field))
		{
			throw UniversityGameplayBinder.Invalid("UGUI font and material must be applied together after complete binding validation.");
		}
		BenchSeat benchSeat = component as BenchSeat;
		if (benchSeat != null)
		{
			benchSeat.animations = (BenchAnimationSet)asset;
			return;
		}
		BenchInteractable benchInteractable = component as BenchInteractable;
		if (benchInteractable != null)
		{
			benchInteractable.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		StreamedWorldPropSlot streamedWorldPropSlot = component as StreamedWorldPropSlot;
		if (streamedWorldPropSlot != null)
		{
			streamedWorldPropSlot.WriteUniversityGameplayReference(field, asset);
			return;
		}
		BushConcealment bushConcealment = component as BushConcealment;
		if (bushConcealment != null)
		{
			bushConcealment.WriteUniversityGameplayReference(field, asset);
			return;
		}
		VehicleSpawnPoint vehicleSpawnPoint = component as VehicleSpawnPoint;
		if (vehicleSpawnPoint != null)
		{
			vehicleSpawnPoint.WriteUniversityGameplayReference(field, asset);
			return;
		}
		DumpsterHideInteractable dumpsterHideInteractable = component as DumpsterHideInteractable;
		if (dumpsterHideInteractable != null)
		{
			dumpsterHideInteractable.WriteUniversityGameplayReference(field, asset);
			return;
		}
		HideFlashPickup hideFlashPickup = component as HideFlashPickup;
		if (hideFlashPickup != null)
		{
			hideFlashPickup.WriteUniversityGameplayReference(field, asset);
			return;
		}
		TrashcanLootInteractable trashcanLootInteractable = component as TrashcanLootInteractable;
		if (trashcanLootInteractable != null)
		{
			trashcanLootInteractable.WriteUniversityGameplayReference(field, asset);
			return;
		}
		WasteCanHideInteractable wasteCanHideInteractable = component as WasteCanHideInteractable;
		if (wasteCanHideInteractable != null)
		{
			wasteCanHideInteractable.WriteUniversityGameplayReference(field, asset);
			return;
		}
		WantedPosterPickup wantedPosterPickup = component as WantedPosterPickup;
		if (wantedPosterPickup != null)
		{
			wantedPosterPickup.WriteUniversityGameplayReference(field, asset);
			return;
		}
		LockerHideInteractable lockerHideInteractable = component as LockerHideInteractable;
		if (lockerHideInteractable != null)
		{
			lockerHideInteractable.WriteUniversityGameplayReference(field, asset);
			return;
		}
		Outfit outfit = component as Outfit;
		if (outfit != null)
		{
			outfit.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		MudPuddleInteractable mudPuddleInteractable = component as MudPuddleInteractable;
		if (mudPuddleInteractable != null)
		{
			mudPuddleInteractable.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		Door door = component as Door;
		if (door != null)
		{
			door.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		FootstepController footstepController = component as FootstepController;
		if (footstepController != null)
		{
			footstepController.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		ClothingLootTableInteractable clothingLootTableInteractable = component as ClothingLootTableInteractable;
		if (clothingLootTableInteractable != null)
		{
			clothingLootTableInteractable.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		NPCSenses npcsenses = component as NPCSenses;
		if (npcsenses != null)
		{
			npcsenses.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		VegetableTheftSource vegetableTheftSource = component as VegetableTheftSource;
		if (vegetableTheftSource != null)
		{
			vegetableTheftSource.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		KombiniSlidingDoor kombiniSlidingDoor = component as KombiniSlidingDoor;
		if (kombiniSlidingDoor != null)
		{
			kombiniSlidingDoor.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		NPCReactionAudioPlayer npcreactionAudioPlayer = component as NPCReactionAudioPlayer;
		if (npcreactionAudioPlayer != null)
		{
			npcreactionAudioPlayer.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		NPCWanderController npcwanderController = component as NPCWanderController;
		if (npcwanderController != null)
		{
			npcwanderController.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		NPCAlertHandler npcalertHandler = component as NPCAlertHandler;
		if (npcalertHandler != null)
		{
			npcalertHandler.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		StaticPhoneNPC staticPhoneNPC = component as StaticPhoneNPC;
		if (staticPhoneNPC != null)
		{
			staticPhoneNPC.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		PoiEmoteZone poiEmoteZone = component as PoiEmoteZone;
		if (poiEmoteZone != null)
		{
			poiEmoteZone.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		StadiumShellEnvironment stadiumShellEnvironment = component as StadiumShellEnvironment;
		if (stadiumShellEnvironment != null)
		{
			stadiumShellEnvironment.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		NPCEquipmentManager npcequipmentManager = component as NPCEquipmentManager;
		if (npcequipmentManager != null)
		{
			npcequipmentManager.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		ShallowPoolWater shallowPoolWater = component as ShallowPoolWater;
		if (shallowPoolWater != null)
		{
			shallowPoolWater.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		CoverWallInteractable coverWallInteractable = component as CoverWallInteractable;
		if (coverWallInteractable != null)
		{
			coverWallInteractable.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		CoverWallSetup coverWallSetup = component as CoverWallSetup;
		if (coverWallSetup != null)
		{
			coverWallSetup.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		SmartphoneFlash smartphoneFlash = component as SmartphoneFlash;
		if (smartphoneFlash != null)
		{
			smartphoneFlash.WritePoiGameplayReference(field, asset, elementIndex);
			return;
		}
		SubwayRideDirector subwayRideDirector = component as SubwayRideDirector;
		if (subwayRideDirector == null)
		{
			throw UniversityGameplayBinder.Invalid("Unsupported gameplay component.");
		}
		subwayRideDirector.WritePoiGameplayReference(field, asset, elementIndex);
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0003086C File Offset: 0x0002EA6C
	public static void ValidateAndApply(GameObject inactivePayload, UniversityGameplayManifest manifest)
	{
		if (inactivePayload == null)
		{
			throw UniversityGameplayBinder.Invalid("Payload is missing.");
		}
		if (inactivePayload.activeInHierarchy)
		{
			throw UniversityGameplayBinder.Invalid("Payload must remain inactive while binding.");
		}
		if (manifest == null)
		{
			throw UniversityGameplayBinder.Invalid("Gameplay manifest is missing.");
		}
		if (string.IsNullOrWhiteSpace(manifest.variant) || string.IsNullOrWhiteSpace(manifest.bindingSetId))
		{
			throw UniversityGameplayBinder.Invalid("Gameplay manifest identity is empty.");
		}
		UniversityResidencyPayloadTag[] components = inactivePayload.GetComponents<UniversityResidencyPayloadTag>();
		if (components.Length != 1)
		{
			throw UniversityGameplayBinder.Invalid("Expected one payload tag on the payload root.");
		}
		UniversityResidencyPayloadTag universityResidencyPayloadTag = components[0];
		if (universityResidencyPayloadTag.formatVersion != 2)
		{
			throw UniversityGameplayBinder.Invalid("Unsupported payload binding format: " + universityResidencyPayloadTag.formatVersion.ToString());
		}
		if (!string.Equals(universityResidencyPayloadTag.variant, manifest.variant, StringComparison.Ordinal) || !string.Equals(universityResidencyPayloadTag.bindingSetId, manifest.bindingSetId, StringComparison.Ordinal))
		{
			throw UniversityGameplayBinder.Invalid("Payload and gameplay manifest identities do not match.");
		}
		string text = (string.IsNullOrEmpty(universityResidencyPayloadTag.family) ? "university" : universityResidencyPayloadTag.family);
		string text2 = (string.IsNullOrEmpty(manifest.family) ? "university" : manifest.family);
		if (string.IsNullOrWhiteSpace(text) || !string.Equals(text, text2, StringComparison.Ordinal))
		{
			throw UniversityGameplayBinder.Invalid("Payload and gameplay manifest families do not match.");
		}
		UniversityGameplayBinding[] records = manifest.records;
		UniversityMaterialBinding[] materialRecords = manifest.materialRecords;
		if (records == null || materialRecords == null)
		{
			throw UniversityGameplayBinder.Invalid("Manifest binding arrays must not be null.");
		}
		if (records.Length == 0 && materialRecords.Length == 0)
		{
			throw UniversityGameplayBinder.Invalid("Gameplay manifest has no bindings.");
		}
		Component[] array = new Component[records.Length];
		int[] array2 = new int[records.Length];
		HashSet<ValueTuple<int, UniversityGameplayField, int>> hashSet = new HashSet<ValueTuple<int, UniversityGameplayField, int>>();
		for (int i = 0; i < records.Length; i++)
		{
			UniversityGameplayBinding universityGameplayBinding = records[i];
			if (universityGameplayBinding == null)
			{
				throw UniversityGameplayBinder.Invalid("Null gameplay binding at index " + i.ToString() + ".");
			}
			Type type = UniversityGameplayBinder.ExpectedComponentType(universityGameplayBinding.field);
			Type type2 = UniversityGameplayBinder.ExpectedAssetType(universityGameplayBinding.field);
			if (universityGameplayBinding.asset == null || !type2.IsInstanceOfType(universityGameplayBinding.asset))
			{
				throw UniversityGameplayBinder.Invalid("Missing or incompatible original asset for " + universityGameplayBinding.field.ToString() + ".");
			}
			if (universityGameplayBinding.siblingPath == null)
			{
				throw UniversityGameplayBinder.Invalid("Null hierarchy path for " + universityGameplayBinding.field.ToString() + ".");
			}
			Transform transform = inactivePayload.transform;
			foreach (int num in universityGameplayBinding.siblingPath)
			{
				if (num < 0 || num >= transform.childCount)
				{
					throw UniversityGameplayBinder.Invalid("Hierarchy path is out of range for " + universityGameplayBinding.field.ToString() + ".");
				}
				transform = transform.GetChild(num);
			}
			Component[] components2 = transform.GetComponents<Component>();
			for (int k = 0; k < components2.Length; k++)
			{
				if (components2[k] == null)
				{
					throw UniversityGameplayBinder.Invalid("Missing script on a gameplay binding target.");
				}
			}
			if (universityGameplayBinding.componentIndex < 0 || universityGameplayBinding.componentIndex >= components2.Length)
			{
				throw UniversityGameplayBinder.Invalid("Component index is out of range for " + universityGameplayBinding.field.ToString() + ".");
			}
			Component component = components2[universityGameplayBinding.componentIndex];
			if (component.GetType() != type)
			{
				throw UniversityGameplayBinder.Invalid("Target component type does not match " + universityGameplayBinding.field.ToString() + ".");
			}
			bool flag = universityGameplayBinding.field >= UniversityGameplayField.StreamedWorldPropSlot_viewPrefab && universityGameplayBinding.field <= UniversityGameplayField.LockerHideInteractable_peeEffectPrefab && universityGameplayBinding.elementIndex == 0 && universityGameplayBinding.expectedElementCount == 0;
			int num2 = (flag ? (-1) : universityGameplayBinding.elementIndex);
			int num3 = (flag ? (-1) : universityGameplayBinding.expectedElementCount);
			UniversityGameplayBinder.ValidateElementAddress(component, universityGameplayBinding.field, num2);
			if (num3 != UniversityGameplayBinder.GetElementCount(component, universityGameplayBinding.field))
			{
				throw UniversityGameplayBinder.Invalid("Gameplay collection shape does not match: " + universityGameplayBinding.field.ToString());
			}
			ValueTuple<int, UniversityGameplayField, int> valueTuple = new ValueTuple<int, UniversityGameplayField, int>(component.GetInstanceID(), universityGameplayBinding.field, num2);
			if (!hashSet.Add(valueTuple))
			{
				throw UniversityGameplayBinder.Invalid("Duplicate gameplay binding for " + universityGameplayBinding.field.ToString() + ".");
			}
			if (UniversityGameplayBinder.ReadReference(component, universityGameplayBinding.field, num2) != null)
			{
				throw UniversityGameplayBinder.Invalid("Gameplay binding would replace an authored reference: " + universityGameplayBinding.field.ToString() + ".");
			}
			array[i] = component;
			array2[i] = num2;
		}
		Dictionary<Renderer, Material[]> dictionary = new Dictionary<Renderer, Material[]>();
		HashSet<ulong> hashSet2 = new HashSet<ulong>();
		for (int l = 0; l < materialRecords.Length; l++)
		{
			UniversityMaterialBinding universityMaterialBinding = materialRecords[l];
			if (universityMaterialBinding == null)
			{
				throw UniversityGameplayBinder.Invalid("Null material binding at index " + l.ToString() + ".");
			}
			if (universityMaterialBinding.material == null)
			{
				throw UniversityGameplayBinder.Invalid("Original shared material is missing at index " + l.ToString() + ".");
			}
			if (universityMaterialBinding.siblingPath == null)
			{
				throw UniversityGameplayBinder.Invalid("Null material hierarchy path at index " + l.ToString() + ".");
			}
			Transform transform2 = inactivePayload.transform;
			foreach (int num4 in universityMaterialBinding.siblingPath)
			{
				if (num4 < 0 || num4 >= transform2.childCount)
				{
					throw UniversityGameplayBinder.Invalid("Material hierarchy path is out of range at index " + l.ToString() + ".");
				}
				transform2 = transform2.GetChild(num4);
			}
			Component[] components3 = transform2.GetComponents<Component>();
			for (int m = 0; m < components3.Length; m++)
			{
				if (components3[m] == null)
				{
					throw UniversityGameplayBinder.Invalid("Missing script on a material binding target.");
				}
			}
			if (universityMaterialBinding.componentIndex < 0 || universityMaterialBinding.componentIndex >= components3.Length)
			{
				throw UniversityGameplayBinder.Invalid("Material component index is out of range at index " + l.ToString() + ".");
			}
			Component component2 = components3[universityMaterialBinding.componentIndex];
			if (component2.GetType() != typeof(MeshRenderer) && component2.GetType() != typeof(ParticleSystemRenderer) && component2.GetType() != typeof(SkinnedMeshRenderer))
			{
				throw UniversityGameplayBinder.Invalid("Material binding requires a MeshRenderer, ParticleSystemRenderer or SkinnedMeshRenderer.");
			}
			Renderer renderer = (Renderer)component2;
			Material[] sharedMaterials;
			if (!dictionary.TryGetValue(renderer, out sharedMaterials))
			{
				sharedMaterials = renderer.sharedMaterials;
				dictionary.Add(renderer, sharedMaterials);
			}
			if (universityMaterialBinding.slotCount != sharedMaterials.Length || universityMaterialBinding.slotCount <= 0)
			{
				throw UniversityGameplayBinder.Invalid("Shared material slot count does not match at index " + l.ToString() + ".");
			}
			if (universityMaterialBinding.slotIndex < 0 || universityMaterialBinding.slotIndex >= sharedMaterials.Length)
			{
				throw UniversityGameplayBinder.Invalid("Shared material slot index is out of range at index " + l.ToString() + ".");
			}
			ulong num5 = ((ulong)renderer.GetInstanceID() << 32) | (ulong)universityMaterialBinding.slotIndex;
			if (!hashSet2.Add(num5))
			{
				throw UniversityGameplayBinder.Invalid("Duplicate shared material slot binding.");
			}
			if (sharedMaterials[universityMaterialBinding.slotIndex] != null)
			{
				throw UniversityGameplayBinder.Invalid("Material binding would replace an authored reference at index " + l.ToString() + ".");
			}
			sharedMaterials[universityMaterialBinding.slotIndex] = universityMaterialBinding.material;
		}
		Dictionary<TextMeshProUGUI, UniversityGameplayBinder.PendingFontBinding> dictionary2 = new Dictionary<TextMeshProUGUI, UniversityGameplayBinder.PendingFontBinding>();
		for (int n = 0; n < records.Length; n++)
		{
			if (UniversityGameplayBinder.IsFontBindingField(records[n].field))
			{
				TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI)array[n];
				UniversityGameplayBinder.PendingFontBinding pendingFontBinding;
				if (!dictionary2.TryGetValue(textMeshProUGUI, out pendingFontBinding))
				{
					pendingFontBinding = new UniversityGameplayBinder.PendingFontBinding();
					dictionary2.Add(textMeshProUGUI, pendingFontBinding);
				}
				if (records[n].field == UniversityGameplayField.TextMeshProUGUI_fontAsset)
				{
					pendingFontBinding.font = (TMP_FontAsset)records[n].asset;
				}
				else
				{
					pendingFontBinding.material = (Material)records[n].asset;
				}
			}
		}
		foreach (UniversityGameplayBinder.PendingFontBinding pendingFontBinding2 in dictionary2.Values)
		{
			UniversityGameplayBinder.ValidateSharedFontAssets(pendingFontBinding2.font, pendingFontBinding2.material);
		}
		for (int num6 = 0; num6 < records.Length; num6++)
		{
			if (!UniversityGameplayBinder.IsFontBindingField(records[num6].field))
			{
				UniversityGameplayBinder.WriteReference(array[num6], records[num6].field, records[num6].asset, array2[num6]);
			}
		}
		foreach (KeyValuePair<Renderer, Material[]> keyValuePair in dictionary)
		{
			keyValuePair.Key.sharedMaterials = keyValuePair.Value;
		}
		foreach (KeyValuePair<TextMeshProUGUI, UniversityGameplayBinder.PendingFontBinding> keyValuePair2 in dictionary2)
		{
			keyValuePair2.Key.fontSharedMaterial = keyValuePair2.Value.material;
			keyValuePair2.Key.font = keyValuePair2.Value.font;
		}
		for (int num7 = 0; num7 < records.Length; num7++)
		{
			if (records[num7].field == UniversityGameplayField.StreamedWorldPropSlot_viewPrefab)
			{
				((StreamedWorldPropSlot)array[num7]).MarkCanonicalGameplayView((GameObject)records[num7].asset);
			}
		}
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x000311D8 File Offset: 0x0002F3D8
	private static void ValidateComponent(Component component, UniversityGameplayField field)
	{
		if (component == null || component.GetType() != UniversityGameplayBinder.ExpectedComponentType(field))
		{
			throw UniversityGameplayBinder.Invalid("Target component type does not match " + field.ToString() + ".");
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00031218 File Offset: 0x0002F418
	private static InvalidOperationException Invalid(string message)
	{
		return new InvalidOperationException("[UniversityGameplayBinder] " + message);
	}

	// Token: 0x020000AA RID: 170
	private sealed class PendingFontBinding
	{
		// Token: 0x040005A0 RID: 1440
		internal TMP_FontAsset font;

		// Token: 0x040005A1 RID: 1441
		internal Material material;
	}
}
