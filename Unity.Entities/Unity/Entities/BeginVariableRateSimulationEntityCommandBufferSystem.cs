using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200005E RID: 94
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(VariableRateSimulationSystemGroup), OrderFirst = true)]
	public class BeginVariableRateSimulationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000B891 File Offset: 0x00009A91
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<BeginVariableRateSimulationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public BeginVariableRateSimulationEntityCommandBufferSystem()
		{
		}

		// Token: 0x0200005F RID: 95
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x0600026C RID: 620 RVA: 0x0000B8B0 File Offset: 0x00009AB0
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x0600026D RID: 621 RVA: 0x0000B8C4 File Offset: 0x00009AC4
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x0600026E RID: 622 RVA: 0x0000B8D2 File Offset: 0x00009AD2
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x0600026F RID: 623 RVA: 0x0000B8E0 File Offset: 0x00009AE0
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000A5 RID: 165
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000A6 RID: 166
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
