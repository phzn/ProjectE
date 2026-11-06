using System;

namespace Unity.Entities
{
	// Token: 0x0200000C RID: 12
	[Obsolete("AlwaysUpdateSystem is deprecated. Systems now always update by default. Use RequireMatchingQueriesForUpdate if you want a System to only update if one of its queries matches. (RemovedAfter Entities 1.0)")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class AlwaysUpdateSystemAttribute : Attribute
	{
	}
}
