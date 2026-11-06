using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Allows you to obtain the method or property name of the caller to the method.</summary>
	// Token: 0x020007E1 RID: 2017
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerMemberNameAttribute : Attribute
	{
	}
}
