using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000088 RID: 136
	[AddComponentMenu("Pathfinding/Link3")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink3.html")]
	public class NodeLink3 : GraphModifier
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x00017814 File Offset: 0x00015A14
		public static NodeLink3 GetNodeLink(GraphNode node)
		{
			NodeLink3 nodeLink;
			NodeLink3.reference.TryGetValue(node, out nodeLink);
			return nodeLink;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00016E81 File Offset: 0x00015081
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00017830 File Offset: 0x00015A30
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00017838 File Offset: 0x00015A38
		public GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00017840 File Offset: 0x00015A40
		public GraphNode EndNode
		{
			get
			{
				return this.endNode;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00017848 File Offset: 0x00015A48
		public override void OnPostScan()
		{
			if (AstarPath.active.isScanning)
			{
				this.InternalOnPostScan();
				return;
			}
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(bool _)
			{
				this.InternalOnPostScan();
				return true;
			}));
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00017878 File Offset: 0x00015A78
		public void InternalOnPostScan()
		{
			if (AstarPath.active.data.pointGraph == null)
			{
				AstarPath.active.data.AddGraph(typeof(PointGraph));
			}
			this.startNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.StartTransform.position);
			this.startNode.link = this;
			this.endNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.EndTransform.position);
			this.endNode.link = this;
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			NodeLink3.reference[this.startNode] = this;
			NodeLink3.reference[this.endNode] = this;
			this.Apply(true);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00017990 File Offset: 0x00015B90
		public override void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
			if (!AstarPath.active.isScanning)
			{
				if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
				{
					this.connectedNode1 = null;
				}
				if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
				{
					this.connectedNode2 = null;
				}
				if (!this.postScanCalled)
				{
					this.OnPostScan();
					return;
				}
				this.Apply(false);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000179F7 File Offset: 0x00015BF7
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && AstarPath.active != null && AstarPath.active.data != null && AstarPath.active.data.pointGraph != null)
			{
				this.OnGraphsPostUpdate();
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00017A38 File Offset: 0x00015C38
		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				NodeLink3.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				NodeLink3.reference.Remove(this.endNode);
			}
			if (this.startNode != null && this.endNode != null)
			{
				this.startNode.RemovePartialConnection(this.endNode);
				this.endNode.RemovePartialConnection(this.startNode);
				if (this.connectedNode1 != null && this.connectedNode2 != null)
				{
					this.startNode.RemovePartialConnection(this.connectedNode1);
					this.connectedNode1.RemovePartialConnection(this.startNode);
					this.endNode.RemovePartialConnection(this.connectedNode2);
					this.connectedNode2.RemovePartialConnection(this.endNode);
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00017B0A File Offset: 0x00015D0A
		private void RemoveConnections(GraphNode node)
		{
			node.ClearConnections(true);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017B13 File Offset: 0x00015D13
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (Application.isPlaying)
			{
				this.Apply(true);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00017B24 File Offset: 0x00015D24
		public void Apply(bool forceNewCheck)
		{
			NearestNodeConstraint none = NearestNodeConstraint.None;
			none.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft();
			none.graphMask = ~GraphMask.FromGraphIndex(this.startNode.GraphIndex);
			bool flag = true;
			NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
			flag &= nearest.node == this.connectedNode1 && nearest.node != null;
			this.connectedNode1 = nearest.node as MeshNode;
			this.clamped1 = nearest.position;
			if (this.connectedNode1 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode1.position, Vector3.up * 5f, Color.red);
			}
			NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
			flag &= nearest2.node == this.connectedNode2 && nearest2.node != null;
			this.connectedNode2 = nearest2.node as MeshNode;
			this.clamped2 = nearest2.position;
			if (this.connectedNode2 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode2.position, Vector3.up * 5f, Color.cyan);
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.startNode.position = (Int3)this.StartTransform.position;
			this.endNode.position = (Int3)this.EndTransform.position;
			if (flag && !forceNewCheck)
			{
				return;
			}
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint num = (uint)Mathf.RoundToInt((float)((Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			GraphNode.Connect(this.startNode, this.endNode, num, OffMeshLinks.Directionality.TwoWay);
			Int3 @int = this.connectedNode2.position - this.connectedNode1.position;
			for (int i = 0; i < this.connectedNode1.GetVertexCount(); i++)
			{
				Int3 vertex = this.connectedNode1.GetVertex(i);
				Int3 vertex2 = this.connectedNode1.GetVertex((i + 1) % this.connectedNode1.GetVertexCount());
				if (Int3.DotLong((vertex2 - vertex).Normal2D(), @int) <= 0L)
				{
					for (int j = 0; j < this.connectedNode2.GetVertexCount(); j++)
					{
						Int3 vertex3 = this.connectedNode2.GetVertex(j);
						Int3 vertex4 = this.connectedNode2.GetVertex((j + 1) % this.connectedNode2.GetVertexCount());
						if (Int3.DotLong((vertex4 - vertex3).Normal2D(), @int) >= 0L && (double)Int3.Angle(vertex4 - vertex3, vertex2 - vertex) > 2.967059810956319)
						{
							float num2 = 0f;
							float num3 = 1f;
							num3 = Math.Min(num3, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex3));
							num2 = Math.Max(num2, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex4));
							if (num3 >= num2)
							{
								Vector3 vector = (Vector3)(vertex2 - vertex) * num2 + (Vector3)vertex;
								Vector3 vector2 = (Vector3)(vertex2 - vertex) * num3 + (Vector3)vertex;
								this.startNode.portalA = vector;
								this.startNode.portalB = vector2;
								this.endNode.portalA = vector2;
								this.endNode.portalB = vector;
								GraphNode.Connect(this.connectedNode1, this.startNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor), OffMeshLinks.Directionality.TwoWay);
								GraphNode.Connect(this.endNode, this.connectedNode2, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor), OffMeshLinks.Directionality.TwoWay);
								return;
							}
							Debug.LogError(string.Concat(new string[]
							{
								"Something went wrong! ",
								num2.ToString(),
								" ",
								num3.ToString(),
								" ",
								vertex,
								" ",
								vertex2,
								" ",
								vertex3,
								" ",
								vertex4,
								"\nTODO, how can this happen?"
							}));
						}
					}
				}
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00018008 File Offset: 0x00016208
		public override void DrawGizmos()
		{
			bool flag = GizmoContext.InActiveSelection(this);
			Color color = (flag ? NodeLink3.GizmosColorSelected : NodeLink3.GizmosColor);
			if (this.StartTransform != null)
			{
				Draw.xz.Circle(this.StartTransform.position, 0.4f, color);
			}
			if (this.EndTransform != null)
			{
				Draw.xz.Circle(this.EndTransform.position, 0.4f, color);
			}
			if (this.StartTransform != null && this.EndTransform != null)
			{
				NodeLink.DrawArch(this.StartTransform.position, this.EndTransform.position, Vector3.up, color);
				if (flag)
				{
					Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
					NodeLink.DrawArch(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f, Vector3.up, color);
					NodeLink.DrawArch(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f, Vector3.up, color);
				}
			}
		}

		// Token: 0x040002EB RID: 747
		protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();

		// Token: 0x040002EC RID: 748
		public Transform end;

		// Token: 0x040002ED RID: 749
		public float costFactor = 1f;

		// Token: 0x040002EE RID: 750
		private NodeLink3Node startNode;

		// Token: 0x040002EF RID: 751
		private NodeLink3Node endNode;

		// Token: 0x040002F0 RID: 752
		private MeshNode connectedNode1;

		// Token: 0x040002F1 RID: 753
		private MeshNode connectedNode2;

		// Token: 0x040002F2 RID: 754
		private Vector3 clamped1;

		// Token: 0x040002F3 RID: 755
		private Vector3 clamped2;

		// Token: 0x040002F4 RID: 756
		private bool postScanCalled;

		// Token: 0x040002F5 RID: 757
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x040002F6 RID: 758
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
