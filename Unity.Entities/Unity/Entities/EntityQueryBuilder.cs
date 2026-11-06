using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200021C RID: 540
	[GenerateTestsForBurstCompatibility]
	public ref struct EntityQueryBuilder
	{
		// Token: 0x06000D35 RID: 3381 RVA: 0x0003BE20 File Offset: 0x0003A020
		public unsafe EntityQueryBuilder(AllocatorManager.AllocatorHandle allocator)
		{
			this._allocator = allocator;
			this._builderDataPtr = (ref this._allocator).Allocate(default(EntityQueryBuilder.BuilderData), 1);
			this._builderDataPtr->_typeData = new UnsafeList<ComponentType>(2, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_indexData = new UnsafeList<EntityQueryBuilder.QueryTypes>(2, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_all = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_any = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_none = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_disabled = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_absent = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_present = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_pendingOptions = EntityQueryOptions.Default;
			this._builderDataPtr->_isFinalized = 0;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0003BF28 File Offset: 0x0003A128
		internal unsafe EntityQueryBuilder(AllocatorManager.AllocatorHandle allocator, ComponentType* componentTypes, int count)
		{
			this._allocator = allocator;
			this._builderDataPtr = (ref this._allocator).Allocate(default(EntityQueryBuilder.BuilderData), 1);
			this._builderDataPtr->_typeData = new UnsafeList<ComponentType>(2, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_indexData = new UnsafeList<EntityQueryBuilder.QueryTypes>(2, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_all = new UnsafeList<ComponentType>(count, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_any = new UnsafeList<ComponentType>(1, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_none = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_disabled = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_absent = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_present = new UnsafeList<ComponentType>(6, this._allocator, NativeArrayOptions.UninitializedMemory);
			this._builderDataPtr->_pendingOptions = EntityQueryOptions.Default;
			this._builderDataPtr->_isFinalized = 0;
			for (int i = 0; i < count; i++)
			{
				ComponentType componentType = componentTypes[i];
				if (componentType.AccessModeType == ComponentType.AccessMode.Exclude)
				{
					ComponentType componentType2 = ComponentType.ReadOnly(componentType.TypeIndex);
					this.WithNone(&componentType2, 1);
				}
				else
				{
					this.WithAll(&componentType, 1);
				}
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0003C079 File Offset: 0x0003A279
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckBuilderPtr()
		{
			if (this._builderDataPtr == null)
			{
				throw new NullReferenceException("The EntityQueryBuilder has not been initialized! The EntityQueryBuilder needs to be passed an Allocator when created!");
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003C090 File Offset: 0x0003A290
		public unsafe EntityQueryBuilder WithOptions(EntityQueryOptions options)
		{
			this._builderDataPtr->_pendingOptions = options;
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003C0B0 File Offset: 0x0003A2B0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleAspect) })]
		public unsafe EntityQueryBuilder WithAspect<TAspect>() where TAspect : struct, IAspect, IAspectCreate<TAspect>
		{
			TAspect taspect = default(TAspect);
			taspect.AddComponentRequirementsTo(ref this._builderDataPtr->_all);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0003C0EE File Offset: 0x0003A2EE
		[Obsolete("Use WithAll<T,...> instead, or WithAll(INativeList) if component types are not known at compile time. (RemovedAfter Entities 1.0)", false)]
		public unsafe EntityQueryBuilder AddAll(ComponentType t)
		{
			this._builderDataPtr->_isFinalized = 0;
			return this.WithAll(&t, 1);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003C108 File Offset: 0x0003A308
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAll<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0003C158 File Offset: 0x0003A358
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003C1D4 File Offset: 0x0003A3D4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0003C280 File Offset: 0x0003A480
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003C358 File Offset: 0x0003A558
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003C460 File Offset: 0x0003A660
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003C594 File Offset: 0x0003A794
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAll<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003C6F8 File Offset: 0x0003A8F8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAllRW<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0003C748 File Offset: 0x0003A948
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAllRW<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._all.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr2._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0003C7C4 File Offset: 0x0003A9C4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAllChunkComponent<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponentReadOnly<T>();
			builderDataPtr._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0003C7FC File Offset: 0x0003A9FC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAllChunkComponentRW<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponent<T>();
			builderDataPtr._all.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0003C834 File Offset: 0x0003AA34
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithAll<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = componentTypes[i];
				builderDataPtr._all.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003C88C File Offset: 0x0003AA8C
		internal unsafe EntityQueryBuilder WithAll(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this._builderDataPtr->_all.Add(in componentTypes[i]);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0003C8D2 File Offset: 0x0003AAD2
		[Obsolete("Use WithAny<T,...> instead, or WithAny(INativeList) if component types are not known at compile time. (RemovedAfter Entities 1.0)", false)]
		public unsafe EntityQueryBuilder AddAny(ComponentType t)
		{
			this._builderDataPtr->_isFinalized = 0;
			return this.WithAny(&t, 1);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003C8EC File Offset: 0x0003AAEC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAny<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003C93C File Offset: 0x0003AB3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003C9B8 File Offset: 0x0003ABB8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003CA64 File Offset: 0x0003AC64
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003CB3C File Offset: 0x0003AD3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003CC44 File Offset: 0x0003AE44
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003CD78 File Offset: 0x0003AF78
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAny<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003CEDC File Offset: 0x0003B0DC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAnyRW<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003CF2C File Offset: 0x0003B12C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAnyRW<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._any.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr2._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003CFA8 File Offset: 0x0003B1A8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAnyChunkComponent<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponentReadOnly<T>();
			builderDataPtr._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003CFE0 File Offset: 0x0003B1E0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAnyChunkComponentRW<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponent<T>();
			builderDataPtr._any.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003D018 File Offset: 0x0003B218
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithAny<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = componentTypes[i];
				builderDataPtr._any.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0003D070 File Offset: 0x0003B270
		internal unsafe EntityQueryBuilder WithAny(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this._builderDataPtr->_any.Add(in componentTypes[i]);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003D0B6 File Offset: 0x0003B2B6
		[Obsolete("Use WithNone<T,...> instead, or WithNone(INativeList) if component types are not known at compile time. (RemovedAfter Entities 1.0)", false)]
		public unsafe EntityQueryBuilder AddNone(ComponentType t)
		{
			this._builderDataPtr->_isFinalized = 0;
			t.AccessModeType = ComponentType.AccessMode.ReadOnly;
			return this.WithNone(&t, 1);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0003D0D8 File Offset: 0x0003B2D8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithNone<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0003D128 File Offset: 0x0003B328
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003D1A4 File Offset: 0x0003B3A4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003D250 File Offset: 0x0003B450
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003D328 File Offset: 0x0003B528
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003D430 File Offset: 0x0003B630
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003D564 File Offset: 0x0003B764
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithNone<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._none.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003D6C8 File Offset: 0x0003B8C8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithNoneChunkComponent<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponentReadOnly<T>();
			builderDataPtr._none.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003D700 File Offset: 0x0003B900
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithNone<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = ComponentType.ReadOnly(componentTypes[i].TypeIndex);
				builderDataPtr._none.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0003D760 File Offset: 0x0003B960
		internal unsafe EntityQueryBuilder WithNone(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = ComponentType.ReadOnly(componentTypes[i].TypeIndex);
				builderDataPtr._none.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithDisabled<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0003D804 File Offset: 0x0003BA04
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003D880 File Offset: 0x0003BA80
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0003D92C File Offset: 0x0003BB2C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0003DA04 File Offset: 0x0003BC04
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003DB0C File Offset: 0x0003BD0C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003DC40 File Offset: 0x0003BE40
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabled<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0003DDA4 File Offset: 0x0003BFA4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithDisabledRW<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0003DDF4 File Offset: 0x0003BFF4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithDisabledRW<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._disabled.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr2._disabled.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0003DE70 File Offset: 0x0003C070
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithDisabled<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = componentTypes[i];
				builderDataPtr._disabled.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0003DEC8 File Offset: 0x0003C0C8
		internal unsafe EntityQueryBuilder WithDisabled(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this._builderDataPtr->_disabled.Add(in componentTypes[i]);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0003DF10 File Offset: 0x0003C110
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAbsent<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003DF60 File Offset: 0x0003C160
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003DFDC File Offset: 0x0003C1DC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003E088 File Offset: 0x0003C288
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0003E160 File Offset: 0x0003C360
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0003E268 File Offset: 0x0003C468
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0003E39C File Offset: 0x0003C59C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithAbsent<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._absent.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0003E500 File Offset: 0x0003C700
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithAbsentChunkComponent<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponentReadOnly<T>();
			builderDataPtr._absent.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003E538 File Offset: 0x0003C738
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithAbsent<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = ComponentType.ReadOnly(componentTypes[i].TypeIndex);
				builderDataPtr._absent.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0003E598 File Offset: 0x0003C798
		internal unsafe EntityQueryBuilder WithAbsent(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = ComponentType.ReadOnly(componentTypes[i].TypeIndex);
				builderDataPtr._absent.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0003E5EC File Offset: 0x0003C7EC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithPresent<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003E63C File Offset: 0x0003C83C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2, T3>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0003E764 File Offset: 0x0003C964
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2, T3, T4>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0003E83C File Offset: 0x0003CA3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2, T3, T4, T5>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0003E944 File Offset: 0x0003CB44
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2, T3, T4, T5, T6>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003EA78 File Offset: 0x0003CC78
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresent<T1, T2, T3, T4, T5, T6, T7>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr2._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr3 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T3>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr3._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr4 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T4>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr4._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr5 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T5>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr5._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr6 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T6>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr6._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr7 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T7>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			builderDataPtr7._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0003EBDC File Offset: 0x0003CDDC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithPresentRW<T1>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003EC2C File Offset: 0x0003CE2C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleComponentData),
			typeof(BurstCompatibleComponentData)
		})]
		public unsafe EntityQueryBuilder WithPresentRW<T1, T2>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T1>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr._present.Add(in componentType);
			ref EntityQueryBuilder.BuilderData builderDataPtr2 = ref *this._builderDataPtr;
			componentType = default(ComponentType);
			componentType.TypeIndex = TypeManager.GetTypeIndex<T2>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			builderDataPtr2._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003ECA8 File Offset: 0x0003CEA8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithPresentChunkComponent<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponentReadOnly<T>();
			builderDataPtr._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003ECE0 File Offset: 0x0003CEE0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe EntityQueryBuilder WithPresentChunkComponentRW<T>()
		{
			ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
			ComponentType componentType = ComponentType.ChunkComponent<T>();
			builderDataPtr._present.Add(in componentType);
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003ED18 File Offset: 0x0003CF18
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(FixedList32Bytes<ComponentType>) })]
		public unsafe EntityQueryBuilder WithPresent<T>(ref T componentTypes) where T : INativeList<ComponentType>
		{
			for (int i = 0; i < componentTypes.Length; i++)
			{
				ref EntityQueryBuilder.BuilderData builderDataPtr = ref *this._builderDataPtr;
				ComponentType componentType = componentTypes[i];
				builderDataPtr._present.Add(in componentType);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003ED70 File Offset: 0x0003CF70
		internal unsafe EntityQueryBuilder WithPresent(ComponentType* componentTypes, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this._builderDataPtr->_present.Add(in componentTypes[i]);
			}
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003EDB6 File Offset: 0x0003CFB6
		public EntityQueryBuilder AddAdditionalQuery()
		{
			return this.FinalizeQueryInternal();
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0003EDBE File Offset: 0x0003CFBE
		[Obsolete("It is no longer necessary to call FinalizeQuery on every EntityQueryBuilder. If you want to build an EntityQuery with multiple query descriptions, call AddAdditionalQuery before each subsequent query description. (RemovedAfter Entities 1.0)", true)]
		public EntityQueryBuilder FinalizeQuery()
		{
			return this;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003EDC8 File Offset: 0x0003CFC8
		internal unsafe EntityQueryBuilder FinalizeQueryInternal()
		{
			if (this._builderDataPtr->_isFinalized != 0)
			{
				return this;
			}
			EntityQueryBuilder.QueryTypes queryTypes = default(EntityQueryBuilder.QueryTypes);
			this.TransferArray(ref this._builderDataPtr->_all, ref queryTypes.All);
			this.TransferArray(ref this._builderDataPtr->_any, ref queryTypes.Any);
			this.TransferArray(ref this._builderDataPtr->_none, ref queryTypes.None);
			this.TransferArray(ref this._builderDataPtr->_disabled, ref queryTypes.Disabled);
			this.TransferArray(ref this._builderDataPtr->_absent, ref queryTypes.Absent);
			this.TransferArray(ref this._builderDataPtr->_present, ref queryTypes.Present);
			queryTypes.Options = this._builderDataPtr->_pendingOptions;
			this._builderDataPtr->_indexData.Add(in queryTypes);
			this._builderDataPtr->_pendingOptions = EntityQueryOptions.Default;
			this._builderDataPtr->_isFinalized = 1;
			return this;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0003EEC4 File Offset: 0x0003D0C4
		private unsafe void TransferArray(ref UnsafeList<ComponentType> source, ref EntityQueryBuilder.ComponentIndexArray result)
		{
			result.Index = (ushort)this._builderDataPtr->_typeData.Length;
			result.Count = (ushort)source.Length;
			this._builderDataPtr->_typeData.AddRange(source);
			source.Clear();
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0003EF14 File Offset: 0x0003D114
		public unsafe void Dispose()
		{
			this._builderDataPtr->_typeData.Dispose();
			this._builderDataPtr->_indexData.Dispose();
			this._builderDataPtr->_all.Dispose();
			this._builderDataPtr->_any.Dispose();
			this._builderDataPtr->_none.Dispose();
			this._builderDataPtr->_disabled.Dispose();
			this._builderDataPtr->_absent.Dispose();
			this._builderDataPtr->_present.Dispose();
			if (CollectionHelper.ShouldDeallocate(this._allocator))
			{
				AllocatorManager.Free<EntityQueryBuilder.BuilderData>(this._allocator, this._builderDataPtr, 1);
				this._allocator = AllocatorManager.Invalid;
			}
			this._builderDataPtr = null;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0003EFD4 File Offset: 0x0003D1D4
		public unsafe EntityQueryBuilder Reset()
		{
			this._builderDataPtr->_typeData.Clear();
			this._builderDataPtr->_indexData.Clear();
			this._builderDataPtr->_all.Clear();
			this._builderDataPtr->_any.Clear();
			this._builderDataPtr->_none.Clear();
			this._builderDataPtr->_disabled.Clear();
			this._builderDataPtr->_absent.Clear();
			this._builderDataPtr->_present.Clear();
			this._builderDataPtr->_pendingOptions = EntityQueryOptions.Default;
			this._builderDataPtr->_isFinalized = 0;
			return this;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0003F07F File Offset: 0x0003D27F
		public EntityQuery Build(ref SystemState systemState)
		{
			return systemState.GetEntityQuery(in this);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0003F088 File Offset: 0x0003D288
		[ExcludeFromBurstCompatTesting("SystemBase is a managed class")]
		public EntityQuery Build(SystemBase systemBase)
		{
			return systemBase.GetEntityQuery(in this);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003F091 File Offset: 0x0003D291
		public EntityQuery Build(EntityManager entityManager)
		{
			return entityManager.CreateEntityQuery(in this);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0003F09C File Offset: 0x0003D29C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void ValidateComponentTypes(in UnsafeList<ComponentType> componentTypes, ref UnsafeList<TypeIndex> allTypeIds)
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<Entity>();
			for (int i = 0; i < componentTypes.Length; i++)
			{
				UnsafeList<ComponentType> unsafeList = componentTypes;
				ComponentType componentType = unsafeList[i];
				allTypeIds.Add(in componentType.TypeIndex);
				if (componentType.AccessModeType == ComponentType.AccessMode.Exclude)
				{
					throw new ArgumentException("EntityQueryDesc cannot contain Exclude Component types");
				}
				if (componentType.TypeIndex == typeIndex)
				{
					throw new ArgumentException("Entity is not allowed in list of component types for EntityQuery");
				}
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003F10C File Offset: 0x0003D30C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[BurstDiscard]
		internal static void ThrowDuplicateComponentTypeError(TypeIndex curId)
		{
			string name = TypeManager.GetType(curId).Name;
			throw new EntityQueryDescValidationException("EntityQuery contains a filter with duplicate component type name " + name + ".  Queries can only contain a single component of a given type in a filter.");
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003F13C File Offset: 0x0003D33C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void Validate(in UnsafeList<ComponentType> allTypes, in UnsafeList<ComponentType> anyTypes, in UnsafeList<ComponentType> noneTypes, in UnsafeList<ComponentType> disabledTypes, in UnsafeList<ComponentType> absentTypes, in UnsafeList<ComponentType> presentTypes)
		{
			int num = allTypes.Length + anyTypes.Length + noneTypes.Length + disabledTypes.Length + absentTypes.Length + presentTypes.Length;
			UnsafeList<TypeIndex> unsafeList = new UnsafeList<TypeIndex>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			if (num > 1)
			{
				unsafeList.Sort<TypeIndex>();
				TypeIndex typeIndex = unsafeList[0];
				for (int i = 1; i < unsafeList.Length; i++)
				{
					TypeIndex typeIndex2 = unsafeList[i];
					if (typeIndex2 == typeIndex)
					{
						throw new EntityQueryDescValidationException(string.Format("EntityQuery contains an EntityQueryDesc with duplicate component type index {0}.  Queries can only contain a single component of a given type in a EntityQueryDesc.", typeIndex2));
					}
					typeIndex = typeIndex2;
				}
			}
			unsafeList.Dispose();
		}

		// Token: 0x04000587 RID: 1415
		internal unsafe EntityQueryBuilder.BuilderData* _builderDataPtr;

		// Token: 0x04000588 RID: 1416
		private AllocatorManager.AllocatorHandle _allocator;

		// Token: 0x0200021D RID: 541
		internal struct ComponentIndexArray
		{
			// Token: 0x04000589 RID: 1417
			public ushort Index;

			// Token: 0x0400058A RID: 1418
			public ushort Count;
		}

		// Token: 0x0200021E RID: 542
		internal struct QueryTypes
		{
			// Token: 0x0400058B RID: 1419
			public EntityQueryBuilder.ComponentIndexArray All;

			// Token: 0x0400058C RID: 1420
			public EntityQueryBuilder.ComponentIndexArray Any;

			// Token: 0x0400058D RID: 1421
			public EntityQueryBuilder.ComponentIndexArray None;

			// Token: 0x0400058E RID: 1422
			public EntityQueryBuilder.ComponentIndexArray Disabled;

			// Token: 0x0400058F RID: 1423
			public EntityQueryBuilder.ComponentIndexArray Absent;

			// Token: 0x04000590 RID: 1424
			public EntityQueryBuilder.ComponentIndexArray Present;

			// Token: 0x04000591 RID: 1425
			public EntityQueryOptions Options;
		}

		// Token: 0x0200021F RID: 543
		internal struct BuilderData
		{
			// Token: 0x04000592 RID: 1426
			internal UnsafeList<ComponentType> _typeData;

			// Token: 0x04000593 RID: 1427
			internal UnsafeList<EntityQueryBuilder.QueryTypes> _indexData;

			// Token: 0x04000594 RID: 1428
			internal UnsafeList<ComponentType> _all;

			// Token: 0x04000595 RID: 1429
			internal UnsafeList<ComponentType> _any;

			// Token: 0x04000596 RID: 1430
			internal UnsafeList<ComponentType> _none;

			// Token: 0x04000597 RID: 1431
			internal UnsafeList<ComponentType> _disabled;

			// Token: 0x04000598 RID: 1432
			internal UnsafeList<ComponentType> _absent;

			// Token: 0x04000599 RID: 1433
			internal UnsafeList<ComponentType> _present;

			// Token: 0x0400059A RID: 1434
			internal EntityQueryOptions _pendingOptions;

			// Token: 0x0400059B RID: 1435
			internal byte _isFinalized;
		}
	}
}
