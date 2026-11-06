using System;

namespace Unity.Entities
{
	// Token: 0x020001BB RID: 443
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Parameter, AllowMultiple = true)]
	public sealed class WithChangeFilterAttribute : Attribute
	{
		// Token: 0x06000A37 RID: 2615 RVA: 0x00002050 File Offset: 0x00000250
		public WithChangeFilterAttribute(params Type[] types)
		{
		}
	}
}
