using System;
using System.Collections.Generic;
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

namespace Features.InsertableToys.Vegetables
{
	// Token: 0x0200117E RID: 4478
	public sealed class VegetableTheftChoiceUI : MonoBehaviour
	{
		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x060088ED RID: 35053 RVA: 0x0038026C File Offset: 0x0037E46C
		// (set) Token: 0x060088EE RID: 35054 RVA: 0x00380273 File Offset: 0x0037E473
		public static VegetableTheftChoiceUI Instance { get; private set; }

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x060088EF RID: 35055 RVA: 0x0038027B File Offset: 0x0037E47B
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x060088F0 RID: 35056 RVA: 0x00380283 File Offset: 0x0037E483
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			VegetableTheftChoiceUI.Instance = null;
		}

		// Token: 0x060088F1 RID: 35057 RVA: 0x0038028C File Offset: 0x0037E48C
		public static bool TryOpen(VegetableTheftSource owner, GameObject player, IReadOnlyList<InsertableToyItemSO> vegetables)
		{
			if (owner == null || vegetables == null || vegetables.Count == 0)
			{
				return false;
			}
			VegetableTheftChoiceUI vegetableTheftChoiceUI = VegetableTheftChoiceUI.EnsureInstance();
			return vegetableTheftChoiceUI != null && vegetableTheftChoiceUI.OpenInternal(owner, player, vegetables);
		}

		// Token: 0x060088F2 RID: 35058 RVA: 0x003802C9 File Offset: 0x0037E4C9
		public static void CloseIfOwnedBy(VegetableTheftSource owner)
		{
			if (VegetableTheftChoiceUI.Instance != null && VegetableTheftChoiceUI.Instance._owner == owner)
			{
				VegetableTheftChoiceUI.Instance.CloseInternal(true);
			}
		}

		// Token: 0x060088F3 RID: 35059 RVA: 0x003802F8 File Offset: 0x0037E4F8
		private static VegetableTheftChoiceUI EnsureInstance()
		{
			if (VegetableTheftChoiceUI.Instance != null)
			{
				return VegetableTheftChoiceUI.Instance;
			}
			VegetableTheftChoiceUI vegetableTheftChoiceUI = Object.FindObjectOfType<VegetableTheftChoiceUI>(true);
			if (vegetableTheftChoiceUI != null)
			{
				VegetableTheftChoiceUI.Instance = vegetableTheftChoiceUI;
				return vegetableTheftChoiceUI;
			}
			GameObject gameObject = new GameObject("[Vegetable Theft Choice UI]");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<VegetableTheftChoiceUI>();
		}

