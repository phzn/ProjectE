using System;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000225 RID: 549
	public struct JobAllocateNodes : IJob
	{
		// Token: 0x06000E28 RID: 3624 RVA: 0x00056B40 File Offset: 0x00054D40
		public unsafe void Execute()
		{
			int3 size = this.dataBounds.size;
			UnsafeSpan<float4> unsafeSpan = this.nodeNormals.AsUnsafeReadOnlySpan<float4>();
			for (int i = 1; i < size.y; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					int num = ((i + this.dataBounds.min.y) * this.nodeArrayBounds.z + (j + this.dataBounds.min.z)) * this.nodeArrayBounds.x + this.dataBounds.min.x;
					for (int k = 0; k < size.x; k++)
					{
						int num2 = num + k;
						bool flag = math.any(*unsafeSpan[num2]);
						GridNodeBase gridNodeBase = this.nodes[num2];
						bool flag2 = gridNodeBase != null;
						if (flag != flag2)
						{
							if (flag)
							{
								gridNodeBase = (this.nodes[num2] = this.newGridNodeDelegate());
								this.active.InitializeNode(gridNodeBase);
							}
							else
							{
								gridNodeBase.ClearCustomConnections(true);
								gridNodeBase.ResetConnectionsInternal();
								gridNodeBase.Destroy();
								this.nodes[num2] = null;
							}
						}
					}
				}
			}
		}

		// Token: 0x04000A47 RID: 2631
		public AstarPath active;

		// Token: 0x04000A48 RID: 2632
		[ReadOnly]
		public NativeArray<float4> nodeNormals;

		// Token: 0x04000A49 RID: 2633
		public IntBounds dataBounds;

		// Token: 0x04000A4A RID: 2634
		public int3 nodeArrayBounds;

		// Token: 0x04000A4B RID: 2635
		public GridNodeBase[] nodes;

		// Token: 0x04000A4C RID: 2636
		public Func<GridNodeBase> newGridNodeDelegate;
	}
}
