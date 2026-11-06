using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Allows an unmanaged method to call a managed method.</summary>
	// Token: 0x020006E4 RID: 1764
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class AllowReversePInvokeCallsAttribute : Attribute
	{
	}
}
