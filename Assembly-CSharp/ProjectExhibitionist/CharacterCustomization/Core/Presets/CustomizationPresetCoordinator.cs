using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B18 RID: 2840
	public class CustomizationPresetCoordinator : MonoBehaviour
	{
		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06005058 RID: 20568 RVA: 0x001F1AC6 File Offset: 0x001EFCC6
		public CustomizationPresetData ActivePreset
		{
			get
			{
				return this._activePreset;
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001F1ACE File Offset: 0x001EFCCE
		private void OnEnable()
		{
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized += this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed += this.HandleSessionClosed;
			}
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001F1B0C File Offset: 0x001EFD0C
		private void OnDisable()
		{
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed -= this.HandleSessionClosed;
			}
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001F1B4C File Offset: 0x001EFD4C
		public List<CustomizationPresetData> GetPresetsForCurrentSession()
		{
			if (this.coreOrchestrator == null || this.coreOrchestrator.CurrentSession == null || this.presetRepository == null)
			{
				return new List<CustomizationPresetData>();
			}
			CustomizationSessionContext currentSession = this.coreOrchestrator.CurrentSession;
			return this.presetRepository.GetPresets(currentSession.characterId, currentSession.targetType, null);
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001F1BAC File Offset: 0x001EFDAC
		public List<CustomizationPresetData> GetPresets(string characterId, CustomizationTargetType targetType, CharacterStateData defaultStateData = null)
		{
			if (this.presetRepository == null || string.IsNullOrWhiteSpace(characterId) || targetType == CustomizationTargetType.None)
			{
				return new List<CustomizationPresetData>();
			}
			return this.presetRepository.GetPresets(characterId, targetType, defaultStateData);
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x001F1BDC File Offset: 0x001EFDDC
		public bool TrySetActivePreset(string presetId, bool applyToBackend)
		{
			if (this.coreOrchestrator == null || this.coreOrchestrator.CurrentSession == null || this.presetRepository == null)
			{
				return false;
			}
			CustomizationPresetData customizationPresetData = CustomizationPresetCoordinator.FindPreset(this.GetPresetsForCurrentSession(), presetId);
			if (customizationPresetData == null)
			{
				return false;
			}
			if (!this.presetRepository.TrySetActivePreset(customizationPresetData.presetId))
			{
				return false;
			}
			this._activePreset = customizationPresetData;
			return !applyToBackend || this.coreOrchestrator.ActiveBackend == null || this.coreOrchestrator.ActiveBackend.ApplyState(customizationPresetData.stateData);
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001F1C6C File Offset: 0x001EFE6C
		public bool TrySetActivePresetForCharacter(string characterId, CustomizationTargetType targetType, string presetId)
		{
			if (this.presetRepository == null || string.IsNullOrWhiteSpace(characterId) || targetType == CustomizationTargetType.None || string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			CustomizationPresetData customizationPresetData = CustomizationPresetCoordinator.FindPreset(this.GetPresets(characterId, targetType, null), presetId);
			return customizationPresetData != null && this.presetRepository.TrySetActivePreset(customizationPresetData.presetId);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001F1CC4 File Offset: 0x001EFEC4
		public bool TryCreatePresetFromCurrentState(string presetName, out CustomizationPresetData createdPreset)
		{
			createdPreset = null;
			if (this.coreOrchestrator == null || this.coreOrchestrator.CurrentSession == null || this.coreOrchestrator.ActiveBackend == null || this.presetRepository == null)
			{
				return false;
			}
			CharacterStateData characterStateData = this.coreOrchestrator.ActiveBackend.CaptureState();
			CustomizationSessionContext currentSession = this.coreOrchestrator.CurrentSession;
			return this.presetRepository.TryCreatePreset(currentSession.characterId, currentSession.targetType, presetName, characterStateData, out createdPreset, false, null);
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001F1D48 File Offset: 0x001EFF48
		public bool TryCreatePreset(string characterId, CustomizationTargetType targetType, string presetName, CharacterStateData stateData, out CustomizationPresetData createdPreset, bool isSystemPreset = false, string iconFileName = null)
		{
			createdPreset = null;
			if (this.presetRepository == null)
			{
				return false;
			}
			CharacterStateData characterStateData = stateData ?? new CharacterStateData();
			return this.presetRepository.TryCreatePreset(characterId, targetType, presetName, characterStateData, out createdPreset, isSystemPreset, iconFileName);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001F1D8C File Offset: 0x001EFF8C
		public bool TryEnsureSystemPreset(string presetId, string characterId, CustomizationTargetType targetType, string presetName, CharacterStateData stateData, string iconFileName = null)
		{
			if (this.presetRepository == null)
			{
				return false;
			}
			CharacterStateData characterStateData = stateData ?? new CharacterStateData();
			return this.presetRepository.TryEnsureSystemPreset(presetId, characterId, targetType, presetName, characterStateData, iconFileName);
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x001F1DC8 File Offset: 0x001EFFC8
		public bool TryDuplicatePreset(string sourcePresetId, string duplicatedPresetName, out CustomizationPresetData duplicatedPreset)
		{
			duplicatedPreset = null;
			return !(this.presetRepository == null) && this.presetRepository.TryDuplicatePreset(sourcePresetId, duplicatedPresetName, out duplicatedPreset);
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001F1DEB File Offset: 0x001EFFEB
		public bool TryUpdatePreset(string presetId, string presetName, CharacterStateData stateData, string iconFileName, out CustomizationPresetData updatedPreset, out string error)
		{
			updatedPreset = null;
			error = null;
			if (this.presetRepository == null)
			{
				error = "Preset repository missing.";
				return false;
			}
			return this.presetRepository.TryUpdatePreset(presetId, presetName, stateData, iconFileName, out updatedPreset, out error);
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001F1E21 File Offset: 0x001F0021
		public bool TryUpdatePresetGender(string presetId, Gender gender)
		{
			return !(this.presetRepository == null) && this.presetRepository.TryUpdatePresetGender(presetId, gender);
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001F1E40 File Offset: 0x001F0040
		public bool TryDuplicatePreset(string characterId, CustomizationTargetType targetType, string sourcePresetId, string duplicatedPresetName, out CustomizationPresetData duplicatedPreset)
		{
			duplicatedPreset = null;
			return !string.IsNullOrWhiteSpace(characterId) && targetType != CustomizationTargetType.None && !(this.presetRepository == null) && CustomizationPresetCoordinator.FindPreset(this.GetPresets(characterId, targetType, null), sourcePresetId) != null && this.presetRepository.TryDuplicatePreset(sourcePresetId, duplicatedPresetName, out duplicatedPreset);
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001F1E8F File Offset: 0x001F008F
		public bool TryDeletePreset(string presetId)
		{
			if (this.presetRepository == null)
			{
				return false;
			}
			if (this._activePreset != null && this._activePreset.presetId == presetId)
			{
				this._activePreset = null;
			}
			return this.presetRepository.TryDeletePreset(presetId);
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001F1ECF File Offset: 0x001F00CF
		public bool TryDeletePreset(string characterId, CustomizationTargetType targetType, string presetId)
		{
			return !string.IsNullOrWhiteSpace(characterId) && targetType != CustomizationTargetType.None && !string.IsNullOrWhiteSpace(presetId) && CustomizationPresetCoordinator.FindPreset(this.GetPresets(characterId, targetType, null), presetId) != null && this.TryDeletePreset(presetId);
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001F1F00 File Offset: 0x001F0100
		public bool SaveActivePresetState()
		{
			if (this._activePreset == null || this.coreOrchestrator == null || this.coreOrchestrator.ActiveBackend == null || this.presetRepository == null)
			{
				return false;
			}
			CharacterStateData characterStateData = this.coreOrchestrator.ActiveBackend.CaptureState();
			return this.presetRepository.TryUpdatePresetState(this._activePreset.presetId, characterStateData);
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001F1F68 File Offset: 0x001F0168
		private void HandleSessionInitialized(CustomizationSessionContext session, ICharacterCustomizationBackend backend)
		{
			if (!this.applyPresetOnSessionInitialized || this.presetRepository == null || backend == null || session == null)
			{
				return;
			}
			if (session.skipPresetAutoApply)
			{
				this._activePreset = null;
				return;
			}
			List<CustomizationPresetData> list = this.presetRepository.GetPresets(session.characterId, session.targetType, null);
			if (list.Count == 0)
			{
				this._activePreset = null;
				return;
			}
			CustomizationPresetData customizationPresetData = null;
			if (!string.IsNullOrWhiteSpace(session.presetId))
			{
				customizationPresetData = CustomizationPresetCoordinator.FindPreset(list, session.presetId);
			}
			if (customizationPresetData == null)
			{
				customizationPresetData = CustomizationPresetCoordinator.FindActivePreset(list);
			}
			if (customizationPresetData == null && this.autoCreateDefaultIfMissing)
			{
				list = this.presetRepository.GetPresets(session.characterId, session.targetType, null);
				customizationPresetData = CustomizationPresetCoordinator.FindActivePreset(list);
			}
			if (customizationPresetData == null)
			{
				return;
			}
			this._activePreset = customizationPresetData;
			backend.ApplyState(customizationPresetData.stateData);
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001F2034 File Offset: 0x001F0234
		private void HandleSessionClosed(CustomizationSessionContext session)
		{
			if (!this.savePresetOnSessionClosed)
			{
				this._activePreset = null;
				return;
			}
			this.SaveActivePresetState();
			this._activePreset = null;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x001F2054 File Offset: 0x001F0254
		private static CustomizationPresetData FindActivePreset(List<CustomizationPresetData> presets)
		{
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i] != null && presets[i].isDefault)
				{
					return presets[i];
				}
			}
			for (int j = 0; j < presets.Count; j++)
			{
				if (presets[j] != null && presets[j].isActive)
				{
					return presets[j];
				}
			}
			if (presets.Count <= 0)
			{
				return null;
			}
			return presets[0];
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x001F20D4 File Offset: 0x001F02D4
		private static CustomizationPresetData FindPreset(List<CustomizationPresetData> presets, string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return null;
			}
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i] != null && presets[i].presetId == presetId)
				{
					return presets[i];
				}
			}
			return null;
		}

		// Token: 0x0400511C RID: 20764
		[Header("References")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x0400511D RID: 20765
		[SerializeField]
		private CustomizationPresetRepository presetRepository;

		// Token: 0x0400511E RID: 20766
		[Header("Flow")]
		[SerializeField]
		private bool applyPresetOnSessionInitialized = true;

		// Token: 0x0400511F RID: 20767
		[SerializeField]
		private bool savePresetOnSessionClosed = true;

		// Token: 0x04005120 RID: 20768
		[SerializeField]
		private bool autoCreateDefaultIfMissing = true;

		// Token: 0x04005121 RID: 20769
		private CustomizationPresetData _activePreset;
	}
}
