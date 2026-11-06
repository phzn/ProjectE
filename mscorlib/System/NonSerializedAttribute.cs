using System;

namespace System
{
	/// <summary>Indicates that a field of a serializable class should not be serialized. This class cannot be inherited.</summary>
	// Token: 0x0200015E RID: 350
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class NonSerializedAttribute : Attribute
	{
	}
}
