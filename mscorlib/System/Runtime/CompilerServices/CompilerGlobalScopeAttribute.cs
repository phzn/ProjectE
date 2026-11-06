using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that a class should be treated as if it has global scope.</summary>
	// Token: 0x020007E3 RID: 2019
	[AttributeUsage(AttributeTargets.Class)]
	[Serializable]
	public class CompilerGlobalScopeAttribute : Attribute
	{
	}
}
