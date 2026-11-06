using System;
using Features.Localization;
using Features.OxGram;
using Features.PhoneCompanion;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Minimap;
using ProjektSumperk;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x0200147F RID: 5247
	[RequireComponent(typeof(RectTransform))]
	public sealed class MapsPhoneApp : MonoBehaviour
	{
		// Token: 0x0600A1B3 RID: 41395 RVA: 0x00431ABC File Offset: 0x0042FCBC
		internal void Initialize(SmartphoneController owner, TMP_FontAsset font)
		{
			this._owner = owner;
			this._font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			this._animator = base.GetComponent<Animator>();
			this._island = base.transform.root.GetComponentInChildren<DynamicIslandController>(true);
			this.BuildUi();
		}

		// Token: 0x0600A1B4 RID: 41396 RVA: 0x00431B10 File Offset: 0x0042FD10
		public void OpenApp()
		{
			if (!PhoneAppAccessPolicy.TryOpen("Map"))
			{
				return;
			}
			this.BuildUi();
			this.EnsureHomeUnderneath();
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			if (this._animator != null && this._animator.isActiveAndEnabled && this._animator.runtimeAnimatorController != null)
			{
				this._animator.Play("BackIdleAnimation", -1, 0f);
			}
			if (!this._registered)
			{
				SmartphoneController.NotifyCustomAppOpened(new Action(this.CloseApp));
				this._registered = true;
			}
			this._nextSnapshot = (this._nextView = 0f);
			if (this._atlas == null && this._atlasRequest == null)
			{
				this._atlasRequest = Resources.LoadAsync<ProceduralMiniMapStyleAtlas>("ProceduralMiniMapStyleAtlas");
			}
			this.RefreshMap();
		}

		// Token: 0x0600A1B5 RID: 41397 RVA: 0x00431BF4 File Offset: 0x0042FDF4
		public void CloseApp()
		{
			this.Unregister();
			if (this._island != null && this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				this._island.UniversalBack(this._animator);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600A1B6 RID: 41398 RVA: 0x00431C54 File Offset: 0x0042FE54
		private void OnEnable()
		{
			RunModeService.Changed += this.RefreshAccess;
		}

		// Token: 0x0600A1B7 RID: 41399 RVA: 0x00431C67 File Offset: 0x0042FE67
		private void OnDisable()
		{
			RunModeService.Changed -= this.RefreshAccess;
			this.Unregister();
		}

		// Token: 0x0600A1B8 RID: 41400 RVA: 0x00431C80 File Offset: 0x0042FE80
		private void Unregister()
		{
			if (!this._registered)
			{
				return;
			}
			SmartphoneController.NotifyCustomAppClosed();
			this._registered = false;
		}

		// Token: 0x0600A1B9 RID: 41401 RVA: 0x00431C98 File Offset: 0x0042FE98
		private void LateUpdate()
		{
			if (!MapsPhoneInstaller.IsAvailable)
			{
				this.RefreshAccess();
				return;
			}
			if (this._content == null)
			{
				return;
			}
			this.FitContent();
			if (Time.unscaledTime < this._nextView)
			{
				return;
			}
			this._nextView = Time.unscaledTime + 0.1f;
			this.RefreshMap();
		}

		// Token: 0x0600A1BA RID: 41402 RVA: 0x00431CF0 File Offset: 0x0042FEF0
		private void RefreshMap()
		{
			if (this._roads == null)
			{
				return;
			}
			bool isLongWayHome = RunModeService.IsLongWayHome;
			this.ApplyModePresentation(isLongWayHome);
			this.FitContent();
			if (this._atlasRequest != null && this._atlasRequest.isDone)
			{
				this._atlas = this._atlasRequest.asset as ProceduralMiniMapStyleAtlas;
				this._atlasRequest = null;
				this._roads.SetStyleAtlas(this._atlas);
				this._nextSnapshot = 0f;
			}
			Transform transform = ((PlayerRegistry.PlayerObject != null) ? PlayerRegistry.PlayerObject.transform : ((this._owner != null && this._owner.playerAnimator != null) ? this._owner.playerAnimator.transform : ((this._owner != null) ? this._owner.transform : null)));
			ChunkManager instance = ChunkManager.Instance;
			bool flag = transform != null && instance != null && SceneManager.GetActiveScene().name == "Game - Current";
			this._roads.gameObject.SetActive(flag);
			this._previewRoads.gameObject.SetActive(flag && !isLongWayHome);
			this._pois.gameObject.SetActive(flag);
			this._playerMarker.gameObject.SetActive(flag);
			this._playerHalo.gameObject.SetActive(flag && isLongWayHome);
			this._youLabel.gameObject.SetActive(flag && isLongWayHome);
			this._homeBearing.gameObject.SetActive(false);
			Vector3 vector;
			bool flag2 = MapsPhoneApp.TryGetHome(out vector);
			this._homeMarker.gameObject.SetActive(flag && flag2);
			MapsPhoneApp.SetText(this._objective, isLongWayHome ? "RETURN TO HOME" : "CITY MAP");
			if (!flag)
			{
				MapsPhoneApp.SetText(this._distance, "WAITING FOR CITY");
				MapsPhoneApp.SetText(this._direction, "Live Maps is available during city runs.");
				MapsPhoneApp.SetText(this._status, "NORTH UP  /  WALKING MAP");
				return;
			}
			Vector3 position = transform.position;
			Vector3 vector2 = ((isLongWayHome || this._streetView || !flag2) ? position : ((position + vector) * 0.5f));
			Vector2 size = this._viewport.rect.size;
			Vector2 vector3 = (flag2 ? new Vector2(Mathf.Abs(position.x - vector.x) + 180f, Mathf.Abs(position.z - vector.z) + 180f) : (Vector2.one * 360f));
			vector3.x = Mathf.Max(300f, vector3.x);
			vector3.y = Mathf.Max(300f, vector3.y);
			float num = (isLongWayHome ? MapsNavigationMath.LocalWalkingScale(size) : (this._streetView ? 2f : MapsNavigationMath.FitScale(size, vector3, 36f)));
			Vector2 vector4 = size / Mathf.Max(0.001f, num);
			if (Time.unscaledTime >= this._nextSnapshot)
			{
				this._nextSnapshot = Time.unscaledTime + 0.75f;
				this._snapshot.Rebuild(instance, this._atlas, vector2, vector4 + Vector2.one * instance.CellSize, isLongWayHome);
				this._roads.SetChunks(this._snapshot.Chunks);
				this._previewRoads.SetChunks(this._snapshot.PreviewChunks);
				this._pois.SetPois(this._snapshot.Pois);
			}
			this._roads.SetView(vector2, 0f, num, 1f);
			this._previewRoads.SetView(vector2, 0f, num, 1f);
			this._pois.SetView(vector2, 0f, num, 1f);
			this._playerMarker.rectTransform.anchoredPosition = MapsNavigationMath.Project(position, vector2, num);
			this._playerMarker.SetHeading(-transform.eulerAngles.y);
			this._playerHalo.anchoredPosition = this._playerMarker.rectTransform.anchoredPosition;
			this._youLabel.rectTransform.anchoredPosition = this._playerMarker.rectTransform.anchoredPosition + Vector2.down * 33f;
			if (flag2)
			{
				Vector2 vector5 = MapsNavigationMath.Project(vector, vector2, num);
				this._homeMarker.anchoredPosition = MapsNavigationMath.ClampMarker(vector5, size, isLongWayHome ? 64f : 28f);
				bool flag3 = (vector5 - this._homeMarker.anchoredPosition).sqrMagnitude > 0.01f;
				this._homeBearing.gameObject.SetActive(isLongWayHome && flag3);
				this._homeBearing.anchoredPosition = this._homeMarker.anchoredPosition + vector5.normalized * 29f;
				this._homeBearing.localEulerAngles = new Vector3(0f, 0f, -MapsNavigationMath.Heading(position, vector));
				string text = MapsNavigationMath.Bearing(position, vector);
				if (isLongWayHome && flag3)
				{
					MapsPhoneApp.SetBearing(this._homeLabel, "HOME {0}", text);
				}
				else
				{
					MapsPhoneApp.SetText(this._homeLabel, "HOME");
				}
				float num2 = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(vector.x, vector.z));
				if (num2 >= 1000f)
				{
					LocalizedGameText.BindFormat(this._distance, "maps_ui", "{0:0.00} km TO HOME", new object[] { num2 / 1000f });
				}
				else
				{
					LocalizedGameText.BindFormat(this._distance, "maps_ui", "{0:0} m TO HOME", new object[] { num2 });
				}
				MapsPhoneApp.SetBearing(this._direction, isLongWayHome ? "HOME {0}  /  DIRECT DISTANCE" : "HOME IS {0}  /  STRAIGHT-LINE DISTANCE", text);
			}
			else
			{
				MapsPhoneApp.SetText(this._distance, "YOU ARE HERE");
				MapsPhoneApp.SetText(this._direction, "Match the streets to your surroundings.");
			}
			MapsPhoneApp.SetText(this._status, isLongWayHome ? "Reach your front door. Stay out of sight." : "Explore the city on foot.");
			LocalizedGameText.BindFormat(this._scaleLabel, "maps_ui", "{0:0} m ACROSS", new object[] { vector4.x });
			if (isLongWayHome)
			{
				MapsPhoneApp.SetBearing(this._mapDetail, "FACING {0}  /  NORTH IS UP", MapsNavigationMath.Bearing(position, position + transform.forward * 10f));
			}
			MapsPhoneApp.SetText(this._viewLabel, this._streetView ? "SHOW OVERVIEW" : "SHOW STREETS");
		}

		// Token: 0x0600A1BB RID: 41403 RVA: 0x004323A8 File Offset: 0x004305A8
		private static bool TryGetHome(out Vector3 home)
		{
			LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
			if (RunModeService.IsLongWayHome && instance != null && instance.HasRoute)
			{
				home = instance.HomePosition;
				return true;
			}
			if (!RunModeService.IsLongWayHome)
			{
				foreach (MiniMapPoiMarker miniMapPoiMarker in MiniMapPoiMarker.Active)
				{
					if (miniMapPoiMarker != null && miniMapPoiMarker.Kind == MiniMapPoiKind.Home && miniMapPoiMarker.isActiveAndEnabled)
					{
						home = miniMapPoiMarker.transform.position;
						return true;
					}
				}
			}
			home = default(Vector3);
			return false;
		}

		// Token: 0x0600A1BC RID: 41404 RVA: 0x00432460 File Offset: 0x00430660
		private void ToggleView()
		{
			if (RunModeService.IsLongWayHome)
			{
				return;
			}
			this._streetView = !this._streetView;
			this._nextSnapshot = 0f;
			this.RefreshMap();
		}

		// Token: 0x0600A1BD RID: 41405 RVA: 0x0043248A File Offset: 0x0043068A
		private void RefreshAccess()
		{
			if (MapsPhoneInstaller.IsAvailable)
			{
				return;
			}
			this.Unregister();
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600A1BE RID: 41406 RVA: 0x004324A8 File Offset: 0x004306A8
		private void ApplyModePresentation(bool longWayHome)
		{
			bool? presentedLongWayHome = this._presentedLongWayHome;
			if ((presentedLongWayHome.GetValueOrDefault() == longWayHome) & (presentedLongWayHome != null))
			{
				return;
			}
			this._presentedLongWayHome = new bool?(longWayHome);
			this._nextSnapshot = 0f;
			this._direction.GetComponent<LocalizedGameText>().SetSourceFontSize(longWayHome ? 16f : 12f);
			this._homeLabel.GetComponent<LocalizedGameText>().SetSourceFontSize(longWayHome ? 14f : 12f);
			this._playerMarker.rectTransform.localScale = Vector3.one * (longWayHome ? 2.1f : 1.4f);
			this._orientationStrip.gameObject.SetActive(longWayHome);
			this._viewLabel.transform.parent.gameObject.SetActive(!longWayHome);
			this._legend.GetComponent<LocalizedGameText>().SetSourceFontSize(longWayHome ? 15f : 13f);
			MapsPhoneApp.SetText(this._legend, longWayHome ? "PINK ARROW = CIEL  /  HOUSE = HOME\nHOME ARROW POINTS TOWARD YOUR DOOR" : "NORTH IS UP  /  PINK ARROW = YOU\nHOUSE = HOME  /  ICONS = PLACES");
			this._mapDetail.GetComponent<LocalizedGameText>().SetSourceFontSize(longWayHome ? 15f : 12f);
			this._mapDetail.color = (longWayHome ? MapsPhoneApp.Accent : MapsPhoneApp.Muted);
			MapsPhoneApp.SetText(this._mapDetail, longWayHome ? "NORTH IS UP" : "Faint roads show the surrounding city.");
		}

		// Token: 0x0600A1BF RID: 41407 RVA: 0x0043260C File Offset: 0x0043080C
		private void FitContent()
		{
			Vector2 size = ((RectTransform)base.transform).rect.size;
			if (size == this._lastRootSize)
			{
				return;
			}
			this._lastRootSize = size;
			float num = ((size.x > 0f && size.y > 0f) ? Mathf.Min(size.x / 480f, size.y / 1014f) : 0.0053659766f);
			this._content.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x0600A1C0 RID: 41408 RVA: 0x004326A0 File Offset: 0x004308A0
		private void BuildUi()
		{
			if (this._content != null)
			{
				return;
			}
			this._content = OxGramUI.Rect("Maps Content", base.transform);
			this._content.anchorMin = (this._content.anchorMax = Vector2.one * 0.5f);
			this._content.sizeDelta = new Vector2(480f, 1014f);
			OxGramUI.Stretch(OxGramUI.Image("Background", this._content, MapsPhoneApp.Background, null).rectTransform, 0f, 0f, 0f, 0f);
			TMP_Text tmp_Text = this.Text("Title", this._content, "MAPS", 28f, Color.white);
			MapsPhoneApp.Position(tmp_Text.rectTransform, 0f, 25f, 480f, 50f);
			tmp_Text.fontStyle = FontStyles.Bold;
			MapsPhoneApp.Position((RectTransform)this.Button("Back", this._content, "< Back", new UnityAction(this.CloseApp), false).transform, 12f, 25f, 100f, 50f);
			Image image = OxGramUI.Image("Destination", this._content, MapsPhoneApp.Panel, PhoneUiSpriteFactory.Rounded);
			MapsPhoneApp.Position(image.rectTransform, 18f, 99f, 444f, 143f);
			this._objective = this.Text("Objective", image.transform, "RETURN TO HOME", 17f, MapsPhoneApp.Accent);
			MapsPhoneApp.Position(this._objective.rectTransform, 12f, 13f, 420f, 25f);
			this._objective.fontStyle = FontStyles.Bold;
			this._distance = this.Text("Distance", image.transform, "LOCATING HOME...", 29f, Color.white);
			MapsPhoneApp.Position(this._distance.rectTransform, 12f, 45f, 420f, 40f);
			this._distance.fontStyle = FontStyles.Bold;
			this._direction = this.Text("Bearing", image.transform, "NORTH UP", 12f, MapsPhoneApp.Muted);
			MapsPhoneApp.Position(this._direction.rectTransform, 12f, 95f, 420f, 30f);
			Image image2 = OxGramUI.Image("Live City", this._content, new Color(0.13f, 0.16f, 0.19f), null);
			this._viewport = image2.rectTransform;
			MapsPhoneApp.Position(this._viewport, 18f, 258f, 444f, 536f);
			image2.gameObject.AddComponent<RectMask2D>();
			this._previewRoads = this.Graphic<ProceduralMiniMapGraphic>("Surrounding Streets", this._viewport);
			this._previewRoads.SetExpanded(true);
			CanvasGroup canvasGroup = this._previewRoads.gameObject.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 0.35f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
			this._roads = this.Graphic<ProceduralMiniMapGraphic>("Streets", this._viewport);
			this._roads.SetExpanded(true);
			this._pois = this.Graphic<ProceduralMiniMapPoiGraphic>("Places", this._viewport);
			this._pois.SetExpanded(true);
			Image image3 = OxGramUI.Image("Ciel Halo", this._viewport, new Color(0.035f, 0.06f, 0.09f, 0.92f), PhoneUiSpriteFactory.Circle);
			this._playerHalo = image3.rectTransform;
			this._playerHalo.anchorMin = (this._playerHalo.anchorMax = Vector2.one * 0.5f);
			this._playerHalo.sizeDelta = Vector2.one * 43f;
			image3.raycastTarget = false;
			this._playerHalo.gameObject.SetActive(false);
			this._homeMarker = OxGramUI.Rect("Home Destination", this._viewport);
			this._homeMarker.anchorMin = (this._homeMarker.anchorMax = Vector2.one * 0.5f);
			this._homeMarker.sizeDelta = new Vector2(40f, 40f);
			this.Graphic<ProceduralMiniMapPoiIconGraphic>("House", this._homeMarker).ConfigureMapMarker(MiniMapPoiKind.Home, false, true, 15f);
			this._homeLabel = this.Text("Home Label", this._homeMarker, "HOME", 12f, Color.white);
			MapsPhoneApp.Position(this._homeLabel.rectTransform, -30f, 38f, 100f, 20f);
			this._homeLabel.fontStyle = FontStyles.Bold;
			this._homeLabel.gameObject.AddComponent<Outline>().effectColor = MapsPhoneApp.Background;
			MapsDirectionGraphic mapsDirectionGraphic = this.Graphic<MapsDirectionGraphic>("Home Bearing", this._viewport);
			mapsDirectionGraphic.color = MapsPhoneApp.Accent;
			this._homeBearing = mapsDirectionGraphic.rectTransform;
			this._homeBearing.anchorMin = (this._homeBearing.anchorMax = Vector2.one * 0.5f);
			this._homeBearing.sizeDelta = Vector2.one * 28f;
			this._homeBearing.gameObject.SetActive(false);
			this._playerMarker = this.Graphic<ProceduralMiniMapMarkerGraphic>("You", this._viewport);
			this._playerMarker.rectTransform.anchorMin = (this._playerMarker.rectTransform.anchorMax = Vector2.one * 0.5f);
			this._playerMarker.rectTransform.sizeDelta = Vector2.one * 24f;
			this._playerMarker.rectTransform.localScale = Vector3.one * 1.4f;
			this._youLabel = this.Text("Ciel Label", this._viewport, "CIEL", 14f, Color.white);
			this._youLabel.rectTransform.anchorMin = (this._youLabel.rectTransform.anchorMax = Vector2.one * 0.5f);
			this._youLabel.rectTransform.sizeDelta = new Vector2(80f, 24f);
			this._youLabel.fontStyle = FontStyles.Bold;
			this._youLabel.gameObject.AddComponent<Outline>().effectColor = MapsPhoneApp.Background;
			this._youLabel.gameObject.SetActive(false);
			Image image4 = OxGramUI.Image("Orientation Strip", this._viewport, new Color(MapsPhoneApp.Background.r, MapsPhoneApp.Background.g, MapsPhoneApp.Background.b, 0.94f), null);
			this._orientationStrip = image4.rectTransform;
			MapsPhoneApp.Position(this._orientationStrip, 0f, 0f, 444f, 44f);
			image4.raycastTarget = false;
			this._orientationStrip.gameObject.SetActive(false);
			TMP_Text tmp_Text2 = this.Text("North", this._viewport, "N", 22f, MapsPhoneApp.Accent);
			MapsPhoneApp.Position(tmp_Text2.rectTransform, 8f, 8f, 36f, 36f);
			tmp_Text2.fontStyle = FontStyles.Bold;
			tmp_Text2.gameObject.AddComponent<Outline>().effectColor = MapsPhoneApp.Background;
			this._scaleLabel = this.Text("Scale", this._viewport, "", 12f, Color.white);
			MapsPhoneApp.Position(this._scaleLabel.rectTransform, 264f, 8f, 168f, 28f);
			this._scaleLabel.gameObject.AddComponent<Outline>().effectColor = MapsPhoneApp.Background;
			this._homeBearing.SetAsLastSibling();
			this._legend = this.Text("Legend", this._content, "NORTH IS UP  /  PINK ARROW = YOU\nHOUSE = HOME  /  ICONS = PLACES", 13f, MapsPhoneApp.Muted);
			MapsPhoneApp.Position(this._legend.rectTransform, 18f, 803f, 444f, 43f);
			this._mapDetail = this.Text("Map Detail", this._content, "Faint roads show the surrounding city.", 12f, MapsPhoneApp.Muted);
			MapsPhoneApp.Position(this._mapDetail.rectTransform, 18f, 852f, 444f, 24f);
			Button button = this.Button("Change View", this._content, "SHOW STREETS", new UnityAction(this.ToggleView), true);
			MapsPhoneApp.Position((RectTransform)button.transform, 18f, 889f, 444f, 52f);
			this._viewLabel = button.GetComponentInChildren<TMP_Text>();
			this._status = this.Text("Walking Status", this._content, "", 13f, MapsPhoneApp.Accent);
			MapsPhoneApp.Position(this._status.rectTransform, 18f, 951f, 444f, 28f);
			this.FitContent();
		}

		// Token: 0x0600A1C1 RID: 41409 RVA: 0x00432FAE File Offset: 0x004311AE
		private T Graphic<T>(string name, Transform parent) where T : MaskableGraphic
		{
			RectTransform rectTransform = OxGramUI.Rect(name, parent);
			OxGramUI.Stretch(rectTransform, 0f, 0f, 0f, 0f);
			T t = rectTransform.gameObject.AddComponent<T>();
			t.raycastTarget = false;
			return t;
		}

		// Token: 0x0600A1C2 RID: 41410 RVA: 0x00432FE8 File Offset: 0x004311E8
		private TMP_Text Text(string name, Transform parent, string value, float size, Color color)
		{
			TMP_Text tmp_Text = OxGramUI.Text(name, parent, value, this._font, size, color, TextAlignmentOptions.Center);
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			LocalizedGameText.BindSource(tmp_Text, "maps_ui", value, null).FitTranslationToRectangle(false);
			return tmp_Text;
		}

		// Token: 0x0600A1C3 RID: 41411 RVA: 0x00433022 File Offset: 0x00431222
		private static void SetText(TMP_Text label, string english)
		{
			LocalizedGameText.BindSource(label, "maps_ui", english, null);
		}

		// Token: 0x0600A1C4 RID: 41412 RVA: 0x00433034 File Offset: 0x00431234
		private static void SetBearing(TMP_Text label, string template, string bearing)
		{
			LocalizedGameText.BindComputed(label, () => GameLocalization.FormatSource("maps_ui", template, new object[] { GameLocalization.GetSource("maps_ui", bearing) }));
		}

		// Token: 0x0600A1C5 RID: 41413 RVA: 0x00433068 File Offset: 0x00431268
		private Button Button(string name, Transform parent, string label, UnityAction onClick, bool filled)
		{
			Image image = OxGramUI.Image(name, parent, filled ? MapsPhoneApp.Accent : Color.clear, PhoneUiSpriteFactory.Rounded);
			Button button = OxGramUI.Button(image.gameObject);
			TMP_Text tmp_Text = this.Text("Label", image.transform, label, 18f, filled ? MapsPhoneApp.Background : MapsPhoneApp.Accent);
			tmp_Text.fontStyle = FontStyles.Bold;
			OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
			button.onClick.AddListener(onClick);
			return button;
		}

		// Token: 0x0600A1C6 RID: 41414 RVA: 0x004330F8 File Offset: 0x004312F8
		private static void Position(RectTransform rect, float left, float top, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(left, -top);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x0600A1C7 RID: 41415 RVA: 0x00433158 File Offset: 0x00431358
		private void EnsureHomeUnderneath()
		{
			if (base.transform.parent == null)
			{
				return;
			}
			foreach (object obj in base.transform.parent)
			{
				Transform transform = (Transform)obj;
				if (!(transform == base.transform))
				{
					if (transform.name == "Home")
					{
						transform.gameObject.SetActive(true);
					}
					else if (transform.name.StartsWith("m", StringComparison.Ordinal) || transform.GetComponent<Animator>() != null || transform.GetComponent<PhotoGallery>() != null || transform.GetComponent<OxGramApp>() != null)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x0400A54F RID: 42319
		private static readonly Color Background = new Color(0.035f, 0.06f, 0.09f);

		// Token: 0x0400A550 RID: 42320
		private static readonly Color Panel = new Color(0.07f, 0.105f, 0.145f);

		// Token: 0x0400A551 RID: 42321
		private static readonly Color Accent = new Color(0.4f, 0.84f, 0.83f);

		// Token: 0x0400A552 RID: 42322
		private static readonly Color Muted = new Color(0.65f, 0.74f, 0.8f);

		// Token: 0x0400A553 RID: 42323
		private readonly MapsLiveCitySnapshot _snapshot = new MapsLiveCitySnapshot();

		// Token: 0x0400A554 RID: 42324
		private SmartphoneController _owner;

		// Token: 0x0400A555 RID: 42325
		private TMP_FontAsset _font;

		// Token: 0x0400A556 RID: 42326
		private RectTransform _content;

		// Token: 0x0400A557 RID: 42327
		private RectTransform _viewport;

		// Token: 0x0400A558 RID: 42328
		private ProceduralMiniMapGraphic _roads;

		// Token: 0x0400A559 RID: 42329
		private ProceduralMiniMapGraphic _previewRoads;

		// Token: 0x0400A55A RID: 42330
		private ProceduralMiniMapPoiGraphic _pois;

		// Token: 0x0400A55B RID: 42331
		private ProceduralMiniMapMarkerGraphic _playerMarker;

		// Token: 0x0400A55C RID: 42332
		private RectTransform _homeMarker;

		// Token: 0x0400A55D RID: 42333
		private RectTransform _homeBearing;

		// Token: 0x0400A55E RID: 42334
		private RectTransform _playerHalo;

		// Token: 0x0400A55F RID: 42335
		private RectTransform _orientationStrip;

		// Token: 0x0400A560 RID: 42336
		private TMP_Text _homeLabel;

		// Token: 0x0400A561 RID: 42337
		private TMP_Text _youLabel;

		// Token: 0x0400A562 RID: 42338
		private TMP_Text _objective;

		// Token: 0x0400A563 RID: 42339
		private TMP_Text _distance;

		// Token: 0x0400A564 RID: 42340
		private TMP_Text _direction;

		// Token: 0x0400A565 RID: 42341
		private TMP_Text _status;

		// Token: 0x0400A566 RID: 42342
		private TMP_Text _viewLabel;

		// Token: 0x0400A567 RID: 42343
		private TMP_Text _scaleLabel;

		// Token: 0x0400A568 RID: 42344
		private TMP_Text _legend;

		// Token: 0x0400A569 RID: 42345
		private TMP_Text _mapDetail;

		// Token: 0x0400A56A RID: 42346
		private Animator _animator;

		// Token: 0x0400A56B RID: 42347
		private DynamicIslandController _island;

		// Token: 0x0400A56C RID: 42348
		private ResourceRequest _atlasRequest;

		// Token: 0x0400A56D RID: 42349
		private ProceduralMiniMapStyleAtlas _atlas;

		// Token: 0x0400A56E RID: 42350
		private bool _registered;

		// Token: 0x0400A56F RID: 42351
		private bool _streetView;

		// Token: 0x0400A570 RID: 42352
		private bool? _presentedLongWayHome;

		// Token: 0x0400A571 RID: 42353
		private float _nextSnapshot;

		// Token: 0x0400A572 RID: 42354
		private float _nextView;

		// Token: 0x0400A573 RID: 42355
		private Vector2 _lastRootSize;
	}
}
