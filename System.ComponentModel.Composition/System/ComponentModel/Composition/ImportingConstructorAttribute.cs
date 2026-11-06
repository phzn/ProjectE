using System;

namespace System.ComponentModel.Composition
{
	/// <summary>Specifies which constructor should be used when creating a part.</summary>
	// Token: 0x02000049 RID: 73
	[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class ImportingConstructorAttribute : Attribute
	{
	}
}
