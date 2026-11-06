using System;

namespace System
{
	/// <summary>Indicates that a method will allow a variable number of arguments in its invocation. This class cannot be inherited.</summary>
	// Token: 0x0200016C RID: 364
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
	public sealed class ParamArrayAttribute : Attribute
	{
	}
}
