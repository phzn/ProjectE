using System;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.Placement
{
	// Token: 0x02001148 RID: 4424
	public class PlacementFeedbackUI : MonoBehaviour
	{
		// Token: 0x06008667 RID: 34407 RVA: 0x0036D930 File Offset: 0x0036BB30
		private void Awake()
		{
			if (this.worldSpaceCanvas == null)
			{
				this.CreateWorldSpaceCanvas();
			}
			GameplayHudVisibility.RegisterCanvas(this.worldSpaceCanvas);
			this._mainCamera = Camera.main;
			this._targetRingColor = this.validColor;
			this._currentRingColor = this.validColor;
			this.Hide();
		}

		// Token: 0x06008668 RID: 34408 RVA: 0x0036D988 File Offset: 0x0036BB88
		private void CreateWorldSpaceCanvas()
		{
			GameObject gameObject = new GameObject("[PlacementFeedbackCanvas]");
			gameObject.transform.SetParent(base.transform, false);
			this.worldSpaceCanvas = gameObject.AddComponent<Canvas>();
			this.worldSpaceCanvas.renderMode = RenderMode.WorldSpace;
			this.worldSpaceCanvas.sortingOrder = 1000;
			gameObject.AddComponent<CanvasScaler>().dynamicPixelsPerUnit = 10f;
			this.canvasRect = gameObject.GetComponent<RectTransform>();
			this.canvasRect.sizeDelta = new Vector2(100f, 100f);
			this.CreateVisualElements(gameObject.transform);
		}

		// Token: 0x06008669 RID: 34409 RVA: 0x0036DA1C File Offset: 0x0036BC1C
		private void CreateVisualElements(Transform parent)
		{
			GameObject gameObject = new GameObject("Ring", new Type[] { typeof(Image) });
			gameObject.transform.SetParent(parent, false);
			this.placementRing = gameObject.GetComponent<Image>();
			this.placementRing.sprite = this.CreateRingSprite();
			this.placementRing.type = Image.Type.Sliced;
			this.placementRing.color = this.validColor;
			this.placementRing.rectTransform.sizeDelta = new Vector2(50f, 50f);
			this.placementRing.rectTransform.anchoredPosition = Vector2.zero;
			GameObject gameObject2 = new GameObject("Dot", new Type[] { typeof(Image) });
			gameObject2.transform.SetParent(parent, false);
			this.placementDot = gameObject2.GetComponent<Image>();
			this.placementDot.sprite = this.CreateDotSprite();
			this.placementDot.color = Color.white;
			this.placementDot.rectTransform.sizeDelta = new Vector2(15f, 15f);
			this.placementDot.rectTransform.anchoredPosition = Vector2.zero;
			GameObject gameObject3 = new GameObject("SurfaceIcon", new Type[] { typeof(Image) });
			gameObject3.transform.SetParent(parent, false);
			this.surfaceIcon = gameObject3.GetComponent<Image>();
			this.surfaceIcon.rectTransform.sizeDelta = new Vector2(20f, 20f);
			this.surfaceIcon.rectTransform.anchoredPosition = new Vector2(0f, 35f);
			this.surfaceIcon.gameObject.SetActive(false);
			GameObject gameObject4 = new GameObject("Distance", new Type[] { typeof(TextMeshProUGUI) });
			gameObject4.transform.SetParent(parent, false);
			this.distanceText = gameObject4.GetComponent<TextMeshProUGUI>();
			this.distanceText.fontSize = 12f;
			this.distanceText.alignment = TextAlignmentOptions.Center;
			this.distanceText.rectTransform.sizeDelta = new Vector2(100f, 20f);
			this.distanceText.rectTransform.anchoredPosition = new Vector2(0f, -35f);
			this.distanceText.gameObject.SetActive(false);
			GameObject gameObject5 = new GameObject("Status", new Type[] { typeof(TextMeshProUGUI) });
			gameObject5.transform.SetParent(parent, false);
			this.statusText = gameObject5.GetComponent<TextMeshProUGUI>();
			this.statusText.fontSize = 10f;
			this.statusText.alignment = TextAlignmentOptions.Center;
			this.statusText.rectTransform.sizeDelta = new Vector2(150f, 30f);
			this.statusText.rectTransform.anchoredPosition = new Vector2(0f, 50f);
			this.statusText.gameObject.SetActive(false);
			GameObject gameObject6 = new GameObject("InputHint", new Type[] { typeof(TextMeshProUGUI) });
			gameObject6.transform.SetParent(parent, false);
			this.inputHintText = gameObject6.GetComponent<TextMeshProUGUI>();
			this.inputHintText.fontSize = 11f;
			this.inputHintText.alignment = TextAlignmentOptions.Center;
			this.inputHintText.rectTransform.sizeDelta = new Vector2(120f, 25f);
			this.inputHintText.rectTransform.anchoredPosition = new Vector2(0f, -50f);
			this.inputHintText.text = "LMB: Place | RMB: Cancel";
		}

		// Token: 0x0600866A RID: 34410 RVA: 0x0036DDCC File Offset: 0x0036BFCC
		private Sprite CreateRingSprite()
		{
			int num = 64;
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBA32, false);
			Color[] array = new Color[num * num];
			float num2 = (float)num / 2f;
			float num3 = (float)num / 2f - 2f;
			float num4 = num3 - 8f;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num5 = Mathf.Sqrt(((float)j - num2) * ((float)j - num2) + ((float)i - num2) * ((float)i - num2));
					if (num5 <= num3 && num5 >= num4)
					{
						array[i * num + j] = Color.white;
					}
					else
					{
						array[i * num + j] = Color.clear;
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x0600866B RID: 34411 RVA: 0x0036DEB8 File Offset: 0x0036C0B8
		private Sprite CreateDotSprite()
		{
			int num = 32;
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBA32, false);
			Color[] array = new Color[num * num];
			float num2 = (float)num / 2f;
			float num3 = (float)num / 2f - 2f;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num4 = Mathf.Sqrt(((float)j - num2) * ((float)j - num2) + ((float)i - num2) * ((float)i - num2));
					array[i * num + j] = ((num4 <= num3) ? Color.white : Color.clear);
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x0600866C RID: 34412 RVA: 0x0036DF86 File Offset: 0x0036C186
		public void Show()
		{
			this._isActive = true;
			if (this.worldSpaceCanvas != null)
			{
				this.worldSpaceCanvas.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600866D RID: 34413 RVA: 0x0036DFAE File Offset: 0x0036C1AE
		public void Hide()
		{
			this._isActive = false;
			if (this.worldSpaceCanvas != null)
			{
				this.worldSpaceCanvas.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600866E RID: 34414 RVA: 0x0036DFD8 File Offset: 0x0036C1D8
		public void UpdateFeedback(PlacementResult result, Transform playerTransform)
		{
			if (!this._isActive)
			{
				return;
			}
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			Vector3 normalized = result.HitNormal.normalized;
			Vector3 vector = result.Position + normalized * this.surfaceOffset;
			this._targetPosition = Vector3.SmoothDamp(this._targetPosition, vector, ref this._velocity, 0.05f);
			this._targetRotation = this.BuildSurfaceRotation(normalized);
			if (this.worldSpaceCanvas != null)
			{
				this.worldSpaceCanvas.transform.position = this._targetPosition + Vector3.up * this.uiOffsetY;
				this.worldSpaceCanvas.transform.rotation = Quaternion.LookRotation(this.worldSpaceCanvas.transform.position - this._mainCamera.transform.position);
				float num = Mathf.Clamp(Vector3.Distance(this._mainCamera.transform.position, this._targetPosition) * 0.1f, 0.5f, 2f);
				this.worldSpaceCanvas.transform.localScale = Vector3.one * num;
			}
			if (result.IsValid)
			{
				this._targetRingColor = this.validColor;
				if (this.surfaceIcon != null)
				{
					this.surfaceIcon.gameObject.SetActive(true);
					this.surfaceIcon.sprite = ((result.SurfaceType == PlacementSurfaceType.Wall) ? this.wallIcon : this.floorIcon);
					this.surfaceIcon.color = Color.white;
				}
				if (this.distanceText != null)
				{
					this.distanceText.gameObject.SetActive(true);
					float num2 = Vector3.Distance(playerTransform.position, result.Position);
					this.distanceText.text = string.Format("{0:F1}m", num2);
					this.distanceText.color = ((num2 > this.maxDisplayDistance * 0.8f) ? this.warningColor : Color.white);
				}
				if (this.statusText != null)
				{
					this.statusText.gameObject.SetActive(false);
				}
				float num3 = 1f + Mathf.Sin(Time.time * this.pulseSpeed) * this.pulseScale;
				this._currentScale = Mathf.Lerp(this._currentScale, num3, Time.deltaTime * 10f);
			}
			else
			{
				this._targetRingColor = this.invalidColor;
				if (this.surfaceIcon != null)
				{
					this.surfaceIcon.gameObject.SetActive(true);
					this.surfaceIcon.sprite = this.invalidIcon;
					this.surfaceIcon.color = this.invalidColor;
				}
				if (this.distanceText != null)
				{
					this.distanceText.gameObject.SetActive(false);
				}
				if (this.statusText != null)
				{
					this.statusText.gameObject.SetActive(true);
					this.statusText.text = this.GetInvalidReason(result, playerTransform);
					this.statusText.color = this.invalidColor;
				}
				this._currentScale = Mathf.Lerp(this._currentScale, this.bounceScale, Time.deltaTime * 15f);
			}
			this._currentRingColor = Color.Lerp(this._currentRingColor, this._targetRingColor, Time.deltaTime * this.fadeSpeed);
			if (this.placementRing != null)
			{
				this.placementRing.color = this._currentRingColor;
				this.placementRing.rectTransform.localScale = Vector3.one * this._currentScale;
			}
			if (this.placementDot != null)
			{
				this.placementDot.color = (result.IsValid ? Color.white : this.invalidColor);
			}
		}

		// Token: 0x0600866F RID: 34415 RVA: 0x0036E3B8 File Offset: 0x0036C5B8
		private Quaternion BuildSurfaceRotation(Vector3 normal)
		{
			Vector3 vector = ((this._mainCamera != null) ? this._mainCamera.transform.forward : Vector3.forward);
			Vector3 vector2 = Vector3.Cross(normal, vector);
			if (vector2.sqrMagnitude < 0.001f)
			{
				vector2 = Vector3.Cross(normal, Vector3.right);
			}
			vector2.Normalize();
			return Quaternion.LookRotation(Vector3.Cross(vector2, normal).normalized, normal);
		}

		// Token: 0x06008670 RID: 34416 RVA: 0x0036E42C File Offset: 0x0036C62C
		private string GetInvalidReason(PlacementResult result, Transform playerTransform)
		{
			if (result.IsValid)
			{
				return "";
			}
			if (Vector3.Distance(playerTransform.position, result.Position) > this.maxDisplayDistance)
			{
				return "Too far!";
			}
			if (result.SurfaceType == PlacementSurfaceType.Invalid)
			{
				return "Invalid surface";
			}
			return "Cannot place here";
		}

		// Token: 0x06008671 RID: 34417 RVA: 0x0036E479 File Offset: 0x0036C679
		private void OnDestroy()
		{
			if (this.worldSpaceCanvas != null && this.worldSpaceCanvas.gameObject != null)
			{
				Object.Destroy(this.worldSpaceCanvas.gameObject);
			}
		}

		// Token: 0x040088F8 RID: 35064
		[Header("Canvas References")]
		[SerializeField]
		private Canvas worldSpaceCanvas;

		// Token: 0x040088F9 RID: 35065
		[SerializeField]
		private RectTransform canvasRect;

		// Token: 0x040088FA RID: 35066
		[Header("Visual Elements")]
		[SerializeField]
		private Image placementRing;

		// Token: 0x040088FB RID: 35067
		[SerializeField]
		private Image placementDot;

		// Token: 0x040088FC RID: 35068
		[SerializeField]
		private Image surfaceIcon;

		// Token: 0x040088FD RID: 35069
		[SerializeField]
		private TextMeshProUGUI distanceText;

		// Token: 0x040088FE RID: 35070
		[SerializeField]
		private TextMeshProUGUI statusText;

		// Token: 0x040088FF RID: 35071
		[SerializeField]
		private TextMeshProUGUI inputHintText;

		// Token: 0x04008900 RID: 35072
		[Header("Surface Icons")]
		[SerializeField]
		private Sprite floorIcon;

		// Token: 0x04008901 RID: 35073
		[SerializeField]
		private Sprite wallIcon;

		// Token: 0x04008902 RID: 35074
		[SerializeField]
		private Sprite invalidIcon;

		// Token: 0x04008903 RID: 35075
		[Header("Colors")]
		[SerializeField]
		private Color validColor = new Color(0.2f, 0.9f, 0.4f, 0.9f);

		// Token: 0x04008904 RID: 35076
		[SerializeField]
		private Color invalidColor = new Color(0.9f, 0.2f, 0.2f, 0.9f);

		// Token: 0x04008905 RID: 35077
		[SerializeField]
		private Color warningColor = new Color(0.9f, 0.7f, 0.2f, 0.9f);

		// Token: 0x04008906 RID: 35078
		[Header("Animation")]
		[SerializeField]
		private float pulseSpeed = 2f;

		// Token: 0x04008907 RID: 35079
		[SerializeField]
		private float pulseScale = 0.1f;

		// Token: 0x04008908 RID: 35080
		[SerializeField]
		private float fadeSpeed = 10f;

		// Token: 0x04008909 RID: 35081
		[SerializeField]
		private float bounceScale = 1.2f;

		// Token: 0x0400890A RID: 35082
		[Header("Positioning")]
		[SerializeField]
		private float surfaceOffset = 0.02f;

		// Token: 0x0400890B RID: 35083
		[SerializeField]
		private float uiOffsetY = 0.3f;

		// Token: 0x0400890C RID: 35084
		[SerializeField]
		private float maxDisplayDistance = 10f;

		// Token: 0x0400890D RID: 35085
		private bool _isActive;

		// Token: 0x0400890E RID: 35086
		private Camera _mainCamera;

		// Token: 0x0400890F RID: 35087
		private Vector3 _targetPosition;

		// Token: 0x04008910 RID: 35088
		private Quaternion _targetRotation;

		// Token: 0x04008911 RID: 35089
		private float _currentScale = 1f;

		// Token: 0x04008912 RID: 35090
		private Color _targetRingColor;

		// Token: 0x04008913 RID: 35091
		private Color _currentRingColor;

		// Token: 0x04008914 RID: 35092
		private Vector3 _velocity;
	}
}
