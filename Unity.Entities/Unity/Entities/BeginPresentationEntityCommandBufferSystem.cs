using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000069 RID: 105
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
	public class BeginPresentationEntityCommandBufferSystem : EntityCommandBufferSystem
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000BA07 File Offset: 0x00009C07
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.RegisterSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>(base.PendingBuffers, base.World.Unmanaged);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B6E2 File Offset: 0x000098E2
		[Preserve]
		public BeginPresentationEntityCommandBufferSystem()
		{
		}

		// Token: 0x0200006A RID: 106
		public struct Singleton : IComponentData, IQueryTypeParameter, IECBSingleton
		{
			// Token: 0x06000288 RID: 648 RVA: 0x0000BA26 File Offset: 0x00009C26
			public unsafe EntityCommandBuffer CreateCommandBuffer(WorldUnmanaged world)
			{
				return EntityCommandBufferSystem.CreateCommandBuffer(ref *this.pendingBuffers, this.allocator, world);
			}

			// Token: 0x06000289 RID: 649 RVA: 0x0000BA3A File Offset: 0x00009C3A
			public unsafe void SetPendingBufferList(ref UnsafeList<EntityCommandBuffer> buffers)
			{
				this.pendingBuffers = (UnsafeList<EntityCommandBuffer>*)UnsafeUtility.AddressOf<UnsafeList<EntityCommandBuffer>>(ref buffers);
			}

			// Token: 0x0600028A RID: 650 RVA: 0x0000BA48 File Offset: 0x00009C48
			public void SetAllocator(Allocator allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x0600028B RID: 651 RVA: 0x0000BA56 File Offset: 0x00009C56
			public void SetAllocator(AllocatorManager.AllocatorHandle allocatorIn)
			{
				this.allocator = allocatorIn;
			}

			// Token: 0x040000AD RID: 173
			internal unsafe UnsafeList<EntityCommandBuffer>* pendingBuffers;

			// Token: 0x040000AE RID: 174
			internal AllocatorManager.AllocatorHandle allocator;
		}
	}
}
