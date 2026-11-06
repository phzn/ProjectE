using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.Inventory.Placement;
using Features.Localization;
using Features.MainMenu;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.RapidActions
{
	// Token: 0x02000D8C RID: 3468
	[DisallowMultipleComponent]
	public class RapidActionsHUDController : MonoBehaviour
	{
		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06006603 RID: 26115 RVA: 0x0028B8D2 File Offset: 0x00289AD2
		public static bool AnyWheelOpen
		{
			get
			{
				return RapidActionsHUDController._wheelOwner != null && RapidActionsHUDController._wheelOwner.isActiveAndEnabled && RapidActionsHUDController._wheelOwner.IsRebindOpen;
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x0028B8FC File Offset: 0x00289AFC
		private void Start()
		{
			this.ResolveDependencies();
			this.BuildRuntimes();
			this.HookCoverPoseEvents();
			this.HookFutanariEvents();
			this.LoadPersistedPreferences();
			this.ApplyStaticConfigToSlots();
			this.RefreshArmsSlot(true);
			PlayerActivityGate.Changed += this.OnActivityGateChanged;
			this.RefreshGatedSlotStates();
			if (this.useArmsSlot && this.playerInput != null)
			{
				this.playerInput.suppressBuiltInCoverPoseInput = true;
			}
			if (this.rebindWheel != null && this.rebindWheel.IsEnabled)
			{
				this.rebindWheel.DisableImmediate();
			}
			if (this.rebindWheelLeft != null && this.rebindWheelLeft.IsEnabled)
			{
				this.rebindWheelLeft.DisableImmediate();
			}
			if (this.rebindWheelRight != null && this.rebindWheelRight.IsEnabled)
			{
				this.rebindWheelRight.DisableImmediate();
			}
			if (this.pagePrevIndicator != null)
			{
				this.pagePrevIndicator.SetActive(false);
			}
			if (this.pageNextIndicator != null)
			{
				this.pageNextIndicator.SetActive(false);
			}
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x0028BA13 File Offset: 0x00289C13
		private void OnEnable()
		{
			if (this.playerInput != null)
			{
				this.playerInput.suppressBuiltInCoverPoseInput = this.useArmsSlot;
			}
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x0028BA34 File Offset: 0x00289C34
		private void OnDisable()
		{
			if (this._activeSlot != null && this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen)
			{
				this.CloseRebindWheel(false);
			}
			else if (this._activeSlot != null)
			{
				this.ResetSlot(this._activeSlot);
			}
			if (this.playerInput != null)
			{
				this.playerInput.suppressBuiltInCoverPoseInput = false;
			}
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x0028BA90 File Offset: 0x00289C90
		private void OnDestroy()
		{
			this.UnhookCoverPoseEvents();
			this.UnhookFutanariEvents();
			PlayerActivityGate.Changed -= this.OnActivityGateChanged;
			this.CloseRebindWheel(false);
			if (this.coverPoseController != null && !this._armsSceneLocked)
			{
				this.coverPoseController.SuppressBuiltInPoseCycleInput = false;
			}
			if (this.playerInput != null)
			{
				this.playerInput.suppressBuiltInCoverPoseInput = false;
			}
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x0028BB00 File Offset: 0x00289D00
		public void SetTripodInputMode(bool active)
		{
			if (this._tripodInputMode == active)
			{
				return;
			}
			this.CancelTransientInput();
			this._tripodInputMode = active;
			this.RefreshCrouchSlotView();
			for (int i = 0; i < this.emoteSlots.Count; i++)
			{
				this.RefreshEmoteSlotView(i);
			}
			this.HandleArmsSlotVisibility();
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06006609 RID: 26121 RVA: 0x0028BB4D File Offset: 0x00289D4D
		public bool IsRebindOpen
		{
			get
			{
				return this._activeSlot != null && this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen;
			}
		}

		// Token: 0x0600660A RID: 26122 RVA: 0x0028BB67 File Offset: 0x00289D67
		public void CancelTransientInput()
		{
			if (this._activeSlot == null)
			{
				return;
			}
			if (this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen)
			{
				this.CloseRebindWheel(false);
				return;
			}
			this.ResetSlot(this._activeSlot);
		}

		// Token: 0x0600660B RID: 26123 RVA: 0x0028BB94 File Offset: 0x00289D94
		private void ResolveDependencies()
		{
			if (this.coverPoseController == null)
			{
				this.coverPoseController = CoverPoseController.Instance;
			}
			if (this.gameplayEmoteController == null)
			{
				this.gameplayEmoteController = Object.FindObjectOfType<GameplayEmoteController>();
			}
			if (this.playerInput == null)
			{
				this.playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
		}

		// Token: 0x0600660C RID: 26124 RVA: 0x0028BBEC File Offset: 0x00289DEC
		private void RefreshLateDependencies()
		{
			if (this.coverPoseController != null && this.gameplayEmoteController != null && this.playerInput != null)
			{
				return;
			}
			if (Time.unscaledTime < this._nextDependencyCheck)
			{
				return;
			}
			this._nextDependencyCheck = Time.unscaledTime + 0.5f;
			bool flag = this.coverPoseController == null;
			bool flag2 = this.gameplayEmoteController == null;
			bool flag3 = this.playerInput == null;
			this.ResolveDependencies();
			if (flag && this.coverPoseController != null)
			{
				this.HookCoverPoseEvents();
				this._armsPrefsApplied = false;
				this.ApplyPersistedArmsPrefs();
				this.RefreshArmsSlot(true);
			}
			if (flag2 && this.gameplayEmoteController != null)
			{
				this.HookFutanariEvents();
				this.LoadEmoteBindingsForCurrentFutanari();
			}
			if (flag3 && this.playerInput != null)
			{
				this.playerInput.suppressBuiltInCoverPoseInput = this.useArmsSlot;
				this.ApplyStaticConfigToSlots();
			}
		}

		// Token: 0x0600660D RID: 26125 RVA: 0x0028BCE0 File Offset: 0x00289EE0
		private void BuildRuntimes()
		{
			this._armsRuntime = new RapidActionsHUDController.SlotRuntime
			{
				Kind = RapidActionsHUDController.SlotKind.Arms,
				Hotkey = null,
				Phase = RapidActionsHUDController.SlotPhase.Idle
			};
			this._emoteRuntimes.Clear();
			for (int i = 0; i < this.emoteSlots.Count; i++)
			{
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[i];
				if (rapidEmoteSlotConfig != null)
				{
					rapidEmoteSlotConfig.RuntimeEmote = rapidEmoteSlotConfig.defaultEmote;
				}
				this._emoteRuntimes.Add(new RapidActionsHUDController.SlotRuntime
				{
					Kind = RapidActionsHUDController.SlotKind.Emote,
					EmoteSlotIndex = i,
					Hotkey = ((rapidEmoteSlotConfig != null) ? rapidEmoteSlotConfig.hotkey : null),
					Phase = RapidActionsHUDController.SlotPhase.Idle
				});
			}
			if (this.crouchSlot != null)
			{
				this.crouchSlot.RuntimeEmote = this.crouchSlot.defaultEmote;
			}
			this._crouchRuntime = new RapidActionsHUDController.SlotRuntime
			{
				Kind = RapidActionsHUDController.SlotKind.Crouch,
				Hotkey = ((this.crouchSlot != null) ? this.crouchSlot.hotkey : null),
				Phase = RapidActionsHUDController.SlotPhase.Idle
			};
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x0028BDD4 File Offset: 0x00289FD4
		private void HookCoverPoseEvents()
		{
			if (this.coverPoseController == null)
			{
				return;
			}
			this.coverPoseController.OnEmotionPoseSelectionChanged += this.OnArmsSelectionChanged;
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x0028BDFC File Offset: 0x00289FFC
		private void UnhookCoverPoseEvents()
		{
			if (this.coverPoseController == null)
			{
				return;
			}
			this.coverPoseController.OnEmotionPoseSelectionChanged -= this.OnArmsSelectionChanged;
		}

		// Token: 0x06006610 RID: 26128 RVA: 0x0028BE24 File Offset: 0x0028A024
		private void OnArmsSelectionChanged()
		{
			this.RefreshArmsSlot(false);
		}

		// Token: 0x06006611 RID: 26129 RVA: 0x0028BE2D File Offset: 0x0028A02D
		private void OnActivityGateChanged(PlayerActivity _)
		{
			this.RefreshGatedSlotStates();
		}

		// Token: 0x06006612 RID: 26130 RVA: 0x0028BE38 File Offset: 0x0028A038
		private void RefreshGatedSlotStates()
		{
			PlayerActivity playerActivity = (GameplayInputGate.BlocksGameplayInput ? PlayerActivity.AllPlayerActions : PlayerActivityGate.BlockedMask);
			this.ApplyGatedSlotStates(playerActivity);
		}

		// Token: 0x06006613 RID: 26131 RVA: 0x0028BE60 File Offset: 0x0028A060
		private void ApplyGatedSlotStates(PlayerActivity effectiveMask)
		{
			bool areMasturbationEmotesBlocked = GameplayEmoteController.AreMasturbationEmotesBlocked;
			int masturbationBlockVersion = GameplayEmoteController.MasturbationBlockVersion;
			if (this._displayedBlockedMask == effectiveMask && this._displayedMasturbationBlockVersion == masturbationBlockVersion)
			{
				return;
			}
			this._displayedBlockedMask = effectiveMask;
			this._displayedMasturbationBlockVersion = masturbationBlockVersion;
			bool flag = (effectiveMask & (PlayerActivity.Cover | PlayerActivity.ArmsPose)) > PlayerActivity.None;
			bool flag2 = (effectiveMask & PlayerActivity.Emote) > PlayerActivity.None;
			if (this.useArmsSlot && this.armsSlot != null && this.armsSlot.slotView != null)
			{
				string text = ((this.coverPoseController != null) ? this.coverPoseController.GetCurrentEmotionPoseLabel() : string.Empty);
				this.armsSlot.slotView.SetDisabledState(flag, "Disabled", text);
			}
			for (int i = 0; i < this.emoteSlots.Count; i++)
			{
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[i];
				if (rapidEmoteSlotConfig != null && !(rapidEmoteSlotConfig.slotView == null))
				{
					string text2 = ((rapidEmoteSlotConfig.RuntimeEmote != null) ? RapidActionsHUDController.GetEmoteDisplayName(rapidEmoteSlotConfig.RuntimeEmote) : string.Empty);
					bool flag3 = flag2 || (areMasturbationEmotesBlocked && GameplayEmoteController.IsCoveredByMasturbationBlock(rapidEmoteSlotConfig.RuntimeEmote));
					rapidEmoteSlotConfig.slotView.SetDisabledState(flag3, "Disabled", text2);
				}
			}
			if (this.useCrouchSlot && this.crouchSlot != null && this.crouchSlot.slotView != null)
			{
				bool flag4 = (effectiveMask & PlayerActivity.Crouch) > PlayerActivity.None;
				string text3 = ((this.crouchSlot.RuntimeEmote != null) ? RapidActionsHUDController.GetEmoteDisplayName(this.crouchSlot.RuntimeEmote) : string.Empty);
				this.crouchSlot.slotView.SetDisabledState(flag4, "Disabled", text3);
			}
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x0028C00F File Offset: 0x0028A20F
		private void InvalidateMasturbationSlotStates()
		{
			if (GameplayEmoteController.AreMasturbationEmotesBlocked)
			{
				this._displayedMasturbationBlockVersion = -1;
			}
		}

		// Token: 0x06006615 RID: 26133 RVA: 0x0028C020 File Offset: 0x0028A220
		private void HookFutanariEvents()
		{
			if (this.gameplayEmoteController != null && this.gameplayEmoteController.FutanariMode != null)
			{
				this.gameplayEmoteController.FutanariMode.OnFutanariModeChanged.AddListener(new UnityAction<bool>(this.OnFutanariModeChanged));
			}
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x0028C070 File Offset: 0x0028A270
		private void UnhookFutanariEvents()
		{
			if (this.gameplayEmoteController != null && this.gameplayEmoteController.FutanariMode != null)
			{
				this.gameplayEmoteController.FutanariMode.OnFutanariModeChanged.RemoveListener(new UnityAction<bool>(this.OnFutanariModeChanged));
			}
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x0028C0BF File Offset: 0x0028A2BF
		private void OnFutanariModeChanged(bool _)
		{
			this.LoadEmoteBindingsForCurrentFutanari();
		}

		// Token: 0x06006618 RID: 26136 RVA: 0x0028C0C7 File Offset: 0x0028A2C7
		private void LoadPersistedPreferences()
		{
			this.ApplyPersistedArmsPrefs();
			this.LoadEmoteBindingsForCurrentFutanari();
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x0028C0D8 File Offset: 0x0028A2D8
		private void ApplyPersistedArmsPrefs()
		{
			if (this._armsPrefsApplied || this.coverPoseController == null)
			{
				return;
			}
			this._armsPrefsApplied = true;
			this._armsSceneLocked = this.coverPoseController.ForceEmotion > CoverEmotionOverride.None;
			if (this._armsSceneLocked)
			{
				int emotionPoseTypeIndex = this.coverPoseController.GetEmotionPoseTypeIndex(true);
				int emotionPoseTypeIndex2 = this.coverPoseController.GetEmotionPoseTypeIndex(false);
				this.coverPoseController.SetEmotionPoseTypeIndex(true, RapidActionsPersistence.LoadArmsPoseIndex(true, emotionPoseTypeIndex));
				this.coverPoseController.SetEmotionPoseTypeIndex(false, RapidActionsPersistence.LoadArmsPoseIndex(false, emotionPoseTypeIndex2));
				return;
			}
			this.coverPoseController.SuppressBuiltInPoseCycleInput = true;
			bool flag;
			int num;
			if (RapidActionsPersistence.HasUnifiedArmsPose())
			{
				RapidActionsPersistence.LoadUnifiedArmsPose(out flag, out num);
			}
			else if (!this.TryFindPoseByLabel(this.defaultUnifiedArmsPoseLabel, out flag, out num))
			{
				flag = false;
				num = 0;
			}
			if (this.coverPoseController.IsPoseAvailable(flag, num))
			{
				this.ApplyUnifiedArmsPose(flag, num, true);
				return;
			}
			List<RapidActionsHUDController.RebindOption> list = new List<RapidActionsHUDController.RebindOption>();
			this.BuildArmsOptions(list);
			if (list.Count == 0)
			{
				return;
			}
			flag = list[0].ArmsExhibit;
			num = list[0].IntPayload;
			this.ApplyUnifiedArmsPose(flag, num, false);
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x0028C1EC File Offset: 0x0028A3EC
		private void ApplyUnifiedArmsPose(bool exhibit, int index, bool persist)
		{
			if (this.coverPoseController == null || this._armsSceneLocked)
			{
				return;
			}
			if (!this.coverPoseController.IsPoseAvailable(exhibit, index))
			{
				return;
			}
			this.coverPoseController.ForceEmotion = (exhibit ? CoverEmotionOverride.Confident : CoverEmotionOverride.Embarrassed);
			this.coverPoseController.SetEmotionPoseTypeIndex(exhibit, index);
			if (this.coverPoseController.IsCovering)
			{
				this.coverPoseController.SetCoverEmotionVariant(exhibit);
			}
			if (persist)
			{
				RapidActionsPersistence.SaveUnifiedArmsPose(exhibit, index);
			}
			this.RefreshArmsSlot(true);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[RapidActions] Unified arms pose -> {0}[{1}] '{2}'", exhibit ? "Confident" : "Embarrassed", index, this.coverPoseController.GetEmotionPoseLabel(exhibit, index)));
			}
		}

		// Token: 0x0600661B RID: 26139 RVA: 0x0028C2A4 File Offset: 0x0028A4A4
		private bool TryFindPoseByLabel(string label, out bool exhibit, out int index)
		{
			exhibit = false;
			index = 0;
			if (this.coverPoseController == null || string.IsNullOrEmpty(label))
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 1;
				int emotionPoseTypeCount = this.coverPoseController.GetEmotionPoseTypeCount(flag);
				for (int j = 0; j < emotionPoseTypeCount; j++)
				{
					if (string.Equals(this.coverPoseController.GetEmotionPoseLabel(flag, j), label, StringComparison.OrdinalIgnoreCase))
					{
						exhibit = flag;
						index = j;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x0028C318 File Offset: 0x0028A518
		private bool CurrentFutanari()
		{
			return this.gameplayEmoteController != null && this.gameplayEmoteController.FutanariMode != null && this.gameplayEmoteController.FutanariMode.FutanariMode;
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x0028C350 File Offset: 0x0028A550
		private void LoadEmoteBindingsForCurrentFutanari()
		{
			bool flag = this.CurrentFutanari();
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < this.emoteSlots.Count; i++)
			{
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[i];
				if (rapidEmoteSlotConfig != null)
				{
					EmoteDefinitionSO emoteDefinitionSO = null;
					if (RapidActionsPersistence.HasEmoteSlot(i, flag))
					{
						string text = RapidActionsPersistence.LoadEmoteSlot(i, flag, string.Empty);
						if (!string.IsNullOrEmpty(text))
						{
							EmoteDefinitionSO emoteDefinitionSO2 = this.ResolveEmoteById(text);
							if (emoteDefinitionSO2 != null && this.IsEmoteAllowed(emoteDefinitionSO2, true))
							{
								emoteDefinitionSO = emoteDefinitionSO2;
							}
						}
					}
					if (emoteDefinitionSO == null && rapidEmoteSlotConfig.defaultEmote != null && this.IsEmoteAllowed(rapidEmoteSlotConfig.defaultEmote, true))
					{
						emoteDefinitionSO = rapidEmoteSlotConfig.defaultEmote;
					}
					if (emoteDefinitionSO == null)
					{
						emoteDefinitionSO = this.FirstAllowedEmoteNotIn(hashSet);
					}
					rapidEmoteSlotConfig.RuntimeEmote = emoteDefinitionSO;
					if (emoteDefinitionSO != null)
					{
						hashSet.Add(RapidActionsHUDController.GetEmoteId(emoteDefinitionSO));
					}
					RapidActionsPersistence.SaveEmoteSlot(i, flag, (emoteDefinitionSO != null) ? RapidActionsHUDController.GetEmoteId(emoteDefinitionSO) : string.Empty);
					this.RefreshEmoteSlotView(i);
				}
			}
			this.InvalidateMasturbationSlotStates();
			if (this.crouchSlot != null)
			{
				EmoteDefinitionSO emoteDefinitionSO3 = null;
				if (RapidActionsPersistence.HasCrouchEmoteId(flag))
				{
					string text2 = RapidActionsPersistence.LoadCrouchEmoteId(flag, string.Empty);
					if (!string.IsNullOrEmpty(text2))
					{
						EmoteDefinitionSO emoteDefinitionSO4 = this.ResolveCrouchEmoteById(text2);
						if (emoteDefinitionSO4 != null && this.IsCrouchEmoteAllowed(emoteDefinitionSO4))
						{
							emoteDefinitionSO3 = emoteDefinitionSO4;
						}
					}
				}
				if (emoteDefinitionSO3 == null && this.crouchSlot.defaultEmote != null && this.IsCrouchEmoteAllowed(this.crouchSlot.defaultEmote))
				{
					emoteDefinitionSO3 = this.crouchSlot.defaultEmote;
				}
				if (emoteDefinitionSO3 == null)
				{
					emoteDefinitionSO3 = this.FirstAllowedCrouchEmote();
				}
				this.crouchSlot.RuntimeEmote = emoteDefinitionSO3;
				RapidActionsPersistence.SaveCrouchEmoteId(flag, (emoteDefinitionSO3 != null) ? RapidActionsHUDController.GetEmoteId(emoteDefinitionSO3) : string.Empty);
				this.RefreshCrouchSlotView();
			}
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x0028C53C File Offset: 0x0028A73C
		private EmoteDefinitionSO FirstAllowedEmoteNotIn(HashSet<string> usedIds)
		{
			if (this.emoteCollection == null)
			{
				return null;
			}
			List<EmoteDefinitionSO> unlockedEmotes = this.emoteCollection.GetUnlockedEmotes();
			for (int i = 0; i < unlockedEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = unlockedEmotes[i];
				if (!(emoteDefinitionSO == null) && this.IsEmoteAllowed(emoteDefinitionSO, true) && (usedIds == null || !usedIds.Contains(RapidActionsHUDController.GetEmoteId(emoteDefinitionSO))))
				{
					return emoteDefinitionSO;
				}
			}
			for (int j = 0; j < unlockedEmotes.Count; j++)
			{
				if (unlockedEmotes[j] != null && this.IsEmoteAllowed(unlockedEmotes[j], true))
				{
					return unlockedEmotes[j];
				}
			}
			return null;
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x0028C5E0 File Offset: 0x0028A7E0
		private EmoteDefinitionSO FirstAllowedCrouchEmote()
		{
			if (this.crouchSlot == null || this.crouchSlot.crouchEmotes == null)
			{
				return null;
			}
			for (int i = 0; i < this.crouchSlot.crouchEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = this.crouchSlot.crouchEmotes[i];
				if (emoteDefinitionSO != null && this.IsCrouchEmoteAllowed(emoteDefinitionSO))
				{
					return emoteDefinitionSO;
				}
			}
			return null;
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x0028C648 File Offset: 0x0028A848
		private EmoteDefinitionSO ResolveCrouchEmoteById(string id)
		{
			if (this.crouchSlot == null || this.crouchSlot.crouchEmotes == null || string.IsNullOrEmpty(id))
			{
				return null;
			}
			for (int i = 0; i < this.crouchSlot.crouchEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = this.crouchSlot.crouchEmotes[i];
				if (!(emoteDefinitionSO == null) && RapidActionsHUDController.GetEmoteId(emoteDefinitionSO) == id)
				{
					return emoteDefinitionSO;
				}
			}
			return null;
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x0028C6BC File Offset: 0x0028A8BC
		private void ApplyStaticConfigToSlots()
		{
			if (this.useArmsSlot && this.armsSlot != null && this.armsSlot.slotView != null)
			{
				this.armsSlot.slotView.SetCategory(this.armsSlot.categoryLabel);
				string text = ((!string.IsNullOrEmpty(this.armsSlot.keyCapOverride)) ? this.armsSlot.keyCapOverride : this.DeriveCoverKeyCap());
				if (ControllerInput.IsActive)
				{
					text = this.DeriveCoverKeyCap();
				}
				this.armsSlot.slotView.SetKeyCap(text);
				if (this.armsSlot.keyIcon != null)
				{
					this.armsSlot.slotView.SetKeyIcon(this.armsSlot.keyIcon);
				}
			}
			else if (this.armsSlot != null && this.armsSlot.slotView != null)
			{
				this.armsSlot.slotView.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.emoteSlots.Count; i++)
			{
				this.RefreshEmoteSlotView(i);
			}
			this.RefreshCrouchSlotView();
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x0028C7DC File Offset: 0x0028A9DC
		private void RefreshCrouchSlotView()
		{
			if (this.crouchSlot == null || this.crouchSlot.slotView == null)
			{
				return;
			}
			if (!this.useCrouchSlot)
			{
				this.crouchSlot.slotView.gameObject.SetActive(false);
				return;
			}
			bool flag = this.crouchSlot.RuntimeEmote != null;
			if (!flag && this.hideCrouchWhenUnbound)
			{
				this.crouchSlot.slotView.gameObject.SetActive(false);
				return;
			}
			this.crouchSlot.slotView.gameObject.SetActive(!this._tripodInputMode);
			this.crouchSlot.slotView.SetCategory(this.crouchSlot.categoryLabel);
			this.crouchSlot.slotView.SetActionName(flag ? RapidActionsHUDController.GetEmoteDisplayName(this.crouchSlot.RuntimeEmote) : string.Empty);
			string text = ((!string.IsNullOrEmpty(this.crouchSlot.keyCapOverride)) ? this.crouchSlot.keyCapOverride : ((this.crouchSlot.hotkey != null) ? this.crouchSlot.hotkey.buttonName : string.Empty));
			if (ControllerInput.IsActive && this.crouchSlot.hotkey != null)
			{
				text = this.crouchSlot.hotkey.buttonName;
			}
			this.crouchSlot.slotView.SetKeyCap(text);
			if (this.crouchSlot.keyIcon != null)
			{
				this.crouchSlot.slotView.SetKeyIcon(this.crouchSlot.keyIcon);
			}
		}

		// Token: 0x06006623 RID: 26147 RVA: 0x0028C964 File Offset: 0x0028AB64
		private void RefreshEmoteSlotView(int slotIndex)
		{
			if (slotIndex < 0 || slotIndex >= this.emoteSlots.Count)
			{
				return;
			}
			RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[slotIndex];
			if (rapidEmoteSlotConfig == null || rapidEmoteSlotConfig.slotView == null)
			{
				return;
			}
			bool flag = rapidEmoteSlotConfig.RuntimeEmote != null;
			if (!flag && this.hideEmoteSlotsWhenUnbound)
			{
				rapidEmoteSlotConfig.slotView.gameObject.SetActive(false);
				return;
			}
			rapidEmoteSlotConfig.slotView.gameObject.SetActive(!this._tripodInputMode);
			rapidEmoteSlotConfig.slotView.SetCategory(rapidEmoteSlotConfig.categoryLabel);
			rapidEmoteSlotConfig.slotView.SetActionName(flag ? RapidActionsHUDController.GetEmoteDisplayName(rapidEmoteSlotConfig.RuntimeEmote) : string.Empty);
			string text = ((!string.IsNullOrEmpty(rapidEmoteSlotConfig.keyCapOverride)) ? rapidEmoteSlotConfig.keyCapOverride : ((rapidEmoteSlotConfig.hotkey != null) ? rapidEmoteSlotConfig.hotkey.buttonName : string.Empty));
			if (ControllerInput.IsActive && rapidEmoteSlotConfig.hotkey != null)
			{
				text = rapidEmoteSlotConfig.hotkey.buttonName;
			}
			rapidEmoteSlotConfig.slotView.SetKeyCap(text);
			if (rapidEmoteSlotConfig.keyIcon != null)
			{
				rapidEmoteSlotConfig.slotView.SetKeyIcon(rapidEmoteSlotConfig.keyIcon);
			}
		}

		// Token: 0x06006624 RID: 26148 RVA: 0x0028CA8D File Offset: 0x0028AC8D
		private static string GetEmoteDisplayName(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(emote.DisplayName))
			{
				return emote.DisplayName;
			}
			if (!string.IsNullOrEmpty(emote.EmoteId))
			{
				return emote.EmoteId;
			}
			return emote.name;
		}

		// Token: 0x06006625 RID: 26149 RVA: 0x0028CACC File Offset: 0x0028ACCC
		private static string GetEmoteId(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(emote.EmoteId))
			{
				return emote.EmoteId;
			}
			return emote.name;
		}

		// Token: 0x06006626 RID: 26150 RVA: 0x0028CAF8 File Offset: 0x0028ACF8
		private EmoteDefinitionSO ResolveEmoteById(string id)
		{
			if (this.emoteCollection == null || string.IsNullOrEmpty(id))
			{
				return null;
			}
			List<EmoteDefinitionSO> unlockedEmotes = this.emoteCollection.GetUnlockedEmotes();
			for (int i = 0; i < unlockedEmotes.Count; i++)
			{
				if (!(unlockedEmotes[i] == null) && RapidActionsHUDController.GetEmoteId(unlockedEmotes[i]) == id)
				{
					return unlockedEmotes[i];
				}
			}
			return null;
		}

		// Token: 0x06006627 RID: 26151 RVA: 0x0028CB68 File Offset: 0x0028AD68
		private string DeriveCoverKeyCap()
		{
			if (this.playerInput == null || this.playerInput.coverPoseInput == null)
			{
				return string.Empty;
			}
			if (ControllerInput.IsActive)
			{
				return ControllerInput.ForKey(KeyCode.X);
			}
			return RapidActionsHUDController.ShortenInputBindingForKeycap(this.playerInput.coverPoseInput.keyboard);
		}

		// Token: 0x06006628 RID: 26152 RVA: 0x0028CBBC File Offset: 0x0028ADBC
		private static string ShortenInputBindingForKeycap(string raw)
		{
			if (string.IsNullOrEmpty(raw))
			{
				return string.Empty;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(raw);
			if (num <= 1231278590U)
			{
				if (num <= 865830204U)
				{
					if (num != 75071339U)
					{
						if (num == 865830204U)
						{
							if (raw == "Mouse0")
							{
								return "ML";
							}
						}
					}
					else if (raw == "LeftAlt")
					{
						return "LAlt";
					}
				}
				else if (num != 882607823U)
				{
					if (num != 899385442U)
					{
						if (num == 1231278590U)
						{
							if (raw == "RightControl")
							{
								return "RCtrl";
							}
						}
					}
					else if (raw == "Mouse2")
					{
						return "MM";
					}
				}
				else if (raw == "Mouse1")
				{
					return "MR";
				}
			}
			else if (num <= 3036628469U)
			{
				if (num != 1428210068U)
				{
					if (num != 2434225852U)
					{
						if (num == 3036628469U)
						{
							if (raw == "LeftControl")
							{
								return "LCtrl";
							}
						}
					}
					else if (raw == "RightAlt")
					{
						return "RAlt";
					}
				}
				else if (raw == "LeftShift")
				{
					return "LSh";
				}
			}
			else if (num != 3250860581U)
			{
				if (num != 3422663135U)
				{
					if (num == 3592460967U)
					{
						if (raw == "RightShift")
						{
							return "RSh";
						}
					}
				}
				else if (raw == "Return")
				{
					return "Ent";
				}
			}
			else if (raw == "Space")
			{
				return "_";
			}
			return raw;
		}

		// Token: 0x06006629 RID: 26153 RVA: 0x0028CD80 File Offset: 0x0028AF80
		private void Update()
		{
			if (this._lastControllerMode != ControllerInput.IsActive || this._lastControllerGlyphVersion != ControllerGlyphs.Version)
			{
				this._lastControllerMode = ControllerInput.IsActive;
				this._lastControllerGlyphVersion = ControllerGlyphs.Version;
				this.ApplyStaticConfigToSlots();
				if (this.IsRebindOpen)
				{
					this.RefreshPagingIndicators();
				}
			}
			this.RefreshLateDependencies();
			this.RefreshGatedSlotStates();
			if (DildoPlacementSystem.OwnsPlacementInput || VehicleItemPlacementSystem.OwnsPlacementInput)
			{
				if (this._activeSlot != null && this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen)
				{
					this.CloseRebindWheel(false);
					return;
				}
				if (this._activeSlot != null)
				{
					this.ResetSlot(this._activeSlot);
				}
				return;
			}
			else if (PlayerActivityGate.IsBlocked(PlayerActivity.RapidHUD))
			{
				if (this._activeSlot != null && this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen)
				{
					this.CloseRebindWheel(false);
					return;
				}
				if (this._activeSlot != null)
				{
					this.ResetSlot(this._activeSlot);
				}
				return;
			}
			else
			{
				this.HandleArmsSlotVisibility();
				if (this._activeSlot != null && this._activeSlot.Phase == RapidActionsHUDController.SlotPhase.RebindOpen)
				{
					RadialControllerNavigation.ShowControllerHint(this.rebindWheel, this._pagingEnabled ? "LB / RB" : null);
					this.TickRebindWheel();
					if (this._pagingEnabled)
					{
						if (this.rebindWheelLeft != null)
						{
							CanvasGroup component = this.rebindWheelLeft.GetComponent<CanvasGroup>();
							if (component != null)
							{
								component.alpha = this.GhostAlphaFor(this.rebindWheelLeft);
							}
						}
						if (this.rebindWheelRight != null)
						{
							CanvasGroup component2 = this.rebindWheelRight.GetComponent<CanvasGroup>();
							if (component2 != null)
							{
								component2.alpha = this.GhostAlphaFor(this.rebindWheelRight);
							}
						}
					}
				}
				if (ControllerInput.IsActive && ControllerInput.UiInputConsumedThisFrame)
				{
					return;
				}
				this.TickSlot(this._armsRuntime);
				for (int i = 0; i < this._emoteRuntimes.Count; i++)
				{
					this.TickSlot(this._emoteRuntimes[i]);
				}
				this.TickSlot(this._crouchRuntime);
				this.RefreshArmsSlot(false);
				return;
			}
		}

		// Token: 0x0600662A RID: 26154 RVA: 0x0028CF64 File Offset: 0x0028B164
		private void HandleArmsSlotVisibility()
		{
			if (!this.useArmsSlot || this.armsSlot == null || this.armsSlot.slotView == null)
			{
				return;
			}
			bool flag = !this._tripodInputMode;
			if (flag && this.hideArmsWhenCoverDisallowed && this.coverPoseController != null)
			{
				flag = this.coverPoseController.IsCoverAllowed;
				if (flag)
				{
					flag = this.coverPoseController.HasAvailablePose;
				}
			}
			if (this.armsSlot.slotView.gameObject.activeSelf != flag)
			{
				this.armsSlot.slotView.gameObject.SetActive(flag);
			}
		}

		// Token: 0x0600662B RID: 26155 RVA: 0x0028D004 File Offset: 0x0028B204
		private void RefreshArmsSlot(bool force)
		{
			if (!this.useArmsSlot || this.armsSlot == null || this.armsSlot.slotView == null)
			{
				return;
			}
			if (this.coverPoseController == null)
			{
				return;
			}
			string currentEmotionPoseLabel = this.coverPoseController.GetCurrentEmotionPoseLabel();
			if (!force && currentEmotionPoseLabel == this._lastArmsLabelPushed)
			{
				return;
			}
			this._lastArmsLabelPushed = currentEmotionPoseLabel;
			this.armsSlot.slotView.SetActionName(currentEmotionPoseLabel);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[RapidActions] Arms slot label -> '{0}' (exhibit={1}, idx={2})", currentEmotionPoseLabel, this.coverPoseController.IsExhibitDominant, this.coverPoseController.CurrentEmotionPoseTypeIndex));
			}
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x0028D0B4 File Offset: 0x0028B2B4
		private void TickSlot(RapidActionsHUDController.SlotRuntime slot)
		{
			if (slot == null)
			{
				return;
			}
			switch (slot.Phase)
			{
			case RapidActionsHUDController.SlotPhase.Idle:
				this.TickIdle(slot);
				return;
			case RapidActionsHUDController.SlotPhase.Pending:
				this.TickPending(slot);
				return;
			case RapidActionsHUDController.SlotPhase.RebindOpen:
				this.TickRebindOpen(slot);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x0028D0F8 File Offset: 0x0028B2F8
		private void TickIdle(RapidActionsHUDController.SlotRuntime slot)
		{
			if (this._activeSlot != null)
			{
				return;
			}
			if (!this.IsSlotInteractable(slot))
			{
				return;
			}
			if (!this.SlotKeyDown(slot))
			{
				return;
			}
			slot.PressStartUnscaledTime = Time.unscaledTime;
			slot.Phase = RapidActionsHUDController.SlotPhase.Pending;
			this._activeSlot = slot;
			if (this.debugMode)
			{
				Debug.Log("[RapidActions] " + this.DescribeSlot(slot) + " -> Pending");
			}
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x0028D160 File Offset: 0x0028B360
		private static bool IsSlotLongPressBlocked(RapidActionsHUDController.SlotRuntime slot)
		{
			switch (slot.Kind)
			{
			case RapidActionsHUDController.SlotKind.Arms:
				return PlayerActivityGate.IsBlocked(PlayerActivity.Cover | PlayerActivity.ArmsPose);
			case RapidActionsHUDController.SlotKind.Emote:
				return PlayerActivityGate.IsBlocked(PlayerActivity.Emote);
			case RapidActionsHUDController.SlotKind.Crouch:
				return PlayerActivityGate.IsBlocked(PlayerActivity.Crouch);
			default:
				return false;
			}
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x0028D1A8 File Offset: 0x0028B3A8
		private void TickPending(RapidActionsHUDController.SlotRuntime slot)
		{
			if (slot.Kind == RapidActionsHUDController.SlotKind.Arms && ControllerInput.IsActive && ControllerContextBindings.CoverPoseInputReserved)
			{
				this.ResetSlot(slot);
				return;
			}
			if (!this.SlotKeyHeld(slot))
			{
				this.ExecuteSlotAction(slot);
				this.ResetSlot(slot);
				return;
			}
			if (Time.unscaledTime - slot.PressStartUnscaledTime >= this.longPressThreshold)
			{
				if (RapidActionsHUDController.IsSlotLongPressBlocked(slot))
				{
					BlockedActionFeedback.Show(null);
					this.ResetSlot(slot);
					return;
				}
				if (!this.CanOpenRebindWheel(slot))
				{
					this.ResetSlot(slot);
					return;
				}
				this.OpenRebindWheel(slot);
			}
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x0028D230 File Offset: 0x0028B430
		private void TickRebindOpen(RapidActionsHUDController.SlotRuntime slot)
		{
			if (EscapeInputCoordinator.TryConsume(this) || GameplayInput.GetMouseButtonDown(1))
			{
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				this.CloseRebindWheel(false);
				return;
			}
			if (this._pagingEnabled && !this._slideInProgress)
			{
				if (ControllerInput.IsActive ? ControllerInput.GetButtonDown(ControllerButton.LeftShoulder) : (this.pagePrevInput != null && this.pagePrevInput.GetButtonDown()))
				{
					if (ControllerInput.IsActive)
					{
						ControllerInput.ConsumeUIInput();
					}
					this.BeginPageSwitch(-1);
					return;
				}
				if (ControllerInput.IsActive ? ControllerInput.GetButtonDown(ControllerButton.RightShoulder) : (this.pageNextInput != null && this.pageNextInput.GetButtonDown()))
				{
					if (ControllerInput.IsActive)
					{
						ControllerInput.ConsumeUIInput();
					}
					this.BeginPageSwitch(1);
					return;
				}
			}
			if (this._slideInProgress)
			{
				return;
			}
			if (!ControllerInput.SubmitDown)
			{
				if (!((ControllerInput.IsActive && this._controllerWheelUsesModifier) ? ControllerInput.Modifier : this.SlotKeyHeld(slot)))
				{
					if (ControllerInput.IsActive)
					{
						ControllerInput.ConsumeUIInput();
					}
					this.CloseRebindWheel(true);
				}
				return;
			}
			ControllerInput.ConsumeUIInput();
			if (this.rebindWheel == null || this.rebindWheel.CurrentButtonIndex < 0)
			{
				MainMenuUiFeedbackAudio.PlayDenied();
				return;
			}
			this.CloseRebindWheel(true);
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x0028D358 File Offset: 0x0028B558
		private void ResetSlot(RapidActionsHUDController.SlotRuntime slot)
		{
			slot.Phase = RapidActionsHUDController.SlotPhase.Idle;
			slot.CurrentOptions = null;
			if (this._activeSlot == slot)
			{
				this._activeSlot = null;
			}
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x0028D378 File Offset: 0x0028B578
		private bool IsSlotInteractable(RapidActionsHUDController.SlotRuntime slot)
		{
			switch (slot.Kind)
			{
			case RapidActionsHUDController.SlotKind.Arms:
				return this.useArmsSlot && this.armsSlot != null && !(this.armsSlot.slotView == null) && (this._tripodInputMode || this.armsSlot.slotView.gameObject.activeInHierarchy) && !(this.playerInput == null) && this.playerInput.coverPoseInput != null;
			case RapidActionsHUDController.SlotKind.Emote:
			{
				if (slot.EmoteSlotIndex < 0 || slot.EmoteSlotIndex >= this.emoteSlots.Count)
				{
					return false;
				}
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[slot.EmoteSlotIndex];
				return rapidEmoteSlotConfig != null && !(rapidEmoteSlotConfig.slotView == null) && (this._tripodInputMode || rapidEmoteSlotConfig.slotView.gameObject.activeInHierarchy) && rapidEmoteSlotConfig.hotkey != null;
			}
			case RapidActionsHUDController.SlotKind.Crouch:
				return this.useCrouchSlot && this.crouchSlot != null && !(this.crouchSlot.slotView == null) && (this._tripodInputMode || this.crouchSlot.slotView.gameObject.activeInHierarchy) && this.crouchSlot.hotkey != null;
			default:
				return false;
			}
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x0028D4CB File Offset: 0x0028B6CB
		private bool SlotKeyDown(RapidActionsHUDController.SlotRuntime slot)
		{
			if (slot.Kind != RapidActionsHUDController.SlotKind.Arms)
			{
				return slot.Hotkey != null && slot.Hotkey.GetButtonDown();
			}
			return this.ReadArmsInput(1);
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x0028D4F2 File Offset: 0x0028B6F2
		private bool SlotKeyHeld(RapidActionsHUDController.SlotRuntime slot)
		{
			if (slot.Kind != RapidActionsHUDController.SlotKind.Arms)
			{
				return slot.Hotkey != null && slot.Hotkey.GetButton();
			}
			return this.ReadArmsInput(0);
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x0028D51C File Offset: 0x0028B71C
		private bool ReadArmsInput(int phase)
		{
			if (this.playerInput == null || this.playerInput.coverPoseInput == null || !this.playerInput.coverPoseInput.useInput)
			{
				return false;
			}
			if (ControllerInput.IsActive)
			{
				return !TextInputFocus.BlocksGameplayInput && !ControllerContextBindings.CoverPoseInputReserved && ControllerGameplayBridge.Read(KeyCode.X, phase);
			}
			if (phase != 1)
			{
				return this.playerInput.coverPoseInput.GetButton();
			}
			return this.playerInput.coverPoseInput.GetButtonDown();
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x0028D5A0 File Offset: 0x0028B7A0
		private void ExecuteSlotAction(RapidActionsHUDController.SlotRuntime slot)
		{
			switch (slot.Kind)
			{
			case RapidActionsHUDController.SlotKind.Arms:
				if (this.playerInput == null)
				{
					return;
				}
				if (PlayerActivityGate.IsBlocked(PlayerActivity.Cover | PlayerActivity.ArmsPose) && (this.coverPoseController == null || !this.coverPoseController.IsCovering))
				{
					BlockedActionFeedback.Show(null);
					return;
				}
				this.playerInput.ToggleCoverPose();
				if (this.debugMode)
				{
					Debug.Log("[RapidActions] Arms tap -> ToggleCoverPose");
					return;
				}
				break;
			case RapidActionsHUDController.SlotKind.Emote:
			{
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[slot.EmoteSlotIndex];
				if (((rapidEmoteSlotConfig != null) ? rapidEmoteSlotConfig.RuntimeEmote : null) == null || this.gameplayEmoteController == null)
				{
					return;
				}
				if (this.gameplayEmoteController.IsPlaying)
				{
					this.gameplayEmoteController.RequestEndEmote();
					if (this.debugMode)
					{
						string text = "[RapidActions] Emote tap #{0} -> EndEmote (was '{1}')";
						object obj = slot.EmoteSlotIndex;
						EmoteDefinitionSO playingEmote = this.gameplayEmoteController.PlayingEmote;
						Debug.Log(string.Format(text, obj, (playingEmote != null) ? playingEmote.DisplayName : null));
						return;
					}
				}
				else
				{
					if (PlayerActivityGate.IsBlocked(PlayerActivity.Emote))
					{
						BlockedActionFeedback.Show(null);
						return;
					}
					bool flag = this.gameplayEmoteController.TryPlayRapidEmote(rapidEmoteSlotConfig.RuntimeEmote, false, false, true);
					if (this.debugMode)
					{
						Debug.Log(string.Format("[RapidActions] Emote tap #{0} '{1}' (played={2})", slot.EmoteSlotIndex, rapidEmoteSlotConfig.RuntimeEmote.DisplayName, flag));
						return;
					}
				}
				break;
			}
			case RapidActionsHUDController.SlotKind.Crouch:
			{
				RapidActionsHUDController.CrouchSlotConfig crouchSlotConfig = this.crouchSlot;
				if (((crouchSlotConfig != null) ? crouchSlotConfig.RuntimeEmote : null) == null || this.gameplayEmoteController == null)
				{
					return;
				}
				if (PlayerActivityGate.IsBlocked(PlayerActivity.Crouch))
				{
					return;
				}
				if (this.gameplayEmoteController.IsPlaying)
				{
					this.gameplayEmoteController.RequestEndEmote();
					if (this.debugMode)
					{
						string text2 = "[RapidActions] Crouch tap -> EndEmote (was '";
						EmoteDefinitionSO playingEmote2 = this.gameplayEmoteController.PlayingEmote;
						Debug.Log(text2 + ((playingEmote2 != null) ? playingEmote2.DisplayName : null) + "')");
						return;
					}
				}
				else
				{
					bool flag2 = this.gameplayEmoteController.TryPlayRapidEmote(this.crouchSlot.RuntimeEmote, true, true, false);
					if (this.debugMode)
					{
						Debug.Log(string.Format("[RapidActions] Crouch tap -> Play '{0}' (played={1})", this.crouchSlot.RuntimeEmote.DisplayName, flag2));
					}
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x0028D7D3 File Offset: 0x0028B9D3
		private bool CanOpenRebindWheel(RapidActionsHUDController.SlotRuntime slot)
		{
			return !EmoteWheelUI.AnyWheelOpen && !SelfieRadialMenuController.AnyWheelOpen && !(this.rebindWheel == null) && this.BuildOptionsFor(slot).Count > 0;
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x0028D804 File Offset: 0x0028BA04
		private void OpenRebindWheel(RapidActionsHUDController.SlotRuntime slot)
		{
			List<RapidActionsHUDController.RebindOption> list = this.BuildOptionsFor(slot);
			if (list.Count == 0)
			{
				this.ResetSlot(slot);
				return;
			}
			if (slot.Kind == RapidActionsHUDController.SlotKind.Emote && RankEmotePagePolicy.UnlockedPages(RankProgressionService.CurrentRank, 1) == 0)
			{
				this.ResetSlot(slot);
				return;
			}
			slot.CurrentOptions = list;
			slot.Phase = RapidActionsHUDController.SlotPhase.RebindOpen;
			this._controllerWheelUsesModifier = ControllerInput.IsActive && ControllerInput.Modifier;
			int count = this.rebindWheel.UltimateRadialButtonList.Count;
			bool flag = slot.Kind == RapidActionsHUDController.SlotKind.Emote && this.rebindWheelLeft != null && this.rebindWheelRight != null && count > 0 && list.Count > count;
			this._pagingEnabled = flag;
			this._pageSize = count;
			this._pageCount = (flag ? Mathf.CeilToInt((float)list.Count / (float)count) : 1);
			int num = 0;
			int num2;
			if (flag && this._emoteSlotLastPage.TryGetValue(slot.EmoteSlotIndex, out num2))
			{
				num = Mathf.Clamp(num2, 0, this._pageCount - 1);
			}
			this._currentPage = num;
			if (slot.Kind == RapidActionsHUDController.SlotKind.Emote && !this.IsPageAvailable(this._currentPage))
			{
				this._currentPage = 0;
			}
			this.CaptureWheelAnchorsIfNeeded();
			this.PopulateWheel(this.rebindWheel, list, this._currentPage);
			if (flag)
			{
				int num3 = this.WrapPage(this._currentPage - 1);
				int num4 = this.WrapPage(this._currentPage + 1);
				this.PopulateWheel(this.rebindWheelLeft, list, num3);
				this.PopulateWheel(this.rebindWheelRight, list, num4);
			}
			this._savedCursorLock = Cursor.lockState;
			this._savedCursorVisible = Cursor.visible;
			this._savedCameraLock = this.playerInput != null && this.playerInput.lockCameraInput;
			this._savedForceCameraLock = vThirdPersonInput.ForceCameraLock;
			this._wheelCameraCaptured = true;
			RapidActionsHUDController._wheelOwner = this;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			vThirdPersonInput.ForceCameraLock = true;
			if (this.playerInput != null)
			{
				this.playerInput.SetLockCameraInput(true);
			}
			this.EnableWheel(this.rebindWheel, false);
			if (flag)
			{
				this.EnableWheel(this.rebindWheelLeft, true);
				this.EnableWheel(this.rebindWheelRight, true);
				this.ResetWheelPositions();
				this.RefreshGhostVisibility(this._currentPage);
			}
			this.RefreshPagingIndicators();
			if (this.debugMode)
			{
				if (flag)
				{
					Debug.Log(string.Format("[RapidActions] {0} -> RebindOpen ({1} opts, {2} pages, start page={3})", new object[]
					{
						this.DescribeSlot(slot),
						list.Count,
						this._pageCount,
						this._currentPage
					}));
					return;
				}
				Debug.Log(string.Format("[RapidActions] {0} -> RebindOpen ({1} options)", this.DescribeSlot(slot), list.Count));
			}
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x0028DAB8 File Offset: 0x0028BCB8
		private void PopulateWheel(UltimateRadialMenu wheel, List<RapidActionsHUDController.RebindOption> options, int page)
		{
			if (wheel == null)
			{
				return;
			}
			RapidActionsHUDController.EnsureWheelDescription(wheel);
			wheel.ClearButtonInformations();
			RadialMenuTextLocalization radialMenuTextLocalization = RadialMenuTextLocalization.Begin(wheel);
			int count = wheel.UltimateRadialButtonList.Count;
			if (count <= 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				RadialCategoryMarker.SetVisible(wheel.UltimateRadialButtonList[i], false);
			}
			int num = page * count;
			int num2 = Mathf.Min(count, Mathf.Max(0, options.Count - num));
			for (int j = 0; j < num2; j++)
			{
				RapidActionsHUDController.RebindOption rebindOption = options[num + j];
				Sprite sprite = rebindOption.Icon;
				if (rebindOption.IsMasturbation && rebindOption.EmotePayload != null && rebindOption.EmotePayload.IconWithBadge != null)
				{
					sprite = rebindOption.EmotePayload.IconWithBadge;
				}
				if (sprite == null)
				{
					sprite = this.defaultRebindSliceSprite;
				}
				UltimateRadialButtonInfo ultimateRadialButtonInfo = new UltimateRadialButtonInfo
				{
					name = rebindOption.Label,
					description = (rebindOption.Description ?? string.Empty),
					icon = sprite,
					key = j.ToString()
				};
				wheel.RegisterButton(new Action(RapidActionsHUDController.NoOpCallback), ultimateRadialButtonInfo, j);
				radialMenuTextLocalization.BindButton(j, "gameplay_prompts", rebindOption.Label, rebindOption.Description);
				RadialCategoryMarker.SetVisible(wheel.UltimateRadialButtonList[j], rebindOption.IsMasturbation);
			}
			if (!this._pagingEnabled && wheel == this.rebindWheel && options.Count > count)
			{
				Debug.LogWarning(string.Format("[RapidActions] Rebind wheel only has {0} slots but {1} options were requested. Wire rebindWheelLeft + rebindWheelRight to enable pagination, or bump the wheel prefab's radial-button count.", count, options.Count));
			}
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x0028DC70 File Offset: 0x0028BE70
		private static void EnsureWheelDescription(UltimateRadialMenu wheel)
		{
			if (wheel.descriptionText == null && wheel.nameText != null)
			{
				Text component = new GameObject("ActionHint", new Type[]
				{
					typeof(RectTransform),
					typeof(Text)
				}).GetComponent<Text>();
				component.transform.SetParent(wheel.nameText.transform.parent, false);
				component.font = wheel.nameText.font;
				component.color = new Color(1f, 1f, 1f, 0.8f);
				component.alignment = TextAnchor.MiddleCenter;
				component.resizeTextForBestFit = true;
				component.resizeTextMinSize = 10;
				component.resizeTextMaxSize = 20;
				component.raycastTarget = false;
				wheel.descriptionText = component;
				wheel.descriptionTextSize = 0.16f;
				wheel.descriptionTextRatioX = 1.8f;
				wheel.descriptionTextRatioY = 0.6f;
				wheel.descriptionTextHorizontalPosition = 50f;
				wheel.descriptionTextVerticalPosition = 37f;
				wheel.displayButtonDescription = true;
				wheel.UpdatePositioning();
			}
			wheel.displayButtonDescription = wheel.descriptionText != null;
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x0028DD9D File Offset: 0x0028BF9D
		private void EnableWheel(UltimateRadialMenu wheel, bool ghost)
		{
			if (wheel == null)
			{
				return;
			}
			RadialControllerNavigation.TakeManualOwnership(wheel);
			if (!wheel.gameObject.activeSelf)
			{
				wheel.gameObject.SetActive(true);
			}
			wheel.Enable();
			this.ApplyGhostState(wheel, ghost);
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x0028DDD8 File Offset: 0x0028BFD8
		private float GhostAlphaFor(UltimateRadialMenu wheel)
		{
			int num = this.WrapPage(this._currentPage + ((wheel == this.rebindWheelLeft) ? (-1) : 1));
			if (!this.IsPageAvailable(num))
			{
				return this.pageGhostAlpha * 0.35f;
			}
			return this.pageGhostAlpha;
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x0028DE24 File Offset: 0x0028C024
		private void ApplyGhostState(UltimateRadialMenu wheel, bool ghost)
		{
			if (wheel == null)
			{
				return;
			}
			CanvasGroup component = wheel.GetComponent<CanvasGroup>();
			if (component == null)
			{
				return;
			}
			if (ghost)
			{
				component.alpha = this.GhostAlphaFor(wheel);
				component.interactable = false;
				component.blocksRaycasts = false;
				return;
			}
			component.alpha = 1f;
			component.interactable = true;
			component.blocksRaycasts = true;
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x0028DE84 File Offset: 0x0028C084
		private void CaptureWheelAnchorsIfNeeded()
		{
			if (this._wheelAnchorsCaptured)
			{
				return;
			}
			if (this.rebindWheel == null)
			{
				return;
			}
			RectTransform rectTransform = this.rebindWheel.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			this._centerAnchored = rectTransform.anchoredPosition;
			if (this.rebindWheelLeft != null)
			{
				RectTransform rectTransform2 = this.rebindWheelLeft.transform as RectTransform;
				if (rectTransform2 != null)
				{
					this._leftAnchored = rectTransform2.anchoredPosition;
				}
			}
			if (this.rebindWheelRight != null)
			{
				RectTransform rectTransform3 = this.rebindWheelRight.transform as RectTransform;
				if (rectTransform3 != null)
				{
					this._rightAnchored = rectTransform3.anchoredPosition;
				}
			}
			this._wheelAnchorsCaptured = true;
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x0028DF34 File Offset: 0x0028C134
		private void ResetWheelPositions()
		{
			if (this.rebindWheel != null)
			{
				RectTransform rectTransform = this.rebindWheel.transform as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.anchoredPosition = this._centerAnchored;
				}
			}
			if (this.rebindWheelLeft != null)
			{
				RectTransform rectTransform2 = this.rebindWheelLeft.transform as RectTransform;
				if (rectTransform2 != null)
				{
					rectTransform2.anchoredPosition = this._leftAnchored;
				}
			}
			if (this.rebindWheelRight != null)
			{
				RectTransform rectTransform3 = this.rebindWheelRight.transform as RectTransform;
				if (rectTransform3 != null)
				{
					rectTransform3.anchoredPosition = this._rightAnchored;
				}
			}
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x0028DFCC File Offset: 0x0028C1CC
		private int WrapPage(int p)
		{
			if (this._pageCount <= 0)
			{
				return 0;
			}
			int num = p % this._pageCount;
			if (num >= 0)
			{
				return num;
			}
			return num + this._pageCount;
		}

		// Token: 0x06006641 RID: 26177 RVA: 0x0028DFFC File Offset: 0x0028C1FC
		private void RefreshPagingIndicators()
		{
			bool flag = this._pagingEnabled && this._pageCount > 1;
			bool flag2 = !ControllerInput.IsActive;
			bool flag3 = flag && this.IsPageAvailable(this.WrapPage(this._currentPage - 1));
			bool flag4 = flag && this.IsPageAvailable(this.WrapPage(this._currentPage + 1));
			if (this.pagePrevIndicator != null)
			{
				this.pagePrevIndicator.SetActive(flag3 && flag2);
			}
			if (this.pageNextIndicator != null)
			{
				this.pageNextIndicator.SetActive(flag4 && flag2);
			}
			this.RefreshPageAccessLabel(flag);
		}

		// Token: 0x06006642 RID: 26178 RVA: 0x0028E09C File Offset: 0x0028C29C
		private bool IsPageAvailable(int page)
		{
			return RankEmotePagePolicy.IsPageAvailable(RankProgressionService.CurrentRank, page, this._pageCount);
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x0028E0B0 File Offset: 0x0028C2B0
		private void RefreshPageAccessLabel(bool show)
		{
			if (this._pageAccessLabel == null && show && this.rebindWheel != null)
			{
				GameObject gameObject = new GameObject("Emote page unlocks", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.rebindWheel.transform, false);
				Vector2 vector = new Vector2(0.5f, 0f);
				component.anchorMax = vector;
				component.anchorMin = vector;
				component.pivot = new Vector2(0.5f, 1f);
				component.anchoredPosition = new Vector2(0f, -24f);
				component.sizeDelta = new Vector2(560f, 60f);
				this._pageAccessLabel = gameObject.GetComponent<TMP_Text>();
				this._pageAccessLabel.fontSize = 17f;
				this._pageAccessLabel.alignment = TextAlignmentOptions.Center;
				this._pageAccessLabel.raycastTarget = false;
			}
			if (this._pageAccessLabel == null)
			{
				return;
			}
			this._pageAccessLabel.gameObject.SetActive(show);
			if (!show)
			{
				return;
			}
			if (this._pageAccessBinding == null)
			{
				this._pageAccessBinding = LocalizedGameText.BindComputed(this._pageAccessLabel, new Func<string>(this.BuildPageAccessLabel));
				this._pageAccessBinding.FitTranslationToRectangle(false);
				return;
			}
			this._pageAccessLabel.text = this.BuildPageAccessLabel();
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x0028E228 File Offset: 0x0028C428
		private string BuildPageAccessLabel()
		{
			if (this._pageCount <= 0)
			{
				return string.Empty;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this._pageCount; i++)
			{
				list.Add((i == this._currentPage) ? string.Format("<b>[{0}]</b>", i + 1) : (this.IsPageAvailable(i) ? (i + 1).ToString() : ("<color=#999999>" + GameLocalization.FormatSource("gameplay_prompts", "{0} Locked", new object[] { i + 1 }) + "</color>")));
			}
			return string.Join("    ", list);
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x0028E2D0 File Offset: 0x0028C4D0
		private void RefreshGhostVisibility(int page)
		{
			if (!this._pagingEnabled)
			{
				return;
			}
			bool flag = page > 0;
			bool flag2 = page < this._pageCount - 1;
			if (this.rebindWheelLeft != null && this.rebindWheelLeft.gameObject.activeSelf != flag)
			{
				this.rebindWheelLeft.gameObject.SetActive(flag);
			}
			if (this.rebindWheelRight != null && this.rebindWheelRight.gameObject.activeSelf != flag2)
			{
				this.rebindWheelRight.gameObject.SetActive(flag2);
			}
		}

		// Token: 0x06006646 RID: 26182 RVA: 0x0028E35C File Offset: 0x0028C55C
		private void BeginPageSwitch(int direction)
		{
			if (!this._pagingEnabled || this._slideInProgress)
			{
				return;
			}
			if (this.rebindWheel == null || this.rebindWheelLeft == null || this.rebindWheelRight == null)
			{
				return;
			}
			int num = this.WrapPage(this._currentPage + direction);
			if (num == this._currentPage)
			{
				return;
			}
			if (!this.IsPageAvailable(num))
			{
				return;
			}
			if (this._pageSlideCoroutine != null)
			{
				base.StopCoroutine(this._pageSlideCoroutine);
			}
			this._pageSlideCoroutine = base.StartCoroutine(this.PageSlideRoutine(direction, num));
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x0028E3F0 File Offset: 0x0028C5F0
		private IEnumerator PageSlideRoutine(int direction, int newPage)
		{
			this._slideInProgress = true;
			if (this.rebindWheelLeft != null && !this.rebindWheelLeft.gameObject.activeSelf)
			{
				this.rebindWheelLeft.gameObject.SetActive(true);
			}
			if (this.rebindWheelRight != null && !this.rebindWheelRight.gameObject.activeSelf)
			{
				this.rebindWheelRight.gameObject.SetActive(true);
			}
			RectTransform centerRT = this.rebindWheel.transform as RectTransform;
			RectTransform leftRT = this.rebindWheelLeft.transform as RectTransform;
			RectTransform rightRT = this.rebindWheelRight.transform as RectTransform;
			Vector2 centerStart = centerRT.anchoredPosition;
			Vector2 leftStart = leftRT.anchoredPosition;
			Vector2 rightStart = rightRT.anchoredPosition;
			Vector2 centerEnd;
			Vector2 rightEnd;
			Vector2 leftEnd;
			if (direction > 0)
			{
				centerEnd = this._leftAnchored;
				rightEnd = this._centerAnchored;
				leftEnd = this._leftAnchored + (this._leftAnchored - this._centerAnchored);
			}
			else
			{
				centerEnd = this._rightAnchored;
				leftEnd = this._centerAnchored;
				rightEnd = this._rightAnchored + (this._rightAnchored - this._centerAnchored);
			}
			float t = 0f;
			float dur = Mathf.Max(0.01f, this.pageSlideDuration);
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / dur));
				centerRT.anchoredPosition = Vector2.LerpUnclamped(centerStart, centerEnd, num);
				leftRT.anchoredPosition = Vector2.LerpUnclamped(leftStart, leftEnd, num);
				rightRT.anchoredPosition = Vector2.LerpUnclamped(rightStart, rightEnd, num);
				yield return null;
			}
			this._currentPage = newPage;
			this.ResetWheelPositions();
			RapidActionsHUDController.SlotRuntime activeSlot = this._activeSlot;
			if (activeSlot != null && activeSlot.CurrentOptions != null)
			{
				int num2 = this.WrapPage(this._currentPage - 1);
				int num3 = this.WrapPage(this._currentPage + 1);
				this.PopulateWheel(this.rebindWheel, activeSlot.CurrentOptions, this._currentPage);
				this.PopulateWheel(this.rebindWheelLeft, activeSlot.CurrentOptions, num2);
				this.PopulateWheel(this.rebindWheelRight, activeSlot.CurrentOptions, num3);
				this.ApplyGhostState(this.rebindWheel, false);
				this.ApplyGhostState(this.rebindWheelLeft, true);
				this.ApplyGhostState(this.rebindWheelRight, true);
			}
			this.RefreshGhostVisibility(this._currentPage);
			this.RefreshPagingIndicators();
			this._slideInProgress = false;
			this._pageSlideCoroutine = null;
			if (this.debugMode)
			{
				Debug.Log(string.Format("[RapidActions] Page switch -> {0}/{1}", this._currentPage, this._pageCount - 1));
			}
			yield break;
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x0028E410 File Offset: 0x0028C610
		private void CloseRebindWheel(bool commit)
		{
			RapidActionsHUDController.SlotRuntime activeSlot = this._activeSlot;
			if (activeSlot != null && commit && activeSlot.CurrentOptions != null)
			{
				int num = ((this.rebindWheel != null) ? this.rebindWheel.CurrentButtonIndex : (-1));
				int num2 = -1;
				if (num >= 0)
				{
					num2 = (this._pagingEnabled ? (this._currentPage * this._pageSize) : 0) + num;
				}
				if (num2 >= 0 && num2 < activeSlot.CurrentOptions.Count)
				{
					this.CommitRebind(activeSlot, activeSlot.CurrentOptions[num2]);
				}
				else if (this.debugMode)
				{
					Debug.Log("[RapidActions] " + this.DescribeSlot(activeSlot) + " rebind cancelled (no slice hovered)");
				}
			}
			if (activeSlot != null && this._pagingEnabled && activeSlot.Kind == RapidActionsHUDController.SlotKind.Emote)
			{
				this._emoteSlotLastPage[activeSlot.EmoteSlotIndex] = this._currentPage;
			}
			if (this._pageSlideCoroutine != null)
			{
				base.StopCoroutine(this._pageSlideCoroutine);
				this._pageSlideCoroutine = null;
			}
			this._slideInProgress = false;
			this.DisableAndClearWheel(this.rebindWheel);
			if (this._pagingEnabled)
			{
				this.DisableAndClearWheel(this.rebindWheelLeft);
				this.DisableAndClearWheel(this.rebindWheelRight);
				this.ResetWheelPositions();
			}
			this._pagingEnabled = false;
			this.RefreshPagingIndicators();
			if (this._wheelCameraCaptured)
			{
				vThirdPersonInput.ForceCameraLock = this._savedForceCameraLock;
				if (this.playerInput != null)
				{
					this.playerInput.SetLockCameraInput(this._savedCameraLock);
				}
				Cursor.lockState = this._savedCursorLock;
				Cursor.visible = this._savedCursorVisible;
				this._wheelCameraCaptured = false;
			}
			if (RapidActionsHUDController._wheelOwner == this)
			{
				RapidActionsHUDController._wheelOwner = null;
			}
			if (activeSlot != null)
			{
				this.ResetSlot(activeSlot);
			}
		}

		// Token: 0x06006649 RID: 26185 RVA: 0x0028E5BA File Offset: 0x0028C7BA
		private void DisableAndClearWheel(UltimateRadialMenu wheel)
		{
			if (wheel == null)
			{
				return;
			}
			if (wheel.IsEnabled)
			{
				wheel.Disable();
			}
			wheel.ClearButtonInformations();
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x00004F31 File Offset: 0x00003131
		private static void NoOpCallback()
		{
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x0028E5DC File Offset: 0x0028C7DC
		private void TickRebindWheel()
		{
			if (this.rebindWheel == null)
			{
				return;
			}
			if (!this.rebindWheel.IsEnabled)
			{
				return;
			}
			if (RadialControllerNavigation.Process(this.rebindWheel, true))
			{
				return;
			}
			RadialControllerNavigation.ProcessPointer(this.rebindWheel, GameplayInput.mousePosition);
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x0028E62C File Offset: 0x0028C82C
		private void CommitRebind(RapidActionsHUDController.SlotRuntime slot, RapidActionsHUDController.RebindOption option)
		{
			switch (slot.Kind)
			{
			case RapidActionsHUDController.SlotKind.Arms:
			{
				if (this.coverPoseController == null)
				{
					return;
				}
				bool flag = (this._armsSceneLocked ? slot.ArmsExhibitSnapshot : option.ArmsExhibit);
				if (!this.coverPoseController.IsPoseAvailable(flag, option.IntPayload))
				{
					return;
				}
				if (this._armsSceneLocked)
				{
					bool armsExhibitSnapshot = slot.ArmsExhibitSnapshot;
					this.coverPoseController.SetEmotionPoseTypeIndex(armsExhibitSnapshot, option.IntPayload);
					RapidActionsPersistence.SaveArmsPoseIndex(armsExhibitSnapshot, option.IntPayload);
					this.RefreshArmsSlot(true);
				}
				else
				{
					this.ApplyUnifiedArmsPose(option.ArmsExhibit, option.IntPayload, true);
				}
				if (this.debugMode)
				{
					Debug.Log(string.Format("[RapidActions] Arms rebind committed: {0} -> idx {1} ('{2}')", option.ArmsExhibit ? "Confident" : "Embarrassed", option.IntPayload, option.Label));
					return;
				}
				break;
			}
			case RapidActionsHUDController.SlotKind.Emote:
			{
				if (!this.IsEmoteAllowed(option.EmotePayload, false))
				{
					return;
				}
				if (option.EmotePayload == null)
				{
					return;
				}
				RapidActionsHUDController.RapidEmoteSlotConfig rapidEmoteSlotConfig = this.emoteSlots[slot.EmoteSlotIndex];
				if (rapidEmoteSlotConfig == null)
				{
					return;
				}
				rapidEmoteSlotConfig.RuntimeEmote = option.EmotePayload;
				this.RefreshEmoteSlotView(slot.EmoteSlotIndex);
				this.InvalidateMasturbationSlotStates();
				RapidActionsPersistence.SaveEmoteSlot(slot.EmoteSlotIndex, this.CurrentFutanari(), RapidActionsHUDController.GetEmoteId(option.EmotePayload));
				if (this.debugMode)
				{
					Debug.Log(string.Format("[RapidActions] Emote rebind committed: slot #{0} -> '{1}'", slot.EmoteSlotIndex, option.Label));
					return;
				}
				break;
			}
			case RapidActionsHUDController.SlotKind.Crouch:
				if (option.EmotePayload == null || this.crouchSlot == null)
				{
					return;
				}
				this.crouchSlot.RuntimeEmote = option.EmotePayload;
				this.RefreshCrouchSlotView();
				RapidActionsPersistence.SaveCrouchEmoteId(this.CurrentFutanari(), RapidActionsHUDController.GetEmoteId(option.EmotePayload));
				if (this.debugMode)
				{
					Debug.Log("[RapidActions] Crouch rebind committed: -> '" + option.Label + "'");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x0028E818 File Offset: 0x0028CA18
		private List<RapidActionsHUDController.RebindOption> BuildOptionsFor(RapidActionsHUDController.SlotRuntime slot)
		{
			List<RapidActionsHUDController.RebindOption> list = new List<RapidActionsHUDController.RebindOption>();
			switch (slot.Kind)
			{
			case RapidActionsHUDController.SlotKind.Arms:
				this.BuildArmsOptions(list);
				break;
			case RapidActionsHUDController.SlotKind.Emote:
				this.BuildEmoteOptions(list);
				break;
			case RapidActionsHUDController.SlotKind.Crouch:
				this.BuildCrouchOptions(list);
				break;
			}
			return list;
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x0028E860 File Offset: 0x0028CA60
		private void BuildCrouchOptions(List<RapidActionsHUDController.RebindOption> result)
		{
			if (this.crouchSlot == null || this.crouchSlot.crouchEmotes == null)
			{
				return;
			}
			for (int i = 0; i < this.crouchSlot.crouchEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = this.crouchSlot.crouchEmotes[i];
				if (!(emoteDefinitionSO == null) && this.IsCrouchEmoteAllowed(emoteDefinitionSO))
				{
					result.Add(new RapidActionsHUDController.RebindOption
					{
						Label = RapidActionsHUDController.GetEmoteDisplayName(emoteDefinitionSO),
						Icon = emoteDefinitionSO.Icon,
						EmotePayload = emoteDefinitionSO,
						IsMasturbation = emoteDefinitionSO.IsMasturbationEmote
					});
				}
			}
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x0028E900 File Offset: 0x0028CB00
		private void BuildArmsOptions(List<RapidActionsHUDController.RebindOption> result)
		{
			if (this.coverPoseController == null)
			{
				return;
			}
			if (this._armsSceneLocked)
			{
				bool isExhibitDominant = this.coverPoseController.IsExhibitDominant;
				if (this._armsRuntime != null)
				{
					this._armsRuntime.ArmsExhibitSnapshot = isExhibitDominant;
				}
				int emotionPoseTypeCount = this.coverPoseController.GetEmotionPoseTypeCount(isExhibitDominant);
				for (int i = 0; i < emotionPoseTypeCount; i++)
				{
					if (this.coverPoseController.IsPoseAvailable(isExhibitDominant, i))
					{
						result.Add(new RapidActionsHUDController.RebindOption
						{
							Label = this.coverPoseController.GetEmotionPoseLabel(isExhibitDominant, i),
							Description = this.GetCoverActionHint(isExhibitDominant, i),
							IntPayload = i,
							ArmsExhibit = isExhibitDominant
						});
					}
				}
				return;
			}
			for (int j = 0; j < 2; j++)
			{
				bool flag = j == 1;
				int emotionPoseTypeCount2 = this.coverPoseController.GetEmotionPoseTypeCount(flag);
				for (int k = 0; k < emotionPoseTypeCount2; k++)
				{
					if (this.coverPoseController.IsPoseAvailable(flag, k))
					{
						result.Add(new RapidActionsHUDController.RebindOption
						{
							Label = this.coverPoseController.GetEmotionPoseLabel(flag, k),
							Description = this.GetCoverActionHint(flag, k),
							IntPayload = k,
							ArmsExhibit = flag
						});
					}
				}
			}
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x0028EA48 File Offset: 0x0028CC48
		private string GetCoverActionHint(bool exhibit, int index)
		{
			if (!(this.coverPoseController != null) || !this.coverPoseController.CanUseLowerBodyCover(exhibit, index))
			{
				return string.Empty;
			}
			if (!ControllerInput.IsActive)
			{
				return "Cover lower body\nMouse left";
			}
			return "Cover lower body\n" + ControllerGlyphs.Plain("RB", ControllerGlyphs.Family);
		}

		// Token: 0x06006651 RID: 26193 RVA: 0x0028EAA0 File Offset: 0x0028CCA0
		private void BuildEmoteOptions(List<RapidActionsHUDController.RebindOption> result)
		{
			if (this.emoteCollection == null)
			{
				return;
			}
			List<EmoteDefinitionSO> unlockedEmotes = this.emoteCollection.GetUnlockedEmotes();
			for (int i = 0; i < unlockedEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = unlockedEmotes[i];
				if (!(emoteDefinitionSO == null) && RankEmotePagePolicy.MatchesAvatar(emoteDefinitionSO, this.CurrentFutanari()))
				{
					result.Add(new RapidActionsHUDController.RebindOption
					{
						Label = RapidActionsHUDController.GetEmoteDisplayName(emoteDefinitionSO),
						Icon = emoteDefinitionSO.Icon,
						EmotePayload = emoteDefinitionSO,
						IsMasturbation = emoteDefinitionSO.IsMasturbationEmote
					});
				}
			}
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x0028EB38 File Offset: 0x0028CD38
		private bool IsEmoteAllowed(EmoteDefinitionSO emote, bool forBinding = false)
		{
			bool flag = this.CurrentFutanari();
			if (!RankEmotePagePolicy.MatchesAvatar(emote, flag))
			{
				return false;
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			if (forBinding && RankEmotePagePolicy.UnlockedPages(currentRank, 1) == 0)
			{
				return RankEmotePagePolicy.PageOf(this.emoteCollection, emote, flag) == 0;
			}
			return RankEmotePagePolicy.IsEmoteAvailable(currentRank, this.emoteCollection, emote, flag) && (currentRank == null || !emote.IsMasturbationEmote || currentRank.allowMasturbationEmotes);
		}

		// Token: 0x06006653 RID: 26195 RVA: 0x0028EBA5 File Offset: 0x0028CDA5
		private bool IsCrouchEmoteAllowed(EmoteDefinitionSO emote)
		{
			return RankEmotePagePolicy.MatchesAvatar(emote, this.CurrentFutanari()) && (this.gameplayEmoteController == null || this.gameplayEmoteController.IsEmoteAllowedByRank(emote, false));
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x0028EBD4 File Offset: 0x0028CDD4
		private string DescribeSlot(RapidActionsHUDController.SlotRuntime slot)
		{
			if (slot == null)
			{
				return "<null>";
			}
			if (slot.Kind == RapidActionsHUDController.SlotKind.Arms)
			{
				return "Arms";
			}
			if (slot.Kind == RapidActionsHUDController.SlotKind.Crouch)
			{
				return "Crouch";
			}
			return string.Format("Emote#{0}", slot.EmoteSlotIndex);
		}

		// Token: 0x04006876 RID: 26742
		private const PlayerActivity ArmsPoseBlockMask = PlayerActivity.Cover | PlayerActivity.ArmsPose;

		// Token: 0x04006877 RID: 26743
		[Header("Slots")]
		[SerializeField]
		private bool useArmsSlot = true;

		// Token: 0x04006878 RID: 26744
		[SerializeField]
		private RapidActionsHUDController.ArmsSlotConfig armsSlot = new RapidActionsHUDController.ArmsSlotConfig();

		// Token: 0x04006879 RID: 26745
		[Tooltip("Emote bindings shown to the right of the Arms row.")]
		[SerializeField]
		private List<RapidActionsHUDController.RapidEmoteSlotConfig> emoteSlots = new List<RapidActionsHUDController.RapidEmoteSlotConfig>(2);

		// Token: 0x0400687A RID: 26746
		[SerializeField]
		private bool useCrouchSlot = true;

		// Token: 0x0400687B RID: 26747
		[SerializeField]
		private RapidActionsHUDController.CrouchSlotConfig crouchSlot = new RapidActionsHUDController.CrouchSlotConfig();

		// Token: 0x0400687C RID: 26748
		[Header("Long-press Rebind")]
		[Tooltip("Seconds the hotkey must be held to open the rebind wheel. Below this, the press counts as a tap and runs the action.")]
		[Range(0.1f, 1f)]
		[SerializeField]
		private float longPressThreshold = 0.3f;

		// Token: 0x0400687D RID: 26749
		[Tooltip("Radial menu prefab used as the rebind wheel. A single instance is reused for every slot.")]
		[SerializeField]
		private UltimateRadialMenu rebindWheel;

		// Token: 0x0400687E RID: 26750
		[Tooltip("If assigned, all rebind wheel slices use this sprite as the icon when an option-specific icon is missing.")]
		[SerializeField]
		private Sprite defaultRebindSliceSprite;

		// Token: 0x0400687F RID: 26751
		[Tooltip("Where to source the emote list for emote-slot rebinds. Required when at least one emote slot is in use.")]
		[SerializeField]
		private GameplayEmoteCollectionSO emoteCollection;

		// Token: 0x04006880 RID: 26752
		[Header("Paged Rebind Wheels (Emote slots)")]
		[Tooltip("Optional second wheel instance shown to the LEFT of the central one as a non-interactive 'previous page' preview. Required to enable pagination.")]
		[SerializeField]
		private UltimateRadialMenu rebindWheelLeft;

		// Token: 0x04006881 RID: 26753
		[Tooltip("Optional second wheel instance shown to the RIGHT of the central one as a non-interactive 'next page' preview. Required to enable pagination.")]
		[SerializeField]
		private UltimateRadialMenu rebindWheelRight;

		// Token: 0x04006882 RID: 26754
		[Tooltip("Input that switches the rebind wheel to the previous page (only while the wheel is open).")]
		[SerializeField]
		private GenericInput pagePrevInput = new GenericInput("Z", "DPadLeft", "DPadLeft");

		// Token: 0x04006883 RID: 26755
		[Tooltip("Input that switches the rebind wheel to the next page (only while the wheel is open).")]
		[SerializeField]
		private GenericInput pageNextInput = new GenericInput("X", "DPadRight", "DPadRight");

		// Token: 0x04006884 RID: 26756
		[Tooltip("Alpha applied to the side preview wheels (the two non-interactive ghosts).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float pageGhostAlpha = 0.4f;

		// Token: 0x04006885 RID: 26757
		[Tooltip("Seconds the page-switch slide animation lasts. Hotkey release is ignored during the slide so a release in mid-slide cannot commit a stale slice.")]
		[Range(0.05f, 0.5f)]
		[SerializeField]
		private float pageSlideDuration = 0.15f;

		// Token: 0x04006886 RID: 26758
		[Tooltip("Optional graphical indicator (e.g. keycap art for Z) shown on the central wheel when a previous page exists. Purely decorative — no click logic. Hidden when only one page is available.")]
		[SerializeField]
		private GameObject pagePrevIndicator;

		// Token: 0x04006887 RID: 26759
		[Tooltip("Optional graphical indicator (e.g. keycap art for X) shown on the central wheel when a next page exists. Purely decorative — no click logic. Hidden when only one page is available.")]
		[SerializeField]
		private GameObject pageNextIndicator;

		// Token: 0x04006888 RID: 26760
		[Header("Dependencies (auto-found)")]
		[SerializeField]
		private CoverPoseController coverPoseController;

		// Token: 0x04006889 RID: 26761
		[SerializeField]
		private GameplayEmoteController gameplayEmoteController;

		// Token: 0x0400688A RID: 26762
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x0400688B RID: 26763
		[Header("Unified Arms Poses")]
		[Tooltip("Label of the pose pinned on first launch when no unified choice has been saved yet (matched case-insensitive across both emotion pose lists).")]
		[SerializeField]
		private string defaultUnifiedArmsPoseLabel = "Stimulation";

		// Token: 0x0400688C RID: 26764
		[Header("Hide When Unavailable")]
		[Tooltip("Hide the Arms row when the player is fully clothed or cover is disallowed (vehicle restriction).")]
		[SerializeField]
		private bool hideArmsWhenCoverDisallowed = true;

		// Token: 0x0400688D RID: 26765
		[Tooltip("Hide an emote slot when its bound emote is null or the gameplay emote system is missing.")]
		[SerializeField]
		private bool hideEmoteSlotsWhenUnbound = true;

		// Token: 0x0400688E RID: 26766
		[Tooltip("Hide the Crouch slot when its bound action is null or the gameplay emote system is missing.")]
		[SerializeField]
		private bool hideCrouchWhenUnbound = true;

		// Token: 0x0400688F RID: 26767
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04006890 RID: 26768
		private RapidActionsHUDController.SlotRuntime _armsRuntime;

		// Token: 0x04006891 RID: 26769
		private readonly List<RapidActionsHUDController.SlotRuntime> _emoteRuntimes = new List<RapidActionsHUDController.SlotRuntime>();

		// Token: 0x04006892 RID: 26770
		private RapidActionsHUDController.SlotRuntime _crouchRuntime;

		// Token: 0x04006893 RID: 26771
		private RapidActionsHUDController.SlotRuntime _activeSlot;

		// Token: 0x04006894 RID: 26772
		private bool _tripodInputMode;

		// Token: 0x04006895 RID: 26773
		private CursorLockMode _savedCursorLock;

		// Token: 0x04006896 RID: 26774
		private bool _savedCursorVisible;

		// Token: 0x04006897 RID: 26775
		private bool _savedCameraLock;

		// Token: 0x04006898 RID: 26776
		private bool _savedForceCameraLock;

		// Token: 0x04006899 RID: 26777
		private bool _wheelCameraCaptured;

		// Token: 0x0400689A RID: 26778
		private bool _lastControllerMode;

		// Token: 0x0400689B RID: 26779
		private int _lastControllerGlyphVersion = -1;

		// Token: 0x0400689C RID: 26780
		private static RapidActionsHUDController _wheelOwner;

		// Token: 0x0400689D RID: 26781
		private string _lastArmsLabelPushed;

		// Token: 0x0400689E RID: 26782
		private PlayerActivity _displayedBlockedMask = (PlayerActivity)(-1);

		// Token: 0x0400689F RID: 26783
		private int _displayedMasturbationBlockVersion = -1;

		// Token: 0x040068A0 RID: 26784
		private float _nextDependencyCheck;

		// Token: 0x040068A1 RID: 26785
		private bool _armsSceneLocked;

		// Token: 0x040068A2 RID: 26786
		private bool _armsPrefsApplied;

		// Token: 0x040068A3 RID: 26787
		private readonly Dictionary<int, int> _emoteSlotLastPage = new Dictionary<int, int>();

		// Token: 0x040068A4 RID: 26788
		private int _currentPage;

		// Token: 0x040068A5 RID: 26789
		private int _pageCount;

		// Token: 0x040068A6 RID: 26790
		private int _pageSize;

		// Token: 0x040068A7 RID: 26791
		private bool _pagingEnabled;

		// Token: 0x040068A8 RID: 26792
		private bool _controllerWheelUsesModifier;

		// Token: 0x040068A9 RID: 26793
		private TMP_Text _pageAccessLabel;

		// Token: 0x040068AA RID: 26794
		private LocalizedGameText _pageAccessBinding;

		// Token: 0x040068AB RID: 26795
		private Coroutine _pageSlideCoroutine;

		// Token: 0x040068AC RID: 26796
		private bool _slideInProgress;

		// Token: 0x040068AD RID: 26797
		private Vector2 _centerAnchored;

		// Token: 0x040068AE RID: 26798
		private Vector2 _leftAnchored;

		// Token: 0x040068AF RID: 26799
		private Vector2 _rightAnchored;

		// Token: 0x040068B0 RID: 26800
		private bool _wheelAnchorsCaptured;

		// Token: 0x02000D8D RID: 3469
		[Serializable]
		public class ArmsSlotConfig
		{
			// Token: 0x040068B1 RID: 26801
			[Tooltip("Slot view shown for the Arms (cover pose) row. Mandatory if useArmsSlot=true.")]
			public RapidActionSlotUI slotView;

			// Token: 0x040068B2 RID: 26802
			[Tooltip("Static category prefix shown before the pose name (e.g. 'Arms:').")]
			public string categoryLabel = "Arms:";

			// Token: 0x040068B3 RID: 26803
			[Tooltip("Optional override for the keycap text. Leave empty to derive from vThirdPersonInput.coverPoseInput.")]
			public string keyCapOverride = "";

			// Token: 0x040068B4 RID: 26804
			[Tooltip("Optional sprite for the keycap background. Leave null to keep the prefab default.")]
			public Sprite keyIcon;
		}

		// Token: 0x02000D8E RID: 3470
		[Serializable]
		public class RapidEmoteSlotConfig
		{
			// Token: 0x040068B5 RID: 26805
			[Tooltip("Slot view that displays this binding. Required.")]
			public RapidActionSlotUI slotView;

			// Token: 0x040068B6 RID: 26806
			[Tooltip("Static category prefix (e.g. 'Emote:').")]
			public string categoryLabel = "Emote:";

			// Token: 0x040068B7 RID: 26807
			[Tooltip("Default emote played when the hotkey is pressed. May be overridden by the user via long-press rebind.")]
			public EmoteDefinitionSO defaultEmote;

			// Token: 0x040068B8 RID: 26808
			[Tooltip("Hotkey that triggers the rapid emote (tap) or opens the rebind wheel (long-press).")]
			public GenericInput hotkey = new GenericInput("1", "DPadLeft", "DPadLeft");

			// Token: 0x040068B9 RID: 26809
			[Tooltip("Short keycap text shown on the prefab (e.g. '1', '2'). If empty, the keyboard binding name from 'hotkey' is used.")]
			public string keyCapOverride = "";

			// Token: 0x040068BA RID: 26810
			[Tooltip("Optional sprite for the keycap background. Leave null to keep the prefab default.")]
			public Sprite keyIcon;

			// Token: 0x040068BB RID: 26811
			[NonSerialized]
			public EmoteDefinitionSO RuntimeEmote;
		}

		// Token: 0x02000D8F RID: 3471
		[Serializable]
		public class CrouchSlotConfig
		{
			// Token: 0x040068BC RID: 26812
			[Tooltip("Slot view shown for the Crouch row. Mandatory if useCrouchSlot=true.")]
			public RapidActionSlotUI slotView;

			// Token: 0x040068BD RID: 26813
			[Tooltip("Static category prefix shown before the action name (e.g. 'Crouch:').")]
			public string categoryLabel = "Crouch:";

			// Token: 0x040068BE RID: 26814
			[Tooltip("Hotkey that triggers the rapid crouch action (tap) or opens the rebind wheel (long-press).")]
			public GenericInput hotkey = new GenericInput("C", "Y", "Y");

			// Token: 0x040068BF RID: 26815
			[Tooltip("Short keycap text shown on the prefab (e.g. 'C'). If empty, the keyboard binding name from 'hotkey' is used.")]
			public string keyCapOverride = "";

			// Token: 0x040068C0 RID: 26816
			[Tooltip("Optional sprite for the keycap background. Leave null to keep the prefab default.")]
			public Sprite keyIcon;

			// Token: 0x040068C1 RID: 26817
			[Tooltip("Crouch actions surfaced in the rebind wheel, in display order. Typically: Crouch, Crawl, Prone.")]
			public List<EmoteDefinitionSO> crouchEmotes = new List<EmoteDefinitionSO>();

			// Token: 0x040068C2 RID: 26818
			[Tooltip("Default crouch action selected on first launch. Must be one of the entries in 'crouchEmotes'.")]
			public EmoteDefinitionSO defaultEmote;

			// Token: 0x040068C3 RID: 26819
			[NonSerialized]
			public EmoteDefinitionSO RuntimeEmote;
		}

		// Token: 0x02000D90 RID: 3472
		private enum SlotKind
		{
			// Token: 0x040068C5 RID: 26821
			Arms,
			// Token: 0x040068C6 RID: 26822
			Emote,
			// Token: 0x040068C7 RID: 26823
			Crouch
		}

		// Token: 0x02000D91 RID: 3473
		private enum SlotPhase
		{
			// Token: 0x040068C9 RID: 26825
			Idle,
			// Token: 0x040068CA RID: 26826
			Pending,
			// Token: 0x040068CB RID: 26827
			RebindOpen
		}

		// Token: 0x02000D92 RID: 3474
		private class SlotRuntime
		{
			// Token: 0x040068CC RID: 26828
			public RapidActionsHUDController.SlotKind Kind;

			// Token: 0x040068CD RID: 26829
			public int EmoteSlotIndex;

			// Token: 0x040068CE RID: 26830
			public GenericInput Hotkey;

			// Token: 0x040068CF RID: 26831
			public RapidActionsHUDController.SlotPhase Phase;

			// Token: 0x040068D0 RID: 26832
			public float PressStartUnscaledTime;

			// Token: 0x040068D1 RID: 26833
			public List<RapidActionsHUDController.RebindOption> CurrentOptions;

			// Token: 0x040068D2 RID: 26834
			public bool ArmsExhibitSnapshot;
		}

		// Token: 0x02000D93 RID: 3475
		private struct RebindOption
		{
			// Token: 0x040068D3 RID: 26835
			public string Label;

			// Token: 0x040068D4 RID: 26836
			public string Description;

			// Token: 0x040068D5 RID: 26837
			public Sprite Icon;

			// Token: 0x040068D6 RID: 26838
			public int IntPayload;

			// Token: 0x040068D7 RID: 26839
			public bool ArmsExhibit;

			// Token: 0x040068D8 RID: 26840
			public EmoteDefinitionSO EmotePayload;

			// Token: 0x040068D9 RID: 26841
			public bool IsMasturbation;
		}
	}
}
