using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020008F7 RID: 2295
	internal struct MonoMethodInfo
	{
		// Token: 0x06004D44 RID: 19780
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_method_info(IntPtr handle, out MonoMethodInfo info);

		// Token: 0x06004D45 RID: 19781
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int get_method_attributes(IntPtr handle);

		// Token: 0x06004D46 RID: 19782 RVA: 0x000F3FE4 File Offset: 0x000F21E4
		internal static MonoMethodInfo GetMethodInfo(IntPtr handle)
		{
			MonoMethodInfo monoMethodInfo;
			MonoMethodInfo.get_method_info(handle, out monoMethodInfo);
			return monoMethodInfo;
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x000F3FFA File Offset: 0x000F21FA
		internal static Type GetDeclaringType(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).parent;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x000F4007 File Offset: 0x000F2207
		internal static Type GetReturnType(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).ret;
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x000F4014 File Offset: 0x000F2214
		internal static MethodAttributes GetAttributes(IntPtr handle)
		{
			return (MethodAttributes)MonoMethodInfo.get_method_attributes(handle);
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x000F401C File Offset: 0x000F221C
		internal static CallingConventions GetCallingConvention(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).callconv;
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x000F4029 File Offset: 0x000F2229
		internal static MethodImplAttributes GetMethodImplementationFlags(IntPtr handle)
		{
			return MonoMethodInfo.GetMethodInfo(handle).iattrs;
		}

		// Token: 0x06004D4C RID: 19788
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParameterInfo[] get_parameter_info(IntPtr handle, MemberInfo member);

		// Token: 0x06004D4D RID: 19789 RVA: 0x000F4036 File Offset: 0x000F2236
		internal static ParameterInfo[] GetParametersInfo(IntPtr handle, MemberInfo member)
		{
			return MonoMethodInfo.get_parameter_info(handle, member);
		}

		// Token: 0x06004D4E RID: 19790
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MarshalAsAttribute get_retval_marshal(IntPtr handle);

		// Token: 0x06004D4F RID: 19791 RVA: 0x000F403F File Offset: 0x000F223F
		internal static ParameterInfo GetReturnParameterInfo(RuntimeMethodInfo method)
		{
			return RuntimeParameterInfo.New(MonoMethodInfo.GetReturnType(method.mhandle), method, MonoMethodInfo.get_retval_marshal(method.mhandle));
		}

		// Token: 0x04003055 RID: 12373
		private Type parent;

		// Token: 0x04003056 RID: 12374
		private Type ret;

		// Token: 0x04003057 RID: 12375
		internal MethodAttributes attrs;

		// Token: 0x04003058 RID: 12376
		internal MethodImplAttributes iattrs;

		// Token: 0x04003059 RID: 12377
		private CallingConventions callconv;
	}
}
