using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000065 RID: 101
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
	public class EndSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000B9AF File Offset: 0x00009BAF
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<EndSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public EndSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x02000066 RID: 102
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x06000280 RID: 640 RVA: 0x0000B9CE File Offset: 0x00009BCE
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000281 RID: 641 RVA: 0x0000B9E2 File Offset: 0x00009BE2
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x06000282 RID: 642 RVA: 0x0000B9F0 File Offset: 0x00009BF0
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000283 RID: 643 RVA: 0x0000B9FE File Offset: 0x00009BFE
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000AB RID: 171
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000AC RID: 172
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
