using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000002 RID: 2
	[UsedByNativeCode]
	[NativeHeader("Modules/Cloth/Cloth.h")]
	public struct ClothSphereColliderPair
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public SphereCollider first { readonly get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public SphereCollider second { readonly get; set; }

		// Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		public ClothSphereColliderPair(SphereCollider a)
		{
			this.first = a;
			this.second = null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002085 File Offset: 0x00000285
		public ClothSphereColliderPair(SphereCollider a, SphereCollider b)
		{
			this.first = a;
			this.second = b;
		}
	}
}
