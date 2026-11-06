using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000134 RID: 308
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class TextValueFieldTraits<TValueType, TValueUxmlAttributeType> : BaseFieldTraits<TValueType, TValueUxmlAttributeType> where TValueUxmlAttributeType : TypedUxmlAttributeDescription<TValueType>, new()
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x00028E0C File Offset: 0x0002700C
		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)ve;
			bool flag = textInputBaseField != null;
			if (flag)
			{
				textInputBaseField.isReadOnly = this.m_IsReadOnly.GetValueFromBag(bag, cc);
				textInputBaseField.isDelayed = this.m_IsDelayed.GetValueFromBag(bag, cc);
			}
		}

		// Token: 0x040004E2 RID: 1250
		private UxmlBoolAttributeDescription m_IsReadOnly = new UxmlBoolAttributeDescription
		{
			name = "readonly"
		};

		// Token: 0x040004E3 RID: 1251
		private UxmlBoolAttributeDescription m_IsDelayed = new UxmlBoolAttributeDescription
		{
			name = "is-delayed"
		};
	}
}
