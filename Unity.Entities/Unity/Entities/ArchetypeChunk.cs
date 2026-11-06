using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001C4 RID: 452
	[DebuggerTypeProxy(typeof(ArchetypeChunkDebugView))]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct ArchetypeChunk : IEquatable<ArchetypeChunk>
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00032734 File Offset: 0x00030934
		public readonly int Count
		{
			get
			{
				ChunkIndex chunk = this.m_Chunk;
				return chunk.Count;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00032750 File Offset: 0x00030950
		[Obsolete("This property is always equal to Count, and will be removed in a future release.")]
		public readonly int ChunkEntityCount
		{
			get
			{
				ChunkIndex chunk = this.m_Chunk;
				return chunk.Count;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0003276B File Offset: 0x0003096B
		public unsafe readonly int Capacity
		{
			get
			{
				return this.m_EntityComponentStore->GetArchetype(this.m_Chunk)->ChunkCapacity;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00032783 File Offset: 0x00030983
		public readonly bool Full
		{
			get
			{
				return this.Count == this.Capacity;
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00032793 File Offset: 0x00030993
		internal unsafe ArchetypeChunk(ChunkIndex chunk, EntityComponentStore* entityComponentStore)
		{
			this.m_Chunk = chunk;
			this.m_EntityComponentStore = entityComponentStore;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000327A3 File Offset: 0x000309A3
		public static bool operator ==(ArchetypeChunk lhs, ArchetypeChunk rhs)
		{
			return lhs.m_Chunk == rhs.m_Chunk && lhs.m_EntityComponentStore == rhs.m_EntityComponentStore;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000327C8 File Offset: 0x000309C8
		public static bool operator !=(ArchetypeChunk lhs, ArchetypeChunk rhs)
		{
			return lhs.m_Chunk != rhs.m_Chunk || lhs.m_EntityComponentStore != rhs.m_EntityComponentStore;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000327F0 File Offset: 0x000309F0
		public override bool Equals(object compare)
		{
			return this == (ArchetypeChunk)compare;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00032803 File Offset: 0x00030A03
		public override readonly int GetHashCode()
		{
			return this.m_Chunk;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00032810 File Offset: 0x00030A10
		public unsafe readonly EntityArchetype Archetype
		{
			get
			{
				return new EntityArchetype
				{
					Archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk)
				};
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0003283E File Offset: 0x00030A3E
		public ulong SequenceNumber
		{
			get
			{
				return this.m_Chunk.SequenceNumber;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0003284C File Offset: 0x00030A4C
		public static ArchetypeChunk Null
		{
			get
			{
				return default(ArchetypeChunk);
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000327A3 File Offset: 0x000309A3
		public bool Equals(ArchetypeChunk archetypeChunk)
		{
			return this.m_Chunk == archetypeChunk.m_Chunk && this.m_EntityComponentStore == archetypeChunk.m_EntityComponentStore;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00032862 File Offset: 0x00030A62
		public unsafe readonly int NumSharedComponents()
		{
			return this.m_EntityComponentStore->GetArchetype(this.m_Chunk)->NumSharedComponents;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0003287A File Offset: 0x00030A7A
		public unsafe readonly bool Invalid()
		{
			return this.m_EntityComponentStore->GetArchetype(this.m_Chunk) == null;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00032894 File Offset: 0x00030A94
		public unsafe readonly NativeArray<Entity> GetNativeArray(EntityTypeHandle entityTypeHandle)
		{
			ref Archetype archetype = ref *this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkIndex chunk = this.m_Chunk;
			byte* buffer = chunk.Buffer;
			int count = this.Count;
			int num = *archetype.Offsets;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Entity>((void*)(buffer + num), count, Allocator.None);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000328D9 File Offset: 0x00030AD9
		public readonly bool DidChange<T>(ref ComponentTypeHandle<T> typeHandle, uint version) where T : IComponentData
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion<T>(ref typeHandle), version);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000328E8 File Offset: 0x00030AE8
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool DidChange<T>(ComponentTypeHandle<T> typeHandle, uint version) where T : IComponentData
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion<T>(ref typeHandle), version);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000328F8 File Offset: 0x00030AF8
		public readonly bool DidChange(ref DynamicComponentTypeHandle typeHandle, uint version)
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion(ref typeHandle), version);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00032907 File Offset: 0x00030B07
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool DidChange(DynamicComponentTypeHandle typeHandle, uint version)
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion(ref typeHandle), version);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00032917 File Offset: 0x00030B17
		public readonly bool DidChange<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, uint version) where T : struct, ValueType, IBufferElementData
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion<T>(ref bufferTypeHandle), version);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00032926 File Offset: 0x00030B26
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool DidChange<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle, uint version) where T : struct, ValueType, IBufferElementData
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion<T>(ref bufferTypeHandle), version);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00032936 File Offset: 0x00030B36
		public readonly bool DidChange<T>(SharedComponentTypeHandle<T> chunkSharedComponentData, uint version) where T : struct, ISharedComponentData
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion<T>(chunkSharedComponentData), version);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00032945 File Offset: 0x00030B45
		public readonly bool DidChange(ref DynamicSharedComponentTypeHandle typeHandle, uint version)
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion(ref typeHandle), version);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00032954 File Offset: 0x00030B54
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool DidChange(DynamicSharedComponentTypeHandle typeHandle, uint version)
		{
			return ChangeVersionUtility.DidChange(this.GetChangeVersion(ref typeHandle), version);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00032964 File Offset: 0x00030B64
		public unsafe readonly uint GetChangeVersion<T>(ref ComponentTypeHandle<T> typeHandle) where T : IComponentData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			short indexInArchetype = typeHandle.m_LookupCache.IndexInArchetype;
			if (Hint.Unlikely(indexInArchetype == -1))
			{
				return 0U;
			}
			ref Archetype archetype2 = ref *this.Archetype.Archetype;
			int num = (int)indexInArchetype;
			ChunkIndex chunk = this.m_Chunk;
			return archetype2.Chunks.GetChangeVersion(num, chunk.ListIndex);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x000329EA File Offset: 0x00030BEA
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly uint GetChangeVersion<T>(ComponentTypeHandle<T> typeHandle) where T : IComponentData
		{
			return this.GetChangeVersion<T>(ref typeHandle);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x000329F4 File Offset: 0x00030BF4
		public unsafe readonly uint GetChangeVersion(ref DynamicComponentTypeHandle typeHandle)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			int typeLookupCache = (int)typeHandle.m_TypeLookupCache;
			if (Hint.Unlikely(typeLookupCache == -1))
			{
				return 0U;
			}
			ref Archetype archetype = ref *this.Archetype.Archetype;
			int num = typeLookupCache;
			ChunkIndex chunk = this.m_Chunk;
			return archetype.Chunks.GetChangeVersion(num, chunk.ListIndex);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00032A5B File Offset: 0x00030C5B
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly uint GetChangeVersion(DynamicComponentTypeHandle typeHandle)
		{
			return this.GetChangeVersion(ref typeHandle);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00032A68 File Offset: 0x00030C68
		public unsafe readonly uint GetChangeVersion<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(bufferTypeHandle.m_LookupCache.Archetype != archetype))
			{
				bufferTypeHandle.m_LookupCache.Update(archetype, bufferTypeHandle.m_TypeIndex);
			}
			short indexInArchetype = bufferTypeHandle.m_LookupCache.IndexInArchetype;
			if (Hint.Unlikely(indexInArchetype == -1))
			{
				return 0U;
			}
			ref Archetype archetype2 = ref *this.Archetype.Archetype;
			int num = (int)indexInArchetype;
			ChunkIndex chunk = this.m_Chunk;
			return archetype2.Chunks.GetChangeVersion(num, chunk.ListIndex);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00032AEE File Offset: 0x00030CEE
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly uint GetChangeVersion<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			return this.GetChangeVersion<T>(ref bufferTypeHandle);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00032AF8 File Offset: 0x00030CF8
		public unsafe readonly uint GetChangeVersion<T>(SharedComponentTypeHandle<T> chunkSharedComponentData) where T : struct, ISharedComponentData
		{
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), chunkSharedComponentData.m_TypeIndex);
			if (Hint.Unlikely(indexInTypeArray == -1))
			{
				return 0U;
			}
			ref Archetype archetype = ref *this.Archetype.Archetype;
			int num = indexInTypeArray;
			ChunkIndex chunk = this.m_Chunk;
			return archetype.Chunks.GetChangeVersion(num, chunk.ListIndex);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00032B54 File Offset: 0x00030D54
		public unsafe readonly uint GetChangeVersion(ref DynamicSharedComponentTypeHandle typeHandle)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_cachedTypeIndexinArchetype);
			int cachedTypeIndexinArchetype = (int)typeHandle.m_cachedTypeIndexinArchetype;
			if (Hint.Unlikely(cachedTypeIndexinArchetype == -1))
			{
				return 0U;
			}
			ref Archetype archetype = ref *this.Archetype.Archetype;
			int num = cachedTypeIndexinArchetype;
			ChunkIndex chunk = this.m_Chunk;
			return archetype.Chunks.GetChangeVersion(num, chunk.ListIndex);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00032BBB File Offset: 0x00030DBB
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly uint GetChangeVersion(DynamicSharedComponentTypeHandle typeHandle)
		{
			return this.GetChangeVersion(ref typeHandle);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00032BC5 File Offset: 0x00030DC5
		public readonly bool DidOrderChange(uint version)
		{
			return ChangeVersionUtility.DidChange(this.GetOrderVersion(), version);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00032BD4 File Offset: 0x00030DD4
		public unsafe readonly uint GetOrderVersion()
		{
			ref Archetype archetype = ref *this.Archetype.Archetype;
			ChunkIndex chunk = this.m_Chunk;
			return archetype.Chunks.GetOrderVersion(chunk.ListIndex);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00032C04 File Offset: 0x00030E04
		public unsafe readonly bool IsComponentEnabled<T>(ref ComponentTypeHandle<T> typeHandle, int entityIndexInChunk) where T : IComponentData, IEnableableComponent, new()
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			short indexInArchetype = typeHandle.m_LookupCache.IndexInArchetype;
			return !Hint.Unlikely(indexInArchetype == -1) && this.m_EntityComponentStore->IsComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)indexInArchetype);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00032C79 File Offset: 0x00030E79
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool IsComponentEnabled<T>(ComponentTypeHandle<T> typeHandle, int entityIndexInChunk) where T : IComponentData, IEnableableComponent, new()
		{
			return this.IsComponentEnabled<T>(ref typeHandle, entityIndexInChunk);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00032C84 File Offset: 0x00030E84
		public unsafe readonly bool IsComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, int entityIndexInChunk) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(bufferTypeHandle.m_LookupCache.Archetype != archetype))
			{
				bufferTypeHandle.m_LookupCache.Update(archetype, bufferTypeHandle.m_TypeIndex);
			}
			short indexInArchetype = bufferTypeHandle.m_LookupCache.IndexInArchetype;
			return !Hint.Unlikely(indexInArchetype == -1) && this.m_EntityComponentStore->IsComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)indexInArchetype);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00032CF9 File Offset: 0x00030EF9
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool IsComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle, int entityIndexInChunk) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			return this.IsComponentEnabled<T>(ref bufferTypeHandle, entityIndexInChunk);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00032D04 File Offset: 0x00030F04
		public unsafe readonly bool IsComponentEnabled(ref DynamicComponentTypeHandle typeHandle, int entityIndexInChunk)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			short typeLookupCache = typeHandle.m_TypeLookupCache;
			return !Hint.Unlikely(typeLookupCache == -1) && this.m_EntityComponentStore->IsComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)typeLookupCache);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00032D5C File Offset: 0x00030F5C
		public unsafe readonly v128 GetEnableableBits(ref DynamicComponentTypeHandle handle)
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkDataUtility.GetIndexInTypeArray(archetype, handle.m_TypeIndex, ref handle.m_TypeLookupCache);
			if (handle.m_TypeLookupCache == -1)
			{
				return default(v128);
			}
			int typeLookupCache = (int)handle.m_TypeLookupCache;
			int num = archetype->TypeIndexInArchetypeToMemoryOrderIndex[typeLookupCache];
			if (handle.m_TypeLookupCache != -1)
			{
				ref Archetype ptr = ref *archetype;
				int num2 = num;
				ChunkIndex chunk = this.m_Chunk;
				return *ptr.Chunks.GetComponentEnabledMaskArrayForTypeInChunk(num2, chunk.ListIndex);
			}
			return default(v128);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00032DE8 File Offset: 0x00030FE8
		public unsafe readonly EnabledMask GetEnabledMask<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData, IEnableableComponent
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			if (Hint.Unlikely(typeHandle.m_LookupCache.IndexInArchetype == -1))
			{
				return new EnabledMask(SafeBitRef.Null, null);
			}
			int* ptr = null;
			ulong* ptr2 = (typeHandle.IsReadOnly ? ChunkDataUtility.GetEnabledRefRO(this.m_Chunk, this.Archetype.Archetype, (int)typeHandle.m_LookupCache.IndexInArchetype).Ptr : ChunkDataUtility.GetEnabledRefRW(this.m_Chunk, this.Archetype.Archetype, (int)typeHandle.m_LookupCache.IndexInArchetype, typeHandle.GlobalSystemVersion, out ptr).Ptr);
			return new EnabledMask(new SafeBitRef(ptr2, 0), ptr);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00032EC4 File Offset: 0x000310C4
		public unsafe readonly EnabledMask GetEnabledMask<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> typeHandle) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			if (Hint.Unlikely(typeHandle.m_LookupCache.IndexInArchetype == -1))
			{
				return new EnabledMask(SafeBitRef.Null, null);
			}
			int* ptr = default(int*);
			ulong* ptr2 = (typeHandle.IsReadOnly ? ChunkDataUtility.GetEnabledRefRO(this.m_Chunk, this.Archetype.Archetype, (int)typeHandle.m_LookupCache.IndexInArchetype).Ptr : ChunkDataUtility.GetEnabledRefRW(this.m_Chunk, this.Archetype.Archetype, (int)typeHandle.m_LookupCache.IndexInArchetype, typeHandle.GlobalSystemVersion, out ptr).Ptr);
			return new EnabledMask(new SafeBitRef(ptr2, 0), ptr);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00032FA4 File Offset: 0x000311A4
		public unsafe readonly EnabledMask GetEnabledMask(ref DynamicComponentTypeHandle typeHandle)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			short typeLookupCache = typeHandle.m_TypeLookupCache;
			if (Hint.Unlikely(typeLookupCache == -1))
			{
				return new EnabledMask(SafeBitRef.Null, null);
			}
			int* ptr = default(int*);
			ulong* ptr2 = (typeHandle.IsReadOnly ? ChunkDataUtility.GetEnabledRefRO(this.m_Chunk, this.Archetype.Archetype, (int)typeLookupCache).Ptr : ChunkDataUtility.GetEnabledRefRW(this.m_Chunk, this.Archetype.Archetype, (int)typeLookupCache, typeHandle.GlobalSystemVersion, out ptr).Ptr);
			return new EnabledMask(new SafeBitRef(ptr2, 0), ptr);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00033052 File Offset: 0x00031252
		private unsafe readonly short GetRequiredTypeIndexInArchetype<T>(ref ComponentTypeHandle<T> typeHandle, Archetype* archetype) where T : IComponentData, new()
		{
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			return typeHandle.m_LookupCache.IndexInArchetype;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00033089 File Offset: 0x00031289
		private unsafe readonly short GetRequiredTypeIndexInArchetype<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, Archetype* archetype) where T : struct, ValueType, IBufferElementData
		{
			if (Hint.Unlikely(bufferTypeHandle.m_LookupCache.Archetype != archetype))
			{
				bufferTypeHandle.m_LookupCache.Update(archetype, bufferTypeHandle.m_TypeIndex);
			}
			return bufferTypeHandle.m_LookupCache.IndexInArchetype;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000330C0 File Offset: 0x000312C0
		private unsafe readonly short GetRequiredTypeIndexInArchetype(ref DynamicComponentTypeHandle typeHandle, Archetype* archetype)
		{
			ChunkDataUtility.GetIndexInTypeArray(archetype, typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			return typeHandle.m_TypeLookupCache;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x000330DC File Offset: 0x000312DC
		private unsafe readonly void SetComponentEnabledForAllInChunk(Archetype* archetype, short typeIndexInArchetype, bool value)
		{
			int* ptr;
			ChunkDataUtility.GetEnabledRefRW(this.m_Chunk, archetype, (int)typeIndexInArchetype, this.m_EntityComponentStore->GlobalSystemVersion, out ptr).SetBits(0, value, this.Count);
			*ptr = (value ? 0 : this.Count);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00033124 File Offset: 0x00031324
		public unsafe readonly void SetComponentEnabledForAll<T>(ref ComponentTypeHandle<T> typeHandle, bool value) where T : IComponentData, IEnableableComponent, new()
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype<T>(ref typeHandle, archetype);
			this.SetComponentEnabledForAllInChunk(archetype, requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00033150 File Offset: 0x00031350
		public unsafe readonly void SetComponentEnabledForAll<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, bool value) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype<T>(ref bufferTypeHandle, archetype);
			this.SetComponentEnabledForAllInChunk(archetype, requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0003317C File Offset: 0x0003137C
		public unsafe readonly void SetComponentEnabledForAll(ref DynamicComponentTypeHandle typeHandle, bool value)
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype(ref typeHandle, archetype);
			this.SetComponentEnabledForAllInChunk(archetype, requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000331A8 File Offset: 0x000313A8
		public unsafe readonly void SetComponentEnabled<T>(ref ComponentTypeHandle<T> typeHandle, int entityIndexInChunk, bool value) where T : IComponentData, IEnableableComponent, new()
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype<T>(ref typeHandle, archetype);
			this.m_EntityComponentStore->SetComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x000331DE File Offset: 0x000313DE
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly void SetComponentEnabled<T>(ComponentTypeHandle<T> typeHandle, int entityIndexInChunk, bool value) where T : IComponentData, IEnableableComponent, new()
		{
			this.SetComponentEnabled<T>(ref typeHandle, entityIndexInChunk, value);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000331EC File Offset: 0x000313EC
		public unsafe readonly void SetComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, int entityIndexInChunk, bool value) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype<T>(ref bufferTypeHandle, archetype);
			this.m_EntityComponentStore->SetComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00033222 File Offset: 0x00031422
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly void SetComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle, int entityIndexInChunk, bool value) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			this.SetComponentEnabled<T>(ref bufferTypeHandle, entityIndexInChunk, value);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00033230 File Offset: 0x00031430
		public unsafe readonly void SetComponentEnabled(ref DynamicComponentTypeHandle typeHandle, int entityIndexInChunk, bool value)
		{
			Archetype* archetype = this.Archetype.Archetype;
			short requiredTypeIndexInArchetype = this.GetRequiredTypeIndexInArchetype(ref typeHandle, archetype);
			this.m_EntityComponentStore->SetComponentEnabled(this.m_Chunk, entityIndexInChunk, (int)requiredTypeIndexInArchetype, value);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00033268 File Offset: 0x00031468
		public unsafe readonly T GetChunkComponentData<T>(ref ComponentTypeHandle<T> typeHandle) where T : struct
		{
			ChunkIndex chunk = this.m_Chunk;
			Entity metaChunkEntity = chunk.MetaChunkEntity;
			T t;
			UnsafeUtility.CopyPtrToStructure<T>((void*)this.m_EntityComponentStore->GetComponentDataWithTypeRO(metaChunkEntity, typeHandle.m_TypeIndex), out t);
			return t;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0003329E File Offset: 0x0003149E
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly T GetChunkComponentData<T>(ComponentTypeHandle<T> typeHandle) where T : struct
		{
			return this.GetChunkComponentData<T>(ref typeHandle);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000332A8 File Offset: 0x000314A8
		public unsafe readonly void SetChunkComponentData<T>(ref ComponentTypeHandle<T> typeHandle, T value) where T : struct
		{
			ChunkIndex chunk = this.m_Chunk;
			Entity metaChunkEntity = chunk.MetaChunkEntity;
			byte* componentDataWithTypeRW = this.m_EntityComponentStore->GetComponentDataWithTypeRW(metaChunkEntity, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion);
			UnsafeUtility.CopyStructureToPtr<T>(ref value, (void*)componentDataWithTypeRW);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000332E5 File Offset: 0x000314E5
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly void SetChunkComponentData<T>(ComponentTypeHandle<T> typeHandle, T value) where T : struct
		{
			this.SetChunkComponentData<T>(ref typeHandle, value);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x000332F0 File Offset: 0x000314F0
		public unsafe readonly int GetSharedComponentIndex<T>(SharedComponentTypeHandle<T> chunkSharedComponentData) where T : struct, ISharedComponentData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, chunkSharedComponentData.m_TypeIndex);
			if (Hint.Unlikely(indexInTypeArray == -1))
			{
				return -1;
			}
			int num = indexInTypeArray - (int)archetype->FirstSharedComponent;
			ref Archetype ptr = ref *archetype;
			int num2 = num;
			ChunkIndex chunk = this.m_Chunk;
			return ptr.Chunks.GetSharedComponentValue(num2, chunk.ListIndex);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0003334C File Offset: 0x0003154C
		public unsafe readonly int GetSharedComponentIndex(ref DynamicSharedComponentTypeHandle typeHandle)
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkDataUtility.GetIndexInTypeArray(archetype, typeHandle.m_TypeIndex, ref typeHandle.m_cachedTypeIndexinArchetype);
			short cachedTypeIndexinArchetype = typeHandle.m_cachedTypeIndexinArchetype;
			if (Hint.Unlikely(cachedTypeIndexinArchetype == -1))
			{
				return -1;
			}
			int num = (int)(cachedTypeIndexinArchetype - archetype->FirstSharedComponent);
			ref Archetype ptr = ref *archetype;
			int num2 = num;
			ChunkIndex chunk = this.m_Chunk;
			return ptr.Chunks.GetSharedComponentValue(num2, chunk.ListIndex);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000333B4 File Offset: 0x000315B4
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly int GetSharedComponentIndex(DynamicSharedComponentTypeHandle typeHandle)
		{
			return this.GetSharedComponentIndex(ref typeHandle);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000333C0 File Offset: 0x000315C0
		[Obsolete("Use GetSharedComponentManaged (UnityUpgradable) -> GetSharedComponentManaged<T>(*)", true)]
		public T GetSharedComponentData<T>(SharedComponentTypeHandle<T> chunkSharedComponentData, EntityManager entityManager) where T : struct, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000333D6 File Offset: 0x000315D6
		public readonly T GetSharedComponentManaged<T>(SharedComponentTypeHandle<T> chunkSharedComponentData, EntityManager entityManager) where T : struct, ISharedComponentData
		{
			return entityManager.GetSharedComponentManaged<T>(this.GetSharedComponentIndex<T>(chunkSharedComponentData));
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000333E8 File Offset: 0x000315E8
		[Obsolete("Use GetSharedComponent (UnityUpgradable) -> GetSharedComponent<T>(*)", true)]
		public T GetSharedComponentDataUnmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SharedComponentTypeHandle<T> chunkSharedComponentData) where T : struct, ValueType, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000333FE File Offset: 0x000315FE
		public readonly T GetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SharedComponentTypeHandle<T> chunkSharedComponentData, EntityManager entityManager) where T : struct, ValueType, ISharedComponentData
		{
			return entityManager.GetSharedComponent<T>(this.GetSharedComponentIndex<T>(chunkSharedComponentData));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00033410 File Offset: 0x00031610
		public unsafe readonly T GetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SharedComponentTypeHandle<T> chunkSharedComponentData) where T : struct, ValueType, ISharedComponentData
		{
			int sharedComponentIndex = this.GetSharedComponentIndex<T>(chunkSharedComponentData);
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			this.m_EntityComponentStore->GetSharedComponentData_Unmanaged(sharedComponentIndex, typeIndex, UnsafeUtility.AddressOf<T>(ref t));
			return t;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00033448 File Offset: 0x00031648
		public unsafe readonly void* GetDynamicSharedComponentDataAddress(ref DynamicSharedComponentTypeHandle typeHandle)
		{
			int sharedComponentIndex = this.GetSharedComponentIndex(ref typeHandle);
			TypeIndex typeIndex = typeHandle.m_TypeIndex;
			return this.m_EntityComponentStore->GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, typeIndex);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00033471 File Offset: 0x00031671
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public unsafe readonly void* GetDynamicSharedComponentDataAddress(DynamicSharedComponentTypeHandle typeHandle)
		{
			return this.GetDynamicSharedComponentDataAddress(ref typeHandle);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0003347B File Offset: 0x0003167B
		public readonly object GetSharedComponentDataBoxed(ref DynamicSharedComponentTypeHandle typeHandle, EntityManager entityManager)
		{
			return entityManager.GetSharedComponentDataBoxed(this.GetSharedComponentIndex(ref typeHandle), typeHandle.m_TypeIndex);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00033491 File Offset: 0x00031691
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly object GetSharedComponentDataBoxed(DynamicSharedComponentTypeHandle typeHandle, EntityManager entityManager)
		{
			return this.GetSharedComponentDataBoxed(ref typeHandle, entityManager);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0003349C File Offset: 0x0003169C
		public unsafe readonly bool Has<T>(ref ComponentTypeHandle<T> typeHandle) where T : IComponentData, new()
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(typeHandle.m_LookupCache.Archetype != archetype))
			{
				typeHandle.m_LookupCache.Update(archetype, typeHandle.m_TypeIndex);
			}
			return typeHandle.m_LookupCache.IndexInArchetype != -1;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000334F6 File Offset: 0x000316F6
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool Has<T>(ComponentTypeHandle<T> typeHandle) where T : IComponentData, new()
		{
			return this.Has<T>(ref typeHandle);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00033500 File Offset: 0x00031700
		public unsafe readonly bool Has<T>()
		{
			return ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), TypeManager.GetTypeIndex<T>()) != -1;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00033523 File Offset: 0x00031723
		public unsafe readonly bool Has(ref DynamicComponentTypeHandle typeHandle)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			return typeHandle.m_TypeLookupCache != -1;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00033553 File Offset: 0x00031753
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool Has(DynamicComponentTypeHandle typeHandle)
		{
			return this.Has(ref typeHandle);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00033560 File Offset: 0x00031760
		public unsafe readonly bool HasChunkComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			Archetype* metaChunkArchetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk)->MetaChunkArchetype;
			return !Hint.Unlikely(metaChunkArchetype == null) && ChunkDataUtility.GetIndexInTypeArray(metaChunkArchetype, typeHandle.m_TypeIndex) != -1;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000335A4 File Offset: 0x000317A4
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool HasChunkComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return this.HasChunkComponent<T>(ref typeHandle);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000335B0 File Offset: 0x000317B0
		public unsafe readonly bool HasChunkComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			Archetype* metaChunkArchetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk)->MetaChunkArchetype;
			return !Hint.Unlikely(metaChunkArchetype == null) && ChunkDataUtility.GetIndexInTypeArray(metaChunkArchetype, TypeManager.GetTypeIndex<T>()) != -1;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x000335F3 File Offset: 0x000317F3
		public unsafe readonly bool Has<T>(SharedComponentTypeHandle<T> typeHandle) where T : struct, ISharedComponentData
		{
			return ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex) != -1;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00033617 File Offset: 0x00031817
		public unsafe readonly bool Has(ref DynamicSharedComponentTypeHandle typeHandle)
		{
			ChunkDataUtility.GetIndexInTypeArray(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeHandle.m_TypeIndex, ref typeHandle.m_cachedTypeIndexinArchetype);
			return typeHandle.m_cachedTypeIndexinArchetype != -1;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00033647 File Offset: 0x00031847
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool Has(DynamicSharedComponentTypeHandle typeHandle)
		{
			return this.Has(ref typeHandle);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00033654 File Offset: 0x00031854
		public unsafe readonly bool Has<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			if (Hint.Unlikely(bufferTypeHandle.m_LookupCache.Archetype != archetype))
			{
				bufferTypeHandle.m_LookupCache.Update(archetype, bufferTypeHandle.m_TypeIndex);
			}
			return bufferTypeHandle.m_LookupCache.IndexInArchetype != -1;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000336AE File Offset: 0x000318AE
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly bool Has<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			return this.Has<T>(ref bufferTypeHandle);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000336B8 File Offset: 0x000318B8
		public unsafe readonly NativeArray<T> GetNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			byte* ptr = (typeHandle.IsReadOnly ? ChunkDataUtility.GetOptionalComponentDataWithTypeRO(this.m_Chunk, archetype, 0, typeHandle.m_TypeIndex, ref typeHandle.m_LookupCache) : ChunkDataUtility.GetOptionalComponentDataWithTypeRW(this.m_Chunk, archetype, 0, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion, ref typeHandle.m_LookupCache));
			if (Hint.Unlikely(ptr == null))
			{
				return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(null, 0, Allocator.Invalid);
			}
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)ptr, this.Count, Allocator.None);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0003373D File Offset: 0x0003193D
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly NativeArray<T> GetNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return this.GetNativeArray<T>(ref typeHandle);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00033748 File Offset: 0x00031948
		public unsafe readonly Entity* GetEntityDataPtrRO(EntityTypeHandle entityTypeHandle)
		{
			ref Archetype archetype = ref *this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkIndex chunk = this.m_Chunk;
			byte* buffer = chunk.Buffer;
			int num = *archetype.Offsets;
			return (Entity*)(buffer + num);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00033780 File Offset: 0x00031980
		public unsafe readonly T* GetComponentDataPtrRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (T*)ChunkDataUtility.GetOptionalComponentDataWithTypeRO(this.m_Chunk, this.m_EntityComponentStore->GetArchetype(this.m_Chunk), 0, typeHandle.m_TypeIndex, ref typeHandle.m_LookupCache);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000337B8 File Offset: 0x000319B8
		public unsafe readonly T* GetComponentDataPtrRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (T*)ChunkDataUtility.GetOptionalComponentDataWithTypeRW(this.m_Chunk, this.m_EntityComponentStore->GetArchetype(this.m_Chunk), 0, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion, ref typeHandle.m_LookupCache);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000337E9 File Offset: 0x000319E9
		public unsafe readonly void* GetRequiredComponentDataPtrRO<T>(ref ComponentTypeHandle<T> typeHandle)
		{
			return (void*)ChunkDataUtility.GetComponentDataWithTypeRO(this.m_Chunk, this.m_EntityComponentStore->GetArchetype(this.m_Chunk), 0, typeHandle.m_TypeIndex, ref typeHandle.m_LookupCache);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00033814 File Offset: 0x00031A14
		public unsafe readonly void* GetRequiredComponentDataPtrRW<T>(ref ComponentTypeHandle<T> typeHandle)
		{
			return (void*)ChunkDataUtility.GetComponentDataWithTypeRW(this.m_Chunk, this.m_EntityComponentStore->GetArchetype(this.m_Chunk), 0, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion, ref typeHandle.m_LookupCache);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00033848 File Offset: 0x00031A48
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckZeroSizedGetDynamicComponentDataArrayReinterpret(in DynamicComponentTypeHandle typeHandle)
		{
			if (Hint.Unlikely(typeHandle.IsZeroSized))
			{
				FixedString128Bytes fixedString128Bytes = typeHandle.m_TypeIndex.ToFixedString();
				throw new ArgumentException(string.Format("ArchetypeChunk.GetDynamicComponentDataArrayReinterpret<{0}> cannot be called on zero-sized IComponentData", fixedString128Bytes));
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00033888 File Offset: 0x00031A88
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckComponentSizeMatches(in DynamicComponentTypeHandle typeHandle, int typeSize, int expectedTypeSize)
		{
			if (Hint.Unlikely(typeSize != expectedTypeSize))
			{
				FixedString128Bytes fixedString128Bytes = typeHandle.m_TypeIndex.ToFixedString();
				throw new InvalidOperationException(string.Format("Dynamic chunk component type {0} (size = {1}) size does not equal {2}. Component size must match with expectedTypeSize.", fixedString128Bytes, typeSize, expectedTypeSize));
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000338D4 File Offset: 0x00031AD4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCannotBeAliasedDueToSizeConstraints<T>(in DynamicComponentTypeHandle typeHandle, int outTypeSize, int outLength, int byteLen, int length)
		{
			if (Hint.Unlikely(outTypeSize * outLength != byteLen))
			{
				typeHandle.m_TypeIndex.ToFixedString();
				throw new InvalidOperationException(string.Format("Dynamic chunk component type {0} (array length {1}) and {2} cannot be aliased due to size constraints. The size of the types and lengths involved must line up.", TypeManager.GetType(typeHandle.m_TypeIndex), length, typeof(T)));
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003392C File Offset: 0x00031B2C
		public unsafe readonly NativeArray<T> GetDynamicComponentDataArrayReinterpret<T>(ref DynamicComponentTypeHandle typeHandle, int expectedTypeSize) where T : struct
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkDataUtility.GetIndexInTypeArray(archetype, typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			short typeLookupCache = typeHandle.m_TypeLookupCache;
			if (Hint.Unlikely(typeLookupCache == -1))
			{
				return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(null, 0, Allocator.Invalid);
			}
			ushort num = archetype->SizeOfs[typeLookupCache];
			int num2 = this.Count * (int)num;
			int num3 = UnsafeUtility.SizeOf<T>();
			int num4 = num2 / num3;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)(typeHandle.IsReadOnly ? ChunkDataUtility.GetComponentDataRO(this.m_Chunk, archetype, 0, (int)typeLookupCache) : ChunkDataUtility.GetComponentDataRW(this.m_Chunk, archetype, 0, (int)typeLookupCache, typeHandle.GlobalSystemVersion)), num4, Allocator.None);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000339CC File Offset: 0x00031BCC
		internal unsafe readonly void* GetDynamicComponentDataPtr(ref DynamicComponentTypeHandle typeHandle)
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			ChunkDataUtility.GetIndexInTypeArray(archetype, typeHandle.m_TypeIndex, ref typeHandle.m_TypeLookupCache);
			short typeLookupCache = typeHandle.m_TypeLookupCache;
			if (Hint.Unlikely(typeLookupCache == -1))
			{
				return null;
			}
			if (!typeHandle.IsReadOnly)
			{
				return (void*)ChunkDataUtility.GetComponentDataRW(this.m_Chunk, archetype, 0, (int)typeLookupCache, typeHandle.GlobalSystemVersion);
			}
			return (void*)ChunkDataUtility.GetComponentDataRO(this.m_Chunk, archetype, 0, (int)typeLookupCache);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00033A3D File Offset: 0x00031C3D
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly NativeArray<T> GetDynamicComponentDataArrayReinterpret<T>(DynamicComponentTypeHandle typeHandle, int expectedTypeSize) where T : struct
		{
			return this.GetDynamicComponentDataArrayReinterpret<T>(ref typeHandle, expectedTypeSize);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00033A48 File Offset: 0x00031C48
		public unsafe readonly ManagedComponentAccessor<T> GetManagedComponentAccessor<T>(ref ComponentTypeHandle<T> typeHandle, EntityManager manager) where T : class
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			byte* optionalComponentDataWithTypeRW = ChunkDataUtility.GetOptionalComponentDataWithTypeRW(this.m_Chunk, archetype, 0, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion, ref typeHandle.m_LookupCache);
			NativeArray<int> nativeArray;
			if (Hint.Unlikely(optionalComponentDataWithTypeRW == null))
			{
				nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(null, 0, Allocator.Invalid);
			}
			else
			{
				int count = this.Count;
				nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)optionalComponentDataWithTypeRW, count, Allocator.None);
			}
			return new ManagedComponentAccessor<T>(nativeArray, manager);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00033AB6 File Offset: 0x00031CB6
		[Obsolete("The typeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly ManagedComponentAccessor<T> GetManagedComponentAccessor<T>(ComponentTypeHandle<T> typeHandle, EntityManager manager) where T : class
		{
			return this.GetManagedComponentAccessor<T>(ref typeHandle, manager);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00033AC4 File Offset: 0x00031CC4
		public unsafe readonly BufferAccessor<T> GetBufferAccessor<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			TypeIndex typeIndex = bufferTypeHandle.m_TypeIndex;
			if (Hint.Unlikely(bufferTypeHandle.m_LookupCache.Archetype != archetype))
			{
				bufferTypeHandle.m_LookupCache.Update(this.m_EntityComponentStore->GetArchetype(this.m_Chunk), typeIndex);
			}
			byte* ptr = (bufferTypeHandle.IsReadOnly ? ChunkDataUtility.GetOptionalComponentDataWithTypeRO(this.m_Chunk, archetype, 0, typeIndex, ref bufferTypeHandle.m_LookupCache) : ChunkDataUtility.GetOptionalComponentDataWithTypeRW(this.m_Chunk, archetype, 0, typeIndex, bufferTypeHandle.GlobalSystemVersion, ref bufferTypeHandle.m_LookupCache));
			if (Hint.Unlikely(ptr == null))
			{
				return new BufferAccessor<T>(null, 0, 0, 0);
			}
			int indexInArchetype = (int)bufferTypeHandle.m_LookupCache.IndexInArchetype;
			int num = archetype->BufferCapacities[indexInArchetype];
			int count = this.Count;
			int componentSizeOf = (int)bufferTypeHandle.m_LookupCache.ComponentSizeOf;
			return new BufferAccessor<T>(ptr, count, componentSizeOf, num);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00033BAA File Offset: 0x00031DAA
		[Obsolete("The bufferTypeHandle argument should now be passed by ref. (RemovedAfter Entities 1.0)", false)]
		public readonly BufferAccessor<T> GetBufferAccessor<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BufferTypeHandle<T> bufferTypeHandle) where T : struct, ValueType, IBufferElementData
		{
			return this.GetBufferAccessor<T>(ref bufferTypeHandle);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00033BB4 File Offset: 0x00031DB4
		public unsafe readonly UnsafeUntypedBufferAccessor GetUntypedBufferAccessor(ref DynamicComponentTypeHandle chunkBufferTypeHandle)
		{
			Archetype* archetype = this.m_EntityComponentStore->GetArchetype(this.m_Chunk);
			short typeLookupCache = chunkBufferTypeHandle.m_TypeLookupCache;
			ChunkDataUtility.GetIndexInTypeArray(archetype, chunkBufferTypeHandle.m_TypeIndex, ref typeLookupCache);
			chunkBufferTypeHandle.m_TypeLookupCache = typeLookupCache;
			if (Hint.Unlikely(typeLookupCache == -1))
			{
				return default(UnsafeUntypedBufferAccessor);
			}
			int num = archetype->BufferCapacities[typeLookupCache];
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(chunkBufferTypeHandle.m_TypeIndex);
			byte* ptr = (chunkBufferTypeHandle.IsReadOnly ? ChunkDataUtility.GetComponentDataRO(this.m_Chunk, archetype, 0, (int)typeLookupCache) : ChunkDataUtility.GetComponentDataRW(this.m_Chunk, archetype, 0, (int)typeLookupCache, chunkBufferTypeHandle.GlobalSystemVersion));
			int count = this.Count;
			int num2 = (int)archetype->SizeOfs[typeLookupCache];
			int elementSize = typeInfo.ElementSize;
			int alignmentInBytes = typeInfo.AlignmentInBytes;
			return new UnsafeUntypedBufferAccessor(ptr, count, num2, elementSize, alignmentInBytes, num);
		}

		// Token: 0x040004CD RID: 1229
		[NativeDisableUnsafePtrRestriction]
		[FieldOffset(0)]
		internal ChunkIndex m_Chunk;

		// Token: 0x040004CE RID: 1230
		[NativeDisableUnsafePtrRestriction]
		[FieldOffset(8)]
		internal unsafe EntityComponentStore* m_EntityComponentStore;
	}
}
