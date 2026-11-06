using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.AI
{
	// Token: 0x02000006 RID: 6
	[StaticAccessor("NavMeshWorldBindings", StaticAccessorType.DoubleColon)]
	public struct NavMeshWorld
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002150 File Offset: 0x00000350
		public bool IsValid()
		{
			return this.world != IntPtr.Zero;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002174 File Offset: 0x00000374
		public static NavMeshWorld GetDefaultWorld()
		{
			NavMeshWorld navMeshWorld;
			NavMeshWorld.GetDefaultWorld_Injected(out navMeshWorld);
			return navMeshWorld;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002189 File Offset: 0x00000389
		private static void AddDependencyInternal(IntPtr navmesh, JobHandle handle)
		{
			NavMeshWorld.AddDependencyInternal_Injected(navmesh, ref handle);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002193 File Offset: 0x00000393
		public void AddDependency(JobHandle job)
		{
			NavMeshWorld.AddDependencyInternal(this.world, job);
		}

		// Token: 0x0600000E RID: 14
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDefaultWorld_Injected(out NavMeshWorld ret);

		// Token: 0x0600000F RID: 15
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddDependencyInternal_Injected(IntPtr navmesh, ref JobHandle handle);

		// Token: 0x04000013 RID: 19
		internal IntPtr world;
	}
}
