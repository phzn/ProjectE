using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000325 RID: 805
	public static class TypeManager
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x0005117D File Offset: 0x0004F37D
		public static IEnumerable<TypeManager.TypeInfo> AllTypes
		{
			get
			{
				return TypeManager.s_TypeInfos.GetSubArray(0, TypeManager.s_TypeCount);
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x00051194 File Offset: 0x0004F394
		internal static bool IsInitialized
		{
			get
			{
				return TypeManager.s_Initialized;
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0005119C File Offset: 0x0004F39C
		public static void RegisterUnityEngineObjectType(Type type)
		{
			if (type == null || !type.IsClass || type.IsInterface || type.FullName != "UnityEngine.Object")
			{
				throw new ArgumentException(string.Format("{0} must be typeof(UnityEngine.Object).", type));
			}
			TypeManager.UnityEngineObjectType = type;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000511EC File Offset: 0x0004F3EC
		public static TypeManager.TypeInfo[] GetAllTypes()
		{
			TypeManager.TypeInfo[] array = new TypeManager.TypeInfo[TypeManager.s_TypeCount];
			for (int i = 0; i < TypeManager.s_TypeCount; i++)
			{
				array[i] = TypeManager.s_TypeInfos[i];
			}
			return array;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00051227 File Offset: 0x0004F427
		internal unsafe static TypeManager.EntityOffsetInfo* GetEntityOffsetsPointer()
		{
			return (TypeManager.EntityOffsetInfo*)(void*)(*TypeManager.SharedEntityOffsetInfos.Ref.Data);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00051239 File Offset: 0x0004F439
		internal unsafe static TypeManager.EntityOffsetInfo* GetEntityOffsets(in TypeManager.TypeInfo typeInfo)
		{
			return TypeManager.GetEntityOffsetsPointer() + typeInfo.EntityOffsetStartIndex;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00051250 File Offset: 0x0004F450
		[GenerateTestsForBurstCompatibility]
		public unsafe static TypeManager.EntityOffsetInfo* GetEntityOffsets(TypeIndex typeIndex, out int count)
		{
			TypeManager.TypeInfo* ptr = TypeManager.GetTypeInfoPointer() + typeIndex.Index;
			count = ptr->EntityOffsetCount;
			return TypeManager.GetEntityOffsets(in *ptr);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00051281 File Offset: 0x0004F481
		internal unsafe static TypeManager.EntityOffsetInfo* GetBlobAssetRefOffsetsPointer()
		{
			return (TypeManager.EntityOffsetInfo*)(void*)(*TypeManager.SharedBlobAssetRefOffsets.Ref.Data);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00051293 File Offset: 0x0004F493
		internal unsafe static TypeManager.EntityOffsetInfo* GetBlobAssetRefOffsets(in TypeManager.TypeInfo typeInfo)
		{
			return TypeManager.GetBlobAssetRefOffsetsPointer() + typeInfo.BlobAssetRefOffsetStartIndex;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000512A9 File Offset: 0x0004F4A9
		internal unsafe static TypeManager.EntityOffsetInfo* GetWeakAssetRefOffsetsPointer()
		{
			return (TypeManager.EntityOffsetInfo*)(void*)(*TypeManager.SharedWeakAssetRefOffsets.Ref.Data);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x000512BB File Offset: 0x0004F4BB
		internal unsafe static TypeManager.EntityOffsetInfo* GetWeakAssetRefOffsets(in TypeManager.TypeInfo typeInfo)
		{
			return TypeManager.GetWeakAssetRefOffsetsPointer() + typeInfo.WeakAssetRefOffsetStartIndex;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000512D1 File Offset: 0x0004F4D1
		internal unsafe static TypeManager.EntityOffsetInfo* GetUnityObjectRefOffsetsPointer()
		{
			return (TypeManager.EntityOffsetInfo*)(void*)(*TypeManager.SharedUnityObjectRefOffsets.Ref.Data);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000512E3 File Offset: 0x0004F4E3
		internal unsafe static TypeManager.EntityOffsetInfo* GetUnityObjectRefOffsets(in TypeManager.TypeInfo typeInfo)
		{
			return TypeManager.GetUnityObjectRefOffsetsPointer() + typeInfo.UnityObjectRefOffsetStartIndex;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x000512F9 File Offset: 0x0004F4F9
		internal unsafe static UnsafeParallelHashMapData* GetStableTypeHashMapPointer()
		{
			return (UnsafeParallelHashMapData*)(void*)(*TypeManager.SharedStableTypeHashes.Ref.Data);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0005130B File Offset: 0x0004F50B
		internal unsafe static TypeIndex* GetWriteGroupsPointer()
		{
			return (TypeIndex*)(void*)(*TypeManager.SharedWriteGroups.Ref.Data);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0005131D File Offset: 0x0004F51D
		[GenerateTestsForBurstCompatibility]
		public unsafe static TypeIndex* GetWriteGroups(in TypeManager.TypeInfo typeInfo)
		{
			if (typeInfo.WriteGroupCount == 0)
			{
				return null;
			}
			return TypeManager.GetWriteGroupsPointer() + typeInfo.WriteGroupStartIndex;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0005133E File Offset: 0x0004F53E
		internal unsafe static UnsafeText* GetTypeNamesPointer()
		{
			return (UnsafeText*)(void*)(*TypeManager.SharedTypeNames.Ref.Data);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00051350 File Offset: 0x0004F550
		internal unsafe static UnsafeText* GetTypeName(TypeIndex typeIndex)
		{
			return TypeManager.GetTypeNamesPointer();
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00051357 File Offset: 0x0004F557
		internal unsafe static FixedString128Bytes GetTypeNameFixed(TypeIndex typeIndex)
		{
			return new FixedString128Bytes(in *TypeManager.GetTypeName(typeIndex));
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00051364 File Offset: 0x0004F564
		[GenerateTestsForBurstCompatibility]
		public static readonly ref TypeManager.TypeInfo GetTypeInfo(TypeIndex typeIndex)
		{
			return TypeManager.GetTypeInfoPointer() + typeIndex.Index;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0005137C File Offset: 0x0004F57C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static readonly ref TypeManager.TypeInfo GetTypeInfo<T>()
		{
			return TypeManager.GetTypeInfoPointer() + TypeManager.GetTypeIndex<T>().Index;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000513A4 File Offset: 0x0004F5A4
		internal unsafe static TypeManager.TypeInfo* GetTypeInfoPointer()
		{
			return (TypeManager.TypeInfo*)(void*)(*TypeManager.SharedTypeInfos.Ref.Data);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x000513B6 File Offset: 0x0004F5B6
		internal unsafe static FastEquality.TypeInfo* GetFastEqualityTypeInfoPointer()
		{
			return (FastEquality.TypeInfo*)(void*)(*TypeManager.SharedFastEqualityTypeInfo.Ref.Data);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000513C8 File Offset: 0x0004F5C8
		internal unsafe static int* GetDescendantCountPointer()
		{
			return (int*)(void*)(*TypeManager.SharedDescendantCounts.Ref.Data);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x000513DA File Offset: 0x0004F5DA
		internal static int GetDescendantIndex(TypeIndex typeIndex)
		{
			return typeIndex.Index;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x000513E3 File Offset: 0x0004F5E3
		internal unsafe static ulong* GetFullTypeNameHashesPointer()
		{
			return (ulong*)(void*)(*TypeManager.SharedTypeFullNameHashes.Ref.Data);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x000513F5 File Offset: 0x0004F5F5
		public static Type GetType(TypeIndex typeIndex)
		{
			return TypeManager.s_Types[typeIndex.Index];
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00051408 File Offset: 0x0004F608
		public static int GetTypeCount()
		{
			return TypeManager.s_TypeCount;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00050EE6 File Offset: 0x0004F0E6
		public static bool IsBuffer(TypeIndex typeIndex)
		{
			return (typeIndex.Value & 67108864) != 0;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0005140F File Offset: 0x0004F60F
		[Obsolete("IsSystemStateComponent() has been renamed to IsCleanupComponent(). IsSystemStateComponent() will be removed in a future package release. (UnityUpgradable) -> IsCleanupComponent(*)", false)]
		public static bool IsSystemStateComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsCleanupComponent;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0005140F File Offset: 0x0004F60F
		public static bool IsCleanupComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsCleanupComponent;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00051418 File Offset: 0x0004F618
		[Obsolete("IsSystemStateSharedComponent() has been renamed to IsCleanupSharedComponent(). IsSystemStateSharedComponent() will be removed in a future package release. (UnityUpgradable) -> IsCleanupSharedComponent(*)", false)]
		public static bool IsSystemStateSharedComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsCleanupSharedComponent;
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00051418 File Offset: 0x0004F618
		public static bool IsCleanupSharedComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsCleanupSharedComponent;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00051421 File Offset: 0x0004F621
		public static bool IsSharedComponentType(TypeIndex typeIndex)
		{
			return typeIndex.IsSharedComponentType;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0005142A File Offset: 0x0004F62A
		public static bool IsIEquatable(TypeIndex typeIndex)
		{
			return typeIndex.IsIEquatable;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00051433 File Offset: 0x0004F633
		public static bool IsManagedComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsManagedComponent;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0005143C File Offset: 0x0004F63C
		public static bool IsManagedSharedComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsManagedSharedComponent;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00051445 File Offset: 0x0004F645
		public static bool IsManagedType(TypeIndex typeIndex)
		{
			return typeIndex.IsManagedType;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0005144E File Offset: 0x0004F64E
		public static bool IsZeroSized(TypeIndex typeIndex)
		{
			return typeIndex.IsZeroSized;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00051457 File Offset: 0x0004F657
		public static bool IsChunkComponent(TypeIndex typeIndex)
		{
			return typeIndex.IsChunkComponent;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00051460 File Offset: 0x0004F660
		public static bool IsEnableable(TypeIndex typeIndex)
		{
			return typeIndex.IsEnableable;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00051469 File Offset: 0x0004F669
		public static bool HasEntityReferences(TypeIndex typeIndex)
		{
			return typeIndex.HasEntityReferences;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00051472 File Offset: 0x0004F672
		public static bool IsTemporaryBakingType(TypeIndex typeIndex)
		{
			return typeIndex.IsTemporaryBakingType;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00051460 File Offset: 0x0004F660
		public static bool IsEnableableType(TypeIndex typeIndex)
		{
			return typeIndex.IsEnableable;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0005147B File Offset: 0x0004F67B
		public static bool IsBakingOnlyType(TypeIndex typeIndex)
		{
			return typeIndex.IsBakingOnlyType;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00051484 File Offset: 0x0004F684
		public static TypeIndex MakeChunkComponentTypeIndex(TypeIndex typeIndex)
		{
			return new TypeIndex
			{
				Value = (typeIndex.Value | 536870912 | 1073741824)
			};
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000514B4 File Offset: 0x0004F6B4
		[GenerateTestsForBurstCompatibility]
		public unsafe static bool IsDescendantOf(TypeIndex typeIndex, TypeIndex baseTypeIndex)
		{
			int descendantIndex = TypeManager.GetDescendantIndex(typeIndex);
			int descendantIndex2 = TypeManager.GetDescendantIndex(baseTypeIndex);
			int num = descendantIndex2 + TypeManager.GetDescendantCountPointer()[descendantIndex2];
			return descendantIndex >= descendantIndex2 && descendantIndex <= num;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000514EC File Offset: 0x0004F6EC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public unsafe static int GetDescendantCount(TypeIndex typeIndex)
		{
			int descendantIndex = TypeManager.GetDescendantIndex(typeIndex);
			return TypeManager.GetDescendantCountPointer()[descendantIndex];
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0005150C File Offset: 0x0004F70C
		[GenerateTestsForBurstCompatibility]
		public unsafe static bool HasDescendants(TypeIndex baseTypeIndex)
		{
			int descendantIndex = TypeManager.GetDescendantIndex(baseTypeIndex);
			return TypeManager.GetDescendantCountPointer()[descendantIndex] > 0;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0005152E File Offset: 0x0004F72E
		public static ulong CalculateFullNameHash(string fullName)
		{
			return TypeHash.FNV1A64(fullName);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00051536 File Offset: 0x0004F736
		[GenerateTestsForBurstCompatibility]
		public unsafe static ulong GetFullNameHash(TypeIndex typeIndex)
		{
			return TypeManager.GetFullTypeNameHashesPointer()[typeIndex.Index];
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0005154C File Offset: 0x0004F74C
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		private unsafe static void AddTypeInfoToTables(Type type, TypeManager.TypeInfo typeInfo, string typeName, int descendantCount)
		{
			if (!TypeManager.s_StableTypeHashToTypeIndex.TryAdd(typeInfo.StableTypeHash, typeInfo.TypeIndex))
			{
				int index = TypeManager.s_StableTypeHashToTypeIndex[typeInfo.StableTypeHash].Index;
				throw new ArgumentException(string.Format("{0} and {1} have a conflict in the stable type hash. Use the [TypeVersion(...)] attribute to force a different stable type hash for one of them.", type, TypeManager.s_Types[index]));
			}
			TypeManager.s_TypeInfos[typeInfo.TypeIndex.Index] = typeInfo;
			TypeManager.s_DescendantCounts[typeInfo.TypeIndex.Index] = descendantCount;
			TypeManager.s_Types.Add(type);
			UnsafeText unsafeText = new UnsafeText(Encoding.UTF8.GetByteCount(typeName), Allocator.Persistent);
			(ref unsafeText).CopyFrom<UnsafeText>(typeName);
			TypeManager.s_TypeNames.Add(in unsafeText);
			ulong num = TypeHash.FNV1A64(typeName);
			TypeManager.s_TypeFullNameHashes.Add(in num);
			TypeManager.s_TypeCount++;
			if (type != null)
			{
				*TypeManager.SharedTypeIndex.Get(type) = typeInfo.TypeIndex;
				TypeManager.s_ManagedTypeToIndex.Add(type, typeInfo.TypeIndex);
			}
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0005165C File Offset: 0x0004F85C
		public static void Initialize()
		{
			if (TypeManager.s_Initialized)
			{
				return;
			}
			TypeManager.s_Initialized = true;
			try
			{
				if (!TypeManager.s_AppDomainUnloadRegistered)
				{
					AppDomain.CurrentDomain.DomainUnload += delegate(object _, EventArgs __)
					{
						if (TypeManager.s_Initialized)
						{
							TypeManager.Shutdown();
						}
					};
					AppDomain.CurrentDomain.ProcessExit += delegate(object _, EventArgs __)
					{
						TypeManager.Shutdown();
					};
					TypeManager.s_AppDomainUnloadRegistered = true;
				}
				TypeManager.ObjectOffset = UnsafeUtility.SizeOf<TypeManager.ObjectOffsetType>();
				TypeManager.s_ManagedTypeToIndex = new Dictionary<Type, TypeIndex>(1000);
				TypeManager.s_FailedTypeBuildException = new Dictionary<Type, Exception>();
				TypeManager.s_TypeCount = 0;
				TypeManager.s_TypeInfos = new NativeArray<TypeManager.TypeInfo>(8192, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				TypeManager.s_DescendantCounts = new NativeArray<int>(8192, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				TypeManager.s_StableTypeHashToTypeIndex = new UnsafeParallelHashMap<ulong, TypeIndex>(8192, Allocator.Persistent);
				TypeManager.s_EntityOffsetList = new NativeList<TypeManager.EntityOffsetInfo>(Allocator.Persistent);
				TypeManager.s_BlobAssetRefOffsetList = new NativeList<TypeManager.EntityOffsetInfo>(Allocator.Persistent);
				TypeManager.s_WeakAssetRefOffsetList = new NativeList<TypeManager.EntityOffsetInfo>(Allocator.Persistent);
				TypeManager.s_UnityObjectRefOffsetList = new NativeList<TypeManager.EntityOffsetInfo>(Allocator.Persistent);
				TypeManager.s_WriteGroupList = new NativeList<TypeIndex>(Allocator.Persistent);
				TypeManager.s_FastEqualityTypeInfoList = new NativeList<FastEquality.TypeInfo>(Allocator.Persistent);
				TypeManager.s_Types = new List<Type>();
				TypeManager.s_TypeNames = new UnsafeList<UnsafeText>(8192, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				TypeManager.s_TypeFullNameHashes = new UnsafeList<ulong>(8192, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				TypeManager.s_SharedComponent_FunctionPointers = new UnsafeList<TypeManager.SharedComponentFnPtrs>(8192, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				TypeManager.s_SharedComponentFns_gcDefeat = new TypeManager.ManagedSharedComponentFnPtrs[8192];
				FastEquality.Initialize();
				TypeManager.InitializeSystemsState();
				TypeManager.RegisterSpecialComponents();
				TypeManager.RegisterSpecialSystems();
				TypeManager.InitializeAllComponentTypes();
				TypeManager.InitializeAllSystemTypes();
				TypeManager.InitializeSharedStatics();
				EntityNameStorage.Initialize();
				TypeManager.InitializeAspects(true);
			}
			catch
			{
				TypeManager.Shutdown();
				throw;
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00051848 File Offset: 0x0004FA48
		private unsafe static void InitializeSharedStatics()
		{
			*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data = new IntPtr((void*)TypeManager.s_SharedComponent_FunctionPointers.Ptr);
			*TypeManager.SharedTypeInfos.Ref.Data = new IntPtr(TypeManager.s_TypeInfos.GetUnsafePtr<TypeManager.TypeInfo>());
			*TypeManager.SharedDescendantCounts.Ref.Data = new IntPtr(TypeManager.s_DescendantCounts.GetUnsafePtr<int>());
			*TypeManager.SharedEntityOffsetInfos.Ref.Data = new IntPtr((void*)TypeManager.s_EntityOffsetList.GetUnsafePtr<TypeManager.EntityOffsetInfo>());
			*TypeManager.SharedBlobAssetRefOffsets.Ref.Data = new IntPtr((void*)TypeManager.s_BlobAssetRefOffsetList.GetUnsafePtr<TypeManager.EntityOffsetInfo>());
			*TypeManager.SharedWeakAssetRefOffsets.Ref.Data = new IntPtr((void*)TypeManager.s_WeakAssetRefOffsetList.GetUnsafePtr<TypeManager.EntityOffsetInfo>());
			*TypeManager.SharedUnityObjectRefOffsets.Ref.Data = new IntPtr((void*)TypeManager.s_UnityObjectRefOffsetList.GetUnsafePtr<TypeManager.EntityOffsetInfo>());
			*TypeManager.SharedStableTypeHashes.Ref.Data = new IntPtr((void*)TypeManager.s_StableTypeHashToTypeIndex.m_Buffer);
			*TypeManager.SharedWriteGroups.Ref.Data = new IntPtr((void*)TypeManager.s_WriteGroupList.GetUnsafePtr<TypeIndex>());
			*TypeManager.SharedFastEqualityTypeInfo.Ref.Data = new IntPtr((void*)TypeManager.s_FastEqualityTypeInfoList.GetUnsafePtr<FastEquality.TypeInfo>());
			*TypeManager.SharedTypeNames.Ref.Data = new IntPtr((void*)TypeManager.s_TypeNames.Ptr);
			*TypeManager.SharedTypeFullNameHashes.Ref.Data = new IntPtr((void*)TypeManager.s_TypeFullNameHashes.Ptr);
			TypeManager.InitializeSystemSharedStatics();
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00051994 File Offset: 0x0004FB94
		private unsafe static void ShutdownSharedStatics()
		{
			*TypeManager.SharedTypeInfos.Ref.Data = 0;
			*TypeManager.SharedDescendantCounts.Ref.Data = 0;
			*TypeManager.SharedEntityOffsetInfos.Ref.Data = 0;
			*TypeManager.SharedBlobAssetRefOffsets.Ref.Data = 0;
			*TypeManager.SharedWeakAssetRefOffsets.Ref.Data = 0;
			*TypeManager.SharedStableTypeHashes.Ref.Data = 0;
			*TypeManager.SharedWriteGroups.Ref.Data = 0;
			*TypeManager.SharedFastEqualityTypeInfo.Ref.Data = 0;
			*TypeManager.SharedTypeNames.Ref.Data = 0;
			*TypeManager.SharedTypeFullNameHashes.Ref.Data = 0;
			*TypeManager.SharedSystemTypeNames.Ref.Data = 0;
			*TypeManager.SharedSystemAttributes.Ref.Data = 0;
			*TypeManager.SharedSystemCount.Ref.Data = 0;
			*TypeManager.SharedSystemTypeHashes.Ref.Data = 0;
			*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data = 0;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00051A90 File Offset: 0x0004FC90
		private unsafe static void RegisterSpecialComponents()
		{
			TypeManager.AddFastEqualityInfo(null, false, null);
			TypeManager.AddTypeInfoToTables(null, new TypeManager.TypeInfo(0, TypeManager.TypeCategory.ComponentData, 0, -1, 0UL, 0UL, -1, 0, 0, 0, 128, 0, -1, false, 0, -1, 0, -1, false, 0, -1, 0, 0UL), "null", 0);
			TypeIndex typeIndex = new TypeIndex
			{
				Value = 1
			};
			ulong num = TypeHash.CalculateStableTypeHash(typeof(Entity), null, null);
			TypeManager.AddFastEqualityInfo(typeof(Entity), false, null);
			TypeManager.EntityOffsetInfo entityOffsetInfo = default(TypeManager.EntityOffsetInfo);
			entityOffsetInfo.Offset = 0;
			TypeManager.s_EntityOffsetList.Add(in entityOffsetInfo);
			TypeManager.AddTypeInfoToTables(typeof(Entity), new TypeManager.TypeInfo(1, TypeManager.TypeCategory.EntityData, typeIndex.Value, 0, 0UL, num, -1, UnsafeUtility.SizeOf<Entity>(), UnsafeUtility.SizeOf<Entity>(), TypeManager.CalculateAlignmentInChunk(sizeof(Entity)), 128, 0, -1, false, 0, -1, 0, -1, false, 0, -1, UnsafeUtility.SizeOf<Entity>(), 0UL), "Unity.Entities.Entity", 0);
			*TypeManager.SharedTypeIndex<Entity>.Ref.Data = typeIndex;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00051B88 File Offset: 0x0004FD88
		public static void Shutdown()
		{
			if (!TypeManager.s_Initialized)
			{
				return;
			}
			TypeManager.s_Initialized = false;
			TypeManager.s_TypeCount = 0;
			TypeManager.s_Types.Clear();
			TypeManager.ShutdownSystemsState();
			TypeManager.s_FailedTypeBuildException = null;
			TypeManager.s_ManagedTypeToIndex.Clear();
			TypeManager.DisposeNative();
			TypeManager.ShutdownSharedStatics();
			EntityNameStorage.Shutdown();
			TypeManager.ShutdownAspects();
			FastEquality.Shutdown();
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00051BE4 File Offset: 0x0004FDE4
		private static void DisposeNative()
		{
			TypeManager.s_TypeInfos.Dispose();
			TypeManager.s_DescendantCounts.Dispose();
			TypeManager.s_StableTypeHashToTypeIndex.Dispose();
			TypeManager.s_EntityOffsetList.Dispose();
			TypeManager.s_BlobAssetRefOffsetList.Dispose();
			TypeManager.s_WeakAssetRefOffsetList.Dispose();
			TypeManager.s_UnityObjectRefOffsetList.Dispose();
			TypeManager.s_WriteGroupList.Dispose();
			TypeManager.s_SharedComponent_FunctionPointers.Dispose();
			foreach (FastEquality.TypeInfo typeInfo in TypeManager.s_FastEqualityTypeInfoList)
			{
				typeInfo.Dispose();
			}
			TypeManager.s_FastEqualityTypeInfoList.Dispose();
			foreach (UnsafeText unsafeText in TypeManager.s_TypeNames)
			{
				unsafeText.Dispose();
			}
			TypeManager.s_TypeNames.Dispose();
			TypeManager.s_TypeFullNameHashes.Dispose();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00051CF0 File Offset: 0x0004FEF0
		private static TypeIndex FindTypeIndex(Type type)
		{
			if (type == null)
			{
				return TypeIndex.Null;
			}
			TypeIndex @null = TypeIndex.Null;
			TypeManager.s_ManagedTypeToIndex.TryGetValue(type, out @null);
			return @null;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00051D24 File Offset: 0x0004FF24
		internal static bool TryGetTypeIndex(Type type, out TypeIndex index)
		{
			if (type == null)
			{
				index = TypeIndex.Null;
				return true;
			}
			TypeIndex typeIndex;
			if (TypeManager.s_ManagedTypeToIndex.TryGetValue(type, out typeIndex))
			{
				index = typeIndex;
				return true;
			}
			index = TypeIndex.Null;
			return false;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00051D6B File Offset: 0x0004FF6B
		[BurstDiscard]
		private static void ManagedException<T>()
		{
			TypeManager.ManagedException(typeof(T));
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00051D7C File Offset: 0x0004FF7C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void BurstException<T>()
		{
			throw new ArgumentException(string.Format("Unknown Type:`{0}` All ComponentType must be known at compile time & be successfully registered. For generic components, each concrete type must be registered with [RegisterGenericComponentType].", typeof(T)));
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00051D98 File Offset: 0x0004FF98
		private static void ManagedException(Type type)
		{
			Exception ex;
			if (type != null && TypeManager.s_FailedTypeBuildException.TryGetValue(type, out ex))
			{
				throw new ArgumentException(ex.Message);
			}
			throw new ArgumentException(string.Format("Unknown Type:`{0}` All ComponentType must be known at compile time. For generic components, each concrete type must be registered with [RegisterGenericComponentType].", (type == null) ? "null" : type));
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00051DE9 File Offset: 0x0004FFE9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public unsafe static TypeIndex GetTypeIndex<T>()
		{
			TypeIndex typeIndex = *TypeManager.SharedTypeIndex<T>.Ref.Data;
			if (typeIndex == TypeIndex.Null)
			{
				TypeManager.ManagedException<T>();
			}
			return typeIndex;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00051E0C File Offset: 0x0005000C
		public static TypeIndex GetTypeIndex(Type type)
		{
			TypeIndex typeIndex = TypeManager.FindTypeIndex(type);
			if (typeIndex == TypeIndex.Null)
			{
				TypeManager.ManagedException(type);
			}
			return typeIndex;
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00051E27 File Offset: 0x00050027
		[BurstDiscard]
		internal unsafe static void ManagedEquals<T>(ref T left, ref T right, TypeIndex typeIndex, out int result) where T : struct
		{
			result = (FastEquality.ManagedEquals(left, right, TypeManager.GetFastEqualityTypeInfoPointer()[typeIndex.Index]) ? 1 : 0);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00051E68 File Offset: 0x00050068
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static bool Equals<T>(ref T left, ref T right) where T : struct
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (typeIndex.IsSharedComponentType)
			{
				if (typeIndex.IsManagedType)
				{
					int num = -1;
					TypeManager.ManagedEquals<T>(ref left, ref right, typeIndex, out num);
					if (Hint.Unlikely(num < 0))
					{
						throw new ArgumentException(string.Format("Attempted to compare managed component type '{0}' from Burst compiled method", typeof(T)));
					}
					return num == 1;
				}
				else if (TypeManager.IsIEquatable(typeIndex))
				{
					return TypeManager.SharedComponentEquals(UnsafeUtility.AddressOf<T>(ref left), UnsafeUtility.AddressOf<T>(ref right), typeIndex);
				}
			}
			return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf<T>(ref left), UnsafeUtility.AddressOf<T>(ref right), (long)UnsafeUtility.SizeOf<T>()) == 0;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00051EF8 File Offset: 0x000500F8
		[GenerateTestsForBurstCompatibility]
		public unsafe static bool Equals(void* left, void* right, TypeIndex typeIndex)
		{
			if (typeIndex.IsSharedComponentType)
			{
				return TypeManager.SharedComponentEquals(left, right, typeIndex);
			}
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndex);
			return UnsafeUtility.MemCmp(left, right, (long)typeInfo.TypeSize) == 0;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00016E6B File Offset: 0x0001506B
		[BurstDiscard]
		private static void SetIfNotBurst(ref bool answer)
		{
			answer = true;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00051F34 File Offset: 0x00050134
		private static bool IsBursted()
		{
			bool flag = false;
			TypeManager.SetIfNotBurst(ref flag);
			return !flag;
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00051F50 File Offset: 0x00050150
		[GenerateTestsForBurstCompatibility]
		internal unsafe static bool SharedComponentEquals(void* left, void* right, TypeIndex typeIndex)
		{
			if (!typeIndex.IsIEquatable)
			{
				TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfoPointer()[typeIndex.Index];
				return UnsafeUtility.MemCmp(left, right, (long)typeInfo.TypeSize) == 0;
			}
			bool flag;
			bool flag2;
			TypeManager.CallManagedEquals(left, right, typeIndex, out flag, out flag2);
			if (flag2)
			{
				return flag;
			}
			return TypeManager.GetIEquatable_EqualsFn(typeIndex).Invoke(left, right);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00051FB9 File Offset: 0x000501B9
		[BurstDiscard]
		private unsafe static void CallManagedEquals(void* left, void* right, TypeIndex typeIndex, out bool wasequal, out bool didwrite)
		{
			didwrite = true;
			wasequal = TypeManager.s_SharedComponentFns_gcDefeat[typeIndex.Index].EqualsFn(left, right);
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00051FE0 File Offset: 0x000501E0
		[GenerateTestsForBurstCompatibility]
		internal unsafe static int SharedComponentGetHashCode(void* data, TypeIndex typeIndex)
		{
			if (typeIndex.IsIEquatable)
			{
				int num;
				bool flag;
				TypeManager.CallManagedGetHashCode(data, typeIndex, out num, out flag);
				if (flag)
				{
					return num;
				}
				FunctionPointer<FastEquality.TypeInfo.GetHashCodeDelegate> iequatable_GetHashCodeFn = TypeManager.GetIEquatable_GetHashCodeFn(typeIndex);
				if (iequatable_GetHashCodeFn.IsCreated)
				{
					return iequatable_GetHashCodeFn.Invoke(data);
				}
			}
			TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfoPointer()[typeIndex.Index];
			return (int)XXHash.Hash32((byte*)data, typeInfo.TypeSize, 0U);
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0005204C File Offset: 0x0005024C
		[BurstDiscard]
		private unsafe static void CallManagedGetHashCode(void* data, TypeIndex typeIndex, out int hashCodeWithBurst, out bool didwrite)
		{
			FastEquality.TypeInfo.GetHashCodeDelegate getHashCodeFn = TypeManager.s_SharedComponentFns_gcDefeat[typeIndex.Index].GetHashCodeFn;
			if (getHashCodeFn != null)
			{
				hashCodeWithBurst = getHashCodeFn(data);
				didwrite = true;
				return;
			}
			hashCodeWithBurst = 0;
			didwrite = false;
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00052088 File Offset: 0x00050288
		public unsafe static bool Equals(object left, object right, TypeIndex typeIndex)
		{
			if (left == null || right == null)
			{
				return left == right;
			}
			if (TypeManager.IsManagedType(typeIndex))
			{
				FastEquality.TypeInfo typeInfo = TypeManager.GetFastEqualityTypeInfoPointer()[typeIndex.Index];
				return FastEquality.ManagedEquals(left, right, typeInfo);
			}
			GCHandle gchandle = GCHandle.Alloc(left, GCHandleType.Pinned);
			GCHandle gchandle2 = GCHandle.Alloc(right, GCHandleType.Pinned);
			byte* ptr = (byte*)(void*)gchandle.AddrOfPinnedObject();
			byte* ptr2 = (byte*)(void*)gchandle2.AddrOfPinnedObject();
			bool flag = TypeManager.Equals((void*)ptr, (void*)ptr2, typeIndex);
			gchandle.Free();
			gchandle2.Free();
			return flag;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00052110 File Offset: 0x00050310
		public unsafe static bool Equals(object left, void* right, TypeIndex typeIndex)
		{
			GCHandle gchandle = GCHandle.Alloc(left, GCHandleType.Pinned);
			byte* ptr = (byte*)(void*)gchandle.AddrOfPinnedObject();
			bool flag = TypeManager.Equals((void*)ptr, right, typeIndex);
			gchandle.Free();
			return flag;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00052141 File Offset: 0x00050341
		[BurstDiscard]
		internal unsafe static void ManagedGetHashCode<T>(ref T val, TypeIndex typeIndex, out int hash, out int did_work) where T : struct
		{
			did_work = 1;
			hash = FastEquality.ManagedGetHashCode(val, TypeManager.GetFastEqualityTypeInfoPointer()[typeIndex.Index]);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00052174 File Offset: 0x00050374
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static int GetHashCode<T>(ref T val) where T : struct
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (!typeIndex.IsManagedType)
			{
				return TypeManager.GetHashCode(UnsafeUtility.AddressOf<T>(ref val), typeIndex);
			}
			int num;
			int num2;
			TypeManager.ManagedGetHashCode<T>(ref val, typeIndex, out num, out num2);
			if (Hint.Unlikely(num2 == 0))
			{
				throw new ArgumentException(string.Format("Attempted to GetHashCode using managed component type '{0}' from Burst compiled method.", typeof(T)));
			}
			return num;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x000521D0 File Offset: 0x000503D0
		[GenerateTestsForBurstCompatibility]
		public unsafe static int GetHashCode(void* val, TypeIndex typeIndex)
		{
			if (typeIndex.IsSharedComponentType)
			{
				return TypeManager.SharedComponentGetHashCode(val, typeIndex);
			}
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndex);
			return (int)XXHash.Hash32((byte*)val, typeInfo.TypeSize, 0U);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00052208 File Offset: 0x00050408
		public unsafe static int GetHashCode(object val, TypeIndex typeIndex)
		{
			if (TypeManager.IsManagedType(typeIndex))
			{
				FastEquality.TypeInfo typeInfo = TypeManager.GetFastEqualityTypeInfoPointer()[typeIndex.Index];
				return FastEquality.ManagedGetHashCode(val, typeInfo);
			}
			GCHandle gchandle = GCHandle.Alloc(val, GCHandleType.Pinned);
			byte* ptr = (byte*)(void*)gchandle.AddrOfPinnedObject();
			int hashCode = TypeManager.GetHashCode((void*)ptr, typeIndex);
			gchandle.Free();
			return hashCode;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00052264 File Offset: 0x00050464
		public unsafe static void SetIEquatable_EqualsFn(int indexInTypeArray, Delegate fn, bool burstCompile)
		{
			IntPtr intPtr;
			if (burstCompile)
			{
				intPtr = BurstCompiler.CompileFunctionPointer<Delegate>(fn).Value;
			}
			else
			{
				intPtr = Marshal.GetFunctionPointerForDelegate(fn);
			}
			TypeManager.s_SharedComponentFns_gcDefeat[indexInTypeArray & 8191].EqualsFn = (FastEquality.TypeInfo.CompareEqualDelegate)fn;
			TypeManager.s_SharedComponent_FunctionPointers.Ptr[indexInTypeArray & 8191].EqualsFn = new FunctionPointer<FastEquality.TypeInfo.CompareEqualDelegate>(intPtr);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x000522D0 File Offset: 0x000504D0
		public unsafe static void SetIEquatable_GetHashCodeFn(int indexInTypeArray, Delegate fn, bool burstCompile)
		{
			IntPtr intPtr;
			if (burstCompile)
			{
				intPtr = BurstCompiler.CompileFunctionPointer<Delegate>(fn).Value;
			}
			else
			{
				intPtr = Marshal.GetFunctionPointerForDelegate(fn);
			}
			TypeManager.s_SharedComponentFns_gcDefeat[indexInTypeArray & 8191].GetHashCodeFn = (FastEquality.TypeInfo.GetHashCodeDelegate)fn;
			TypeManager.s_SharedComponent_FunctionPointers.Ptr[indexInTypeArray & 8191].GetHashCodeFn = new FunctionPointer<FastEquality.TypeInfo.GetHashCodeDelegate>(intPtr);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0005233C File Offset: 0x0005053C
		public unsafe static void SetIRefCounted_RetainFn(int indexInTypeArray, IRefCounted.RefCountDelegate fn, bool burstCompile)
		{
			IntPtr intPtr;
			if (burstCompile)
			{
				intPtr = BurstCompiler.CompileFunctionPointer<IRefCounted.RefCountDelegate>(fn).Value;
			}
			else
			{
				intPtr = Marshal.GetFunctionPointerForDelegate<IRefCounted.RefCountDelegate>(fn);
			}
			TypeManager.s_SharedComponentFns_gcDefeat[indexInTypeArray & 8191].RetainFn = fn;
			TypeManager.s_SharedComponent_FunctionPointers.Ptr[indexInTypeArray & 8191].RetainFn = new FunctionPointer<IRefCounted.RefCountDelegate>(intPtr);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000523A0 File Offset: 0x000505A0
		public unsafe static void SetIRefCounted_ReleaseFn(int indexInTypeArray, IRefCounted.RefCountDelegate fn, bool burstCompile)
		{
			IntPtr intPtr;
			if (burstCompile)
			{
				intPtr = BurstCompiler.CompileFunctionPointer<IRefCounted.RefCountDelegate>(fn).Value;
			}
			else
			{
				intPtr = Marshal.GetFunctionPointerForDelegate<IRefCounted.RefCountDelegate>(fn);
			}
			TypeManager.s_SharedComponentFns_gcDefeat[indexInTypeArray & 8191].ReleaseFn = fn;
			TypeManager.s_SharedComponent_FunctionPointers.Ptr[indexInTypeArray & 8191].ReleaseFn = new FunctionPointer<IRefCounted.RefCountDelegate>(intPtr);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00052404 File Offset: 0x00050604
		public unsafe static FunctionPointer<FastEquality.TypeInfo.CompareEqualDelegate> GetIEquatable_EqualsFn(TypeIndex typeIndex)
		{
			return ((TypeManager.SharedComponentFnPtrs*)((byte*)(void*)(*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data) + (IntPtr)typeIndex.Index * (IntPtr)sizeof(TypeManager.SharedComponentFnPtrs)))->EqualsFn;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0005242B File Offset: 0x0005062B
		public unsafe static FunctionPointer<FastEquality.TypeInfo.GetHashCodeDelegate> GetIEquatable_GetHashCodeFn(TypeIndex typeIndex)
		{
			return ((TypeManager.SharedComponentFnPtrs*)((byte*)(void*)(*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data) + (IntPtr)typeIndex.Index * (IntPtr)sizeof(TypeManager.SharedComponentFnPtrs)))->GetHashCodeFn;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00052454 File Offset: 0x00050654
		public unsafe static void CallIRefCounted_Retain(TypeIndex typeIndex, IntPtr data)
		{
			if (TypeManager.IsBursted())
			{
				((TypeManager.SharedComponentFnPtrs*)((byte*)(void*)(*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data) + (IntPtr)typeIndex.Index * (IntPtr)sizeof(TypeManager.SharedComponentFnPtrs)))->RetainFn.Invoke(data);
				return;
			}
			TypeManager.CallManagedRetain(typeIndex, data);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x000524A0 File Offset: 0x000506A0
		[BurstDiscard]
		private static void CallManagedRetain(TypeIndex typeindex, IntPtr data)
		{
			TypeManager.s_SharedComponentFns_gcDefeat[typeindex.Index].RetainFn(data);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x000524C0 File Offset: 0x000506C0
		public unsafe static void CallIRefCounted_Release(TypeIndex typeIndex, IntPtr data)
		{
			if (TypeManager.IsBursted())
			{
				((TypeManager.SharedComponentFnPtrs*)((byte*)(void*)(*TypeManager.Shared_SharedComponentData_FnPtrs.Ref.Data) + (IntPtr)typeIndex.Index * (IntPtr)sizeof(TypeManager.SharedComponentFnPtrs)))->ReleaseFn.Invoke(data);
				return;
			}
			TypeManager.CallManagedRelease(typeIndex, data);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0005250C File Offset: 0x0005070C
		[BurstDiscard]
		private static void CallManagedRelease(TypeIndex typeindex, IntPtr data)
		{
			TypeManager.s_SharedComponentFns_gcDefeat[typeindex.Index].ReleaseFn(data);
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0005252C File Offset: 0x0005072C
		[GenerateTestsForBurstCompatibility]
		public static TypeIndex GetTypeIndexFromStableTypeHash(ulong stableTypeHash)
		{
			UnsafeParallelHashMap<ulong, TypeIndex> unsafeParallelHashMap = default(UnsafeParallelHashMap<ulong, TypeIndex>);
			unsafeParallelHashMap.m_Buffer = TypeManager.GetStableTypeHashMapPointer();
			TypeIndex typeIndex;
			if (unsafeParallelHashMap.TryGetValue(stableTypeHash, out typeIndex))
			{
				return typeIndex;
			}
			return TypeIndex.Null;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00052560 File Offset: 0x00050760
		public unsafe static object ConstructComponentFromBuffer(TypeIndex typeIndex, void* data)
		{
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndex);
			object obj = Activator.CreateInstance(TypeManager.GetType(typeIndex));
			if (data != null && typeInfo.TypeSize != 0)
			{
				GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
				byte* ptr = (byte*)(void*)gchandle.AddrOfPinnedObject();
				UnsafeUtility.MemCpy((void*)ptr, data, (long)typeInfo.TypeSize);
				gchandle.Free();
			}
			return obj;
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000525C0 File Offset: 0x000507C0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckComponentType(Type type)
		{
			int num = 0;
			Type[] array = new Type[]
			{
				typeof(IComponentData),
				typeof(IBufferElementData),
				typeof(ISharedComponentData)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsAssignableFrom(type))
				{
					num++;
				}
			}
			if (num > 1)
			{
				throw new ArgumentException(string.Format("Component {0} can only implement one of IComponentData, ISharedComponentData and IBufferElementData", type));
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00052630 File Offset: 0x00050830
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public unsafe static NativeArray<TypeIndex> GetWriteGroupTypes(TypeIndex typeIndex)
		{
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndex);
			void* writeGroups = (void*)TypeManager.GetWriteGroups(in typeInfo);
			int writeGroupCount = typeInfo.WriteGroupCount;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<TypeIndex>(writeGroups, writeGroupCount, Allocator.None);
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00052660 File Offset: 0x00050860
		internal static int CalculateAlignmentInChunk(int sizeOfTypeInBytes)
		{
			int num = 16;
			if (sizeOfTypeInBytes < num && CollectionHelper.IsPowerOfTwo(sizeOfTypeInBytes))
			{
				num = sizeOfTypeInBytes;
			}
			return num;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00052680 File Offset: 0x00050880
		private static TypeManager.TypeTreeNode[] BuildTypeTree(Type[] componentTypes, HashSet<Type> componentTypeSet, Dictionary<Type, int> descendantCountByType)
		{
			TypeManager.<>c__DisplayClass153_0 CS$<>8__locals1;
			CS$<>8__locals1.componentTypeSet = componentTypeSet;
			CS$<>8__locals1.descendantCountByType = descendantCountByType;
			CS$<>8__locals1.typeTree = new Dictionary<Type, TypeManager.TypeTreeNode>(8192);
			for (int i = 0; i < componentTypes.Length; i++)
			{
				TypeManager.<BuildTypeTree>g__LookupOrAdd|153_1(componentTypes[i], ref CS$<>8__locals1);
			}
			int num = TypeManager.s_TypeCount;
			foreach (TypeManager.TypeTreeNode typeTreeNode in CS$<>8__locals1.typeTree.Values.ToArray<TypeManager.TypeTreeNode>())
			{
				if (typeTreeNode.Parent == null)
				{
					num += TypeManager.<BuildTypeTree>g__DepthFirst|153_2(typeTreeNode, num, ref CS$<>8__locals1);
				}
			}
			TypeManager.TypeTreeNode[] array;
			return array;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0005270D File Offset: 0x0005090D
		private static bool IsSupportedComponentType(Type type)
		{
			return typeof(IComponentData).IsAssignableFrom(type) || typeof(ISharedComponentData).IsAssignableFrom(type) || typeof(IBufferElementData).IsAssignableFrom(type);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00052745 File Offset: 0x00050945
		private static void AddUnityEngineObjectTypeToListIfSupported(HashSet<Type> componentTypeSet, Type type)
		{
			if (type.ContainsGenericParameters)
			{
				return;
			}
			if (type.IsAbstract && !typeof(Component).IsAssignableFrom(type))
			{
				return;
			}
			componentTypeSet.Add(type);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00052774 File Offset: 0x00050974
		private static bool IsInstantiableComponentType(Type type)
		{
			return !type.IsAbstract && (type.IsValueType || typeof(IComponentData).IsAssignableFrom(type)) && !type.ContainsGenericParameters && !Attribute.IsDefined(type, typeof(DisableAutoTypeRegistrationAttribute));
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000527C6 File Offset: 0x000509C6
		private static void AddComponentTypeToListIfSupported(HashSet<Type> typeSet, Type type)
		{
			if (!TypeManager.IsInstantiableComponentType(type))
			{
				return;
			}
			typeSet.Add(type);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x000527DC File Offset: 0x000509DC
		private static void InitializeAllComponentTypes()
		{
			try
			{
				HashSet<Type> hashSet = new HashSet<Type>();
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				TypeManager.UnityEngineObjectType = typeof(Object);
				foreach (Assembly assembly in assemblies)
				{
					bool flag;
					bool flag2;
					TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(assembly, out flag, out flag2);
					if (flag || flag2)
					{
						Type[] types = assembly.GetTypes();
						if (flag2)
						{
							foreach (Type type in types)
							{
								if (TypeManager.UnityEngineObjectType.IsAssignableFrom(type))
								{
									TypeManager.AddUnityEngineObjectTypeToListIfSupported(hashSet, type);
								}
							}
						}
						if (flag)
						{
							foreach (Type type2 in types)
							{
								if (TypeManager.IsSupportedComponentType(type2))
								{
									TypeManager.AddComponentTypeToListIfSupported(hashSet, type2);
								}
							}
						}
					}
				}
				Assembly[] array = assemblies;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (RegisterGenericComponentTypeAttribute registerGenericComponentTypeAttribute in array[i].GetCustomAttributes<RegisterGenericComponentTypeAttribute>())
					{
						Type concreteType = registerGenericComponentTypeAttribute.ConcreteType;
						if (TypeManager.IsSupportedComponentType(concreteType))
						{
							hashSet.Add(concreteType);
						}
					}
				}
				Type[] array3 = new Type[hashSet.Count];
				hashSet.CopyTo(array3);
				Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
				Dictionary<int, HashSet<TypeIndex>> dictionary2 = new Dictionary<int, HashSet<TypeIndex>>();
				Dictionary<Type, int> dictionary3 = new Dictionary<Type, int>();
				int num = TypeManager.s_TypeCount;
				foreach (TypeManager.TypeTreeNode typeTreeNode in TypeManager.BuildTypeTree(array3, hashSet, dictionary3))
				{
					array3[typeTreeNode.IndexInTypeArray - num] = typeTreeNode.Type;
					dictionary[typeTreeNode.Type] = typeTreeNode.IndexInTypeArray;
				}
				TypeManager.GatherSharedComponentMethods(dictionary);
				TypeManager.GatherWriteGroups(array3, num, dictionary, dictionary2);
				TypeManager.AddAllComponentTypes(array3, num, dictionary2, dictionary3);
			}
			finally
			{
				if (TypeManager.s_FailedTypeBuildException.Count > 0)
				{
					throw new ArgumentException("TypeManager initialization failed. Please fix all exceptions logged above before continuing.");
				}
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x000529F8 File Offset: 0x00050BF8
		private unsafe static void GatherSharedComponentMethods(Dictionary<Type, int> indexByType)
		{
			foreach (Type type in indexByType.Keys)
			{
				if (typeof(IRefCounted).IsAssignableFrom(type))
				{
					int num = indexByType[type];
					MethodInfo methodInfo = type.GetMethod("__codegen__Retain", BindingFlags.Static | BindingFlags.Public);
					IRefCounted.RefCountDelegate refCountDelegate = (IRefCounted.RefCountDelegate)methodInfo.CreateDelegate(typeof(IRefCounted.RefCountDelegate));
					TypeManager.SetIRefCounted_RetainFn(num, refCountDelegate, Attribute.IsDefined(methodInfo, typeof(BurstCompileAttribute)));
					methodInfo = type.GetMethod("__codegen__Release", BindingFlags.Static | BindingFlags.Public);
					IRefCounted.RefCountDelegate refCountDelegate2 = (IRefCounted.RefCountDelegate)methodInfo.CreateDelegate(typeof(IRefCounted.RefCountDelegate));
					TypeManager.SetIRefCounted_ReleaseFn(num, refCountDelegate2, Attribute.IsDefined(methodInfo, typeof(BurstCompileAttribute)));
				}
			}
			foreach (Type type2 in indexByType.Keys)
			{
				int num2 = indexByType[type2];
				if (typeof(ISharedComponentData).IsAssignableFrom(type2))
				{
					if (type2.GetInterfaces().Any<Type>((Type i) => i.Name == "IEquatable`1"))
					{
						MethodInfo methodInfo2 = type2.GetMethod("__codegen__Equals", new Type[]
						{
							typeof(void*),
							typeof(void*)
						});
						Delegate @delegate = methodInfo2.CreateDelegate(typeof(FastEquality.TypeInfo.CompareEqualDelegate));
						TypeManager.SetIEquatable_EqualsFn(num2, @delegate, Attribute.IsDefined(methodInfo2, typeof(BurstCompileAttribute)));
						methodInfo2 = type2.GetMethod("__codegen__GetHashCode", BindingFlags.Static | BindingFlags.Public);
						if (methodInfo2 != null)
						{
							Delegate delegate2 = methodInfo2.CreateDelegate(typeof(FastEquality.TypeInfo.GetHashCodeDelegate));
							TypeManager.SetIEquatable_GetHashCodeFn(num2, delegate2, Attribute.IsDefined(methodInfo2, typeof(BurstCompileAttribute)));
						}
					}
				}
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00052C28 File Offset: 0x00050E28
		private unsafe static void AddAllComponentTypes(Type[] componentTypes, int startTypeIndex, Dictionary<int, HashSet<TypeIndex>> writeGroupByType, Dictionary<Type, int> descendantCountByType)
		{
			TypeManager.BuildComponentCache buildComponentCache = new TypeManager.BuildComponentCache(componentTypes.Length);
			int num = startTypeIndex;
			foreach (Type type in componentTypes)
			{
				try
				{
					if (TypeManager.FindTypeIndex(type) != TypeIndex.Null)
					{
						throw new InvalidOperationException(string.Format("ComponentType {0} cannot be initialized more than once.", type));
					}
					TypeManager.TypeInfo typeInfo;
					if (writeGroupByType.ContainsKey(num))
					{
						HashSet<TypeIndex> hashSet = writeGroupByType[num];
						TypeIndex[] array = new TypeIndex[hashSet.Count];
						hashSet.CopyTo(array);
						typeInfo = TypeManager.BuildComponentType(type, array, buildComponentCache);
					}
					else
					{
						typeInfo = TypeManager.BuildComponentType(type, buildComponentCache);
					}
					int index = typeInfo.TypeIndex.Index;
					if (num != index)
					{
						throw new InvalidOperationException(string.Format("ComponentType.TypeIndex does not match precalculated index for {0}. Expected: {1:x8} Actual: {2:x8}", type, num, index));
					}
					int num2 = descendantCountByType[type];
					TypeManager.AddTypeInfoToTables(type, typeInfo, type.FullName, num2);
					num++;
				}
				catch (Exception ex)
				{
					if (type != null)
					{
						*TypeManager.SharedTypeIndex.Get(type) = TypeIndex.Null;
						TypeManager.s_FailedTypeBuildException[type] = ex;
					}
					Debug.LogException(ex);
				}
			}
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00052D4C File Offset: 0x00050F4C
		private static void GatherWriteGroups(Type[] componentTypes, int startTypeIndex, Dictionary<Type, int> indexByType, Dictionary<int, HashSet<TypeIndex>> writeGroupByType)
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				MemberInfo memberInfo = componentTypes[i];
				TypeIndex typeIndex = new TypeIndex
				{
					Value = startTypeIndex + i
				};
				foreach (Attribute attribute in memberInfo.GetCustomAttributes(typeof(WriteGroupAttribute)))
				{
					WriteGroupAttribute writeGroupAttribute = (WriteGroupAttribute)attribute;
					if (!indexByType.ContainsKey(writeGroupAttribute.TargetType))
					{
						Debug.LogError(string.Format("GatherWriteGroups: looking for {0} but it hasn't been set up yet", writeGroupAttribute.TargetType));
					}
					int num = indexByType[writeGroupAttribute.TargetType];
					if (!writeGroupByType.ContainsKey(num))
					{
						HashSet<TypeIndex> hashSet = new HashSet<TypeIndex>();
						writeGroupByType.Add(num, hashSet);
					}
					writeGroupByType[num].Add(typeIndex);
				}
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00052E2C File Offset: 0x0005102C
		public static bool IsAssemblyReferencingEntities(Assembly assembly)
		{
			if (assembly.GetName().Name.Contains("Unity.Entities"))
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].Name.Contains("Unity.Entities"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00052E80 File Offset: 0x00051080
		internal static void IsAssemblyReferencingEntitiesOrUnityEngine(Assembly assembly, out bool referencesEntities, out bool referencesUnityEngine)
		{
			string name = assembly.GetName().Name;
			referencesEntities = false;
			referencesUnityEngine = false;
			if (name.Contains("Unity.Entities"))
			{
				referencesEntities = true;
			}
			if (name.Contains("UnityEngine"))
			{
				referencesUnityEngine = true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				string name2 = referencedAssemblies[i].Name;
				if (!referencesEntities && name2.Contains("Unity.Entities"))
				{
					referencesEntities = true;
				}
				if (!referencesUnityEngine && name2.Contains("UnityEngine"))
				{
					referencesUnityEngine = true;
				}
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00002287 File Offset: 0x00000487
		internal static void CheckIsAllowedAsComponentData(Type type, string baseTypeDesc, HashSet<Type> allowedComponentCache = null)
		{
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00052F04 File Offset: 0x00051104
		internal static void CheckTypeOverrideAndUpdateHasReferences(Type type, Dictionary<Type, EntityRemapUtility.EntityBlobRefResult> cache, ref bool hasEntityReferences, ref bool hasBlobReferences, ref bool hasUnityObjectReferences)
		{
			TypeManager.TypeOverridesAttribute customAttribute = type.GetCustomAttribute<TypeManager.TypeOverridesAttribute>(true);
			if (customAttribute != null)
			{
				EntityRemapUtility.HasRefResult hasRefResult;
				EntityRemapUtility.HasRefResult hasRefResult2;
				EntityRemapUtility.HasRefResult hasRefResult3;
				EntityRemapUtility.HasEntityReferencesManaged(type, out hasRefResult, out hasRefResult2, out hasRefResult3, cache, 128);
				if (customAttribute.HasNoEntityReferences && hasRefResult == EntityRemapUtility.HasRefResult.HasRef)
				{
					throw new ArgumentException(string.Format("Component type '{0}' has a {1} marking the component as not having ", type, "TypeOverridesAttribute") + "Entity references, however the type does in fact contain a (potentially nested) Entity reference. This is not allowed. Please refer to the documentation for TypeOverridesAttribute for how to use this attribute.");
				}
				if (customAttribute.HasNoBlobReferences && hasRefResult2 == EntityRemapUtility.HasRefResult.HasRef)
				{
					throw new ArgumentException(string.Format("Component type '{0}' has a {1} marking the component as not having ", type, "TypeOverridesAttribute") + "BlobAssetReferences, however the type does in fact contain a (potentially nested) BlobAssetReference. This is not allowed. Please refer to the documentation for TypeOverridesAttribute for how to use this attribute.");
				}
				if (customAttribute.HasNoUnityObjectReferences && !customAttribute.AllowForceNoReferences && hasRefResult3 == EntityRemapUtility.HasRefResult.HasRef)
				{
					throw new ArgumentException(string.Format("Component type '{0}' has a {1} marking the component as not having ", type, "TypeOverridesAttribute") + "UnityObjectRef<T>, however the type does in fact contain a (potentially nested) UnityObjectRef<T>. This is not allowed. Please refer to the documentation for TypeOverridesAttribute for how to use this attribute.");
				}
			}
			if (customAttribute != null && customAttribute.HasNoEntityReferences)
			{
				hasEntityReferences = false;
			}
			if (customAttribute != null && customAttribute.HasNoBlobReferences)
			{
				hasBlobReferences = false;
			}
			if (customAttribute != null && customAttribute.HasNoUnityObjectReferences)
			{
				hasUnityObjectReferences = false;
			}
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00052FE8 File Offset: 0x000511E8
		internal static void CheckIsAllowedAsManagedComponentData(Type type, string baseTypeDesc, Dictionary<Type, bool> nestedContainerCache, out bool hasNativeContainer)
		{
			if (type.IsClass && typeof(IComponentData).IsAssignableFrom(type))
			{
				hasNativeContainer = TypeManager.DoesComponentContainNativeContainer(type, type, nestedContainerCache);
				return;
			}
			throw new ArgumentException(string.Format("{0} cannot be used as managed component data for unknown reasons (BUG)", type));
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0005301F File Offset: 0x0005121F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void ThrowOnDisallowedManagedComponentData(Type type, string baseTypeDesc)
		{
			if (type.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new ArgumentException(string.Format("{0} is a class based {1}. Class based {2} must implement a default constructor.", type, baseTypeDesc, baseTypeDesc));
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00053048 File Offset: 0x00051248
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void ThrowOnDisallowedComponentData(Type type, Type baseType, string baseTypeDesc, HashSet<Type> cache)
		{
			if (type.IsPrimitive)
			{
				return;
			}
			if (type.IsPointer)
			{
				return;
			}
			if (cache.Contains(type))
			{
				return;
			}
			cache.Add(type);
			if (type.IsValueType && !type.IsByRef && !type.IsClass && !type.IsInterface && !type.IsArray)
			{
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
				}
				return;
			}
			if (type == baseType)
			{
				throw new ArgumentException(string.Format("{0} is a {1} and thus must be a struct containing only primitive or blittable members.", type, baseTypeDesc));
			}
			throw new ArgumentException(string.Format("{0} contains a field of {1}, which is neither primitive nor blittable.", baseType, type));
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000530EC File Offset: 0x000512EC
		internal static bool DoesComponentContainNativeContainer(Type type, Type baseType, Dictionary<Type, bool> nestedContainerCache)
		{
			if (type.IsPrimitive)
			{
				return false;
			}
			bool flag;
			if (nestedContainerCache.TryGetValue(type, out flag))
			{
				return flag;
			}
			if (type.IsArray)
			{
				flag = false;
				Type elementType = type.GetElementType();
				if (elementType != null)
				{
					flag = TypeManager.DoesComponentContainNativeContainer(elementType, baseType, nestedContainerCache);
				}
				nestedContainerCache.TryAdd(type, flag);
				return flag;
			}
			flag = Attribute.IsDefined(type, typeof(NativeContainerAttribute));
			nestedContainerCache.Add(type, flag);
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				flag |= TypeManager.DoesComponentContainNativeContainer(fieldInfo.FieldType, baseType, nestedContainerCache);
			}
			nestedContainerCache[type] = flag;
			return flag;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00053190 File Offset: 0x00051390
		private static bool IsComponentChunkSerializable(Type type, TypeManager.TypeCategory category, bool hasEntityReference, TypeManager.BuildComponentCache cache)
		{
			ValueTuple<bool, bool> valueTuple = TypeManager.IsTypeValidForSerialization(type, cache.ChunkSerializableCache);
			if (!valueTuple.Item2 && category == TypeManager.TypeCategory.ISharedComponentData && hasEntityReference)
			{
				valueTuple.Item1 = false;
			}
			return valueTuple.Item1;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000531CC File Offset: 0x000513CC
		[return: TupleElementNames(new string[] { "isSerializable", "hasChunkSerializableAttribute" })]
		private static ValueTuple<bool, bool> IsTypeValidForSerialization(Type type, [TupleElementNames(new string[] { "isChunkSerializable", "hasChunkSerializableAttribute" })] Dictionary<Type, ValueTuple<bool, bool>> cache)
		{
			ValueTuple<bool, bool> valueTuple = new ValueTuple<bool, bool>(false, false);
			if (cache.TryGetValue(type, out valueTuple))
			{
				return valueTuple;
			}
			if (Attribute.IsDefined(type, typeof(ChunkSerializableAttribute)))
			{
				valueTuple = new ValueTuple<bool, bool>(true, true);
				cache.Add(type, valueTuple);
				return valueTuple;
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!fieldInfo.IsStatic)
				{
					if (fieldInfo.FieldType.IsPointer || fieldInfo.FieldType == typeof(UIntPtr) || fieldInfo.FieldType == typeof(IntPtr))
					{
						return valueTuple;
					}
					if (fieldInfo.FieldType.IsValueType && !fieldInfo.FieldType.IsPrimitive && !fieldInfo.FieldType.IsEnum && !TypeManager.IsTypeValidForSerialization(fieldInfo.FieldType, cache).Item1)
					{
						cache.Add(type, valueTuple);
						return valueTuple;
					}
				}
			}
			valueTuple = new ValueTuple<bool, bool>(true, false);
			cache.Add(type, valueTuple);
			return valueTuple;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000532D3 File Offset: 0x000514D3
		private static bool IsZeroSizeStruct(Type t)
		{
			if (t.IsValueType && !t.IsPrimitive)
			{
				return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).All<FieldInfo>((FieldInfo fi) => TypeManager.IsZeroSizeStruct(fi.FieldType));
			}
			return false;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00053314 File Offset: 0x00051514
		private static ulong ComputeBloomFilterMask(ulong typeHash)
		{
			uint num = (uint)((typeHash & (ulong)(-1)) ^ (typeHash >> 32));
			Unity.Mathematics.Random random = new Unity.Mathematics.Random((num != 0U) ? num : 17U);
			ulong num2 = 0UL;
			for (int i = 0; i < 5; i++)
			{
				num2 |= 1UL << random.NextInt(64);
			}
			return num2;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0005335D File Offset: 0x0005155D
		internal static TypeManager.TypeInfo BuildComponentType(Type type, TypeManager.BuildComponentCache caches)
		{
			return TypeManager.BuildComponentType(type, null, caches);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00053368 File Offset: 0x00051568
		internal unsafe static TypeManager.TypeInfo BuildComponentType(Type type, TypeIndex[] writeGroups, TypeManager.BuildComponentCache caches)
		{
			int num = 0;
			int num2 = -1;
			bool flag;
			ulong num3 = TypeHash.CalculateMemoryOrdering(type, out flag, caches.TypeHashCache);
			ulong num4 = ((!flag) ? num3 : TypeHash.CalculateStableTypeHash(type, null, caches.TypeHashCache));
			ulong num5 = TypeManager.ComputeBloomFilterMask(num4);
			bool flag2 = type.IsClass;
			bool flag3 = typeof(IRefCounted).IsAssignableFrom(type);
			int num6 = 128;
			int num7 = 0;
			MaximumChunkCapacityAttribute customAttribute = type.GetCustomAttribute<MaximumChunkCapacityAttribute>();
			if (customAttribute != null && customAttribute.Capacity < num6)
			{
				num6 = customAttribute.Capacity;
			}
			int length = TypeManager.s_EntityOffsetList.Length;
			int length2 = TypeManager.s_BlobAssetRefOffsetList.Length;
			int length3 = TypeManager.s_WeakAssetRefOffsetList.Length;
			int length4 = TypeManager.s_UnityObjectRefOffsetList.Length;
			int num8 = 0;
			int num9 = 0;
			bool flag4 = TypeManager.DoesComponentContainNativeContainer(type, type, caches.NestedNativeContainerCache);
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			TypeManager.TypeCategory typeCategory;
			if (typeof(IComponentData).IsAssignableFrom(type) && !flag2)
			{
				TypeManager.CheckIsAllowedAsComponentData(type, "IComponentData", caches.AllowedComponentCache);
				typeCategory = TypeManager.TypeCategory.ComponentData;
				num7 = UnsafeUtility.SizeOf(type);
				num9 = TypeManager.CalculateAlignmentInChunk(num7);
				if (TypeManager.IsZeroSizeStruct(type))
				{
					num = 0;
				}
				else
				{
					num = num7;
				}
				EntityRemapUtility.CalculateFieldOffsetsUnmanaged(type, out flag5, out flag6, out flag7, out flag8, ref TypeManager.s_EntityOffsetList, ref TypeManager.s_BlobAssetRefOffsetList, ref TypeManager.s_WeakAssetRefOffsetList, ref TypeManager.s_UnityObjectRefOffsetList, caches.CalculateFieldOffsetsUnmanagedCache);
			}
			else if (typeof(IComponentData).IsAssignableFrom(type) && flag2)
			{
				TypeManager.CheckIsAllowedAsManagedComponentData(type, "IComponentData", caches.NestedNativeContainerCache, out flag4);
				typeCategory = TypeManager.TypeCategory.ComponentData;
				num = 4;
				EntityRemapUtility.HasRefResult hasRefResult;
				EntityRemapUtility.HasRefResult hasRefResult2;
				EntityRemapUtility.HasRefResult hasRefResult3;
				EntityRemapUtility.HasEntityReferencesManaged(type, out hasRefResult, out hasRefResult2, out hasRefResult3, caches.HasEntityOrBlobAssetReferenceCache, 128);
				flag5 = hasRefResult > EntityRemapUtility.HasRefResult.NoRef;
				flag6 = hasRefResult2 > EntityRemapUtility.HasRefResult.NoRef;
				flag8 = hasRefResult3 > EntityRemapUtility.HasRefResult.NoRef;
			}
			else if (typeof(IBufferElementData).IsAssignableFrom(type))
			{
				TypeManager.CheckIsAllowedAsComponentData(type, "IBufferElementData", caches.AllowedComponentCache);
				typeCategory = TypeManager.TypeCategory.BufferData;
				num7 = UnsafeUtility.SizeOf(type);
				num9 = TypeManager.CalculateAlignmentInChunk(num7);
				num8 = num7;
				InternalBufferCapacityAttribute internalBufferCapacityAttribute = (InternalBufferCapacityAttribute)type.GetCustomAttribute(typeof(InternalBufferCapacityAttribute));
				if (internalBufferCapacityAttribute != null)
				{
					num2 = internalBufferCapacityAttribute.Capacity;
				}
				else
				{
					num2 = 128 / num8;
				}
				num = sizeof(BufferHeader) + num2 * num8;
				EntityRemapUtility.CalculateFieldOffsetsUnmanaged(type, out flag5, out flag6, out flag7, out flag8, ref TypeManager.s_EntityOffsetList, ref TypeManager.s_BlobAssetRefOffsetList, ref TypeManager.s_WeakAssetRefOffsetList, ref TypeManager.s_UnityObjectRefOffsetList, caches.CalculateFieldOffsetsUnmanagedCache);
			}
			else if (typeof(ISharedComponentData).IsAssignableFrom(type))
			{
				num7 = UnsafeUtility.SizeOf(type);
				typeCategory = TypeManager.TypeCategory.ISharedComponentData;
				flag2 = !UnsafeUtility.IsUnmanaged(type);
				if (flag2)
				{
					EntityRemapUtility.HasRefResult hasRefResult4;
					EntityRemapUtility.HasRefResult hasRefResult5;
					EntityRemapUtility.HasRefResult hasRefResult6;
					EntityRemapUtility.HasEntityReferencesManaged(type, out hasRefResult4, out hasRefResult5, out hasRefResult6, caches.HasEntityOrBlobAssetReferenceCache, 128);
					flag5 = false;
					flag6 = hasRefResult5 > EntityRemapUtility.HasRefResult.NoRef;
					flag8 = hasRefResult6 > EntityRemapUtility.HasRefResult.NoRef;
				}
				else
				{
					EntityRemapUtility.CalculateFieldOffsetsUnmanaged(type, out flag5, out flag6, out flag7, out flag8, ref TypeManager.s_EntityOffsetList, ref TypeManager.s_BlobAssetRefOffsetList, ref TypeManager.s_WeakAssetRefOffsetList, ref TypeManager.s_UnityObjectRefOffsetList, caches.CalculateFieldOffsetsUnmanagedCache);
				}
			}
			else
			{
				if (!type.IsClass)
				{
					throw new ArgumentException(string.Format("{0} is not a valid component.", type));
				}
				typeCategory = TypeManager.TypeCategory.UnityEngineObject;
				num = 4;
				num9 = 4;
				flag5 = false;
				flag6 = false;
				flag8 = false;
			}
			TypeManager.CheckTypeOverrideAndUpdateHasReferences(type, caches.HasEntityOrBlobAssetReferenceCache, ref flag5, ref flag6, ref flag8);
			TypeManager.AddFastEqualityInfo(type, typeCategory == TypeManager.TypeCategory.UnityEngineObject, caches.FastEqualityLayoutInfoCache);
			int num10 = TypeManager.s_EntityOffsetList.Length - length;
			int num11 = TypeManager.s_BlobAssetRefOffsetList.Length - length2;
			int num12 = TypeManager.s_WeakAssetRefOffsetList.Length - length3;
			int num13 = TypeManager.s_UnityObjectRefOffsetList.Length - length4;
			int length5 = TypeManager.s_WriteGroupList.Length;
			int num14 = ((writeGroups == null) ? 0 : writeGroups.Length);
			if (writeGroups != null)
			{
				foreach (TypeIndex typeIndex in writeGroups)
				{
					TypeManager.s_WriteGroupList.Add(in typeIndex);
				}
			}
			int num15 = TypeManager.s_TypeCount;
			bool flag9 = typeof(ICleanupSharedComponentData).IsAssignableFrom(type);
			bool flag10 = typeof(ICleanupBufferElementData).IsAssignableFrom(type);
			bool flag11 = flag9 || flag10 || typeof(ICleanupComponentData).IsAssignableFrom(type);
			bool flag12 = typeof(IEnableableComponent).IsAssignableFrom(type);
			if (flag12 && ((typeCategory != TypeManager.TypeCategory.ComponentData && typeCategory != TypeManager.TypeCategory.BufferData) || flag11))
			{
				throw new ArgumentException(string.Format("IEnableableComponent is not supported for type {0}. Only IComponentData and IBufferElementData can be disabled. Cleanup components are not supported.", type));
			}
			bool flag13 = Attribute.IsDefined(type, typeof(TemporaryBakingTypeAttribute));
			bool flag14 = Attribute.IsDefined(type, typeof(BakingTypeAttribute));
			bool flag15 = type.GetInterfaces().Any<Type>((Type i) => i.Name.Contains("IEquatable"));
			bool flag16 = TypeManager.IsComponentChunkSerializable(type, typeCategory, flag5, caches);
			if (num15 != 0)
			{
				if (num == 0)
				{
					num15 |= 1073741824;
				}
				if (typeCategory == TypeManager.TypeCategory.ISharedComponentData)
				{
					num15 |= 134217728;
				}
				if (flag11)
				{
					num15 |= 33554432;
				}
				if (flag9)
				{
					num15 |= 167772160;
				}
				if (num2 >= 0)
				{
					num15 |= 67108864;
				}
				if (!flag5)
				{
					num15 |= 131072;
				}
				if (flag4)
				{
					num15 |= 524288;
				}
				if (flag2)
				{
					num15 |= 268435456;
				}
				if (flag12)
				{
					num15 |= 16777216;
				}
				if (flag3)
				{
					num15 |= 4194304;
				}
				if (flag13)
				{
					num15 |= 2097152;
				}
				if (flag14)
				{
					num15 |= 1048576;
				}
				if (flag15)
				{
					num15 |= 8388608;
				}
				if (!flag16)
				{
					num15 |= 262144;
				}
			}
			return new TypeManager.TypeInfo(num15, typeCategory, num10, length, num3, num4, num2, num, (num8 > 0) ? num8 : num, num9, num6, num14, length5, flag6, num11, length2, num12, length3, flag8, num13, length4, num7, num5);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x000538E0 File Offset: 0x00051AE0
		private static void AddFastEqualityInfo(Type type, bool isUnityEngineObject = false, Dictionary<Type, List<FastEquality.LayoutInfo>> cache = null)
		{
			FastEquality.TypeInfo typeInfo;
			if (type == null || isUnityEngineObject)
			{
				typeInfo = FastEquality.TypeInfo.Null;
				TypeManager.s_FastEqualityTypeInfoList.Add(in typeInfo);
				return;
			}
			typeInfo = FastEquality.CreateTypeInfo(type, null);
			TypeManager.s_FastEqualityTypeInfoList.Add(in typeInfo);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00053920 File Offset: 0x00051B20
		internal static void InitializeAspects(bool ignoreTestAspects = true)
		{
			Type[] array;
			if (!ignoreTestAspects)
			{
				array = (from t in TypeManager.GetTypesDerivedFrom(typeof(IAspect))
					where t.Namespace != null && t.GetCustomAttribute<DisableGenerationAttribute>() == null
					select t).ToArray<Type>();
			}
			else
			{
				array = (from t in TypeManager.GetTypesDerivedFrom(typeof(IAspect))
					where t.Namespace != null && !t.Namespace.Contains("test", StringComparison.InvariantCultureIgnoreCase) && t.GetCustomAttribute<DisableGenerationAttribute>() == null
					select t).ToArray<Type>();
			}
			TypeManager.s_AspectTypes = array;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x000539A7 File Offset: 0x00051BA7
		private static void ShutdownAspects()
		{
			TypeManager.s_AspectTypes = null;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x000539AF File Offset: 0x00051BAF
		[ExcludeFromBurstCompatTesting("Returns managed types")]
		internal static Type[] GetAllAspectTypes()
		{
			return TypeManager.s_AspectTypes;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x000539B6 File Offset: 0x00051BB6
		[ExcludeFromBurstCompatTesting("Returns managed type")]
		internal static Type GetAspectType(int typeIndex)
		{
			if (typeIndex < 0 || typeIndex >= TypeManager.s_AspectTypes.Length)
			{
				return null;
			}
			return TypeManager.s_AspectTypes[typeIndex];
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x000539CF File Offset: 0x00051BCF
		[ExcludeFromBurstCompatTesting("Takes managed type")]
		internal static int GetAspectTypeIndex(Type type)
		{
			if (!(type != null))
			{
				return 0;
			}
			return Array.IndexOf<Type>(TypeManager.s_AspectTypes, type);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x000539E7 File Offset: 0x00051BE7
		[ExcludeFromBurstCompatTesting("Takes managed type")]
		internal static int GetAspectTypeIndex<T>() where T : struct, IAspect
		{
			return TypeManager.GetAspectTypeIndex(typeof(T));
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x000539F8 File Offset: 0x00051BF8
		internal static TypeManager.SystemAttributeKind AttributeTypeToKind(Type attributeType)
		{
			if (attributeType == typeof(UpdateBeforeAttribute))
			{
				return TypeManager.SystemAttributeKind.UpdateBefore;
			}
			if (attributeType == typeof(UpdateAfterAttribute))
			{
				return TypeManager.SystemAttributeKind.UpdateAfter;
			}
			if (attributeType == typeof(CreateBeforeAttribute))
			{
				return TypeManager.SystemAttributeKind.CreateBefore;
			}
			if (attributeType == typeof(CreateAfterAttribute))
			{
				return TypeManager.SystemAttributeKind.CreateAfter;
			}
			if (attributeType == typeof(DisableAutoCreationAttribute))
			{
				return TypeManager.SystemAttributeKind.DisableAutoCreation;
			}
			if (attributeType == typeof(UpdateInGroupAttribute))
			{
				return TypeManager.SystemAttributeKind.UpdateInGroup;
			}
			if (attributeType == typeof(RequireMatchingQueriesForUpdateAttribute))
			{
				return TypeManager.SystemAttributeKind.RequireMatchingQueriesForUpdate;
			}
			throw new ArgumentException(string.Format("Unknown attribute type {0}", attributeType));
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00053AA4 File Offset: 0x00051CA4
		internal static Type SystemAttributeKindToType(TypeManager.SystemAttributeKind kind)
		{
			switch (kind)
			{
			case TypeManager.SystemAttributeKind.UpdateBefore:
				return typeof(UpdateBeforeAttribute);
			case TypeManager.SystemAttributeKind.UpdateAfter:
				return typeof(UpdateAfterAttribute);
			case TypeManager.SystemAttributeKind.CreateBefore:
				return typeof(CreateBeforeAttribute);
			case TypeManager.SystemAttributeKind.CreateAfter:
				return typeof(CreateAfterAttribute);
			case TypeManager.SystemAttributeKind.DisableAutoCreation:
				return typeof(DisableAutoCreationAttribute);
			case TypeManager.SystemAttributeKind.UpdateInGroup:
				return typeof(UpdateInGroupAttribute);
			case TypeManager.SystemAttributeKind.RequireMatchingQueriesForUpdate:
				return typeof(RequireMatchingQueriesForUpdateAttribute);
			default:
				throw new ArgumentOutOfRangeException("kind", kind, null);
			}
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00053B34 File Offset: 0x00051D34
		private static void InitializeSystemsState()
		{
			TypeManager.s_SystemCount = 0;
			TypeManager.s_SystemTypes = new List<Type>(1024);
			TypeManager.s_ManagedSystemTypeToIndex = new Dictionary<Type, SystemTypeIndex>(1024);
			TypeManager.s_SystemTypeSizes = new List<int>(1024);
			TypeManager.s_SystemTypeHashes = new UnsafeList<long>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			TypeManager.s_SystemFilterTypeMap = new Dictionary<TypeManager.LookupFlags, NativeList<SystemTypeIndex>>(1024);
			TypeManager.s_SystemTypeFlagsList = new UnsafeList<int>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			TypeManager.s_SystemFilterFlagsList = new UnsafeList<WorldSystemFilterFlags>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			TypeManager.s_SystemTypeNames = new UnsafeList<UnsafeText>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			TypeManager.s_SystemAttributes = new UnsafeList<UnsafeList<TypeManager.SystemAttribute>>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00053BF4 File Offset: 0x00051DF4
		private static void ShutdownSystemsState()
		{
			TypeManager.s_SystemTypes.Clear();
			TypeManager.s_ManagedSystemTypeToIndex.Clear();
			TypeManager.s_SystemTypeSizes.Clear();
			foreach (NativeList<SystemTypeIndex> nativeList in TypeManager.s_SystemFilterTypeMap.Values)
			{
				nativeList.Dispose();
			}
			TypeManager.s_SystemFilterTypeMap.Clear();
			TypeManager.s_SystemTypeFlagsList.Dispose();
			TypeManager.s_SystemFilterFlagsList.Dispose();
			foreach (UnsafeText unsafeText in TypeManager.s_SystemTypeNames)
			{
				unsafeText.Dispose();
			}
			TypeManager.s_SystemTypeNames.Dispose();
			for (int i = 0; i < TypeManager.s_SystemAttributes.Length; i++)
			{
				UnsafeList<TypeManager.SystemAttribute> unsafeList = TypeManager.s_SystemAttributes[i];
				if (unsafeList.IsCreated)
				{
					unsafeList.Dispose();
				}
			}
			TypeManager.s_SystemAttributes.Dispose();
			TypeManager.s_SystemTypeHashes.Dispose();
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00053D20 File Offset: 0x00051F20
		private static void RegisterSpecialSystems()
		{
			TypeManager.AddSystemTypeToTables(null, "null", 0, 0L, 0, (WorldSystemFilterFlags)0U);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00053D34 File Offset: 0x00051F34
		internal static void InitializeAllSystemTypes()
		{
			try
			{
				List<Type> list = TypeManager.GetTypesDerivedFrom(typeof(ISystem)).ToList<Type>();
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					foreach (RegisterGenericSystemTypeAttribute registerGenericSystemTypeAttribute in assemblies[i].GetCustomAttributes<RegisterGenericSystemTypeAttribute>())
					{
						list.Add(registerGenericSystemTypeAttribute.ConcreteType);
					}
				}
				HashSet<Type> hashSet = new HashSet<Type>(32);
				foreach (Type type in list)
				{
					if (type.IsValueType && !type.ContainsGenericParameters)
					{
						string fullName = type.FullName;
						int num = UnsafeUtility.SizeOf(type);
						long hashCode = Unity.Burst.BurstRuntime.GetHashCode64(type);
						int systemTypeFlags = TypeManager.GetSystemTypeFlags(type);
						if (typeof(ISystem).IsAssignableFrom(type) && (systemTypeFlags & 536870912) != 0)
						{
							Debug.LogError(string.Format("System {0} has managed fields, but implements ISystem, which is not allowed. If you need to use managed fields, please inherit from SystemBase.", type));
						}
						WorldSystemFilterFlags worldSystemFilterFlags = TypeManager.MakeWorldFilterFlags(type, ref hashSet);
						TypeManager.AddSystemTypeToTables(type, fullName, num, hashCode, systemTypeFlags, worldSystemFilterFlags);
					}
				}
				foreach (Type type2 in TypeManager.GetTypesDerivedFrom(typeof(ComponentSystemBase)))
				{
					if (!type2.IsAbstract && !type2.ContainsGenericParameters)
					{
						string fullName2 = type2.FullName;
						int num2 = -1;
						long hashCode2 = Unity.Burst.BurstRuntime.GetHashCode64(type2);
						int systemTypeFlags2 = TypeManager.GetSystemTypeFlags(type2);
						WorldSystemFilterFlags worldSystemFilterFlags2 = TypeManager.MakeWorldFilterFlags(type2, ref hashSet);
						TypeManager.AddSystemTypeToTables(type2, fullName2, num2, hashCode2, systemTypeFlags2, worldSystemFilterFlags2);
					}
				}
				UnsafeList<TypeManager.SystemAttribute> unsafeList = default(UnsafeList<TypeManager.SystemAttribute>);
				TypeManager.s_SystemAttributes.Add(in unsafeList);
				for (int j = 1; j < TypeManager.s_SystemCount; j++)
				{
					TypeManager.AddSystemAttributesToTable(TypeManager.GetSystemType(j));
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00053F8C File Offset: 0x0005218C
		private static void AddSystemAttributesToTable(Type systemType)
		{
			Type typeFromHandle = typeof(DisableAutoCreationAttribute);
			int num = 0;
			UnsafeList<TypeManager.SystemAttribute> unsafeList = new UnsafeList<TypeManager.SystemAttribute>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			foreach (Type type in new Type[]
			{
				typeof(UpdateBeforeAttribute),
				typeof(UpdateAfterAttribute),
				typeof(CreateBeforeAttribute),
				typeof(CreateAfterAttribute),
				typeof(DisableAutoCreationAttribute),
				typeof(UpdateInGroupAttribute),
				typeof(RequireMatchingQueriesForUpdateAttribute)
			})
			{
				TypeManager.SystemAttributeKind systemAttributeKind = (TypeManager.SystemAttributeKind)num;
				num++;
				if (type == typeFromHandle)
				{
					object[] customAttributes = systemType.GetCustomAttributes(type, false);
					bool flag = false;
					for (int j = 0; j < customAttributes.Length; j++)
					{
						if ((customAttributes[j] as Attribute).GetType() == typeFromHandle)
						{
							flag = true;
							TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
							systemAttribute.Kind = systemAttributeKind;
							systemAttribute.TargetSystemTypeIndex = -1;
							unsafeList.Add(in systemAttribute);
							break;
						}
					}
					if (!flag && systemType.Assembly.GetCustomAttribute(type) != null)
					{
						TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
						systemAttribute.Kind = systemAttributeKind;
						systemAttribute.TargetSystemTypeIndex = -1;
						unsafeList.Add(in systemAttribute);
					}
				}
				else
				{
					object[] customAttributes2 = systemType.GetCustomAttributes(type, true);
					if (customAttributes2.Length != 0)
					{
						if (systemAttributeKind == TypeManager.SystemAttributeKind.CreateAfter)
						{
							for (int k = 0; k < customAttributes2.Length; k++)
							{
								CreateAfterAttribute createAfterAttribute = customAttributes2[k] as CreateAfterAttribute;
								TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
								systemAttribute.Kind = systemAttributeKind;
								systemAttribute.TargetSystemTypeIndex = (TypeManager.IsSystemType(createAfterAttribute.SystemType) ? TypeManager.GetSystemTypeIndex(createAfterAttribute.SystemType) : (-1));
								unsafeList.Add(in systemAttribute);
							}
						}
						if (systemAttributeKind == TypeManager.SystemAttributeKind.CreateBefore)
						{
							for (int l = 0; l < customAttributes2.Length; l++)
							{
								CreateBeforeAttribute createBeforeAttribute = customAttributes2[l] as CreateBeforeAttribute;
								TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
								systemAttribute.Kind = systemAttributeKind;
								systemAttribute.TargetSystemTypeIndex = (TypeManager.IsSystemType(createBeforeAttribute.SystemType) ? TypeManager.GetSystemTypeIndex(createBeforeAttribute.SystemType) : (-1));
								unsafeList.Add(in systemAttribute);
							}
						}
						if (systemAttributeKind == TypeManager.SystemAttributeKind.UpdateAfter)
						{
							for (int m = 0; m < customAttributes2.Length; m++)
							{
								UpdateAfterAttribute updateAfterAttribute = customAttributes2[m] as UpdateAfterAttribute;
								TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
								systemAttribute.Kind = systemAttributeKind;
								systemAttribute.TargetSystemTypeIndex = (TypeManager.IsSystemType(updateAfterAttribute.SystemType) ? TypeManager.GetSystemTypeIndex(updateAfterAttribute.SystemType) : (-1));
								unsafeList.Add(in systemAttribute);
							}
						}
						if (systemAttributeKind == TypeManager.SystemAttributeKind.UpdateBefore)
						{
							for (int n = 0; n < customAttributes2.Length; n++)
							{
								UpdateBeforeAttribute updateBeforeAttribute = customAttributes2[n] as UpdateBeforeAttribute;
								TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
								systemAttribute.Kind = systemAttributeKind;
								systemAttribute.TargetSystemTypeIndex = (TypeManager.IsSystemType(updateBeforeAttribute.SystemType) ? TypeManager.GetSystemTypeIndex(updateBeforeAttribute.SystemType) : (-1));
								unsafeList.Add(in systemAttribute);
							}
						}
						if (systemAttributeKind == TypeManager.SystemAttributeKind.UpdateInGroup)
						{
							for (int num2 = 0; num2 < customAttributes2.Length; num2++)
							{
								UpdateInGroupAttribute updateInGroupAttribute = customAttributes2[num2] as UpdateInGroupAttribute;
								int num3 = 0;
								if (updateInGroupAttribute.OrderFirst)
								{
									num3 |= 1;
								}
								if (updateInGroupAttribute.OrderLast)
								{
									num3 |= 2;
								}
								TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
								systemAttribute.Kind = systemAttributeKind;
								systemAttribute.TargetSystemTypeIndex = ((TypeManager.IsSystemType(updateInGroupAttribute.GroupType) && TypeManager.IsSystemAGroup(updateInGroupAttribute.GroupType)) ? TypeManager.GetSystemTypeIndex(updateInGroupAttribute.GroupType) : (-1));
								systemAttribute.Flags = num3;
								unsafeList.Add(in systemAttribute);
							}
						}
						if (systemAttributeKind == TypeManager.SystemAttributeKind.RequireMatchingQueriesForUpdate)
						{
							TypeManager.SystemAttribute systemAttribute = default(TypeManager.SystemAttribute);
							systemAttribute.Kind = systemAttributeKind;
							systemAttribute.TargetSystemTypeIndex = -1;
							unsafeList.Add(in systemAttribute);
						}
					}
				}
			}
			TypeManager.GetSystemTypeIndex(systemType);
			TypeManager.s_SystemAttributes.Add(in unsafeList);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00054388 File Offset: 0x00052588
		private static int GetSystemTypeFlags(Type systemType)
		{
			int num = 0;
			if (systemType.GetConstructors().Any<ConstructorInfo>((ConstructorInfo c) => c.GetParameters().Length == 0))
			{
				num |= 134217728;
			}
			if (systemType.IsSubclassOf(typeof(ComponentSystemGroup)))
			{
				num |= 1073741824;
			}
			if (!UnsafeUtility.IsUnmanaged(systemType))
			{
				num |= 536870912;
			}
			if (typeof(ISystemStartStop).IsAssignableFrom(systemType))
			{
				num |= 268435456;
			}
			return num;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00054410 File Offset: 0x00052610
		public static ComponentSystemBase ConstructSystem(Type systemType)
		{
			if (!typeof(ComponentSystemBase).IsAssignableFrom(systemType))
			{
				throw new ArgumentException("'" + systemType.FullName + "' cannot be constructed as it does not inherit from ComponentSystemBase");
			}
			TypeManager.AddSystemTypeToTablesAfterInit(systemType);
			return (ComponentSystemBase)Activator.CreateInstance(systemType);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00054450 File Offset: 0x00052650
		public static T ConstructSystem<T>() where T : ComponentSystemBase
		{
			return (T)((object)TypeManager.ConstructSystem(typeof(T)));
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00054466 File Offset: 0x00052666
		public static T ConstructSystem<T>(Type systemType) where T : ComponentSystemBase
		{
			return (T)((object)TypeManager.ConstructSystem(systemType));
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00054474 File Offset: 0x00052674
		public static IReadOnlyList<Type> GetSystems(WorldSystemFilterFlags filterFlags = WorldSystemFilterFlags.All, WorldSystemFilterFlags requiredFlags = (WorldSystemFilterFlags)0U)
		{
			List<Type> list = new List<Type>();
			NativeList<SystemTypeIndex> systemTypeIndices = TypeManager.GetSystemTypeIndices(filterFlags, requiredFlags);
			for (int i = 0; i < systemTypeIndices.Length; i++)
			{
				list.Add(TypeManager.GetSystemType(systemTypeIndices[i]));
			}
			return list;
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x000544B8 File Offset: 0x000526B8
		public static NativeList<SystemTypeIndex> GetSystemTypeIndices(WorldSystemFilterFlags filterFlags = WorldSystemFilterFlags.All, WorldSystemFilterFlags requiredFlags = (WorldSystemFilterFlags)0U)
		{
			if ((filterFlags & WorldSystemFilterFlags.Default) != (WorldSystemFilterFlags)0U)
			{
				filterFlags &= ~WorldSystemFilterFlags.Default;
				filterFlags |= WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.Presentation;
			}
			requiredFlags &= ~WorldSystemFilterFlags.Default;
			TypeManager.LookupFlags lookupFlags = new TypeManager.LookupFlags
			{
				OptionalFlags = filterFlags,
				RequiredFlags = requiredFlags
			};
			NativeList<SystemTypeIndex> nativeList;
			if (TypeManager.s_SystemFilterTypeMap.TryGetValue(lookupFlags, out nativeList))
			{
				return nativeList;
			}
			NativeList<SystemTypeIndex> nativeList2 = new NativeList<SystemTypeIndex>(TypeManager.s_SystemTypes.Count - 1, Allocator.Temp);
			for (int i = 1; i < TypeManager.s_SystemTypes.Count; i++)
			{
				Type type = TypeManager.s_SystemTypes[i];
				if (TypeManager.FilterSystemType(i, lookupFlags))
				{
					SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(type);
					nativeList2.Add(in systemTypeIndex);
				}
			}
			TypeManager.SortSystemTypesInCreationOrder(nativeList2);
			NativeList<SystemTypeIndex> nativeList3 = new NativeList<SystemTypeIndex>(nativeList2.Length, Allocator.Persistent);
			nativeList3.CopyFrom(in nativeList2);
			TypeManager.s_SystemFilterTypeMap[lookupFlags] = nativeList3;
			return nativeList3;
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0005459C File Offset: 0x0005279C
		internal static void AddSystemTypeToTablesAfterInit(Type systemType)
		{
			if (systemType == null || TypeManager.s_ManagedSystemTypeToIndex.ContainsKey(systemType))
			{
				return;
			}
			int num = -1;
			if (systemType.IsValueType)
			{
				num = UnsafeUtility.SizeOf(systemType);
			}
			long hashCode = Unity.Burst.BurstRuntime.GetHashCode64(systemType);
			string fullName = systemType.FullName;
			int systemTypeFlags = TypeManager.GetSystemTypeFlags(systemType);
			WorldSystemFilterFlags worldSystemFilterFlags = (WorldSystemFilterFlags)0U;
			TypeManager.AddSystemTypeToTables(systemType, fullName, num, hashCode, systemTypeFlags, worldSystemFilterFlags);
			TypeManager.AddSystemAttributesToTable(systemType);
			TypeManager.InitializeSystemSharedStatics();
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00054600 File Offset: 0x00052800
		public static void SortSystemTypesInCreationOrder(List<Type> systemTypes)
		{
			NativeList<SystemTypeIndex> nativeList = new NativeList<SystemTypeIndex>(systemTypes.Count, Allocator.Temp);
			for (int i = 0; i < systemTypes.Count; i++)
			{
				SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(systemTypes[i]);
				nativeList.Add(in systemTypeIndex);
			}
			TypeManager.SortSystemTypesInCreationOrder(nativeList);
			for (int j = 0; j < nativeList.Length; j++)
			{
				systemTypes[j] = TypeManager.GetSystemType(nativeList[j]);
			}
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00054674 File Offset: 0x00052874
		public unsafe static void SortSystemTypesInCreationOrder(NativeList<SystemTypeIndex> indices)
		{
			UnsafeList<ComponentSystemSorter.SystemElement> unsafeList = new UnsafeList<ComponentSystemSorter.SystemElement>(indices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			unsafeList.Length = indices.Length;
			for (int i = 0; i < indices.Length; i++)
			{
				unsafeList[i] = new ComponentSystemSorter.SystemElement
				{
					SystemTypeIndex = indices[i],
					Index = new UpdateIndex(i, false),
					OrderingBucket = 0,
					updateBefore = new NativeList<int>(16, Allocator.Temp),
					nAfter = 0
				};
			}
			NativeHashMap<SystemTypeIndex, int> nativeHashMap = new NativeHashMap<SystemTypeIndex, int>(unsafeList.Length, Allocator.Temp);
			NativeHashMap<SystemTypeIndex, int>* ptr = (NativeHashMap<SystemTypeIndex, int>*)UnsafeUtility.AddressOf<NativeHashMap<SystemTypeIndex, int>>(ref nativeHashMap);
			UnsafeList<ComponentSystemSorter.SystemElement>* ptr2 = (UnsafeList<ComponentSystemSorter.SystemElement>*)UnsafeUtility.AddressOf<UnsafeList<ComponentSystemSorter.SystemElement>>(ref unsafeList);
			NativeHashSet<SystemTypeIndex> nativeHashSet = new NativeHashSet<SystemTypeIndex>(16, Allocator.Temp);
			NativeHashSet<SystemTypeIndex>* ptr3 = (NativeHashSet<SystemTypeIndex>*)UnsafeUtility.AddressOf<NativeHashSet<SystemTypeIndex>>(ref nativeHashSet);
			ComponentSystemSorter.FindConstraints(-1, ptr2, ptr, TypeManager.SystemAttributeKind.CreateAfter, TypeManager.SystemAttributeKind.CreateBefore, ptr3);
			foreach (SystemTypeIndex systemTypeIndex in nativeHashSet)
			{
				ComponentSystemSorter.WarnAboutAnySystemAttributeBadness(systemTypeIndex, null);
			}
			nativeHashSet.Clear();
			ComponentSystemSorter.Sort(ptr2, ptr);
			for (int j = 0; j < unsafeList.Length; j++)
			{
				indices[j] = unsafeList[j].SystemTypeIndex;
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x000547E4 File Offset: 0x000529E4
		internal unsafe static void AddSystemTypeToTables(Type type, string typeName, int typeSize, long typeHash, int systemTypeFlags, WorldSystemFilterFlags filterFlags)
		{
			if (type != null && TypeManager.s_ManagedSystemTypeToIndex.ContainsKey(type))
			{
				return;
			}
			SystemTypeIndex systemTypeIndex = TypeManager.s_SystemCount++ | systemTypeFlags;
			if (type != null)
			{
				TypeManager.s_ManagedSystemTypeToIndex.Add(type, systemTypeIndex);
				*TypeManager.SharedSystemTypeIndex.Get(type) = systemTypeIndex;
			}
			TypeManager.s_SystemTypes.Add(type);
			TypeManager.s_SystemTypeSizes.Add(typeSize);
			TypeManager.s_SystemTypeHashes.Add(in typeHash);
			UnsafeText unsafeText = new UnsafeText(-1, Allocator.Persistent);
			byte[] bytes = Encoding.UTF8.GetBytes(typeName);
			byte[] array;
			byte* ptr;
			if ((array = bytes) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			(ref unsafeText).Append<UnsafeText>(ptr, (int)((ushort)bytes.Length));
			array = null;
			TypeManager.s_SystemTypeNames.Add(in unsafeText);
			TypeManager.s_SystemTypeFlagsList.Add(in systemTypeFlags);
			TypeManager.s_SystemFilterFlagsList.Add(in filterFlags);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000548CC File Offset: 0x00052ACC
		internal unsafe static void InitializeSystemSharedStatics()
		{
			*TypeManager.SharedSystemTypeNames.Ref.Data = new IntPtr((void*)TypeManager.s_SystemTypeNames.Ptr);
			*TypeManager.SharedSystemAttributes.Ref.Data = new IntPtr((void*)TypeManager.s_SystemAttributes.Ptr);
			*TypeManager.SharedSystemCount.Ref.Data = TypeManager.s_SystemCount;
			*TypeManager.SharedSystemTypeHashes.Ref.Data = new IntPtr((void*)TypeManager.s_SystemTypeHashes.Ptr);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00054938 File Offset: 0x00052B38
		internal static WorldSystemFilterFlags GetSystemFilterFlags(Type type)
		{
			int index = TypeManager.GetSystemTypeIndex(type).Index;
			return TypeManager.s_SystemFilterFlagsList[index];
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0005495F File Offset: 0x00052B5F
		internal static WorldSystemFilterFlags GetSystemFilterFlags(SystemTypeIndex i)
		{
			return TypeManager.s_SystemFilterFlagsList[i.Index];
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00054972 File Offset: 0x00052B72
		public static bool IsSystemType(Type type)
		{
			return TypeManager.GetSystemTypeIndexNoThrow(type) != -1;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00054988 File Offset: 0x00052B88
		internal unsafe static bool IsSystemTypeIndex(SystemTypeIndex systemTypeIndex)
		{
			int num = systemTypeIndex & 134217727;
			return num >= 0 && num < *TypeManager.SharedSystemCount.Ref.Data;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000549B6 File Offset: 0x00052BB6
		internal unsafe static UnsafeText* GetSystemTypeNamesPointer()
		{
			return (UnsafeText*)(void*)(*TypeManager.SharedSystemTypeNames.Ref.Data);
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000549C8 File Offset: 0x00052BC8
		internal unsafe static UnsafeText* GetSystemNameInternal(SystemTypeIndex systemIndex)
		{
			return TypeManager.GetSystemTypeNamesPointer() + systemIndex.Index;
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000549DF File Offset: 0x00052BDF
		public static NativeText.ReadOnly GetSystemName(Type type)
		{
			return TypeManager.GetSystemName(Math.Max(0, TypeManager.GetSystemTypeIndexNoThrow(type)));
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000549FC File Offset: 0x00052BFC
		public unsafe static NativeText.ReadOnly GetSystemName(SystemTypeIndex systemIndex)
		{
			UnsafeText* systemNameInternal = TypeManager.GetSystemNameInternal(systemIndex);
			return new NativeText.ReadOnly(systemNameInternal);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00054A16 File Offset: 0x00052C16
		public unsafe static SystemTypeIndex GetSystemTypeIndex<T>()
		{
			int num = *TypeManager.SharedSystemTypeIndex<T>.Ref.Data;
			if (num <= 0)
			{
				TypeManager.ManagedException<T>();
			}
			return num;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00054A31 File Offset: 0x00052C31
		internal unsafe static SystemTypeIndex GetSystemTypeIndexNoThrow<T>()
		{
			return *TypeManager.SharedSystemTypeIndex<T>.Ref.Data;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00054A44 File Offset: 0x00052C44
		internal static long GetSystemTypeHash(Type type)
		{
			return TypeManager.s_SystemTypeHashes[TypeManager.GetSystemTypeIndex(type).Index];
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00054A69 File Offset: 0x00052C69
		internal unsafe static long GetSystemTypeHash(SystemTypeIndex systemTypeIndex)
		{
			return *(long*)((byte*)(void*)(*TypeManager.SharedSystemTypeHashes.Ref.Data) + (IntPtr)systemTypeIndex.Index * 8);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00054A87 File Offset: 0x00052C87
		internal static long GetSystemTypeHash<T>()
		{
			return Unity.Burst.BurstRuntime.GetHashCode64<T>();
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00054A90 File Offset: 0x00052C90
		internal static int GetSystemTypeSize(Type type)
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(type);
			return TypeManager.s_SystemTypeSizes[systemTypeIndex.Index];
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00054AB5 File Offset: 0x00052CB5
		internal static int GetSystemTypeSize(SystemTypeIndex index)
		{
			return TypeManager.s_SystemTypeSizes[index.Index];
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00054AC8 File Offset: 0x00052CC8
		internal static int GetSystemTypeSize<T>()
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex<T>();
			return TypeManager.s_SystemTypeSizes[systemTypeIndex.Index];
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00054AEC File Offset: 0x00052CEC
		internal static SystemTypeIndex GetSystemTypeIndexNoThrow(Type type)
		{
			if (type == null)
			{
				return 0;
			}
			SystemTypeIndex systemTypeIndex;
			if (TypeManager.s_ManagedSystemTypeToIndex.TryGetValue(type, out systemTypeIndex))
			{
				return systemTypeIndex;
			}
			return -1;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00054B20 File Offset: 0x00052D20
		public static SystemTypeIndex GetSystemTypeIndex(Type type)
		{
			SystemTypeIndex systemTypeIndexNoThrow = TypeManager.GetSystemTypeIndexNoThrow(type);
			if (!(systemTypeIndexNoThrow == -1))
			{
				return systemTypeIndexNoThrow;
			}
			if (!typeof(ISystem).IsAssignableFrom(type) && !typeof(ComponentSystemBase).IsAssignableFrom(type))
			{
				throw new ArgumentException(string.Format("The passed-in Type {0} is not a type that derives from SystemBase or ISystem", type));
			}
			int num = TypeManager.s_SystemCount;
			TypeManager.AddSystemTypeToTablesAfterInit(type);
			return num;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00054B8C File Offset: 0x00052D8C
		internal static Type GetSystemType(SystemTypeIndex systemTypeIndex)
		{
			int index = systemTypeIndex.Index;
			return TypeManager.s_SystemTypes[index];
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00054BAC File Offset: 0x00052DAC
		public static bool IsSystemAGroup(Type type)
		{
			return TypeManager.GetSystemTypeIndex(type).IsGroup;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00054BC8 File Offset: 0x00052DC8
		public static bool IsSystemManaged(Type type)
		{
			return TypeManager.GetSystemTypeIndex(type).IsManaged;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00054BE3 File Offset: 0x00052DE3
		public static int GetSystemTypeFlags(SystemTypeIndex systemTypeIndex)
		{
			return TypeManager.s_SystemTypeFlagsList[systemTypeIndex.Index];
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00054BF8 File Offset: 0x00052DF8
		internal unsafe static NativeList<TypeManager.SystemAttribute> GetSystemAttributes(SystemTypeIndex systemTypeIndex, TypeManager.SystemAttributeKind kind, Allocator allocator = Allocator.Temp)
		{
			UnsafeList<TypeManager.SystemAttribute>* ptr = (UnsafeList<TypeManager.SystemAttribute>*)(void*)(*TypeManager.SharedSystemAttributes.Ref.Data);
			if (TypeManager.IsSystemTypeIndex(systemTypeIndex))
			{
				UnsafeList<TypeManager.SystemAttribute> unsafeList = ptr[(IntPtr)systemTypeIndex.Index * (IntPtr)sizeof(UnsafeList<TypeManager.SystemAttribute>) / (IntPtr)sizeof(UnsafeList<TypeManager.SystemAttribute>)];
				NativeList<TypeManager.SystemAttribute> nativeList = new NativeList<TypeManager.SystemAttribute>(unsafeList.Length, allocator);
				for (int i = 0; i < unsafeList.Length; i++)
				{
					if (unsafeList[i].Kind == kind)
					{
						TypeManager.SystemAttribute systemAttribute = unsafeList[i];
						nativeList.Add(in systemAttribute);
					}
				}
				return nativeList;
			}
			Debug.LogError(string.Format("System type index {0} is not valid, returning empty attribute list.", systemTypeIndex));
			return new NativeList<TypeManager.SystemAttribute>(0, allocator);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00054CA4 File Offset: 0x00052EA4
		public static Attribute[] GetSystemAttributes(Type systemType, Type attributeType)
		{
			Type typeFromHandle = typeof(DisableAutoCreationAttribute);
			Attribute[] array;
			if (attributeType == typeFromHandle)
			{
				object[] customAttributes = systemType.GetCustomAttributes(attributeType, false);
				List<Attribute> list = new List<Attribute>();
				bool flag = false;
				for (int i = 0; i < customAttributes.Length; i++)
				{
					Attribute attribute = customAttributes[i] as Attribute;
					list.Add(attribute);
					if (attribute.GetType() == typeFromHandle)
					{
						flag = true;
					}
				}
				if (!flag && systemType.Assembly.GetCustomAttribute(attributeType) != null)
				{
					list.Add(new DisableAutoCreationAttribute());
				}
				array = list.ToArray();
			}
			else
			{
				object[] customAttributes2 = systemType.GetCustomAttributes(attributeType, true);
				array = new Attribute[customAttributes2.Length];
				for (int j = 0; j < customAttributes2.Length; j++)
				{
					array[j] = customAttributes2[j] as Attribute;
				}
			}
			return array;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00054D6C File Offset: 0x00052F6C
		private static bool FilterSystemType(SystemTypeIndex systemTypeIndex, TypeManager.LookupFlags lookupFlags)
		{
			NativeList<TypeManager.SystemAttribute> systemAttributes = TypeManager.GetSystemAttributes(systemTypeIndex, TypeManager.SystemAttributeKind.DisableAutoCreation, Allocator.Temp);
			if (systemTypeIndex.IsManaged && !systemTypeIndex.HasDefaultCtor)
			{
				if (systemAttributes.Length == 0)
				{
					Debug.LogWarning(string.Format("Missing default ctor on {0} (or if you don't want this to be auto-creatable, tag it with [DisableAutoCreation])", TypeManager.GetSystemName(systemTypeIndex)));
				}
				return false;
			}
			if (systemAttributes.Length > 0)
			{
				return false;
			}
			if (lookupFlags.OptionalFlags == (WorldSystemFilterFlags)4294967295U)
			{
				return true;
			}
			WorldSystemFilterFlags systemFilterFlags = TypeManager.GetSystemFilterFlags(systemTypeIndex);
			if ((lookupFlags.RequiredFlags & WorldSystemFilterFlags.Editor) != (WorldSystemFilterFlags)0U)
			{
				lookupFlags.OptionalFlags |= WorldSystemFilterFlags.Editor;
			}
			return (lookupFlags.OptionalFlags & systemFilterFlags) != (WorldSystemFilterFlags)0U && (lookupFlags.RequiredFlags & systemFilterFlags) == lookupFlags.RequiredFlags;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00054E0C File Offset: 0x0005300C
		internal static IEnumerable<Type> GetTypesDerivedFrom(Type type)
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (TypeManager.IsAssemblyReferencingEntities(assembly))
				{
					try
					{
						foreach (Type type2 in assembly.GetTypes())
						{
							if (type.IsAssignableFrom(type2))
							{
								list.Add(type2);
							}
						}
					}
					catch (ReflectionTypeLoadException ex)
					{
						foreach (Type type3 in ex.Types)
						{
							if (type3 != null && type.IsAssignableFrom(type3))
							{
								list.Add(type3);
							}
						}
						Debug.LogWarning("DefaultWorldInitialization failed loading assembly: " + (assembly.IsDynamic ? assembly.ToString() : assembly.Location));
					}
				}
			}
			return list;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00054EFC File Offset: 0x000530FC
		private static WorldSystemFilterFlags GetParentGroupDefaultFilterFlags(Type type, ref HashSet<Type> visitedSystemGroupsSet)
		{
			if (!Attribute.IsDefined(type, typeof(UpdateInGroupAttribute), true))
			{
				return WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation;
			}
			IEnumerable<UpdateInGroupAttribute> customAttributes = type.GetCustomAttributes<UpdateInGroupAttribute>(true);
			WorldSystemFilterFlags worldSystemFilterFlags = (WorldSystemFilterFlags)0U;
			foreach (UpdateInGroupAttribute updateInGroupAttribute in customAttributes)
			{
				Type groupType = updateInGroupAttribute.GroupType;
				if (!visitedSystemGroupsSet.Add(groupType))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("The following systems form a cycle in their UpdateInGroup attributes: ");
					foreach (Type type2 in visitedSystemGroupsSet)
					{
						stringBuilder.Append(string.Format("{0} ", type2));
					}
					throw new InvalidOperationException(stringBuilder.ToString());
				}
				WorldSystemFilterFlags worldSystemFilterFlags2 = WorldSystemFilterFlags.Default;
				if (Attribute.IsDefined(groupType, typeof(WorldSystemFilterAttribute), true))
				{
					worldSystemFilterFlags2 = groupType.GetCustomAttribute<WorldSystemFilterAttribute>(true).ChildDefaultFilterFlags;
				}
				if ((worldSystemFilterFlags2 & WorldSystemFilterFlags.Default) != (WorldSystemFilterFlags)0U)
				{
					worldSystemFilterFlags2 &= ~WorldSystemFilterFlags.Default;
					worldSystemFilterFlags2 |= TypeManager.GetParentGroupDefaultFilterFlags(groupType, ref visitedSystemGroupsSet);
				}
				worldSystemFilterFlags |= worldSystemFilterFlags2;
			}
			return worldSystemFilterFlags;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0005501C File Offset: 0x0005321C
		private static WorldSystemFilterFlags MakeWorldFilterFlags(Type type, ref HashSet<Type> visitedSystemGroupsSet)
		{
			WorldSystemFilterFlags worldSystemFilterFlags = WorldSystemFilterFlags.Default;
			if (Attribute.IsDefined(type, typeof(WorldSystemFilterAttribute), true))
			{
				worldSystemFilterFlags = type.GetCustomAttribute<WorldSystemFilterAttribute>(true).FilterFlags;
			}
			if ((worldSystemFilterFlags & WorldSystemFilterFlags.Default) != (WorldSystemFilterFlags)0U)
			{
				worldSystemFilterFlags &= ~WorldSystemFilterFlags.Default;
				visitedSystemGroupsSet.Clear();
				worldSystemFilterFlags |= TypeManager.GetParentGroupDefaultFilterFlags(type, ref visitedSystemGroupsSet);
			}
			if (Attribute.IsDefined(type, typeof(ExecuteInEditMode)))
			{
				Debug.LogWarning(string.Format("{0} is decorated with {1}. Support for this attribute on systems is deprecated as it is meant for MonoBehaviours only. ", type, typeof(ExecuteInEditMode)) + "Please use [WorldSystemFilter(Editor)] instead to ensure your system is added and runs in the Editor's default world.");
			}
			if (Attribute.IsDefined(type, typeof(ExecuteAlways)))
			{
				Debug.LogWarning(string.Format("{0} is decorated with {1}. Support for this attribute on systems is deprecated as it is meant for MonoBehaviours only. ", type, typeof(ExecuteAlways)) + "Please use the [WorldSystemFilter] attribute to specify if you want to ensure your system is added and runs in the Editor (Editor) and/or Player (Default) default world. You can specify [WorldSystemFilter(Editor | Default] for both.");
				worldSystemFilterFlags |= WorldSystemFilterFlags.Editor;
			}
			return worldSystemFilterFlags;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000550D8 File Offset: 0x000532D8
		[CompilerGenerated]
		internal static TypeManager.TypeTreeNode <BuildTypeTree>g__Add|153_0(Type type, ref TypeManager.<>c__DisplayClass153_0 A_1)
		{
			TypeManager.TypeTreeNode typeTreeNode = new TypeManager.TypeTreeNode
			{
				Type = type,
				Children = null,
				Parent = null
			};
			TypeManager.TypeTreeNode typeTreeNode2 = TypeManager.<BuildTypeTree>g__LookupOrAdd|153_1(type.BaseType, ref A_1);
			if (typeTreeNode2 != null)
			{
				if (typeTreeNode2.Children == null)
				{
					typeTreeNode2.Children = new List<TypeManager.TypeTreeNode>();
				}
				typeTreeNode2.Children.Add(typeTreeNode);
				typeTreeNode.Parent = typeTreeNode2;
			}
			A_1.typeTree[type] = typeTreeNode;
			return typeTreeNode;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00055144 File Offset: 0x00053344
		[CompilerGenerated]
		internal static TypeManager.TypeTreeNode <BuildTypeTree>g__LookupOrAdd|153_1(Type type, ref TypeManager.<>c__DisplayClass153_0 A_1)
		{
			if (type == null)
			{
				return null;
			}
			if (!A_1.componentTypeSet.Contains(type))
			{
				return null;
			}
			if (A_1.typeTree.ContainsKey(type))
			{
				return A_1.typeTree[type];
			}
			return TypeManager.<BuildTypeTree>g__Add|153_0(type, ref A_1);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00055184 File Offset: 0x00053384
		[CompilerGenerated]
		internal static int <BuildTypeTree>g__DepthFirst|153_2(TypeManager.TypeTreeNode node, int index, ref TypeManager.<>c__DisplayClass153_0 A_2)
		{
			int num = 1;
			if (node.Children != null)
			{
				for (int i = 0; i < node.Children.Count; i++)
				{
					TypeManager.TypeTreeNode typeTreeNode = node.Children[i];
					num += TypeManager.<BuildTypeTree>g__DepthFirst|153_2(typeTreeNode, index + num, ref A_2);
				}
			}
			node.IndexInTypeArray = index;
			A_2.descendantCountByType[node.Type] = num - 1;
			return num;
		}

		// Token: 0x040007CD RID: 1997
		public const int MaximumTypesCount = 8192;

		// Token: 0x040007CE RID: 1998
		public const int HasNoEntityReferencesFlag = 131072;

		// Token: 0x040007CF RID: 1999
		public const int IsNotChunkSerializableTypeFlag = 262144;

		// Token: 0x040007D0 RID: 2000
		public const int HasNativeContainerFlag = 524288;

		// Token: 0x040007D1 RID: 2001
		public const int BakingOnlyTypeFlag = 1048576;

		// Token: 0x040007D2 RID: 2002
		public const int TemporaryBakingTypeFlag = 2097152;

		// Token: 0x040007D3 RID: 2003
		public const int IRefCountedComponentFlag = 4194304;

		// Token: 0x040007D4 RID: 2004
		public const int IEquatableTypeFlag = 8388608;

		// Token: 0x040007D5 RID: 2005
		public const int EnableableComponentFlag = 16777216;

		// Token: 0x040007D6 RID: 2006
		[Obsolete("SystemStateTypeFlag has been renamed to CleanupComponentTypeFlag. SystemStateTypeFlag will be removed in a future package release. (UnityUpgradable) -> CleanupComponentTypeFlag", false)]
		public const int SystemStateTypeFlag = 33554432;

		// Token: 0x040007D7 RID: 2007
		public const int CleanupComponentTypeFlag = 33554432;

		// Token: 0x040007D8 RID: 2008
		public const int BufferComponentTypeFlag = 67108864;

		// Token: 0x040007D9 RID: 2009
		public const int SharedComponentTypeFlag = 134217728;

		// Token: 0x040007DA RID: 2010
		public const int ManagedComponentTypeFlag = 268435456;

		// Token: 0x040007DB RID: 2011
		public const int ChunkComponentTypeFlag = 536870912;

		// Token: 0x040007DC RID: 2012
		public const int ZeroSizeInChunkTypeFlag = 1073741824;

		// Token: 0x040007DD RID: 2013
		[Obsolete("SystemStateSharedComponentTypeFlag has been renamed to CleanupSharedComponentTypeFlag. SystemStateSharedComponentTypeFlag will be removed in a future package release. (UnityUpgradable) -> CleanupSharedComponentTypeFlag", false)]
		public const int SystemStateSharedComponentTypeFlag = 167772160;

		// Token: 0x040007DE RID: 2014
		public const int CleanupSharedComponentTypeFlag = 167772160;

		// Token: 0x040007DF RID: 2015
		public const int CleanupBufferComponentTypeFlag = 100663296;

		// Token: 0x040007E0 RID: 2016
		public const int ManagedSharedComponentTypeFlag = 402653184;

		// Token: 0x040007E1 RID: 2017
		public const int ClearFlagsMask = 8191;

		// Token: 0x040007E2 RID: 2018
		public const int MaximumChunkCapacity = 128;

		// Token: 0x040007E3 RID: 2019
		public const int MaximumSupportedAlignment = 16;

		// Token: 0x040007E4 RID: 2020
		public const int DefaultBufferCapacityNumerator = 128;

		// Token: 0x040007E5 RID: 2021
		private const int kInitialComponentCount = 2;

		// Token: 0x040007E6 RID: 2022
		private static int s_TypeCount;

		// Token: 0x040007E7 RID: 2023
		private static bool s_Initialized;

		// Token: 0x040007E8 RID: 2024
		private static NativeArray<TypeManager.TypeInfo> s_TypeInfos;

		// Token: 0x040007E9 RID: 2025
		private static UnsafeParallelHashMap<ulong, TypeIndex> s_StableTypeHashToTypeIndex;

		// Token: 0x040007EA RID: 2026
		private static NativeList<TypeManager.EntityOffsetInfo> s_EntityOffsetList;

		// Token: 0x040007EB RID: 2027
		private static NativeList<TypeManager.EntityOffsetInfo> s_BlobAssetRefOffsetList;

		// Token: 0x040007EC RID: 2028
		private static NativeList<TypeManager.EntityOffsetInfo> s_WeakAssetRefOffsetList;

		// Token: 0x040007ED RID: 2029
		private static NativeList<TypeManager.EntityOffsetInfo> s_UnityObjectRefOffsetList;

		// Token: 0x040007EE RID: 2030
		private static NativeList<TypeIndex> s_WriteGroupList;

		// Token: 0x040007EF RID: 2031
		private static NativeList<FastEquality.TypeInfo> s_FastEqualityTypeInfoList;

		// Token: 0x040007F0 RID: 2032
		private static List<Type> s_Types;

		// Token: 0x040007F1 RID: 2033
		private static UnsafeList<UnsafeText> s_TypeNames;

		// Token: 0x040007F2 RID: 2034
		private static UnsafeList<ulong> s_TypeFullNameHashes;

		// Token: 0x040007F3 RID: 2035
		private static NativeArray<int> s_DescendantCounts;

		// Token: 0x040007F4 RID: 2036
		private static TypeManager.ManagedSharedComponentFnPtrs[] s_SharedComponentFns_gcDefeat;

		// Token: 0x040007F5 RID: 2037
		private static UnsafeList<TypeManager.SharedComponentFnPtrs> s_SharedComponent_FunctionPointers;

		// Token: 0x040007F6 RID: 2038
		private static bool s_AppDomainUnloadRegistered;

		// Token: 0x040007F7 RID: 2039
		private static Dictionary<Type, TypeIndex> s_ManagedTypeToIndex;

		// Token: 0x040007F8 RID: 2040
		private static Dictionary<Type, Exception> s_FailedTypeBuildException;

		// Token: 0x040007F9 RID: 2041
		public static int ObjectOffset;

		// Token: 0x040007FA RID: 2042
		internal static Type UnityEngineObjectType;

		// Token: 0x040007FB RID: 2043
		private static Type[] s_AspectTypes;

		// Token: 0x040007FC RID: 2044
		private static int s_SystemCount;

		// Token: 0x040007FD RID: 2045
		private static List<Type> s_SystemTypes;

		// Token: 0x040007FE RID: 2046
		private static Dictionary<Type, SystemTypeIndex> s_ManagedSystemTypeToIndex;

		// Token: 0x040007FF RID: 2047
		private static List<int> s_SystemTypeSizes;

		// Token: 0x04000800 RID: 2048
		private static UnsafeList<long> s_SystemTypeHashes;

		// Token: 0x04000801 RID: 2049
		private static Dictionary<TypeManager.LookupFlags, NativeList<SystemTypeIndex>> s_SystemFilterTypeMap;

		// Token: 0x04000802 RID: 2050
		private static UnsafeList<int> s_SystemTypeFlagsList;

		// Token: 0x04000803 RID: 2051
		private static UnsafeList<WorldSystemFilterFlags> s_SystemFilterFlagsList;

		// Token: 0x04000804 RID: 2052
		private static UnsafeList<UnsafeText> s_SystemTypeNames;

		// Token: 0x04000805 RID: 2053
		private static UnsafeList<UnsafeList<TypeManager.SystemAttribute>> s_SystemAttributes;

		// Token: 0x02000326 RID: 806
		[AttributeUsage(AttributeTargets.Struct)]
		public class ForcedMemoryOrderingAttribute : Attribute
		{
			// Token: 0x06001633 RID: 5683 RVA: 0x000551E7 File Offset: 0x000533E7
			public ForcedMemoryOrderingAttribute(ulong ordering)
			{
				this.MemoryOrdering = ordering;
			}

			// Token: 0x04000806 RID: 2054
			public ulong MemoryOrdering;
		}

		// Token: 0x02000327 RID: 807
		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
		public class TypeOverridesAttribute : Attribute
		{
			// Token: 0x06001634 RID: 5684 RVA: 0x000551F6 File Offset: 0x000533F6
			public TypeOverridesAttribute(bool hasNoEntityReferences, bool hasNoBlobReferences, bool hasNoUnityObjectReferences = false)
			{
				this.HasNoEntityReferences = hasNoEntityReferences;
				this.HasNoBlobReferences = hasNoBlobReferences;
				this.HasNoUnityObjectReferences = hasNoUnityObjectReferences;
				this.AllowForceNoReferences = false;
			}

			// Token: 0x06001635 RID: 5685 RVA: 0x0005521A File Offset: 0x0005341A
			internal TypeOverridesAttribute(bool hasNoEntityReferences, bool hasNoBlobReferences, bool hasNoUnityObjectReferences, bool allowForcedNoReferences)
			{
				this.HasNoEntityReferences = hasNoEntityReferences;
				this.HasNoBlobReferences = hasNoBlobReferences;
				this.HasNoUnityObjectReferences = hasNoUnityObjectReferences;
				this.AllowForceNoReferences = allowForcedNoReferences;
			}

			// Token: 0x04000807 RID: 2055
			public bool HasNoEntityReferences;

			// Token: 0x04000808 RID: 2056
			public bool HasNoBlobReferences;

			// Token: 0x04000809 RID: 2057
			public bool HasNoUnityObjectReferences;

			// Token: 0x0400080A RID: 2058
			internal bool AllowForceNoReferences;
		}

		// Token: 0x02000328 RID: 808
		[AttributeUsage(AttributeTargets.Struct)]
		public class TypeVersionAttribute : Attribute
		{
			// Token: 0x06001636 RID: 5686 RVA: 0x0005523F File Offset: 0x0005343F
			public TypeVersionAttribute(int version)
			{
				this.TypeVersion = version;
			}

			// Token: 0x0400080B RID: 2059
			public int TypeVersion;
		}

		// Token: 0x02000329 RID: 809
		public enum TypeCategory
		{
			// Token: 0x0400080D RID: 2061
			ComponentData,
			// Token: 0x0400080E RID: 2062
			BufferData,
			// Token: 0x0400080F RID: 2063
			ISharedComponentData,
			// Token: 0x04000810 RID: 2064
			EntityData,
			// Token: 0x04000811 RID: 2065
			UnityEngineObject
		}

		// Token: 0x0200032A RID: 810
		public struct SharedComponentFnPtrs
		{
			// Token: 0x04000812 RID: 2066
			public FunctionPointer<IRefCounted.RefCountDelegate> RetainFn;

			// Token: 0x04000813 RID: 2067
			public FunctionPointer<IRefCounted.RefCountDelegate> ReleaseFn;

			// Token: 0x04000814 RID: 2068
			public FunctionPointer<FastEquality.TypeInfo.CompareEqualDelegate> EqualsFn;

			// Token: 0x04000815 RID: 2069
			public FunctionPointer<FastEquality.TypeInfo.GetHashCodeDelegate> GetHashCodeFn;
		}

		// Token: 0x0200032B RID: 811
		public struct ManagedSharedComponentFnPtrs
		{
			// Token: 0x04000816 RID: 2070
			public IRefCounted.RefCountDelegate RetainFn;

			// Token: 0x04000817 RID: 2071
			public IRefCounted.RefCountDelegate ReleaseFn;

			// Token: 0x04000818 RID: 2072
			public FastEquality.TypeInfo.CompareEqualDelegate EqualsFn;

			// Token: 0x04000819 RID: 2073
			public FastEquality.TypeInfo.GetHashCodeDelegate GetHashCodeFn;
		}

		// Token: 0x0200032C RID: 812
		private struct ObjectOffsetType
		{
			// Token: 0x0400081A RID: 2074
			private unsafe void* v0;

			// Token: 0x0400081B RID: 2075
			private unsafe void* v1;
		}

		// Token: 0x0200032D RID: 813
		public struct EntityOffsetInfo
		{
			// Token: 0x0400081C RID: 2076
			public int Offset;
		}

		// Token: 0x0200032E RID: 814
		[GenerateTestsForBurstCompatibility]
		public readonly struct TypeInfo
		{
			// Token: 0x06001637 RID: 5687 RVA: 0x00055250 File Offset: 0x00053450
			public TypeInfo(int typeIndex, TypeManager.TypeCategory category, int entityOffsetCount, int entityOffsetStartIndex, ulong memoryOrdering, ulong stableTypeHash, int bufferCapacity, int sizeInChunk, int elementSize, int alignmentInBytes, int maximumChunkCapacity, int writeGroupCount, int writeGroupStartIndex, bool hasBlobRefs, int blobAssetRefOffsetCount, int blobAssetRefOffsetStartIndex, int weakAssetRefOffsetCount, int weakAssetRefOffsetStartIndex, bool hasUnityObjectRefs, int unityObjectRefOffsetCount, int unityObjectRefOffsetStartIndex, int typeSize, ulong bloomFilterMask = 0UL)
			{
				this.TypeIndex = new TypeIndex
				{
					Value = typeIndex
				};
				this.Category = category;
				this.EntityOffsetCount = entityOffsetCount;
				this.EntityOffsetStartIndex = entityOffsetStartIndex;
				this.MemoryOrdering = memoryOrdering;
				this.StableTypeHash = stableTypeHash;
				this.BloomFilterMask = bloomFilterMask;
				this.BufferCapacity = bufferCapacity;
				this.SizeInChunk = sizeInChunk;
				this.ElementSize = elementSize;
				this.AlignmentInBytes = alignmentInBytes;
				this.MaximumChunkCapacity = maximumChunkCapacity;
				this.WriteGroupCount = writeGroupCount;
				this.WriteGroupStartIndex = writeGroupStartIndex;
				this._HasBlobAssetRefs = (hasBlobRefs ? 1 : 0);
				this._HasUnityObjectRefs = (hasUnityObjectRefs ? 1 : 0);
				this.BlobAssetRefOffsetCount = blobAssetRefOffsetCount;
				this.BlobAssetRefOffsetStartIndex = blobAssetRefOffsetStartIndex;
				this.WeakAssetRefOffsetCount = weakAssetRefOffsetCount;
				this.WeakAssetRefOffsetStartIndex = weakAssetRefOffsetStartIndex;
				this.UnityObjectRefOffsetCount = unityObjectRefOffsetCount;
				this.UnityObjectRefOffsetStartIndex = unityObjectRefOffsetStartIndex;
				this.TypeSize = typeSize;
			}

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x06001638 RID: 5688 RVA: 0x0005532E File Offset: 0x0005352E
			public int AlignmentInChunkInBytes
			{
				get
				{
					if (this.Category == TypeManager.TypeCategory.BufferData)
					{
						return 16;
					}
					return this.AlignmentInBytes;
				}
			}

			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x06001639 RID: 5689 RVA: 0x00055342 File Offset: 0x00053542
			public bool TemporaryBakingType
			{
				get
				{
					return TypeManager.IsTemporaryBakingType(this.TypeIndex);
				}
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x0600163A RID: 5690 RVA: 0x0005534F File Offset: 0x0005354F
			public bool BakingOnlyType
			{
				get
				{
					return TypeManager.IsBakingOnlyType(this.TypeIndex);
				}
			}

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x0600163B RID: 5691 RVA: 0x0005535C File Offset: 0x0005355C
			public bool EnableableType
			{
				get
				{
					return TypeManager.IsEnableableType(this.TypeIndex);
				}
			}

			// Token: 0x170001CA RID: 458
			// (get) Token: 0x0600163C RID: 5692 RVA: 0x00055369 File Offset: 0x00053569
			public bool IsZeroSized
			{
				get
				{
					return this.SizeInChunk == 0;
				}
			}

			// Token: 0x170001CB RID: 459
			// (get) Token: 0x0600163D RID: 5693 RVA: 0x00055374 File Offset: 0x00053574
			public bool HasWriteGroups
			{
				get
				{
					return this.WriteGroupCount > 0;
				}
			}

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x0600163E RID: 5694 RVA: 0x0005537F File Offset: 0x0005357F
			public bool HasBlobAssetRefs
			{
				get
				{
					return this._HasBlobAssetRefs != 0;
				}
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x0600163F RID: 5695 RVA: 0x0005538A File Offset: 0x0005358A
			public bool HasWeakAssetRefs
			{
				get
				{
					return this.WeakAssetRefOffsetCount != 0;
				}
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x06001640 RID: 5696 RVA: 0x00055395 File Offset: 0x00053595
			public bool HasUnityObjectRefs
			{
				get
				{
					return this._HasUnityObjectRefs != 0;
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x06001641 RID: 5697 RVA: 0x000553A0 File Offset: 0x000535A0
			[ExcludeFromBurstCompatTesting("Returns managed Type")]
			public Type Type
			{
				get
				{
					return TypeManager.GetType(this.TypeIndex);
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x06001642 RID: 5698 RVA: 0x000553B0 File Offset: 0x000535B0
			[GenerateTestsForBurstCompatibility]
			public unsafe NativeText.ReadOnly DebugTypeName
			{
				get
				{
					UnsafeText* typeName = TypeManager.GetTypeName(this.TypeIndex);
					return new NativeText.ReadOnly(typeName);
				}
			}

			// Token: 0x0400081D RID: 2077
			public readonly TypeIndex TypeIndex;

			// Token: 0x0400081E RID: 2078
			public readonly int SizeInChunk;

			// Token: 0x0400081F RID: 2079
			public readonly int ElementSize;

			// Token: 0x04000820 RID: 2080
			public readonly int BufferCapacity;

			// Token: 0x04000821 RID: 2081
			public readonly ulong MemoryOrdering;

			// Token: 0x04000822 RID: 2082
			public readonly ulong StableTypeHash;

			// Token: 0x04000823 RID: 2083
			public readonly ulong BloomFilterMask;

			// Token: 0x04000824 RID: 2084
			public readonly int AlignmentInBytes;

			// Token: 0x04000825 RID: 2085
			public readonly TypeManager.TypeCategory Category;

			// Token: 0x04000826 RID: 2086
			public readonly int EntityOffsetCount;

			// Token: 0x04000827 RID: 2087
			internal readonly int EntityOffsetStartIndex;

			// Token: 0x04000828 RID: 2088
			private readonly int _HasBlobAssetRefs;

			// Token: 0x04000829 RID: 2089
			private readonly int _HasUnityObjectRefs;

			// Token: 0x0400082A RID: 2090
			public readonly int BlobAssetRefOffsetCount;

			// Token: 0x0400082B RID: 2091
			internal readonly int BlobAssetRefOffsetStartIndex;

			// Token: 0x0400082C RID: 2092
			public readonly int WeakAssetRefOffsetCount;

			// Token: 0x0400082D RID: 2093
			internal readonly int WeakAssetRefOffsetStartIndex;

			// Token: 0x0400082E RID: 2094
			public readonly int UnityObjectRefOffsetCount;

			// Token: 0x0400082F RID: 2095
			internal readonly int UnityObjectRefOffsetStartIndex;

			// Token: 0x04000830 RID: 2096
			public readonly int WriteGroupCount;

			// Token: 0x04000831 RID: 2097
			internal readonly int WriteGroupStartIndex;

			// Token: 0x04000832 RID: 2098
			public readonly int MaximumChunkCapacity;

			// Token: 0x04000833 RID: 2099
			public readonly int TypeSize;
		}

		// Token: 0x0200032F RID: 815
		internal class TypeTreeNode
		{
			// Token: 0x04000834 RID: 2100
			public Type Type;

			// Token: 0x04000835 RID: 2101
			public TypeManager.TypeTreeNode Parent;

			// Token: 0x04000836 RID: 2102
			public List<TypeManager.TypeTreeNode> Children;

			// Token: 0x04000837 RID: 2103
			public int IndexInTypeArray;
		}

		// Token: 0x02000330 RID: 816
		internal class BuildComponentCache
		{
			// Token: 0x06001644 RID: 5700 RVA: 0x000553D0 File Offset: 0x000535D0
			public BuildComponentCache(int initialCapacity = 0)
			{
				this.TypeHashCache = new Dictionary<Type, ulong>(initialCapacity);
				this.NestedNativeContainerCache = new Dictionary<Type, bool>(initialCapacity);
				this.HasEntityOrBlobAssetReferenceCache = new Dictionary<Type, EntityRemapUtility.EntityBlobRefResult>(initialCapacity);
				this.FastEqualityLayoutInfoCache = new Dictionary<Type, List<FastEquality.LayoutInfo>>(initialCapacity);
				this.AllowedComponentCache = new HashSet<Type>(initialCapacity);
				this.ChunkSerializableCache = new Dictionary<Type, ValueTuple<bool, bool>>();
				this.CalculateFieldOffsetsUnmanagedCache = new HashSet<Type>(initialCapacity);
			}

			// Token: 0x04000838 RID: 2104
			public Dictionary<Type, ulong> TypeHashCache;

			// Token: 0x04000839 RID: 2105
			public Dictionary<Type, bool> NestedNativeContainerCache;

			// Token: 0x0400083A RID: 2106
			public Dictionary<Type, EntityRemapUtility.EntityBlobRefResult> HasEntityOrBlobAssetReferenceCache;

			// Token: 0x0400083B RID: 2107
			public Dictionary<Type, List<FastEquality.LayoutInfo>> FastEqualityLayoutInfoCache;

			// Token: 0x0400083C RID: 2108
			public Dictionary<Type, ValueTuple<bool, bool>> ChunkSerializableCache;

			// Token: 0x0400083D RID: 2109
			public HashSet<Type> AllowedComponentCache;

			// Token: 0x0400083E RID: 2110
			public HashSet<Type> CalculateFieldOffsetsUnmanagedCache;
		}

		// Token: 0x02000331 RID: 817
		private struct SharedTypeIndex
		{
			// Token: 0x06001645 RID: 5701 RVA: 0x00055438 File Offset: 0x00053638
			public static ref TypeIndex Get(Type componentType)
			{
				return SharedStatic<TypeIndex>.GetOrCreate(typeof(TypeManager.TypeManagerKeyContext), componentType, 0U).Data;
			}
		}

		// Token: 0x02000332 RID: 818
		private struct TypeManagerKeyContext
		{
		}

		// Token: 0x02000333 RID: 819
		private struct SharedTypeInfos
		{
			// Token: 0x0400083F RID: 2111
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -7370724514926575087L);
		}

		// Token: 0x02000334 RID: 820
		private struct SharedDescendantCounts
		{
			// Token: 0x04000840 RID: 2112
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -4225648524671505415L);
		}

		// Token: 0x02000335 RID: 821
		private struct SharedEntityOffsetInfos
		{
			// Token: 0x04000841 RID: 2113
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 4613171575622649367L);
		}

		// Token: 0x02000336 RID: 822
		private struct SharedStableTypeHashes
		{
			// Token: 0x04000842 RID: 2114
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 1091035532283752893L);
		}

		// Token: 0x02000337 RID: 823
		private struct SharedBlobAssetRefOffsets
		{
			// Token: 0x04000843 RID: 2115
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 1378319503455576808L);
		}

		// Token: 0x02000338 RID: 824
		private struct SharedWeakAssetRefOffsets
		{
			// Token: 0x04000844 RID: 2116
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 4529358897470670499L);
		}

		// Token: 0x02000339 RID: 825
		private struct SharedUnityObjectRefOffsets
		{
			// Token: 0x04000845 RID: 2117
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 8510461414510770097L);
		}

		// Token: 0x0200033A RID: 826
		private struct SharedWriteGroups
		{
			// Token: 0x04000846 RID: 2118
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 2323268239268127373L);
		}

		// Token: 0x0200033B RID: 827
		private struct SharedFastEqualityTypeInfo
		{
			// Token: 0x04000847 RID: 2119
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 6642422414414337826L);
		}

		// Token: 0x0200033C RID: 828
		private struct SharedTypeNames
		{
			// Token: 0x04000848 RID: 2120
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -8796907678697770368L);
		}

		// Token: 0x0200033D RID: 829
		private struct SharedTypeFullNameHashes
		{
			// Token: 0x04000849 RID: 2121
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -3734372280155314340L);
		}

		// Token: 0x0200033E RID: 830
		internal struct SharedTypeIndex<TComponent>
		{
			// Token: 0x0400084A RID: 2122
			public static readonly SharedStatic<TypeIndex> Ref = SharedStatic<TypeIndex>.GetOrCreatePartiallyUnsafeWithHashCode<TComponent>(0U, -7082256032705860890L);
		}

		// Token: 0x0200033F RID: 831
		private sealed class Shared_SharedComponentData_FnPtrs
		{
			// Token: 0x06001652 RID: 5714 RVA: 0x0000222E File Offset: 0x0000042E
			private Shared_SharedComponentData_FnPtrs()
			{
			}

			// Token: 0x0400084B RID: 2123
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -9072410514754595121L);
		}

		// Token: 0x02000340 RID: 832
		internal enum SystemAttributeKind
		{
			// Token: 0x0400084D RID: 2125
			UpdateBefore,
			// Token: 0x0400084E RID: 2126
			UpdateAfter,
			// Token: 0x0400084F RID: 2127
			CreateBefore,
			// Token: 0x04000850 RID: 2128
			CreateAfter,
			// Token: 0x04000851 RID: 2129
			DisableAutoCreation,
			// Token: 0x04000852 RID: 2130
			UpdateInGroup,
			// Token: 0x04000853 RID: 2131
			RequireMatchingQueriesForUpdate
		}

		// Token: 0x02000341 RID: 833
		internal struct SystemAttribute
		{
			// Token: 0x04000854 RID: 2132
			public const int kOrderFirstFlag = 1;

			// Token: 0x04000855 RID: 2133
			public const int kOrderLastFlag = 2;

			// Token: 0x04000856 RID: 2134
			public TypeManager.SystemAttributeKind Kind;

			// Token: 0x04000857 RID: 2135
			public SystemTypeIndex TargetSystemTypeIndex;

			// Token: 0x04000858 RID: 2136
			public int Flags;
		}

		// Token: 0x02000342 RID: 834
		internal readonly struct SystemTypeInfo
		{
			// Token: 0x06001654 RID: 5716 RVA: 0x000555E8 File Offset: 0x000537E8
			public SystemTypeInfo(int typeIndex, int size, long hash)
			{
				this.TypeIndex = typeIndex;
				this.Size = size;
				this.Hash = hash;
			}

			// Token: 0x170001D1 RID: 465
			// (get) Token: 0x06001655 RID: 5717 RVA: 0x000555FF File Offset: 0x000537FF
			public bool IsSystemGroup
			{
				get
				{
					return (this.TypeIndex & 1073741824) != 0;
				}
			}

			// Token: 0x170001D2 RID: 466
			// (get) Token: 0x06001656 RID: 5718 RVA: 0x00055610 File Offset: 0x00053810
			public Type Type
			{
				get
				{
					return TypeManager.GetSystemType(this.TypeIndex);
				}
			}

			// Token: 0x170001D3 RID: 467
			// (get) Token: 0x06001657 RID: 5719 RVA: 0x00055624 File Offset: 0x00053824
			[GenerateTestsForBurstCompatibility]
			public unsafe NativeText.ReadOnly DebugTypeName
			{
				get
				{
					UnsafeText* systemNameInternal = TypeManager.GetSystemNameInternal(this.TypeIndex);
					return new NativeText.ReadOnly(systemNameInternal);
				}
			}

			// Token: 0x04000859 RID: 2137
			public const int kIsSystemGroupFlag = 1073741824;

			// Token: 0x0400085A RID: 2138
			public const int kIsSystemManagedFlag = 536870912;

			// Token: 0x0400085B RID: 2139
			public const int kIsSystemISystemStartStopFlag = 268435456;

			// Token: 0x0400085C RID: 2140
			public const int kSystemHasDefaultCtor = 134217728;

			// Token: 0x0400085D RID: 2141
			public const int kClearSystemTypeFlagsMask = 134217727;

			// Token: 0x0400085E RID: 2142
			public readonly int TypeIndex;

			// Token: 0x0400085F RID: 2143
			public readonly int Size;

			// Token: 0x04000860 RID: 2144
			public readonly long Hash;
		}

		// Token: 0x02000343 RID: 835
		private struct LookupFlags
		{
			// Token: 0x04000861 RID: 2145
			public WorldSystemFilterFlags OptionalFlags;

			// Token: 0x04000862 RID: 2146
			public WorldSystemFilterFlags RequiredFlags;
		}

		// Token: 0x02000344 RID: 836
		private struct SharedSystemTypeIndex
		{
			// Token: 0x06001658 RID: 5720 RVA: 0x00055648 File Offset: 0x00053848
			public static ref SystemTypeIndex Get(Type systemType)
			{
				return SharedStatic<SystemTypeIndex>.GetOrCreate(typeof(TypeManager.TypeManagerKeyContext), systemType, 0U).Data;
			}
		}

		// Token: 0x02000345 RID: 837
		private struct SharedSystemTypeNames
		{
			// Token: 0x04000863 RID: 2147
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -3279719753113964411L);
		}

		// Token: 0x02000346 RID: 838
		private struct SharedSystemAttributes
		{
			// Token: 0x04000864 RID: 2148
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, 1607848033891335118L);
		}

		// Token: 0x02000347 RID: 839
		private struct SharedSystemTypeHashes
		{
			// Token: 0x04000865 RID: 2149
			public static readonly SharedStatic<IntPtr> Ref = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7082256032705860890L, -8182200663837825411L);
		}

		// Token: 0x02000348 RID: 840
		internal struct SharedSystemTypeIndex<TSystem>
		{
			// Token: 0x04000866 RID: 2150
			public static readonly SharedStatic<int> Ref = SharedStatic<int>.GetOrCreatePartiallyUnsafeWithHashCode<TSystem>(0U, -7082256032705860890L);
		}

		// Token: 0x02000349 RID: 841
		private struct SharedSystemCount
		{
			// Token: 0x04000867 RID: 2151
			public static readonly SharedStatic<int> Ref = SharedStatic<int>.GetOrCreateUnsafe(0U, -7082256032705860890L, -5423767520629440078L);
		}
	}
}
