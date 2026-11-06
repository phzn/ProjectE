using System;
using System.Globalization;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000A1 RID: 161
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class DoubleField : TextValueField<double>
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00016816 File Offset: 0x00014A16
		private DoubleField.DoubleInput doubleInput
		{
			get
			{
				return (DoubleField.DoubleInput)base.textInputBase;
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00016824 File Offset: 0x00014A24
		protected override string ValueToString(double v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016850 File Offset: 0x00014A50
		protected override double StringToValue(string str)
		{
			double num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToDouble(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00016897 File Offset: 0x00014A97
		public DoubleField()
			: this(null, 1000)
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000168A7 File Offset: 0x00014AA7
		public DoubleField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000168B4 File Offset: 0x00014AB4
		public DoubleField(string label, int maxLength = 1000)
			: base(label, maxLength, new DoubleField.DoubleInput())
		{
			base.AddToClassList(DoubleField.ussClassName);
			base.labelElement.AddToClassList(DoubleField.labelUssClassName);
			base.visualInput.AddToClassList(DoubleField.inputUssClassName);
			base.AddLabelDragger<double>();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00016908 File Offset: 0x00014B08
		internal override bool CanTryParse(string textString)
		{
			double num;
			return double.TryParse(textString, out num);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001691D File Offset: 0x00014B1D
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, double startValue)
		{
			this.doubleInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x0400028A RID: 650
		public new static readonly string ussClassName = "unity-double-field";

		// Token: 0x0400028B RID: 651
		public new static readonly string labelUssClassName = DoubleField.ussClassName + "__label";

		// Token: 0x0400028C RID: 652
		public new static readonly string inputUssClassName = DoubleField.ussClassName + "__input";

		// Token: 0x020000A2 RID: 162
		public new class UxmlFactory : UxmlFactory<DoubleField, DoubleField.UxmlTraits>
		{
		}

		// Token: 0x020000A3 RID: 163
		public new class UxmlTraits : TextValueFieldTraits<double, UxmlDoubleAttributeDescription>
		{
		}

		// Token: 0x020000A4 RID: 164
		private class DoubleInput : TextValueField<double>.TextValueInput
		{
			// Token: 0x170000FC RID: 252
			// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00016975 File Offset: 0x00014B75
			private DoubleField parentDoubleField
			{
				get
				{
					return (DoubleField)base.parent;
				}
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00016982 File Offset: 0x00014B82
			internal DoubleInput()
			{
				base.formatString = UINumericFieldsUtils.k_DoubleFieldFormatString;
			}

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00016998 File Offset: 0x00014B98
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForFloat;
				}
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x000169A0 File Offset: 0x00014BA0
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, double startValue)
			{
				double num = NumericFieldDraggerUtility.CalculateFloatDragSensitivity(startValue);
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				double num3 = this.StringToValue(base.text);
				num3 += (double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num;
				num3 = Mathf.RoundBasedOnMinimumDifference(num3, num);
				bool isDelayed = this.parentDoubleField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(num3);
				}
				else
				{
					this.parentDoubleField.value = num3;
				}
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x00016A1C File Offset: 0x00014C1C
			protected override string ValueToString(double v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x00016A3C File Offset: 0x00014C3C
			protected override double StringToValue(string str)
			{
				return this.parentDoubleField.StringToValue(str);
			}
		}
	}
}
