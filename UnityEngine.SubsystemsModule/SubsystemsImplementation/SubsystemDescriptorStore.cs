using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000011 RID: 17
	[NativeHeader("Modules/Subsystems/SubsystemManager.h")]
	public static class SubsystemDescriptorStore
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000027DF File Offset: 0x000009DF
		[RequiredByNativeCode]
		internal static void InitializeManagedDescriptor(IntPtr ptr, IntegratedSubsystemDescriptor desc)
		{
			desc.m_Ptr = ptr;
			SubsystemDescriptorStore.s_IntegratedDescriptors.Add(desc);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000027F8 File Offset: 0x000009F8
		[RequiredByNativeCode]
		internal static void ClearManagedDescriptors()
		{
			foreach (IntegratedSubsystemDescriptor integratedSubsystemDescriptor in SubsystemDescriptorStore.s_IntegratedDescriptors)
			{
				integratedSubsystemDescriptor.m_Ptr = IntPtr.Zero;
			}
			SubsystemDescriptorStore.s_IntegratedDescriptors.Clear();
		}

		// Token: 0x06000051 RID: 81
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReportSingleSubsystemAnalytics(string id);

		// Token: 0x06000052 RID: 82 RVA: 0x0000285C File Offset: 0x00000A5C
		public static void RegisterDescriptor(SubsystemDescriptorWithProvider descriptor)
		{
			descriptor.ThrowIfInvalid();
			SubsystemDescriptorStore.RegisterDescriptor<SubsystemDescriptorWithProvider, SubsystemDescriptorWithProvider>(descriptor, SubsystemDescriptorStore.s_StandaloneDescriptors);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002874 File Offset: 0x00000A74
		internal static void GetAllSubsystemDescriptors(List<ISubsystemDescriptor> descriptors)
		{
			descriptors.Clear();
			int num = SubsystemDescriptorStore.s_IntegratedDescriptors.Count + SubsystemDescriptorStore.s_StandaloneDescriptors.Count + SubsystemDescriptorStore.s_DeprecatedDescriptors.Count;
			bool flag = descriptors.Capacity < num;
			if (flag)
			{
				descriptors.Capacity = num;
			}
			SubsystemDescriptorStore.AddDescriptorSubset<IntegratedSubsystemDescriptor>(SubsystemDescriptorStore.s_IntegratedDescriptors, descriptors);
			SubsystemDescriptorStore.AddDescriptorSubset<SubsystemDescriptorWithProvider>(SubsystemDescriptorStore.s_StandaloneDescriptors, descriptors);
			SubsystemDescriptorStore.AddDescriptorSubset<SubsystemDescriptor>(SubsystemDescriptorStore.s_DeprecatedDescriptors, descriptors);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000028E4 File Offset: 0x00000AE4
		private static void AddDescriptorSubset<TBaseTypeInList>(List<TBaseTypeInList> copyFrom, List<ISubsystemDescriptor> copyTo) where TBaseTypeInList : ISubsystemDescriptor
		{
			foreach (TBaseTypeInList tbaseTypeInList in copyFrom)
			{
				copyTo.Add(tbaseTypeInList);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000293C File Offset: 0x00000B3C
		internal static void GetSubsystemDescriptors<T>(List<T> descriptors) where T : ISubsystemDescriptor
		{
			descriptors.Clear();
			SubsystemDescriptorStore.AddDescriptorSubset<IntegratedSubsystemDescriptor, T>(SubsystemDescriptorStore.s_IntegratedDescriptors, descriptors);
			SubsystemDescriptorStore.AddDescriptorSubset<SubsystemDescriptorWithProvider, T>(SubsystemDescriptorStore.s_StandaloneDescriptors, descriptors);
			SubsystemDescriptorStore.AddDescriptorSubset<SubsystemDescriptor, T>(SubsystemDescriptorStore.s_DeprecatedDescriptors, descriptors);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000296C File Offset: 0x00000B6C
		private static void AddDescriptorSubset<TBaseTypeInList, TQueryType>(List<TBaseTypeInList> copyFrom, List<TQueryType> copyTo) where TBaseTypeInList : ISubsystemDescriptor where TQueryType : ISubsystemDescriptor
		{
			foreach (TBaseTypeInList tbaseTypeInList in copyFrom)
			{
				TQueryType tqueryType;
				bool flag;
				if (tbaseTypeInList is TQueryType)
				{
					tqueryType = tbaseTypeInList as TQueryType;
					flag = true;
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					copyTo.Add(tqueryType);
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000029E8 File Offset: 0x00000BE8
		internal static void RegisterDescriptor<TDescriptor, TBaseTypeInList>(TDescriptor descriptor, List<TBaseTypeInList> storeInList) where TDescriptor : TBaseTypeInList where TBaseTypeInList : ISubsystemDescriptor
		{
			for (int i = 0; i < storeInList.Count; i++)
			{
				TBaseTypeInList tbaseTypeInList = storeInList[i];
				bool flag = tbaseTypeInList.id != descriptor.id;
				if (!flag)
				{
					Debug.LogWarning("Registering subsystem descriptor with duplicate ID '" + descriptor.id + "' - overwriting previous entry.");
					storeInList[i] = (TBaseTypeInList)((object)descriptor);
					return;
				}
			}
			SubsystemDescriptorStore.ReportSingleSubsystemAnalytics(descriptor.id);
			storeInList.Add((TBaseTypeInList)((object)descriptor));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002A95 File Offset: 0x00000C95
		internal static void RegisterDeprecatedDescriptor(SubsystemDescriptor descriptor)
		{
			SubsystemDescriptorStore.RegisterDescriptor<SubsystemDescriptor, SubsystemDescriptor>(descriptor, SubsystemDescriptorStore.s_DeprecatedDescriptors);
		}

		// Token: 0x0400000E RID: 14
		private static List<IntegratedSubsystemDescriptor> s_IntegratedDescriptors = new List<IntegratedSubsystemDescriptor>();

		// Token: 0x0400000F RID: 15
		private static List<SubsystemDescriptorWithProvider> s_StandaloneDescriptors = new List<SubsystemDescriptorWithProvider>();

		// Token: 0x04000010 RID: 16
		private static List<SubsystemDescriptor> s_DeprecatedDescriptors = new List<SubsystemDescriptor>();
	}
}
