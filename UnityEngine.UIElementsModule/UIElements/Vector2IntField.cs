using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000099 RID: 153
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class Vector2IntField : BaseCompositeField<Vector2Int, IntegerField, int>
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x00016398 File Offset: 0x00014598
		internal override BaseCompositeField<Vector2Int, IntegerField, int>.FieldDescription[] DescribeFields()
		{
			BaseCompositeField<Vector2Int, IntegerField, int>.FieldDescription[] array = new BaseCompositeField<Vector2Int, IntegerField, int>.FieldDescription[2];
			array[0] = new BaseCompositeField<Vector2Int, IntegerField, int>.FieldDescription("X", "unity-x-input", (Vector2Int r) => r.x, delegate(ref Vector2Int r, int v)
			{
				r.x = v;
			});
			array[1] = new BaseCompositeField<Vector2Int, IntegerField, int>.FieldDescription("Y", "unity-y-input", (Vector2Int r) => r.y, delegate(ref Vector2Int r, int v)
			{
				r.y = v;
			});
			return array;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00016458 File Offset: 0x00014658
		public Vector2IntField()
			: this(null)
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00016463 File Offset: 0x00014663
		public Vector2IntField(string label)
			: base(label, 2)
		{
			base.AddToClassList(Vector2IntField.ussClassName);
			base.labelElement.AddToClassList(Vector2IntField.labelUssClassName);
			base.visualInput.AddToClassList(Vector2IntField.inputUssClassName);
		}

		// Token: 0x04000273 RID: 627
		public new static readonly string ussClassName = "unity-vector2-int-field";

		// Token: 0x04000274 RID: 628
		public new static readonly string labelUssClassName = Vector2IntField.ussClassName + "__label";

		// Token: 0x04000275 RID: 629
		public new static readonly string inputUssClassName = Vector2IntField.ussClassName + "__input";

		// Token: 0x0200009A RID: 154
		public new class UxmlFactory : UxmlFactory<Vector2IntField, Vector2IntField.UxmlTraits>
		{
		}

		// Token: 0x0200009B RID: 155
		public new class UxmlTraits : BaseField<Vector2Int>.UxmlTraits
		{
			// Token: 0x060005D0 RID: 1488 RVA: 0x000164DC File Offset: 0x000146DC
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Vector2IntField vector2IntField = (Vector2IntField)ve;
				vector2IntField.SetValueWithoutNotify(new Vector2Int(this.m_XValue.GetValueFromBag(bag, cc), this.m_YValue.GetValueFromBag(bag, cc)));
			}

			// Token: 0x04000276 RID: 630
			private UxmlIntAttributeDescription m_XValue = new UxmlIntAttributeDescription
			{
				name = "x"
			};

			// Token: 0x04000277 RID: 631
			private UxmlIntAttributeDescription m_YValue = new UxmlIntAttributeDescription
			{
				name = "y"
			};
		}
	}
}
