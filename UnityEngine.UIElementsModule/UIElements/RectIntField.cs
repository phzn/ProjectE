using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000089 RID: 137
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class RectIntField : BaseCompositeField<RectInt, IntegerField, int>
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x000158EC File Offset: 0x00013AEC
		internal override BaseCompositeField<RectInt, IntegerField, int>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<RectInt, IntegerField, int>.FieldDescription[] array = new BaseCompositeField<RectInt, IntegerField, int>.FieldDescription[4];
			array[0] = new BaseCompositeField<RectInt, IntegerField, int>.FieldDescription("X", "unity-x-input", (RectInt r) => r.x, delegate(ref RectInt r, int v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<RectInt, IntegerField, int>.FieldDescription("Y", "unity-y-input", (RectInt r) => r.y, delegate(ref RectInt r, int v)
			{
				r.y = v;
			});
			array[2] = new BaseCompositeField<RectInt, IntegerField, int>.FieldDescription("W", "unity-width-input", (RectInt r) => r.width, delegate(ref RectInt r, int v)
			{
				r.width = v;
			});
			array[3] = new BaseCompositeField<RectInt, IntegerField, int>.FieldDescription("H", "unity-height-input", (RectInt r) => r.height, delegate(ref RectInt r, int v)
			{
				r.height = v;
			});
			return array;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00015A54 File Offset: 0x00013C54
		public RectIntField()
			: this(null)
		{
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00015A60 File Offset: 0x00013C60
		public RectIntField(string label)
			: base(label, 2)
		{
			base.AddToClassList(RectIntField.ussClassName);
			base.AddToClassList(BaseCompositeField<RectInt, IntegerField, int>.twoLinesVariantUssClassName);
			base.labelElement.AddToClassList(RectIntField.labelUssClassName);
			base.visualInput.AddToClassList(RectIntField.inputUssClassName);
		}

		// Token: 0x0400023C RID: 572
		public new static readonly string ussClassName = "unity-rect-int-field";

		// Token: 0x0400023D RID: 573
		public new static readonly string labelUssClassName = RectIntField.ussClassName + "__label";

		// Token: 0x0400023E RID: 574
		public new static readonly string inputUssClassName = RectIntField.ussClassName + "__input";

		// Token: 0x0200008A RID: 138
		public new class UxmlFactory : UxmlFactory<RectIntField, RectIntField.UxmlTraits>
		{
		}

		// Token: 0x0200008B RID: 139
		public new class UxmlTraits : BaseField<RectInt>.UxmlTraits
		{
			// Token: 0x06000592 RID: 1426 RVA: 0x00015AF0 File Offset: 0x00013CF0
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				RectIntField rectIntField = (RectIntField)ve;
				rectIntField.SetValueWithoutNotify(new RectInt(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc), this.m_WValue.GetValueFromBag(bag, cc), this.m_HValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x0400023F RID: 575
			private UxmlIntAttributeDescription m_XValue = new UxmlIntAttributeDescription
			{
				name = "x"
			};

			// Token: 0x04000240 RID: 576
			private UxmlIntAttributeDescription m_YValue = new UxmlIntAttributeDescription
			{
				name = "y"
			};

			// Token: 0x04000241 RID: 577
			private UxmlIntAttributeDescription m_WValue = new UxmlIntAttributeDescription
			{
				name = "w"
			};

			// Token: 0x04000242 RID: 578
			private UxmlIntAttributeDescription m_HValue = new UxmlIntAttributeDescription
			{
				name = "h"
			};
		}
	}
}
