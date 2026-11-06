using System;

namespace Unity.Entities
{
	// Token: 0x02000312 RID: 786
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class RegisterGenericComponentTypeAttribute : Attribute
	{
		// Token: 0x06001539 RID: 5433 RVA: 0x00050825 File Offset: 0x0004EA25
		public RegisterGenericComponentTypeAttribute(Type type)
		{
			this.ConcreteType = type;
		}

		// Token: 0x040007AA RID: 1962
		public Type ConcreteType;
	}
}
