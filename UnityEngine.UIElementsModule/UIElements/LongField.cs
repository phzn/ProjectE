using System;
using System.Globalization;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000E4 RID: 228
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class LongField : TextValueField<long>
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0001D925 File Offset: 0x0001BB25
		private LongField.LongInput longInput
		{
			get
			{
				return (LongField.LongInput)base.textInputBase;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001D934 File Offset: 0x0001BB34
		protected override string ValueToString(long v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001D960 File Offset: 0x0001BB60
		protected override long StringToValue(string str)
		{
			long num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToLong(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001D9A7 File Offset: 0x0001BBA7
		public LongField()
			: this(null, 1000)
		{
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001D9B7 File Offset: 0x0001BBB7
		public LongField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001D9C4 File Offset: 0x0001BBC4
		public LongField(string label, int maxLength = 1000)
			: base(label, maxLength, new LongField.LongInput())
		{
			base.AddToClassList(LongField.ussClassName);
			base.labelElement.AddToClassList(LongField.labelUssClassName);
			base.visualInput.AddToClassList(LongField.inputUssClassName);
			base.AddLabelDragger<long>();
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001DA18 File Offset: 0x0001BC18
		internal override bool CanTryParse(string textString)
		{
			long num;
			return long.TryParse(textString, out num);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001DA2D File Offset: 0x0001BC2D
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, long startValue)
		{
			this.longInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x04000357 RID: 855
		public new static readonly string ussClassName = "unity-long-field";

		// Token: 0x04000358 RID: 856
		public new static readonly string labelUssClassName = LongField.ussClassName + "__label";

		// Token: 0x04000359 RID: 857
		public new static readonly string inputUssClassName = LongField.ussClassName + "__input";

		// Token: 0x020000E5 RID: 229
		public new class UxmlFactory : UxmlFactory<LongField, LongField.UxmlTraits>
		{
		}

		// Token: 0x020000E6 RID: 230
		public new class UxmlTraits : TextValueFieldTraits<long, UxmlLongAttributeDescription>
		{
		}

		// Token: 0x020000E7 RID: 231
		private class LongInput : TextValueField<long>.TextValueInput
		{
			// Token: 0x17000169 RID: 361
			// (get) Token: 0x060007CB RID: 1995 RVA: 0x0001DA85 File Offset: 0x0001BC85
			private LongField parentLongField
			{
				get
				{
					return (LongField)base.parent;
				}
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x0001DA92 File Offset: 0x0001BC92
			internal LongInput()
			{
				base.formatString = UINumericFieldsUtils.k_IntFieldFormatString;
			}

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x060007CD RID: 1997 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForInt;
				}
			}

			// Token: 0x060007CE RID: 1998 RVA: 0x0001DAC0 File Offset: 0x0001BCC0
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, long startValue)
			{
				double num = (double)NumericFieldDraggerUtility.CalculateIntDragSensitivity(startValue);
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				long num3 = this.StringToValue(base.text);
				long num4 = (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num);
				num3 = this.ClampMinMaxLongValue(num4, num3);
				bool isDelayed = this.parentLongField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(num3);
				}
				else
				{
					this.parentLongField.value = num3;
				}
			}

			// Token: 0x060007CF RID: 1999 RVA: 0x0001DB44 File Offset: 0x0001BD44
			private long ClampMinMaxLongValue(long niceDelta, long value)
			{
				long num = Math.Abs(niceDelta);
				bool flag = niceDelta > 0L;
				long num2;
				if (flag)
				{
					bool flag2 = value > 0L && num > long.MaxValue - value;
					if (flag2)
					{
						num2 = long.MaxValue;
					}
					else
					{
						num2 = value + niceDelta;
					}
				}
				else
				{
					bool flag3 = value < 0L && value < long.MinValue + num;
					if (flag3)
					{
						num2 = long.MinValue;
					}
					else
					{
						num2 = value - num;
					}
				}
				return num2;
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x0001DBC0 File Offset: 0x0001BDC0
			protected override string ValueToString(long v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x0001DBE0 File Offset: 0x0001BDE0
			protected override long StringToValue(string str)
			{
				return this.parentLongField.StringToValue(str);
			}
		}
	}
}
