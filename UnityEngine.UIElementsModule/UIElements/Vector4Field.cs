using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000095 RID: 149
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class Vector4Field : BaseCompositeField<Vector4, FloatField, float>
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0001608C File Offset: 0x0001428C
		internal override BaseCompositeField<Vector4, FloatField, float>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<Vector4, FloatField, float>.FieldDescription[] array = new BaseCompositeField<Vector4, FloatField, float>.FieldDescription[4];
			array[0] = new BaseCompositeField<Vector4, FloatField, float>.FieldDescription("X", "unity-x-input", (Vector4 r) => r.x, delegate(ref Vector4 r, float v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<Vector4, FloatField, float>.FieldDescription("Y", "unity-y-input", (Vector4 r) => r.y, delegate(ref Vector4 r, float v)
			{
				r.y = v;
			});
			array[2] = new BaseCompositeField<Vector4, FloatField, float>.FieldDescription("Z", "unity-z-input", (Vector4 r) => r.z, delegate(ref Vector4 r, float v)
			{
				r.z = v;
			});
			array[3] = new BaseCompositeField<Vector4, FloatField, float>.FieldDescription("W", "unity-w-input", (Vector4 r) => r.w, delegate(ref Vector4 r, float v)
			{
				r.w = v;
			});
			return array;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000161F4 File Offset: 0x000143F4
		public Vector4Field()
			: this(null)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000161FF File Offset: 0x000143FF
		public Vector4Field(string label)
			: base(label, 4)
		{
			base.AddToClassList(Vector4Field.ussClassName);
			base.labelElement.AddToClassList(Vector4Field.labelUssClassName);
			base.visualInput.AddToClassList(Vector4Field.inputUssClassName);
		}

		// Token: 0x04000263 RID: 611
		public new static readonly string ussClassName = "unity-vector4-field";

		// Token: 0x04000264 RID: 612
		public new static readonly string labelUssClassName = Vector4Field.ussClassName + "__label";

		// Token: 0x04000265 RID: 613
		public new static readonly string inputUssClassName = Vector4Field.ussClassName + "__input";

		// Token: 0x02000096 RID: 150
		public new class UxmlFactory : UxmlFactory<Vector4Field, Vector4Field.UxmlTraits>
		{
		}

		// Token: 0x02000097 RID: 151
		public new class UxmlTraits : BaseField<Vector4>.UxmlTraits
		{
			// Token: 0x060005BF RID: 1471 RVA: 0x00016278 File Offset: 0x00014478
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Vector4Field vector4Field = (Vector4Field)ve;
				vector4Field.SetValueWithoutNotify(new Vector4(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc), this.m_ZValue.GetValueFromBag(bag, cc), this.m_WValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x04000266 RID: 614
			private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
			{
				name = "x"
			};

			// Token: 0x04000267 RID: 615
			private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
			{
				name = "y"
			};

			// Token: 0x04000268 RID: 616
			private UxmlFloatAttributeDescription m_ZValue = new UxmlFloatAttributeDescription
			{
				name = "z"
			};

			// Token: 0x04000269 RID: 617
			private UxmlFloatAttributeDescription m_WValue = new UxmlFloatAttributeDescription
			{
				name = "w"
			};
		}
	}
}
