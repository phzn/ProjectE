using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that any private members contained in an assembly's types are not available to reflection.</summary>
	// Token: 0x020007EE RID: 2030
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class DisablePrivateReflectionAttribute : Attribute
	{
	}
}
