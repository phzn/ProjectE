using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000059 RID: 89
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
	public class BeginFixedStepSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000B78B File Offset: 0x0000998B
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public BeginFixedStepSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x0200005A RID: 90
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x0600025D RID: 605 RVA: 0x0000B7AA File Offset: 0x000099AA
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x0600025E RID: 606 RVA: 0x0000B7BE File Offset: 0x000099BE
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x0600025F RID: 607 RVA: 0x0000B7CC File Offset: 0x000099CC
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000260 RID: 608 RVA: 0x0000B7DA File Offset: 0x000099DA
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000A1 RID: 161
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000A2 RID: 162
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
