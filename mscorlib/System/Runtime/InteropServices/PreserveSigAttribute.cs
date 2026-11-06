using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that the HRESULT or <see langword="retval" /> signature transformation that takes place during COM interop calls should be suppressed.</summary>
	// Token: 0x02000700 RID: 1792
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class PreserveSigAttribute : Attribute
	{
		// Token: 0x06004090 RID: 16528 RVA: 0x000E11D5 File Offset: 0x000DF3D5
		internal static Attribute GetCustomAttribute(RuntimeMethodInfo method)
		{
			if ((method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) == MethodImplAttributes.IL)
			{
				return null;
			}
			return new PreserveSigAttribute();
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x000E11EC File Offset: 0x000DF3EC
		internal static bool IsDefined(RuntimeMethodInfo method)
		{
			return (method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) > MethodImplAttributes.IL;
		}
	}
}
