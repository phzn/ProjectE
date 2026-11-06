using System;
using System.Linq;
using System.Reflection;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002E3 RID: 739
	internal struct AspectTypeInfoManager
	{
		// Token: 0x06001419 RID: 5145 RVA: 0x0004D8D0 File Offset: 0x0004BAD0
		private unsafe static void InitializeAspectTypeInfo()
		{
			Type[] allAspectTypes = TypeManager.GetAllAspectTypes();
			if (allAspectTypes == null || !allAspectTypes.Any<Type>())
			{
				return;
			}
			*AspectTypeInfoManager.s_AspectTypeInfo.Data = new AspectTypeInfo
			{
				AspectTypes = new UnsafeList<AspectType>(allAspectTypes.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
				AspectRequiredComponents = new UnsafeParallelMultiHashMap<AspectType, ComponentType>(allAspectTypes.Length, Allocator.Persistent),
				AspectExcludedComponents = new UnsafeParallelMultiHashMap<AspectType, ComponentType>(allAspectTypes.Length, Allocator.Persistent)
			};
			for (int i = 0; i < allAspectTypes.Length; i++)
			{
				AspectType aspectType = AspectType.FromTypeIndex(i);
				Type managedType = aspectType.GetManagedType();
				AspectTypeInfoManager.AddComponentRequirementsTo addComponentRequirementsTo = (AspectTypeInfoManager.AddComponentRequirementsTo)Delegate.CreateDelegate(typeof(AspectTypeInfoManager.AddComponentRequirementsTo), Activator.CreateInstance(managedType), managedType.GetMethod("AddComponentRequirementsTo", BindingFlags.Instance | BindingFlags.Public));
				UnsafeList<ComponentType> unsafeList = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeList<ComponentType> unsafeList2 = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeList<ComponentType> unsafeList3 = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeList<ComponentType> unsafeList4 = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeList<ComponentType> unsafeList5 = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeList<ComponentType> unsafeList6 = new UnsafeList<ComponentType>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				addComponentRequirementsTo(ref unsafeList);
				for (int num = 0; num != unsafeList.Length; num++)
				{
					AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectRequiredComponents.Add(aspectType, unsafeList[num]);
				}
				for (int num2 = 0; num2 != unsafeList3.Length; num2++)
				{
					AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectExcludedComponents.Add(aspectType, unsafeList3[num2]);
				}
				unsafeList.Dispose();
				unsafeList2.Dispose();
				unsafeList3.Dispose();
				unsafeList4.Dispose();
				unsafeList5.Dispose();
				unsafeList6.Dispose();
				AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectTypes.AddNoResize(aspectType);
			}
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0004DA9C File Offset: 0x0004BC9C
		internal static void Dispose()
		{
			AspectTypeInfoManager.s_AspectTypeInfo.Data.Dispose();
			if (AspectTypeInfoManager.m_ArchetypeAspectMap.IsCreated)
			{
				AspectTypeInfoManager.m_ArchetypeAspectMap.Dispose();
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0004DAC4 File Offset: 0x0004BCC4
		internal static NativeArray<AspectType> GetAspectTypesFromEntity(World world, Entity entity)
		{
			if (AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectTypes.Length == 0)
			{
				AspectTypeInfoManager.InitializeAspectTypeInfo();
			}
			EntityArchetype archetype = world.EntityManager.GetChunk(entity).Archetype;
			NativeList<AspectType> nativeList = new NativeList<AspectType>(16, Allocator.Temp);
			if (!AspectTypeInfoManager.m_ArchetypeAspectMap.IsCreated)
			{
				AspectTypeInfoManager.m_ArchetypeAspectMap = new UnsafeParallelMultiHashMap<ulong, AspectType>(16, Allocator.Persistent);
			}
			if (AspectTypeInfoManager.m_ArchetypeAspectMap.ContainsKey(archetype.StableHash))
			{
				using (UnsafeParallelMultiHashMap<ulong, AspectType>.Enumerator enumerator = AspectTypeInfoManager.m_ArchetypeAspectMap.GetValuesForKey(archetype.StableHash).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AspectType aspectType = enumerator.Current;
						nativeList.Add(in aspectType);
					}
					goto IL_0145;
				}
			}
			NativeArray<ComponentType> componentTypes = archetype.GetComponentTypes(Allocator.Temp);
			foreach (AspectType aspectType2 in AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectTypes)
			{
				if (AspectTypeInfoManager.HasAnyQueryComponents(aspectType2) && AspectTypeInfoManager.ContainAllRequiredComponents(componentTypes, aspectType2) && !AspectTypeInfoManager.ContainAnyExcludeComponents(componentTypes, aspectType2))
				{
					AspectTypeInfoManager.m_ArchetypeAspectMap.Add(archetype.StableHash, aspectType2);
					nativeList.Add(in aspectType2);
				}
			}
			componentTypes.Dispose();
			IL_0145:
			NativeArray<AspectType> nativeArray = nativeList.ToArray(Allocator.Temp);
			nativeList.Dispose();
			return nativeArray;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0004DC48 File Offset: 0x0004BE48
		private static bool HasAnyQueryComponents(AspectType aspectType)
		{
			bool flag;
			using (UnsafeParallelMultiHashMap<AspectType, ComponentType>.Enumerator valuesForKey = AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectRequiredComponents.GetValuesForKey(aspectType))
			{
				flag = valuesForKey.MoveNext();
			}
			return flag;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004DC94 File Offset: 0x0004BE94
		private static bool ContainAllRequiredComponents(NativeArray<ComponentType> componentTypes, AspectType aspectType)
		{
			foreach (ComponentType componentType in AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectRequiredComponents.GetValuesForKey(aspectType))
			{
				if (!componentTypes.Contains(componentType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004DD04 File Offset: 0x0004BF04
		private static bool ContainAnyExcludeComponents(NativeArray<ComponentType> componentTypes, AspectType aspectType)
		{
			foreach (ComponentType componentType in AspectTypeInfoManager.s_AspectTypeInfo.Data.AspectExcludedComponents.GetValuesForKey(aspectType))
			{
				if (componentTypes.Contains(componentType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400075C RID: 1884
		private static readonly SharedStatic<AspectTypeInfo> s_AspectTypeInfo = SharedStatic<AspectTypeInfo>.GetOrCreateUnsafe(0U, -8089265941129340009L, 0L);

		// Token: 0x0400075D RID: 1885
		private static UnsafeParallelMultiHashMap<ulong, AspectType> m_ArchetypeAspectMap;

		// Token: 0x020002E4 RID: 740
		private struct AspectTypeInfoTag
		{
		}

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x06001421 RID: 5153
		private delegate void AddComponentRequirementsTo(ref UnsafeList<ComponentType> all);
	}
}
