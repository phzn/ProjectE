using System;

namespace Unity.Properties
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class GeneratePropertyBagsForTypeAttribute : Attribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002E2B File Offset: 0x0000102B
		public Type Type { get; }

		// Token: 0x06000053 RID: 83 RVA: 0x00002E34 File Offset: 0x00001034
		public GeneratePropertyBagsForTypeAttribute(Type type)
		{
			bool flag = !TypeTraits.IsContainer(type);
			if (flag)
			{
				throw new ArgumentException(type.Name + " is not a valid container type.");
			}
			this.Type = type;
		}
	}
}
