using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x02000037 RID: 55
	[AddComponentMenu("UI/Slider", 34)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class Slider : Selectable, IDragHandler, IEventSystemHandler, IInitializePotentialDragHandler, ICanvasElement
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00014095 File Offset: 0x00012295
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001409D File Offset: 0x0001229D
		public RectTransform fillRect
		{
			get
			{
				return this.m_FillRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_FillRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000140B9 File Offset: 0x000122B9
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000140C1 File Offset: 0x000122C1
		public RectTransform handleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000140DD File Offset: 0x000122DD
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x000140E5 File Offset: 0x000122E5
		public Slider.Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Slider.Direction>(ref this.m_Direction, value))
				{
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000140FB File Offset: 0x000122FB
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00014103 File Offset: 0x00012303
		public float minValue
		{
			get
			{
				return this.m_MinValue;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00014126 File Offset: 0x00012326
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0001412E File Offset: 0x0001232E
		public float maxValue
		{
			get
			{
				return this.m_MaxValue;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00014151 File Offset: 0x00012351
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00014159 File Offset: 0x00012359
		public bool wholeNumbers
		{
			get
			{
				return this.m_WholeNumbers;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001417C File Offset: 0x0001237C
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00014198 File Offset: 0x00012398
		public virtual float value
		{
			get
			{
				if (!this.wholeNumbers)
				{
					return this.m_Value;
				}
				return Mathf.Round(this.m_Value);
			}
			set
			{
				this.Set(value, true);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000141A2 File Offset: 0x000123A2
		public virtual void SetValueWithoutNotify(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x000141AC File Offset: 0x000123AC
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x000141DE File Offset: 0x000123DE
		public float normalizedValue
		{
			get
			{
				if (Mathf.Approximately(this.minValue, this.maxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.minValue, this.maxValue, this.value);
			}
			set
			{
				this.value = Mathf.Lerp(this.minValue, this.maxValue, value);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000141F8 File Offset: 0x000123F8
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00014200 File Offset: 0x00012400
		public Slider.SliderEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00014209 File Offset: 0x00012409
		private float stepSize
		{
			get
			{
				if (!this.wholeNumbers)
				{
					return (this.maxValue - this.minValue) * 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001422C File Offset: 0x0001242C
		protected Slider()
		{
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00014255 File Offset: 0x00012455
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00014257 File Offset: 0x00012457
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00014259 File Offset: 0x00012459
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001425B File Offset: 0x0001245B
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.Set(this.m_Value, false);
			this.UpdateVisuals();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001427C File Offset: 0x0001247C
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001428F File Offset: 0x0001248F
		protected virtual void Update()
		{
			if (this.m_DelayedUpdateVisuals)
			{
				this.m_DelayedUpdateVisuals = false;
				this.Set(this.m_Value, false);
				this.UpdateVisuals();
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000142B4 File Offset: 0x000124B4
		protected override void OnDidApplyAnimationProperties()
		{
			this.m_Value = this.ClampValue(this.m_Value);
			float num = this.normalizedValue;
			if (this.m_FillContainerRect != null)
			{
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					num = this.m_FillImage.fillAmount;
				}
				else
				{
					num = (this.reverseValue ? (1f - this.m_FillRect.anchorMin[(int)this.axis]) : this.m_FillRect.anchorMax[(int)this.axis]);
				}
			}
			else if (this.m_HandleContainerRect != null)
			{
				num = (this.reverseValue ? (1f - this.m_HandleRect.anchorMin[(int)this.axis]) : this.m_HandleRect.anchorMin[(int)this.axis]);
			}
			this.UpdateVisuals();
			if (num != this.normalizedValue)
			{
				UISystemProfilerApi.AddMarker("Slider.value", this);
				this.onValueChanged.Invoke(this.m_Value);
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000143DC File Offset: 0x000125DC
		private void UpdateCachedReferences()
		{
			if (this.m_FillRect && this.m_FillRect != (RectTransform)base.transform)
			{
				this.m_FillTransform = this.m_FillRect.transform;
				this.m_FillImage = this.m_FillRect.GetComponent<Image>();
				if (this.m_FillTransform.parent != null)
				{
					this.m_FillContainerRect = this.m_FillTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_FillRect = null;
				this.m_FillContainerRect = null;
				this.m_FillImage = null;
			}
			if (this.m_HandleRect && this.m_HandleRect != (RectTransform)base.transform)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
					return;
				}
			}
			else
			{
				this.m_HandleRect = null;
				this.m_HandleContainerRect = null;
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000144E0 File Offset: 0x000126E0
		private float ClampValue(float input)
		{
			float num = Mathf.Clamp(input, this.minValue, this.maxValue);
			if (this.wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00014510 File Offset: 0x00012710
		protected virtual void Set(float input, bool sendCallback = true)
		{
			float num = this.ClampValue(input);
			if (this.m_Value == num)
			{
				return;
			}
			this.m_Value = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				UISystemProfilerApi.AddMarker("Slider.value", this);
				this.m_OnValueChanged.Invoke(num);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00014556 File Offset: 0x00012756
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateVisuals();
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001456D File Offset: 0x0001276D
		private Slider.Axis axis
		{
			get
			{
				if (this.m_Direction != Slider.Direction.LeftToRight && this.m_Direction != Slider.Direction.RightToLeft)
				{
					return Slider.Axis.Vertical;
				}
				return Slider.Axis.Horizontal;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00014583 File Offset: 0x00012783
		private bool reverseValue
		{
			get
			{
				return this.m_Direction == Slider.Direction.RightToLeft || this.m_Direction == Slider.Direction.TopToBottom;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001459C File Offset: 0x0001279C
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_FillContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_FillRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					this.m_FillImage.fillAmount = this.normalizedValue;
				}
				else if (this.reverseValue)
				{
					zero[(int)this.axis] = 1f - this.normalizedValue;
				}
				else
				{
					one[(int)this.axis] = this.normalizedValue;
				}
				this.m_FillRect.anchorMin = zero;
				this.m_FillRect.anchorMax = one;
			}
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero2 = Vector2.zero;
				Vector2 one2 = Vector2.one;
				zero2[(int)this.axis] = (one2[(int)this.axis] = (this.reverseValue ? (1f - this.normalizedValue) : this.normalizedValue));
				this.m_HandleRect.anchorMin = zero2;
				this.m_HandleRect.anchorMax = one2;
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000146EC File Offset: 0x000128EC
		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform rectTransform = this.m_HandleContainerRect ?? this.m_FillContainerRect;
			if (rectTransform != null && rectTransform.rect.size[(int)this.axis] > 0f)
			{
				Vector2 zero = Vector2.zero;
				if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref zero))
				{
					return;
				}
				Vector2 vector;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, zero, cam, out vector))
				{
					return;
				}
				vector -= rectTransform.rect.position;
				float num = Mathf.Clamp01((vector - this.m_Offset)[(int)this.axis] / rectTransform.rect.size[(int)this.axis]);
				this.normalizedValue = (this.reverseValue ? (1f - num) : num);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000147CA File Offset: 0x000129CA
		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000147E8 File Offset: 0x000129E8
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
			{
				Vector2 vector;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out vector))
				{
					this.m_Offset = vector;
					return;
				}
			}
			else
			{
				this.UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00014872 File Offset: 0x00012A72
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001488C File Offset: 0x00012A8C
		public override void OnMove(AxisEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.axis == Slider.Axis.Horizontal && this.FindSelectableOnLeft() == null)
				{
					this.Set(this.reverseValue ? (this.value + this.stepSize) : (this.value - this.stepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Up:
				if (this.axis == Slider.Axis.Vertical && this.FindSelectableOnUp() == null)
				{
					this.Set(this.reverseValue ? (this.value - this.stepSize) : (this.value + this.stepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Right:
				if (this.axis == Slider.Axis.Horizontal && this.FindSelectableOnRight() == null)
				{
					this.Set(this.reverseValue ? (this.value - this.stepSize) : (this.value + this.stepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Down:
				if (this.axis == Slider.Axis.Vertical && this.FindSelectableOnDown() == null)
				{
					this.Set(this.reverseValue ? (this.value + this.stepSize) : (this.value - this.stepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000149F8 File Offset: 0x00012BF8
		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00014A28 File Offset: 0x00012C28
		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014A58 File Offset: 0x00012C58
		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00014A88 File Offset: 0x00012C88
		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00014AB7 File Offset: 0x00012CB7
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00014AC0 File Offset: 0x00012CC0
		public void SetDirection(Slider.Direction direction, bool includeRectLayouts)
		{
			Slider.Axis axis = this.axis;
			bool reverseValue = this.reverseValue;
			this.direction = direction;
			if (!includeRectLayouts)
			{
				return;
			}
			if (this.axis != axis)
			{
				RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, true, true);
			}
			if (this.reverseValue != reverseValue)
			{
				RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, true, true);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00014B22 File Offset: 0x00012D22
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x0400015A RID: 346
		[SerializeField]
		private RectTransform m_FillRect;

		// Token: 0x0400015B RID: 347
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x0400015C RID: 348
		[Space]
		[SerializeField]
		private Slider.Direction m_Direction;

		// Token: 0x0400015D RID: 349
		[SerializeField]
		private float m_MinValue;

		// Token: 0x0400015E RID: 350
		[SerializeField]
		private float m_MaxValue = 1f;

		// Token: 0x0400015F RID: 351
		[SerializeField]
		private bool m_WholeNumbers;

		// Token: 0x04000160 RID: 352
		[SerializeField]
		protected float m_Value;

		// Token: 0x04000161 RID: 353
		[Space]
		[SerializeField]
		private Slider.SliderEvent m_OnValueChanged = new Slider.SliderEvent();

		// Token: 0x04000162 RID: 354
		private Image m_FillImage;

		// Token: 0x04000163 RID: 355
		private Transform m_FillTransform;

		// Token: 0x04000164 RID: 356
		private RectTransform m_FillContainerRect;

		// Token: 0x04000165 RID: 357
		private Transform m_HandleTransform;

		// Token: 0x04000166 RID: 358
		private RectTransform m_HandleContainerRect;

		// Token: 0x04000167 RID: 359
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04000168 RID: 360
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x04000169 RID: 361
		private bool m_DelayedUpdateVisuals;

		// Token: 0x020000AE RID: 174
		public enum Direction
		{
			// Token: 0x0400030F RID: 783
			LeftToRight,
			// Token: 0x04000310 RID: 784
			RightToLeft,
			// Token: 0x04000311 RID: 785
			BottomToTop,
			// Token: 0x04000312 RID: 786
			TopToBottom
		}

		// Token: 0x020000AF RID: 175
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		// Token: 0x020000B0 RID: 176
		private enum Axis
		{
			// Token: 0x04000314 RID: 788
			Horizontal,
			// Token: 0x04000315 RID: 789
			Vertical
		}
	}
}
