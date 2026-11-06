using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002E RID: 46
	[NativeHeader("Modules/Physics2D/SpringJoint2D.h")]
	public sealed class SpringJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000413 RID: 1043
		// (set) Token: 0x06000414 RID: 1044
		public extern bool autoConfigureDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000415 RID: 1045
		// (set) Token: 0x06000416 RID: 1046
		public extern float distance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000417 RID: 1047
		// (set) Token: 0x06000418 RID: 1048
		public extern float dampingRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000419 RID: 1049
		// (set) Token: 0x0600041A RID: 1050
		public extern float frequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
