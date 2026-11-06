using System;
using System.Collections.Generic;
using System.Globalization;
using Features.CensorBars;
using Features.CielOS.Apps.Story;
using Features.CielOS.UI;
using Features.Inventory;
using Features.Localization;
using Features.MainMenu;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Minimap;
using ProjectExhibitionist.PoiTravel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001466 RID: 5222
	public sealed class CielOSMapsApp : MonoBehaviour
	{
		// Token: 0x0600A124 RID: 41252 RVA: 0x0042BE00 File Offset: 0x0042A000
		private void Awake()
		{
			this.ResolvePalette();
			this._tripSettings = RunModeService.LoadMapsSettings();
			this.BuildUi();
			this.ChooseMapVariant();
			this.SelectDestination(CielOSMapsApp.Destinations[0]);
			PoiFastTravelProgressService.Changed += this.RefreshAll;
			RankProgressionService.Changed += this.RefreshAll;
		}

		// Token: 0x0600A125 RID: 41253 RVA: 0x0042BE59 File Offset: 0x0042A059
		private void OnDestroy()
		{
			PoiFastTravelProgressService.Changed -= this.RefreshAll;
			RankProgressionService.Changed -= this.RefreshAll;
			CielOSStaticCityMap staticCityMap = this._staticCityMap;
			if (staticCityMap == null)
			{
				return;
			}
			staticCityMap.Dispose();
		}

		// Token: 0x0600A126 RID: 41254 RVA: 0x0042BE90 File Offset: 0x0042A090
		private void LateUpdate()
		{
			if (this._mapCanvas == null || this._staticCityMap == null)
			{
				return;
			}
			Vector2 size = this._mapCanvas.rect.size;
			if ((size - this._lastMapCanvasSize).sqrMagnitude < 0.25f)
			{
				return;
			}
			this._lastMapCanvasSize = size;
			this._staticCityMap.RefreshProjection();
			this.PositionMarkers();
		}

		// Token: 0x0600A127 RID: 41255 RVA: 0x0042BEFC File Offset: 0x0042A0FC
		private void ResolvePalette()
		{
			CielOSTheme active = CielOSTheme.Active;
			this._background = ((active != null) ? active.desktopBackground : new Color(0.055f, 0.035f, 0.08f, 1f));
			this._surface = ((active != null) ? active.windowBackground : new Color(0.11f, 0.08f, 0.15f, 1f));
			this._surfaceRaised = ((active != null) ? active.controlBackground : new Color(0.18f, 0.13f, 0.23f, 1f));
			this._text = ((active != null) ? active.controlText : new Color(0.95f, 0.91f, 0.98f, 1f));
			this._muted = new Color(this._text.r, this._text.g, this._text.b, 0.78f);
			this._accent = ((active != null) ? active.accent : new Color(0.663f, 0.376f, 0.788f, 1f));
			this._outline = ((active != null) ? active.outline : new Color(0.78f, 0.62f, 0.9f, 0.35f));
			this._locked = new Color(0.27f, 0.23f, 0.31f, 1f);
		}

		// Token: 0x0600A128 RID: 41256 RVA: 0x0042C080 File Offset: 0x0042A280
		private void ChooseMapVariant()
		{
			int num = Random.Range(0, 5);
			if (num == CielOSMapsApp._lastMapVariant)
			{
				num = (num + 1 + Random.Range(0, 4)) % 5;
			}
			CielOSMapsApp._lastMapVariant = num;
			CielOSStaticCityMap staticCityMap = this._staticCityMap;
			if (staticCityMap != null)
			{
				staticCityMap.SetVariant(num);
			}
			if (this._variantLabel != null)
			{
				LocalizedGameText.BindFormat(this._variantLabel, "maps_ui", "CITY MAP  /  LAYOUT {0:00}", new object[] { num + 1 });
			}
			this.PositionMarkers();
		}

		// Token: 0x0600A129 RID: 41257 RVA: 0x0042C100 File Offset: 0x0042A300
		private void BuildUi()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			Image orAddImage = CielOSMapsApp.GetOrAddImage(base.gameObject);
			orAddImage.color = this._background;
			orAddImage.raycastTarget = true;
			this.BuildBackgroundPattern(rectTransform);
			this.BuildHeader(rectTransform);
			RectTransform rectTransform2 = CielOSMapsApp.NewRect("Body", rectTransform);
			CielOSMapsApp.Stretch(rectTransform2, new Vector2(18f, 18f), new Vector2(-18f, -86f));
			RectTransform rectTransform3 = this.NewPanel("Destination Panel", rectTransform2, this._surface);
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = new Vector2(0.31f, 1f);
			rectTransform3.offsetMin = Vector2.zero;
			rectTransform3.offsetMax = new Vector2(-8f, 0f);
			this.BuildSidebar(rectTransform3);
			RectTransform rectTransform4 = this.NewPanel("Map Panel", rectTransform2, new Color(0.035f, 0.02f, 0.06f, 1f));
			rectTransform4.anchorMin = new Vector2(0.31f, 0f);
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.offsetMin = new Vector2(8f, 0f);
			rectTransform4.offsetMax = Vector2.zero;
			this.BuildMap(rectTransform4);
		}

		// Token: 0x0600A12A RID: 41258 RVA: 0x0042C2A8 File Offset: 0x0042A4A8
		private void BuildBackgroundPattern(RectTransform parent)
		{
			Texture2D texture2D = Resources.Load<Texture2D>("Tutorials/Art/project_exhibitionist_background");
			if (texture2D == null)
			{
				return;
			}
			RectTransform rectTransform = CielOSMapsApp.NewRect("Pattern Background", parent);
			CielOSMapsApp.Stretch(rectTransform, Vector2.zero, Vector2.zero);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform2 = CielOSMapsApp.NewRect("ProjectExPattern", rectTransform);
			CielOSMapsApp.Stretch(rectTransform2, Vector2.zero, Vector2.zero);
			RawImage rawImage = rectTransform2.gameObject.AddComponent<RawImage>();
			rawImage.texture = texture2D;
			rawImage.color = new Color(1f, 1f, 1f, 0.65f);
			rawImage.raycastTarget = false;
			AspectRatioFitter aspectRatioFitter = rectTransform2.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			aspectRatioFitter.aspectRatio = (float)texture2D.width / (float)texture2D.height;
		}

		// Token: 0x0600A12B RID: 41259 RVA: 0x0042C36C File Offset: 0x0042A56C
		private void BuildHeader(RectTransform parent)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect("Header", parent);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(24f, -78f);
			rectTransform.offsetMax = new Vector2(-24f, -12f);
			Image image = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Maps Badge", rectTransform), this._accent);
			CielOSTheme.Round(image);
			RectTransform rectTransform2 = image.rectTransform;
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(50f, 50f);
			CielOSIcon.ApplyColored(CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Glyph", rectTransform2), Color.white), CielOSIconId.Maps, 64);
			CielOSMapsApp.Stretch((RectTransform)image.transform.GetChild(0), new Vector2(7f, 7f), new Vector2(-7f, -7f));
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("MAPS", rectTransform, 24f, FontStyles.Bold, TextAlignmentOptions.BottomLeft, this._text).rectTransform, new Vector2(0f, 0f), new Vector2(0.55f, 1f), new Vector2(66f, 25f), new Vector2(0f, 0f));
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("Instant access to the places you know best.", rectTransform, 12f, FontStyles.Normal, TextAlignmentOptions.TopLeft, this._muted).rectTransform, new Vector2(0f, 0f), new Vector2(0.65f, 1f), new Vector2(66f, -5f), new Vector2(0f, -41f));
			this.BuildTripSettingsButton(rectTransform);
		}

		// Token: 0x0600A12C RID: 41260 RVA: 0x0042C578 File Offset: 0x0042A778
		private void BuildSidebar(RectTransform sidebar)
		{
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("DESTINATIONS", sidebar, 13f, FontStyles.Bold, TextAlignmentOptions.Left, this._text).rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(18f, -48f), new Vector2(-130f, -14f));
			this._readyCountLabel = CielOSMapsApp.AddLabel("0 / 5 READY", sidebar, 10f, FontStyles.Bold, TextAlignmentOptions.Right, this._accent);
			CielOSMapsApp.SetRect(this._readyCountLabel.rectTransform, new Vector2(0.5f, 1f), Vector2.one, new Vector2(0f, -48f), new Vector2(-18f, -14f));
			RectTransform rectTransform = CielOSMapsApp.NewRect("Destination List", sidebar);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(12f, 174f);
			rectTransform.offsetMax = new Vector2(-12f, -58f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(0, 0, 0, 0);
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childForceExpandWidth = true;
			for (int i = 0; i < CielOSMapsApp.Destinations.Length; i++)
			{
				this._rows.Add(this.BuildDestinationRow(rectTransform, CielOSMapsApp.Destinations[i]));
			}
			RectTransform rectTransform2 = this.NewPanel("Selection Detail", sidebar, new Color(this._surfaceRaised.r * 0.82f, this._surfaceRaised.g * 0.82f, this._surfaceRaised.b * 0.82f, 1f));
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.offsetMin = new Vector2(12f, 12f);
			rectTransform2.offsetMax = new Vector2(-12f, 162f);
			this._selectionEyebrow = CielOSMapsApp.AddLabel("SELECTED DESTINATION", rectTransform2, 9f, FontStyles.Bold, TextAlignmentOptions.TopLeft, this._accent);
			CielOSMapsApp.SetRect(this._selectionEyebrow.rectTransform, Vector2.zero, Vector2.one, new Vector2(14f, 104f), new Vector2(-14f, -10f));
			this._selectionTitle = CielOSMapsApp.AddLabel("University", rectTransform2, 18f, FontStyles.Bold, TextAlignmentOptions.TopLeft, this._text);
			CielOSMapsApp.SetRect(this._selectionTitle.rectTransform, Vector2.zero, Vector2.one, new Vector2(14f, 72f), new Vector2(-14f, -34f));
			this._selectionDescription = CielOSMapsApp.AddLabel(string.Empty, rectTransform2, 10.5f, FontStyles.Normal, TextAlignmentOptions.TopLeft, this._muted);
			this._selectionDescription.enableWordWrapping = true;
			CielOSMapsApp.SetRect(this._selectionDescription.rectTransform, Vector2.zero, Vector2.one, new Vector2(14f, 43f), new Vector2(-14f, -66f));
			this._travelButton = this.AddButton("Fast Travel", rectTransform2, out this._travelButtonImage);
			RectTransform rectTransform3 = (RectTransform)this._travelButton.transform;
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.offsetMin = new Vector2(14f, 12f);
			rectTransform3.offsetMax = new Vector2(-14f, 48f);
			this._travelLabel = CielOSMapsApp.AddLabel(string.Empty, rectTransform3, 11f, FontStyles.Bold, TextAlignmentOptions.Center, this._text);
			CielOSMapsApp.Stretch(this._travelLabel.rectTransform, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			this._travelButton.onClick.AddListener(new UnityAction(this.BeginFastTravel));
		}

		// Token: 0x0600A12D RID: 41261 RVA: 0x0042C998 File Offset: 0x0042AB98
		private CielOSMapsApp.DestinationRow BuildDestinationRow(RectTransform parent, CielOSMapsApp.Destination destination)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect(destination.DisplayName + " Row", parent);
			LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 70f;
			layoutElement.preferredHeight = 70f;
			Image image = CielOSMapsApp.AddImage(rectTransform, this._surfaceRaised);
			CielOSTheme.Outline(image, new Color?(this._outline), 1f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			this.ConfigureButtonColors(button);
			CielOSSfx.Attach(button);
			button.onClick.AddListener(delegate
			{
				this.SelectDestination(destination);
			});
			RectTransform rectTransform2 = CielOSMapsApp.NewRect("Icon", rectTransform);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.anchoredPosition = new Vector2(12f, 0f);
			rectTransform2.sizeDelta = new Vector2(42f, 42f);
			CielOSMapsApp.AddImage(rectTransform2, new Color(this._accent.r, this._accent.g, this._accent.b, 0.24f));
			GameObject gameObject = new GameObject("POI Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPoiIconGraphic)
			});
			gameObject.transform.SetParent(rectTransform2, false);
			CielOSMapsApp.Stretch((RectTransform)gameObject.transform, new Vector2(4f, 4f), new Vector2(-4f, -4f));
			ProceduralMiniMapPoiIconGraphic component = gameObject.GetComponent<ProceduralMiniMapPoiIconGraphic>();
			component.SetKind(destination.Kind);
			component.raycastTarget = false;
			TMP_Text tmp_Text = CielOSMapsApp.AddLabel("?", rectTransform2, 25f, FontStyles.Bold, TextAlignmentOptions.Center, new Color(this._muted.r, this._muted.g, this._muted.b, 0.85f));
			CielOSMapsApp.Stretch(tmp_Text.rectTransform, Vector2.zero, Vector2.zero);
			TMP_Text tmp_Text2 = CielOSMapsApp.AddLabel(destination.DisplayName, rectTransform, 13f, FontStyles.Bold, TextAlignmentOptions.TopLeft, this._text);
			CielOSMapsApp.SetRect(tmp_Text2.rectTransform, Vector2.zero, Vector2.one, new Vector2(66f, 28f), new Vector2(-12f, -9f));
			TMP_Text tmp_Text3 = CielOSMapsApp.AddLabel(string.Empty, rectTransform, 9.5f, FontStyles.Bold, TextAlignmentOptions.BottomLeft, this._muted);
			CielOSMapsApp.SetRect(tmp_Text3.rectTransform, Vector2.zero, Vector2.one, new Vector2(66f, 9f), new Vector2(-88f, -35f));
			RectTransform rectTransform5 = CielOSMapsApp.NewRect("Progress Dots", rectTransform);
			RectTransform rectTransform6 = rectTransform5;
			RectTransform rectTransform7 = rectTransform5;
			vector = new Vector2(1f, 0f);
			rectTransform7.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			rectTransform5.pivot = new Vector2(1f, 0f);
			rectTransform5.anchoredPosition = new Vector2(-12f, 13f);
			rectTransform5.sizeDelta = new Vector2(66f, 10f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform5.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 5f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
			Image[] array = new Image[1];
			for (int i = 0; i < array.Length; i++)
			{
				Image image2 = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Discovery " + (i + 1).ToString(), rectTransform5), this._locked);
				image2.rectTransform.sizeDelta = new Vector2(16f, 6f);
				array[i] = image2;
			}
			return new CielOSMapsApp.DestinationRow
			{
				Destination = destination,
				Background = image,
				PoiIcon = component,
				Question = tmp_Text,
				Name = tmp_Text2,
				Progress = tmp_Text3,
				ProgressDots = array
			};
		}

		// Token: 0x0600A12E RID: 41262 RVA: 0x0042CDE0 File Offset: 0x0042AFE0
		private void BuildMap(RectTransform mapPanel)
		{
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("EXPLORE THE CITY", mapPanel, 13f, FontStyles.Bold, TextAlignmentOptions.Left, this._text).rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(18f, -45f), new Vector2(-250f, -12f));
			this._variantLabel = CielOSMapsApp.AddLabel("CITY MAP  /  LAYOUT 01", mapPanel, 9.5f, FontStyles.Bold, TextAlignmentOptions.Right, this._muted);
			CielOSMapsApp.SetRect(this._variantLabel.rectTransform, new Vector2(0.5f, 1f), Vector2.one, new Vector2(0f, -45f), new Vector2(-18f, -12f));
			RectTransform rectTransform = CielOSMapsApp.NewRect("Map Viewport", mapPanel);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(14f, 48f);
			rectTransform.offsetMax = new Vector2(-14f, -54f);
			this._mapCanvas = CielOSMapsApp.NewRect("Map Canvas", rectTransform);
			RectTransform mapCanvas = this._mapCanvas;
			RectTransform mapCanvas2 = this._mapCanvas;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			mapCanvas2.anchorMax = vector;
			mapCanvas.anchorMin = vector;
			this._mapCanvas.pivot = new Vector2(0.5f, 0.5f);
			this._mapCanvas.sizeDelta = new Vector2(900f, 600f);
			AspectRatioFitter aspectRatioFitter = this._mapCanvas.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			aspectRatioFitter.aspectRatio = 1.5f;
			Image image = CielOSMapsApp.AddImage(this._mapCanvas, new Color(0.135f, 0.155f, 0.175f, 1f));
			image.raycastTarget = false;
			CielOSTheme.Outline(image, new Color?(new Color(this._accent.r, this._accent.g, this._accent.b, 0.6f)), 2f);
			GameObject gameObject = new GameObject("Baked Real City Photograph", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject.transform.SetParent(this._mapCanvas, false);
			CielOSMapsApp.Stretch((RectTransform)gameObject.transform, Vector2.zero, Vector2.zero);
			RawImage component = gameObject.GetComponent<RawImage>();
			component.raycastTarget = false;
			this._staticCityMap = new CielOSStaticCityMap(component);
			for (int i = 0; i < CielOSMapsApp.Destinations.Length; i++)
			{
				this._markers.Add(this.BuildMapMarker(this._mapCanvas, CielOSMapsApp.Destinations[i]));
			}
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("CLICK A MARKER OR DESTINATION  •  EXIT RETURNS HOME", mapPanel, 9.5f, FontStyles.Bold, TextAlignmentOptions.Center, this._muted).rectTransform, Vector2.zero, new Vector2(1f, 0f), new Vector2(14f, 12f), new Vector2(-14f, 38f));
		}

		// Token: 0x0600A12F RID: 41263 RVA: 0x0042D0F0 File Offset: 0x0042B2F0
		private CielOSMapsApp.DestinationMarker BuildMapMarker(RectTransform mapCanvas, CielOSMapsApp.Destination destination)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect(destination.DisplayName + " Marker", mapCanvas);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(62f, 62f);
			Image image = CielOSMapsApp.AddImage(rectTransform, new Color(1f, 1f, 1f, 0.002f));
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			this.ConfigureButtonColors(button);
			CielOSSfx.Attach(button);
			button.onClick.AddListener(delegate
			{
				this.SelectDestination(destination);
			});
			GameObject gameObject = new GameObject("Cartographic Target", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(CielOSMapHotspotGraphic)
			});
			gameObject.transform.SetParent(rectTransform, false);
			RectTransform rectTransform4 = (RectTransform)gameObject.transform;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform4.pivot = new Vector2(0.5f, 0.5f);
			rectTransform4.anchoredPosition = Vector2.zero;
			rectTransform4.sizeDelta = new Vector2(56f, 56f);
			CielOSMapHotspotGraphic component = gameObject.GetComponent<CielOSMapHotspotGraphic>();
			component.Configure(false, this._accent);
			component.raycastTarget = false;
			GameObject gameObject2 = new GameObject("POI Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPoiIconGraphic)
			});
			gameObject2.transform.SetParent(rectTransform, false);
			RectTransform rectTransform5 = (RectTransform)gameObject2.transform;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform5.anchorMax = vector;
			rectTransform5.anchorMin = vector;
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			rectTransform5.anchoredPosition = Vector2.zero;
			rectTransform5.sizeDelta = new Vector2(34f, 34f);
			ProceduralMiniMapPoiIconGraphic component2 = gameObject2.GetComponent<ProceduralMiniMapPoiIconGraphic>();
			component2.ConfigureMapMarker(destination.Kind, false, false, 12f);
			component2.raycastTarget = false;
			TMP_Text tmp_Text = CielOSMapsApp.AddLabel("?", rectTransform, 22f, FontStyles.Bold, TextAlignmentOptions.Center, new Color(0.95f, 0.96f, 0.94f, 1f));
			CielOSMapsApp.Stretch(tmp_Text.rectTransform, new Vector2(7f, 6f), new Vector2(-7f, -4f));
			tmp_Text.raycastTarget = false;
			Outline outline = tmp_Text.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0.01f, 0.015f, 0.02f, 0.9f);
			outline.effectDistance = new Vector2(1f, -1f);
			TMP_Text tmp_Text2 = CielOSMapsApp.AddLabel(destination.DisplayName.ToUpperInvariant(), rectTransform, 9.5f, FontStyles.Bold, TextAlignmentOptions.Center, this._text);
			tmp_Text2.enableWordWrapping = false;
			tmp_Text2.overflowMode = TextOverflowModes.Overflow;
			RectTransform rectTransform6 = tmp_Text2.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform6.anchorMax = vector;
			rectTransform6.anchorMin = vector;
			rectTransform6.pivot = new Vector2(0.5f, 1f);
			rectTransform6.anchoredPosition = new Vector2(0f, -5f);
			rectTransform6.sizeDelta = new Vector2(130f, 18f);
			Outline outline2 = tmp_Text2.gameObject.AddComponent<Outline>();
			outline2.effectColor = new Color(0.02f, 0.025f, 0.035f, 0.98f);
			outline2.effectDistance = new Vector2(1.25f, -1.25f);
			return new CielOSMapsApp.DestinationMarker
			{
				Destination = destination,
				Rect = rectTransform,
				Hotspot = component,
				PoiIcon = component2,
				Question = tmp_Text,
				Label = tmp_Text2
			};
		}

		// Token: 0x0600A130 RID: 41264 RVA: 0x0042D514 File Offset: 0x0042B714
		private void PositionMarkers()
		{
			if (this._staticCityMap == null)
			{
				return;
			}
			for (int i = 0; i < this._markers.Count; i++)
			{
				CielOSMapsApp.DestinationMarker destinationMarker = this._markers[i];
				RectTransform rect = destinationMarker.Rect;
				RectTransform rect2 = destinationMarker.Rect;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rect2.anchorMax = vector;
				rect.anchorMin = vector;
				destinationMarker.Rect.anchoredPosition = this._staticCityMap.GetPoiLocalPosition(destinationMarker.Destination.Kind);
			}
		}

		// Token: 0x0600A131 RID: 41265 RVA: 0x0042D597 File Offset: 0x0042B797
		private void SelectDestination(CielOSMapsApp.Destination destination)
		{
			this._selected = destination;
			this.RefreshAll();
		}

		// Token: 0x0600A132 RID: 41266 RVA: 0x0042D5A8 File Offset: 0x0042B7A8
		private void RefreshAll()
		{
			int num = 0;
			for (int i = 0; i < CielOSMapsApp.Destinations.Length; i++)
			{
				if (PoiFastTravelProgressService.IsDestinationUnlocked(CielOSMapsApp.Destinations[i].SceneName))
				{
					num++;
				}
			}
			LocalizedGameText.BindFormat(this._readyCountLabel, "maps_ui", "{0} / {1} READY", new object[]
			{
				num,
				CielOSMapsApp.Destinations.Length
			});
			for (int j = 0; j < this._rows.Count; j++)
			{
				CielOSMapsApp.DestinationRow destinationRow = this._rows[j];
				int visitCount = PoiFastTravelProgressService.GetVisitCount(destinationRow.Destination.SceneName);
				bool flag = PoiFastTravelProgressService.IsDestinationUnlocked(destinationRow.Destination.SceneName);
				bool flag2 = destinationRow.Destination == this._selected;
				destinationRow.Background.color = (flag2 ? new Color(this._accent.r, this._accent.g, this._accent.b, flag ? 0.34f : 0.2f) : this._surfaceRaised);
				destinationRow.PoiIcon.enabled = flag;
				destinationRow.Question.enabled = !flag;
				LocalizedGameText.BindSource(destinationRow.Name, "maps_ui", flag ? destinationRow.Destination.DisplayName : "???", null);
				destinationRow.Name.color = (flag ? this._text : this._muted);
				if (flag)
				{
					LocalizedGameText.BindSource(destinationRow.Progress, "maps_ui", "FAST TRAVEL READY", null);
				}
				else
				{
					LocalizedGameText.BindFormat(destinationRow.Progress, "maps_ui", "DISCOVERY  {0} / {1}", new object[] { visitCount, 1 });
				}
				destinationRow.Progress.color = (flag ? new Color(0.48f, 0.92f, 0.76f, 1f) : this._muted);
				for (int k = 0; k < destinationRow.ProgressDots.Length; k++)
				{
					destinationRow.ProgressDots[k].color = ((flag || k < visitCount) ? this._accent : this._locked);
				}
			}
			for (int l = 0; l < this._markers.Count; l++)
			{
				CielOSMapsApp.DestinationMarker destinationMarker = this._markers[l];
				bool flag3 = PoiFastTravelProgressService.IsDestinationUnlocked(destinationMarker.Destination.SceneName);
				bool flag4 = destinationMarker.Destination == this._selected;
				destinationMarker.Hotspot.Configure(flag4, this._accent);
				destinationMarker.PoiIcon.enabled = flag3;
				destinationMarker.PoiIcon.ConfigureMapMarker(destinationMarker.Destination.Kind, false, false, 12f);
				destinationMarker.Question.enabled = !flag3;
				LocalizedGameText.BindSource(destinationMarker.Label, "maps_ui", destinationMarker.Destination.DisplayName, (string value) => value.ToUpperInvariant());
				destinationMarker.Label.color = new Color(1f, 0.95f, 1f, 1f);
				destinationMarker.Label.enabled = flag4 && flag3;
				destinationMarker.Rect.localScale = Vector3.one;
			}
			this.RefreshSelection();
		}

		// Token: 0x0600A133 RID: 41267 RVA: 0x0042D908 File Offset: 0x0042BB08
		private void RefreshSelection()
		{
			if (this._selected == null || this._selectionTitle == null)
			{
				return;
			}
			int visitCount = PoiFastTravelProgressService.GetVisitCount(this._selected.SceneName);
			bool flag = PoiFastTravelProgressService.IsDestinationUnlocked(this._selected.SceneName);
			LocalizedGameText.BindSource(this._selectionEyebrow, "maps_ui", flag ? this._selected.District : "UNDISCOVERED DESTINATION", null);
			this._selectionEyebrow.color = (flag ? this._accent : this._muted);
			LocalizedGameText.BindSource(this._selectionTitle, "maps_ui", flag ? this._selected.DisplayName : "???", null);
			this._selectionTitle.color = (flag ? this._text : this._muted);
			if (flag)
			{
				LocalizedGameText.BindSource(this._selectionDescription, "maps_ui", "Travel now. The POI exit will bring Ciel straight back home.", null);
			}
			else
			{
				int num = 1 - visitCount;
				LocalizedGameText.BindFormat(this._selectionDescription, "maps_ui", (num == 1) ? "Find this place in the procedural city {0} more time to unlock it." : "Find this place in the procedural city {0} more times to unlock it.", new object[] { num });
			}
			this._travelButton.interactable = flag;
			this._travelButtonImage.color = (flag ? this._accent : this._locked);
			if (flag)
			{
				LocalizedGameText.BindSource(this._travelLabel, "maps_ui", "FAST TRAVEL", null);
				return;
			}
			LocalizedGameText.BindFormat(this._travelLabel, "maps_ui", "LOCKED  {0} / {1}", new object[] { visitCount, 1 });
		}

		// Token: 0x0600A134 RID: 41268 RVA: 0x0042DA98 File Offset: 0x0042BC98
		private void BeginFastTravel()
		{
			if (RunModeService.IsLongWayHome)
			{
				this.ShowTravelError("RETURN HOME ON FOOT");
				return;
			}
			if (this._selected == null || !PoiFastTravelProgressService.IsDestinationUnlocked(this._selected.SceneName))
			{
				return;
			}
			if (!CielOSMapsApp.IsRequiredRankReached())
			{
				this.ShowTravelError("RANK REQUIRED");
				return;
			}
			string text;
			if (!CielOSStoryFlow.BeforeStartRun(out text))
			{
				this.ShowTravelError("READ THE NEW STORY CHAPTER FIRST");
				return;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance == null || instance.IsTransitioning)
			{
				this.ShowTravelError("TRAVEL SYSTEM BUSY");
				return;
			}
			string sceneName = this._selected.SceneName;
			if (!Application.CanStreamedLevelBeLoaded(sceneName))
			{
				this.ShowTravelError("DESTINATION OFFLINE");
				return;
			}
			GameObject gameObject = PlayerRegistry.PlayerObject;
			if (gameObject == null)
			{
				gameObject = GameObject.FindGameObjectWithTag("Player");
			}
			if (gameObject == null)
			{
				this.ShowTravelError("PLAYER NOT READY");
				return;
			}
			string text2 = instance.CurrentGameplayScene;
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = SceneManager.GetActiveScene().name;
			}
			if (!string.Equals(text2, "CielHome", StringComparison.Ordinal))
			{
				this.ShowTravelError("AVAILABLE FROM HOME ONLY");
				return;
			}
			string text3;
			if (!RunModeService.TryPrepareMapsVisit(this._tripSettings, out text3))
			{
				this.ShowTravelError(text3);
				return;
			}
			PoiTravelState.BeginVisit(text2, gameObject.transform.position, gameObject.transform.rotation, PoiTravelOrigin.HomeFastTravel, null, null);
			if (this._travelButton != null)
			{
				this._travelButton.interactable = false;
			}
			LocalizedGameText.BindSource(this._travelLabel, "maps_ui", "OPENING DESTINATION...", null);
			CielOSController instance2 = CielOSController.Instance;
			if (instance2 != null)
			{
				instance2.Close();
			}
			instance.LoadGameplayScene(sceneName);
		}

		// Token: 0x0600A135 RID: 41269 RVA: 0x0042DC24 File Offset: 0x0042BE24
		private static bool IsRequiredRankReached()
		{
			if (RankProgressionService.IsAdminRankActive)
			{
				return true;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null)
			{
				return true;
			}
			int num = -1;
			for (int i = 0; i < config.ranksInOrder.Count; i++)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
				if (rankDefinitionSO != null && rankDefinitionSO.rankId == "hornyslut_declared")
				{
					num = i;
					break;
				}
			}
			return num < 0 || RankProgressionService.RankIndex >= num;
		}

		// Token: 0x0600A136 RID: 41270 RVA: 0x0042DCA8 File Offset: 0x0042BEA8
		private void ShowTravelError(string message)
		{
			LocalizedGameText.BindSource(this._travelLabel, "maps_ui", message, null);
		}

		// Token: 0x0600A137 RID: 41271 RVA: 0x0042DCC0 File Offset: 0x0042BEC0
		private Button AddButton(string name, Transform parent, out Image image)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect(name, parent);
			image = CielOSMapsApp.AddImage(rectTransform, this._accent);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			this.ConfigureButtonColors(button);
			CielOSSfx.Attach(button);
			return button;
		}

		// Token: 0x0600A138 RID: 41272 RVA: 0x0042DD08 File Offset: 0x0042BF08
		private void ConfigureButtonColors(Button button)
		{
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1.12f, 1.12f, 1.12f, 1f);
			colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
			colors.selectedColor = Color.white;
			colors.disabledColor = new Color(0.62f, 0.62f, 0.62f, 0.72f);
			colors.fadeDuration = 0.08f;
			button.colors = colors;
		}

		// Token: 0x0600A139 RID: 41273 RVA: 0x0042DDA7 File Offset: 0x0042BFA7
		private RectTransform NewPanel(string name, Transform parent, Color color)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect(name, parent);
			CielOSTheme.Outline(CielOSMapsApp.AddImage(rectTransform, color), new Color?(this._outline), 1f);
			return rectTransform;
		}

		// Token: 0x0600A13A RID: 41274 RVA: 0x003E80BB File Offset: 0x003E62BB
		private static RectTransform NewRect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x0600A13B RID: 41275 RVA: 0x0015358E File Offset: 0x0015178E
		private static Image AddImage(RectTransform rect, Color color)
		{
			Image image = rect.gameObject.AddComponent<Image>();
			image.color = color;
			return image;
		}

		// Token: 0x0600A13C RID: 41276 RVA: 0x0042DDCC File Offset: 0x0042BFCC
		private static Image GetOrAddImage(GameObject target)
		{
			Image component = target.GetComponent<Image>();
			if (!(component != null))
			{
				return target.AddComponent<Image>();
			}
			return component;
		}

		// Token: 0x0600A13D RID: 41277 RVA: 0x0042DDF4 File Offset: 0x0042BFF4
		private static TMP_Text AddLabel(string text, Transform parent, float size, FontStyles style, TextAlignmentOptions alignment, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = CielOSMapsApp.NewRect(text, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.color = color;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.enableWordWrapping = false;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				textMeshProUGUI.font = active.uiFont;
			}
			LocalizedGameText.BindSource(textMeshProUGUI, "maps_ui", text, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x0600A13E RID: 41278 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x0600A13F RID: 41279 RVA: 0x0042DE83 File Offset: 0x0042C083
		private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
		{
			CielOSMapsApp.SetRect(rect, Vector2.zero, Vector2.one, offsetMin, offsetMax);
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x0600A140 RID: 41280 RVA: 0x0042DE97 File Offset: 0x0042C097
		private Color TripHighlight
		{
			get
			{
				return Color.Lerp(this._accent, Color.white, 0.62f);
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x0600A141 RID: 41281 RVA: 0x0042DEAE File Offset: 0x0042C0AE
		private static Color TripInk
		{
			get
			{
				return new Color(0.14f, 0.075f, 0.2f, 1f);
			}
		}

		// Token: 0x0600A142 RID: 41282 RVA: 0x0042DEC9 File Offset: 0x0042C0C9
		private static Color WithAlpha(Color color, float alpha)
		{
			color.a = alpha;
			return color;
		}

		// Token: 0x0600A143 RID: 41283 RVA: 0x0042DED4 File Offset: 0x0042C0D4
		private RectTransform TripPanel(string name, Transform parent, Color color)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect(name, parent);
			CielOSTheme.Round(CielOSMapsApp.AddImage(rectTransform, color));
			return rectTransform;
		}

		// Token: 0x0600A144 RID: 41284 RVA: 0x0042DEEC File Offset: 0x0042C0EC
		private void BuildTripSettingsButton(RectTransform header)
		{
			Image image;
			Button button = this.AddButton("Trip Settings", header, out image);
			image.color = this.TripHighlight;
			CielOSTheme.Round(image);
			CielOSTheme.Outline(image, new Color?(CielOSMapsApp.WithAlpha(Color.white, 0.35f)), 1f);
			RectTransform rectTransform = (RectTransform)button.transform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(1f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(1f, 0.5f);
			rectTransform.sizeDelta = new Vector2(264f, 54f);
			Image image2 = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Icon Badge", rectTransform), CielOSMapsApp.WithAlpha(CielOSMapsApp.TripInk, 0.1f));
			CielOSTheme.Round(image2);
			CielOSMapsApp.SetRect(image2.rectTransform, Vector2.zero, Vector2.up, new Vector2(8f, 8f), new Vector2(46f, -8f));
			image2.raycastTarget = false;
			this._tripButtonIcon = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Settings Icon", image2.rectTransform), CielOSMapsApp.TripInk);
			CielOSIcon.Apply(this._tripButtonIcon, CielOSIconId.Settings, CielOSMapsApp.TripInk, 64);
			CielOSMapsApp.Stretch(this._tripButtonIcon.rectTransform, new Vector2(8f, 8f), new Vector2(-8f, -8f));
			this._tripButtonIcon.raycastTarget = false;
			this._tripButtonLabel = CielOSMapsApp.AddLabel("TRIP SETTINGS", rectTransform, 17f, FontStyles.Bold, TextAlignmentOptions.Center, CielOSMapsApp.TripInk);
			CielOSMapsApp.Stretch(this._tripButtonLabel.rectTransform, new Vector2(56f, 0f), new Vector2(-16f, 0f));
			button.onClick.AddListener(delegate
			{
				this.SetTripSettingsOpen(!this._tripOpen);
			});
		}

		// Token: 0x0600A145 RID: 41285 RVA: 0x0042E0C0 File Offset: 0x0042C2C0
		private void SetTripSettingsOpen(bool open)
		{
			this._tripOpen = open;
			Transform transform = base.transform.Find("Body");
			if (transform != null)
			{
				transform.gameObject.SetActive(!open);
			}
			LocalizedGameText.BindSource(this._tripButtonLabel, "maps_ui", open ? "BACK TO MAP" : "TRIP SETTINGS", null);
			CielOSIcon.Apply(this._tripButtonIcon, open ? CielOSIconId.Maps : CielOSIconId.Settings, CielOSMapsApp.TripInk, 64);
			if (open)
			{
				this.BuildTripSettings();
				return;
			}
			this.DestroyTripView();
			this.RefreshSelection();
		}

		// Token: 0x0600A146 RID: 41286 RVA: 0x0042E14C File Offset: 0x0042C34C
		private void DestroyTripView()
		{
			if (this._tripRoot == null)
			{
				return;
			}
			this._tripRoot.gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(this._tripRoot.gameObject);
			}
			else
			{
				Object.DestroyImmediate(this._tripRoot.gameObject);
			}
			this._tripRoot = null;
			this._tripList = null;
			this._tripPicker = null;
			this._tripScroll = null;
		}

		// Token: 0x0600A147 RID: 41287 RVA: 0x0042E1C0 File Offset: 0x0042C3C0
		private void BuildTripSettings()
		{
			if (this._tripScroll != null)
			{
				this._tripScrollPositions[this._tripPage] = this._tripScroll.verticalNormalizedPosition;
			}
			this.DestroyTripView();
			this._tripRoot = CielOSMapsApp.NewRect("Trip Configuration", base.transform);
			CielOSMapsApp.Stretch(this._tripRoot, new Vector2(18f, 18f), new Vector2(-18f, -86f));
			RectTransform rectTransform = this.TripPanel("Trip Categories", this._tripRoot, CielOSMapsApp.WithAlpha(this._surface, 0.54f));
			CielOSMapsApp.SetRect(rectTransform, Vector2.zero, Vector2.up, Vector2.zero, new Vector2(238f, 0f));
			CielOSMapsApp.SetRect(CielOSMapsApp.AddLabel("MAKE IT YOURS", rectTransform, 15f, FontStyles.Bold, TextAlignmentOptions.Left, this.TripHighlight).rectTransform, Vector2.up, Vector2.one, new Vector2(22f, -58f), new Vector2(-18f, -18f));
			this.TripTab(rectTransform, CielOSMapsApp.TripPage.Start, "GETTING READY", CielOSIconId.Avatar, 0);
			this.TripTab(rectTransform, CielOSMapsApp.TripPage.World, "WORLD", CielOSIconId.Weather, 1);
			this.TripTab(rectTransform, CielOSMapsApp.TripPage.Rules, "RULES", CielOSIconId.Settings, 2);
			this.TripTab(rectTransform, CielOSMapsApp.TripPage.BodyPaint, "BODYPAINT", CielOSIconId.Paint, 3);
			this.TripTab(rectTransform, CielOSMapsApp.TripPage.Inventory, "INVENTORY", CielOSIconId.Files, 4);
			TMP_Text tmp_Text = CielOSMapsApp.AddLabel("Your next visit, your rules.\nSaved automatically for Maps.", rectTransform, 13f, FontStyles.Normal, TextAlignmentOptions.BottomLeft, this._muted);
			tmp_Text.enableWordWrapping = true;
			CielOSMapsApp.SetRect(tmp_Text.rectTransform, Vector2.zero, Vector2.right, new Vector2(22f, 24f), new Vector2(-22f, 84f));
			RectTransform rectTransform2 = this.TripPanel("Trip Settings Panel", this._tripRoot, CielOSMapsApp.WithAlpha(this._surface, 0.78f));
			CielOSMapsApp.Stretch(rectTransform2, new Vector2(254f, 0f), Vector2.zero);
			string text = ((this._tripPage == CielOSMapsApp.TripPage.Start) ? "GETTING READY" : ((this._tripPage == CielOSMapsApp.TripPage.BodyPaint) ? "BODYPAINT" : this._tripPage.ToString().ToUpperInvariant()));
			Image image = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Section Badge", rectTransform2), CielOSMapsApp.WithAlpha(this._accent, 0.24f));
			CielOSTheme.Round(image);
			CielOSMapsApp.SetRect(image.rectTransform, Vector2.up, Vector2.up, new Vector2(24f, -77f), new Vector2(80f, -21f));
			image.raycastTarget = false;
			Image image2 = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Icon", image.rectTransform), this.TripHighlight);
			CielOSIconId cielOSIconId = ((this._tripPage == CielOSMapsApp.TripPage.Start) ? CielOSIconId.Avatar : ((this._tripPage == CielOSMapsApp.TripPage.World) ? CielOSIconId.Weather : ((this._tripPage == CielOSMapsApp.TripPage.BodyPaint) ? CielOSIconId.Paint : ((this._tripPage == CielOSMapsApp.TripPage.Inventory) ? CielOSIconId.Files : CielOSIconId.Settings))));
			CielOSIcon.Apply(image2, cielOSIconId, this.TripHighlight, 64);
			CielOSMapsApp.Stretch(image2.rectTransform, new Vector2(14f, 14f), new Vector2(-14f, -14f));
			image2.raycastTarget = false;
			CielOSMapsApp.SetRect(this.TripLabel(text, rectTransform2, 26f, FontStyles.Bold, this._text).rectTransform, Vector2.up, Vector2.one, new Vector2(96f, -60f), new Vector2(-184f, -21f));
			TMP_Text tmp_Text2 = CielOSMapsApp.AddLabel("Applies to every destination. Returning home restores Ciel's saved look.", rectTransform2, 13f, FontStyles.Normal, TextAlignmentOptions.Left, this._muted);
			CielOSMapsApp.SetRect(tmp_Text2.rectTransform, Vector2.up, Vector2.one, new Vector2(96f, -94f), new Vector2(-24f, -60f));
			tmp_Text2.enableWordWrapping = true;
			Button button = this.TripAction("Reset Page", rectTransform2, "RESET PAGE", false);
			CielOSMapsApp.SetRect((RectTransform)button.transform, Vector2.one, Vector2.one, new Vector2(-160f, -61f), new Vector2(-24f, -23f));
			button.onClick.AddListener(new UnityAction(this.ResetTripPage));
			RectTransform rectTransform3 = CielOSMapsApp.NewRect("Settings Viewport", rectTransform2);
			CielOSMapsApp.Stretch(rectTransform3, new Vector2(24f, 24f), new Vector2(-34f, -118f));
			CielOSMapsApp.AddImage(rectTransform3, Color.clear);
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			this._tripList = CielOSMapsApp.NewRect("Settings List", rectTransform3);
			this._tripList.anchorMin = Vector2.up;
			this._tripList.anchorMax = Vector2.one;
			this._tripList.pivot = Vector2.up;
			this._tripList.anchoredPosition = Vector2.zero;
			this._tripList.sizeDelta = Vector2.zero;
			this._tripRowY = 0f;
			switch (this._tripPage)
			{
			case CielOSMapsApp.TripPage.Start:
				this.BuildTripStart();
				break;
			case CielOSMapsApp.TripPage.World:
				this.BuildTripWorld();
				break;
			case CielOSMapsApp.TripPage.Rules:
				this.BuildTripRules();
				break;
			case CielOSMapsApp.TripPage.BodyPaint:
				this.BuildTripPaint();
				break;
			case CielOSMapsApp.TripPage.Inventory:
				this.BuildTripInventory();
				break;
			}
			this._tripList.sizeDelta = new Vector2(0f, Mathf.Max(0f, this._tripRowY - 12f));
			this._tripScroll = rectTransform3.gameObject.AddComponent<ScrollRect>();
			this._tripScroll.viewport = rectTransform3;
			this._tripScroll.content = this._tripList;
			this._tripScroll.horizontal = false;
			this._tripScroll.movementType = ScrollRect.MovementType.Clamped;
			this._tripScroll.scrollSensitivity = 34f;
			rectTransform3.gameObject.AddComponent<SmoothCollectionScroll>().Configure(48f, 0.16f);
			RectTransform rectTransform4 = CielOSMapsApp.NewRect("Scroll Track", rectTransform2);
			CielOSMapsApp.SetRect(rectTransform4, Vector2.right, Vector2.one, new Vector2(-16f, 24f), new Vector2(-10f, -118f));
			CielOSMapsApp.AddImage(rectTransform4, this._background);
			RectTransform rectTransform5 = CielOSMapsApp.NewRect("Handle", rectTransform4);
			CielOSMapsApp.Stretch(rectTransform5, Vector2.zero, Vector2.zero);
			Image image3 = CielOSMapsApp.AddImage(rectTransform5, this._accent);
			CielOSTheme.Round(image3);
			Scrollbar scrollbar = rectTransform4.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.handleRect = rectTransform5;
			scrollbar.targetGraphic = image3;
			this._tripScroll.verticalScrollbar = scrollbar;
			this._tripScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			float num;
			this._tripScroll.verticalNormalizedPosition = (this._tripScrollPositions.TryGetValue(this._tripPage, out num) ? num : 1f);
		}

		// Token: 0x0600A148 RID: 41288 RVA: 0x0042E85C File Offset: 0x0042CA5C
		private void TripTab(RectTransform rail, CielOSMapsApp.TripPage page, string title, CielOSIconId glyph, int index)
		{
			Image image;
			Button button = this.AddButton("Category " + page.ToString(), rail, out image);
			bool flag = this._tripPage == page;
			image.color = (flag ? this.TripHighlight : CielOSMapsApp.WithAlpha(this._surfaceRaised, 0.58f));
			CielOSTheme.Round(image);
			RectTransform rectTransform = (RectTransform)button.transform;
			CielOSMapsApp.SetRect(rectTransform, Vector2.up, Vector2.one, new Vector2(12f, -128f - (float)index * 64f), new Vector2(-12f, -74f - (float)index * 64f));
			Image image2 = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Icon", rectTransform), flag ? CielOSMapsApp.TripInk : this.TripHighlight);
			CielOSIcon.Apply(image2, glyph, image2.color, 64);
			CielOSMapsApp.SetRect(image2.rectTransform, Vector2.zero, Vector2.up, new Vector2(16f, 15f), new Vector2(40f, -15f));
			image2.raycastTarget = false;
			CielOSMapsApp.Stretch(this.TripLabel(title, rectTransform, 14f, FontStyles.Bold, flag ? CielOSMapsApp.TripInk : this._text).rectTransform, new Vector2(54f, 0f), new Vector2(-12f, 0f));
			button.onClick.AddListener(delegate
			{
				if (this._tripScroll != null)
				{
					this._tripScrollPositions[this._tripPage] = this._tripScroll.verticalNormalizedPosition;
				}
				this._tripScroll = null;
				this._tripPage = page;
				this.BuildTripSettings();
			});
		}

		// Token: 0x0600A149 RID: 41289 RVA: 0x0042E9F1 File Offset: 0x0042CBF1
		private TMP_Text TripLabel(string english, Transform parent, float size, FontStyles style, Color color)
		{
			TMP_Text tmp_Text = CielOSMapsApp.AddLabel(english, parent, size, style, TextAlignmentOptions.Left, color);
			LocalizedGameText.BindSource(tmp_Text, "cielos_settings", english, null).FitTranslationToRectangle(false);
			return tmp_Text;
		}

		// Token: 0x0600A14A RID: 41290 RVA: 0x0042EA18 File Offset: 0x0042CC18
		private Button TripAction(string name, Transform parent, string title, bool primary)
		{
			Image image;
			Button button = this.AddButton(name, parent, out image);
			image.color = (primary ? this.TripHighlight : this._surfaceRaised);
			CielOSTheme.Round(image);
			TMP_Text tmp_Text = this.TripLabel(title, button.transform, 13f, FontStyles.Bold, primary ? CielOSMapsApp.TripInk : this._text);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			CielOSMapsApp.Stretch(tmp_Text.rectTransform, new Vector2(8f, 0f), new Vector2(-8f, 0f));
			return button;
		}

		// Token: 0x0600A14B RID: 41291 RVA: 0x0042EAA8 File Offset: 0x0042CCA8
		private RectTransform TripRow(string name, string title, string description, float height = 110f)
		{
			RectTransform rectTransform = this.NewPanel(name, this._tripList, this._surfaceRaised);
			CielOSTheme.Outline(rectTransform.GetComponent<Image>(), new Color?(CielOSMapsApp.WithAlpha(this._outline, 0.18f)), 1f);
			CielOSTheme.Round(rectTransform.GetComponent<Image>());
			CielOSMapsApp.SetRect(rectTransform, Vector2.up, Vector2.one, new Vector2(0f, -this._tripRowY - height), new Vector2(0f, -this._tripRowY));
			this._tripRowY += height + 12f;
			CielOSMapsApp.SetRect(this.TripLabel(title, rectTransform, 16f, FontStyles.Bold, this._text).rectTransform, Vector2.zero, new Vector2(0.44f, 1f), new Vector2(18f, height - 44f), new Vector2(-12f, -13f));
			TMP_Text tmp_Text = this.TripLabel(description, rectTransform, 13f, FontStyles.Normal, this._muted);
			tmp_Text.enableWordWrapping = true;
			CielOSMapsApp.SetRect(tmp_Text.rectTransform, Vector2.zero, new Vector2(0.44f, 1f), new Vector2(18f, 10f), new Vector2(-12f, -42f));
			return rectTransform;
		}

		// Token: 0x0600A14C RID: 41292 RVA: 0x0042EBF0 File Offset: 0x0042CDF0
		private RectTransform TripControl(RectTransform row)
		{
			RectTransform rectTransform = CielOSMapsApp.NewRect("Control", row);
			CielOSMapsApp.SetRect(rectTransform, new Vector2(0.45f, 0.5f), new Vector2(1f, 0.5f), new Vector2(4f, -24f), new Vector2(-16f, 24f));
			return rectTransform;
		}

		// Token: 0x0600A14D RID: 41293 RVA: 0x0042EC4C File Offset: 0x0042CE4C
		private void TripChoices(RectTransform row, string[] labels, int selected, Action<int> choose)
		{
			RectTransform rectTransform = this.TripControl(row);
			for (int i = 0; i < labels.Length; i++)
			{
				int index = i;
				Button button = this.TripAction("Choice " + i.ToString(), rectTransform, labels[i], selected == i);
				CielOSMapsApp.SetRect((RectTransform)button.transform, new Vector2((float)i / (float)labels.Length, 0f), new Vector2(((float)i + 1f) / (float)labels.Length, 1f), new Vector2(2f, 0f), new Vector2(-2f, 0f));
				if (selected != i)
				{
					button.targetGraphic.color = this._background;
				}
				button.onClick.AddListener(delegate
				{
					choose(index);
					this.SaveTripSettings(true);
				});
			}
		}

		// Token: 0x0600A14E RID: 41294 RVA: 0x0042ED44 File Offset: 0x0042CF44
		private void TripSlider(RectTransform row, float value, float min, float max, float step, bool enabled, Action<float> change, Func<float, string> format)
		{
			RectTransform rectTransform = this.TripControl(row);
			TMP_Text number = this.TripLabel(format(value), rectTransform, 15f, FontStyles.Bold, enabled ? this._text : this._muted);
			number.alignment = TextAlignmentOptions.Right;
			CielOSMapsApp.SetRect(number.rectTransform, Vector2.zero, Vector2.one, new Vector2(0f, 22f), Vector2.zero);
			RectTransform rectTransform2 = CielOSMapsApp.NewRect("Slider", rectTransform);
			CielOSMapsApp.SetRect(rectTransform2, Vector2.zero, Vector2.right, new Vector2(8f, 0f), new Vector2(-8f, 22f));
			RectTransform rectTransform3 = CielOSMapsApp.NewRect("Track", rectTransform2);
			CielOSMapsApp.SetRect(rectTransform3, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, -3f), new Vector2(0f, 3f));
			CielOSMapsApp.AddImage(rectTransform3, this._background);
			RectTransform rectTransform4 = CielOSMapsApp.NewRect("Fill", rectTransform3);
			CielOSMapsApp.Stretch(rectTransform4, Vector2.zero, Vector2.zero);
			CielOSMapsApp.AddImage(rectTransform4, enabled ? this._accent : this._muted);
			RectTransform rectTransform5 = CielOSMapsApp.NewRect("Handle", rectTransform2);
			rectTransform5.sizeDelta = new Vector2(14f, 22f);
			Image image = CielOSMapsApp.AddImage(rectTransform5, enabled ? this._text : this._muted);
			CielOSTheme.Round(image);
			Slider slider = rectTransform2.gameObject.AddComponent<Slider>();
			slider.fillRect = rectTransform4;
			slider.handleRect = rectTransform5;
			slider.targetGraphic = image;
			slider.minValue = 0f;
			slider.maxValue = Mathf.Round((max - min) / step);
			slider.wholeNumbers = true;
			slider.interactable = enabled;
			slider.SetValueWithoutNotify(Mathf.Round((value - min) / step));
			slider.onValueChanged.AddListener(delegate(float raw)
			{
				float num = min + raw * step;
				change(num);
				LocalizedGameText.BindSource(number, "cielos_settings", format(num), null);
				this.SaveTripSettings(false);
			});
		}

		// Token: 0x0600A14F RID: 41295 RVA: 0x0042EF90 File Offset: 0x0042D190
		private void TripSelector(RectTransform row, string[] labels, int selected, Action<int> choose, bool enabled = true)
		{
			RectTransform rectTransform = this.TripControl(row);
			TMP_Text tmp_Text = this.TripLabel(labels[Mathf.Clamp(selected, 0, labels.Length - 1)], rectTransform, 14f, FontStyles.Bold, enabled ? this._text : this._muted);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			CielOSMapsApp.Stretch(tmp_Text.rectTransform, new Vector2(48f, 0f), new Vector2(-48f, 0f));
			Button button = this.TripAction("Previous", rectTransform, "<", false);
			CielOSMapsApp.SetRect((RectTransform)button.transform, Vector2.zero, Vector2.up, Vector2.zero, new Vector2(40f, 0f));
			Button button2 = this.TripAction("Next", rectTransform, ">", false);
			CielOSMapsApp.SetRect((RectTransform)button2.transform, Vector2.right, Vector2.one, new Vector2(-40f, 0f), Vector2.zero);
			button.interactable = (button2.interactable = enabled);
			button.onClick.AddListener(delegate
			{
				if (enabled)
				{
					choose((selected - 1 + labels.Length) % labels.Length);
					this.SaveTripSettings(true);
				}
			});
			button2.onClick.AddListener(delegate
			{
				if (enabled)
				{
					choose((selected + 1) % labels.Length);
					this.SaveTripSettings(true);
				}
			});
		}

		// Token: 0x0600A150 RID: 41296 RVA: 0x0042F106 File Offset: 0x0042D306
		private void SaveTripSettings(bool rebuild)
		{
			RunModeService.SaveMapsSettings(this._tripSettings);
			if (rebuild)
			{
				this.BuildTripSettings();
			}
		}

		// Token: 0x0600A151 RID: 41297 RVA: 0x0042F11C File Offset: 0x0042D31C
		private void BuildTripStart()
		{
			this.TripChoices(this.TripRow("Outfit", "CURRENT OUTFIT", "Wear what Ciel has on, or leave her clothes at home.", 110f), new string[] { "WEAR IT", "LEAVE IT" }, this._tripSettings.usePresetOutfit ? 0 : 1, delegate(int i)
			{
				this._tripSettings.usePresetOutfit = i == 0;
			});
			string[] array = new string[24];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = j.ToString("00") + ":00";
			}
			this.TripSelector(this.TripRow("Departure Time", "TIME TO LEAVE", "An early start or a late night?", 110f), array, this._tripSettings.startHour, delegate(int i)
			{
				this._tripSettings.startHour = i;
			}, true);
			this.TripChoices(this.TripRow("Time Flow", "LET TIME PASS", "Let day turn to night, or hold on to your favourite hour.", 110f), new string[] { "FLOWING", "FROZEN" }, this._tripSettings.timeProgressionEnabled ? 0 : 1, delegate(int i)
			{
				this._tripSettings.timeProgressionEnabled = i == 0;
			});
			this.TripSlider(this.TripRow("Day Length", "A DAY LASTS", "Real minutes for a full day and night.", 110f), this._tripSettings.dayLengthMinutes, 5f, 120f, 1f, this._tripSettings.timeProgressionEnabled, delegate(float v)
			{
				this._tripSettings.dayLengthMinutes = v;
			}, new Func<float, string>(CielOSMapsApp.Minutes));
		}

		// Token: 0x0600A152 RID: 41298 RVA: 0x0042F294 File Offset: 0x0042D494
		private void BuildTripWorld()
		{
			this.TripChoices(this.TripRow("Weather Mode", "CHANGING SKIES", "Let the skies change, or keep one forecast.", 110f), new string[] { "CHANGING", "FIXED" }, this._tripSettings.variableWeather ? 0 : 1, delegate(int i)
			{
				this._tripSettings.variableWeather = i == 0;
			});
			string[] names = DynamicWeatherPolicy.AuthoredPresetNames;
			int num = Array.FindIndex<string>(names, (string n) => DynamicWeatherPolicy.NormalizeName(n) == DynamicWeatherPolicy.NormalizeName(this._tripSettings.weatherPresetName));
			if (num < 0)
			{
				num = Mathf.Clamp(this._tripSettings.weatherIndex, 0, names.Length - 1);
			}
			this.TripSelector(this.TripRow("Start Weather", "START WITH", "The sky at the start of your run.", 110f), Array.ConvertAll<string, string>(names, new Converter<string, string>(CielOSMapsApp.TripWeatherName)), num, delegate(int i)
			{
				this._tripSettings.weatherIndex = i;
				this._tripSettings.weatherPresetName = names[i];
				if (this._tripSettings.allowedWeatherPresetNames.Length != 0 && !this.TripWeatherIncluded(names[i]))
				{
					List<string> list = new List<string>(this._tripSettings.allowedWeatherPresetNames) { names[i] };
					this._tripSettings.allowedWeatherPresetNames = list.ToArray();
				}
			}, true);
			this.TripSlider(this.TripRow("Weather Interval", "NEW WEATHER EVERY", "Real minutes between weather changes.", 110f), this._tripSettings.weatherChangeIntervalMinutes, 1f, 20f, 1f, this._tripSettings.variableWeather, delegate(float v)
			{
				this._tripSettings.weatherChangeIntervalMinutes = v;
			}, new Func<float, string>(CielOSMapsApp.Minutes));
			RectTransform rectTransform = this.TripRow("Forecast Pool", "WEATHER POOL", "Choose the weather types that can appear. Keep at least one selected.", 166f);
			RectTransform rectTransform2 = this.TripControl(rectTransform);
			rectTransform2.offsetMin = new Vector2(4f, -65f);
			rectTransform2.offsetMax = new Vector2(-16f, 65f);
			for (int j = 0; j < names.Length; j++)
			{
				string name = names[j];
				bool included = this.TripWeatherIncluded(name);
				Button button = this.TripAction("Forecast " + name, rectTransform2, CielOSMapsApp.TripWeatherName(name), included);
				if (!included)
				{
					button.targetGraphic.color = this._background;
				}
				int num2 = j % 3;
				int num3 = j / 3;
				CielOSMapsApp.SetRect((RectTransform)button.transform, new Vector2((float)num2 / 3f, 1f), new Vector2(((float)num2 + 1f) / 3f, 1f), new Vector2(2f, (float)(-(float)num3) * 44f - 38f), new Vector2(-2f, (float)(-(float)num3) * 44f));
				button.interactable = this._tripSettings.variableWeather;
				button.onClick.AddListener(delegate
				{
					this.SetTripWeatherIncluded(name, !included);
					this.SaveTripSettings(true);
				});
			}
		}

		// Token: 0x0600A153 RID: 41299 RVA: 0x0042F574 File Offset: 0x0042D774
		private bool TripWeatherIncluded(string name)
		{
			return this._tripSettings.allowedWeatherPresetNames.Length == 0 || Array.Exists<string>(this._tripSettings.allowedWeatherPresetNames, (string n) => DynamicWeatherPolicy.NormalizeName(n) == DynamicWeatherPolicy.NormalizeName(name));
		}

		// Token: 0x0600A154 RID: 41300 RVA: 0x0042F5BC File Offset: 0x0042D7BC
		private void SetTripWeatherIncluded(string name, bool included)
		{
			List<string> list = new List<string>();
			foreach (string text in DynamicWeatherPolicy.AuthoredPresetNames)
			{
				if ((DynamicWeatherPolicy.NormalizeName(text) == DynamicWeatherPolicy.NormalizeName(name)) ? included : this.TripWeatherIncluded(text))
				{
					list.Add(text);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this._tripSettings.allowedWeatherPresetNames = list.ToArray();
			if (!this.TripWeatherIncluded(this._tripSettings.weatherPresetName))
			{
				this._tripSettings.weatherPresetName = list[0];
				this._tripSettings.weatherIndex = Array.IndexOf<string>(DynamicWeatherPolicy.AuthoredPresetNames, list[0]);
			}
		}

		// Token: 0x0600A155 RID: 41301 RVA: 0x0042F668 File Offset: 0x0042D868
		private static string TripWeatherName(string name)
		{
			string text = DynamicWeatherPolicy.NormalizeName(name);
			if (text == "CALMRAIN")
			{
				return "LIGHT RAIN";
			}
			if (text == "FOGNESS")
			{
				return "FOG";
			}
			if (!(text == "SUNNYCONSTELLATION"))
			{
				return name.ToUpperInvariant();
			}
			return "STARRY SKY";
		}

		// Token: 0x0600A156 RID: 41302 RVA: 0x0042F6C0 File Offset: 0x0042D8C0
		private void BuildTripRules()
		{
			this.TripChoices(this.TripRow("NPC Reactions", "NPC REACTIONS", "Choose how passers-by respond to Ciel.", 110f), new string[] { "IGNORE CIEL", "REACT ONLY", "ENCOUNTERS" }, (!this._tripSettings.detectionEnabled) ? 0 : (this._tripSettings.encountersEnabled ? 2 : 1), delegate(int i)
			{
				this._tripSettings.detectionEnabled = i != 0;
				this._tripSettings.encountersEnabled = i == 2;
			});
			FreeNpcDifficulty[] difficultyLevels = new FreeNpcDifficulty[]
			{
				FreeNpcDifficulty.VeryEasy,
				FreeNpcDifficulty.Easy,
				FreeNpcDifficulty.Default,
				FreeNpcDifficulty.Hard,
				FreeNpcDifficulty.Extreme
			};
			int num = Array.IndexOf<FreeNpcDifficulty>(difficultyLevels, this._tripSettings.npcDifficulty);
			this.TripSelector(this.TripRow("NPC Aggressiveness", "NPC AGGRESSIVENESS", "Adjust how far NPCs can see Ciel.", 110f), new string[] { "VERY EASY", "EASY", "DEFAULT", "HARD", "EXTREME" }, (num < 0) ? 2 : num, delegate(int i)
			{
				this._tripSettings.npcDifficulty = difficultyLevels[i];
			}, this._tripSettings.detectionEnabled);
			this.TripChoices(this.TripRow("Stamina", "STAMINA", "Use stamina normally, or keep it full for movement and actions.", 110f), new string[] { "NORMAL", "INFINITE" }, this._tripSettings.infiniteStamina ? 1 : 0, delegate(int i)
			{
				this._tripSettings.infiniteStamina = i == 1;
			});
			this.TripChoices(this.TripRow("Loot", "TRASHCAN LOOT", "Choose how much loot you find in trashcans.", 110f), new string[] { "SCARCE", "NORMAL", "ABUNDANT" }, (int)this._tripSettings.lootAmount, delegate(int i)
			{
				this._tripSettings.lootAmount = (FreeLootAmount)i;
			});
			this.TripChoices(this.TripRow("Clothing Duration", "CLOTHING DURATION", "Choose how long clothes last. 2× doubles the duration.", 110f), new string[] { "TIMED", "INFINITE" }, this._tripSettings.clothingDurabilityEnabled ? 0 : 1, delegate(int i)
			{
				this._tripSettings.clothingDurabilityEnabled = i == 0;
			});
			this.TripSlider(this.TripRow("Clothing Multiplier", "CLOTHING DURATION", "Choose how long clothes last. 2× doubles the duration.", 110f), this._tripSettings.clothingDurationMultiplier, 0.25f, 5f, 0.25f, this._tripSettings.clothingDurabilityEnabled, delegate(float v)
			{
				this._tripSettings.clothingDurationMultiplier = v;
			}, new Func<float, string>(CielOSMapsApp.Multiplier));
			this.TripChoices(this.TripRow("Item Duration", "ITEM DURATION & USES", "Items can run out of time or uses. 2× doubles timers.", 110f), new string[] { "LIMITED", "INFINITE" }, this._tripSettings.itemDurabilityEnabled ? 0 : 1, delegate(int i)
			{
				this._tripSettings.itemDurabilityEnabled = i == 0;
			});
			this.TripSlider(this.TripRow("Item Multiplier", "ITEM DURATION & USES", "Items can run out of time or uses. 2× doubles timers.", 110f), this._tripSettings.itemDurationMultiplier, 0.25f, 5f, 0.25f, this._tripSettings.itemDurabilityEnabled, delegate(float v)
			{
				this._tripSettings.itemDurationMultiplier = v;
			}, new Func<float, string>(CielOSMapsApp.Multiplier));
		}

		// Token: 0x0600A157 RID: 41303 RVA: 0x0042F9E8 File Offset: 0x0042DBE8
		private void BuildTripPaint()
		{
			this.TripChoices(this.TripRow("Bodypaint Pen Uses", "BODYPAINT PEN USES", "Choose limited or infinite uses for preset and custom bodypaint pens.", 110f), new string[] { "LIMITED", "INFINITE" }, this._tripSettings.infiniteBodyPaintPenUses ? 1 : 0, delegate(int i)
			{
				this._tripSettings.infiniteBodyPaintPenUses = i == 1;
			});
			this.TripChoices(this.TripRow("Bodypaint Mode", "BODYPAINT DURATION", "Use each paint’s usual duration, choose a timer, or keep it indefinitely.", 110f), new string[] { "DEFAULT", "TIMED", "INFINITE" }, (int)this._tripSettings.bodyPaintDurationMode, delegate(int i)
			{
				this._tripSettings.bodyPaintDurationMode = (BodyPaintDurationMode)i;
			});
			this.TripSlider(this.TripRow("Bodypaint Timer", "PAINT LASTS FOR", "Applies to mud, tint, preset and custom bodypaint.", 110f), this._tripSettings.bodyPaintDurationSeconds, 30f, 1800f, 30f, this._tripSettings.bodyPaintDurationMode == BodyPaintDurationMode.Custom, delegate(float v)
			{
				this._tripSettings.bodyPaintDurationSeconds = v;
			}, (float v) => Mathf.FloorToInt(v / 60f).ToString("00") + ":" + Mathf.RoundToInt(v % 60f).ToString("00"));
			this.TripChoices(this.TripRow("Rain Wash", "RAIN WASHES PAINT", "Let rain wash bodypaint away, or protect it from the weather.", 110f), new string[] { "WASHES OFF", "PROTECTED" }, this._tripSettings.bodyPaintRainWashEnabled ? 0 : 1, delegate(int i)
			{
				this._tripSettings.bodyPaintRainWashEnabled = i == 0;
			});
		}

		// Token: 0x0600A158 RID: 41304 RVA: 0x0042FB63 File Offset: 0x0042DD63
		private static string Minutes(float value)
		{
			return GameLocalization.FormatSource("cielos_settings", "{0} MIN", new object[] { Mathf.RoundToInt(value) });
		}

		// Token: 0x0600A159 RID: 41305 RVA: 0x0024D310 File Offset: 0x0024B510
		private static string Multiplier(float value)
		{
			return "x" + value.ToString("0.00", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A15A RID: 41306 RVA: 0x0042FB88 File Offset: 0x0042DD88
		private void BuildTripInventory()
		{
			for (int i = 0; i < 3; i++)
			{
				int index = i;
				InventoryItemSO inventoryItemSO = FreeRunLoadoutCatalog.ResolveEligible(this._tripSettings.startingItemIds[i]);
				RectTransform rectTransform = this.TripRow("Starting Item " + i.ToString(), GameLocalization.FormatSource("cielos_settings", "SLOT {0}", new object[] { i + 1 }), "Choose one unlocked item to carry at the start.", 112f);
				RectTransform rectTransform2 = this.TripControl(rectTransform);
				Button button = this.TripAction("Browse Items", rectTransform2, (inventoryItemSO != null) ? CielOSMapsApp.ItemName(inventoryItemSO) : "EMPTY", false);
				CielOSMapsApp.Stretch((RectTransform)button.transform, Vector2.zero, Vector2.zero);
				button.targetGraphic.color = this._background;
				if (inventoryItemSO != null && inventoryItemSO.icon != null)
				{
					Image image = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Item Icon", button.transform), Color.white);
					image.sprite = inventoryItemSO.icon;
					StreamerCensorImage.BindItem(image, inventoryItemSO);
					image.preserveAspect = true;
					image.raycastTarget = false;
					CielOSMapsApp.SetRect(image.rectTransform, Vector2.zero, Vector2.up, new Vector2(8f, 4f), new Vector2(44f, -4f));
					button.GetComponentInChildren<TMP_Text>().rectTransform.offsetMin = new Vector2(52f, 0f);
				}
				button.onClick.AddListener(delegate
				{
					this.OpenTripInventoryPicker(index);
				});
			}
		}

		// Token: 0x0600A15B RID: 41307 RVA: 0x0042FD2F File Offset: 0x0042DF2F
		private static string ItemName(InventoryItemSO item)
		{
			return InteractionLocalization.ObjectName(string.IsNullOrWhiteSpace(item.itemName) ? item.name : item.itemName);
		}

		// Token: 0x0600A15C RID: 41308 RVA: 0x0042FD54 File Offset: 0x0042DF54
		private void OpenTripInventoryPicker(int slot)
		{
			CielOSMapsApp.<>c__DisplayClass98_0 CS$<>8__locals1 = new CielOSMapsApp.<>c__DisplayClass98_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slot = slot;
			if (this._tripPicker != null)
			{
				return;
			}
			this._tripPicker = this.TripPanel("Inventory Picker", this._tripRoot, this._surface);
			CielOSMapsApp.Stretch(this._tripPicker, new Vector2(254f, 0f), Vector2.zero);
			CielOSMapsApp.SetRect(this.TripLabel("BROWSE ITEMS", this._tripPicker, 26f, FontStyles.Bold, this._text).rectTransform, Vector2.up, Vector2.one, new Vector2(24f, -60f), new Vector2(-160f, -16f));
			Button button = this.TripAction("Close Picker", this._tripPicker, "CANCEL", false);
			CielOSMapsApp.SetRect((RectTransform)button.transform, Vector2.one, Vector2.one, new Vector2(-138f, -54f), new Vector2(-20f, -18f));
			button.onClick.AddListener(delegate
			{
				CS$<>8__locals1.<>4__this._tripPicker.gameObject.SetActive(false);
				Object.Destroy(CS$<>8__locals1.<>4__this._tripPicker.gameObject);
				CS$<>8__locals1.<>4__this._tripPicker = null;
			});
			RectTransform rectTransform = CielOSMapsApp.NewRect("Items Viewport", this._tripPicker);
			CielOSMapsApp.Stretch(rectTransform, new Vector2(24f, 24f), new Vector2(-24f, -86f));
			CielOSMapsApp.AddImage(rectTransform, Color.clear);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform2 = CielOSMapsApp.NewRect("Items", rectTransform);
			rectTransform2.anchorMin = Vector2.up;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = Vector2.up;
			rectTransform2.anchoredPosition = Vector2.zero;
			List<InventoryItemSO> list = new List<InventoryItemSO>();
			list.Add(null);
			list.AddRange(FreeRunLoadoutCatalog.GetUnlockedChoices());
			int num = 0;
			using (List<InventoryItemSO>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CielOSMapsApp.<>c__DisplayClass98_1 CS$<>8__locals2 = new CielOSMapsApp.<>c__DisplayClass98_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.item = enumerator.Current;
					int num2 = ((CS$<>8__locals2.item == null) ? (-1) : Array.FindIndex<string>(this._tripSettings.startingItemIds, (string id) => id == CS$<>8__locals2.item.itemId));
					if (num2 < 0 || num2 == CS$<>8__locals2.CS$<>8__locals1.slot)
					{
						InventoryItemSO choice = CS$<>8__locals2.item;
						Button button2 = this.TripAction("Item " + num.ToString(), rectTransform2, (CS$<>8__locals2.item == null) ? "EMPTY" : CielOSMapsApp.ItemName(CS$<>8__locals2.item), CS$<>8__locals2.item != null && CS$<>8__locals2.item.itemId == this._tripSettings.startingItemIds[CS$<>8__locals2.CS$<>8__locals1.slot]);
						CielOSMapsApp.SetRect((RectTransform)button2.transform, Vector2.up, Vector2.one, new Vector2(0f, (float)(-(float)num) * 64f - 56f), new Vector2(0f, (float)(-(float)num) * 64f));
						TMP_Text componentInChildren = button2.GetComponentInChildren<TMP_Text>();
						componentInChildren.alignment = TextAlignmentOptions.Left;
						componentInChildren.rectTransform.offsetMin = new Vector2(64f, 0f);
						if (CS$<>8__locals2.item != null && CS$<>8__locals2.item.icon != null)
						{
							Image image = CielOSMapsApp.AddImage(CielOSMapsApp.NewRect("Icon", button2.transform), Color.white);
							image.sprite = CS$<>8__locals2.item.icon;
							StreamerCensorImage.BindItem(image, CS$<>8__locals2.item);
							image.preserveAspect = true;
							image.raycastTarget = false;
							CielOSMapsApp.SetRect(image.rectTransform, Vector2.zero, Vector2.up, new Vector2(10f, 8f), new Vector2(50f, -8f));
						}
						button2.onClick.AddListener(delegate
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this._tripSettings.startingItemIds[CS$<>8__locals2.CS$<>8__locals1.slot] = ((choice == null) ? string.Empty : choice.itemId);
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.SaveTripSettings(true);
						});
						num++;
					}
				}
			}
			rectTransform2.sizeDelta = new Vector2(0f, (float)num * 64f);
			ScrollRect scrollRect = rectTransform.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform;
			scrollRect.content = rectTransform2;
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			rectTransform.gameObject.AddComponent<SmoothCollectionScroll>().Configure(48f, 0.16f);
		}

		// Token: 0x0600A15D RID: 41309 RVA: 0x0043021C File Offset: 0x0042E41C
		private void ResetTripPage()
		{
			FreeRunSettings freeRunSettings = new FreeRunSettings();
			switch (this._tripPage)
			{
			case CielOSMapsApp.TripPage.Start:
				this._tripSettings.usePresetOutfit = freeRunSettings.usePresetOutfit;
				this._tripSettings.startHour = freeRunSettings.startHour;
				this._tripSettings.timeProgressionEnabled = freeRunSettings.timeProgressionEnabled;
				this._tripSettings.dayLengthMinutes = freeRunSettings.dayLengthMinutes;
				break;
			case CielOSMapsApp.TripPage.World:
				this._tripSettings.variableWeather = freeRunSettings.variableWeather;
				this._tripSettings.weatherIndex = freeRunSettings.weatherIndex;
				this._tripSettings.weatherPresetName = freeRunSettings.weatherPresetName;
				this._tripSettings.weatherChangeIntervalMinutes = freeRunSettings.weatherChangeIntervalMinutes;
				this._tripSettings.allowedWeatherPresetNames = freeRunSettings.allowedWeatherPresetNames;
				break;
			case CielOSMapsApp.TripPage.Rules:
				this._tripSettings.detectionEnabled = freeRunSettings.detectionEnabled;
				this._tripSettings.encountersEnabled = freeRunSettings.encountersEnabled;
				this._tripSettings.npcDifficulty = freeRunSettings.npcDifficulty;
				this._tripSettings.infiniteStamina = freeRunSettings.infiniteStamina;
				this._tripSettings.lootAmount = freeRunSettings.lootAmount;
				this._tripSettings.clothingDurabilityEnabled = freeRunSettings.clothingDurabilityEnabled;
				this._tripSettings.clothingDurationMultiplier = freeRunSettings.clothingDurationMultiplier;
				this._tripSettings.itemDurabilityEnabled = freeRunSettings.itemDurabilityEnabled;
				this._tripSettings.itemDurationMultiplier = freeRunSettings.itemDurationMultiplier;
				break;
			case CielOSMapsApp.TripPage.BodyPaint:
				this._tripSettings.infiniteBodyPaintPenUses = freeRunSettings.infiniteBodyPaintPenUses;
				this._tripSettings.bodyPaintDurationMode = freeRunSettings.bodyPaintDurationMode;
				this._tripSettings.bodyPaintDurationSeconds = freeRunSettings.bodyPaintDurationSeconds;
				this._tripSettings.bodyPaintRainWashEnabled = freeRunSettings.bodyPaintRainWashEnabled;
				break;
			case CielOSMapsApp.TripPage.Inventory:
				this._tripSettings.startingItemIds = freeRunSettings.startingItemIds;
				break;
			}
			this.SaveTripSettings(true);
		}

		// Token: 0x0400A4E2 RID: 42210
		private const string RequiredRankId = "hornyslut_declared";

		// Token: 0x0400A4E3 RID: 42211
		private static readonly CielOSMapsApp.Destination[] Destinations = new CielOSMapsApp.Destination[]
		{
			new CielOSMapsApp.Destination("POI - University", "University", "ACADEMIC DISTRICT", MiniMapPoiKind.University),
			new CielOSMapsApp.Destination("POI - Stadium", "Stadium", "SPORTS DISTRICT", MiniMapPoiKind.Stadium),
			new CielOSMapsApp.Destination("POI - Mall", "Shopping Mall", "COMMERCIAL DISTRICT", MiniMapPoiKind.Mall),
			new CielOSMapsApp.Destination("POI - Art Gallery", "Art Gallery", "CULTURAL DISTRICT", MiniMapPoiKind.Gallery),
			new CielOSMapsApp.Destination("POI - Pool", "Swimming Pool", "LEISURE DISTRICT", MiniMapPoiKind.Pool)
		};

		// Token: 0x0400A4E4 RID: 42212
		private static int _lastMapVariant = -1;

		// Token: 0x0400A4E5 RID: 42213
		private readonly List<CielOSMapsApp.DestinationRow> _rows = new List<CielOSMapsApp.DestinationRow>(CielOSMapsApp.Destinations.Length);

		// Token: 0x0400A4E6 RID: 42214
		private readonly List<CielOSMapsApp.DestinationMarker> _markers = new List<CielOSMapsApp.DestinationMarker>(CielOSMapsApp.Destinations.Length);

		// Token: 0x0400A4E7 RID: 42215
		private CielOSMapsApp.Destination _selected;

		// Token: 0x0400A4E8 RID: 42216
		private RectTransform _mapCanvas;

		// Token: 0x0400A4E9 RID: 42217
		private CielOSStaticCityMap _staticCityMap;

		// Token: 0x0400A4EA RID: 42218
		private Vector2 _lastMapCanvasSize;

		// Token: 0x0400A4EB RID: 42219
		private TMP_Text _readyCountLabel;

		// Token: 0x0400A4EC RID: 42220
		private TMP_Text _variantLabel;

		// Token: 0x0400A4ED RID: 42221
		private TMP_Text _selectionEyebrow;

		// Token: 0x0400A4EE RID: 42222
		private TMP_Text _selectionTitle;

		// Token: 0x0400A4EF RID: 42223
		private TMP_Text _selectionDescription;

		// Token: 0x0400A4F0 RID: 42224
		private TMP_Text _travelLabel;

		// Token: 0x0400A4F1 RID: 42225
		private Button _travelButton;

		// Token: 0x0400A4F2 RID: 42226
		private Image _travelButtonImage;

		// Token: 0x0400A4F3 RID: 42227
		private Color _background;

		// Token: 0x0400A4F4 RID: 42228
		private Color _surface;

		// Token: 0x0400A4F5 RID: 42229
		private Color _surfaceRaised;

		// Token: 0x0400A4F6 RID: 42230
		private Color _text;

		// Token: 0x0400A4F7 RID: 42231
		private Color _muted;

		// Token: 0x0400A4F8 RID: 42232
		private Color _accent;

		// Token: 0x0400A4F9 RID: 42233
		private Color _outline;

		// Token: 0x0400A4FA RID: 42234
		private Color _locked;

		// Token: 0x0400A4FB RID: 42235
		private FreeRunSettings _tripSettings;

		// Token: 0x0400A4FC RID: 42236
		private CielOSMapsApp.TripPage _tripPage;

		// Token: 0x0400A4FD RID: 42237
		private RectTransform _tripRoot;

		// Token: 0x0400A4FE RID: 42238
		private RectTransform _tripList;

		// Token: 0x0400A4FF RID: 42239
		private RectTransform _tripPicker;

		// Token: 0x0400A500 RID: 42240
		private ScrollRect _tripScroll;

		// Token: 0x0400A501 RID: 42241
		private TMP_Text _tripButtonLabel;

		// Token: 0x0400A502 RID: 42242
		private Image _tripButtonIcon;

		// Token: 0x0400A503 RID: 42243
		private bool _tripOpen;

		// Token: 0x0400A504 RID: 42244
		private float _tripRowY;

		// Token: 0x0400A505 RID: 42245
		private readonly Dictionary<CielOSMapsApp.TripPage, float> _tripScrollPositions = new Dictionary<CielOSMapsApp.TripPage, float>();

		// Token: 0x02001467 RID: 5223
		private sealed class Destination
		{
			// Token: 0x0600A169 RID: 41321 RVA: 0x0043054B File Offset: 0x0042E74B
			public Destination(string sceneName, string displayName, string district, MiniMapPoiKind kind)
			{
				this.SceneName = sceneName;
				this.DisplayName = displayName;
				this.District = district;
				this.Kind = kind;
			}

			// Token: 0x0400A506 RID: 42246
			public readonly string SceneName;

			// Token: 0x0400A507 RID: 42247
			public readonly string DisplayName;

			// Token: 0x0400A508 RID: 42248
			public readonly string District;

			// Token: 0x0400A509 RID: 42249
			public readonly MiniMapPoiKind Kind;
		}

		// Token: 0x02001468 RID: 5224
		private sealed class DestinationRow
		{
			// Token: 0x0400A50A RID: 42250
			public CielOSMapsApp.Destination Destination;

			// Token: 0x0400A50B RID: 42251
			public Image Background;

			// Token: 0x0400A50C RID: 42252
			public ProceduralMiniMapPoiIconGraphic PoiIcon;

			// Token: 0x0400A50D RID: 42253
			public TMP_Text Question;

			// Token: 0x0400A50E RID: 42254
			public TMP_Text Name;

			// Token: 0x0400A50F RID: 42255
			public TMP_Text Progress;

			// Token: 0x0400A510 RID: 42256
			public Image[] ProgressDots;
		}

		// Token: 0x02001469 RID: 5225
		private sealed class DestinationMarker
		{
			// Token: 0x0400A511 RID: 42257
			public CielOSMapsApp.Destination Destination;

			// Token: 0x0400A512 RID: 42258
			public RectTransform Rect;

			// Token: 0x0400A513 RID: 42259
			public CielOSMapHotspotGraphic Hotspot;

			// Token: 0x0400A514 RID: 42260
			public ProceduralMiniMapPoiIconGraphic PoiIcon;

			// Token: 0x0400A515 RID: 42261
			public TMP_Text Question;

			// Token: 0x0400A516 RID: 42262
			public TMP_Text Label;
		}

		// Token: 0x0200146A RID: 5226
		private enum TripPage
		{
			// Token: 0x0400A518 RID: 42264
			Start,
			// Token: 0x0400A519 RID: 42265
			World,
			// Token: 0x0400A51A RID: 42266
			Rules,
			// Token: 0x0400A51B RID: 42267
			BodyPaint,
			// Token: 0x0400A51C RID: 42268
			Inventory
		}
	}
}
