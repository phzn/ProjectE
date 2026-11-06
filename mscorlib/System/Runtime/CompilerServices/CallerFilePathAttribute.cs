using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Allows you to obtain the full path of the source file that contains the caller. This is the file path at the time of compile.</summary>
	// Token: 0x020007DF RID: 2015
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerFilePathAttribute : Attribute
	{
	}
}
