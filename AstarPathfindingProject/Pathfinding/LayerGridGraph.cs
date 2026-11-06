using System;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000D7 RID: 215
	[Preserve]
	public class LayerGridGraph : GridGraph, IUpdatableGraph
	{
		// Token: 0x0600072C RID: 1836 RVA: 0x00024E17 File Offset: 0x00023017
		protected override void DisposeUnmanagedData()
		{
			base.DisposeUnmanagedData();
			LevelGridNode.ClearGridGraph((int)this.graphIndex, this);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00024E2B File Offset: 0x0002302B
		public LayerGridGraph()
		{
			this.newGridNodeDelegate = () => new LevelGridNode();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00024E64 File Offset: 0x00023064
		protected override GridNodeBase[] AllocateNodesJob(int size, out JobHandle dependency)
		{
			LevelGridNode[] array = new LevelGridNode[size];
			AstarPath active = this.active;
			GridNodeBase[] array2 = array;
			dependency = active.AllocateNodes<GridNodeBase>(array2, size, this.newGridNodeDelegate, 1U);
			return array;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00024E97 File Offset: 0x00023097
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00024E9F File Offset: 0x0002309F
		public override int LayerCount
		{
			get
			{
				return this.layerCount;
			}
			protected set
			{
				this.layerCount = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00024EA8 File Offset: 0x000230A8
		public override int MaxLayers
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00024EAC File Offset: 0x000230AC
		public override int CountNodes()
		{
			if (this.nodes == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00024EE8 File Offset: 0x000230E8
		public override int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			IntRect intRect = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, intRect);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				return 0;
			}
			int num = 0;
			try
			{
				for (int i = 0; i < this.layerCount; i++)
				{
					int num2 = i * base.Width * base.Depth;
					for (int j = rect.ymin; j <= rect.ymax; j++)
					{
						int num3 = num2 + j * base.Width;
						for (int k = rect.xmin; k <= rect.xmax; k++)
						{
							GridNodeBase gridNodeBase = this.nodes[num3 + k];
							if (gridNodeBase != null)
							{
								buffer[num] = gridNodeBase;
								num++;
							}
						}
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException("Buffer is too small");
			}
			return num;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00024FE8 File Offset: 0x000231E8
		public GridNodeBase GetNode(int x, int z, int layer)
		{
			if (x < 0 || z < 0 || x >= this.width || z >= this.depth || layer < 0 || layer >= this.layerCount)
			{
				return null;
			}
			return this.nodes[x + z * this.width + layer * this.width * this.depth];
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002503F File Offset: 0x0002323F
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			this.layerCount = 0;
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			return base.ScanInternal(async);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00025074 File Offset: 0x00023274
		protected override GridNodeBase GetNearestFromGraphSpace(Vector3 positionGraphSpace)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
			{
				return null;
			}
			int x = (int)positionGraphSpace.x;
			float z = positionGraphSpace.z;
			int num = Mathf.Clamp(x, 0, this.width - 1);
			int num2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			Vector3 vector = base.transform.Transform(positionGraphSpace);
			return this.GetNearestNode(vector, num, num2);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000250F0 File Offset: 0x000232F0
		private GridNodeBase GetNearestNode(Vector3 position, int x, int z)
		{
			int num = this.width * z + x;
			float num2 = float.PositiveInfinity;
			GridNodeBase gridNodeBase = null;
			for (int i = 0; i < this.layerCount; i++)
			{
				GridNodeBase gridNodeBase2 = this.nodes[num + this.width * this.depth * i];
				if (gridNodeBase2 != null)
				{
					float sqrMagnitude = ((Vector3)gridNodeBase2.position - position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						gridNodeBase = gridNodeBase2;
					}
				}
			}
			return gridNodeBase;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00025168 File Offset: 0x00023368
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
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
			base.SerializeNodeSurfaceNormals(ctx);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000251EC File Offset: 0x000233EC
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			GridNodeBase[] array = new LevelGridNode[num];
			this.nodes = array;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = this.newGridNodeDelegate();
					this.active.InitializeNode(this.nodes[i]);
					this.nodes[i].DeserializeNode(ctx);
				}
				else
				{
					this.nodes[i] = null;
				}
			}
			base.DeserializeNativeData(ctx, ctx.meta.version >= AstarSerializer.V4_3_37);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002529C File Offset: 0x0002349C
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			base.UpdateTransform();
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			if (this.width * this.depth * this.layerCount != this.nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph, just prior to saving it. Nodes will be discarded");
				this.nodes = new GridNodeBase[0];
				return;
			}
			for (int i = 0; i < this.layerCount; i++)
			{
				for (int j = 0; j < this.depth; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						LevelGridNode levelGridNode = this.nodes[j * this.width + k + this.width * this.depth * i] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.NodeInGridIndex = j * this.width + k;
							levelGridNode.LayerCoordinateInGrid = i;
						}
					}
				}
			}
		}

		// Token: 0x040004BD RID: 1213
		[JsonMember]
		internal int layerCount;

		// Token: 0x040004BE RID: 1214
		[JsonMember]
		public float characterHeight = 0.4f;

		// Token: 0x040004BF RID: 1215
		internal int lastScannedWidth;

		// Token: 0x040004C0 RID: 1216
		internal int lastScannedDepth;
	}
}
