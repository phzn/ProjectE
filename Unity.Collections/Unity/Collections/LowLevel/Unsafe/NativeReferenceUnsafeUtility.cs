using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000101 RID: 257
	[GenerateTestsForBurstCompatibility]
	public static class NativeReferenceUnsafeUtility
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x000213CE File Offset: 0x0001F5CE
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T* GetUnsafePtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeReference<T> reference) where T : struct, ValueType
		{
			return (T*)reference.m_Data;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000213CE File Offset: 0x0001F5CE
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T* GetUnsafeReadOnlyPtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeReference<T> reference) where T : struct, ValueType
		{
			return (T*)reference.m_Data;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000213CE File Offset: 0x0001F5CE
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T* GetUnsafePtrWithoutChecks<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeReference<T> reference) where T : struct, ValueType
		{
			return (T*)reference.m_Data;
		}
	}
}
