using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000235 RID: 565
	public class RadialSlider : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x00067057 File Offset: 0x00065257
		// (set) Token: 0x06000F71 RID: 3953 RVA: 0x0006705F File Offset: 0x0006525F
		public float SliderAngle
		{
			get
			{
				return this.currentAngle;
			}
			set
			{
				this.currentAngle = Mathf.Clamp(value, 0f, 360f);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00067077 File Offset: 0x00065277
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0006708F File Offset: 0x0006528F
		public float SliderValue
		{
			get
			{
				return (float)((long)(this.SliderValueRaw * this.valueDisplayPrecision)) / this.valueDisplayPrecision;
			}
			set
			{
				this.SliderValueRaw = value;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x00067098 File Offset: 0x00065298
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x000670AD File Offset: 0x000652AD
		public float SliderValueRaw
		{
			get
			{
				return this.SliderAngle / 360f * this.maxValue;
			}
			set
			{
				this.SliderAngle = value * 360f / this.maxValue;
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000670C3 File Offset: 0x000652C3
		private void Awake()
		{
			this.graphicRaycaster = base.GetComponentInParent<GraphicRaycaster>();
			if (this.graphicRaycaster == null)
			{
				Debug.LogWarning("<b>[Radial Slider]</b> Could not find GraphicRaycaster component in parent.", this);
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000670EC File Offset: 0x000652EC
		private void Start()
		{
			this.valueDisplayPrecision = Mathf.Pow(10f, (float)this.decimals);
			if (this.rememberValue)
			{
				this.LoadState();
			}
			else
			{
				this.SliderAngle = this.currentValue * 3.6f;
			}
			this.SliderValue = this.currentValue;
			this.onValueChanged.Invoke(this.SliderValueRaw);
			this.UpdateUI();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00067158 File Offset: 0x00065358
		public void OnPointerDown(PointerEventData eventData)
		{
			this.hitRectTransform = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
			this.isPointerDown = true;
			this.currentAngleOnPointerDown = this.SliderAngle;
			this.HandleSliderMouseInput(eventData, true);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00067199 File Offset: 0x00065399
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.HasValueChanged())
			{
				this.SaveState();
			}
			this.hitRectTransform = null;
			this.isPointerDown = false;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000671B7 File Offset: 0x000653B7
		public void OnDrag(PointerEventData eventData)
		{
			if (this.currentValue >= this.minValue)
			{
				this.HandleSliderMouseInput(eventData, false);
				return;
			}
			if (this.currentValue <= this.minValue)
			{
				this.SliderValueRaw = this.minValue;
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000671EA File Offset: 0x000653EA
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.onPointerEnter.Invoke();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000671F7 File Offset: 0x000653F7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.onPointerExit.Invoke();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00067204 File Offset: 0x00065404
		public void LoadState()
		{
			this.currentAngle = PlayerPrefs.GetFloat(this.sliderTag + "Radial");
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00067221 File Offset: 0x00065421
		public void SaveState()
		{
			if (!this.rememberValue)
			{
				return;
			}
			PlayerPrefs.SetFloat(this.sliderTag + "Radial", this.currentAngle);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00067248 File Offset: 0x00065448
		public void UpdateUI()
		{
			if (this.SliderValueRaw >= this.minValue)
			{
				float num = this.SliderAngle / 360f;
				this.indicatorPivot.transform.localEulerAngles = new Vector3(180f, 0f, this.SliderAngle);
				this.sliderImage.fillAmount = num;
				this.valueText.text = string.Format("{0}{1}", this.SliderValue, this.isPercent ? "%" : "");
				this.currentValue = this.SliderValue;
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000672E1 File Offset: 0x000654E1
		private bool HasValueChanged()
		{
			return this.SliderAngle != this.currentAngleOnPointerDown;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000672F4 File Offset: 0x000654F4
		private void HandleSliderMouseInput(PointerEventData eventData, bool allowValueWrap)
		{
			if (!this.isPointerDown)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.hitRectTransform, eventData.position, eventData.pressEventCamera, out vector);
			float num = Mathf.Atan2(-vector.y, vector.x) * 57.29578f + 180f;
			if (!allowValueWrap)
			{
				this.currentAngle = this.SliderAngle;
				if (Mathf.Abs(num - this.currentAngle) >= 180f)
				{
					num = ((this.currentAngle < num) ? 0f : 360f);
				}
			}
			this.SliderAngle = num;
			this.UpdateUI();
			if (this.HasValueChanged())
			{
				this.onValueChanged.Invoke(this.SliderValueRaw);
			}
		}

		// Token: 0x04001058 RID: 4184
		private const string PREFS_UI_SAVE_NAME = "Radial";

		// Token: 0x04001059 RID: 4185
		public float currentValue = 50f;

		// Token: 0x0400105A RID: 4186
		public Image sliderImage;

		// Token: 0x0400105B RID: 4187
		public Transform indicatorPivot;

		// Token: 0x0400105C RID: 4188
		public TextMeshProUGUI valueText;

		// Token: 0x0400105D RID: 4189
		public float minValue;

		// Token: 0x0400105E RID: 4190
		public float maxValue = 100f;

		// Token: 0x0400105F RID: 4191
		[Range(0f, 8f)]
		public int decimals;

		// Token: 0x04001060 RID: 4192
		public bool isPercent;

		// Token: 0x04001061 RID: 4193
		public RadialSlider.StartPoint startPoint;

		// Token: 0x04001062 RID: 4194
		public bool rememberValue;

		// Token: 0x04001063 RID: 4195
		public string sliderTag;

		// Token: 0x04001064 RID: 4196
		[SerializeField]
		public RadialSlider.SliderEvent onValueChanged = new RadialSlider.SliderEvent();

		// Token: 0x04001065 RID: 4197
		public UnityEvent onPointerEnter;

		// Token: 0x04001066 RID: 4198
		public UnityEvent onPointerExit;

		// Token: 0x04001067 RID: 4199
		private GraphicRaycaster graphicRaycaster;

		// Token: 0x04001068 RID: 4200
		private RectTransform hitRectTransform;

		// Token: 0x04001069 RID: 4201
		private bool isPointerDown;

		// Token: 0x0400106A RID: 4202
		private float currentAngle;

		// Token: 0x0400106B RID: 4203
		private float currentAngleOnPointerDown;

		// Token: 0x0400106C RID: 4204
		private float valueDisplayPrecision;

		// Token: 0x02000236 RID: 566
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		// Token: 0x02000237 RID: 567
		public enum StartPoint
		{
			// Token: 0x0400106E RID: 4206
			Left,
			// Token: 0x0400106F RID: 4207
			Right,
			// Token: 0x04001070 RID: 4208
			Top,
			// Token: 0x04001071 RID: 4209
			Down
		}
	}
}
