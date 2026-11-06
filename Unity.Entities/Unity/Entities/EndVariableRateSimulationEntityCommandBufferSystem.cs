using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000060 RID: 96
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(VariableRateSimulationSystemGroup), OrderLast = true)]
	public class EndVariableRateSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000B8E9 File Offset: 0x00009AE9
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<EndVariableRateSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public EndVariableRateSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x02000061 RID: 97
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x06000272 RID: 626 RVA: 0x0000B908 File Offset: 0x00009B08
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000273 RID: 627 RVA: 0x0000B91C File Offset: 0x00009B1C
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x06000274 RID: 628 RVA: 0x0000B92A File Offset: 0x00009B2A
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000275 RID: 629 RVA: 0x0000B938 File Offset: 0x00009B38
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000A7 RID: 167
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000A8 RID: 168
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
