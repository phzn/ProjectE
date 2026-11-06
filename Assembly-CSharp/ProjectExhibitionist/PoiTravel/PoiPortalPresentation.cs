using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x0200095D RID: 2397
	[DisallowMultipleComponent]
	public sealed class PoiPortalPresentation : MonoBehaviour
	{
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x00183E62 File Offset: 0x00182062
		private bool IsHomeReturn
		{
			get
			{
				return this._mode == PoiScenePortal.PortalMode.ReturnHome;
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00183E6D File Offset: 0x0018206D
		public void SetHomeCardAnchor(Vector3 worldPosition)
		{
			this._homeCardAnchor = base.transform.InverseTransformPoint(worldPosition);
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x00183E81 File Offset: 0x00182081
		public bool IsWorldSpace
		{
			get
			{
				return this._canvas != null && this._canvas.renderMode == RenderMode.WorldSpace;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06004161 RID: 16737 RVA: 0x00183EA1 File Offset: 0x001820A1
		public float AwarenessRadius
		{
			get
			{
				return this._awarenessRadius;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06004162 RID: 16738 RVA: 0x00183EA9 File Offset: 0x001820A9
		public string DisplayedTitle
		{
			get
			{
				if (!(this._titleText != null))
				{
					return string.Empty;
				}
				return this._titleText.text;
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00183ECC File Offset: 0x001820CC
		public void Configure(PoiScenePortal.PortalMode mode, string displayName, string inputLabel, float interactionRadius, float awarenessRadius)
		{
			if (this._configured && this.IsHomeReturn != (mode == PoiScenePortal.PortalMode.ReturnHome))
			{
				this._visualRoot.gameObject.SetActive(false);
				PoiPortalPresentation.DestroyRuntimeObject(this._visualRoot.gameObject);
				this.OnDestroy();
				this._markerRenderers.Clear();
				this._markerRoot = (this._orbitRoot = null);
				this._configured = false;
			}
			this._mode = mode;
			this._displayName = PoiPortalPresentation.CleanDisplayName(displayName);
			this._inputLabel = (string.IsNullOrWhiteSpace(inputLabel) ? "E" : inputLabel);
			this._interactionRadius = Mathf.Max(0.5f, interactionRadius);
			this._awarenessRadius = (this.IsHomeReturn ? (this._interactionRadius + 0.75f) : Mathf.Max(this._interactionRadius + 4f, awarenessRadius));
			this._accent = PoiPortalPresentation.PortalAccent;
			this._secondary = PoiPortalPresentation.PortalSecondary;
			if (!this._configured)
			{
				this.BuildVisuals();
				this._configured = true;
			}
			this.ApplyPaletteAndCopy();
			this.SetContextActive(false);
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00183FD8 File Offset: 0x001821D8
		public void SetContextActive(bool active)
		{
			this._contextActive = active;
			if (!active)
			{
				this._interactionAvailable = false;
				this.SetMarkerVisible(false);
				if (this._canvas != null)
				{
					this._canvas.enabled = false;
				}
			}
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0018400C File Offset: 0x0018220C
		public void SetInteractionAvailable(bool available)
		{
			this._interactionAvailable = available;
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00184018 File Offset: 0x00182218
		public void SetPlayerState(Transform player, float sqrDistance, bool winner, bool committed, string inputLabel, bool blocked = false)
		{
			this._player = player;
			this._distance = ((player != null && !float.IsInfinity(sqrDistance)) ? Mathf.Sqrt(Mathf.Max(0f, sqrDistance)) : float.MaxValue);
			this._interactionAvailable = winner;
			this._blocked = blocked;
			this._committed = committed;
			if (!string.IsNullOrWhiteSpace(inputLabel) && inputLabel != this._inputLabel)
			{
				this._inputLabel = inputLabel;
				if (this._keyText != null)
				{
					this.BindPortalKey();
				}
			}
			this.RefreshHint(false);
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x001840AC File Offset: 0x001822AC
		public void PlayCommit()
		{
			this._committed = true;
			this.RefreshHint(true);
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x001840BC File Offset: 0x001822BC
		public void CancelCommit()
		{
			this._committed = false;
			this.RefreshHint(true);
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x001840CC File Offset: 0x001822CC
		private void LateUpdate()
		{
			if (!this._configured)
			{
				return;
			}
			bool flag = this._contextActive && this._player != null && this._distance <= this._awarenessRadius * 1.2f;
			this.SetMarkerVisible(flag);
			float num = this.CalculateTargetCanvasAlpha();
			if (this.IsHomeReturn && this._distance > this._awarenessRadius)
			{
				this._canvasAlpha = 0f;
			}
			this._canvasAlpha = Mathf.MoveTowards(this._canvasAlpha, num, Time.unscaledDeltaTime * 4.5f);
			if (this._canvas != null)
			{
				this._canvas.enabled = this._contextActive && this._canvasAlpha > 0.01f;
				this._canvasGroup.alpha = this._canvasAlpha;
			}
			if (!this._contextActive)
			{
				return;
			}
			this.ResolveCamera();
			this.BillboardAndScale();
			this.AnimateMarker();
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x001841BC File Offset: 0x001823BC
		private float CalculateTargetCanvasAlpha()
		{
			if (!this._contextActive || this._player == null || this._committed)
			{
				return 0f;
			}
			float num = Mathf.InverseLerp(this._awarenessRadius * 0.88f, this._awarenessRadius, this._distance);
			return (1f - Mathf.SmoothStep(0f, 1f, num)) * (this._interactionAvailable ? 1f : 0.86f);
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x00184238 File Offset: 0x00182438
		private void ResolveCamera()
		{
			Camera main = Camera.main;
			if (this._camera == null || !this._camera.isActiveAndEnabled || (main != null && main != this._camera))
			{
				this._camera = main;
			}
			if (this._canvas != null && this._canvas.worldCamera != this._camera)
			{
				this._canvas.worldCamera = this._camera;
			}
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x001842BC File Offset: 0x001824BC
		private void BillboardAndScale()
		{
			if (this._camera == null || this._canvasRect == null)
			{
				return;
			}
			Vector3 vector = this._visualRoot.TransformPoint(this.IsHomeReturn ? this._homeCardAnchor : new Vector3(0f, 2.18f, 0f));
			Vector3 vector2 = Vector3.ProjectOnPlane(this._camera.transform.position - vector, Vector3.up);
			if (vector2.sqrMagnitude > 0.001f)
			{
				vector2.Normalize();
				this._canvasRect.position = vector;
				this._canvasRect.rotation = Quaternion.LookRotation(-vector2, Vector3.up);
			}
			else
			{
				this._canvasRect.position = vector;
			}
			float num = Mathf.Lerp(0.88f, 1.7f, Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(5f, this._awarenessRadius, this._distance)));
			float num2 = (this._interactionAvailable ? (1f + Mathf.Sin(Time.unscaledTime * 4.2f) * 0.018f) : 1f);
			this._canvasRect.localScale = Vector3.one * (this.IsHomeReturn ? 0.0017f : (0.0034f * num * num2));
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00184410 File Offset: 0x00182610
		private void AnimateMarker()
		{
			if (this._markerRoot == null || !this._markerRoot.gameObject.activeSelf)
			{
				return;
			}
			float num = ((this._mode == PoiScenePortal.PortalMode.VisitPoi) ? 1f : (-1f));
			if (this._orbitRoot != null)
			{
				this._orbitRoot.Rotate(0f, num * 18f * Time.unscaledDeltaTime, 0f, Space.Self);
			}
			float num2 = 1f + Mathf.Sin(Time.unscaledTime * 2.4f) * 0.035f;
			float num3 = 0.75f * num2;
			this._markerRoot.localScale = new Vector3(num3, 1f, num3);
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x001844C0 File Offset: 0x001826C0
		private void BuildVisuals()
		{
			this._visualRoot = new GameObject("__POI_PORTAL_PRESENTATION_RUNTIME").transform;
			this._visualRoot.SetParent(base.transform, false);
			this._visualRoot.gameObject.layer = base.gameObject.layer;
			if (!this.IsHomeReturn)
			{
				this.BuildMarker();
			}
			this.BuildCanvas();
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x00184524 File Offset: 0x00182724
		private void BuildMarker()
		{
			this._markerRoot = new GameObject("Luminous Threshold").transform;
			this._markerRoot.SetParent(this._visualRoot, false);
			this._markerRoot.localPosition = new Vector3(0f, 0.035f, 0f);
			this._markerRoot.localScale = new Vector3(0.75f, 1f, 0.75f);
			this._haloMaterial = this.CreateMarkerMaterial("POI Halo", true);
			this._coreMaterial = this.CreateMarkerMaterial("POI Core", false);
			this._orbitMaterial = this.CreateMarkerMaterial("POI Orbit", true);
			this.CreateMarkerMesh("Soft Halo", this._markerRoot, this.BuildAnnulus(0.92f, 1.62f, 64), this._haloMaterial, Vector3.zero);
			this.CreateMarkerMesh("Threshold Ring", this._markerRoot, this.BuildAnnulus(1.31f, 1.42f, 64), this._coreMaterial, new Vector3(0f, 0.012f, 0f));
			this._orbitRoot = new GameObject("Orbiting Segments").transform;
			this._orbitRoot.SetParent(this._markerRoot, false);
			this.CreateMarkerMesh("Segments", this._orbitRoot, this.BuildSegmentedAnnulus(1.52f, 1.68f, 12, 0.56f), this._orbitMaterial, new Vector3(0f, 0.024f, 0f));
			GameObject gameObject = new GameObject("Locator Spine");
			gameObject.transform.SetParent(this._markerRoot, false);
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.useWorldSpace = false;
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, new Vector3(0f, 0.08f, 0f));
			lineRenderer.SetPosition(1, new Vector3(0f, 1.92f, 0f));
			lineRenderer.startWidth = 0.026f;
			lineRenderer.endWidth = 0.006f;
			lineRenderer.numCapVertices = 2;
			lineRenderer.alignment = LineAlignment.View;
			lineRenderer.sharedMaterial = this._orbitMaterial;
			lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;
			this._markerRenderers.Add(lineRenderer);
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00184750 File Offset: 0x00182950
		private void BuildCanvas()
		{
			GameObject gameObject = new GameObject("Destination Wayfinder", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasGroup)
			});
			gameObject.transform.SetParent(this._visualRoot, false);
			gameObject.transform.localPosition = new Vector3(0f, 2.32f, 0f);
			gameObject.layer = base.gameObject.layer;
			this._canvasRect = (RectTransform)gameObject.transform;
			this._canvasRect.sizeDelta = new Vector2(460f, 128f);
			this._canvasRect.localScale = Vector3.one * (this.IsHomeReturn ? 0.0017f : 0.0034f);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.WorldSpace;
			this._canvas.overrideSorting = false;
			this._canvas.sortingOrder = 0;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			this._canvasGroup = gameObject.GetComponent<CanvasGroup>();
			this._canvasGroup.alpha = 0f;
			this._canvasGroup.interactable = false;
			this._canvasGroup.blocksRaycasts = false;
			this.AddImage("Outer Glow", this._canvasRect, new Vector2(468f, 136f), Vector2.zero, new Color(1f, 1f, 1f, 0.12f));
			this.AddImage("Signal Frame", this._canvasRect, new Vector2(458f, 126f), Vector2.zero, Color.white);
			this.AddImage("Deep Panel", this._canvasRect, new Vector2(450f, 118f), Vector2.zero, PoiPortalPresentation.PortalPanel);
			this._patternImage = this.AddRawImage("Ciel Signature Pattern", this._canvasRect, new Vector2(442f, 110f), Vector2.zero, PoiPortalPresentation.PortalPatternTint);
			PoiPortalPresentation.ConfigurePattern(this._patternImage);
			RectTransform rectTransform = this.AddImage("Destination Diamond", this._canvasRect, new Vector2(50f, 50f), new Vector2(-180f, 1f), Color.white);
			rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f);
			this.AddImage("Diamond Core", rectTransform, new Vector2(36f, 36f), Vector2.zero, PoiPortalPresentation.PortalPanel).localRotation = Quaternion.identity;
			this.AddImage("Diamond Dot", rectTransform, new Vector2(11f, 11f), Vector2.zero, Color.white).localRotation = Quaternion.identity;
			TMP_FontAsset tmp_FontAsset = this.ResolveFont();
			this._eyebrowText = this.AddText("Mode", this._canvasRect, new Vector2(300f, 20f), new Vector2(25f, 38f), 16f, FontStyles.Bold, tmp_FontAsset, TextAlignmentOptions.MidlineLeft);
			this._eyebrowText.characterSpacing = 5f;
			this._titleText = this.AddText("Destination", this._canvasRect, new Vector2(320f, 50f), new Vector2(35f, 2f), 35f, FontStyles.Normal, tmp_FontAsset, TextAlignmentOptions.MidlineLeft);
			this._titleText.overflowMode = TextOverflowModes.Overflow;
			this._hintText = this.AddText("Hint", this._canvasRect, new Vector2(280f, 22f), new Vector2(15f, -39f), 15f, FontStyles.Bold, tmp_FontAsset, TextAlignmentOptions.MidlineLeft);
			this._hintText.gameObject.SetActive(!this.IsHomeReturn);
			RectTransform rectTransform2 = this.AddImage("Input Chip", this._canvasRect, new Vector2(52f, 34f), new Vector2(189f, -36f), Color.white);
			this._keyChip = rectTransform2.gameObject;
			this._keyText = this.AddText("Key", rectTransform2, new Vector2(48f, 30f), Vector2.zero, 19f, FontStyles.Bold, tmp_FontAsset, TextAlignmentOptions.Center);
			this._keyText.text = this._inputLabel;
			this._keyChip.SetActive(false);
			if (this.IsHomeReturn)
			{
				this.ApplyHomeOverlayMaterials();
			}
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00184BB4 File Offset: 0x00182DB4
		private void ApplyHomeOverlayMaterials()
		{
			Shader shader = Resources.Load<Shader>("HomeReturnOverlay");
			Shader shader2 = Resources.Load<Shader>("HomeReturnTextOverlay");
			if (shader == null || shader2 == null)
			{
				return;
			}
			Material material = new Material(shader)
			{
				name = "Home Return UI",
				hideFlags = HideFlags.HideAndDontSave
			};
			this._ownedMaterials.Add(material);
			Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
			foreach (Graphic graphic in this._canvasRect.GetComponentsInChildren<Graphic>(true))
			{
				TMP_Text tmp_Text = graphic as TMP_Text;
				if (tmp_Text != null)
				{
					Material fontSharedMaterial = tmp_Text.fontSharedMaterial;
					if (!(fontSharedMaterial == null))
					{
						Material material2;
						if (!dictionary.TryGetValue(fontSharedMaterial, out material2))
						{
							material2 = new Material(fontSharedMaterial)
							{
								shader = shader2,
								name = "Home Return Text",
								hideFlags = HideFlags.HideAndDontSave
							};
							material2.renderQueue = 4000;
							dictionary.Add(fontSharedMaterial, material2);
							this._ownedMaterials.Add(material2);
						}
						tmp_Text.fontSharedMaterial = material2;
					}
				}
				else
				{
					graphic.material = material;
				}
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x00184CCC File Offset: 0x00182ECC
		private RectTransform AddImage(string objectName, Transform parent, Vector2 size, Vector2 position, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.layer = parent.gameObject.layer;
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = size;
			rectTransform.anchoredPosition = position;
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return rectTransform;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x00184D98 File Offset: 0x00182F98
		private RawImage AddRawImage(string objectName, Transform parent, Vector2 size, Vector2 position, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.layer = parent.gameObject.layer;
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = size;
			rectTransform.anchoredPosition = position;
			RawImage component = gameObject.GetComponent<RawImage>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x00184E64 File Offset: 0x00183064
		public static void ConfigurePattern(RawImage image)
		{
			if (image == null)
			{
				return;
			}
			PoiArrivalCatalogSO poiArrivalCatalogSO = Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog");
			Sprite sprite = ((poiArrivalCatalogSO != null) ? poiArrivalCatalogSO.PortalPattern : null);
			if (sprite == null || sprite.texture == null)
			{
				image.gameObject.SetActive(false);
				return;
			}
			image.texture = sprite.texture;
			Rect textureRect = sprite.textureRect;
			Rect rect = new Rect(textureRect.x / (float)sprite.texture.width, textureRect.y / (float)sprite.texture.height, textureRect.width / (float)sprite.texture.width, textureRect.height / (float)sprite.texture.height);
			float num = textureRect.width / Mathf.Max(1f, textureRect.height);
			RectTransform rectTransform = image.rectTransform;
			float num2 = rectTransform.rect.width / Mathf.Max(1f, rectTransform.rect.height);
			if (num2 > num)
			{
				float num3 = rect.height * num / num2;
				rect.y += (rect.height - num3) * 0.5f;
				rect.height = num3;
			}
			else if (num2 < num)
			{
				float num4 = rect.width * num2 / num;
				rect.x += (rect.width - num4) * 0.5f;
				rect.width = num4;
			}
			image.uvRect = rect;
			image.gameObject.SetActive(true);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00185004 File Offset: 0x00183204
		private TextMeshProUGUI AddText(string objectName, Transform parent, Vector2 size, Vector2 position, float fontSize, FontStyles style, TMP_FontAsset font, TextAlignmentOptions alignment = TextAlignmentOptions.MidlineLeft)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.layer = parent.gameObject.layer;
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = size;
			rectTransform.anchoredPosition = position;
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			if (font != null)
			{
				component.font = font;
			}
			component.fontSize = fontSize;
			component.fontStyle = style;
			component.alignment = alignment;
			component.enableWordWrapping = false;
			component.overflowMode = TextOverflowModes.Ellipsis;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x00185100 File Offset: 0x00183300
		private void ApplyPaletteAndCopy()
		{
			foreach (Image image in this._canvasRect.GetComponentsInChildren<Image>(true))
			{
				if (image.name == "Deep Panel")
				{
					image.color = PoiPortalPresentation.PortalPanel;
				}
				else if (image.name == "Diamond Core")
				{
					Color portalPanel = PoiPortalPresentation.PortalPanel;
					portalPanel.a = 1f;
					image.color = portalPanel;
				}
				else if (image.name == "Outer Glow")
				{
					Color accent = this._accent;
					accent.a = 0.16f;
					image.color = accent;
				}
				else if (image.name == "Diamond Dot")
				{
					image.color = this._secondary;
				}
				else
				{
					image.color = this._accent;
				}
			}
			this.ConfigurePortalBinding(LocalizedGameText.BindComputed(this._eyebrowText, delegate
			{
				if (this.IsHomeReturn)
				{
					return GameLocalization.GetSource("portal_ui", "HOME  /  FINISH YOUR RUN");
				}
				if (this._mode != PoiScenePortal.PortalMode.VisitPoi)
				{
					return GameLocalization.FormatSource("portal_ui", "WAY OUT  /  {0}", new object[] { NavigationLocalization.PlaceName(this._displayName).ToUpperInvariant() });
				}
				return GameLocalization.GetSource("portal_ui", "POINT OF INTEREST");
			}));
			this.ConfigurePortalBinding(LocalizedGameText.BindComputed(this._titleText, delegate
			{
				if (this.IsHomeReturn)
				{
					return GameLocalization.GetSource("portal_ui", "Return <b>home</b>");
				}
				if (this._mode != PoiScenePortal.PortalMode.VisitPoi)
				{
					return GameLocalization.GetSource("portal_ui", PoiTravelState.IsFastTravelVisit ? "Return to <b>Home</b>" : "Return to <b>City</b>");
				}
				return GameLocalization.FormatSource("portal_ui", "Visit <b>{0}</b>", new object[] { NavigationLocalization.PlaceName(this._displayName) });
			}));
			this._eyebrowText.color = this._secondary;
			if (this._patternImage != null)
			{
				this._patternImage.color = PoiPortalPresentation.PortalPatternTint;
			}
			this._titleText.color = PoiPortalPresentation.PortalTitle;
			this._hintText.color = PoiPortalPresentation.PortalText;
			this._keyText.color = PoiPortalPresentation.PortalPanel;
			PoiPortalPresentation.SetMaterialColor(this._haloMaterial, this._accent * 2.2f, 0.13f);
			PoiPortalPresentation.SetMaterialColor(this._coreMaterial, this._accent * 2.8f, 0.92f);
			PoiPortalPresentation.SetMaterialColor(this._orbitMaterial, this._secondary * 3.2f, 0.78f);
			this.RefreshHint(true);
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x001852E0 File Offset: 0x001834E0
		private void RefreshHint(bool force = false)
		{
			if (this._hintText == null)
			{
				return;
			}
			if (this.IsHomeReturn)
			{
				bool flag = this._blocked && !this._committed && this._distance <= this._interactionRadius * 1.35f;
				this.BindPortalHint(flag ? "FINISH CURRENT ACTION" : string.Empty);
				this._hintText.gameObject.SetActive(flag);
				if (!flag)
				{
					this.BindPortalKey();
				}
				this._keyChip.SetActive(!this._committed && !flag);
				this._keyText.color = ((this._interactionAvailable && !this._blocked) ? PoiPortalPresentation.PortalPanel : PoiPortalPresentation.PortalText);
				this._keyChip.GetComponent<Image>().color = ((this._interactionAvailable && !this._blocked) ? PoiPortalPresentation.PortalAccent : new Color32(69, 48, 81, byte.MaxValue));
				return;
			}
			int metres = ((this._distance < float.MaxValue) ? Mathf.CeilToInt(this._distance) : (-1));
			if (!force && metres == this._shownMetres && this._interactionAvailable == this._shownInteractionAvailable && this._blocked == this._shownBlocked && this._committed == this._shownCommitted)
			{
				return;
			}
			this._shownMetres = metres;
			this._shownInteractionAvailable = this._interactionAvailable;
			this._shownBlocked = this._blocked;
			this._shownCommitted = this._committed;
			if (this._committed)
			{
				this.BindPortalHint((this._mode == PoiScenePortal.PortalMode.VisitPoi) ? "OPENING DESTINATION..." : "RETURNING...");
				this._keyChip.SetActive(false);
				return;
			}
			bool flag2 = this._distance <= this._interactionRadius * 1.35f;
			if (this._blocked && flag2)
			{
				this.BindPortalHint("FINISH CURRENT ACTION");
				this._keyChip.SetActive(false);
				return;
			}
			if (this._interactionAvailable)
			{
				this.BindPortalHint((this._mode == PoiScenePortal.PortalMode.VisitPoi) ? "ENTER DESTINATION" : "LEAVE POINT OF INTEREST");
				this.BindPortalKey();
				this._keyChip.SetActive(true);
				return;
			}
			string kind = ((this._mode == PoiScenePortal.PortalMode.VisitPoi) ? "ENTRANCE" : "EXIT");
			this.ConfigurePortalBinding(LocalizedGameText.BindComputed(this._hintText, delegate
			{
				if (metres < 0)
				{
					return GameLocalization.GetSource("portal_ui", kind);
				}
				return GameLocalization.FormatSource("portal_ui", "{0}  /  {1} M", new object[]
				{
					GameLocalization.GetSource("portal_ui", kind),
					metres
				});
			}));
			this._keyChip.SetActive(false);
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x00185553 File Offset: 0x00183753
		private void BindPortalHint(string english)
		{
			this.ConfigurePortalBinding(LocalizedGameText.BindSource(this._hintText, "portal_ui", english, null));
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0018556D File Offset: 0x0018376D
		private void BindPortalKey()
		{
			InteractionLocalization.BindKey(this._keyText, this._inputLabel);
			this.ConfigurePortalBinding(this._keyText.GetComponent<LocalizedGameText>());
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00185594 File Offset: 0x00183794
		private void ConfigurePortalBinding(LocalizedGameText binding)
		{
			binding.FitTranslationToRectangle(false);
			if (!this.IsHomeReturn)
			{
				return;
			}
			TMP_FontAsset sourceFont = LocalizedGameText.SourceFont(binding.GetComponent<TMP_Text>());
			binding.SetMaterialResolver(delegate(TMP_FontAsset font)
			{
				if (font == sourceFont)
				{
					return null;
				}
				Material material;
				if (this._localizedOverlayMaterials.TryGetValue(font, out material))
				{
					return material;
				}
				Shader shader = Resources.Load<Shader>("HomeReturnTextOverlay");
				if (shader == null)
				{
					return null;
				}
				material = new Material(font.material)
				{
					shader = shader,
					name = "Home Return Localized Text",
					hideFlags = HideFlags.HideAndDontSave,
					renderQueue = 4000
				};
				this._localizedOverlayMaterials.Add(font, material);
				this._ownedMaterials.Add(material);
				return material;
			});
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x001855E4 File Offset: 0x001837E4
		private TMP_FontAsset ResolveFont()
		{
			PoiArrivalCatalogSO poiArrivalCatalogSO = Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog");
			if (poiArrivalCatalogSO != null && poiArrivalCatalogSO.PortalFont != null)
			{
				return poiArrivalCatalogSO.PortalFont;
			}
			if (TMP_Settings.defaultFontAsset != null)
			{
				return TMP_Settings.defaultFontAsset;
			}
			return Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x00185637 File Offset: 0x00183837
		private void SetMarkerVisible(bool visible)
		{
			if (this._markerRoot != null && this._markerRoot.gameObject.activeSelf != visible)
			{
				this._markerRoot.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0018566C File Offset: 0x0018386C
		private void CreateMarkerMesh(string objectName, Transform parent, Mesh mesh, Material material, Vector3 localPosition)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			gameObject.transform.SetParent(parent, false);
			gameObject.transform.localPosition = localPosition;
			gameObject.layer = base.gameObject.layer;
			gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.sharedMaterial = material;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			component.lightProbeUsage = LightProbeUsage.Off;
			component.reflectionProbeUsage = ReflectionProbeUsage.Off;
			this._markerRenderers.Add(component);
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0018570C File Offset: 0x0018390C
		private Material CreateMarkerMaterial(string materialName, bool additive)
		{
			PoiArrivalCatalogSO poiArrivalCatalogSO = Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog");
			Shader shader = ((poiArrivalCatalogSO != null) ? poiArrivalCatalogSO.PortalMarkerShader : null);
			if (shader == null)
			{
				shader = Shader.Find("Project Exhibitionist/POI Portal Marker");
			}
			if (shader == null)
			{
				shader = Shader.Find("Universal Render Pipeline/Unlit");
			}
			if (shader == null)
			{
				shader = Shader.Find("Unlit/Color");
			}
			Material material = new Material(shader)
			{
				name = materialName + " [Runtime]",
				hideFlags = HideFlags.DontSave,
				renderQueue = 3000
			};
			if (material.HasProperty("_Surface"))
			{
				material.SetFloat("_Surface", 1f);
			}
			if (material.HasProperty("_Blend"))
			{
				material.SetFloat("_Blend", additive ? 2f : 0f);
			}
			if (material.HasProperty("_SrcBlend"))
			{
				material.SetFloat("_SrcBlend", 5f);
			}
			if (material.HasProperty("_DstBlend"))
			{
				material.SetFloat("_DstBlend", additive ? 1f : 10f);
			}
			if (material.HasProperty("_ZWrite"))
			{
				material.SetFloat("_ZWrite", 0f);
			}
			if (material.HasProperty("_Cull"))
			{
				material.SetFloat("_Cull", 0f);
			}
			material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.SetShaderPassEnabled("ShadowCaster", false);
			this._ownedMaterials.Add(material);
			return material;
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x00185884 File Offset: 0x00183A84
		private static void SetMaterialColor(Material material, Color hdrColor, float alpha)
		{
			if (material == null)
			{
				return;
			}
			hdrColor.a = alpha;
			if (material.HasProperty("_BaseColor"))
			{
				material.SetColor("_BaseColor", hdrColor);
			}
			if (material.HasProperty("_Color"))
			{
				material.SetColor("_Color", hdrColor);
			}
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x001858D8 File Offset: 0x00183AD8
		private Mesh BuildAnnulus(float innerRadius, float outerRadius, int segments)
		{
			Vector3[] array = new Vector3[(segments + 1) * 2];
			Vector3[] array2 = new Vector3[array.Length];
			int[] array3 = new int[segments * 6];
			for (int i = 0; i <= segments; i++)
			{
				float num = 6.2831855f * (float)i / (float)segments;
				Vector3 vector = new Vector3(Mathf.Sin(num), 0f, Mathf.Cos(num));
				array[i * 2] = vector * innerRadius;
				array[i * 2 + 1] = vector * outerRadius;
				array2[i * 2] = Vector3.up;
				array2[i * 2 + 1] = Vector3.up;
				if (i != segments)
				{
					int num2 = i * 2;
					int num3 = i * 6;
					array3[num3] = num2;
					array3[num3 + 1] = num2 + 1;
					array3[num3 + 2] = num2 + 2;
					array3[num3 + 3] = num2 + 2;
					array3[num3 + 4] = num2 + 1;
					array3[num3 + 5] = num2 + 3;
				}
			}
			Mesh mesh = new Mesh
			{
				name = "POI Annulus [Runtime]",
				hideFlags = HideFlags.DontSave
			};
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.triangles = array3;
			mesh.RecalculateBounds();
			this._ownedMeshes.Add(mesh);
			return mesh;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00185A10 File Offset: 0x00183C10
		private Mesh BuildSegmentedAnnulus(float innerRadius, float outerRadius, int dashCount, float fill)
		{
			Vector3[] array = new Vector3[dashCount * 4];
			Vector3[] array2 = new Vector3[array.Length];
			int[] array3 = new int[dashCount * 6];
			float num = 6.2831855f / (float)dashCount;
			float num2 = num * Mathf.Clamp01(fill) * 0.5f;
			for (int i = 0; i < dashCount; i++)
			{
				float num3 = (float)i * num;
				Vector3 vector = new Vector3(Mathf.Sin(num3 - num2), 0f, Mathf.Cos(num3 - num2));
				Vector3 vector2 = new Vector3(Mathf.Sin(num3 + num2), 0f, Mathf.Cos(num3 + num2));
				int num4 = i * 4;
				array[num4] = vector * innerRadius;
				array[num4 + 1] = vector * outerRadius;
				array[num4 + 2] = vector2 * innerRadius;
				array[num4 + 3] = vector2 * outerRadius;
				for (int j = 0; j < 4; j++)
				{
					array2[num4 + j] = Vector3.up;
				}
				int num5 = i * 6;
				array3[num5] = num4;
				array3[num5 + 1] = num4 + 1;
				array3[num5 + 2] = num4 + 2;
				array3[num5 + 3] = num4 + 2;
				array3[num5 + 4] = num4 + 1;
				array3[num5 + 5] = num4 + 3;
			}
			Mesh mesh = new Mesh
			{
				name = "POI Segments [Runtime]",
				hideFlags = HideFlags.DontSave
			};
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.triangles = array3;
			mesh.RecalculateBounds();
			this._ownedMeshes.Add(mesh);
			return mesh;
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00185BA4 File Offset: 0x00183DA4
		private static string CleanDisplayName(string displayName)
		{
			string text = (string.IsNullOrWhiteSpace(displayName) ? "Point of Interest" : displayName.Trim());
			if (text.EndsWith(" Exit", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - " Exit".Length);
			}
			return text;
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00185BF0 File Offset: 0x00183DF0
		private void OnDestroy()
		{
			for (int i = 0; i < this._ownedMaterials.Count; i++)
			{
				PoiPortalPresentation.DestroyRuntimeObject(this._ownedMaterials[i]);
			}
			for (int j = 0; j < this._ownedMeshes.Count; j++)
			{
				PoiPortalPresentation.DestroyRuntimeObject(this._ownedMeshes[j]);
			}
			this._ownedMaterials.Clear();
			this._localizedOverlayMaterials.Clear();
			this._ownedMeshes.Clear();
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyRuntimeObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0400414A RID: 16714
		private readonly Dictionary<TMP_FontAsset, Material> _localizedOverlayMaterials = new Dictionary<TMP_FontAsset, Material>();

		// Token: 0x0400414B RID: 16715
		private const float BaseCanvasScale = 0.0034f;

		// Token: 0x0400414C RID: 16716
		private const float HomeCanvasScale = 0.0017f;

		// Token: 0x0400414D RID: 16717
		private const float MarkerHorizontalScale = 0.75f;

		// Token: 0x0400414E RID: 16718
		private const int UiWidth = 460;

		// Token: 0x0400414F RID: 16719
		private const int UiHeight = 128;

		// Token: 0x04004150 RID: 16720
		private static readonly Color PortalAccent = new Color32(169, 96, 201, byte.MaxValue);

		// Token: 0x04004151 RID: 16721
		private static readonly Color PortalSecondary = new Color32(199, 133, 242, byte.MaxValue);

		// Token: 0x04004152 RID: 16722
		private static readonly Color PortalPanel = new Color32(23, 15, 31, 245);

		// Token: 0x04004153 RID: 16723
		private static readonly Color PortalTitle = new Color32(242, 219, byte.MaxValue, byte.MaxValue);

		// Token: 0x04004154 RID: 16724
		private static readonly Color PortalText = new Color32(230, 219, 242, byte.MaxValue);

		// Token: 0x04004155 RID: 16725
		private static readonly Color PortalPatternTint = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 56);

		// Token: 0x04004156 RID: 16726
		private readonly List<Material> _ownedMaterials = new List<Material>(4);

		// Token: 0x04004157 RID: 16727
		private readonly List<Mesh> _ownedMeshes = new List<Mesh>(3);

		// Token: 0x04004158 RID: 16728
		private readonly List<Renderer> _markerRenderers = new List<Renderer>(4);

		// Token: 0x04004159 RID: 16729
		private PoiScenePortal.PortalMode _mode;

		// Token: 0x0400415A RID: 16730
		private string _displayName;

		// Token: 0x0400415B RID: 16731
		private string _inputLabel;

		// Token: 0x0400415C RID: 16732
		private float _interactionRadius;

		// Token: 0x0400415D RID: 16733
		private float _awarenessRadius;

		// Token: 0x0400415E RID: 16734
		private bool _configured;

		// Token: 0x0400415F RID: 16735
		private bool _contextActive;

		// Token: 0x04004160 RID: 16736
		private bool _interactionAvailable;

		// Token: 0x04004161 RID: 16737
		private bool _blocked;

		// Token: 0x04004162 RID: 16738
		private bool _committed;

		// Token: 0x04004163 RID: 16739
		private Transform _player;

		// Token: 0x04004164 RID: 16740
		private float _distance = float.MaxValue;

		// Token: 0x04004165 RID: 16741
		private float _canvasAlpha;

		// Token: 0x04004166 RID: 16742
		private int _shownMetres = -1;

		// Token: 0x04004167 RID: 16743
		private bool _shownInteractionAvailable;

		// Token: 0x04004168 RID: 16744
		private bool _shownBlocked;

		// Token: 0x04004169 RID: 16745
		private bool _shownCommitted;

		// Token: 0x0400416A RID: 16746
		private Camera _camera;

		// Token: 0x0400416B RID: 16747
		private Transform _visualRoot;

		// Token: 0x0400416C RID: 16748
		private Transform _markerRoot;

		// Token: 0x0400416D RID: 16749
		private Transform _orbitRoot;

		// Token: 0x0400416E RID: 16750
		private Canvas _canvas;

		// Token: 0x0400416F RID: 16751
		private CanvasGroup _canvasGroup;

		// Token: 0x04004170 RID: 16752
		private RectTransform _canvasRect;

		// Token: 0x04004171 RID: 16753
		private TextMeshProUGUI _eyebrowText;

		// Token: 0x04004172 RID: 16754
		private TextMeshProUGUI _titleText;

		// Token: 0x04004173 RID: 16755
		private TextMeshProUGUI _hintText;

		// Token: 0x04004174 RID: 16756
		private RawImage _patternImage;

		// Token: 0x04004175 RID: 16757
		private GameObject _keyChip;

		// Token: 0x04004176 RID: 16758
		private TextMeshProUGUI _keyText;

		// Token: 0x04004177 RID: 16759
		private Material _haloMaterial;

		// Token: 0x04004178 RID: 16760
		private Material _coreMaterial;

		// Token: 0x04004179 RID: 16761
		private Material _orbitMaterial;

		// Token: 0x0400417A RID: 16762
		private Color _accent;

		// Token: 0x0400417B RID: 16763
		private Color _secondary;

		// Token: 0x0400417C RID: 16764
		private Vector3 _homeCardAnchor = Vector3.up;
	}
}
