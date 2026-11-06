using System;

namespace Unity.Entities
{
	// Token: 0x020001BD RID: 445
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class WithOptionsAttribute : Attribute
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x00002050 File Offset: 0x00000250
		public WithOptionsAttribute(EntityQueryOptions option)
		{
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00002050 File Offset: 0x00000250
		public WithOptionsAttribute(params EntityQueryOptions[] options)
		{
		}
	}
}