		// Token: 0x060088F4 RID: 35060 RVA: 0x00380345 File Offset: 0x0037E545
		private void Awake()
		{
			if (VegetableTheftChoiceUI.Instance != null && VegetableTheftChoiceUI.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			VegetableTheftChoiceUI.Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.BuildUi();
		}

		// Token: 0x060088F5 RID: 35061 RVA: 0x00380384 File Offset: 0x0037E584
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

		// Token: 0x060088F6 RID: 35062 RVA: 0x003803B4 File Offset: 0x0037E5B4
		private void OnDestroy()
		{
			this.CloseInternal(true);
			if (VegetableTheftChoiceUI.Instance == this)
			{
				VegetableTheftChoiceUI.Instance = null;
			}
		}

		// Token: 0x060088F7 RID: 35063 RVA: 0x003803D0 File Offset: 0x0037E5D0
		private void OnDisable()
		{
			this.CloseInternal(true);
		}

		// Token: 0x060088F8 RID: 35064 RVA: 0x003803DC File Offset: 0x0037E5DC
		private bool OpenInternal(VegetableTheftSource owner, GameObject player, IReadOnlyList<InsertableToyItemSO> vegetables)
		{
			if (this._isOpen || this._isClosing || owner == null || vegetables == null || vegetables.Count == 0)
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
			bool flag2;
			try
			{
				this._vegetables.Clear();
				bool flag = false;
				int num = 0;
				while (num < vegetables.Count && num < 4)
				{
					InsertableToyItemSO insertableToyItemSO = vegetables[num];
					this._vegetables.Add(insertableToyItemSO);
					flag |= insertableToyItemSO != null;
					num++;
				}
				if (!flag)
				{
					this._owner = null;
					this.ReleaseModalContext();
					flag2 = false;
				}
				else
				{
					this._isOpen = true;
					this._isCommitting = false;
					this._selectionUnlockFrame = Time.frameCount;
					this.ResolveAndLockPlayer(player);
					this.RefreshCards();
					this.CaptureUiSelection();
					this._canvasRoot.SetActive(true);
					this.SelectFirstCard();
					flag2 = true;
				}
			}
			catch
			{
				this.CloseInternal(true);
				throw;
			}
			return flag2;
		}

		// Token: 0x060088F9 RID: 35065 RVA: 0x003803D0 File Offset: 0x0037E5D0
		public void Close()
		{
			this.CloseInternal(true);
		}

		// Token: 0x060088FA RID: 35066 RVA: 0x00380524 File Offset: 0x0037E724
		private void CloseInternal(bool notifyOwner)
		{
			if (this._isClosing)
			{
				return;
			}
			this._isClosing = true;
			try
			{
				VegetableTheftSource owner = this._owner;
				if (!this._isOpen && !this._gameplayStateCaptured && this._lease == null)
				{
					this._owner = null;
					this._vegetables.Clear();
					if (notifyOwner && owner != null)
					{
						owner.NotifyTheftMenuClosed();
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
					this._vegetables.Clear();
					if (notifyOwner && owner != null)
					{
						owner.NotifyTheftMenuClosed();
					}
				}
			}
			finally
			{
				this._isClosing = false;
			}
		}

		// Token: 0x060088FB RID: 35067 RVA: 0x00380600 File Offset: 0x0037E800
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
			bool flag = PlayerActivityGate.TryAcquire("VegetableTheftChoice#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI, PlayerActivity.AllPlayerActions, out playerActivityLease, false, null);
			this._lease = playerActivityLease;
			return flag && this._lease != null && this._lease.IsActive;
		}

		// Token: 0x060088FC RID: 35068 RVA: 0x00380674 File Offset: 0x0037E874
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

		// Token: 0x060088FD RID: 35069 RVA: 0x00380690 File Offset: 0x0037E890
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

		// Token: 0x060088FE RID: 35070 RVA: 0x003807B4 File Offset: 0x0037E9B4
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

		// Token: 0x060088FF RID: 35071 RVA: 0x00380820 File Offset: 0x0037EA20
		private void TrySteal(int index)
		{
			if (this._isCommitting || !this._isOpen || this._owner == null || index < 0 || index >= this._vegetables.Count || Time.frameCount <= this._selectionUnlockFrame)
			{
				return;
			}
			this._isCommitting = true;
			this.SetCardsInteractable(false);
			InsertableToyItemSO insertableToyItemSO;
			string text;
			if (this._owner.TryClaimVegetable(index, out insertableToyItemSO, out text))
			{
				string itemName = ((insertableToyItemSO != null && !string.IsNullOrWhiteSpace(insertableToyItemSO.itemName)) ? insertableToyItemSO.itemName : "Vegetable");
				string itemScope = ((insertableToyItemSO != null && !string.IsNullOrWhiteSpace(insertableToyItemSO.itemName)) ? "items" : "pencil_vegetables");
				ToastService.ShowComputed(() => GameLocalization.FormatSource("pencil_vegetables", "{0} is now in Ciel's hand.", new object[] { GameLocalization.GetSource(itemScope, itemName) }), "Vegetable stolen", -1f, "pencil_vegetables", null);
				this.CloseInternal(true);
				return;
			}
			string text2 = (string.IsNullOrEmpty(text) ? "COULDN'T STEAL THIS VEGETABLE" : text);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._status, "pencil_vegetables", text2, (string value) => value.ToUpperInvariant());
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this._status.color = new Color(1f, 0.42f, 0.55f, 1f);
			this._isCommitting = false;
			this.SetCardsInteractable(true);
		}

		// Token: 0x06008900 RID: 35072 RVA: 0x00380988 File Offset: 0x0037EB88
		private void SetCardsInteractable(bool value)
		{
			for (int i = 0; i < this._cards.Length; i++)
			{
				VegetableTheftChoiceUI.CardView cardView = this._cards[i];
				if (cardView != null && cardView.root.gameObject.activeSelf)
				{
					cardView.button.interactable = value;
				}
			}
		}

		// Token: 0x06008901 RID: 35073 RVA: 0x003809D4 File Offset: 0x0037EBD4
		private void CaptureUiSelection()
		{
			EventSystem current = EventSystem.current;
			this._previousSelectedObject = ((current != null) ? current.currentSelectedGameObject : null);
		}

		// Token: 0x06008902 RID: 35074 RVA: 0x00380A00 File Offset: 0x0037EC00
		private void SelectFirstCard()
		{
			EventSystem current = EventSystem.current;
			if (current == null || this._vegetables.Count == 0)
			{
				return;
			}
			int num = 0;
			while (num < this._vegetables.Count && num < this._cards.Length)
			{
				if (!(this._vegetables[num] == null) && this._cards[num] != null)
				{
					current.SetSelectedGameObject(this._cards[num].button.gameObject);
					return;
				}
				num++;
			}
		}

		// Token: 0x06008903 RID: 35075 RVA: 0x00380A84 File Offset: 0x0037EC84
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

		// Token: 0x06008904 RID: 35076 RVA: 0x00380B0C File Offset: 0x0037ED0C
		private void RefreshCards()
		{
			this._panel.sizeDelta = new Vector2(920f, 780f);
			LocalizedGameText.BindSource(this._status, "pencil_vegetables", string.Empty, null);
			int num = Mathf.Min(this._vegetables.Count, this._cards.Length);
			int num2 = Mathf.CeilToInt((float)num / 2f);
			for (int i = 0; i < this._cards.Length; i++)
			{
				VegetableTheftChoiceUI.CardView cardView = this._cards[i];
				bool flag = i < num;
				cardView.root.gameObject.SetActive(flag);
				if (flag)
				{
					InsertableToyItemSO insertableToyItemSO = this._vegetables[i];
					Sprite sprite = ((insertableToyItemSO != null) ? insertableToyItemSO.icon : null);
					cardView.icon.sprite = sprite;
					cardView.icon.enabled = sprite != null;
					cardView.missingIcon.gameObject.SetActive(sprite == null);
					bool flag2 = insertableToyItemSO != null && !string.IsNullOrWhiteSpace(insertableToyItemSO.itemName);
					string text = (flag2 ? insertableToyItemSO.itemName : "UNKNOWN VEGETABLE");
					LocalizedGameText localizedGameText = LocalizedGameText.BindSource(cardView.itemName, flag2 ? "items" : "pencil_vegetables", text, (string value) => value.ToUpperInvariant());
					if (localizedGameText != null)
					{
						localizedGameText.FitTranslationToRectangle(false);
					}
					cardView.button.interactable = insertableToyItemSO != null;
					int num3 = i / 2;
					int num4 = num3 * 2;
					int num5 = Mathf.Min(2, num - num4);
					float num6 = ((float)(i - num4) - (float)(num5 - 1) * 0.5f) * 284f;
					float num7 = -55f + ((float)(num2 - 1) * 0.5f - (float)num3) * 268f;
					cardView.root.anchoredPosition = new Vector2(num6, num7);
				}
			}
		}

		// Token: 0x06008905 RID: 35077 RVA: 0x00380CFC File Offset: 0x0037EEFC
		private void BuildUi()
		{
			this._canvasRoot = new GameObject("VegetableTheftCanvas", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			this._canvasRoot.transform.SetParent(base.transform, false);
			VegetableTheftChoiceUI.SetUiLayer(this._canvasRoot);
			Canvas component = this._canvasRoot.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.overrideSorting = true;
			component.sortingOrder = 900;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = this._canvasRoot.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.matchWidthOrHeight = 0.5f;
			RectTransform rectTransform = VegetableTheftChoiceUI.CreateImage("Backdrop", this._canvasRoot.transform, new Color(0.012f, 0.008f, 0.018f, 0.76f), true).rectTransform;
			VegetableTheftChoiceUI.Stretch(rectTransform);
			Image image = VegetableTheftChoiceUI.CreateImage("Panel", rectTransform, new Color(0.075f, 0.055f, 0.105f, 0.985f), true);
			this._panel = image.rectTransform;
			RectTransform panel = this._panel;
			RectTransform panel2 = this._panel;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			panel2.anchorMax = vector;
			panel.anchorMin = vector;
			this._panel.pivot = new Vector2(0.5f, 0.5f);
			this._panel.anchoredPosition = Vector2.zero;
			this._panel.sizeDelta = new Vector2(920f, 780f);
			Shadow shadow = image.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.65f);
			shadow.effectDistance = new Vector2(0f, -10f);
			RectTransform rectTransform2 = VegetableTheftChoiceUI.CreateImage("Accent", this._panel, new Color(0.33f, 0.87f, 0.45f, 1f), false).rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(0f, 5f);
			VegetableTheftChoiceUI.PlaceTop(VegetableTheftChoiceUI.CreateText("Title", this._panel, "CHOOSE A VEGETABLE", 34f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center).rectTransform, new Vector2(0f, -25f), new Vector2(800f, 48f));
			VegetableTheftChoiceUI.PlaceTop(VegetableTheftChoiceUI.CreateText("Subtitle", this._panel, "Choose one. It will be placed in Ciel's hand.", 17f, FontStyles.Normal, new Color(0.74f, 0.7f, 0.8f, 1f), TextAlignmentOptions.Center).rectTransform, new Vector2(0f, -72f), new Vector2(800f, 30f));
			Button button = VegetableTheftChoiceUI.CreateButton("Close", this._panel, new Color(0.18f, 0.14f, 0.23f, 0.95f));
			RectTransform component3 = button.GetComponent<RectTransform>();
			RectTransform rectTransform3 = component3;
			RectTransform rectTransform4 = component3;
			vector = new Vector2(1f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			component3.pivot = new Vector2(1f, 1f);
			component3.anchoredPosition = new Vector2(-18f, -18f);
			component3.sizeDelta = new Vector2(42f, 42f);
			VegetableTheftChoiceUI.Stretch(VegetableTheftChoiceUI.CreateText("Label", component3, "×", 30f, FontStyles.Normal, Color.white, TextAlignmentOptions.Center).rectTransform);
			button.onClick.AddListener(new UnityAction(this.Close));
			for (int i = 0; i < this._cards.Length; i++)
			{
				this._cards[i] = this.CreateCard(i);
			}
			this._status = VegetableTheftChoiceUI.CreateText("Status", this._panel, string.Empty, 16f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);
			RectTransform rectTransform5 = this._status.rectTransform;
			RectTransform rectTransform6 = this._status.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform6.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			this._status.rectTransform.pivot = new Vector2(0.5f, 0f);
			this._status.rectTransform.anchoredPosition = new Vector2(0f, 40f);
			this._status.rectTransform.sizeDelta = new Vector2(800f, 28f);
			TextMeshProUGUI textMeshProUGUI = VegetableTheftChoiceUI.CreateText("EscapeHint", this._panel, "ESC  CLOSE", 13f, FontStyles.Bold, new Color(0.55f, 0.51f, 0.61f, 1f), TextAlignmentOptions.Center);
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

		// Token: 0x06008906 RID: 35078 RVA: 0x00381298 File Offset: 0x0037F498
		private VegetableTheftChoiceUI.CardView CreateCard(int index)
		{
			VegetableTheftChoiceUI.CardView cardView = new VegetableTheftChoiceUI.CardView();
			cardView.background = VegetableTheftChoiceUI.CreateImage("Choice_" + (index + 1).ToString(), this._panel, new Color(0.115f, 0.086f, 0.15f, 1f), true);
			cardView.root = cardView.background.rectTransform;
			RectTransform root = cardView.root;
			RectTransform root2 = cardView.root;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			root2.anchorMax = vector;
			root.anchorMin = vector;
			cardView.root.pivot = new Vector2(0.5f, 0.5f);
			cardView.root.sizeDelta = new Vector2(266f, 250f);
			cardView.outline = cardView.background.gameObject.AddComponent<Outline>();
			cardView.outline.effectColor = new Color(0.33f, 0.87f, 0.45f, 0.95f);
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
			colors.highlightedColor = new Color(1.1f, 1.14f, 1.1f, 1f);
			colors.pressedColor = new Color(0.78f, 0.88f, 0.8f, 1f);
			colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.65f);
			colors.fadeDuration = 0.08f;
			cardView.button.colors = colors;
			int capturedIndex = index;
			cardView.button.onClick.AddListener(delegate
			{
				this.TrySteal(capturedIndex);
			});
			cardView.icon = VegetableTheftChoiceUI.CreateImage("Icon", cardView.root, Color.white, false);
			RectTransform rectTransform = cardView.icon.rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -14f);
			rectTransform.sizeDelta = new Vector2(180f, 150f);
			cardView.icon.preserveAspect = true;
			cardView.missingIcon = VegetableTheftChoiceUI.CreateText("MissingIcon", cardView.root, "?", 64f, FontStyles.Bold, new Color(0.52f, 0.47f, 0.58f, 1f), TextAlignmentOptions.Center);
			RectTransform rectTransform2 = cardView.missingIcon.rectTransform;
			RectTransform rectTransform3 = cardView.missingIcon.rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			cardView.missingIcon.rectTransform.pivot = new Vector2(0.5f, 1f);
			cardView.missingIcon.rectTransform.anchoredPosition = new Vector2(0f, -42f);
			cardView.missingIcon.rectTransform.sizeDelta = new Vector2(150f, 90f);
			cardView.itemName = VegetableTheftChoiceUI.CreateText("Name", cardView.root, string.Empty, 20f, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);
			cardView.itemName.enableAutoSizing = true;
			cardView.itemName.fontSizeMin = 13f;
			cardView.itemName.fontSizeMax = 20f;
			RectTransform rectTransform4 = cardView.itemName.rectTransform;
			RectTransform rectTransform5 = cardView.itemName.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform5.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			cardView.itemName.rectTransform.pivot = new Vector2(0.5f, 0f);
			cardView.itemName.rectTransform.anchoredPosition = new Vector2(0f, 49f);
			cardView.itemName.rectTransform.sizeDelta = new Vector2(226f, 42f);
			cardView.steal = VegetableTheftChoiceUI.CreateText("Steal", cardView.root, "STEAL", 14f, FontStyles.Bold, new Color(0.88f, 1f, 0.9f, 1f), TextAlignmentOptions.Center);
			RectTransform rectTransform6 = cardView.steal.rectTransform;
			RectTransform rectTransform7 = cardView.steal.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			cardView.steal.rectTransform.pivot = new Vector2(0.5f, 0f);
			cardView.steal.rectTransform.anchoredPosition = new Vector2(0f, 17f);
			cardView.steal.rectTransform.sizeDelta = new Vector2(180f, 24f);
			return cardView;
		}

		// Token: 0x06008907 RID: 35079 RVA: 0x003817DD File Offset: 0x0037F9DD
		private static Image CreateImage(string name, Transform parent, Color color, bool raycastTarget)
		{
			Image image = VegetableTheftChoiceUI.NewUiObject(name, parent).AddComponent<Image>();
			image.color = color;
			image.raycastTarget = raycastTarget;
			return image;
		}

		// Token: 0x06008908 RID: 35080 RVA: 0x003817FC File Offset: 0x0037F9FC
		private static Button CreateButton(string name, Transform parent, Color color)
		{
			Image image = VegetableTheftChoiceUI.CreateImage(name, parent, color, true);
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic
			};
			return button;
		}

