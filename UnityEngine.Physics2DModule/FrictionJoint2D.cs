using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000030 RID: 48
	[NativeHeader("Modules/Physics2D/FrictionJoint2D.h")]
	public sealed class FrictionJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000423 RID: 1059
		// (set) Token: 0x06000424 RID: 1060
		public extern float maxForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000425 RID: 1061
		// (set) Token: 0x06000426 RID: 1062
		public extern float maxTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
