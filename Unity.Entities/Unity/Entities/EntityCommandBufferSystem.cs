using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200013B RID: 315
	public abstract class EntityCommandBufferSystem : SystemBase
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00021607 File Offset: 0x0001F807
		protected internal unsafe ref UnsafeList<EntityCommandBuffer> PendingBuffers
		{
			get
			{
				return ref *this.m_PendingBuffers;
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00021610 File Offset: 0x0001F810
		public EntityCommandBuffer CreateCommandBuffer()
		{
			return EntityCommandBufferSystem.CreateCommandBuffer(this.PendingBuffers, this.m_EntityCommandBufferAllocator.Allocator.Handle.ToAllocator, base.World.Unmanaged);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00021650 File Offset: 0x0001F850
		public void AddJobHandleForProducer(JobHandle producerJob)
		{
			this.m_ProducerHandle = JobHandle.CombineDependencies(this.m_ProducerHandle, producerJob);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00021664 File Offset: 0x0001F864
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.m_EntityCommandBufferAllocator = new AllocatorHelper<RewindableAllocator>(Allocator.Persistent, false, 0);
			this.m_EntityCommandBufferAllocator.Allocator.Initialize(16384, false);
			this.m_PendingBuffers = UnsafeList<EntityCommandBuffer>.Create(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000216B4 File Offset: 0x0001F8B4
		[Preserve]
		protected unsafe override void OnDestroy()
		{
			this.FlushPendingBuffers(false);
			this.PendingBuffers.Clear();
			this.m_PendingBuffers->Dispose();
			AllocatorManager.Free<UnsafeList<EntityCommandBuffer>>(Allocator.Persistent, this.m_PendingBuffers, 1);
			this.m_EntityCommandBufferAllocator.Allocator.Dispose();
			this.m_EntityCommandBufferAllocator.Dispose();
			base.OnDestroy();
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00021711 File Offset: 0x0001F911
		[Preserve]
		protected override void OnUpdate()
		{
			this.FlushPendingBuffers(true);
			this.PendingBuffers.Clear();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00021728 File Offset: 0x0001F928
		internal unsafe void FlushPendingBuffers(bool playBack)
		{
			if (this.m_PendingBuffers->IsEmpty)
			{
				return;
			}
			base.CompleteDependency();
			this.m_ProducerHandle.Complete();
			this.m_ProducerHandle = default(JobHandle);
			int length = this.PendingBuffers.Length;
			for (int i = 0; i < length; i++)
			{
				EntityCommandBuffer entityCommandBuffer = this.PendingBuffers[i];
				if (entityCommandBuffer.IsCreated)
				{
					if (playBack)
					{
						entityCommandBuffer.Playback(base.EntityManager);
					}
					entityCommandBuffer.Dispose();
					this.PendingBuffers[i] = entityCommandBuffer;
				}
			}
			this.m_EntityCommandBufferAllocator.Allocator.Rewind();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000217C4 File Offset: 0x0001F9C4
		public unsafe static EntityCommandBuffer CreateCommandBuffer(ref UnsafeList<EntityCommandBuffer> pendingBuffers, AllocatorManager.AllocatorHandle allocator, WorldUnmanaged world)
		{
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(allocator, PlaybackPolicy.SinglePlayback);
			SystemState* ptr = world.ResolveSystemState(world.ExecutingSystem);
			entityCommandBuffer.SystemID = ((ptr != null) ? ptr->m_SystemID : 0);
			entityCommandBuffer.OriginSystemHandle = ((ptr != null) ? ptr->m_Handle : default(SystemHandle));
			pendingBuffers.Add(in entityCommandBuffer);
			return entityCommandBuffer;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00021824 File Offset: 0x0001FA24
		public static EntityCommandBuffer CreateCommandBuffer(ref UnsafeList<EntityCommandBuffer> pendingBuffers, WorldUnmanaged world)
		{
			return EntityCommandBufferSystem.CreateCommandBuffer(ref pendingBuffers, world.UpdateAllocator.Handle.ToAllocator, world);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00016D44 File Offset: 0x00014F44
		[Preserve]
		protected EntityCommandBufferSystem()
		{
		}

		// Token: 0x04000384 RID: 900
		protected unsafe UnsafeList<EntityCommandBuffer>* m_PendingBuffers;

		// Token: 0x04000385 RID: 901
		internal AllocatorHelper<RewindableAllocator> m_EntityCommandBufferAllocator;

		// Token: 0x04000386 RID: 902
		private JobHandle m_ProducerHandle;
	}
}
