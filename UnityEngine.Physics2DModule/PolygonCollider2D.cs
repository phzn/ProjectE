using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000028 RID: 40
	[NativeHeader("Modules/Physics2D/Public/PolygonCollider2D.h")]
	public sealed class PolygonCollider2D : Collider2D
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003C7 RID: 967
		// (set) Token: 0x060003C8 RID: 968
		public extern bool useDelaunayMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003C9 RID: 969
		// (set) Token: 0x060003CA RID: 970
		public extern bool autoTiling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003CB RID: 971
		[NativeMethod("GetPointCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetTotalPointCount();

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003CC RID: 972
		// (set) Token: 0x060003CD RID: 973
		public extern Vector2[] points
		{
			[NativeMethod("GetPoints_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetPoints_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003CE RID: 974
		// (set) Token: 0x060003CF RID: 975
		public extern int pathCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00008420 File Offset: 0x00006620
		public Vector2[] GetPath(int index)
		{
			bool flag = index >= this.pathCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Path {0} does not exist.", index));
			}
			bool flag2 = index < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Path {0} does not exist; negative path index is invalid.", index));
			}
			return this.GetPath_Internal(index);
		}

		// Token: 0x060003D1 RID: 977
		[NativeMethod("GetPath_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Vector2[] GetPath_Internal(int index);

		// Token: 0x060003D2 RID: 978 RVA: 0x00008480 File Offset: 0x00006680
		public void SetPath(int index, Vector2[] points)
		{
			bool flag = index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Negative path index {0} is invalid.", index));
			}
			this.SetPath_Internal(index, points);
		}

		// Token: 0x060003D3 RID: 979
		[NativeMethod("SetPath_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPath_Internal(int index, [NotNull("ArgumentNullException")] Vector2[] points);

		// Token: 0x060003D4 RID: 980 RVA: 0x000084B8 File Offset: 0x000066B8
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

		// Token: 0x060003D5 RID: 981
		[NativeMethod("GetPathList_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPathList_Internal(int index, [NotNull("ArgumentNullException")] List<Vector2> points);

		// Token: 0x060003D6 RID: 982 RVA: 0x00008524 File Offset: 0x00006724
		public void SetPath(int index, List<Vector2> points)
		{
			bool flag = index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Negative path index {0} is invalid.", index));
			}
			this.SetPathList_Internal(index, points);
		}

		// Token: 0x060003D7 RID: 983
		[NativeMethod("SetPathList_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPathList_Internal(int index, [NotNull("ArgumentNullException")] List<Vector2> points);

		// Token: 0x060003D8 RID: 984 RVA: 0x00008559 File Offset: 0x00006759
		[ExcludeFromDocs]
		public void CreatePrimitive(int sides)
		{
			this.CreatePrimitive(sides, Vector2.one, Vector2.zero);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000856E File Offset: 0x0000676E
		[ExcludeFromDocs]
		public void CreatePrimitive(int sides, Vector2 scale)
		{
			this.CreatePrimitive(sides, scale, Vector2.zero);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00008580 File Offset: 0x00006780
		public void CreatePrimitive(int sides, [DefaultValue("Vector2.one")] Vector2 scale, [DefaultValue("Vector2.zero")] Vector2 offset)
		{
			bool flag = sides < 3;
			if (flag)
			{
				Debug.LogWarning("Cannot create a 2D polygon primitive collider with less than two sides.", this);
			}
			else
			{
				bool flag2 = scale.x <= 0f || scale.y <= 0f;
				if (flag2)
				{
					Debug.LogWarning("Cannot create a 2D polygon primitive collider with an axis scale less than or equal to zero.", this);
				}
				else
				{
					this.CreatePrimitive_Internal(sides, scale, offset, true);
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000085E3 File Offset: 0x000067E3
		[NativeMethod("CreatePrimitive")]
		private void CreatePrimitive_Internal(int sides, [DefaultValue("Vector2.one")] Vector2 scale, [DefaultValue("Vector2.zero")] Vector2 offset, bool autoRefresh)
		{
			this.CreatePrimitive_Internal_Injected(sides, ref scale, ref offset, autoRefresh);
		}

		// Token: 0x060003DD RID: 989
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CreatePrimitive_Internal_Injected(int sides, [DefaultValue("Vector2.one")] ref Vector2 scale, [DefaultValue("Vector2.zero")] ref Vector2 offset, bool autoRefresh);
	}
}
