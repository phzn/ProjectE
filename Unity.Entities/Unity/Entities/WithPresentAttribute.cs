using System;

namespace Unity.Entities
{
	// Token: 0x020001B7 RID: 439
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithPresentAttribute : Attribute
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x00002050 File Offset: 0x00000250
		public WithPresentAttribute(params Type[] types)
		{
		}
	}
}
