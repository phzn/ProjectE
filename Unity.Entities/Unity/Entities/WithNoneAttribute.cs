using System;

namespace Unity.Entities
{
	// Token: 0x020001B9 RID: 441
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithNoneAttribute : Attribute
	{
		// Token: 0x06000A35 RID: 2613 RVA: 0x00002050 File Offset: 0x00000250
		public WithNoneAttribute(params Type[] types)
		{
		}
	}
}
