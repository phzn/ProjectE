using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000311 RID: 785
	public struct EntityAccess<[IsUnmanaged] T> where T : struct, ValueType, IComponentData
	{
		// Token: 0x06001325 RID: 4901 RVA: 0x00073C4F File Offset: 0x00071E4F
		public EntityAccess(bool readOnly)
		{
			this.handle = default(ComponentTypeHandle<T>);
			this.readOnly = readOnly;
			this.lastSystemVersion = 0U;
			this.worldSequenceNumber = 0UL;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00073C74 File Offset: 0x00071E74
		public unsafe void Update(EntityManager entityManager)
		{
			uint globalSystemVersion = entityManager.GlobalSystemVersion;
			ulong sequenceNumber = entityManager.WorldUnmanaged.SequenceNumber;
			if (globalSystemVersion != this.lastSystemVersion || this.worldSequenceNumber != sequenceNumber)
			{
				if (this.lastSystemVersion == 0U || this.worldSequenceNumber != sequenceNumber)
				{
					this.handle = entityManager.GetComponentTypeHandle<T>(this.readOnly);
				}
				this.lastSystemVersion = globalSystemVersion;
				this.worldSequenceNumber = sequenceNumber;
				if (this.readOnly)
				{
					entityManager.CompleteDependencyBeforeRO<T>();
				}
				else
				{
					entityManager.CompleteDependencyBeforeRW<T>();
				}
			}
			byte* ptr = (byte*)UnsafeUtility.AddressOf<ComponentTypeHandle<T>>(ref this.handle);
			*(int*)(ptr + EntityAccessHelper.GlobalSystemVersionOffset) = (int)globalSystemVersion;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00073D0A File Offset: 0x00071F0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool HasComponent(EntityStorageInfo storage)
		{
			return storage.Chunk.Has<T>(ref this.handle);
		}

		// Token: 0x1700031A RID: 794
		public unsafe ref T this[EntityStorageInfo storage]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (!this.readOnly)
				{
					return ref *(T*)((byte*)storage.Chunk.GetRequiredComponentDataPtrRW<T>(ref this.handle) + (IntPtr)storage.IndexInChunk * (IntPtr)sizeof(T));
				}
				return ref *(T*)((byte*)storage.Chunk.GetRequiredComponentDataPtrRO<T>(ref this.handle) + (IntPtr)storage.IndexInChunk * (IntPtr)sizeof(T));
			}
		}

		// Token: 0x04000E0B RID: 3595
		public ComponentTypeHandle<T> handle;

		// Token: 0x04000E0C RID: 3596
		private uint lastSystemVersion;

		// Token: 0x04000E0D RID: 3597
		private ulong worldSequenceNumber;

		// Token: 0x04000E0E RID: 3598
		private bool readOnly;
	}
}
