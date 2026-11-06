using System;

namespace System.Diagnostics.Contracts
{
	/// <summary>Marks a method as being the invariant method for a class.</summary>
	// Token: 0x020009C7 RID: 2503
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class ContractInvariantMethodAttribute : Attribute
	{
	}
}
