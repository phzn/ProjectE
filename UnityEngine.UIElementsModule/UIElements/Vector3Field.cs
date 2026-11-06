using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000091 RID: 145
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class Vector3Field : BaseCompositeField<Vector3, FloatField, float>
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x00015E08 File Offset: 0x00014008
		internal override BaseCompositeField<Vector3, FloatField, float>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<Vector3, FloatField, float>.FieldDescription[] array = new BaseCompositeField<Vector3, FloatField, float>.FieldDescription[3];
			array[0] = new BaseCompositeField<Vector3, FloatField, float>.FieldDescription("X", "unity-x-input", (Vector3 r) => r.x, delegate(ref Vector3 r, float v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<Vector3, FloatField, float>.FieldDescription("Y", "unity-y-input", (Vector3 r) => r.y, delegate(ref Vector3 r, float v)
			{
				r.y = v;
			});
			array[2] = new BaseCompositeField<Vector3, FloatField, float>.FieldDescription("Z", "unity-z-input", (Vector3 r) => r.z, delegate(ref Vector3 r, float v)
			{
				r.z = v;
			});
			return array;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00015F1C File Offset: 0x0001411C
		public Vector3Field()
			: this(null)
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00015F27 File Offset: 0x00014127
		public Vector3Field(string label)
			: base(label, 3)
		{
			base.AddToClassList(Vector3Field.ussClassName);
			base.labelElement.AddToClassList(Vector3Field.labelUssClassName);
			base.visualInput.AddToClassList(Vector3Field.inputUssClassName);
		}

		// Token: 0x04000256 RID: 598
		public new static readonly string ussClassName = "unity-vector3-field";

		// Token: 0x04000257 RID: 599
		public new static readonly string labelUssClassName = Vector3Field.ussClassName + "__label";

		// Token: 0x04000258 RID: 600
		public new static readonly string inputUssClassName = Vector3Field.ussClassName + "__input";

		// Token: 0x02000092 RID: 146
		public new class UxmlFactory : UxmlFactory<Vector3Field, Vector3Field.UxmlTraits>
		{
		}

		// Token: 0x02000093 RID: 147
		public new class UxmlTraits : BaseField<Vector3>.UxmlTraits
		{
			// Token: 0x060005B0 RID: 1456 RVA: 0x00015FA0 File Offset: 0x000141A0
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Vector3Field vector3Field = (Vector3Field)ve;
				vector3Field.SetValueWithoutNotify(new Vector3(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc), this.m_ZValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x04000259 RID: 601
			private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
			{
				name = "x"
			};

			// Token: 0x0400025A RID: 602
			private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
			{
				name = "y"
			};

			// Token: 0x0400025B RID: 603
			private UxmlFloatAttributeDescription m_ZValue = new UxmlFloatAttributeDescription
			{
				name = "z"
			};
		}
	}
}
