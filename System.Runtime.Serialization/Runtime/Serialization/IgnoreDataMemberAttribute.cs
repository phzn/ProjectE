using System;

namespace System.Runtime.Serialization
{
	/// <summary>When applied to the member of a type, specifies that the member is not part of a data contract and is not serialized.</summary>
	// Token: 0x020000E8 RID: 232
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class IgnoreDataMemberAttribute : Attribute
	{
	}
}
