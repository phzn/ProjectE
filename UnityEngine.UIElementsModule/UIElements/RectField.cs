using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000085 RID: 133
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class RectField : BaseCompositeField<Rect, FloatField, float>
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x000155C0 File Offset: 0x000137C0
		internal override BaseCompositeField<Rect, FloatField, float>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<Rect, FloatField, float>.FieldDescription[] array = new BaseCompositeField<Rect, FloatField, float>.FieldDescription[4];
			array[0] = new BaseCompositeField<Rect, FloatField, float>.FieldDescription("X", "unity-x-input", (Rect r) => r.x, delegate(ref Rect r, float v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<Rect, FloatField, float>.FieldDescription("Y", "unity-y-input", (Rect r) => r.y, delegate(ref Rect r, float v)
			{
				r.y = v;
			});
			array[2] = new BaseCompositeField<Rect, FloatField, float>.FieldDescription("W", "unity-width-input", (Rect r) => r.width, delegate(ref Rect r, float v)
			{
				r.width = v;
			});
			array[3] = new BaseCompositeField<Rect, FloatField, float>.FieldDescription("H", "unity-height-input", (Rect r) => r.height, delegate(ref Rect r, float v)
			{
				r.height = v;
			});
			return array;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00015728 File Offset: 0x00013928
		public RectField()
			: this(null)
		{
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00015734 File Offset: 0x00013934
		public RectField(string label)
			: base(label, 2)
		{
			base.AddToClassList(RectField.ussClassName);
			base.AddToClassList(BaseCompositeField<Rect, FloatField, float>.twoLinesVariantUssClassName);
			base.labelElement.AddToClassList(RectField.labelUssClassName);
			base.visualInput.AddToClassList(RectField.inputUssClassName);
		}

		// Token: 0x0400022C RID: 556
		public new static readonly string ussClassName = "unity-rect-field";

		// Token: 0x0400022D RID: 557
		public new static readonly string labelUssClassName = RectField.ussClassName + "__label";

		// Token: 0x0400022E RID: 558
		public new static readonly string inputUssClassName = RectField.ussClassName + "__input";

		// Token: 0x02000086 RID: 134
		public new class UxmlFactory : UxmlFactory<RectField, RectField.UxmlTraits>
		{
		}

		// Token: 0x02000087 RID: 135
		public new class UxmlTraits : BaseField<Rect>.UxmlTraits
		{
			// Token: 0x06000581 RID: 1409 RVA: 0x000157C4 File Offset: 0x000139C4
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				RectField rectField = (RectField)ve;
				rectField.SetValueWithoutNotify(new Rect(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc), this.m_WValue.GetValueFromBag(bag, cc), this.m_HValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x0400022F RID: 559
			private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
			{
				name = "x"
			};

			// Token: 0x04000230 RID: 560
			private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
			{
				name = "y"
			};

			// Token: 0x04000231 RID: 561
			private UxmlFloatAttributeDescription m_WValue = new UxmlFloatAttributeDescription
			{
				name = "w"
			};

			// Token: 0x04000232 RID: 562
			private UxmlFloatAttributeDescription m_HValue = new UxmlFloatAttributeDescription
			{
				name = "h"
			};
		}
	}
}
