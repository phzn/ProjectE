using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000035 RID: 53
	[NativeHeader("Modules/Physics2D/FixedJoint2D.h")]
	public sealed class FixedJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000472 RID: 1138
		// (set) Token: 0x06000473 RID: 1139
		public extern float dampingRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000474 RID: 1140
		// (set) Token: 0x06000475 RID: 1141
		public extern float frequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000476 RID: 1142
		public extern float referenceAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
