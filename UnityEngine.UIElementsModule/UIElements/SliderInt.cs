using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200012D RID: 301
	public class SliderInt : BaseSlider<int>
	{
		// Token: 0x06000A07 RID: 2567 RVA: 0x00028406 File Offset: 0x00026606
		public SliderInt()
			: this(null, 0, 10, SliderDirection.Horizontal, 0f)
		{
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0002841A File Offset: 0x0002661A
		public SliderInt(int start, int end, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
			: this(null, start, end, direction, pageSize)
		{
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0002842A File Offset: 0x0002662A
		public SliderInt(string label, int start = 0, int end = 10, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
			: base(label, start, end, direction, pageSize)
		{
			base.AddToClassList(SliderInt.ussClassName);
			base.labelElement.AddToClassList(SliderInt.labelUssClassName);
			base.visualInput.AddToClassList(SliderInt.inputUssClassName);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002846C File Offset: 0x0002666C
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00028484 File Offset: 0x00026684
		public override float pageSize
		{
			get
			{
				return base.pageSize;
			}
			set
			{
				base.pageSize = (float)Mathf.RoundToInt(value);
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00028498 File Offset: 0x00026698
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, int startValue)
		{
			double num = (double)NumericFieldDraggerUtility.CalculateIntDragSensitivity((long)startValue, (long)base.lowValue, (long)base.highValue);
			float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
			long num3 = (long)this.value;
			num3 += (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num);
			this.value = (int)num3;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000284F4 File Offset: 0x000266F4
		internal override int SliderLerpUnclamped(int a, int b, float interpolant)
		{
			return Mathf.RoundToInt(Mathf.LerpUnclamped((float)a, (float)b, interpolant));
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00028518 File Offset: 0x00026718
		internal override float SliderNormalizeValue(int currentValue, int lowerValue, int higherValue)
		{
			return ((float)currentValue - (float)lowerValue) / ((float)higherValue - (float)lowerValue);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00028538 File Offset: 0x00026738
		internal override int SliderRange()
		{
			return Math.Abs(base.highValue - base.lowValue);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0002855C File Offset: 0x0002675C
		internal override int ParseStringToValue(string previousValue, string newValue)
		{
			int num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToInt(newValue, previousValue, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : 0;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00028594 File Offset: 0x00026794
		internal override void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
		{
			bool flag = Mathf.Approximately(this.pageSize, 0f);
			if (flag)
			{
				base.ComputeValueAndDirectionFromClick(sliderLength, dragElementLength, dragElementPos, dragElementLastPos);
			}
			else
			{
				float num = sliderLength - dragElementLength;
				bool flag2 = Mathf.Abs(num) < 1E-30f;
				if (!flag2)
				{
					int num2 = (int)this.pageSize;
					bool flag3 = (base.lowValue > base.highValue && !base.inverted) || (base.lowValue < base.highValue && base.inverted) || (base.direction == SliderDirection.Vertical && !base.inverted);
					if (flag3)
					{
						num2 = -num2;
					}
					bool flag4 = dragElementLastPos < dragElementPos;
					bool flag5 = dragElementLastPos > dragElementPos + dragElementLength;
					bool flag6 = (base.inverted ? flag5 : flag4);
					bool flag7 = (base.inverted ? flag4 : flag5);
					bool flag8 = flag6 && base.clampedDragger.dragDirection != ClampedDragger<int>.DragDirection.LowToHigh;
					if (flag8)
					{
						base.clampedDragger.dragDirection = ClampedDragger<int>.DragDirection.HighToLow;
						this.value -= num2;
					}
					else
					{
						bool flag9 = flag7 && base.clampedDragger.dragDirection != ClampedDragger<int>.DragDirection.HighToLow;
						if (flag9)
						{
							base.clampedDragger.dragDirection = ClampedDragger<int>.DragDirection.LowToHigh;
							this.value += num2;
						}
					}
				}
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000286E8 File Offset: 0x000268E8
		internal override void ComputeValueFromKey(BaseSlider<int>.SliderKey sliderKey, bool isShift)
		{
			if (sliderKey != BaseSlider<int>.SliderKey.None)
			{
				if (sliderKey != BaseSlider<int>.SliderKey.Lowest)
				{
					if (sliderKey != BaseSlider<int>.SliderKey.Highest)
					{
						bool flag = sliderKey == BaseSlider<int>.SliderKey.LowerPage || sliderKey == BaseSlider<int>.SliderKey.HigherPage;
						float num = BaseSlider<int>.GetClosestPowerOfTen(Mathf.Abs((float)(base.highValue - base.lowValue) * 0.01f));
						bool flag2 = num < 1f;
						if (flag2)
						{
							num = 1f;
						}
						bool flag3 = flag;
						if (flag3)
						{
							num *= this.pageSize;
						}
						else if (isShift)
						{
							num *= 10f;
						}
						bool flag4 = sliderKey == BaseSlider<int>.SliderKey.Lower || sliderKey == BaseSlider<int>.SliderKey.LowerPage;
						if (flag4)
						{
							num = -num;
						}
						this.value = Mathf.RoundToInt(BaseSlider<int>.RoundToMultipleOf((float)this.value + num * 0.5001f, Mathf.Abs(num)));
					}
					else
					{
						this.value = base.highValue;
					}
				}
				else
				{
					this.value = base.lowValue;
				}
			}
		}

		// Token: 0x040004D0 RID: 1232
		internal const int kDefaultHighValue = 10;

		// Token: 0x040004D1 RID: 1233
		public new static readonly string ussClassName = "unity-slider-int";

		// Token: 0x040004D2 RID: 1234
		public new static readonly string labelUssClassName = SliderInt.ussClassName + "__label";

		// Token: 0x040004D3 RID: 1235
		public new static readonly string inputUssClassName = SliderInt.ussClassName + "__input";

		// Token: 0x0200012E RID: 302
		public new class UxmlFactory : UxmlFactory<SliderInt, SliderInt.UxmlTraits>
		{
		}

		// Token: 0x0200012F RID: 303
		public new class UxmlTraits : BaseSlider<int>.UxmlTraits
		{
			// Token: 0x06000A15 RID: 2581 RVA: 0x0002880C File Offset: 0x00026A0C
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				SliderInt sliderInt = (SliderInt)ve;
				sliderInt.lowValue = this.m_LowValue.GetValueFromBag(bag, cc);
				sliderInt.highValue = this.m_HighValue.GetValueFromBag(bag, cc);
				sliderInt.direction = this.m_Direction.GetValueFromBag(bag, cc);
				sliderInt.pageSize = (float)this.m_PageSize.GetValueFromBag(bag, cc);
				sliderInt.showInputField = this.m_ShowInputField.GetValueFromBag(bag, cc);
				sliderInt.inverted = this.m_Inverted.GetValueFromBag(bag, cc);
				base.Init(ve, bag, cc);
			}

			// Token: 0x040004D4 RID: 1236
			private UxmlIntAttributeDescription m_LowValue = new UxmlIntAttributeDescription
			{
				name = "low-value"
			};

			// Token: 0x040004D5 RID: 1237
			private UxmlIntAttributeDescription m_HighValue = new UxmlIntAttributeDescription
			{
				name = "high-value",
				defaultValue = 10
			};

			// Token: 0x040004D6 RID: 1238
			private UxmlIntAttributeDescription m_PageSize = new UxmlIntAttributeDescription
			{
				name = "page-size",
				defaultValue = 0
			};

			// Token: 0x040004D7 RID: 1239
			private UxmlBoolAttributeDescription m_ShowInputField = new UxmlBoolAttributeDescription
			{
				name = "show-input-field",
				defaultValue = false
			};

			// Token: 0x040004D8 RID: 1240
			private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
			{
				name = "direction",
				defaultValue = SliderDirection.Horizontal
			};

			// Token: 0x040004D9 RID: 1241
			private UxmlBoolAttributeDescription m_Inverted = new UxmlBoolAttributeDescription
			{
				name = "inverted",
				defaultValue = false
			};
		}
	}
}
