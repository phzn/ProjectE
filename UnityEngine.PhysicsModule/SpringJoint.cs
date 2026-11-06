using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000032 RID: 50
	[NativeClass("Unity::SpringJoint")]
	[NativeHeader("Modules/Physics/SpringJoint.h")]
	public class SpringJoint : Joint
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003CE RID: 974
		// (set) Token: 0x060003CF RID: 975
		public extern float spring
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003D0 RID: 976
		// (set) Token: 0x060003D1 RID: 977
		public extern float damper
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003D2 RID: 978
		// (set) Token: 0x060003D3 RID: 979
		public extern float minDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003D4 RID: 980
		// (set) Token: 0x060003D5 RID: 981
		public extern float maxDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003D6 RID: 982
		// (set) Token: 0x060003D7 RID: 983
		public extern float tolerance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
