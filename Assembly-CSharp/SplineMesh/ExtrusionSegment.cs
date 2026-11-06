using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SplineMesh
{
	// Token: 0x02000192 RID: 402
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class ExtrusionSegment : MonoBehaviour
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00055AB1 File Offset: 0x00053CB1
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x00055AB9 File Offset: 0x00053CB9
		public List<ExtrusionSegment.Vertex> ShapeVertices
		{
			get
			{
				return this.shapeVertices;
			}
			set
			{
				if (value == this.shapeVertices)
				{
					return;
				}
				this.SetDirty();
				this.shapeVertices = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00055AD2 File Offset: 0x00053CD2
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x00055ADA File Offset: 0x00053CDA
		public float TextureScale
		{
			get
			{
				return this.textureScale;
			}
			set
			{
				if (value == this.textureScale)
				{
					return;
				}
				this.SetDirty();
				this.textureScale = value;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00055AF3 File Offset: 0x00053CF3
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x00055AFB File Offset: 0x00053CFB
		public float TextureOffset
		{
			get
			{
				return this.textureOffset;
			}
			set
			{
				if (value == this.textureOffset)
				{
					return;
				}
				this.SetDirty();
				this.textureOffset = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00055B14 File Offset: 0x00053D14
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x00055B1C File Offset: 0x00053D1C
		public float SampleSpacing
		{
			get
			{
				return this.sampleSpacing;
			}
			set
			{
				if (value == this.sampleSpacing)
				{
					return;
				}
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("SampleSpacing", "Must be greater than 0");
				}
				this.SetDirty();
				this.sampleSpacing = value;
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00055B4D File Offset: 0x00053D4D
		private void OnEnable()
		{
			this.mf = base.GetComponent<MeshFilter>();
			if (this.mf.sharedMesh == null)
			{
				this.mf.sharedMesh = new Mesh();
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00055B80 File Offset: 0x00053D80
		public void SetInterval(CubicBezierCurve curve)
		{
			if (this.curve == curve)
			{
				return;
			}
			if (curve == null)
			{
				throw new ArgumentNullException("curve");
			}
			if (this.curve != null)
			{
				this.curve.Changed.RemoveListener(new UnityAction(this.SetDirty));
			}
			this.curve = curve;
			this.spline = null;
			curve.Changed.AddListener(new UnityAction(this.SetDirty));
			this.useSpline = false;
			this.SetDirty();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00055BFC File Offset: 0x00053DFC
		public void SetInterval(Spline spline, float intervalStart, float intervalEnd = 0f)
		{
			if (this.spline == spline && this.intervalStart == intervalStart && this.intervalEnd == intervalEnd)
			{
				return;
			}
			if (spline == null)
			{
				throw new ArgumentNullException("spline");
			}
			if (intervalStart < 0f || intervalStart >= spline.Length)
			{
				throw new ArgumentOutOfRangeException("interval start must be 0 or greater and lesser than spline length (was " + intervalStart.ToString() + ")");
			}
			if ((intervalEnd != 0f && intervalEnd <= intervalStart) || intervalEnd > spline.Length)
			{
				throw new ArgumentOutOfRangeException("interval end must be 0 or greater than interval start, and lesser than spline length (was " + intervalEnd.ToString() + ")");
			}
			if (this.spline != null)
			{
				this.spline.CurveChanged.RemoveListener(new UnityAction(this.SetDirty));
			}
			this.spline = spline;
			spline.CurveChanged.AddListener(new UnityAction(this.SetDirty));
			this.curve = null;
			this.intervalStart = intervalStart;
			this.intervalEnd = intervalEnd;
			this.useSpline = true;
			this.SetDirty();
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00055D06 File Offset: 0x00053F06
		private void SetDirty()
		{
			this.isDirty = true;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00055D0F File Offset: 0x00053F0F
		private void LateUpdate()
		{
			this.ComputeIfNeeded();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00055D17 File Offset: 0x00053F17
		public void ComputeIfNeeded()
		{
			if (this.isDirty)
			{
				this.Compute();
				this.isDirty = false;
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00055D30 File Offset: 0x00053F30
		private List<CurveSample> GetPath()
		{
			List<CurveSample> list = new List<CurveSample>();
			if (this.useSpline)
			{
				for (float num = this.intervalStart; num < this.intervalEnd; num += this.sampleSpacing)
				{
					list.Add(this.spline.GetSampleAtDistance(num));
				}
				list.Add(this.spline.GetSampleAtDistance(this.intervalEnd));
			}
			else
			{
				for (float num2 = 0f; num2 < this.curve.Length; num2 += this.sampleSpacing)
				{
					list.Add(this.curve.GetSampleAtDistance(num2));
				}
				list.Add(this.curve.GetSampleAtDistance(this.curve.Length));
			}
			return list;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00055DE0 File Offset: 0x00053FE0
		public void Compute()
		{
			List<CurveSample> path = this.GetPath();
			int count = this.shapeVertices.Count;
			int num = path.Count - 1;
			List<int> list = new List<int>(count * 2 * num * 3);
			List<MeshVertex> list2 = new List<MeshVertex>(count * 2 * num * 3);
			foreach (CurveSample curveSample in path)
			{
				foreach (ExtrusionSegment.Vertex vertex in this.shapeVertices)
				{
					list2.Add(curveSample.GetBent(new MeshVertex(new Vector3(0f, vertex.point.y, -vertex.point.x), new Vector3(0f, vertex.normal.y, -vertex.normal.x), new Vector2(vertex.uCoord, this.textureScale * (curveSample.distanceInCurve + this.textureOffset)))));
				}
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < this.shapeVertices.Count; j++)
				{
					int num3 = ((j == this.shapeVertices.Count - 1) ? (-(this.shapeVertices.Count - 1)) : 1);
					int num4 = num2 + this.shapeVertices.Count;
					int num5 = num2;
					int num6 = num2 + num3;
					int num7 = num2 + num3 + this.shapeVertices.Count;
					list.Add(num6);
					list.Add(num5);
					list.Add(num4);
					list.Add(num4);
					list.Add(num7);
					list.Add(num6);
					num2++;
				}
			}
			MeshUtility.Update(this.mf.sharedMesh, this.mf.sharedMesh, list, list2.Select<MeshVertex, Vector3>((MeshVertex b) => b.position), list2.Select<MeshVertex, Vector3>((MeshVertex b) => b.normal), list2.Select<MeshVertex, Vector2>((MeshVertex b) => b.uv), null, null, null, null, null, null, null);
			MeshCollider component = base.GetComponent<MeshCollider>();
			if (component != null)
			{
				component.sharedMesh = this.mf.sharedMesh;
			}
		}

		// Token: 0x04000C67 RID: 3175
		private bool isDirty;

		// Token: 0x04000C68 RID: 3176
		private MeshFilter mf;

		// Token: 0x04000C69 RID: 3177
		private Mesh result;

		// Token: 0x04000C6A RID: 3178
		private bool useSpline;

		// Token: 0x04000C6B RID: 3179
		private CubicBezierCurve curve;

		// Token: 0x04000C6C RID: 3180
		private Spline spline;

		// Token: 0x04000C6D RID: 3181
		private float intervalStart;

		// Token: 0x04000C6E RID: 3182
		private float intervalEnd;

		// Token: 0x04000C6F RID: 3183
		private List<ExtrusionSegment.Vertex> shapeVertices = new List<ExtrusionSegment.Vertex>();

		// Token: 0x04000C70 RID: 3184
		private float textureScale = 1f;

		// Token: 0x04000C71 RID: 3185
		private float textureOffset;

		// Token: 0x04000C72 RID: 3186
		private float sampleSpacing = 0.1f;

		// Token: 0x02000193 RID: 403
		[Serializable]
		public class Vertex
		{
			// Token: 0x06000C80 RID: 3200 RVA: 0x000560C1 File Offset: 0x000542C1
			public Vertex(Vector2 point, Vector2 normal, float uCoord)
			{
				this.point = point;
				this.normal = normal;
				this.uCoord = uCoord;
			}

			// Token: 0x06000C81 RID: 3201 RVA: 0x000560DE File Offset: 0x000542DE
			public Vertex(ExtrusionSegment.Vertex other)
			{
				this.point = other.point;
				this.normal = other.normal;
				this.uCoord = other.uCoord;
			}

			// Token: 0x04000C73 RID: 3187
			public Vector2 point;

			// Token: 0x04000C74 RID: 3188
			public Vector2 normal;

			// Token: 0x04000C75 RID: 3189
			public float uCoord;
		}
	}
}
