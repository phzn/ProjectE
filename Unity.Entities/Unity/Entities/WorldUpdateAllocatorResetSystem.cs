using System;

namespace Unity.Entities
{
	// Token: 0x02000366 RID: 870
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	public struct WorldUpdateAllocatorResetSystem : ISystem
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x000581F4 File Offset: 0x000563F4
		public void OnUpdate(ref SystemState state)
		{
			state.m_WorldUnmanaged.ResetUpdateAllocator();
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00058201 File Offset: 0x00056401
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((WorldUpdateAllocatorResetSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}
	}
}
