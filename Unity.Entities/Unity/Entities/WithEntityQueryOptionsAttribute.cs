using System;

namespace Unity.Entities
{
	// Token: 0x020001BC RID: 444
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	[Obsolete("This type has been renamed to WithOptions. (Removed after Entities 1.0) (UnityUpgradable) -> WithOptionsAttribute", true)]
	public sealed class WithEntityQueryOptionsAttribute : Attribute
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x00002050 File Offset: 0x00000250
		public WithEntityQueryOptionsAttribute(EntityQueryOptions option)
		{
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00002050 File Offset: 0x00000250
		public WithEntityQueryOptionsAttribute(params EntityQueryOptions[] options)
		{
		}
	}
}
