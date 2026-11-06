using System;
using Pathfinding.Drawing;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000083 RID: 131
	[AddComponentMenu("Pathfinding/Link")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink.html")]
	public class NodeLink : GraphModifier
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00016E81 File Offset: 0x00015081
		public Transform Start
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00016E89 File Offset: 0x00015089
		public Transform End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00016E91 File Offset: 0x00015091
		public override void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
			this.Apply();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00016E9C File Offset: 0x0001509C
		public static void DrawArch(Vector3 a, Vector3 b, Vector3 up, Color color)
		{
			Vector3 vector = b - a;
			if (vector == Vector3.zero)
			{
				return;
			}
			Vector3 vector2 = Vector3.Cross(up, vector);
			Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * vector.magnitude * 0.1f;
			Draw.Bezier(a, a + vector3, b + vector3, b, color);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00016F18 File Offset: 0x00015118
		public virtual void Apply()
		{
			if (this.Start == null || this.End == null || AstarPath.active == null)
			{
				return;
			}
			GraphNode node = AstarPath.active.GetNearest(this.Start.position).node;
			GraphNode node2 = AstarPath.active.GetNearest(this.End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (this.deleteConnection)
			{
				GraphNode.Disconnect(node, node2);
				return;
			}
			uint num = (uint)Math.Round((double)((float)(node.position - node2.position).costMagnitude * this.costFactor));
			GraphNode.Connect(node, node2, num, this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00016FDC File Offset: 0x000151DC
		public override void DrawGizmos()
		{
			if (this.Start == null || this.End == null)
			{
				return;
			}
			NodeLink.DrawArch(this.Start.position, this.End.position, Vector3.up, this.deleteConnection ? Color.red : Color.green);
		}

		// Token: 0x040002DB RID: 731
		public Transform end;

		// Token: 0x040002DC RID: 732
		public float costFactor = 1f;

		// Token: 0x040002DD RID: 733
		public bool oneWay;

		// Token: 0x040002DE RID: 734
		public bool deleteConnection;
	}
}
