using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that a parameter is optional.</summary>
	// Token: 0x02000703 RID: 1795
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalAttribute : Attribute
	{
		// Token: 0x06004099 RID: 16537 RVA: 0x000E122F File Offset: 0x000DF42F
		internal static Attribute GetCustomAttribute(RuntimeParameterInfo parameter)
		{
			if (!parameter.IsOptional)
			{
				return null;
			}
			return new OptionalAttribute();
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x000E1240 File Offset: 0x000DF440
		internal static bool IsDefined(RuntimeParameterInfo parameter)
		{
			return parameter.IsOptional;
		}
	}
}
