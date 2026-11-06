using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000319 RID: 793
	[Serializable]
	public struct SceneSection : ISharedComponentData, IQueryTypeParameter, IEquatable<SceneSection>
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x00050907 File Offset: 0x0004EB07
		public bool Equals(SceneSection other)
		{
			return this.SceneGUID.Equals(other.SceneGUID) && this.Section == other.Section;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0005092C File Offset: 0x0004EB2C
		public override int GetHashCode()
		{
			return (this.SceneGUID.GetHashCode() * 397) ^ this.Section;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0005094C File Offset: 0x0004EB4C
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static bool __codegen__Equals(void* self, void* A_1)
		{
			return ((SceneSection*)self)->Equals(*(SceneSection*)A_1);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0005095C File Offset: 0x0004EB5C
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public unsafe static int __codegen__GetHashCode(void* self)
		{
			return ((SceneSection*)self)->GetHashCode();
		}

		// Token: 0x040007B9 RID: 1977
		public Hash128 SceneGUID;

		// Token: 0x040007BA RID: 1978
		public int Section;
	}
}
