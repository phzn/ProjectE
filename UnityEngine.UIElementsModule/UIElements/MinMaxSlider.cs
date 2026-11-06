using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000E8 RID: 232
	public class MinMaxSlider : BaseField<Vector2>
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001DBFE File Offset: 0x0001BDFE
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0001DC06 File Offset: 0x0001BE06
		internal VisualElement dragElement { get; private set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001DC0F File Offset: 0x0001BE0F
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0001DC17 File Offset: 0x0001BE17
		internal VisualElement dragMinThumb { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001DC20 File Offset: 0x0001BE20
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0001DC28 File Offset: 0x0001BE28
		internal VisualElement dragMaxThumb { get; private set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001DC31 File Offset: 0x0001BE31
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001DC39 File Offset: 0x0001BE39
		internal ClampedDragger<float> clampedDragger { get; private set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001DC44 File Offset: 0x0001BE44
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0001DC61 File Offset: 0x0001BE61
		public float minValue
		{
			get
			{
				return this.value.x;
			}
			set
			{
				base.value = this.ClampValues(new Vector2(value, base.rawValue.y));
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001DC84 File Offset: 0x0001BE84
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0001DCA1 File Offset: 0x0001BEA1
		public float maxValue
		{
			get
			{
				return this.value.y;
			}
			set
			{
				base.value = this.ClampValues(new Vector2(base.rawValue.x, value));
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0001DCC4 File Offset: 0x0001BEC4
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0001DCDC File Offset: 0x0001BEDC
		public override Vector2 value
		{
			get
			{
				return base.value;
			}
			set
			{
				base.value = this.ClampValues(value);
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001DCED File Offset: 0x0001BEED
		public override void SetValueWithoutNotify(Vector2 newValue)
		{
			base.SetValueWithoutNotify(this.ClampValues(newValue));
			this.UpdateDragElementPosition();
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001DD08 File Offset: 0x0001BF08
		public float range
		{
			get
			{
				return Math.Abs(this.highLimit - this.lowLimit);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001DD2C File Offset: 0x0001BF2C
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0001DD44 File Offset: 0x0001BF44
		public float lowLimit
		{
			get
			{
				return this.m_MinLimit;
			}
			set
			{
				bool flag = !Mathf.Approximately(this.m_MinLimit, value);
				if (flag)
				{
					bool flag2 = value > this.m_MaxLimit;
					if (flag2)
					{
						throw new ArgumentException("lowLimit is greater than highLimit");
					}
					this.m_MinLimit = value;
					this.value = base.rawValue;
					this.UpdateDragElementPosition();
					bool flag3 = !string.IsNullOrEmpty(base.viewDataKey);
					if (flag3)
					{
						base.SaveViewData();
					}
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		public float highLimit
		{
			get
			{
				return this.m_MaxLimit;
			}
			set
			{
				bool flag = !Mathf.Approximately(this.m_MaxLimit, value);
				if (flag)
				{
					bool flag2 = value < this.m_MinLimit;
					if (flag2)
					{
						throw new ArgumentException("highLimit is smaller than lowLimit");
					}
					this.m_MaxLimit = value;
					this.value = base.rawValue;
					this.UpdateDragElementPosition();
					bool flag3 = !string.IsNullOrEmpty(base.viewDataKey);
					if (flag3)
					{
						base.SaveViewData();
					}
				}
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001DE3C File Offset: 0x0001C03C
		public MinMaxSlider()
			: this(null, 0f, 10f, float.MinValue, float.MaxValue)
		{
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001DE5B File Offset: 0x0001C05B
		public MinMaxSlider(float minValue, float maxValue, float minLimit, float maxLimit)
			: this(null, minValue, maxValue, minLimit, maxLimit)
		{
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001DE6C File Offset: 0x0001C06C
		public MinMaxSlider(string label, float minValue = 0f, float maxValue = 10f, float minLimit = -3.4028235E+38f, float maxLimit = 3.4028235E+38f)
			: base(label, null)
		{
			this.m_MinLimit = float.MinValue;
			this.m_MaxLimit = float.MaxValue;
			this.lowLimit = minLimit;
			this.highLimit = maxLimit;
			Vector2 vector = this.ClampValues(new Vector2(minValue, maxValue));
			this.minValue = vector.x;
			this.maxValue = vector.y;
			base.AddToClassList(MinMaxSlider.ussClassName);
			base.labelElement.AddToClassList(MinMaxSlider.labelUssClassName);
			base.visualInput.AddToClassList(MinMaxSlider.inputUssClassName);
			base.pickingMode = PickingMode.Ignore;
			this.m_DragState = MinMaxSlider.DragState.NoThumb;
			base.visualInput.pickingMode = PickingMode.Position;
			VisualElement visualElement = new VisualElement
			{
				name = "unity-tracker"
			};
			visualElement.AddToClassList(MinMaxSlider.trackerUssClassName);
			base.visualInput.Add(visualElement);
			this.dragElement = new VisualElement
			{
				name = "unity-dragger"
			};
			this.dragElement.AddToClassList(MinMaxSlider.draggerUssClassName);
			this.dragElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.UpdateDragElementPosition), TrickleDown.NoTrickleDown);
			base.visualInput.Add(this.dragElement);
			this.dragMinThumb = new VisualElement
			{
				name = "unity-thumb-min"
			};
			this.dragMaxThumb = new VisualElement
			{
				name = "unity-thumb-max"
			};
			this.dragMinThumb.AddToClassList(MinMaxSlider.minThumbUssClassName);
			this.dragMaxThumb.AddToClassList(MinMaxSlider.maxThumbUssClassName);
			this.dragElement.Add(this.dragMinThumb);
			this.dragElement.Add(this.dragMaxThumb);
			this.clampedDragger = new ClampedDragger<float>(null, new Action(this.SetSliderValueFromClick), new Action(this.SetSliderValueFromDrag));
			base.visualInput.AddManipulator(this.clampedDragger);
			this.m_MinLimit = minLimit;
			this.m_MaxLimit = maxLimit;
			base.rawValue = this.ClampValues(new Vector2(minValue, maxValue));
			this.UpdateDragElementPosition();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001E074 File Offset: 0x0001C274
		private Vector2 ClampValues(Vector2 valueToClamp)
		{
			bool flag = this.m_MinLimit > this.m_MaxLimit;
			if (flag)
			{
				this.m_MinLimit = this.m_MaxLimit;
			}
			Vector2 vector = default(Vector2);
			bool flag2 = valueToClamp.y > this.m_MaxLimit;
			if (flag2)
			{
				valueToClamp.y = this.m_MaxLimit;
			}
			vector.x = Mathf.Clamp(valueToClamp.x, this.m_MinLimit, valueToClamp.y);
			vector.y = Mathf.Clamp(valueToClamp.y, valueToClamp.x, this.m_MaxLimit);
			return vector;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001E10C File Offset: 0x0001C30C
		private void UpdateDragElementPosition(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				this.UpdateDragElementPosition();
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001E14C File Offset: 0x0001C34C
		private void UpdateDragElementPosition()
		{
			bool flag = base.panel == null;
			if (!flag)
			{
				float num = -this.dragElement.resolvedStyle.marginLeft - this.dragElement.resolvedStyle.marginRight;
				int num2 = this.dragElement.resolvedStyle.unitySliceLeft + this.dragElement.resolvedStyle.unitySliceRight;
				float num3 = Mathf.Round(this.SliderLerpUnclamped((float)this.dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width + num - (float)this.dragElement.resolvedStyle.unitySliceRight, this.SliderNormalizeValue(this.minValue, this.lowLimit, this.highLimit)) - (float)this.dragElement.resolvedStyle.unitySliceLeft);
				float num4 = Mathf.Round(this.SliderLerpUnclamped((float)this.dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width + num - (float)this.dragElement.resolvedStyle.unitySliceRight, this.SliderNormalizeValue(this.maxValue, this.lowLimit, this.highLimit)) + (float)this.dragElement.resolvedStyle.unitySliceRight);
				this.dragElement.style.width = Mathf.Max((float)num2, num4 - num3);
				this.dragElement.style.left = num3;
				this.UpdateDragThumbsRect();
				this.dragMaxThumb.style.left = this.dragElement.resolvedStyle.width - (float)this.dragElement.resolvedStyle.unitySliceRight;
				this.dragMaxThumb.style.top = 0f;
				this.dragMinThumb.style.width = this.m_DragMinThumbRect.width;
				this.dragMinThumb.style.height = this.m_DragMinThumbRect.height;
				this.dragMinThumb.style.left = 0f;
				this.dragMinThumb.style.top = 0f;
				this.dragMaxThumb.style.width = this.m_DragMaxThumbRect.width;
				this.dragMaxThumb.style.height = this.m_DragMaxThumbRect.height;
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001E3DC File Offset: 0x0001C5DC
		private void UpdateDragThumbsRect()
		{
			float left = this.dragElement.resolvedStyle.left;
			float num = this.dragElement.resolvedStyle.left + (this.dragElement.resolvedStyle.width - (float)this.dragElement.resolvedStyle.unitySliceRight);
			float num2 = this.dragElement.layout.yMin + this.dragMinThumb.resolvedStyle.marginTop;
			float num3 = this.dragElement.layout.yMin + this.dragMaxThumb.resolvedStyle.marginTop;
			float num4 = Mathf.Max(this.dragElement.resolvedStyle.height, this.dragMinThumb.resolvedStyle.height);
			float num5 = Mathf.Max(this.dragElement.resolvedStyle.height, this.dragMaxThumb.resolvedStyle.height);
			this.m_DragMinThumbRect = new Rect(left, num2, (float)this.dragElement.resolvedStyle.unitySliceLeft, num4);
			this.m_DragMaxThumbRect = new Rect(num, num3, (float)this.dragElement.resolvedStyle.unitySliceRight, num5);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001E50C File Offset: 0x0001C70C
		internal float SliderLerpUnclamped(float a, float b, float interpolant)
		{
			return Mathf.LerpUnclamped(a, b, interpolant);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0001E528 File Offset: 0x0001C728
		internal float SliderNormalizeValue(float currentValue, float lowerValue, float higherValue)
		{
			return (currentValue - lowerValue) / (higherValue - lowerValue);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001E544 File Offset: 0x0001C744
		private float ComputeValueFromPosition(float positionToConvert)
		{
			float num = this.SliderNormalizeValue(positionToConvert, (float)this.dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width - (float)this.dragElement.resolvedStyle.unitySliceRight);
			return this.SliderLerpUnclamped(this.lowLimit, this.highLimit, num);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001E5A8 File Offset: 0x0001C7A8
		[EventInterest(new Type[] { typeof(GeometryChangedEvent) })]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag = evt == null;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<GeometryChangedEvent>.TypeId();
				if (flag2)
				{
					this.UpdateDragElementPosition((GeometryChangedEvent)evt);
				}
			}
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001E5EC File Offset: 0x0001C7EC
		private void SetSliderValueFromDrag()
		{
			bool flag = this.clampedDragger.dragDirection != ClampedDragger<float>.DragDirection.Free;
			if (!flag)
			{
				float x = this.m_DragElementStartPos.x;
				float num = x + this.clampedDragger.delta.x;
				this.ComputeValueFromDraggingThumb(x, num);
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001E63C File Offset: 0x0001C83C
		private void SetSliderValueFromClick()
		{
			bool flag = this.clampedDragger.dragDirection == ClampedDragger<float>.DragDirection.Free;
			if (!flag)
			{
				this.UpdateDragThumbsRect();
				bool flag2 = this.m_DragMinThumbRect.Contains(this.clampedDragger.startMousePosition);
				if (flag2)
				{
					this.m_DragState = MinMaxSlider.DragState.MinThumb;
				}
				else
				{
					bool flag3 = this.m_DragMaxThumbRect.Contains(this.clampedDragger.startMousePosition);
					if (flag3)
					{
						this.m_DragState = MinMaxSlider.DragState.MaxThumb;
					}
					else
					{
						bool flag4 = this.clampedDragger.startMousePosition.x > this.dragElement.layout.xMin && this.clampedDragger.startMousePosition.x < this.dragElement.layout.xMax;
						if (flag4)
						{
							this.m_DragState = MinMaxSlider.DragState.MiddleThumb;
						}
						else
						{
							this.m_DragState = MinMaxSlider.DragState.NoThumb;
						}
					}
				}
				bool flag5 = this.m_DragState == MinMaxSlider.DragState.NoThumb;
				if (flag5)
				{
					float num = this.ComputeValueFromPosition(this.clampedDragger.startMousePosition.x);
					bool flag6 = this.clampedDragger.startMousePosition.x < this.dragElement.layout.x;
					if (flag6)
					{
						this.m_DragState = MinMaxSlider.DragState.MinThumb;
						this.value = new Vector2(num, this.value.y);
					}
					else
					{
						this.m_DragState = MinMaxSlider.DragState.MaxThumb;
						this.value = new Vector2(this.value.x, num);
					}
				}
				this.m_ValueStartPos = this.value;
				this.clampedDragger.dragDirection = ClampedDragger<float>.DragDirection.Free;
				this.m_DragElementStartPos = this.clampedDragger.startMousePosition;
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
		private void ComputeValueFromDraggingThumb(float dragElementStartPos, float dragElementEndPos)
		{
			float num = this.ComputeValueFromPosition(dragElementStartPos);
			float num2 = this.ComputeValueFromPosition(dragElementEndPos);
			float num3 = num2 - num;
			switch (this.m_DragState)
			{
			case MinMaxSlider.DragState.MinThumb:
			{
				float num4 = this.m_ValueStartPos.x + num3;
				bool flag = num4 > this.maxValue;
				if (flag)
				{
					num4 = this.maxValue;
				}
				else
				{
					bool flag2 = num4 < this.lowLimit;
					if (flag2)
					{
						num4 = this.lowLimit;
					}
				}
				this.value = new Vector2(num4, this.maxValue);
				break;
			}
			case MinMaxSlider.DragState.MiddleThumb:
			{
				Vector2 value = this.value;
				value.x = this.m_ValueStartPos.x + num3;
				value.y = this.m_ValueStartPos.y + num3;
				float num5 = this.m_ValueStartPos.y - this.m_ValueStartPos.x;
				bool flag3 = value.x < this.lowLimit;
				if (flag3)
				{
					value.x = this.lowLimit;
					value.y = this.lowLimit + num5;
				}
				else
				{
					bool flag4 = value.y > this.highLimit;
					if (flag4)
					{
						value.y = this.highLimit;
						value.x = this.highLimit - num5;
					}
				}
				this.value = value;
				break;
			}
			case MinMaxSlider.DragState.MaxThumb:
			{
				float num6 = this.m_ValueStartPos.y + num3;
				bool flag5 = num6 < this.minValue;
				if (flag5)
				{
					num6 = this.minValue;
				}
				else
				{
					bool flag6 = num6 > this.highLimit;
					if (flag6)
					{
						num6 = this.highLimit;
					}
				}
				this.value = new Vector2(this.minValue, num6);
				break;
			}
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected override void UpdateMixedValueContent()
		{
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001E9A4 File Offset: 0x0001CBA4
		internal override void RegisterEditingCallbacks()
		{
			base.visualInput.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			base.visualInput.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001E9D9 File Offset: 0x0001CBD9
		internal override void UnregisterEditingCallbacks()
		{
			base.visualInput.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			base.visualInput.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0400035E RID: 862
		private Vector2 m_DragElementStartPos;

		// Token: 0x0400035F RID: 863
		private Vector2 m_ValueStartPos;

		// Token: 0x04000360 RID: 864
		private Rect m_DragMinThumbRect;

		// Token: 0x04000361 RID: 865
		private Rect m_DragMaxThumbRect;

		// Token: 0x04000362 RID: 866
		private MinMaxSlider.DragState m_DragState;

		// Token: 0x04000363 RID: 867
		private float m_MinLimit;

		// Token: 0x04000364 RID: 868
		private float m_MaxLimit;

		// Token: 0x04000365 RID: 869
		internal const float kDefaultHighValue = 10f;

		// Token: 0x04000366 RID: 870
		public new static readonly string ussClassName = "unity-min-max-slider";

		// Token: 0x04000367 RID: 871
		public new static readonly string labelUssClassName = MinMaxSlider.ussClassName + "__label";

		// Token: 0x04000368 RID: 872
		public new static readonly string inputUssClassName = MinMaxSlider.ussClassName + "__input";

		// Token: 0x04000369 RID: 873
		public static readonly string trackerUssClassName = MinMaxSlider.ussClassName + "__tracker";

		// Token: 0x0400036A RID: 874
		public static readonly string draggerUssClassName = MinMaxSlider.ussClassName + "__dragger";

		// Token: 0x0400036B RID: 875
		public static readonly string minThumbUssClassName = MinMaxSlider.ussClassName + "__min-thumb";

		// Token: 0x0400036C RID: 876
		public static readonly string maxThumbUssClassName = MinMaxSlider.ussClassName + "__max-thumb";

		// Token: 0x020000E9 RID: 233
		public new class UxmlFactory : UxmlFactory<MinMaxSlider, MinMaxSlider.UxmlTraits>
		{
		}

		// Token: 0x020000EA RID: 234
		public new class UxmlTraits : BaseField<Vector2>.UxmlTraits
		{
			// Token: 0x060007F9 RID: 2041 RVA: 0x0001EAA8 File Offset: 0x0001CCA8
			public UxmlTraits()
			{
				this.m_PickingMode.defaultValue = PickingMode.Ignore;
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x0001EB58 File Offset: 0x0001CD58
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				MinMaxSlider minMaxSlider = (MinMaxSlider)ve;
				minMaxSlider.lowLimit = this.m_LowLimit.GetValueFromBag(bag, cc);
				minMaxSlider.highLimit = this.m_HighLimit.GetValueFromBag(bag, cc);
				Vector2 vector = new Vector2(this.m_MinValue.GetValueFromBag(bag, cc), this.m_MaxValue.GetValueFromBag(bag, cc));
				minMaxSlider.value = vector;
			}

			// Token: 0x0400036D RID: 877
			private UxmlFloatAttributeDescription m_MinValue = new UxmlFloatAttributeDescription
			{
				name = "min-value",
				defaultValue = 0f
			};

			// Token: 0x0400036E RID: 878
			private UxmlFloatAttributeDescription m_MaxValue = new UxmlFloatAttributeDescription
			{
				name = "max-value",
				defaultValue = 10f
			};

			// Token: 0x0400036F RID: 879
			private UxmlFloatAttributeDescription m_LowLimit = new UxmlFloatAttributeDescription
			{
				name = "low-limit",
				defaultValue = float.MinValue
			};

			// Token: 0x04000370 RID: 880
			private UxmlFloatAttributeDescription m_HighLimit = new UxmlFloatAttributeDescription
			{
				name = "high-limit",
				defaultValue = float.MaxValue
			};
		}

		// Token: 0x020000EB RID: 235
		private enum DragState
		{
			// Token: 0x04000372 RID: 882
			NoThumb,
			// Token: 0x04000373 RID: 883
			MinThumb,
			// Token: 0x04000374 RID: 884
			MiddleThumb,
			// Token: 0x04000375 RID: 885
			MaxThumb
		}
	}
}
