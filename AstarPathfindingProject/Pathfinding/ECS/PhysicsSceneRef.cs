using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

namespace Pathfinding.ECS
{
	// Token: 0x02000303 RID: 771
	public struct PhysicsSceneRef : ISharedComponentData, IQueryTypeParameter, IEquatable<PhysicsSceneRef>
	{
		// Token: 0x0600131B RID: 4891 RVA: 0x00073AC4 File Offset: 0x00071CC4
		public bool Equals(PhysicsSceneRef other)
		{
			return this.physicsScene == other.physicsScene;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00073AD7 File Offset: 0x00071CD7
		public override int GetHashCode()
		{
			return this.physicsScene.GetHashCode();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00073AEA File Offset: 0x00071CEA
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static bool __codegen__Equals(void* self, void* A_1)
		{
			return ((PhysicsSceneRef*)self)->Equals(*(PhysicsSceneRef*)A_1);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00073AFA File Offset: 0x00071CFA
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static int __codegen__GetHashCode(void* self)
		{
			return ((PhysicsSceneRef*)self)->GetHashCode();
		}

		// Token: 0x04000DFE RID: 3582
		public PhysicsScene physicsScene;
	}
}
