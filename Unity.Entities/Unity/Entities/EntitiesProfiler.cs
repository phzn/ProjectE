using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000249 RID: 585
	internal class EntitiesProfiler
	{
		// Token: 0x0200024A RID: 586
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_PROFILER")]
		[StructLayout(LayoutKind.Explicit, Size = 1024)]
		public struct ArchetypeData : IEquatable<EntitiesProfiler.ArchetypeData>
		{
			// Token: 0x06000F22 RID: 3874 RVA: 0x000440E0 File Offset: 0x000422E0
			public unsafe ArchetypeData(Archetype* archetype)
			{
				this.StableHash = archetype->StableHash;
				this.ChunkCapacity = archetype->ChunkCapacity;
				this.InstanceSize = archetype->InstanceSize;
				this.ComponentTypes = default(EntitiesProfiler.FixedComponentTypeDataList);
				int num = 0;
				while (num < archetype->TypesCount && num < this.ComponentTypes.Capacity)
				{
					TypeIndex typeIndex = archetype->Types[num].TypeIndex;
					ulong stableTypeHash = TypeManager.GetTypeInfo(typeIndex).StableTypeHash;
					ComponentTypeFlags componentTypeFlags = (TypeManager.IsChunkComponent(typeIndex) ? ComponentTypeFlags.ChunkComponent : ComponentTypeFlags.None);
					this.ComponentTypes.Add(stableTypeHash, componentTypeFlags);
					num++;
				}
			}

			// Token: 0x06000F23 RID: 3875 RVA: 0x00044176 File Offset: 0x00042376
			public bool Equals(EntitiesProfiler.ArchetypeData other)
			{
				return this.StableHash == other.StableHash;
			}

			// Token: 0x06000F24 RID: 3876 RVA: 0x00044188 File Offset: 0x00042388
			[ExcludeFromBurstCompatTesting("Takes managed object")]
			public override bool Equals(object obj)
			{
				if (obj is EntitiesProfiler.ArchetypeData)
				{
					EntitiesProfiler.ArchetypeData archetypeData = (EntitiesProfiler.ArchetypeData)obj;
					return this.Equals(archetypeData);
				}
				return false;
			}

			// Token: 0x06000F25 RID: 3877 RVA: 0x000441AF File Offset: 0x000423AF
			public override int GetHashCode()
			{
				return this.StableHash.GetHashCode();
			}

			// Token: 0x06000F26 RID: 3878 RVA: 0x000441BC File Offset: 0x000423BC
			public static bool operator ==(EntitiesProfiler.ArchetypeData lhs, EntitiesProfiler.ArchetypeData rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06000F27 RID: 3879 RVA: 0x000441C6 File Offset: 0x000423C6
			public static bool operator !=(EntitiesProfiler.ArchetypeData lhs, EntitiesProfiler.ArchetypeData rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x0400060A RID: 1546
			[FieldOffset(0)]
			public readonly ulong StableHash;

			// Token: 0x0400060B RID: 1547
			[FieldOffset(8)]
			public readonly int ChunkCapacity;

			// Token: 0x0400060C RID: 1548
			[FieldOffset(12)]
			public readonly int InstanceSize;

			// Token: 0x0400060D RID: 1549
			[FieldOffset(16)]
			public readonly EntitiesProfiler.FixedComponentTypeDataList ComponentTypes;
		}

		// Token: 0x0200024B RID: 587
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		public readonly struct ComponentTypeData
		{
			// Token: 0x06000F28 RID: 3880 RVA: 0x000441D3 File Offset: 0x000423D3
			public ComponentTypeData(ulong stableTypeHash, ComponentTypeFlags flags)
			{
				this.StableTypeHash = stableTypeHash;
				this.Flags = flags;
			}

			// Token: 0x06000F29 RID: 3881 RVA: 0x000441E4 File Offset: 0x000423E4
			public TypeIndex GetTypeIndex()
			{
				TypeIndex typeIndex = TypeManager.GetTypeIndexFromStableTypeHash(this.StableTypeHash);
				if (typeIndex == TypeIndex.Null)
				{
					return typeIndex;
				}
				if ((this.Flags & ComponentTypeFlags.ChunkComponent) != ComponentTypeFlags.None)
				{
					typeIndex = TypeManager.MakeChunkComponentTypeIndex(typeIndex);
				}
				return typeIndex;
			}

			// Token: 0x0400060E RID: 1550
			[FieldOffset(0)]
			public readonly ulong StableTypeHash;

			// Token: 0x0400060F RID: 1551
			[FieldOffset(8)]
			public readonly ComponentTypeFlags Flags;
		}

		// Token: 0x0200024C RID: 588
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_PROFILER")]
		[StructLayout(LayoutKind.Explicit, Size = 1008)]
		public struct FixedComponentTypeDataList
		{
			// Token: 0x1700010D RID: 269
			// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0004421E File Offset: 0x0004241E
			private static int s_ComponentTypeDataSize
			{
				get
				{
					return 9;
				}
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00044222 File Offset: 0x00042422
			public int Length
			{
				get
				{
					return (int)this.m_Length;
				}
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0004422A File Offset: 0x0004242A
			public int Capacity
			{
				get
				{
					return 1006 / EntitiesProfiler.FixedComponentTypeDataList.s_ComponentTypeDataSize;
				}
			}

			// Token: 0x17000110 RID: 272
			public unsafe EntitiesProfiler.ComponentTypeData this[int index]
			{
				get
				{
					fixed (byte* ptr = &this.m_Buffer.FixedElementField)
					{
						byte* ptr2 = ptr;
						int num = index * EntitiesProfiler.FixedComponentTypeDataList.s_ComponentTypeDataSize;
						ulong num2 = *UnsafeUtility.AsRef<ulong>((void*)(ptr2 + num));
						ComponentTypeFlags componentTypeFlags = *UnsafeUtility.AsRef<ComponentTypeFlags>((void*)(ptr2 + num + 8));
						return new EntitiesProfiler.ComponentTypeData(num2, componentTypeFlags);
					}
				}
			}

			// Token: 0x06000F2E RID: 3886 RVA: 0x00044278 File Offset: 0x00042478
			public unsafe void Add(ulong stableTypeHash, ComponentTypeFlags flags)
			{
				fixed (byte* ptr = &this.m_Buffer.FixedElementField)
				{
					byte* ptr2 = ptr;
					ushort length = this.m_Length;
					this.m_Length = length + 1;
					int num = (int)length * EntitiesProfiler.FixedComponentTypeDataList.s_ComponentTypeDataSize;
					UnsafeUtility.MemCpy((void*)(ptr2 + num), (void*)(&stableTypeHash), 8L);
					UnsafeUtility.MemCpy((void*)(ptr2 + num + 8), (void*)(&flags), 1L);
				}
			}

			// Token: 0x04000610 RID: 1552
			private const int k_BufferSize = 1006;

			// Token: 0x04000611 RID: 1553
			[FieldOffset(0)]
			private ushort m_Length;

			// Token: 0x04000612 RID: 1554
			[FixedBuffer(typeof(byte), 1006)]
			[FieldOffset(2)]
			private EntitiesProfiler.FixedComponentTypeDataList.<m_Buffer>e__FixedBuffer m_Buffer;

			// Token: 0x0200024D RID: 589
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1006)]
			public struct <m_Buffer>e__FixedBuffer
			{
				// Token: 0x04000613 RID: 1555
				public byte FixedElementField;
			}
		}
	}
}
