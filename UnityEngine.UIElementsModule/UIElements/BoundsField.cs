using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000076 RID: 118
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class BoundsField : BaseField<Bounds>
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x0001492D File Offset: 0x00012B2D
		public BoundsField()
			: this(null)
		{
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014938 File Offset: 0x00012B38
		public BoundsField(string label)
			: base(label, null)
		{
			base.delegatesFocus = false;
			base.visualInput.focusable = false;
			base.AddToClassList(BoundsField.ussClassName);
			base.visualInput.AddToClassList(BoundsField.inputUssClassName);
			base.labelElement.AddToClassList(BoundsField.labelUssClassName);
			this.m_CenterField = new Vector3Field("Center");
			this.m_CenterField.name = "unity-m_Center-input";
			this.m_CenterField.delegatesFocus = true;
			this.m_CenterField.AddToClassList(BoundsField.centerFieldUssClassName);
			this.m_CenterField.RegisterValueChangedCallback<Vector3>(delegate(ChangeEvent<Vector3> e)
			{
				Bounds value = this.value;
				value.center = e.newValue;
				this.value = value;
			});
			base.visualInput.hierarchy.Add(this.m_CenterField);
			this.m_ExtentsField = new Vector3Field("Extents");
			this.m_ExtentsField.name = "unity-m_Extent-input";
			this.m_ExtentsField.delegatesFocus = true;
			this.m_ExtentsField.AddToClassList(BoundsField.extentsFieldUssClassName);
			this.m_ExtentsField.RegisterValueChangedCallback<Vector3>(delegate(ChangeEvent<Vector3> e)
			{
				Bounds value2 = this.value;
				value2.extents = e.newValue;
				this.value = value2;
			});
			base.visualInput.hierarchy.Add(this.m_ExtentsField);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00014A74 File Offset: 0x00012C74
		public override void SetValueWithoutNotify(Bounds newValue)
		{
			base.SetValueWithoutNotify(newValue);
			this.m_CenterField.SetValueWithoutNotify(base.rawValue.center);
			this.m_ExtentsField.SetValueWithoutNotify(base.rawValue.extents);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00014ABE File Offset: 0x00012CBE
		protected override void UpdateMixedValueContent()
		{
			this.m_CenterField.showMixedValue = base.showMixedValue;
			this.m_ExtentsField.showMixedValue = base.showMixedValue;
		}

		// Token: 0x04000204 RID: 516
		public new static readonly string ussClassName = "unity-bounds-field";

		// Token: 0x04000205 RID: 517
		public new static readonly string labelUssClassName = BoundsField.ussClassName + "__label";

		// Token: 0x04000206 RID: 518
		public new static readonly string inputUssClassName = BoundsField.ussClassName + "__input";

		// Token: 0x04000207 RID: 519
		public static readonly string centerFieldUssClassName = BoundsField.ussClassName + "__center-field";

		// Token: 0x04000208 RID: 520
		public static readonly string extentsFieldUssClassName = BoundsField.ussClassName + "__extents-field";

		// Token: 0x04000209 RID: 521
		private Vector3Field m_CenterField;

		// Token: 0x0400020A RID: 522
		private Vector3Field m_ExtentsField;

		// Token: 0x02000077 RID: 119
		public new class UxmlFactory : UxmlFactory<BoundsField, BoundsField.UxmlTraits>
		{
		}

		// Token: 0x02000078 RID: 120
		public new class UxmlTraits : BaseField<Bounds>.UxmlTraits
		{
			// Token: 0x06000552 RID: 1362 RVA: 0x00014BB0 File Offset: 0x00012DB0
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				BoundsField boundsField = (BoundsField)ve;
				boundsField.SetValueWithoutNotify(new Bounds(new Vector3(this.m_CenterXValue.GetValueFromBag(bag, cc), this.m_CenterYValue.GetValueFromBag(bag, cc), this.m_CenterZValue.GetValueFromBag(bag, cc)), new Vector3(this.m_ExtentsXValue.GetValueFromBag(bag, cc), this.m_ExtentsYValue.GetValueFromBag(bag, cc), this.m_ExtentsZValue.GetValueFromBag(bag, cc))));
			}

			// Token: 0x0400020B RID: 523
			private UxmlFloatAttributeDescription m_CenterXValue = new UxmlFloatAttributeDescription
			{
				name = "cx"
			};

			// Token: 0x0400020C RID: 524
			private UxmlFloatAttributeDescription m_CenterYValue = new UxmlFloatAttributeDescription
			{
				name = "cy"
			};

			// Token: 0x0400020D RID: 525
			private UxmlFloatAttributeDescription m_CenterZValue = new UxmlFloatAttributeDescription
			{
				name = "cz"
			};

			// Token: 0x0400020E RID: 526
			private UxmlFloatAttributeDescription m_ExtentsXValue = new UxmlFloatAttributeDescription
			{
				name = "ex"
			};

			// Token: 0x0400020F RID: 527
			private UxmlFloatAttributeDescription m_ExtentsYValue = new UxmlFloatAttributeDescription
			{
				name = "ey"
			};

			// Token: 0x04000210 RID: 528
			private UxmlFloatAttributeDescription m_ExtentsZValue = new UxmlFloatAttributeDescription
			{
				name = "ez"
			};
		}
	}
}
