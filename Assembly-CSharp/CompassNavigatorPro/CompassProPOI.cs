using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CompassNavigatorPro
{
	// Token: 0x0200029D RID: 669
	[HelpURL("https://kronnect.com/guides-category/compass-navigator-pro-2/")]
	[AddComponentMenu("Compass Navigator Pro/Compass POI")]
	[ExecuteAlways]
	[DefaultExecutionOrder(101)]
	public class CompassProPOI : MonoBehaviour
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0007C0B7 File Offset: 0x0007A2B7
		public GameObject compassIconGameObject
		{
			get
			{
				if (this.compassIconRT != null)
				{
					return this.compassIconRT.gameObject;
				}
				return null;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x0007C0D4 File Offset: 0x0007A2D4
		public GameObject miniMapIconGameObject
		{
			get
			{
				if (this.miniMapIconRT != null)
				{
					return this.miniMapIconRT.gameObject;
				}
				return null;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x0007C0F1 File Offset: 0x0007A2F1
		public GameObject indicatorGameObject
		{
			get
			{
				if (this.indicatorRT != null)
				{
					return this.indicatorRT.gameObject;
				}
				return null;
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0007C110 File Offset: 0x0007A310
		private void OnValidate()
		{
			this.radius = MathF.Max(0f, this.radius);
			this.iconScale = Mathf.Max(0f, this.iconScale);
			this.miniMapCircleRadius = Mathf.Max(0f, this.miniMapCircleRadius);
			this.miniMapCircleAnimationRepetitions = Mathf.Max(1, this.miniMapCircleAnimationRepetitions);
			this.onScreenIndicatorNearFadeMin = Mathf.Max(0f, this.onScreenIndicatorNearFadeMin);
			this.onScreenIndicatorNearFadeDistance = Mathf.Max(0f, this.onScreenIndicatorNearFadeDistance);
			this.onScreenIndicatorNearFadeMin = Mathf.Min(this.onScreenIndicatorNearFadeMin, this.onScreenIndicatorNearFadeDistance);
			this.onScreenIndicatorFarDistance = Mathf.Max(0f, this.onScreenIndicatorFarDistance);
			this.onScreenIndicatorFarFadeDistance = Mathf.Max(0f, this.onScreenIndicatorFarFadeDistance);
			this.onScreenIndicatorFarFadeDistance = Mathf.Min(this.onScreenIndicatorFarFadeDistance, this.onScreenIndicatorFarDistance);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0007C1F8 File Offset: 0x0007A3F8
		private void OnEnable()
		{
			if (this.iconNonVisited == null)
			{
				this.iconNonVisited = Resources.Load<Sprite>("CNPro/Sprites/compassIcon");
			}
			if (this.id == 0)
			{
				this.GenerateNewId();
			}
			else if (!this.dontDestroyOnLoad)
			{
				CompassPro instance = CompassPro.instance;
				if (instance != null)
				{
					int count = instance.pois.Count;
					for (int i = 0; i < count; i++)
					{
						CompassProPOI compassProPOI = instance.pois[i];
						if (compassProPOI != null && compassProPOI != this && compassProPOI.id == this.id)
						{
							this.GenerateNewId();
							i = -1;
						}
					}
				}
			}
			this.RegisterPOI();
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0007C29D File Offset: 0x0007A49D
		private void OnDisable()
		{
			this.UnRegisterPOI();
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0007C2A5 File Offset: 0x0007A4A5
		private void OnDestroy()
		{
			this.Release();
			this.UnRegisterPOI();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0007C2B4 File Offset: 0x0007A4B4
		public void GenerateNewId()
		{
			this.id = Guid.NewGuid().GetHashCode();
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0007C2DC File Offset: 0x0007A4DC
		public void Release()
		{
			if (this.compassIconRT != null)
			{
				Misc.DestroySafe(this.compassIconRT.gameObject);
			}
			if (this.miniMapIconRT != null)
			{
				Misc.DestroySafe(this.miniMapIconRT.gameObject);
			}
			if (this.indicatorRT != null)
			{
				Misc.DestroySafe(this.indicatorRT.gameObject);
			}
			if (this.miniMapCircleMaterial != null)
			{
				Misc.DestroySafe(this.miniMapCircleMaterial);
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0007C35C File Offset: 0x0007A55C
		public void RegisterPOI()
		{
			CompassPro instance = CompassPro.instance;
			if (instance == null)
			{
				return;
			}
			if (this.dontDestroyOnLoad && Application.isPlaying)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
			instance.POIRegister(this);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0007C39C File Offset: 0x0007A59C
		public void UnRegisterPOI()
		{
			CompassPro instance = CompassPro.instance;
			if (instance != null)
			{
				instance.POIUnregister(this);
			}
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0007C3BF File Offset: 0x0007A5BF
		public void StartHeartbeat()
		{
			if (this.isVisited)
			{
				return;
			}
			this.heartbeatPlayer = base.StartCoroutine(this.HeartBeatPlayer());
			this.heartbeatIsActive = true;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0007C3E3 File Offset: 0x0007A5E3
		public void StopHeartbeat()
		{
			if (this.heartbeatPlayer != null)
			{
				base.StopCoroutine(this.heartbeatPlayer);
			}
			this.heartbeatIsActive = false;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0007C400 File Offset: 0x0007A600
		public void StartCircleAnimation()
		{
			if (!this.miniMapCircleAnimationWhenAppears)
			{
				this.miniMapCircleAnimationWhenAppears = true;
			}
			this.circleVisibleTime = 0f;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0007C41C File Offset: 0x0007A61C
		private IEnumerator HeartBeatPlayer()
		{
			AudioClip heartbeatSound = ((this.heartbeatAudioClip != null) ? this.heartbeatAudioClip : CompassPro.instance.heartbeatDefaultAudioClip);
			if (heartbeatSound == null)
			{
				Debug.LogWarning("Compass POI: heartbeat sound not set.");
				yield break;
			}
			this.heartbeatDistance = Mathf.Max(1f, this.heartbeatDistance);
			float minDistance = CompassPro.instance.visitedDistance;
			for (;;)
			{
				float num = this.distanceToFollow;
				if (this.distanceToFollow > this.heartbeatDistance || this.isVisited)
				{
					break;
				}
				if (num < minDistance)
				{
					num = minDistance;
				}
				Vector3 position = CompassPro.instance.cameraMain.transform.position;
				AudioSource.PlayClipAtPoint(heartbeatSound, position);
				if (this.OnHeartbeat != null)
				{
					this.OnHeartbeat();
				}
				float num2 = (num - minDistance) / this.heartbeatDistance;
				float num3 = this.heartbeatInterval.Evaluate(num2);
				yield return new WaitForSeconds(num3);
			}
			this.heartbeatIsActive = false;
			yield break;
			yield break;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0007C42C File Offset: 0x0007A62C
		public Rect GetCompassIconScreenRect()
		{
			if (this.isVisible && this.compassIconRT != null && this.compass != null)
			{
				Vector3 position = this.compassIconRT.transform.position;
				Vector3 vector = this.compassIconRT.sizeDelta;
				return new Rect(position.x - vector.x * 0.5f, (float)Screen.height - position.y - vector.y * 0.5f, vector.x, vector.y);
			}
			return new Rect(0f, 0f, 0f, 0f);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0007C4D8 File Offset: 0x0007A6D8
		public Rect GetMiniMapIconScreenRect()
		{
			if (this.miniMapIsVisible && this.miniMapIconRT != null && this.compass != null)
			{
				return this.miniMapIconRT.GetScreenRect();
			}
			return new Rect(0f, 0f, 0f, 0f);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0007C530 File Offset: 0x0007A730
		public bool ToggleCompassBarIconVisibility(bool visible)
		{
			this.isVisible = visible;
			if (this.compassIconRT == null)
			{
				return false;
			}
			GameObject gameObject = this.compassIconRT.gameObject;
			if (gameObject.activeSelf != visible)
			{
				gameObject.SetActive(visible);
				return true;
			}
			return false;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0007C573 File Offset: 0x0007A773
		public bool ToggleMiniMapIconVisibility(bool visible)
		{
			this.miniMapIsVisible = visible;
			if (this.miniMapIconImage != null && this.miniMapIconImage.enabled != visible)
			{
				this.miniMapIconImage.enabled = visible;
				return true;
			}
			return false;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0007C5A7 File Offset: 0x0007A7A7
		public bool ToggleIndicatorVisibility(bool visible)
		{
			this.indicatorIsVisible = visible;
			if (this.indicatorImage != null && this.indicatorImage.isActiveAndEnabled != visible)
			{
				this.indicatorImage.gameObject.SetActive(visible);
				return true;
			}
			return false;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0007C5E0 File Offset: 0x0007A7E0
		public bool ToggleMiniMapCircleVisibility(bool visible)
		{
			this.circleIsVisible = visible;
			if (this.miniMapCircleImage != null && this.miniMapCircleImage.isActiveAndEnabled != visible)
			{
				this.miniMapCircleRT.gameObject.SetActive(visible);
				return true;
			}
			return false;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0007C619 File Offset: 0x0007A819
		public void SetVisited(bool visited)
		{
			if (CompassPro.instance != null)
			{
				CompassPro.instance.POISetVisited(this, visited);
			}
		}

		// Token: 0x0400142F RID: 5167
		[Tooltip("Unique ID to be used when DontDestroyOnLoad option is set to true.")]
		public int id;

		// Token: 0x04001430 RID: 5168
		[Tooltip("Higher priority icons are rendered on top of others. This option only has effect for POIs loaded at the start. POIs loaded in runtime are added to the hierarchy and will render on top of the previous ones.")]
		public int priority;

		// Token: 0x04001431 RID: 5169
		[Tooltip("POI visibility in compass bar.")]
		public POIVisibility visibility;

		// Token: 0x04001432 RID: 5170
		[Tooltip("If enabled, the POI will be always managed ignoring the current area of interest (if area of interest option is enabled in the compass inspector).")]
		public bool ignoreAreaOfInterest;

		// Token: 0x04001433 RID: 5171
		[Tooltip("If enabled, the icon will stop at the edges of the bar even if it's behind the player.")]
		public bool clampPosition;

		// Token: 0x04001434 RID: 5172
		[Tooltip("A value of 0 uses the global visible distance property from the Compass Bar settings. A value greater than 0 will override the global value for this POI. Useful when you need this POI to use a different visible distance.")]
		public float visibleDistanceOverride;

		// Token: 0x04001435 RID: 5173
		[Tooltip("A value of 0 uses the global visible min distance property from the Compass Bar settings. A value greater than 0 will override the global value for this POI.")]
		public float visibleMinDistanceOverride;

		// Token: 0x04001436 RID: 5174
		[Tooltip("A value of 0 uses the global min distance text from the Compass Bar settings. Distance text won't be shown for objects within this distance.")]
		public float titleMinPOIDistanceOverride;

		// Token: 0x04001437 RID: 5175
		[Tooltip("Title to be shown when this POI is in the center of the compass bar and it's a known location (isVisited = true)")]
		public string title;

		// Token: 0x04001438 RID: 5176
		[Tooltip("Rule for title visibility.")]
		public TitleVisibility titleVisibility;

		// Token: 0x04001439 RID: 5177
		[Tooltip("Specifies if this POI can be marked as visited when reached.")]
		public bool canBeVisited = true;

		// Token: 0x0400143A RID: 5178
		[Tooltip("A value of 0 uses the global visited distance property from the Compass Bar settings. A value greater than 0 will override the global value for this POI. Useful when you need this POI to use a different visited distance.")]
		public float visitedDistanceOverride;

		// Token: 0x0400143B RID: 5179
		[Tooltip("Specifies if POI must be removed from the UI when visited.")]
		public bool hideWhenVisited;

		// Token: 0x0400143C RID: 5180
		[Tooltip("Specifies if this POI has been already visited.")]
		public bool isVisited;

		// Token: 0x0400143D RID: 5181
		[Tooltip("Text to show when discovered. Leave this to null if you don't want to show any text.")]
		public string visitedText;

		// Token: 0x0400143E RID: 5182
		public bool playAudioClipWhenVisited = true;

		// Token: 0x0400143F RID: 5183
		[Tooltip("Sound to play when POI is visited the first time. If nothing set, the default audio-clip assigned in Compass Navigator Pro inspector will be used.")]
		public AudioClip visitedAudioClipOverride;

		// Token: 0x04001440 RID: 5184
		[Tooltip("Radius of interest of this POI. Useful for area POIs, like rooms, cities or generic areas of search. The radius is used by the circle feature in the mini-map or to determine if player reaches an area.")]
		public float radius;

		// Token: 0x04001441 RID: 5185
		[Tooltip("User defined icon scale multiplier.")]
		public float iconScale = 1f;

		// Token: 0x04001442 RID: 5186
		[Tooltip("When true, the icon scale won't be altered based on distance.")]
		public bool iconScaleIsFixed;

		// Token: 0x04001443 RID: 5187
		[Tooltip("Show distance to the POI in the compass bar under the icon")]
		public bool iconShowDistance = true;

		// Token: 0x04001444 RID: 5188
		[Tooltip("The icon for the POI if has not been discovered/visited.")]
		public Sprite iconNonVisited;

		// Token: 0x04001445 RID: 5189
		[Tooltip("The icon for the POI if has been visited.")]
		public Sprite iconVisited;

		// Token: 0x04001446 RID: 5190
		[Tooltip("Tinting color")]
		public Color tintColor = Color.white;

		// Token: 0x04001447 RID: 5191
		[Tooltip("If the icon will be shown in the scene during playmode. If enabled, the indicator will fade in smoothly as the player approaches it.")]
		public bool showOnScreenIndicator = true;

		// Token: 0x04001448 RID: 5192
		[Tooltip("Scale for this POI indicator")]
		public float onScreenIndicatorScale = 1f;

		// Token: 0x04001449 RID: 5193
		[Tooltip("Show distance to camera for this POI indicator")]
		public bool onScreenIndicatorShowDistance = true;

		// Token: 0x0400144A RID: 5194
		[Tooltip("Show title for this POI indicator")]
		public bool onScreenIndicatorShowTitle = true;

		// Token: 0x0400144B RID: 5195
		[Tooltip("Optional offset added to the POI position to compute the distance or the screen coordinate of the visual indicators.")]
		public Vector3 positionOffset;

		// Token: 0x0400144C RID: 5196
		[Tooltip("If the icon will be shown around the edges of screen in the scene during playmode when it's not visible in the screen.")]
		public bool showOffScreenIndicator = true;

		// Token: 0x0400144D RID: 5197
		[Tooltip("Show distance to camera for this POI when it's off-screen")]
		public bool offScreenIndicatorShowDistance = true;

		// Token: 0x0400144E RID: 5198
		[Tooltip("Custom separation between offscreen indicator and screen edges. 0 = use global setting.")]
		public float offScreenIndicatorMarginOverride;

		// Token: 0x0400144F RID: 5199
		[Tooltip("Distance at which the on-screen indicator will start to fade when it approaches camera")]
		public float onScreenIndicatorNearFadeDistance;

		// Token: 0x04001450 RID: 5200
		[Tooltip("Minimum distance at which the on-screen indicator disappear")]
		public float onScreenIndicatorNearFadeMin;

		// Token: 0x04001451 RID: 5201
		[Tooltip("Distance at which the indicator will not be visible")]
		public float onScreenIndicatorFarDistance;

		// Token: 0x04001452 RID: 5202
		[Tooltip("Distance at which the on-screen indicator will start to fade out before reaching max visible distance")]
		public float onScreenIndicatorFarFadeDistance;

		// Token: 0x04001453 RID: 5203
		[Tooltip("Sound to play when beacon is shown.")]
		public AudioClip beaconAudioClip;

		// Token: 0x04001454 RID: 5204
		[Tooltip("Sound to play when scan effect hits this POI.")]
		public AudioClip scanHitAudioClip;

		// Token: 0x04001455 RID: 5205
		[Tooltip("Preserves the state of this POI between scene changes. Note that this POI only will be visible in the scene where it was first created.")]
		public bool dontDestroyOnLoad;

		// Token: 0x04001456 RID: 5206
		[Tooltip("Enables heartbeat effect. Plays a sound with variable speed when approaching this POI.")]
		public bool heartbeatEnabled;

		// Token: 0x04001457 RID: 5207
		[Tooltip("Sound to play when heartbeat effect is enabled.")]
		public AudioClip heartbeatAudioClip;

		// Token: 0x04001458 RID: 5208
		[Tooltip("Distance to start playing heartbeat effect is enabled.")]
		public float heartbeatDistance = 20f;

		// Token: 0x04001459 RID: 5209
		[Tooltip("Interval of heartbeat rate based on distance.")]
		public AnimationCurve heartbeatInterval = AnimationCurve.Linear(0f, 0.25f, 1f, 3f);

		// Token: 0x0400145A RID: 5210
		public POIMiniMapType miniMapType;

		// Token: 0x0400145B RID: 5211
		[Tooltip("POI visibility on the mini-map.")]
		public POIVisibility miniMapVisibility;

		// Token: 0x0400145C RID: 5212
		[Tooltip("Optionally assign a custom prefab for this POI icon on the mini-map. If null, the system will use the icon prefab set in the Compass Navigator Pro component.")]
		public GameObject miniMapIconPrefabOverride;

		// Token: 0x0400145D RID: 5213
		[Tooltip("A value of 0 uses the global visible distance property from the mini-map settings. A value greater than 0 will override the global value for this POI. Useful when you need this POI to use a different visible distance.")]
		public float miniMapVisibleDistanceOverride;

		// Token: 0x0400145E RID: 5214
		[Tooltip("If enabled, the icon will stop at the edges of the mini-map even if it's behind the player.")]
		public bool miniMapClampPosition;

		// Token: 0x0400145F RID: 5215
		[Tooltip("Scale multiplier applied to POI when clamped to border")]
		public float miniMapClampedScaleMultiplier = 1.1f;

		// Token: 0x04001460 RID: 5216
		[Tooltip("If enabled, the minimap icon will be rotated according to the POI rotation.")]
		public bool miniMapShowRotation;

		// Token: 0x04001461 RID: 5217
		[Tooltip("Custom angle rotation adjustment.")]
		public float miniMapRotationAngleOffset;

		// Token: 0x04001462 RID: 5218
		[Tooltip("Icon scale on the mini-map.")]
		public float miniMapIconScale = 1f;

		// Token: 0x04001463 RID: 5219
		[Tooltip("Add a circle around the POI in the mini-map illustrating the POI radius")]
		public bool miniMapShowCircle;

		// Token: 0x04001464 RID: 5220
		[Tooltip("Radius of the circle to be shown on the minimap. Set it to 0 to use the general radius value.")]
		public float miniMapCircleRadius;

		// Token: 0x04001465 RID: 5221
		public Color miniMapCircleColor = new Color(0f, 1f, 0f, 0.5f);

		// Token: 0x04001466 RID: 5222
		public Color miniMapCircleInnerColor = new Color(0f, 0f, 1f, 0f);

		// Token: 0x04001467 RID: 5223
		[Range(0f, 1f)]
		public float miniMapCircleStartRadius = 0.25f;

		// Token: 0x04001468 RID: 5224
		[Tooltip("Add a circle animation when the icon appears in the mini-map")]
		public bool miniMapCircleAnimationWhenAppears = true;

		// Token: 0x04001469 RID: 5225
		[Tooltip("Number of repetitions for the circle animation")]
		public int miniMapCircleAnimationRepetitions = 5;

		// Token: 0x0400146A RID: 5226
		public OnHeartbeatEvent OnHeartbeat;

		// Token: 0x0400146B RID: 5227
		public Scene scene;

		// Token: 0x0400146C RID: 5228
		[NonSerialized]
		public float distanceToFollow;

		// Token: 0x0400146D RID: 5229
		[NonSerialized]
		public Vector3 positionWS;

		// Token: 0x0400146E RID: 5230
		[NonSerialized]
		public Vector3 viewportPos;

		// Token: 0x0400146F RID: 5231
		[NonSerialized]
		public Vector3 lastIndicatorViewportPos;

		// Token: 0x04001470 RID: 5232
		[NonSerialized]
		public int viewportPosFrameCount;

		// Token: 0x04001471 RID: 5233
		[NonSerialized]
		public float visitedTime;

		// Token: 0x04001472 RID: 5234
		[NonSerialized]
		public bool isVisible;

		// Token: 0x04001473 RID: 5235
		public RectTransform compassIconRT;

		// Token: 0x04001474 RID: 5236
		public Image compassIconImage;

		// Token: 0x04001475 RID: 5237
		[NonSerialized]
		public bool curvedMaterialSet;

		// Token: 0x04001476 RID: 5238
		[NonSerialized]
		public float compassCurrentIconScale;

		// Token: 0x04001477 RID: 5239
		[NonSerialized]
		public bool miniMapIsVisible;

		// Token: 0x04001478 RID: 5240
		public RectTransform miniMapIconRT;

		// Token: 0x04001479 RID: 5241
		public RectTransform miniMapIconImageRT;

		// Token: 0x0400147A RID: 5242
		public Image miniMapIconImage;

		// Token: 0x0400147B RID: 5243
		public Image miniMapCircleImage;

		// Token: 0x0400147C RID: 5244
		public Material miniMapCircleMaterial;

		// Token: 0x0400147D RID: 5245
		public RectTransform miniMapCircleRT;

		// Token: 0x0400147E RID: 5246
		[NonSerialized]
		public bool circleIsVisible;

		// Token: 0x0400147F RID: 5247
		[NonSerialized]
		public float circleScale = 2f;

		// Token: 0x04001480 RID: 5248
		[NonSerialized]
		public float lastCircleRadius;

		// Token: 0x04001481 RID: 5249
		[NonSerialized]
		public float lastCircleHeight;

		// Token: 0x04001482 RID: 5250
		[NonSerialized]
		public float lastCircleZoomLevel;

		// Token: 0x04001483 RID: 5251
		[NonSerialized]
		public float circleVisibleTime;

		// Token: 0x04001484 RID: 5252
		[NonSerialized]
		public int insideCircle;

		// Token: 0x04001485 RID: 5253
		[NonSerialized]
		public int isOnScreen;

		// Token: 0x04001486 RID: 5254
		[NonSerialized]
		public bool indicatorIsVisible;

		// Token: 0x04001487 RID: 5255
		public RectTransform indicatorRT;

		// Token: 0x04001488 RID: 5256
		public Image indicatorImage;

		// Token: 0x04001489 RID: 5257
		public CanvasGroup indicatorCanvasGroup;

		// Token: 0x0400148A RID: 5258
		public RectTransform indicatorArrowRT;

		// Token: 0x0400148B RID: 5259
		public TextMeshProUGUI indicatorDistanceText;

		// Token: 0x0400148C RID: 5260
		public TextMeshProUGUI indicatorTitleText;

		// Token: 0x0400148D RID: 5261
		[NonSerialized]
		public float prevIndicatorDistance;

		// Token: 0x0400148E RID: 5262
		[NonSerialized]
		public float lastCompassIconDistance;

		// Token: 0x0400148F RID: 5263
		[NonSerialized]
		public string lastIndicatorDistanceText;

		// Token: 0x04001490 RID: 5264
		[NonSerialized]
		public string lastCompassIconDistanceText;

		// Token: 0x04001491 RID: 5265
		public TextMeshProUGUI compassIconDistanceText;

		// Token: 0x04001492 RID: 5266
		[NonReorderable]
		public RectTransform compassIconDistanceTextRT;

		// Token: 0x04001493 RID: 5267
		[NonSerialized]
		public CompassPro compass;

		// Token: 0x04001494 RID: 5268
		[NonSerialized]
		public float visibleTime;

		// Token: 0x04001495 RID: 5269
		[NonSerialized]
		public bool heartbeatIsActive;

		// Token: 0x04001496 RID: 5270
		private Coroutine heartbeatPlayer;

		// Token: 0x04001497 RID: 5271
		[HideInInspector]
		public float miniMapCurrentIconScale;
	}
}
