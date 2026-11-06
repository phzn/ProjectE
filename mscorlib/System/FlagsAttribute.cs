using System;

namespace System
{
	/// <summary>Indicates that an enumeration can be treated as a bit field; that is, a set of flags.</summary>
	// Token: 0x0200011A RID: 282
	[AttributeUsage(AttributeTargets.Enum, Inherited = false)]
	[Serializable]
	public class FlagsAttribute : Attribute
	{
	}
}
