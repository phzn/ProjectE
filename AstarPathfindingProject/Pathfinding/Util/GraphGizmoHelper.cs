using System;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020002A6 RID: 678
	public class GraphGizmoHelper : IAstarPooledObject, IDisposable
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00066385 File Offset: 0x00064585
		// (set) Token: 0x060010F9 RID: 4345 RVA: 0x0006638D File Offset: 0x0006458D
		public DrawingData.Hasher hasher { get; private set; }

		// Token: 0x060010FA RID: 4346 RVA: 0x00066396 File Offset: 0x00064596
		public GraphGizmoHelper()
		{
			this.drawConnection = new Action<GraphNode>(this.DrawConnection);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x000663B0 File Offset: 0x000645B0
		public static GraphGizmoHelper GetSingleFrameGizmoHelper(DrawingData gizmos, AstarPath active, RedrawScope redrawScope, bool renderInGame)
		{
			return GraphGizmoHelper.GetGizmoHelper(gizmos, active, DrawingData.Hasher.NotSupplied, redrawScope, renderInGame);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000663C0 File Offset: 0x000645C0
		public static GraphGizmoHelper GetGizmoHelper(DrawingData gizmos, AstarPath active, DrawingData.Hasher hasher, RedrawScope redrawScope, bool renderInGame)
		{
			GraphGizmoHelper graphGizmoHelper = ObjectPool<GraphGizmoHelper>.Claim();
			graphGizmoHelper.Init(active, hasher, gizmos, redrawScope, renderInGame);
			return graphGizmoHelper;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000663D4 File Offset: 0x000645D4
		public void Init(AstarPath active, DrawingData.Hasher hasher, DrawingData gizmos, RedrawScope redrawScope, bool renderInGame)
		{
			if (active != null)
			{
				this.debugData = active.debugPathData;
				this.debugPathID = active.debugPathID;
				this.debugMode = active.debugMode;
				this.debugFloor = active.debugFloor;
				this.debugRoof = active.debugRoof;
				this.nodeStorage = active.nodeStorage;
				this.showSearchTree = false;
			}
			this.hasher = hasher;
			this.builder = gizmos.GetBuilder(hasher, redrawScope, renderInGame);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00066451 File Offset: 0x00064651
		public void OnEnterPool()
		{
			this.builder.Dispose();
			this.debugData = null;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00066465 File Offset: 0x00064665
		public void DrawConnections(GraphNode node)
		{
			if (!this.showSearchTree)
			{
				this.drawConnectionColor = this.NodeColor(node);
				this.drawConnectionStart = (Vector3)node.position;
				node.GetConnections(this.drawConnection, 32);
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0006649B File Offset: 0x0006469B
		private void DrawConnection(GraphNode other)
		{
			this.builder.Line(this.drawConnectionStart, ((Vector3)other.position + this.drawConnectionStart) * 0.5f, this.drawConnectionColor);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000664D4 File Offset: 0x000646D4
		public Color NodeColor(GraphNode node)
		{
			Color color;
			if (node.Walkable)
			{
				switch (this.debugMode)
				{
				case GraphDebugMode.SolidColor:
					return AstarColor.SolidColor;
				case GraphDebugMode.Penalty:
					return Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (node.Penalty - this.debugFloor) / (this.debugRoof - this.debugFloor));
				case GraphDebugMode.Areas:
					return AstarColor.GetAreaColor(node.Area);
				case GraphDebugMode.Tags:
					return AstarColor.GetTagColor(node.Tag);
				case GraphDebugMode.HierarchicalNode:
				case GraphDebugMode.NavmeshBorderObstacles:
					return AstarColor.GetTagColor((uint)node.HierarchicalNodeIndex);
				}
				color = AstarColor.SolidColor;
			}
			else
			{
				color = AstarColor.UnwalkableNode;
			}
			return color;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0006658F File Offset: 0x0006478F
		public void DrawWireTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			this.builder.Line(a, b, color);
			this.builder.Line(b, c, color);
			this.builder.Line(c, a, color);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000665C0 File Offset: 0x000647C0
		public void DrawTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			int[] array = ArrayPool<int>.Claim(numTriangles * 3);
			for (int i = 0; i < numTriangles * 3; i++)
			{
				array[i] = i;
			}
			this.builder.SolidMesh(vertices, array, colors, numTriangles * 3, numTriangles * 3);
			ArrayPool<int>.Release(ref array, false);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00066604 File Offset: 0x00064804
		public void DrawWireTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			for (int i = 0; i < numTriangles; i++)
			{
				this.DrawWireTriangle(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2], colors[i * 3]);
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0006664C File Offset: 0x0006484C
		public void Dispose()
		{
			GraphGizmoHelper graphGizmoHelper = this;
			ObjectPool<GraphGizmoHelper>.Release(ref graphGizmoHelper);
		}

		// Token: 0x04000C30 RID: 3120
		private PathHandler debugData;

		// Token: 0x04000C31 RID: 3121
		private ushort debugPathID;

		// Token: 0x04000C32 RID: 3122
		private GraphDebugMode debugMode;

		// Token: 0x04000C33 RID: 3123
		public bool showSearchTree;

		// Token: 0x04000C34 RID: 3124
		private float debugFloor;

		// Token: 0x04000C35 RID: 3125
		private float debugRoof;

		// Token: 0x04000C36 RID: 3126
		public CommandBuilder builder;

		// Token: 0x04000C37 RID: 3127
		private Vector3 drawConnectionStart;

		// Token: 0x04000C38 RID: 3128
		private Color drawConnectionColor;

		// Token: 0x04000C39 RID: 3129
		private readonly Action<GraphNode> drawConnection;

		// Token: 0x04000C3A RID: 3130
		private GlobalNodeStorage nodeStorage;
	}
}
