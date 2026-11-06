using System;
using System.Collections.Generic;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B02 RID: 2818
	public class CustomizationSceneSetupValidator : MonoBehaviour
	{
		// Token: 0x06004FC4 RID: 20420 RVA: 0x001EE4A0 File Offset: 0x001EC6A0
		[ContextMenu("Validate Setup")]
		public void ValidateSetup()
		{
			List<string> list = new List<string>();
			if (this.coreOrchestrator == null)
			{
				list.Add("Missing CustomizationCoreOrchestrator reference.");
			}
			if (this.bootstrapper == null)
			{
				list.Add("Missing CustomizationSceneBootstrapper reference.");
			}
			if (this.hubController == null)
			{
				list.Add("Missing CustomizationHubController reference.");
			}
			if (this.editorViewRouter == null)
			{
				list.Add("Missing CustomizationEditorViewRouter reference.");
			}
			if (this.presetCoordinator == null)
			{
				list.Add("Missing CustomizationPresetCoordinator reference.");
			}
			if (this.presetRepository == null)
			{
				list.Add("Missing CustomizationPresetRepository reference.");
			}
			if (this.presetListController == null)
			{
				list.Add("Missing CustomizationHubPresetListController reference.");
			}
			else if (this.presetListController.SystemPresetConfig == null)
			{
				list.Add("CustomizationHubPresetListController is missing SystemPresetConfig SO reference.");
			}
			if (this.hubController != null)
			{
				if (!this.hubController.HasHubRoot)
				{
					list.Add("CustomizationHubController is missing Hub Root binding.");
				}
				if (!this.hubController.HasMainCharacterTabRoot)
				{
					list.Add("CustomizationHubController is missing Main Character Tab Root binding.");
				}
				if (!this.hubController.HasNpcTabRoot)
				{
					list.Add("CustomizationHubController is missing NPC Tab Root binding.");
				}
				if (!this.hubController.HasEditorViewRouter)
				{
					list.Add("CustomizationHubController is missing Editor View Router binding.");
				}
			}
			if (this.editorViewRouter != null)
			{
				if (!this.editorViewRouter.HasSharedCanvasRoot)
				{
					list.Add("CustomizationEditorViewRouter is missing Shared Canvas Root binding.");
				}
				if (this.editorViewRouter.UsesCommonEditorRoot && !this.editorViewRouter.HasCommonEditorRoot)
				{
					list.Add("CustomizationEditorViewRouter is missing Common Editor Root binding.");
				}
				if (!this.editorViewRouter.HasMainCharacterEditorRoot)
				{
					list.Add("CustomizationEditorViewRouter is missing Main Character Editor Root binding.");
				}
				if (!this.editorViewRouter.HasNpcEditorRoot)
				{
					list.Add("CustomizationEditorViewRouter is missing NPC Editor Root binding.");
				}
			}
			MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>(true);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			foreach (MonoBehaviour monoBehaviour in componentsInChildren)
			{
				if (monoBehaviour is ICharacterCustomizationBackend)
				{
					flag3 = true;
				}
				PrefabCustomizationSpawnProvider prefabCustomizationSpawnProvider = monoBehaviour as PrefabCustomizationSpawnProvider;
				if (prefabCustomizationSpawnProvider != null)
				{
					if (prefabCustomizationSpawnProvider.TargetType == CustomizationTargetType.MainCharacter)
					{
						flag = true;
					}
					if (prefabCustomizationSpawnProvider.TargetType == CustomizationTargetType.Npc)
					{
						flag2 = true;
					}
					if (!prefabCustomizationSpawnProvider.HasValidPrefab)
					{
						flag4 = true;
					}
				}
			}
			if (!flag3)
			{
				list.Add("No ICharacterCustomizationBackend found in children.");
			}
			if (!flag)
			{
				list.Add("Main character spawn provider not detected.");
			}
			if (!flag2)
			{
				list.Add("NPC spawn provider not detected.");
			}
			if (flag4)
			{
				list.Add("At least one PrefabCustomizationSpawnProvider has no valid target configured (pre-spawn target list or legacy prefab fallback).");
			}
			if (list.Count == 0)
			{
				Debug.Log("[CustomizationSetupValidator] Setup looks valid.");
				return;
			}
			for (int j = 0; j < list.Count; j++)
			{
				Debug.LogWarning("[CustomizationSetupValidator] " + list[j]);
			}
		}

		// Token: 0x040050BC RID: 20668
		[Header("Core")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x040050BD RID: 20669
		[SerializeField]
		private CustomizationSceneBootstrapper bootstrapper;

		// Token: 0x040050BE RID: 20670
		[SerializeField]
		private CustomizationHubController hubController;

		// Token: 0x040050BF RID: 20671
		[SerializeField]
		private CustomizationEditorViewRouter editorViewRouter;

		// Token: 0x040050C0 RID: 20672
		[Header("Presets")]
		[SerializeField]
		private CustomizationPresetCoordinator presetCoordinator;

		// Token: 0x040050C1 RID: 20673
		[SerializeField]
		private CustomizationPresetRepository presetRepository;

		// Token: 0x040050C2 RID: 20674
		[SerializeField]
		private CustomizationHubPresetListController presetListController;
	}
}
