using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000CF RID: 207
	public class BaseFieldTraits<TValueType, TValueUxmlAttributeType> : BaseField<TValueType>.UxmlTraits where TValueUxmlAttributeType : TypedUxmlAttributeDescription<TValueType>, new()
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A92D File Offset: 0x00018B2D
		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((INotifyValueChanged<TValueType>)ve).SetValueWithoutNotify(this.m_Value.GetValueFromBag(bag, cc));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A958 File Offset: 0x00018B58
		public BaseFieldTraits()
		{
			TValueUxmlAttributeType tvalueUxmlAttributeType = new TValueUxmlAttributeType();
			tvalueUxmlAttributeType.name = "value";
			this.m_Value = tvalueUxmlAttributeType;
			base..ctor();
		}

		// Token: 0x04000317 RID: 791
		private TValueUxmlAttributeType m_Value;
	}
}
