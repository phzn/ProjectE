using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002DE RID: 734
	internal struct Archetype
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x0004CAF0 File Offset: 0x0004ACF0
		public bool CleanupComplete
		{
			get
			{
				return (this.Flags & ArchetypeFlags.CleanupComplete) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0004CAFD File Offset: 0x0004ACFD
		public bool CleanupNeeded
		{
			get
			{
				return (this.Flags & ArchetypeFlags.CleanupNeeded) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x0004CB0A File Offset: 0x0004AD0A
		public bool Disabled
		{
			get
			{
				return (this.Flags & ArchetypeFlags.Disabled) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0004CB17 File Offset: 0x0004AD17
		public bool Prefab
		{
			get
			{
				return (this.Flags & ArchetypeFlags.Prefab) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x0004CB24 File Offset: 0x0004AD24
		public bool HasChunkHeader
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasChunkHeader) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0004CB32 File Offset: 0x0004AD32
		public bool HasBlobAssetRefs
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasBlobAssetRefs) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0004CB40 File Offset: 0x0004AD40
		public bool HasManagedEntityRefs
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasManagedEntityRefs) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0004CB51 File Offset: 0x0004AD51
		public bool HasCompanionComponents
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasCompanionComponents) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0004CB5F File Offset: 0x0004AD5F
		public bool HasWeakAssetRefs
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasWeakAssetRefs) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0004CB70 File Offset: 0x0004AD70
		public bool HasUnityObjectRefs
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasUnityObjectRefs) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x0004CB81 File Offset: 0x0004AD81
		public bool HasSystemInstanceComponents
		{
			get
			{
				return (this.Flags & ArchetypeFlags.HasSystemInstanceComponents) > (ArchetypeFlags)0;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0004CB92 File Offset: 0x0004AD92
		public int NumNativeComponentData
		{
			get
			{
				return (int)(this.FirstBufferComponent - 1);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0004CB9C File Offset: 0x0004AD9C
		public int NumBufferComponents
		{
			get
			{
				return (int)(this.FirstManagedComponent - this.FirstBufferComponent);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0004CBAB File Offset: 0x0004ADAB
		public int NumManagedComponents
		{
			get
			{
				return (int)(this.FirstTagComponent - this.FirstManagedComponent);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0004CBBA File Offset: 0x0004ADBA
		public int NumTagComponents
		{
			get
			{
				return (int)(this.FirstSharedComponent - this.FirstTagComponent);
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0004CBC9 File Offset: 0x0004ADC9
		public int NumSharedComponents
		{
			get
			{
				return (int)(this.FirstChunkComponent - this.FirstSharedComponent);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0004CBD8 File Offset: 0x0004ADD8
		public int NumChunkComponents
		{
			get
			{
				return this.TypesCount - (int)this.FirstChunkComponent;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0004CBE7 File Offset: 0x0004ADE7
		public int NonZeroSizedTypesCount
		{
			get
			{
				return (int)this.FirstTagComponent;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0004CBEF File Offset: 0x0004ADEF
		public int NativeComponentsEnd
		{
			get
			{
				return (int)this.FirstBufferComponent;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0004CBF7 File Offset: 0x0004ADF7
		public int BufferComponentsEnd
		{
			get
			{
				return (int)this.FirstManagedComponent;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0004CBE7 File Offset: 0x0004ADE7
		public int ManagedComponentsEnd
		{
			get
			{
				return (int)this.FirstTagComponent;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0004CBFF File Offset: 0x0004ADFF
		public int TagComponentsEnd
		{
			get
			{
				return (int)this.FirstSharedComponent;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0004CC07 File Offset: 0x0004AE07
		public int SharedComponentsEnd
		{
			get
			{
				return (int)this.FirstChunkComponent;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0004CC0F File Offset: 0x0004AE0F
		public int ChunkComponentsEnd
		{
			get
			{
				return this.TypesCount;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x0004CC17 File Offset: 0x0004AE17
		public bool HasChunkComponents
		{
			get
			{
				return (int)this.FirstChunkComponent != this.TypesCount;
			}
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004CC2A File Offset: 0x0004AE2A
		public unsafe bool IsManaged(int typeIndexInArchetype)
		{
			return this.Types[typeIndexInArchetype].IsManagedComponent;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004CC44 File Offset: 0x0004AE44
		public unsafe override string ToString()
		{
			string text = "";
			for (int i = 0; i < this.TypesCount; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = this.Types[i];
				text += string.Format("  - {0}", componentTypeInArchetype);
			}
			return text;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004CC98 File Offset: 0x0004AE98
		public unsafe void AddToChunkList(ChunkIndex chunk, SharedComponentValues sharedComponentIndices, uint changeVersion, ref EntityComponentStore.ChunkListChanges changes)
		{
			chunk.ListIndex = this.Chunks.Count;
			if (this.Chunks.Count == this.Chunks.Capacity)
			{
				int num = ((this.Chunks.Capacity == 0) ? 1 : (this.Chunks.Capacity * 2));
				if (this.Chunks.InsideAllocation(sharedComponentIndices.firstIndex))
				{
					int num2 = (int)((long)(sharedComponentIndices.firstIndex - this.Chunks.GetSharedComponentValueArrayForType(0)));
					this.Chunks.Grow(num);
					sharedComponentIndices = this.Chunks.GetSharedComponentValues(num2);
				}
				else
				{
					this.Chunks.Grow(num);
				}
			}
			this.Chunks.Add(chunk, sharedComponentIndices, changeVersion);
			fixed (Archetype* ptr = &this)
			{
				Archetype* ptr2 = ptr;
				changes.TrackArchetype(ptr2);
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004CD60 File Offset: 0x0004AF60
		public unsafe void RemoveFromChunkList(ChunkIndex chunk, ref EntityComponentStore.ChunkListChanges changes)
		{
			int listIndex = chunk.ListIndex;
			this.Chunks.RemoveAtSwapBack(listIndex);
			this.Chunks[listIndex].ListIndex = listIndex;
			fixed (Archetype* ptr = &this)
			{
				Archetype* ptr2 = ptr;
				changes.TrackArchetype(ptr2);
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004CDA5 File Offset: 0x0004AFA5
		private void AddToChunkListWithEmptySlots(ChunkIndex chunk)
		{
			chunk.ListWithEmptySlotsIndex = this.ChunksWithEmptySlots.Length;
			this.ChunksWithEmptySlots.Add(in chunk);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0004CDC8 File Offset: 0x0004AFC8
		private unsafe void RemoveFromChunkListWithEmptySlots(ChunkIndex chunk)
		{
			int listWithEmptySlotsIndex = chunk.ListWithEmptySlotsIndex;
			this.ChunksWithEmptySlots.RemoveAtSwapBack(listWithEmptySlotsIndex);
			if (listWithEmptySlotsIndex < this.ChunksWithEmptySlots.Length)
			{
				ChunkIndex chunkIndex = this.ChunksWithEmptySlots.Ptr[listWithEmptySlotsIndex];
				chunkIndex.ListWithEmptySlotsIndex = listWithEmptySlotsIndex;
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004CE1C File Offset: 0x0004B01C
		internal unsafe void EmptySlotTrackingRemoveChunk(ChunkIndex chunk)
		{
			fixed (Archetype* ptr = &this)
			{
			}
			if (this.NumSharedComponents == 0)
			{
				this.RemoveFromChunkListWithEmptySlots(chunk);
				return;
			}
			this.FreeChunksBySharedComponents.Remove(chunk);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004CE4C File Offset: 0x0004B04C
		internal unsafe void EmptySlotTrackingAddChunk(ChunkIndex chunk)
		{
			fixed (Archetype* ptr = &this)
			{
			}
			if (this.NumSharedComponents == 0)
			{
				this.AddToChunkListWithEmptySlots(chunk);
				return;
			}
			this.FreeChunksBySharedComponents.Add(chunk);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004CE7C File Offset: 0x0004B07C
		internal ChunkIndex GetExistingChunkWithEmptySlots(SharedComponentValues sharedComponentValues)
		{
			if (this.NumSharedComponents == 0 && this.ChunksWithEmptySlots.Length != 0)
			{
				return this.ChunksWithEmptySlots[0];
			}
			return this.FreeChunksBySharedComponents.TryGet(sharedComponentValues, this.NumSharedComponents);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004CEB2 File Offset: 0x0004B0B2
		internal unsafe bool CompareMask(EntityQueryMask mask)
		{
			return (*((ref this.QueryMaskArray.FixedElementField) + mask.Index) & mask.Mask) == mask.Mask;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004CED7 File Offset: 0x0004B0D7
		internal void SetMask(EntityQueryMask mask)
		{
			ref byte ptr = (ref this.QueryMaskArray.FixedElementField) + mask.Index;
			ptr |= mask.Mask;
		}

		// Token: 0x0400072D RID: 1837
		public ArchetypeChunkData Chunks;

		// Token: 0x0400072E RID: 1838
		public UnsafeList<ChunkIndex> ChunksWithEmptySlots;

		// Token: 0x0400072F RID: 1839
		public ChunkListMap FreeChunksBySharedComponents;

		// Token: 0x04000730 RID: 1840
		public unsafe ComponentTypeInArchetype* Types;

		// Token: 0x04000731 RID: 1841
		public unsafe int* EnableableTypeIndexInArchetype;

		// Token: 0x04000732 RID: 1842
		public UnsafeList<IntPtr> MatchingQueryData;

		// Token: 0x04000733 RID: 1843
		public unsafe Archetype* NextChangedArchetype;

		// Token: 0x04000734 RID: 1844
		public int EntityCount;

		// Token: 0x04000735 RID: 1845
		public int ChunkCapacity;

		// Token: 0x04000736 RID: 1846
		public int TypesCount;

		// Token: 0x04000737 RID: 1847
		public int EnableableTypesCount;

		// Token: 0x04000738 RID: 1848
		public int InstanceSize;

		// Token: 0x04000739 RID: 1849
		public int InstanceSizeWithOverhead;

		// Token: 0x0400073A RID: 1850
		public int ScalarEntityPatchCount;

		// Token: 0x0400073B RID: 1851
		public int BufferEntityPatchCount;

		// Token: 0x0400073C RID: 1852
		public ulong StableHash;

		// Token: 0x0400073D RID: 1853
		public ulong BloomFilterMask;

		// Token: 0x0400073E RID: 1854
		public unsafe int* TypeMemoryOrderIndexToIndexInArchetype;

		// Token: 0x0400073F RID: 1855
		public unsafe int* TypeIndexInArchetypeToMemoryOrderIndex;

		// Token: 0x04000740 RID: 1856
		public unsafe int* Offsets;

		// Token: 0x04000741 RID: 1857
		public unsafe ushort* SizeOfs;

		// Token: 0x04000742 RID: 1858
		public unsafe int* BufferCapacities;

		// Token: 0x04000743 RID: 1859
		public short FirstBufferComponent;

		// Token: 0x04000744 RID: 1860
		public short FirstManagedComponent;

		// Token: 0x04000745 RID: 1861
		public short FirstTagComponent;

		// Token: 0x04000746 RID: 1862
		public short FirstSharedComponent;

		// Token: 0x04000747 RID: 1863
		public short FirstChunkComponent;

		// Token: 0x04000748 RID: 1864
		public ArchetypeFlags Flags;

		// Token: 0x04000749 RID: 1865
		public unsafe Archetype* CopyArchetype;

		// Token: 0x0400074A RID: 1866
		public unsafe Archetype* InstantiateArchetype;

		// Token: 0x0400074B RID: 1867
		public unsafe Archetype* CleanupResidueArchetype;

		// Token: 0x0400074C RID: 1868
		public unsafe Archetype* MetaChunkArchetype;

		// Token: 0x0400074D RID: 1869
		public unsafe EntityRemapUtility.EntityPatchInfo* ScalarEntityPatches;

		// Token: 0x0400074E RID: 1870
		public unsafe EntityRemapUtility.BufferEntityPatchInfo* BufferEntityPatches;

		// Token: 0x0400074F RID: 1871
		public unsafe EntityComponentStore* EntityComponentStore;

		// Token: 0x04000750 RID: 1872
		[FixedBuffer(typeof(byte), 128)]
		public Archetype.<QueryMaskArray>e__FixedBuffer QueryMaskArray;

		// Token: 0x020002DF RID: 735
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <QueryMaskArray>e__FixedBuffer
		{
			// Token: 0x04000751 RID: 1873
			public byte FixedElementField;
		}
	}
}
