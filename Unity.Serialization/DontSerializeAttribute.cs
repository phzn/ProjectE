using System;

namespace Unity.Serialization
{
	// Token: 0x0200000B RID: 11
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DontSerializeAttribute : Attribute
	{
	}
}
