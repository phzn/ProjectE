using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000038 RID: 56
	[NativeHeader("Modules/Physics2D/AreaEffector2D.h")]
	public class AreaEffector2D : Effector2D
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000490 RID: 1168
		// (set) Token: 0x06000491 RID: 1169
		public extern float forceAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000492 RID: 1170
		// (set) Token: 0x06000493 RID: 1171
		public extern bool useGlobalAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000494 RID: 1172
		// (set) Token: 0x06000495 RID: 1173
		public extern float forceMagnitude
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000496 RID: 1174
		// (set) Token: 0x06000497 RID: 1175
		public extern float forceVariation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000498 RID: 1176
		// (set) Token: 0x06000499 RID: 1177
		public extern float drag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600049A RID: 1178
		// (set) Token: 0x0600049B RID: 1179
		public extern float angularDrag
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600049C RID: 1180
		// (set) Token: 0x0600049D RID: 1181
		public extern EffectorSelection2D forceTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00008900 File Offset: 0x00006B00
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x00008918 File Offset: 0x00006B18
		[Obsolete("AreaEffector2D.forceDirection has been deprecated. Use AreaEffector2D.forceAngle instead (UnityUpgradable) -> forceAngle", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float forceDirection
		{
			get
			{
				return this.forceAngle;
			}
			set
			{
				this.forceAngle = value;
			}
		}
	}
}
