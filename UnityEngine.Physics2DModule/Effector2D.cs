using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000037 RID: 55
	[NativeHeader("Modules/Physics2D/Effector2D.h")]
	public class Effector2D : Behaviour
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000488 RID: 1160
		// (set) Token: 0x06000489 RID: 1161
		public extern bool useColliderMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600048A RID: 1162
		// (set) Token: 0x0600048B RID: 1163
		public extern int colliderMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600048C RID: 1164
		internal extern bool requiresCollider
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600048D RID: 1165
		internal extern bool designedForTrigger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600048E RID: 1166
		internal extern bool designedForNonTrigger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
