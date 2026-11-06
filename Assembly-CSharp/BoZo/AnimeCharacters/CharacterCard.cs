using System;
using Features.Localization;
using MoreMountains.Feedbacks;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200085C RID: 2140
	public class CharacterCard : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
	{
		// Token: 0x06003892 RID: 14482 RVA: 0x0014A2D9 File Offset: 0x001484D9
		public Sprite GetNpcGenderIcon(Gender gender)
		{
			if (gender != Gender.Female)
			{
				return this.npcMaleIcon;
			}
			return this.npcFemaleIcon;
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0014A2EC File Offset: 0x001484EC
		private void Awake()
		{
			if (this.cardButton == null)
			{
				this.cardButton = base.GetComponentInChildren<Button>();
			}
			if (this.cardButton != null)
			{
				this.cardButton.onClick.AddListener(new UnityAction(this.OnCardClick));
			}
			if (this.nameText == null)
			{
				this.nameText = base.GetComponentInChildren<TextMeshProUGUI>();
			}
			if (this.iconImage == null)
			{
				this.iconImage = base.GetComponentInChildren<Image>();
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0014A374 File Offset: 0x00148574
		public void Initialize(CharacterDataExtended data)
		{
			this.DisableNpcRoster();
			this.characterData = data;
			this.presetData = null;
			this.lastPresetClickTime = -10f;
			this.onPresetSingleClick = null;
			this.onPresetDoubleClick = null;
			if (data != null)
			{
				if (this.nameText != null)
				{
					LocalizedGameText.BindComputed(this.nameText, delegate
					{
						if (string.IsNullOrEmpty(data.characterName))
						{
							return GameLocalization.GetSource("cielos_apps", "Unknown Character");
						}
						return data.characterName;
					});
				}
				if (this.iconImage != null)
				{
					this.iconImage.gameObject.SetActive(true);
					if (data.icon != null)
					{
						this.iconImage.sprite = data.icon;
					}
					else if (this.defaultIcon != null)
					{
						this.iconImage.sprite = this.defaultIcon;
					}
					else
					{
						this.iconImage.gameObject.SetActive(false);
					}
				}
				Debug.Log("[CharacterCard] Initialized card for: " + data.characterName);
			}
			if (this.defaultBadge != null)
			{
				this.defaultBadge.SetActive(false);
			}
			if (this.systemBadge != null)
			{
				this.systemBadge.SetActive(false);
			}
			this.SetSelected(false);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0014A4C4 File Offset: 0x001486C4
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.presetData == null || eventData == null || this.IsNpcRosterControl(eventData) || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			CharacterCard.PlayFeedback(this.pointerDownFeedback);
			Action<CustomizationPresetData, Vector2> action = this.onPresetPointerDown;
			if (action == null)
			{
				return;
			}
			action(this.presetData, eventData.position);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0014A515 File Offset: 0x00148715
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.presetData == null || eventData == null || this.IsNpcRosterControl(eventData) || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			Action<CustomizationPresetData, Vector2> action = this.onPresetPointerUp;
			if (action == null)
			{
				return;
			}
			action(this.presetData, eventData.position);
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0014A550 File Offset: 0x00148750
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.presetData == null || eventData == null)
			{
				return;
			}
			CharacterCard.PlayFeedback(this.pointerExitFeedback);
			Action<CustomizationPresetData, Vector2> action = this.onPresetPointerExit;
			if (action == null)
			{
				return;
			}
			action(this.presetData, eventData.position);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x0014A585 File Offset: 0x00148785
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.presetData == null || eventData == null)
			{
				return;
			}
			CharacterCard.PlayFeedback(this.pointerEnterFeedback);
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0014A5A0 File Offset: 0x001487A0
		public void InitializePreset(CustomizationPresetData preset, Action<CustomizationPresetData> singleClick, Action<CustomizationPresetData> doubleClick, Action<CustomizationPresetData, Vector2> pointerDown = null, Action<CustomizationPresetData, Vector2> pointerUp = null, Action<CustomizationPresetData, Vector2> pointerExit = null, Action<CustomizationPresetData, Vector2> rightClick = null)
		{
			this.DisableNpcRoster();
			this.presetData = preset;
			this.lastPresetClickTime = -10f;
			this.characterData = null;
			this.onPresetSingleClick = singleClick;
			this.onPresetDoubleClick = doubleClick;
			this.onPresetPointerDown = pointerDown;
			this.onPresetPointerUp = pointerUp;
			this.onPresetPointerExit = pointerExit;
			this.onPresetRightClick = rightClick;
			if (this.nameText != null)
			{
				LocalizedGameText.BindComputed(this.nameText, delegate
				{
					if (preset == null)
					{
						return GameLocalization.GetSource("cielos_apps", "Unknown Preset");
					}
					return preset.presetName;
				});
			}
			if (this.defaultBadge != null)
			{
				this.defaultBadge.SetActive(preset != null && preset.isDefault);
			}
			if (this.systemBadge != null)
			{
				this.systemBadge.SetActive(preset != null && preset.isSystemPreset);
			}
			this.LoadPresetIcon(preset);
			this.SetSelected(false);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x0014A6A4 File Offset: 0x001488A4
		public void ConfigureNpcRoster(bool enabled, bool eligible, Action<CustomizationPresetData> toggle)
		{
			if (this.presetData == null || this.presetData.targetType != CustomizationTargetType.Npc)
			{
				this.DisableNpcRoster();
				return;
			}
			this.onNpcRosterToggle = toggle;
			if (this.npcRosterUI == null)
			{
				this.npcRosterUI = new NpcRosterCardUI(base.transform, this.iconImage, this.nameText, this.npcMaleIcon, this.npcFemaleIcon, new Action(this.ToggleNpcRoster));
			}
			this.npcRosterUI.Show(this.presetData, enabled, eligible);
			if (this.defaultBadge != null)
			{
				this.defaultBadge.SetActive(false);
			}
			if (this.systemBadge != null)
			{
				this.systemBadge.SetActive(false);
			}
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0014A759 File Offset: 0x00148959
		public void RefreshNpcRoster(bool enabled, bool eligible)
		{
			NpcRosterCardUI npcRosterCardUI = this.npcRosterUI;
			if (npcRosterCardUI == null)
			{
				return;
			}
			npcRosterCardUI.Refresh(enabled, eligible);
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x0014A76D File Offset: 0x0014896D
		private void DisableNpcRoster()
		{
			NpcRosterCardUI npcRosterCardUI = this.npcRosterUI;
			if (npcRosterCardUI != null)
			{
				npcRosterCardUI.Hide();
			}
			this.onNpcRosterToggle = null;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0014A787 File Offset: 0x00148987
		private void ToggleNpcRoster()
		{
			this.lastPresetClickTime = -10f;
			if (this.presetData != null)
			{
				Action<CustomizationPresetData> action = this.onNpcRosterToggle;
				if (action == null)
				{
					return;
				}
				action(this.presetData);
			}
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0014A7B2 File Offset: 0x001489B2
		private bool IsNpcRosterControl(PointerEventData eventData)
		{
			return this.npcRosterUI != null && this.npcRosterUI.OwnsPointer(eventData);
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0014A7CC File Offset: 0x001489CC
		private void LoadPresetIcon(CustomizationPresetData preset)
		{
			if (this.iconImage == null)
			{
				return;
			}
			if (preset == null || string.IsNullOrWhiteSpace(preset.iconFileName))
			{
				this.ApplyFallbackIcon(preset);
				return;
			}
			Sprite sprite;
			if (PresetIconCache.TryGet(preset.iconFileName, out sprite))
			{
				this.iconImage.sprite = sprite;
				NpcRosterCardUI npcRosterCardUI = this.npcRosterUI;
				if (npcRosterCardUI != null)
				{
					npcRosterCardUI.RefreshPortraitAspect();
				}
				this.iconImage.gameObject.SetActive(true);
				return;
			}
			this.ApplyFallbackIcon(preset);
			string expectedPresetId = preset.presetId;
			string iconFileName = preset.iconFileName;
			PresetIconCache.Load(iconFileName, delegate(Sprite loadedSprite)
			{
				if (loadedSprite == null || this.iconImage == null)
				{
					return;
				}
				if (!this.IsStillRepresenting(expectedPresetId, iconFileName))
				{
					return;
				}
				this.iconImage.sprite = loadedSprite;
				NpcRosterCardUI npcRosterCardUI2 = this.npcRosterUI;
				if (npcRosterCardUI2 != null)
				{
					npcRosterCardUI2.RefreshPortraitAspect();
				}
				this.iconImage.gameObject.SetActive(true);
			});
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0014A881 File Offset: 0x00148A81
		private bool IsStillRepresenting(string presetId, string iconFileName)
		{
			return this.presetData != null && this.presetData.presetId == presetId && this.presetData.iconFileName == iconFileName;
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0014A8B4 File Offset: 0x00148AB4
		private void ApplyFallbackIcon(CustomizationPresetData preset)
		{
			if (this.iconImage == null)
			{
				return;
			}
			if (preset != null && this.defaultIcon != null)
			{
				this.iconImage.sprite = this.defaultIcon;
				NpcRosterCardUI npcRosterCardUI = this.npcRosterUI;
				if (npcRosterCardUI != null)
				{
					npcRosterCardUI.RefreshPortraitAspect();
				}
				this.iconImage.gameObject.SetActive(true);
				return;
			}
			this.iconImage.gameObject.SetActive(false);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0014A928 File Offset: 0x00148B28
		private void OnCardClick()
		{
			if (this.presetData != null)
			{
				this.HandlePresetCardClick();
				return;
			}
			if (this.characterData != null)
			{
				Debug.Log("[CharacterCard] Card clicked for: " + this.characterData.characterName);
				CharactersManager charactersManager = Object.FindObjectOfType<CharactersManager>();
				if (charactersManager != null)
				{
					charactersManager.SetSelectedCharacterId(this.characterData.characterId, true);
				}
				if (CharacterStateManager.Instance != null)
				{
					CharacterStateManager.Instance.LoadCharacterById(this.characterData.characterId);
				}
				this.SwitchToCharacterCreator();
				this.OnCharacterSelected.Invoke(this.characterData);
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0014A9C0 File Offset: 0x00148BC0
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.presetData == null || eventData == null || this.IsNpcRosterControl(eventData))
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				Action<CustomizationPresetData, Vector2> action = this.onPresetRightClick;
				if (action == null)
				{
					return;
				}
				action(this.presetData, eventData.position);
			}
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x0014A9FC File Offset: 0x00148BFC
		private void HandlePresetCardClick()
		{
			float unscaledTime = Time.unscaledTime;
			bool flag = !ControllerInput.IsActive && unscaledTime - this.lastPresetClickTime <= this.doubleClickThresholdSeconds;
			this.lastPresetClickTime = unscaledTime;
			if (flag)
			{
				this.lastPresetClickTime = -10f;
				CharacterCard.PlayFeedback(this.doubleClickFeedback);
				Action<CustomizationPresetData> action = this.onPresetDoubleClick;
				if (action == null)
				{
					return;
				}
				action(this.presetData);
				return;
			}
			else
			{
				CharacterCard.PlayFeedback(this.singleClickFeedback);
				Action<CustomizationPresetData> action2 = this.onPresetSingleClick;
				if (action2 == null)
				{
					return;
				}
				action2(this.presetData);
				return;
			}
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0014AA83 File Offset: 0x00148C83
		public CharacterDataExtended GetCharacterData()
		{
			return this.characterData;
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x0014AA8B File Offset: 0x00148C8B
		public bool ControllerEditPreset()
		{
			if (this.presetData == null || this.onPresetDoubleClick == null)
			{
				return false;
			}
			this.onPresetDoubleClick(this.presetData);
			return true;
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x0014AAB1 File Offset: 0x00148CB1
		public string GetPresetId()
		{
			if (this.presetData == null)
			{
				return string.Empty;
			}
			return this.presetData.presetId;
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x0014AACC File Offset: 0x00148CCC
		public void SetSelected(bool selected)
		{
			if (this.selectedHighlight != null)
			{
				this.selectedHighlight.SetActive(selected);
			}
			if (selected)
			{
				CharacterCard.PlayFeedback(this.selectedFeedback);
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x0014AAF6 File Offset: 0x00148CF6
		public void PlaySetInUseConfirmedFeedback()
		{
			CharacterCard.PlayFeedback(this.setInUseConfirmedFeedback);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x0014AB03 File Offset: 0x00148D03
		public void PlayHoldCanceledFeedback()
		{
			CharacterCard.PlayFeedback(this.holdCanceledFeedback);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x0014AB10 File Offset: 0x00148D10
		private static void PlayFeedback(MMF_Player feedbackPlayer)
		{
			if (feedbackPlayer != null)
			{
				feedbackPlayer.PlayFeedbacks();
			}
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x0014AB24 File Offset: 0x00148D24
		private void SwitchToCharacterCreator()
		{
			CustomizationNavigationRouter customizationNavigationRouter = Object.FindObjectOfType<CustomizationNavigationRouter>(true);
			if (customizationNavigationRouter != null)
			{
				customizationNavigationRouter.OpenCharacterCreator(false);
				Debug.Log("[CharacterCard] Navigation successful via CustomizationNavigationRouter");
				return;
			}
			Debug.LogError("[CharacterCard] CustomizationNavigationRouter not found! Please add/configure it in the scene.");
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x0014AB5D File Offset: 0x00148D5D
		private void OnDestroy()
		{
			if (this.cardButton != null)
			{
				this.cardButton.onClick.RemoveListener(new UnityAction(this.OnCardClick));
			}
		}

		// Token: 0x04003982 RID: 14722
		[Header("UI References")]
		[SerializeField]
		private Image iconImage;

		// Token: 0x04003983 RID: 14723
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04003984 RID: 14724
		[SerializeField]
		private Button cardButton;

		// Token: 0x04003985 RID: 14725
		[Header("Preset UI (Optional)")]
		[SerializeField]
		private GameObject defaultBadge;

		// Token: 0x04003986 RID: 14726
		[SerializeField]
		private GameObject systemBadge;

		// Token: 0x04003987 RID: 14727
		[SerializeField]
		private GameObject selectedHighlight;

		// Token: 0x04003988 RID: 14728
		[Header("NPC Roster Icons")]
		[SerializeField]
		private Sprite npcMaleIcon;

		// Token: 0x04003989 RID: 14729
		[SerializeField]
		private Sprite npcFemaleIcon;

		// Token: 0x0400398A RID: 14730
		[Header("Preset Interaction")]
		[SerializeField]
		private float doubleClickThresholdSeconds = 0.4f;

		// Token: 0x0400398B RID: 14731
		[Header("FEEL Micro Interactions")]
		[SerializeField]
		private MMF_Player pointerEnterFeedback;

		// Token: 0x0400398C RID: 14732
		[SerializeField]
		private MMF_Player pointerDownFeedback;

		// Token: 0x0400398D RID: 14733
		[SerializeField]
		private MMF_Player pointerExitFeedback;

		// Token: 0x0400398E RID: 14734
		[SerializeField]
		private MMF_Player singleClickFeedback;

		// Token: 0x0400398F RID: 14735
		[SerializeField]
		private MMF_Player doubleClickFeedback;

		// Token: 0x04003990 RID: 14736
		[SerializeField]
		private MMF_Player selectedFeedback;

		// Token: 0x04003991 RID: 14737
		[SerializeField]
		private MMF_Player setInUseConfirmedFeedback;

		// Token: 0x04003992 RID: 14738
		[SerializeField]
		private MMF_Player holdCanceledFeedback;

		// Token: 0x04003993 RID: 14739
		[Header("Default Icon")]
		[SerializeField]
		private Sprite defaultIcon;

		// Token: 0x04003994 RID: 14740
		private CharacterDataExtended characterData;

		// Token: 0x04003995 RID: 14741
		private CustomizationPresetData presetData;

		// Token: 0x04003996 RID: 14742
		private Action<CustomizationPresetData> onPresetSingleClick;

		// Token: 0x04003997 RID: 14743
		private Action<CustomizationPresetData> onPresetDoubleClick;

		// Token: 0x04003998 RID: 14744
		private Action<CustomizationPresetData, Vector2> onPresetPointerDown;

		// Token: 0x04003999 RID: 14745
		private Action<CustomizationPresetData, Vector2> onPresetPointerUp;

		// Token: 0x0400399A RID: 14746
		private Action<CustomizationPresetData, Vector2> onPresetPointerExit;

		// Token: 0x0400399B RID: 14747
		private Action<CustomizationPresetData, Vector2> onPresetRightClick;

		// Token: 0x0400399C RID: 14748
		private float lastPresetClickTime = -10f;

		// Token: 0x0400399D RID: 14749
		private NpcRosterCardUI npcRosterUI;

		// Token: 0x0400399E RID: 14750
		private Action<CustomizationPresetData> onNpcRosterToggle;

		// Token: 0x0400399F RID: 14751
		public UnityEvent<CharacterDataExtended> OnCharacterSelected = new UnityEvent<CharacterDataExtended>();
	}
}
