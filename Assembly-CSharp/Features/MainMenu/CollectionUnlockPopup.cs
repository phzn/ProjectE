using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.CensorBars;
using Features.Detection;
using Features.GameOver;
using Features.Inventory;
using Features.Loading;
using Features.Localization;
using Features.Missions;
using Features.PhoneCompanion;
using Features.RunModes;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001033 RID: 4147
	public sealed class CollectionUnlockPopup : MonoBehaviour
	{
		// Token: 0x06007D89 RID: 32137 RVA: 0x00336BDE File Offset: 0x00334DDE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Install()
		{
			CollectionUnlockPopup._instance = null;
			CollectionUnlockRegistry.GameplayItemDiscovered -= CollectionUnlockPopup.Notify;
			CollectionUnlockRegistry.GameplayItemDiscovered += CollectionUnlockPopup.Notify;
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x00336C08 File Offset: 0x00334E08
		private static void Notify(InventoryItemSO item, ItemRarity rarity, int variant)
		{
			if (!Application.isPlaying || item == null || !CollectionUnlockPopup.IsRunContext())
			{
				return;
			}
			if (CollectionUnlockPopup._instance == null)
			{
				GameObject gameObject = new GameObject("Collection Unlock HUD", new Type[] { typeof(RectTransform) });
				Object.DontDestroyOnLoad(gameObject);
				CollectionUnlockPopup._instance = gameObject.AddComponent<CollectionUnlockPopup>();
			}
			CollectionUnlockPopup._instance._pending.Enqueue(new CollectionUnlockPopup.Discovery
			{
				Item = item,
				Variant = variant
			});
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x00336C90 File Offset: 0x00334E90
		private static bool IsRunContext()
		{
			return CollectionUnlockPopup.AllowsContext(RunModeService.CurrentMode, MissionService.IsRunActive, SceneManager.GetActiveScene().name);
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x00336CBC File Offset: 0x00334EBC
		internal static bool AllowsContext(RunModeKind mode, bool activeRun, string sceneName)
		{
			return mode == RunModeKind.Classic && activeRun && !string.IsNullOrEmpty(sceneName) && (sceneName == "Game - Current" || sceneName == "Game - Old" || sceneName == "City" || sceneName.StartsWith("POI - ", StringComparison.Ordinal) || sceneName == "School - Gymnasium");
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x00336D20 File Offset: 0x00334F20
		private void Awake()
		{
			Canvas canvas = base.gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 120;
			CanvasScaler canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			this._card = CollectionUnlockPopup.Rect("Collection Unlocked", base.transform);
			RectTransform card = this._card;
			RectTransform card2 = this._card;
			RectTransform card3 = this._card;
			Vector2 vector = new Vector2(0.5f, 1f);
			card3.pivot = vector;
			card.anchorMin = (card2.anchorMax = vector);
			this._card.sizeDelta = new Vector2(380f, 88f);
			this._card.anchoredPosition = new Vector2(0f, -100f);
			Image image = this._card.gameObject.AddComponent<Image>();
			image.color = new Color(0.18f, 0.035f, 0.25f, 0.96f);
			image.raycastTarget = false;
			this._group = this._card.gameObject.AddComponent<CanvasGroup>();
			this._group.interactable = (this._group.blocksRaycasts = false);
			this._group.alpha = 0f;
			RectTransform rectTransform = CollectionUnlockPopup.Rect("Accent", this._card);
			CollectionUnlockPopup.Fit(rectTransform, Vector2.zero, new Vector2(0.012f, 1f), Vector2.zero, Vector2.zero);
			Image image2 = rectTransform.gameObject.AddComponent<Image>();
			image2.color = new Color(0.96f, 0.18f, 0.43f);
			image2.raycastTarget = false;
			this._icon = CollectionUnlockPopup.Rect("Icon", this._card).gameObject.AddComponent<Image>();
			CollectionUnlockPopup.Fit(this._icon.rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(16f, 12f), new Vector2(80f, -12f));
			this._icon.preserveAspect = true;
			this._icon.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = this.Text("Unlocked", 24f, new Color(1f, 0.78f, 0.87f));
			CollectionUnlockPopup.Fit(textMeshProUGUI.rectTransform, new Vector2(0f, 0.5f), Vector2.one, new Vector2(96f, 0f), new Vector2(-16f, -8f));
			LocalizedGameText.Bind(textMeshProUGUI, "collection.unlocked", "Unlocked!", null);
			this._name = this.Text("Item Name", 19f, Color.white);
			CollectionUnlockPopup.Fit(this._name.rectTransform, Vector2.zero, new Vector2(1f, 0.5f), new Vector2(96f, 10f), new Vector2(-16f, 0f));
			this._name.richText = false;
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0f;
			this._audio.ignoreListenerPause = true;
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Sfx);
			this._chime = CollectionUnlockPopup.BuildChime();
			this._card.gameObject.SetActive(false);
			GameplayHudVisibility.RegisterCanvas(canvas);
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x00337088 File Offset: 0x00335288
		private void Update()
		{
			if ((LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning))
			{
				this._card.gameObject.SetActive(false);
				return;
			}
			if (!CollectionUnlockPopup.IsRunContext() || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver))
			{
				this.Clear();
				return;
			}
			bool flag = !GameSettingsService.ShowGameplayUI || GraphicsSettingsRuntimeUI.AnyOpen || (HUDManager.Instance != null && HUDManager.Instance.IsHidden) || SmartphoneController.IsAnySelfieActive || Time.timeScale <= 0f;
			this._card.gameObject.SetActive(!flag && (this._presenting || this._pending.Count > 0));
			if (flag)
			{
				return;
			}
			if (!this._presenting)
			{
				if (this._pending.Count == 0)
				{
					return;
				}
				CollectionUnlockPopup.Discovery discovery = this._pending.Dequeue();
				if (discovery.Item == null)
				{
					return;
				}
				this.Show(discovery);
			}
			this._elapsed += Time.unscaledDeltaTime;
			float num = Mathf.Clamp01(this._elapsed / 0.22f);
			float num2 = Mathf.Clamp01((this._elapsed - 2.6f) / 0.28f);
			this._group.alpha = Mathf.Min(num, 1f - num2);
			this._card.localScale = Vector3.one * Mathf.Lerp(0.86f, 1f, 1f - Mathf.Pow(1f - num, 3f));
			this._card.anchoredPosition = new Vector2(0f, -100f + 16f * num2);
			if (num2 >= 1f)
			{
				this._presenting = false;
				this._card.gameObject.SetActive(false);
			}
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x00337280 File Offset: 0x00335480
		private void Show(CollectionUnlockPopup.Discovery discovery)
		{
			this._elapsed = 0f;
			this._presenting = true;
			InventoryItemSO item = discovery.Item;
			this._icon.sprite = ItemVariantLook.ResolveIcon(item, discovery.Variant);
			this._icon.enabled = this._icon.sprite != null;
			StreamerCensorImage.BindItem(this._icon, item);
			LocalizedGameText.BindComputed(this._name, () => CollectionUnlockPopup.DisplayName(item, discovery.Variant)).FitTranslationToRectangle(false);
			this._audio.volume = 0.45f;
			this._audio.PlayOneShot(this._chime);
		}

		// Token: 0x06007D90 RID: 32144 RVA: 0x0033734C File Offset: 0x0033554C
		internal static string DisplayName(InventoryItemSO item, int variantIndex)
		{
			if (item == null)
			{
				return string.Empty;
			}
			string text = (string.IsNullOrWhiteSpace(item.itemName) ? item.name : item.itemName);
			string source = GameLocalization.GetSource("items", text);
			MCSurfaceVariantSO mcsurfaceVariantSO = ItemVariantLook.ResolveVariant(item, variantIndex);
			string text2 = ((mcsurfaceVariantSO != null) ? (string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName) : null);
			if (string.IsNullOrWhiteSpace(text2) && item.rarityVariants != null)
			{
				foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
				{
					if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex == variantIndex)
					{
						text2 = rarityVariantEntry.surfaceName;
						break;
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return source + " — " + GameLocalization.GetSource("item_variant", text2);
			}
			return source;
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x0033744C File Offset: 0x0033564C
		private void Clear()
		{
			this._pending.Clear();
			this._presenting = false;
			if (this._card != null)
			{
				this._card.gameObject.SetActive(false);
			}
			if (this._audio != null && this._audio.isPlaying)
			{
				this._audio.Stop();
			}
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x003374B0 File Offset: 0x003356B0
		private void OnDisable()
		{
			this.Clear();
		}

		// Token: 0x06007D93 RID: 32147 RVA: 0x003374B8 File Offset: 0x003356B8
		private void OnDestroy()
		{
			if (CollectionUnlockPopup._instance == this)
			{
				CollectionUnlockPopup._instance = null;
			}
			if (this._chime != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._chime);
					return;
				}
				Object.DestroyImmediate(this._chime);
			}
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x00337504 File Offset: 0x00335704
		private static AudioClip BuildChime()
		{
			float[] array = new float[28664];
			float[] array2 = new float[] { 659.25f, 830.61f, 987.77f };
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					float num = (float)i / 44100f - (float)j * 0.09f;
					if (num >= 0f)
					{
						float num2 = Mathf.Clamp01(num / 0.009f) * Mathf.Exp(-num * 9f) * Mathf.Clamp01((0.65f - (float)i / 44100f) / 0.06f);
						float num3 = 6.2831855f * array2[j] * num;
						array[i] += (Mathf.Sin(num3) + 0.2f * Mathf.Sin(num3 * 2f)) * num2 * 0.23f;
					}
				}
			}
			AudioClip audioClip = AudioClip.Create("Collection unlock chime", array.Length, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x00337604 File Offset: 0x00335804
		private TextMeshProUGUI Text(string name, float size, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = CollectionUnlockPopup.Rect(name, this._card).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF") ?? TMP_Settings.defaultFontAsset;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			return textMeshProUGUI;
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x0013F605 File Offset: 0x0013D805
		private static RectTransform Rect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void Fit(RectTransform rect, Vector2 min, Vector2 max, Vector2 insetMin, Vector2 insetMax)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = insetMin;
			rect.offsetMax = insetMax;
		}

		// Token: 0x0400816A RID: 33130
		private static CollectionUnlockPopup _instance;

		// Token: 0x0400816B RID: 33131
		private readonly Queue<CollectionUnlockPopup.Discovery> _pending = new Queue<CollectionUnlockPopup.Discovery>();

		// Token: 0x0400816C RID: 33132
		private RectTransform _card;

		// Token: 0x0400816D RID: 33133
		private CanvasGroup _group;

		// Token: 0x0400816E RID: 33134
		private Image _icon;

		// Token: 0x0400816F RID: 33135
		private TextMeshProUGUI _name;

		// Token: 0x04008170 RID: 33136
		private AudioSource _audio;

		// Token: 0x04008171 RID: 33137
		private AudioClip _chime;

		// Token: 0x04008172 RID: 33138
		private bool _presenting;

		// Token: 0x04008173 RID: 33139
		private float _elapsed;

		// Token: 0x02001034 RID: 4148
		private struct Discovery
		{
			// Token: 0x04008174 RID: 33140
			public InventoryItemSO Item;

			// Token: 0x04008175 RID: 33141
			public int Variant;
		}
	}
}
