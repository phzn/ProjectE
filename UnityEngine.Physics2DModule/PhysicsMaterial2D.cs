using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003F RID: 63
	[NativeHeader("Modules/Physics2D/Public/PhysicsMaterial2D.h")]
	public sealed class PhysicsMaterial2D : Object
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x00008A05 File Offset: 0x00006C05
		public PhysicsMaterial2D()
		{
			PhysicsMaterial2D.Create_Internal(this, null);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00008A17 File Offset: 0x00006C17
		public PhysicsMaterial2D(string name)
		{
			PhysicsMaterial2D.Create_Internal(this, name);
		}

		// Token: 0x060004F3 RID: 1267
		[NativeMethod("Create_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Create_Internal([Writable] PhysicsMaterial2D scriptMaterial, string name);

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004F4 RID: 1268
		// (set) Token: 0x060004F5 RID: 1269
		public extern float bounciness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004F6 RID: 1270
		// (set) Token: 0x060004F7 RID: 1271
		public extern float friction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
