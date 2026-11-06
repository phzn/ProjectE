using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.ExtrusionShapes
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public class SplineShape : IExtrudeShape
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0001035C File Offset: 0x0000E55C
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00010364 File Offset: 0x0000E564
		public int SideCount
		{
			get
			{
				return this.m_SideCount;
			}
			set
			{
				this.m_SideCount = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001036D File Offset: 0x0000E56D
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00010375 File Offset: 0x0000E575
		public SplineContainer SplineContainer
		{
			get
			{
				return this.m_Template;
			}
			set
			{
				this.m_Template = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0001037E File Offset: 0x0000E57E
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00010386 File Offset: 0x0000E586
		public int SplineIndex
		{
			get
			{
				return this.m_SplineIndex;
			}
			set
			{
				this.m_SplineIndex = math.max(0, value);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00010395 File Offset: 0x0000E595
		public Spline Spline
		{
			get
			{
				if (!(this.m_Template != null))
				{
					return null;
				}
				return this.m_Template[this.m_SplineIndex % this.m_Template.Splines.Count];
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000103CC File Offset: 0x0000E5CC
		public float2 GetPosition(float t, int index)
		{
			if (this.Spline == null)
			{
				return 0f;
			}
			if (t == 1f)
			{
				t = 0.9999f;
			}
			else if (t == 0f)
			{
				t = 0.0001f;
			}
			float2 @float;
			switch (this.m_Axis)
			{
			case SplineShape.Axis.X:
				@float = this.Spline.EvaluatePosition(1f - t).zy;
				break;
			case SplineShape.Axis.Y:
				@float = this.Spline.EvaluatePosition(1f - t).xz;
				break;
			case SplineShape.Axis.Z:
				@float = this.Spline.EvaluatePosition(1f - t).xy;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return @float;
		}

		// Token: 0x04000101 RID: 257
		[SerializeField]
		private SplineContainer m_Template;

		// Token: 0x04000102 RID: 258
		[SerializeField]
		[SplineIndex("m_Template")]
		private int m_SplineIndex;

		// Token: 0x04000103 RID: 259
		[SerializeField]
		[Min(2f)]
		private int m_SideCount = 12;

		// Token: 0x04000104 RID: 260
		[SerializeField]
		[Tooltip("The axis of the template spline to be used when winding the vertices along the extruded mesh.")]
		public SplineShape.Axis m_Axis = SplineShape.Axis.Y;

		// Token: 0x02000072 RID: 114
		public enum Axis
		{
			// Token: 0x0400017D RID: 381
			X,
			// Token: 0x0400017E RID: 382
			Y,
			// Token: 0x0400017F RID: 383
			Z
		}
	}
}
