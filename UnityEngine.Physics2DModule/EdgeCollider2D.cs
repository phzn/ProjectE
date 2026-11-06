using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000026 RID: 38
	[NativeHeader("Modules/Physics2D/Public/EdgeCollider2D.h")]
	public sealed class EdgeCollider2D : Collider2D
	{
		// Token: 0x060003A6 RID: 934
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003A7 RID: 935
		// (set) Token: 0x060003A8 RID: 936
		public extern float edgeRadius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003A9 RID: 937
		public extern int edgeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003AA RID: 938
		public extern int pointCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003AB RID: 939
		// (set) Token: 0x060003AC RID: 940
		public extern Vector2[] points
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003AD RID: 941
		[NativeMethod("GetPoints_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPoints([NotNull("ArgumentNullException")] List<Vector2> points);

		// Token: 0x060003AE RID: 942
		[NativeMethod("SetPoints_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetPoints([NotNull("ArgumentNullException")] List<Vector2> points);

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003AF RID: 943
		// (set) Token: 0x060003B0 RID: 944
		public extern bool useAdjacentStartPoint
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003B1 RID: 945
		// (set) Token: 0x060003B2 RID: 946
		public extern bool useAdjacentEndPoint
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000083A8 File Offset: 0x000065A8
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x000083BE File Offset: 0x000065BE
		public Vector2 adjacentStartPoint
		{
			get
			{
				Vector2 vector;
				this.get_adjacentStartPoint_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_adjacentStartPoint_Injected(ref value);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000083C8 File Offset: 0x000065C8
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x000083DE File Offset: 0x000065DE
		public Vector2 adjacentEndPoint
		{
			get
			{
				Vector2 vector;
				this.get_adjacentEndPoint_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_adjacentEndPoint_Injected(ref value);
			}
		}

		// Token: 0x060003B8 RID: 952
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_adjacentStartPoint_Injected(out Vector2 ret);

		// Token: 0x060003B9 RID: 953
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_adjacentStartPoint_Injected(ref Vector2 value);

		// Token: 0x060003BA RID: 954
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_adjacentEndPoint_Injected(out Vector2 ret);

		// Token: 0x060003BB RID: 955
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_adjacentEndPoint_Injected(ref Vector2 value);
	}
}
