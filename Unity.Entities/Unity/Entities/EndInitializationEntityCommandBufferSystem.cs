using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000056 RID: 86
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	public class EndInitializationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000B723 File Offset: 0x00009923
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<EndInitializationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public EndInitializationEntityCommandBufferSystem()
		{
		}

		// Token: 0x02000057 RID: 87
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x06000255 RID: 597 RVA: 0x0000B742 File Offset: 0x00009942
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000256 RID: 598 RVA: 0x0000B756 File Offset: 0x00009956
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x06000257 RID: 599 RVA: 0x0000B764 File Offset: 0x00009964
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000258 RID: 600 RVA: 0x0000B772 File Offset: 0x00009972
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x0400009F RID: 159
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000A0 RID: 160
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
