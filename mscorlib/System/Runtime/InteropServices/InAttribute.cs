using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that data should be marshaled from the caller to the callee, but not back to the caller.</summary>
	// Token: 0x02000701 RID: 1793
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class InAttribute : Attribute
	{
		// Token: 0x06004093 RID: 16531 RVA: 0x000E11FD File Offset: 0x000DF3FD
		internal static Attribute GetCustomAttribute(RuntimeParameterInfo parameter)
		{
			if (!parameter.IsIn)
			{
				return null;
			}
			return new InAttribute();
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x000E120E File Offset: 0x000DF40E
		internal static bool IsDefined(RuntimeParameterInfo parameter)
		{
			return parameter.IsIn;
		}
	}
}
