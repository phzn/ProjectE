using System;
using System.Collections.Generic;
using System.Globalization;
using CompassNavigatorPro;
using DA_Assets.CR;
using ExhibitionistDemo.UI;
using Features.Achievements;
using Features.Flash;
using Features.Localization;
using Features.MainMenu;
using Features.MatchRecap;
using Features.PlayerEmotes;
using Features.RenderingPrecision;
using Features.RunModes;
using Features.Score;
using Features.Suspicion;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.CoverWall;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009A6 RID: 2470
	[DefaultExecutionOrder(-350)]
	[DisallowMultipleComponent]
	public sealed class ProceduralMiniMapController : MonoBehaviour
	{
		// Token: 0x060044F2 RID: 17650 RVA: 0x0019CAB0 File Offset: 0x0019ACB0
		private void UpdateControllerMapReticle(bool visible)
		{
			visible &= this._expanded;
			if (visible && this._controllerReticle == null && this._mapPanel != null)
			{
				this._controllerReticle = new GameObject("Controller Map Centre", new Type[] { typeof(RectTransform) });
				RectTransform rectTransform = (RectTransform)this._controllerReticle.transform;
				rectTransform.SetParent(this._mapPanel, false);
				RectTransform rectTransform2 = rectTransform;
				Vector2 vector = (rectTransform.anchorMax = (rectTransform.pivot = Vector2.one * 0.5f));
				rectTransform2.anchorMin = vector;
				rectTransform.sizeDelta = Vector2.one * 22f;
				for (int i = 0; i < 2; i++)
				{
					GameObject gameObject = new GameObject("Reticle", new Type[]
					{
						typeof(RectTransform),
						typeof(Image)
					});
					RectTransform rectTransform3 = (RectTransform)gameObject.transform;
					rectTransform3.SetParent(rectTransform, false);
					rectTransform3.sizeDelta = ((i == 0) ? new Vector2(20f, 2f) : new Vector2(2f, 20f));
					Image component = gameObject.GetComponent<Image>();
					component.color = Color.white;
					component.raycastTarget = false;
				}
				this._controllerMapHint = new GameObject("Controller Map Controls", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				RectTransform rectTransform4 = (RectTransform)this._controllerMapHint.transform;
				rectTransform4.SetParent(this._mapPanel, false);
				vector = new Vector2(0.5f, 0f);
				rectTransform4.anchorMax = vector;
				rectTransform4.anchorMin = vector;
				rectTransform4.pivot = new Vector2(0.5f, 1f);
				rectTransform4.anchoredPosition = new Vector2(0f, -8f);
				rectTransform4.sizeDelta = new Vector2(680f, 54f);
				TextMeshProUGUI component2 = this._controllerMapHint.GetComponent<TextMeshProUGUI>();
				component2.fontSize = 17f;
				component2.alignment = TextAlignmentOptions.Center;
				component2.color = Color.white;
				component2.raycastTarget = false;
				this._controllerHintAllowsPan = null;
			}
			this.UpdateControllerMapHint();
			if (this._controllerReticle != null)
			{
				this._controllerReticle.SetActive(visible);
			}
			if (this._controllerMapHint != null)
			{
				this._controllerMapHint.SetActive(visible);
			}
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0019CD18 File Offset: 0x0019AF18
		private void UpdateControllerMapHint()
		{
			if (!(this._controllerMapHint == null))
			{
				bool? controllerHintAllowsPan = this._controllerHintAllowsPan;
				bool canPanExpandedMap = this.CanPanExpandedMap;
				if (!((controllerHintAllowsPan.GetValueOrDefault() == canPanExpandedMap) & (controllerHintAllowsPan != null)))
				{
					this._controllerHintAllowsPan = new bool?(this.CanPanExpandedMap);
					TextMeshProUGUI component = this._controllerMapHint.GetComponent<TextMeshProUGUI>();
					if (this.CanPanExpandedMap)
					{
						LocalizedGameText.BindSource(component, "navigation_ui", "LS: pan   LT / RT: zoom   R3: recenter\nX: place pin at +   Y: clear pin   B: back", null).FitTranslationToRectangle(false);
						return;
					}
					LocalizedGameText.Bind(component, "navigation.map.controller_without_pan", "LT / RT: zoom   R3: recenter\nX: place pin at +   Y: clear pin   B: back", null).FitTranslationToRectangle(false);
					return;
				}
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x0019CDAA File Offset: 0x0019AFAA
		public bool IsExpanded
		{
			get
			{
				return this._expanded;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x0019CDB2 File Offset: 0x0019AFB2
		public static bool IsMapExpanded
		{
			get
			{
				return ProceduralMiniMapController._instance != null && ProceduralMiniMapController._instance._expanded;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x060044F6 RID: 17654 RVA: 0x0019CDCD File Offset: 0x0019AFCD
		public bool VisualVisible
		{
			get
			{
				return this._visualVisible;
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0019CDD8 File Offset: 0x0019AFD8
		public void SetVisualVisible(bool visible)
		{
			this._visualVisible = visible;
			bool flag = visible || this._expanded;
			if (this._mapPanel != null)
			{
				if (this._visualGroup == null)
				{
					this._visualGroup = ProceduralMiniMapController.CreateVisualWrapper(this._mapPanel, "Map Visual Preference");
				}
				this._visualGroup.alpha = (flag ? 1f : 0f);
			}
			if (this._screenDim != null)
			{
				if (this._backdropVisualGroup == null)
				{
					this._backdropVisualGroup = ProceduralMiniMapController.CreateVisualWrapper(this._screenDim.rectTransform, "Map Backdrop Preference");
				}
				this._backdropVisualGroup.alpha = (flag ? 1f : 0f);
			}
			this.UpdateFreeSeedPresentation();
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x0019CE9C File Offset: 0x0019B09C
		private static CanvasGroup CreateVisualWrapper(RectTransform target, string name)
		{
			int siblingIndex = target.GetSiblingIndex();
			Vector2 anchorMin = target.anchorMin;
			Vector2 anchorMax = target.anchorMax;
			Vector2 pivot = target.pivot;
			Vector2 sizeDelta = target.sizeDelta;
			Vector3 anchoredPosition3D = target.anchoredPosition3D;
			Quaternion localRotation = target.localRotation;
			Vector3 localScale = target.localScale;
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			gameObject.layer = target.gameObject.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(target.parent, false);
			ProceduralMiniMapController.Stretch(component);
			component.SetSiblingIndex(siblingIndex);
			target.SetParent(component, false);
			target.anchorMin = anchorMin;
			target.anchorMax = anchorMax;
			target.pivot = pivot;
			target.sizeDelta = sizeDelta;
			target.anchoredPosition3D = anchoredPosition3D;
			target.localRotation = localRotation;
			target.localScale = localScale;
			return gameObject.GetComponent<CanvasGroup>();
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x060044F9 RID: 17657 RVA: 0x0019CF84 File Offset: 0x0019B184
		public static bool InitialRenderReady
		{
			get
			{
				return ProceduralMiniMapController._instance == null || !ProceduralMiniMapController._instance.isActiveAndEnabled || (!ProceduralMiniMapController._instance._chunkVisualsDirty && ProceduralMiniMapController._instance._snapshotQueue.Count == 0 && (ProceduralMiniMapController._styleAtlasRequest == null || ProceduralMiniMapController._styleAtlasRequest.isDone) && (ProceduralMiniMapController._hudStyleRequest == null || ProceduralMiniMapController._hudStyleRequest.isDone));
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x0019CFF4 File Offset: 0x0019B1F4
		public static bool CanOpenMap
		{
			get
			{
				PlayerContextResource playerContextResource;
				return !RunModeService.IsLongWayHome && (PlayerActivityGate.CanAcquireUtilityModal(PlayerActivity.AllPlayerActions, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation | PlayerContextResource.WaterMovement, out playerContextResource, "Rank") || (WallCoverActivityLock.ActiveController != null && WallCoverActivityLock.ActiveController.CanOpenModalMap)) && !GraphicsSettingsRuntimeUI.AnyOpen && !ClothActionGate.IsBusy;
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0019D04A File Offset: 0x0019B24A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			ProceduralMiniMapController._instance = null;
			ProceduralMiniMapController._styleAtlasRequest = null;
			ProceduralMiniMapController._preloadedStyleAtlas = null;
			ProceduralMiniMapController._hudStyleRequest = null;
			ProceduralMiniMapController._preloadedHudStyle = null;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x0019D06A File Offset: 0x0019B26A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void PreloadStyleAtlas()
		{
			ProceduralMiniMapController.BeginStyleAtlasLoad();
			ProceduralMiniMapController.BeginHudStyleLoad();
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x0019D076 File Offset: 0x0019B276
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (ProceduralMiniMapController._instance != null || Object.FindObjectOfType<ProceduralMiniMapController>(true) != null)
			{
				return;
			}
			new GameObject("[Procedural MiniMap Runtime]").AddComponent<ProceduralMiniMapController>();
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x0019D0A4 File Offset: 0x0019B2A4
		private void Awake()
		{
			if (ProceduralMiniMapController._instance != null && ProceduralMiniMapController._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			ProceduralMiniMapController._instance = this;
			base.transform.SetParent(null, false);
			Object.DontDestroyOnLoad(base.gameObject);
			this._previewCellComparison = new Comparison<Vector2Int>(this.ComparePreviewCells);
			this._snapshotCandidateComparison = new Comparison<ChunkInstance>(this.CompareSnapshotCandidates);
			this._visitedPoiKinds.Add(MiniMapPoiKind.Home);
			this.TryUnlockInteriorScene(SceneManager.GetActiveScene());
			this.TryCompleteStyleAtlasLoad();
			this.EnsureHudBinding();
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0019D140 File Offset: 0x0019B340
		private void OnEnable()
		{
			RunModeService.Changed -= this.RefreshFreeWorldSeed;
			RunModeService.Changed += this.RefreshFreeWorldSeed;
			this.RefreshFreeWorldSeed();
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			GameSettingsService.SettingsChanged -= this.RefreshVisualPreference;
			GameSettingsService.SettingsChanged += this.RefreshVisualPreference;
			this.RefreshVisualPreference();
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			SceneManager.sceneUnloaded += this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
			GameSystemsBootstrapper.OnGameplayReady -= this.HandleGameplayReady;
			GameSystemsBootstrapper.OnGameplayReady += this.HandleGameplayReady;
			MiniMapPoiMarker.Changed -= this.HandleMiniMapPoiMarkersChanged;
			MiniMapPoiMarker.Changed += this.HandleMiniMapPoiMarkersChanged;
			this.InvalidateEnvironmentBinding();
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x0019D260 File Offset: 0x0019B460
		private void OnDisable()
		{
			RunModeService.Changed -= this.RefreshFreeWorldSeed;
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			GameSettingsService.SettingsChanged -= this.RefreshVisualPreference;
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			GameSystemsBootstrapper.OnGameplayReady -= this.HandleGameplayReady;
			MiniMapPoiMarker.Changed -= this.HandleMiniMapPoiMarkersChanged;
			this.ClearPoiReadinessSubscriptions();
			this._expanded = false;
			this.ApplyMapInteractionLayer();
			this.ReleaseMapContext();
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x0019D30E File Offset: 0x0019B50E
		private void RefreshVisualPreference()
		{
			this.SetVisualVisible(GameSettingsService.ShowMinimap);
			this.UpdateWantedPlacement(this._wantedInPoi);
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x0019D327 File Offset: 0x0019B527
		private void OnDestroy()
		{
			this.UnbindChunkManager();
			this.BindPoiContext(null);
			this.BindStatsService(null);
			this.DisposeSnapshotResources();
			this.ReleaseMapContext();
			this.ClearUiReferences(true);
			if (ProceduralMiniMapController._instance == this)
			{
				ProceduralMiniMapController._instance = null;
			}
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x0019D364 File Offset: 0x0019B564
		private void Update()
		{
			bool flag = this.TryConsumeMapToggle(GameplayInput.GetKeyDown(KeyCode.M), Input.GetKey(KeyCode.M) || ControllerInput.GetKey(KeyCode.M), Time.frameCount, Time.unscaledTime);
			if (this.TryCompleteStyleAtlasLoad() && this._mapGraphic != null)
			{
				this._mapGraphic.SetStyleAtlas(this._styleAtlas);
				this.RebuildChunkVisuals();
			}
			if (this.EnvironmentBindingNeedsRefresh())
			{
				this.RefreshEnvironmentBinding();
			}
			if (this._canvas == null)
			{
				if (this._expanded)
				{
					this.SetExpanded(false);
				}
				return;
			}
			bool flag2 = GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.MinimapEnabled;
			bool flag3 = !RunModeService.IsLongWayHome && flag2 && this._chunkManager != null && this._chunkManager.isActiveAndEnabled && this._player != null;
			bool flag4 = this._player != null && ProceduralMiniMapController.ShouldShowFacecamWanted((SceneContext.Current != null) ? SceneContext.Current.Profile : null, RunModeService.IsLongWayHome);
			this.FitUiRootToHudCanvas();
			this.UpdateHudAvailability(flag3, flag4);
			this.UpdateWantedIndicator();
			this.UpdateSeedCopyFeedback();
			if (!flag3)
			{
				return;
			}
			if (Time.unscaledTime >= this._nextLegacyMapCheck)
			{
				this._nextLegacyMapCheck = Time.unscaledTime + 1f;
				this.DisableLegacyRenderedMiniMap();
			}
			this.HandleInput(flag);
			this.UpdateTransition();
			this.UpdateMapView();
			this.UpdatePinMarker();
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0019D4D8 File Offset: 0x0019B6D8
		private void LateUpdate()
		{
			if (this._chunkVisualsDirty && Time.unscaledTime >= this._nextChunkVisualRebuild)
			{
				this._chunkVisualsDirty = false;
				this.RebuildChunkVisuals();
			}
			if (this._snapshotQueue.Count == 0 || Time.unscaledTime < this._nextSnapshotTime)
			{
				return;
			}
			this.CaptureNextChunkSnapshot();
			this._nextSnapshotTime = Time.unscaledTime + 0.35f;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x0019D53C File Offset: 0x0019B73C
		private bool TryConsumeMapToggle(bool pressed, bool held, int frame, float unscaledTime)
		{
			bool mapToggleHeld = this._mapToggleHeld;
			this._mapToggleHeld = held || pressed;
			if (!pressed || mapToggleHeld || frame == this._lastMapToggleFrame || unscaledTime < this._nextMapToggleTime)
			{
				return false;
			}
			this._lastMapToggleFrame = frame;
			this._nextMapToggleTime = unscaledTime + 0.24f;
			return true;
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x0019D58C File Offset: 0x0019B78C
		private void HandleInput(bool mapToggleRequested)
		{
			this.UpdateControllerMapReticle(this._expanded && ControllerInput.IsControllerActive);
			if (GraphicsSettingsRuntimeUI.AnyOpen || ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if (TextInputFocus.BlocksGameplayInput)
			{
				return;
			}
			if (mapToggleRequested)
			{
				if (!this._expanded && !ProceduralMiniMapController.CanOpenMap)
				{
					return;
				}
				if (!this._expanded && TutorialService.TryShowFirstUse("minimap"))
				{
					return;
				}
				this.SetExpanded(!this._expanded);
				return;
			}
			else
			{
				if (!this._expanded)
				{
					return;
				}
				if (this._mapLease != null && !this._mapLease.IsActive)
				{
					this.SetExpanded(false);
					return;
				}
				if (EscapeInputCoordinator.TryConsume(this))
				{
					this.SetExpanded(false);
					return;
				}
				if (ControllerInput.IsControllerActive)
				{
					Vector2 navigate = ControllerInput.Navigate;
					if (navigate.sqrMagnitude > 0.04f)
					{
						this.SetPan(this._panOffset + this.LocalToWorldDelta(navigate * (420f * Time.unscaledDeltaTime), Mathf.Max(0.001f, this._currentPixelsPerMeter)));
					}
					float num = (ControllerInput.GetButton(ControllerButton.RightTrigger) ? 1f : 0f) - (ControllerInput.GetButton(ControllerButton.LeftTrigger) ? 1f : 0f);
					if (num != 0f)
					{
						this.SetExpandedZoom(Mathf.Clamp(this._expandedZoom * Mathf.Exp(num * Time.unscaledDeltaTime * 1.2f), 1f, 3f));
						this.SetPan(this._panOffset);
					}
					if (ControllerInput.GetButtonDown(ControllerButton.RightStick))
					{
						this.SetPan(Vector3.zero);
					}
					if (ControllerInput.GetButtonDown(ControllerButton.West))
					{
						MapPinController.Place(this.ViewCenter);
					}
					if (ControllerInput.GetButtonDown(ControllerButton.North))
					{
						MapPinController.Clear();
					}
					this.UpdateControllerMapReticle(true);
					return;
				}
				this.UpdateControllerMapReticle(false);
				if (this.HandleSeedCopyPointer(GameplayInput.mousePosition, GameplayInput.GetMouseButtonDown(0), GameplayInput.GetMouseButtonUp(0)))
				{
					this._dragging = false;
					return;
				}
				float y = GameplayInput.mouseScrollDelta.y;
				if (Mathf.Abs(y) > 0.01f)
				{
					this.ZoomAroundCursor(this._expandedZoom * Mathf.Pow(1.18f, y));
				}
				this.HandleDrag();
				if (GameplayInput.GetMouseButtonDown(2))
				{
					this.HandlePinClick();
				}
				return;
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x0019D7A3 File Offset: 0x0019B9A3
		private void HandleDrag()
		{
			this.HandleMapPointerDrag(GameplayInput.mousePosition, GameplayInput.GetMouseButtonDown(0), GameplayInput.GetMouseButton(0), GameplayInput.GetMouseButtonUp(0));
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x0019D7C8 File Offset: 0x0019B9C8
		private void HandleMapPointerDrag(Vector2 screenPosition, bool pressed, bool held, bool released)
		{
			if (!this.CanPanExpandedMap)
			{
				this._dragging = false;
				return;
			}
			Vector2 vector;
			if (pressed && this.TryGetMapLocalPoint(screenPosition, out vector) && vector.sqrMagnitude <= this._mapPanel.sizeDelta.x * this._mapPanel.sizeDelta.x * 0.25f)
			{
				this._dragging = true;
				this._lastDragScreen = screenPosition;
			}
			if (released)
			{
				this._dragging = false;
			}
			if (!this._dragging || !held)
			{
				return;
			}
			Vector2 vector2;
			bool flag = this.TryGetMapLocalDelta(this._lastDragScreen, screenPosition, out vector2);
			this._lastDragScreen = screenPosition;
			if (!flag || vector2.sqrMagnitude < 0.01f)
			{
				return;
			}
			this.SetPan(this._panOffset - this.LocalToWorldDelta(vector2, this._currentPixelsPerMeter));
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x0019D890 File Offset: 0x0019BA90
		private void ZoomAroundCursor(float zoom)
		{
			zoom = Mathf.Clamp(zoom, 1f, 3f);
			if (Mathf.Approximately(zoom, this._expandedZoom))
			{
				return;
			}
			Vector2 vector;
			bool flag = this.TryGetMapLocalPoint(GameplayInput.mousePosition, out vector) && vector.sqrMagnitude <= this._mapPanel.sizeDelta.x * this._mapPanel.sizeDelta.x * 0.25f;
			Vector3 vector2 = (flag ? this.MapToWorld(vector) : Vector3.zero);
			this.SetExpandedZoom(zoom);
			if (Mathf.Approximately(this._expandedZoom, 1f))
			{
				this.SetPan(Vector3.zero);
				return;
			}
			if (flag && this._player != null)
			{
				float num = 2.05f * this._expandedZoom;
				Vector3 vector3 = vector2 - this.LocalToWorldDelta(vector, num);
				this.SetPan(vector3 - this._player.position);
				return;
			}
			this.SetPan(this._panOffset);
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x0019D994 File Offset: 0x0019BB94
		private void SetPan(Vector3 pan)
		{
			pan.y = 0f;
			float maxPanMeters = this.GetMaxPanMeters();
			if (pan.sqrMagnitude > maxPanMeters * maxPanMeters)
			{
				pan = ((maxPanMeters > 0f) ? (pan.normalized * maxPanMeters) : Vector3.zero);
			}
			this._panOffset = pan;
			this._viewDirty = true;
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x0019D9EC File Offset: 0x0019BBEC
		private float GetMaxPanMeters()
		{
			float num = 185.36586f;
			float num2 = 380f / (2.05f * this._expandedZoom);
			return Mathf.Max(0f, num - num2);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x0019DA1F File Offset: 0x0019BC1F
		private bool TryGetMapLocalPoint(Vector2 screenPoint, out Vector2 local)
		{
			return UIScreenCoordinates.TryScreenPointToLocalPoint(this._mapPanel, this._canvas, screenPoint, out local);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x0019DA34 File Offset: 0x0019BC34
		private bool TryGetMapLocalDelta(Vector2 from, Vector2 to, out Vector2 delta)
		{
			delta = Vector2.zero;
			Vector2 vector;
			Vector2 vector2;
			if (!this.TryGetMapLocalPoint(from, out vector) || !this.TryGetMapLocalPoint(to, out vector2))
			{
				return false;
			}
			delta = vector2 - vector;
			return true;
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x0019DA72 File Offset: 0x0019BC72
		private bool CanPanExpandedMap
		{
			get
			{
				return this._expanded && !Mathf.Approximately(this._expandedZoom, 1f);
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x0019DA91 File Offset: 0x0019BC91
		private void UpdatePanControlHints()
		{
			if (this._panControlHint != null)
			{
				this._panControlHint.SetActive(this.CanPanExpandedMap);
			}
			this.UpdateControllerMapHint();
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004510 RID: 17680 RVA: 0x0019DAB8 File Offset: 0x0019BCB8
		private Vector3 ViewCenter
		{
			get
			{
				return ((this._player != null) ? this._player.position : Vector3.zero) + (this._expanded ? this._panOffset : Vector3.zero);
			}
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x0019DAF4 File Offset: 0x0019BCF4
		private Vector3 LocalToWorldDelta(Vector2 local, float pixelsPerMeter)
		{
			float num = local.x / pixelsPerMeter;
			float num2 = local.y / pixelsPerMeter;
			float num3 = this._currentHeading * 0.017453292f;
			float num4 = Mathf.Cos(num3);
			float num5 = Mathf.Sin(num3);
			return new Vector3(num * num4 + num2 * num5, 0f, -num * num5 + num2 * num4);
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0019DB45 File Offset: 0x0019BD45
		public void InterruptForEncounter()
		{
			if (this._expanded)
			{
				this.SetExpandedInternal(false, false);
				return;
			}
			this.ReleaseMapContext();
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x0019DB5E File Offset: 0x0019BD5E
		private void SetExpanded(bool expanded)
		{
			this.SetExpandedInternal(expanded, true);
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x0019DB68 File Offset: 0x0019BD68
		private void SetExpandedInternal(bool expanded, bool playAudio)
		{
			if (expanded == this._expanded)
			{
				return;
			}
			if (expanded)
			{
				if (!ProceduralMiniMapController.CanOpenMap || !this.TryAcquireMapContext())
				{
					return;
				}
				this._expanded = true;
				this.SetGameplayCaptured(true);
				this.SetHudHidden(true);
				if (playAudio)
				{
					MiniMapAudio.Play(MiniMapSound.MapOpen);
				}
			}
			else
			{
				this._expanded = false;
				this.SetSeedCopied(false);
				this.ReleaseMapContext();
				if (playAudio)
				{
					MiniMapAudio.Play(MiniMapSound.MapClose);
				}
			}
			this.SetVisualVisible(this._visualVisible);
			this.ApplyMapInteractionLayer();
			this._panOffset = Vector3.zero;
			this._dragging = false;
			this._expandedZoom = 1f;
			this.UpdatePanControlHints();
			this._viewDirty = true;
			this._lastPreviewCell = new Vector2Int(int.MinValue, int.MinValue);
			this._lastPlayerPosition = new Vector3(float.PositiveInfinity, 0f, 0f);
			this.RebuildChunkVisuals();
			if (this._mapGraphic != null)
			{
				this._mapGraphic.SetExpanded(this._expanded);
			}
			if (this._poiGraphic != null)
			{
				this._poiGraphic.SetExpanded(this._expanded);
			}
			if (this._playerMarker != null)
			{
				this._playerMarker.SetExpanded(this._expanded);
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0019DCA0 File Offset: 0x0019BEA0
		private void SetExpandedZoom(float zoom)
		{
			zoom = Mathf.Clamp(zoom, 1f, 3f);
			if (Mathf.Approximately(zoom, this._expandedZoom))
			{
				return;
			}
			float num = ((this._chunkManager != null) ? Mathf.Max(1f, this._chunkManager.CellSize) : 1f);
			int num2 = this.ComputeExpandedPreviewCells(num);
			this._expandedZoom = zoom;
			this.UpdatePanControlHints();
			this._viewDirty = true;
			if (this.ComputeExpandedPreviewCells(num) != num2)
			{
				this.RebuildChunkVisuals();
			}
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0019DD25 File Offset: 0x0019BF25
		private int ComputeExpandedPreviewCells(float cellSize)
		{
			return Mathf.Clamp(Mathf.CeilToInt(380f / (2.05f * this._expandedZoom) / Mathf.Max(1f, cellSize)) + 1, 3, 8);
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x0019DD54 File Offset: 0x0019BF54
		private bool TryAcquireMapContext()
		{
			if (this._mapLease != null)
			{
				if (this._mapLease.IsActive)
				{
					return true;
				}
				this._mapLease = null;
			}
			if (!ProceduralMiniMapController.CanOpenMap)
			{
				return false;
			}
			vBasicCoverController activeController = WallCoverActivityLock.ActiveController;
			if (activeController != null)
			{
				if (!activeController.CanOpenModalMap)
				{
					return false;
				}
				activeController.PrepareForModalUI();
			}
			return PlayerActivityGate.TryAcquireUtilityModal("ProceduralMiniMap", this, (activeController != null) ? PlayerActivity.Inventory : PlayerActivity.AllPlayerActions, PlayerContextResource.ModalUI, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly | PlayerActivity.ArmsPose, out this._mapLease, "Rank");
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x0019DDD9 File Offset: 0x0019BFD9
		private void ReleaseMapContext()
		{
			this.SetGameplayCaptured(false);
			this.SetHudHidden(false);
			if (this._mapLease == null)
			{
				return;
			}
			this._mapLease.Dispose();
			this._mapLease = null;
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x0019DE04 File Offset: 0x0019C004
		private void ApplyMapInteractionLayer()
		{
			if (this._canvas == null)
			{
				return;
			}
			if (this._mapRaycaster != null)
			{
				this._mapRaycaster.enabled = this._expanded;
			}
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x0019DE34 File Offset: 0x0019C034
		private void SetGameplayCaptured(bool captured)
		{
			if (captured)
			{
				if (this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput == null && this._player != null)
				{
					this._playerInput = this._player.GetComponentInChildren<vThirdPersonInput>(true);
				}
				if (this._playerInput != null)
				{
					this._prevLockCamera = this._playerInput.lockCameraInput;
					this._prevSuppressZoom = this._playerInput.suppressCameraZoom;
					this._capturedCoverInput = WallCoverActivityLock.ActiveController != null;
					if (this._capturedCoverInput)
					{
						this._prevCoverLockInput = this._playerInput.lockInput;
						this._playerInput.SetLockAllInput(true);
						if (this._playerInput.cc != null)
						{
							this._playerInput.cc.input = Vector3.zero;
						}
					}
					this._playerInput.SetLockCameraInput(true);
					this._playerInput.suppressCameraZoom = true;
				}
				this._prevForceCameraLock = vThirdPersonInput.ForceCameraLock;
				vThirdPersonInput.ForceCameraLock = true;
				this._prevCursorLock = Cursor.lockState;
				this._prevCursorVisible = Cursor.visible;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				this._gameplayStateCaptured = true;
				return;
			}
			else
			{
				if (!this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput != null)
				{
					if (this._capturedCoverInput)
					{
						this._playerInput.SetLockAllInput(this._prevCoverLockInput);
					}
					this._playerInput.SetLockCameraInput(this._prevLockCamera);
					this._playerInput.suppressCameraZoom = this._prevSuppressZoom;
				}
				this._capturedCoverInput = false;
				vThirdPersonInput.ForceCameraLock = this._prevForceCameraLock;
				Cursor.lockState = this._prevCursorLock;
				Cursor.visible = this._prevCursorVisible;
				this._gameplayStateCaptured = false;
				return;
			}
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x0019DFE4 File Offset: 0x0019C1E4
		private void SetHudHidden(bool hidden)
		{
			if (hidden)
			{
				if (this._hiddenHud != null)
				{
					return;
				}
				List<GameObject> list = new List<GameObject>(ProceduralMiniMapController.HudContainersToHide.Length + 4);
				Transform[] array = Object.FindObjectsOfType<Transform>(true);
				for (int i = 0; i < ProceduralMiniMapController.HudContainersToHide.Length; i++)
				{
					int j = 0;
					while (j < array.Length)
					{
						if (!(array[j].name != ProceduralMiniMapController.HudContainersToHide[i]))
						{
							if (array[j] == this._hudTop && this._uiRoot != null)
							{
								for (int k = 0; k < array[j].childCount; k++)
								{
									Transform child = array[j].GetChild(k);
									if (child != this._uiRoot)
									{
										list.Add(child.gameObject);
									}
								}
								break;
							}
							list.Add(array[j].gameObject);
							break;
						}
						else
						{
							j++;
						}
					}
				}
				this._hiddenHud = list.ToArray();
				this._hiddenHudPrevActive = new bool[this._hiddenHud.Length];
				for (int l = 0; l < this._hiddenHud.Length; l++)
				{
					this._hiddenHudPrevActive[l] = this._hiddenHud[l].activeSelf;
					this._hiddenHud[l].SetActive(false);
				}
				return;
			}
			else
			{
				if (this._hiddenHud == null)
				{
					return;
				}
				for (int m = 0; m < this._hiddenHud.Length; m++)
				{
					if (this._hiddenHud[m] != null)
					{
						this._hiddenHud[m].SetActive(this._hiddenHudPrevActive[m]);
					}
				}
				this._hiddenHud = null;
				this._hiddenHudPrevActive = null;
				return;
			}
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0019E17A File Offset: 0x0019C37A
		private Vector2 ProjectToMap(Vector3 world)
		{
			return this.ProjectToMap(world, this.ViewCenter);
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x0019E18C File Offset: 0x0019C38C
		private Vector2 ProjectToMap(Vector3 world, Vector3 center)
		{
			float num = world.x - center.x;
			float num2 = world.z - center.z;
			float num3 = this._currentHeading * 0.017453292f;
			float num4 = Mathf.Cos(num3);
			float num5 = Mathf.Sin(num3);
			return new Vector2((num * num4 - num2 * num5) * this._currentPixelsPerMeter, (num * num5 + num2 * num4) * this._currentPixelsPerMeter);
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x0019E1EF File Offset: 0x0019C3EF
		private Vector3 MapToWorld(Vector2 local)
		{
			return this.ViewCenter + this.LocalToWorldDelta(local, this._currentPixelsPerMeter);
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x0019E20C File Offset: 0x0019C40C
		private void UpdatePinMarker()
		{
			if (this._pinMarker == null)
			{
				return;
			}
			bool flag = MapPinController.HasPin && this._canvas.enabled && this._player != null;
			if (this._pinMarker.gameObject.activeSelf != flag)
			{
				this._pinMarker.gameObject.SetActive(flag);
			}
			if (flag)
			{
				this._pinMarker.rectTransform.anchoredPosition = this.ProjectToMap(MapPinController.Position);
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x0019E290 File Offset: 0x0019C490
		private void HandlePinClick()
		{
			Vector2 vector;
			if (!this.TryGetMapLocalPoint(GameplayInput.mousePosition, out vector))
			{
				return;
			}
			float num = this._mapPanel.sizeDelta.x * 0.5f;
			if (vector.sqrMagnitude > num * num)
			{
				return;
			}
			if (MapPinController.HasPin && (vector - this.ProjectToMap(MapPinController.Position)).sqrMagnitude <= 324f)
			{
				MapPinController.Clear();
				return;
			}
			MapPinController.Place(this.MapToWorld(vector));
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x0019E310 File Offset: 0x0019C510
		private void UpdateTransition()
		{
			float num = (this._expanded ? 1f : 0f);
			float transition = this._transition;
			this._transition = Mathf.MoveTowards(this._transition, num, Time.unscaledDeltaTime / 0.24f);
			if (this._transitionVisualsInitialized && Mathf.Approximately(transition, this._transition) && Mathf.Approximately(this._transition, num))
			{
				return;
			}
			this._transitionVisualsInitialized = true;
			this._viewDirty = true;
			float num2 = this._transition * this._transition * (3f - 2f * this._transition);
			Vector2 vector = new Vector2(0f, 1f);
			Vector2 vector2 = new Vector2(0.5f, 0.5f);
			Vector2 vector3 = Vector2.Lerp(vector, vector2, num2);
			this._mapPanel.anchorMin = vector3;
			this._mapPanel.anchorMax = vector3;
			this._mapPanel.anchoredPosition = Vector2.Lerp(new Vector2(108f, -108f), Vector2.zero, num2);
			float num3 = Mathf.Lerp(176f, 760f, num2);
			this._mapPanel.sizeDelta = new Vector2(num3, num3);
			Color color = this._screenDim.color;
			color.a = 0.78f * num2;
			this._screenDim.color = color;
			this._screenDim.raycastTarget = this._expanded;
			Color color2 = this._outerShadow.color;
			color2.a = Mathf.Lerp(0.52f, 0.78f, num2);
			this._outerShadow.color = color2;
			this._outerShadow.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp(192f, 782f, num2);
			Color color3 = this._rim.color;
			color3.a = Mathf.Lerp(0.88f, 0.98f, num2);
			this._rim.color = color3;
			this._rim.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp(182f, 768f, num2);
			this._playerMarkerRect.sizeDelta = Vector2.Lerp(new Vector2(20f, 24f), new Vector2(22f, 26f), num2);
			this._legendGroup.alpha = num2;
			this._legendGroup.interactable = this._expanded && num2 > 0.95f;
			this._legendGroup.blocksRaycasts = this._expanded && num2 > 0.95f && this._hasFreeWorldSeed;
			if (this._wantedGroup != null)
			{
				this._wantedGroup.alpha = 1f - num2;
			}
			this.UpdateFreeSeedPresentation();
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x0019E5CC File Offset: 0x0019C7CC
		private void UpdateMapView()
		{
			using (ProceduralMiniMapController.UpdateMapViewMarker.Auto())
			{
				this.UpdateMapViewCore();
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0019E610 File Offset: 0x0019C810
		private void UpdateMapViewCore()
		{
			bool flag = !Mathf.Approximately(this._transition, this._expanded ? 1f : 0f);
			float unscaledTime = Time.unscaledTime;
			if (!flag && !this._viewDirty && unscaledTime < this._nextMapViewUpdate)
			{
				return;
			}
			this._nextMapViewUpdate = unscaledTime + 0.033333335f;
			Vector3 position = this._player.position;
			float y = this._player.eulerAngles.y;
			this.TryDiscoverNearbyPoi(position);
			if ((position - this._lastPlayerPosition).sqrMagnitude < 0.01f && Mathf.Abs(Mathf.DeltaAngle(y, this._lastPlayerYaw)) < 0.5f && !flag && !this._viewDirty)
			{
				return;
			}
			this._viewDirty = false;
			this._lastPlayerPosition = position;
			this._lastPlayerYaw = y;
			float num = this._transition * this._transition * (3f - 2f * this._transition);
			float num2 = Mathf.LerpAngle(y, 0f, num);
			float num3 = Mathf.Lerp(1.6f, 2.05f * this._expandedZoom, num);
			this._currentPixelsPerMeter = num3;
			this._currentHeading = num2;
			Vector3 vector = position + this._panOffset * num;
			this._mapGraphic.SetView(vector, num2, num3, 1f);
			this._poiGraphic.SetView(vector, num2, num3, 1f);
			this._playerMarker.SetHeading(Mathf.LerpAngle(0f, -y, num));
			Vector2 vector2 = this.ProjectToMap(position, vector);
			float num4 = this._mapPanel.sizeDelta.x * 0.5f - 14f;
			if (vector2.sqrMagnitude > num4 * num4)
			{
				vector2 = vector2.normalized * num4;
			}
			this._playerMarkerRect.anchoredPosition = vector2;
			if (this._chunkManager != null)
			{
				Vector2Int vector2Int = this._chunkManager.WorldToCell(vector);
				if (vector2Int != this._lastPreviewCell)
				{
					this._lastPreviewCell = vector2Int;
					this.RequestChunkVisualRebuild();
				}
			}
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x0019E82C File Offset: 0x0019CA2C
		private bool EnvironmentBindingNeedsRefresh()
		{
			return this._environmentBindingDirty || ChunkManager.Instance != this._chunkManager || (ProceduralMiniMapController.ResolveRegisteredPlayer() != this._player || MatchStatsService.Instance != this._statsService) || ((!(this._canvas != null) || !(this._uiRoot != null) || !(this._hudTop != null) || !(this._uiRoot.parent == this._hudTop)) && Time.unscaledTime >= this._nextMissingBindingRetry);
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x0019E8D4 File Offset: 0x0019CAD4
		private void InvalidateEnvironmentBinding()
		{
			this._environmentBindingDirty = true;
			this._legacyCompassCacheDirty = true;
			this._nextMissingBindingRetry = 0f;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x0019E8F0 File Offset: 0x0019CAF0
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			this._startHomePositions.Clear();
			if (this._hasHomePoiPosition)
			{
				this._homePoiPosition += delta;
			}
			this._lastPlayerPosition += delta;
			this._snapshotSortPlayerPosition += delta;
			this.InvalidateEnvironmentBinding();
			this.RequestChunkVisualRebuild();
			this._nextMapViewUpdate = 0f;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x0019E95D File Offset: 0x0019CB5D
		private void HandleSceneLoaded(Scene ignoredScene, LoadSceneMode ignoredMode)
		{
			this.InvalidateEnvironmentBinding();
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0019E95D File Offset: 0x0019CB5D
		private void HandleSceneUnloaded(Scene ignoredScene)
		{
			this.InvalidateEnvironmentBinding();
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0019E965 File Offset: 0x0019CB65
		private void HandleActiveSceneChanged(Scene ignoredPrevious, Scene next)
		{
			this.TryUnlockInteriorScene(next);
			this.InvalidateEnvironmentBinding();
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x0019E974 File Offset: 0x0019CB74
		private void TryUnlockInteriorScene(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return;
			}
			MiniMapPoiKind miniMapPoiKind;
			if (MiniMapPoiDiscoveryPolicy.TryResolveInteriorSceneKind(scene.name, out miniMapPoiKind))
			{
				this.UnlockPoiKind(miniMapPoiKind);
			}
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x0019E95D File Offset: 0x0019CB5D
		private void HandleGameplayReady()
		{
			this.InvalidateEnvironmentBinding();
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x0019E9AB File Offset: 0x0019CBAB
		private static Transform ResolveRegisteredPlayer()
		{
			if (PlayerRegistry.Player != null)
			{
				return PlayerRegistry.Player;
			}
			if (!(GameSystemsBootstrapper.Instance != null))
			{
				return null;
			}
			return GameSystemsBootstrapper.Instance.Player;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x0019E9DC File Offset: 0x0019CBDC
		private void RefreshEnvironmentBinding()
		{
			using (ProceduralMiniMapController.RefreshEnvironmentBindingMarker.Auto())
			{
				this.RefreshEnvironmentBindingCore();
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x0019EA20 File Offset: 0x0019CC20
		private void RefreshEnvironmentBindingCore()
		{
			this._environmentBindingDirty = false;
			if (!this.EnsureHudBinding())
			{
				this._nextMissingBindingRetry = Time.unscaledTime + 0.5f;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (instance != this._chunkManager)
			{
				this.UnbindChunkManager();
				this.BindChunkManager(instance);
			}
			this._player = ProceduralMiniMapController.ResolveRegisteredPlayer();
			this.BindPoiContext((this._player != null) ? this._player.GetComponentInChildren<PlayerPoiEmoteContext>(true) : null);
			this.BindStatsService(MatchStatsService.Instance);
			this.QueueNearestChunkSnapshots();
			if (this._chunkManager != null && this._chunkManager.isActiveAndEnabled)
			{
				this.DisableLegacyRenderedMiniMap();
			}
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x0019EAD0 File Offset: 0x0019CCD0
		private void BindChunkManager(ChunkManager manager)
		{
			this._chunkManager = manager;
			if (this._chunkManager == null)
			{
				return;
			}
			this._chunkManager.OnChunkSpawned += this.HandleChunkChanged;
			this._chunkManager.OnChunkRemoved += this.HandleChunkChanged;
			this._chunkManager.OnChunkZonesReady += this.HandleChunkChanged;
			this._chunkManager.OnChunkActivated += this.HandleChunkChanged;
			this._chunkManager.OnChunkDeactivated += this.HandleChunkChanged;
			this._chunkManager.OnInitialGenerationComplete += this.RequestChunkVisualRebuild;
			this.RebuildChunkVisuals();
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x0019EB84 File Offset: 0x0019CD84
		private void UnbindChunkManager()
		{
			this.ClearPoiReadinessSubscriptions();
			if (this._chunkManager != null)
			{
				this._chunkManager.OnChunkSpawned -= this.HandleChunkChanged;
				this._chunkManager.OnChunkRemoved -= this.HandleChunkChanged;
				this._chunkManager.OnChunkZonesReady -= this.HandleChunkChanged;
				this._chunkManager.OnChunkActivated -= this.HandleChunkChanged;
				this._chunkManager.OnChunkDeactivated -= this.HandleChunkChanged;
				this._chunkManager.OnInitialGenerationComplete -= this.RequestChunkVisualRebuild;
			}
			this._chunkManager = null;
			this.ClearMiniMapPreviewCache();
			this.RecycleChunkVisuals();
			this._pois.Clear();
			this._preSpawnedGridSizes.Clear();
			this._preSpawnedSemanticNames.Clear();
			this._startHomePositions.Clear();
			this._hasHomePoiPosition = false;
			this._homePoiPosition = default(Vector3);
			this._homePoiExplicitMarker = false;
			this._snapshotCandidatesScratch.Clear();
			this._lastPreviewCell = new Vector2Int(int.MinValue, int.MinValue);
			this._snapshotQueue.Clear();
			this._queuedSnapshotIds.Clear();
			this._runtimeChunkTiles.Clear();
			if (this._mapGraphic != null)
			{
				this._mapGraphic.SetStyleAtlas(this._styleAtlas);
				this._mapGraphic.SetChunks(this._chunks);
				this._poiGraphic.SetPois(this._pois);
			}
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0019ED0D File Offset: 0x0019CF0D
		private void HandleChunkChanged(ChunkInstance ignored)
		{
			this.RequestChunkVisualRebuild();
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x0019ED15 File Offset: 0x0019CF15
		private void RequestChunkVisualRebuild()
		{
			this._chunkVisualsDirty = true;
			this._nextChunkVisualRebuild = Time.unscaledTime + 0.15f;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x0019ED0D File Offset: 0x0019CF0D
		private void HandleMiniMapPoiMarkersChanged()
		{
			this.RequestChunkVisualRebuild();
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x0019ED30 File Offset: 0x0019CF30
		private void BindPoiContext(PlayerPoiEmoteContext context)
		{
			if (this._poiContext == context)
			{
				return;
			}
			if (this._poiContext != null)
			{
				this._poiContext.OnZoneEntered -= this.HandlePoiZoneEntered;
			}
			this._poiContext = context;
			if (this._poiContext != null)
			{
				this._poiContext.OnZoneEntered += this.HandlePoiZoneEntered;
				if (this._poiContext.CurrentProfile != null)
				{
					this.MarkPoiVisited(this._poiContext.CurrentProfile);
				}
			}
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x0019EDC1 File Offset: 0x0019CFC1
		private void HandlePoiZoneEntered(PoiEmoteProfileSO profile)
		{
			this.MarkPoiVisited(profile);
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x0019EDCC File Offset: 0x0019CFCC
		private void BindStatsService(MatchStatsService service)
		{
			if (this._statsService == service)
			{
				return;
			}
			if (this._statsService != null)
			{
				this._statsService.RunReset -= this.HandleRunReset;
			}
			this._statsService = service;
			if (this._statsService != null)
			{
				this._statsService.RunReset += this.HandleRunReset;
			}
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x0019EE3C File Offset: 0x0019D03C
		private void HandleRunReset()
		{
			this.ClearMiniMapPreviewCache();
			this._startHomePositions.Clear();
			this._hasHomePoiPosition = false;
			this._homePoiPosition = default(Vector3);
			this._homePoiExplicitMarker = false;
			this._visitedPoiKinds.Clear();
			this._visitedPoiKinds.Add(MiniMapPoiKind.Home);
			this.UpdateLegendDiscovery();
			this.RebuildChunkVisuals();
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x0019EE98 File Offset: 0x0019D098
		private void MarkPoiVisited(PoiEmoteProfileSO profile)
		{
			if (profile == null)
			{
				return;
			}
			MiniMapPoiKind miniMapPoiKind = ProceduralMiniMapController.ResolvePoiKind(string.Concat(new string[] { profile.PoiId, " ", profile.DisplayName, " ", profile.name }));
			if (miniMapPoiKind == MiniMapPoiKind.None)
			{
				return;
			}
			if (MiniMapPoiDiscoveryPolicy.RequiresInteriorScene(miniMapPoiKind))
			{
				MiniMapPoiKind miniMapPoiKind2;
				if (!MiniMapPoiDiscoveryPolicy.TryResolveInteriorSceneKind(SceneManager.GetActiveScene().name, out miniMapPoiKind2) || miniMapPoiKind2 != miniMapPoiKind)
				{
					return;
				}
				this.UnlockPoiKind(miniMapPoiKind);
				return;
			}
			else
			{
				if (!MiniMapPoiDiscoveryPolicy.CanDiscoverInWorld(miniMapPoiKind) || this._chunkManager == null || this._player == null || !this.IsInsideActualPoi(miniMapPoiKind, this._player.position))
				{
					return;
				}
				this.UnlockPoiKind(miniMapPoiKind);
				return;
			}
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x0019EF5C File Offset: 0x0019D15C
		private void TryDiscoverNearbyPoi(Vector3 playerPosition)
		{
			for (int i = 0; i < this._pois.Count; i++)
			{
				MiniMapPoiVisual miniMapPoiVisual = this._pois[i];
				if (!miniMapPoiVisual.previewOnly && miniMapPoiVisual.kind != MiniMapPoiKind.None && !this._visitedPoiKinds.Contains(miniMapPoiVisual.kind) && MiniMapPoiDiscoveryPolicy.CanDiscoverInWorld(miniMapPoiVisual.kind) && MiniMapPoiDiscoveryPolicy.IsInsideWorldVisitArea(miniMapPoiVisual, playerPosition))
				{
					this.UnlockPoiKind(miniMapPoiVisual.kind);
					return;
				}
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x0019EFD4 File Offset: 0x0019D1D4
		private bool IsInsideActualPoi(MiniMapPoiKind kind, Vector3 playerPosition)
		{
			if (kind == MiniMapPoiKind.None)
			{
				return false;
			}
			for (int i = 0; i < this._pois.Count; i++)
			{
				MiniMapPoiVisual miniMapPoiVisual = this._pois[i];
				if (!miniMapPoiVisual.previewOnly && miniMapPoiVisual.kind == kind && MiniMapPoiDiscoveryPolicy.IsInsideWorldVisitArea(miniMapPoiVisual, playerPosition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x0019F026 File Offset: 0x0019D226
		private void UnlockPoiKind(MiniMapPoiKind kind)
		{
			if (kind == MiniMapPoiKind.None || !this._visitedPoiKinds.Add(kind))
			{
				return;
			}
			AchievementTrackingService.RecordMapPoiVisit(kind);
			this.UpdateLegendDiscovery();
			this.RebuildChunkVisuals();
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x0019F04C File Offset: 0x0019D24C
		private void RecycleChunkVisuals()
		{
			for (int i = 0; i < this._chunks.Count; i++)
			{
				MiniMapChunkVisual miniMapChunkVisual = this._chunks[i];
				if (miniMapChunkVisual != null)
				{
					miniMapChunkVisual.zones.Clear();
					this._chunkVisualPool.Push(miniMapChunkVisual);
				}
			}
			this._chunks.Clear();
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x0019F0A4 File Offset: 0x0019D2A4
		private MiniMapChunkVisual RentChunkVisual()
		{
			MiniMapChunkVisual miniMapChunkVisual = ((this._chunkVisualPool.Count > 0) ? this._chunkVisualPool.Pop() : new MiniMapChunkVisual());
			miniMapChunkVisual.center = default(Vector3);
			miniMapChunkVisual.yaw = 0f;
			miniMapChunkVisual.size = default(Vector2);
			miniMapChunkVisual.shape = ChunkShape.XShape;
			miniMapChunkVisual.stableHash = 0;
			miniMapChunkVisual.previewOnly = false;
			miniMapChunkVisual.tile = default(MiniMapTileReference);
			miniMapChunkVisual.zones.Clear();
			return miniMapChunkVisual;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x0019F120 File Offset: 0x0019D320
		private int ComparePreviewCells(Vector2Int a, Vector2Int b)
		{
			int num = a.x - this._previewSortCenter.x;
			int num2 = a.y - this._previewSortCenter.y;
			int num3 = b.x - this._previewSortCenter.x;
			int num4 = b.y - this._previewSortCenter.y;
			int num5 = (num * num + num2 * num2).CompareTo(num3 * num3 + num4 * num4);
			if (num5 != 0)
			{
				return num5;
			}
			int num6 = a.x.CompareTo(b.x);
			if (num6 == 0)
			{
				return a.y.CompareTo(b.y);
			}
			return num6;
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0019F1D0 File Offset: 0x0019D3D0
		private int CompareSnapshotCandidates(ChunkInstance a, ChunkInstance b)
		{
			bool flag = a != null && a.spawnedZones != null && a.spawnedZones.Count > 0;
			bool flag2 = b != null && b.spawnedZones != null && b.spawnedZones.Count > 0;
			if (flag != flag2)
			{
				if (!flag)
				{
					return 1;
				}
				return -1;
			}
			else if (a == null)
			{
				if (b != null)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (b == null)
				{
					return -1;
				}
				float sqrMagnitude = (a.worldPosition - this._snapshotSortPlayerPosition).sqrMagnitude;
				float sqrMagnitude2 = (b.worldPosition - this._snapshotSortPlayerPosition).sqrMagnitude;
				return sqrMagnitude.CompareTo(sqrMagnitude2);
			}
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0019F270 File Offset: 0x0019D470
		private void RebuildChunkVisuals()
		{
			using (ProceduralMiniMapController.RebuildChunkVisualsMarker.Auto())
			{
				this.RebuildChunkVisualsCore();
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x0019F2B4 File Offset: 0x0019D4B4
		private void RebuildChunkVisualsCore()
		{
			this.ClearPoiReadinessSubscriptions();
			this.RecycleChunkVisuals();
			this._pois.Clear();
			if (this._chunkManager == null)
			{
				if (this._mapGraphic != null)
				{
					this._mapGraphic.SetChunks(this._chunks);
				}
				if (this._poiGraphic != null)
				{
					this._poiGraphic.SetPois(this._pois);
				}
				return;
			}
			float num = Mathf.Max(1f, this._chunkManager.CellSize);
			foreach (MiniMapPoiMarker miniMapPoiMarker in MiniMapPoiMarker.Active)
			{
				if (!(miniMapPoiMarker == null) && miniMapPoiMarker.isActiveAndEnabled && miniMapPoiMarker.Kind != MiniMapPoiKind.None && miniMapPoiMarker.IsOperationallyReady)
				{
					this.AddPoi(miniMapPoiMarker.transform.position, miniMapPoiMarker.Kind, false, true, null, default(Vector2), 0f);
				}
			}
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (value != null && !(value.chunkRoot == null))
				{
					if (value.chunkData == null && ProceduralMiniMapController.IsStartChunk(this.GetPreSpawnedSemanticName(value.chunkRoot.transform)))
					{
						this.AddPoi(this.ResolveStartChunkHomePosition(value.chunkRoot.transform), MiniMapPoiKind.Home, false, false, null, default(Vector2), 0f);
					}
					if (this._chunkManager.IsChunkPresentationVisible(value))
					{
						this._chunkManager.MarkMiniMapObserved(keyValuePair.Key);
						ChunkData chunkData = value.chunkData;
						ChunkTemplate chunkTemplate = ((chunkData != null) ? chunkData.template : null);
						Vector2Int vector2Int = ((chunkData != null) ? chunkData.gridSize : this.ResolvePreSpawnedGridSize(value.chunkRoot.transform, num));
						ChunkSpawnResolver.SpawnEntry spawnEntry = default(ChunkSpawnResolver.SpawnEntry);
						Vector2Int vector2Int2;
						bool flag = chunkData != null && this._chunkManager.TryGetCommittedMiniMapEntry(keyValuePair.Key, out vector2Int2, out spawnEntry) && spawnEntry.wasPresented;
						MiniMapChunkVisual miniMapChunkVisual = (flag ? this.BuildSeededPreviewChunk(keyValuePair.Key, spawnEntry, num) : this.RentChunkVisual());
						miniMapChunkVisual.center = value.worldPosition;
						miniMapChunkVisual.yaw = value.rotation.eulerAngles.y;
						miniMapChunkVisual.size = new Vector2((float)vector2Int.x * num, (float)vector2Int.y * num);
						miniMapChunkVisual.shape = ((chunkTemplate != null) ? chunkTemplate.shape : ProceduralMiniMapController.ResolveChunkShape((chunkData != null) ? chunkData.name : this.GetPreSpawnedSemanticName(value.chunkRoot.transform)));
						MiniMapChunkVisual miniMapChunkVisual2 = miniMapChunkVisual;
						string text = ((chunkData != null) ? chunkData.name : value.chunkRoot.name);
						vector2Int2 = keyValuePair.Key;
						int x = vector2Int2.x;
						vector2Int2 = keyValuePair.Key;
						miniMapChunkVisual2.stableHash = ProceduralMiniMapController.StableHash(text, x, vector2Int2.y);
						MiniMapTileReference miniMapTileReference;
						if (this._runtimeChunkTiles.TryGetValue(value, out miniMapTileReference))
						{
							miniMapChunkVisual.tile = miniMapTileReference;
						}
						if (value.spawnedZones != null)
						{
							for (int i = 0; i < value.spawnedZones.Count; i++)
							{
								GameObject gameObject = value.spawnedZones[i];
								if (!(gameObject == null) && gameObject.activeInHierarchy)
								{
									Vector2 vector = ProceduralMiniMapController.ResolveZoneSize((chunkTemplate != null && chunkTemplate.zoneSlots != null && i < chunkTemplate.zoneSlots.Count) ? chunkTemplate.zoneSlots[i] : null, num, miniMapChunkVisual.size);
									string name = gameObject.name;
									MiniMapTileReference miniMapTileReference2 = default(MiniMapTileReference);
									ProceduralMiniMapStyleAtlas.Entry entry;
									if (this._snapshotStyleAtlas == null && this._styleAtlas != null && this._styleAtlas.TryGet(null, name, out entry))
									{
										miniMapTileReference2 = MiniMapTileReference.From(entry);
										vector = entry.worldSize;
									}
									if (!flag)
									{
										List<MiniMapZoneVisual> zones = miniMapChunkVisual.zones;
										MiniMapZoneVisual miniMapZoneVisual = default(MiniMapZoneVisual);
										miniMapZoneVisual.center = gameObject.transform.position;
										miniMapZoneVisual.yaw = gameObject.transform.eulerAngles.y;
										miniMapZoneVisual.size = vector;
										miniMapZoneVisual.kind = ProceduralMiniMapController.ResolveZoneKind(name);
										string text2 = name;
										vector2Int2 = keyValuePair.Key;
										int num2 = vector2Int2.x * 17 + i;
										vector2Int2 = keyValuePair.Key;
										miniMapZoneVisual.stableHash = ProceduralMiniMapController.StableHash(text2, num2, vector2Int2.y * 31 - i);
										miniMapZoneVisual.tile = miniMapTileReference2;
										zones.Add(miniMapZoneVisual);
									}
									this.AddPoiIfRecognized(gameObject.transform.position, name, false, gameObject, vector, gameObject.transform.eulerAngles.y);
								}
							}
						}
						if (miniMapChunkVisual.zones.Count == 0 && chunkData == null)
						{
							string preSpawnedSemanticName = this.GetPreSpawnedSemanticName(value.chunkRoot.transform);
							MiniMapTileReference miniMapTileReference3 = default(MiniMapTileReference);
							Vector2 vector2 = miniMapChunkVisual.size * 0.76f;
							ProceduralMiniMapStyleAtlas.Entry entry2;
							if (this._snapshotStyleAtlas == null && this._styleAtlas != null && this._styleAtlas.TryGet(null, preSpawnedSemanticName, out entry2))
							{
								miniMapTileReference3 = MiniMapTileReference.From(entry2);
								vector2 = entry2.worldSize;
							}
							List<MiniMapZoneVisual> zones2 = miniMapChunkVisual.zones;
							MiniMapZoneVisual miniMapZoneVisual = default(MiniMapZoneVisual);
							miniMapZoneVisual.center = value.worldPosition;
							miniMapZoneVisual.yaw = value.rotation.eulerAngles.y;
							miniMapZoneVisual.size = vector2;
							miniMapZoneVisual.kind = ProceduralMiniMapController.ResolveZoneKind(preSpawnedSemanticName);
							string text3 = preSpawnedSemanticName;
							vector2Int2 = keyValuePair.Key;
							int x2 = vector2Int2.x;
							vector2Int2 = keyValuePair.Key;
							miniMapZoneVisual.stableHash = ProceduralMiniMapController.StableHash(text3, x2, vector2Int2.y);
							miniMapZoneVisual.tile = miniMapTileReference3;
							zones2.Add(miniMapZoneVisual);
							if (!ProceduralMiniMapController.IsStartChunk(preSpawnedSemanticName))
							{
								this.AddPoiIfRecognized(value.worldPosition, preSpawnedSemanticName, false, value.chunkRoot, vector2, value.rotation.eulerAngles.y);
							}
						}
						this._chunks.Add(miniMapChunkVisual);
					}
				}
			}
			if (this._player != null)
			{
				this.AppendSeededExpandedPreview(num, this._expanded ? this.ComputeExpandedPreviewCells(num) : 2);
			}
			this.RetainHomePoi();
			if (this._mapGraphic != null)
			{
				this._mapGraphic.SetChunks(this._chunks);
			}
			if (this._poiGraphic != null)
			{
				this._poiGraphic.SetPois(this._pois);
			}
			this.QueueNearestChunkSnapshots();
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x0019F9C4 File Offset: 0x0019DBC4
		private void RetainHomePoi()
		{
			for (int i = 0; i < this._pois.Count; i++)
			{
				MiniMapPoiVisual miniMapPoiVisual = this._pois[i];
				if (miniMapPoiVisual.kind == MiniMapPoiKind.Home && !miniMapPoiVisual.previewOnly)
				{
					this._homePoiPosition = miniMapPoiVisual.center;
					this._homePoiExplicitMarker = miniMapPoiVisual.explicitMarker;
					this._hasHomePoiPosition = true;
					return;
				}
			}
			if (this._hasHomePoiPosition)
			{
				this.AddPoi(this._homePoiPosition, MiniMapPoiKind.Home, false, this._homePoiExplicitMarker, null, default(Vector2), 0f);
			}
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x0019FA50 File Offset: 0x0019DC50
		private void AppendSeededExpandedPreview(float cellSize, int previewRadius)
		{
			this.AppendMiniMapPreview(cellSize, previewRadius);
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x0019FA5C File Offset: 0x0019DC5C
		private MiniMapChunkVisual BuildSeededMetadataCell(Vector2Int cell, ChunkSpawnResolver.SpawnEntry entry, float cellSize)
		{
			ChunkData chunkData = entry.chunkData;
			if (chunkData == null)
			{
				return null;
			}
			MiniMapChunkVisual miniMapChunkVisual = this.RentChunkVisual();
			miniMapChunkVisual.center = this._chunkManager.CellToWorld(cell, Vector2Int.one);
			miniMapChunkVisual.yaw = (float)entry.rotation * 90f;
			miniMapChunkVisual.size = new Vector2(cellSize, cellSize);
			miniMapChunkVisual.shape = ((chunkData.template != null) ? chunkData.template.shape : ProceduralMiniMapController.ResolveChunkShape(chunkData.name));
			miniMapChunkVisual.stableHash = ProceduralMiniMapController.StableHash(chunkData.name, cell.x, cell.y);
			miniMapChunkVisual.previewOnly = true;
			return miniMapChunkVisual;
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x0019FB0C File Offset: 0x0019DD0C
		private bool TryResolveRenderablePreviewEntry(Vector2Int origin, HashSet<Vector2Int> occupied, bool specialReserved, out ChunkSpawnResolver.SpawnEntry entry, out bool reservesSpecial)
		{
			reservesSpecial = false;
			if ((!this._chunkManager.TryResolveMiniMapPreview(origin, out entry) || !entry.IsValid) && (!this._chunkManager.TryResolveMiniMapFallback(origin, null, specialReserved, out entry) || !entry.IsValid))
			{
				return false;
			}
			ChunkData chunkData = entry.chunkData;
			if (!ProceduralMiniMapController.PreviewCellsAreFree(occupied, origin, chunkData.gridSize) && (!this._chunkManager.TryResolveMiniMapFallback(origin, chunkData, specialReserved, out entry) || !entry.IsValid))
			{
				return false;
			}
			if (ProceduralMiniMapController.PreviewSelectionContainsSpecial(entry) && specialReserved)
			{
				if (ProceduralZonePolicy.IsAlwaysSpecialChunk(entry.chunkData))
				{
					if (!this._chunkManager.TryResolveMiniMapFallback(origin, entry.chunkData, true, out entry) || !entry.IsValid)
					{
						return false;
					}
				}
				else if (!this._chunkManager.TryResolveMiniMapWithoutSpecialZones(origin, entry.chunkData, out entry) || !entry.IsValid)
				{
					return false;
				}
			}
			if (!ProceduralMiniMapController.PreviewCellsAreFree(occupied, origin, entry.chunkData.gridSize))
			{
				return false;
			}
			reservesSpecial = !specialReserved && ProceduralMiniMapController.PreviewSelectionContainsSpecial(entry);
			return true;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x0019FC18 File Offset: 0x0019DE18
		private MiniMapChunkVisual BuildSeededPreviewChunk(Vector2Int origin, ChunkSpawnResolver.SpawnEntry entry, float cellSize)
		{
			ChunkData chunkData = entry.chunkData;
			if (chunkData == null)
			{
				return null;
			}
			ChunkTemplate template = chunkData.template;
			float num = (float)entry.rotation * 90f;
			Vector3 vector = this._chunkManager.CellToWorld(origin, chunkData.gridSize);
			Vector2 worldSize = chunkData.GetWorldSize(cellSize);
			MiniMapChunkVisual miniMapChunkVisual = this.RentChunkVisual();
			miniMapChunkVisual.center = vector;
			miniMapChunkVisual.yaw = num;
			miniMapChunkVisual.size = worldSize;
			miniMapChunkVisual.shape = ((template != null) ? template.shape : ProceduralMiniMapController.ResolveChunkShape(chunkData.name));
			miniMapChunkVisual.stableHash = ProceduralMiniMapController.StableHash(chunkData.name, origin.x, origin.y);
			miniMapChunkVisual.previewOnly = false;
			if (template == null || template.zoneSlots == null)
			{
				return miniMapChunkVisual;
			}
			Quaternion quaternion = Quaternion.Euler(0f, num, 0f);
			for (int i = 0; i < template.zoneSlots.Count; i++)
			{
				ZoneSlot zoneSlot = template.zoneSlots[i];
				ZoneData zoneData = ProceduralMiniMapController.ResolvePreviewZone(entry, i);
				if (!(zoneData == null))
				{
					ProceduralMiniMapController.MiniMapSlotDescriptor miniMapSlotDescriptor = this.GetMiniMapSlotDescriptor(template, i);
					ProceduralMiniMapController.MiniMapZoneDescriptor miniMapZoneDescriptor = this.GetMiniMapZoneDescriptor(zoneData);
					string name = miniMapZoneDescriptor.name;
					Vector3 vector2 = vector + quaternion * miniMapSlotDescriptor.localPosition;
					float num2 = num + miniMapSlotDescriptor.localYaw;
					MiniMapTileReference tile = miniMapZoneDescriptor.tile;
					Vector2 vector3 = (tile.valid ? tile.worldSize : ProceduralMiniMapController.ResolveZoneSize(zoneSlot, cellSize, worldSize));
					miniMapChunkVisual.zones.Add(new MiniMapZoneVisual
					{
						center = vector2,
						yaw = num2,
						size = vector3,
						kind = miniMapZoneDescriptor.kind,
						stableHash = ProceduralMiniMapController.StableHash(name, origin.x * 17 + i, origin.y * 31 - i),
						tile = tile
					});
					this.AddMiniMapPreviewPoi(vector2, miniMapZoneDescriptor.poiKind, vector3, num2);
				}
			}
			return miniMapChunkVisual;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0019FE2C File Offset: 0x0019E02C
		private static bool PreviewSelectionContainsSpecial(ChunkSpawnResolver.SpawnEntry entry)
		{
			if (!entry.IsValid || entry.chunkData == null)
			{
				return false;
			}
			if (ProceduralZonePolicy.IsAlwaysSpecialChunk(entry.chunkData))
			{
				return true;
			}
			if (entry.zoneIndices == null || entry.chunkData.availableZones == null)
			{
				return false;
			}
			for (int i = 0; i < entry.zoneIndices.Length; i++)
			{
				int num = entry.zoneIndices[i];
				if (num >= 0 && num < entry.chunkData.availableZones.Count && ProceduralZonePolicy.IsSpecialZone(entry.chunkData.availableZones[num]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x0019FEC8 File Offset: 0x0019E0C8
		private static ZoneData ResolvePreviewZone(ChunkSpawnResolver.SpawnEntry entry, int slotIndex)
		{
			if (!entry.IsValid || entry.zoneIndices == null || slotIndex < 0 || slotIndex >= entry.zoneIndices.Length || entry.chunkData.availableZones == null)
			{
				return null;
			}
			int num = entry.zoneIndices[slotIndex];
			if (num < 0 || num >= entry.chunkData.availableZones.Count)
			{
				return null;
			}
			return entry.chunkData.availableZones[num];
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0019FF38 File Offset: 0x0019E138
		private static bool PreviewCellsAreFree(HashSet<Vector2Int> occupied, Vector2Int origin, Vector2Int size)
		{
			for (int i = 0; i < Mathf.Max(1, size.x); i++)
			{
				for (int j = 0; j < Mathf.Max(1, size.y); j++)
				{
					if (occupied.Contains(origin + new Vector2Int(i, j)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x0019FF90 File Offset: 0x0019E190
		private static void MarkPreviewCells(HashSet<Vector2Int> occupied, Vector2Int origin, Vector2Int size)
		{
			for (int i = 0; i < Mathf.Max(1, size.x); i++)
			{
				for (int j = 0; j < Mathf.Max(1, size.y); j++)
				{
					occupied.Add(origin + new Vector2Int(i, j));
				}
			}
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x0019FFE4 File Offset: 0x0019E1E4
		private void QueueNearestChunkSnapshots()
		{
			if (this._styleAtlas != null && this._styleAtlas.HasEntries)
			{
				return;
			}
			if (this._chunkManager == null || this._player == null)
			{
				return;
			}
			int num = this._runtimeChunkTiles.Count + this._snapshotQueue.Count;
			if (num >= 2)
			{
				return;
			}
			List<ChunkInstance> snapshotCandidatesScratch = this._snapshotCandidatesScratch;
			snapshotCandidatesScratch.Clear();
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (value != null && !(value.chunkRoot == null) && value.chunkRoot.activeInHierarchy)
				{
					int instanceID = value.chunkRoot.GetInstanceID();
					if (!this._runtimeChunkTiles.ContainsKey(value) && !this._queuedSnapshotIds.Contains(instanceID))
					{
						snapshotCandidatesScratch.Add(value);
					}
				}
			}
			this._snapshotSortPlayerPosition = this._player.position;
			snapshotCandidatesScratch.Sort(this._snapshotCandidateComparison);
			int num2 = 2 - num;
			int num3 = 0;
			while (num3 < snapshotCandidatesScratch.Count && num3 < num2)
			{
				ChunkInstance chunkInstance = snapshotCandidatesScratch[num3];
				this._snapshotQueue.Enqueue(chunkInstance);
				this._queuedSnapshotIds.Add(chunkInstance.chunkRoot.GetInstanceID());
				num3++;
			}
			snapshotCandidatesScratch.Clear();
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x001A0164 File Offset: 0x0019E364
		private void CaptureNextChunkSnapshot()
		{
			if (this._snapshotQueue.Count == 0 || this._chunkManager == null)
			{
				return;
			}
			ChunkInstance chunkInstance = this._snapshotQueue.Dequeue();
			if (chunkInstance == null || chunkInstance.chunkRoot == null || !chunkInstance.chunkRoot.activeInHierarchy)
			{
				return;
			}
			this.EnsureSnapshotResources();
			if (this._snapshotCamera == null || this._snapshotTarget == null || this._snapshotAtlas == null)
			{
				return;
			}
			float num = Mathf.Max(1f, this._chunkManager.CellSize);
			Vector2Int vector2Int = ((chunkInstance.chunkData != null) ? chunkInstance.chunkData.gridSize : this.ResolvePreSpawnedGridSize(chunkInstance.chunkRoot.transform, num));
			Vector2 vector = new Vector2((float)Mathf.Max(1, vector2Int.x) * num, (float)Mathf.Max(1, vector2Int.y) * num);
			float y = chunkInstance.rotation.eulerAngles.y;
			this._snapshotCamera.transform.SetPositionAndRotation(chunkInstance.worldPosition + Vector3.up * 250f, Quaternion.Euler(90f, y, 0f));
			this._snapshotCamera.aspect = vector.x / vector.y;
			this._snapshotCamera.orthographicSize = vector.y * 0.515f;
			this._snapshotCamera.targetTexture = this._snapshotTarget;
			RenderTexture active = RenderTexture.active;
			Texture2D texture2D = null;
			LODGroup[] componentsInChildren = chunkInstance.chunkRoot.GetComponentsInChildren<LODGroup>(true);
			List<Renderer> list = ProceduralMiniMapController.DisableCaptureArtifacts(chunkInstance.chunkRoot);
			try
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != null)
					{
						componentsInChildren[i].ForceLOD(0);
					}
				}
				this._snapshotCamera.Render();
				RenderTexture.active = this._snapshotTarget;
				texture2D = new Texture2D(256, 256, TextureFormat.RGBA32, false, false)
				{
					name = "MiniMap Chunk Snapshot",
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					hideFlags = HideFlags.HideAndDontSave
				};
				texture2D.ReadPixels(new Rect(0f, 0f, 256f, 256f), 0, 0, false);
				texture2D.Apply(false, false);
			}
			finally
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j] != null)
					{
						componentsInChildren[j].ForceLOD(-1);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] != null)
					{
						list[k].enabled = true;
					}
				}
				RenderTexture.active = active;
				this._snapshotCamera.targetTexture = null;
			}
			if (texture2D == null)
			{
				return;
			}
			int count = this._runtimeChunkTiles.Count;
			this._snapshotAtlas.SetPixels32(count * 256, 0, 256, 256, texture2D.GetPixels32());
			this._snapshotAtlas.Apply(false, false);
			Object.Destroy(texture2D);
			float num2 = 768f;
			float num3 = 0.5f / num2;
			float num4 = 0.001953125f;
			this._runtimeChunkTiles[chunkInstance] = new MiniMapTileReference
			{
				valid = true,
				uv = new Rect((float)(count * 256) / num2 + num3, num4, 256f / num2 - num3 * 2f, 1f - num4 * 2f),
				worldSize = vector
			};
			if (this._mapGraphic != null)
			{
				this._mapGraphic.SetStyleAtlas(this._snapshotStyleAtlas);
			}
			this.RebuildChunkVisuals();
			if (this._runtimeChunkTiles.Count >= 2 && this._snapshotQueue.Count == 0)
			{
				this.ReleaseSnapshotCaptureObjects();
			}
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x001A0540 File Offset: 0x0019E740
		private static List<Renderer> DisableCaptureArtifacts(GameObject chunkRoot)
		{
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in chunkRoot.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && renderer.enabled)
				{
					string text = renderer.gameObject.name.ToLowerInvariant();
					bool flag = text.Contains("lod1") || text.Contains("lod2") || text.Contains("lod3");
					bool flag2 = text.Contains("impostor") || text.Contains("mirage") || text.Contains("streaming proxy") || text.EndsWith("_proxy");
					if (flag || flag2)
					{
						renderer.enabled = false;
						list.Add(renderer);
					}
				}
			}
			return list;
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x001A0618 File Offset: 0x0019E818
		private void EnsureSnapshotResources()
		{
			if (this._snapshotAtlas == null)
			{
				this._snapshotAtlas = new Texture2D(768, 256, TextureFormat.RGBA32, false, false)
				{
					name = "Runtime MiniMap Snapshot Atlas",
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					hideFlags = HideFlags.HideAndDontSave
				};
				int num = 768;
				Color32[] array = new Color32[num * 256];
				Color32 color = new Color32(18, 25, 27, byte.MaxValue);
				Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				for (int i = 0; i < 256; i++)
				{
					for (int j = 0; j < num; j++)
					{
						array[i * num + j] = ((j >= 512) ? color2 : color);
					}
				}
				this._snapshotAtlas.SetPixels32(array);
				this._snapshotAtlas.Apply(false, false);
				this._snapshotStyleAtlas = ScriptableObject.CreateInstance<ProceduralMiniMapStyleAtlas>();
				this._snapshotStyleAtlas.name = "Runtime MiniMap Snapshot Style";
				this._snapshotStyleAtlas.hideFlags = HideFlags.HideAndDontSave;
				this._snapshotStyleAtlas.Configure(this._snapshotAtlas, new List<ProceduralMiniMapStyleAtlas.Entry>());
			}
			if (this._snapshotTarget == null)
			{
				this._snapshotTarget = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32)
				{
					name = "MiniMap One-Shot Capture",
					antiAliasing = 2,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					hideFlags = HideFlags.HideAndDontSave
				};
				this._snapshotTarget.Create();
			}
			if (this._snapshotCamera == null)
			{
				GameObject gameObject = new GameObject("MiniMap One-Shot Camera", new Type[] { typeof(Camera) });
				gameObject.transform.SetParent(base.transform, false);
				gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
				this._snapshotCamera = gameObject.GetComponent<Camera>();
				this._snapshotCamera.enabled = false;
				this._snapshotCamera.orthographic = true;
				this._snapshotCamera.aspect = 1f;
				this._snapshotCamera.nearClipPlane = 0.1f;
				this._snapshotCamera.farClipPlane = 500f;
				this._snapshotCamera.clearFlags = CameraClearFlags.Color;
				this._snapshotCamera.backgroundColor = new Color(0.07f, 0.095f, 0.1f, 1f);
				this._snapshotCamera.allowHDR = false;
				this._snapshotCamera.allowMSAA = true;
				this._snapshotCamera.useOcclusionCulling = false;
				this._snapshotCamera.cullingMask = ProceduralMiniMapController.BuildSnapshotCullingMask();
			}
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x001A08A4 File Offset: 0x0019EAA4
		private static int BuildSnapshotCullingMask()
		{
			int num = -1;
			string[] array = new string[]
			{
				"UI", "Player", "Enemy", "CompanionAI", "Triggers", "PlayerHead", "PlayerClothing", "NPCDetectionHidden", "TVCameraOnly", "StadiumScreenOnly",
				"StadiumMonitorVCam", "CharacterPreview", "PostProcess"
			};
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = LayerMask.NameToLayer(array[i]);
				if (num2 >= 0)
				{
					num &= ~(1 << num2);
				}
			}
			return num;
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x001A0950 File Offset: 0x0019EB50
		private void DisposeSnapshotResources()
		{
			this._snapshotQueue.Clear();
			this._queuedSnapshotIds.Clear();
			this._runtimeChunkTiles.Clear();
			this.ReleaseSnapshotCaptureObjects();
			if (this._snapshotStyleAtlas != null)
			{
				Object.Destroy(this._snapshotStyleAtlas);
			}
			this._snapshotStyleAtlas = null;
			if (this._snapshotAtlas != null)
			{
				Object.Destroy(this._snapshotAtlas);
			}
			this._snapshotAtlas = null;
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x001A09C4 File Offset: 0x0019EBC4
		private void ReleaseSnapshotCaptureObjects()
		{
			if (this._snapshotCamera != null)
			{
				Object.Destroy(this._snapshotCamera.gameObject);
			}
			this._snapshotCamera = null;
			if (this._snapshotTarget != null)
			{
				this._snapshotTarget.Release();
				Object.Destroy(this._snapshotTarget);
			}
			this._snapshotTarget = null;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x001A0A24 File Offset: 0x0019EC24
		private Vector2Int ResolvePreSpawnedGridSize(Transform root, float cellSize)
		{
			int instanceID = root.GetInstanceID();
			Vector2Int vector2Int;
			if (this._preSpawnedGridSizes.TryGetValue(instanceID, out vector2Int))
			{
				return vector2Int;
			}
			PreSpawnedChunk component = root.GetComponent<PreSpawnedChunk>();
			if (component != null)
			{
				vector2Int = new Vector2Int(Mathf.Max(1, component.gridSize.x), Mathf.Max(1, component.gridSize.y));
				this._preSpawnedGridSizes[instanceID] = vector2Int;
				return vector2Int;
			}
			SnapPoint[] componentsInChildren = root.GetComponentsInChildren<SnapPoint>(true);
			if (componentsInChildren == null || componentsInChildren.Length < 2)
			{
				this._preSpawnedGridSizes[instanceID] = Vector2Int.one;
				return Vector2Int.one;
			}
			Vector3 vector = Vector3.positiveInfinity;
			Vector3 vector2 = Vector3.negativeInfinity;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == null))
				{
					Vector3 position = componentsInChildren[i].transform.position;
					vector = Vector3.Min(vector, position);
					vector2 = Vector3.Max(vector2, position);
				}
			}
			vector2Int = new Vector2Int(Mathf.Max(1, Mathf.RoundToInt((vector2.x - vector.x) / cellSize)), Mathf.Max(1, Mathf.RoundToInt((vector2.z - vector.z) / cellSize)));
			this._preSpawnedGridSizes[instanceID] = vector2Int;
			return vector2Int;
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x001A0B5C File Offset: 0x0019ED5C
		private static ChunkShape ResolveChunkShape(string value)
		{
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "startchunk") || ProceduralMiniMapController.ContainsIgnoreCase(value, "start chunk"))
			{
				return ChunkShape.XShape;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "xchunk") || ProceduralMiniMapController.ContainsIgnoreCase(value, "x chunk"))
			{
				return ChunkShape.XShape;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "tchunk") || ProceduralMiniMapController.ContainsIgnoreCase(value, "t chunk"))
			{
				return ChunkShape.TShape;
			}
			return ChunkShape.MonoShape;
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x001A0BBE File Offset: 0x0019EDBE
		private static bool IsStartChunk(string value)
		{
			return ProceduralMiniMapController.ContainsIgnoreCase(value, "startchunk") || ProceduralMiniMapController.ContainsIgnoreCase(value, "start chunk");
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x001A0BDC File Offset: 0x0019EDDC
		private Vector3 ResolveStartChunkHomePosition(Transform root)
		{
			Vector3 vector;
			using (ProceduralMiniMapController.ResolveStartHomeMarker.Auto())
			{
				vector = this.ResolveStartChunkHomePositionCore(root);
			}
			return vector;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x001A0C20 File Offset: 0x0019EE20
		private Vector3 ResolveStartChunkHomePositionCore(Transform root)
		{
			if (root == null)
			{
				return Vector3.zero;
			}
			int instanceID = root.GetInstanceID();
			Vector3 vector;
			if (this._startHomePositions.TryGetValue(instanceID, out vector))
			{
				return vector;
			}
			this._transformSearch.Clear();
			this._transformSearch.Push(root);
			while (this._transformSearch.Count > 0)
			{
				Transform transform = this._transformSearch.Pop();
				if (!(transform == null))
				{
					string name = transform.name;
					if (name.IndexOf("cielhouse", StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("cielhome", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						vector = transform.position;
						this._startHomePositions[instanceID] = vector;
						this._transformSearch.Clear();
						return vector;
					}
					for (int i = 0; i < transform.childCount; i++)
					{
						this._transformSearch.Push(transform.GetChild(i));
					}
				}
			}
			vector = root.position;
			this._startHomePositions[instanceID] = vector;
			return vector;
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x001A0D1C File Offset: 0x0019EF1C
		private string GetPreSpawnedSemanticName(Transform root)
		{
			int instanceID = root.GetInstanceID();
			string text;
			if (this._preSpawnedSemanticNames.TryGetValue(instanceID, out text))
			{
				return text;
			}
			string text2 = root.name;
			int num = 0;
			foreach (object obj in root)
			{
				Transform transform = (Transform)obj;
				text2 = text2 + " " + transform.name;
				num++;
				if (num >= 8)
				{
					break;
				}
			}
			this._preSpawnedSemanticNames[instanceID] = text2;
			return text2;
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x001A0DC0 File Offset: 0x0019EFC0
		private static Vector2 ResolveZoneSize(ZoneSlot slot, float cellSize, Vector2 fallback)
		{
			if (slot == null)
			{
				return fallback * 0.78f;
			}
			Vector2 physicalSize = slot.parentChunkSize.GetPhysicalSize(cellSize);
			Vector2 vector = new Vector2(Mathf.Max(1f, physicalSize.x - 9f), Mathf.Max(1f, physicalSize.y - 9f));
			ZoneOccupancy occupancy = slot.occupancy;
			if (occupancy == ZoneOccupancy.Quarter)
			{
				return vector * 0.5f;
			}
			if (occupancy != ZoneOccupancy.Half)
			{
				return vector;
			}
			return new Vector2(vector.x * 0.5f, vector.y);
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x001A0E54 File Offset: 0x0019F054
		private static MiniMapZoneKind ResolveZoneKind(string value)
		{
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "park") || ProceduralMiniMapController.ContainsIgnoreCase(value, "garden"))
			{
				return MiniMapZoneKind.Park;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "football") || ProceduralMiniMapController.ContainsIgnoreCase(value, "stadium") || ProceduralMiniMapController.ContainsIgnoreCase(value, "sport"))
			{
				return MiniMapZoneKind.Sports;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "subway") || ProceduralMiniMapController.ContainsIgnoreCase(value, "metro"))
			{
				return MiniMapZoneKind.Subway;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "kombini") || ProceduralMiniMapController.ContainsIgnoreCase(value, "shop") || ProceduralMiniMapController.ContainsIgnoreCase(value, "store") || ProceduralMiniMapController.ContainsIgnoreCase(value, "sushi"))
			{
				return MiniMapZoneKind.Commercial;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "suburban"))
			{
				return MiniMapZoneKind.Suburban;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "residential"))
			{
				return MiniMapZoneKind.Residential;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "city"))
			{
				return MiniMapZoneKind.City;
			}
			return MiniMapZoneKind.Generic;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x001A0F28 File Offset: 0x0019F128
		private void AddPoiIfRecognized(Vector3 center, string value, bool previewOnly, GameObject source = null, Vector2 footprintSize = default(Vector2), float footprintYaw = 0f)
		{
			MiniMapPoiKind miniMapPoiKind = ProceduralMiniMapController.ResolvePoiKind(value);
			if (miniMapPoiKind == MiniMapPoiKind.None || ((miniMapPoiKind == MiniMapPoiKind.Metro || miniMapPoiKind == MiniMapPoiKind.Kombini) && previewOnly && source == null) || !this.CanPublishPoi(source, miniMapPoiKind))
			{
				return;
			}
			this.AddPoi(center, miniMapPoiKind, previewOnly, false, source, footprintSize, footprintYaw);
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x001A0F78 File Offset: 0x0019F178
		private bool CanPublishPoi(GameObject source, MiniMapPoiKind kind)
		{
			IncrementalPooledZoneActivation incrementalPooledZoneActivation;
			if (MiniMapPoiMarker.IsSourceOperational((source != null) ? source.transform : null, kind, out incrementalPooledZoneActivation))
			{
				return true;
			}
			if (incrementalPooledZoneActivation != null && this._pendingPoiReadinessGates.Add(incrementalPooledZoneActivation))
			{
				incrementalPooledZoneActivation.OperationalReady += this.HandlePoiReadinessGateReady;
			}
			return false;
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x001A0FCD File Offset: 0x0019F1CD
		private void HandlePoiReadinessGateReady(IncrementalPooledZoneActivation source)
		{
			if (source != null)
			{
				source.OperationalReady -= this.HandlePoiReadinessGateReady;
				this._pendingPoiReadinessGates.Remove(source);
			}
			this.RequestChunkVisualRebuild();
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x001A1000 File Offset: 0x0019F200
		private void ClearPoiReadinessSubscriptions()
		{
			foreach (IncrementalPooledZoneActivation incrementalPooledZoneActivation in this._pendingPoiReadinessGates)
			{
				if (incrementalPooledZoneActivation != null)
				{
					incrementalPooledZoneActivation.OperationalReady -= this.HandlePoiReadinessGateReady;
				}
			}
			this._pendingPoiReadinessGates.Clear();
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x001A1074 File Offset: 0x0019F274
		private void AddPoi(Vector3 center, MiniMapPoiKind kind, bool previewOnly, bool explicitMarker = false, GameObject source = null, Vector2 footprintSize = default(Vector2), float footprintYaw = 0f)
		{
			BoxCollider[] array = MiniMapPoiDiscoveryPolicy.CaptureInteriorVolumes(source, kind);
			MiniMapPoiVisual miniMapPoiVisual = new MiniMapPoiVisual
			{
				center = center,
				visitCenter = center,
				kind = kind,
				previewOnly = previewOnly,
				explicitMarker = explicitMarker,
				discovered = (kind == MiniMapPoiKind.Home || this._visitedPoiKinds.Contains(kind)),
				footprintSize = footprintSize,
				footprintYaw = footprintYaw,
				interiorVolumes = array,
				discoveryMode = MiniMapPoiDiscoveryPolicy.ResolveMode(kind, array)
			};
			for (int i = 0; i < this._pois.Count; i++)
			{
				if (this._pois[i].kind == kind)
				{
					MiniMapPoiVisual miniMapPoiVisual2 = this._pois[i];
					if (ProceduralMiniMapController.ArePoiPositionsDuplicate(miniMapPoiVisual2.center, center, kind))
					{
						if (ProceduralMiniMapController.ShouldPreferPoi(miniMapPoiVisual2.explicitMarker, miniMapPoiVisual2.previewOnly, explicitMarker, previewOnly))
						{
							miniMapPoiVisual2.center = center;
							miniMapPoiVisual2.previewOnly = previewOnly;
							miniMapPoiVisual2.explicitMarker = explicitMarker;
						}
						if (array != null && array.Length != 0)
						{
							miniMapPoiVisual2.interiorVolumes = array;
						}
						if (footprintSize.x > 0f && footprintSize.y > 0f && (!previewOnly || miniMapPoiVisual2.footprintSize.x <= 0f))
						{
							miniMapPoiVisual2.visitCenter = center;
							miniMapPoiVisual2.footprintSize = footprintSize;
							miniMapPoiVisual2.footprintYaw = footprintYaw;
						}
						miniMapPoiVisual2.discoveryMode = MiniMapPoiDiscoveryPolicy.ResolveMode(kind, miniMapPoiVisual2.interiorVolumes);
						miniMapPoiVisual2.discovered = kind == MiniMapPoiKind.Home || this._visitedPoiKinds.Contains(kind);
						this._pois[i] = miniMapPoiVisual2;
						return;
					}
				}
			}
			this._pois.Add(miniMapPoiVisual);
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x001A1228 File Offset: 0x0019F428
		private static bool ArePoiPositionsDuplicate(Vector3 a, Vector3 b, MiniMapPoiKind kind)
		{
			float num = ((kind == MiniMapPoiKind.Metro || kind == MiniMapPoiKind.Kombini) ? 48f : 30f);
			float num2 = a.x - b.x;
			float num3 = a.z - b.z;
			return num2 * num2 + num3 * num3 <= num * num;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x001A1274 File Offset: 0x0019F474
		private static bool ShouldPreferPoi(bool existingExplicitMarker, bool existingPreviewOnly, bool candidateExplicitMarker, bool candidatePreviewOnly)
		{
			if (existingExplicitMarker != candidateExplicitMarker)
			{
				return candidateExplicitMarker;
			}
			return existingPreviewOnly && !candidatePreviewOnly;
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x001A1288 File Offset: 0x0019F488
		private static MiniMapPoiKind ResolvePoiKind(string value)
		{
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "university") || ProceduralMiniMapController.ContainsIgnoreCase(value, "universit") || ProceduralMiniMapController.ContainsIgnoreCase(value, "campus"))
			{
				return MiniMapPoiKind.University;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "subway") || ProceduralMiniMapController.ContainsIgnoreCase(value, "metro"))
			{
				return MiniMapPoiKind.Metro;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "kombini"))
			{
				return MiniMapPoiKind.Kombini;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "stadium") || ProceduralMiniMapController.ContainsIgnoreCase(value, "football") || ProceduralMiniMapController.ContainsIgnoreCase(value, "gymnasium"))
			{
				return MiniMapPoiKind.Stadium;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "park") || ProceduralMiniMapController.ContainsIgnoreCase(value, "garden"))
			{
				return MiniMapPoiKind.Park;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "mall"))
			{
				return MiniMapPoiKind.Mall;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "gallery") || ProceduralMiniMapController.ContainsIgnoreCase(value, "museum"))
			{
				return MiniMapPoiKind.Gallery;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "pool") || ProceduralMiniMapController.ContainsIgnoreCase(value, "swimming"))
			{
				return MiniMapPoiKind.Pool;
			}
			if (ProceduralMiniMapController.ContainsIgnoreCase(value, "home") || ProceduralMiniMapController.ContainsIgnoreCase(value, "safehouse"))
			{
				return MiniMapPoiKind.Home;
			}
			return MiniMapPoiKind.None;
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool ContainsIgnoreCase(string value, string token)
		{
			return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x001A1394 File Offset: 0x0019F594
		private static int StableHash(string value, int x, int y)
		{
			uint num = 2166136261U;
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					num ^= (uint)value[i];
					num *= 16777619U;
				}
			}
			num = (num ^ (uint)x) * 16777619U;
			return (int)((num ^ (uint)y) * 16777619U);
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x001A13E4 File Offset: 0x0019F5E4
		private void DisableLegacyRenderedMiniMap()
		{
			using (ProceduralMiniMapController.DisableLegacyMarker.Auto())
			{
				this.DisableLegacyRenderedMiniMapCore();
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x001A1428 File Offset: 0x0019F628
		private void DisableLegacyRenderedMiniMapCore()
		{
			this.RefreshLegacyCompassCache();
			bool flag = false;
			for (int i = 0; i < this._legacyCompasses.Count; i++)
			{
				CompassPro compassPro = this._legacyCompasses[i];
				if (compassPro == null)
				{
					flag = true;
				}
				else if (compassPro.showMiniMap)
				{
					compassPro.showMiniMap = false;
				}
			}
			if (flag)
			{
				this._legacyCompassCacheDirty = true;
			}
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x001A1488 File Offset: 0x0019F688
		private void RefreshLegacyCompassCache()
		{
			if (!this._legacyCompassCacheDirty)
			{
				return;
			}
			this._legacyCompassCacheDirty = false;
			this._legacyCompasses.Clear();
			if (this._hudTop == null)
			{
				return;
			}
			Canvas componentInParent = this._hudTop.GetComponentInParent<Canvas>(true);
			if (componentInParent != null && componentInParent.gameObject.name == "HUDCanvas")
			{
				componentInParent.GetComponentsInChildren<CompassPro>(true, this._legacyCompasses);
			}
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x001A14FC File Offset: 0x0019F6FC
		private bool EnsureHudBinding()
		{
			bool flag;
			using (ProceduralMiniMapController.EnsureHudBindingMarker.Auto())
			{
				flag = this.EnsureHudBindingCore();
			}
			return flag;
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x001A1540 File Offset: 0x0019F740
		private bool EnsureHudBindingCore()
		{
			if (this._canvas != null && this._uiRoot != null && this._hudTop != null && this._uiRoot.parent == this._hudTop)
			{
				this.FitUiRootToHudCanvas();
				return true;
			}
			RectTransform rectTransform = (ProceduralMiniMapController.IsHudTop(this._hudTop) ? this._hudTop : this.FindHudTop());
			if (rectTransform == null)
			{
				this.ClearUiReferences(true);
				this._hudTop = null;
				this._legacyCompasses.Clear();
				this._legacyCompassCacheDirty = true;
				return false;
			}
			if (this._expanded)
			{
				this._expanded = false;
				this._transition = 0f;
				this.ReleaseMapContext();
			}
			this.ClearUiReferences(true);
			if (this._hudTop != rectTransform)
			{
				this._legacyCompasses.Clear();
			}
			this._hudTop = rectTransform;
			this._legacyCompassCacheDirty = true;
			this.BuildUi(rectTransform);
			this.FitUiRootToHudCanvas();
			return this._canvas != null;
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x001A1648 File Offset: 0x0019F848
		private static bool IsHudTop(RectTransform target)
		{
			if (target == null || target.name != "UITop")
			{
				return false;
			}
			Canvas componentInParent = target.GetComponentInParent<Canvas>(true);
			return componentInParent != null && componentInParent.gameObject.name == "HUDCanvas";
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x001A169C File Offset: 0x0019F89C
		private RectTransform FindHudTop()
		{
			Scene scene = ((GameSystemsBootstrapper.Instance != null) ? GameSystemsBootstrapper.Instance.gameObject.scene : default(Scene));
			RectTransform rectTransform;
			if (this.TryFindHudTop(scene, out rectTransform))
			{
				return rectTransform;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			if (activeScene.handle != scene.handle && this.TryFindHudTop(activeScene, out rectTransform))
			{
				return rectTransform;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.handle != scene.handle && sceneAt.handle != activeScene.handle && this.TryFindHudTop(sceneAt, out rectTransform))
				{
					return rectTransform;
				}
			}
			return null;
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x001A1750 File Offset: 0x0019F950
		private bool TryFindHudTop(Scene scene, out RectTransform target)
		{
			target = null;
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return false;
			}
			this._sceneRoots.Clear();
			scene.GetRootGameObjects(this._sceneRoots);
			for (int i = 0; i < this._sceneRoots.Count; i++)
			{
				GameObject gameObject = this._sceneRoots[i];
				if (!(gameObject == null))
				{
					Transform transform = ((gameObject.name == "HUDCanvas") ? gameObject.transform : gameObject.transform.Find("HUDCanvas"));
					if (!(transform == null))
					{
						target = this.FindNamedRectTransform(transform, "UITop");
						if (target != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x001A1808 File Offset: 0x0019FA08
		private RectTransform FindNamedRectTransform(Transform root, string objectName)
		{
			this._transformSearch.Clear();
			this._transformSearch.Push(root);
			while (this._transformSearch.Count > 0)
			{
				Transform transform = this._transformSearch.Pop();
				if (transform.name == objectName)
				{
					RectTransform rectTransform = transform as RectTransform;
					if (rectTransform != null)
					{
						return rectTransform;
					}
				}
				for (int i = transform.childCount - 1; i >= 0; i--)
				{
					this._transformSearch.Push(transform.GetChild(i));
				}
			}
			return null;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x001A1888 File Offset: 0x0019FA88
		private void FitUiRootToHudCanvas()
		{
			if (this._uiRoot == null || this._hudTop == null)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime < this._nextUiFitTime)
			{
				return;
			}
			this._nextUiFitTime = unscaledTime + 0.5f;
			Canvas componentInParent = this._hudTop.GetComponentInParent<Canvas>();
			RectTransform rectTransform = ((componentInParent != null && componentInParent.rootCanvas != null) ? (componentInParent.rootCanvas.transform as RectTransform) : null);
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.GetWorldCorners(this._hudCanvasWorldCorners);
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			for (int i = 0; i < this._hudCanvasWorldCorners.Length; i++)
			{
				Vector3 vector3 = this._hudTop.InverseTransformPoint(this._hudCanvasWorldCorners[i]);
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			Vector2 vector4 = (vector + vector2) * 0.5f;
			Vector2 vector5 = vector2 - vector;
			Vector2 vector6 = new Vector2(0.5f, 0.5f);
			Rect rect = this._hudTop.rect;
			Vector2 vector7 = new Vector2(Mathf.Lerp(rect.xMin, rect.xMax, vector6.x), Mathf.Lerp(rect.yMin, rect.yMax, vector6.y));
			this._uiRoot.anchorMin = (this._uiRoot.anchorMax = vector6);
			this._uiRoot.pivot = vector6;
			this._uiRoot.anchoredPosition = vector4 - vector7;
			this._uiRoot.sizeDelta = vector5;
			this._uiRoot.localScale = Vector3.one;
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x001A1A60 File Offset: 0x0019FC60
		private void ClearUiReferences(bool destroyRoot)
		{
			if (destroyRoot && this._uiRoot != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._uiRoot.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._uiRoot.gameObject);
				}
			}
			this._canvas = null;
			this._mapRaycaster = null;
			this._uiRoot = null;
			this._mapPanel = null;
			this._visualGroup = null;
			this._backdropVisualGroup = null;
			this._screenDim = null;
			this._outerShadow = null;
			this._rim = null;
			this._mapGraphic = null;
			this._poiGraphic = null;
			this._pinMarker = null;
			this._playerMarker = null;
			this._playerMarkerRect = null;
			this._legendGroup = null;
			this._panControlHint = null;
			this._controllerReticle = null;
			this._controllerMapHint = null;
			this._controllerHintAllowsPan = null;
			this.ClearFreeSeedUiReferences();
			this._wantedGroup = null;
			this._faceCamContainer = null;
			this._distanceHudRect = null;
			this._scoreHudRect = null;
			this._wantedInPoi = false;
			this._nextFaceCamBindingRetry = 0f;
			Array.Clear(this._wantedIcons, 0, this._wantedIcons.Length);
			this._wantedIconSprite = null;
			this._lastWantedLevel = -1;
			this._legendRows.Clear();
			this._transitionVisualsInitialized = false;
			this._nextUiFitTime = 0f;
			this._nextMapViewUpdate = 0f;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x001A1BB0 File Offset: 0x0019FDB0
		private static void BeginStyleAtlasLoad()
		{
			if (ProceduralMiniMapController._preloadedStyleAtlas != null || ProceduralMiniMapController._styleAtlasRequest != null)
			{
				return;
			}
			using (ProceduralMiniMapController.LoadStyleAtlasMarker.Auto())
			{
				ProceduralMiniMapController._styleAtlasRequest = Resources.LoadAsync<ProceduralMiniMapStyleAtlas>("ProceduralMiniMapStyleAtlas");
			}
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x001A1C10 File Offset: 0x0019FE10
		private bool TryCompleteStyleAtlasLoad()
		{
			if (this._styleAtlas != null)
			{
				return false;
			}
			if (ProceduralMiniMapController._preloadedStyleAtlas == null)
			{
				ProceduralMiniMapController.BeginStyleAtlasLoad();
				if (ProceduralMiniMapController._styleAtlasRequest == null || !ProceduralMiniMapController._styleAtlasRequest.isDone)
				{
					return false;
				}
				ProceduralMiniMapController._preloadedStyleAtlas = ProceduralMiniMapController._styleAtlasRequest.asset as ProceduralMiniMapStyleAtlas;
			}
			if (ProceduralMiniMapController._preloadedStyleAtlas == null)
			{
				return false;
			}
			this._styleAtlas = ProceduralMiniMapController._preloadedStyleAtlas;
			return true;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x001A1C84 File Offset: 0x0019FE84
		private void BuildUi(RectTransform hudTop)
		{
			using (ProceduralMiniMapController.BuildUiMarker.Auto())
			{
				this.BuildUiCore(hudTop);
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001A1CC8 File Offset: 0x0019FEC8
		private void BuildUiCore(RectTransform hudTop)
		{
			int num = LayerMask.NameToLayer("UI");
			if (num < 0)
			{
				num = 5;
			}
			GameObject gameObject = new GameObject("ProceduralMiniMapCanvas", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(GraphicRaycaster)
			});
			gameObject.layer = num;
			gameObject.transform.SetParent(hudTop, false);
			this._uiRoot = gameObject.GetComponent<RectTransform>();
			this._canvas = gameObject.GetComponent<Canvas>();
			this._mapRaycaster = gameObject.GetComponent<GraphicRaycaster>();
			this.ApplyMapInteractionLayer();
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			this._screenDim = ProceduralMiniMapController.CreateImage(gameObject.transform, "Expanded Backdrop", new Color(0.025f, 0.035f, 0.052f, 0f));
			ProceduralMiniMapController.Stretch(this._screenDim.rectTransform);
			this._screenDim.raycastTarget = false;
			GameObject gameObject2 = new GameObject("Map Panel", new Type[] { typeof(RectTransform) });
			gameObject2.layer = num;
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._mapPanel = gameObject2.GetComponent<RectTransform>();
			RectTransform mapPanel = this._mapPanel;
			RectTransform mapPanel2 = this._mapPanel;
			Vector2 vector = new Vector2(0f, 1f);
			mapPanel2.anchorMax = vector;
			mapPanel.anchorMin = vector;
			this._mapPanel.pivot = new Vector2(0.5f, 0.5f);
			this._mapPanel.anchoredPosition = new Vector2(108f, -108f);
			this._mapPanel.sizeDelta = Vector2.one * 176f;
			this._outerShadow = ProceduralMiniMapController.CreateImage(this._mapPanel, "Shadow", new Color(0.01f, 0.015f, 0.025f, 0.52f));
			ProceduralMiniMapController.Center(this._outerShadow.rectTransform, Vector2.one * 192f);
			this._outerShadow.sprite = ProceduralMiniMapController.GetCircleSprite();
			Image image = ProceduralMiniMapController.CreateImage(this._mapPanel, "Circular Clip", Color.white);
			ProceduralMiniMapController.Stretch(image.rectTransform);
			image.sprite = ProceduralMiniMapController.GetCircleSprite();
			image.raycastTarget = false;
			image.gameObject.AddComponent<Mask>().showMaskGraphic = false;
			ProceduralMiniMapController.Stretch(ProceduralMiniMapController.CreateImage(image.transform, "Map Background", new Color(0.135f, 0.155f, 0.175f, 1f)).rectTransform);
			GameObject gameObject3 = new GameObject("2.5D Map Geometry", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapGraphic)
			});
			gameObject3.layer = num;
			gameObject3.transform.SetParent(image.transform, false);
			ProceduralMiniMapController.Stretch(gameObject3.GetComponent<RectTransform>());
			this._mapGraphic = gameObject3.GetComponent<ProceduralMiniMapGraphic>();
			this._mapGraphic.raycastTarget = false;
			this.TryCompleteStyleAtlasLoad();
			this._mapGraphic.SetStyleAtlas(this._styleAtlas);
			this._mapGraphic.SetChunks(this._chunks);
			GameObject gameObject4 = new GameObject("POI Overlay", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPoiGraphic)
			});
			gameObject4.layer = num;
			gameObject4.transform.SetParent(image.transform, false);
			ProceduralMiniMapController.Stretch(gameObject4.GetComponent<RectTransform>());
			this._poiGraphic = gameObject4.GetComponent<ProceduralMiniMapPoiGraphic>();
			this._poiGraphic.raycastTarget = false;
			this._poiGraphic.SetPois(this._pois);
			GameObject gameObject5 = new GameObject("Pin Marker", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPinGraphic)
			});
			gameObject5.layer = num;
			gameObject5.transform.SetParent(image.transform, false);
			RectTransform component = gameObject5.GetComponent<RectTransform>();
			vector = new Vector2(0.5f, 0.5f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.pivot = new Vector2(0.5f, 0f);
			component.sizeDelta = new Vector2(28f, 34f);
			this._pinMarker = gameObject5.GetComponent<ProceduralMiniMapPinGraphic>();
			this._pinMarker.raycastTarget = false;
			gameObject5.SetActive(false);
			this._rim = ProceduralMiniMapController.CreateImage(this._mapPanel, "Map Rim", new Color(0.88f, 0.92f, 0.94f, 0.88f));
			ProceduralMiniMapController.Center(this._rim.rectTransform, Vector2.one * 182f);
			this._rim.sprite = ProceduralMiniMapController.GetRingSprite();
			this._rim.raycastTarget = false;
			GameObject gameObject6 = new GameObject("Player Marker", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapMarkerGraphic)
			});
			gameObject6.layer = num;
			gameObject6.transform.SetParent(this._mapPanel, false);
			RectTransform component2 = gameObject6.GetComponent<RectTransform>();
			ProceduralMiniMapController.Center(component2, new Vector2(20f, 24f));
			this._playerMarkerRect = component2;
			this._playerMarker = gameObject6.GetComponent<ProceduralMiniMapMarkerGraphic>();
			this._playerMarker.raycastTarget = false;
			this.BuildLegend(this._mapPanel, num);
			this.UpdateLegendDiscovery();
			this.BuildWantedIndicator(this._mapPanel, num);
			this.RefreshFreeWorldSeed();
			this.SetVisualVisible(this._visualVisible);
			this._canvas.enabled = false;
			this._transitionVisualsInitialized = false;
			this._nextUiFitTime = 0f;
			this._nextMapViewUpdate = 0f;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x001A228F File Offset: 0x001A048F
		private static void BeginHudStyleLoad()
		{
			if (ProceduralMiniMapController._preloadedHudStyle == null && ProceduralMiniMapController._hudStyleRequest == null)
			{
				ProceduralMiniMapController._hudStyleRequest = Resources.LoadAsync<ProceduralMiniMapHudStyle>("ProceduralMiniMapHudStyle");
			}
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x001A22B4 File Offset: 0x001A04B4
		private void BuildWantedIndicator(RectTransform parent, int uiLayer)
		{
			GameObject gameObject = new GameObject("Wanted Level", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			gameObject.layer = uiLayer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			RectTransform rectTransform2 = component;
			Vector2 vector = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			component.pivot = new Vector2(0.5f, 1f);
			component.anchoredPosition = new Vector2(0f, -7f);
			component.sizeDelta = new Vector2(160f, 28f);
			this._wantedGroup = gameObject.GetComponent<CanvasGroup>();
			this._wantedGroup.interactable = false;
			this._wantedGroup.blocksRaycasts = false;
			this._wantedGroup.alpha = 1f - this._transition;
			for (int i = 0; i < 5; i++)
			{
				Image image = ProceduralMiniMapController.CreateImage(component, string.Format("Wanted Panties {0}", i + 1), Color.white);
				ProceduralMiniMapController.Center(image.rectTransform, new Vector2(32f, 28f));
				image.rectTransform.anchoredPosition = new Vector2((float)(i - 2) * 32f, 0f);
				image.preserveAspect = true;
				image.enabled = false;
				this._wantedIcons[i] = image;
			}
			ProceduralMiniMapController.BeginHudStyleLoad();
			this.UpdateWantedIndicator();
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x001A242C File Offset: 0x001A062C
		private void UpdateWantedIndicator()
		{
			if (this._wantedGroup == null)
			{
				return;
			}
			if (ProceduralMiniMapController._preloadedHudStyle == null && ProceduralMiniMapController._hudStyleRequest != null && ProceduralMiniMapController._hudStyleRequest.isDone)
			{
				ProceduralMiniMapController._preloadedHudStyle = ProceduralMiniMapController._hudStyleRequest.asset as ProceduralMiniMapHudStyle;
			}
			Sprite sprite = ((ProceduralMiniMapController._preloadedHudStyle != null) ? ProceduralMiniMapController._preloadedHudStyle.WantedIcon : null);
			int num = ((SuspicionManager.Instance != null) ? SuspicionManager.Instance.CurrentLevel : 0);
			if (num == this._lastWantedLevel && sprite == this._wantedIconSprite)
			{
				return;
			}
			this._lastWantedLevel = num;
			this._wantedIconSprite = sprite;
			int num2 = ((num <= 0) ? 0 : ((num == 1) ? 1 : ((num == 2) ? 3 : 5)));
			Color color = new Color(0.5f, 0.54f, 0.6f, 0.38f);
			for (int i = 0; i < this._wantedIcons.Length; i++)
			{
				Image image = this._wantedIcons[i];
				if (!(image == null))
				{
					image.sprite = sprite;
					image.enabled = sprite != null;
					image.color = ((i < num2) ? Color.white : color);
				}
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x001A2560 File Offset: 0x001A0760
		public static bool ShouldShowPoiWanted(SceneProfileSO profile)
		{
			return profile != null && profile.participatesInRun && !profile.proceduralMap && !profile.minimap;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x001A2586 File Offset: 0x001A0786
		public static bool ShouldShowFacecamWanted(SceneProfileSO profile, bool isLongWayHome)
		{
			return ProceduralMiniMapController.ShouldShowPoiWanted(profile) || (isLongWayHome && profile != null && profile.participatesInRun && profile.detection);
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x001A25B0 File Offset: 0x001A07B0
		private void UpdateHudAvailability(bool mapAvailable, bool facecamWanted)
		{
			if (!mapAvailable)
			{
				if (this._expanded)
				{
					this.SetExpanded(false);
				}
				if (this._transition > 0f)
				{
					this._transition = 0f;
					this._transitionVisualsInitialized = false;
					this.UpdateTransition();
				}
			}
			bool flag = facecamWanted && !mapAvailable;
			Canvas canvas = ((this._hudTop != null) ? this._hudTop.GetComponentInParent<Canvas>(true) : null);
			this._canvas.enabled = (mapAvailable || flag) && (canvas == null || canvas.enabled);
			this._mapPanel.gameObject.SetActive(mapAvailable);
			this._screenDim.gameObject.SetActive(mapAvailable);
			this.SetVisualVisible(this._visualVisible);
			this.UpdateWantedPlacement(flag);
			this.UpdateFreeSeedPresentation();
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x001A2680 File Offset: 0x001A0880
		private void UpdateWantedPlacement(bool inPoi)
		{
			if (this._wantedGroup == null || this._uiRoot == null || this._mapPanel == null)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)this._wantedGroup.transform;
			Transform transform = (inPoi ? this._uiRoot : this._mapPanel);
			if (rectTransform.parent != transform)
			{
				rectTransform.SetParent(transform, false);
			}
			Vector2 vector;
			if (!inPoi)
			{
				if (this._wantedInPoi)
				{
					RectTransform rectTransform2 = rectTransform;
					RectTransform rectTransform3 = rectTransform;
					vector = new Vector2(0.5f, 0f);
					rectTransform3.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					rectTransform.pivot = new Vector2(0.5f, 1f);
					rectTransform.anchoredPosition = new Vector2(0f, -7f);
					rectTransform.sizeDelta = new Vector2(160f, 28f);
					rectTransform.localScale = Vector3.one;
					this._wantedGroup.alpha = 1f - this._transition;
				}
				this._wantedInPoi = false;
				return;
			}
			this._wantedInPoi = true;
			this._wantedGroup.alpha = 1f;
			RectTransform rectTransform4 = rectTransform;
			RectTransform rectTransform5 = rectTransform;
			vector = new Vector2(0.5f, 0.5f);
			rectTransform5.anchorMax = vector;
			rectTransform4.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.sizeDelta = new Vector2(160f, 28f);
			rectTransform.localScale = Vector3.one;
			if ((this._faceCamContainer == null || this._distanceHudRect == null || this._scoreHudRect == null) && Time.unscaledTime >= this._nextFaceCamBindingRetry)
			{
				this._nextFaceCamBindingRetry = Time.unscaledTime + 0.5f;
				Canvas canvas = ((this._hudTop != null) ? this._hudTop.GetComponentInParent<Canvas>(true) : null);
				if (canvas != null)
				{
					Transform transform2 = canvas.rootCanvas.transform;
					if (this._faceCamContainer == null)
					{
						this._faceCamContainer = this.FindNamedRectTransform(transform2, "FaceCamContainer");
					}
					if (this._distanceHudRect == null)
					{
						DistanceHUD componentInChildren = transform2.GetComponentInChildren<DistanceHUD>(true);
						this._distanceHudRect = ((componentInChildren != null) ? componentInChildren.transform : null) as RectTransform;
					}
					if (this._scoreHudRect == null)
					{
						RunScoreHUD componentInChildren2 = transform2.GetComponentInChildren<RunScoreHUD>(true);
						this._scoreHudRect = ((componentInChildren2 != null) ? componentInChildren2.transform : null) as RectTransform;
					}
				}
			}
			Rect rect = this._uiRoot.rect;
			Vector2 vector2 = new Vector2(rect.xMax - 100f, rect.yMax - 46f);
			if (this._faceCamContainer != null && this._faceCamContainer.gameObject.activeInHierarchy && GameSettingsService.ShowFaceCamera)
			{
				this._faceCamContainer.GetWorldCorners(this._faceCamWorldCorners);
				Vector2 vector3 = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
				Vector2 vector4 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
				for (int i = 0; i < this._faceCamWorldCorners.Length; i++)
				{
					Vector3 vector5 = this._uiRoot.InverseTransformPoint(this._faceCamWorldCorners[i]);
					vector3 = Vector2.Min(vector3, vector5);
					vector4 = Vector2.Max(vector4, vector5);
				}
				vector2 = new Vector2((vector3.x + vector4.x) * 0.5f, vector4.y + 10f);
			}
			else if (GameSettingsService.ShowDistanceAndScore)
			{
				this.PlaceWantedBelowHud(this._distanceHudRect, rectTransform.rect.size, ref vector2);
				this.PlaceWantedBelowHud(this._scoreHudRect, rectTransform.rect.size, ref vector2);
			}
			vector2.x = Mathf.Clamp(vector2.x, rect.xMin + 88f, rect.xMax - 88f);
			vector2.y = Mathf.Clamp(vector2.y, rect.yMin + 8f, rect.yMax - 36f);
			rectTransform.anchoredPosition = vector2 - rect.center;
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x001A2AB4 File Offset: 0x001A0CB4
		private void PlaceWantedBelowHud(RectTransform hud, Vector2 wantedSize, ref Vector2 position)
		{
			if (hud == null || !hud.gameObject.activeInHierarchy)
			{
				return;
			}
			Graphic component = hud.GetComponent<Graphic>();
			if (component != null && component.canvasRenderer.GetInheritedAlpha() <= 0.001f)
			{
				return;
			}
			hud.GetWorldCorners(this._faceCamWorldCorners);
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			for (int i = 0; i < this._faceCamWorldCorners.Length; i++)
			{
				Vector3 vector3 = this._uiRoot.InverseTransformPoint(this._faceCamWorldCorners[i]);
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			if (vector.x >= position.x + wantedSize.x * 0.5f || vector2.x <= position.x - wantedSize.x * 0.5f)
			{
				return;
			}
			position.y = Mathf.Min(position.y, vector.y - 10f - wantedSize.y);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x001A2BCC File Offset: 0x001A0DCC
		private RectTransform BuildLegend(Transform parent, int uiLayer)
		{
			GameObject gameObject = new GameObject("Map Legend", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(CanvasGroup)
			});
			gameObject.layer = uiLayer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			RectTransform rectTransform2 = component;
			RectTransform rectTransform3 = component;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = vector;
			Vector2 vector2 = (rectTransform2.anchorMax = vector);
			rectTransform.anchorMin = vector2;
			component.anchoredPosition = new Vector2(-547f, 0f);
			component.sizeDelta = new Vector2(286f, 570f);
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(0.075f, 0.035f, 0.075f, 0.965f);
			component2.raycastTarget = false;
			ProceduralMiniMapController.Round(gameObject, 22f);
			this._legendGroup = gameObject.GetComponent<CanvasGroup>();
			this._legendGroup.alpha = 0f;
			this._legendGroup.interactable = false;
			this._legendGroup.blocksRaycasts = false;
			TextMeshProUGUI textMeshProUGUI = ProceduralMiniMapController.CreateLabel(component, "Legend Heading", "LOCATIONS", 20f, FontStyles.Bold);
			textMeshProUGUI.alignment = TextAlignmentOptions.Left;
			RectTransform rectTransform4 = textMeshProUGUI.rectTransform;
			RectTransform rectTransform5 = textMeshProUGUI.rectTransform;
			vector2 = new Vector2(0f, 1f);
			rectTransform5.anchorMax = vector2;
			rectTransform4.anchorMin = vector2;
			textMeshProUGUI.rectTransform.pivot = new Vector2(0f, 1f);
			textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(20f, -16f);
			textMeshProUGUI.rectTransform.sizeDelta = new Vector2(246f, 28f);
			textMeshProUGUI.color = new Color(1f, 0.78f, 0.89f, 1f);
			TextMeshProUGUI textMeshProUGUI2 = ProceduralMiniMapController.CreateLabel(component, "Legend Subheading", "VISIT TO REVEAL", 11f, FontStyles.Bold);
			textMeshProUGUI2.alignment = TextAlignmentOptions.Left;
			textMeshProUGUI2.characterSpacing = 1.4f;
			RectTransform rectTransform6 = textMeshProUGUI2.rectTransform;
			RectTransform rectTransform7 = textMeshProUGUI2.rectTransform;
			vector2 = new Vector2(0f, 1f);
			rectTransform7.anchorMax = vector2;
			rectTransform6.anchorMin = vector2;
			textMeshProUGUI2.rectTransform.pivot = new Vector2(0f, 1f);
			textMeshProUGUI2.rectTransform.anchoredPosition = new Vector2(20f, -44f);
			textMeshProUGUI2.rectTransform.sizeDelta = new Vector2(246f, 20f);
			textMeshProUGUI2.color = new Color(0.78f, 0.67f, 0.76f, 0.84f);
			for (int i = 0; i < ProceduralMiniMapController.LegendPoiKinds.Length; i++)
			{
				this._legendRows.Add(ProceduralMiniMapController.CreateLegendRow(component, uiLayer, ProceduralMiniMapController.LegendPoiKinds[i], ProceduralMiniMapController.LegendPoiLabels[i], -76f - (float)i * 40f));
			}
			this.BuildControlHints(component, uiLayer);
			this.BuildFreeSeedFooter(component);
			return component;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x001A2ED0 File Offset: 0x001A10D0
		private void BuildControlHints(RectTransform parent, int uiLayer)
		{
			RectTransform rectTransform = ProceduralMiniMapController.CreateImage(parent, "Controls Divider", new Color(0.48f, 0.34f, 0.46f, 0.42f)).rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(20f, -437f);
			rectTransform.sizeDelta = new Vector2(246f, 1f);
			TextMeshProUGUI textMeshProUGUI = ProceduralMiniMapController.CreateLabel(parent, "Controls Heading", "MAP CONTROLS", 10f, FontStyles.Bold);
			textMeshProUGUI.alignment = TextAlignmentOptions.Left;
			textMeshProUGUI.characterSpacing = 1.2f;
			textMeshProUGUI.color = new Color(0.78f, 0.67f, 0.76f, 0.84f);
			RectTransform rectTransform2 = textMeshProUGUI.rectTransform;
			RectTransform rectTransform3 = textMeshProUGUI.rectTransform;
			vector = new Vector2(0f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			textMeshProUGUI.rectTransform.pivot = new Vector2(0f, 1f);
			textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(20f, -445f);
			textMeshProUGUI.rectTransform.sizeDelta = new Vector2(246f, 16f);
			ProceduralMiniMapController.CreateControlHint(parent, uiLayer, "M / ESC", "CLOSE", new Vector2(17f, -469f));
			ProceduralMiniMapController.CreateControlHint(parent, uiLayer, "WHEEL", "ZOOM", new Vector2(143f, -469f));
			this._panControlHint = ProceduralMiniMapController.CreateControlHint(parent, uiLayer, "LMB DRAG", "PAN", new Vector2(17f, -504f));
			ProceduralMiniMapController.CreateControlHint(parent, uiLayer, "MMB", "PIN", new Vector2(143f, -504f));
			this.UpdatePanControlHints();
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x001A30B8 File Offset: 0x001A12B8
		private static GameObject CreateControlHint(RectTransform parent, int uiLayer, string key, string action, Vector2 position)
		{
			GameObject gameObject = new GameObject(action + " Control", new Type[] { typeof(RectTransform) });
			gameObject.layer = uiLayer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0f, 1f);
			component.pivot = vector;
			component.anchorMin = (component.anchorMax = vector);
			component.anchoredPosition = position;
			component.sizeDelta = new Vector2(126f, 27f);
			Image image = ProceduralMiniMapController.CreateImage(component, key + " Key", new Color(0.18f, 0.105f, 0.18f, 0.98f));
			RectTransform rectTransform = image.rectTransform;
			vector = new Vector2(0f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(0f, 0f);
			rectTransform.sizeDelta = new Vector2(62f, 24f);
			ProceduralMiniMapController.Round(image.gameObject, 6f);
			TextMeshProUGUI textMeshProUGUI = ProceduralMiniMapController.CreateLabel(rectTransform, key + " Label", key, 10f, FontStyles.Bold);
			ProceduralMiniMapController.Stretch(textMeshProUGUI.rectTransform);
			textMeshProUGUI.color = new Color(0.98f, 0.91f, 0.96f, 1f);
			TextMeshProUGUI textMeshProUGUI2 = ProceduralMiniMapController.CreateLabel(component, action + " Label", action, 10f, FontStyles.Bold);
			textMeshProUGUI2.alignment = TextAlignmentOptions.Left;
			textMeshProUGUI2.color = new Color(0.78f, 0.73f, 0.79f, 0.94f);
			textMeshProUGUI2.rectTransform.anchorMin = new Vector2(0f, 0f);
			textMeshProUGUI2.rectTransform.anchorMax = new Vector2(1f, 1f);
			textMeshProUGUI2.rectTransform.offsetMin = new Vector2(69f, 0f);
			textMeshProUGUI2.rectTransform.offsetMax = Vector2.zero;
			return gameObject;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x001A32B8 File Offset: 0x001A14B8
		private static ProceduralMiniMapController.LegendRowView CreateLegendRow(RectTransform parent, int uiLayer, MiniMapPoiKind kind, string labelText, float y)
		{
			GameObject gameObject = new GameObject(labelText + " Row", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.layer = uiLayer;
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			RectTransform rectTransform2 = component;
			RectTransform rectTransform3 = component;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform3.pivot = vector;
			Vector2 vector2 = (rectTransform2.anchorMax = vector);
			rectTransform.anchorMin = vector2;
			component.anchoredPosition = new Vector2(17f, y);
			component.sizeDelta = new Vector2(252f, 34f);
			Image component2 = gameObject.GetComponent<Image>();
			component2.raycastTarget = false;
			ProceduralMiniMapController.Round(gameObject, 10f);
			GameObject gameObject2 = new GameObject(labelText + " Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPoiIconGraphic)
			});
			gameObject2.layer = uiLayer;
			gameObject2.transform.SetParent(component, false);
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			vector2 = new Vector2(0f, 0.5f);
			component3.anchorMax = vector2;
			component3.anchorMin = vector2;
			component3.pivot = new Vector2(0.5f, 0.5f);
			component3.anchoredPosition = new Vector2(22f, 0f);
			component3.sizeDelta = new Vector2(24f, 24f);
			ProceduralMiniMapPoiIconGraphic component4 = gameObject2.GetComponent<ProceduralMiniMapPoiIconGraphic>();
			component4.raycastTarget = false;
			component4.SetKind(kind);
			TextMeshProUGUI textMeshProUGUI = ProceduralMiniMapController.CreateLabel(component, labelText + " Unknown", "?", 17f, FontStyles.Bold);
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			RectTransform rectTransform4 = textMeshProUGUI.rectTransform;
			RectTransform rectTransform5 = textMeshProUGUI.rectTransform;
			vector2 = new Vector2(0f, 0.5f);
			rectTransform5.anchorMax = vector2;
			rectTransform4.anchorMin = vector2;
			textMeshProUGUI.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.anchoredPosition = new Vector2(22f, 0f);
			textMeshProUGUI.rectTransform.sizeDelta = new Vector2(24f, 24f);
			TextMeshProUGUI textMeshProUGUI2 = ProceduralMiniMapController.CreateLabel(component, labelText + " Label", labelText, 14f, FontStyles.Bold);
			textMeshProUGUI2.alignment = TextAlignmentOptions.Left;
			textMeshProUGUI2.rectTransform.anchorMin = Vector2.zero;
			textMeshProUGUI2.rectTransform.anchorMax = Vector2.one;
			textMeshProUGUI2.rectTransform.offsetMin = new Vector2(44f, 0f);
			textMeshProUGUI2.rectTransform.offsetMax = new Vector2(-12f, 0f);
			return new ProceduralMiniMapController.LegendRowView
			{
				kind = kind,
				discoveredLabel = labelText,
				background = component2,
				icon = component4,
				question = textMeshProUGUI,
				label = textMeshProUGUI2
			};
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x001A35A4 File Offset: 0x001A17A4
		private void UpdateLegendDiscovery()
		{
			for (int i = 0; i < this._legendRows.Count; i++)
			{
				ProceduralMiniMapController.LegendRowView row = this._legendRows[i];
				if (row != null && !(row.background == null) && !(row.icon == null) && !(row.question == null) && !(row.label == null))
				{
					bool flag = this._visitedPoiKinds.Contains(row.kind);
					row.background.color = (flag ? new Color(0.21f, 0.095f, 0.19f, 0.98f) : new Color(0.105f, 0.075f, 0.12f, 0.82f));
					row.icon.enabled = flag;
					row.question.enabled = !flag;
					LocalizedGameText.BindComputed(row.label, delegate
					{
						if (!this._visitedPoiKinds.Contains(row.kind))
						{
							return "???";
						}
						return GameLocalization.GetSource("navigation_ui", row.discoveredLabel);
					}).FitTranslationToRectangle(false);
					row.question.color = new Color(0.62f, 0.55f, 0.61f, 0.72f);
					row.label.color = (flag ? new Color(1f, 0.97f, 1f, 1f) : new Color(0.58f, 0.51f, 0.57f, 0.78f));
				}
			}
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x001A377C File Offset: 0x001A197C
		private static Image CreateImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x001A37F0 File Offset: 0x001A19F0
		private static TextMeshProUGUI CreateLabel(Transform parent, string name, string text, float fontSize, FontStyles style)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = text;
			component.fontSize = fontSize;
			component.fontStyle = style;
			component.alignment = TextAlignmentOptions.Center;
			component.color = new Color(0.91f, 0.94f, 0.96f, 0.94f);
			component.raycastTarget = false;
			LocalizedGameText.BindSource(component, "navigation_ui", text, null).FitTranslationToRectangle(false);
			return component;
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x001A38DC File Offset: 0x001A1ADC
		private static void Center(RectTransform rect, Vector2 size)
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = Vector2.zero;
			rect.sizeDelta = size;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x001A3923 File Offset: 0x001A1B23
		private static Sprite GetCircleSprite()
		{
			if (ProceduralMiniMapController._circleSprite == null)
			{
				ProceduralMiniMapController.EnsureRadialSprites();
			}
			return ProceduralMiniMapController._circleSprite;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x001A393C File Offset: 0x001A1B3C
		private static Sprite GetRingSprite()
		{
			if (ProceduralMiniMapController._ringSprite == null)
			{
				ProceduralMiniMapController.EnsureRadialSprites();
			}
			return ProceduralMiniMapController._ringSprite;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x001A3958 File Offset: 0x001A1B58
		private static void EnsureRadialSprites()
		{
			if (ProceduralMiniMapController._circleSprite != null && ProceduralMiniMapController._ringSprite != null)
			{
				return;
			}
			using (ProceduralMiniMapController.BuildRadialSpritesMarker.Auto())
			{
				int num = 262144;
				byte[] array = new byte[num];
				byte[] array2 = new byte[num];
				float num2 = 255f;
				float num3 = 251f;
				float num4 = (num2 - 0.5f) * (num2 - 0.5f);
				float num5 = (num2 + 0.5f) * (num2 + 0.5f);
				float num6 = (num3 - 0.5f) * (num3 - 0.5f);
				float num7 = (num3 + 0.5f) * (num3 + 0.5f);
				float num8 = 255.5f;
				int num9 = 0;
				for (int i = 0; i < 512; i++)
				{
					float num10 = (float)i - num8;
					float num11 = num10 * num10;
					int j = 0;
					while (j < 512)
					{
						float num12 = (float)j - num8;
						float num13 = num12 * num12 + num11;
						float num14 = 0f;
						bool flag = false;
						float num15;
						if (num13 <= num4)
						{
							num15 = 1f;
						}
						else if (num13 >= num5)
						{
							num15 = 0f;
						}
						else
						{
							num14 = (float)Math.Sqrt((double)num13);
							flag = true;
							num15 = num2 - num14 + 0.5f;
						}
						array[num9] = (byte)(num15 * 255f + 0.5f);
						if (num15 > 0f && num13 > num6)
						{
							float num16;
							if (num13 >= num7)
							{
								num16 = 1f;
							}
							else
							{
								if (!flag)
								{
									num14 = (float)Math.Sqrt((double)num13);
								}
								num16 = num14 - num3 + 0.5f;
							}
							array2[num9] = (byte)(num15 * num16 * 255f + 0.5f);
						}
						j++;
						num9++;
					}
				}
				if (ProceduralMiniMapController._circleSprite == null)
				{
					ProceduralMiniMapController._circleSprite = ProceduralMiniMapController.CreateRadialSprite("ProceduralMiniMapCircle", array);
				}
				if (ProceduralMiniMapController._ringSprite == null)
				{
					ProceduralMiniMapController._ringSprite = ProceduralMiniMapController.CreateRadialSprite("ProceduralMiniMapRing", array2);
				}
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x001A3B64 File Offset: 0x001A1D64
		private static Sprite CreateRadialSprite(string name, byte[] pixels)
		{
			Texture2D texture2D = new Texture2D(512, 512, TextureFormat.Alpha8, false, true);
			texture2D.name = name;
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			texture2D.SetPixelData<byte>(pixels, 0, 0);
			texture2D.Apply(false, true);
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 512f, 512f), Vector2.one * 0.5f, 100f);
			sprite.name = name;
			sprite.hideFlags = HideFlags.HideAndDontSave;
			return sprite;
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x001A3BF4 File Offset: 0x001A1DF4
		private void BuildFreeSeedFooter(RectTransform legend)
		{
			Image image = ProceduralMiniMapController.CreateImage(legend, "Free World Seed Footer", new Color(0.075f, 0.035f, 0.075f, 0.965f));
			this._seedFooter = image.rectTransform;
			RectTransform seedFooter = this._seedFooter;
			RectTransform seedFooter2 = this._seedFooter;
			Vector2 vector = new Vector2(0f, 0f);
			seedFooter2.anchorMax = vector;
			seedFooter.anchorMin = vector;
			this._seedFooter.pivot = new Vector2(0f, 1f);
			this._seedFooter.anchoredPosition = new Vector2(0f, -10f);
			this._seedFooter.sizeDelta = new Vector2(286f, 78f);
			ProceduralMiniMapController.Round(image.gameObject, 16f);
			this._expandedSeedLabel = ProceduralMiniMapController.CreateLabel(this._seedFooter, "Seed Label", string.Empty, 16f, FontStyles.Bold);
			RectTransform rectTransform = this._expandedSeedLabel.rectTransform;
			Vector2 vector2 = new Vector2(0f, 1f);
			rectTransform.pivot = vector2;
			vector = (rectTransform.anchorMax = vector2);
			rectTransform.anchorMin = vector;
			rectTransform.anchoredPosition = new Vector2(20f, -8f);
			rectTransform.sizeDelta = new Vector2(246f, 24f);
			this._expandedSeedLabel.enableWordWrapping = false;
			this._expandedSeedLabel.color = new Color(1f, 0.78f, 0.89f, 1f);
			Image image2 = ProceduralMiniMapController.CreateImage(this._seedFooter, "Copy Seed", new Color(0.22f, 0.105f, 0.2f, 1f));
			RectTransform rectTransform2 = image2.rectTransform;
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			RectTransform rectTransform5 = rectTransform2;
			vector2 = new Vector2(0f, 1f);
			rectTransform5.pivot = vector2;
			vector = (rectTransform4.anchorMax = vector2);
			rectTransform3.anchorMin = vector;
			rectTransform2.anchoredPosition = new Vector2(20f, -36f);
			rectTransform2.sizeDelta = new Vector2(246f, 32f);
			image2.raycastTarget = true;
			ProceduralMiniMapController.Round(image2.gameObject, 8f);
			this._copySeedButton = image2.gameObject.AddComponent<Button>();
			this._copySeedButton.targetGraphic = image2;
			ColorBlock colors = this._copySeedButton.colors;
			colors.highlightedColor = new Color(1f, 0.78f, 0.9f, 1f);
			colors.pressedColor = new Color(0.75f, 0.55f, 0.7f, 1f);
			this._copySeedButton.colors = colors;
			this._copySeedButton.onClick.AddListener(new UnityAction(this.CopyFreeWorldSeed));
			this._copySeedLabel = ProceduralMiniMapController.CreateLabel(rectTransform2, "Copy Seed Label", "COPY SEED", 14f, FontStyles.Bold);
			ProceduralMiniMapController.Stretch(this._copySeedLabel.rectTransform);
			this._copySeedLabel.rectTransform.offsetMin = new Vector2(10f, 0f);
			this._copySeedLabel.rectTransform.offsetMax = new Vector2(-10f, 0f);
			this._copySeedLabel.enableWordWrapping = false;
			this._seedFooter.gameObject.SetActive(false);
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x001A3F24 File Offset: 0x001A2124
		private void RefreshFreeWorldSeed()
		{
			int num;
			bool flag = RunModeService.TryGetFreeWorldSeed(out num);
			bool flag2 = flag != this._hasFreeWorldSeed || num != this._freeWorldSeed;
			this._hasFreeWorldSeed = flag;
			this._freeWorldSeed = num;
			if (flag2)
			{
				this.SetSeedCopied(false);
				this._seedCopyPointerPressed = false;
				this._lastSeedCopyFrame = -1;
			}
			if (flag)
			{
				string text = num.ToString(CultureInfo.InvariantCulture);
				LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this._expandedSeedLabel, "navigation_ui", "SEED {0}", new object[] { text });
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(true);
				}
			}
			this.UpdateFreeSeedPresentation();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x001A3FB8 File Offset: 0x001A21B8
		private void UpdateFreeSeedPresentation()
		{
			if (!this._expanded)
			{
				this._seedCopyPointerPressed = false;
			}
			if (this._seedFooter != null)
			{
				this._seedFooter.gameObject.SetActive(this._hasFreeWorldSeed && this._expanded);
			}
			if (this._copySeedButton != null)
			{
				this._copySeedButton.interactable = this._hasFreeWorldSeed && this._expanded;
			}
			if (this._legendGroup != null)
			{
				this._legendGroup.blocksRaycasts = this._hasFreeWorldSeed && this._expanded && this._transition > 0.95f;
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x001A4068 File Offset: 0x001A2268
		private void CopyFreeWorldSeed()
		{
			int num;
			if (!this.CanCopyFreeWorldSeed() || !RunModeService.TryGetFreeWorldSeed(out num) || this._lastSeedCopyFrame == Time.frameCount)
			{
				return;
			}
			GUIUtility.systemCopyBuffer = num.ToString(CultureInfo.InvariantCulture);
			this._lastSeedCopyFrame = Time.frameCount;
			this.SetSeedCopied(true);
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x001A40B8 File Offset: 0x001A22B8
		private bool CanCopyFreeWorldSeed()
		{
			return this._expanded && this._mapLease != null && this._mapLease.IsActive && this._canvas != null && this._canvas.isActiveAndEnabled && this._legendGroup != null && this._legendGroup.alpha > 0.95f && this._copySeedButton != null && this._copySeedButton.isActiveAndEnabled && this._copySeedButton.IsInteractable();
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x001A4148 File Offset: 0x001A2348
		private bool HandleSeedCopyPointer(Vector2 screenPosition, bool pressed, bool released)
		{
			if (!this.CanCopyFreeWorldSeed())
			{
				this._seedCopyPointerPressed = false;
				return false;
			}
			Camera camera = UIScreenCoordinates.ResolveEventCamera(this._canvas);
			RectTransform rectTransform = (RectTransform)this._copySeedButton.transform;
			Rect rect = rectTransform.rect;
			Vector2 vector;
			bool flag = UIScreenCoordinates.TryScreenPointToLocalPoint(rectTransform, this._canvas, screenPosition, out vector) && vector.x >= rect.xMin && vector.x <= rect.xMax && vector.y >= rect.yMin && vector.y <= rect.yMax && this._copySeedButton.targetGraphic.Raycast(screenPosition, camera);
			if (pressed)
			{
				this._seedCopyPointerPressed = flag;
			}
			bool seedCopyPointerPressed = this._seedCopyPointerPressed;
			if (released)
			{
				this._seedCopyPointerPressed = false;
				if (seedCopyPointerPressed && flag)
				{
					this.CopyFreeWorldSeed();
				}
			}
			return seedCopyPointerPressed;
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x001A4218 File Offset: 0x001A2418
		private void SetSeedCopied(bool copied)
		{
			if (this._seedCopied == copied)
			{
				return;
			}
			this._seedCopied = copied;
			this._seedCopiedUntil = (copied ? (Time.unscaledTime + 1.5f) : 0f);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._copySeedLabel, "navigation_ui", copied ? "COPIED" : "COPY SEED", null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(true);
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x001A427C File Offset: 0x001A247C
		private void UpdateSeedCopyFeedback()
		{
			if (this._seedCopied && Time.unscaledTime >= this._seedCopiedUntil)
			{
				this.SetSeedCopied(false);
			}
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x001A429C File Offset: 0x001A249C
		private void ClearFreeSeedUiReferences()
		{
			this._seedFooter = null;
			this._expandedSeedLabel = null;
			this._copySeedButton = null;
			this._copySeedLabel = null;
			this._hasFreeWorldSeed = false;
			this._freeWorldSeed = 0;
			this._seedCopied = false;
			this._seedCopiedUntil = 0f;
			this._seedCopyPointerPressed = false;
			this._lastSeedCopyFrame = -1;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x001A42F3 File Offset: 0x001A24F3
		private void ClearMiniMapPreviewCache()
		{
			this.ClearMiniMapTileDescriptors();
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x001A42FC File Offset: 0x001A24FC
		private void AppendMiniMapPreview(float cellSize, int previewRadius)
		{
			previewRadius = Mathf.Clamp(previewRadius, 0, 8);
			Vector2Int vector2Int = this._chunkManager.WorldToCell(this.ViewCenter);
			HashSet<Vector2Int> previewOccupiedScratch = this._previewOccupiedScratch;
			previewOccupiedScratch.Clear();
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (this._chunkManager.IsChunkPresentationVisible(value))
				{
					Vector2Int vector2Int2 = ((value.chunkData != null) ? value.chunkData.gridSize : this.ResolvePreSpawnedGridSize(value.chunkRoot.transform, cellSize));
					ProceduralMiniMapController.MarkPreviewCells(previewOccupiedScratch, keyValuePair.Key, vector2Int2);
				}
			}
			for (int i = -previewRadius; i <= previewRadius; i++)
			{
				for (int j = -previewRadius; j <= previewRadius; j++)
				{
					Vector2Int vector2Int3 = vector2Int + new Vector2Int(j, i);
					Vector2Int vector2Int4;
					ChunkSpawnResolver.SpawnEntry spawnEntry;
					if (!previewOccupiedScratch.Contains(vector2Int3) && this._chunkManager.TryGetObservedMiniMapEntry(vector2Int3, out vector2Int4, out spawnEntry) && ProceduralMiniMapController.PreviewCellsAreFree(previewOccupiedScratch, vector2Int4, spawnEntry.chunkData.gridSize))
					{
						MiniMapChunkVisual miniMapChunkVisual = this.BuildSeededPreviewChunk(vector2Int4, spawnEntry, cellSize);
						if (miniMapChunkVisual != null)
						{
							this._chunks.Add(miniMapChunkVisual);
							ProceduralMiniMapController.MarkPreviewCells(previewOccupiedScratch, vector2Int4, spawnEntry.chunkData.gridSize);
						}
					}
				}
			}
			for (int k = -previewRadius; k <= previewRadius; k++)
			{
				for (int l = -previewRadius; l <= previewRadius; l++)
				{
					Vector2Int vector2Int5 = vector2Int + new Vector2Int(l, k);
					ChunkSpawnResolver.SpawnEntry spawnEntry2;
					if (!previewOccupiedScratch.Contains(vector2Int5) && this._chunkManager.TryResolveMiniMapMetadataPreview(vector2Int5, out spawnEntry2) && spawnEntry2.IsValid)
					{
						MiniMapChunkVisual miniMapChunkVisual2 = this.BuildSeededMetadataCell(vector2Int5, spawnEntry2, cellSize);
						if (miniMapChunkVisual2 != null)
						{
							this._chunks.Add(miniMapChunkVisual2);
						}
					}
				}
			}
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x001A44E0 File Offset: 0x001A26E0
		private void AddMiniMapPreviewPoi(Vector3 center, MiniMapPoiKind kind, Vector2 size, float yaw)
		{
			if (kind != MiniMapPoiKind.None)
			{
				this.AddPoi(center, kind, true, false, null, size, yaw);
			}
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x001A44F4 File Offset: 0x001A26F4
		private void ClearMiniMapTileDescriptors()
		{
			this._miniMapSlotDescriptors.Clear();
			this._miniMapZoneDescriptors.Clear();
			this._tileDescriptorAtlas = null;
			this._tileDescriptorSnapshotAtlas = null;
			this._tileDescriptorTexture = null;
			this._tileDescriptorEntries = null;
			this._tileDescriptorEntryCount = 0;
			this._tileDescriptorAtlasRevision = 0;
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x001A4544 File Offset: 0x001A2744
		private void EnsureMiniMapTileDescriptorContext()
		{
			Texture2D texture2D = ((this._styleAtlas != null) ? this._styleAtlas.Atlas : null);
			List<ProceduralMiniMapStyleAtlas.Entry> list = ((this._styleAtlas != null) ? this._styleAtlas.Entries : null);
			int num = ((list != null) ? list.Count : 0);
			int num2 = ((this._styleAtlas != null) ? this._styleAtlas.Revision : 0);
			if (this._tileDescriptorAtlas == this._styleAtlas && this._tileDescriptorSnapshotAtlas == this._snapshotStyleAtlas && this._tileDescriptorTexture == texture2D && this._tileDescriptorEntries == list && this._tileDescriptorEntryCount == num && this._tileDescriptorAtlasRevision == num2)
			{
				return;
			}
			this.ClearMiniMapTileDescriptors();
			this._tileDescriptorAtlas = this._styleAtlas;
			this._tileDescriptorSnapshotAtlas = this._snapshotStyleAtlas;
			this._tileDescriptorTexture = texture2D;
			this._tileDescriptorEntries = list;
			this._tileDescriptorEntryCount = num;
			this._tileDescriptorAtlasRevision = num2;
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x001A4630 File Offset: 0x001A2830
		private ProceduralMiniMapController.MiniMapSlotDescriptor GetMiniMapSlotDescriptor(ChunkTemplate template, int slotIndex)
		{
			this.EnsureMiniMapTileDescriptorContext();
			if (template == null || template.zoneSlots == null || slotIndex < 0 || slotIndex >= template.zoneSlots.Count)
			{
				return default(ProceduralMiniMapController.MiniMapSlotDescriptor);
			}
			long num = ((long)template.GetInstanceID() << 32) | (long)((ulong)slotIndex);
			ProceduralMiniMapController.MiniMapSlotDescriptor miniMapSlotDescriptor;
			if (this._miniMapSlotDescriptors.TryGetValue(num, out miniMapSlotDescriptor))
			{
				return miniMapSlotDescriptor;
			}
			ZoneSlot zoneSlot = template.zoneSlots[slotIndex];
			Vector3 vector = Vector3.zero;
			float num2 = ((zoneSlot != null) ? ((float)zoneSlot.rotation * 90f) : 0f);
			if (zoneSlot != null && zoneSlot.spawnTransform != null && template.chunkPrefab != null)
			{
				Transform transform = template.chunkPrefab.transform;
				vector = transform.InverseTransformPoint(zoneSlot.spawnTransform.position);
				vector.y = 0f;
				num2 = Mathf.DeltaAngle(transform.eulerAngles.y, zoneSlot.spawnTransform.eulerAngles.y);
			}
			miniMapSlotDescriptor = new ProceduralMiniMapController.MiniMapSlotDescriptor(vector, num2);
			if (this._miniMapSlotDescriptors.Count < 256)
			{
				this._miniMapSlotDescriptors.Add(num, miniMapSlotDescriptor);
			}
			return miniMapSlotDescriptor;
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x001A4750 File Offset: 0x001A2950
		private ProceduralMiniMapController.MiniMapZoneDescriptor GetMiniMapZoneDescriptor(ZoneData zone)
		{
			this.EnsureMiniMapTileDescriptorContext();
			if (zone == null)
			{
				return default(ProceduralMiniMapController.MiniMapZoneDescriptor);
			}
			int instanceID = zone.GetInstanceID();
			ProceduralMiniMapController.MiniMapZoneDescriptor miniMapZoneDescriptor;
			if (this._miniMapZoneDescriptors.TryGetValue(instanceID, out miniMapZoneDescriptor))
			{
				return miniMapZoneDescriptor;
			}
			string text = ((!string.IsNullOrEmpty(zone.zoneName)) ? zone.zoneName : zone.name);
			MiniMapTileReference miniMapTileReference = default(MiniMapTileReference);
			ProceduralMiniMapStyleAtlas.Entry entry;
			if (this._snapshotStyleAtlas == null && this._styleAtlas != null && this._styleAtlas.TryGet(zone, text, out entry))
			{
				miniMapTileReference = MiniMapTileReference.From(entry);
			}
			miniMapZoneDescriptor = new ProceduralMiniMapController.MiniMapZoneDescriptor(text, ProceduralMiniMapController.ResolveZoneKind(text), ProceduralMiniMapController.ResolvePoiKind(text), miniMapTileReference);
			if (this._miniMapZoneDescriptors.Count < 256)
			{
				this._miniMapZoneDescriptors.Add(instanceID, miniMapZoneDescriptor);
			}
			return miniMapZoneDescriptor;
		}

		// Token: 0x04004488 RID: 17544
		private GameObject _controllerReticle;

		// Token: 0x04004489 RID: 17545
		private GameObject _controllerMapHint;

		// Token: 0x0400448A RID: 17546
		private bool? _controllerHintAllowsPan;

		// Token: 0x0400448B RID: 17547
		private const string RuntimeObjectName = "[Procedural MiniMap Runtime]";

		// Token: 0x0400448C RID: 17548
		private const float CompactSize = 176f;

		// Token: 0x0400448D RID: 17549
		private const float CompactHudOffset = 108f;

		// Token: 0x0400448E RID: 17550
		private const int WantedIconSlots = 5;

		// Token: 0x0400448F RID: 17551
		private const float ExpandedSize = 760f;

		// Token: 0x04004490 RID: 17552
		private const float TransitionDuration = 0.24f;

		// Token: 0x04004491 RID: 17553
		private const int SnapshotResolution = 256;

		// Token: 0x04004492 RID: 17554
		private const int SnapshotLimit = 2;

		// Token: 0x04004493 RID: 17555
		private const int CompactPreviewRadius = 2;

		// Token: 0x04004494 RID: 17556
		private const int MinExpandedPreviewRadius = 3;

		// Token: 0x04004495 RID: 17557
		private const int MaxExpandedPreviewRadius = 8;

		// Token: 0x04004496 RID: 17558
		private const float StreamingRebuildQuietWindow = 0.15f;

		// Token: 0x04004497 RID: 17559
		private const float CompactPixelsPerMeter = 1.6f;

		// Token: 0x04004498 RID: 17560
		private const float ExpandedBasePixelsPerMeter = 2.05f;

		// Token: 0x04004499 RID: 17561
		private const float MinExpandedZoom = 1f;

		// Token: 0x0400449A RID: 17562
		private const float MaxExpandedZoom = 3f;

		// Token: 0x0400449B RID: 17563
		private const float PinMarkerHitRadius = 18f;

		// Token: 0x0400449C RID: 17564
		private const float MissingBindingRetryInterval = 0.5f;

		// Token: 0x0400449D RID: 17565
		private const float CompactViewRefreshInterval = 0.033333335f;

		// Token: 0x0400449E RID: 17566
		private const float UiFitRefreshInterval = 0.5f;

		// Token: 0x0400449F RID: 17567
		private const float PoiDedupRadius = 30f;

		// Token: 0x040044A0 RID: 17568
		private const float MetroPoiDedupRadius = 48f;

		// Token: 0x040044A1 RID: 17569
		private const int RadialSpriteResolution = 512;

		// Token: 0x040044A2 RID: 17570
		private const string GateReason = "ProceduralMiniMap";

		// Token: 0x040044A3 RID: 17571
		private const PlayerActivity MapBlockedActivities = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly | PlayerActivity.ArmsPose;

		// Token: 0x040044A4 RID: 17572
		private static readonly string[] HudContainersToHide = new string[] { "UITop", "UIBottom" };

		// Token: 0x040044A5 RID: 17573
		private static readonly MiniMapPoiKind[] LegendPoiKinds = new MiniMapPoiKind[]
		{
			MiniMapPoiKind.Home,
			MiniMapPoiKind.Park,
			MiniMapPoiKind.Metro,
			MiniMapPoiKind.Kombini,
			MiniMapPoiKind.University,
			MiniMapPoiKind.Stadium,
			MiniMapPoiKind.Mall,
			MiniMapPoiKind.Gallery,
			MiniMapPoiKind.Pool
		};

		// Token: 0x040044A6 RID: 17574
		private static readonly string[] LegendPoiLabels = new string[] { "CielHome", "Park", "Subway", "Kombini", "University", "Stadium", "Shopping Mall", "Art Gallery", "Swimming Pool" };

		// Token: 0x040044A7 RID: 17575
		private static ProceduralMiniMapController _instance;

		// Token: 0x040044A8 RID: 17576
		private static Sprite _circleSprite;

		// Token: 0x040044A9 RID: 17577
		private static Sprite _ringSprite;

		// Token: 0x040044AA RID: 17578
		private static ResourceRequest _styleAtlasRequest;

		// Token: 0x040044AB RID: 17579
		private static ProceduralMiniMapStyleAtlas _preloadedStyleAtlas;

		// Token: 0x040044AC RID: 17580
		private static ResourceRequest _hudStyleRequest;

		// Token: 0x040044AD RID: 17581
		private static ProceduralMiniMapHudStyle _preloadedHudStyle;

		// Token: 0x040044AE RID: 17582
		private static readonly ProfilerMarker BuildRadialSpritesMarker = new ProfilerMarker("ProceduralMiniMapController.BuildRadialSprites");

		// Token: 0x040044AF RID: 17583
		private static readonly ProfilerMarker UpdateMapViewMarker = new ProfilerMarker("ProceduralMiniMapController.UpdateMapView");

		// Token: 0x040044B0 RID: 17584
		private static readonly ProfilerMarker RefreshEnvironmentBindingMarker = new ProfilerMarker("ProceduralMiniMapController.RefreshEnvironmentBinding");

		// Token: 0x040044B1 RID: 17585
		private static readonly ProfilerMarker RebuildChunkVisualsMarker = new ProfilerMarker("ProceduralMiniMapController.RebuildChunkVisuals");

		// Token: 0x040044B2 RID: 17586
		private static readonly ProfilerMarker ResolveStartHomeMarker = new ProfilerMarker("ProceduralMiniMapController.ResolveStartHome");

		// Token: 0x040044B3 RID: 17587
		private static readonly ProfilerMarker DisableLegacyMarker = new ProfilerMarker("ProceduralMiniMapController.DisableLegacy");

		// Token: 0x040044B4 RID: 17588
		private static readonly ProfilerMarker EnsureHudBindingMarker = new ProfilerMarker("ProceduralMiniMapController.EnsureHudBinding");

		// Token: 0x040044B5 RID: 17589
		private static readonly ProfilerMarker BuildUiMarker = new ProfilerMarker("ProceduralMiniMapController.BuildUi");

		// Token: 0x040044B6 RID: 17590
		private static readonly ProfilerMarker LoadStyleAtlasMarker = new ProfilerMarker("ProceduralMiniMapController.LoadStyleAtlas");

		// Token: 0x040044B7 RID: 17591
		private readonly List<MiniMapChunkVisual> _chunks = new List<MiniMapChunkVisual>(64);

		// Token: 0x040044B8 RID: 17592
		private readonly Stack<MiniMapChunkVisual> _chunkVisualPool = new Stack<MiniMapChunkVisual>(64);

		// Token: 0x040044B9 RID: 17593
		private readonly List<MiniMapPoiVisual> _pois = new List<MiniMapPoiVisual>(24);

		// Token: 0x040044BA RID: 17594
		private readonly HashSet<Vector2Int> _previewOccupiedScratch = new HashSet<Vector2Int>();

		// Token: 0x040044BB RID: 17595
		private readonly List<Vector2Int> _previewCandidateCellsScratch = new List<Vector2Int>(289);

		// Token: 0x040044BC RID: 17596
		private readonly List<ProceduralMiniMapController.LegendRowView> _legendRows = new List<ProceduralMiniMapController.LegendRowView>(9);

		// Token: 0x040044BD RID: 17597
		private readonly HashSet<MiniMapPoiKind> _visitedPoiKinds = new HashSet<MiniMapPoiKind>();

		// Token: 0x040044BE RID: 17598
		private readonly HashSet<IncrementalPooledZoneActivation> _pendingPoiReadinessGates = new HashSet<IncrementalPooledZoneActivation>();

		// Token: 0x040044BF RID: 17599
		private readonly Dictionary<int, Vector2Int> _preSpawnedGridSizes = new Dictionary<int, Vector2Int>(8);

		// Token: 0x040044C0 RID: 17600
		private readonly Dictionary<int, string> _preSpawnedSemanticNames = new Dictionary<int, string>(8);

		// Token: 0x040044C1 RID: 17601
		private readonly Dictionary<int, Vector3> _startHomePositions = new Dictionary<int, Vector3>(8);

		// Token: 0x040044C2 RID: 17602
		private bool _hasHomePoiPosition;

		// Token: 0x040044C3 RID: 17603
		private Vector3 _homePoiPosition;

		// Token: 0x040044C4 RID: 17604
		private bool _homePoiExplicitMarker;

		// Token: 0x040044C5 RID: 17605
		private readonly List<ChunkInstance> _snapshotCandidatesScratch = new List<ChunkInstance>(64);

		// Token: 0x040044C6 RID: 17606
		private readonly Vector3[] _hudCanvasWorldCorners = new Vector3[4];

		// Token: 0x040044C7 RID: 17607
		private readonly List<GameObject> _sceneRoots = new List<GameObject>(32);

		// Token: 0x040044C8 RID: 17608
		private readonly Stack<Transform> _transformSearch = new Stack<Transform>(64);

		// Token: 0x040044C9 RID: 17609
		private readonly List<CompassPro> _legacyCompasses = new List<CompassPro>(2);

		// Token: 0x040044CA RID: 17610
		private ChunkManager _chunkManager;

		// Token: 0x040044CB RID: 17611
		private Transform _player;

		// Token: 0x040044CC RID: 17612
		private PlayerPoiEmoteContext _poiContext;

		// Token: 0x040044CD RID: 17613
		private MatchStatsService _statsService;

		// Token: 0x040044CE RID: 17614
		private Canvas _canvas;

		// Token: 0x040044CF RID: 17615
		private GraphicRaycaster _mapRaycaster;

		// Token: 0x040044D0 RID: 17616
		private RectTransform _uiRoot;

		// Token: 0x040044D1 RID: 17617
		private RectTransform _hudTop;

		// Token: 0x040044D2 RID: 17618
		private RectTransform _mapPanel;

		// Token: 0x040044D3 RID: 17619
		private Image _screenDim;

		// Token: 0x040044D4 RID: 17620
		private Image _outerShadow;

		// Token: 0x040044D5 RID: 17621
		private Image _rim;

		// Token: 0x040044D6 RID: 17622
		private ProceduralMiniMapGraphic _mapGraphic;

		// Token: 0x040044D7 RID: 17623
		private ProceduralMiniMapPoiGraphic _poiGraphic;

		// Token: 0x040044D8 RID: 17624
		private ProceduralMiniMapMarkerGraphic _playerMarker;

		// Token: 0x040044D9 RID: 17625
		private RectTransform _playerMarkerRect;

		// Token: 0x040044DA RID: 17626
		private ProceduralMiniMapStyleAtlas _styleAtlas;

		// Token: 0x040044DB RID: 17627
		private CanvasGroup _legendGroup;

		// Token: 0x040044DC RID: 17628
		private CanvasGroup _wantedGroup;

		// Token: 0x040044DD RID: 17629
		private readonly Image[] _wantedIcons = new Image[5];

		// Token: 0x040044DE RID: 17630
		private Sprite _wantedIconSprite;

		// Token: 0x040044DF RID: 17631
		private int _lastWantedLevel = -1;

		// Token: 0x040044E0 RID: 17632
		private RectTransform _faceCamContainer;

		// Token: 0x040044E1 RID: 17633
		private RectTransform _distanceHudRect;

		// Token: 0x040044E2 RID: 17634
		private RectTransform _scoreHudRect;

		// Token: 0x040044E3 RID: 17635
		private bool _wantedInPoi;

		// Token: 0x040044E4 RID: 17636
		private float _nextFaceCamBindingRetry;

		// Token: 0x040044E5 RID: 17637
		private readonly Vector3[] _faceCamWorldCorners = new Vector3[4];

		// Token: 0x040044E6 RID: 17638
		private readonly Queue<ChunkInstance> _snapshotQueue = new Queue<ChunkInstance>(2);

		// Token: 0x040044E7 RID: 17639
		private readonly HashSet<int> _queuedSnapshotIds = new HashSet<int>();

		// Token: 0x040044E8 RID: 17640
		private readonly Dictionary<ChunkInstance, MiniMapTileReference> _runtimeChunkTiles = new Dictionary<ChunkInstance, MiniMapTileReference>(2);

		// Token: 0x040044E9 RID: 17641
		private Texture2D _snapshotAtlas;

		// Token: 0x040044EA RID: 17642
		private ProceduralMiniMapStyleAtlas _snapshotStyleAtlas;

		// Token: 0x040044EB RID: 17643
		private Camera _snapshotCamera;

		// Token: 0x040044EC RID: 17644
		private RenderTexture _snapshotTarget;

		// Token: 0x040044ED RID: 17645
		private float _nextSnapshotTime;

		// Token: 0x040044EE RID: 17646
		private bool _expanded;

		// Token: 0x040044EF RID: 17647
		private bool _mapToggleHeld;

		// Token: 0x040044F0 RID: 17648
		private int _lastMapToggleFrame = -1;

		// Token: 0x040044F1 RID: 17649
		private float _nextMapToggleTime;

		// Token: 0x040044F2 RID: 17650
		private float _transition;

		// Token: 0x040044F3 RID: 17651
		private float _nextLegacyMapCheck;

		// Token: 0x040044F4 RID: 17652
		private float _nextMissingBindingRetry;

		// Token: 0x040044F5 RID: 17653
		private float _nextMapViewUpdate;

		// Token: 0x040044F6 RID: 17654
		private float _nextUiFitTime;

		// Token: 0x040044F7 RID: 17655
		private bool _transitionVisualsInitialized;

		// Token: 0x040044F8 RID: 17656
		private bool _environmentBindingDirty = true;

		// Token: 0x040044F9 RID: 17657
		private bool _legacyCompassCacheDirty = true;

		// Token: 0x040044FA RID: 17658
		private Vector2Int _lastPreviewCell = new Vector2Int(int.MinValue, int.MinValue);

		// Token: 0x040044FB RID: 17659
		private Vector3 _lastPlayerPosition = new Vector3(float.PositiveInfinity, 0f, 0f);

		// Token: 0x040044FC RID: 17660
		private Vector2Int _previewSortCenter;

		// Token: 0x040044FD RID: 17661
		private Vector3 _snapshotSortPlayerPosition;

		// Token: 0x040044FE RID: 17662
		private Comparison<Vector2Int> _previewCellComparison;

		// Token: 0x040044FF RID: 17663
		private Comparison<ChunkInstance> _snapshotCandidateComparison;

		// Token: 0x04004500 RID: 17664
		private float _lastPlayerYaw = float.PositiveInfinity;

		// Token: 0x04004501 RID: 17665
		private float _expandedZoom = 1f;

		// Token: 0x04004502 RID: 17666
		private float _currentPixelsPerMeter = 1.6f;

		// Token: 0x04004503 RID: 17667
		private float _currentHeading;

		// Token: 0x04004504 RID: 17668
		private Vector3 _panOffset;

		// Token: 0x04004505 RID: 17669
		private bool _viewDirty;

		// Token: 0x04004506 RID: 17670
		private bool _chunkVisualsDirty;

		// Token: 0x04004507 RID: 17671
		private float _nextChunkVisualRebuild;

		// Token: 0x04004508 RID: 17672
		private bool _dragging;

		// Token: 0x04004509 RID: 17673
		private Vector2 _lastDragScreen;

		// Token: 0x0400450A RID: 17674
		private GameObject _panControlHint;

		// Token: 0x0400450B RID: 17675
		private ProceduralMiniMapPinGraphic _pinMarker;

		// Token: 0x0400450C RID: 17676
		private PlayerActivityLease _mapLease;

		// Token: 0x0400450D RID: 17677
		private vThirdPersonInput _playerInput;

		// Token: 0x0400450E RID: 17678
		private bool _gameplayStateCaptured;

		// Token: 0x0400450F RID: 17679
		private bool _prevLockCamera;

		// Token: 0x04004510 RID: 17680
		private bool _capturedCoverInput;

		// Token: 0x04004511 RID: 17681
		private bool _prevCoverLockInput;

		// Token: 0x04004512 RID: 17682
		private bool _prevSuppressZoom;

		// Token: 0x04004513 RID: 17683
		private bool _prevForceCameraLock;

		// Token: 0x04004514 RID: 17684
		private CursorLockMode _prevCursorLock;

		// Token: 0x04004515 RID: 17685
		private bool _prevCursorVisible;

		// Token: 0x04004516 RID: 17686
		private GameObject[] _hiddenHud;

		// Token: 0x04004517 RID: 17687
		private bool[] _hiddenHudPrevActive;

		// Token: 0x04004518 RID: 17688
		private CanvasGroup _visualGroup;

		// Token: 0x04004519 RID: 17689
		private CanvasGroup _backdropVisualGroup;

		// Token: 0x0400451A RID: 17690
		private bool _visualVisible = true;

		// Token: 0x0400451B RID: 17691
		private RectTransform _seedFooter;

		// Token: 0x0400451C RID: 17692
		private TextMeshProUGUI _expandedSeedLabel;

		// Token: 0x0400451D RID: 17693
		private Button _copySeedButton;

		// Token: 0x0400451E RID: 17694
		private TextMeshProUGUI _copySeedLabel;

		// Token: 0x0400451F RID: 17695
		private bool _hasFreeWorldSeed;

		// Token: 0x04004520 RID: 17696
		private int _freeWorldSeed;

		// Token: 0x04004521 RID: 17697
		private bool _seedCopied;

		// Token: 0x04004522 RID: 17698
		private float _seedCopiedUntil;

		// Token: 0x04004523 RID: 17699
		private bool _seedCopyPointerPressed;

		// Token: 0x04004524 RID: 17700
		private int _lastSeedCopyFrame = -1;

		// Token: 0x04004525 RID: 17701
		private const int MaximumMiniMapSlotDescriptors = 256;

		// Token: 0x04004526 RID: 17702
		private const int MaximumMiniMapZoneDescriptors = 256;

		// Token: 0x04004527 RID: 17703
		private readonly Dictionary<long, ProceduralMiniMapController.MiniMapSlotDescriptor> _miniMapSlotDescriptors = new Dictionary<long, ProceduralMiniMapController.MiniMapSlotDescriptor>(32);

		// Token: 0x04004528 RID: 17704
		private readonly Dictionary<int, ProceduralMiniMapController.MiniMapZoneDescriptor> _miniMapZoneDescriptors = new Dictionary<int, ProceduralMiniMapController.MiniMapZoneDescriptor>(32);

		// Token: 0x04004529 RID: 17705
		private ProceduralMiniMapStyleAtlas _tileDescriptorAtlas;

		// Token: 0x0400452A RID: 17706
		private ProceduralMiniMapStyleAtlas _tileDescriptorSnapshotAtlas;

		// Token: 0x0400452B RID: 17707
		private Texture2D _tileDescriptorTexture;

		// Token: 0x0400452C RID: 17708
		private List<ProceduralMiniMapStyleAtlas.Entry> _tileDescriptorEntries;

		// Token: 0x0400452D RID: 17709
		private int _tileDescriptorEntryCount;

		// Token: 0x0400452E RID: 17710
		private int _tileDescriptorAtlasRevision;

		// Token: 0x020009A7 RID: 2471
		private sealed class LegendRowView
		{
			// Token: 0x0400452F RID: 17711
			public MiniMapPoiKind kind;

			// Token: 0x04004530 RID: 17712
			public string discoveredLabel;

			// Token: 0x04004531 RID: 17713
			public Image background;

			// Token: 0x04004532 RID: 17714
			public ProceduralMiniMapPoiIconGraphic icon;

			// Token: 0x04004533 RID: 17715
			public TextMeshProUGUI question;

			// Token: 0x04004534 RID: 17716
			public TextMeshProUGUI label;
		}

		// Token: 0x020009A8 RID: 2472
		private readonly struct MiniMapSlotDescriptor
		{
			// Token: 0x0600459C RID: 17820 RVA: 0x001A4AE6 File Offset: 0x001A2CE6
			public MiniMapSlotDescriptor(Vector3 position, float yaw)
			{
				this.localPosition = position;
				this.localYaw = yaw;
			}

			// Token: 0x04004535 RID: 17717
			public readonly Vector3 localPosition;

			// Token: 0x04004536 RID: 17718
			public readonly float localYaw;
		}

		// Token: 0x020009A9 RID: 2473
		private readonly struct MiniMapZoneDescriptor
		{
			// Token: 0x0600459D RID: 17821 RVA: 0x001A4AF6 File Offset: 0x001A2CF6
			public MiniMapZoneDescriptor(string zoneName, MiniMapZoneKind zoneKind, MiniMapPoiKind pointOfInterest, MiniMapTileReference tileReference)
			{
				this.name = zoneName;
				this.kind = zoneKind;
				this.poiKind = pointOfInterest;
				this.tile = tileReference;
			}

			// Token: 0x04004537 RID: 17719
			public readonly string name;

			// Token: 0x04004538 RID: 17720
			public readonly MiniMapZoneKind kind;

			// Token: 0x04004539 RID: 17721
			public readonly MiniMapPoiKind poiKind;

			// Token: 0x0400453A RID: 17722
			public readonly MiniMapTileReference tile;
		}
	}
}
