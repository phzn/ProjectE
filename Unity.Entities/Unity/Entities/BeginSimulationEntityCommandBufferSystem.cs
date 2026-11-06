using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000063 RID: 99
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
	public class BeginSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000B957 File Offset: 0x00009B57
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public BeginSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x02000064 RID: 100
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x0600027A RID: 634 RVA: 0x0000B976 File Offset: 0x00009B76
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x0600027B RID: 635 RVA: 0x0000B98A File Offset: 0x00009B8A
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x0600027C RID: 636 RVA: 0x0000B998 File Offset: 0x00009B98
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x0600027D RID: 637 RVA: 0x0000B9A6 File Offset: 0x00009BA6
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000A9 RID: 169
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000AA RID: 170
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
