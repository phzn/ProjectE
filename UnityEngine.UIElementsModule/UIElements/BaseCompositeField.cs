using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000056 RID: 86
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public abstract class BaseCompositeField<TValueType, TField, TFieldValue> : BaseField<TValueType> where TField : TextValueField<TFieldValue>, new()
	{
		// Token: 0x0600039F RID: 927 RVA: 0x0000DD60 File Offset: 0x0000BF60
		private VisualElement GetSpacer()
		{
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.spacerUssClassName);
			visualElement.visible = false;
			visualElement.focusable = false;
			return visualElement;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000DD95 File Offset: 0x0000BF95
		internal List<TField> fields
		{
			get
			{
				return this.m_Fields;
			}
		}

		// Token: 0x060003A1 RID: 929
		internal abstract BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription[] DescribeFields();

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000DD9D File Offset: 0x0000BF9D
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000DDA5 File Offset: 0x0000BFA5
		internal int propertyIndex
		{
			get
			{
				return this.m_PropertyIndex;
			}
			set
			{
				this.m_PropertyIndex = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000DDAE File Offset: 0x0000BFAE
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		internal bool forceUpdateDisplay
		{
			get
			{
				return this.m_ForceUpdateDisplay;
			}
			set
			{
				this.m_ForceUpdateDisplay = value;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		protected BaseCompositeField(string label, int fieldsByLine)
			: base(label, null)
		{
			base.delegatesFocus = false;
			base.visualInput.focusable = false;
			base.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName);
			base.labelElement.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.labelUssClassName);
			base.visualInput.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.inputUssClassName);
			this.m_ShouldUpdateDisplay = true;
			this.m_Fields = new List<TField>();
			BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription[] array = this.DescribeFields();
			int num = 1;
			bool flag = fieldsByLine > 1;
			if (flag)
			{
				num = array.Length / fieldsByLine;
			}
			bool flag2 = false;
			bool flag3 = num > 1;
			if (flag3)
			{
				flag2 = true;
				base.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.multilineVariantUssClassName);
			}
			this.m_PropertyIndex = 0;
			for (int i = 0; i < num; i++)
			{
				VisualElement visualElement = null;
				bool flag4 = flag2;
				if (flag4)
				{
					visualElement = new VisualElement();
					visualElement.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.fieldGroupUssClassName);
				}
				bool flag5 = true;
				for (int j = i * fieldsByLine; j < i * fieldsByLine + fieldsByLine; j++)
				{
					BaseCompositeField<TValueType, TField, TFieldValue>.<>c__DisplayClass24_0 CS$<>8__locals1 = new BaseCompositeField<TValueType, TField, TFieldValue>.<>c__DisplayClass24_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.desc = array[j];
					BaseCompositeField<TValueType, TField, TFieldValue>.<>c__DisplayClass24_0 CS$<>8__locals2 = CS$<>8__locals1;
					TField tfield = new TField();
					tfield.name = CS$<>8__locals1.desc.ussName;
					CS$<>8__locals2.field = tfield;
					CS$<>8__locals1.field.delegatesFocus = true;
					CS$<>8__locals1.field.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.fieldUssClassName);
					bool flag6 = flag5;
					if (flag6)
					{
						CS$<>8__locals1.field.AddToClassList(BaseCompositeField<TValueType, TField, TFieldValue>.firstFieldVariantUssClassName);
						flag5 = false;
					}
					CS$<>8__locals1.field.label = CS$<>8__locals1.desc.name;
					CS$<>8__locals1.field.onValidateValue += delegate(TFieldValue newValue)
					{
						TValueType value = CS$<>8__locals1.<>4__this.value;
						CS$<>8__locals1.desc.write(ref value, newValue);
						TValueType tvalueType = CS$<>8__locals1.<>4__this.ValidatedValue(value);
						return CS$<>8__locals1.desc.read(tvalueType);
					};
					CS$<>8__locals1.field.RegisterValueChangedCallback<TFieldValue>(delegate(ChangeEvent<TFieldValue> e)
					{
						TValueType value2 = CS$<>8__locals1.<>4__this.value;
						CS$<>8__locals1.desc.write(ref value2, e.newValue);
						TFieldValue newValue = e.newValue;
						string text = newValue.ToString();
						string text2 = ((TField)((object)e.currentTarget)).text;
						bool flag11 = text != text2 || CS$<>8__locals1.field.CanTryParse(text2);
						if (flag11)
						{
							CS$<>8__locals1.<>4__this.m_ShouldUpdateDisplay = false;
						}
						CS$<>8__locals1.<>4__this.value = value2;
						CS$<>8__locals1.<>4__this.m_ShouldUpdateDisplay = true;
					});
					this.m_Fields.Add(CS$<>8__locals1.field);
					bool flag7 = flag2;
					if (flag7)
					{
						visualElement.Add(CS$<>8__locals1.field);
					}
					else
					{
						base.visualInput.hierarchy.Add(CS$<>8__locals1.field);
					}
				}
				bool flag8 = fieldsByLine < 3;
				if (flag8)
				{
					int num2 = 3 - fieldsByLine;
					for (int k = 0; k < num2; k++)
					{
						bool flag9 = flag2;
						if (flag9)
						{
							visualElement.Add(this.GetSpacer());
						}
						else
						{
							base.visualInput.hierarchy.Add(this.GetSpacer());
						}
					}
				}
				bool flag10 = flag2;
				if (flag10)
				{
					base.visualInput.hierarchy.Add(visualElement);
				}
			}
			this.UpdateDisplay();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000E098 File Offset: 0x0000C298
		private void UpdateDisplay()
		{
			bool flag = this.m_Fields.Count != 0;
			if (flag)
			{
				int num = 0;
				BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription[] array = this.DescribeFields();
				foreach (BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription fieldDescription in array)
				{
					this.m_Fields[num].SetValueWithoutNotify(fieldDescription.read(base.rawValue));
					num++;
				}
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000E114 File Offset: 0x0000C314
		public override void SetValueWithoutNotify(TValueType newValue)
		{
			bool flag = this.m_ForceUpdateDisplay || (this.m_ShouldUpdateDisplay && !EqualityComparer<TValueType>.Default.Equals(base.rawValue, newValue));
			base.SetValueWithoutNotify(newValue);
			bool flag2 = flag;
			if (flag2)
			{
				this.UpdateDisplay();
			}
			this.m_ForceUpdateDisplay = false;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000E16A File Offset: 0x0000C36A
		internal override void OnViewDataReady()
		{
			this.m_ForceUpdateDisplay = true;
			base.OnViewDataReady();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000E17C File Offset: 0x0000C37C
		protected override void UpdateMixedValueContent()
		{
			foreach (TField tfield in this.m_Fields)
			{
				tfield.showMixedValue = base.showMixedValue;
			}
		}

		// Token: 0x0400011A RID: 282
		private List<TField> m_Fields;

		// Token: 0x0400011B RID: 283
		private bool m_ShouldUpdateDisplay;

		// Token: 0x0400011C RID: 284
		private bool m_ForceUpdateDisplay;

		// Token: 0x0400011D RID: 285
		private int m_PropertyIndex;

		// Token: 0x0400011E RID: 286
		public new static readonly string ussClassName = "unity-composite-field";

		// Token: 0x0400011F RID: 287
		public new static readonly string labelUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "__label";

		// Token: 0x04000120 RID: 288
		public new static readonly string inputUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "__input";

		// Token: 0x04000121 RID: 289
		public static readonly string spacerUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "__field-spacer";

		// Token: 0x04000122 RID: 290
		public static readonly string multilineVariantUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "--multi-line";

		// Token: 0x04000123 RID: 291
		public static readonly string fieldGroupUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "__field-group";

		// Token: 0x04000124 RID: 292
		public static readonly string fieldUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "__field";

		// Token: 0x04000125 RID: 293
		public static readonly string firstFieldVariantUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.fieldUssClassName + "--first";

		// Token: 0x04000126 RID: 294
		public static readonly string twoLinesVariantUssClassName = BaseCompositeField<TValueType, TField, TFieldValue>.ussClassName + "--two-lines";

		// Token: 0x02000057 RID: 87
		internal struct FieldDescription
		{
			// Token: 0x060003AC RID: 940 RVA: 0x0000E297 File Offset: 0x0000C497
			public FieldDescription(string name, string ussName, Func<TValueType, TFieldValue> read, BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription.WriteDelegate write)
			{
				this.name = name;
				this.ussName = ussName;
				this.read = read;
				this.write = write;
			}

			// Token: 0x04000127 RID: 295
			internal readonly string name;

			// Token: 0x04000128 RID: 296
			internal readonly string ussName;

			// Token: 0x04000129 RID: 297
			internal readonly Func<TValueType, TFieldValue> read;

			// Token: 0x0400012A RID: 298
			internal readonly BaseCompositeField<TValueType, TField, TFieldValue>.FieldDescription.WriteDelegate write;

			// Token: 0x02000058 RID: 88
			// (Invoke) Token: 0x060003AE RID: 942
			public delegate void WriteDelegate(ref TValueType val, TFieldValue fieldValue);
		}
	}
}
