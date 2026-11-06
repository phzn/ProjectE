using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Graphs.Grid;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Graphs.Grid.Rules;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000C8 RID: 200
	[JsonOptIn]
	[Preserve]
	public class GridGraph : NavGraph, IUpdatableGraph, ITransformedGraph, IRaycastableGraph
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x0001F0A5 File Offset: 0x0001D2A5
		protected override void DisposeUnmanagedData()
		{
			this.DestroyAllNodes();
			GridNode.ClearGridGraph((int)this.graphIndex, this);
			this.rules.DisposeUnmanagedData();
			this.nodeData.Dispose();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001F0CF File Offset: 0x0001D2CF
		protected override void DestroyAllNodes()
		{
			this.GetNodes(delegate(GraphNode node)
			{
				(node as GridNodeBase).ClearCustomConnections(true);
				node.ClearConnections(false);
				node.Destroy();
			});
			this.nodes = null;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00019DC2 File Offset: 0x00017FC2
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x0001F0FD File Offset: 0x0001D2FD
		public virtual int LayerCount
		{
			get
			{
				return 1;
			}
			protected set
			{
				if (value != 1)
				{
					throw new NotSupportedException("Grid graphs cannot have multiple layers");
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public virtual int MaxLayers
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001F10E File Offset: 0x0001D30E
		public override int CountNodes()
		{
			if (this.nodes == null)
			{
				return 0;
			}
			return this.nodes.Length;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001F124 File Offset: 0x0001D324
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					action(this.nodes[i]);
				}
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001F168 File Offset: 0x0001D368
		public override void GetNodes<T>(GraphNode.NodeActionWithData<T> action, ref T data)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					action(this.nodes[i], ref data);
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x0001F1B2 File Offset: 0x0001D3B2
		[Obsolete("This field has been renamed to maxStepHeight")]
		public float maxClimb
		{
			get
			{
				return this.maxStepHeight;
			}
			set
			{
				this.maxStepHeight = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001F1BB File Offset: 0x0001D3BB
		protected bool useRaycastNormal
		{
			get
			{
				return Math.Abs(90f - this.maxSlope) > float.Epsilon;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001F1D5 File Offset: 0x0001D3D5
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001F1DD File Offset: 0x0001D3DD
		public Vector2 size { get; protected set; }

		// Token: 0x060006AF RID: 1711 RVA: 0x0001F1E6 File Offset: 0x0001D3E6
		public static int[] GetNeighbourDirections(NumNeighbours neighbours)
		{
			if (neighbours == NumNeighbours.Four)
			{
				return GridGraph.axisAlignedNeighbourIndices;
			}
			if (neighbours != NumNeighbours.Six)
			{
				return GridGraph.allNeighbourIndices;
			}
			return GridGraph.hexagonNeighbourIndices;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001F202 File Offset: 0x0001D402
		internal ref GridGraphNodeData nodeDataRef
		{
			get
			{
				return ref this.nodeData;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001F20A File Offset: 0x0001D40A
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001F212 File Offset: 0x0001D412
		public GraphTransform transform { get; private set; } = new GraphTransform(Matrix4x4.identity);

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001F21B File Offset: 0x0001D41B
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0001F244 File Offset: 0x0001D444
		public bool is2D
		{
			get
			{
				return Quaternion.Euler(this.rotation) * Vector3.up == -Vector3.forward;
			}
			set
			{
				if (value != this.is2D)
				{
					this.rotation = (value ? new Vector3(this.rotation.y - 90f, 270f, 90f) : new Vector3(0f, this.rotation.x + 90f, 0f));
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0001F2A5 File Offset: 0x0001D4A5
		public override bool isScanned
		{
			get
			{
				return this.nodes != null;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001F2B0 File Offset: 0x0001D4B0
		protected virtual GridNodeBase[] AllocateNodesJob(int size, out JobHandle dependency)
		{
			GridNodeBase[] array = new GridNodeBase[size];
			dependency = this.active.AllocateNodes<GridNodeBase>(array, size, this.newGridNodeDelegate, 1U);
			return array;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001F2DF File Offset: 0x0001D4DF
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			throw new Exception("This method cannot be used for Grid Graphs. Please use the other overload of RelocateNodes instead");
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001F2EC File Offset: 0x0001D4EC
		public unsafe void RelocateNodes(Vector3 center, Quaternion rotation, float nodeSize, float aspectRatio = 1f, float isometricAngle = 0f)
		{
			base.AssertSafeToUpdateGraph();
			GraphTransform transform = this.transform;
			this.center = center;
			this.rotation = rotation.eulerAngles;
			this.aspectRatio = aspectRatio;
			this.isometricAngle = isometricAngle;
			base.DirtyBounds(this.bounds);
			this.SetDimensions(this.width, this.depth, nodeSize);
			new JobRelocateNodes
			{
				previousWorldToGraph = transform.inverseMatrix,
				graphToWorld = this.transform.matrix,
				positions = this.nodeData.positions,
				bounds = this.nodeData.bounds
			}.Run<JobRelocateNodes>();
			UnsafeSpan<Vector3> unsafeSpan = this.nodeData.positions.AsUnsafeSpan<Vector3>();
			for (int i = 0; i < this.nodes.Length; i++)
			{
				GridNodeBase gridNodeBase = this.nodes[i];
				if (gridNodeBase != null)
				{
					gridNodeBase.position = (Int3)(*unsafeSpan[i]);
				}
			}
			base.DirtyBounds(this.bounds);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001F3F4 File Offset: 0x0001D5F4
		public override bool IsInsideBounds(Vector3 point)
		{
			if (this.nodes == null)
			{
				return false;
			}
			Vector3 vector = this.transform.InverseTransform(point);
			return vector.x >= 0f && vector.z >= 0f && vector.x <= (float)this.width && vector.z <= (float)this.depth && (this.collision.use2D || !this.collision.heightCheck || (vector.y >= -0.001f && vector.y <= this.collision.fromHeight + 0.001f));
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0001F49C File Offset: 0x0001D69C
		public override Bounds bounds
		{
			get
			{
				return this.transform.Transform(new Bounds(new Vector3((float)this.width * 0.5f, this.collision.fromHeight * 0.5f, (float)this.depth * 0.5f), new Vector3((float)this.width, this.collision.fromHeight, (float)this.depth)));
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001F507 File Offset: 0x0001D707
		public Int3 GraphPointToWorld(int x, int z, float height)
		{
			return (Int3)this.transform.Transform(new Vector3((float)x + 0.5f, height, (float)z + 0.5f));
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001F52F File Offset: 0x0001D72F
		public static float ConvertHexagonSizeToNodeSize(InspectorGridHexagonNodeSize mode, float value)
		{
			if (mode == InspectorGridHexagonNodeSize.Diameter)
			{
				value *= 1.5f / (float)Math.Sqrt(2.0);
			}
			else if (mode == InspectorGridHexagonNodeSize.Width)
			{
				value *= (float)Math.Sqrt(1.5);
			}
			return value;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001F567 File Offset: 0x0001D767
		public static float ConvertNodeSizeToHexagonSize(InspectorGridHexagonNodeSize mode, float value)
		{
			if (mode == InspectorGridHexagonNodeSize.Diameter)
			{
				value *= (float)Math.Sqrt(2.0) / 1.5f;
			}
			else if (mode == InspectorGridHexagonNodeSize.Width)
			{
				value *= (float)Math.Sqrt(0.6666666865348816);
			}
			return value;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001F59F File Offset: 0x0001D79F
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001F5A7 File Offset: 0x0001D7A7
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001F5C1 File Offset: 0x0001D7C1
		public float GetConnectionCost(int dir)
		{
			return this.neighbourCosts[dir];
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001F5CC File Offset: 0x0001D7CC
		public void SetGridShape(InspectorGridMode shape)
		{
			switch (shape)
			{
			case InspectorGridMode.Grid:
				this.isometricAngle = 0f;
				this.aspectRatio = 1f;
				this.uniformEdgeCosts = false;
				if (this.neighbours == NumNeighbours.Six)
				{
					this.neighbours = NumNeighbours.Eight;
				}
				break;
			case InspectorGridMode.IsometricGrid:
				this.uniformEdgeCosts = false;
				if (this.neighbours == NumNeighbours.Six)
				{
					this.neighbours = NumNeighbours.Eight;
				}
				this.isometricAngle = GridGraph.StandardIsometricAngle;
				break;
			case InspectorGridMode.Hexagonal:
				this.isometricAngle = GridGraph.StandardIsometricAngle;
				this.aspectRatio = 1f;
				this.uniformEdgeCosts = true;
				this.neighbours = NumNeighbours.Six;
				break;
			}
			this.inspectorGridMode = shape;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001F670 File Offset: 0x0001D870
		public void AlignToTilemap(GridLayout grid)
		{
			Vector3 vector = grid.CellToWorld(new Vector3Int(0, 0, 0));
			Vector3 vector2 = grid.CellToWorld(new Vector3Int(1, 0, 0)) - vector;
			Vector3 vector3 = grid.CellToWorld(new Vector3Int(0, 1, 0)) - vector;
			switch (grid.cellLayout)
			{
			case GridLayout.CellLayout.Rectangle:
			{
				quaternion quaternion = new quaternion(new float3x3(vector2.normalized, -Vector3.Cross(vector2, vector3).normalized, vector3.normalized));
				this.nodeSize = vector3.magnitude;
				this.isometricAngle = 0f;
				this.aspectRatio = vector2.magnitude / this.nodeSize;
				if (!float.IsFinite(this.aspectRatio))
				{
					this.aspectRatio = 1f;
				}
				this.rotation = quaternion.eulerAngles;
				this.uniformEdgeCosts = false;
				if (this.neighbours == NumNeighbours.Six)
				{
					this.neighbours = NumNeighbours.Eight;
				}
				this.inspectorGridMode = InspectorGridMode.Grid;
				break;
			}
			case GridLayout.CellLayout.Hexagon:
			{
				Vector3 vector4 = grid.CellToWorld(new Vector3Int(1, 0, 0)) - vector;
				Vector3 vector5 = grid.CellToWorld(new Vector3Int(-1, 1, 0)) - vector;
				this.aspectRatio = vector4.magnitude / Mathf.Sqrt(0.6666667f) / (Vector3.Cross(vector4.normalized, vector5).magnitude / (1.5f * Mathf.Sqrt(2f) / 3f));
				this.nodeSize = GridGraph.ConvertHexagonSizeToNodeSize(InspectorGridHexagonNodeSize.Width, vector4.magnitude / this.aspectRatio);
				Vector3 vector6 = -Vector3.Cross(vector4, Vector3.Cross(vector4, vector5));
				quaternion quaternion2 = new quaternion(new float3x3(vector4.normalized, -Vector3.Cross(vector4, vector6).normalized, vector6.normalized));
				this.rotation = quaternion2.eulerAngles;
				this.uniformEdgeCosts = true;
				this.neighbours = NumNeighbours.Six;
				this.inspectorGridMode = InspectorGridMode.Hexagonal;
				break;
			}
			case GridLayout.CellLayout.Isometric:
			{
				Vector3 vector7 = grid.CellToWorld(new Vector3Int(1, 1, 0)) - vector;
				Vector3 vector8 = grid.CellToWorld(new Vector3Int(1, -1, 0)) - vector;
				if (vector7.magnitude > vector8.magnitude)
				{
					Memory.Swap<Vector3>(ref vector7, ref vector8);
				}
				quaternion quaternion3 = math.mul(new quaternion(new float3x3(vector8.normalized, -Vector3.Cross(vector8, vector7).normalized, vector7.normalized)), quaternion.RotateY(-0.7853982f));
				this.isometricAngle = Mathf.Acos(vector7.magnitude / vector8.magnitude) * 57.29578f;
				this.nodeSize = vector8.magnitude / Mathf.Sqrt(2f);
				this.rotation = quaternion3.eulerAngles;
				this.uniformEdgeCosts = false;
				this.aspectRatio = 1f;
				if (this.neighbours == NumNeighbours.Six)
				{
					this.neighbours = NumNeighbours.Eight;
				}
				this.inspectorGridMode = InspectorGridMode.IsometricGrid;
				break;
			}
			}
			this.UpdateTransform();
			bool flag = grid.cellLayout == GridLayout.CellLayout.Hexagon;
			Vector3 vector9 = new Vector3((this.width % 2 == 0 != flag) ? 0f : 0.5f, 0f, (this.depth % 2 == 0 != flag) ? 0f : 0.5f);
			Vector3 vector10 = this.transform.TransformVector(vector9);
			Vector3Int vector3Int = grid.WorldToCell(this.center + vector10);
			vector3Int.z = 0;
			this.center = grid.CellToWorld(vector3Int) - vector10;
			if (float.IsNaN(this.center.x))
			{
				this.center = Vector3.zero;
			}
			this.UpdateTransform();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001FA64 File Offset: 0x0001DC64
		public void SetDimensions(int width, int depth, float nodeSize)
		{
			if (width < 1)
			{
				throw new ArgumentOutOfRangeException("width", "width must be at least 1");
			}
			if (depth < 1)
			{
				throw new ArgumentOutOfRangeException("depth", "depth must be at least 1");
			}
			if (nodeSize <= 0f)
			{
				throw new ArgumentOutOfRangeException("nodeSize", "nodeSize must be greater than 0");
			}
			this.unclampedSize = new Vector2((float)width, (float)depth) * nodeSize;
			this.nodeSize = nodeSize;
			this.UpdateTransform();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001FAD3 File Offset: 0x0001DCD3
		public void UpdateTransform()
		{
			this.CalculateDimensions(out this.width, out this.depth, out this.nodeSize);
			this.transform = this.CalculateTransform();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001FAFC File Offset: 0x0001DCFC
		public GraphTransform CalculateTransform()
		{
			int num;
			int num2;
			float num3;
			this.CalculateDimensions(out num, out num2, out num3);
			if (this.neighbours == NumNeighbours.Six)
			{
				Vector3 vector = new Vector3(num3 * this.aspectRatio * Mathf.Sqrt(0.6666667f), 0f, 0f);
				Vector3 vector2 = new Vector3(0f, 1f, 0f);
				Vector3 vector3 = new Vector3(-this.aspectRatio * num3 * 0.5f * Mathf.Sqrt(0.6666667f), 0f, num3 * (1.5f * Mathf.Sqrt(2f) / 3f));
				Matrix4x4 matrix4x = new Matrix4x4(vector, vector2, vector3, new Vector4(0f, 0f, 0f, 1f));
				matrix4x = Matrix4x4.TRS((Matrix4x4.TRS(this.center, Quaternion.Euler(this.rotation), Vector3.one) * matrix4x).MultiplyPoint3x4(-new Vector3((float)num, 0f, (float)num2) * 0.5f), Quaternion.Euler(this.rotation), Vector3.one) * matrix4x;
				return new GraphTransform(matrix4x);
			}
			Vector3 vector4 = new Vector3(Mathf.Cos(0.017453292f * this.isometricAngle), 1f, 1f);
			Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(num3 * this.aspectRatio, 1f, num3));
			float num4 = Mathf.Atan2(num3, num3 * this.aspectRatio) * 57.29578f;
			matrix4x2 = Matrix4x4.Rotate(Quaternion.Euler(0f, -num4, 0f)) * Matrix4x4.Scale(vector4) * Matrix4x4.Rotate(Quaternion.Euler(0f, num4, 0f)) * matrix4x2;
			return new GraphTransform(Matrix4x4.TRS((Matrix4x4.TRS(this.center, Quaternion.Euler(this.rotation), Vector3.one) * matrix4x2).MultiplyPoint3x4(-new Vector3((float)num, 0f, (float)num2) * 0.5f), Quaternion.Euler(this.rotation), Vector3.one) * matrix4x2);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001FD40 File Offset: 0x0001DF40
		private void CalculateDimensions(out int width, out int depth, out float nodeSize)
		{
			Vector2 vector = this.unclampedSize;
			vector.x *= Mathf.Sign(vector.x);
			vector.y *= Mathf.Sign(vector.y);
			nodeSize = Mathf.Max(this.nodeSize, vector.x / 1024f);
			nodeSize = Mathf.Max(this.nodeSize, vector.y / 1024f);
			vector.x = ((vector.x < nodeSize) ? nodeSize : vector.x);
			vector.y = ((vector.y < nodeSize) ? nodeSize : vector.y);
			this.size = vector;
			width = Mathf.FloorToInt(this.size.x / nodeSize);
			depth = Mathf.FloorToInt(this.size.y / nodeSize);
			if (Mathf.Approximately(this.size.x / nodeSize, (float)Mathf.CeilToInt(this.size.x / nodeSize)))
			{
				width = Mathf.CeilToInt(this.size.x / nodeSize);
			}
			if (Mathf.Approximately(this.size.y / nodeSize, (float)Mathf.CeilToInt(this.size.y / nodeSize)))
			{
				depth = Mathf.CeilToInt(this.size.y / nodeSize);
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001FE98 File Offset: 0x0001E098
		public override float NearestNodeDistanceSqrLowerBound(Vector3 position, ref NearestNodeConstraint constraint)
		{
			if (this.nodes == null || this.depth * this.width * this.LayerCount != this.nodes.Length)
			{
				return float.PositiveInfinity;
			}
			position = this.transform.InverseTransform(position);
			float x = position.x;
			float z = position.z;
			float num = Mathf.Clamp(x, 0f, (float)this.width);
			float num2 = Mathf.Clamp(z, 0f, (float)this.depth);
			return (x - num) * (x - num) + (z - num2) * (z - num2);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001FF24 File Offset: 0x0001E124
		protected virtual GridNodeBase GetNearestFromGraphSpace(Vector3 positionGraphSpace)
		{
			if (this.nodes == null || this.depth * this.width != this.nodes.Length)
			{
				return null;
			}
			int x = (int)positionGraphSpace.x;
			float z = positionGraphSpace.z;
			int num = Mathf.Clamp(x, 0, this.width - 1);
			int num2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			return this.nodes[num2 * this.width + num];
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001FF94 File Offset: 0x0001E194
		public override NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
		{
			if (this.nodes == null || this.depth * this.width * this.LayerCount != this.nodes.Length)
			{
				return NNInfo.Empty;
			}
			Vector3 vector = position;
			position = this.transform.InverseTransform(position);
			float x = position.x;
			float z = position.z;
			int num = Mathf.Clamp((int)x, 0, this.width - 1);
			int num2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			GridNodeBase gridNodeBase = null;
			bool isProjectedDistance = constraint.distanceMetric.isProjectedDistance;
			float num3 = constraint.maxDistanceSqrOrDefault(this.active);
			float num4 = num3;
			int layerCount = this.LayerCount;
			int num5 = this.width * this.depth;
			long num6 = 0L;
			float num7 = 0f;
			Int3 @int = default(Int3);
			if (isProjectedDistance)
			{
				@int = (Int3)this.transform.WorldUpAtGraphPosition(vector);
				num6 = Int3.DotLong((Int3)vector, @int);
				num7 = constraint.distanceMetric.distanceScaleAlongProjectionDirection * 0.001f * 0.001f;
			}
			bool allNodesAreSuitable = constraint.allNodesAreSuitable;
			for (int i = 0; i < layerCount; i++)
			{
				GridNodeBase gridNodeBase2 = this.nodes[num2 * this.width + num + num5 * i];
				if (gridNodeBase2 != null && (allNodesAreSuitable || constraint.Suitable(gridNodeBase2)))
				{
					float num12;
					if (isProjectedDistance)
					{
						float num8 = math.clamp(x, (float)num, (float)num + 1f) - x;
						float num9 = math.clamp(z, (float)num2, (float)num2 + 1f) - z;
						float num10 = this.nodeSize * this.nodeSize * (num8 * num8 + num9 * num9);
						float num11 = (float)(Int3.DotLong(gridNodeBase2.position, @int) - num6) * num7;
						num12 = Mathf.Sqrt(num10) + Mathf.Abs(num11);
						num12 *= num12;
					}
					else
					{
						num12 = ((Vector3)gridNodeBase2.position - vector).sqrMagnitude;
					}
					if (num12 <= num4)
					{
						num4 = num12;
						gridNodeBase = gridNodeBase2;
					}
				}
			}
			float num13 = Mathf.Min(Mathf.Min(x - (float)num, 1f - (x - (float)num)), Mathf.Min(z - (float)num2, 1f - (z - (float)num2))) * this.nodeSize;
			int num14 = 1;
			for (;;)
			{
				float num15 = (float)math.max(0, num14 - 2) * this.nodeSize + num13;
				if (num4 - 1E-05f <= num15 * num15)
				{
					break;
				}
				bool flag = false;
				int num16 = num + num14;
				int num17 = num2;
				int num18 = -1;
				int num19 = 1;
				for (int j = 0; j < 4; j++)
				{
					for (int k = 0; k < num14; k++)
					{
						if (num16 >= 0 && num17 >= 0 && num16 < this.width && num17 < this.depth)
						{
							flag = true;
							int num20 = num16 + num17 * this.width;
							for (int l = 0; l < layerCount; l++)
							{
								GridNodeBase gridNodeBase3 = this.nodes[num20 + num5 * l];
								if (gridNodeBase3 != null && (allNodesAreSuitable || constraint.Suitable(gridNodeBase3)))
								{
									float num25;
									if (isProjectedDistance)
									{
										float num21 = math.clamp(x, (float)num16, (float)num16 + 1f) - x;
										float num22 = math.clamp(z, (float)num17, (float)num17 + 1f) - z;
										float num23 = this.nodeSize * this.nodeSize * (num21 * num21 + num22 * num22);
										float num24 = (float)(Int3.DotLong(gridNodeBase3.position, @int) - num6) * num7;
										num25 = Mathf.Sqrt(num23) + Mathf.Abs(num24);
										num25 *= num25;
									}
									else
									{
										num25 = ((Vector3)gridNodeBase3.position - vector).sqrMagnitude;
									}
									if (num25 <= num4)
									{
										num4 = num25;
										gridNodeBase = gridNodeBase3;
									}
								}
							}
						}
						num16 += num18;
						num17 += num19;
					}
					int num26 = -num19;
					int num27 = num18;
					num18 = num26;
					num19 = num27;
				}
				if (!flag)
				{
					break;
				}
				num14++;
			}
			if (gridNodeBase == null)
			{
				return NNInfo.Empty;
			}
			if (isProjectedDistance)
			{
				for (;;)
				{
					int num28 = num - gridNodeBase.XCoordinateInGrid;
					int num29 = num2 - gridNodeBase.ZCoordinateInGrid;
					if (num28 == 0 && num29 == 0)
					{
						break;
					}
					int num30 = ((num28 > 0) ? 1 : ((num28 < 0) ? 3 : (-1)));
					int num31 = ((num29 > 0) ? 2 : ((num29 < 0) ? 0 : (-1)));
					if (Mathf.Abs(num28) < Mathf.Abs(num29))
					{
						Memory.Swap<int>(ref num30, ref num31);
					}
					GridNodeBase gridNodeBase4 = gridNodeBase.GetNeighbourAlongDirection(num30);
					if (gridNodeBase4 != null && (allNodesAreSuitable || constraint.Suitable(gridNodeBase4)))
					{
						gridNodeBase = gridNodeBase4;
					}
					else
					{
						if (num31 == -1 || (gridNodeBase4 = gridNodeBase.GetNeighbourAlongDirection(num31)) == null || (!allNodesAreSuitable && !constraint.Suitable(gridNodeBase4)))
						{
							break;
						}
						gridNodeBase = gridNodeBase4;
					}
				}
			}
			int xcoordinateInGrid = gridNodeBase.XCoordinateInGrid;
			int zcoordinateInGrid = gridNodeBase.ZCoordinateInGrid;
			Vector3 vector2 = this.transform.Transform(new Vector3(Mathf.Clamp(x, (float)xcoordinateInGrid, (float)xcoordinateInGrid + 1f), this.transform.InverseTransform((Vector3)gridNodeBase.position).y, Mathf.Clamp(z, (float)zcoordinateInGrid, (float)zcoordinateInGrid + 1f)));
			float num32 = (isProjectedDistance ? num4 : (vector2 - vector).sqrMagnitude);
			if (num32 > num3)
			{
				return NNInfo.Empty;
			}
			return new NNInfo(gridNodeBase, vector2, num32);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000204D0 File Offset: 0x0001E6D0
		public override NNInfo RandomPointOnSurface(NearestNodeConstraint constraint, bool highQuality = true)
		{
			if (!this.isScanned || this.nodes.Length == 0)
			{
				return NNInfo.Empty;
			}
			for (int i = 0; i < 10; i++)
			{
				GridNodeBase gridNodeBase = this.nodes[global::UnityEngine.Random.Range(0, this.nodes.Length)];
				if (gridNodeBase != null && constraint.Suitable(gridNodeBase))
				{
					return new NNInfo(gridNodeBase, gridNodeBase.RandomPointOnSurface(), 0f);
				}
			}
			return base.RandomPointOnSurface(constraint, highQuality);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00020540 File Offset: 0x0001E740
		public virtual void SetUpOffsetsAndCosts()
		{
			this.neighbourOffsets[0] = -this.width;
			this.neighbourOffsets[1] = 1;
			this.neighbourOffsets[2] = this.width;
			this.neighbourOffsets[3] = -1;
			this.neighbourOffsets[4] = -this.width + 1;
			this.neighbourOffsets[5] = this.width + 1;
			this.neighbourOffsets[6] = this.width - 1;
			this.neighbourOffsets[7] = -this.width - 1;
			float num = ((this.neighbours == NumNeighbours.Six) ? GridGraph.ConvertNodeSizeToHexagonSize(InspectorGridHexagonNodeSize.Width, this.nodeSize) : this.nodeSize);
			float num2 = Mathf.Round(num * 1000f);
			float num3 = (this.uniformEdgeCosts ? num2 : Mathf.Round(num * Mathf.Sqrt(2f) * 1000f));
			this.neighbourCosts[0] = num2;
			this.neighbourCosts[1] = num2;
			this.neighbourCosts[2] = num2;
			this.neighbourCosts[3] = num2;
			this.neighbourCosts[4] = num3;
			this.neighbourCosts[5] = num3;
			this.neighbourCosts[6] = num3;
			this.neighbourCosts[7] = num3;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00020654 File Offset: 0x0001E854
		public IGraphUpdatePromise TranslateInDirection(int dx, int dz)
		{
			return new GridGraph.GridGraphMovePromise(this, dx, dz);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00020660 File Offset: 0x0001E860
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			if (this.nodeSize <= 0f)
			{
				return null;
			}
			this.UpdateTransform();
			if (this.width > 1024 || this.depth > 1024)
			{
				Debug.LogError("One of the grid's sides is longer than 1024 nodes");
				return null;
			}
			this.SetUpOffsetsAndCosts();
			GridNode.SetGridGraph((int)this.graphIndex, this);
			if (this.collision == null)
			{
				this.collision = new GraphCollision();
			}
			this.collision.Initialize(this.transform, this.nodeSize);
			JobDependencyTracker jobDependencyTracker = ObjectPool<JobDependencyTracker>.Claim();
			JobHandle jobHandle;
			GridNodeBase[] array = this.AllocateNodesJob(this.width * this.depth, out jobHandle);
			return new GridGraph.GridGraphUpdatePromise(this, this.transform, new GridGraph.GridGraphUpdatePromise.NodesHolder
			{
				nodes = array
			}, new int3(this.width, 1, this.depth), new IntRect(0, 0, this.width - 1, this.depth - 1), jobDependencyTracker, jobHandle, Allocator.Persistent, GridGraph.RecalculationMode.RecalculateFromScratch, null, true, true);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00020748 File Offset: 0x0001E948
		public void SetWalkability(bool[] walkability, IntRect rect)
		{
			base.AssertSafeToUpdateGraph();
			IntRect intRect = new IntRect(0, 0, this.width - 1, this.depth - 1);
			if (!intRect.Contains(rect))
			{
				string[] array = new string[5];
				array[0] = "Rect (";
				int num = 1;
				IntRect intRect2 = rect;
				array[num] = intRect2.ToString();
				array[2] = ") must be within the graph bounds (";
				int num2 = 3;
				intRect2 = intRect;
				array[num2] = intRect2.ToString();
				array[4] = ")";
				throw new ArgumentException(string.Concat(array));
			}
			if (walkability.Length != rect.Width * rect.Height)
			{
				throw new ArgumentException("Array must have the same length as rect.Width*rect.Height");
			}
			if (this.LayerCount != 1)
			{
				throw new InvalidOperationException("This method only works in single-layered grid graphs.");
			}
			for (int i = 0; i < rect.Height; i++)
			{
				int num3 = (i + rect.ymin) * this.width + rect.xmin;
				for (int j = 0; j < rect.Width; j++)
				{
					bool flag = walkability[i * rect.Width + j];
					this.nodes[num3 + j].WalkableErosion = flag;
					this.nodes[num3 + j].Walkable = flag;
				}
			}
			this.RecalculateConnectionsInRegion(rect.Expand(1));
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002087F File Offset: 0x0001EA7F
		public void RecalculateAllConnections()
		{
			this.RecalculateConnectionsInRegion(new IntRect(0, 0, this.width - 1, this.depth - 1));
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000208A0 File Offset: 0x0001EAA0
		public void RecalculateConnectionsInRegion(IntRect recalculateRect)
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodes == null || this.nodes.Length != this.width * this.depth * this.LayerCount)
			{
				throw new InvalidOperationException("The Grid Graph is not scanned, cannot recalculate connections.");
			}
			IntRect intRect = new IntRect(0, 0, this.width - 1, this.depth - 1);
			IntRect intRect2 = IntRect.Intersection(recalculateRect, intRect);
			if (!intRect2.IsValid())
			{
				return;
			}
			JobDependencyTracker jobDependencyTracker = ObjectPool<JobDependencyTracker>.Claim();
			IntRect intRect3 = IntRect.Intersection(intRect2.Expand(1), intRect);
			IntBounds intBounds = new IntBounds(intRect3.xmin, 0, intRect3.ymin, intRect3.xmax + 1, this.LayerCount, intRect3.ymax + 1);
			if (intBounds.volume < 200)
			{
				jobDependencyTracker.SetLinearDependencies(true);
			}
			bool flag = this is LayerGridGraph;
			GridGraphScanData gridGraphScanData = new GridGraphScanData
			{
				dependencyTracker = jobDependencyTracker,
				nodes = GridGraphNodeData.ReadFromNodes(this.nodes, new Slice3D(this.nodeData.bounds, intBounds), default(JobHandle), this.nodeData.normals, Allocator.TempJob, flag, jobDependencyTracker),
				transform = this.transform,
				up = this.transform.WorldUpAtGraphPosition(Vector3.zero)
			};
			LayerGridGraph layerGridGraph = this as LayerGridGraph;
			float num = ((layerGridGraph != null) ? layerGridGraph.characterHeight : float.PositiveInfinity);
			IntBounds intBounds2 = new IntBounds(intRect2.xmin, 0, intRect2.ymin, intRect2.xmax + 1, this.LayerCount, intRect2.ymax + 1);
			gridGraphScanData.Connections(this.maxStepHeight, this.maxStepUsesSlope, intBounds2, this.neighbours, this.cutCorners, this.collision.use2D, true, num);
			this.nodeData.CopyFrom(gridGraphScanData.nodes, intBounds2, true, jobDependencyTracker);
			jobDependencyTracker.AllWritesDependency.Complete();
			gridGraphScanData.AssignNodeConnections(this.nodes, new int3(this.width, this.LayerCount, this.depth), intBounds2);
			ObjectPool<JobDependencyTracker>.Release(ref jobDependencyTracker);
			this.active.DirtyBounds(this.GetBoundsFromRect(intRect2));
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00020ABC File Offset: 0x0001ECBC
		public void CalculateConnectionsForCellAndNeighbours(int x, int z)
		{
			this.RecalculateConnectionsInRegion(new IntRect(x - 1, z - 1, x + 1, z + 1));
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		[Obsolete("This method is very slow since 4.3.80. Use RecalculateConnectionsInRegion or RecalculateAllConnections instead to batch connection recalculations.")]
		public virtual void CalculateConnections(GridNodeBase node)
		{
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex % this.width;
			int num2 = nodeInGridIndex / this.width;
			this.CalculateConnections(num, num2);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00020B04 File Offset: 0x0001ED04
		[Obsolete("This method is very slow since 4.3.80. Use RecalculateConnectionsInRegion instead to batch connection recalculations.")]
		public virtual void CalculateConnections(int x, int z)
		{
			this.RecalculateConnectionsInRegion(new IntRect(x, z, x, z));
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00020B18 File Offset: 0x0001ED18
		public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope, bool renderInGame)
		{
			using (GraphGizmoHelper singleFrameGizmoHelper = GraphGizmoHelper.GetSingleFrameGizmoHelper(gizmos, this.active, redrawScope, renderInGame))
			{
				int num;
				int num2;
				float num3;
				this.CalculateDimensions(out num, out num2, out num3);
				Bounds bounds = default(Bounds);
				bounds.SetMinMax(Vector3.zero, new Vector3((float)num, 0f, (float)num2));
				using (singleFrameGizmoHelper.builder.WithMatrix(this.CalculateTransform().matrix))
				{
					singleFrameGizmoHelper.builder.WireBox(bounds, Color.white);
					int num4 = ((this.nodes != null) ? this.nodes.Length : (-1));
					if (drawNodes && this.width * this.depth * this.LayerCount != num4)
					{
						Color color = new Color(1f, 1f, 1f, 0.2f);
						singleFrameGizmoHelper.builder.WireGrid(new float3((float)num * 0.5f, 0f, (float)num2 * 0.5f), Quaternion.identity, new int2(num, num2), new float2((float)num, (float)num2), color);
					}
				}
			}
			if (!drawNodes)
			{
				return;
			}
			GridNodeBase[] array = ArrayPool<GridNodeBase>.Claim(1024 * this.LayerCount);
			for (int i = this.width / 32; i >= 0; i--)
			{
				for (int j = this.depth / 32; j >= 0; j--)
				{
					int nodesInRegion = this.GetNodesInRegion(new IntRect(i * 32, j * 32, (i + 1) * 32 - 1, (j + 1) * 32 - 1), array);
					NodeHasher nodeHasher = new NodeHasher(this.active);
					nodeHasher.Add<bool>(this.showMeshOutline);
					nodeHasher.Add<bool>(this.showMeshSurface);
					nodeHasher.Add<bool>(this.showNodeConnections);
					for (int k = 0; k < nodesInRegion; k++)
					{
						nodeHasher.HashNode(array[k]);
					}
					if (!gizmos.Draw(nodeHasher, redrawScope))
					{
						using (GraphGizmoHelper gizmoHelper = GraphGizmoHelper.GetGizmoHelper(gizmos, this.active, nodeHasher, redrawScope, renderInGame))
						{
							if (this.showNodeConnections)
							{
								if (gizmoHelper.showSearchTree)
								{
									gizmoHelper.builder.PushLineWidth(2f, true);
								}
								for (int l = 0; l < nodesInRegion; l++)
								{
									if (array[l].Walkable)
									{
										gizmoHelper.DrawConnections(array[l]);
									}
								}
								if (gizmoHelper.showSearchTree)
								{
									gizmoHelper.builder.PopLineWidth();
								}
							}
							if (this.showMeshSurface || this.showMeshOutline)
							{
								this.CreateNavmeshSurfaceVisualization(array, nodesInRegion, gizmoHelper);
							}
						}
					}
				}
			}
			ArrayPool<GridNodeBase>.Release(ref array, false);
			if (this.active.showUnwalkableNodes)
			{
				base.DrawUnwalkableNodes(gizmos, this.nodeSize * 0.3f, redrawScope, renderInGame);
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00020E08 File Offset: 0x0001F008
		private void CreateNavmeshSurfaceVisualization(GridNodeBase[] nodes, int nodeCount, GraphGizmoHelper helper)
		{
			int num = 0;
			for (int i = 0; i < nodeCount; i++)
			{
				if (nodes[i].Walkable)
				{
					num++;
				}
			}
			int[] array;
			if (this.neighbours != NumNeighbours.Six)
			{
				RuntimeHelpers.InitializeArray(array = new int[4], fieldof(<PrivateImplementationDetails>.BAED642339816AFFB3FE8719792D0E4CE82F12DB72B7373D244EAA65445800FE).FieldHandle);
			}
			else
			{
				array = GridGraph.hexagonNeighbourIndices;
			}
			int[] array2 = array;
			float num2 = ((this.neighbours == NumNeighbours.Six) ? 0.333333f : 0.5f);
			int num3 = array2.Length - 2;
			int num4 = 3 * num3;
			Vector3[] array3 = ArrayPool<Vector3>.Claim(num * num4);
			Color[] array4 = ArrayPool<Color>.Claim(num * num4);
			int num5 = 0;
			for (int j = 0; j < nodeCount; j++)
			{
				GridNodeBase gridNodeBase = nodes[j];
				if (gridNodeBase.Walkable)
				{
					Color color = helper.NodeColor(gridNodeBase);
					if (color.a > 0.001f)
					{
						for (int k = 0; k < array2.Length; k++)
						{
							int num6 = array2[k];
							int num7 = array2[(k + 1) % array2.Length];
							GridNodeBase gridNodeBase2 = null;
							GridNodeBase neighbourAlongDirection = gridNodeBase.GetNeighbourAlongDirection(num6);
							if (neighbourAlongDirection != null && this.neighbours != NumNeighbours.Six)
							{
								gridNodeBase2 = neighbourAlongDirection.GetNeighbourAlongDirection(num7);
							}
							GridNodeBase neighbourAlongDirection2 = gridNodeBase.GetNeighbourAlongDirection(num7);
							if (neighbourAlongDirection2 != null && gridNodeBase2 == null && this.neighbours != NumNeighbours.Six)
							{
								gridNodeBase2 = neighbourAlongDirection2.GetNeighbourAlongDirection(num6);
							}
							Vector3 vector = new Vector3((float)gridNodeBase.XCoordinateInGrid + 0.5f, 0f, (float)gridNodeBase.ZCoordinateInGrid + 0.5f);
							vector.x += (float)(GridGraph.neighbourXOffsets[num6] + GridGraph.neighbourXOffsets[num7]) * num2;
							vector.z += (float)(GridGraph.neighbourZOffsets[num6] + GridGraph.neighbourZOffsets[num7]) * num2;
							vector.y += this.transform.InverseTransform((Vector3)gridNodeBase.position).y;
							if (neighbourAlongDirection != null)
							{
								vector.y += this.transform.InverseTransform((Vector3)neighbourAlongDirection.position).y;
							}
							if (neighbourAlongDirection2 != null)
							{
								vector.y += this.transform.InverseTransform((Vector3)neighbourAlongDirection2.position).y;
							}
							if (gridNodeBase2 != null)
							{
								vector.y += this.transform.InverseTransform((Vector3)gridNodeBase2.position).y;
							}
							vector.y /= 1f + ((neighbourAlongDirection != null) ? 1f : 0f) + ((neighbourAlongDirection2 != null) ? 1f : 0f) + ((gridNodeBase2 != null) ? 1f : 0f);
							vector = this.transform.Transform(vector);
							array3[num5 + k] = vector;
						}
						if (this.neighbours == NumNeighbours.Six)
						{
							array3[num5 + 6] = array3[num5];
							array3[num5 + 7] = array3[num5 + 2];
							array3[num5 + 8] = array3[num5 + 3];
							array3[num5 + 9] = array3[num5];
							array3[num5 + 10] = array3[num5 + 3];
							array3[num5 + 11] = array3[num5 + 5];
						}
						else
						{
							array3[num5 + 4] = array3[num5];
							array3[num5 + 5] = array3[num5 + 2];
						}
						for (int l = 0; l < num4; l++)
						{
							array4[num5 + l] = color;
						}
						for (int m = 0; m < array2.Length; m++)
						{
							GridNodeBase neighbourAlongDirection3 = gridNodeBase.GetNeighbourAlongDirection(array2[(m + 1) % array2.Length]);
							if (neighbourAlongDirection3 == null || (this.showMeshOutline && gridNodeBase.NodeInGridIndex < neighbourAlongDirection3.NodeInGridIndex))
							{
								helper.builder.Line(array3[num5 + m], array3[num5 + (m + 1) % array2.Length], (neighbourAlongDirection3 == null) ? Color.black : color);
							}
						}
						num5 += num4;
					}
				}
			}
			if (this.showMeshSurface)
			{
				helper.DrawTriangles(array3, array4, num5 * num3 / num4);
			}
			ArrayPool<Vector3>.Release(ref array3, false);
			ArrayPool<Color>.Release(ref array4, false);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002123C File Offset: 0x0001F43C
		public Bounds GetBoundsFromRect(IntRect rect)
		{
			rect = IntRect.Intersection(rect, new IntRect(0, 0, this.width - 1, this.depth - 1));
			if (!rect.IsValid())
			{
				return default(Bounds);
			}
			return this.transform.Transform(new Bounds(new Vector3((float)(rect.xmin + rect.xmax), this.collision.fromHeight, (float)(rect.ymin + rect.ymax)) * 0.5f, new Vector3((float)(rect.Width + 1), this.collision.fromHeight, (float)(rect.Height + 1))));
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000212E8 File Offset: 0x0001F4E8
		public IntRect GetRectFromBounds(Bounds bounds)
		{
			bounds = this.transform.InverseTransform(bounds);
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			int num = Mathf.FloorToInt(min.x + 0.01f);
			int num2 = Mathf.FloorToInt(max.x - 0.01f);
			int num3 = Mathf.FloorToInt(min.z + 0.01f);
			int num4 = Mathf.FloorToInt(max.z - 0.01f);
			IntRect intRect = new IntRect(num, num3, num2, num4);
			IntRect intRect2 = new IntRect(0, 0, this.width - 1, this.depth - 1);
			return IntRect.Intersection(intRect, intRect2);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00021384 File Offset: 0x0001F584
		public List<GraphNode> GetNodesInRegion(Bounds bounds)
		{
			return this.GetNodesInRegion(bounds, null);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002138E File Offset: 0x0001F58E
		public List<GraphNode> GetNodesInRegion(GraphUpdateShape shape)
		{
			return this.GetNodesInRegion(shape.GetBounds(), shape);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000213A0 File Offset: 0x0001F5A0
		protected virtual List<GraphNode> GetNodesInRegion(Bounds bounds, GraphUpdateShape shape)
		{
			IntRect rectFromBounds = this.GetRectFromBounds(bounds);
			if (this.nodes == null || !rectFromBounds.IsValid() || this.nodes.Length != this.width * this.depth * this.LayerCount)
			{
				return ListPool<GraphNode>.Claim();
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rectFromBounds.Width * rectFromBounds.Height);
			int num = rectFromBounds.Width;
			for (int i = 0; i < this.LayerCount; i++)
			{
				for (int j = rectFromBounds.ymin; j <= rectFromBounds.ymax; j++)
				{
					int num2 = i * this.width * this.depth + j * this.width + rectFromBounds.xmin;
					for (int k = 0; k < num; k++)
					{
						GridNodeBase gridNodeBase = this.nodes[num2 + k];
						if (gridNodeBase != null)
						{
							Vector3 vector = (Vector3)gridNodeBase.position;
							if (bounds.Contains(vector) && (shape == null || shape.Contains(vector)))
							{
								list.Add(gridNodeBase);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000214B0 File Offset: 0x0001F6B0
		public List<GraphNode> GetNodesInRegion(IntRect rect)
		{
			IntRect intRect = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, intRect);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth * this.LayerCount)
			{
				return ListPool<GraphNode>.Claim(0);
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rect.Width * rect.Height);
			int num = rect.Width;
			for (int i = 0; i < this.LayerCount; i++)
			{
				for (int j = rect.ymin; j <= rect.ymax; j++)
				{
					int num2 = i * this.width * this.depth + j * this.width + rect.xmin;
					for (int k = 0; k < num; k++)
					{
						GridNodeBase gridNodeBase = this.nodes[num2 + k];
						if (gridNodeBase != null)
						{
							list.Add(gridNodeBase);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000215AC File Offset: 0x0001F7AC
		public virtual int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			IntRect intRect = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, intRect);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth)
			{
				return 0;
			}
			if (buffer.Length < rect.Width * rect.Height)
			{
				throw new ArgumentException("Buffer is too small");
			}
			int num = 0;
			int i = rect.ymin;
			while (i <= rect.ymax)
			{
				Array.Copy(this.nodes, i * this.Width + rect.xmin, buffer, num, rect.Width);
				i++;
				num += rect.Width;
			}
			return num;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002166A File Offset: 0x0001F86A
		public virtual GridNodeBase GetNode(int x, int z)
		{
			if (x < 0 || z < 0 || x >= this.width || z >= this.depth)
			{
				return null;
			}
			return this.nodes[x + z * this.width];
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002169C File Offset: 0x0001F89C
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			if (!this.isScanned || this.nodes.Length != this.width * this.depth * this.LayerCount)
			{
				Debug.LogWarning("The Grid Graph is not scanned, cannot update graph");
				return null;
			}
			this.collision.Initialize(this.transform, this.nodeSize);
			return new GridGraph.CombinedGridGraphUpdatePromise(this, graphUpdates);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000216FC File Offset: 0x0001F8FC
		public override IGraphSnapshot Snapshot(Bounds bounds)
		{
			if (this.active.isScanning || this.active.IsAnyWorkItemInProgress)
			{
				throw new InvalidOperationException("Trying to capture a grid graph snapshot while inside a work item. This is not supported, as the graphs may be in an inconsistent state.");
			}
			if (!this.isScanned || this.nodes.Length != this.width * this.depth * this.LayerCount)
			{
				return null;
			}
			IntRect intRect;
			IntRect intRect2;
			IntRect intRect3;
			IntRect intRect4;
			GridGraph.GridGraphUpdatePromise.CalculateRectangles(this, this.GetRectFromBounds(bounds), out intRect, out intRect2, out intRect3, out intRect4);
			if (!intRect3.IsValid())
			{
				return null;
			}
			IntBounds intBounds = new IntBounds(intRect3.xmin, 0, intRect3.ymin, intRect3.xmax + 1, this.LayerCount, intRect3.ymax + 1);
			GridGraphNodeData gridGraphNodeData = new GridGraphNodeData
			{
				allocationMethod = Allocator.Persistent,
				bounds = intBounds,
				numNodes = intBounds.volume
			};
			gridGraphNodeData.AllocateBuffers(null);
			gridGraphNodeData.CopyFrom(this.nodeData, true, null);
			return new GridGraph.GridGraphSnapshot
			{
				nodes = gridGraphNodeData,
				graph = this
			};
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000217F4 File Offset: 0x0001F9F4
		public bool Linecast(Vector3 from, Vector3 to)
		{
			GraphHitInfo graphHitInfo;
			return this.Linecast(from, to, out graphHitInfo);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002180C File Offset: 0x0001FA0C
		[Obsolete("The hint parameter is deprecated")]
		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint)
		{
			GraphHitInfo graphHitInfo;
			return this.Linecast(from, to, hint, out graphHitInfo);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00021824 File Offset: 0x0001FA24
		[Obsolete("The hint parameter is deprecated")]
		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(from, to, hint, out hit, ref none, null);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00021845 File Offset: 0x0001FA45
		protected static long CrossMagnitude(int2 a, int2 b)
		{
			return (long)a.x * (long)b.y - (long)b.x * (long)a.y;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00021868 File Offset: 0x0001FA68
		protected bool ClipLineSegmentToBounds(Vector3 a, Vector3 b, out Vector3 outA, out Vector3 outB)
		{
			if (a.x < 0f || a.z < 0f || a.x > (float)this.width || a.z > (float)this.depth || b.x < 0f || b.z < 0f || b.x > (float)this.width || b.z > (float)this.depth)
			{
				Vector3 vector = new Vector3(0f, 0f, 0f);
				Vector3 vector2 = new Vector3(0f, 0f, (float)this.depth);
				Vector3 vector3 = new Vector3((float)this.width, 0f, (float)this.depth);
				Vector3 vector4 = new Vector3((float)this.width, 0f, 0f);
				int num = 0;
				bool flag;
				Vector3 vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector, vector2, out flag);
				if (flag)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector, vector2, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector2, vector3, out flag);
				if (flag)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector2, vector3, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3, vector4, out flag);
				if (flag)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector3, vector4, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector4, vector, out flag);
				if (flag)
				{
					num++;
					if (!VectorMath.RightOrColinearXZ(vector4, vector, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				if (num == 0)
				{
					outA = Vector3.zero;
					outB = Vector3.zero;
					return false;
				}
			}
			outA = a;
			outB = b;
			return true;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00021A24 File Offset: 0x0001FC24
		[Obsolete("The hint parameter is deprecated")]
		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace)
		{
			return this.Linecast(from, to, out hit, ref traversalConstraint, trace);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00021A34 File Offset: 0x0001FC34
		public bool Linecast(Vector3 from, Vector3 to, out GraphHitInfo hit)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(from, to, out hit, ref none, null);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00021A54 File Offset: 0x0001FC54
		public bool Linecast(Vector3 from, Vector3 to, out GraphHitInfo hit, List<GraphNode> trace = null)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(from, to, out hit, ref none, trace);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00021A74 File Offset: 0x0001FC74
		public bool Linecast(Vector3 from, Vector3 to, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace = null)
		{
			GridHitInfo gridHitInfo;
			bool flag = this.Linecast(from, to, out gridHitInfo, ref traversalConstraint, trace);
			hit = new GraphHitInfo
			{
				origin = from,
				node = gridHitInfo.node
			};
			if (!flag)
			{
				hit.point = to;
				return flag;
			}
			int direction = gridHitInfo.direction;
			if (direction == -1 || gridHitInfo.node == null)
			{
				hit.point = ((gridHitInfo.node == null || !traversalConstraint.CanTraverse(gridHitInfo.node)) ? from : to);
				if (gridHitInfo.node != null)
				{
					hit.point = gridHitInfo.node.ProjectOnSurface(hit.point);
				}
				hit.tangentOrigin = Vector3.zero;
				hit.tangent = Vector3.zero;
				return flag;
			}
			Vector3 vector = this.transform.InverseTransform(from);
			Vector3 vector2 = this.transform.InverseTransform(to);
			Vector2 vector3 = new Vector2(vector.x - 0.5f, vector.z - 0.5f);
			Vector2 vector4 = new Vector2(vector2.x - 0.5f, vector2.z - 0.5f);
			Vector2 vector5 = new Vector2((float)GridGraph.neighbourXOffsets[direction], (float)GridGraph.neighbourZOffsets[direction]);
			Vector2 vector6 = new Vector2((float)GridGraph.neighbourXOffsets[(direction - 1 + 4) & 3], (float)GridGraph.neighbourZOffsets[(direction - 1 + 4) & 3]);
			Vector2 vector7 = new Vector2((float)GridGraph.neighbourXOffsets[(direction + 1) & 3], (float)GridGraph.neighbourZOffsets[(direction + 1) & 3]);
			Vector2 vector8 = new Vector2((float)gridHitInfo.node.XCoordinateInGrid, (float)gridHitInfo.node.ZCoordinateInGrid) + (vector5 + vector6) * 0.5f;
			Vector2 vector9 = VectorMath.LineIntersectionPoint(vector8, vector8 + vector7, vector3, vector4);
			Vector3 vector10 = this.transform.InverseTransform((Vector3)gridHitInfo.node.position);
			Vector3 vector11 = new Vector3(vector9.x + 0.5f, vector10.y, vector9.y + 0.5f);
			Vector3 vector12 = new Vector3(vector8.x + 0.5f, vector10.y, vector8.y + 0.5f);
			hit.point = this.transform.Transform(vector11);
			hit.tangentOrigin = this.transform.Transform(vector12);
			hit.tangent = this.transform.TransformVector(new Vector3(vector7.x, 0f, vector7.y));
			return flag;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00021CE8 File Offset: 0x0001FEE8
		[Obsolete("Use Linecast instead")]
		public bool SnappedLinecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit)
		{
			return this.Linecast((Vector3)base.GetNearest(from, null).node.position, (Vector3)base.GetNearest(to, null).node.position, hint, out hit);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00021D24 File Offset: 0x0001FF24
		public bool Linecast(GridNodeBase fromNode, GridNodeBase toNode)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(fromNode, toNode, ref none);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00021D44 File Offset: 0x0001FF44
		public bool Linecast(GridNodeBase fromNode, GridNodeBase toNode, ref TraversalConstraint traversalConstraint)
		{
			int2 @int = new int2(512, 512);
			GridHitInfo gridHitInfo;
			return this.Linecast(fromNode, @int, toNode, @int, out gridHitInfo, ref traversalConstraint, null, false);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00021D74 File Offset: 0x0001FF74
		public bool Linecast(Vector3 from, Vector3 to, out GridHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace = null)
		{
			Vector3 vector = this.transform.InverseTransform(from);
			Vector3 vector2 = this.transform.InverseTransform(to);
			Vector3 vector3;
			Vector3 vector4;
			if (!this.ClipLineSegmentToBounds(vector, vector2, out vector3, out vector4))
			{
				hit = new GridHitInfo
				{
					node = null,
					direction = -1
				};
				return false;
			}
			if ((vector - vector3).sqrMagnitude > 1.0000001E-06f)
			{
				hit = new GridHitInfo
				{
					node = null,
					direction = -1
				};
				return true;
			}
			bool flag = (vector2 - vector4).sqrMagnitude > 1.0000001E-06f;
			GridNodeBase nearestFromGraphSpace = this.GetNearestFromGraphSpace(vector3);
			GridNodeBase nearestFromGraphSpace2 = this.GetNearestFromGraphSpace(vector4);
			if (nearestFromGraphSpace == null || nearestFromGraphSpace2 == null)
			{
				hit = new GridHitInfo
				{
					node = null,
					direction = -1
				};
				return false;
			}
			return this.Linecast(nearestFromGraphSpace, new Vector2(vector3.x - (float)nearestFromGraphSpace.XCoordinateInGrid, vector3.z - (float)nearestFromGraphSpace.ZCoordinateInGrid), nearestFromGraphSpace2, new Vector2(vector4.x - (float)nearestFromGraphSpace2.XCoordinateInGrid, vector4.z - (float)nearestFromGraphSpace2.ZCoordinateInGrid), out hit, ref traversalConstraint, trace, flag);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00021EB0 File Offset: 0x000200B0
		public bool Linecast(GridNodeBase fromNode, Vector2 normalizedFromPoint, GridNodeBase toNode, Vector2 normalizedToPoint, out GridHitInfo hit, List<GraphNode> trace = null, bool continuePastEnd = false)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(fromNode, normalizedFromPoint, toNode, normalizedToPoint, out hit, ref none, trace, continuePastEnd);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00021ED8 File Offset: 0x000200D8
		public bool Linecast(GridNodeBase fromNode, Vector2 normalizedFromPoint, GridNodeBase toNode, Vector2 normalizedToPoint, out GridHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace = null, bool continuePastEnd = false)
		{
			int2 @int = new int2((int)Mathf.Round(normalizedFromPoint.x * 1024f), (int)Mathf.Round(normalizedFromPoint.y * 1024f));
			int2 int2 = new int2((int)Mathf.Round(normalizedToPoint.x * 1024f), (int)Mathf.Round(normalizedToPoint.y * 1024f));
			return this.Linecast(fromNode, @int, toNode, int2, out hit, ref traversalConstraint, trace, continuePastEnd);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00021F50 File Offset: 0x00020150
		public bool Linecast(GridNodeBase fromNode, int2 fixedNormalizedFromPoint, GridNodeBase toNode, int2 fixedNormalizedToPoint, out GridHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace = null, bool continuePastEnd = false)
		{
			if (fixedNormalizedFromPoint.x < 0 || fixedNormalizedFromPoint.x > 1024)
			{
				throw new ArgumentOutOfRangeException("fixedNormalizedFromPoint", "must be between 0 and 1024");
			}
			if (fixedNormalizedToPoint.x < 0 || fixedNormalizedToPoint.x > 1024)
			{
				throw new ArgumentOutOfRangeException("fixedNormalizedToPoint", "must be between 0 and 1024");
			}
			if (fromNode == null)
			{
				throw new ArgumentNullException("fromNode");
			}
			if (toNode == null)
			{
				throw new ArgumentNullException("toNode");
			}
			if (!traversalConstraint.CanTraverse(fromNode))
			{
				hit = new GridHitInfo
				{
					node = fromNode,
					direction = -1
				};
				return true;
			}
			if (fromNode == toNode)
			{
				hit = new GridHitInfo
				{
					node = fromNode,
					direction = -1
				};
				if (trace != null)
				{
					trace.Add(fromNode);
				}
				return false;
			}
			int2 @int = new int2(fromNode.XCoordinateInGrid, fromNode.ZCoordinateInGrid);
			int2 int2 = new int2(toNode.XCoordinateInGrid, toNode.ZCoordinateInGrid);
			int2 int3 = new int2(@int.x * 1024, @int.y * 1024) + fixedNormalizedFromPoint;
			int2 int4 = new int2(int2.x * 1024, int2.y * 1024) + fixedNormalizedToPoint;
			int2 int5 = int4 - int3;
			int i = Math.Abs(@int.x - int2.x) + Math.Abs(@int.y - int2.y);
			if (continuePastEnd)
			{
				i = int.MaxValue;
			}
			if (math.all(int3 == int4))
			{
				i = 0;
			}
			int num = 0;
			int2 int6 = int5;
			if (int6.x == 0)
			{
				int6.x = Math.Sign(512 - fixedNormalizedToPoint.x);
			}
			if (int6.y == 0)
			{
				int6.y = Math.Sign(512 - fixedNormalizedToPoint.y);
			}
			if (int6.x <= 0 && int6.y > 0)
			{
				num = 1;
			}
			else if (int6.x < 0 && int6.y <= 0)
			{
				num = 2;
			}
			else if (int6.x >= 0 && int6.y < 0)
			{
				num = 3;
			}
			int num2 = (num + 1) & 3;
			int num3 = (num + 2) & 3;
			int num4 = num2 + 4;
			long num5 = GridGraph.CrossMagnitude(int5, new int2(GridGraph.neighbourXOffsets[num3] + GridGraph.neighbourXOffsets[num2], GridGraph.neighbourZOffsets[num3] + GridGraph.neighbourZOffsets[num2]));
			int2 int7 = new int2(512, 512) - fixedNormalizedFromPoint;
			long num6 = GridGraph.CrossMagnitude(int5, int7) * 2L / 1024L;
			long num7 = (long)(-int5.y * 2);
			long num8 = (long)(int5.x * 2);
			int num9 = num3;
			int num10 = num2;
			int2 int8 = new int2(int2.x * 1024, int2.y * 1024) + new int2(512, 512);
			if (GridGraph.CrossMagnitude(int5, int8 - int3) < 0L)
			{
				num9 = num2;
				num10 = num3;
			}
			GridNodeBase gridNodeBase = null;
			GridNodeBase gridNodeBase2 = null;
			while (i > 0)
			{
				if (trace != null)
				{
					trace.Add(fromNode);
				}
				long num11 = num6 + num5;
				int num12;
				GridNodeBase gridNodeBase3;
				if (num11 == 0L)
				{
					num12 = num4;
					gridNodeBase3 = ((i > 1) ? fromNode.GetNeighbourAlongDirection(num12) : null);
					if (gridNodeBase3 != null && (gridNodeBase3 == gridNodeBase || !traversalConstraint.CanTraverse(fromNode, gridNodeBase3)))
					{
						gridNodeBase3 = null;
					}
					if (gridNodeBase3 != null)
					{
						i--;
					}
					if (gridNodeBase3 == null)
					{
						num12 = num9;
						gridNodeBase3 = fromNode.GetNeighbourAlongDirection(num12);
						if (gridNodeBase3 != null && (gridNodeBase3 == gridNodeBase || !traversalConstraint.CanTraverse(fromNode, gridNodeBase3)))
						{
							gridNodeBase3 = null;
						}
					}
					if (gridNodeBase3 == null)
					{
						num12 = num10;
						gridNodeBase3 = fromNode.GetNeighbourAlongDirection(num12);
						if (gridNodeBase3 != null && (gridNodeBase3 == gridNodeBase || !traversalConstraint.CanTraverse(fromNode, gridNodeBase3)))
						{
							gridNodeBase3 = null;
						}
					}
				}
				else
				{
					num12 = ((num11 < 0L) ? num3 : num2);
					gridNodeBase3 = fromNode.GetNeighbourAlongDirection(num12);
					if (gridNodeBase3 != null && (gridNodeBase3 == gridNodeBase || !traversalConstraint.CanTraverse(fromNode, gridNodeBase3)))
					{
						gridNodeBase3 = null;
					}
				}
				if (gridNodeBase3 == null)
				{
					int j = -1;
					while (j <= 1)
					{
						int num13 = (num12 + j + 4) & 3;
						if (num6 + num7 / 2L * (long)(GridGraph.neighbourXOffsets[num12] + GridGraph.neighbourXOffsets[num13]) + num8 / 2L * (long)(GridGraph.neighbourZOffsets[num12] + GridGraph.neighbourZOffsets[num13]) == 0L)
						{
							gridNodeBase3 = fromNode.GetNeighbourAlongDirection(num13);
							if (gridNodeBase3 != null && (gridNodeBase3 == gridNodeBase || gridNodeBase3 == gridNodeBase2 || !traversalConstraint.CanTraverse(fromNode, gridNodeBase3)))
							{
								gridNodeBase3 = null;
							}
							if (gridNodeBase3 != null)
							{
								i = 1 + Math.Abs(gridNodeBase3.XCoordinateInGrid - int2.x) + Math.Abs(gridNodeBase3.ZCoordinateInGrid - int2.y);
								num12 = num13;
								gridNodeBase = fromNode;
								gridNodeBase2 = gridNodeBase3;
								break;
							}
							break;
						}
						else
						{
							j += 2;
						}
					}
					if (gridNodeBase3 == null)
					{
						hit = new GridHitInfo
						{
							node = fromNode,
							direction = num12
						};
						return true;
					}
				}
				num6 += num7 * (long)GridGraph.neighbourXOffsets[num12] + num8 * (long)GridGraph.neighbourZOffsets[num12];
				fromNode = gridNodeBase3;
				i--;
			}
			hit = new GridHitInfo
			{
				node = fromNode,
				direction = -1
			};
			if (fromNode != toNode)
			{
				int2 int9 = int4 - (new int2(fromNode.XCoordinateInGrid, fromNode.ZCoordinateInGrid) * 1024 + new int2(512, 512));
				if (math.all(math.abs(int9) == new int2(512, 512)))
				{
					int2 int10 = int9 * 2 / 1024;
					int num14 = -1;
					for (int k = 0; k < 4; k++)
					{
						if (GridGraph.neighbourXOffsets[k] + GridGraph.neighbourXOffsets[(k + 1) & 3] == int10.x && GridGraph.neighbourZOffsets[k] + GridGraph.neighbourZOffsets[(k + 1) & 3] == int10.y)
						{
							num14 = k;
							break;
						}
					}
					int num15 = ((trace != null) ? trace.Count : 0);
					int num16 = num14;
					GridNodeBase gridNodeBase4 = fromNode;
					int num17 = 0;
					while (num17 < 3 && gridNodeBase4 != toNode)
					{
						if (trace != null)
						{
							trace.Add(gridNodeBase4);
						}
						GridNodeBase neighbourAlongDirection = gridNodeBase4.GetNeighbourAlongDirection(num16);
						if (neighbourAlongDirection == null || !traversalConstraint.CanTraverse(gridNodeBase4, neighbourAlongDirection))
						{
							gridNodeBase4 = null;
							break;
						}
						gridNodeBase4 = neighbourAlongDirection;
						num16 = (num16 + 1) & 3;
						num17++;
					}
					if (gridNodeBase4 != toNode)
					{
						if (trace != null)
						{
							trace.RemoveRange(num15, trace.Count - num15);
						}
						gridNodeBase4 = fromNode;
						num16 = (num14 + 1) & 3;
						int num18 = 0;
						while (num18 < 3 && gridNodeBase4 != toNode)
						{
							if (trace != null)
							{
								trace.Add(gridNodeBase4);
							}
							GridNodeBase neighbourAlongDirection2 = gridNodeBase4.GetNeighbourAlongDirection(num16);
							if (neighbourAlongDirection2 == null || !traversalConstraint.CanTraverse(gridNodeBase4, neighbourAlongDirection2))
							{
								gridNodeBase4 = null;
								break;
							}
							gridNodeBase4 = neighbourAlongDirection2;
							num16 = (num16 - 1 + 4) & 3;
							num18++;
						}
						if (gridNodeBase4 != toNode && trace != null)
						{
							trace.RemoveRange(num15, trace.Count - num15);
						}
					}
					fromNode = gridNodeBase4;
				}
			}
			if (trace != null)
			{
				trace.Add(fromNode);
			}
			return fromNode != toNode;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00022654 File Offset: 0x00020854
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(Vector3 from, Vector3 to, out GridHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(from, to, out hit, ref none, trace);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00022680 File Offset: 0x00020880
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(GridNodeBase fromNode, GridNodeBase toNode, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(fromNode, toNode, ref none);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000226A8 File Offset: 0x000208A8
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(Vector3 from, Vector3 to, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(from, to, out hit, ref none, trace);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000226D4 File Offset: 0x000208D4
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(Vector3 from, Vector3 to, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(from, to, hint, out hit, ref none, trace);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00022700 File Offset: 0x00020900
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.nodes.Length);
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i].SerializeNode(ctx);
			}
			this.SerializeNodeSurfaceNormals(ctx);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00022760 File Offset: 0x00020960
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			GridNodeBase[] array = new GridNode[num];
			this.nodes = array;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i] = this.newGridNodeDelegate();
				this.active.InitializeNode(this.nodes[i]);
				this.nodes[i].DeserializeNode(ctx);
			}
			this.DeserializeNativeData(ctx, ctx.meta.version >= AstarSerializer.V4_3_6);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000227F8 File Offset: 0x000209F8
		protected void DeserializeNativeData(GraphSerializationContext ctx, bool normalsSerialized)
		{
			this.UpdateTransform();
			JobDependencyTracker jobDependencyTracker = ObjectPool<JobDependencyTracker>.Claim();
			bool flag = this is LayerGridGraph;
			int3 @int = new int3(this.width, this.LayerCount, this.depth);
			this.nodeData = GridGraphNodeData.ReadFromNodes(this.nodes, new Slice3D(@int, new IntBounds(0, @int)), default(JobHandle), default(NativeArray<float4>), Allocator.Persistent, flag, jobDependencyTracker);
			this.nodeData.PersistBuffers(jobDependencyTracker);
			this.DeserializeNodeSurfaceNormals(ctx, this.nodes, !normalsSerialized);
			jobDependencyTracker.AllWritesDependency.Complete();
			ObjectPool<JobDependencyTracker>.Release(ref jobDependencyTracker);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000228A0 File Offset: 0x00020AA0
		protected void SerializeNodeSurfaceNormals(GraphSerializationContext ctx)
		{
			UnsafeSpan<float4> unsafeSpan = this.nodeData.normals.AsUnsafeReadOnlySpan<float4>();
			for (int i = 0; i < this.nodes.Length; i++)
			{
				ctx.SerializeVector3(new Vector3(unsafeSpan[i].x, unsafeSpan[i].y, unsafeSpan[i].z));
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00022904 File Offset: 0x00020B04
		protected void DeserializeNodeSurfaceNormals(GraphSerializationContext ctx, GridNodeBase[] nodes, bool ignoreForCompatibility)
		{
			if (this.nodeData.normals.IsCreated)
			{
				this.nodeData.normals.Dispose();
			}
			this.nodeData.normals = new NativeArray<float4>(nodes.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			if (ignoreForCompatibility)
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					this.nodeData.normals[i] = ((nodes[i] != null) ? new float4(0f, 1f, 0f, 0f) : float4.zero);
				}
				return;
			}
			for (int j = 0; j < nodes.Length; j++)
			{
				Vector3 vector = ctx.DeserializeVector3();
				this.nodeData.normals[j] = new float4(vector.x, vector.y, vector.z, 0f);
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000229D4 File Offset: 0x00020BD4
		private void HandleBackwardsCompatibility(GraphSerializationContext ctx)
		{
			if (ctx.meta.version <= AstarSerializer.V4_3_2)
			{
				this.maxStepUsesSlope = false;
			}
			if (this.penaltyPosition)
			{
				this.penaltyPosition = false;
				this.rules.AddRule(new RuleElevationPenalty
				{
					penaltyScale = 1000f * this.penaltyPositionFactor * 1000f,
					elevationRange = new Vector2(-this.penaltyPositionOffset / 1000f, -this.penaltyPositionOffset / 1000f + 1000f),
					curve = AnimationCurve.Linear(0f, 0f, 1f, 1f)
				});
			}
			if (this.penaltyAngle)
			{
				this.penaltyAngle = false;
				AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
				Keyframe[] array = new Keyframe[7];
				for (int i = 0; i < array.Length; i++)
				{
					float num = 1.5707964f * (float)i / (float)(array.Length - 1);
					float num2 = (1f - Mathf.Pow(Mathf.Cos(num), this.penaltyAnglePower)) * this.penaltyAngleFactor;
					Keyframe keyframe = new Keyframe(57.29578f * num, num2);
					array[i] = keyframe;
				}
				float num3 = array.Max<Keyframe>((Keyframe k) => k.value);
				if (num3 > 0f)
				{
					for (int j = 0; j < array.Length; j++)
					{
						Keyframe[] array2 = array;
						int num4 = j;
						array2[num4].value = array2[num4].value / num3;
					}
				}
				animationCurve.keys = array;
				for (int l = 0; l < array.Length; l++)
				{
					animationCurve.SmoothTangents(l, 0.5f);
				}
				this.rules.AddRule(new RuleAnglePenalty
				{
					penaltyScale = num3,
					curve = animationCurve
				});
			}
			if (this.textureData.enabled)
			{
				this.textureData.enabled = false;
				List<float> list = this.textureData.factors.Select<float, float>((float x) => x / 255f).ToList<float>();
				while (list.Count < 4)
				{
					list.Add(1000f);
				}
				List<RuleTexture.ChannelUse> list2 = this.textureData.channels.Cast<RuleTexture.ChannelUse>().ToList<RuleTexture.ChannelUse>();
				while (list2.Count < 4)
				{
					list2.Add(RuleTexture.ChannelUse.None);
				}
				this.rules.AddRule(new RuleTexture
				{
					texture = this.textureData.source,
					channels = list2.ToArray(),
					channelScales = list.ToArray(),
					scalingMode = RuleTexture.ScalingMode.FixedScale,
					nodesPerPixel = 1f
				});
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00022C88 File Offset: 0x00020E88
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			this.HandleBackwardsCompatibility(ctx);
			this.UpdateTransform();
			this.SetUpOffsetsAndCosts();
			GridNode.SetGridGraph((int)this.graphIndex, this);
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			if (this.width * this.depth != this.nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
				this.nodes = new GridNodeBase[0];
				return;
			}
			for (int i = 0; i < this.depth; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					GridNodeBase gridNodeBase = this.nodes[i * this.width + j];
					if (gridNodeBase == null)
					{
						Debug.LogError("Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator");
						return;
					}
					gridNodeBase.NodeInGridIndex = i * this.width + j;
				}
			}
		}

		// Token: 0x04000445 RID: 1093
		[JsonMember]
		public InspectorGridMode inspectorGridMode;

		// Token: 0x04000446 RID: 1094
		[JsonMember]
		public InspectorGridHexagonNodeSize inspectorHexagonSizeMode;

		// Token: 0x04000447 RID: 1095
		public int width;

		// Token: 0x04000448 RID: 1096
		public int depth;

		// Token: 0x04000449 RID: 1097
		[JsonMember]
		public float aspectRatio = 1f;

		// Token: 0x0400044A RID: 1098
		[JsonMember]
		public float isometricAngle;

		// Token: 0x0400044B RID: 1099
		public static readonly float StandardIsometricAngle = 90f - Mathf.Atan(1f / Mathf.Sqrt(2f)) * 57.29578f;

		// Token: 0x0400044C RID: 1100
		public static readonly float StandardDimetricAngle = Mathf.Acos(0.5f) * 57.29578f;

		// Token: 0x0400044D RID: 1101
		[JsonMember]
		public bool uniformEdgeCosts;

		// Token: 0x0400044E RID: 1102
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x0400044F RID: 1103
		[JsonMember]
		public Vector3 center;

		// Token: 0x04000450 RID: 1104
		[JsonMember]
		public Vector2 unclampedSize = new Vector2(10f, 10f);

		// Token: 0x04000451 RID: 1105
		[JsonMember]
		public float nodeSize = 1f;

		// Token: 0x04000452 RID: 1106
		[JsonMember]
		public GraphCollision collision = new GraphCollision();

		// Token: 0x04000453 RID: 1107
		[JsonMember]
		public float maxStepHeight = 0.4f;

		// Token: 0x04000454 RID: 1108
		[JsonMember]
		public bool maxStepUsesSlope = true;

		// Token: 0x04000455 RID: 1109
		[JsonMember]
		public float maxSlope = 90f;

		// Token: 0x04000456 RID: 1110
		[JsonMember]
		public int erodeIterations;

		// Token: 0x04000457 RID: 1111
		[JsonMember]
		public bool erosionUseTags;

		// Token: 0x04000458 RID: 1112
		[JsonMember]
		public int erosionFirstTag = 1;

		// Token: 0x04000459 RID: 1113
		[JsonMember]
		public int erosionTagsPrecedenceMask = -1;

		// Token: 0x0400045A RID: 1114
		[JsonMember]
		public NumNeighbours neighbours = NumNeighbours.Eight;

		// Token: 0x0400045B RID: 1115
		[JsonMember]
		public bool cutCorners = true;

		// Token: 0x0400045C RID: 1116
		[JsonMember]
		[Obsolete("Use the RuleElevationPenalty class instead")]
		public float penaltyPositionOffset;

		// Token: 0x0400045D RID: 1117
		[JsonMember]
		[Obsolete("Use the RuleElevationPenalty class instead")]
		public bool penaltyPosition;

		// Token: 0x0400045E RID: 1118
		[JsonMember]
		[Obsolete("Use the RuleElevationPenalty class instead")]
		public float penaltyPositionFactor = 1f;

		// Token: 0x0400045F RID: 1119
		[JsonMember]
		[Obsolete("Use the RuleAnglePenalty class instead")]
		public bool penaltyAngle;

		// Token: 0x04000460 RID: 1120
		[JsonMember]
		[Obsolete("Use the RuleAnglePenalty class instead")]
		public float penaltyAngleFactor = 100f;

		// Token: 0x04000461 RID: 1121
		[JsonMember]
		[Obsolete("Use the RuleAnglePenalty class instead")]
		public float penaltyAnglePower = 1f;

		// Token: 0x04000462 RID: 1122
		[JsonMember]
		public GridGraphRules rules = new GridGraphRules();

		// Token: 0x04000463 RID: 1123
		[JsonMember]
		public bool showMeshOutline = true;

		// Token: 0x04000464 RID: 1124
		[JsonMember]
		public bool showNodeConnections;

		// Token: 0x04000465 RID: 1125
		[JsonMember]
		public bool showMeshSurface = true;

		// Token: 0x04000466 RID: 1126
		[JsonMember]
		[Obsolete("Use the RuleTexture class instead")]
		public GridGraph.TextureData textureData = new GridGraph.TextureData();

		// Token: 0x04000468 RID: 1128
		[NonSerialized]
		public readonly int[] neighbourOffsets = new int[8];

		// Token: 0x04000469 RID: 1129
		[NonSerialized]
		public readonly float[] neighbourCosts = new float[8];

		// Token: 0x0400046A RID: 1130
		public static readonly int[] neighbourXOffsets = new int[] { 0, 1, 0, -1, 1, 1, -1, -1 };

		// Token: 0x0400046B RID: 1131
		public static readonly int[] neighbourZOffsets = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };

		// Token: 0x0400046C RID: 1132
		internal static readonly int[] hexagonNeighbourIndices = new int[] { 0, 1, 5, 2, 3, 7 };

		// Token: 0x0400046D RID: 1133
		internal static readonly int[] axisAlignedNeighbourIndices = new int[] { 0, 1, 2, 3 };

		// Token: 0x0400046E RID: 1134
		internal static readonly int[] allNeighbourIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

		// Token: 0x0400046F RID: 1135
		internal const int HexagonConnectionMask = 175;

		// Token: 0x04000470 RID: 1136
		public GridNodeBase[] nodes;

		// Token: 0x04000471 RID: 1137
		protected GridGraphNodeData nodeData;

		// Token: 0x04000473 RID: 1139
		protected Func<GridNodeBase> newGridNodeDelegate = () => new GridNode();

		// Token: 0x04000474 RID: 1140
		public const int FixedPrecisionScale = 1024;

		// Token: 0x020000C9 RID: 201
		public class TextureData
		{
			// Token: 0x060006FF RID: 1791 RVA: 0x00022F14 File Offset: 0x00021114
			public void Initialize()
			{
				if (this.enabled && this.source != null)
				{
					for (int i = 0; i < this.channels.Length; i++)
					{
						if (this.channels[i] != GridGraph.TextureData.ChannelUse.None)
						{
							try
							{
								this.data = this.source.GetPixels32();
								break;
							}
							catch (UnityException ex)
							{
								Debug.LogWarning(ex.ToString());
								this.data = null;
								break;
							}
						}
					}
				}
			}

			// Token: 0x06000700 RID: 1792 RVA: 0x00022F8C File Offset: 0x0002118C
			public void Apply(GridNode node, int x, int z)
			{
				if (this.enabled && this.data != null && x < this.source.width && z < this.source.height)
				{
					Color32 color = this.data[z * this.source.width + x];
					if (this.channels[0] != GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.r, this.channels[0], this.factors[0]);
					}
					if (this.channels[1] != GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.g, this.channels[1], this.factors[1]);
					}
					if (this.channels[2] != GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.b, this.channels[2], this.factors[2]);
					}
					node.WalkableErosion = node.Walkable;
				}
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x00023074 File Offset: 0x00021274
			private void ApplyChannel(GridNode node, int x, int z, int value, GridGraph.TextureData.ChannelUse channelUse, float factor)
			{
				switch (channelUse)
				{
				case GridGraph.TextureData.ChannelUse.Penalty:
					node.Penalty += (uint)Mathf.RoundToInt((float)value * factor);
					return;
				case GridGraph.TextureData.ChannelUse.Position:
					node.position = GridNode.GetGridGraph(node.GraphIndex).GraphPointToWorld(x, z, (float)value);
					return;
				case GridGraph.TextureData.ChannelUse.WalkablePenalty:
					if (value == 0)
					{
						node.Walkable = false;
						return;
					}
					node.Penalty += (uint)Mathf.RoundToInt((float)(value - 1) * factor);
					return;
				default:
					return;
				}
			}

			// Token: 0x04000475 RID: 1141
			public bool enabled;

			// Token: 0x04000476 RID: 1142
			public Texture2D source;

			// Token: 0x04000477 RID: 1143
			public float[] factors = new float[3];

			// Token: 0x04000478 RID: 1144
			public GridGraph.TextureData.ChannelUse[] channels = new GridGraph.TextureData.ChannelUse[3];

			// Token: 0x04000479 RID: 1145
			private Color32[] data;

			// Token: 0x020000CA RID: 202
			public enum ChannelUse
			{
				// Token: 0x0400047B RID: 1147
				None,
				// Token: 0x0400047C RID: 1148
				Penalty,
				// Token: 0x0400047D RID: 1149
				Position,
				// Token: 0x0400047E RID: 1150
				WalkablePenalty
			}
		}

		// Token: 0x020000CB RID: 203
		public enum RecalculationMode
		{
			// Token: 0x04000480 RID: 1152
			RecalculateFromScratch,
			// Token: 0x04000481 RID: 1153
			RecalculateMinimal,
			// Token: 0x04000482 RID: 1154
			NoRecalculation
		}

		// Token: 0x020000CC RID: 204
		private class GridGraphMovePromise : IGraphUpdatePromise
		{
			// Token: 0x06000703 RID: 1795 RVA: 0x00023114 File Offset: 0x00021314
			private static void DecomposeInsetsToRectangles(int width, int height, int insetLeft, int insetRight, int insetBottom, int insetTop, IntRect[] output)
			{
				output[0] = new IntRect(0, 0, insetLeft - 1, height - 1);
				output[1] = new IntRect(width - insetRight, 0, width - 1, height - 1);
				output[2] = new IntRect(insetLeft, 0, width - insetRight - 1, insetBottom - 1);
				output[3] = new IntRect(insetLeft, height - insetTop - 1, width - insetRight - 1, height - 1);
			}

			// Token: 0x06000704 RID: 1796 RVA: 0x00023184 File Offset: 0x00021384
			public GridGraphMovePromise(GridGraph graph, int dx, int dz)
			{
				this.graph = graph;
				this.dx = dx;
				this.dz = dz;
				GraphTransform graphTransform = graph.transform * Matrix4x4.Translate(new Vector3((float)dx, 0f, (float)dz));
				this.startingSize = new int3(graph.width, graph.LayerCount, graph.depth);
				if (math.abs(dx) > graph.width / 2 || math.abs(dz) > graph.depth / 2)
				{
					this.rects = new IntRect[]
					{
						new IntRect(0, 0, graph.width - 1, graph.depth - 1)
					};
				}
				else
				{
					int num = math.max(1, -dx);
					int num2 = math.max(1, dx);
					int num3 = math.max(1, -dz);
					int num4 = math.max(1, dz);
					this.rects = new IntRect[4];
					GridGraph.GridGraphMovePromise.DecomposeInsetsToRectangles(graph.width, graph.depth, num, num2, num3, num4, this.rects);
				}
				IGraphUpdatePromise[] array = new GridGraph.GridGraphUpdatePromise[this.rects.Length];
				this.promises = array;
				GridGraph.GridGraphUpdatePromise.NodesHolder nodesHolder = new GridGraph.GridGraphUpdatePromise.NodesHolder
				{
					nodes = graph.nodes
				};
				for (int i = 0; i < this.rects.Length; i++)
				{
					JobDependencyTracker jobDependencyTracker = ObjectPool<JobDependencyTracker>.Claim();
					this.promises[i] = new GridGraph.GridGraphUpdatePromise(graph, graphTransform, nodesHolder, this.startingSize, this.rects[i], jobDependencyTracker, default(JobHandle), Allocator.Persistent, GridGraph.RecalculationMode.RecalculateMinimal, null, true, false);
				}
			}

			// Token: 0x06000705 RID: 1797 RVA: 0x000232FD File Offset: 0x000214FD
			public IEnumerator<JobHandle> Prepare()
			{
				yield return this.graph.nodeData.Rotate2D(-this.dx, -this.dz, default(JobHandle));
				int num;
				for (int i = 0; i < this.promises.Length; i = num + 1)
				{
					IEnumerator<JobHandle> it = this.promises[i].Prepare();
					while (it.MoveNext())
					{
						JobHandle jobHandle = it.Current;
						yield return jobHandle;
					}
					it = null;
					num = i;
				}
				yield break;
			}

			// Token: 0x06000706 RID: 1798 RVA: 0x0002330C File Offset: 0x0002150C
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.AssertSafeToUpdateGraph();
				GridNodeBase[] nodes = this.graph.nodes;
				if (!math.all(new int3(this.graph.width, this.graph.LayerCount, this.graph.depth) == this.startingSize))
				{
					throw new InvalidOperationException("The graph has been resized since the update was created. This is not allowed.");
				}
				if (nodes == null || nodes.Length != this.graph.width * this.graph.depth * this.graph.LayerCount)
				{
					throw new InvalidOperationException("The Grid Graph is not scanned, cannot recalculate connections.");
				}
				Memory.Rotate3DArray<GridNodeBase>(nodes, this.startingSize, -this.dx, -this.dz);
				for (int i = 0; i < this.startingSize.y; i++)
				{
					int num = i * this.startingSize.x * this.startingSize.z;
					for (int j = 0; j < this.startingSize.z; j++)
					{
						int num2 = j * this.startingSize.x;
						for (int k = 0; k < this.startingSize.x; k++)
						{
							int num3 = num2 + k;
							GridNodeBase gridNodeBase = nodes[num + num3];
							if (gridNodeBase != null)
							{
								gridNodeBase.NodeInGridIndex = num3;
							}
						}
					}
				}
				int layerCount = this.graph.LayerCount;
				for (int l = 0; l < this.rects.Length; l++)
				{
					IntRect intRect = this.rects[l];
					for (int m = 0; m < layerCount; m++)
					{
						int num4 = m * this.graph.width * this.graph.depth;
						for (int n = intRect.ymin; n <= intRect.ymax; n++)
						{
							int num5 = n * this.graph.width + num4;
							for (int num6 = intRect.xmin; num6 <= intRect.xmax; num6++)
							{
								GridNodeBase gridNodeBase2 = nodes[num5 + num6];
								if (gridNodeBase2 != null)
								{
									gridNodeBase2.ClearCustomConnections(true);
								}
							}
						}
					}
				}
				for (int num7 = 0; num7 < this.promises.Length; num7++)
				{
					this.promises[num7].Apply(ctx);
				}
				this.graph.center += this.graph.transform.TransformVector(new Vector3((float)this.dx, 0f, (float)this.dz));
				this.graph.UpdateTransform();
				if (this.promises.Length != 0)
				{
					this.graph.rules.ExecuteRuleMainThread(GridGraphRule.Pass.AfterApplied, (this.promises[0] as GridGraph.GridGraphUpdatePromise).context);
				}
			}

			// Token: 0x04000483 RID: 1155
			public GridGraph graph;

			// Token: 0x04000484 RID: 1156
			public int dx;

			// Token: 0x04000485 RID: 1157
			public int dz;

			// Token: 0x04000486 RID: 1158
			private IGraphUpdatePromise[] promises;

			// Token: 0x04000487 RID: 1159
			private IntRect[] rects;

			// Token: 0x04000488 RID: 1160
			private int3 startingSize;
		}

		// Token: 0x020000CE RID: 206
		private class GridGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x0600070D RID: 1805 RVA: 0x000236CD File Offset: 0x000218CD
			public int CostEstimate
			{
				get
				{
					return this.fullRecalculationBounds.volume;
				}
			}

			// Token: 0x0600070E RID: 1806 RVA: 0x000236DC File Offset: 0x000218DC
			public GridGraphUpdatePromise(GridGraph graph, GraphTransform transform, GridGraph.GridGraphUpdatePromise.NodesHolder nodes, int3 nodeArrayBounds, IntRect rect, JobDependencyTracker dependencyTracker, JobHandle nodesDependsOn, Allocator allocationMethod, GridGraph.RecalculationMode recalculationMode, GraphUpdateObject graphUpdateObject, bool ownsJobDependencyTracker, bool isFinalUpdate)
			{
				this.graph = graph;
				this.transform = transform;
				this.nodes = nodes;
				this.nodeArrayBounds = nodeArrayBounds;
				this.dependencyTracker = dependencyTracker;
				this.nodesDependsOn = nodesDependsOn;
				this.allocationMethod = allocationMethod;
				this.recalculationMode = recalculationMode;
				this.graphUpdateObject = graphUpdateObject;
				this.ownsJobDependencyTracker = ownsJobDependencyTracker;
				this.isFinalUpdate = isFinalUpdate;
				IntRect intRect;
				IntRect intRect2;
				IntRect intRect3;
				GridGraph.GridGraphUpdatePromise.CalculateRectangles(graph, rect, out this.rect, out intRect, out intRect2, out intRect3);
				if (recalculationMode == GridGraph.RecalculationMode.RecalculateFromScratch)
				{
					intRect = intRect3;
				}
				if (!intRect.IsValid())
				{
					this.emptyUpdate = true;
				}
				this.readBounds = new IntBounds(intRect3.xmin, 0, intRect3.ymin, intRect3.xmax + 1, nodeArrayBounds.y, intRect3.ymax + 1);
				this.fullRecalculationBounds = new IntBounds(intRect.xmin, 0, intRect.ymin, intRect.xmax + 1, nodeArrayBounds.y, intRect.ymax + 1);
				this.writeMaskBounds = new IntBounds(intRect2.xmin, 0, intRect2.ymin, intRect2.xmax + 1, nodeArrayBounds.y, intRect2.ymax + 1);
				if (ownsJobDependencyTracker)
				{
					dependencyTracker.SetLinearDependencies(this.CostEstimate < 500);
				}
			}

			// Token: 0x0600070F RID: 1807 RVA: 0x00023814 File Offset: 0x00021A14
			public static void CalculateRectangles(GridGraph graph, IntRect rect, out IntRect originalRect, out IntRect fullRecalculationRect, out IntRect writeMaskRect, out IntRect readRect)
			{
				fullRecalculationRect = rect;
				GraphCollision collision = graph.collision;
				if (collision.collisionCheck && collision.type != ColliderType.Ray)
				{
					fullRecalculationRect = fullRecalculationRect.Expand(Mathf.FloorToInt(collision.diameter * 0.5f + 0.5f));
				}
				writeMaskRect = fullRecalculationRect.Expand(graph.erodeIterations + 1);
				readRect = writeMaskRect.Expand(graph.erodeIterations + 1);
				IntRect intRect = new IntRect(0, 0, graph.width - 1, graph.depth - 1);
				readRect = IntRect.Intersection(readRect, intRect);
				fullRecalculationRect = IntRect.Intersection(fullRecalculationRect, intRect);
				writeMaskRect = IntRect.Intersection(writeMaskRect, intRect);
				originalRect = IntRect.Intersection(rect, intRect);
			}

			// Token: 0x06000710 RID: 1808 RVA: 0x000238EE File Offset: 0x00021AEE
			public IEnumerator<JobHandle> Prepare()
			{
				if (this.emptyUpdate)
				{
					yield break;
				}
				GraphCollision collision = this.graph.collision;
				GridGraphRules rules = this.graph.rules;
				if (this.recalculationMode != GridGraph.RecalculationMode.RecalculateFromScratch)
				{
					this.writeMaskBounds.max.y = (this.fullRecalculationBounds.max.y = (this.readBounds.max.y = this.graph.nodeData.bounds.max.y));
				}
				int minLayers = ((this.recalculationMode == GridGraph.RecalculationMode.RecalculateFromScratch) ? 1 : this.fullRecalculationBounds.max.y);
				if (this.recalculationMode == GridGraph.RecalculationMode.RecalculateMinimal && this.readBounds == this.fullRecalculationBounds)
				{
					this.recalculationMode = GridGraph.RecalculationMode.RecalculateFromScratch;
				}
				bool layeredDataLayout = this.graph is LayerGridGraph;
				LayerGridGraph layerGridGraph = this.graph as LayerGridGraph;
				float characterHeight = ((layerGridGraph != null) ? layerGridGraph.characterHeight : float.PositiveInfinity);
				this.context = new GridGraphRules.Context
				{
					graph = this.graph,
					data = new GridGraphScanData
					{
						dependencyTracker = this.dependencyTracker,
						transform = this.transform,
						up = this.transform.TransformVector(Vector3.up).normalized
					}
				};
				IEnumerator<JobHandle> wait;
				if (this.recalculationMode == GridGraph.RecalculationMode.RecalculateFromScratch || this.recalculationMode == GridGraph.RecalculationMode.RecalculateMinimal)
				{
					if (collision.heightCheck && !collision.use2D)
					{
						NativeArray<int> layerCount = this.dependencyTracker.NewNativeArray<int>(1, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
						float num = ((this.graph.neighbours == NumNeighbours.Six) ? GridGraph.ConvertNodeSizeToHexagonSize(InspectorGridHexagonNodeSize.Width, this.graph.nodeSize) : this.graph.nodeSize);
						yield return this.context.data.HeightCheck(collision, num, this.graph.MaxLayers, this.fullRecalculationBounds, layerCount, characterHeight, this.allocationMethod);
						int num2 = Mathf.Max(minLayers, layerCount[0]);
						this.readBounds.max.y = (this.fullRecalculationBounds.max.y = (this.writeMaskBounds.max.y = num2));
						this.context.data.heightHitsBounds.max.y = layerCount[0];
						this.context.data.nodes = new GridGraphNodeData
						{
							bounds = this.fullRecalculationBounds,
							numNodes = this.fullRecalculationBounds.volume,
							layeredDataLayout = layeredDataLayout,
							allocationMethod = this.allocationMethod
						};
						this.context.data.nodes.AllocateBuffers(this.dependencyTracker);
						this.context.data.SetDefaultNodePositions(this.transform);
						this.context.data.CopyHits(this.context.data.heightHitsBounds);
						this.context.data.CalculateWalkabilityFromHeightData(this.graph.useRaycastNormal, collision.unwalkableWhenNoGround, this.graph.maxSlope, characterHeight);
						layerCount = default(NativeArray<int>);
					}
					else
					{
						this.context.data.nodes = new GridGraphNodeData
						{
							bounds = this.fullRecalculationBounds,
							numNodes = this.fullRecalculationBounds.volume,
							layeredDataLayout = layeredDataLayout,
							allocationMethod = this.allocationMethod
						};
						this.context.data.nodes.AllocateBuffers(this.dependencyTracker);
						this.context.data.SetDefaultNodePositions(this.transform);
						this.context.data.nodes.walkable.MemSet(true).Schedule(this.dependencyTracker);
						this.context.data.nodes.normals.MemSet(new float4(this.context.data.up.x, this.context.data.up.y, this.context.data.up.z, 0f)).Schedule(this.dependencyTracker);
					}
					this.context.data.SetDefaultPenalties(this.graph.initialPenalty);
					JobHandle.ScheduleBatchedJobs();
					rules.RebuildIfNecessary();
					wait = rules.ExecuteRule(GridGraphRule.Pass.BeforeCollision, this.context);
					while (wait.MoveNext())
					{
						JobHandle jobHandle = wait.Current;
						yield return jobHandle;
					}
					wait = null;
					if (collision.collisionCheck)
					{
						this.context.tracker.timeSlice = TimeSlice.MillisFromNow(1f);
						wait = this.context.data.CollisionCheck(collision, this.fullRecalculationBounds);
						while (wait != null && wait.MoveNext())
						{
							yield return wait.Current;
							this.context.tracker.timeSlice = TimeSlice.MillisFromNow(2f);
						}
						wait = null;
					}
					wait = rules.ExecuteRule(GridGraphRule.Pass.BeforeConnections, this.context);
					while (wait.MoveNext())
					{
						JobHandle jobHandle2 = wait.Current;
						yield return jobHandle2;
					}
					wait = null;
					if (this.recalculationMode == GridGraph.RecalculationMode.RecalculateMinimal)
					{
						GridGraphNodeData gridGraphNodeData = new GridGraphNodeData
						{
							bounds = this.readBounds,
							numNodes = this.readBounds.volume,
							layeredDataLayout = layeredDataLayout,
							allocationMethod = this.allocationMethod
						};
						gridGraphNodeData.AllocateBuffers(this.dependencyTracker);
						gridGraphNodeData.normals.MemSet(float4.zero).Schedule(this.dependencyTracker);
						gridGraphNodeData.walkable.MemSet(false).Schedule(this.dependencyTracker);
						gridGraphNodeData.walkableWithErosion.MemSet(false).Schedule(this.dependencyTracker);
						gridGraphNodeData.CopyFrom(this.graph.nodeData, true, this.dependencyTracker);
						gridGraphNodeData.CopyFrom(this.context.data.nodes, this.graphUpdateObject == null || this.graphUpdateObject.resetPenaltyOnPhysics, this.dependencyTracker);
						this.context.data.nodes = gridGraphNodeData;
					}
				}
				else
				{
					this.context.data.nodes = new GridGraphNodeData
					{
						bounds = this.readBounds,
						numNodes = this.readBounds.volume,
						layeredDataLayout = layeredDataLayout,
						allocationMethod = this.allocationMethod
					};
					this.context.data.nodes.AllocateBuffers(this.dependencyTracker);
					this.context.data.nodes.CopyFrom(this.graph.nodeData, true, this.dependencyTracker);
				}
				if (this.graphUpdateObject != null)
				{
					if (this.graphUpdateObject.GetType() != typeof(GraphUpdateObject))
					{
						GridNodeBase[] array = this.nodes.nodes;
						for (int i = this.writeMaskBounds.min.y; i < this.writeMaskBounds.max.y; i++)
						{
							for (int j = this.writeMaskBounds.min.z; j < this.writeMaskBounds.max.z; j++)
							{
								int num3 = i * this.nodeArrayBounds.x * this.nodeArrayBounds.z + j * this.nodeArrayBounds.x;
								for (int k = this.writeMaskBounds.min.x; k < this.writeMaskBounds.max.x; k++)
								{
									this.graphUpdateObject.WillUpdateNode(array[num3 + k]);
								}
							}
						}
					}
					IntRect intRect = this.rect;
					if (intRect.IsValid())
					{
						IntBounds intBounds = new IntBounds(intRect.xmin, 0, intRect.ymin, intRect.xmax + 1, this.context.data.nodes.layers, intRect.ymax + 1).Offset(-this.context.data.nodes.bounds.min);
						NativeArray<int> nativeArray = this.dependencyTracker.NewNativeArray<int>(intBounds.volume, this.context.data.nodes.allocationMethod, NativeArrayOptions.ClearMemory);
						int num4 = 0;
						int3 size = this.context.data.nodes.bounds.size;
						for (int l = intBounds.min.y; l < intBounds.max.y; l++)
						{
							for (int m = intBounds.min.z; m < intBounds.max.z; m++)
							{
								int num5 = l * size.x * size.z + m * size.x;
								for (int n = intBounds.min.x; n < intBounds.max.x; n++)
								{
									nativeArray[num4++] = num5 + n;
								}
							}
						}
						this.graphUpdateObject.ApplyJob(new GraphUpdateObject.GraphUpdateData
						{
							nodePositions = this.context.data.nodes.positions,
							nodePenalties = this.context.data.nodes.penalties,
							nodeWalkable = this.context.data.nodes.walkable,
							nodeNormals = this.context.data.nodes.normals,
							nodeTags = this.context.data.nodes.tags,
							nodeIndices = nativeArray
						}, this.dependencyTracker);
					}
				}
				this.context.data.Connections(this.graph.maxStepHeight, this.graph.maxStepUsesSlope, this.context.data.nodes.bounds, this.graph.neighbours, this.graph.cutCorners, collision.use2D, false, characterHeight);
				wait = rules.ExecuteRule(GridGraphRule.Pass.AfterConnections, this.context);
				while (wait.MoveNext())
				{
					JobHandle jobHandle3 = wait.Current;
					yield return jobHandle3;
				}
				wait = null;
				if (this.graph.erodeIterations > 0)
				{
					this.context.data.Erosion(this.graph.neighbours, this.graph.erodeIterations, this.writeMaskBounds, this.graph.erosionUseTags, this.graph.erosionFirstTag, this.graph.erosionTagsPrecedenceMask);
					wait = rules.ExecuteRule(GridGraphRule.Pass.AfterErosion, this.context);
					while (wait.MoveNext())
					{
						JobHandle jobHandle4 = wait.Current;
						yield return jobHandle4;
					}
					wait = null;
					this.context.data.Connections(this.graph.maxStepHeight, this.graph.maxStepUsesSlope, this.context.data.nodes.bounds, this.graph.neighbours, this.graph.cutCorners, collision.use2D, true, characterHeight);
					wait = rules.ExecuteRule(GridGraphRule.Pass.AfterConnections, this.context);
					while (wait.MoveNext())
					{
						JobHandle jobHandle5 = wait.Current;
						yield return jobHandle5;
					}
					wait = null;
				}
				else
				{
					this.context.data.nodes.walkable.CopyToJob<bool>(this.context.data.nodes.walkableWithErosion).Schedule(this.dependencyTracker);
				}
				wait = rules.ExecuteRule(GridGraphRule.Pass.PostProcess, this.context);
				while (wait.MoveNext())
				{
					JobHandle jobHandle6 = wait.Current;
					yield return jobHandle6;
				}
				wait = null;
				this.graph.nodeData.TrackBuffers(this.dependencyTracker);
				if (this.recalculationMode == GridGraph.RecalculationMode.RecalculateFromScratch)
				{
					this.graph.nodeData = this.context.data.nodes;
				}
				else
				{
					this.graph.nodeData.ResizeLayerCount(this.context.data.nodes.layers, this.dependencyTracker);
					this.graph.nodeData.CopyFrom(this.context.data.nodes, this.writeMaskBounds, true, this.dependencyTracker);
				}
				this.graph.nodeData.PersistBuffers(this.dependencyTracker);
				yield return this.nodesDependsOn;
				yield return this.dependencyTracker.AllWritesDependency;
				this.dependencyTracker.ClearMemory();
				yield break;
			}

			// Token: 0x06000711 RID: 1809 RVA: 0x00023900 File Offset: 0x00021B00
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.AssertSafeToUpdateGraph();
				if (this.emptyUpdate)
				{
					this.Dispose();
					if (this.isFinalUpdate)
					{
						GridGraphRules rules = this.graph.rules;
						GridGraphRule.Pass pass = GridGraphRule.Pass.AfterApplied;
						GridGraphRules.Context context;
						if ((context = this.context) == null)
						{
							(context = new GridGraphRules.Context()).graph = this.graph;
						}
						rules.ExecuteRuleMainThread(pass, context);
					}
					return;
				}
				bool flag = this.nodes.nodes != this.graph.nodes;
				if (this.context.data.nodes.layers > 1)
				{
					this.nodeArrayBounds.y = this.context.data.nodes.layers;
					int num = this.nodeArrayBounds.x * this.nodeArrayBounds.y * this.nodeArrayBounds.z;
					Memory.Realloc<GridNodeBase>(ref this.nodes.nodes, num);
					JobAllocateNodes jobAllocateNodes = default(JobAllocateNodes);
					jobAllocateNodes.active = this.graph.active;
					jobAllocateNodes.nodeNormals = this.graph.nodeData.normals;
					jobAllocateNodes.dataBounds = this.context.data.nodes.bounds;
					jobAllocateNodes.nodeArrayBounds = this.nodeArrayBounds;
					jobAllocateNodes.nodes = this.nodes.nodes;
					jobAllocateNodes.newGridNodeDelegate = this.graph.newGridNodeDelegate;
					jobAllocateNodes.Execute();
				}
				this.graph.nodeData.AssignToNodes(this.nodes.nodes, this.nodeArrayBounds, this.writeMaskBounds, this.graph.graphIndex, default(JobHandle), this.dependencyTracker).Complete();
				if (this.nodes.nodes != this.graph.nodes)
				{
					if (flag)
					{
						this.graph.DestroyAllNodes();
					}
					this.graph.nodes = this.nodes.nodes;
					this.graph.LayerCount = this.context.data.nodes.layers;
				}
				ctx.DirtyBounds(this.graph.GetBoundsFromRect(new IntRect(this.writeMaskBounds.min.x, this.writeMaskBounds.min.z, this.writeMaskBounds.max.x - 1, this.writeMaskBounds.max.z - 1)));
				this.Dispose();
				if (this.isFinalUpdate)
				{
					this.graph.rules.ExecuteRuleMainThread(GridGraphRule.Pass.AfterApplied, this.context);
				}
			}

			// Token: 0x06000712 RID: 1810 RVA: 0x00023B91 File Offset: 0x00021D91
			public void Dispose()
			{
				if (this.ownsJobDependencyTracker)
				{
					ObjectPool<JobDependencyTracker>.Release(ref this.dependencyTracker);
					if (this.context != null)
					{
						this.context.data.dependencyTracker = null;
					}
				}
			}

			// Token: 0x0400048E RID: 1166
			public GridGraph graph;

			// Token: 0x0400048F RID: 1167
			public GridGraph.GridGraphUpdatePromise.NodesHolder nodes;

			// Token: 0x04000490 RID: 1168
			public JobDependencyTracker dependencyTracker;

			// Token: 0x04000491 RID: 1169
			public int3 nodeArrayBounds;

			// Token: 0x04000492 RID: 1170
			public IntRect rect;

			// Token: 0x04000493 RID: 1171
			public JobHandle nodesDependsOn;

			// Token: 0x04000494 RID: 1172
			public Allocator allocationMethod;

			// Token: 0x04000495 RID: 1173
			public GridGraph.RecalculationMode recalculationMode;

			// Token: 0x04000496 RID: 1174
			public GraphUpdateObject graphUpdateObject;

			// Token: 0x04000497 RID: 1175
			private IntBounds writeMaskBounds;

			// Token: 0x04000498 RID: 1176
			internal GridGraphRules.Context context;

			// Token: 0x04000499 RID: 1177
			private bool emptyUpdate;

			// Token: 0x0400049A RID: 1178
			private IntBounds readBounds;

			// Token: 0x0400049B RID: 1179
			private IntBounds fullRecalculationBounds;

			// Token: 0x0400049C RID: 1180
			public bool ownsJobDependencyTracker;

			// Token: 0x0400049D RID: 1181
			private bool isFinalUpdate;

			// Token: 0x0400049E RID: 1182
			private GraphTransform transform;

			// Token: 0x020000CF RID: 207
			public class NodesHolder
			{
				// Token: 0x0400049F RID: 1183
				public GridNodeBase[] nodes;
			}
		}

		// Token: 0x020000D1 RID: 209
		private class CombinedGridGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x0600071A RID: 1818 RVA: 0x00024AB0 File Offset: 0x00022CB0
			public CombinedGridGraphUpdatePromise(GridGraph graph, List<GraphUpdateObject> graphUpdates)
			{
				this.promises = ListPool<IGraphUpdatePromise>.Claim();
				GridGraph.GridGraphUpdatePromise.NodesHolder nodesHolder = new GridGraph.GridGraphUpdatePromise.NodesHolder
				{
					nodes = graph.nodes
				};
				for (int i = 0; i < graphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject = graphUpdates[i];
					GridGraph.GridGraphUpdatePromise gridGraphUpdatePromise = new GridGraph.GridGraphUpdatePromise(graph, graph.transform, nodesHolder, new int3(graph.width, graph.LayerCount, graph.depth), graph.GetRectFromBounds(graphUpdateObject.bounds), ObjectPool<JobDependencyTracker>.Claim(), default(JobHandle), Allocator.Persistent, graphUpdateObject.updatePhysics ? GridGraph.RecalculationMode.RecalculateMinimal : GridGraph.RecalculationMode.NoRecalculation, graphUpdateObject, true, i == graphUpdates.Count - 1);
					this.promises.Add(gridGraphUpdatePromise);
				}
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x00024B5F File Offset: 0x00022D5F
			public IEnumerator<JobHandle> Prepare()
			{
				int num;
				for (int i = 0; i < this.promises.Count; i = num + 1)
				{
					IEnumerator<JobHandle> it = this.promises[i].Prepare();
					while (it.MoveNext())
					{
						JobHandle jobHandle = it.Current;
						yield return jobHandle;
					}
					it = null;
					num = i;
				}
				yield break;
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x00024B70 File Offset: 0x00022D70
			public void Apply(IGraphUpdateContext ctx)
			{
				for (int i = 0; i < this.promises.Count; i++)
				{
					this.promises[i].Apply(ctx);
				}
				ListPool<IGraphUpdatePromise>.Release(ref this.promises);
			}

			// Token: 0x040004AA RID: 1194
			private List<IGraphUpdatePromise> promises;
		}

		// Token: 0x020000D3 RID: 211
		private class GridGraphSnapshot : IGraphSnapshot, IDisposable
		{
			// Token: 0x06000723 RID: 1827 RVA: 0x00024C80 File Offset: 0x00022E80
			public void Dispose()
			{
				this.nodes.Dispose();
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x00024C90 File Offset: 0x00022E90
			public void Restore(IGraphUpdateContext ctx)
			{
				this.graph.AssertSafeToUpdateGraph();
				if (!this.graph.isScanned)
				{
					return;
				}
				if (!this.graph.nodeData.bounds.Contains(this.nodes.bounds))
				{
					Debug.LogError("Cannot restore snapshot because the graph dimensions have changed since the snapshot was taken");
					return;
				}
				JobDependencyTracker jobDependencyTracker = ObjectPool<JobDependencyTracker>.Claim();
				this.graph.nodeData.CopyFrom(this.nodes, true, jobDependencyTracker);
				this.nodes.AssignToNodes(this.graph.nodes, this.graph.nodeData.bounds.size, this.nodes.bounds, this.graph.graphIndex, default(JobHandle), jobDependencyTracker).Complete();
				jobDependencyTracker.AllWritesDependency.Complete();
				ObjectPool<JobDependencyTracker>.Release(ref jobDependencyTracker);
				ctx.DirtyBounds(this.graph.GetBoundsFromRect(new IntRect(this.nodes.bounds.min.x, this.nodes.bounds.min.z, this.nodes.bounds.max.x - 1, this.nodes.bounds.max.z - 1)));
			}

			// Token: 0x040004B0 RID: 1200
			internal GridGraphNodeData nodes;

			// Token: 0x040004B1 RID: 1201
			internal GridGraph graph;
		}
	}
}
