using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020002DE RID: 734
	[HelpURL("https://arongranberg.com/astar/documentation/stable/localspacerichai.html")]
	public class LocalSpaceRichAI : RichAI
	{
		// Token: 0x06001239 RID: 4665 RVA: 0x00070408 File Offset: 0x0006E608
		protected override Vector3 ClampPositionToGraph(Vector3 newPosition)
		{
			this.RefreshTransform();
			NNInfo nninfo = ((AstarPath.active != null) ? AstarPath.active.GetNearest(this.graph.transformation.InverseTransform(newPosition)) : default(NNInfo));
			float num;
			this.movementPlane.ToPlane(newPosition, out num);
			return this.movementPlane.ToWorld(this.movementPlane.ToPlane((nninfo.node != null) ? this.graph.transformation.Transform(nninfo.position) : newPosition), num);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00070496 File Offset: 0x0006E696
		private void RefreshTransform()
		{
			this.graph.Refresh();
			this.richPath.transform = this.graph.transformation;
			this.movementPlane = this.graph.transformation.ToSimpleMovementPlane();
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000704CF File Offset: 0x0006E6CF
		protected override void Start()
		{
			this.RefreshTransform();
			base.Start();
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000704E0 File Offset: 0x0006E6E0
		protected override void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			this.RefreshTransform();
			base.CalculatePathRequestEndpoints(out start, out end);
			start = this.graph.transformation.InverseTransform(start);
			end = this.graph.transformation.InverseTransform(end);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00070533 File Offset: 0x0006E733
		protected override void OnUpdate(float dt)
		{
			this.RefreshTransform();
			base.OnUpdate(dt);
		}

		// Token: 0x04000D71 RID: 3441
		public LocalSpaceGraph graph;
	}
}
