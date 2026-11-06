using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x02000228 RID: 552
	internal struct MatchingArchetype
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0004194C File Offset: 0x0003FB4C
		public unsafe int* EnableableTypeMemoryOrderInArchetype_All
		{
			get
			{
				fixed (int* ptr = &this.IndexInArchetype.FixedElementField)
				{
					return ptr + this.RequiredComponentCount;
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00041974 File Offset: 0x0003FB74
		public unsafe int* EnableableTypeMemoryOrderInArchetype_None
		{
			get
			{
				fixed (int* ptr = &this.IndexInArchetype.FixedElementField)
				{
					return ptr + this.RequiredComponentCount + this.EnableableComponentsCount_All;
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000419A4 File Offset: 0x0003FBA4
		public unsafe int* EnableableTypeMemoryOrderInArchetype_Any
		{
			get
			{
				fixed (int* ptr = &this.IndexInArchetype.FixedElementField)
				{
					return ptr + this.RequiredComponentCount + this.EnableableComponentsCount_All + this.EnableableComponentsCount_None;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x000419E0 File Offset: 0x0003FBE0
		public unsafe int* EnableableTypeMemoryOrderInArchetype_Disabled
		{
			get
			{
				fixed (int* ptr = &this.IndexInArchetype.FixedElementField)
				{
					return ptr + this.RequiredComponentCount + this.EnableableComponentsCount_All + this.EnableableComponentsCount_None + this.EnableableComponentsCount_Any;
				}
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00041A24 File Offset: 0x0003FC24
		public unsafe static int CalculateMatchingArchetypeEnableableTypeIntersectionCount(Archetype* archetype, TypeIndex* queryComponents, int queryComponentCount)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < queryComponentCount; i++)
			{
				TypeIndex typeIndex = queryComponents[i];
				if (TypeManager.IsEnableable(typeIndex))
				{
					int nextIndexInTypeArray = ChunkDataUtility.GetNextIndexInTypeArray(archetype, typeIndex, num2);
					if (nextIndexInTypeArray >= 0)
					{
						num2 = nextIndexInTypeArray;
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00041A74 File Offset: 0x0003FC74
		public unsafe static MatchingArchetype* Create(ref BlockAllocator allocator, Archetype* archetype, in ArchetypeQuery archetypeQuery, int queryRequiredComponentCount)
		{
			int num = MatchingArchetype.CalculateMatchingArchetypeEnableableTypeIntersectionCount(archetype, archetypeQuery.All, archetypeQuery.AllCount);
			int num2 = MatchingArchetype.CalculateMatchingArchetypeEnableableTypeIntersectionCount(archetype, archetypeQuery.None, archetypeQuery.NoneCount);
			int num3 = MatchingArchetype.CalculateMatchingArchetypeEnableableTypeIntersectionCount(archetype, archetypeQuery.Any, archetypeQuery.AnyCount);
			int num4 = MatchingArchetype.CalculateMatchingArchetypeEnableableTypeIntersectionCount(archetype, archetypeQuery.Disabled, archetypeQuery.DisabledCount);
			int num5 = num + num2 + num3 + num4;
			MatchingArchetype* ptr = (MatchingArchetype*)allocator.Allocate(MatchingArchetype.GetAllocationSize(queryRequiredComponentCount, num5), 8);
			ptr->Archetype = archetype;
			ptr->RequiredComponentCount = queryRequiredComponentCount;
			ptr->EnableableComponentsCount_All = num;
			ptr->EnableableComponentsCount_None = num2;
			ptr->EnableableComponentsCount_Any = num3;
			ptr->EnableableComponentsCount_Disabled = num4;
			return ptr;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00041B19 File Offset: 0x0003FD19
		private unsafe static int GetAllocationSize(int requiredComponentsCount, int enableableComponentCount)
		{
			return sizeof(MatchingArchetype) + 4 * (requiredComponentsCount - 1) + 4 * enableableComponentCount;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00041B2C File Offset: 0x0003FD2C
		public unsafe bool ChunkMatchesFilter(int chunkIndex, ref EntityQueryFilter filter)
		{
			ArchetypeChunkData chunks = this.Archetype->Chunks;
			for (int i = 0; i < filter.Shared.Count; i++)
			{
				int num = *((ref filter.Shared.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4);
				int num2 = *((ref filter.Shared.SharedComponentIndex.FixedElementField) + (IntPtr)i * 4);
				int num3 = *((ref this.IndexInArchetype.FixedElementField) + (IntPtr)num * 4) - (int)this.Archetype->FirstSharedComponent;
				if (chunks.GetSharedComponentValueArrayForType(num3)[chunkIndex] != num2)
				{
					return false;
				}
			}
			if (filter.Changed.Count == 0 && !filter.UseOrderFiltering)
			{
				return true;
			}
			bool flag = filter.UseOrderFiltering && ChangeVersionUtility.DidChange(chunks.GetOrderVersion(chunkIndex), filter.RequiredChangeVersion);
			bool flag2 = false;
			for (int j = 0; j < filter.Changed.Count; j++)
			{
				int num4 = *((ref filter.Changed.IndexInEntityQuery.FixedElementField) + (IntPtr)j * 4);
				int num5 = *((ref this.IndexInArchetype.FixedElementField) + (IntPtr)num4 * 4);
				uint* changeVersionArrayForType = chunks.GetChangeVersionArrayForType(num5);
				uint requiredChangeVersion = filter.RequiredChangeVersion;
				flag2 |= ChangeVersionUtility.DidChange(changeVersionArrayForType[chunkIndex], requiredChangeVersion);
			}
			return flag2 || flag;
		}

		// Token: 0x040005B4 RID: 1460
		public unsafe Archetype* Archetype;

		// Token: 0x040005B5 RID: 1461
		public int RequiredComponentCount;

		// Token: 0x040005B6 RID: 1462
		public int EnableableComponentsCount_All;

		// Token: 0x040005B7 RID: 1463
		public int EnableableComponentsCount_None;

		// Token: 0x040005B8 RID: 1464
		public int EnableableComponentsCount_Any;

		// Token: 0x040005B9 RID: 1465
		public int EnableableComponentsCount_Disabled;

		// Token: 0x040005BA RID: 1466
		[FixedBuffer(typeof(int), 1)]
		public MatchingArchetype.<IndexInArchetype>e__FixedBuffer IndexInArchetype;

		// Token: 0x02000229 RID: 553
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <IndexInArchetype>e__FixedBuffer
		{
			// Token: 0x040005BB RID: 1467
			public int FixedElementField;
		}
	}
}
