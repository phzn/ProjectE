using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Specifies that a type contains an unmanaged array that might potentially overflow. This class cannot be inherited.</summary>
	// Token: 0x0200080B RID: 2059
	[AttributeUsage(AttributeTargets.Struct)]
	[Serializable]
	public sealed class UnsafeValueTypeAttribute : Attribute
	{
	}
}
