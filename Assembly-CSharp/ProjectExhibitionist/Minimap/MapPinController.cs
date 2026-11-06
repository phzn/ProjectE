using System;
using CompassNavigatorPro;
using Features.Localization;
using Features.RenderingPrecision;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009A0 RID: 2464
	[DisallowMultipleComponent]
	public sealed class MapPinController : MonoBehaviour
	{
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x060044B7 RID: 17591 RVA: 0x0019B490 File Offset: 0x00199690
		// (remove) Token: 0x060044B8 RID: 17592 RVA: 0x0019B4C4 File Offset: 0x001996C4
		public static event Action Changed;

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x060044B9 RID: 17593 RVA: 0x0019B4F7 File Offset: 0x001996F7
		public static bool HasPin
		{
			get
			{
				return MapPinController._instance != null && MapPinController._instance._hasPin;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x060044BA RID: 17594 RVA: 0x0019B512 File Offset: 0x00199712
		public static Vector3 Position
		{
			get
			{
				if (!(MapPinController._instance != null))
				{
					return Vector3.zero;
				}
				return MapPinController._instance._position;
			}
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0019B531 File Offset: 0x00199731
		public static void Place(Vector3 worldPosition)
		{
			MapPinController.Ensure().PlaceInternal(worldPosition);
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0019B53E File Offset: 0x0019973E
		public static void Clear()
		{
			if (MapPinController._instance != null)
			{
				MapPinController._instance.ClearInternal(true);
			}
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0019B558 File Offset: 0x00199758
		public static void ClearForTransition()
		{
			if (MapPinController._instance != null)
			{
				MapPinController._instance.ClearInternal(false);
			}
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0019B572 File Offset: 0x00199772
		private static MapPinController Ensure()
		{
			if (MapPinController._instance != null)
			{
				return MapPinController._instance;
			}
			GameObject gameObject = new GameObject("[Map Pin]");
			Object.DontDestroyOnLoad(gameObject);
			MapPinController._instance = gameObject.AddComponent<MapPinController>();
			return MapPinController._instance;
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x0019B5A6 File Offset: 0x001997A6
		private void Awake()
		{
			if (MapPinController._instance != null && MapPinController._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			MapPinController._instance = this;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0019B5D4 File Offset: 0x001997D4
		private void OnDestroy()
		{
			if (this._beamMaterial != null)
			{
				Object.Destroy(this._beamMaterial);
			}
			if (MapPinController._instance == this)
			{
				MapPinController._instance = null;
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x0019B602 File Offset: 0x00199802
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x0019B626 File Offset: 0x00199826
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x0019B64A File Offset: 0x0019984A
		private void HandleActiveSceneChanged(Scene previous, Scene next)
		{
			if (previous.handle != next.handle)
			{
				MapPinController.ClearForTransition();
			}
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0019B661 File Offset: 0x00199861
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (!this._hasPin)
			{
				return;
			}
			this._position += delta;
			Action changed = MapPinController.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0019B68D File Offset: 0x0019988D
		private void LateUpdate()
		{
			this.UpdateLabel();
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0019B698 File Offset: 0x00199898
		private void UpdateLabel()
		{
			if (this._label == null)
			{
				return;
			}
			Camera main = Camera.main;
			bool flag = this._hasPin && main != null;
			Vector3 vector = Vector3.zero;
			if (flag)
			{
				vector = main.WorldToScreenPoint(this._position + Vector3.up * 3f);
				flag = vector.z > 0f;
			}
			if (this._labelCanvas.enabled != flag)
			{
				this._labelCanvas.enabled = flag;
			}
			if (!flag)
			{
				return;
			}
			this._labelRect.position = new Vector3(vector.x, vector.y, 0f);
			if (Time.unscaledTime < this._nextLabelDistanceUpdate)
			{
				return;
			}
			this._nextLabelDistanceUpdate = Time.unscaledTime + 0.1f;
			Transform player = PlayerRegistry.Player;
			Vector3 vector2 = ((player != null) ? player.position : main.transform.position);
			int num = Mathf.RoundToInt(Vector3.Distance(new Vector3(vector2.x, 0f, vector2.z), new Vector3(this._position.x, 0f, this._position.z)));
			if (num != this._lastRoundedDistance)
			{
				this._lastRoundedDistance = num;
				this._distanceNumberText = num.ToString();
				LocalizedGameText labelBinding = this._labelBinding;
				if (labelBinding == null)
				{
					return;
				}
				labelBinding.Refresh();
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0019B7FC File Offset: 0x001999FC
		private void Update()
		{
			if (!this._hasPin || this._groundSnapped || Time.unscaledTime < this._nextGroundProbe)
			{
				return;
			}
			this._nextGroundProbe = Time.unscaledTime + 0.5f;
			float num;
			if (MapPinController.TryProbeGround(this._position, out num))
			{
				this._position.y = num;
				this._groundSnapped = true;
				this.ApplyPosition();
				Action changed = MapPinController.Changed;
				if (changed == null)
				{
					return;
				}
				changed();
			}
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0019B870 File Offset: 0x00199A70
		private void PlaceInternal(Vector3 worldPosition)
		{
			this.EnsureVisuals();
			this._position = worldPosition;
			float num;
			this._groundSnapped = MapPinController.TryProbeGround(worldPosition, out num);
			if (this._groundSnapped)
			{
				this._position.y = num;
			}
			this._nextGroundProbe = Time.unscaledTime + 0.5f;
			this._nextLabelDistanceUpdate = 0f;
			this._lastRoundedDistance = int.MinValue;
			this._hasPin = true;
			this._root.SetActive(true);
			this.ApplyPosition();
			MiniMapAudio.Play(MiniMapSound.PinPlace);
			Action changed = MapPinController.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0019B904 File Offset: 0x00199B04
		private void ClearInternal(bool playSound = true)
		{
			if (!this._hasPin)
			{
				return;
			}
			this._hasPin = false;
			this._position = Vector3.zero;
			this._groundSnapped = false;
			if (this._root != null)
			{
				this._root.SetActive(false);
			}
			if (this._labelCanvas != null)
			{
				this._labelCanvas.enabled = false;
			}
			if (playSound)
			{
				MiniMapAudio.Play(MiniMapSound.PinRemove);
			}
			Action changed = MapPinController.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x0019B980 File Offset: 0x00199B80
		private void ApplyPosition()
		{
			if (this._root == null)
			{
				return;
			}
			this._root.transform.position = this._position;
			this._beam.localPosition = new Vector3(0f, 30f, 0f);
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x0019B9D4 File Offset: 0x00199BD4
		private void EnsureVisuals()
		{
			if (this._root != null)
			{
				return;
			}
			this._root = new GameObject("Map Pin Beam");
			this._root.transform.SetParent(base.transform, false);
			Shader shader = Shader.Find("ProjectExhibitionist/MapPinBeam");
			if (shader == null)
			{
				Debug.LogWarning("[MapPin] Beam shader missing, falling back to URP Unlit.");
				shader = Shader.Find("Universal Render Pipeline/Unlit");
			}
			this._beamMaterial = new Material(shader)
			{
				name = "Map Pin Beam",
				hideFlags = HideFlags.HideAndDontSave
			};
			if (this._beamMaterial.HasProperty("_Color"))
			{
				this._beamMaterial.SetColor("_Color", MapPinController.BeamColor);
			}
			if (this._beamMaterial.HasProperty("_BaseColor"))
			{
				this._beamMaterial.SetColor("_BaseColor", MapPinController.BeamColor);
			}
			this._beamMaterial.renderQueue = 3050;
			this._beam = this.CreateCylinder("Beam", new Vector3(0.7f, 30f, 0.7f));
			this.EnsurePoi();
			this.EnsureLabel();
			this._root.SetActive(false);
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x0019BAFC File Offset: 0x00199CFC
		private void EnsureLabel()
		{
			if (this._label != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Map Pin Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._labelCanvas = gameObject.GetComponent<Canvas>();
			this._labelCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._labelCanvas.sortingOrder = 240;
			GameplayHudVisibility.RegisterCanvas(this._labelCanvas);
			this._labelCanvas.enabled = false;
			GameObject gameObject2 = new GameObject("Distance", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._labelRect = gameObject2.GetComponent<RectTransform>();
			this._labelRect.sizeDelta = new Vector2(90f, 18f);
			this._labelRect.pivot = new Vector2(0.5f, 0f);
			this._label = gameObject2.GetComponent<TextMeshProUGUI>();
			this._label.fontSize = 11f;
			this._label.fontStyle = FontStyles.Bold;
			this._label.alignment = TextAlignmentOptions.Center;
			this._label.color = new Color(1f, 0.72f, 0.85f, 0.9f);
			this._label.outlineWidth = 0.2f;
			this._label.outlineColor = new Color32(20, 10, 18, 200);
			this._label.raycastTarget = false;
			this._labelBinding = LocalizedGameText.BindComputed(this._label, delegate
			{
				if (!string.IsNullOrEmpty(this._distanceNumberText))
				{
					return GameLocalization.FormatSource("world_ui", "{0} m", new object[] { this._distanceNumberText });
				}
				return string.Empty;
			});
			LocalizedGameText labelBinding = this._labelBinding;
			if (labelBinding == null)
			{
				return;
			}
			labelBinding.FitTranslationToRectangle(false);
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0019BCE0 File Offset: 0x00199EE0
		private Transform CreateCylinder(string name, Vector3 scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			gameObject.name = name;
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			gameObject.transform.SetParent(this._root.transform, false);
			gameObject.transform.localScale = scale;
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			component2.sharedMaterial = this._beamMaterial;
			component2.shadowCastingMode = ShadowCastingMode.Off;
			component2.receiveShadows = false;
			component2.lightProbeUsage = LightProbeUsage.Off;
			component2.reflectionProbeUsage = ReflectionProbeUsage.Off;
			return gameObject.transform;
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x0019BD68 File Offset: 0x00199F68
		private void EnsurePoi()
		{
			if (this._poi != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Map Pin POI");
			gameObject.transform.SetParent(this._root.transform, false);
			gameObject.transform.localPosition = new Vector3(0f, 2f, 0f);
			this._poi = gameObject.AddComponent<CompassProPOI>();
			this._poi.title = "PIN";
			this._poi.titleVisibility = TitleVisibility.Always;
			this._poi.visibility = POIVisibility.AlwaysVisible;
			this._poi.ignoreAreaOfInterest = true;
			this._poi.clampPosition = true;
			this._poi.canBeVisited = false;
			this._poi.iconShowDistance = true;
			this._poi.iconScaleIsFixed = true;
			this._poi.iconScale = 1.15f;
			this._poi.showOnScreenIndicator = false;
			this._poi.onScreenIndicatorShowDistance = false;
			this._poi.onScreenIndicatorShowTitle = false;
			this._poi.showOffScreenIndicator = false;
			this._poi.visibleDistanceOverride = 100000f;
			this._poi.tintColor = Color.white;
			this._poi.iconNonVisited = MapPinController.GetIconSprite();
			this._poi.iconVisited = this._poi.iconNonVisited;
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x0019BEBC File Offset: 0x0019A0BC
		private static bool TryProbeGround(Vector3 position, out float groundY)
		{
			int num = -1;
			string[] array = new string[]
			{
				"UI", "Player", "PlayerHead", "PlayerClothing", "Triggers", "Enemy", "CompanionAI", "NPCDetectionHidden", "CharacterPreview", "PostProcess",
				"Ignore Raycast"
			};
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = LayerMask.NameToLayer(array[i]);
				if (num2 >= 0)
				{
					num &= ~(1 << num2);
				}
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(new Vector3(position.x, position.y + 300f, position.z), Vector3.down, out raycastHit, 600f, num, QueryTriggerInteraction.Ignore))
			{
				groundY = raycastHit.point.y;
				return true;
			}
			groundY = position.y;
			return false;
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0019BFA0 File Offset: 0x0019A1A0
		private static Sprite GetIconSprite()
		{
			if (MapPinController._iconSprite != null)
			{
				return MapPinController._iconSprite;
			}
			Texture2D texture2D = new Texture2D(64, 64, TextureFormat.RGBA32, false, false)
			{
				name = "Map Pin Icon",
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.HideAndDontSave
			};
			Color32[] array = new Color32[4096];
			Vector2 vector = Vector2.one * 63f * 0.5f;
			float num = 29.44f;
			float num2 = 12.8f;
			Color32 color = MapPinController.ThemePink;
			Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					float num3 = Vector2.Distance(new Vector2((float)j, (float)i), vector);
					float num4 = Mathf.Clamp01(num - num3 + 0.5f);
					Color32 color3 = ((num3 < num2) ? color2 : color);
					color3.a = (byte)Mathf.RoundToInt(num4 * 255f);
					array[i * 64 + j] = color3;
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			MapPinController._iconSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 64f), Vector2.one * 0.5f, 100f);
			MapPinController._iconSprite.name = "Map Pin Icon";
			MapPinController._iconSprite.hideFlags = HideFlags.HideAndDontSave;
			return MapPinController._iconSprite;
		}

		// Token: 0x04004459 RID: 17497
		private const string BeamShaderName = "ProjectExhibitionist/MapPinBeam";

		// Token: 0x0400445A RID: 17498
		private const float BeamHeight = 60f;

		// Token: 0x0400445B RID: 17499
		private const float BeamRadius = 0.7f;

		// Token: 0x0400445C RID: 17500
		private const float GroundProbeHeight = 300f;

		// Token: 0x0400445D RID: 17501
		private const float LabelHeight = 3f;

		// Token: 0x0400445E RID: 17502
		public static readonly Color ThemePink = new Color(1f, 0.36f, 0.62f, 1f);

		// Token: 0x0400445F RID: 17503
		private static readonly Color BeamColor = new Color(1f, 0.36f, 0.62f, 0.55f);

		// Token: 0x04004460 RID: 17504
		private static MapPinController _instance;

		// Token: 0x04004461 RID: 17505
		private static Sprite _iconSprite;

		// Token: 0x04004463 RID: 17507
		private bool _hasPin;

		// Token: 0x04004464 RID: 17508
		private Vector3 _position;

		// Token: 0x04004465 RID: 17509
		private bool _groundSnapped;

		// Token: 0x04004466 RID: 17510
		private float _nextGroundProbe;

		// Token: 0x04004467 RID: 17511
		private float _nextLabelDistanceUpdate;

		// Token: 0x04004468 RID: 17512
		private int _lastRoundedDistance = int.MinValue;

		// Token: 0x04004469 RID: 17513
		private GameObject _root;

		// Token: 0x0400446A RID: 17514
		private Transform _beam;

		// Token: 0x0400446B RID: 17515
		private Material _beamMaterial;

		// Token: 0x0400446C RID: 17516
		private CompassProPOI _poi;

		// Token: 0x0400446D RID: 17517
		private Canvas _labelCanvas;

		// Token: 0x0400446E RID: 17518
		private TextMeshProUGUI _label;

		// Token: 0x0400446F RID: 17519
		private RectTransform _labelRect;

		// Token: 0x04004470 RID: 17520
		private LocalizedGameText _labelBinding;

		// Token: 0x04004471 RID: 17521
		private string _distanceNumberText = string.Empty;
	}
}
