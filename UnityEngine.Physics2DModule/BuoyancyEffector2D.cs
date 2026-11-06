using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000039 RID: 57
	[NativeHeader("Modules/Physics2D/BuoyancyEffector2D.h")]
	public class BuoyancyEffector2D : Effector2D
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004A1 RID: 1185
		// (set) Token: 0x060004A2 RID: 1186
		public extern float surfaceLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004A3 RID: 1187
		// (set) Token: 0x060004A4 RID: 1188
		public extern float density
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004A5 RID: 1189
		// (set) Token: 0x060004A6 RID: 1190
		public extern float linearDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004A7 RID: 1191
		// (set) Token: 0x060004A8 RID: 1192
		public extern float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004A9 RID: 1193
		// (set) Token: 0x060004AA RID: 1194
		public extern float flowAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004AB RID: 1195
		// (set) Token: 0x060004AC RID: 1196
		public extern float flowMagnitude
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004AD RID: 1197
		// (set) Token: 0x060004AE RID: 1198
		public extern float flowVariation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
