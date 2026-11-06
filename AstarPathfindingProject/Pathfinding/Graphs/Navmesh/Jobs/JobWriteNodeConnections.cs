using System;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x02000200 RID: 512
	public struct JobWriteNodeConnections : IJob
	{
		// Token: 0x06000DB1 RID: 3505 RVA: 0x000535F0 File Offset: 0x000517F0
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			for (int i = 0; i < array.Length; i++)
			{
				JobCalculateTriangleConnections.TileNodeConnectionsUnsafe tileNodeConnectionsUnsafe = this.nodeConnections[i];
				this.Apply(array[i].nodes, tileNodeConnectionsUnsafe);
				tileNodeConnectionsUnsafe.neighbourCounts.Dispose();
				tileNodeConnectionsUnsafe.neighbours.Dispose();
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00053650 File Offset: 0x00051850
		private void Apply(TriangleMeshNode[] nodes, JobCalculateTriangleConnections.TileNodeConnectionsUnsafe connections)
		{
			UnsafeAppendBuffer.Reader reader = connections.neighbourCounts.AsReader();
			UnsafeAppendBuffer.Reader reader2 = connections.neighbours.AsReader();
			foreach (TriangleMeshNode triangleMeshNode in nodes)
			{
				int num = reader.ReadNext<int>();
				Connection[] array = (triangleMeshNode.connections = ArrayPool<Connection>.ClaimWithExactLength(num));
				for (int j = 0; j < num; j++)
				{
					int num2 = reader2.ReadNext<int>();
					byte b = (byte)reader2.ReadNext<int>();
					TriangleMeshNode triangleMeshNode2 = nodes[num2];
					int costMagnitude = (triangleMeshNode.position - triangleMeshNode2.position).costMagnitude;
					array[j] = new Connection(triangleMeshNode2, (uint)costMagnitude, b);
				}
			}
		}

		// Token: 0x040009BD RID: 2493
		[ReadOnly]
		public NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nodeConnections;

		// Token: 0x040009BE RID: 2494
		public GCHandle tiles;
	}
}
