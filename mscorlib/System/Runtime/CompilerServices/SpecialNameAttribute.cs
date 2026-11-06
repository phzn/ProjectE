using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that a type or member is treated in a special way by the runtime or tools.  This class cannot be inherited.</summary>
	// Token: 0x02000804 RID: 2052
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class SpecialNameAttribute : Attribute
	{
	}
}
