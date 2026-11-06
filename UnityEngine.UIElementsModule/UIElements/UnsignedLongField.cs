using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x0200014C RID: 332
	public class UnsignedLongField : TextValueField<ulong>
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002BE8A File Offset: 0x0002A08A
		private UnsignedLongField.UnsignedLongInput unsignedLongInput
		{
			get
			{
				return (UnsignedLongField.UnsignedLongInput)base.textInputBase;
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002BE98 File Offset: 0x0002A098
		protected override string ValueToString(ulong v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		protected override ulong StringToValue(string str)
		{
			ulong num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToULong(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002BF0B File Offset: 0x0002A10B
		public UnsignedLongField()
			: this(null, 1000)
		{
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002BF1B File Offset: 0x0002A11B
		public UnsignedLongField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002BF28 File Offset: 0x0002A128
		public UnsignedLongField(string label, int maxLength = 1000)
			: base(label, maxLength, new UnsignedLongField.UnsignedLongInput())
		{
			base.AddToClassList(UnsignedLongField.ussClassName);
			base.labelElement.AddToClassList(UnsignedLongField.labelUssClassName);
			base.visualInput.AddToClassList(UnsignedLongField.inputUssClassName);
			base.AddLabelDragger<ulong>();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002BF7C File Offset: 0x0002A17C
		internal override bool CanTryParse(string textString)
		{
			ulong num;
			return ulong.TryParse(textString, out num);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002BF91 File Offset: 0x0002A191
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, ulong startValue)
		{
			this.unsignedLongInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x04000533 RID: 1331
		public new static readonly string ussClassName = "unity-unsigned-long-field";

		// Token: 0x04000534 RID: 1332
		public new static readonly string labelUssClassName = UnsignedLongField.ussClassName + "__label";

		// Token: 0x04000535 RID: 1333
		public new static readonly string inputUssClassName = UnsignedLongField.ussClassName + "__input";

		// Token: 0x0200014D RID: 333
		public new class UxmlFactory : UxmlFactory<UnsignedLongField, UnsignedLongField.UxmlTraits>
		{
		}

		// Token: 0x0200014E RID: 334
		public new class UxmlTraits : TextValueFieldTraits<ulong, UxmlUnsignedLongAttributeDescription>
		{
		}

		// Token: 0x0200014F RID: 335
		private class UnsignedLongInput : TextValueField<ulong>.TextValueInput
		{
			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0002BFE9 File Offset: 0x0002A1E9
			private UnsignedLongField parentUnsignedLongField
			{
				get
				{
					return (UnsignedLongField)base.parent;
				}
			}

			// Token: 0x06000AE5 RID: 2789 RVA: 0x0002BFF6 File Offset: 0x0002A1F6
			internal UnsignedLongInput()
			{
				base.formatString = UINumericFieldsUtils.k_IntFieldFormatString;
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0001D6DC File Offset: 0x0001B8DC
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForInt;
				}
			}

			// Token: 0x06000AE7 RID: 2791 RVA: 0x0002C00C File Offset: 0x0002A20C
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, ulong startValue)
			{
				double num = NumericFieldDraggerUtility.CalculateIntDragSensitivity(startValue);
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				ulong num3 = this.StringToValue(base.text);
				long num4 = (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num);
				num3 = this.ClampToMinMaxULongValue(num4, num3);
				bool isDelayed = this.parentUnsignedLongField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(num3);
				}
				else
				{
					this.parentUnsignedLongField.value = num3;
				}
			}

			// Token: 0x06000AE8 RID: 2792 RVA: 0x0002C090 File Offset: 0x0002A290
			private ulong ClampToMinMaxULongValue(long niceDelta, ulong value)
			{
				ulong num = (ulong)Math.Abs(niceDelta);
				bool flag = niceDelta > 0L;
				ulong num2;
				if (flag)
				{
					bool flag2 = num > ulong.MaxValue - value;
					if (flag2)
					{
						num2 = ulong.MaxValue;
					}
					else
					{
						num2 = value + num;
					}
				}
				else
				{
					bool flag3 = num > value;
					if (flag3)
					{
						num2 = 0UL;
					}
					else
					{
						num2 = value - num;
					}
				}
				return num2;
			}

			// Token: 0x06000AE9 RID: 2793 RVA: 0x0002C0E0 File Offset: 0x0002A2E0
			protected override string ValueToString(ulong v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x06000AEA RID: 2794 RVA: 0x0002C100 File Offset: 0x0002A300
			protected override ulong StringToValue(string str)
			{
				return this.parentUnsignedLongField.StringToValue(str);
			}
		}
	}
}
