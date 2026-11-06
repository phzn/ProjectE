using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A05 RID: 2565
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	public sealed class MaybeNullAttribute : Attribute
	{
	}
}
