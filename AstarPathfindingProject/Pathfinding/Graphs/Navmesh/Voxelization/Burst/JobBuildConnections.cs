using System;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001EB RID: 491
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobBuildConnections : IJob
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x00050298 File Offset: 0x0004E498
		public void Execute()
		{
			int num = this.field.width * this.field.depth;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.field.width; j++)
				{
					CompactVoxelCell compactVoxelCell = this.field.cells[j + i];
					int k = compactVoxelCell.index;
					int num3 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num3)
					{
						CompactVoxelSpan compactVoxelSpan = this.field.spans[k];
						compactVoxelSpan.con = uint.MaxValue;
						for (int l = 0; l < 4; l++)
						{
							int num4 = j + VoxelUtilityBurst.DX[l];
							int num5 = i + VoxelUtilityBurst.DZ[l] * this.field.width;
							if (num4 >= 0 && num5 >= 0 && num5 < num && num4 < this.field.width)
							{
								CompactVoxelCell compactVoxelCell2 = this.field.cells[num4 + num5];
								int m = compactVoxelCell2.index;
								int num6 = compactVoxelCell2.index + compactVoxelCell2.count;
								while (m < num6)
								{
									CompactVoxelSpan compactVoxelSpan2 = this.field.spans[m];
									int num7 = (int)Math.Max(compactVoxelSpan.y, compactVoxelSpan2.y);
									if (Math.Min((int)((uint)compactVoxelSpan.y + compactVoxelSpan.h), (int)((uint)compactVoxelSpan2.y + compactVoxelSpan2.h)) - num7 >= this.voxelWalkableHeight && Math.Abs((int)(compactVoxelSpan2.y - compactVoxelSpan.y)) <= this.voxelWalkableClimb)
									{
										uint num8 = (uint)(m - compactVoxelCell2.index);
										if (num8 <= 65535U)
										{
											compactVoxelSpan.SetConnection(l, num8);
											break;
										}
										break;
									}
									else
									{
										m++;
									}
								}
							}
						}
						this.field.spans[k] = compactVoxelSpan;
						k++;
					}
				}
				i += this.field.width;
				num2++;
			}
		}

		// Token: 0x0400093A RID: 2362
		public CompactVoxelField field;

		// Token: 0x0400093B RID: 2363
		public int voxelWalkableHeight;

		// Token: 0x0400093C RID: 2364
		public int voxelWalkableClimb;
	}
}
