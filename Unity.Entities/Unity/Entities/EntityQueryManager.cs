using System;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000227 RID: 551
	[GenerateTestsForBurstCompatibility]
	internal struct EntityQueryManager
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x0003F2BC File Offset: 0x0003D4BC
		public unsafe static void Create(EntityQueryManager* queryManager, ComponentDependencyManager* dependencyManager)
		{
			queryManager->m_DependencyManager = dependencyManager;
			queryManager->m_EntityQueryDataChunkAllocator = new BlockAllocator(AllocatorManager.Persistent, 16777216);
			*UnsafeUtility.As<UntypedUnsafeParallelHashMap, UnsafeParallelMultiHashMap<int, int>>(ref queryManager->m_EntityQueryDataCacheUntyped) = new UnsafeParallelMultiHashMap<int, int>(1024, Allocator.Persistent);
			queryManager->m_EntityQueryDatas = new UnsafePtrList<EntityQueryData>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			queryManager->m_EntityQueryMasksAllocated = 0;
			queryManager->m_disabledTypeIndex = TypeManager.GetTypeIndex<Disabled>();
			queryManager->m_prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>();
			queryManager->m_systemInstanceTypeIndex = TypeManager.GetTypeIndex<SystemInstance>();
			queryManager->m_chunkHeaderTypeIndex = TypeManager.GetTypeIndex<ChunkHeader>();
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003F34B File Offset: 0x0003D54B
		public unsafe static void Destroy(EntityQueryManager* manager)
		{
			manager->Dispose();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0003F354 File Offset: 0x0003D554
		private unsafe void Dispose()
		{
			UnsafeUtility.As<UntypedUnsafeParallelHashMap, UnsafeParallelMultiHashMap<int, int>>(ref this.m_EntityQueryDataCacheUntyped).Dispose();
			for (int i = 0; i < this.m_EntityQueryDatas.Length; i++)
			{
				((IntPtr*)(this.m_EntityQueryDatas.Ptr + (IntPtr)i * (IntPtr)sizeof(EntityQueryData*) / (IntPtr)sizeof(EntityQueryData*)))->Dispose();
			}
			this.m_EntityQueryDatas.Dispose();
			this.m_EntityQueryDataChunkAllocator.Dispose();
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0003F3B8 File Offset: 0x0003D5B8
		private unsafe ArchetypeQuery* CreateArchetypeQueries(ref UnsafeScratchAllocator unsafeScratchAllocator, ComponentType* requiredTypes, int count)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			for (int num = 0; num != count; num++)
			{
				if (requiredTypes[num].AccessModeType == ComponentType.AccessMode.Exclude)
				{
					ComponentType componentType = ComponentType.ReadOnly(requiredTypes[num].TypeIndex);
					entityQueryBuilder.WithNone(&componentType, 1);
				}
				else
				{
					entityQueryBuilder.WithAll(requiredTypes + num, 1);
				}
			}
			entityQueryBuilder.FinalizeQueryInternal();
			ArchetypeQuery* ptr = this.CreateArchetypeQueries(ref unsafeScratchAllocator, in entityQueryBuilder);
			entityQueryBuilder.Dispose();
			return ptr;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003F444 File Offset: 0x0003D644
		internal unsafe void ConstructTypeArray(ref UnsafeScratchAllocator unsafeScratchAllocator, UnsafeList<ComponentType> types, out TypeIndex* outTypes, out byte* outAccessModes, out int outLength)
		{
			if (types.Length == 0)
			{
				outTypes = (IntPtr)((UIntPtr)0);
				outAccessModes = (IntPtr)((UIntPtr)0);
				outLength = 0;
				return;
			}
			outLength = types.Length;
			outTypes = unsafeScratchAllocator.Allocate<TypeIndex>(types.Length);
			outAccessModes = unsafeScratchAllocator.Allocate<byte>(types.Length);
			ComponentType* ptr;
			checked
			{
				ptr = stackalloc ComponentType[unchecked((UIntPtr)types.Length) * (UIntPtr)sizeof(ComponentType)];
			}
			for (int i = 0; i < types.Length; i++)
			{
				SortingUtilities.InsertSorted(ptr, i, types[i]);
			}
			for (int num = 0; num != types.Length; num++)
			{
				*(outTypes + (IntPtr)num * (IntPtr)sizeof(TypeIndex)) = ptr[num].TypeIndex;
				*(outAccessModes + (IntPtr)num) = (byte)ptr[num].AccessModeType;
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003F510 File Offset: 0x0003D710
		private unsafe void IncludeDependentWriteGroups(ComponentType type, ref UnsafeList<ComponentType> explicitList)
		{
			if (type.AccessModeType != ComponentType.AccessMode.ReadOnly)
			{
				return;
			}
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(type.TypeIndex);
			TypeIndex* writeGroups = TypeManager.GetWriteGroups(in typeInfo);
			int writeGroupCount = typeInfo.WriteGroupCount;
			for (int i = 0; i < writeGroupCount; i++)
			{
				ComponentType writeGroupReadOnlyComponentType = EntityQueryManager.GetWriteGroupReadOnlyComponentType(writeGroups, i);
				if (!explicitList.Contains(writeGroupReadOnlyComponentType))
				{
					explicitList.Add(in writeGroupReadOnlyComponentType);
					this.IncludeDependentWriteGroups(writeGroupReadOnlyComponentType, ref explicitList);
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0003F57C File Offset: 0x0003D77C
		private unsafe static ComponentType GetWriteGroupReadOnlyComponentType(TypeIndex* writeGroupTypes, int i)
		{
			return ComponentType.ReadOnly(TypeManager.GetTypeInfo(writeGroupTypes[i]).TypeIndex);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003F5A0 File Offset: 0x0003D7A0
		private unsafe void ExcludeWriteGroups(ComponentType type, ref UnsafeList<ComponentType> noneList, UnsafeList<ComponentType> explicitList)
		{
			if (type.AccessModeType == ComponentType.AccessMode.ReadOnly)
			{
				return;
			}
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(type.TypeIndex);
			TypeIndex* writeGroups = TypeManager.GetWriteGroups(in typeInfo);
			int writeGroupCount = typeInfo.WriteGroupCount;
			for (int i = 0; i < writeGroupCount; i++)
			{
				ComponentType writeGroupReadOnlyComponentType = EntityQueryManager.GetWriteGroupReadOnlyComponentType(writeGroups, i);
				if (!noneList.Contains(writeGroupReadOnlyComponentType) && !explicitList.Contains(writeGroupReadOnlyComponentType))
				{
					noneList.Add(in writeGroupReadOnlyComponentType);
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0003F610 File Offset: 0x0003D810
		private unsafe ArchetypeQuery* CreateArchetypeQueries(ref UnsafeScratchAllocator unsafeScratchAllocator, in EntityQueryBuilder queryBuilder)
		{
			UnsafeList<ComponentType> typeData = queryBuilder._builderDataPtr->_typeData;
			UnsafeList<EntityQueryBuilder.QueryTypes> indexData = queryBuilder._builderDataPtr->_indexData;
			ArchetypeQuery* ptr = (ArchetypeQuery*)unsafeScratchAllocator.Allocate(sizeof(ArchetypeQuery) * indexData.Length, UnsafeUtility.AlignOf<ArchetypeQuery>());
			UnsafeList<ComponentType> unsafeList = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<ComponentType> unsafeList2 = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<ComponentType> unsafeList3 = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<ComponentType> unsafeList4 = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<ComponentType> unsafeList5 = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<ComponentType> unsafeList6 = new UnsafeList<ComponentType>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int num = 0; num != indexData.Length; num++)
			{
				ptr[num].Options = indexData[num].Options;
				EntityQueryBuilder.ComponentIndexArray all = indexData[num].All;
				for (int i = (int)all.Index; i < (int)(all.Index + all.Count); i++)
				{
					ComponentType componentType = typeData[i];
					unsafeList.Add(in componentType);
					if (typeData[i].TypeIndex == this.m_disabledTypeIndex)
					{
						ptr[num].Options |= EntityQueryOptions.IncludeDisabledEntities;
					}
					else if (typeData[i].TypeIndex == this.m_prefabTypeIndex)
					{
						ptr[num].Options |= EntityQueryOptions.IncludePrefab;
					}
					else if (typeData[i].TypeIndex == this.m_chunkHeaderTypeIndex)
					{
						ptr[num].Options |= EntityQueryOptions.IncludeMetaChunks;
					}
					else if (typeData[i].TypeIndex == this.m_systemInstanceTypeIndex)
					{
						ptr[num].Options |= EntityQueryOptions.IncludeSystems;
					}
				}
				EntityQueryBuilder.ComponentIndexArray any = indexData[num].Any;
				for (int j = (int)any.Index; j < (int)(any.Index + any.Count); j++)
				{
					ComponentType componentType = typeData[j];
					unsafeList2.Add(in componentType);
				}
				EntityQueryBuilder.ComponentIndexArray none = indexData[num].None;
				for (int k = (int)none.Index; k < (int)(none.Index + none.Count); k++)
				{
					ComponentType componentType = typeData[k];
					unsafeList3.Add(in componentType);
				}
				EntityQueryBuilder.ComponentIndexArray disabled = indexData[num].Disabled;
				for (int l = (int)disabled.Index; l < (int)(disabled.Index + disabled.Count); l++)
				{
					ComponentType componentType = typeData[l];
					unsafeList4.Add(in componentType);
				}
				EntityQueryBuilder.ComponentIndexArray absent = indexData[num].Absent;
				for (int m = (int)absent.Index; m < (int)(absent.Index + absent.Count); m++)
				{
					ComponentType componentType = typeData[m];
					unsafeList5.Add(in componentType);
				}
				EntityQueryBuilder.ComponentIndexArray present = indexData[num].Present;
				for (int n = (int)present.Index; n < (int)(present.Index + present.Count); n++)
				{
					ComponentType componentType = typeData[n];
					unsafeList6.Add(in componentType);
				}
				if ((indexData[num].Options & EntityQueryOptions.FilterWriteGroup) > EntityQueryOptions.Default)
				{
					UnsafeList<ComponentType> unsafeList7 = new UnsafeList<ComponentType>(unsafeList.Length + unsafeList2.Length + unsafeList3.Length + unsafeList4.Length + unsafeList5.Length + unsafeList6.Length + 16, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
					unsafeList7.AddRange(unsafeList);
					unsafeList7.AddRange(unsafeList2);
					unsafeList7.AddRange(unsafeList3);
					unsafeList7.AddRange(unsafeList4);
					unsafeList7.AddRange(unsafeList5);
					unsafeList7.AddRange(unsafeList6);
					for (int num2 = 0; num2 < unsafeList2.Length; num2++)
					{
						this.IncludeDependentWriteGroups(unsafeList2[num2], ref unsafeList7);
					}
					for (int num3 = 0; num3 < unsafeList.Length; num3++)
					{
						this.IncludeDependentWriteGroups(unsafeList[num3], ref unsafeList7);
					}
					for (int num4 = 0; num4 < unsafeList4.Length; num4++)
					{
						this.IncludeDependentWriteGroups(unsafeList4[num4], ref unsafeList7);
					}
					for (int num5 = 0; num5 < unsafeList6.Length; num5++)
					{
						this.IncludeDependentWriteGroups(unsafeList6[num5], ref unsafeList7);
					}
					for (int num6 = 0; num6 < unsafeList2.Length; num6++)
					{
						this.ExcludeWriteGroups(unsafeList2[num6], ref unsafeList3, unsafeList7);
					}
					for (int num7 = 0; num7 < unsafeList.Length; num7++)
					{
						this.ExcludeWriteGroups(unsafeList[num7], ref unsafeList3, unsafeList7);
					}
					for (int num8 = 0; num8 < unsafeList4.Length; num8++)
					{
						this.ExcludeWriteGroups(unsafeList4[num8], ref unsafeList3, unsafeList7);
					}
					for (int num9 = 0; num9 < unsafeList6.Length; num9++)
					{
						this.ExcludeWriteGroups(unsafeList6[num9], ref unsafeList3, unsafeList7);
					}
					unsafeList7.Dispose();
				}
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList3, out ptr[num].None, out ptr[num].NoneAccessMode, out ptr[num].NoneCount);
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList, out ptr[num].All, out ptr[num].AllAccessMode, out ptr[num].AllCount);
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList2, out ptr[num].Any, out ptr[num].AnyAccessMode, out ptr[num].AnyCount);
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList4, out ptr[num].Disabled, out ptr[num].DisabledAccessMode, out ptr[num].DisabledCount);
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList5, out ptr[num].Absent, out ptr[num].AbsentAccessMode, out ptr[num].AbsentCount);
				this.ConstructTypeArray(ref unsafeScratchAllocator, unsafeList6, out ptr[num].Present, out ptr[num].PresentAccessMode, out ptr[num].PresentCount);
				ulong num10 = 0UL;
				for (int num11 = 0; num11 < ptr[num].AllCount; num11++)
				{
					num10 |= TypeManager.GetTypeInfo(ptr[num].All[num11]).BloomFilterMask;
				}
				ptr[num].AllBloomFilterMask = num10;
				unsafeList.Clear();
				unsafeList2.Clear();
				unsafeList3.Clear();
				unsafeList4.Clear();
				unsafeList5.Clear();
				unsafeList6.Clear();
			}
			unsafeList.Dispose();
			unsafeList2.Dispose();
			unsafeList3.Dispose();
			unsafeList4.Dispose();
			unsafeList5.Dispose();
			unsafeList6.Dispose();
			return ptr;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003FD68 File Offset: 0x0003DF68
		internal unsafe static bool CompareQueryArray(in EntityQueryBuilder builder, EntityQueryBuilder.ComponentIndexArray arr, TypeIndex* typeArray, byte* accessModeArray, int typeArrayCount)
		{
			int count = (int)arr.Count;
			if (typeArrayCount != count)
			{
				return false;
			}
			ComponentType* ptr;
			UnsafeList<ComponentType> typeData;
			checked
			{
				ptr = stackalloc ComponentType[unchecked((UIntPtr)count) * (UIntPtr)sizeof(ComponentType)];
				typeData = builder._builderDataPtr->_typeData;
			}
			for (int i = 0; i < count; i++)
			{
				SortingUtilities.InsertSorted(ptr, i, typeData[(int)arr.Index + i]);
			}
			for (int j = 0; j < count; j++)
			{
				if (typeArray[j] != ptr[j].TypeIndex || accessModeArray[j] != (byte)ptr[j].AccessModeType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003FE14 File Offset: 0x0003E014
		public unsafe static bool CompareQuery(in EntityQueryBuilder queryBuilder, EntityQueryData* queryData)
		{
			EntityQueryBuilder entityQueryBuilder = queryBuilder;
			entityQueryBuilder.FinalizeQueryInternal();
			UnsafeList<EntityQueryBuilder.QueryTypes> indexData = queryBuilder._builderDataPtr->_indexData;
			int length = indexData.Length;
			if (queryData->ArchetypeQueryCount != length)
			{
				return false;
			}
			for (int num = 0; num != length; num++)
			{
				ref ArchetypeQuery ptr = ref queryData->ArchetypeQueries[num];
				EntityQueryBuilder.QueryTypes queryTypes = indexData[num];
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.All, ptr.All, ptr.AllAccessMode, ptr.AllCount))
				{
					return false;
				}
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.None, ptr.None, ptr.NoneAccessMode, ptr.NoneCount))
				{
					return false;
				}
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.Any, ptr.Any, ptr.AnyAccessMode, ptr.AnyCount))
				{
					return false;
				}
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.Disabled, ptr.Disabled, ptr.DisabledAccessMode, ptr.DisabledCount))
				{
					return false;
				}
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.Absent, ptr.Absent, ptr.AbsentAccessMode, ptr.AbsentCount))
				{
					return false;
				}
				if (!EntityQueryManager.CompareQueryArray(in queryBuilder, queryTypes.Present, ptr.Present, ptr.PresentAccessMode, ptr.PresentCount))
				{
					return false;
				}
				if (queryTypes.Options != ptr.Options)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003FF74 File Offset: 0x0003E174
		private unsafe int IntersectSortedComponentIndexArrays(ComponentType* arrayA, int arrayACount, ComponentType* arrayB, int arrayBCount, ComponentType* outArray)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num2 < arrayACount && num3 < arrayBCount)
			{
				if (arrayA[num2] < arrayB[num3])
				{
					num2++;
				}
				else if (arrayB[num3] < arrayA[num2])
				{
					num3++;
				}
				else
				{
					outArray[num] = arrayB[num3];
					num++;
					num2++;
					num3++;
				}
			}
			return num;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00040020 File Offset: 0x0003E220
		private unsafe ComponentType* CalculateRequiredComponentsFromQuery(ref UnsafeScratchAllocator allocator, ArchetypeQuery* queries, int queryCount, out int outRequiredComponentsCount)
		{
			int num = queries->AllCount + queries->DisabledCount + queries->PresentCount + queries->NoneCount;
			ComponentType* ptr = (ComponentType*)allocator.Allocate<ComponentType>(num + 1);
			*ptr = ComponentType.ReadWrite<Entity>();
			if (num == 0)
			{
				outRequiredComponentsCount = 1;
				return ptr;
			}
			ComponentType* ptr2 = ptr + 1;
			int num2 = 0;
			for (int i = 0; i < queries->AllCount; i++)
			{
				ptr2[(IntPtr)(num2++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
				{
					TypeIndex = queries->All[i],
					AccessModeType = (ComponentType.AccessMode)queries->AllAccessMode[i]
				};
			}
			for (int j = 0; j < queries->DisabledCount; j++)
			{
				ptr2[(IntPtr)(num2++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
				{
					TypeIndex = queries->Disabled[j],
					AccessModeType = (ComponentType.AccessMode)queries->DisabledAccessMode[j]
				};
			}
			for (int k = 0; k < queries->PresentCount; k++)
			{
				ptr2[(IntPtr)(num2++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
				{
					TypeIndex = queries->Present[k],
					AccessModeType = (ComponentType.AccessMode)queries->PresentAccessMode[k]
				};
			}
			for (int l = 0; l < queries->NoneCount; l++)
			{
				if (TypeManager.IsEnableable(queries->None[l]))
				{
					ptr2[(IntPtr)(num2++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
					{
						TypeIndex = queries->None[l],
						AccessModeType = ComponentType.AccessMode.Exclude
					};
				}
			}
			NativeSortExtension.Sort<ComponentType>(ptr2, num2);
			int num3 = 0;
			for (int m = 1; m < queryCount; m++)
			{
				num3 = math.max(num3, queries[m].AllCount + queries[m].DisabledCount + queries[m].PresentCount + queries[m].NoneCount);
			}
			ComponentType* ptr3 = (ComponentType*)allocator.Allocate<ComponentType>(num3);
			for (int n = 1; n < queryCount; n++)
			{
				int num4 = 0;
				for (int num5 = 0; num5 < queries[n].AllCount; num5++)
				{
					ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
					{
						TypeIndex = queries[n].All[num5],
						AccessModeType = (ComponentType.AccessMode)queries[n].AllAccessMode[num5]
					};
				}
				for (int num6 = 0; num6 < queries[n].DisabledCount; num6++)
				{
					ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
					{
						TypeIndex = queries[n].Disabled[num6],
						AccessModeType = (ComponentType.AccessMode)queries[n].DisabledAccessMode[num6]
					};
				}
				for (int num7 = 0; num7 < queries[n].PresentCount; num7++)
				{
					ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
					{
						TypeIndex = queries[n].Present[num7],
						AccessModeType = (ComponentType.AccessMode)queries[n].PresentAccessMode[num7]
					};
				}
				for (int num8 = 0; num8 < queries->NoneCount; num8++)
				{
					if (TypeManager.IsEnableable(queries->None[num8]))
					{
						ptr3[(IntPtr)(num4++) * (IntPtr)sizeof(ComponentType)] = new ComponentType
						{
							TypeIndex = queries->None[num8],
							AccessModeType = ComponentType.AccessMode.Exclude
						};
					}
				}
				NativeSortExtension.Sort<ComponentType>(ptr3, num4);
				num2 = this.IntersectSortedComponentIndexArrays(ptr2, num2, ptr3, num4, ptr2);
			}
			outRequiredComponentsCount = num2 + 1;
			return ptr;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000404BC File Offset: 0x0003E6BC
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		internal unsafe static void ConvertToEntityQueryBuilder(ref EntityQueryBuilder builder, EntityQueryDesc[] queryDesc)
		{
			for (int num = 0; num != queryDesc.Length; num++)
			{
				ref EntityQueryDesc ptr = ref queryDesc[num];
				ComponentType[] array;
				ComponentType* ptr2;
				if ((array = ptr.All) == null || array.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array[0];
				}
				builder.WithAll(ptr2, ptr.All.Length);
				array = null;
				ComponentType* ptr3;
				if ((array = ptr.Any) == null || array.Length == 0)
				{
					ptr3 = null;
				}
				else
				{
					ptr3 = &array[0];
				}
				builder.WithAny(ptr3, ptr.Any.Length);
				array = null;
				ComponentType* ptr4;
				if ((array = ptr.None) == null || array.Length == 0)
				{
					ptr4 = null;
				}
				else
				{
					ptr4 = &array[0];
				}
				builder.WithNone(ptr4, ptr.None.Length);
				array = null;
				ComponentType* ptr5;
				if ((array = ptr.Disabled) == null || array.Length == 0)
				{
					ptr5 = null;
				}
				else
				{
					ptr5 = &array[0];
				}
				builder.WithDisabled(ptr5, ptr.Disabled.Length);
				array = null;
				ComponentType* ptr6;
				if ((array = ptr.Absent) == null || array.Length == 0)
				{
					ptr6 = null;
				}
				else
				{
					ptr6 = &array[0];
				}
				builder.WithAbsent(ptr6, ptr.Absent.Length);
				array = null;
				ComponentType* ptr7;
				if ((array = ptr.Present) == null || array.Length == 0)
				{
					ptr7 = null;
				}
				else
				{
					ptr7 = &array[0];
				}
				builder.WithPresent(ptr7, ptr.Present.Length);
				array = null;
				builder.WithOptions(ptr.Options);
				builder.FinalizeQueryInternal();
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00040630 File Offset: 0x0003E830
		public unsafe EntityQuery CreateEntityQuery(EntityDataAccess* access, EntityQueryBuilder query)
		{
			query.FinalizeQueryInternal();
			byte* ptr = stackalloc byte[(UIntPtr)2048];
			UnsafeScratchAllocator unsafeScratchAllocator = new UnsafeScratchAllocator((void*)ptr, 2048);
			ArchetypeQuery* ptr2 = this.CreateArchetypeQueries(ref unsafeScratchAllocator, in query);
			UnsafeList<EntityQueryBuilder.QueryTypes> indexData = query._builderDataPtr->_indexData;
			int num;
			ComponentType* ptr3 = this.CalculateRequiredComponentsFromQuery(ref unsafeScratchAllocator, ptr2, indexData.Length, out num);
			return this.CreateEntityQuery(access, ptr2, indexData.Length, ptr3, num);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004069C File Offset: 0x0003E89C
		public unsafe EntityQuery CreateEntityQuery(EntityDataAccess* access, ComponentType* inRequiredComponents, int inRequiredComponentsCount)
		{
			byte* ptr = stackalloc byte[(UIntPtr)1024];
			UnsafeScratchAllocator unsafeScratchAllocator = new UnsafeScratchAllocator((void*)ptr, 1024);
			ArchetypeQuery* ptr2 = this.CreateArchetypeQueries(ref unsafeScratchAllocator, inRequiredComponents, inRequiredComponentsCount);
			ComponentType* ptr3 = (ComponentType*)unsafeScratchAllocator.Allocate<ComponentType>(inRequiredComponentsCount + 1);
			*ptr3 = ComponentType.ReadWrite<Entity>();
			for (int num = 0; num != inRequiredComponentsCount; num++)
			{
				SortingUtilities.InsertSorted(ptr3 + 1, num, inRequiredComponents[num]);
			}
			int num2 = inRequiredComponentsCount + 1;
			return this.CreateEntityQuery(access, ptr2, 1, ptr3, num2);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00040724 File Offset: 0x0003E924
		private unsafe bool Matches(EntityQueryData* queryData, ArchetypeQuery* archetypeQueries, int archetypeQueryCount, ComponentType* requiredComponents, int requiredComponentsCount)
		{
			if (requiredComponentsCount != queryData->RequiredComponentsCount)
			{
				return false;
			}
			if (archetypeQueryCount != queryData->ArchetypeQueryCount)
			{
				return false;
			}
			if (requiredComponentsCount > 0 && UnsafeUtility.MemCmp((void*)requiredComponents, (void*)queryData->RequiredComponents, (long)(sizeof(ComponentType) * requiredComponentsCount)) != 0)
			{
				return false;
			}
			for (int i = 0; i < archetypeQueryCount; i++)
			{
				if (!archetypeQueries[i].Equals(queryData->ArchetypeQueries[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000407A0 File Offset: 0x0003E9A0
		private unsafe void* ChunkAllocate<T>(int count = 1, void* source = null) where T : struct
		{
			int num = count * UnsafeUtility.SizeOf<T>();
			if (num == 0)
			{
				return null;
			}
			byte* ptr = this.m_EntityQueryDataChunkAllocator.Allocate(num, UnsafeUtility.AlignOf<T>());
			if (source != null)
			{
				UnsafeUtility.MemCpy((void*)ptr, source, (long)num);
			}
			return (void*)ptr;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000407DC File Offset: 0x0003E9DC
		public unsafe EntityQuery CreateEntityQuery(EntityDataAccess* access, ArchetypeQuery* archetypeQueries, int archetypeQueryCount, ComponentType* requiredComponents, int requiredComponentCount)
		{
			int num = (int)math.hash((void*)requiredComponents, requiredComponentCount * sizeof(ComponentType), 0U);
			for (int i = 0; i < archetypeQueryCount; i++)
			{
				num = (num * 397) ^ archetypeQueries[i].GetHashCode();
			}
			EntityQueryData* ptr = null;
			ref UnsafeParallelMultiHashMap<int, int> ptr2 = ref UnsafeUtility.As<UntypedUnsafeParallelHashMap, UnsafeParallelMultiHashMap<int, int>>(ref this.m_EntityQueryDataCacheUntyped);
			int num2;
			NativeParallelMultiHashMapIterator<int> nativeParallelMultiHashMapIterator;
			if (ptr2.TryGetFirstValue(num, out num2, out nativeParallelMultiHashMapIterator))
			{
				EntityQueryData* ptr3;
				for (;;)
				{
					ptr3 = *(IntPtr*)(this.m_EntityQueryDatas.Ptr + (IntPtr)num2 * (IntPtr)sizeof(EntityQueryData*) / (IntPtr)sizeof(EntityQueryData*));
					if (this.Matches(ptr3, archetypeQueries, archetypeQueryCount, requiredComponents, requiredComponentCount))
					{
						break;
					}
					if (!ptr2.TryGetNextValue(out num2, ref nativeParallelMultiHashMapIterator))
					{
						goto IL_0097;
					}
				}
				ptr = ptr3;
			}
			IL_0097:
			if (ptr == null)
			{
				bool flag = (archetypeQueries->Options & EntityQueryOptions.IgnoreComponentEnabledState) > EntityQueryOptions.Default;
				for (int j = 1; j < archetypeQueryCount; j++)
				{
					if ((archetypeQueries[j].Options & EntityQueryOptions.IgnoreComponentEnabledState) > EntityQueryOptions.Default != flag)
					{
						throw new ArgumentException("All EntityQueryOptions passed to CreateEntityQuery() must have the same value for the IgnoreComponentEnabledState flag");
					}
				}
				int num3 = 0;
				for (int k = 0; k < archetypeQueryCount; k++)
				{
					num3 += archetypeQueries[k].AllCount + archetypeQueries[k].AnyCount + archetypeQueries[k].NoneCount + archetypeQueries[k].DisabledCount + archetypeQueries[k].AbsentCount + archetypeQueries[k].PresentCount;
				}
				NativeList<TypeIndex> nativeList = new NativeList<TypeIndex>(num3, Allocator.Temp);
				NativeList<ComponentType> nativeList2 = new NativeList<ComponentType>(num3, Allocator.Temp);
				for (int l = 0; l < archetypeQueryCount; l++)
				{
					ref ArchetypeQuery ptr4 = ref archetypeQueries[l];
					for (int m = 0; m < ptr4.AllCount; m++)
					{
						if (ptr4.All[m].IsEnableable)
						{
							nativeList.Add(in ptr4.All[m]);
						}
					}
					for (int n = 0; n < ptr4.NoneCount; n++)
					{
						if (ptr4.None[n].IsEnableable)
						{
							nativeList.Add(in ptr4.None[n]);
						}
					}
					for (int num4 = 0; num4 < ptr4.AnyCount; num4++)
					{
						if (ptr4.Any[num4].IsEnableable)
						{
							nativeList.Add(in ptr4.Any[num4]);
							ComponentType componentType = ComponentType.ReadOnly(ptr4.Any[num4]);
							nativeList2.Add(in componentType);
						}
					}
					for (int num5 = 0; num5 < ptr4.DisabledCount; num5++)
					{
						nativeList.Add(in ptr4.Disabled[num5]);
					}
				}
				if (nativeList.Length > 0)
				{
					nativeList.Sort<TypeIndex>();
					int num6 = 0;
					for (int num7 = 1; num7 < nativeList.Length; num7++)
					{
						if (nativeList[num7] != nativeList[num6])
						{
							nativeList[++num6] = nativeList[num7];
						}
					}
					nativeList.Length = num6 + 1;
					if (nativeList.Length > 8)
					{
						throw new ArgumentException(string.Format("EntityQuery objects may not reference more than {0} enableable components", 8));
					}
				}
				if (nativeList2.Length > 0)
				{
					nativeList2.Sort<ComponentType>();
					int num8 = 0;
					for (int num9 = 1; num9 < nativeList2.Length; num9++)
					{
						if (nativeList2[num9] != nativeList2[num8])
						{
							nativeList2[++num8] = nativeList2[num9];
						}
					}
					nativeList2.Length = num8 + 1;
				}
				ptr = (EntityQueryData*)this.ChunkAllocate<EntityQueryData>(1, null);
				ptr->RequiredComponentsCount = requiredComponentCount;
				ptr->RequiredComponents = (ComponentType*)this.ChunkAllocate<ComponentType>(requiredComponentCount, (void*)requiredComponents);
				ptr->EnableableComponentTypeIndexCount = (flag ? 0 : nativeList.Length);
				ptr->EnableableComponentTypeIndices = (TypeIndex*)this.ChunkAllocate<TypeIndex>(nativeList.Length, (void*)nativeList.GetUnsafeReadOnlyPtr<TypeIndex>());
				ptr->HasEnableableComponents = ((nativeList.Length > 0 && !flag) ? 1 : 0);
				this.InitializeReaderWriter(ptr, requiredComponents, requiredComponentCount, nativeList2.GetUnsafeReadOnlyPtr<ComponentType>(), nativeList2.Length);
				ptr->ArchetypeQueryCount = archetypeQueryCount;
				ptr->ArchetypeQueries = (ArchetypeQuery*)this.ChunkAllocate<ArchetypeQuery>(archetypeQueryCount, (void*)archetypeQueries);
				for (int num10 = 0; num10 < archetypeQueryCount; num10++)
				{
					ptr->ArchetypeQueries[num10].All = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].AllCount, (void*)archetypeQueries[num10].All);
					ptr->ArchetypeQueries[num10].Any = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].AnyCount, (void*)archetypeQueries[num10].Any);
					ptr->ArchetypeQueries[num10].None = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].NoneCount, (void*)archetypeQueries[num10].None);
					ptr->ArchetypeQueries[num10].Disabled = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].DisabledCount, (void*)archetypeQueries[num10].Disabled);
					ptr->ArchetypeQueries[num10].Absent = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].AbsentCount, (void*)archetypeQueries[num10].Absent);
					ptr->ArchetypeQueries[num10].Present = (TypeIndex*)this.ChunkAllocate<TypeIndex>(ptr->ArchetypeQueries[num10].PresentCount, (void*)archetypeQueries[num10].Present);
					ptr->ArchetypeQueries[num10].AllAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].AllCount, (void*)archetypeQueries[num10].AllAccessMode);
					ptr->ArchetypeQueries[num10].AnyAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].AnyCount, (void*)archetypeQueries[num10].AnyAccessMode);
					ptr->ArchetypeQueries[num10].NoneAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].NoneCount, (void*)archetypeQueries[num10].NoneAccessMode);
					ptr->ArchetypeQueries[num10].DisabledAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].DisabledCount, (void*)archetypeQueries[num10].DisabledAccessMode);
					ptr->ArchetypeQueries[num10].AbsentAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].AbsentCount, (void*)archetypeQueries[num10].AbsentAccessMode);
					ptr->ArchetypeQueries[num10].PresentAccessMode = (byte*)this.ChunkAllocate<byte>(ptr->ArchetypeQueries[num10].PresentCount, (void*)archetypeQueries[num10].PresentAccessMode);
				}
				EntityComponentStore* entityComponentStore = access->EntityComponentStore;
				ptr->MatchingArchetypes = new UnsafeMatchingArchetypePtrList(access->EntityComponentStore);
				ptr->CreateMatchingChunkCache(access->EntityComponentStore);
				ptr->EntityQueryMask = default(EntityQueryMask);
				for (int num11 = 0; num11 < entityComponentStore->m_Archetypes.Length; num11++)
				{
					Archetype* ptr5 = *(IntPtr*)(entityComponentStore->m_Archetypes.Ptr + (IntPtr)num11 * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
					this.AddArchetypeIfMatching(ptr5, ptr);
				}
				ptr2.Add(num, this.m_EntityQueryDatas.Length);
				this.m_EntityQueryDatas.Add((void*)ptr);
				ptr->InvalidateChunkCache();
			}
			return EntityQuery.Construct(ptr, access);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00041014 File Offset: 0x0003F214
		private unsafe void InitializeReaderWriter(EntityQueryData* queryData, ComponentType* requiredTypes, int requiredCount, ComponentType* implicitReadTypes, int implicitReadCount)
		{
			queryData->ReaderTypesCount = 0;
			queryData->WriterTypesCount = 0;
			for (int num = 1; num != requiredCount; num++)
			{
				if (!requiredTypes[num].IsZeroSized || requiredTypes[num].IsEnableable)
				{
					ComponentType.AccessMode accessMode = requiredTypes[num].AccessModeType;
					if (accessMode != ComponentType.AccessMode.ReadWrite)
					{
						if (accessMode - ComponentType.AccessMode.ReadOnly <= 1)
						{
							queryData->ReaderTypesCount = queryData->ReaderTypesCount + 1;
						}
					}
					else
					{
						queryData->WriterTypesCount = queryData->WriterTypesCount + 1;
					}
				}
			}
			queryData->ReaderTypesCount = queryData->ReaderTypesCount + implicitReadCount;
			queryData->ReaderTypes = (TypeIndex*)this.m_EntityQueryDataChunkAllocator.Allocate(sizeof(TypeIndex) * queryData->ReaderTypesCount, 4);
			queryData->WriterTypes = (TypeIndex*)this.m_EntityQueryDataChunkAllocator.Allocate(sizeof(TypeIndex) * queryData->WriterTypesCount, 4);
			int num2 = 0;
			int num3 = 0;
			for (int num4 = 1; num4 != requiredCount; num4++)
			{
				if (!requiredTypes[num4].IsZeroSized || requiredTypes[num4].IsEnableable)
				{
					ComponentType.AccessMode accessMode = requiredTypes[num4].AccessModeType;
					if (accessMode != ComponentType.AccessMode.ReadWrite)
					{
						if (accessMode - ComponentType.AccessMode.ReadOnly <= 1)
						{
							queryData->ReaderTypes[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = requiredTypes[num4].TypeIndex;
						}
					}
					else
					{
						queryData->WriterTypes[(IntPtr)(num3++) * (IntPtr)sizeof(TypeIndex)] = requiredTypes[num4].TypeIndex;
					}
				}
			}
			for (int i = 0; i < implicitReadCount; i++)
			{
				queryData->ReaderTypes[(IntPtr)(num2++) * (IntPtr)sizeof(TypeIndex)] = implicitReadTypes[i].TypeIndex;
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000411C8 File Offset: 0x0003F3C8
		public unsafe void AddAdditionalArchetypes(UnsafePtrList<Archetype> archetypeList)
		{
			for (int i = 0; i < archetypeList.Length; i++)
			{
				for (int j = 0; j < this.m_EntityQueryDatas.Length; j++)
				{
					EntityQueryData* ptr = *(IntPtr*)(this.m_EntityQueryDatas.Ptr + (IntPtr)j * (IntPtr)sizeof(EntityQueryData*) / (IntPtr)sizeof(EntityQueryData*));
					this.AddArchetypeIfMatching(*(IntPtr*)(archetypeList.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*)), ptr);
				}
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0004122C File Offset: 0x0003F42C
		private unsafe void AddArchetypeIfMatching(Archetype* archetype, EntityQueryData* query)
		{
			int num = -1;
			for (int num2 = 0; num2 != query->ArchetypeQueryCount; num2++)
			{
				if (EntityQueryManager.IsMatchingArchetype(archetype, query->ArchetypeQueries + num2))
				{
					num = num2;
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			ref ArchetypeQuery ptr = ref query->ArchetypeQueries[num];
			MatchingArchetype* ptr2 = MatchingArchetype.Create(ref this.m_EntityQueryDataChunkAllocator, archetype, in ptr, query->RequiredComponentsCount);
			int* ptr3 = &ptr2->IndexInArchetype.FixedElementField;
			ptr2->Archetype->SetMask(query->EntityQueryMask);
			query->MatchingArchetypes.Add((void*)ptr2);
			IntPtr intPtr = (IntPtr)((void*)query);
			archetype->MatchingQueryData.Add(in intPtr);
			int num3 = 0;
			for (int i = 0; i < query->RequiredComponentsCount; i++)
			{
				if (query->RequiredComponents[i].AccessModeType != ComponentType.AccessMode.Exclude)
				{
					num3 = ChunkDataUtility.GetNextIndexInTypeArray(archetype, query->RequiredComponents[i].TypeIndex, num3);
					ptr3[i] = num3;
				}
				else
				{
					ptr3[i] = -1;
				}
			}
			int num4 = 0;
			num3 = 0;
			for (int j = 0; j < ptr.AllCount; j++)
			{
				TypeIndex typeIndex = ptr.All[j];
				if (TypeManager.IsEnableable(typeIndex))
				{
					num3 = ChunkDataUtility.GetNextIndexInTypeArray(archetype, typeIndex, num3);
					ptr2->EnableableTypeMemoryOrderInArchetype_All[(IntPtr)(num4++) * 4] = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num3];
				}
			}
			int num5 = 0;
			num3 = 0;
			for (int k = 0; k < ptr.NoneCount; k++)
			{
				TypeIndex typeIndex2 = ptr.None[k];
				if (TypeManager.IsEnableable(typeIndex2))
				{
					int nextIndexInTypeArray = ChunkDataUtility.GetNextIndexInTypeArray(archetype, typeIndex2, num3);
					if (nextIndexInTypeArray != -1)
					{
						ptr2->EnableableTypeMemoryOrderInArchetype_None[(IntPtr)(num5++) * 4] = archetype->TypeIndexInArchetypeToMemoryOrderIndex[nextIndexInTypeArray];
						num3 = nextIndexInTypeArray;
					}
				}
			}
			int num6 = 0;
			num3 = 0;
			for (int l = 0; l < ptr.AnyCount; l++)
			{
				TypeIndex typeIndex3 = ptr.Any[l];
				if (TypeManager.IsEnableable(typeIndex3))
				{
					int nextIndexInTypeArray2 = ChunkDataUtility.GetNextIndexInTypeArray(archetype, typeIndex3, num3);
					if (nextIndexInTypeArray2 != -1)
					{
						ptr2->EnableableTypeMemoryOrderInArchetype_Any[(IntPtr)(num6++) * 4] = archetype->TypeIndexInArchetypeToMemoryOrderIndex[nextIndexInTypeArray2];
						num3 = nextIndexInTypeArray2;
					}
				}
			}
			int num7 = 0;
			num3 = 0;
			for (int m = 0; m < ptr.DisabledCount; m++)
			{
				TypeIndex typeIndex4 = ptr.Disabled[m];
				if (TypeManager.IsEnableable(typeIndex4))
				{
					num3 = ChunkDataUtility.GetNextIndexInTypeArray(archetype, typeIndex4, num3);
					ptr2->EnableableTypeMemoryOrderInArchetype_Disabled[(IntPtr)(num7++) * 4] = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num3];
				}
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000414EC File Offset: 0x0003F6EC
		private unsafe static bool IsMatchingArchetype(Archetype* archetype, EntityQueryData* query)
		{
			for (int num = 0; num != query->ArchetypeQueryCount; num++)
			{
				if (EntityQueryManager.IsMatchingArchetype(archetype, query->ArchetypeQueries + num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00041528 File Offset: 0x0003F728
		private unsafe static bool IsMatchingArchetype(Archetype* archetype, ArchetypeQuery* query)
		{
			ulong bloomFilterMask = archetype->BloomFilterMask;
			if (Hint.Likely((query->AllBloomFilterMask & bloomFilterMask) != query->AllBloomFilterMask))
			{
				return false;
			}
			EntityQueryOptions options = query->Options;
			bool flag = (options & EntityQueryOptions.IncludeDisabledEntities) > EntityQueryOptions.Default;
			bool flag2 = (options & EntityQueryOptions.IncludePrefab) > EntityQueryOptions.Default;
			bool flag3 = (options & EntityQueryOptions.IncludeSystems) > EntityQueryOptions.Default;
			bool flag4 = (options & EntityQueryOptions.IncludeMetaChunks) > EntityQueryOptions.Default;
			if (archetype->Disabled && !flag)
			{
				return false;
			}
			if (archetype->Prefab && !flag2)
			{
				return false;
			}
			if (archetype->HasSystemInstanceComponents && !flag3)
			{
				return false;
			}
			if (archetype->HasChunkHeader && !flag4)
			{
				return false;
			}
			ComponentTypeInArchetype* types = archetype->Types;
			int typesCount = archetype->TypesCount;
			return typesCount >= query->AllCount && typesCount >= query->DisabledCount && typesCount >= query->PresentCount && (EntityQueryManager.TestMatchingArchetypeRequiredComponent(types, typesCount, query->All, query->AllCount) && EntityQueryManager.TestMatchingArchetypeRequiredComponent(types, typesCount, query->Disabled, query->DisabledCount) && EntityQueryManager.TestMatchingArchetypeRequiredComponent(types, typesCount, query->Present, query->PresentCount) && EntityQueryManager.TestMatchingArchetypeOptional(types, typesCount, query->Any, query->AnyCount) && EntityQueryManager.TestMatchingArchetypeExcludedComponent(types, typesCount, query->None, query->NoneCount, true)) && EntityQueryManager.TestMatchingArchetypeExcludedComponent(types, typesCount, query->Absent, query->AbsentCount, false);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00041670 File Offset: 0x0003F870
		private unsafe static bool TestMatchingArchetypeRequiredComponent(ComponentTypeInArchetype* archetypeTypes, int archetypeTypesCount, TypeIndex* queryTypes, int queryTypesCount)
		{
			if (queryTypesCount == 0)
			{
				return true;
			}
			int num = 0;
			TypeIndex typeIndex = queryTypes[num];
			for (int i = 0; i < archetypeTypesCount; i++)
			{
				TypeIndex typeIndex2 = archetypeTypes[i].TypeIndex;
				if (Hint.Unlikely(typeIndex2 == typeIndex))
				{
					if (++num == queryTypesCount)
					{
						return true;
					}
					typeIndex = queryTypes[num];
				}
				else if (typeIndex2 > typeIndex)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000416EC File Offset: 0x0003F8EC
		private unsafe static bool TestMatchingArchetypeOptional(ComponentTypeInArchetype* archetypeTypes, int archetypeTypesCount, TypeIndex* queryTypes, int queryTypesCount)
		{
			if (queryTypesCount == 0)
			{
				return true;
			}
			int num = 0;
			TypeIndex typeIndex = queryTypes[num];
			for (int i = 0; i < archetypeTypesCount; i++)
			{
				TypeIndex typeIndex2 = archetypeTypes[i].TypeIndex;
				while (typeIndex2 > typeIndex)
				{
					if (++num == queryTypesCount)
					{
						return false;
					}
					typeIndex = queryTypes[num];
				}
				if (Hint.Unlikely(typeIndex2 == typeIndex))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00041768 File Offset: 0x0003F968
		private unsafe static bool TestMatchingArchetypeExcludedComponent(ComponentTypeInArchetype* archetypeTypes, int archetypeTypesCount, TypeIndex* queryTypes, int queryTypesCount, bool ignoreEnableableTypes)
		{
			TypeIndex* ptr;
			checked
			{
				ptr = stackalloc TypeIndex[unchecked((UIntPtr)queryTypesCount) * (UIntPtr)sizeof(TypeIndex)];
			}
			if (ignoreEnableableTypes)
			{
				int num = 0;
				for (int i = 0; i < queryTypesCount; i++)
				{
					if (!TypeManager.IsEnableable(queryTypes[i]))
					{
						ptr[(IntPtr)(num++) * (IntPtr)sizeof(TypeIndex)] = queryTypes[i];
					}
				}
				queryTypes = ptr;
				queryTypesCount = num;
			}
			if (queryTypesCount == 0)
			{
				return true;
			}
			int num2 = 0;
			TypeIndex typeIndex = queryTypes[num2];
			for (int j = 0; j < archetypeTypesCount; j++)
			{
				TypeIndex typeIndex2 = archetypeTypes[j].TypeIndex;
				while (typeIndex2 > typeIndex)
				{
					if (++num2 == queryTypesCount)
					{
						return true;
					}
					typeIndex = queryTypes[num2];
				}
				if (Hint.Unlikely(typeIndex2 == typeIndex))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00041850 File Offset: 0x0003FA50
		public unsafe static int FindMatchingArchetypeIndexForArchetype(ref UnsafeMatchingArchetypePtrList matchingArchetypes, Archetype* archetype)
		{
			int length = matchingArchetypes.Length;
			MatchingArchetype** ptr = matchingArchetypes.Ptr;
			for (int i = 0; i < length; i++)
			{
				if (archetype == ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0004188E File Offset: 0x0003FA8E
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowIfEntityQueryMasksIsGreaterThanLimit(int entityQueryMasksAllocated)
		{
			if (entityQueryMasksAllocated >= 1024)
			{
				throw new Exception("You have reached the limit of 1024 unique EntityQueryMasks, and cannot generate any more.");
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000418A4 File Offset: 0x0003FAA4
		public unsafe EntityQueryMask GetEntityQueryMask(EntityQueryData* query, EntityComponentStore* ecStore)
		{
			if (query->EntityQueryMask.IsCreated())
			{
				return query->EntityQueryMask;
			}
			EntityQueryMask entityQueryMask = new EntityQueryMask((byte)(this.m_EntityQueryMasksAllocated / 8), (byte)(1 << this.m_EntityQueryMasksAllocated % 8), ecStore);
			this.m_EntityQueryMasksAllocated++;
			int length = query->MatchingArchetypes.Length;
			MatchingArchetype** ptr = query->MatchingArchetypes.Ptr;
			for (int i = 0; i < length; i++)
			{
				ref byte ptr2 = (ref ((IntPtr*)(ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*)))->Archetype->QueryMaskArray.FixedElementField) + entityQueryMask.Index;
				ptr2 |= entityQueryMask.Mask;
			}
			query->EntityQueryMask = entityQueryMask;
			return entityQueryMask;
		}

		// Token: 0x040005AA RID: 1450
		private unsafe ComponentDependencyManager* m_DependencyManager;

		// Token: 0x040005AB RID: 1451
		private BlockAllocator m_EntityQueryDataChunkAllocator;

		// Token: 0x040005AC RID: 1452
		private UnsafePtrList<EntityQueryData> m_EntityQueryDatas;

		// Token: 0x040005AD RID: 1453
		internal const int MAX_ENABLEABLE_COMPONENTS_PER_QUERY = 8;

		// Token: 0x040005AE RID: 1454
		private UntypedUnsafeParallelHashMap m_EntityQueryDataCacheUntyped;

		// Token: 0x040005AF RID: 1455
		internal int m_EntityQueryMasksAllocated;

		// Token: 0x040005B0 RID: 1456
		private TypeIndex m_disabledTypeIndex;

		// Token: 0x040005B1 RID: 1457
		private TypeIndex m_prefabTypeIndex;

		// Token: 0x040005B2 RID: 1458
		private TypeIndex m_systemInstanceTypeIndex;

		// Token: 0x040005B3 RID: 1459
		private TypeIndex m_chunkHeaderTypeIndex;
	}
}
