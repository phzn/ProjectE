using System;
using Unity.Mathematics;
using UnityEngine.Splines.ExtrusionShapes;

namespace UnityEngine.Splines
{
	// Token: 0x02000030 RID: 48
	public struct ExtrudeSettings<T> where T : IExtrudeShape
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000AFB0 File Offset: 0x000091B0
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public int SegmentCount
		{
			get
			{
				return this.m_SegmentCount;
			}
			set
			{
				this.m_SegmentCount = math.clamp(value, 2, 4096);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000AFCC File Offset: 0x000091CC
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000AFD4 File Offset: 0x000091D4
		public bool CapEnds
		{
			get
			{
				return this.m_CapEnds;
			}
			set
			{
				this.m_CapEnds = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000AFDD File Offset: 0x000091DD
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000AFE5 File Offset: 0x000091E5
		public bool FlipNormals
		{
			get
			{
				return this.m_FlipNormals;
			}
			set
			{
				this.m_FlipNormals = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000AFEE File Offset: 0x000091EE
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000AFFC File Offset: 0x000091FC
		public float2 Range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = math.clamp(new float2(math.min(value.x, value.y), math.max(value.x, value.y)), 0f, 1f);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000B054 File Offset: 0x00009254
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000B05C File Offset: 0x0000925C
		public float Radius
		{
			get
			{
				return this.m_Radius;
			}
			set
			{
				this.m_Radius = math.clamp(value, 1E-05f, 10000f);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000B074 File Offset: 0x00009274
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000B07C File Offset: 0x0000927C
		public T Shape
		{
			get
			{
				return this.m_Shape;
			}
			set
			{
				this.m_Shape = value;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B085 File Offset: 0x00009285
		internal bool DoCapEnds<K>(K spline) where K : ISpline
		{
			return this.m_CapEnds && !spline.Closed;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000B0A1 File Offset: 0x000092A1
		internal bool DoCloseSpline<K>(K spline) where K : ISpline
		{
			return math.abs(1f - (this.Range.y - this.Range.x)) < float.Epsilon && spline.Closed;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000B0DC File Offset: 0x000092DC
		internal int sides
		{
			get
			{
				if (this.Shape is SplineShape)
				{
					T t;
					if (!this.wrapped)
					{
						t = this.Shape;
						return t.SideCount;
					}
					t = this.Shape;
					return t.SideCount + 1;
				}
				else
				{
					T t;
					if (!this.wrapped)
					{
						t = this.Shape;
						return t.SideCount + 1;
					}
					t = this.Shape;
					return t.SideCount;
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000B164 File Offset: 0x00009364
		internal bool wrapped
		{
			get
			{
				SplineShape splineShape = this.Shape as SplineShape;
				if (splineShape != null && splineShape.Spline != null)
				{
					return splineShape.Spline.Closed;
				}
				return !(this.Shape is Road);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000B1AE File Offset: 0x000093AE
		public ExtrudeSettings(T shape)
		{
			this = new ExtrudeSettings<T>(16, false, new float2(0f, 1f), 0.5f, shape);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000B1D0 File Offset: 0x000093D0
		public ExtrudeSettings(int segments, bool capped, float2 range, float radius, T shape)
		{
			this.m_SegmentCount = math.clamp(segments, 2, 4096);
			this.m_FlipNormals = false;
			this.m_Range = math.clamp(new float2(math.min(range.x, range.y), math.max(range.x, range.y)), 0f, 1f);
			this.m_CapEnds = capped;
			this.m_Radius = math.clamp(radius, 1E-05f, 10000f);
			this.m_Shape = shape;
		}

		// Token: 0x040000BB RID: 187
		private const int k_SegmentsMin = 2;

		// Token: 0x040000BC RID: 188
		private const int k_SegmentsMax = 4096;

		// Token: 0x040000BD RID: 189
		private const float k_RadiusMin = 1E-05f;

		// Token: 0x040000BE RID: 190
		private const float k_RadiusMax = 10000f;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private T m_Shape;

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		private bool m_CapEnds;

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		private bool m_FlipNormals;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private int m_SegmentCount;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private float m_Radius;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private Vector2 m_Range;
	}
}
