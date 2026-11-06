using System;
using System.Collections.Generic;
using Features.CielOS;
using Features.PhoneCompanion;
using Features.Police;
using Features.RunModes;
using Features.TimeFreeze;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x02001276 RID: 4726
	[DefaultExecutionOrder(200)]
	public sealed class DetectionThreatIndicatorHUD : MonoBehaviour
	{
		// Token: 0x060092EB RID: 37611 RVA: 0x003C503C File Offset: 0x003C323C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (Object.FindObjectOfType<DetectionThreatIndicatorHUD>(true) != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Detection Threat Indicators");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<DetectionThreatIndicatorHUD>();
		}

		// Token: 0x060092EC RID: 37612 RVA: 0x003C5064 File Offset: 0x003C3264
		private void Awake()
		{
			this._canvas = base.gameObject.AddComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 115;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			this._canvasRect = (RectTransform)base.transform;
			for (int i = 0; i < 12; i++)
			{
				this._pool.Add(this.CreateIndicator(i));
			}
		}

		// Token: 0x060092ED RID: 37613 RVA: 0x003C5108 File Offset: 0x003C3308
		private void LateUpdate()
		{
			bool flag = (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) | SmartphoneController.IsAnyPhoneOut | CielOSController.AnyOpen | (HUDManager.Instance != null && HUDManager.Instance.IsHidden) | (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive) | TimeFreezeController.IsAnyActive;
			SensoryManager instance = SensoryManager.Instance;
			if (flag || instance == null || instance.IsPaused || !instance.IsRunModeDetectionEnabled)
			{
				this.HideFrom(0);
				return;
			}
			if (this._camera == null || !this._camera.isActiveAndEnabled)
			{
				this._camera = Camera.main;
			}
			if (this._camera == null)
			{
				this.HideFrom(0);
				return;
			}
			StealthActor player = instance.GetPlayer();
			if (player == null)
			{
				this.HideFrom(0);
				return;
			}
			if (this._player != player)
			{
				this._player = player;
				this._firstPerson = player.GetComponentInParent<FirstPersonController>();
			}
			Vector3 vector = ((player.VisibilityPoint != null) ? player.Position : (player.transform.position + Vector3.up * 1.1f));
			Vector2 vector2;
			if (!this.TryGetRingCenter(vector, out vector2))
			{
				this.HideFrom(0);
				return;
			}
			IReadOnlyList<NPCSenses> registeredNPCs = instance.GetRegisteredNPCs();
			int num = 0;
			int num2 = 0;
			while (num2 < registeredNPCs.Count && num < this._pool.Count)
			{
				NPCSenses npcsenses = registeredNPCs[num2];
				float num3;
				if (DetectionThreatIndicatorHUD.TryGetWarningLevel(npcsenses, out num3))
				{
					Vector2 direction = DetectionThreatIndicatorHUD.GetDirection(player.Position, npcsenses.EyePosition, this._camera.transform.rotation);
					this.Draw(this._pool[num++], vector2, direction, num3);
				}
				num2++;
			}
			this.HideFrom(num);
		}

		// Token: 0x060092EE RID: 37614 RVA: 0x003C52F0 File Offset: 0x003C34F0
		private static bool TryGetWarningLevel(NPCSenses npc, out float level)
		{
			level = 0f;
			if (npc == null || !npc.isActiveAndEnabled || npc.IsDetected)
			{
				return false;
			}
			AttentionUIController attentionUI = npc.AttentionUI;
			IDetectionUISource detectionUISource = ((attentionUI != null) ? attentionUI.UISource : null);
			Object @object = detectionUISource as Object;
			if (@object != null && @object == null)
			{
				detectionUISource = null;
			}
			if (RunModeService.IsLongWayHome)
			{
				detectionUISource = null;
			}
			bool flag = ((detectionUISource != null) ? detectionUISource.VisualActive : npc.HasLineOfSight);
			level = Mathf.Clamp01((detectionUISource != null) ? detectionUISource.DetectionLevel : npc.CurrentAttention);
			return flag && level >= 0.3f;
		}

		// Token: 0x060092EF RID: 37615 RVA: 0x003C5390 File Offset: 0x003C3590
		private static Vector2 GetDirection(Vector3 playerPosition, Vector3 observerPosition, Quaternion cameraRotation)
		{
			Vector3 vector = Vector3.ProjectOnPlane(observerPosition - playerPosition, Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(cameraRotation * Vector3.forward, Vector3.up);
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = Vector3.Cross(Vector3.ProjectOnPlane(cameraRotation * Vector3.right, Vector3.up), Vector3.up);
			}
			vector2.Normalize();
			Vector3 vector3 = Vector3.Cross(Vector3.up, vector2);
			Vector2 up = new Vector2(Vector3.Dot(vector, vector3), Vector3.Dot(vector, vector2));
			if (up.sqrMagnitude < 0.0025f)
			{
				up = Vector2.up;
			}
			return up.normalized;
		}

		// Token: 0x060092F0 RID: 37616 RVA: 0x003C5438 File Offset: 0x003C3638
		private bool TryGetRingCenter(Vector3 focus, out Vector2 center)
		{
			if (this._firstPerson != null && this._firstPerson.IsFirstPerson && !this._firstPerson.IsCinematicViewSuspended)
			{
				center = this._canvasRect.rect.center;
				return true;
			}
			center = Vector2.zero;
			Vector3 vector = this._camera.WorldToScreenPoint(focus);
			if (vector.z <= Mathf.Max(0.01f, this._camera.nearClipPlane))
			{
				return false;
			}
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvasRect, vector, null, out center))
			{
				return false;
			}
			Rect rect = this._canvasRect.rect;
			float num = Mathf.Min(146f, Mathf.Min(rect.width, rect.height) * 0.4f);
			center.x = Mathf.Clamp(center.x, rect.xMin + num, rect.xMax - num);
			center.y = Mathf.Clamp(center.y, rect.yMin + num, rect.yMax - num);
			return true;
		}

		// Token: 0x060092F1 RID: 37617 RVA: 0x003C5550 File Offset: 0x003C3750
		private void Draw(DetectionThreatIndicatorHUD.Indicator indicator, Vector2 center, Vector2 direction, float level)
		{
			indicator.rect.anchoredPosition = center;
			indicator.rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * 57.29578f - 90f);
			float num = Mathf.InverseLerp(0.3f, 1f, level);
			Color color = Color.Lerp(new Color(0.96f, 0.18f, 0.43f, 1f), new Color(1f, 0.4f, 0.6f, 1f), num);
			indicator.graphic.color = color;
			indicator.rect.localScale = Vector3.one;
			indicator.root.SetActive(true);
		}

		// Token: 0x060092F2 RID: 37618 RVA: 0x003C5614 File Offset: 0x003C3814
		private void HideFrom(int index)
		{
			for (int i = index; i < this._pool.Count; i++)
			{
				if (this._pool[i].root.activeSelf)
				{
					this._pool[i].root.SetActive(false);
				}
			}
		}

		// Token: 0x060092F3 RID: 37619 RVA: 0x003C5668 File Offset: 0x003C3868
		private DetectionThreatIndicatorHUD.Indicator CreateIndicator(int index)
		{
			GameObject gameObject = new GameObject(string.Format("Observer {0}", index + 1), new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ThreatChevronGraphic)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(280f, 280f);
			ThreatChevronGraphic component = gameObject.GetComponent<ThreatChevronGraphic>();
			component.raycastTarget = false;
			Outline outline = gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0.1f, 0.025f, 0.15f, 0.9f);
			outline.effectDistance = new Vector2(1.25f, -1.25f);
			outline.useGraphicAlpha = false;
			gameObject.SetActive(false);
			return new DetectionThreatIndicatorHUD.Indicator(gameObject, rectTransform, component);
		}

		// Token: 0x04009781 RID: 38785
		private const float WarningThreshold = 0.3f;

		// Token: 0x04009782 RID: 38786
		private const int PoolSize = 12;

		// Token: 0x04009783 RID: 38787
		private readonly List<DetectionThreatIndicatorHUD.Indicator> _pool = new List<DetectionThreatIndicatorHUD.Indicator>(12);

		// Token: 0x04009784 RID: 38788
		private Canvas _canvas;

		// Token: 0x04009785 RID: 38789
		private RectTransform _canvasRect;

		// Token: 0x04009786 RID: 38790
		private Camera _camera;

		// Token: 0x04009787 RID: 38791
		private StealthActor _player;

		// Token: 0x04009788 RID: 38792
		private FirstPersonController _firstPerson;

		// Token: 0x02001277 RID: 4727
		private readonly struct Indicator
		{
			// Token: 0x060092F5 RID: 37621 RVA: 0x003C5791 File Offset: 0x003C3991
			public Indicator(GameObject root, RectTransform rect, ThreatChevronGraphic graphic)
			{
				this.root = root;
				this.rect = rect;
				this.graphic = graphic;
			}

			// Token: 0x04009789 RID: 38793
			public readonly GameObject root;

			// Token: 0x0400978A RID: 38794
			public readonly RectTransform rect;

			// Token: 0x0400978B RID: 38795
			public readonly ThreatChevronGraphic graphic;
		}
	}
}
