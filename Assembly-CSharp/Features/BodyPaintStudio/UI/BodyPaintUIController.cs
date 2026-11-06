using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.BodyPaintStudio.UI
{
	// Token: 0x02001522 RID: 5410
	public class BodyPaintUIController : MonoBehaviour
	{
		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600A73F RID: 42815 RVA: 0x0045F41C File Offset: 0x0045D61C
		// (remove) Token: 0x0600A740 RID: 42816 RVA: 0x0045F454 File Offset: 0x0045D654
		public event Action OnExitRequested;

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600A741 RID: 42817 RVA: 0x0045F48C File Offset: 0x0045D68C
		// (remove) Token: 0x0600A742 RID: 42818 RVA: 0x0045F4C4 File Offset: 0x0045D6C4
		public event Action OnClearRequested;

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600A743 RID: 42819 RVA: 0x0045F4FC File Offset: 0x0045D6FC
		// (remove) Token: 0x0600A744 RID: 42820 RVA: 0x0045F534 File Offset: 0x0045D734
		public event Action<int> OnPoseSelected;

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600A745 RID: 42821 RVA: 0x0045F569 File Offset: 0x0045D769
		// (set) Token: 0x0600A746 RID: 42822 RVA: 0x0045F571 File Offset: 0x0045D771
		public Color CurrentColor { get; private set; } = Color.red;

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x0600A747 RID: 42823 RVA: 0x0045F57A File Offset: 0x0045D77A
		// (set) Token: 0x0600A748 RID: 42824 RVA: 0x0045F582 File Offset: 0x0045D782
		public float BrushRadiusMeters { get; private set; } = 0.035f;

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600A749 RID: 42825 RVA: 0x0045F58B File Offset: 0x0045D78B
		// (set) Token: 0x0600A74A RID: 42826 RVA: 0x0045F593 File Offset: 0x0045D793
		public float BrushOpacity { get; private set; } = 1f;

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x0600A74B RID: 42827 RVA: 0x0045F59C File Offset: 0x0045D79C
		public bool IsEraser
		{
			get
			{
				return this.eraserToggle != null && this.eraserToggle.isOn;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x0600A74C RID: 42828 RVA: 0x0045F5B9 File Offset: 0x0045D7B9
		public bool IsEyedropper
		{
			get
			{
				return this.eyedropperToggle != null && this.eyedropperToggle.isOn;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600A74D RID: 42829 RVA: 0x0045F5D6 File Offset: 0x0045D7D6
		public BodyPaintBrushCursor BrushCursor
		{
			get
			{
				return this.brushCursor;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600A74E RID: 42830 RVA: 0x0045F5DE File Offset: 0x0045D7DE
		public bool IsPresetDialogOpen
		{
			get
			{
				return this.presetsUI != null && this.presetsUI.IsDialogOpen;
			}
		}

		// Token: 0x0600A74F RID: 42831 RVA: 0x0045F5FB File Offset: 0x0045D7FB
		public void ConfigurePresets(BodyPaintCanvasService canvas, Camera previewCamera)
		{
			if (this.presetsUI == null)
			{
				this.presetsUI = base.GetComponent<BodyPaintPresetsUI>();
			}
			if (this.presetsUI != null)
			{
				this.presetsUI.Configure(canvas, previewCamera);
			}
		}

		// Token: 0x0600A750 RID: 42832 RVA: 0x0045F632 File Offset: 0x0045D832
		public bool TryClosePresetDialog()
		{
			return this.presetsUI != null && this.presetsUI.TryCloseDialog();
		}

		// Token: 0x0600A751 RID: 42833 RVA: 0x0045F650 File Offset: 0x0045D850
		public void ConfigureSurface(string title, bool smallSurface, bool allowPoses = true)
		{
			if (this.poseContainer != null)
			{
				((this.poseContainer.parent != null && this.poseContainer.parent.name == "PoseRow") ? this.poseContainer.parent : this.poseContainer).gameObject.SetActive(allowPoses);
			}
			if (this.eraserToggle != null)
			{
				this.eraserToggle.SetIsOnWithoutNotify(false);
			}
			this.ClearEyedropper();
			this._sessionMinBrushRadius = (smallSurface ? 0.0005f : this.minBrushRadiusMeters);
			this._sessionMaxBrushRadius = (smallSurface ? 0.025f : this.maxBrushRadiusMeters);
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
			{
				if (tmp_Text.name == "Title")
				{
					tmp_Text.text = GameLocalization.GetSource("neck_signs", title);
				}
			}
			if (this.brushSizeSlider != null)
			{
				if (smallSurface)
				{
					this.brushSizeSlider.value = 0.12f;
				}
				this.HandleBrushSize(this.brushSizeSlider.value);
			}
		}

		// Token: 0x0600A752 RID: 42834 RVA: 0x0045F773 File Offset: 0x0045D973
		public void ClearEyedropper()
		{
			if (this.eyedropperToggle != null)
			{
				this.eyedropperToggle.SetIsOnWithoutNotify(false);
			}
		}

		// Token: 0x0600A753 RID: 42835 RVA: 0x0045F790 File Offset: 0x0045D990
		private void Awake()
		{
			if (this.colorWheel != null)
			{
				this.colorWheel.OnHueSatChanged += this.HandleHueSatChanged;
			}
			if (this.valueSlider != null)
			{
				this.valueSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleValueSlider));
			}
			if (this.redSlider != null)
			{
				this.redSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.HandleRgbSliders();
				});
			}
			if (this.greenSlider != null)
			{
				this.greenSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.HandleRgbSliders();
				});
			}
			if (this.blueSlider != null)
			{
				this.blueSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.HandleRgbSliders();
				});
			}
			if (this.brushSizeSlider != null)
			{
				this.brushSizeSlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleBrushSize));
			}
			if (this.brushOpacitySlider != null)
			{
				this.brushOpacitySlider.onValueChanged.AddListener(new UnityAction<float>(this.HandleBrushOpacity));
			}
			if (this.clearButton != null)
			{
				this.clearButton.onClick.AddListener(delegate
				{
					Action onClearRequested = this.OnClearRequested;
					if (onClearRequested == null)
					{
						return;
					}
					onClearRequested();
				});
			}
			if (this.exitButton != null)
			{
				this.exitButton.onClick.AddListener(delegate
				{
					Action onExitRequested = this.OnExitRequested;
					if (onExitRequested == null)
					{
						return;
					}
					onExitRequested();
				});
			}
			this.SetupPaletteButtons();
			this.SetupPoseButtons();
		}

		// Token: 0x0600A754 RID: 42836 RVA: 0x0045F920 File Offset: 0x0045DB20
		private void SetupPoseButtons()
		{
			if (this.poseContainer == null)
			{
				return;
			}
			int num = 0;
			foreach (object obj in this.poseContainer)
			{
				Component component = (Transform)obj;
				Button component2 = component.GetComponent<Button>();
				if (!(component2 == null))
				{
					num++;
					int captured = num;
					component2.onClick.AddListener(delegate
					{
						Action<int> onPoseSelected = this.OnPoseSelected;
						if (onPoseSelected == null)
						{
							return;
						}
						onPoseSelected(captured);
					});
				}
			}
		}

		// Token: 0x0600A755 RID: 42837 RVA: 0x0045F9C4 File Offset: 0x0045DBC4
		private void Start()
		{
			if (this.brushSizeSlider != null)
			{
				this.HandleBrushSize(this.brushSizeSlider.value);
			}
			else
			{
				this.BrushRadiusMeters = 0.035f;
			}
			if (this.brushOpacitySlider != null)
			{
				this.HandleBrushOpacity(this.brushOpacitySlider.value);
			}
			this.ApplyHsv(true);
		}

		// Token: 0x0600A756 RID: 42838 RVA: 0x0045FA24 File Offset: 0x0045DC24
		private void SetupPaletteButtons()
		{
			if (this.paletteContainer == null)
			{
				return;
			}
			foreach (object obj in this.paletteContainer)
			{
				Transform transform = (Transform)obj;
				Button component = transform.GetComponent<Button>();
				Image component2 = transform.GetComponent<Image>();
				if (!(component == null) && !(component2 == null))
				{
					Color swatch = component2.color;
					component.onClick.AddListener(delegate
					{
						this.SetColor(swatch);
					});
				}
			}
		}

		// Token: 0x0600A757 RID: 42839 RVA: 0x0045FAD8 File Offset: 0x0045DCD8
		public void SetColor(Color color)
		{
			Color.RGBToHSV(color, out this._hue, out this._sat, out this._val);
			this.ApplyHsv(true);
		}

		// Token: 0x0600A758 RID: 42840 RVA: 0x0045FAF9 File Offset: 0x0045DCF9
		private void HandleHueSatChanged(float hue, float sat)
		{
			this._hue = hue;
			this._sat = sat;
			this.ApplyHsv(false);
		}

		// Token: 0x0600A759 RID: 42841 RVA: 0x0045FB10 File Offset: 0x0045DD10
		private void HandleValueSlider(float v)
		{
			if (this._syncing)
			{
				return;
			}
			this._val = Mathf.Clamp01(v);
			this.ApplyHsv(false);
		}

		// Token: 0x0600A75A RID: 42842 RVA: 0x0045FB30 File Offset: 0x0045DD30
		private void HandleRgbSliders()
		{
			if (this._syncing)
			{
				return;
			}
			Color.RGBToHSV(new Color((this.redSlider != null) ? this.redSlider.value : this.CurrentColor.r, (this.greenSlider != null) ? this.greenSlider.value : this.CurrentColor.g, (this.blueSlider != null) ? this.blueSlider.value : this.CurrentColor.b, 1f), out this._hue, out this._sat, out this._val);
			this.ApplyHsv(true);
		}

		// Token: 0x0600A75B RID: 42843 RVA: 0x0045FBE0 File Offset: 0x0045DDE0
		private void ApplyHsv(bool refreshWheelKnob)
		{
			this.CurrentColor = Color.HSVToRGB(this._hue, this._sat, this._val);
			this._syncing = true;
			if (this.valueSlider != null)
			{
				this.valueSlider.SetValueWithoutNotify(this._val);
			}
			if (this.redSlider != null)
			{
				this.redSlider.SetValueWithoutNotify(this.CurrentColor.r);
			}
			if (this.greenSlider != null)
			{
				this.greenSlider.SetValueWithoutNotify(this.CurrentColor.g);
			}
			if (this.blueSlider != null)
			{
				this.blueSlider.SetValueWithoutNotify(this.CurrentColor.b);
			}
			if (this.redValueLabel != null)
			{
				TMP_Text tmp_Text = this.redValueLabel;
				Color color = this.CurrentColor;
				tmp_Text.text = color.r.ToString("0.000");
			}
			if (this.greenValueLabel != null)
			{
				TMP_Text tmp_Text2 = this.greenValueLabel;
				Color color = this.CurrentColor;
				tmp_Text2.text = color.g.ToString("0.000");
			}
			if (this.blueValueLabel != null)
			{
				TMP_Text tmp_Text3 = this.blueValueLabel;
				Color color = this.CurrentColor;
				tmp_Text3.text = color.b.ToString("0.000");
			}
			if (this.colorPreview != null)
			{
				this.colorPreview.color = new Color(this.CurrentColor.r, this.CurrentColor.g, this.CurrentColor.b, 1f);
			}
			if (this.colorWheel != null)
			{
				this.colorWheel.SetValue(this._val);
				if (refreshWheelKnob)
				{
					this.colorWheel.SetKnobFromHueSat(this._hue, this._sat);
				}
			}
			this._syncing = false;
		}

		// Token: 0x0600A75C RID: 42844 RVA: 0x0045FDB4 File Offset: 0x0045DFB4
		private void HandleBrushSize(float normalized)
		{
			float num = ((this._sessionMinBrushRadius > 0f) ? this._sessionMinBrushRadius : this.minBrushRadiusMeters);
			float num2 = ((this._sessionMaxBrushRadius > 0f) ? this._sessionMaxBrushRadius : this.maxBrushRadiusMeters);
			this.BrushRadiusMeters = Mathf.Lerp(num, num2, Mathf.Clamp01(normalized));
		}

		// Token: 0x0600A75D RID: 42845 RVA: 0x0045FE0C File Offset: 0x0045E00C
		private void HandleBrushOpacity(float normalized)
		{
			this.BrushOpacity = Mathf.Clamp01(normalized);
		}

		// Token: 0x0400AC00 RID: 44032
		[Header("Color")]
		[SerializeField]
		private BodyPaintColorWheel colorWheel;

		// Token: 0x0400AC01 RID: 44033
		[SerializeField]
		private Slider valueSlider;

		// Token: 0x0400AC02 RID: 44034
		[SerializeField]
		private Slider redSlider;

		// Token: 0x0400AC03 RID: 44035
		[SerializeField]
		private Slider greenSlider;

		// Token: 0x0400AC04 RID: 44036
		[SerializeField]
		private Slider blueSlider;

		// Token: 0x0400AC05 RID: 44037
		[SerializeField]
		private TMP_Text redValueLabel;

		// Token: 0x0400AC06 RID: 44038
		[SerializeField]
		private TMP_Text greenValueLabel;

		// Token: 0x0400AC07 RID: 44039
		[SerializeField]
		private TMP_Text blueValueLabel;

		// Token: 0x0400AC08 RID: 44040
		[SerializeField]
		private Image colorPreview;

		// Token: 0x0400AC09 RID: 44041
		[Header("Brush")]
		[SerializeField]
		private Slider brushSizeSlider;

		// Token: 0x0400AC0A RID: 44042
		[SerializeField]
		private Slider brushOpacitySlider;

		// Token: 0x0400AC0B RID: 44043
		[SerializeField]
		private Toggle eraserToggle;

		// Token: 0x0400AC0C RID: 44044
		[SerializeField]
		private Toggle eyedropperToggle;

		// Token: 0x0400AC0D RID: 44045
		[Header("Palette")]
		[SerializeField]
		private Transform paletteContainer;

		// Token: 0x0400AC0E RID: 44046
		[Header("Pose")]
		[Tooltip("Container dei bottoni posa: il figlio N seleziona la posa N+1")]
		[SerializeField]
		private Transform poseContainer;

		// Token: 0x0400AC0F RID: 44047
		[Header("Actions")]
		[SerializeField]
		private Button clearButton;

		// Token: 0x0400AC10 RID: 44048
		[SerializeField]
		private Button exitButton;

		// Token: 0x0400AC11 RID: 44049
		[Header("Presets")]
		[SerializeField]
		private BodyPaintPresetsUI presetsUI;

		// Token: 0x0400AC12 RID: 44050
		[Header("Cursor")]
		[SerializeField]
		private BodyPaintBrushCursor brushCursor;

		// Token: 0x0400AC13 RID: 44051
		[Header("Brush Range")]
		[Tooltip("Raggio pennello min/max in METRI (world space, pennello sferico). Min 2mm per il lavoro di precisione con lo zoom estremo.")]
		[SerializeField]
		private float minBrushRadiusMeters = 0.002f;

		// Token: 0x0400AC14 RID: 44052
		[SerializeField]
		private float maxBrushRadiusMeters = 0.09f;

		// Token: 0x0400AC18 RID: 44056
		private float _hue;

		// Token: 0x0400AC19 RID: 44057
		private float _sat = 1f;

		// Token: 0x0400AC1A RID: 44058
		private float _val = 1f;

		// Token: 0x0400AC1B RID: 44059
		private bool _syncing;

		// Token: 0x0400AC1C RID: 44060
		private float _sessionMinBrushRadius;

		// Token: 0x0400AC1D RID: 44061
		private float _sessionMaxBrushRadius;
	}
}
