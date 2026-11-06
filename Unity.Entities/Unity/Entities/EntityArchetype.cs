using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002FC RID: 764
	[DebuggerTypeProxy(typeof(EntityArchetypeDebugView))]
	public struct EntityArchetype : IEquatable<EntityArchetype>
	{
		// Token: 0x060014B4 RID: 5300 RVA: 0x0004F14B File Offset: 0x0004D34B
		internal unsafe EntityArchetype(Archetype* archetype)
		{
			this.Archetype = archetype;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0004F154 File Offset: 0x0004D354
		public bool Valid
		{
			get
			{
				return this.Archetype != null;
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004F163 File Offset: 0x0004D363
		public static bool operator ==(EntityArchetype lhs, EntityArchetype rhs)
		{
			return lhs.Archetype == rhs.Archetype;
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004F173 File Offset: 0x0004D373
		public static bool operator !=(EntityArchetype lhs, EntityArchetype rhs)
		{
			return lhs.Archetype != rhs.Archetype;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004F186 File Offset: 0x0004D386
		public override bool Equals(object compare)
		{
			return this == (EntityArchetype)compare;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0004F163 File Offset: 0x0004D363
		public bool Equals(EntityArchetype entityArchetype)
		{
			return this.Archetype == entityArchetype.Archetype;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0004F199 File Offset: 0x0004D399
		public override int GetHashCode()
		{
			return this.Archetype;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0004F1A2 File Offset: 0x0004D3A2
		public override string ToString()
		{
			return EntityManager.EntityManagerDebug.GetArchetypeDebugString(this.Archetype);
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004F1AF File Offset: 0x0004D3AF
		public NativeArray<ComponentType> GetComponentTypes(Allocator allocator = Allocator.Temp)
		{
			return this.GetComponentTypes(allocator);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004F1C0 File Offset: 0x0004D3C0
		public unsafe NativeArray<ComponentType> GetComponentTypes(AllocatorManager.AllocatorHandle allocator)
		{
			int typesCount = this.Archetype->TypesCount;
			NativeArray<ComponentType> nativeArray = CollectionHelper.CreateNativeArray<ComponentType>(typesCount - 1, allocator, NativeArrayOptions.ClearMemory);
			for (int i = 1; i < typesCount; i++)
			{
				nativeArray[i - 1] = this.Archetype->Types[i].ToComponentType();
			}
			return nativeArray;
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004F218 File Offset: 0x0004D418
		public unsafe static void CalculateDifference(EntityArchetype before, EntityArchetype after, TypeIndex* addedTypes, out int addedTypesCount, TypeIndex* removedTypes, out int removedTypesCount)
		{
			int i = 1;
			int j = 1;
			ComponentTypeInArchetype* types = before.Archetype->Types;
			int typesCount = before.Archetype->TypesCount;
			ComponentTypeInArchetype* types2 = after.Archetype->Types;
			int typesCount2 = after.Archetype->TypesCount;
			int num = 0;
			int num2 = 0;
			while (i < typesCount)
			{
				if (j >= typesCount2)
				{
					break;
				}
				if (types[i].TypeIndex == types2[j].TypeIndex)
				{
					j++;
					i++;
				}
				else if (types[i].TypeIndex > types2[j].TypeIndex)
				{
					addedTypes[num++] = types2[j].TypeIndex;
					j++;
				}
				else
				{
					removedTypes[num2++] = types[i].TypeIndex;
					i++;
				}
			}
			while (j < typesCount2)
			{
				addedTypes[num++] = types2[j].TypeIndex;
				j++;
			}
			while (i < typesCount)
			{
				removedTypes[num2++] = types[i].TypeIndex;
				i++;
			}
			addedTypesCount = num;
			removedTypesCount = num2;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0004F393 File Offset: 0x0004D593
		public unsafe int TypesCount
		{
			get
			{
				return this.Archetype->TypesCount;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0004F3A0 File Offset: 0x0004D5A0
		public unsafe int ChunkCount
		{
			get
			{
				return this.Archetype->Chunks.Count;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0004F3B2 File Offset: 0x0004D5B2
		public unsafe int ChunkCapacity
		{
			get
			{
				return this.Archetype->ChunkCapacity;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0004F3BF File Offset: 0x0004D5BF
		public unsafe bool Prefab
		{
			get
			{
				return this.Archetype->Prefab;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0004F3CC File Offset: 0x0004D5CC
		public unsafe bool Disabled
		{
			get
			{
				return this.Archetype->Disabled;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0004F3D9 File Offset: 0x0004D5D9
		public unsafe ulong StableHash
		{
			get
			{
				return this.Archetype->StableHash;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x0004F3E6 File Offset: 0x0004D5E6
		internal unsafe ComponentTypeInArchetype* Types
		{
			get
			{
				return this.Archetype->Types;
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004F3F3 File Offset: 0x0004D5F3
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void CheckValidEntityArchetype()
		{
			if (!this.Valid)
			{
				throw new ArgumentException("EntityArchetype argument is invalid. Calling new EntityArchetype() produces an invalid value. Call EntityManager.CreateArchetype() to get a valid EntityArchetype value.");
			}
		}

		// Token: 0x04000792 RID: 1938
		[NativeDisableUnsafePtrRestriction]
		internal unsafe Archetype* Archetype;
	}
}
