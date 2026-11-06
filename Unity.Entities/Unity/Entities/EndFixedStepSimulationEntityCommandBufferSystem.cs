using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200005B RID: 91
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true)]
	public class EndFixedStepSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000B7E3 File Offset: 0x000099E3
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public EndFixedStepSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x0200005C RID: 92
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x06000263 RID: 611 RVA: 0x0000B802 File Offset: 0x00009A02
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000264 RID: 612 RVA: 0x0000B816 File Offset: 0x00009A16
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x06000265 RID: 613 RVA: 0x0000B824 File Offset: 0x00009A24
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000266 RID: 614 RVA: 0x0000B832 File Offset: 0x00009A32
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000A3 RID: 163
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000A4 RID: 164
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
