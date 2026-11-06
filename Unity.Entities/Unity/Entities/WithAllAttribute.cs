using System;

namespace Unity.Entities
{
	// Token: 0x020001B8 RID: 440
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithAllAttribute : Attribute
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x00002050 File Offset: 0x00000250
		public WithAllAttribute(params Type[] types)
		{
		}
	}
}
