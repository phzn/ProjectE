using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000054 RID: 84
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
	public class BeginInitializationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000B6C3 File Offset: 0x000098C3
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public BeginInitializationEntityCommandBufferSystem()
		{
		}

		// Token: 0x02000055 RID: 85
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x0600024F RID: 591 RVA: 0x0000B6EA File Offset: 0x000098EA
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000250 RID: 592 RVA: 0x0000B6FE File Offset: 0x000098FE
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x06000251 RID: 593 RVA: 0x0000B70C File Offset: 0x0000990C
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x06000252 RID: 594 RVA: 0x0000B71A File Offset: 0x0000991A
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x0400009D RID: 157
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x0400009E RID: 158
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
