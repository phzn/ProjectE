using System;

namespace Unity.Entities
{
	// Token: 0x020001B5 RID: 437
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithAbsentAttribute : Attribute
	{
		// Token: 0x06000A31 RID: 2609 RVA: 0x00002050 File Offset: 0x00000250
		public WithAbsentAttribute(params Type[] types)
		{
		}
	}
}
