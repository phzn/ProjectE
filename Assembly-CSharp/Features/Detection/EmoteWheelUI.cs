using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Inventory.UI;
using Features.Localization;
using Invector.vCharacterController;
using MoreMountains.Feedbacks;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x02001286 RID: 4742
	public class EmoteWheelUI : MonoBehaviour
	{
		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06009310 RID: 37648 RVA: 0x003C61EC File Offset: 0x003C43EC
		// (remove) Token: 0x06009311 RID: 37649 RVA: 0x003C6224 File Offset: 0x003C4424
		public event Action<EmoteDefinitionSO> OnEmoteConfirmed;

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06009312 RID: 37650 RVA: 0x003C6259 File Offset: 0x003C4459
		public static bool AnyWheelOpen
		{
			get
			{
				return EmoteWheelUI._wheelOwner != null && EmoteWheelUI._wheelOwner.isActiveAndEnabled && EmoteWheelUI._wheelOwner.IsOpen;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06009313 RID: 37651 RVA: 0x003C6280 File Offset: 0x003C4480
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06009314 RID: 37652 RVA: 0x003C6288 File Offset: 0x003C4488
		public int FocusedIndex
		{
			get
			{
				return this._focusedIndex;
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06009315 RID: 37653 RVA: 0x003C6290 File Offset: 0x003C4490
		public EmoteDefinitionSO FocusedEmote
		{
			get
			{
				if (this._currentEmotes.Count <= 0 || this._focusedIndex < 0 || this._focusedIndex >= this._currentEmotes.Count)
				{
					return null;
				}
				return this._currentEmotes[this._focusedIndex];
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06009316 RID: 37654 RVA: 0x003C62CF File Offset: 0x003C44CF
		private int CenterSlot
		{
			get
			{
				return this.CardSlots.Count / 2;
			}
		}

		// Token: 0x06009317 RID: 37655 RVA: 0x003C62DE File Offset: 0x003C44DE
		private void Awake()
		{
			if (this.WheelRoot != null)
			{
				this.WheelRoot.SetActive(false);
			}
		}

		// Token: 0x06009318 RID: 37656 RVA: 0x003C62FC File Offset: 0x003C44FC
		public void Open(ChoiceCategory category)
		{
			if (this.EmoteCollection == null)
			{
				Debug.LogError("[EmoteWheelUI] EmoteCollection not assigned!");
				return;
			}
			if (category != ChoiceCategory.Shame)
			{
				if (category != ChoiceCategory.Police)
				{
					this._activeTheme = this.ExposeTheme;
				}
				else
				{
					this._activeTheme = ((this.PoliceTheme != null) ? this.PoliceTheme : this.ExposeTheme);
				}
			}
			else
			{
				this._activeTheme = this.ShameTheme;
			}
			if (this._activeTheme == null || this._activeTheme.CardPrefab == null)
			{
				Debug.LogError(string.Format("[EmoteWheelUI] Theme or CardPrefab missing for {0}!", category));
				return;
			}
			this._currentEmotes = this.EmoteCollection.GetEmotesForCategory(category);
			if (this._currentEmotes == null || this._currentEmotes.Count == 0)
			{
				Debug.LogWarning(string.Format("[EmoteWheelUI] No emotes found for category {0}", category));
				return;
			}
			this._focusedIndex = 0;
			this._scrollAccumulator = 0f;
			this._confirmLocked = false;
			this._cardPrefabOverridesByEmoteId = null;
			this._defaultCardPrefabOverride = null;
			EmoteWheelUI.EnsureHierarchyActive(this.WheelRoot);
			this.ApplyArcBackground();
			this.SpawnCards();
			this.RefreshSlots(true);
			this._isOpen = true;
			EmoteWheelUI._wheelOwner = this;
			this._controllerScrollDirection = 0;
			this.RefreshInstructions();
			if (this.OpenFeedback != null)
			{
				this.OpenFeedback.PlayFeedbacks();
			}
			Debug.Log(string.Format("[EmoteWheelUI] Opened for {0} with {1} emotes", category, this._currentEmotes.Count));
		}

		// Token: 0x06009319 RID: 37657 RVA: 0x003C647A File Offset: 0x003C467A
		public void OpenWithEmotes(EmoteWheelThemeSO theme, List<EmoteDefinitionSO> emotes)
		{
			this.OpenWithEmotes(theme, emotes, null, null);
		}

		// Token: 0x0600931A RID: 37658 RVA: 0x003C6486 File Offset: 0x003C4686
		public void OpenWithEmotes(EmoteWheelThemeSO theme, List<EmoteDefinitionSO> emotes, Dictionary<string, EmoteCardUI> cardPrefabOverridesByEmoteId)
		{
			this.OpenWithEmotes(theme, emotes, cardPrefabOverridesByEmoteId, null);
		}

		// Token: 0x0600931B RID: 37659 RVA: 0x003C6494 File Offset: 0x003C4694
		public void OpenWithEmotes(EmoteWheelThemeSO theme, List<EmoteDefinitionSO> emotes, Dictionary<string, EmoteCardUI> cardPrefabOverridesByEmoteId, EmoteCardUI defaultCardPrefabOverride)
		{
			if (theme == null || theme.CardPrefab == null)
			{
				Debug.LogError("[EmoteWheelUI] Theme or CardPrefab is null!");
				return;
			}
			if (emotes == null || emotes.Count == 0)
			{
				Debug.LogWarning("[EmoteWheelUI] No emotes provided to OpenWithEmotes");
				return;
			}
			this._activeTheme = theme;
			this._currentEmotes = emotes;
			this._cardPrefabOverridesByEmoteId = cardPrefabOverridesByEmoteId;
			this._defaultCardPrefabOverride = defaultCardPrefabOverride;
			this._focusedIndex = 0;
			this._scrollAccumulator = 0f;
			this._confirmLocked = false;
			EmoteWheelUI.EnsureHierarchyActive(this.WheelRoot);
			this.ApplyArcBackground();
			this.SpawnCards();
			this.RefreshSlots(true);
			this._isOpen = true;
			EmoteWheelUI._wheelOwner = this;
			this._controllerScrollDirection = 0;
			this.RefreshInstructions();
			if (this.OpenFeedback != null)
			{
				this.OpenFeedback.PlayFeedbacks();
			}
			Debug.Log(string.Format("[EmoteWheelUI] Opened (gameplay) with {0} emotes", this._currentEmotes.Count));
		}

		// Token: 0x0600931C RID: 37660 RVA: 0x003C6580 File Offset: 0x003C4780
		public void Close()
		{
			this._isOpen = false;
			if (EmoteWheelUI._wheelOwner == this)
			{
				EmoteWheelUI._wheelOwner = null;
			}
			if (this.InstructionText != null)
			{
				this.InstructionText.enabled = false;
			}
			this.DestroyCards();
			this._cardPrefabOverridesByEmoteId = null;
			this._defaultCardPrefabOverride = null;
			if (this.WheelRoot != null)
			{
				this.WheelRoot.SetActive(false);
			}
			if (this.CloseFeedback != null)
			{
				this.CloseFeedback.PlayFeedbacks();
			}
		}

		// Token: 0x0600931D RID: 37661 RVA: 0x003C6608 File Offset: 0x003C4808
		private void Update()
		{
			if (!this._isOpen)
			{
				return;
			}
			this.RefreshAvailability();
			this.RefreshInstructions();
			if (FlyCameraPhotoMode.AnyOpen || GameplayInputGate.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if (InventoryDialogController.Instance != null && InventoryDialogController.Instance.IsOpen)
			{
				return;
			}
			this.HandleScrollInput();
			this.HandleConfirmInput();
			this.AnimateCards();
		}

		// Token: 0x0600931E RID: 37662 RVA: 0x003C6674 File Offset: 0x003C4874
		private void HandleScrollInput()
		{
			if (ControllerInput.IsActive)
			{
				int num = 0;
				if (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder))
				{
					num = -1;
				}
				else if (ControllerInput.GetButtonDown(ControllerButton.RightShoulder))
				{
					num = 1;
				}
				else
				{
					Vector2 radialDirection = ControllerInput.RadialDirection;
					if (radialDirection.sqrMagnitude < 0.25f)
					{
						radialDirection = new Vector2(ControllerInput.Navigation.x, 0f);
					}
					float num2 = ((Mathf.Abs(radialDirection.x) >= Mathf.Abs(radialDirection.y)) ? radialDirection.x : (-radialDirection.y));
					int num3 = ((Mathf.Abs(num2) < 0.5f) ? 0 : ((num2 > 0f) ? 1 : (-1)));
					if (num3 == 0)
					{
						this._controllerScrollDirection = 0;
					}
					else if (num3 != this._controllerScrollDirection || Time.unscaledTime >= this._nextControllerScroll)
					{
						bool flag = num3 != this._controllerScrollDirection;
						this._controllerScrollDirection = num3;
						this._nextControllerScroll = Time.unscaledTime + (flag ? 0.32f : 0.14f);
						num = num3;
					}
				}
				if (num != 0)
				{
					this.ScrollByStep(num);
					ControllerInput.ConsumeUIInput();
				}
				return;
			}
			float axisRaw = GameplayInput.GetAxisRaw("Mouse ScrollWheel");
			if (Mathf.Abs(axisRaw) < 0.001f)
			{
				return;
			}
			this._scrollAccumulator += axisRaw;
			if (Mathf.Abs(this._scrollAccumulator) >= this.ScrollStepThreshold * 0.1f)
			{
				int num4 = ((this._scrollAccumulator > 0f) ? (-1) : 1);
				this._scrollAccumulator = 0f;
				this.ScrollByStep(num4);
			}
		}

		// Token: 0x0600931F RID: 37663 RVA: 0x003C67EC File Offset: 0x003C49EC
		public void ScrollByStep(int direction)
		{
			if (this._currentEmotes.Count == 0)
			{
				return;
			}
			this._focusedIndex = (this._focusedIndex + direction) % this._currentEmotes.Count;
			if (this._focusedIndex < 0)
			{
				this._focusedIndex += this._currentEmotes.Count;
			}
			this.RefreshSlots(false);
			if (this.ScrollFeedback != null)
			{
				this.ScrollFeedback.PlayFeedbacks();
			}
		}

		// Token: 0x06009320 RID: 37664 RVA: 0x003C6862 File Offset: 0x003C4A62
		private void HandleConfirmInput()
		{
			if (!(ControllerInput.IsActive ? ControllerInput.SubmitDown : GameplayInput.GetKeyDown(this.ConfirmKey)))
			{
				return;
			}
			if (ControllerInput.IsActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			this.TryConfirmFocusedEmote();
		}

		// Token: 0x06009321 RID: 37665 RVA: 0x003C6894 File Offset: 0x003C4A94
		public bool TryConfirmFocusedEmote()
		{
			if (!this._isOpen || this._confirmLocked)
			{
				return false;
			}
			EmoteDefinitionSO focusedEmote = this.FocusedEmote;
			if (focusedEmote == null || focusedEmote.IsBlockedByClothing)
			{
				this.RefreshAvailability();
				return false;
			}
			this._confirmLocked = true;
			if (this.InstructionText != null)
			{
				this.InstructionText.enabled = false;
			}
			if (this.ConfirmFeedback != null)
			{
				this.ConfirmFeedback.PlayFeedbacks();
			}
			Debug.Log(string.Concat(new string[] { "[EmoteWheelUI] Confirmed emote: ", focusedEmote.DisplayName, " (", focusedEmote.EmoteId, ")" }));
			Action<EmoteDefinitionSO> onEmoteConfirmed = this.OnEmoteConfirmed;
			if (onEmoteConfirmed != null)
			{
				onEmoteConfirmed(focusedEmote);
			}
			return true;
		}

		// Token: 0x06009322 RID: 37666 RVA: 0x003C695A File Offset: 0x003C4B5A
		public void ResetConfirmation()
		{
			this._confirmLocked = false;
			this.RefreshAvailability();
			this.RefreshInstructions();
		}

		// Token: 0x06009323 RID: 37667 RVA: 0x003C6970 File Offset: 0x003C4B70
		public TextMeshProUGUI ResolveInstructionText()
		{
			if (this.InstructionText == null && this.WheelRoot != null)
			{
				Transform transform = this.WheelRoot.transform.Find("Hint");
				if (transform != null)
				{
					this.InstructionText = transform.GetComponent<TextMeshProUGUI>();
				}
				if (this.InstructionText == null)
				{
					this.InstructionText = EncounterInputHints.CreateText(this.WheelRoot.transform, null);
				}
			}
			if (this.InstructionText != null)
			{
				this.InstructionText.raycastTarget = false;
				if (this._instructionBinding == null)
				{
					this._instructionBinding = LocalizedGameText.BindComputed(this.InstructionText, () => this.BuildInstructionText(this._instructionDevice));
					this._instructionBinding.FitTranslationToRectangle(false);
				}
			}
			return this.InstructionText;
		}

		// Token: 0x06009324 RID: 37668 RVA: 0x003C6A40 File Offset: 0x003C4C40
		public void RefreshInstructions()
		{
			bool flag = this._isOpen && !this._confirmLocked && !FlyCameraPhotoMode.AnyOpen && !GraphicsSettingsRuntimeUI.AnyOpen && !GameplayInputGate.BlocksGameplayInput;
			if (flag)
			{
				this.ResolveInstructionText();
			}
			if (this.InstructionText == null)
			{
				return;
			}
			this.InstructionText.enabled = flag;
			if (flag)
			{
				this._instructionDevice = vInput.instance.inputDevice;
				this.InstructionText.text = this.BuildInstructionText(this._instructionDevice);
			}
		}

		// Token: 0x06009325 RID: 37669 RVA: 0x003C6AC8 File Offset: 0x003C4CC8
		public string BuildInstructionText(InputDevice device)
		{
			string text = ((device == InputDevice.MouseKeyboard) ? EncounterInputLabels.Format(this.ConfirmKey.ToString()) : EncounterInputLabels.ForInput(this.ControllerConfirmInput, device));
			string text2 = (string.IsNullOrEmpty(text) ? string.Empty : GameLocalization.FormatSource("gameplay_prompts", "Press <b>{0}</b> to confirm your choice", new object[] { text }));
			if (this.FocusedEmote != null && this.FocusedEmote.IsBlockedByClothing)
			{
				text2 = GameLocalization.GetSource("gameplay_prompts", "Unavailable with current clothing");
			}
			string text3;
			if (device == InputDevice.MouseKeyboard)
			{
				text3 = GameLocalization.GetSource("gameplay_prompts", "Scroll to change choice");
			}
			else
			{
				string text4 = EncounterInputLabels.ForInput(this.ControllerPreviousInput, device);
				string text5 = EncounterInputLabels.ForInput(this.ControllerNextInput, device);
				string text6 = (string.IsNullOrEmpty(text4) ? text5 : (string.IsNullOrEmpty(text5) ? text4 : EncounterInputHints.JoinInputAlternatives(new string[] { text4, text5 })));
				text3 = (string.IsNullOrEmpty(text6) ? string.Empty : GameLocalization.FormatSource("gameplay_prompts", "Press <b>{0}</b> to change choice", new object[] { text6 }));
			}
			if (string.IsNullOrEmpty(text2))
			{
				return text3;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				return text2 + "<br><size=85%><color=#D9D9D9>" + text3 + "</color></size>";
			}
			return text2;
		}

		// Token: 0x06009326 RID: 37670 RVA: 0x003C6C00 File Offset: 0x003C4E00
		private void RefreshAvailability()
		{
			foreach (EmoteCardUI emoteCardUI in this._spawnedCards)
			{
				if (emoteCardUI != null)
				{
					emoteCardUI.SetAvailable(emoteCardUI.EmoteData != null && !emoteCardUI.EmoteData.IsBlockedByClothing);
				}
			}
		}

		// Token: 0x06009327 RID: 37671 RVA: 0x003C6C7C File Offset: 0x003C4E7C
		private void ApplyArcBackground()
		{
			if (this._activeTheme == null || this.ArcBackground == null)
			{
				return;
			}
			if (this._activeTheme.ArcSprite != null)
			{
				this.ArcBackground.sprite = this._activeTheme.ArcSprite;
			}
			this.ArcBackground.color = this._activeTheme.ArcTint;
		}

		// Token: 0x06009328 RID: 37672 RVA: 0x003C6CE8 File Offset: 0x003C4EE8
		private void SpawnCards()
		{
			this.DestroyCards();
			for (int i = 0; i < this.CardSlots.Count; i++)
			{
				this._spawnedCards.Add(null);
				this._spawnedCardPrefabIds.Add(0);
			}
		}

		// Token: 0x06009329 RID: 37673 RVA: 0x003C6D2C File Offset: 0x003C4F2C
		private void DestroyCards()
		{
			for (int i = 0; i < this._spawnedCards.Count; i++)
			{
				if (this._spawnedCards[i] != null)
				{
					Object.Destroy(this._spawnedCards[i].gameObject);
				}
			}
			this._spawnedCards.Clear();
			this._spawnedCardPrefabIds.Clear();
		}

		// Token: 0x0600932A RID: 37674 RVA: 0x003C6D90 File Offset: 0x003C4F90
		private void RefreshSlots(bool instant)
		{
			if (this._activeTheme == null || this._currentEmotes.Count == 0)
			{
				return;
			}
			int count = this._spawnedCards.Count;
			int centerSlot = this.CenterSlot;
			int count2 = this._currentEmotes.Count;
			for (int i = 0; i < count; i++)
			{
				int num = i - centerSlot;
				int num2 = ((this._focusedIndex + num) % count2 + count2) % count2;
				EmoteDefinitionSO emoteDefinitionSO = this._currentEmotes[num2];
				EmoteCardUI emoteCardUI = this.EnsureCardForSlot(i, emoteDefinitionSO);
				if (!(emoteCardUI == null))
				{
					emoteCardUI.Setup(emoteDefinitionSO);
					emoteCardUI.SetAvailable(emoteDefinitionSO != null && !emoteDefinitionSO.IsBlockedByClothing);
					bool flag = i == centerSlot;
					float num3 = (flag ? this._activeTheme.FocusedScale : 1f);
					float num4 = (flag ? 1f : this._activeTheme.SideAlpha);
					emoteCardUI.SetFocusState(flag, num3, num4);
					if (instant)
					{
						emoteCardUI.ApplyInstant();
					}
				}
			}
		}

		// Token: 0x0600932B RID: 37675 RVA: 0x003C6E98 File Offset: 0x003C5098
		private void AnimateCards()
		{
			for (int i = 0; i < this._spawnedCards.Count; i++)
			{
				if (this._spawnedCards[i] != null)
				{
					this._spawnedCards[i].AnimateTowardsTargets(this.TransitionSpeed);
				}
			}
		}

		// Token: 0x0600932C RID: 37676 RVA: 0x003C6EE8 File Offset: 0x003C50E8
		private EmoteCardUI EnsureCardForSlot(int slotIndex, EmoteDefinitionSO emote)
		{
			if (slotIndex < 0 || slotIndex >= this.CardSlots.Count)
			{
				return null;
			}
			RectTransform rectTransform = this.CardSlots[slotIndex];
			if (rectTransform == null)
			{
				return null;
			}
			EmoteCardUI emoteCardUI = this.ResolveCardPrefab(emote);
			if (emoteCardUI == null)
			{
				return null;
			}
			int instanceID = emoteCardUI.GetInstanceID();
			while (this._spawnedCards.Count <= slotIndex)
			{
				this._spawnedCards.Add(null);
			}
			while (this._spawnedCardPrefabIds.Count <= slotIndex)
			{
				this._spawnedCardPrefabIds.Add(0);
			}
			EmoteCardUI emoteCardUI2 = this._spawnedCards[slotIndex];
			int num = this._spawnedCardPrefabIds[slotIndex];
			if (emoteCardUI2 != null && num != instanceID)
			{
				Object.Destroy(emoteCardUI2.gameObject);
				emoteCardUI2 = null;
			}
			if (emoteCardUI2 == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(emoteCardUI.gameObject, rectTransform);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.one;
				component.offsetMin = Vector2.zero;
				component.offsetMax = Vector2.zero;
				component.localScale = Vector3.one;
				emoteCardUI2 = gameObject.GetComponent<EmoteCardUI>();
				this._spawnedCards[slotIndex] = emoteCardUI2;
				this._spawnedCardPrefabIds[slotIndex] = instanceID;
			}
			return emoteCardUI2;
		}

		// Token: 0x0600932D RID: 37677 RVA: 0x003C701C File Offset: 0x003C521C
		private EmoteCardUI ResolveCardPrefab(EmoteDefinitionSO emote)
		{
			if (emote != null && this._cardPrefabOverridesByEmoteId != null)
			{
				string text = EmoteWheelUI.BuildEmoteKey(emote);
				EmoteCardUI emoteCardUI;
				if (!string.IsNullOrEmpty(text) && this._cardPrefabOverridesByEmoteId.TryGetValue(text, out emoteCardUI) && emoteCardUI != null)
				{
					return emoteCardUI;
				}
			}
			if (this._defaultCardPrefabOverride != null)
			{
				return this._defaultCardPrefabOverride;
			}
			if (!(this._activeTheme != null))
			{
				return null;
			}
			return this._activeTheme.CardPrefab;
		}

		// Token: 0x0600932E RID: 37678 RVA: 0x0028CACC File Offset: 0x0028ACCC
		private static string BuildEmoteKey(EmoteDefinitionSO emote)
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

		// Token: 0x0600932F RID: 37679 RVA: 0x003C7094 File Offset: 0x003C5294
		private static void EnsureHierarchyActive(GameObject target)
		{
			if (target == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			Transform transform = target.transform;
			while (transform != null)
			{
				if (!transform.gameObject.activeSelf)
				{
					list.Add(transform.gameObject);
				}
				transform = transform.parent;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].SetActive(true);
			}
		}

		// Token: 0x040097E4 RID: 38884
		[Header("Data")]
		public EmoteCollectionSO EmoteCollection;

		// Token: 0x040097E5 RID: 38885
		[Header("Themes")]
		public EmoteWheelThemeSO ShameTheme;

		// Token: 0x040097E6 RID: 38886
		public EmoteWheelThemeSO ExposeTheme;

		// Token: 0x040097E7 RID: 38887
		[Tooltip("Tema usato per la categoria Police (cattura Polizia). Se null, ripiega su ExposeTheme.")]
		public EmoteWheelThemeSO PoliceTheme;

		// Token: 0x040097E8 RID: 38888
		[Header("Layout")]
		[Tooltip("Root GO of the wheel UI (toggled on/off).")]
		public GameObject WheelRoot;

		// Token: 0x040097E9 RID: 38889
		[Tooltip("Arc background image (sprite swapped per theme)")]
		public Image ArcBackground;

		// Token: 0x040097EA RID: 38890
		[Header("Card Slots (5 pre-positioned RectTransforms)")]
		[Tooltip("Empty containers ordered left→right: 0=far left, 2=center/selected, 4=far right")]
		public List<RectTransform> CardSlots = new List<RectTransform>();

		// Token: 0x040097EB RID: 38891
		[Header("Scroll Settings")]
		[Tooltip("Mouse scroll sensitivity threshold (lower = more sensitive)")]
		public float ScrollStepThreshold = 1f;

		// Token: 0x040097EC RID: 38892
		[Header("Animation")]
		public float TransitionSpeed = 10f;

		// Token: 0x040097ED RID: 38893
		[Header("Input")]
		[Tooltip("Key to confirm the currently selected (center) emote")]
		public KeyCode ConfirmKey = KeyCode.E;

		// Token: 0x040097EE RID: 38894
		[Tooltip("Controller confirmation; keyboard continues to use ConfirmKey.")]
		public GenericInput ControllerConfirmInput = new GenericInput("None", "A", "");

		// Token: 0x040097EF RID: 38895
		public GenericInput ControllerPreviousInput = new GenericInput("None", "LB", "");

		// Token: 0x040097F0 RID: 38896
		public GenericInput ControllerNextInput = new GenericInput("None", "RB", "");

		// Token: 0x040097F1 RID: 38897
		[Tooltip("Optional existing hint text. Defaults to WheelRoot/Hint, preserving its authored style.")]
		public TextMeshProUGUI InstructionText;

		// Token: 0x040097F2 RID: 38898
		private InputDevice _instructionDevice;

		// Token: 0x040097F3 RID: 38899
		private LocalizedGameText _instructionBinding;

		// Token: 0x040097F4 RID: 38900
		[Header("Feel Feedbacks")]
		[Tooltip("Played when the wheel opens")]
		public MMF_Player OpenFeedback;

		// Token: 0x040097F5 RID: 38901
		[Tooltip("Played on each scroll tick")]
		public MMF_Player ScrollFeedback;

		// Token: 0x040097F6 RID: 38902
		[Tooltip("Played when the player confirms an emote")]
		public MMF_Player ConfirmFeedback;

		// Token: 0x040097F7 RID: 38903
		[Tooltip("Played when the wheel closes")]
		public MMF_Player CloseFeedback;

		// Token: 0x040097F9 RID: 38905
		private EmoteWheelThemeSO _activeTheme;

		// Token: 0x040097FA RID: 38906
		private List<EmoteDefinitionSO> _currentEmotes = new List<EmoteDefinitionSO>();

		// Token: 0x040097FB RID: 38907
		private readonly List<EmoteCardUI> _spawnedCards = new List<EmoteCardUI>();

		// Token: 0x040097FC RID: 38908
		private readonly List<int> _spawnedCardPrefabIds = new List<int>();

		// Token: 0x040097FD RID: 38909
		private Dictionary<string, EmoteCardUI> _cardPrefabOverridesByEmoteId;

		// Token: 0x040097FE RID: 38910
		private EmoteCardUI _defaultCardPrefabOverride;

		// Token: 0x040097FF RID: 38911
		private int _focusedIndex;

		// Token: 0x04009800 RID: 38912
		private float _scrollAccumulator;

		// Token: 0x04009801 RID: 38913
		private bool _isOpen;

		// Token: 0x04009802 RID: 38914
		private bool _confirmLocked;

		// Token: 0x04009803 RID: 38915
		private static EmoteWheelUI _wheelOwner;

		// Token: 0x04009804 RID: 38916
		private int _controllerScrollDirection;

		// Token: 0x04009805 RID: 38917
		private float _nextControllerScroll;
	}
}
