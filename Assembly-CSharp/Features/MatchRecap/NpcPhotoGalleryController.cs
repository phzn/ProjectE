using System;
using System.Collections;
using DA_Assets.CR;
using Features.CensorBars;
using Features.GameOver.Newspaper;
using Features.Localization;
using Features.Score;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MatchRecap
{
	// Token: 0x02000FE0 RID: 4064
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-725)]
	public sealed class NpcPhotoGalleryController : MonoBehaviour
	{
		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06007AFE RID: 31486 RVA: 0x00323B16 File Offset: 0x00321D16
		// (set) Token: 0x06007AFF RID: 31487 RVA: 0x00323B1D File Offset: 0x00321D1D
		public static NpcPhotoGalleryController Instance { get; private set; }

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x06007B00 RID: 31488 RVA: 0x00323B25 File Offset: 0x00321D25
		// (set) Token: 0x06007B01 RID: 31489 RVA: 0x00323B2D File Offset: 0x00321D2D
		public bool IsShowing { get; private set; }

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06007B02 RID: 31490 RVA: 0x00323B38 File Offset: 0x00321D38
		public bool IsReady
		{
			get
			{
				return this._group != null && this._photo != null && this._counter != null && this._caption != null && this._status != null && this._saveButton != null;
			}
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x00323B9C File Offset: 0x00321D9C
		public static NpcPhotoGalleryController Ensure(Canvas parentCanvas, Font font, Sprite themeSprite)
		{
			if (NpcPhotoGalleryController.Instance != null && NpcPhotoGalleryController.Instance.IsReady)
			{
				return NpcPhotoGalleryController.Instance;
			}
			if (parentCanvas == null)
			{
				return null;
			}
			if (NpcPhotoGalleryController.Instance != null)
			{
				Object gameObject = NpcPhotoGalleryController.Instance.gameObject;
				NpcPhotoGalleryController.Instance = null;
				Object.Destroy(gameObject);
			}
			GameObject gameObject2 = new GameObject("NPC Photo Gallery", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(GraphicRaycaster),
				typeof(CanvasGroup)
			});
			gameObject2.layer = parentCanvas.gameObject.layer;
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.SetParent(parentCanvas.transform, false);
			NpcPhotoGalleryController.Stretch(component, 0f, 0f, 0f, 0f);
			component.SetAsLastSibling();
			Canvas component2 = gameObject2.GetComponent<Canvas>();
			component2.overrideSorting = true;
			component2.sortingOrder = parentCanvas.sortingOrder + 180;
			GameplayHudVisibility.RegisterCanvas(component2);
			NpcPhotoGalleryController npcPhotoGalleryController = gameObject2.AddComponent<NpcPhotoGalleryController>();
			NpcPhotoGalleryController npcPhotoGalleryController2;
			try
			{
				npcPhotoGalleryController.Build(font, themeSprite);
				npcPhotoGalleryController2 = npcPhotoGalleryController;
			}
			catch (Exception ex)
			{
				if (NpcPhotoGalleryController.Instance == npcPhotoGalleryController)
				{
					NpcPhotoGalleryController.Instance = null;
				}
				Object.Destroy(gameObject2);
				Debug.LogException(ex);
				npcPhotoGalleryController2 = null;
			}
			return npcPhotoGalleryController2;
		}

		// Token: 0x06007B04 RID: 31492 RVA: 0x00323CE8 File Offset: 0x00321EE8
		private void Awake()
		{
			if (NpcPhotoGalleryController.Instance != null && NpcPhotoGalleryController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NpcPhotoGalleryController.Instance = this;
			NpcPhotoSessionArchive.Changed += this.HandleArchiveChanged;
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x00323D27 File Offset: 0x00321F27
		private void OnDestroy()
		{
			NpcPhotoSessionArchive.Changed -= this.HandleArchiveChanged;
			this.RestoreCursor();
			if (NpcPhotoGalleryController.Instance == this)
			{
				NpcPhotoGalleryController.Instance = null;
			}
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x00323D54 File Offset: 0x00321F54
		private void Build(Font font, Sprite themeSprite)
		{
			this._rootRect = (RectTransform)base.transform;
			this._group = base.GetComponent<CanvasGroup>();
			this._group.alpha = 0f;
			this._group.interactable = false;
			this._group.blocksRaycasts = false;
			if (font == null)
			{
				font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			}
			Image image = NpcPhotoGalleryController.CreateImage("Dim", this._rootRect, new Color(0.08f, 0.015f, 0.1f, 0.82f));
			NpcPhotoGalleryController.Stretch(image.rectTransform, 0f, 0f, 0f, 0f);
			Button button = image.gameObject.AddComponent<Button>();
			button.transition = Selectable.Transition.None;
			button.onClick.AddListener(new UnityAction(this.Hide));
			Image image2 = NpcPhotoGalleryController.CreateImage("Gallery Panel", this._rootRect, NpcPhotoGalleryController.PanelColor);
			this._panelRect = image2.rectTransform;
			NpcPhotoGalleryController.Place(this._panelRect, Vector2.zero, new Vector2(560f, 760f), new Vector2(0.5f, 0.5f));
			image2.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(18f, 18f, 18f, 18f));
			image2.gameObject.AddComponent<RectMask2D>();
			Outline outline = image2.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0.12f, 0.025f, 0.16f, 0.88f);
			outline.effectDistance = new Vector2(4f, -4f);
			if (themeSprite != null)
			{
				Image image3 = NpcPhotoGalleryController.CreateImage("Recap Pattern", this._panelRect, new Color(1f, 1f, 1f, 0.28f));
				image3.sprite = themeSprite;
				image3.type = Image.Type.Simple;
				image3.preserveAspect = true;
				NpcPhotoGalleryController.Place(image3.rectTransform, Vector2.zero, new Vector2(780f, 780f), new Vector2(0.5f, 0.5f));
				image3.transform.SetAsFirstSibling();
			}
			Image image4 = NpcPhotoGalleryController.CreateImage("Accent", this._panelRect, NpcPhotoGalleryController.AccentColor);
			image4.rectTransform.anchorMin = Vector2.zero;
			image4.rectTransform.anchorMax = new Vector2(0f, 1f);
			image4.rectTransform.offsetMin = new Vector2(0f, 14f);
			image4.rectTransform.offsetMax = new Vector2(7f, -14f);
			image4.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(4f, 4f, 4f, 4f));
			NpcPhotoGalleryController.Place(NpcPhotoGalleryController.CreateText("Title", this._panelRect, font, "NPC PHOTOS", 28, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft).rectTransform, new Vector2(26f, -18f), new Vector2(310f, 44f), new Vector2(0f, 1f));
			this._counter = NpcPhotoGalleryController.CreateText("Counter", this._panelRect, font, "0 / 0", 16, FontStyle.Bold, new Color(1f, 0.78f, 0.88f, 1f), TextAnchor.MiddleRight);
			NpcPhotoGalleryController.Place(this._counter.rectTransform, new Vector2(-68f, -22f), new Vector2(150f, 38f), Vector2.one);
			Button button2 = NpcPhotoGalleryController.CreateButton("Close", this._panelRect, font, "X", new Vector2(42f, 42f), NpcPhotoGalleryController.DarkColor, Color.white, 18);
			NpcPhotoGalleryController.Place((RectTransform)button2.transform, new Vector2(-16f, -16f), new Vector2(42f, 42f), Vector2.one);
			button2.onClick.AddListener(new UnityAction(this.Hide));
			Image image5 = NpcPhotoGalleryController.CreateImage("Polaroid", this._panelRect, NpcPhotoGalleryController.PaperColor);
			RectTransform rectTransform = image5.rectTransform;
			NpcPhotoGalleryController.Place(rectTransform, new Vector2(0f, 28f), new Vector2(400f, 520f), new Vector2(0.5f, 0.5f));
			Outline outline2 = image5.gameObject.AddComponent<Outline>();
			outline2.effectColor = new Color(0.1f, 0.025f, 0.12f, 0.58f);
			outline2.effectDistance = new Vector2(5f, -5f);
			this._photo = NpcPhotoGalleryController.CreateRawImage("Photo", rectTransform);
			StreamerCensorImage.Bind(this._photo, false);
			NpcPhotoGalleryController.Stretch(this._photo.rectTransform, 18f, 18f, 18f, 72f);
			this._caption = NpcPhotoGalleryController.CreateText("Caption", rectTransform, font, string.Empty, 15, FontStyle.Bold, new Color(0.24f, 0.08f, 0.27f, 0.82f), TextAnchor.MiddleCenter);
			this._caption.rectTransform.anchorMin = Vector2.zero;
			this._caption.rectTransform.anchorMax = new Vector2(1f, 0f);
			this._caption.rectTransform.offsetMin = new Vector2(16f, 10f);
			this._caption.rectTransform.offsetMax = new Vector2(-16f, 64f);
			this._previousButton = NpcPhotoGalleryController.CreateButton("Previous", this._panelRect, font, "<", new Vector2(48f, 72f), NpcPhotoGalleryController.DarkColor, Color.white, 30);
			NpcPhotoGalleryController.Place((RectTransform)this._previousButton.transform, new Vector2(16f, 28f), new Vector2(48f, 72f), new Vector2(0f, 0.5f));
			this._previousButton.onClick.AddListener(delegate
			{
				this.Move(-1);
			});
			this._nextButton = NpcPhotoGalleryController.CreateButton("Next", this._panelRect, font, ">", new Vector2(48f, 72f), NpcPhotoGalleryController.DarkColor, Color.white, 30);
			NpcPhotoGalleryController.Place((RectTransform)this._nextButton.transform, new Vector2(-16f, 28f), new Vector2(48f, 72f), new Vector2(1f, 0.5f));
			this._nextButton.onClick.AddListener(delegate
			{
				this.Move(1);
			});
			this._status = NpcPhotoGalleryController.CreateText("Status", this._panelRect, font, "A / D  BROWSE", 13, FontStyle.Bold, new Color(1f, 1f, 1f, 0.68f), TextAnchor.MiddleCenter);
			NpcPhotoGalleryController.Place(this._status.rectTransform, new Vector2(0f, 92f), new Vector2(420f, 28f), new Vector2(0.5f, 0f));
			this._saveButton = NpcPhotoGalleryController.CreateButton("Save to PC", this._panelRect, font, "SAVE TO PC  [S]", new Vector2(238f, 54f), NpcPhotoGalleryController.AccentColor, Color.white, 17);
			NpcPhotoGalleryController.Place((RectTransform)this._saveButton.transform, new Vector2(-106f, 28f), new Vector2(238f, 54f), new Vector2(0.5f, 0f));
			this._saveLabel = this._saveButton.GetComponentInChildren<Text>();
			this._saveButton.onClick.AddListener(new UnityAction(this.SaveCurrent));
			Button button3 = NpcPhotoGalleryController.CreateButton("OpenExportFolder", this._panelRect, font, "OPEN FOLDER", new Vector2(174f, 54f), NpcPhotoGalleryController.DarkColor, Color.white, 16);
			NpcPhotoGalleryController.Place((RectTransform)button3.transform, new Vector2(112f, 28f), new Vector2(174f, 54f), new Vector2(0.5f, 0f));
			LocalizedLegacyGameText.BindSource(button3.GetComponentInChildren<Text>(), "phone_story", "OPEN FOLDER").FitTranslationToRectangle();
			button3.onClick.AddListener(new UnityAction(this.OpenExportFolder));
			this.ResetImmediate();
		}

		// Token: 0x06007B07 RID: 31495 RVA: 0x003245A0 File Offset: 0x003227A0
		public void Toggle()
		{
			if (this.IsShowing)
			{
				this.Hide();
				return;
			}
			this.Show();
		}

		// Token: 0x06007B08 RID: 31496 RVA: 0x003245B8 File Offset: 0x003227B8
		public void Show()
		{
			if (this.IsShowing || NpcPhotoSessionArchive.Count == 0)
			{
				return;
			}
			if (NewspaperController.Instance != null && NewspaperController.Instance.IsShowing)
			{
				NewspaperController.Instance.Hide();
			}
			if (MatchRecapController.Instance != null && MatchRecapController.Instance.IsShowing)
			{
				MatchRecapController.Instance.Hide();
			}
			this.IsShowing = true;
			this._openedFrame = Time.frameCount;
			this._index = Mathf.Clamp(this._index, 0, NpcPhotoSessionArchive.Count - 1);
			this.Refresh();
			this.UnlockCursor();
			if (this._transition != null)
			{
				base.StopCoroutine(this._transition);
			}
			this._transition = base.StartCoroutine(this.AnimateVisibility(true));
		}

		// Token: 0x06007B09 RID: 31497 RVA: 0x0032467C File Offset: 0x0032287C
		public void Hide()
		{
			if (!this.IsShowing && (this._group == null || this._group.alpha <= 0f))
			{
				return;
			}
			this.IsShowing = false;
			this.RestoreCursor();
			if (this._transition != null)
			{
				base.StopCoroutine(this._transition);
			}
			this._transition = base.StartCoroutine(this.AnimateVisibility(false));
		}

		// Token: 0x06007B0A RID: 31498 RVA: 0x003246E8 File Offset: 0x003228E8
		private void Update()
		{
			if (!this.IsShowing || Time.frameCount == this._openedFrame)
			{
				return;
			}
			if (ControllerWorldPointer.BlocksContextInput)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (isActive != this._lastControllerHints)
			{
				this._lastControllerHints = isActive;
				this.Refresh();
			}
			if (NpcPhotoGalleryController.ReadCloseInput())
			{
				EscapeInputCoordinator.Consume(this);
				ControllerInput.ConsumeUIInput();
				this.Hide();
				return;
			}
			if (ControllerInput.IsActive)
			{
				int num = (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder) ? (-1) : (ControllerInput.GetButtonDown(ControllerButton.RightShoulder) ? 1 : 0));
				if (num != 0)
				{
					this.Move(num);
				}
				else
				{
					if (!ControllerInput.GetButtonDown(ControllerButton.West))
					{
						return;
					}
					this.SaveCurrent();
				}
				ControllerInput.ConsumeUIInput();
				return;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				this.Move(-1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				this.Move(1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				this.SaveCurrent();
			}
		}

		// Token: 0x06007B0B RID: 31499 RVA: 0x003247D3 File Offset: 0x003229D3
		private static bool ReadCloseInput()
		{
			return Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || (ControllerInput.IsActive && (ControllerInput.GetButtonDown(ControllerButton.East) || ControllerInput.GetButtonUp(ControllerButton.Select)));
		}

		// Token: 0x06007B0C RID: 31500 RVA: 0x00324804 File Offset: 0x00322A04
		private void Move(int direction)
		{
			int count = NpcPhotoSessionArchive.Count;
			if (count <= 0)
			{
				return;
			}
			this._index = (this._index + direction + count) % count;
			this.Refresh();
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x00324834 File Offset: 0x00322A34
		private void SaveCurrent()
		{
			string error;
			string text;
			if (NpcPhotoSessionArchive.SaveToPc(this._index, out text, out error))
			{
				this.BindSavedLocation();
				this._status.color = new Color(1f, 0.84f, 0.33f, 1f);
			}
			else
			{
				LocalizedLegacyGameText.BindComputed(this._status, delegate
				{
					if (!string.IsNullOrWhiteSpace(error))
					{
						return GameLocalization.GetSource("rank_ui", error).ToUpperInvariant();
					}
					return GameLocalization.GetSource("rank_ui", "SAVE FAILED");
				});
				this._status.color = new Color(1f, 0.48f, 0.55f, 1f);
			}
			this.RefreshSaveState(false);
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x003248CC File Offset: 0x00322ACC
		private void Refresh()
		{
			int count = NpcPhotoSessionArchive.Count;
			if (count <= 0)
			{
				this._photo.texture = null;
				LocalizedLegacyGameText.BindSource(this._counter, "rank_ui", "0 / 0");
				LocalizedLegacyGameText.BindSource(this._caption, "rank_ui", "NO PHOTOS THIS RUN");
				this._saveButton.interactable = false;
				return;
			}
			this._index = Mathf.Clamp(this._index, 0, count - 1);
			NpcPhotoSessionArchive.Entry entry = NpcPhotoSessionArchive.Get(this._index);
			this._photo.texture = ((entry != null) ? entry.Texture : null);
			LocalizedLegacyGameText.BindFormat(this._counter, "rank_ui", "{0:00} / {1:00}", new object[]
			{
				this._index + 1,
				count
			});
			LocalizedLegacyGameText.BindComputed(this._caption, delegate
			{
				if (entry == null || entry.CaptureCount <= 1)
				{
					return string.Empty;
				}
				return GameLocalization.FormatSource("rank_ui", "{0} NPCs  -  SAME MOMENT", new object[] { entry.CaptureCount });
			});
			LocalizedLegacyGameText.BindComputed(this._status, delegate
			{
				string source = GameLocalization.GetSource("rank_ui", "A / D  BROWSE    -    P  CLOSE");
				if (!ControllerInput.IsActive)
				{
					return source;
				}
				return ControllerGlyphs.Plain(source.Replace("A / D", "LB / RB").Replace("P  ", "B / View  "), ControllerGlyphs.Family);
			});
			this._status.color = new Color(1f, 1f, 1f, 0.68f);
			this.RefreshSaveState(true);
			this._previousButton.interactable = count > 1;
			this._nextButton.interactable = count > 1;
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x00324A38 File Offset: 0x00322C38
		private void RefreshSaveState(bool resetStatus)
		{
			NpcPhotoSessionArchive.Entry entry = NpcPhotoSessionArchive.Get(this._index);
			bool saved = entry != null && entry.IsSaved;
			this._saveButton.interactable = entry != null && !saved;
			LocalizedLegacyGameText.BindComputed(this._saveLabel, delegate
			{
				string source = GameLocalization.GetSource("rank_ui", saved ? "SAVED TO PC" : "SAVE TO PC  [S]");
				if (!ControllerInput.IsActive)
				{
					return source;
				}
				return ControllerGlyphs.Plain(source.Replace("[S]", "[X]"), ControllerGlyphs.Family);
			});
			if (resetStatus & saved)
			{
				this.BindSavedLocation();
				this._status.color = new Color(1f, 0.84f, 0.33f, 1f);
			}
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x00324ACF File Offset: 0x00322CCF
		private void HandleArchiveChanged()
		{
			if (!this.IsShowing)
			{
				return;
			}
			if (NpcPhotoSessionArchive.Count == 0)
			{
				this.Hide();
				return;
			}
			this.Refresh();
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x00324AEE File Offset: 0x00322CEE
		private void OpenExportFolder()
		{
			this._openExportDirectory(NpcPhotoSessionArchive.ExportDirectory);
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x00324B00 File Offset: 0x00322D00
		private void BindSavedLocation()
		{
			LocalizedLegacyGameText.BindComputed(this._status, () => GameLocalization.GetSource("rank_ui", "SAVED TO PC") + "  -  " + NpcPhotoSessionArchive.ExportLocationLabel);
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x00324B2D File Offset: 0x00322D2D
		private IEnumerator AnimateVisibility(bool showing)
		{
			this._group.interactable = showing;
			this._group.blocksRaycasts = showing;
			float startAlpha = this._group.alpha;
			Vector3 startScale = this._panelRect.localScale;
			Vector3 targetScale = (showing ? Vector3.one : (Vector3.one * 0.96f));
			float elapsed = 0f;
			while (elapsed < 0.22f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.22f);
				float num2 = 1f - Mathf.Pow(1f - num, 3f);
				this._group.alpha = Mathf.Lerp(startAlpha, showing ? 1f : 0f, num2);
				this._panelRect.localScale = Vector3.Lerp(startScale, targetScale, num2);
				yield return null;
			}
			this._group.alpha = (showing ? 1f : 0f);
			this._panelRect.localScale = targetScale;
			if (!showing)
			{
				this._group.interactable = false;
				this._group.blocksRaycasts = false;
				this._photo.texture = null;
			}
			this._transition = null;
			yield break;
		}

		// Token: 0x06007B14 RID: 31508 RVA: 0x00324B44 File Offset: 0x00322D44
		private void ResetImmediate()
		{
			this.IsShowing = false;
			this._group.alpha = 0f;
			this._group.interactable = false;
			this._group.blocksRaycasts = false;
			this._panelRect.localScale = Vector3.one * 0.96f;
			this._photo.texture = null;
		}

		// Token: 0x06007B15 RID: 31509 RVA: 0x00324BA6 File Offset: 0x00322DA6
		private void UnlockCursor()
		{
			if (this._cursorStateSaved)
			{
				return;
			}
			this._savedCursorLock = Cursor.lockState;
			this._savedCursorVisible = Cursor.visible;
			this._cursorStateSaved = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// Token: 0x06007B16 RID: 31510 RVA: 0x00324BDA File Offset: 0x00322DDA
		private void RestoreCursor()
		{
			if (!this._cursorStateSaved)
			{
				return;
			}
			Cursor.lockState = this._savedCursorLock;
			Cursor.visible = this._savedCursorVisible;
			this._cursorStateSaved = false;
		}

		// Token: 0x06007B17 RID: 31511 RVA: 0x00324C04 File Offset: 0x00322E04
		private static Image CreateImage(string name, Transform parent, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = true;
			return component;
		}

		// Token: 0x06007B18 RID: 31512 RVA: 0x00324C78 File Offset: 0x00322E78
		private static RawImage CreateRawImage(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			RawImage component = gameObject.GetComponent<RawImage>();
			component.color = Color.white;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06007B19 RID: 31513 RVA: 0x00324CF0 File Offset: 0x00322EF0
		private static Text CreateText(string name, Transform parent, Font font, string value, int size, FontStyle style, Color color, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = font;
			component.text = value;
			component.fontSize = size;
			component.fontStyle = style;
			component.color = color;
			component.alignment = alignment;
			component.horizontalOverflow = HorizontalWrapMode.Overflow;
			component.verticalOverflow = VerticalWrapMode.Overflow;
			component.raycastTarget = false;
			LocalizedLegacyGameText.BindSource(component, "rank_ui", value).FitTranslationToRectangle();
			return component;
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x00324DA8 File Offset: 0x00322FA8
		private static Button CreateButton(string name, Transform parent, Font font, string label, Vector2 size, Color color, Color textColor, int fontSize)
		{
			Image image = NpcPhotoGalleryController.CreateImage(name, parent, color);
			image.rectTransform.sizeDelta = size;
			image.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(10f, 10f, 10f, 10f));
			Button button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1.1f, 1.1f, 1.1f, 1f);
			colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
			colors.disabledColor = new Color(0.42f, 0.42f, 0.42f, 0.68f);
			button.colors = colors;
			NpcPhotoGalleryController.Stretch(NpcPhotoGalleryController.CreateText("Label", image.transform, font, label, fontSize, FontStyle.Bold, textColor, TextAnchor.MiddleCenter).rectTransform, 6f, 6f, 4f, 4f);
			return button;
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x00324EB8 File Offset: 0x003230B8
		private static void Place(RectTransform rect, Vector2 position, Vector2 size, Vector2 pivot)
		{
			rect.anchorMax = pivot;
			rect.anchorMin = pivot;
			rect.pivot = pivot;
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x06007B1C RID: 31516 RVA: 0x00243365 File Offset: 0x00241565
		private static void Stretch(RectTransform rect, float left, float right, float top, float bottom)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(-right, -top);
		}

		// Token: 0x04007E62 RID: 32354
		private static readonly Color PanelColor = new Color(0.43f, 0.2f, 0.51f, 0.99f);

		// Token: 0x04007E63 RID: 32355
		private static readonly Color DarkColor = new Color(0.16f, 0.055f, 0.24f, 0.94f);

		// Token: 0x04007E64 RID: 32356
		private static readonly Color AccentColor = new Color(0.96f, 0.18f, 0.43f, 1f);

		// Token: 0x04007E65 RID: 32357
		private static readonly Color PaperColor = new Color(1f, 0.975f, 0.94f, 1f);

		// Token: 0x04007E68 RID: 32360
		private RectTransform _rootRect;

		// Token: 0x04007E69 RID: 32361
		private RectTransform _panelRect;

		// Token: 0x04007E6A RID: 32362
		private CanvasGroup _group;

		// Token: 0x04007E6B RID: 32363
		private RawImage _photo;

		// Token: 0x04007E6C RID: 32364
		private Text _counter;

		// Token: 0x04007E6D RID: 32365
		private Text _caption;

		// Token: 0x04007E6E RID: 32366
		private Text _status;

		// Token: 0x04007E6F RID: 32367
		private Text _saveLabel;

		// Token: 0x04007E70 RID: 32368
		private Button _saveButton;

		// Token: 0x04007E71 RID: 32369
		private Button _previousButton;

		// Token: 0x04007E72 RID: 32370
		private Button _nextButton;

		// Token: 0x04007E73 RID: 32371
		private Coroutine _transition;

		// Token: 0x04007E74 RID: 32372
		private int _index;

		// Token: 0x04007E75 RID: 32373
		private int _openedFrame = -1;

		// Token: 0x04007E76 RID: 32374
		private Action<string> _openExportDirectory = delegate(string directory)
		{
			PlayerMediaExport.OpenDirectory(directory);
		};

		// Token: 0x04007E77 RID: 32375
		private bool _cursorStateSaved;

		// Token: 0x04007E78 RID: 32376
		private CursorLockMode _savedCursorLock;

		// Token: 0x04007E79 RID: 32377
		private bool _savedCursorVisible;

		// Token: 0x04007E7A RID: 32378
		private bool _lastControllerHints;
	}
}
