using System;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200029E RID: 670
	public class GraphTransform : IMovementPlane, ITransform
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00064C06 File Offset: 0x00062E06
		public bool identity
		{
			get
			{
				return this.isIdentity;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x00064C0E File Offset: 0x00062E0E
		public bool onlyTranslational
		{
			get
			{
				return this.isOnlyTranslational;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00064C16 File Offset: 0x00062E16
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00064C1E File Offset: 0x00062E1E
		public Matrix4x4 matrix { get; private set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00064C27 File Offset: 0x00062E27
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x00064C2F File Offset: 0x00062E2F
		public Matrix4x4 inverseMatrix { get; private set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00064C38 File Offset: 0x00062E38
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00064C40 File Offset: 0x00062E40
		public Quaternion rotation { get; private set; }

		// Token: 0x060010B9 RID: 4281 RVA: 0x00064C49 File Offset: 0x00062E49
		public GraphTransform(Matrix4x4 matrix)
		{
			this.Set(matrix);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00064C58 File Offset: 0x00062E58
		protected void Set(Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.inverseMatrix = matrix.inverse;
			this.isIdentity = matrix.isIdentity;
			this.isOnlyTranslational = GraphTransform.MatrixIsTranslational(matrix);
			this.up = matrix.MultiplyVector(Vector3.up).normalized;
			this.translation = matrix.MultiplyPoint3x4(Vector3.zero);
			this.i3translation = (Int3)this.translation;
			this.rotation = Quaternion.LookRotation(this.TransformVector(Vector3.forward), this.TransformVector(Vector3.up));
			this.inverseRotation = Quaternion.Inverse(this.rotation);
			this.isXY = this.rotation == Quaternion.Euler(-90f, 0f, 0f);
			this.isXZ = this.rotation == Quaternion.Euler(0f, 0f, 0f);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00064D4B File Offset: 0x00062F4B
		public Vector3 WorldUpAtGraphPosition(Vector3 point)
		{
			return this.up;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00064D54 File Offset: 0x00062F54
		private static bool MatrixIsTranslational(Matrix4x4 matrix)
		{
			return matrix.GetColumn(0) == new Vector4(1f, 0f, 0f, 0f) && matrix.GetColumn(1) == new Vector4(0f, 1f, 0f, 0f) && matrix.GetColumn(2) == new Vector4(0f, 0f, 1f, 0f) && matrix.m33 == 1f;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00064DE8 File Offset: 0x00062FE8
		public Vector3 Transform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point + this.translation;
			}
			return this.matrix.MultiplyPoint3x4(point);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00064E1C File Offset: 0x0006301C
		public Vector3 TransformVector(Vector3 dir)
		{
			if (this.onlyTranslational)
			{
				return dir;
			}
			return this.matrix.MultiplyVector(dir);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00064E44 File Offset: 0x00063044
		public void Transform(Int3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.i3translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = (Int3)this.matrix.MultiplyPoint3x4((Vector3)arr[j]);
			}
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00064EC0 File Offset: 0x000630C0
		public unsafe void Transform(UnsafeSpan<Int3> arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					*arr[i] += this.i3translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				*arr[j] = (Int3)this.matrix.MultiplyPoint3x4((Vector3)(*arr[j]));
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00064F50 File Offset: 0x00063150
		public void Transform(Vector3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = this.matrix.MultiplyPoint3x4(arr[j]);
			}
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00064FC0 File Offset: 0x000631C0
		public Vector3 InverseTransform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.translation;
			}
			return this.inverseMatrix.MultiplyPoint3x4(point);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00064FF4 File Offset: 0x000631F4
		public Vector3 InverseTransformVector(Vector3 dir)
		{
			if (this.onlyTranslational)
			{
				return dir;
			}
			return this.inverseMatrix.MultiplyVector(dir);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0006501C File Offset: 0x0006321C
		public Int3 InverseTransform(Int3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.i3translation;
			}
			return (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)point);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00065058 File Offset: 0x00063258
		public void InverseTransform(Int3[] arr)
		{
			for (int i = arr.Length - 1; i >= 0; i--)
			{
				arr[i] = (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)arr[i]);
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0006509C File Offset: 0x0006329C
		public unsafe void InverseTransform(UnsafeSpan<Int3> arr)
		{
			for (int i = arr.Length - 1; i >= 0; i--)
			{
				*arr[i] = (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)(*arr[i]));
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x000650EF File Offset: 0x000632EF
		public static GraphTransform operator *(GraphTransform lhs, Matrix4x4 rhs)
		{
			return new GraphTransform(lhs.matrix * rhs);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00065102 File Offset: 0x00063302
		public static GraphTransform operator *(Matrix4x4 lhs, GraphTransform rhs)
		{
			return new GraphTransform(lhs * rhs.matrix);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00065118 File Offset: 0x00063318
		public Bounds Transform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center + this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0006534C File Offset: 0x0006354C
		public Bounds InverseTransform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center - this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00065580 File Offset: 0x00063780
		Vector2 IMovementPlane.ToPlane(Vector3 point)
		{
			if (this.isXY)
			{
				return new Vector2(point.x, point.y);
			}
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			return new Vector2(point.x, point.z);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000655CE File Offset: 0x000637CE
		Vector2 IMovementPlane.ToPlane(Vector3 point, out float elevation)
		{
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			elevation = point.y;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000655FF File Offset: 0x000637FF
		Vector3 IMovementPlane.ToWorld(Vector2 point, float elevation)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0006561E File Offset: 0x0006381E
		public SimpleMovementPlane ToSimpleMovementPlane()
		{
			return new SimpleMovementPlane(this.rotation);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0006562C File Offset: 0x0006382C
		public void CopyTo(MutableGraphTransform graphTransform)
		{
			graphTransform.isXY = this.isXY;
			graphTransform.isXZ = this.isXZ;
			graphTransform.isOnlyTranslational = this.isOnlyTranslational;
			graphTransform.isIdentity = this.isIdentity;
			graphTransform.matrix = this.matrix;
			graphTransform.inverseMatrix = this.inverseMatrix;
			graphTransform.up = this.up;
			graphTransform.translation = this.translation;
			graphTransform.i3translation = this.i3translation;
			graphTransform.rotation = this.rotation;
			graphTransform.inverseRotation = this.inverseRotation;
		}

		// Token: 0x04000BFE RID: 3070
		private bool isXY;

		// Token: 0x04000BFF RID: 3071
		private bool isXZ;

		// Token: 0x04000C00 RID: 3072
		private bool isOnlyTranslational;

		// Token: 0x04000C01 RID: 3073
		private bool isIdentity;

		// Token: 0x04000C04 RID: 3076
		private Vector3 up;

		// Token: 0x04000C05 RID: 3077
		private Vector3 translation;

		// Token: 0x04000C06 RID: 3078
		private Int3 i3translation;

		// Token: 0x04000C08 RID: 3080
		private Quaternion inverseRotation;

		// Token: 0x04000C09 RID: 3081
		public static readonly GraphTransform identityTransform = new GraphTransform(Matrix4x4.identity);

		// Token: 0x04000C0A RID: 3082
		public static readonly GraphTransform xyPlane = new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f), Vector3.one));

		// Token: 0x04000C0B RID: 3083
		public static readonly GraphTransform xzPlane = new GraphTransform(Matrix4x4.identity);
	}
}
