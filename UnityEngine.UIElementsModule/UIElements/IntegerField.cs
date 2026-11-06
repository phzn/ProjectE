using System;
using System.Globalization;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000DA RID: 218
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class IntegerField : TextValueField<int>
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001D55B File Offset: 0x0001B75B
		private IntegerField.IntegerInput integerInput
		{
			get
			{
				return (IntegerField.IntegerInput)base.textInputBase;
			}
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001D568 File Offset: 0x0001B768
		protected override string ValueToString(int v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001D594 File Offset: 0x0001B794
		protected override int StringToValue(string str)
		{
			int num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToInt(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001D5DB File Offset: 0x0001B7DB
		public IntegerField()
			: this(null, 1000)
		{
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001D5EB File Offset: 0x0001B7EB
		public IntegerField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001D5F8 File Offset: 0x0001B7F8
		public IntegerField(string label, int maxLength = 1000)
			: base(label, maxLength, new IntegerField.IntegerInput())
		{
			base.AddToClassList(IntegerField.ussClassName);
			base.labelElement.AddToClassList(IntegerField.labelUssClassName);
			base.visualInput.AddToClassList(IntegerField.inputUssClassName);
			base.AddLabelDragger<int>();
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001D64C File Offset: 0x0001B84C
		internal override bool CanTryParse(string textString)
		{
			int num;
			return int.TryParse(textString, out num);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001D661 File Offset: 0x0001B861
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, int startValue)
		{
			this.integerInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x0400034F RID: 847
		public new static readonly string ussClassName = "unity-integer-field";

		// Token: 0x04000350 RID: 848
		public new static readonly string labelUssClassName = IntegerField.ussClassName + "__label";

		// Token: 0x04000351 RID: 849
		public new static readonly string inputUssClassName = IntegerField.ussClassName + "__input";

		// Token: 0x020000DB RID: 219
		public new class UxmlFactory : UxmlFactory<IntegerField, IntegerField.UxmlTraits>
		{
		}

		// Token: 0x020000DC RID: 220
		public new class UxmlTraits : TextValueFieldTraits<int, UxmlIntAttributeDescription>
		{
		}

		// Token: 0x020000DD RID: 221
		private class IntegerInput : TextValueField<int>.TextValueInput
		{
			// Token: 0x17000162 RID: 354
			// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001D6B9 File Offset: 0x0001B8B9
			private IntegerField parentIntegerField
			{
				get
				{
					return (IntegerField)base.parent;
				}
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x0001D6C6 File Offset: 0x0001B8C6
			internal IntegerInput()
			{
				base.formatString = UINumericFieldsUtils.k_IntFieldFormatString;
			}

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001D6DC File Offset: 0x0001B8DC
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForInt;
				}
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, int startValue)
			{
				double num = (double)NumericFieldDraggerUtility.CalculateIntDragSensitivity((long)startValue);
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				long num3 = (long)this.StringToValue(base.text);
				num3 += (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num);
				bool isDelayed = this.parentIntegerField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(Mathf.ClampToInt(num3));
				}
				else
				{
					this.parentIntegerField.value = Mathf.ClampToInt(num3);
				}
			}

			// Token: 0x060007A9 RID: 1961 RVA: 0x0001D76C File Offset: 0x0001B96C
			protected override string ValueToString(int v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x0001D78C File Offset: 0x0001B98C
			protected override int StringToValue(string str)
			{
				return this.parentIntegerField.StringToValue(str);
			}
		}
	}
}
