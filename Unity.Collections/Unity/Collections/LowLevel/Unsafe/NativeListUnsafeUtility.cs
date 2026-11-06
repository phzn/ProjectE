using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000100 RID: 256
	[GenerateTestsForBurstCompatibility]
	public static class NativeListUnsafeUtility
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x000213B9 File Offset: 0x0001F5B9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T* GetUnsafePtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			return list.m_ListData->Ptr;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000213B9 File Offset: 0x0001F5B9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static T* GetUnsafeReadOnlyPtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			return list.m_ListData->Ptr;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000213C6 File Offset: 0x0001F5C6
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static void* GetInternalListDataPtrUnchecked<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref NativeList<T> list) where T : struct, ValueType
		{
			return (void*)list.m_ListData;
		}
	}
}
