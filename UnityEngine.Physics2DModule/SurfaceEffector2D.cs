using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003C RID: 60
	[NativeHeader("Modules/Physics2D/SurfaceEffector2D.h")]
	public class SurfaceEffector2D : Effector2D
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004D8 RID: 1240
		// (set) Token: 0x060004D9 RID: 1241
		public extern float speed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004DA RID: 1242
		// (set) Token: 0x060004DB RID: 1243
		public extern float speedVariation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004DC RID: 1244
		// (set) Token: 0x060004DD RID: 1245
		public extern float forceScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004DE RID: 1246
		// (set) Token: 0x060004DF RID: 1247
		public extern bool useContactForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004E0 RID: 1248
		// (set) Token: 0x060004E1 RID: 1249
		public extern bool useFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004E2 RID: 1250
		// (set) Token: 0x060004E3 RID: 1251
		public extern bool useBounce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
