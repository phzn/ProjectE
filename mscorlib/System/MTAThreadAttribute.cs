using System;

namespace System
{
	/// <summary>Indicates that the COM threading model for an application is multithreaded apartment (MTA).</summary>
	// Token: 0x02000190 RID: 400
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MTAThreadAttribute : Attribute
	{
	}
}
