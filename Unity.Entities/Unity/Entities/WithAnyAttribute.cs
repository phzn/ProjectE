using System;

namespace Unity.Entities
{
	// Token: 0x020001BA RID: 442
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithAnyAttribute : Attribute
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x00002050 File Offset: 0x00000250
		public WithAnyAttribute(params Type[] types)
		{
		}
	}
}
