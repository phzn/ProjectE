using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BoZo.CharacterCreator.MC.StyleSystem.UI;
using DA_Assets.CR;
using Features.Localization;
using Features.MainMenu;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000878 RID: 2168
	public class ColorManager : MonoBehaviour
	{
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06003A3E RID: 14910 RVA: 0x00156D04 File Offset: 0x00154F04
		// (remove) Token: 0x06003A3F RID: 14911 RVA: 0x00156D3C File Offset: 0x00154F3C
		public event ColorManager.ColorChangedHandler ColorChanged;

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x00156D71 File Offset: 0x00154F71
		public ColorPreviewUI PreviewPrefab
		{
			get
			{
				return this.colorPreviewPrefab;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003A41 RID: 14913 RVA: 0x00156D79 File Offset: 0x00154F79
		public ColorPickerControlExtended Picker
		{
			get
			{
				return this.colorPicker;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x00156D81 File Offset: 0x00154F81
		public GameObject PickerPanel
		{
			get
			{
				return this.colorPickerPanel;
			}
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00156D89 File Offset: 0x00154F89
		public void ConfigureRuntime(ColorPreviewUI previewPrefab, Transform previewContainer, ColorPickerControlExtended picker, GameObject pickerPanel, bool onlyBodyParts, OutfitSystem runtimeOutfitSystem = null)
		{
			this.colorPreviewPrefab = previewPrefab;
			this.colorPreviewContainer = previewContainer;
			this.colorPicker = picker;
			this.colorPickerPanel = pickerPanel;
			this.bodyPartsOnly = onlyBodyParts;
			this.targetOutfitSystem = runtimeOutfitSystem;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x00156DB8 File Offset: 0x00154FB8
		public List<ColorManager.MaterialColorMapping> GetMaterialMappings()
		{
			return this.materialMappings;
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00156DC0 File Offset: 0x00154FC0
		public static void ClearTemporaryItemSelections()
		{
			ColorManager.temporaryItemColors.Clear();
			ColorManager.temporaryItemColorOwners.Clear();
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00156DD8 File Offset: 0x00154FD8
		private void Awake()
		{
			this.stateManager = Object.FindObjectOfType<CharacterStateManager>();
			if (this.stateManager == null)
			{
				Debug.LogWarning("CharacterStateManager non trovato nella scena.");
			}
			if (this.colorPicker == null)
			{
				this.colorPicker = Object.FindObjectOfType<ColorPickerControlExtended>();
				if (this.colorPicker == null)
				{
					Debug.LogWarning("ColorPickerControlExtended non trovato nella scena.");
				}
			}
			if (this.colorPickerPanel != null)
			{
				this.colorPickerPanel.SetActive(false);
			}
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00156E53 File Offset: 0x00155053
		private void Start()
		{
			if (this.colorPicker != null)
			{
				ColorPickerControlExtended colorPickerControlExtended = this.colorPicker;
				colorPickerControlExtended.OnColorPickerClosed = (Action)Delegate.Combine(colorPickerControlExtended.OnColorPickerClosed, new Action(this.OnColorPickerClosed));
			}
			this.BuildColorUI();
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x00156E90 File Offset: 0x00155090
		private void OnEnable()
		{
			if (this._started)
			{
				this.BuildColorUI();
			}
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00156EA0 File Offset: 0x001550A0
		private void OnDisable()
		{
			this.ObserveOutfits(null);
			if (this._outfitRefreshRoutine != null)
			{
				base.StopCoroutine(this._outfitRefreshRoutine);
				this._outfitRefreshRoutine = null;
			}
			if (this.colorPicker != null && this.colorPicker.IsOpen)
			{
				this.colorPicker.Close();
			}
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00156EF5 File Offset: 0x001550F5
		private void OnDestroy()
		{
			this.ObserveOutfits(null);
			if (this.colorPicker != null)
			{
				ColorPickerControlExtended colorPickerControlExtended = this.colorPicker;
				colorPickerControlExtended.OnColorPickerClosed = (Action)Delegate.Remove(colorPickerControlExtended.OnColorPickerClosed, new Action(this.OnColorPickerClosed));
			}
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00156F34 File Offset: 0x00155134
		private void ObserveOutfits(OutfitSystem target)
		{
			if (this._observedOutfitSystem == target)
			{
				return;
			}
			if (this._observedOutfitSystem != null)
			{
				OutfitSystem observedOutfitSystem = this._observedOutfitSystem;
				observedOutfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(observedOutfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnPreviewOutfitsChanged));
			}
			this._observedOutfitSystem = target;
			if (this._observedOutfitSystem != null)
			{
				OutfitSystem observedOutfitSystem2 = this._observedOutfitSystem;
				observedOutfitSystem2.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(observedOutfitSystem2.OnOutfitChanged, new UnityAction<Outfit>(this.OnPreviewOutfitsChanged));
			}
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00156FC4 File Offset: 0x001551C4
		private void OnPreviewOutfitsChanged(Outfit changedOutfit)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.bodyPartsOnly && changedOutfit != null && !this.IsBodyPartOutfit(changedOutfit))
			{
				return;
			}
			if (this.colorPicker != null && this.colorPicker.IsOpen)
			{
				this.colorPicker.Close();
			}
			if (this._outfitRefreshRoutine != null)
			{
				base.StopCoroutine(this._outfitRefreshRoutine);
			}
			this._outfitRefreshRoutine = base.StartCoroutine(this.RefreshLoadedColorsNextFrame());
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x00157041 File Offset: 0x00155241
		private IEnumerator RefreshLoadedColorsNextFrame()
		{
			yield return null;
			this._outfitRefreshRoutine = null;
			if (base.isActiveAndEnabled)
			{
				this.BuildColorUI();
			}
			yield break;
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00157050 File Offset: 0x00155250
		private void BuildColorUI()
		{
			this._started = true;
			if (this.colorPicker != null && this.colorPicker.IsOpen)
			{
				this.colorPicker.Close();
			}
			this.ClearColorPreviewContainer();
			this.BuildFromOutfits();
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x0015708C File Offset: 0x0015528C
		private void BuildFromOutfits()
		{
			if (this.colorPreviewPrefab == null || this.colorPreviewContainer == null)
			{
				Debug.LogWarning("[ColorManager] colorPreviewPrefab/Container non configurati!");
				return;
			}
			OutfitSystem outfitSystem = ((this.targetOutfitSystem != null) ? this.targetOutfitSystem : Object.FindObjectOfType<OutfitSystem>(false));
			this.ObserveOutfits(outfitSystem);
			if (outfitSystem == null)
			{
				Debug.LogWarning("[ColorManager] Nessun OutfitSystem attivo: niente colori NPC.");
				return;
			}
			List<Outfit> outfits = outfitSystem.GetOutfits();
			if (outfits == null)
			{
				return;
			}
			if (this.bodyPartsOnly)
			{
				this.BuildBodyStyleEditor(outfits);
				return;
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < outfits.Count; i++)
			{
				Outfit outfit = outfits[i];
				if (!(outfit == null) && outfit.ColorChannels != null && outfit.ColorChannels.Length != 0)
				{
					List<Color> colors = outfit.GetColors();
					string text = ColorManager.ResolveOutfitLabel(outfit);
					List<ColorPreviewUI> list = new List<ColorPreviewUI>();
					for (int j = 0; j < outfit.ColorChannels.Length; j++)
					{
						string text2 = outfit.ColorChannels[j];
						if (!string.IsNullOrWhiteSpace(text2))
						{
							int num = j + 1;
							string text3 = ColorManager.ResolveLinkedChannelKey(outfit, num);
							if (text3 != null)
							{
								if (hashSet.Contains(text3))
								{
									goto IL_0179;
								}
								hashSet.Add(text3);
							}
							Color color = ((colors != null && j < colors.Count) ? colors[j] : Color.white);
							color.a = 1f;
							string text4 = text + " · " + text2;
							ColorPreviewUI colorPreviewUI = this.CreateOutfitColorCard(outfit, num, text4, color, null, false);
							if (colorPreviewUI != null)
							{
								list.Add(colorPreviewUI);
							}
						}
						IL_0179:;
					}
				}
			}
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00157238 File Offset: 0x00155438
		private bool IsBodyPartOutfit(Outfit outfit)
		{
			if (outfit == null)
			{
				return false;
			}
			string text = ((outfit.Type != null) ? outfit.Type.TypeName : outfit.name);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string text2 = (string.IsNullOrWhiteSpace(outfit.OutfitName) ? outfit.name : outfit.OutfitName);
			string text3 = text + " " + text2;
			string[] array = new string[]
			{
				"ear", "eye", "brow", "mouth", "teeth", "tongue", "nose", "hair", "headacc", "accessory",
				"hat", "mask", "glasses", "lash"
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (text3.IndexOf(array[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return false;
				}
			}
			for (int j = 0; j < this.bodyPartTypeKeywords.Count; j++)
			{
				string text4 = this.bodyPartTypeKeywords[j];
				if (!string.IsNullOrWhiteSpace(text4) && text3.IndexOf(text4, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x00157388 File Offset: 0x00155588
		private void BuildBodyStyleEditor(List<Outfit> outfits)
		{
			bool flag = false;
			for (int i = 0; i < outfits.Count; i++)
			{
				Outfit outfit = outfits[i];
				if (!(outfit == null) && outfit.ColorChannels != null && outfit.ColorChannels.Length != 0 && this.IsBodyPartOutfit(outfit))
				{
					flag = true;
					List<ColorPreviewUI> list = new List<ColorPreviewUI>();
					List<ColorManager.PresetOption> presets = ColorManager.BuildPresetOptions(outfit, null);
					this.CreateBodySectionHeading("SKIN PRESETS");
					ColorManager.BodyPresetDeck deck = this.CreateBodyPresetDeck(outfit, presets, list);
					this.CreateBodySectionHeading("FINE-TUNE");
					List<Color> colors = outfit.GetColors();
					Action <>9__0;
					for (int j = 0; j < outfit.ColorChannels.Length; j++)
					{
						string text = outfit.ColorChannels[j];
						if (!string.IsNullOrWhiteSpace(text))
						{
							int num = j + 1;
							Color color = ((colors != null && j < colors.Count) ? colors[j] : Color.white);
							color.a = 1f;
							Outfit outfit2 = outfit;
							int num2 = num;
							string text2 = ColorManager.FormatBodyChannelLabel(text);
							Color color2 = color;
							Action action;
							if ((action = <>9__0) == null)
							{
								action = (<>9__0 = delegate
								{
									deck.SetSelectedIndex(ColorManager.FindMatchingPresetIndex(outfit, presets));
								});
							}
							ColorPreviewUI colorPreviewUI = this.CreateOutfitColorCard(outfit2, num2, text2, color2, action, true);
							if (colorPreviewUI != null)
							{
								list.Add(colorPreviewUI);
							}
						}
					}
					deck.SetSelectedIndex(ColorManager.FindMatchingPresetIndex(outfit, presets));
				}
			}
			if (!flag)
			{
				this.CreateBodySectionHeading("NO BODY COLORS");
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00157544 File Offset: 0x00155744
		private ColorManager.BodyPresetDeck CreateBodyPresetDeck(Outfit outfit, List<ColorManager.PresetOption> presets, List<ColorPreviewUI> cards)
		{
			ColorManager.BodyPresetDeck deck = new ColorManager.BodyPresetDeck
			{
				outfit = outfit,
				presets = presets
			};
			GameObject gameObject = new GameObject("Skin Preset Selector", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(LayoutElement)
			});
			gameObject.layer = this.colorPreviewContainer.gameObject.layer;
			gameObject.transform.SetParent(this.colorPreviewContainer, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, 78f);
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color32(43, 27, 56, 235);
			component2.maskable = true;
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(16f, 16f, 16f, 16f));
			LayoutElement component3 = gameObject.GetComponent<LayoutElement>();
			component3.minHeight = 78f;
			component3.preferredHeight = 78f;
			deck.label = this.CreateAnchoredText(gameObject.transform, "Preset Label", new Vector2(62f, 0f), Vector2.one);
			deck.label.rectTransform.offsetMax = new Vector2(-62f, 0f);
			deck.label.fontSize = 16f;
			deck.label.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
			deck.label.alignment = TextAlignmentOptions.Center;
			deck.label.color = Color.white;
			deck.currentIndex = ColorManager.FindMatchingPresetIndex(outfit, presets);
			Action<int> applyStep = delegate(int step)
			{
				if (outfit == null || presets == null || presets.Count == 0)
				{
					return;
				}
				int num = ((deck.currentIndex < 0) ? ((step > 0) ? 0 : (presets.Count - 1)) : ((deck.currentIndex + step + presets.Count) % presets.Count));
				this.EnsureColorPickerReference();
				if (this.colorPicker != null && this.colorPicker.IsOpen)
				{
					this.colorPicker.Close();
				}
				ColorManager.PresetOption presetOption = presets[num];
				int num2 = Mathf.Min(outfit.ColorChannels.Length, presetOption.colors.Count);
				for (int i = 0; i < num2; i++)
				{
					Color color = presetOption.colors[i];
					color.a = 1f;
					outfit.SetColor(color, i + 1, false);
				}
				ColorManager.RefreshOutfitCardsFromSource(outfit, cards);
				deck.SetSelectedIndex(num);
			};
			ColorManager.CreateVariantArrow(gameObject.transform, "‹", true, delegate
			{
				applyStep(-1);
			}, 48f, 25f);
			ColorManager.CreateVariantArrow(gameObject.transform, "›", false, delegate
			{
				applyStep(1);
			}, 48f, 25f);
			deck.SetSelectedIndex(deck.currentIndex);
			return deck;
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x001577C8 File Offset: 0x001559C8
		private void CreateBodySectionHeading(string title)
		{
			GameObject gameObject = new GameObject("Section - " + title, new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.layer = this.colorPreviewContainer.gameObject.layer;
			gameObject.transform.SetParent(this.colorPreviewContainer, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, 40f);
			LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
			component2.minHeight = 40f;
			component2.preferredHeight = 40f;
			TextMeshProUGUI textMeshProUGUI = this.CreateAnchoredText(gameObject.transform, "Title", Vector2.zero, Vector2.one);
			textMeshProUGUI.fontSize = 20f;
			textMeshProUGUI.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
			textMeshProUGUI.color = Color.white;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "cielos_apps", title, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x001578BC File Offset: 0x00155ABC
		private TextMeshProUGUI CreateAnchoredText(Transform parent, string objectName, Vector2 offsetMin, Vector2 anchorMax)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = anchorMax;
			component.offsetMin = offsetMin;
			component.offsetMax = Vector2.zero;
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.font = this.ResolveUiFont();
			component2.alignment = TextAlignmentOptions.MidlineLeft;
			component2.enableWordWrapping = false;
			component2.overflowMode = TextOverflowModes.Ellipsis;
			component2.raycastTarget = false;
			return component2;
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00157978 File Offset: 0x00155B78
		private Image CreateBodyPresetSwatch(Transform parent, Color color)
		{
			GameObject gameObject = new GameObject("Swatch", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0.5f);
			component.anchorMax = new Vector2(0f, 0.5f);
			component.pivot = new Vector2(0f, 0.5f);
			component.anchoredPosition = new Vector2(10f, 0f);
			component.sizeDelta = new Vector2(30f, 30f);
			Image component2 = gameObject.GetComponent<Image>();
			color.a = 1f;
			component2.color = color;
			component2.maskable = true;
			component2.raycastTarget = false;
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(15f, 15f, 15f, 15f));
			return component2;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00157A94 File Offset: 0x00155C94
		private TextMeshProUGUI CreateBodyPresetLabel(Transform parent, string value)
		{
			GameObject gameObject = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = new Vector2(47f, 0f);
			component.offsetMax = new Vector2(-8f, 0f);
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.font = this.ResolveUiFont();
			component2.fontSize = 10.5f;
			component2.fontStyle = FontStyles.Bold;
			component2.alignment = TextAlignmentOptions.MidlineLeft;
			component2.enableAutoSizing = true;
			component2.fontSizeMin = 8f;
			component2.fontSizeMax = 10.5f;
			component2.enableWordWrapping = false;
			component2.raycastTarget = false;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(component2, "cielos_apps", value, (string text) => text.ToUpperInvariant());
			if (localizedGameText == null)
			{
				return component2;
			}
			localizedGameText.FitTranslationToRectangle(false);
			return component2;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00157BCC File Offset: 0x00155DCC
		private GameObject CreateBodyPresetCheckmark(Transform parent)
		{
			GameObject gameObject = new GameObject("Selected", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(1f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(1f, 1f);
			component.anchoredPosition = new Vector2(-5f, -3f);
			component.sizeDelta = new Vector2(18f, 18f);
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.font = this.ResolveUiFont();
			component2.text = "✓";
			component2.fontSize = 13f;
			component2.fontStyle = FontStyles.Bold;
			component2.alignment = TextAlignmentOptions.Center;
			component2.color = Color.white;
			component2.raycastTarget = false;
			gameObject.SetActive(false);
			return gameObject;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x00157CF0 File Offset: 0x00155EF0
		private TMP_FontAsset ResolveUiFont()
		{
			if (this.colorPreviewPrefab == null)
			{
				return null;
			}
			TextMeshProUGUI componentInChildren = this.colorPreviewPrefab.GetComponentInChildren<TextMeshProUGUI>(true);
			if (!(componentInChildren != null))
			{
				return null;
			}
			return LocalizedGameText.SourceFont(componentInChildren);
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00157D2C File Offset: 0x00155F2C
		private static string FormatBodyChannelLabel(string channelName)
		{
			string text = channelName.Replace("_", " ").Trim();
			if (text.Equals("Base", StringComparison.OrdinalIgnoreCase))
			{
				return "BASE SKIN";
			}
			if (text.Equals("Lines", StringComparison.OrdinalIgnoreCase))
			{
				return "BODY LINES";
			}
			return text.ToUpperInvariant();
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00157D80 File Offset: 0x00155F80
		private static void RefreshOutfitCardsFromSource(Outfit outfit, List<ColorPreviewUI> cards)
		{
			if (outfit == null || cards == null)
			{
				return;
			}
			List<Color> colors = outfit.GetColors();
			if (colors == null)
			{
				return;
			}
			for (int i = 0; i < cards.Count; i++)
			{
				ColorPreviewUI colorPreviewUI = cards[i];
				if (!(colorPreviewUI == null) && colorPreviewUI.ChannelIndex >= 1 && colorPreviewUI.ChannelIndex - 1 < colors.Count)
				{
					Color color = colors[colorPreviewUI.ChannelIndex - 1];
					color.a = 1f;
					colorPreviewUI.PreviewColor = color;
				}
			}
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00157E04 File Offset: 0x00156004
		private static void ConfigureStableColorButton(Button button)
		{
			if (button == null)
			{
				return;
			}
			MainMenuButtonFx mainMenuButtonFx = button.GetComponent<MainMenuButtonFx>();
			if (mainMenuButtonFx == null)
			{
				mainMenuButtonFx = button.gameObject.AddComponent<MainMenuButtonFx>();
			}
			mainMenuButtonFx.SetTintChildGraphics(false);
			mainMenuButtonFx.SetScaleEffectsEnabled(false);
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00157E48 File Offset: 0x00156048
		private void CreatePaletteRowForList(Outfit outfit, string outfitLabel, List<ColorPreviewUI> outfitCards)
		{
			GameObject gameObject = new GameObject("Palette Row - " + outfitLabel, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(HorizontalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject.layer = this.colorPreviewContainer.gameObject.layer;
			gameObject.transform.SetParent(this.colorPreviewContainer, false);
			this._paletteRows.Add(gameObject);
			gameObject.GetComponent<Image>().color = new Color32(45, 28, 58, 200);
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(12f, 12f, 12f, 12f));
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minHeight = 64f;
			component.preferredHeight = 64f;
			HorizontalLayoutGroup component2 = gameObject.GetComponent<HorizontalLayoutGroup>();
			component2.padding = new RectOffset(14, 14, 8, 8);
			component2.spacing = 10f;
			component2.childAlignment = TextAnchor.MiddleLeft;
			component2.childControlWidth = false;
			component2.childControlHeight = false;
			component2.childForceExpandWidth = false;
			component2.childForceExpandHeight = false;
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject2.layer = gameObject.layer;
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 44f);
			TextMeshProUGUI component3 = gameObject2.GetComponent<TextMeshProUGUI>();
			component3.fontSize = 15f;
			component3.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
			component3.alignment = TextAlignmentOptions.MidlineLeft;
			component3.color = Color.white;
			component3.raycastTarget = false;
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(component3, () => GameLocalization.FormatSource("cielos_apps", "{0} Presets", new object[] { GameLocalization.GetSource("cielos_apps", outfitLabel) }));
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			for (int i = 0; i < 6; i++)
			{
				List<Color> list = ColorManager.BuildPalette(outfit, i);
				if (list.Count != 0)
				{
					GameObject gameObject3 = new GameObject(string.Format("Palette {0}", i + 1), new Type[]
					{
						typeof(RectTransform),
						typeof(CanvasRenderer),
						typeof(Image),
						typeof(Button),
						typeof(Outline)
					});
					gameObject3.layer = gameObject.layer;
					gameObject3.transform.SetParent(gameObject.transform, false);
					gameObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(44f, 44f);
					Image component4 = gameObject3.GetComponent<Image>();
					component4.color = list[0];
					component4.maskable = true;
					gameObject3.AddComponent<CornerRounder>().SetRadii(new Vector4(22f, 22f, 22f, 22f));
					Outline component5 = gameObject3.GetComponent<Outline>();
					component5.effectColor = new Color(1f, 1f, 1f, 0.7f);
					component5.effectDistance = new Vector2(1.2f, -1.2f);
					Button component6 = gameObject3.GetComponent<Button>();
					component6.transition = Selectable.Transition.None;
					ColorManager.ConfigureStableColorButton(component6);
					List<Color> capturedPalette = list;
					Outfit capturedOutfit = outfit;
					List<ColorPreviewUI> capturedCards = outfitCards;
					component6.onClick.AddListener(delegate
					{
						if (capturedOutfit == null)
						{
							return;
						}
						int num = Mathf.Min(capturedOutfit.ColorChannels.Length, capturedPalette.Count);
						for (int j = 0; j < num; j++)
						{
							Color color = capturedPalette[j];
							color.a = 1f;
							capturedOutfit.SetColor(color, j + 1, false);
						}
						List<Color> colors = capturedOutfit.GetColors();
						for (int k = 0; k < capturedCards.Count; k++)
						{
							ColorPreviewUI colorPreviewUI = capturedCards[k];
							if (!(colorPreviewUI == null) && colorPreviewUI.ChannelIndex >= 1 && colors != null && colorPreviewUI.ChannelIndex - 1 < colors.Count)
							{
								Color color2 = colors[colorPreviewUI.ChannelIndex - 1];
								color2.a = 1f;
								colorPreviewUI.PreviewColor = color2;
							}
						}
					});
				}
			}
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x001581D0 File Offset: 0x001563D0
		private static string ResolveOutfitLabel(Outfit outfit)
		{
			if (!string.IsNullOrWhiteSpace(outfit.OutfitName))
			{
				return outfit.OutfitName;
			}
			if (outfit.Type != null)
			{
				return outfit.Type.TypeName;
			}
			return outfit.name.Replace("(Clone)", "").Trim();
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00158228 File Offset: 0x00156428
		private static string ResolveLinkedChannelKey(Outfit outfit, int channelIndex)
		{
			if (outfit.LinkedColorSets == null || outfit.Type == null)
			{
				return null;
			}
			for (int i = 0; i < outfit.LinkedColorSets.Length; i++)
			{
				LinkedColorSets linkedColorSets = outfit.LinkedColorSets[i];
				if (linkedColorSets != null && !(linkedColorSets.linkedType == null) && channelIndex <= linkedColorSets.linkedChannelRange)
				{
					string name = outfit.Type.name;
					string name2 = linkedColorSets.linkedType.name;
					return ((string.CompareOrdinal(name, name2) <= 0) ? (name + "|" + name2) : (name2 + "|" + name)) + "#" + channelIndex.ToString();
				}
			}
			return null;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x001582D4 File Offset: 0x001564D4
		private ColorPreviewUI CreateOutfitColorCard(Outfit outfit, int channelIndex, string label, Color initial, Action onColorEdited = null, bool bodyFineTune = false)
		{
			ColorPreviewUI colorPreviewUI = Object.Instantiate<ColorPreviewUI>(this.colorPreviewPrefab, this.colorPreviewContainer);
			colorPreviewUI.gameObject.SetActive(true);
			colorPreviewUI.ConfigureSimpleMode();
			if (bodyFineTune)
			{
				colorPreviewUI.ConfigureBodyFineTuneStyle();
			}
			colorPreviewUI.Initialize(initial, label);
			if (bodyFineTune)
			{
				colorPreviewUI.gameObject.name = "Body Color - " + label;
			}
			colorPreviewUI.ChannelIndex = channelIndex;
			Outfit capturedOutfit = outfit;
			int capturedIndex = channelIndex;
			ColorPreviewUI capturedPreview = colorPreviewUI;
			Action<Color> <>9__1;
			colorPreviewUI.OnPreviewClicked.AddListener(delegate
			{
				this.EnsureColorPickerReference();
				if (this.colorPicker == null || capturedOutfit == null)
				{
					return;
				}
				if (this.colorPickerPanel != null)
				{
					this.colorPickerPanel.SetActive(true);
				}
				Color previewColor = capturedPreview.PreviewColor;
				ColorPickerControlExtended colorPickerControlExtended = this.colorPicker;
				Color color = previewColor;
				string label2 = label;
				Action<Color> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(Color col)
					{
						if (capturedOutfit == null || capturedPreview == null || !capturedPreview.gameObject.activeInHierarchy)
						{
							return;
						}
						col.a = 1f;
						capturedOutfit.SetColor(col, capturedIndex, false);
						capturedPreview.PreviewColor = col;
						Action onColorEdited2 = onColorEdited;
						if (onColorEdited2 == null)
						{
							return;
						}
						onColorEdited2();
					});
				}
				colorPickerControlExtended.ShowColorPicker(color, label2, action);
			});
			this.colorPreviews.Add(colorPreviewUI);
			return colorPreviewUI;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00158398 File Offset: 0x00156598
		public void ShowContextualControls(OutfitSelector card, Outfit outfit, bool restoreTemporaryColors = true)
		{
			if (card == null || outfit == null || outfit.ColorChannels == null || outfit.ColorChannels.Length == 0)
			{
				return;
			}
			this.EnsureColorPickerReference();
			if (this.colorPicker != null && this.colorPicker.IsOpen)
			{
				this.colorPicker.Close();
			}
			StyleItemSO styleItem = card.GetStyleItem();
			if (restoreTemporaryColors)
			{
				ColorManager.RestoreTemporaryItemColors(styleItem, outfit);
			}
			else
			{
				ColorManager.RememberTemporaryItemColors(styleItem, outfit);
			}
			ColorManager.DestroyAllChildrenNamed(card.ContextualControlsRoot, "Context Colors");
			this.EnsureColorPickerReference();
			GameObject gameObject = new GameObject("Context Colors", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject.layer = card.gameObject.layer;
			gameObject.transform.SetParent(card.ContextualControlsRoot, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(1f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(1f, 1f);
			int num = outfit.ColorChannels.Length;
			float num2 = ((num > 7) ? 2f : 3f);
			float num3 = Mathf.Clamp((152f - (float)Mathf.Max(0, num - 1) * num2) / (float)Mathf.Max(1, num), 15f, 22f);
			component.anchoredPosition = new Vector2(-10f, -10f);
			component.sizeDelta = new Vector2((float)num * num3 + (float)Mathf.Max(0, num - 1) * num2, num3 + 4f);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			HorizontalLayoutGroup component2 = gameObject.GetComponent<HorizontalLayoutGroup>();
			component2.padding = new RectOffset(0, 0, 0, 0);
			component2.spacing = num2;
			component2.childControlWidth = false;
			component2.childControlHeight = false;
			component2.childForceExpandWidth = false;
			component2.childForceExpandHeight = false;
			component2.childAlignment = TextAnchor.MiddleRight;
			List<Color> colors = outfit.GetColors();
			List<Image> list = new List<Image>();
			for (int i = 0; i < num; i++)
			{
				int num4 = i + 1;
				string text = (string.IsNullOrWhiteSpace(outfit.ColorChannels[i]) ? string.Format("Color {0}", num4) : outfit.ColorChannels[i]);
				Color color = ((colors != null && i < colors.Count) ? colors[i] : Color.white);
				color.a = 1f;
				list.Add(this.CreateContextualColorButton(gameObject.transform, outfit, styleItem, num4, text, color, num3));
			}
			ColorManager.DestroyAllChildrenNamed(card.ContextualControlsRoot, "Context Palettes");
			OutfitColorVariantSetSO outfitColorVariantSetSO = ((styleItem != null) ? styleItem.colorVariants : null);
			bool flag = outfitColorVariantSetSO != null && outfitColorVariantSetSO.variants != null && outfitColorVariantSetSO.variants.Count > 0;
			List<ColorManager.PresetOption> list2 = ColorManager.BuildPresetOptions(outfit, outfitColorVariantSetSO);
			if (list2.Count > 0)
			{
				this.CreateContextualVariantSelector(card, outfit, styleItem, list2, list, flag ? "Custom" : "Default");
			}
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x001586B0 File Offset: 0x001568B0
		private static void DestroyAllChildrenNamed(Transform parent, string name)
		{
			for (int i = parent.childCount - 1; i >= 0; i--)
			{
				Transform child = parent.GetChild(i);
				if (!(child.name != name))
				{
					child.name = name + " (stale)";
					child.gameObject.SetActive(false);
					Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x00158710 File Offset: 0x00156910
		private static List<ColorManager.PresetOption> BuildPresetOptions(Outfit outfit, OutfitColorVariantSetSO variantSet)
		{
			List<ColorManager.PresetOption> list = new List<ColorManager.PresetOption>();
			if (variantSet != null && variantSet.variants != null && variantSet.variants.Count > 0)
			{
				for (int i = 0; i < variantSet.variants.Count; i++)
				{
					OutfitColorVariantSetSO.Variant variant = variantSet.variants[i];
					if (variant != null && variant.channelColors != null && variant.channelColors.Count != 0)
					{
						list.Add(new ColorManager.PresetOption
						{
							name = (string.IsNullOrWhiteSpace(variant.variantName) ? string.Format("Variant {0}", i + 1) : variant.variantName),
							colors = variant.channelColors
						});
					}
				}
			}
			ColorManager.PaletteSeed[] seeds = ColorManager.GetPaletteSeeds(outfit);
			int p;
			Predicate<ColorManager.PresetOption> <>9__0;
			int p2;
			for (p = 0; p < seeds.Length; p = p2 + 1)
			{
				List<Color> list2 = ColorManager.ExpandPalette(outfit, seeds[p], ColorManager.ResolvePaletteFamily(outfit));
				if (list2.Count != 0)
				{
					List<ColorManager.PresetOption> list3 = list;
					Predicate<ColorManager.PresetOption> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (ColorManager.PresetOption option) => option != null && string.Equals(option.name, seeds[p].name, StringComparison.OrdinalIgnoreCase));
					}
					if (!list3.Exists(predicate))
					{
						list.Add(new ColorManager.PresetOption
						{
							name = seeds[p].name,
							colors = list2
						});
					}
				}
				p2 = p;
			}
			return list;
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x00158884 File Offset: 0x00156A84
		private static bool IsSkinToneOutfit(Outfit outfit)
		{
			return outfit != null && outfit.Type != null && (outfit.Type.TypeName.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0 || outfit.Type.TypeName.IndexOf("head", StringComparison.OrdinalIgnoreCase) >= 0);
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x001588E4 File Offset: 0x00156AE4
		private static ColorManager.PaletteSeed Seed(string name, params string[] hexColors)
		{
			Color32[] array = new Color32[hexColors.Length];
			for (int i = 0; i < hexColors.Length; i++)
			{
				Color color;
				ColorUtility.TryParseHtmlString("#" + hexColors[i], out color);
				array[i] = color;
			}
			return new ColorManager.PaletteSeed(name, array);
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x00158934 File Offset: 0x00156B34
		private static ColorManager.PaletteSeed[] GetPaletteSeeds(Outfit outfit)
		{
			switch (ColorManager.ResolvePaletteFamily(outfit))
			{
			case ColorManager.PaletteFamily.Skin:
				return ColorManager.SkinPaletteSeeds;
			case ColorManager.PaletteFamily.Hair:
				return ColorManager.HairPaletteSeeds;
			case ColorManager.PaletteFamily.Eyes:
				return ColorManager.EyePaletteSeeds;
			case ColorManager.PaletteFamily.Fur:
				return ColorManager.FurPaletteSeeds;
			case ColorManager.PaletteFamily.Denim:
				return ColorManager.DenimPaletteSeeds;
			case ColorManager.PaletteFamily.Formal:
				return ColorManager.FormalPaletteSeeds;
			case ColorManager.PaletteFamily.Activewear:
				return ColorManager.ActivewearPaletteSeeds;
			case ColorManager.PaletteFamily.Footwear:
				return ColorManager.FootwearPaletteSeeds;
			case ColorManager.PaletteFamily.Accessory:
				return ColorManager.AccessoryPaletteSeeds;
			default:
				return ColorManager.FabricPaletteSeeds;
			}
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x001589B0 File Offset: 0x00156BB0
		private static ColorManager.PaletteFamily ResolvePaletteFamily(Outfit outfit)
		{
			if (outfit == null)
			{
				return ColorManager.PaletteFamily.Fabric;
			}
			string text = ((outfit.Type != null) ? outfit.Type.TypeName : string.Empty);
			string text2 = string.Concat(new string[] { text, " ", outfit.OutfitName, " ", outfit.name }).ToLowerInvariant();
			if (text.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0 || text.Equals("Head", StringComparison.OrdinalIgnoreCase))
			{
				return ColorManager.PaletteFamily.Skin;
			}
			if (text2.Contains("hair"))
			{
				return ColorManager.PaletteFamily.Hair;
			}
			if (text2.Contains("eye") && !text2.Contains("eyewear"))
			{
				return ColorManager.PaletteFamily.Eyes;
			}
			if (text2.Contains("ear") || text2.Contains("fur") || text2.Contains("fluff"))
			{
				return ColorManager.PaletteFamily.Fur;
			}
			if (text2.Contains("denim") || text2.Contains("jean"))
			{
				return ColorManager.PaletteFamily.Denim;
			}
			if (text.IndexOf("feet", StringComparison.OrdinalIgnoreCase) >= 0 || text2.Contains("shoe") || text2.Contains("boot") || text2.Contains("sandal") || text2.Contains("leather"))
			{
				return ColorManager.PaletteFamily.Footwear;
			}
			if (text2.Contains("lace") || text2.Contains("dress") || text2.Contains("formal") || text2.Contains("gown") || text2.Contains("suit"))
			{
				return ColorManager.PaletteFamily.Formal;
			}
			if (text2.Contains("swim") || text2.Contains("bikini") || text2.Contains("sport") || text2.Contains("rain") || text2.Contains("active"))
			{
				return ColorManager.PaletteFamily.Activewear;
			}
			if (text.IndexOf("hat", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("acc", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("face", StringComparison.OrdinalIgnoreCase) >= 0 || text2.Contains("glasses") || text2.Contains("mask") || text2.Contains("jewel"))
			{
				return ColorManager.PaletteFamily.Accessory;
			}
			return ColorManager.PaletteFamily.Fabric;
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00158BD4 File Offset: 0x00156DD4
		private static List<Color> ExpandPalette(Outfit outfit, ColorManager.PaletteSeed seed, ColorManager.PaletteFamily family)
		{
			List<Color> list = new List<Color>();
			if (outfit == null || outfit.ColorChannels == null || seed == null || seed.colors.Length == 0)
			{
				return list;
			}
			for (int i = 0; i < outfit.ColorChannels.Length; i++)
			{
				string text = (outfit.ColorChannels[i] ?? string.Empty).ToLowerInvariant();
				int num = i % seed.colors.Length;
				if (family == ColorManager.PaletteFamily.Skin)
				{
					Color color = seed.colors[0];
					if (text.Contains("undertone"))
					{
						list.Add(Color.Lerp(color, seed.colors[1], 0.55f));
					}
					else if (text.Contains("line") || text.Contains("lash") || text.Contains("brow"))
					{
						list.Add(seed.colors[3]);
					}
					else if (text.Contains("nail") || text.Contains("mouth"))
					{
						list.Add(Color.Lerp(color, seed.colors[1], 0.72f));
					}
					else if (text.Contains("sclera") || text.Contains("teeth"))
					{
						list.Add(new Color32(245, 241, 232, byte.MaxValue));
					}
					else
					{
						list.Add(color);
					}
				}
				else
				{
					if (family == ColorManager.PaletteFamily.Eyes)
					{
						if (text.Contains("sclera") || text.Contains("white"))
						{
							list.Add(new Color32(245, 244, 239, byte.MaxValue));
							goto IL_042F;
						}
						if (text.Contains("ring") || text.Contains("line") || text.Contains("lash"))
						{
							num = 1;
						}
						else if (text.Contains("pupil"))
						{
							num = 3;
						}
						else if (text.Contains("highlight") || text.Contains("shine"))
						{
							num = 2;
						}
						else if (text.Contains("iris") || text.Contains("base"))
						{
							num = 0;
						}
					}
					else if (family == ColorManager.PaletteFamily.Hair)
					{
						if (text.Contains("root") || text.Contains("line") || text.Contains("shadow"))
						{
							num = 3;
						}
						else if (text.Contains("highlight") || text.Contains("shine"))
						{
							num = 2;
						}
						else if (text.Contains("under") || text.Contains("tip") || text.Contains("second"))
						{
							num = 1;
						}
						else
						{
							num = ((i == 0) ? 0 : num);
						}
					}
					else if (family == ColorManager.PaletteFamily.Fur)
					{
						if (text.Contains("inner"))
						{
							num = 1;
						}
						else if (text.Contains("fluff") || text.Contains("tip"))
						{
							num = 2;
						}
						else if (text.Contains("line") || text.Contains("shadow"))
						{
							num = 3;
						}
						else
						{
							num = 0;
						}
					}
					else if (text.Contains("metal") || text.Contains("buckle") || text.Contains("zip") || text.Contains("button") || text.Contains("eyelet"))
					{
						num = 2;
					}
					else if (text.Contains("line") || text.Contains("shadow") || text.Contains("sole"))
					{
						num = 3;
					}
					else if (text.Contains("accent") || text.Contains("trim") || text.Contains("second") || text.Contains("lace"))
					{
						num = 1;
					}
					else if (text.Contains("base") || text.Contains("main") || text.Contains("fabric"))
					{
						num = 0;
					}
					list.Add(seed.colors[Mathf.Clamp(num, 0, seed.colors.Length - 1)]);
				}
				IL_042F:;
			}
			return list;
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00159024 File Offset: 0x00157224
		private GameObject ResolveMcStyleSelectorTemplate()
		{
			if (this._mcSelectorTemplate != null)
			{
				return this._mcSelectorTemplate;
			}
			AccessoryListPanelController accessoryListPanelController = Object.FindObjectOfType<AccessoryListPanelController>(true);
			GameObject gameObject = ((accessoryListPanelController != null) ? accessoryListPanelController.CardPrefabTemplate : null);
			Transform transform = ((gameObject != null) ? gameObject.transform.Find("StyleSelector") : null);
			if (transform == null)
			{
				Debug.LogWarning("[ColorManager] Template StyleSelector MC non trovato: uso il selettore fallback.");
				return null;
			}
			this._mcSelectorTemplate = transform.gameObject;
			return this._mcSelectorTemplate;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001590A4 File Offset: 0x001572A4
		private void CreateContextualVariantSelector(OutfitSelector card, Outfit outfit, StyleItemSO styleItem, List<ColorManager.PresetOption> options, List<Image> channelSwatches, string noMatchLabel)
		{
			Transform contextualControlsRoot = card.ContextualControlsRoot;
			ColorManager.DestroyAllChildrenNamed(contextualControlsRoot, "Context Variants");
			int[] currentIndex = new int[] { ColorManager.FindMatchingPresetIndex(outfit, options) };
			string text = ((currentIndex[0] >= 0) ? options[currentIndex[0]].name : noMatchLabel);
			TextMeshProUGUI label = null;
			Action<int> applyStep = delegate(int step)
			{
				if (outfit == null || options.Count == 0)
				{
					return;
				}
				int num = ((currentIndex[0] < 0) ? ((step > 0) ? 0 : (options.Count - 1)) : ((currentIndex[0] + step + options.Count) % options.Count));
				currentIndex[0] = num;
				ColorManager.PresetOption presetOption = options[num];
				if (label != null)
				{
					CustomizationPresentation.BindColorLabel(label, presetOption.name, false);
				}
				int num2 = Mathf.Min(outfit.ColorChannels.Length, presetOption.colors.Count);
				for (int i = 0; i < num2; i++)
				{
					Color color = presetOption.colors[i];
					color.a = 1f;
					outfit.SetColor(color, i + 1, false);
				}
				ColorManager.RememberTemporaryItemColors(styleItem, outfit);
				List<Color> colors = outfit.GetColors();
				for (int j = 0; j < channelSwatches.Count; j++)
				{
					if (!(channelSwatches[j] == null) && colors != null && j < colors.Count)
					{
						Color color2 = colors[j];
						color2.a = 1f;
						channelSwatches[j].color = color2;
					}
				}
			};
			GameObject gameObject = this.ResolveMcStyleSelectorTemplate();
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, contextualControlsRoot);
				gameObject2.name = "Context Variants";
				gameObject2.SetActive(true);
				Transform transform = gameObject2.transform.Find("Prev");
				Transform transform2 = gameObject2.transform.Find("Next");
				Transform transform3 = gameObject2.transform.Find("StyleLabel");
				label = ((transform3 != null) ? transform3.GetComponent<TextMeshProUGUI>() : null);
				Button button = ((transform != null) ? transform.GetComponent<Button>() : null);
				if (button != null)
				{
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(delegate
					{
						applyStep(-1);
					});
					button.interactable = true;
				}
				Button button2 = ((transform2 != null) ? transform2.GetComponent<Button>() : null);
				if (button2 != null)
				{
					button2.onClick.RemoveAllListeners();
					button2.onClick.AddListener(delegate
					{
						applyStep(1);
					});
					button2.interactable = true;
				}
				card.SetNameRaised(true);
			}
			else
			{
				GameObject gameObject3 = new GameObject("Context Variants", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(Image),
					typeof(LayoutElement)
				});
				gameObject3.layer = contextualControlsRoot.gameObject.layer;
				gameObject3.transform.SetParent(contextualControlsRoot, false);
				RectTransform component = gameObject3.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0.5f, 0f);
				component.anchorMax = new Vector2(0.5f, 0f);
				component.pivot = new Vector2(0.5f, 0f);
				component.anchoredPosition = new Vector2(0f, 38f);
				component.sizeDelta = new Vector2(158f, 22f);
				gameObject3.GetComponent<LayoutElement>().ignoreLayout = true;
				gameObject3.GetComponent<Image>().color = new Color32(35, 20, 46, 235);
				gameObject3.AddComponent<CornerRounder>().SetRadii(new Vector4(11f, 11f, 11f, 11f));
				label = ColorManager.CreateVariantLabel(gameObject3.transform);
				ColorManager.CreateVariantArrow(gameObject3.transform, "‹", true, delegate
				{
					applyStep(-1);
				}, 20f, 15f);
				ColorManager.CreateVariantArrow(gameObject3.transform, "›", false, delegate
				{
					applyStep(1);
				}, 20f, 15f);
			}
			if (label != null)
			{
				CustomizationPresentation.BindColorLabel(label, text, false);
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x0015940C File Offset: 0x0015760C
		private static TextMeshProUGUI CreateVariantLabel(Transform parent)
		{
			GameObject gameObject = new GameObject("Variant Label", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = new Vector2(20f, 0f);
			component.offsetMax = new Vector2(-20f, 0f);
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.fontSize = 11f;
			component2.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
			component2.alignment = TextAlignmentOptions.Center;
			component2.enableAutoSizing = true;
			component2.fontSizeMin = 8f;
			component2.fontSizeMax = 11f;
			component2.color = Color.white;
			component2.raycastTarget = false;
			return component2;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00159508 File Offset: 0x00157708
		private static void CreateVariantArrow(Transform parent, string glyph, bool left, Action onClick, float hitWidth = 20f, float glyphSize = 15f)
		{
			GameObject gameObject = new GameObject(left ? "Arrow Left" : "Arrow Right", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Button)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = (left ? new Vector2(0f, 0f) : new Vector2(1f, 0f));
			component.anchorMax = (left ? new Vector2(0f, 1f) : new Vector2(1f, 1f));
			component.pivot = new Vector2(left ? 0f : 1f, 0.5f);
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = new Vector2(hitWidth, 0f);
			gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
			GameObject gameObject2 = new GameObject("Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject2.layer = gameObject.layer;
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform component2 = gameObject2.GetComponent<RectTransform>();
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.offsetMin = Vector2.zero;
			component2.offsetMax = Vector2.zero;
			TextMeshProUGUI component3 = gameObject2.GetComponent<TextMeshProUGUI>();
			component3.text = glyph;
			component3.fontSize = glyphSize;
			component3.fontStyle = FontStyles.Bold;
			component3.alignment = TextAlignmentOptions.Center;
			component3.color = new Color32(242, 166, 200, byte.MaxValue);
			component3.raycastTarget = false;
			Button component4 = gameObject.GetComponent<Button>();
			component4.transition = Selectable.Transition.None;
			component4.onClick.AddListener(delegate
			{
				onClick();
			});
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x00159740 File Offset: 0x00157940
		private static int FindMatchingPresetIndex(Outfit outfit, List<ColorManager.PresetOption> options)
		{
			List<Color> colors = outfit.GetColors();
			if (colors == null)
			{
				return -1;
			}
			for (int i = 0; i < options.Count; i++)
			{
				ColorManager.PresetOption presetOption = options[i];
				if (presetOption != null && presetOption.colors != null && presetOption.colors.Count != 0)
				{
					int num = Mathf.Min(colors.Count, presetOption.colors.Count);
					if (num != 0)
					{
						bool flag = true;
						for (int j = 0; j < num; j++)
						{
							Color color = colors[j];
							Color color2 = presetOption.colors[j];
							if (Mathf.Abs(color.r - color2.r) > 0.007843138f || Mathf.Abs(color.g - color2.g) > 0.007843138f || Mathf.Abs(color.b - color2.b) > 0.007843138f)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00159840 File Offset: 0x00157A40
		private Image CreateContextualColorButton(Transform parent, Outfit outfit, StyleItemSO styleItem, int channelIndex, string channelName, Color initial, float size)
		{
			GameObject gameObject = new GameObject("Color - " + channelName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredWidth = size;
			component.preferredHeight = size;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(1f, 1f, 1f, 0.85f);
			component2.maskable = true;
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(size * 0.5f, size * 0.5f, size * 0.5f, size * 0.5f));
			GameObject gameObject2 = new GameObject("Fill", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject2.layer = gameObject.layer;
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.offsetMin = new Vector2(2f, 2f);
			component3.offsetMax = new Vector2(-2f, -2f);
			Image component4 = gameObject2.GetComponent<Image>();
			component4.color = initial;
			component4.maskable = true;
			component4.raycastTarget = false;
			CornerRounder cornerRounder = gameObject2.AddComponent<CornerRounder>();
			float num = (size - 4f) * 0.5f;
			cornerRounder.SetRadii(new Vector4(num, num, num, num));
			Button component5 = gameObject.GetComponent<Button>();
			component5.transition = Selectable.Transition.None;
			ColorManager.ConfigureStableColorButton(component5);
			Outfit capturedOutfit = outfit;
			StyleItemSO capturedStyleItem = styleItem;
			Image capturedSwatch = component4;
			int capturedIndex = channelIndex;
			string pickerLabel = ColorManager.ResolveOutfitLabel(outfit) + " · " + channelName;
			Action<Color> <>9__1;
			component5.onClick.AddListener(delegate
			{
				this.EnsureColorPickerReference();
				if (this.colorPicker == null || capturedOutfit == null)
				{
					return;
				}
				if (this.colorPicker.IsOpen)
				{
					this.colorPicker.Close();
				}
				if (this.colorPickerPanel != null)
				{
					this.colorPickerPanel.SetActive(true);
				}
				Color color3 = capturedSwatch.color;
				List<Color> colors = capturedOutfit.GetColors();
				if (colors != null && capturedIndex - 1 < colors.Count)
				{
					color3 = colors[capturedIndex - 1];
					color3.a = 1f;
					capturedSwatch.color = color3;
				}
				ColorPickerControlExtended colorPickerControlExtended = this.colorPicker;
				Color color2 = color3;
				string pickerLabel2 = pickerLabel;
				Action<Color> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(Color color)
					{
						if (capturedOutfit == null || capturedSwatch == null || !capturedSwatch.gameObject.activeInHierarchy)
						{
							return;
						}
						color.a = 1f;
						capturedOutfit.SetColor(color, capturedIndex, false);
						capturedSwatch.color = color;
						ColorManager.RememberTemporaryItemColors(capturedStyleItem, capturedOutfit);
					});
				}
				colorPickerControlExtended.ShowColorPicker(color2, pickerLabel2, action);
			});
			return component4;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00159A90 File Offset: 0x00157C90
		private static string BuildTemporaryItemKey(StyleItemSO styleItem, Outfit outfit)
		{
			if (styleItem != null)
			{
				string text = ((styleItem.prefab != null) ? styleItem.prefab.name : string.Empty);
				string text2 = ((styleItem.itemType != null) ? styleItem.itemType.TypeName : string.Empty);
				return string.Concat(new string[] { text2, "|", styleItem.name, "|", styleItem.itemName, "|", text });
			}
			if (outfit == null)
			{
				return string.Empty;
			}
			string text3 = ((outfit.Type != null) ? outfit.Type.TypeName : string.Empty);
			return string.Concat(new string[]
			{
				text3,
				"|",
				outfit.OutfitName,
				"|",
				outfit.gameObject.name
			});
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00159B90 File Offset: 0x00157D90
		private static void RememberTemporaryItemColors(StyleItemSO cardStyleItem, Outfit outfit)
		{
			string text = ColorManager.BuildTemporaryItemKey(cardStyleItem, outfit);
			List<Color> list = ((outfit != null) ? outfit.GetColors() : null);
			if (string.IsNullOrEmpty(text) || list == null)
			{
				return;
			}
			List<Color> list2 = new List<Color>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Color color = list[i];
				color.a = 1f;
				list2.Add(color);
			}
			ColorManager.temporaryItemColors[text] = list2;
			ColorManager.temporaryItemColorOwners[text] = outfit.GetInstanceID();
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x00159C1C File Offset: 0x00157E1C
		private static void RestoreTemporaryItemColors(StyleItemSO styleItem, Outfit outfit)
		{
			string text = ColorManager.BuildTemporaryItemKey(styleItem, outfit);
			List<Color> list;
			if (string.IsNullOrEmpty(text) || !ColorManager.temporaryItemColors.TryGetValue(text, out list) || list == null)
			{
				return;
			}
			int num;
			if (ColorManager.temporaryItemColorOwners.TryGetValue(text, out num) && num == outfit.GetInstanceID())
			{
				ColorManager.RememberTemporaryItemColors(styleItem, outfit);
				return;
			}
			int num2 = Mathf.Min((outfit.ColorChannels != null) ? outfit.ColorChannels.Length : 0, list.Count);
			for (int i = 0; i < num2; i++)
			{
				outfit.SetColor(list[i], i + 1, false);
			}
			ColorManager.RememberTemporaryItemColors(styleItem, outfit);
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00159CB8 File Offset: 0x00157EB8
		private static List<Color> BuildPalette(Outfit outfit, int presetIndex)
		{
			ColorManager.PaletteSeed[] paletteSeeds = ColorManager.GetPaletteSeeds(outfit);
			if (paletteSeeds == null || paletteSeeds.Length == 0)
			{
				return new List<Color>();
			}
			int num = Mathf.Clamp(presetIndex, 0, paletteSeeds.Length - 1);
			return ColorManager.ExpandPalette(outfit, paletteSeeds[num], ColorManager.ResolvePaletteFamily(outfit));
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00159CF8 File Offset: 0x00157EF8
		private void EnsureColorPickerReference()
		{
			if (this.colorPickerPanel != null)
			{
				ColorPickerControlExtended colorPickerControlExtended = this.colorPickerPanel.GetComponentInChildren<ColorPickerControlExtended>(true);
				if (colorPickerControlExtended == null)
				{
					colorPickerControlExtended = this.colorPickerPanel.GetComponentInParent<ColorPickerControlExtended>(true);
				}
				if (colorPickerControlExtended != null)
				{
					this.colorPicker = colorPickerControlExtended;
					return;
				}
			}
			if (this.colorPicker == null)
			{
				this.colorPicker = Object.FindObjectOfType<ColorPickerControlExtended>(true);
			}
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00159D64 File Offset: 0x00157F64
		private void ClearColorPreviewContainer()
		{
			foreach (ColorPreviewUI colorPreviewUI in this.colorPreviews)
			{
				if (!(colorPreviewUI == null))
				{
					colorPreviewUI.gameObject.SetActive(false);
					Object.Destroy(colorPreviewUI.gameObject);
				}
			}
			this.colorPreviews.Clear();
			foreach (ColorManager.LinkedColorGroup linkedColorGroup in this.linkedColorGroups)
			{
				if (linkedColorGroup != null && !(linkedColorGroup.previewUI == null))
				{
					linkedColorGroup.previewUI.gameObject.SetActive(false);
					Object.Destroy(linkedColorGroup.previewUI.gameObject);
				}
			}
			this.linkedColorGroups.Clear();
			this._paletteRows.Clear();
			if (this.colorPreviewContainer != null)
			{
				for (int i = this.colorPreviewContainer.childCount - 1; i >= 0; i--)
				{
					GameObject gameObject = this.colorPreviewContainer.GetChild(i).gameObject;
					gameObject.SetActive(false);
					Object.Destroy(gameObject);
				}
			}
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00159EA8 File Offset: 0x001580A8
		private void RebindToActivePreview()
		{
			this.meshRenderers.RemoveAll((SkinnedMeshRenderer r) => r == null);
			OutfitSystem outfitSystem = Object.FindObjectOfType<OutfitSystem>(false);
			if (outfitSystem == null)
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = outfitSystem.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				this.meshRenderers.Clear();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != null && !this.meshRenderers.Contains(componentsInChildren[i]))
					{
						this.meshRenderers.Add(componentsInChildren[i]);
					}
				}
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00159F44 File Offset: 0x00158144
		public void AnalyzeMaterials()
		{
			this.materialMappings.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.meshRenderers)
			{
				if (!(skinnedMeshRenderer == null))
				{
					foreach (Material material in skinnedMeshRenderer.sharedMaterials)
					{
						if (!(material == null))
						{
							ColorManager.MaterialColorMapping materialColorMapping = new ColorManager.MaterialColorMapping
							{
								material = material,
								materialName = material.name
							};
							for (int j = 1; j <= 9; j++)
							{
								string text = "_Color_" + j.ToString();
								if (material.HasProperty(text))
								{
									materialColorMapping.colorPropertyNames.Add(text);
								}
							}
							materialColorMapping.config = this.FindConfigForMaterial(material);
							this.materialMappings.Add(materialColorMapping);
						}
					}
				}
			}
			Debug.Log(string.Format("Analizzati {0} materiali con canali di colore.", this.materialMappings.Count));
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x0015A06C File Offset: 0x0015826C
		private ColorChannelConfigSO FindConfigForMaterial(Material material)
		{
			foreach (ColorChannelConfigSO colorChannelConfigSO in this.colorConfigs)
			{
				Debug.Log("Material name: " + material.name + ", Config name: " + colorChannelConfigSO.categoryName);
				if (material.name.Contains(colorChannelConfigSO.categoryName))
				{
					Debug.Log("Found config: " + colorChannelConfigSO.categoryName);
					return colorChannelConfigSO;
				}
			}
			return null;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x0015A108 File Offset: 0x00158308
		public void CreateColorPreviews()
		{
			foreach (ColorPreviewUI colorPreviewUI in this.colorPreviews)
			{
				if (colorPreviewUI != null)
				{
					Object.Destroy(colorPreviewUI.gameObject);
				}
			}
			this.colorPreviews.Clear();
			foreach (ColorManager.MaterialColorMapping materialColorMapping in this.materialMappings)
			{
				if (!(materialColorMapping.config == null))
				{
					foreach (string text in materialColorMapping.colorPropertyNames)
					{
						ColorChannelConfigSO.ColorChannelInfo channelByPropertyName = materialColorMapping.config.GetChannelByPropertyName(text);
						if (channelByPropertyName != null && channelByPropertyName.isEnabled)
						{
							Color color = materialColorMapping.material.GetColor(text);
							this.CreateColorPreview(materialColorMapping.material, text, channelByPropertyName.displayLabel, color);
							Debug.Log("Created preview for " + text);
						}
					}
				}
			}
			this.SortColorPreviews();
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x0015A25C File Offset: 0x0015845C
		private void CreateColorPreview(Material material, string propertyName, string label, Color color)
		{
			if (this.colorPreviewPrefab == null || this.colorPreviewContainer == null)
			{
				Debug.LogError("ColorManager: colorPreviewPrefab o colorPreviewContainer è null!");
				return;
			}
			ColorPreviewUI preview = Object.Instantiate<ColorPreviewUI>(this.colorPreviewPrefab, this.colorPreviewContainer);
			preview.gameObject.SetActive(true);
			preview.Initialize(material, propertyName, label, color);
			preview.OnColorSelected.AddListener(delegate(Material mat, string prop, Color col)
			{
				this.OnColorPreviewClicked(mat, prop, preview);
			});
			this.colorPreviews.Add(preview);
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00004F31 File Offset: 0x00003131
		private void SortColorPreviews()
		{
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x0015A304 File Offset: 0x00158504
		public void OnColorPreviewClicked(Material material, string propertyName, ColorPreviewUI preview)
		{
			Debug.Log("Cliccato su " + preview.BlendName + ", proprietà: " + propertyName);
			this.isEditingLinkedColor = false;
			this.currentSelectedPreview = preview;
			if (this.colorPicker != null && this.colorPickerPanel != null)
			{
				this.colorPickerPanel.SetActive(true);
				this.colorPicker.ShowColorPicker(material, propertyName, preview.BlendName);
				this.colorPicker.OnColorChanged = delegate(Material mat, string prop, Color color)
				{
					if (this.currentSelectedPreview != null && !this.isEditingLinkedColor)
					{
						this.currentSelectedPreview.PreviewColor = color;
					}
				};
				return;
			}
			Debug.LogWarning("ColorPickerControlExtended o colorPickerPanel non configurati.");
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x0015A398 File Offset: 0x00158598
		public void SetColor(Material material, string propertyName, Color color)
		{
			if (material != null && material.HasProperty(propertyName))
			{
				material.SetColor(propertyName, color);
				this.UpdateColorPreview(material, propertyName, color);
				this.stateManager != null;
				ColorManager.ColorChangedHandler colorChanged = this.ColorChanged;
				if (colorChanged == null)
				{
					return;
				}
				colorChanged(material, propertyName, color);
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x0015A3E8 File Offset: 0x001585E8
		public void NotifyColorChanged(Material material, string propertyName, Color color)
		{
			this.UpdateColorPreview(material, propertyName, color);
			ColorManager.ColorChangedHandler colorChanged = this.ColorChanged;
			if (colorChanged == null)
			{
				return;
			}
			colorChanged(material, propertyName, color);
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x0015A408 File Offset: 0x00158608
		private void UpdateColorPreview(Material material, string propertyName, Color color)
		{
			foreach (ColorPreviewUI colorPreviewUI in this.colorPreviews)
			{
				if (colorPreviewUI.BlendName.Contains(propertyName))
				{
					colorPreviewUI.PreviewColor = color;
					break;
				}
			}
			foreach (ColorManager.LinkedColorGroup linkedColorGroup in this.linkedColorGroups)
			{
				foreach (ColorManager.LinkedMaterialProperty linkedMaterialProperty in linkedColorGroup.linkedProperties)
				{
					if (linkedMaterialProperty.material == material && linkedMaterialProperty.propertyName == propertyName && linkedColorGroup.previewUI != null)
					{
						linkedColorGroup.previewUI.PreviewColor = color;
						break;
					}
				}
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x0015A520 File Offset: 0x00158720
		public void AddMeshRenderer(SkinnedMeshRenderer renderer)
		{
			if (renderer != null && !this.meshRenderers.Contains(renderer))
			{
				this.meshRenderers.Add(renderer);
				this.AnalyzeMaterials();
				this.CreateColorPreviews();
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x0015A551 File Offset: 0x00158751
		public void RemoveMeshRenderer(SkinnedMeshRenderer renderer)
		{
			if (renderer != null && this.meshRenderers.Contains(renderer))
			{
				this.meshRenderers.Remove(renderer);
				this.AnalyzeMaterials();
				this.CreateColorPreviews();
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x0015A584 File Offset: 0x00158784
		private void CreateLinkedColorGroups()
		{
			this.linkedColorGroups.Clear();
			foreach (ColorGroupConfigSO colorGroupConfigSO in this.colorGroups)
			{
				if (!(colorGroupConfigSO == null) && colorGroupConfigSO.linkedChannels.Count >= 2)
				{
					ColorManager.LinkedColorGroup linkedColorGroup = new ColorManager.LinkedColorGroup();
					linkedColorGroup.groupConfig = colorGroupConfigSO;
					bool flag = true;
					foreach (ColorGroupConfigSO.LinkedColorChannel linkedColorChannel in colorGroupConfigSO.linkedChannels)
					{
						if (linkedColorChannel.channelConfig == null || string.IsNullOrEmpty(linkedColorChannel.propertyName))
						{
							flag = false;
							break;
						}
						Material material = this.FindMaterialForChannel(linkedColorChannel.channelConfig, linkedColorChannel.propertyName);
						if (material == null)
						{
							flag = false;
							break;
						}
						linkedColorGroup.linkedProperties.Add(new ColorManager.LinkedMaterialProperty(material, linkedColorChannel.propertyName));
					}
					if (flag)
					{
						this.linkedColorGroups.Add(linkedColorGroup);
					}
				}
			}
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x0015A6B8 File Offset: 0x001588B8
		private Material FindMaterialForChannel(ColorChannelConfigSO config, string propertyName)
		{
			foreach (ColorManager.MaterialColorMapping materialColorMapping in this.materialMappings)
			{
				if (materialColorMapping.config == config)
				{
					using (List<string>.Enumerator enumerator2 = materialColorMapping.colorPropertyNames.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current == propertyName)
							{
								return materialColorMapping.material;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x0015A760 File Offset: 0x00158960
		private void CreateLinkedColorPreviews()
		{
			foreach (ColorManager.LinkedColorGroup linkedColorGroup in this.linkedColorGroups)
			{
				if (linkedColorGroup.previewUI != null)
				{
					Object.Destroy(linkedColorGroup.previewUI.gameObject);
					linkedColorGroup.previewUI = null;
				}
			}
			this.linkedColorGroups.Sort(delegate(ColorManager.LinkedColorGroup a, ColorManager.LinkedColorGroup b)
			{
				if (a.groupConfig == null && b.groupConfig == null)
				{
					return 0;
				}
				if (a.groupConfig == null)
				{
					return 1;
				}
				if (b.groupConfig == null)
				{
					return -1;
				}
				return a.groupConfig.sortOrder.CompareTo(b.groupConfig.sortOrder);
			});
			foreach (ColorManager.LinkedColorGroup linkedColorGroup2 in this.linkedColorGroups)
			{
				if (!(linkedColorGroup2.groupConfig == null) && linkedColorGroup2.linkedProperties.Count >= 2)
				{
					ColorPreviewUI colorPreviewUI = Object.Instantiate<ColorPreviewUI>(this.colorPreviewPrefab, this.colorPreviewContainer);
					if (colorPreviewUI == null)
					{
						Debug.LogError("[ColorManager] Errore durante l'istanziazione del prefab ColorPreviewUI");
					}
					else
					{
						List<ColorPreviewUI.MaterialPropertyPair> list = new List<ColorPreviewUI.MaterialPropertyPair>();
						foreach (ColorManager.LinkedMaterialProperty linkedMaterialProperty in linkedColorGroup2.linkedProperties)
						{
							list.Add(new ColorPreviewUI.MaterialPropertyPair(linkedMaterialProperty.material, linkedMaterialProperty.propertyName));
						}
						colorPreviewUI.InitializeLinked(linkedColorGroup2.groupConfig.displayLabel, linkedColorGroup2.groupConfig.defaultColor, list);
						colorPreviewUI.OnLinkedColorsSelected.AddListener(new UnityAction<List<ColorPreviewUI.MaterialPropertyPair>, Color>(this.OnLinkedColorPreviewClicked));
						linkedColorGroup2.previewUI = colorPreviewUI;
					}
				}
			}
			this.SortLinkedColorPreviews();
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0015A928 File Offset: 0x00158B28
		private void SortLinkedColorPreviews()
		{
			List<ColorPreviewUI> list = new List<ColorPreviewUI>();
			foreach (ColorManager.LinkedColorGroup linkedColorGroup in this.linkedColorGroups)
			{
				if (linkedColorGroup.previewUI != null)
				{
					list.Add(linkedColorGroup.previewUI);
				}
			}
			if (list.Count <= 1)
			{
				return;
			}
			List<ValueTuple<ColorPreviewUI, int>> list2 = new List<ValueTuple<ColorPreviewUI, int>>();
			foreach (ColorManager.LinkedColorGroup linkedColorGroup2 in this.linkedColorGroups)
			{
				if (linkedColorGroup2.previewUI != null && linkedColorGroup2.groupConfig != null)
				{
					list2.Add(new ValueTuple<ColorPreviewUI, int>(linkedColorGroup2.previewUI, linkedColorGroup2.groupConfig.sortOrder));
				}
			}
			list2.Sort(([TupleElementNames(new string[] { "preview", "sortOrder" })] ValueTuple<ColorPreviewUI, int> a, [TupleElementNames(new string[] { "preview", "sortOrder" })] ValueTuple<ColorPreviewUI, int> b) => a.Item2.CompareTo(b.Item2));
			int num = 0;
			for (int i = 0; i < list2.Count; i++)
			{
				int num2 = num + i;
				list2[i].Item1.transform.SetSiblingIndex(num2);
			}
			for (int j = 0; j < this.colorPreviews.Count; j++)
			{
				int num3 = list2.Count + j;
				this.colorPreviews[j].transform.SetSiblingIndex(num3);
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x0015AAB8 File Offset: 0x00158CB8
		public void OnLinkedColorPreviewClicked(List<ColorPreviewUI.MaterialPropertyPair> materialPropertyPairs, Color color)
		{
			this.isEditingLinkedColor = true;
			ColorPreviewUI colorPreviewUI = null;
			foreach (ColorManager.LinkedColorGroup linkedColorGroup in this.linkedColorGroups)
			{
				if (linkedColorGroup.previewUI != null && linkedColorGroup.previewUI.PreviewColor == color)
				{
					colorPreviewUI = linkedColorGroup.previewUI;
					break;
				}
			}
			if (colorPreviewUI == null)
			{
				Debug.LogError("[ColorManager] Nessuna preview trovata per il colore selezionato!");
				return;
			}
			this.currentSelectedLinkedPreview = colorPreviewUI;
			if (!(this.colorPicker != null) || !(this.colorPickerPanel != null))
			{
				Debug.LogError("[ColorManager] ColorPickerControlExtended o colorPickerPanel non configurati.");
				return;
			}
			this.colorPickerPanel.SetActive(true);
			if (materialPropertyPairs.Count > 0)
			{
				ColorPreviewUI.MaterialPropertyPair materialPropertyPair = materialPropertyPairs[0];
				this.colorPicker.ShowColorPicker(materialPropertyPair.material, materialPropertyPair.propertyName, colorPreviewUI.BlendName);
				this.colorPicker.OnColorChanged = delegate(Material mat, string prop, Color newColor)
				{
					if (this.isEditingLinkedColor)
					{
						if (this.currentSelectedLinkedPreview != null)
						{
							this.currentSelectedLinkedPreview.PreviewColor = newColor;
						}
						foreach (ColorPreviewUI.MaterialPropertyPair materialPropertyPair2 in materialPropertyPairs)
						{
							if (materialPropertyPair2.material != null && !string.IsNullOrEmpty(materialPropertyPair2.propertyName))
							{
								materialPropertyPair2.material.SetColor(materialPropertyPair2.propertyName, newColor);
							}
						}
					}
				};
				return;
			}
			Debug.LogError("[ColorManager] Nessuna proprietà collegata disponibile!");
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x0015ABF8 File Offset: 0x00158DF8
		private void OnColorPickerClosed()
		{
			this.isEditingLinkedColor = false;
			Debug.Log("[ColorManager] Color picker chiuso, reset del flag isEditingLinkedColor");
		}

		// Token: 0x04003A93 RID: 14995
		[Tooltip("SkinnedMeshRenderer da cui estrarre i materiali")]
		[SerializeField]
		private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x04003A94 RID: 14996
		[Tooltip("Configurazioni dei canali di colore")]
		[SerializeField]
		private List<ColorChannelConfigSO> colorConfigs = new List<ColorChannelConfigSO>();

		// Token: 0x04003A95 RID: 14997
		[Tooltip("Configurazioni dei gruppi di colori")]
		[SerializeField]
		private List<ColorGroupConfigSO> colorGroups = new List<ColorGroupConfigSO>();

		// Token: 0x04003A96 RID: 14998
		[Tooltip("Prefab per la preview del colore")]
		[SerializeField]
		private ColorPreviewUI colorPreviewPrefab;

		// Token: 0x04003A97 RID: 14999
		[Tooltip("Container per le preview dei colori")]
		[SerializeField]
		private Transform colorPreviewContainer;

		// Token: 0x04003A98 RID: 15000
		[Tooltip("Riferimento al ColorPickerControlExtended")]
		[SerializeField]
		private ColorPickerControlExtended colorPicker;

		// Token: 0x04003A99 RID: 15001
		[Tooltip("GameObject del pannello del color picker")]
		[SerializeField]
		private GameObject colorPickerPanel;

		// Token: 0x04003A9A RID: 15002
		[Header("Filtro (tab STYLE)")]
		[Tooltip("Se attivo, mostra solo i canali della pelle/corpo. Testa, occhi, capelli e accessori restano nelle rispettive categorie.")]
		[SerializeField]
		private bool bodyPartsOnly;

		// Token: 0x04003A9B RID: 15003
		[SerializeField]
		[HideInInspector]
		private OutfitSystem targetOutfitSystem;

		// Token: 0x04003A9C RID: 15004
		[Tooltip("Sottostringhe dei TypeName considerate corpo/pelle (match case-insensitive).")]
		[SerializeField]
		private List<string> bodyPartTypeKeywords = new List<string> { "body", "skin" };

		// Token: 0x04003A9D RID: 15005
		private List<ColorManager.MaterialColorMapping> materialMappings = new List<ColorManager.MaterialColorMapping>();

		// Token: 0x04003A9E RID: 15006
		private List<ColorPreviewUI> colorPreviews = new List<ColorPreviewUI>();

		// Token: 0x04003A9F RID: 15007
		private static readonly Dictionary<string, List<Color>> temporaryItemColors = new Dictionary<string, List<Color>>(StringComparer.Ordinal);

		// Token: 0x04003AA0 RID: 15008
		private static readonly Dictionary<string, int> temporaryItemColorOwners = new Dictionary<string, int>(StringComparer.Ordinal);

		// Token: 0x04003AA1 RID: 15009
		private List<ColorManager.LinkedColorGroup> linkedColorGroups = new List<ColorManager.LinkedColorGroup>();

		// Token: 0x04003AA2 RID: 15010
		private ColorPreviewUI currentSelectedLinkedPreview;

		// Token: 0x04003AA3 RID: 15011
		private CharacterStateManager stateManager;

		// Token: 0x04003AA4 RID: 15012
		private bool _started;

		// Token: 0x04003AA5 RID: 15013
		private OutfitSystem _observedOutfitSystem;

		// Token: 0x04003AA6 RID: 15014
		private Coroutine _outfitRefreshRoutine;

		// Token: 0x04003AA7 RID: 15015
		private readonly List<GameObject> _paletteRows = new List<GameObject>();

		// Token: 0x04003AA8 RID: 15016
		private static readonly ColorManager.PaletteSeed[] SkinPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Porcelain", new string[] { "FFE1D3", "F0BFB4", "9D625B", "56363A" }),
			ColorManager.Seed("Fair", new string[] { "F1C4AB", "DFA18F", "8E554A", "4A2D31" }),
			ColorManager.Seed("Tan", new string[] { "DA9F7C", "C77C68", "77443B", "3D2829" }),
			ColorManager.Seed("Bronze", new string[] { "B3744F", "95523F", "5A332C", "302123" }),
			ColorManager.Seed("Deep", new string[] { "7E4B32", "673629", "43241F", "25191B" }),
			ColorManager.Seed("Umber", new string[] { "4C2B22", "3E211D", "2B1817", "181013" })
		};

		// Token: 0x04003AA9 RID: 15017
		private static readonly ColorManager.PaletteSeed[] HairPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Midnight", new string[] { "171722", "34314A", "776B9D", "090A10" }),
			ColorManager.Seed("Espresso", new string[] { "2B1814", "5B3025", "A66B46", "140B0A" }),
			ColorManager.Seed("Chestnut", new string[] { "63331F", "A55A32", "E19A5B", "321812" }),
			ColorManager.Seed("Copper", new string[] { "9A3E25", "D86B3C", "F4B071", "4B1D18" }),
			ColorManager.Seed("Honey", new string[] { "B9782E", "E2B75D", "FFF0AC", "68401D" }),
			ColorManager.Seed("Platinum", new string[] { "D8D4CE", "F4F0E8", "FFFFFF", "8B8790" }),
			ColorManager.Seed("Silver", new string[] { "6F7380", "AEB5C2", "E6EBF2", "343541" }),
			ColorManager.Seed("Rose Quartz", new string[] { "C74F78", "F08FAE", "FFD0DE", "68283F" }),
			ColorManager.Seed("Lavender", new string[] { "6C4AA0", "A785D6", "E0CCFF", "35234F" }),
			ColorManager.Seed("Ocean", new string[] { "185B87", "2F9BBB", "8CE0E0", "0D2E4C" }),
			ColorManager.Seed("Emerald", new string[] { "176A58", "36A77E", "9BE5B7", "0C372E" }),
			ColorManager.Seed("Cherry", new string[] { "7E1933", "C53355", "F27C91", "3E0C20" })
		};

		// Token: 0x04003AAA RID: 15018
		private static readonly ColorManager.PaletteSeed[] EyePaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Hazel", new string[] { "9A7135", "49321C", "E6C477", "1B1511" }),
			ColorManager.Seed("Amber", new string[] { "D78A24", "68370F", "FFD66E", "28170B" }),
			ColorManager.Seed("Emerald", new string[] { "2D9B68", "104A37", "91E0AD", "09251D" }),
			ColorManager.Seed("Sea Glass", new string[] { "3D9FA6", "15525E", "A8ECE1", "0A2C33" }),
			ColorManager.Seed("Sky", new string[] { "53A5E8", "235286", "BBDFFF", "101F3D" }),
			ColorManager.Seed("Sapphire", new string[] { "315CC5", "182A69", "7FA8FF", "0B1235" }),
			ColorManager.Seed("Violet", new string[] { "7753C7", "38266D", "C4A6FF", "1A1237" }),
			ColorManager.Seed("Rose", new string[] { "CA5278", "6D2943", "FFACC4", "351320" }),
			ColorManager.Seed("Silver", new string[] { "A7AFBA", "4C5360", "EDF4FF", "252A32" }),
			ColorManager.Seed("Onyx", new string[] { "30323B", "0D0E13", "8C91A0", "050507" })
		};

		// Token: 0x04003AAB RID: 15019
		private static readonly ColorManager.PaletteSeed[] FurPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Snow", new string[] { "F2EEE8", "E9B7AE", "FFFFFF", "756B72" }),
			ColorManager.Seed("Cream", new string[] { "E8D2AE", "D99B8C", "FFF0CE", "76604B" }),
			ColorManager.Seed("Calico", new string[] { "D9904F", "F1C1A4", "F7EFE3", "4A3128" }),
			ColorManager.Seed("Ginger", new string[] { "B85B2E", "E5A073", "F4D0AE", "55261C" }),
			ColorManager.Seed("Cocoa", new string[] { "6A4435", "C18A72", "E7C2AD", "30211F" }),
			ColorManager.Seed("Tuxedo", new string[] { "24242A", "E9E7E4", "FFFFFF", "0D0D11" }),
			ColorManager.Seed("Blue Grey", new string[] { "68738B", "C4AAB5", "DDE5F0", "303647" }),
			ColorManager.Seed("Lilac", new string[] { "8E72AE", "D2A9BE", "E9DDF5", "453654" }),
			ColorManager.Seed("Mint", new string[] { "64A891", "D3A7A2", "CDEBDD", "2D5147" }),
			ColorManager.Seed("Midnight", new string[] { "25243A", "A67B9E", "777AAE", "0D0C17" })
		};

		// Token: 0x04003AAC RID: 15020
		private static readonly ColorManager.PaletteSeed[] DenimPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Classic Indigo", new string[] { "244A78", "527EB0", "D5B98B", "14283F" }),
			ColorManager.Seed("Stonewash", new string[] { "6F8FAA", "AFC4D2", "E7DED0", "3F566D" }),
			ColorManager.Seed("Dark Denim", new string[] { "17283E", "36516F", "B58E64", "0B1421" }),
			ColorManager.Seed("Black Denim", new string[] { "292B32", "555965", "C7CAD0", "111217" }),
			ColorManager.Seed("Ecru", new string[] { "D7CEBA", "F0E8D8", "9C7657", "6E6659" }),
			ColorManager.Seed("Vintage Blue", new string[] { "3F6682", "83A2AF", "C98B67", "253B4C" }),
			ColorManager.Seed("Berry Denim", new string[] { "65384E", "A76982", "E4B1BF", "321C29" }),
			ColorManager.Seed("Olive Denim", new string[] { "52604B", "87947A", "D3C6A2", "293127" })
		};

		// Token: 0x04003AAD RID: 15021
		private static readonly ColorManager.PaletteSeed[] FormalPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Ivory Gold", new string[] { "F0E9DC", "D5B66C", "FFF7EA", "54483A" }),
			ColorManager.Seed("Black Tie", new string[] { "1C1B22", "B9A56A", "E7E2D8", "09090D" }),
			ColorManager.Seed("Midnight Navy", new string[] { "172640", "426083", "C3CCD9", "0A1323" }),
			ColorManager.Seed("Burgundy", new string[] { "671F32", "B25A70", "DAB58B", "30101B" }),
			ColorManager.Seed("Emerald", new string[] { "16493B", "4B876D", "D1B879", "09251D" }),
			ColorManager.Seed("Champagne", new string[] { "CBB88E", "F1DEC0", "FFFFFF", "6A5A45" }),
			ColorManager.Seed("Dusty Rose", new string[] { "A86E7D", "E2B6C0", "F8E7E6", "583742" }),
			ColorManager.Seed("Royal Violet", new string[] { "44246E", "8766B5", "D5C4EB", "211136" }),
			ColorManager.Seed("Silver", new string[] { "5A606B", "AEB5BF", "EFF2F5", "272A31" }),
			ColorManager.Seed("Chocolate", new string[] { "4C3028", "9B6550", "D7B28C", "241714" })
		};

		// Token: 0x04003AAE RID: 15022
		private static readonly ColorManager.PaletteSeed[] ActivewearPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Graphite", new string[] { "252831", "505866", "AAB2BF", "111319" }),
			ColorManager.Seed("Ocean Sport", new string[] { "176B9A", "42C1C7", "E6FBFA", "0A344D" }),
			ColorManager.Seed("Berry Punch", new string[] { "B72E62", "F0688B", "FFD3DD", "57142E" }),
			ColorManager.Seed("Lime Energy", new string[] { "7DAA24", "C9E65D", "26372D", "364D13" }),
			ColorManager.Seed("Sunset", new string[] { "E25F3F", "F5B14B", "5B315E", "702A25" }),
			ColorManager.Seed("Electric Violet", new string[] { "6542D1", "B16BE2", "35C5D2", "2B1B62" }),
			ColorManager.Seed("Marine", new string[] { "123A66", "2C76A8", "F1E8D2", "081C34" }),
			ColorManager.Seed("Forest", new string[] { "295A46", "69A36C", "D5D6A2", "132D24" }),
			ColorManager.Seed("Coral", new string[] { "D84F56", "FF9C89", "FFF0DC", "6B242B" }),
			ColorManager.Seed("Ice", new string[] { "B9DCE4", "EDF8F8", "638AA2", "324A5B" })
		};

		// Token: 0x04003AAF RID: 15023
		private static readonly ColorManager.PaletteSeed[] FootwearPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Black Leather", new string[] { "242125", "4F4748", "B6A47F", "0E0D10" }),
			ColorManager.Seed("Cognac", new string[] { "8A4E28", "C6844D", "E0C39A", "402313" }),
			ColorManager.Seed("Dark Brown", new string[] { "4D3025", "79513E", "B99A75", "241713" }),
			ColorManager.Seed("Sand", new string[] { "B99D79", "E2D1B5", "7D6651", "5D4B3A" }),
			ColorManager.Seed("White Sneaker", new string[] { "E9E8E4", "FFFFFF", "8B929D", "4B4D53" }),
			ColorManager.Seed("Navy", new string[] { "1E3553", "4D6B8E", "D8D0BD", "0E1A2B" }),
			ColorManager.Seed("Burgundy", new string[] { "67283A", "A85A68", "D5B58A", "30121D" }),
			ColorManager.Seed("Olive", new string[] { "4F5B3C", "879064", "C7B58A", "252C1E" }),
			ColorManager.Seed("Pastel", new string[] { "B89EC9", "F0CAD3", "F7F1E7", "655676" }),
			ColorManager.Seed("Cyber", new string[] { "252530", "8F48D7", "36D4CF", "0D0D13" })
		};

		// Token: 0x04003AB0 RID: 15024
		private static readonly ColorManager.PaletteSeed[] AccessoryPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Neutral", new string[] { "CFC7B8", "F2EEE5", "8E8176", "49434A" }),
			ColorManager.Seed("Onyx", new string[] { "25242B", "55515E", "B9B4C1", "0D0D11" }),
			ColorManager.Seed("Ruby", new string[] { "A92343", "E06076", "D7B06E", "4D0F21" }),
			ColorManager.Seed("Sapphire", new string[] { "214E9A", "5D8DD3", "D5C27D", "10264D" }),
			ColorManager.Seed("Amethyst", new string[] { "65409B", "A783C7", "D9BD73", "311D4D" }),
			ColorManager.Seed("Jade", new string[] { "24745C", "6CB193", "D5BD76", "10392D" }),
			ColorManager.Seed("Rose Gold", new string[] { "B97872", "E2B1A4", "F3DAC2", "66403E" }),
			ColorManager.Seed("Silver", new string[] { "78808B", "C0C7CE", "F1F3F5", "363A42" }),
			ColorManager.Seed("Candy", new string[] { "E46F9B", "75C7D6", "F8D76C", "71344E" }),
			ColorManager.Seed("Autumn", new string[] { "9A542E", "D18B48", "73804E", "492819" })
		};

		// Token: 0x04003AB1 RID: 15025
		private static readonly ColorManager.PaletteSeed[] FabricPaletteSeeds = new ColorManager.PaletteSeed[]
		{
			ColorManager.Seed("Monochrome", new string[] { "2A2930", "ECE9E2", "90939C", "111116" }),
			ColorManager.Seed("Navy Cream", new string[] { "1F3A5A", "E8DFC9", "A57B54", "0D1D30" }),
			ColorManager.Seed("Sage", new string[] { "748069", "CBD0B5", "A67B5B", "3A4437" }),
			ColorManager.Seed("Burgundy", new string[] { "7B2941", "D58A9C", "E5C8A0", "35121F" }),
			ColorManager.Seed("Terracotta", new string[] { "B65F3D", "E4A06F", "F0D1A6", "582B20" }),
			ColorManager.Seed("Blush", new string[] { "D48B9D", "F2C8D0", "FFF0E8", "754450" }),
			ColorManager.Seed("Lavender", new string[] { "7961A5", "BBA7D6", "E7DBF0", "3A2C52" }),
			ColorManager.Seed("Ocean", new string[] { "236F8A", "66B8C1", "D5EEE9", "103949" }),
			ColorManager.Seed("Sunflower", new string[] { "D6A72D", "F2D66B", "465B43", "715315" }),
			ColorManager.Seed("Forest", new string[] { "315C46", "75956A", "D2C99C", "172E24" }),
			ColorManager.Seed("Cyber", new string[] { "372751", "A344C7", "32C9CF", "161022" }),
			ColorManager.Seed("Cocoa", new string[] { "5B3B32", "A9745E", "D5B78F", "2B1C19" })
		};

		// Token: 0x04003AB2 RID: 15026
		private GameObject _mcSelectorTemplate;

		// Token: 0x04003AB3 RID: 15027
		private ColorPreviewUI currentSelectedPreview;

		// Token: 0x04003AB4 RID: 15028
		private bool isEditingLinkedColor;

		// Token: 0x02000879 RID: 2169
		[Serializable]
		public class MaterialColorMapping
		{
			// Token: 0x04003AB5 RID: 15029
			public Material material;

			// Token: 0x04003AB6 RID: 15030
			public string materialName;

			// Token: 0x04003AB7 RID: 15031
			public List<string> colorPropertyNames = new List<string>();

			// Token: 0x04003AB8 RID: 15032
			public ColorChannelConfigSO config;
		}

		// Token: 0x0200087A RID: 2170
		[Serializable]
		public class LinkedColorGroup
		{
			// Token: 0x04003AB9 RID: 15033
			[Tooltip("Configurazione del gruppo di colori")]
			public ColorGroupConfigSO groupConfig;

			// Token: 0x04003ABA RID: 15034
			[Tooltip("Lista delle coppie materiale-proprietà collegate")]
			public List<ColorManager.LinkedMaterialProperty> linkedProperties = new List<ColorManager.LinkedMaterialProperty>();

			// Token: 0x04003ABB RID: 15035
			[Tooltip("Preview UI associata a questo gruppo")]
			[HideInInspector]
			public ColorPreviewUI previewUI;
		}

		// Token: 0x0200087B RID: 2171
		[Serializable]
		public class LinkedMaterialProperty
		{
			// Token: 0x06003A8B RID: 14987 RVA: 0x0015C10D File Offset: 0x0015A30D
			public LinkedMaterialProperty(Material mat, string prop)
			{
				this.material = mat;
				this.propertyName = prop;
			}

			// Token: 0x04003ABC RID: 15036
			public Material material;

			// Token: 0x04003ABD RID: 15037
			public string propertyName;
		}

		// Token: 0x0200087C RID: 2172
		// (Invoke) Token: 0x06003A8D RID: 14989
		public delegate void ColorChangedHandler(Material material, string propertyName, Color color);

		// Token: 0x0200087D RID: 2173
		private sealed class BodyPresetDeck
		{
			// Token: 0x06003A90 RID: 14992 RVA: 0x0015C124 File Offset: 0x0015A324
			public void SetSelectedIndex(int selectedIndex)
			{
				this.currentIndex = selectedIndex;
				if (this.label != null)
				{
					CustomizationPresentation.BindColorLabel(this.label, (selectedIndex >= 0 && this.presets != null && selectedIndex < this.presets.Count) ? this.presets[selectedIndex].name : "CUSTOM", true);
				}
			}

			// Token: 0x04003ABE RID: 15038
			public Outfit outfit;

			// Token: 0x04003ABF RID: 15039
			public List<ColorManager.PresetOption> presets;

			// Token: 0x04003AC0 RID: 15040
			public TextMeshProUGUI label;

			// Token: 0x04003AC1 RID: 15041
			public int currentIndex = -1;
		}

		// Token: 0x0200087E RID: 2174
		private class PresetOption
		{
			// Token: 0x04003AC2 RID: 15042
			public string name;

			// Token: 0x04003AC3 RID: 15043
			public List<Color> colors;
		}

		// Token: 0x0200087F RID: 2175
		private enum PaletteFamily
		{
			// Token: 0x04003AC5 RID: 15045
			Skin,
			// Token: 0x04003AC6 RID: 15046
			Hair,
			// Token: 0x04003AC7 RID: 15047
			Eyes,
			// Token: 0x04003AC8 RID: 15048
			Fur,
			// Token: 0x04003AC9 RID: 15049
			Denim,
			// Token: 0x04003ACA RID: 15050
			Formal,
			// Token: 0x04003ACB RID: 15051
			Activewear,
			// Token: 0x04003ACC RID: 15052
			Footwear,
			// Token: 0x04003ACD RID: 15053
			Accessory,
			// Token: 0x04003ACE RID: 15054
			Fabric
		}

		// Token: 0x02000880 RID: 2176
		private sealed class PaletteSeed
		{
			// Token: 0x06003A93 RID: 14995 RVA: 0x0015C193 File Offset: 0x0015A393
			public PaletteSeed(string name, params Color32[] colors)
			{
				this.name = name;
				this.colors = colors;
			}

			// Token: 0x04003ACF RID: 15055
			public readonly string name;

			// Token: 0x04003AD0 RID: 15056
			public readonly Color32[] colors;
		}
	}
}
