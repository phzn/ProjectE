using System;
using System.Globalization;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000AD RID: 173
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class FloatField : TextValueField<float>
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000172B2 File Offset: 0x000154B2
		private FloatField.FloatInput floatInput
		{
			get
			{
				return (FloatField.FloatInput)base.textInputBase;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000172C0 File Offset: 0x000154C0
		protected override string ValueToString(float v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000172EC File Offset: 0x000154EC
		protected override float StringToValue(string str)
		{
			float num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToFloat(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00017333 File Offset: 0x00015533
		public FloatField()
			: this(null, 1000)
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00017343 File Offset: 0x00015543
		public FloatField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00017350 File Offset: 0x00015550
		public FloatField(string label, int maxLength = 1000)
			: base(label, maxLength, new FloatField.FloatInput())
		{
			base.AddToClassList(FloatField.ussClassName);
			base.labelElement.AddToClassList(FloatField.labelUssClassName);
			base.visualInput.AddToClassList(FloatField.inputUssClassName);
			base.AddLabelDragger<float>();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000173A4 File Offset: 0x000155A4
		internal override bool CanTryParse(string textString)
		{
			float num;
			return float.TryParse(textString, out num);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000173B9 File Offset: 0x000155B9
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, float startValue)
		{
			this.floatInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x040002A2 RID: 674
		public new static readonly string ussClassName = "unity-float-field";

		// Token: 0x040002A3 RID: 675
		public new static readonly string labelUssClassName = FloatField.ussClassName + "__label";

		// Token: 0x040002A4 RID: 676
		public new static readonly string inputUssClassName = FloatField.ussClassName + "__input";

		// Token: 0x020000AE RID: 174
		public new class UxmlFactory : UxmlFactory<FloatField, FloatField.UxmlTraits>
		{
		}

		// Token: 0x020000AF RID: 175
		public new class UxmlTraits : TextValueFieldTraits<float, UxmlFloatAttributeDescription>
		{
		}

		// Token: 0x020000B0 RID: 176
		private class FloatInput : TextValueField<float>.TextValueInput
		{
			// Token: 0x17000102 RID: 258
			// (get) Token: 0x0600062A RID: 1578 RVA: 0x00017411 File Offset: 0x00015611
			private FloatField parentFloatField
			{
				get
				{
					return (FloatField)base.parent;
				}
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x0001741E File Offset: 0x0001561E
			internal FloatInput()
			{
				base.formatString = UINumericFieldsUtils.k_FloatFieldFormatString;
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x0600062C RID: 1580 RVA: 0x00016998 File Offset: 0x00014B98
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForFloat;
				}
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x00017434 File Offset: 0x00015634
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, float startValue)
			{
				double num = NumericFieldDraggerUtility.CalculateFloatDragSensitivity((double)startValue);
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				double num3 = (double)this.StringToValue(base.text);
				num3 += (double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num;
				num3 = Mathf.RoundBasedOnMinimumDifference(num3, num);
				bool isDelayed = this.parentFloatField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(Mathf.ClampToFloat(num3));
				}
				else
				{
					this.parentFloatField.value = Mathf.ClampToFloat(num3);
				}
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x000174BC File Offset: 0x000156BC
			protected override string ValueToString(float v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x000174DC File Offset: 0x000156DC
			protected override float StringToValue(string str)
			{
				return this.parentFloatField.StringToValue(str);
			}
		}
	}
}
