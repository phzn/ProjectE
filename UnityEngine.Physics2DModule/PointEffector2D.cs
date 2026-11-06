using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003A RID: 58
	[NativeHeader("Modules/Physics2D/PointEffector2D.h")]
	public class PointEffector2D : Effector2D
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004B0 RID: 1200
		// (set) Token: 0x060004B1 RID: 1201
		public extern float forceMagnitude
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004B2 RID: 1202
		// (set) Token: 0x060004B3 RID: 1203
		public extern float forceVariation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004B4 RID: 1204
		// (set) Token: 0x060004B5 RID: 1205
		public extern float distanceScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004B6 RID: 1206
		// (set) Token: 0x060004B7 RID: 1207
		public extern float drag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004B8 RID: 1208
		// (set) Token: 0x060004B9 RID: 1209
		public extern float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004BA RID: 1210
		// (set) Token: 0x060004BB RID: 1211
		public extern EffectorSelection2D forceSource
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004BC RID: 1212
		// (set) Token: 0x060004BD RID: 1213
		public extern EffectorSelection2D forceTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004BE RID: 1214
		// (set) Token: 0x060004BF RID: 1215
		public extern EffectorForceMode2D forceMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
