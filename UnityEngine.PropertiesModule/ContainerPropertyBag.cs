using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000036 RID: 54
	public abstract class ContainerPropertyBag<TContainer> : PropertyBag<TContainer>, INamedProperties<TContainer>
	{
		// Token: 0x060000FF RID: 255 RVA: 0x000052CC File Offset: 0x000034CC
		static ContainerPropertyBag()
		{
			bool flag = !TypeTraits.IsContainer(typeof(TContainer));
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Failed to create a property bag for Type=[{0}]. The type is not a valid container type.", typeof(TContainer)));
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000530B File Offset: 0x0000350B
		protected void AddProperty<TValue>(Property<TContainer, TValue> property)
		{
			this.m_PropertiesList.Add(property);
			this.m_PropertiesHash.Add(property.Name, property);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000532E File Offset: 0x0000352E
		public override PropertyCollection<TContainer> GetProperties()
		{
			return new PropertyCollection<TContainer>(this.m_PropertiesList);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000532E File Offset: 0x0000352E
		public override PropertyCollection<TContainer> GetProperties(ref TContainer container)
		{
			return new PropertyCollection<TContainer>(this.m_PropertiesList);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000533B File Offset: 0x0000353B
		public bool TryGetProperty(ref TContainer container, string name, out IProperty<TContainer> property)
		{
			return this.m_PropertiesHash.TryGetValue(name, out property);
		}

		// Token: 0x04000059 RID: 89
		private readonly List<IProperty<TContainer>> m_PropertiesList = new List<IProperty<TContainer>>();

		// Token: 0x0400005A RID: 90
		private readonly Dictionary<string, IProperty<TContainer>> m_PropertiesHash = new Dictionary<string, IProperty<TContainer>>();
	}
}
