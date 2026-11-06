using System;
using Unity.Properties;

namespace Unity.Serialization
{
	// Token: 0x02000016 RID: 22
	internal class SerializedReferenceVisitor : IPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000276F File Offset: 0x0000096F
		public void SetSerializedReference(SerializedReferences references)
		{
			this.m_SerializedReferences = references;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002778 File Offset: 0x00000978
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				property.Accept(this, ref container);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027D0 File Offset: 0x000009D0
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			TValue value = property.GetValue(ref container);
			bool flag = !TypeTraits<TValue>.IsValueType;
			if (flag)
			{
				if (value == null)
				{
					return;
				}
				flag = !value.GetType().IsValueType;
			}
			if (!flag)
			{
				PropertyContainer.TryAccept<TValue>(this, ref value, default(VisitParameters));
				return;
			}
			object obj = value;
			if (this.m_SerializedReferences.SetVisited(obj))
			{
				PropertyContainer.TryAccept<TValue>(this, ref value, default(VisitParameters));
				return;
			}
			this.m_SerializedReferences.AddSerializedReference(obj);
		}

		// Token: 0x04000014 RID: 20
		private SerializedReferences m_SerializedReferences;
	}
}
