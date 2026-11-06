using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003B RID: 59
	[NativeHeader("Modules/Physics2D/PlatformEffector2D.h")]
	public class PlatformEffector2D : Effector2D
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004C1 RID: 1217
		// (set) Token: 0x060004C2 RID: 1218
		public extern bool useOneWay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004C3 RID: 1219
		// (set) Token: 0x060004C4 RID: 1220
		public extern bool useOneWayGrouping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004C5 RID: 1221
		// (set) Token: 0x060004C6 RID: 1222
		public extern bool useSideFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004C7 RID: 1223
		// (set) Token: 0x060004C8 RID: 1224
		public extern bool useSideBounce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004C9 RID: 1225
		// (set) Token: 0x060004CA RID: 1226
		public extern float surfaceArc
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004CB RID: 1227
		// (set) Token: 0x060004CC RID: 1228
		public extern float sideArc
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004CD RID: 1229
		// (set) Token: 0x060004CE RID: 1230
		public extern float rotationalOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0000892C File Offset: 0x00006B2C
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00008944 File Offset: 0x00006B44
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("PlatformEffector2D.oneWay has been deprecated. Use PlatformEffector2D.useOneWay instead (UnityUpgradable) -> useOneWay", true)]
		public bool oneWay
		{
			get
			{
				return this.useOneWay;
			}
			set
			{
				this.useOneWay = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00008950 File Offset: 0x00006B50
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00008968 File Offset: 0x00006B68
		[Obsolete("PlatformEffector2D.sideFriction has been deprecated. Use PlatformEffector2D.useSideFriction instead (UnityUpgradable) -> useSideFriction", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool sideFriction
		{
			get
			{
				return this.useSideFriction;
			}
			set
			{
				this.useSideFriction = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00008974 File Offset: 0x00006B74
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0000898C File Offset: 0x00006B8C
		[Obsolete("PlatformEffector2D.sideBounce has been deprecated. Use PlatformEffector2D.useSideBounce instead (UnityUpgradable) -> useSideBounce", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool sideBounce
		{
			get
			{
				return this.useSideBounce;
			}
			set
			{
				this.useSideBounce = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00008998 File Offset: 0x00006B98
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x000089B0 File Offset: 0x00006BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("PlatformEffector2D.sideAngleVariance has been deprecated. Use PlatformEffector2D.sideArc instead (UnityUpgradable) -> sideArc", true)]
		public float sideAngleVariance
		{
			get
			{
				return this.sideArc;
			}
			set
			{
				this.sideArc = value;
			}
		}
	}
}
