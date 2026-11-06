using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000079 RID: 121
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class BoundsIntField : BaseField<BoundsInt>
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x00014CD2 File Offset: 0x00012ED2
		public BoundsIntField()
			: this(null)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00014CE0 File Offset: 0x00012EE0
		public BoundsIntField(string label)
			: base(label, null)
		{
			base.delegatesFocus = false;
			base.visualInput.focusable = false;
			base.AddToClassList(BoundsIntField.ussClassName);
			base.visualInput.AddToClassList(BoundsIntField.inputUssClassName);
			base.labelElement.AddToClassList(BoundsIntField.labelUssClassName);
			this.m_PositionField = new Vector3IntField("Position");
			this.m_PositionField.name = "unity-m_Position-input";
			this.m_PositionField.delegatesFocus = true;
			this.m_PositionField.AddToClassList(BoundsIntField.positionUssClassName);
			this.m_PositionField.RegisterValueChangedCallback<Vector3Int>(delegate(ChangeEvent<Vector3Int> e)
			{
				BoundsInt value = this.value;
				value.position = e.newValue;
				this.value = value;
			});
			base.visualInput.hierarchy.Add(this.m_PositionField);
			this.m_SizeField = new Vector3IntField("Size");
			this.m_SizeField.name = "unity-m_Size-input";
			this.m_SizeField.delegatesFocus = true;
			this.m_SizeField.AddToClassList(BoundsIntField.sizeUssClassName);
			this.m_SizeField.RegisterValueChangedCallback<Vector3Int>(delegate(ChangeEvent<Vector3Int> e)
			{
				BoundsInt value2 = this.value;
				value2.size = e.newValue;
				this.value = value2;
			});
			base.visualInput.hierarchy.Add(this.m_SizeField);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00014E1C File Offset: 0x0001301C
		public override void SetValueWithoutNotify(BoundsInt newValue)
		{
			base.SetValueWithoutNotify(newValue);
			this.m_PositionField.SetValueWithoutNotify(base.rawValue.position);
			this.m_SizeField.SetValueWithoutNotify(base.rawValue.size);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00014E66 File Offset: 0x00013066
		protected override void UpdateMixedValueContent()
		{
			this.m_PositionField.showMixedValue = base.showMixedValue;
			this.m_SizeField.showMixedValue = base.showMixedValue;
		}

		// Token: 0x04000211 RID: 529
		private Vector3IntField m_PositionField;

		// Token: 0x04000212 RID: 530
		private Vector3IntField m_SizeField;

		// Token: 0x04000213 RID: 531
		public new static readonly string ussClassName = "unity-bounds-int-field";

		// Token: 0x04000214 RID: 532
		public new static readonly string labelUssClassName = BoundsIntField.ussClassName + "__label";

		// Token: 0x04000215 RID: 533
		public new static readonly string inputUssClassName = BoundsIntField.ussClassName + "__input";

		// Token: 0x04000216 RID: 534
		public static readonly string positionUssClassName = BoundsIntField.ussClassName + "__position-field";

		// Token: 0x04000217 RID: 535
		public static readonly string sizeUssClassName = BoundsIntField.ussClassName + "__size-field";

		// Token: 0x0200007A RID: 122
		public new class UxmlFactory : UxmlFactory<BoundsIntField, BoundsIntField.UxmlTraits>
		{
		}

		// Token: 0x0200007B RID: 123
		public new class UxmlTraits : BaseField<BoundsInt>.UxmlTraits
		{
			// Token: 0x0600055C RID: 1372 RVA: 0x00014F58 File Offset: 0x00013158
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				BoundsIntField boundsIntField = (BoundsIntField)ve;
				boundsIntField.SetValueWithoutNotify(new BoundsInt(new Vector3Int(this.m_PositionXValue.GetValueFromBag(bag, cc), this.m_PositionYValue.GetValueFromBag(bag, cc), this.m_PositionZValue.GetValueFromBag(bag, cc)), new Vector3Int(this.m_SizeXValue.GetValueFromBag(bag, cc), this.m_SizeYValue.GetValueFromBag(bag, cc), this.m_SizeZValue.GetValueFromBag(bag, cc))));
			}

			// Token: 0x04000218 RID: 536
			private UxmlIntAttributeDescription m_PositionXValue = new UxmlIntAttributeDescription
			{
				name = "px"
			};

			// Token: 0x04000219 RID: 537
			private UxmlIntAttributeDescription m_PositionYValue = new UxmlIntAttributeDescription
			{
				name = "py"
			};

			// Token: 0x0400021A RID: 538
			private UxmlIntAttributeDescription m_PositionZValue = new UxmlIntAttributeDescription
			{
				name = "pz"
			};

			// Token: 0x0400021B RID: 539
			private UxmlIntAttributeDescription m_SizeXValue = new UxmlIntAttributeDescription
			{
				name = "sx"
			};

			// Token: 0x0400021C RID: 540
			private UxmlIntAttributeDescription m_SizeYValue = new UxmlIntAttributeDescription
			{
				name = "sy"
			};

			// Token: 0x0400021D RID: 541
			private UxmlIntAttributeDescription m_SizeZValue = new UxmlIntAttributeDescription
			{
				name = "sz"
			};
		}
	}
}
