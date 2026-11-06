using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000148 RID: 328
	public class UnsignedIntegerField : TextValueField<uint>
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0002BC42 File Offset: 0x00029E42
		private UnsignedIntegerField.UnsignedIntegerInput integerInput
		{
			get
			{
				return (UnsignedIntegerField.UnsignedIntegerInput)base.textInputBase;
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002BC50 File Offset: 0x00029E50
		protected override string ValueToString(uint v)
		{
			return v.ToString(base.formatString, CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002BC7C File Offset: 0x00029E7C
		protected override uint StringToValue(string str)
		{
			uint num;
			ExpressionEvaluator.Expression expression;
			bool flag = UINumericFieldsUtils.TryConvertStringToUInt(str, base.textInputBase.originalText, out num, out expression);
			Action<ExpressionEvaluator.Expression> expressionEvaluated = this.expressionEvaluated;
			if (expressionEvaluated != null)
			{
				expressionEvaluated(expression);
			}
			return flag ? num : base.rawValue;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002BCC3 File Offset: 0x00029EC3
		public UnsignedIntegerField()
			: this(null, 1000)
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002BCD3 File Offset: 0x00029ED3
		public UnsignedIntegerField(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002BCE0 File Offset: 0x00029EE0
		public UnsignedIntegerField(string label, int maxLength = 1000)
			: base(label, maxLength, new UnsignedIntegerField.UnsignedIntegerInput())
		{
			base.AddToClassList(UnsignedIntegerField.ussClassName);
			base.labelElement.AddToClassList(UnsignedIntegerField.labelUssClassName);
			base.visualInput.AddToClassList(UnsignedIntegerField.inputUssClassName);
			base.AddLabelDragger<uint>();
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002BD34 File Offset: 0x00029F34
		internal override bool CanTryParse(string textString)
		{
			uint num;
			return uint.TryParse(textString, out num);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002BD49 File Offset: 0x00029F49
		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, uint startValue)
		{
			this.integerInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		// Token: 0x04000530 RID: 1328
		public new static readonly string ussClassName = "unity-unsigned-integer-field";

		// Token: 0x04000531 RID: 1329
		public new static readonly string labelUssClassName = UnsignedIntegerField.ussClassName + "__label";

		// Token: 0x04000532 RID: 1330
		public new static readonly string inputUssClassName = UnsignedIntegerField.ussClassName + "__input";

		// Token: 0x02000149 RID: 329
		public new class UxmlFactory : UxmlFactory<UnsignedIntegerField, UnsignedIntegerField.UxmlTraits>
		{
		}

		// Token: 0x0200014A RID: 330
		public new class UxmlTraits : TextValueFieldTraits<uint, UxmlUnsignedIntAttributeDescription>
		{
		}

		// Token: 0x0200014B RID: 331
		private class UnsignedIntegerInput : TextValueField<uint>.TextValueInput
		{
			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0002BDA1 File Offset: 0x00029FA1
			private UnsignedIntegerField parentUnsignedIntegerField
			{
				get
				{
					return (UnsignedIntegerField)base.parent;
				}
			}

			// Token: 0x06000AD4 RID: 2772 RVA: 0x0002BDAE File Offset: 0x00029FAE
			internal UnsignedIntegerInput()
			{
				base.formatString = UINumericFieldsUtils.k_IntFieldFormatString;
			}

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0001D6DC File Offset: 0x0001B8DC
			protected override string allowedCharacters
			{
				get
				{
					return UINumericFieldsUtils.k_AllowedCharactersForInt;
				}
			}

			// Token: 0x06000AD6 RID: 2774 RVA: 0x0002BDC4 File Offset: 0x00029FC4
			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, uint startValue)
			{
				double num = (double)NumericFieldDraggerUtility.CalculateIntDragSensitivity((long)((ulong)startValue));
				float num2 = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
				long num3 = (long)((ulong)this.StringToValue(base.text));
				num3 += (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, num2) * num);
				bool isDelayed = this.parentUnsignedIntegerField.isDelayed;
				if (isDelayed)
				{
					base.text = this.ValueToString(Mathf.ClampToUInt(num3));
				}
				else
				{
					this.parentUnsignedIntegerField.value = Mathf.ClampToUInt(num3);
				}
			}

			// Token: 0x06000AD7 RID: 2775 RVA: 0x0002BE4C File Offset: 0x0002A04C
			protected override string ValueToString(uint v)
			{
				return v.ToString(base.formatString);
			}

			// Token: 0x06000AD8 RID: 2776 RVA: 0x0002BE6C File Offset: 0x0002A06C
			protected override uint StringToValue(string str)
			{
				return this.parentUnsignedIntegerField.StringToValue(str);
			}
		}
	}
}
