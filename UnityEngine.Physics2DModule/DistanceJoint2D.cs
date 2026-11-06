using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002F RID: 47
	[NativeHeader("Modules/Physics2D/DistanceJoint2D.h")]
	public sealed class DistanceJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600041C RID: 1052
		// (set) Token: 0x0600041D RID: 1053
		public extern bool autoConfigureDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600041E RID: 1054
		// (set) Token: 0x0600041F RID: 1055
		public extern float distance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000420 RID: 1056
		// (set) Token: 0x06000421 RID: 1057
		public extern bool maxDistanceOnly
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
