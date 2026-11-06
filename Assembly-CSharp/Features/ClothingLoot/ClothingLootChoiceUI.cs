using System;
using System.Collections.Generic;
using Features.CensorBars;
using Features.Inventory;
using Features.Localization;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.ClothingLoot
{
	// Token: 0x02001346 RID: 4934
	public sealed class ClothingLootChoiceUI : MonoBehaviour
	{
		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06009882 RID: 39042 RVA: 0x003E8305 File Offset: 0x003E6505
		// (set) Token: 0x06009883 RID: 39043 RVA: 0x003E830C File Offset: 0x003E650C
		public static ClothingLootChoiceUI Instance { get; private set; }

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x06009884 RID: 39044 RVA: 0x003E8314 File Offset: 0x003E6514
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x06009885 RID: 39045 RVA: 0x003E831C File Offset: 0x003E651C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			ClothingLootChoiceUI.Instance = null;
		}

		// Token: 0x06009886 RID: 39046 RVA: 0x003E8324 File Offset: 0x003E6524
		public static bool TryOpen(ClothingLootTableInteractable owner, GameObject player, IReadOnlyList<ClothingLootOffer> offers)
		{
			if (owner == null || offers == null || offers.Count == 0)
			{
				return false;
			}
			ClothingLootChoiceUI clothingLootChoiceUI = ClothingLootChoiceUI.EnsureInstance();
			return clothingLootChoiceUI != null && clothingLootChoiceUI.OpenInternal(owner, player, offers);
		}

		// Token: 0x06009887 RID: 39047 RVA: 0x003E8361 File Offset: 0x003E6561
		public static void CloseIfOwnedBy(ClothingLootTableInteractable owner)
		{
			if (ClothingLootChoiceUI.Instance != null && ClothingLootChoiceUI.Instance._owner == owner)
			{
				ClothingLootChoiceUI.Instance.CloseInternal(true);
			}
		}

		// Token: 0x06009888 RID: 39048 RVA: 0x003E8390 File Offset: 0x003E6590
		private static ClothingLootChoiceUI EnsureInstance()
		{
			if (ClothingLootChoiceUI.Instance != null)
			{
				return ClothingLootChoiceUI.Instance;
			}
			ClothingLootChoiceUI clothingLootChoiceUI = Object.FindObjectOfType<ClothingLootChoiceUI>(true);
			if (clothingLootChoiceUI != null)
			{
				ClothingLootChoiceUI.Instance = clothingLootChoiceUI;
				return clothingLootChoiceUI;
			}
			GameObject gameObject = new GameObject("[Clothing Loot Choice UI]");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<ClothingLootChoiceUI>();
		}

		// Token: 0x06009889 RID: 39049 RVA: 0x003E83DD File Offset: 0x003E65DD
		private void Awake()
		{
			if (ClothingLootChoiceUI.Instance != null && ClothingLootChoiceUI.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			ClothingLootChoiceUI.Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.BuildUi();
		}

		// Token: 0x0600988A RID: 39050 RVA: 0x003E841C File Offset: 0x003E661C
		private void Update()
		{
			if (!this._isOpen)
			{
				return;
			}
			if (this._owner == null)
			{
				this.CloseInternal(false);
				return;
			}
			if (EscapeInputCoordinator.TryConsume(this))
			{
				this.CloseInternal(true);
			}
		}

		// Token: 0x0600988B RID: 39051 RVA: 0x003E844C File Offset: 0x003E664C
		private void OnDestroy()
		{
			this.CloseInternal(true);
			if (ClothingLootChoiceUI.Instance == this)
			{
				ClothingLootChoiceUI.Instance = null;
			}
		}

		// Token: 0x0600988C RID: 39052 RVA: 0x003E8468 File Offset: 0x003E6668
		private void OnDisable()
		{
			this.CloseInternal(true);
		}

		// Token: 0x0600988D RID: 39053 RVA: 0x003E8474 File Offset: 0x003E6674
		private bool OpenInternal(ClothingLootTableInteractable owner, GameObject player, IReadOnlyList<ClothingLootOffer> offers)
		{
			if (this._isOpen || this._isClosing || owner == null || offers == null || offers.Count == 0)
			{
				return false;
			}
			this._owner = owner;
			if (!this.TryAcquireModalContext())
			{
				this._owner = null;
				return false;
			}
			if (!base.isActiveAndEnabled || owner == null || !owner.isActiveAndEnabled || this._owner != owner || this._lease == null || !this._lease.IsActive)
			{
				this._owner = null;
				this.ReleaseModalContext();
				return false;
			}
			bool flag;
			try
			{
				this._offers.Clear();
				int num = 0;
				while (num < offers.Count && num < 4)
				{
					this._offers.Add(offers[num]);
					num++;
				}
				this._isOpen = true;
				this._isCommitting = false;
				this._selectionUnlockFrame = Time.frameCount;
				this.ResolveAndLockPlayer(player);
				this.RefreshCards();
				this.CaptureUiSelection();
				this._canvasRoot.SetActive(true);
				this.SelectFirstCard();
				flag = true;
			}
			catch
			{
				this.CloseInternal(false);
				throw;
			}
			return flag;
		}

		// Token: 0x0600988E RID: 39054 RVA: 0x003E8468 File Offset: 0x003E6668
		public void Close()
		{
			this.CloseInternal(true);
		}

		// Token: 0x0600988F RID: 39055 RVA: 0x003E859C File Offset: 0x003E679C
		private void CloseInternal(bool notifyOwner)
		{
			if (this._isClosing)
			{
				return;
			}
			this._isClosing = true;
			try
			{
				ClothingLootTableInteractable owner = this._owner;
				if (!this._isOpen && !this._gameplayStateCaptured && this._lease == null)
				{
					this._owner = null;
					this._offers.Clear();
					if (notifyOwner && owner != null)
					{
						owner.NotifyLootMenuClosed();
					}
				}
				else
				{
					this._owner = null;
					this._isOpen = false;
					this._isCommitting = false;
					this.RestoreUiSelection();
					if (this._canvasRoot != null)
					{
						this._canvasRoot.SetActive(false);
					}
					this.RestorePlayerState();
					this.ReleaseModalContext();
					this._offers.Clear();
					if (notifyOwner && owner != null)
					{
						owner.NotifyLootMenuClosed();
					}
				}
			}
			finally
			{
				this._isClosing = false;
			}
		}

		// Token: 0x06009890 RID: 39056 RVA: 0x003E8678 File Offset: 0x003E6878
		private bool TryAcquireModalContext()
		{
			if (this._lease != null)
			{
				if (this._lease.IsActive)
				{
					return true;
				}
				this._lease = null;
			}
			PlayerActivityLease playerActivityLease;
			bool flag = PlayerActivityGate.TryAcquire("ClothingLootChoice#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI, PlayerActivity.AllPlayerActions, out playerActivityLease, false, null);
			this._lease = playerActivityLease;
			return flag && this._lease != null && this._lease.IsActive;
		}

		// Token: 0x06009891 RID: 39057 RVA: 0x003E86EC File Offset: 0x003E68EC
		private void ReleaseModalContext()
		{
			PlayerActivityLease lease = this._lease;
			this._lease = null;
			if (lease == null)
			{
				return;
			}
			lease.Dispose();
		}

		// Token: 0x06009892 RID: 39058 RVA: 0x003E8708 File Offset: 0x003E6908
		private void ResolveAndLockPlayer(GameObject player)
		{
			this._playerInput = ((player != null) ? player.GetComponentInChildren<vThirdPersonInput>(true) : null);
			if (this._playerInput == null && PlayerRegistry.PlayerObject != null)
			{
				this._playerInput = PlayerRegistry.PlayerObject.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (this._playerInput == null)
			{
				this._playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this._gameplayStateCaptured)
			{
				return;
			}
			if (this._playerInput != null)
			{
				this._previousLockInput = this._playerInput.lockInput;
				this._previousLockCamera = this._playerInput.lockCameraInput;
				this._playerInput.SetLockAllInput(true);
				this._playerInput.SetLockCameraInput(true);
				if (this._playerInput.cc != null)
				{
					this._playerInput.cc.input = Vector2.zero;
					this._playerInput.cc.isSprinting = false;
				}
			}
			this._previousCursorLock = Cursor.lockState;
			this._previousCursorVisible = Cursor.visible;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this._gameplayStateCaptured = true;
		}

		// Token: 0x06009893 RID: 39059 RVA: 0x003E882C File Offset: 0x003E6A2C
		private void RestorePlayerState()
		{
			if (!this._gameplayStateCaptured)
			{
				return;
			}
			if (this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(this._previousLockInput);
				this._playerInput.SetLockCameraInput(this._previousLockCamera);
			}
			Cursor.lockState = this._previousCursorLock;
			Cursor.visible = this._previousCursorVisible;
			this._gameplayStateCaptured = false;
			this._playerInput = null;
		}

		// Token: 0x06009894 RID: 39060 RVA: 0x003E8898 File Offset: 0x003E6A98
		private void TryTake(int index)
		{
			if (this._isCommitting || !this._isOpen || this._owner == null || index < 0 || index >= this._offers.Count || Time.frameCount <= this._selectionUnlockFrame)
			{
				return;
			}
			this._isCommitting = true;
			this.SetCardsInteractable(false);
			ClothingLootOffer clothingLootOffer;
			string text;
			if (this._owner.TryClaimOffer(index, out clothingLootOffer, out text))
			{
				string itemName = ((clothingLootOffer.clothing != null) ? clothingLootOffer.clothing.itemName : "Clothing");
				string itemScope = ((clothingLootOffer.clothing != null) ? "items" : "gameplay_residuals");
				ToastService.ShowComputed(() => GameLocalization.FormatSource("gameplay_residuals", "{0} added to your clothing inventory.", new object[] { GameLocalization.GetSource(itemScope, itemName) }), "Clothing found", -1f, "gameplay_residuals", null);
				this.CloseInternal(true);
				return;
			}
			string text2 = (string.IsNullOrEmpty(text) ? "COULDN'T TAKE THIS ITEM" : text);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._status, "gameplay_residuals", text2, (string value) => value.ToUpperInvariant());
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this._status.color = new Color(1f, 0.42f, 0.55f, 1f);
			this._isCommitting = false;
			this.SetCardsInteractable(true);
		}

		// Token: 0x06009895 RID: 39061 RVA: 0x003E89F8 File Offset: 0x003E6BF8
		private void SetCardsInteractable(bool value)
		{
			for (int i = 0; i < this._cards.Length; i++)
			{
				if (this._cards[i] != null && this._cards[i].root.gameObject.activeSelf)
				{
					this._cards[i].button.interactable = value;
				}
			}
		}

		// Token: 0x06009896 RID: 39062 RVA: 0x003E8A50 File Offset: 0x003E6C50
		private void CaptureUiSelection()
		{
			EventSystem current = EventSystem.current;
			this._previousSelectedObject = ((current != null) ? current.currentSelectedGameObject : null);
		}

		// Token: 0x06009897 RID: 39063 RVA: 0x003E8A7C File Offset: 0x003E6C7C
		private void SelectFirstCard()
		{
			EventSystem current = EventSystem.current;
			if (current == null || this._offers.Count == 0 || this._cards[0] == null)
			{
				return;
			}
			current.SetSelectedGameObject(this._cards[0].button.gameObject);
		}

		// Token: 0x06009898 RID: 39064 RVA: 0x003E8AC8 File Offset: 0x003E6CC8
		private void RestoreUiSelection()
		{
			EventSystem current = EventSystem.current;
			if (current != null)
			{
				GameObject currentSelectedGameObject = current.currentSelectedGameObject;
				if (currentSelectedGameObject != null && this._canvasRoot != null && currentSelectedGameObject.transform.IsChildOf(this._canvasRoot.transform))
				{
					GameObject gameObject = ((this._previousSelectedObject != null && this._previousSelectedObject.activeInHierarchy) ? this._previousSelectedObject : null);
					current.SetSelectedGameObject(gameObject);
				}
			}
			this._previousSelectedObject = null;
		}

		// Token: 0x06009899 RID: 39065 RVA: 0x003E8B50 File Offset: 0x003E6D50
		private void RefreshCards()
		{
			int count = this._offers.Count;
			float num = Mathf.Clamp((float)count * 266f + (float)Mathf.Max(0, count - 1) * 18f + 92f, 390f, 1260f);
			this._panel.sizeDelta = new Vector2(num, 570f);
			string text = ((count == 1) ? "Only one piece left. Take it or press Esc to leave." : "Pick one piece. The other choices will be gone.");
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._subtitle, "gameplay_residuals", text, null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			LocalizedGameText.BindSource(this._status, "gameplay_residuals", string.Empty, null);
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			for (int i = 0; i < this._cards.Length; i++)
			{
				ClothingLootChoiceUI.CardView cardView = this._cards[i];
				bool flag = i < count;
				cardView.root.gameObject.SetActive(flag);
				if (flag)
				{
					ClothingLootOffer clothingLootOffer = this._offers[i];
					Sprite icon = clothingLootOffer.Icon;
					cardView.icon.sprite = icon;
					StreamerCensorImage.BindItem(cardView.icon, clothingLootOffer.clothing);
					cardView.icon.enabled = icon != null;
					cardView.missingIcon.gameObject.SetActive(icon == null);
					string text2 = ((clothingLootOffer.clothing != null) ? clothingLootOffer.clothing.itemName : "UNKNOWN");
					LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(cardView.itemName, (clothingLootOffer.clothing != null) ? "items" : "gameplay_residuals", text2, (string value) => value.ToUpperInvariant());
					if (localizedGameText2 != null)
					{
						localizedGameText2.FitTranslationToRectangle(false);
					}
					LocalizedGameText localizedGameText3 = LocalizedGameText.BindSource(cardView.rarity, "rarity", clothingLootOffer.rarity.ToString().ToUpperInvariant(), null);
					if (localizedGameText3 != null)
					{
						localizedGameText3.FitTranslationToRectangle(false);
					}
					Color color = ((instance != null) ? instance.GetColor(clothingLootOffer.rarity) : Color.white);
					cardView.rarity.color = color;
					cardView.outline.effectColor = new Color(color.r, color.g, color.b, 0.95f);
					cardView.button.interactable = true;
					float num2 = ((float)i - (float)(count - 1) * 0.5f) * 284f;
					cardView.root.anchoredPosition = new Vector2(num2, -18f);
				}
			}
		}

		// Token: 0x0600989A RID: 39066 RVA: 0x003E8DEC File Offset: 0x003E6FEC
		private void BuildUi()
		{
			this._canvasRoot = new GameObject("ClothingLootCanvas", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			this._canvasRoot.transform.SetParent(base.transform, false);
			ClothingLootChoiceUI.SetUiLayer(this._canvasRoot);
			this._canvas = this._canvasRoot.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.overrideSorting = true;
			this._canvas.sortingOrder = 900;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = this._canvasRoot.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			RectTransform rectTransform = ClothingLootChoiceUI.CreateImage("Backdrop", this._canvasRoot.transform, new Color(0.012f, 0.008f, 0.018f, 0.76f), true).rectTransform;
			ClothingLootChoiceUI.Stretch(rectTransform);
			Image image = ClothingLootChoiceUI.CreateImage("Panel", rectTransform, new Color(0.075f, 0.055f, 0.105f, 0.985f), true);
			this._panel = image.rectTransform;
			RectTransform panel = this._panel;
			RectTransform panel2 = this._panel;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			panel2.anchorMax = vector;
			panel.anchorMin = vector;
			this._panel.pivot = new Vector2(0.5f, 0.5f);
			this._panel.anchoredPosition = Vector2.zero;
			this._panel.sizeDelta = new Vector2(1260f, 570f);
			Shadow shadow = image.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.65f);
			shadow.effectDistance = new Vector2(0f, -10f);
			RectTransform rectTransform2 = ClothingLootChoiceUI.CreateImage("Accent", this._panel, new Color(1f, 0.23f, 0.58f, 1f), false).rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(0f, 5f);
			ClothingLootChoiceUI.PlaceTop(ClothingLootChoiceUI.CreateText("Title", this._panel, "CHOOSE ONE", 34f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center).rectTransform, new Vector2(0f, -25f), new Vector2(800f, 48f));
			this._subtitle = ClothingLootChoiceUI.CreateText("Subtitle", this._panel, string.Empty, 17f, FontStyles.Normal, new Color(0.74f, 0.7f, 0.8f, 1f), TextAlignmentOptions.Center);
			ClothingLootChoiceUI.PlaceTop(this._subtitle.rectTransform, new Vector2(0f, -72f), new Vector2(900f, 30f));
			Button button = ClothingLootChoiceUI.CreateButton("Close", this._panel, new Color(0.18f, 0.14f, 0.23f, 0.95f));
			RectTransform component2 = button.GetComponent<RectTransform>();
			RectTransform rectTransform3 = component2;
			RectTransform rectTransform4 = component2;
			vector = new Vector2(1f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			component2.pivot = new Vector2(1f, 1f);
			component2.anchoredPosition = new Vector2(-18f, -18f);
			component2.sizeDelta = new Vector2(42f, 42f);
			ClothingLootChoiceUI.Stretch(ClothingLootChoiceUI.CreateText("Label", component2, "×", 30f, FontStyles.Normal, Color.white, TextAlignmentOptions.Center).rectTransform);
			button.onClick.AddListener(new UnityAction(this.Close));
			for (int i = 0; i < this._cards.Length; i++)
			{
				this._cards[i] = this.CreateCard(i);
			}
			this._status = ClothingLootChoiceUI.CreateText("Status", this._panel, string.Empty, 16f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);
			RectTransform rectTransform5 = this._status.rectTransform;
			RectTransform rectTransform6 = this._status.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform6.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			this._status.rectTransform.pivot = new Vector2(0.5f, 0f);
			this._status.rectTransform.anchoredPosition = new Vector2(0f, 39f);
			this._status.rectTransform.sizeDelta = new Vector2(1000f, 28f);
			TextMeshProUGUI textMeshProUGUI = ClothingLootChoiceUI.CreateText("EscapeHint", this._panel, "ESC  CLOSE", 13f, FontStyles.Bold, new Color(0.55f, 0.51f, 0.61f, 1f), TextAlignmentOptions.Center);
			RectTransform rectTransform7 = textMeshProUGUI.rectTransform;
			RectTransform rectTransform8 = textMeshProUGUI.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform8.anchorMax = vector;
			rectTransform7.anchorMin = vector;
			textMeshProUGUI.rectTransform.pivot = new Vector2(0.5f, 0f);
			textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(0f, 14f);
			textMeshProUGUI.rectTransform.sizeDelta = new Vector2(300f, 22f);
			this._canvasRoot.SetActive(false);
		}

		// Token: 0x0600989B RID: 39067 RVA: 0x003E93B0 File Offset: 0x003E75B0
		private ClothingLootChoiceUI.CardView CreateCard(int index)
		{
			ClothingLootChoiceUI.CardView cardView = new ClothingLootChoiceUI.CardView();
			cardView.background = ClothingLootChoiceUI.CreateImage("Choice_" + (index + 1).ToString(), this._panel, new Color(0.115f, 0.086f, 0.15f, 1f), true);
			cardView.root = cardView.background.rectTransform;
			RectTransform root = cardView.root;
			RectTransform root2 = cardView.root;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			root2.anchorMax = vector;
			root.anchorMin = vector;
			cardView.root.pivot = new Vector2(0.5f, 0.5f);
			cardView.root.sizeDelta = new Vector2(266f, 350f);
			cardView.outline = cardView.background.gameObject.AddComponent<Outline>();
			cardView.outline.effectDistance = new Vector2(3f, 3f);
			cardView.outline.useGraphicAlpha = false;
			cardView.button = cardView.background.gameObject.AddComponent<Button>();
			cardView.button.targetGraphic = cardView.background;
			cardView.button.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic
			};
			ColorBlock colors = cardView.button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1.12f, 1.08f, 1.15f, 1f);
			colors.pressedColor = new Color(0.82f, 0.76f, 0.9f, 1f);
			colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.65f);
			colors.fadeDuration = 0.08f;
			cardView.button.colors = colors;
			int capturedIndex = index;
			cardView.button.onClick.AddListener(delegate
			{
				this.TryTake(capturedIndex);
			});
			cardView.icon = ClothingLootChoiceUI.CreateImage("Icon", cardView.root, Color.white, false);
			RectTransform rectTransform = cardView.icon.rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -20f);
			rectTransform.sizeDelta = new Vector2(208f, 208f);
			cardView.icon.preserveAspect = true;
			cardView.missingIcon = ClothingLootChoiceUI.CreateText("MissingIcon", cardView.root, "?", 72f, FontStyles.Bold, new Color(0.52f, 0.47f, 0.58f, 1f), TextAlignmentOptions.Center);
			RectTransform rectTransform2 = cardView.missingIcon.rectTransform;
			RectTransform rectTransform3 = cardView.missingIcon.rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			cardView.missingIcon.rectTransform.pivot = new Vector2(0.5f, 1f);
			cardView.missingIcon.rectTransform.anchoredPosition = new Vector2(0f, -70f);
			cardView.missingIcon.rectTransform.sizeDelta = new Vector2(160f, 100f);
			cardView.itemName = ClothingLootChoiceUI.CreateText("Name", cardView.root, string.Empty, 21f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);
			cardView.itemName.enableAutoSizing = true;
			cardView.itemName.fontSizeMin = 14f;
			cardView.itemName.fontSizeMax = 21f;
			RectTransform rectTransform4 = cardView.itemName.rectTransform;
			RectTransform rectTransform5 = cardView.itemName.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform5.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			cardView.itemName.rectTransform.pivot = new Vector2(0.5f, 0f);
			cardView.itemName.rectTransform.anchoredPosition = new Vector2(0f, 75f);
			cardView.itemName.rectTransform.sizeDelta = new Vector2(226f, 46f);
			cardView.rarity = ClothingLootChoiceUI.CreateText("Rarity", cardView.root, string.Empty, 14f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);
			RectTransform rectTransform6 = cardView.rarity.rectTransform;
			RectTransform rectTransform7 = cardView.rarity.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			cardView.rarity.rectTransform.pivot = new Vector2(0.5f, 0f);
			cardView.rarity.rectTransform.anchoredPosition = new Vector2(0f, 48f);
			cardView.rarity.rectTransform.sizeDelta = new Vector2(220f, 22f);
			cardView.take = ClothingLootChoiceUI.CreateText("Take", cardView.root, "TAKE", 14f, FontStyles.Bold, new Color(0.94f, 0.9f, 0.98f, 1f), TextAlignmentOptions.Center);
			RectTransform rectTransform8 = cardView.take.rectTransform;
			RectTransform rectTransform9 = cardView.take.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform9.anchorMax = vector;
			rectTransform8.anchorMin = vector;
			cardView.take.rectTransform.pivot = new Vector2(0.5f, 0f);
			cardView.take.rectTransform.anchoredPosition = new Vector2(0f, 18f);
			cardView.take.rectTransform.sizeDelta = new Vector2(180f, 24f);
			return cardView;
		}

		// Token: 0x0600989C RID: 39068 RVA: 0x003E998E File Offset: 0x003E7B8E
		private static Image CreateImage(string name, Transform parent, Color color, bool raycastTarget)
		{
			Image image = ClothingLootChoiceUI.NewUiObject(name, parent).AddComponent<Image>();
			image.color = color;
			image.raycastTarget = raycastTarget;
			return image;
		}

		// Token: 0x0600989D RID: 39069 RVA: 0x003E99AC File Offset: 0x003E7BAC
		private static Button CreateButton(string name, Transform parent, Color color)
		{
			Image image = ClothingLootChoiceUI.CreateImage(name, parent, color, true);
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic
			};
			return button;
		}

		// Token: 0x0600989E RID: 39070 RVA: 0x003E99EC File Offset: 0x003E7BEC
		private static TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = ClothingLootChoiceUI.NewUiObject(name, parent).AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			textMeshProUGUI.raycastTarget = false;
			if (value == "×" || value == "?")
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			else if (!string.IsNullOrEmpty(value))
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "gameplay_residuals", value, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			return textMeshProUGUI;
		}

		// Token: 0x0600989F RID: 39071 RVA: 0x003E9A86 File Offset: 0x003E7C86
		private static GameObject NewUiObject(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			ClothingLootChoiceUI.SetUiLayer(gameObject);
			gameObject.transform.SetParent(parent, false);
			return gameObject;
		}

		// Token: 0x060098A0 RID: 39072 RVA: 0x003E9AB4 File Offset: 0x003E7CB4
		private static void SetUiLayer(GameObject target)
		{
			int num = LayerMask.NameToLayer("UI");
			target.layer = ((num >= 0) ? num : 5);
		}

		// Token: 0x060098A1 RID: 39073 RVA: 0x003E9ADC File Offset: 0x003E7CDC
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x060098A2 RID: 39074 RVA: 0x003E9B2C File Offset: 0x003E7D2C
		private static void PlaceTop(RectTransform rect, Vector2 position, Vector2 size)
		{
			Vector2 vector = new Vector2(0.5f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0.5f, 1f);
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x04009D99 RID: 40345
		private const string RuntimeObjectName = "[Clothing Loot Choice UI]";

		// Token: 0x04009D9A RID: 40346
		private const string GateReason = "ClothingLootChoice";

		// Token: 0x04009D9B RID: 40347
		private const string LocalizationScope = "gameplay_residuals";

		// Token: 0x04009D9C RID: 40348
		private const int SortingOrder = 900;

		// Token: 0x04009D9D RID: 40349
		private const float CardWidth = 266f;

		// Token: 0x04009D9E RID: 40350
		private const float CardHeight = 350f;

		// Token: 0x04009D9F RID: 40351
		private const float CardGap = 18f;

		// Token: 0x04009DA0 RID: 40352
		private const PlayerContextResource ModalResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI;

		// Token: 0x04009DA2 RID: 40354
		private readonly List<ClothingLootOffer> _offers = new List<ClothingLootOffer>(4);

		// Token: 0x04009DA3 RID: 40355
		private readonly ClothingLootChoiceUI.CardView[] _cards = new ClothingLootChoiceUI.CardView[4];

		// Token: 0x04009DA4 RID: 40356
		private ClothingLootTableInteractable _owner;

		// Token: 0x04009DA5 RID: 40357
		private PlayerActivityLease _lease;

		// Token: 0x04009DA6 RID: 40358
		private GameObject _canvasRoot;

		// Token: 0x04009DA7 RID: 40359
		private Canvas _canvas;

		// Token: 0x04009DA8 RID: 40360
		private RectTransform _panel;

		// Token: 0x04009DA9 RID: 40361
		private TextMeshProUGUI _subtitle;

		// Token: 0x04009DAA RID: 40362
		private TextMeshProUGUI _status;

		// Token: 0x04009DAB RID: 40363
		private bool _isOpen;

		// Token: 0x04009DAC RID: 40364
		private bool _isCommitting;

		// Token: 0x04009DAD RID: 40365
		private bool _isClosing;

		// Token: 0x04009DAE RID: 40366
		private int _selectionUnlockFrame;

		// Token: 0x04009DAF RID: 40367
		private GameObject _previousSelectedObject;

		// Token: 0x04009DB0 RID: 40368
		private vThirdPersonInput _playerInput;

		// Token: 0x04009DB1 RID: 40369
		private bool _previousLockInput;

		// Token: 0x04009DB2 RID: 40370
		private bool _previousLockCamera;

		// Token: 0x04009DB3 RID: 40371
		private bool _gameplayStateCaptured;

		// Token: 0x04009DB4 RID: 40372
		private CursorLockMode _previousCursorLock;

		// Token: 0x04009DB5 RID: 40373
		private bool _previousCursorVisible;

		// Token: 0x02001347 RID: 4935
		private sealed class CardView
		{
			// Token: 0x04009DB6 RID: 40374
			public RectTransform root;

			// Token: 0x04009DB7 RID: 40375
			public Button button;

			// Token: 0x04009DB8 RID: 40376
			public Image background;

			// Token: 0x04009DB9 RID: 40377
			public Image icon;

			// Token: 0x04009DBA RID: 40378
			public TextMeshProUGUI missingIcon;

			// Token: 0x04009DBB RID: 40379
			public TextMeshProUGUI itemName;

			// Token: 0x04009DBC RID: 40380
			public TextMeshProUGUI rarity;

			// Token: 0x04009DBD RID: 40381
			public TextMeshProUGUI take;

			// Token: 0x04009DBE RID: 40382
			public Outline outline;
		}
	}
}
