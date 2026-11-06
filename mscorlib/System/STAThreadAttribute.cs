using System;

namespace System
{
	/// <summary>Indicates that the COM threading model for an application is single-threaded apartment (STA).</summary>
	// Token: 0x0200018F RID: 399
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class STAThreadAttribute : Attribute
	{
	}
}
