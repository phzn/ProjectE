using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using Features.Detection.Customization;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B11 RID: 2833
	public class NpcCustomizationController : MonoBehaviour
	{
		// Token: 0x0600501A RID: 20506 RVA: 0x001EFBD4 File Offset: 0x001EDDD4
		private void OnEnable()
		{
			this.ResolveRefs();
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized += this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed += this.HandleSessionClosed;
				if (this.coreOrchestrator.CurrentSession != null)
				{
					this.HandleSessionInitialized(this.coreOrchestrator.CurrentSession, this.coreOrchestrator.ActiveBackend);
				}
			}
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x001EFC4C File Offset: 0x001EDE4C
		private void OnDisable()
		{
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed -= this.HandleSessionClosed;
			}
			this.UnbindGenderListener();
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x001EFC9C File Offset: 0x001EDE9C
		private void HandleSessionInitialized(CustomizationSessionContext context, ICharacterCustomizationBackend backend)
		{
			if (context == null || context.targetType != CustomizationTargetType.Npc)
			{
				return;
			}
			this.ResolveRefs();
			if (this._editingSession == context && this._sessionInfoManager != null)
			{
				if (!NpcCustomizationController.IsSystemPresetId(context.presetId))
				{
					this.BindGenderListener();
				}
				return;
			}
			this._editingSession = context;
			this._sessionInfoManager = this.ResolveGeneralInfoManager();
			bool flag = NpcCustomizationController.IsSystemPresetId(context.presetId);
			this.ClearCapturedPresetPhoto();
			if (!flag)
			{
				this.CaptureSessionOriginalStance();
				this.BindGenderListener();
			}
			else
			{
				this._hasSessionOriginalStance = false;
				this.UnbindGenderListener();
			}
			string presetId = context.presetId;
			CharacterData characterData = ((!string.IsNullOrWhiteSpace(presetId)) ? this.ResolveStoredOrSystemLook(presetId) : null);
			CharacterGeneralInfo characterGeneralInfo = ((characterData != null && characterData.generalInfo != null) ? new CharacterGeneralInfo(characterData.generalInfo) : NpcCustomizationController.CreateDraftGeneralInfo(this.ResolvePreviewOutfitSystem()));
			if (!string.IsNullOrWhiteSpace(presetId))
			{
				string assignedDocId = DialogueLibraryService.GetAssignedDocId(presetId);
				if (!string.IsNullOrWhiteSpace(assignedDocId))
				{
					characterGeneralInfo.dialogueOverrideId = assignedDocId;
				}
			}
			this._sessionGeneralInfo = new CharacterGeneralInfo(characterGeneralInfo);
			if (this._sessionInfoManager != null)
			{
				this._sessionInfoManager.LoadPresetGeneralInfo(characterGeneralInfo, !flag);
			}
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x001EFDB3 File Offset: 0x001EDFB3
		private static CharacterGeneralInfo CreateDraftGeneralInfo(OutfitSystem outfitSystem)
		{
			return new CharacterGeneralInfo
			{
				gender = ((outfitSystem != null && outfitSystem.stance >= 0.5f) ? Gender.Female : Gender.Male)
			};
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x001EFDDC File Offset: 0x001EDFDC
		private void HandleSessionClosed(CustomizationSessionContext context)
		{
			if (context == null || context.targetType != CustomizationTargetType.Npc)
			{
				this.UnbindGenderListener();
				return;
			}
			if (this._sessionInfoManager != null && this._sessionGeneralInfo != null)
			{
				this._sessionInfoManager.LoadPresetGeneralInfo(this._sessionGeneralInfo, !NpcCustomizationController.IsSystemPresetId(context.presetId));
			}
			if (this._hasSessionOriginalStance)
			{
				this.ApplyStanceToPreview(this._sessionOriginalStance);
			}
			this.UnbindGenderListener();
			this._hasSessionOriginalStance = false;
			this._editingSession = null;
			this._sessionInfoManager = null;
			this._sessionGeneralInfo = null;
			this.ClearCapturedPresetPhoto();
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001EFE70 File Offset: 0x001EE070
		private void BindGenderListener()
		{
			CharacterGeneralInfoManager characterGeneralInfoManager = this.ResolveGeneralInfoManager();
			if (characterGeneralInfoManager == this._boundInfoManager)
			{
				return;
			}
			this.UnbindGenderListener();
			if (characterGeneralInfoManager != null)
			{
				CharacterGeneralInfoManager characterGeneralInfoManager2 = characterGeneralInfoManager;
				characterGeneralInfoManager2.OnGenderChanged = (Action<Gender>)Delegate.Combine(characterGeneralInfoManager2.OnGenderChanged, new Action<Gender>(this.HandleGenderChanged));
				this._boundInfoManager = characterGeneralInfoManager;
			}
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001EFECB File Offset: 0x001EE0CB
		private void UnbindGenderListener()
		{
			if (this._boundInfoManager != null)
			{
				CharacterGeneralInfoManager boundInfoManager = this._boundInfoManager;
				boundInfoManager.OnGenderChanged = (Action<Gender>)Delegate.Remove(boundInfoManager.OnGenderChanged, new Action<Gender>(this.HandleGenderChanged));
				this._boundInfoManager = null;
			}
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001EFF0C File Offset: 0x001EE10C
		private void HandleGenderChanged(Gender gender)
		{
			float num = ((gender == Gender.Female) ? 1f : 0f);
			this.ApplyStanceToPreview(num);
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001EFF34 File Offset: 0x001EE134
		private void CaptureSessionOriginalStance()
		{
			OutfitSystem outfitSystem = this.ResolvePreviewOutfitSystem();
			if (outfitSystem != null)
			{
				this._sessionOriginalStance = outfitSystem.stance;
				this._hasSessionOriginalStance = true;
				return;
			}
			this._hasSessionOriginalStance = false;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x001EFF6C File Offset: 0x001EE16C
		private void ApplyStanceToPreview(float stance)
		{
			OutfitSystem outfitSystem = this.ResolvePreviewOutfitSystem();
			if (outfitSystem != null)
			{
				outfitSystem.SetStance(stance);
			}
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x001EFF90 File Offset: 0x001EE190
		private OutfitSystem ResolvePreviewOutfitSystem()
		{
			GameObject gameObject = ((this.hubPresetListController != null) ? this.hubPresetListController.NpcPreviewTarget : null);
			if (!(gameObject != null))
			{
				return null;
			}
			return gameObject.GetComponentInChildren<OutfitSystem>(true);
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x001EFFCC File Offset: 0x001EE1CC
		private CharacterGeneralInfoManager ResolveGeneralInfoManager()
		{
			if (this._sessionInfoManager != null)
			{
				return this._sessionInfoManager;
			}
			CharacterGeneralInfoManager componentInChildren = base.GetComponentInChildren<CharacterGeneralInfoManager>(true);
			if (!(componentInChildren != null))
			{
				return Object.FindObjectOfType<CharacterGeneralInfoManager>(true);
			}
			return componentInChildren;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x001F0008 File Offset: 0x001EE208
		public bool TrySavePreset(out string error)
		{
			bool flag;
			try
			{
				flag = this.TrySavePresetInternal(out error);
			}
			catch (Exception ex)
			{
				error = "Could not complete the NPC save. " + ex.Message;
				Debug.LogException(ex, this);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x001F0050 File Offset: 0x001EE250
		private bool TrySavePresetInternal(out string error)
		{
			error = null;
			this.ResolveRefs();
			CharacterGeneralInfoManager characterGeneralInfoManager = this.ResolveGeneralInfoManager();
			if (characterGeneralInfoManager == null)
			{
				error = "NPC general info editor not found.";
				return false;
			}
			string text;
			if (characterGeneralInfoManager.birthdayInput != null && !characterGeneralInfoManager.birthdayInput.TryGetValidBirthday(out text))
			{
				error = "Please enter a valid date of birth for someone 18 or older.";
				return false;
			}
			if (this.hubPresetListController == null)
			{
				error = "PresetListController not found.";
				return false;
			}
			CharactersManager charactersManager = this.hubPresetListController.CharactersManager;
			if (charactersManager == null)
			{
				charactersManager = Object.FindObjectOfType<CharactersManager>(true);
			}
			if (charactersManager == null)
			{
				error = "CharactersManager not found.";
				return false;
			}
			string text2 = this.ResolveEditingPresetId();
			bool flag = string.IsNullOrWhiteSpace(text2);
			if (NpcCustomizationController.IsSystemPresetId(text2))
			{
				return this.TrySaveSystemMetadata(text2, out error);
			}
			GameObject npcPreviewTarget = this.hubPresetListController.NpcPreviewTarget;
			OutfitSystem outfitSystem = ((npcPreviewTarget != null) ? npcPreviewTarget.GetComponentInChildren<OutfitSystem>(true) : null);
			if (outfitSystem == null)
			{
				error = "Preview NPC OutfitSystem not found.";
				return false;
			}
			CharacterData characterData = BMAC_SaveSystem.GetCharacterData(outfitSystem);
			if (characterData == null)
			{
				error = "Failed to capture the NPC appearance (CharacterData is null).";
				return false;
			}
			Gender gender = ((characterGeneralInfoManager != null) ? characterGeneralInfoManager.GetCurrentGender() : NpcCustomizationController.ResolveCapturedGender(characterData, outfitSystem));
			CharacterGeneralInfo characterGeneralInfo = NpcCustomizationController.ReadGeneralInfoFromEditor(characterGeneralInfoManager, gender, this._sessionGeneralInfo ?? characterData.generalInfo);
			if (characterGeneralInfo != null)
			{
				characterData.generalInfo = characterGeneralInfo;
			}
			characterData.stance = ((gender == Gender.Female) ? 1f : 0f);
			charactersManager.EnsureCustomNpcOwnerExists();
			string text3 = "CustomNPC";
			if (flag)
			{
				string text4 = this.ResolveDraftName(charactersManager, text3);
				CustomizationPresetData customizationPresetData;
				if (!charactersManager.TryCreatePresetForCharacter(text3, text4, out customizationPresetData, out error, null, null))
				{
					if (string.IsNullOrWhiteSpace(error))
					{
						error = "Failed to create the new custom NPC.";
					}
					return false;
				}
				text2 = customizationPresetData.presetId;
				this.BindEditingPresetId(text2);
			}
			string text5 = NpcCustomizationController.ResolveDisplayNameFromInfo(characterGeneralInfo) ?? this.ResolveDisplayName(text2);
			string text6 = "custom_" + text2 + ".png";
			bool flag2 = this.TryCaptureIcon(npcPreviewTarget, text6);
			PresetIconCache.Invalidate(text6);
			if (!flag2)
			{
				Debug.LogWarning(string.Concat(new string[] { "[NpcCustomization] Icona NON ricreata per '", text2, "' (camera '", this.cameraName, "' non risolta?). Il look è salvato; la card userà l'icona esistente." }), this);
			}
			NpcLookStore.Save(text2, characterData, text5, text6);
			DialogueLibraryService.AssignDocToNpc(text2, (characterGeneralInfo != null) ? characterGeneralInfo.dialogueOverrideId : string.Empty);
			string text7 = NpcCustomizationController.ResolveUniqueName(charactersManager, text3, text2, text5);
			CustomizationPresetData customizationPresetData2;
			string text8;
			if (!charactersManager.TryUpdatePresetForCharacter(text3, text2, text7, new CharacterStateData(), text6, out customizationPresetData2, out text8))
			{
				error = "The NPC look was written, but its preset details could not be updated. " + text8;
				return false;
			}
			if (!charactersManager.TryUpdatePresetGender(text3, text2, gender))
			{
				error = "The NPC look was written, but its population gender could not be updated.";
				return false;
			}
			this.hubPresetListController.LoadPresetsForTarget(CustomizationTargetType.Npc, null, text2);
			this.RememberSavedGeneralInfo(characterGeneralInfo);
			Debug.Log(string.Format("[NpcCustomization] Custom NPC salvato '{0}' (look BoZo + icona, gender={1}).", text2, gender), this);
			return true;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x001F030D File Offset: 0x001EE50D
		private static Gender ResolveCapturedGender(CharacterData look, OutfitSystem outfitSystem)
		{
			if (((look != null) ? look.stance : ((outfitSystem != null) ? outfitSystem.stance : 0f)) < 0.5f)
			{
				return Gender.Male;
			}
			return Gender.Female;
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x001F033C File Offset: 0x001EE53C
		private static CharacterGeneralInfo ReadGeneralInfoFromEditor(CharacterGeneralInfoManager infoManager, Gender gender, CharacterGeneralInfo existingInfo)
		{
			if (infoManager == null)
			{
				return null;
			}
			CharacterGeneralInfo characterGeneralInfo = ((existingInfo != null) ? new CharacterGeneralInfo(existingInfo) : new CharacterGeneralInfo());
			characterGeneralInfo.characterName = infoManager.GetCurrentName() ?? string.Empty;
			characterGeneralInfo.surname = infoManager.GetCurrentSurname() ?? string.Empty;
			characterGeneralInfo.description = infoManager.GetCurrentDescription() ?? string.Empty;
			characterGeneralInfo.attitude = infoManager.GetCurrentAttitude() ?? string.Empty;
			characterGeneralInfo.birthday = infoManager.GetCurrentBirthday() ?? string.Empty;
			characterGeneralInfo.gender = gender;
			characterGeneralInfo.personality = infoManager.GetCurrentPersonality();
			characterGeneralInfo.dialogueOverrideId = infoManager.GetCurrentDialogueOverride() ?? string.Empty;
			return characterGeneralInfo;
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x001F03F5 File Offset: 0x001EE5F5
		public static bool IsSystemPresetId(string presetId)
		{
			return !string.IsNullOrWhiteSpace(presetId) && presetId.StartsWith("sys_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x001F0410 File Offset: 0x001EE610
		public static CharacterData CreateSystemMetadataOverride(CharacterData systemLook, CharacterGeneralInfo editedInfo)
		{
			if (systemLook == null)
			{
				return null;
			}
			CharacterData characterData = JsonUtility.FromJson<CharacterData>(JsonUtility.ToJson(systemLook));
			if (characterData == null || editedInfo == null)
			{
				return characterData;
			}
			CharacterGeneralInfo characterGeneralInfo = systemLook.generalInfo ?? new CharacterGeneralInfo();
			CharacterGeneralInfo characterGeneralInfo2 = new CharacterGeneralInfo(characterGeneralInfo);
			characterGeneralInfo2.characterName = editedInfo.characterName ?? string.Empty;
			characterGeneralInfo2.surname = editedInfo.surname ?? string.Empty;
			characterGeneralInfo2.birthday = editedInfo.birthday ?? string.Empty;
			characterGeneralInfo2.attitude = editedInfo.attitude ?? string.Empty;
			characterGeneralInfo2.description = editedInfo.description ?? string.Empty;
			characterGeneralInfo2.personality = editedInfo.personality;
			string text;
			if ((text = editedInfo.bio) == null)
			{
				text = characterGeneralInfo.bio ?? string.Empty;
			}
			characterGeneralInfo2.bio = text;
			characterGeneralInfo2.dialogueOverrideId = editedInfo.dialogueOverrideId ?? string.Empty;
			characterGeneralInfo2.gender = characterGeneralInfo.gender;
			characterGeneralInfo2.occupation = characterGeneralInfo.occupation;
			CharacterGeneralInfo characterGeneralInfo3 = characterGeneralInfo2;
			characterData.generalInfo = characterGeneralInfo3;
			characterData.characterName = (characterGeneralInfo3.characterName + " " + characterGeneralInfo3.surname).Trim();
			return characterData;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001F0538 File Offset: 0x001EE738
		private bool TrySaveSystemMetadata(string presetId, out string error)
		{
			error = null;
			SystemPresetEntry systemPresetEntry = ((this.hubPresetListController != null) ? this.hubPresetListController.FindSystemPresetEntry(presetId) : null);
			CharacterData characterData = ((systemPresetEntry != null && systemPresetEntry.sourceAsset != null) ? systemPresetEntry.sourceAsset.data : null);
			if (characterData == null)
			{
				error = "System NPC source data not found.";
				return false;
			}
			CharacterGeneralInfo characterGeneralInfo = NpcCustomizationController.ReadGeneralInfoFromEditor(this.ResolveGeneralInfoManager(), (characterData.generalInfo != null) ? characterData.generalInfo.gender : Gender.Male, characterData.generalInfo);
			if (characterGeneralInfo == null)
			{
				error = "NPC general info editor not found.";
				return false;
			}
			CharacterData characterData2 = NpcCustomizationController.CreateSystemMetadataOverride(characterData, characterGeneralInfo);
			string text = NpcCustomizationController.ResolveDisplayNameFromInfo(characterData2.generalInfo) ?? this.ResolveDisplayName(presetId);
			NpcLookStore.NpcLookMeta npcLookMeta = NpcLookStore.LoadMeta(presetId);
			string text2 = ((npcLookMeta != null && !string.IsNullOrWhiteSpace(npcLookMeta.iconFileName)) ? npcLookMeta.iconFileName : CustomizationHubPresetListController.GetSystemPresetIconFileName(presetId));
			NpcLookStore.Save(presetId, characterData2, text, text2);
			DialogueLibraryService.AssignDocToNpc(presetId, characterGeneralInfo.dialogueOverrideId);
			this.hubPresetListController.LoadPresetsForTarget(CustomizationTargetType.Npc, null, presetId);
			this.RememberSavedGeneralInfo(characterData2.generalInfo);
			Debug.Log("[NpcCustomization] System NPC metadata saved '" + presetId + "' (appearance preserved).", this);
			return true;
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001F065C File Offset: 0x001EE85C
		private CharacterData ResolveStoredOrSystemLook(string presetId)
		{
			CharacterData characterData = NpcLookStore.LoadLook(presetId);
			if (!NpcCustomizationController.IsSystemPresetId(presetId))
			{
				return characterData;
			}
			if (this.hubPresetListController == null)
			{
				return null;
			}
			SystemPresetEntry systemPresetEntry = this.hubPresetListController.FindSystemPresetEntry(presetId);
			CharacterData characterData2 = ((systemPresetEntry != null && systemPresetEntry.sourceAsset != null) ? systemPresetEntry.sourceAsset.data : null);
			if (characterData == null || characterData.generalInfo == null)
			{
				return characterData2;
			}
			return NpcCustomizationController.CreateSystemMetadataOverride(characterData2, characterData.generalInfo);
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x001F06D0 File Offset: 0x001EE8D0
		private static string ResolveDisplayNameFromInfo(CharacterGeneralInfo info)
		{
			if (info == null)
			{
				return null;
			}
			string text = (info.characterName + " " + info.surname).Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001F070C File Offset: 0x001EE90C
		private static string ResolveUniqueName(CharactersManager charactersManager, string ownerId, string presetId, string desiredName)
		{
			if (string.IsNullOrWhiteSpace(desiredName))
			{
				return desiredName;
			}
			List<CustomizationPresetData> presetsForCharacter = charactersManager.GetPresetsForCharacter(ownerId);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (presetsForCharacter != null)
			{
				for (int i = 0; i < presetsForCharacter.Count; i++)
				{
					if (presetsForCharacter[i] != null && presetsForCharacter[i].presetId != presetId && !string.IsNullOrWhiteSpace(presetsForCharacter[i].presetName))
					{
						hashSet.Add(presetsForCharacter[i].presetName.Trim());
					}
				}
			}
			if (!hashSet.Contains(desiredName))
			{
				return desiredName;
			}
			for (int j = 2; j <= 999; j++)
			{
				string text = string.Format("{0} ({1})", desiredName, j);
				if (!hashSet.Contains(text))
				{
					return text;
				}
			}
			return desiredName + " " + Guid.NewGuid().ToString("N").Substring(0, 6);
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x001F07F4 File Offset: 0x001EE9F4
		private string ResolveDraftName(CharactersManager charactersManager, string ownerId)
		{
			string text = "New NPC";
			List<CustomizationPresetData> presetsForCharacter = charactersManager.GetPresetsForCharacter(ownerId);
			if (presetsForCharacter == null || presetsForCharacter.Count == 0)
			{
				return text;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < presetsForCharacter.Count; i++)
			{
				if (presetsForCharacter[i] != null && !string.IsNullOrWhiteSpace(presetsForCharacter[i].presetName))
				{
					hashSet.Add(presetsForCharacter[i].presetName.Trim());
				}
			}
			if (!hashSet.Contains(text))
			{
				return text;
			}
			for (int j = 2; j <= 999; j++)
			{
				string text2 = string.Format("{0} {1}", text, j);
				if (!hashSet.Contains(text2))
				{
					return text2;
				}
			}
			return text + " " + Guid.NewGuid().ToString("N").Substring(0, 6);
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x001F08D4 File Offset: 0x001EEAD4
		private string ResolveEditingPresetId()
		{
			if (this.coreOrchestrator != null && this.coreOrchestrator.CurrentSession != null)
			{
				return this.coreOrchestrator.CurrentSession.presetId;
			}
			CustomizationPresetData customizationPresetData = ((this.hubPresetListController != null) ? this.hubPresetListController.SelectedPreset : null);
			if (customizationPresetData == null || customizationPresetData.targetType != CustomizationTargetType.Npc)
			{
				return null;
			}
			return customizationPresetData.presetId;
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x001F0940 File Offset: 0x001EEB40
		private void BindEditingPresetId(string presetId)
		{
			CustomizationSessionContext customizationSessionContext = ((this.coreOrchestrator != null) ? this.coreOrchestrator.CurrentSession : this._editingSession);
			if (customizationSessionContext != null && customizationSessionContext.targetType == CustomizationTargetType.Npc)
			{
				customizationSessionContext.presetId = presetId;
				this._editingSession = customizationSessionContext;
			}
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001F0989 File Offset: 0x001EEB89
		private void RememberSavedGeneralInfo(CharacterGeneralInfo info)
		{
			this._sessionGeneralInfo = ((info != null) ? new CharacterGeneralInfo(info) : null);
			if (this._hasSessionOriginalStance)
			{
				this.CaptureSessionOriginalStance();
			}
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x001F09AC File Offset: 0x001EEBAC
		private string ResolveDisplayName(string presetId)
		{
			CharactersManager charactersManager = ((this.hubPresetListController != null) ? this.hubPresetListController.CharactersManager : null);
			if (charactersManager == null)
			{
				charactersManager = Object.FindObjectOfType<CharactersManager>(true);
			}
			if (charactersManager != null)
			{
				List<CustomizationPresetData> presetsForCharacter = charactersManager.GetPresetsForCharacter("CustomNPC");
				if (presetsForCharacter != null)
				{
					for (int i = 0; i < presetsForCharacter.Count; i++)
					{
						if (presetsForCharacter[i] != null && presetsForCharacter[i].presetId == presetId && !string.IsNullOrWhiteSpace(presetsForCharacter[i].presetName))
						{
							return presetsForCharacter[i].presetName;
						}
					}
				}
			}
			NpcLookStore.NpcLookMeta npcLookMeta = NpcLookStore.LoadMeta(presetId);
			if (npcLookMeta != null && !string.IsNullOrWhiteSpace(npcLookMeta.displayName))
			{
				return npcLookMeta.displayName;
			}
			CustomizationPresetData customizationPresetData = ((this.hubPresetListController != null) ? this.hubPresetListController.SelectedPreset : null);
			if (customizationPresetData != null && customizationPresetData.presetId == presetId && !string.IsNullOrWhiteSpace(customizationPresetData.presetName))
			{
				return customizationPresetData.presetName;
			}
			if (presetId == null || !presetId.StartsWith("sys_"))
			{
				return presetId;
			}
			return presetId.Substring(4);
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x001F0AD0 File Offset: 0x001EECD0
		private void ResolveRefs()
		{
			if (this.coreOrchestrator == null)
			{
				this.coreOrchestrator = Object.FindObjectOfType<CustomizationCoreOrchestrator>(true);
			}
			if (this.hubPresetListController == null)
			{
				this.hubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
			}
			if (this.iconCamera == null)
			{
				this.iconCamera = this.ResolveCameraByName();
			}
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x001F0B2C File Offset: 0x001EED2C
		private Camera ResolveCameraByName()
		{
			Camera[] array = Resources.FindObjectsOfTypeAll<Camera>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].gameObject.scene.IsValid() && array[i].name == this.cameraName)
				{
					return array[i];
				}
			}
			return null;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x001F0B8C File Offset: 0x001EED8C
		private bool TryCaptureIcon(GameObject preview, string iconFileName)
		{
			if (string.IsNullOrWhiteSpace(iconFileName))
			{
				return false;
			}
			GameDataPaths.EnsureCustomizationDirectories();
			string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, iconFileName);
			if (this._capturedPresetPhoto != null)
			{
				try
				{
					File.WriteAllBytes(text, this._capturedPresetPhoto.EncodeToPNG());
					return true;
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[NpcCustomization] Salvataggio foto scelta fallito: " + ex.Message, this);
					return false;
				}
			}
			Texture2D texture2D = this.CaptureIconTexture();
			if (texture2D == null)
			{
				return false;
			}
			bool flag;
			try
			{
				File.WriteAllBytes(text, texture2D.EncodeToPNG());
				flag = true;
			}
			catch (Exception ex2)
			{
				Debug.LogWarning("[NpcCustomization] Cattura icona fallita: " + ex2.Message, this);
				flag = false;
			}
			finally
			{
				Object.Destroy(texture2D);
			}
			return flag;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x001F0C64 File Offset: 0x001EEE64
		public Texture2D CapturePresetPhoto()
		{
			this.ClearCapturedPresetPhoto();
			this._capturedPresetPhoto = this.CaptureIconTexture();
			return this._capturedPresetPhoto;
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x001F0C80 File Offset: 0x001EEE80
		public bool RenderPresetPreview(RenderTexture destination, bool fullTextureQuality = false)
		{
			this.ResolveRefs();
			if (this.iconCamera == null)
			{
				this.iconCamera = this.ResolveCameraByName();
			}
			GameObject gameObject = ((this.hubPresetListController != null) ? this.hubPresetListController.NpcPreviewTarget : null);
			if (this.iconCamera == null || destination == null || gameObject == null || !gameObject.activeInHierarchy)
			{
				return false;
			}
			Camera camera = this.iconCamera;
			CameraClearFlags clearFlags = camera.clearFlags;
			Color color = camera.backgroundColor;
			bool activeSelf = camera.gameObject.activeSelf;
			bool flag;
			try
			{
				if (!activeSelf)
				{
					camera.gameObject.SetActive(true);
				}
				if (this.overrideBackground)
				{
					camera.clearFlags = CameraClearFlags.Color;
					camera.backgroundColor = this.backgroundColor;
				}
				NpcPortraitRendering.Render(camera, destination, gameObject, fullTextureQuality);
				flag = true;
			}
			finally
			{
				camera.clearFlags = clearFlags;
				camera.backgroundColor = color;
				if (!activeSelf)
				{
					camera.gameObject.SetActive(false);
				}
			}
			return flag;
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x001F0D80 File Offset: 0x001EEF80
		private Texture2D CaptureIconTexture()
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(this.iconWidth, this.iconHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			Texture2D texture2D;
			try
			{
				if (!this.RenderPresetPreview(temporary, false))
				{
					texture2D = null;
				}
				else
				{
					RenderTexture.active = temporary;
					Texture2D texture2D2 = new Texture2D(temporary.width, temporary.height, TextureFormat.RGBA32, false);
					texture2D2.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
					texture2D2.Apply();
					texture2D = texture2D2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcCustomization] Cattura icona fallita: " + ex.Message, this);
				texture2D = null;
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return texture2D;
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x001F0E44 File Offset: 0x001EF044
		public void ClearCapturedPresetPhoto()
		{
			if (this._capturedPresetPhoto != null)
			{
				Object.Destroy(this._capturedPresetPhoto);
				this._capturedPresetPhoto = null;
			}
		}

		// Token: 0x040050ED RID: 20717
		[Header("References (auto-resolved se nulli)")]
		[Tooltip("Orchestratore: fornisce la sessione corrente (presetId del system NPC in edit).")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x040050EE RID: 20718
		[Tooltip("List controller: fornisce il preview NPC attivo (OutfitSystem) e ricarica la lista.")]
		[SerializeField]
		private CustomizationHubPresetListController hubPresetListController;

		// Token: 0x040050EF RID: 20719
		[Header("Icona")]
		[Tooltip("Camera dedicata per l'icona (stessa usata dal batch, es. NPCPresetsCamera). Se nulla, cercata per nome cameraName.")]
		[SerializeField]
		private Camera iconCamera;

		// Token: 0x040050F0 RID: 20720
		[SerializeField]
		private string cameraName = "NPCPresetsCamera";

		// Token: 0x040050F1 RID: 20721
		[SerializeField]
		private int iconWidth = 512;

		// Token: 0x040050F2 RID: 20722
		[SerializeField]
		private int iconHeight = 512;

		// Token: 0x040050F3 RID: 20723
		[SerializeField]
		private bool overrideBackground = true;

		// Token: 0x040050F4 RID: 20724
		[SerializeField]
		private Color backgroundColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040050F5 RID: 20725
		private const float FemaleStanceThreshold = 0.5f;

		// Token: 0x040050F6 RID: 20726
		private CharacterGeneralInfoManager _boundInfoManager;

		// Token: 0x040050F7 RID: 20727
		private float _sessionOriginalStance;

		// Token: 0x040050F8 RID: 20728
		private bool _hasSessionOriginalStance;

		// Token: 0x040050F9 RID: 20729
		private Texture2D _capturedPresetPhoto;

		// Token: 0x040050FA RID: 20730
		private CustomizationSessionContext _editingSession;

		// Token: 0x040050FB RID: 20731
		private CharacterGeneralInfoManager _sessionInfoManager;

		// Token: 0x040050FC RID: 20732
		private CharacterGeneralInfo _sessionGeneralInfo;
	}
}
