using System;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x02000013 RID: 19
	public class Triangulation
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00006B24 File Offset: 0x00004D24
		public void BuildTriangles(MeshData data)
		{
			if ((!this.Interior && !this.Exterior) || data.Vertices.Count == 0)
			{
				return;
			}
			this.MonotoneTriangulation.WorkBufferPool = this.WorkBufferPool;
			this.MonotoneTriangulation.BuildTriangles(data);
			if (this.Delaunay || this.Interior != this.Exterior || this.Infinity)
			{
				Triangles triangles = new Triangles(data);
				if (this.Delaunay)
				{
					this.DelaunayRefine.WorkBufferPool = this.WorkBufferPool;
					this.DelaunayRefine.RefineTriangles(triangles);
				}
				this.Filter.WorkBufferPool = this.WorkBufferPool;
				this.Filter.Infinity = this.Infinity;
				if (!this.Exterior)
				{
					this.Filter.Target = -1;
					this.Filter.Do(triangles, data.Triangles);
					return;
				}
				if (!this.Interior)
				{
					this.Filter.Target = 1;
					this.Filter.Do(triangles, data.Triangles);
					return;
				}
				if (this.Infinity)
				{
					this.Filter.Target = 0;
					this.Filter.Do(triangles, data.Triangles);
					return;
				}
				triangles.Fill(data.Triangles);
			}
		}

		// Token: 0x0400003B RID: 59
		public bool Delaunay = true;

		// Token: 0x0400003C RID: 60
		public bool Interior = true;

		// Token: 0x0400003D RID: 61
		public bool Exterior = true;

		// Token: 0x0400003E RID: 62
		public bool Infinity;

		// Token: 0x0400003F RID: 63
		public WorkBufferPool WorkBufferPool;

		// Token: 0x04000040 RID: 64
		private MonotoneTriangulation MonotoneTriangulation = new MonotoneTriangulation();

		// Token: 0x04000041 RID: 65
		private DelaunayRefine DelaunayRefine = new DelaunayRefine();

		// Token: 0x04000042 RID: 66
		private Filter Filter = new Filter();
	}
}
