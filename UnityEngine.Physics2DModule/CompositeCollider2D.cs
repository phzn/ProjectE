using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000029 RID: 41
	[RequireComponent(typeof(Rigidbody2D))]
	[NativeHeader("Modules/Physics2D/Public/CompositeCollider2D.h")]
	public sealed class CompositeCollider2D : Collider2D
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003DE RID: 990
		// (set) Token: 0x060003DF RID: 991
		public extern CompositeCollider2D.GeometryType geometryType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003E0 RID: 992
		// (set) Token: 0x060003E1 RID: 993
		public extern CompositeCollider2D.GenerationType generationType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003E2 RID: 994
		// (set) Token: 0x060003E3 RID: 995
		public extern bool useDelaunayMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003E4 RID: 996
		// (set) Token: 0x060003E5 RID: 997
		public extern float vertexDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003E6 RID: 998
		// (set) Token: 0x060003E7 RID: 999
		public extern float edgeRadius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003E8 RID: 1000
		// (set) Token: 0x060003E9 RID: 1001
		public extern float offsetDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003EA RID: 1002
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GenerateGeometry();

		// Token: 0x060003EB RID: 1003 RVA: 0x000085F4 File Offset: 0x000067F4
		public int GetPathPointCount(int index)
		{
			int num = this.pathCount - 1;
			bool flag = index < 0 || index > num;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Path index {0} must be in the range of 0 to {1}.", index, num));
			}
			return this.GetPathPointCount_Internal(index);
		}

		// Token: 0x060003EC RID: 1004
		[NativeMethod("GetPathPointCount_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPathPointCount_Internal(int index);

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003ED RID: 1005
		public extern int pathCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003EE RID: 1006
		public extern int pointCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00008648 File Offset: 0x00006848
		public int GetPath(int index, Vector2[] points)
		{
			bool flag = index < 0 || index >= this.pathCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Path index {0} must be in the range of 0 to {1}.", index, this.pathCount - 1));
			}
			bool flag2 = points == null;
			if (flag2)
			{
				throw new ArgumentNullException("points");
			}
			return this.GetPathArray_Internal(index, points);
		}

		// Token: 0x060003F0 RID: 1008
		[NativeMethod("GetPathArray_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPathArray_Internal(int index, [NotNull("ArgumentNullException")] [Unmarshalled] Vector2[] points);

		// Token: 0x060003F1 RID: 1009 RVA: 0x000086B4 File Offset: 0x000068B4
		public int GetPath(int index, List<Vector2> points)
		{
			bool flag = index < 0 || index >= this.pathCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Path index {0} must be in the range of 0 to {1}.", index, this.pathCount - 1));
			}
			bool flag2 = points == null;
			if (flag2)
			{
				throw new ArgumentNullException("points");
			}
			return this.GetPathList_Internal(index, points);
		}

		// Token: 0x060003F2 RID: 1010
		[NativeMethod("GetPathList_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPathList_Internal(int index, [NotNull("ArgumentNullException")] List<Vector2> points);

		// Token: 0x0200002A RID: 42
		public enum GeometryType
		{
			// Token: 0x040000A0 RID: 160
			Outlines,
			// Token: 0x040000A1 RID: 161
			Polygons
		}

		// Token: 0x0200002B RID: 43
		public enum GenerationType
		{
			// Token: 0x040000A3 RID: 163
			Synchronous,
			// Token: 0x040000A4 RID: 164
			Manual
		}
	}
}
