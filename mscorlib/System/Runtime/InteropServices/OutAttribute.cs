using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that data should be marshaled from callee back to caller.</summary>
	// Token: 0x02000702 RID: 1794
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OutAttribute : Attribute
	{
		// Token: 0x06004096 RID: 16534 RVA: 0x000E1216 File Offset: 0x000DF416
		internal static Attribute GetCustomAttribute(RuntimeParameterInfo parameter)
		{
			if (!parameter.IsOut)
			{
				return null;
			}
			return new OutAttribute();
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x000E1227 File Offset: 0x000DF427
		internal static bool IsDefined(RuntimeParameterInfo parameter)
		{
			return parameter.IsOut;
		}
	}
}
