using System;

namespace Unity.Entities
{
	// Token: 0x020001B6 RID: 438
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithDisabledAttribute : Attribute
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x00002050 File Offset: 0x00000250
		public WithDisabledAttribute(params Type[] types)
		{
		}
	}
}
