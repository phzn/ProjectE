using System;

namespace Unity.Entities
{
	// Token: 0x020000D9 RID: 217
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface)]
	public class DisallowRefReturnCrossingThisAttribute : Attribute
	{
	}
}