		// Token: 0x06008909 RID: 35081 RVA: 0x0038183C File Offset: 0x0037FA3C
		private static TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = VegetableTheftChoiceUI.NewUiObject(name, parent).AddComponent<TextMeshProUGUI>();
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
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "pencil_vegetables", value, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			return textMeshProUGUI;
		}

		// Token: 0x0600890A RID: 35082 RVA: 0x003818D6 File Offset: 0x0037FAD6
		private static GameObject NewUiObject(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			VegetableTheftChoiceUI.SetUiLayer(gameObject);
			gameObject.transform.SetParent(parent, false);
			return gameObject;
		}

		// Token: 0x0600890B RID: 35083 RVA: 0x00381904 File Offset: 0x0037FB04
		private static void SetUiLayer(GameObject target)
		{
			int num = LayerMask.NameToLayer("UI");
			target.layer = ((num >= 0) ? num : 5);
		}

		// Token: 0x0600890C RID: 35084 RVA: 0x0038192C File Offset: 0x0037FB2C
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x0600890D RID: 35085 RVA: 0x0038197C File Offset: 0x0037FB7C
		private static void PlaceTop(RectTransform rect, Vector2 position, Vector2 size)
		{
			Vector2 vector = new Vector2(0.5f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0.5f, 1f);
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x04008B86 RID: 35718
		private const string RuntimeObjectName = "[Vegetable Theft Choice UI]";

		// Token: 0x04008B87 RID: 35719
		private const string GateReason = "VegetableTheftChoice";

		// Token: 0x04008B88 RID: 35720
		private const string LocalizationScope = "pencil_vegetables";

		// Token: 0x04008B89 RID: 35721
		private const int SortingOrder = 900;

		// Token: 0x04008B8A RID: 35722
		private const int CardCount = 4;

		// Token: 0x04008B8B RID: 35723
		private const int ColumnCount = 2;

		// Token: 0x04008B8C RID: 35724
		private const float CardWidth = 266f;

		// Token: 0x04008B8D RID: 35725
		private const float CardHeight = 250f;

		// Token: 0x04008B8E RID: 35726
		private const float ColumnGap = 18f;

		// Token: 0x04008B8F RID: 35727
		private const float RowGap = 18f;

		// Token: 0x04008B90 RID: 35728
		private const float PanelWidth = 920f;

		// Token: 0x04008B91 RID: 35729
		private const float PanelHeight = 780f;

		// Token: 0x04008B92 RID: 35730
		private const float ContentCenterY = -55f;

		// Token: 0x04008B93 RID: 35731
		private const PlayerContextResource ModalResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI;

		// Token: 0x04008B95 RID: 35733
		private readonly List<InsertableToyItemSO> _vegetables = new List<InsertableToyItemSO>(4);

		// Token: 0x04008B96 RID: 35734
		private readonly VegetableTheftChoiceUI.CardView[] _cards = new VegetableTheftChoiceUI.CardView[4];

		// Token: 0x04008B97 RID: 35735
		private VegetableTheftSource _owner;

		// Token: 0x04008B98 RID: 35736
		private PlayerActivityLease _lease;

		// Token: 0x04008B99 RID: 35737
		private GameObject _canvasRoot;

		// Token: 0x04008B9A RID: 35738
		private RectTransform _panel;

		// Token: 0x04008B9B RID: 35739
		private TextMeshProUGUI _status;

		// Token: 0x04008B9C RID: 35740
		private bool _isOpen;

		// Token: 0x04008B9D RID: 35741
		private bool _isCommitting;

		// Token: 0x04008B9E RID: 35742
		private bool _isClosing;

		// Token: 0x04008B9F RID: 35743
		private int _selectionUnlockFrame;

		// Token: 0x04008BA0 RID: 35744
		private GameObject _previousSelectedObject;

		// Token: 0x04008BA1 RID: 35745
		private vThirdPersonInput _playerInput;

		// Token: 0x04008BA2 RID: 35746
		private bool _previousLockInput;

		// Token: 0x04008BA3 RID: 35747
		private bool _previousLockCamera;

		// Token: 0x04008BA4 RID: 35748
		private bool _gameplayStateCaptured;

		// Token: 0x04008BA5 RID: 35749
		private CursorLockMode _previousCursorLock;

		// Token: 0x04008BA6 RID: 35750
		private bool _previousCursorVisible;

		// Token: 0x0200117F RID: 4479
		private sealed class CardView
		{
			// Token: 0x04008BA7 RID: 35751
			public RectTransform root;

			// Token: 0x04008BA8 RID: 35752
			public Button button;

			// Token: 0x04008BA9 RID: 35753
			public Image background;

			// Token: 0x04008BAA RID: 35754
			public Image icon;

			// Token: 0x04008BAB RID: 35755
			public TextMeshProUGUI missingIcon;

			// Token: 0x04008BAC RID: 35756
			public TextMeshProUGUI itemName;

			// Token: 0x04008BAD RID: 35757
			public TextMeshProUGUI steal;

			// Token: 0x04008BAE RID: 35758
			public Outline outline;
		}
	}
}
