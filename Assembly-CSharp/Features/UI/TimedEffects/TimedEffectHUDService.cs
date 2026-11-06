using System;
using System.Collections.Generic;
using Features.Vehicles.Spawning;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.UI.TimedEffects
{
	// Token: 0x02000BC5 RID: 3013
	[DefaultExecutionOrder(120)]
	public class TimedEffectHUDService : MonoBehaviour
	{
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x0600567E RID: 22142 RVA: 0x00219EBD File Offset: 0x002180BD
		// (set) Token: 0x0600567F RID: 22143 RVA: 0x00219EC4 File Offset: 0x002180C4
		public static TimedEffectHUDService Instance { get; private set; }

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005680 RID: 22144 RVA: 0x00219ECC File Offset: 0x002180CC
		public int ActiveEffectCount
		{
			get
			{
				return this.activeEntries.Count;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005681 RID: 22145 RVA: 0x00219ED9 File Offset: 0x002180D9
		public RectTransform LayoutRect
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005682 RID: 22146 RVA: 0x00219EE6 File Offset: 0x002180E6
		public string DisplayedEffectId
		{
			get
			{
				if (!(this.displayedEntry != null))
				{
					return null;
				}
				return this.displayedEntry.EffectId;
			}
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x00219F04 File Offset: 0x00218104
		public bool TryGetVisibleGuiRect(out Rect bounds)
		{
			bounds = default(Rect);
			if (!base.isActiveAndEnabled || this.displayedEntry == null || !this.displayedEntry.IsPresented || this.layoutCanvas == null || !this.layoutCanvas.isActiveAndEnabled || !this.displayedEntry.gameObject.activeInHierarchy)
			{
				return false;
			}
			CanvasGroup canvasGroup;
			if (this.displayedEntry.TryGetComponent<CanvasGroup>(out canvasGroup) && canvasGroup.alpha <= 0.001f)
			{
				return false;
			}
			Camera camera = ((this.layoutCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.layoutCanvas.worldCamera);
			this.LayoutRect.GetWorldCorners(this.vehicleCorners);
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			for (int i = 0; i < this.vehicleCorners.Length; i++)
			{
				Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(camera, this.vehicleCorners[i]);
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			bounds = Rect.MinMaxRect(vector.x, (float)Screen.height - vector2.y, vector2.x, (float)Screen.height - vector.y);
			return true;
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x0021A048 File Offset: 0x00218248
		private void Awake()
		{
			if (TimedEffectHUDService.Instance != null && TimedEffectHUDService.Instance != this)
			{
				Debug.LogWarning("[TimedEffectHUDService] Duplicate instance destroyed.", this);
				Object.Destroy(base.gameObject);
				return;
			}
			TimedEffectHUDService.Instance = this;
			this.ConfigureScreenLayout();
			this.PrewarmPool();
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x0021A098 File Offset: 0x00218298
		private void LateUpdate()
		{
			this.UpdatePresentedEffect();
			this.ApplyScreenLayout();
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x0021A0A8 File Offset: 0x002182A8
		private void UpdatePresentedEffect()
		{
			TimedEffectHUDEntry timedEffectHUDEntry = null;
			int num = int.MaxValue;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < this.activeEntries.Count; i++)
			{
				TimedEffectHUDEntry timedEffectHUDEntry2 = this.activeEntries[i];
				if (!(timedEffectHUDEntry2 == null) && timedEffectHUDEntry2.CanPresent)
				{
					int num3 = (float.IsPositiveInfinity(timedEffectHUDEntry2.RemainingTime) ? (timedEffectHUDEntry2.IsPaused ? 3 : 2) : (timedEffectHUDEntry2.IsPaused ? 1 : 0));
					if (num3 < num || (num3 == num && timedEffectHUDEntry2.RemainingTime < num2))
					{
						timedEffectHUDEntry = timedEffectHUDEntry2;
						num = num3;
						num2 = timedEffectHUDEntry2.RemainingTime;
					}
				}
			}
			this.displayedEntry = timedEffectHUDEntry;
			for (int j = 0; j < this.activeEntries.Count; j++)
			{
				if (this.activeEntries[j] != null)
				{
					this.activeEntries[j].SetPresentationVisible(this.activeEntries[j] == timedEffectHUDEntry);
				}
			}
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x0021A1A6 File Offset: 0x002183A6
		private void OnDestroy()
		{
			if (TimedEffectHUDService.Instance == this)
			{
				TimedEffectHUDService.Instance = null;
			}
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x0021A1BC File Offset: 0x002183BC
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			for (int i = this.activeEntries.Count - 1; i >= 0; i--)
			{
				TimedEffectHUDEntry timedEffectHUDEntry = this.activeEntries[i];
				if (timedEffectHUDEntry == null || !timedEffectHUDEntry.Tick(deltaTime))
				{
					if (timedEffectHUDEntry != null)
					{
						timedEffectHUDEntry.ResetEntry();
						this.pool.Enqueue(timedEffectHUDEntry);
					}
					this.activeEntries.RemoveAt(i);
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[TimedEffectHUDService] Entry recycled (index {0})", i));
					}
				}
			}
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x0021A24C File Offset: 0x0021844C
		public void RegisterEffect(TimedEffectData data)
		{
			if (data == null || string.IsNullOrEmpty(data.EffectId))
			{
				Debug.LogWarning("[TimedEffectHUDService] RegisterEffect called with null or empty data.", this);
				return;
			}
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(data.EffectId);
			if (timedEffectHUDEntry != null)
			{
				timedEffectHUDEntry.Setup(data);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[TimedEffectHUDService] Refreshed effect '{0}' ({1:F1}s)", data.EffectId, data.TotalDuration));
				}
				return;
			}
			TimedEffectHUDEntry timedEffectHUDEntry2 = this.AcquireEntry();
			if (timedEffectHUDEntry2 == null)
			{
				Debug.LogError("[TimedEffectHUDService] Cannot show timed effect because the entry prefab is unavailable.", this);
				return;
			}
			timedEffectHUDEntry2.Setup(data);
			this.activeEntries.Add(timedEffectHUDEntry2);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[TimedEffectHUDService] Registered effect '{0}' ({1:F1}s)", data.EffectId, data.TotalDuration));
			}
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x0021A314 File Offset: 0x00218514
		public void UnregisterEffect(string effectId)
		{
			if (string.IsNullOrEmpty(effectId))
			{
				return;
			}
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			if (timedEffectHUDEntry != null)
			{
				timedEffectHUDEntry.BeginFadeOut();
				if (this.debugLogs)
				{
					Debug.Log("[TimedEffectHUDService] Unregistered effect '" + effectId + "'");
				}
			}
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x0021A360 File Offset: 0x00218560
		public void SetRemainingTime(string effectId, float seconds)
		{
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			if (timedEffectHUDEntry != null)
			{
				timedEffectHUDEntry.SetRemainingTime(seconds);
			}
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x0021A388 File Offset: 0x00218588
		public void SetNormalizedProgress(string effectId, float normalized01)
		{
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			if (timedEffectHUDEntry != null)
			{
				timedEffectHUDEntry.SetNormalizedProgress(normalized01);
			}
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0021A3B0 File Offset: 0x002185B0
		public void SetPaused(string effectId, bool paused)
		{
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			if (timedEffectHUDEntry != null)
			{
				timedEffectHUDEntry.SetPaused(paused);
			}
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0021A3D8 File Offset: 0x002185D8
		public bool IsEffectActive(string effectId)
		{
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			return timedEffectHUDEntry != null && timedEffectHUDEntry.IsActive;
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0021A400 File Offset: 0x00218600
		public float GetRemainingTime(string effectId)
		{
			TimedEffectHUDEntry timedEffectHUDEntry = this.FindEntry(effectId);
			if (!(timedEffectHUDEntry != null))
			{
				return -1f;
			}
			return timedEffectHUDEntry.RemainingTime;
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0021A42C File Offset: 0x0021862C
		private TimedEffectHUDEntry FindEntry(string effectId)
		{
			for (int i = 0; i < this.activeEntries.Count; i++)
			{
				if (this.activeEntries[i] != null && this.activeEntries[i].EffectId == effectId)
				{
					return this.activeEntries[i];
				}
			}
			return null;
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0021A48A File Offset: 0x0021868A
		private TimedEffectHUDEntry AcquireEntry()
		{
			if (this.pool.Count > 0)
			{
				TimedEffectHUDEntry timedEffectHUDEntry = this.pool.Dequeue();
				timedEffectHUDEntry.gameObject.SetActive(true);
				return timedEffectHUDEntry;
			}
			return this.CreateEntry();
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x0021A4B8 File Offset: 0x002186B8
		private TimedEffectHUDEntry CreateEntry()
		{
			if (this.entryPrefab == null)
			{
				Debug.LogError("[TimedEffectHUDService] entryPrefab is not assigned!", this);
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.entryPrefab.gameObject, (this.entryContainer != null) ? this.entryContainer : base.transform);
			TimedEffectHUDEntry component = gameObject.GetComponent<TimedEffectHUDEntry>();
			RectTransform rectTransform = gameObject.transform as RectTransform;
			if (this.enforceVehicleClusterLayout && rectTransform != null)
			{
				RectTransform rectTransform2 = rectTransform;
				RectTransform rectTransform3 = rectTransform;
				RectTransform rectTransform4 = rectTransform;
				Vector2 vector = new Vector2(0f, 1f);
				rectTransform4.pivot = vector;
				rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
				component.SetCompactHudSize(190f, 40f);
				rectTransform.anchoredPosition = Vector2.zero;
			}
			component.SetPresentationVisible(false);
			gameObject.SetActive(false);
			return component;
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x0021A588 File Offset: 0x00218788
		private void PrewarmPool()
		{
			for (int i = 0; i < this.initialPoolSize; i++)
			{
				TimedEffectHUDEntry timedEffectHUDEntry = this.CreateEntry();
				if (timedEffectHUDEntry != null)
				{
					timedEffectHUDEntry.ResetEntry();
					this.pool.Enqueue(timedEffectHUDEntry);
				}
			}
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0021A5C8 File Offset: 0x002187C8
		private void ConfigureScreenLayout()
		{
			if (!this.enforceVehicleClusterLayout)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null || rectTransform.parent == null)
			{
				return;
			}
			Canvas componentInParent = base.GetComponentInParent<Canvas>(true);
			if (componentInParent == null)
			{
				return;
			}
			this.layoutCanvas = componentInParent;
			Transform transform = componentInParent.transform;
			if (rectTransform.parent != transform)
			{
				rectTransform.SetParent(transform, false);
				rectTransform.SetAsLastSibling();
			}
			this.layoutParent = transform as RectTransform;
			Transform transform2 = ((this.entryContainer != null) ? this.entryContainer : base.transform);
			LayoutGroup layoutGroup;
			if (transform2.TryGetComponent<LayoutGroup>(out layoutGroup))
			{
				layoutGroup.enabled = false;
			}
			ContentSizeFitter contentSizeFitter;
			if (transform2.TryGetComponent<ContentSizeFitter>(out contentSizeFitter))
			{
				contentSizeFitter.enabled = false;
			}
			if (transform2 != base.transform)
			{
				RectTransform rectTransform2 = transform2 as RectTransform;
				if (rectTransform2 != null)
				{
					rectTransform2.anchorMin = Vector2.zero;
					rectTransform2.anchorMax = Vector2.one;
					rectTransform2.offsetMin = (rectTransform2.offsetMax = Vector2.zero);
				}
			}
			CanvasGroup canvasGroup = base.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
			}
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			RectTransform rectTransform5 = rectTransform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform5.pivot = vector;
			rectTransform3.anchorMin = (rectTransform4.anchorMax = vector);
			rectTransform.sizeDelta = new Vector2(190f, 40f);
			this.ApplyScreenLayout();
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x0021A758 File Offset: 0x00218958
		private void ApplyScreenLayout()
		{
			if (!this.enforceVehicleClusterLayout || this.layoutParent == null)
			{
				return;
			}
			Vector2 size = this.layoutParent.rect.size;
			Rect rect;
			if (!this.TryGetVehicleBounds(out rect))
			{
				float num = Mathf.Sqrt(Mathf.Max(0f, size.x * size.y) / 2073600f);
				Vector2 preferredTopLeftPosition = VehicleLifetimeHUD.GetPreferredTopLeftPosition();
				float num2 = VehicleLifetimeHUD.RenderedDiameter * num;
				rect = new Rect(preferredTopLeftPosition.x * num, -preferredTopLeftPosition.y * num, num2, num2);
			}
			this.LayoutRect.anchoredPosition = TimedEffectHUDService.CalculateClusterPosition(size, new Vector2((float)Screen.width, (float)Screen.height), Screen.safeArea, rect);
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0021A814 File Offset: 0x00218A14
		private static Vector2 CalculateClusterPosition(Vector2 parentSize, Vector2 screenSize, Rect safeArea, Rect vehicleBounds)
		{
			float num = 0f;
			float num2 = 0f;
			if (screenSize.x > 0f && screenSize.y > 0f && safeArea.width > 0f)
			{
				num = safeArea.xMin * parentSize.x / screenSize.x;
				num2 = (screenSize.y - safeArea.yMax) * parentSize.y / screenSize.y;
			}
			float num3 = Mathf.Max(216f + num, vehicleBounds.xMax + 12f);
			float num4 = Mathf.Max(num2, vehicleBounds.yMin);
			return new Vector2(num3, -num4);
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x0021A8B8 File Offset: 0x00218AB8
		private bool TryGetVehicleBounds(out Rect bounds)
		{
			bounds = default(Rect);
			VehicleLifetimeHUD instance = VehicleLifetimeHUD.Instance;
			if (instance == null || !instance.isActiveAndEnabled)
			{
				return false;
			}
			Canvas screenCanvas = instance.ScreenCanvas;
			if (!(screenCanvas == null) && screenCanvas.isActiveAndEnabled)
			{
				RectTransform rectTransform = instance.transform as RectTransform;
				if (rectTransform != null)
				{
					Camera camera = ((screenCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : screenCanvas.worldCamera);
					Camera camera2 = ((this.layoutCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.layoutCanvas.worldCamera);
					rectTransform.GetWorldCorners(this.vehicleCorners);
					Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
					Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
					for (int i = 0; i < this.vehicleCorners.Length; i++)
					{
						Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(camera, this.vehicleCorners[i]);
						Vector2 vector4;
						if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.layoutParent, vector3, camera2, out vector4))
						{
							return false;
						}
						vector = Vector2.Min(vector, vector4);
						vector2 = Vector2.Max(vector2, vector4);
					}
					bounds = new Rect(vector.x - this.layoutParent.rect.xMin, this.layoutParent.rect.yMax - vector2.y, vector2.x - vector.x, vector2.y - vector.y);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005704 RID: 22276
		[Header("References")]
		[Tooltip("Prefab with TimedEffectHUDEntry component (radial fill + icon)")]
		[SerializeField]
		private TimedEffectHUDEntry entryPrefab;

		// Token: 0x04005705 RID: 22277
		[Tooltip("Parent transform for spawned entries (e.g. VerticalLayoutGroup)")]
		[SerializeField]
		private Transform entryContainer;

		// Token: 0x04005706 RID: 22278
		[Header("Pool")]
		[Tooltip("Initial pool size. Entries beyond this are instantiated on demand.")]
		[SerializeField]
		private int initialPoolSize = 4;

		// Token: 0x04005707 RID: 22279
		[Header("Screen Layout")]
		[Tooltip("Show the next expiring effect in one compact slot beside the vehicle lifetime clock.")]
		[FormerlySerializedAs("enforceRightSideLayout")]
		[FormerlySerializedAs("enforceTopStripLayout")]
		[FormerlySerializedAs("enforceStatusBandLayout")]
		[FormerlySerializedAs("enforceEdgeDockLayout")]
		[SerializeField]
		private bool enforceVehicleClusterLayout = true;

		// Token: 0x04005708 RID: 22280
		private const float MinimumLeft = 216f;

		// Token: 0x04005709 RID: 22281
		private const float NearbyGap = 12f;

		// Token: 0x0400570A RID: 22282
		private const float EntryWidth = 190f;

		// Token: 0x0400570B RID: 22283
		private const float EntryHeight = 40f;

		// Token: 0x0400570C RID: 22284
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400570D RID: 22285
		private readonly List<TimedEffectHUDEntry> activeEntries = new List<TimedEffectHUDEntry>();

		// Token: 0x0400570E RID: 22286
		private readonly Queue<TimedEffectHUDEntry> pool = new Queue<TimedEffectHUDEntry>();

		// Token: 0x0400570F RID: 22287
		private RectTransform layoutParent;

		// Token: 0x04005710 RID: 22288
		private Canvas layoutCanvas;

		// Token: 0x04005711 RID: 22289
		private TimedEffectHUDEntry displayedEntry;

		// Token: 0x04005712 RID: 22290
		private readonly Vector3[] vehicleCorners = new Vector3[4];
	}
}
