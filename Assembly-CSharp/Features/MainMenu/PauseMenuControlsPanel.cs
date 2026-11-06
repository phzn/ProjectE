using System;
using DA_Assets.CR;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x020010AB RID: 4267
	[DisallowMultipleComponent]
	public sealed class PauseMenuControlsPanel : MonoBehaviour
	{
		// Token: 0x06008146 RID: 33094 RVA: 0x0034CEB7 File Offset: 0x0034B0B7
		public void Initialize(TMP_FontAsset font)
		{
			this._font = font;
			this._controllerLayout = ControllerInput.IsActive;
			this._controllerGlyphVersion = ControllerGlyphs.Version;
			this.Build();
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x0034CEDC File Offset: 0x0034B0DC
		private void OnEnable()
		{
			if (this._built && (this._controllerLayout != ControllerInput.IsActive || this._controllerGlyphVersion != ControllerGlyphs.Version))
			{
				this.RebuildForDevice();
			}
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x0034CEDC File Offset: 0x0034B0DC
		private void Update()
		{
			if (this._built && (this._controllerLayout != ControllerInput.IsActive || this._controllerGlyphVersion != ControllerGlyphs.Version))
			{
				this.RebuildForDevice();
			}
		}

		// Token: 0x06008149 RID: 33097 RVA: 0x0034CF08 File Offset: 0x0034B108
		private void RebuildForDevice()
		{
			this._controllerLayout = ControllerInput.IsActive;
			this._controllerGlyphVersion = ControllerGlyphs.Version;
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				gameObject.SetActive(false);
				Object.Destroy(gameObject);
			}
			this._built = false;
			this.Build();
		}

		// Token: 0x0600814A RID: 33098 RVA: 0x0034CF70 File Offset: 0x0034B170
		private void Build()
		{
			if (!this._built)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				if (rectTransform != null)
				{
					this._built = true;
					ScrollRect scrollRect = base.GetComponent<ScrollRect>() ?? base.gameObject.AddComponent<ScrollRect>();
					scrollRect.horizontal = false;
					scrollRect.vertical = true;
					scrollRect.movementType = ScrollRect.MovementType.Clamped;
					scrollRect.inertia = true;
					scrollRect.decelerationRate = 0.12f;
					scrollRect.scrollSensitivity = 38f;
					RectTransform rectTransform2 = PauseMenuControlsPanel.CreateRect("Controls Viewport", rectTransform);
					PauseMenuControlsPanel.Stretch(rectTransform2, Vector2.zero, new Vector2(-18f, 0f));
					Image image = rectTransform2.gameObject.AddComponent<Image>();
					image.color = new Color(0f, 0f, 0f, 0f);
					image.raycastTarget = true;
					rectTransform2.gameObject.AddComponent<RectMask2D>();
					RectTransform rectTransform3 = PauseMenuControlsPanel.CreateRect("Controls List Content", rectTransform2);
					rectTransform3.anchorMin = new Vector2(0f, 1f);
					rectTransform3.anchorMax = Vector2.one;
					rectTransform3.pivot = new Vector2(0.5f, 1f);
					rectTransform3.offsetMin = Vector2.zero;
					rectTransform3.offsetMax = Vector2.zero;
					float num = 0f;
					PauseMenuControlsPanel.ControlSection[] array = (this._controllerLayout ? PauseMenuControlsPanel.ControllerSections : PauseMenuControlsPanel.Sections);
					for (int i = 0; i < array.Length; i++)
					{
						PauseMenuControlsPanel.ControlSection controlSection = array[i];
						if (i > 0)
						{
							num += 13f;
						}
						this.CreateSectionLabel(rectTransform3, controlSection.title, num);
						num += 31f;
						for (int j = 0; j < controlSection.entries.Length; j++)
						{
							PauseMenuControlsPanel.ControlEntry controlEntry = controlSection.entries[j];
							float num2 = this.ControlRowHeight(controlEntry);
							this.CreateControlRow(rectTransform3, controlEntry, num, num2);
							num += num2 + 14f;
						}
					}
					rectTransform3.sizeDelta = new Vector2(0f, num + 8f);
					scrollRect.viewport = rectTransform2;
					scrollRect.content = rectTransform3;
					Scrollbar scrollbar = this.BuildScrollbar(rectTransform);
					scrollRect.verticalScrollbar = scrollbar;
					scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
					scrollRect.verticalNormalizedPosition = 1f;
					(base.GetComponent<SmoothCollectionScroll>() ?? base.gameObject.AddComponent<SmoothCollectionScroll>()).Configure(48f, 0.1f);
					return;
				}
			}
		}

		// Token: 0x0600814B RID: 33099 RVA: 0x0034D1C0 File Offset: 0x0034B3C0
		private void CreateSectionLabel(RectTransform parent, string title, float top)
		{
			TextMeshProUGUI textMeshProUGUI = this.CreateText("Section - " + title, parent, title, 15f, FontStyles.Bold, PauseMenuControlsPanel.Pink, TextAlignmentOptions.MidlineLeft, 11f, 0f);
			PauseMenuControlsPanel.SetTopRect(textMeshProUGUI.rectTransform, top, 31f, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			textMeshProUGUI.characterSpacing = 1.8f;
		}

		// Token: 0x0600814C RID: 33100 RVA: 0x0034D234 File Offset: 0x0034B434
		private float ControlRowHeight(PauseMenuControlsPanel.ControlEntry entry)
		{
			bool flag = entry.binding == "HOLD LB / B OR RELEASE LB" || entry.binding == "A - ON-SCREEN KEYBOARD" || entry.binding == "LT + FACE / SHOULDER";
			if (!this._controllerLayout || !flag)
			{
				return 62f;
			}
			return 102f;
		}

		// Token: 0x0600814D RID: 33101 RVA: 0x0034D290 File Offset: 0x0034B490
		private void CreateControlRow(RectTransform parent, PauseMenuControlsPanel.ControlEntry entry, float top, float height)
		{
			RectTransform rectTransform = PauseMenuControlsPanel.CreateRect("Control - " + entry.action, parent);
			PauseMenuControlsPanel.SetTopRect(rectTransform, top, height, Vector2.zero, new Vector2(-4f, 0f));
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = PauseMenuControlsPanel.RowColor;
			image.raycastTarget = true;
			PauseMenuControlsPanel.Round(rectTransform.gameObject, 14f);
			PauseMenuControlsPanel.SetRect(this.CreateText("Action", rectTransform, entry.action, 18f, FontStyles.Bold, PauseMenuControlsPanel.White, TextAlignmentOptions.MidlineLeft, 13f, 18f).rectTransform, Vector2.zero, new Vector2(0.56f, 1f), new Vector2(22f, 0f), new Vector2(-8f, 0f));
			RectTransform rectTransform2 = PauseMenuControlsPanel.CreateRect("Binding", rectTransform);
			if (height > 62f)
			{
				PauseMenuControlsPanel.SetRect(rectTransform2, new Vector2(0.56f, 0f), Vector2.one, new Vector2(6f, 6f), new Vector2(-16f, -6f));
			}
			else
			{
				PauseMenuControlsPanel.SetRect(rectTransform2, new Vector2(0.56f, 0.18f), new Vector2(1f, 0.82f), new Vector2(6f, 0f), new Vector2(-16f, 0f));
			}
			Image image2 = rectTransform2.gameObject.AddComponent<Image>();
			image2.color = PauseMenuControlsPanel.KeyColor;
			image2.raycastTarget = false;
			PauseMenuControlsPanel.Round(rectTransform2.gameObject, 10f);
			TextMeshProUGUI textMeshProUGUI = this.CreateText("Keys", rectTransform2, entry.binding, 15f, FontStyles.Bold, PauseMenuControlsPanel.PalePink, TextAlignmentOptions.Center, this._controllerLayout ? 15f : 10f, this._controllerLayout ? 18f : 15f);
			if (this._controllerLayout)
			{
				LocalizedGameText.BindComputed(textMeshProUGUI, () => ControllerGlyphs.Format(GameLocalization.GetSource("controls", entry.binding)));
			}
			textMeshProUGUI.characterSpacing = 0.8f;
			PauseMenuControlsPanel.Stretch(textMeshProUGUI.rectTransform, new Vector2(8f, 0f), new Vector2(-8f, 0f));
		}

		// Token: 0x0600814E RID: 33102 RVA: 0x0034D4D8 File Offset: 0x0034B6D8
		private Scrollbar BuildScrollbar(RectTransform parent)
		{
			RectTransform rectTransform = PauseMenuControlsPanel.CreateRect("Scrollbar", parent);
			PauseMenuControlsPanel.SetRect(rectTransform, new Vector2(1f, 0f), Vector2.one, new Vector2(-7f, 0f), Vector2.zero);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.035f, 0.015f, 0.035f, 0.82f);
			image.raycastTarget = false;
			PauseMenuControlsPanel.Round(rectTransform.gameObject, 4f);
			RectTransform rectTransform2 = PauseMenuControlsPanel.CreateRect("Sliding Area", rectTransform);
			PauseMenuControlsPanel.Stretch(rectTransform2, Vector2.zero, Vector2.zero);
			RectTransform rectTransform3 = PauseMenuControlsPanel.CreateRect("Handle", rectTransform2);
			PauseMenuControlsPanel.Stretch(rectTransform3, Vector2.zero, Vector2.zero);
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			image2.color = PauseMenuControlsPanel.Pink;
			PauseMenuControlsPanel.Round(rectTransform3.gameObject, 4f);
			Scrollbar scrollbar = rectTransform.gameObject.AddComponent<Scrollbar>();
			scrollbar.targetGraphic = image2;
			scrollbar.handleRect = rectTransform3;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			return scrollbar;
		}

		// Token: 0x0600814F RID: 33103 RVA: 0x0034D5E0 File Offset: 0x0034B7E0
		private TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment, float minSize = 11f, float maxSize = 0f)
		{
			TextMeshProUGUI textMeshProUGUI = PauseMenuControlsPanel.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.enableAutoSizing = true;
			textMeshProUGUI.fontSizeMin = minSize;
			textMeshProUGUI.fontSizeMax = ((maxSize > 0f) ? maxSize : size);
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			LocalizedGameText.BindSource(textMeshProUGUI, "controls", value, null);
			return textMeshProUGUI;
		}

		// Token: 0x06008150 RID: 33104 RVA: 0x0021CF77 File Offset: 0x0021B177
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.localScale = Vector3.one;
			return component;
		}

		// Token: 0x06008151 RID: 33105 RVA: 0x0034D684 File Offset: 0x0034B884
		private static void SetTopRect(RectTransform rect, float top, float height, Vector2 insetMin, Vector2 insetMax)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(insetMin.x, -top - height + insetMin.y);
			rect.offsetMax = new Vector2(insetMax.x, -top + insetMax.y);
		}

		// Token: 0x06008152 RID: 33106 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x06008153 RID: 33107 RVA: 0x0034D6E9 File Offset: 0x0034B8E9
		private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
		{
			PauseMenuControlsPanel.SetRect(rect, Vector2.zero, Vector2.one, offsetMin, offsetMax);
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x0400848F RID: 33935
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04008490 RID: 33936
		private static readonly Color PalePink = new Color(1f, 0.78f, 0.89f, 1f);

		// Token: 0x04008491 RID: 33937
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04008492 RID: 33938
		private static readonly Color RowColor = new Color(0.165f, 0.118f, 0.2f, 0.9f);

		// Token: 0x04008493 RID: 33939
		private static readonly Color KeyColor = new Color(0.21f, 0.095f, 0.19f, 0.98f);

		// Token: 0x04008494 RID: 33940
		private const float SectionHeight = 31f;

		// Token: 0x04008495 RID: 33941
		private const float RowHeight = 62f;

		// Token: 0x04008496 RID: 33942
		private const float InstructionRowHeight = 102f;

		// Token: 0x04008497 RID: 33943
		private const float RowGap = 14f;

		// Token: 0x04008498 RID: 33944
		private static readonly PauseMenuControlsPanel.ControlSection[] Sections = new PauseMenuControlsPanel.ControlSection[]
		{
			new PauseMenuControlsPanel.ControlSection("MOVEMENT & CAMERA", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Move", "W A S D"),
				new PauseMenuControlsPanel.ControlEntry("Look", "MOUSE"),
				new PauseMenuControlsPanel.ControlEntry("Run", "LEFT SHIFT"),
				new PauseMenuControlsPanel.ControlEntry("Camera zoom", "MOUSE WHEEL"),
				new PauseMenuControlsPanel.ControlEntry("Camera height", "UP / DOWN"),
				new PauseMenuControlsPanel.ControlEntry("First-person view", "V"),
				new PauseMenuControlsPanel.ControlEntry("Photo Mode", "K")
			}),
			new PauseMenuControlsPanel.ControlSection("AUTO MOVEMENT", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Start AutoWalk while walking", "W + CAPS LOCK"),
				new PauseMenuControlsPanel.ControlEntry("Start AutoRun while running", "W + SHIFT + CAPS LOCK"),
				new PauseMenuControlsPanel.ControlEntry("Adjust automatic speed", "CAPS LOCK + WHEEL"),
				new PauseMenuControlsPanel.ControlEntry("Cancel AutoWalk / AutoRun", "ANY W A S D")
			}),
			new PauseMenuControlsPanel.ControlSection("ACTIONS & MENUS", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Interact", "E"),
				new PauseMenuControlsPanel.ControlEntry("Pick up item", "F"),
				new PauseMenuControlsPanel.ControlEntry("End emote", "X"),
				new PauseMenuControlsPanel.ControlEntry("Emotion stance", "TAB"),
				new PauseMenuControlsPanel.ControlEntry("Inventory", "I"),
				new PauseMenuControlsPanel.ControlEntry("Smartphone", "P"),
				new PauseMenuControlsPanel.ControlEntry("Map", "M")
			}),
			new PauseMenuControlsPanel.ControlSection("QUICK ACCESS", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Inventory slots", "1 / 2 / 3"),
				new PauseMenuControlsPanel.ControlEntry("Cover pose", "RIGHT MOUSE"),
				new PauseMenuControlsPanel.ControlEntry("Pause / back", "ESC")
			})
		};

		// Token: 0x04008499 RID: 33945
		private static readonly PauseMenuControlsPanel.ControlSection[] ControllerSections = new PauseMenuControlsPanel.ControlSection[]
		{
			new PauseMenuControlsPanel.ControlSection("CONTROLLER - MOVEMENT & CAMERA", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Move / look", "LS / RS"),
				new PauseMenuControlsPanel.ControlEntry("Toggle run / hold sprint", "L3 / LT + L3"),
				new PauseMenuControlsPanel.ControlEntry("Toggle vehicle sprint", "L3"),
				new PauseMenuControlsPanel.ControlEntry("Jump / confirm", "A"),
				new PauseMenuControlsPanel.ControlEntry("First-person view / Photo Mode", "R3 / D-PAD DOWN"),
				new PauseMenuControlsPanel.ControlEntry("Camera zoom", "LT + RS UP / DOWN"),
				new PauseMenuControlsPanel.ControlEntry("Camera height", "LT + (D-PAD UP / D-PAD DOWN)")
			}),
			new PauseMenuControlsPanel.ControlSection("CONTROLLER - ACTIONS & MENUS", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Interact", "X"),
				new PauseMenuControlsPanel.ControlEntry("Pick up / contextual action", "RT"),
				new PauseMenuControlsPanel.ControlEntry("Crouch / hold for stance wheel", "Y / HOLD Y"),
				new PauseMenuControlsPanel.ControlEntry("Arms pose / hold for actions", "LB / HOLD LB"),
				new PauseMenuControlsPanel.ControlEntry("Emotion stance", "LT + Y"),
				new PauseMenuControlsPanel.ControlEntry("Clothing action / slip", "LT + (D-PAD LEFT / D-PAD RIGHT)"),
				new PauseMenuControlsPanel.ControlEntry("Secondary clothing action", "LT + RT"),
				new PauseMenuControlsPanel.ControlEntry("Auto movement", "LT + VIEW"),
				new PauseMenuControlsPanel.ControlEntry("Inventory", "D-PAD UP"),
				new PauseMenuControlsPanel.ControlEntry("Smartphone / map", "VIEW / HOLD VIEW"),
				new PauseMenuControlsPanel.ControlEntry("Pause / back", "MENU / B")
			}),
			new PauseMenuControlsPanel.ControlSection("CONTROLLER - INVENTORY & WHEELS", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Clothing: focus / next", "D-PAD LEFT"),
				new PauseMenuControlsPanel.ControlEntry("Items: focus / next", "D-PAD RIGHT"),
				new PauseMenuControlsPanel.ControlEntry("Deselect clothing / put away item", "B"),
				new PauseMenuControlsPanel.ControlEntry("Use selected item / place", "RB"),
				new PauseMenuControlsPanel.ControlEntry("Placeable item: aim / cancel", "HOLD LB / B OR RELEASE LB"),
				new PauseMenuControlsPanel.ControlEntry("Active item: adjust intensity", "LT + RS UP / DOWN"),
				new PauseMenuControlsPanel.ControlEntry("Inventory: use / discard", "A / Y"),
				new PauseMenuControlsPanel.ControlEntry("Inventory: select and show item actions", "LB"),
				new PauseMenuControlsPanel.ControlEntry("Quick emote / hold for wheel", "LT + LB / LT + B"),
				new PauseMenuControlsPanel.ControlEntry("Choose wheel action / apply / cancel", "RS / RELEASE / B"),
				new PauseMenuControlsPanel.ControlEntry("End emote", "A"),
				new PauseMenuControlsPanel.ControlEntry("Wheel pages", "LB / RB"),
				new PauseMenuControlsPanel.ControlEntry("Selfie: pose / face wheel", "D-PAD LEFT / D-PAD RIGHT")
			}),
			new PauseMenuControlsPanel.ControlSection("CONTROLLER - PHONE", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Navigate / open app / back", "LS OR D-PAD / A / B"),
				new PauseMenuControlsPanel.ControlEntry("Scroll lists", "RS"),
				new PauseMenuControlsPanel.ControlEntry("Adjust view", "L3 + RS"),
				new PauseMenuControlsPanel.ControlEntry("Zoom screen", "LT + RS UP / DOWN"),
				new PauseMenuControlsPanel.ControlEntry("Center phone", "R3")
			}),
			new PauseMenuControlsPanel.ControlSection("CONTROLLER - INTERFACES", new PauseMenuControlsPanel.ControlEntry[]
			{
				new PauseMenuControlsPanel.ControlEntry("Navigate / confirm / back", "LS OR D-PAD / A / B"),
				new PauseMenuControlsPanel.ControlEntry("Scroll lists", "RS"),
				new PauseMenuControlsPanel.ControlEntry("Text fields", "A - ON-SCREEN KEYBOARD"),
				new PauseMenuControlsPanel.ControlEntry("Customization: category (outside sliders)", "LB / RB"),
				new PauseMenuControlsPanel.ControlEntry("Customization: subcategory", "LT + LB / LT + RB"),
				new PauseMenuControlsPanel.ControlEntry("Slider value", "LB / RB"),
				new PauseMenuControlsPanel.ControlEntry("Reset selected slider", "X"),
				new PauseMenuControlsPanel.ControlEntry("Run settings: start run", "MENU"),
				new PauseMenuControlsPanel.ControlEntry("Customization: save", "Y"),
				new PauseMenuControlsPanel.ControlEntry("More contextual actions", "LT + FACE / SHOULDER")
			})
		};

		// Token: 0x0400849A RID: 33946
		private bool _controllerLayout;

		// Token: 0x0400849B RID: 33947
		private int _controllerGlyphVersion;

		// Token: 0x0400849C RID: 33948
		private TMP_FontAsset _font;

		// Token: 0x0400849D RID: 33949
		private bool _built;

		// Token: 0x020010AC RID: 4268
		private readonly struct ControlEntry
		{
			// Token: 0x06008157 RID: 33111 RVA: 0x0034DE54 File Offset: 0x0034C054
			public ControlEntry(string action, string binding)
			{
				this.action = action;
				this.binding = binding;
			}

			// Token: 0x0400849E RID: 33950
			public readonly string action;

			// Token: 0x0400849F RID: 33951
			public readonly string binding;
		}

		// Token: 0x020010AD RID: 4269
		private readonly struct ControlSection
		{
			// Token: 0x06008158 RID: 33112 RVA: 0x0034DE64 File Offset: 0x0034C064
			public ControlSection(string title, params PauseMenuControlsPanel.ControlEntry[] entries)
			{
				this.title = title;
				this.entries = entries;
			}

			// Token: 0x040084A0 RID: 33952
			public readonly string title;

			// Token: 0x040084A1 RID: 33953
			public readonly PauseMenuControlsPanel.ControlEntry[] entries;
		}
	}
}
