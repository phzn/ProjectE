using System;

namespace Unity.Entities
{
	// Token: 0x02000313 RID: 787
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class RegisterGenericSystemTypeAttribute : Attribute
	{
		// Token: 0x0600153A RID: 5434 RVA: 0x00050834 File Offset: 0x0004EA34
		public RegisterGenericSystemTypeAttribute(Type type)
		{
			this.ConcreteType = type;
		}

		// Token: 0x040007AB RID: 1963
		public Type ConcreteType;
	}
}
