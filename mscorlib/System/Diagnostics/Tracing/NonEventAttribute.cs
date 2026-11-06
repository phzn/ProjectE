using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Identifies a method that is not generating an event.</summary>
	// Token: 0x020009FD RID: 2557
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class NonEventAttribute : Attribute
	{
	}
}
