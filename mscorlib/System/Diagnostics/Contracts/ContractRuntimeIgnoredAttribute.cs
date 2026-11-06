using System;

namespace System.Diagnostics.Contracts
{
	/// <summary>Identifies a member that has no run-time behavior.</summary>
	// Token: 0x020009C9 RID: 2505
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ContractRuntimeIgnoredAttribute : Attribute
	{
	}
}
