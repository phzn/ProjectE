using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Fixes the address of a static value type field throughout its lifetime. This class cannot be inherited.</summary>
	// Token: 0x020007F1 RID: 2033
	[AttributeUsage(AttributeTargets.Field)]
	[Serializable]
	public sealed class FixedAddressValueTypeAttribute : Attribute
	{
	}
}
