using System;
using DA_Assets.SVGMeshUnity.Internals;
using DA_Assets.SVGMeshUnity.Internals.Cdt2d;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class SVGMesh
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000031D4 File Offset: 0x000013D4
		public void Init(float scale, bool delaunay = false, bool interior = true, bool exterior = false, bool infinity = false)
		{
			this.scale = scale;
			this.delaunay = delaunay;
			this.interior = interior;
			this.exterior = exterior;
			this.infinity = infinity;
			this.bezierToVertex = new BezierToVertex();
			this.bezierToVertex.WorkBufferPool = SVGMesh.workBufferPool;
			this.triangulation = new Triangulation();
			this.triangulation.WorkBufferPool = SVGMesh.workBufferPool;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000323C File Offset: 0x0000143C
		public void Fill(SVGData svg, MeshFilter filter)
		{
			this.meshData.Clear();
			this.bezierToVertex.Scale = this.scale;
			this.bezierToVertex.GetContours(svg, this.meshData);
			this.triangulation.Delaunay = this.delaunay;
			this.triangulation.Interior = this.interior;
			this.triangulation.Exterior = this.exterior;
			this.triangulation.Infinity = this.infinity;
			this.triangulation.BuildTriangles(this.meshData);
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
				this.mesh.MarkDynamic();
			}
			this.meshData.MakeUnityFriendly();
			this.meshData.Upload(this.mesh);
			if (filter != null)
			{
				filter.sharedMesh = this.mesh;
			}
		}

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private float scale;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private bool delaunay;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private bool interior;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private bool exterior;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private bool infinity;

		// Token: 0x0400000D RID: 13
		private static WorkBufferPool workBufferPool = new WorkBufferPool();

		// Token: 0x0400000E RID: 14
		private MeshData meshData = new MeshData();

		// Token: 0x0400000F RID: 15
		private BezierToVertex bezierToVertex;

		// Token: 0x04000010 RID: 16
		private Triangulation triangulation;

		// Token: 0x04000011 RID: 17
		private Mesh mesh;
	}
}
