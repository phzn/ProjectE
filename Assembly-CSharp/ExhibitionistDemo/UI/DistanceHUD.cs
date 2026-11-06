using System;
using Features.Localization;
using Features.RunModes;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ExhibitionistDemo.UI
{
	// Token: 0x02000793 RID: 1939
	public class DistanceHUD : MonoBehaviour
	{
		// Token: 0x060032F9 RID: 13049 RVA: 0x001216C0 File Offset: 0x0011F8C0
		private void OnEnable()
		{
			this.RegisterHudCanvas();
			GameLocalization.LocaleChanged += this.UpdateDisplay;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x001216D9 File Offset: 0x0011F8D9
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.UpdateDisplay;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x001216EC File Offset: 0x0011F8EC
		private void OnTransformParentChanged()
		{
			this.RegisterHudCanvas();
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x001216F4 File Offset: 0x0011F8F4
		private void RegisterHudCanvas()
		{
			GameplayHudVisibility.RegisterCanvas(base.GetComponentInParent<Canvas>(true));
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00121704 File Offset: 0x0011F904
		private void Start()
		{
			if (this.distanceTracker == null)
			{
				this.distanceTracker = Object.FindObjectOfType<DistanceTracker>();
				if (this.distanceTracker == null)
				{
					Debug.LogError("DistanceHUD: No DistanceTracker found in scene!");
				}
			}
			if (this.distanceNumberText == null)
			{
				Debug.LogWarning("DistanceHUD: No distance number Text component assigned!");
			}
			if (this.distanceUnitText == null)
			{
				Debug.LogWarning("DistanceHUD: No distance unit Text component assigned!");
			}
			this.UpdateDisplay();
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x00121778 File Offset: 0x0011F978
		private void Update()
		{
			if (this.updateInterval > 0f)
			{
				this.updateTimer += Time.deltaTime;
				if (this.updateTimer >= this.updateInterval)
				{
					this.updateTimer = 0f;
					this.UpdateDisplay();
					return;
				}
			}
			else
			{
				this.UpdateDisplay();
			}
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x001217CC File Offset: 0x0011F9CC
		private void UpdateDisplay()
		{
			if (this.distanceTracker == null)
			{
				return;
			}
			float displayDistance = this.distanceTracker.GetDisplayDistance(this.use2DDistance);
			string text;
			string text2;
			this.FormatDistance(displayDistance, out text, out text2);
			if (RunModeService.IsLongWayHome)
			{
				text2 = ((text2 == "metres") ? "m" : ((text2 == "feet") ? "ft" : text2)) + " home";
			}
			if (this.distanceNumberText != null)
			{
				this.distanceNumberText.text = text;
				this.FitNumberWidth();
				if (this.useColorGradient)
				{
					float num = Mathf.Clamp01(displayDistance / this.maxDistanceForColor);
					this.distanceNumberText.color = Color.Lerp(this.nearColor, this.farColor, num);
				}
			}
			if (this.distanceUnitText != null)
			{
				LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.BindSource(this.distanceUnitText, "distance_hud", text2);
				if (localizedLegacyGameText != null)
				{
					localizedLegacyGameText.FitTranslationToRectangle();
				}
				this.FitUnitWidth();
				if (this.useColorGradient)
				{
					float num2 = Mathf.Clamp01(displayDistance / this.maxDistanceForColor);
					this.distanceUnitText.color = Color.Lerp(this.nearColor, this.farColor, num2);
				}
			}
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x001218F8 File Offset: 0x0011FAF8
		private void FitNumberWidth()
		{
			HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
			ContentSizeFitter component2 = this.distanceNumberText.GetComponent<ContentSizeFitter>();
			if (component == null || component2 == null || this.distanceNumberText.transform.parent != base.transform || component2.horizontalFit != ContentSizeFitter.FitMode.Unconstrained)
			{
				return;
			}
			RectTransform rectTransform = this.distanceNumberText.rectTransform;
			if (this._numberMaximumWidth <= 0f)
			{
				this._numberMaximumWidth = rectTransform.rect.width;
			}
			if (this._numberMaximumWidth <= 0f)
			{
				return;
			}
			component.childForceExpandWidth = false;
			TextGenerationSettings generationSettings = this.distanceNumberText.GetGenerationSettings(Vector2.zero);
			generationSettings.resizeTextForBestFit = false;
			if (this.distanceNumberText.resizeTextForBestFit)
			{
				generationSettings.fontSize = this.distanceNumberText.resizeTextMaxSize;
			}
			float num = this.distanceNumberText.cachedTextGeneratorForLayout.GetPreferredWidth(this.distanceNumberText.text, generationSettings) / this.distanceNumberText.pixelsPerUnit;
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(Mathf.Ceil(num), 1f, this._numberMaximumWidth));
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x00121A14 File Offset: 0x0011FC14
		private void FitUnitWidth()
		{
			if (this._unitWidthFitter == null)
			{
				this._unitWidthFitter = this.distanceUnitText.GetComponent<ContentSizeFitter>();
				if (this._unitWidthFitter == null)
				{
					return;
				}
				this._originalUnitWidthFit = this._unitWidthFitter.horizontalFit;
			}
			if (this._originalUnitWidthFit != ContentSizeFitter.FitMode.PreferredSize)
			{
				return;
			}
			HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
			if (component == null || this.distanceNumberText == null || this.distanceNumberText.transform.parent != base.transform || this.distanceUnitText.transform.parent != base.transform)
			{
				return;
			}
			float num = ((RectTransform)base.transform).rect.width - (float)component.padding.horizontal - component.spacing - this.distanceNumberText.rectTransform.rect.width;
			if (num <= 0f)
			{
				return;
			}
			float preferredWidth = this.distanceUnitText.preferredWidth;
			if (GameLocalization.CurrentCode == "en" && preferredWidth <= num)
			{
				this._unitWidthFitter.horizontalFit = this._originalUnitWidthFit;
				return;
			}
			this._unitWidthFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			this.distanceUnitText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(preferredWidth, 1f, num));
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x00121B70 File Offset: 0x0011FD70
		private void FormatDistance(float distanceInMeters, out string number, out string unit)
		{
			if (this.measurementSystem == MeasurementSystem.International)
			{
				if (this.autoConvertToLargerUnit && distanceInMeters >= this.conversionThreshold)
				{
					number = (Math.Floor((double)distanceInMeters / 10.0) / 100.0).ToString("F2");
					unit = "km";
					return;
				}
				number = Math.Floor((double)distanceInMeters).ToString("F0");
				unit = "metres";
				return;
			}
			else
			{
				double num = (double)distanceInMeters * 3.28084;
				if (this.autoConvertToLargerUnit && num >= (double)this.conversionThreshold)
				{
					number = (Math.Floor(num / 52.8) / 100.0).ToString("F2");
					unit = "miles";
					return;
				}
				number = Math.Floor(num).ToString("F0");
				unit = "feet";
				return;
			}
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x00121C52 File Offset: 0x0011FE52
		public void ForceUpdate()
		{
			this.UpdateDisplay();
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x00121C5A File Offset: 0x0011FE5A
		public void SetMeasurementSystem(MeasurementSystem system)
		{
			this.measurementSystem = system;
			if (system == MeasurementSystem.International)
			{
				this.conversionThreshold = 1000f;
			}
			else
			{
				this.conversionThreshold = 5280f;
			}
			this.UpdateDisplay();
		}

		// Token: 0x040033A3 RID: 13219
		[Header("References")]
		[Tooltip("The DistanceTracker component to read distance from")]
		public DistanceTracker distanceTracker;

		// Token: 0x040033A4 RID: 13220
		[Tooltip("The Text UI component to display the distance number")]
		public Text distanceNumberText;

		// Token: 0x040033A5 RID: 13221
		[Tooltip("The Text UI component to display the unit (metres/km)")]
		public Text distanceUnitText;

		// Token: 0x040033A6 RID: 13222
		[Header("Display Settings")]
		[Tooltip("Measurement system to use")]
		public MeasurementSystem measurementSystem;

		// Token: 0x040033A7 RID: 13223
		[Tooltip("Use 2D distance (ignoring Y axis) instead of 3D distance")]
		public bool use2DDistance = true;

		// Token: 0x040033A8 RID: 13224
		[Tooltip("Update frequency in seconds (0 = every frame)")]
		public float updateInterval = 0.1f;

		// Token: 0x040033A9 RID: 13225
		[Tooltip("Automatically convert to larger unit (km/miles) when threshold is reached")]
		public bool autoConvertToLargerUnit = true;

		// Token: 0x040033AA RID: 13226
		[Tooltip("Distance threshold for conversion (1000m for km, 5280 feet for miles)")]
		public float conversionThreshold = 1000f;

		// Token: 0x040033AB RID: 13227
		[Header("Optional: Color Based on Distance")]
		[Tooltip("Enable color change based on distance")]
		public bool useColorGradient;

		// Token: 0x040033AC RID: 13228
		[Tooltip("Color at distance 0")]
		public Color nearColor = Color.green;

		// Token: 0x040033AD RID: 13229
		[Tooltip("Color at max distance")]
		public Color farColor = Color.red;

		// Token: 0x040033AE RID: 13230
		[Tooltip("Distance for far color")]
		public float maxDistanceForColor = 100f;

		// Token: 0x040033AF RID: 13231
		private float updateTimer;

		// Token: 0x040033B0 RID: 13232
		private ContentSizeFitter _unitWidthFitter;

		// Token: 0x040033B1 RID: 13233
		private ContentSizeFitter.FitMode _originalUnitWidthFit;

		// Token: 0x040033B2 RID: 13234
		private float _numberMaximumWidth;
	}
}
