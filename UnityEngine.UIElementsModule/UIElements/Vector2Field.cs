using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x0200008D RID: 141
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class Vector2Field : BaseCompositeField<Vector2, FloatField, float>
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x00015C18 File Offset: 0x00013E18
		internal override BaseCompositeField<Vector2, FloatField, float>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<Vector2, FloatField, float>.FieldDescription[] array = new BaseCompositeField<Vector2, FloatField, float>.FieldDescription[2];
			array[0] = new BaseCompositeField<Vector2, FloatField, float>.FieldDescription("X", "unity-x-input", (Vector2 r) => r.x, delegate(ref Vector2 r, float v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<Vector2, FloatField, float>.FieldDescription("Y", "unity-y-input", (Vector2 r) => r.y, delegate(ref Vector2 r, float v)
			{
				r.y = v;
			});
			return array;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00015CD8 File Offset: 0x00013ED8
		public Vector2Field()
			: this(null)
		{
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00015CE3 File Offset: 0x00013EE3
		public Vector2Field(string label)
			: base(label, 2)
		{
			base.AddToClassList(Vector2Field.ussClassName);
			base.labelElement.AddToClassList(Vector2Field.labelUssClassName);
			base.visualInput.AddToClassList(Vector2Field.inputUssClassName);
		}

		// Token: 0x0400024C RID: 588
		public new static readonly string ussClassName = "unity-vector2-field";

		// Token: 0x0400024D RID: 589
		public new static readonly string labelUssClassName = Vector2Field.ussClassName + "__label";

		// Token: 0x0400024E RID: 590
		public new static readonly string inputUssClassName = Vector2Field.ussClassName + "__input";

		// Token: 0x0200008E RID: 142
		public new class UxmlFactory : UxmlFactory<Vector2Field, Vector2Field.UxmlTraits>
		{
		}

		// Token: 0x0200008F RID: 143
		public new class UxmlTraits : BaseField<Vector2>.UxmlTraits
		{
			// Token: 0x060005A3 RID: 1443 RVA: 0x00015D5C File Offset: 0x00013F5C
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Vector2Field vector2Field = (Vector2Field)ve;
				vector2Field.SetValueWithoutNotify(new Vector2(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x0400024F RID: 591
			private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
			{
				name = "x"
			};

			// Token: 0x04000250 RID: 592
			private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
			{
				name = "y"
			};
		}
	}
}
