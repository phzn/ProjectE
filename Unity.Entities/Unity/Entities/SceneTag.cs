using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020001A8 RID: 424
	[ChunkSerializable]
	[Serializable]
	public struct SceneTag : ISharedComponentData, IQueryTypeParameter, IEquatable<SceneTag>
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x000321A6 File Offset: 0x000303A6
		public override int GetHashCode()
		{
			return this.SceneEntity.GetHashCode();
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000321B9 File Offset: 0x000303B9
		public bool Equals(SceneTag other)
		{
			return this.SceneEntity == other.SceneEntity;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000321CC File Offset: 0x000303CC
		public override string ToString()
		{
			return string.Format("SubSceneTag: {0}", this.SceneEntity);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000321E3 File Offset: 0x000303E3
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static bool __codegen__Equals(void* self, void* A_1)
		{
			return ((SceneTag*)self)->Equals(*(SceneTag*)A_1);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000321F3 File Offset: 0x000303F3
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static int __codegen__GetHashCode(void* self)
		{
			return ((SceneTag*)self)->GetHashCode();
		}

		// Token: 0x040004C5 RID: 1221
		public Entity SceneEntity;
	}
}
