using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000065 RID: 101
	public class GraphUpdateShape
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0001269A File Offset: 0x0001089A
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x000126A2 File Offset: 0x000108A2
		public Vector3[] points
		{
			get
			{
				return this._points;
			}
			set
			{
				this._points = value;
				if (this.convex)
				{
					this.CalculateConvexHull();
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000126B9 File Offset: 0x000108B9
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000126C1 File Offset: 0x000108C1
		public bool convex
		{
			get
			{
				return this._convex;
			}
			set
			{
				if (this._convex != value && value)
				{
					this.CalculateConvexHull();
				}
				this._convex = value;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000126E0 File Offset: 0x000108E0
		public GraphUpdateShape()
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001270C File Offset: 0x0001090C
		public GraphUpdateShape(Vector3[] points, bool convex, Matrix4x4 matrix, float minimumHeight)
		{
			this.convex = convex;
			this.points = points;
			this.origin = matrix.MultiplyPoint3x4(Vector3.zero);
			this.right = matrix.MultiplyPoint3x4(Vector3.right) - this.origin;
			this.up = matrix.MultiplyPoint3x4(Vector3.up) - this.origin;
			this.forward = matrix.MultiplyPoint3x4(Vector3.forward) - this.origin;
			this.minimumHeight = minimumHeight;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000127BF File Offset: 0x000109BF
		private void CalculateConvexHull()
		{
			this._convexPoints = ((this.points != null) ? Polygon.ConvexHullXZ(this.points) : null);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000127DD File Offset: 0x000109DD
		public Bounds GetBounds()
		{
			return GraphUpdateShape.GetBounds(this.convex ? this._convexPoints : this.points, this.right, this.up, this.forward, this.origin, this.minimumHeight);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00012818 File Offset: 0x00010A18
		public static Bounds GetBounds(Vector3[] points, Matrix4x4 matrix, float minimumHeight)
		{
			Vector3 vector = matrix.MultiplyPoint3x4(Vector3.zero);
			Vector3 vector2 = matrix.MultiplyPoint3x4(Vector3.right) - vector;
			Vector3 vector3 = matrix.MultiplyPoint3x4(Vector3.up) - vector;
			Vector3 vector4 = matrix.MultiplyPoint3x4(Vector3.forward) - vector;
			return GraphUpdateShape.GetBounds(points, vector2, vector3, vector4, vector, minimumHeight);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00012878 File Offset: 0x00010A78
		private static Bounds GetBounds(Vector3[] points, Vector3 right, Vector3 up, Vector3 forward, Vector3 origin, float minimumHeight)
		{
			if (points == null || points.Length == 0)
			{
				return default(Bounds);
			}
			float num = points[0].y;
			float num2 = points[0].y;
			for (int i = 0; i < points.Length; i++)
			{
				num = Mathf.Min(num, points[i].y);
				num2 = Mathf.Max(num2, points[i].y);
			}
			float num3 = Mathf.Max(minimumHeight - (num2 - num), 0f) * 0.5f;
			num -= num3;
			num2 += num3;
			Vector3 vector = right * points[0].x + up * points[0].y + forward * points[0].z;
			Vector3 vector2 = vector;
			for (int j = 0; j < points.Length; j++)
			{
				Vector3 vector3 = right * points[j].x + forward * points[j].z;
				Vector3 vector4 = vector3 + up * num;
				Vector3 vector5 = vector3 + up * num2;
				vector = Vector3.Min(vector, vector4);
				vector = Vector3.Min(vector, vector5);
				vector2 = Vector3.Max(vector2, vector4);
				vector2 = Vector3.Max(vector2, vector5);
			}
			return new Bounds((vector + vector2) * 0.5f + origin, vector2 - vector);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x000129F6 File Offset: 0x00010BF6
		public bool Contains(GraphNode node)
		{
			return this.Contains((Vector3)node.position);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00012A0C File Offset: 0x00010C0C
		public bool Contains(Vector3 point)
		{
			point -= this.origin;
			Vector3 vector = new Vector3(Vector3.Dot(point, this.right) / this.right.sqrMagnitude, 0f, Vector3.Dot(point, this.forward) / this.forward.sqrMagnitude);
			if (!this.convex)
			{
				return this._points != null && Polygon.ContainsPointXZ(this._points, vector);
			}
			if (this._convexPoints == null)
			{
				return false;
			}
			int i = 0;
			int num = this._convexPoints.Length - 1;
			while (i < this._convexPoints.Length)
			{
				if (VectorMath.RightOrColinearXZ(this._convexPoints[i], this._convexPoints[num], vector))
				{
					return false;
				}
				num = i;
				i++;
			}
			return true;
		}

		// Token: 0x04000239 RID: 569
		private Vector3[] _points;

		// Token: 0x0400023A RID: 570
		private Vector3[] _convexPoints;

		// Token: 0x0400023B RID: 571
		private bool _convex;

		// Token: 0x0400023C RID: 572
		private Vector3 right = Vector3.right;

		// Token: 0x0400023D RID: 573
		private Vector3 forward = Vector3.forward;

		// Token: 0x0400023E RID: 574
		private Vector3 up = Vector3.up;

		// Token: 0x0400023F RID: 575
		private Vector3 origin;

		// Token: 0x04000240 RID: 576
		public float minimumHeight;

		// Token: 0x02000066 RID: 102
		public struct BurstShape
		{
			// Token: 0x060003FC RID: 1020 RVA: 0x00012AD0 File Offset: 0x00010CD0
			public BurstShape(GraphUpdateShape scene, Allocator allocator)
			{
				Vector3[] array = (scene.convex ? scene._convexPoints : scene._points);
				if (array == null)
				{
					this.points = new NativeArray<Vector3>(0, allocator, NativeArrayOptions.ClearMemory);
				}
				else
				{
					this.points = new NativeArray<Vector3>(array, allocator);
				}
				this.origin = scene.origin;
				this.right = scene.right;
				this.forward = scene.forward;
				float num = scene.right.sqrMagnitude;
				if (num > 0f)
				{
					this.right /= num;
				}
				num = scene.forward.sqrMagnitude;
				if (num > 0f)
				{
					this.forward /= num;
				}
				this.containsEverything = false;
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060003FD RID: 1021 RVA: 0x00012B9C File Offset: 0x00010D9C
			public static GraphUpdateShape.BurstShape Everything
			{
				get
				{
					return new GraphUpdateShape.BurstShape
					{
						points = new NativeArray<Vector3>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory),
						origin = float3.zero,
						right = float3.zero,
						forward = float3.zero,
						containsEverything = true
					};
				}
			}

			// Token: 0x060003FE RID: 1022 RVA: 0x00012BF0 File Offset: 0x00010DF0
			public bool Contains(float3 point)
			{
				if (this.containsEverything)
				{
					return true;
				}
				point -= this.origin;
				float3 @float = new float3(math.dot(point, this.right), 0f, math.dot(point, this.forward));
				int num = this.points.Length - 1;
				bool flag = false;
				int i = 0;
				while (i < this.points.Length)
				{
					if (((this.points[i].z <= @float.z && @float.z < this.points[num].z) || (this.points[num].z <= @float.z && @float.z < this.points[i].z)) && @float.x < (this.points[num].x - this.points[i].x) * (@float.z - this.points[i].z) / (this.points[num].z - this.points[i].z) + this.points[i].x)
					{
						flag = !flag;
					}
					num = i++;
				}
				return flag;
			}

			// Token: 0x04000241 RID: 577
			[DeallocateOnJobCompletion]
			private NativeArray<Vector3> points;

			// Token: 0x04000242 RID: 578
			private float3 origin;

			// Token: 0x04000243 RID: 579
			private float3 right;

			// Token: 0x04000244 RID: 580
			private float3 forward;

			// Token: 0x04000245 RID: 581
			private bool containsEverything;
		}
	}
}
