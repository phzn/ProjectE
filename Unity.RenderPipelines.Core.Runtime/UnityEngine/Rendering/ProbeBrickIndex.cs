using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;

namespace UnityEngine.Rendering
{
	// Token: 0x02000082 RID: 130
	internal class ProbeBrickIndex
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x000118F7 File Offset: 0x0000FAF7
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x000118FF File Offset: 0x0000FAFF
		internal int estimatedVMemCost { get; private set; }

		// Token: 0x0600040D RID: 1037 RVA: 0x00011908 File Offset: 0x0000FB08
		private int GetVoxelSubdivLevel()
		{
			return Mathf.Min(3, ProbeReferenceVolume.instance.GetMaxSubdivision() - 1);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001191C File Offset: 0x0000FB1C
		private int SizeOfPhysicalIndexFromBudget(ProbeVolumeTextureMemoryBudget memoryBudget)
		{
			if (memoryBudget == ProbeVolumeTextureMemoryBudget.MemoryBudgetLow)
			{
				return 16000000;
			}
			if (memoryBudget == ProbeVolumeTextureMemoryBudget.MemoryBudgetMedium)
			{
				return 32000000;
			}
			if (memoryBudget != ProbeVolumeTextureMemoryBudget.MemoryBudgetHigh)
			{
				return 32000000;
			}
			return 64000000;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00011950 File Offset: 0x0000FB50
		internal ProbeBrickIndex(ProbeVolumeTextureMemoryBudget memoryBudget)
		{
			this.m_CenterRS = new Vector3Int(0, 0, 0);
			this.m_VoxelToBricks = new Dictionary<Vector3Int, List<ProbeBrickIndex.VoxelMeta>>();
			this.m_BricksToVoxels = new Dictionary<ProbeReferenceVolume.Cell, ProbeBrickIndex.BrickMeta>();
			this.m_NeedUpdateIndexComputeBuffer = false;
			this.m_IndexInChunks = Mathf.CeilToInt((float)this.SizeOfPhysicalIndexFromBudget(memoryBudget) / 243f);
			this.m_AvailableChunkCount = this.m_IndexInChunks;
			this.m_IndexChunks = new BitArray(Mathf.Max(1, this.m_IndexInChunks));
			int num = this.m_IndexInChunks * 243;
			this.m_PhysicalIndexBufferData = new int[num];
			this.m_PhysicalIndexBuffer = new ComputeBuffer(num, 4, ComputeBufferType.Structured);
			this.m_NextFreeChunk = 0;
			this.estimatedVMemCost = num * 4;
			this.Clear();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00011AA2 File Offset: 0x0000FCA2
		public int GetRemainingChunkCount()
		{
			return this.m_AvailableChunkCount;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00011AAC File Offset: 0x0000FCAC
		internal void UploadIndexData()
		{
			int num = this.m_UpdateMaxIndex - this.m_UpdateMinIndex + 1;
			this.m_PhysicalIndexBuffer.SetData(this.m_PhysicalIndexBufferData, this.m_UpdateMinIndex, this.m_UpdateMinIndex, num);
			this.m_NeedUpdateIndexComputeBuffer = false;
			this.m_UpdateMaxIndex = int.MinValue;
			this.m_UpdateMinIndex = int.MaxValue;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00011B04 File Offset: 0x0000FD04
		internal void Clear()
		{
			for (int i = 0; i < this.m_PhysicalIndexBufferData.Length; i++)
			{
				this.m_PhysicalIndexBufferData[i] = -1;
			}
			this.m_NeedUpdateIndexComputeBuffer = true;
			this.m_UpdateMinIndex = 0;
			this.m_UpdateMaxIndex = this.m_PhysicalIndexBufferData.Length - 1;
			this.m_NextFreeChunk = 0;
			this.m_IndexChunks.SetAll(false);
			foreach (List<ProbeBrickIndex.VoxelMeta> list in this.m_VoxelToBricks.Values)
			{
				foreach (ProbeBrickIndex.VoxelMeta voxelMeta in list)
				{
					this.m_VoxelMetaPool.Release(voxelMeta);
				}
				this.m_VoxelMetaListPool.Release(list);
			}
			this.m_VoxelToBricks.Clear();
			foreach (ProbeBrickIndex.BrickMeta brickMeta in this.m_BricksToVoxels.Values)
			{
				this.m_BrickMetaPool.Release(brickMeta);
			}
			this.m_BricksToVoxels.Clear();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00011C58 File Offset: 0x0000FE58
		private void MapBrickToVoxels(ProbeBrickIndex.Brick brick, HashSet<Vector3Int> voxels)
		{
			int subdivisionLevel = brick.subdivisionLevel;
			int num = (int)Mathf.Pow(3f, (float)Mathf.Max(0, subdivisionLevel - this.GetVoxelSubdivLevel()));
			Vector3Int position = brick.position;
			int num2 = ProbeReferenceVolume.CellSize(brick.subdivisionLevel);
			int num3 = ProbeReferenceVolume.CellSize(this.GetVoxelSubdivLevel());
			if (num <= 1)
			{
				Vector3 vector = brick.position;
				vector *= 1f / (float)num3;
				position = new Vector3Int(Mathf.FloorToInt(vector.x) * num3, Mathf.FloorToInt(vector.y) * num3, Mathf.FloorToInt(vector.z) * num3);
			}
			for (int i = position.z; i < position.z + num2; i += num3)
			{
				for (int j = position.y; j < position.y + num2; j += num3)
				{
					for (int k = position.x; k < position.x + num2; k += num3)
					{
						voxels.Add(new Vector3Int(k, j, i));
					}
				}
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00011D64 File Offset: 0x0000FF64
		private void ClearVoxel(Vector3Int pos, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			Vector3Int vector3Int;
			Vector3Int vector3Int2;
			this.ClipToIndexSpace(pos, this.GetVoxelSubdivLevel(), out vector3Int, out vector3Int2, cellInfo);
			this.UpdatePhysicalIndex(vector3Int, vector3Int2, -1, cellInfo);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00011D8D File Offset: 0x0000FF8D
		internal void GetRuntimeResources(ref ProbeReferenceVolume.RuntimeResources rr)
		{
			if (this.m_NeedUpdateIndexComputeBuffer)
			{
				this.UploadIndexData();
			}
			rr.index = this.m_PhysicalIndexBuffer;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00011DA9 File Offset: 0x0000FFA9
		internal void Cleanup()
		{
			CoreUtils.SafeRelease(this.m_PhysicalIndexBuffer);
			this.m_PhysicalIndexBuffer = null;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00011DBD File Offset: 0x0000FFBD
		private int MergeIndex(int index, int size)
		{
			return (index & -1879048193) | ((size & 7) << 28);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00011DD0 File Offset: 0x0000FFD0
		internal bool AssignIndexChunksToCell(int bricksCount, ref ProbeBrickIndex.CellIndexUpdateInfo cellUpdateInfo, bool ignoreErrorLog)
		{
			int num = Mathf.CeilToInt((float)bricksCount / 243f);
			int num2 = -1;
			for (int i = 0; i < this.m_IndexInChunks; i++)
			{
				if (!this.m_IndexChunks[i] && i + num < this.m_IndexInChunks)
				{
					int num3 = 0;
					int num4 = i;
					while (num4 < i + num && !this.m_IndexChunks[num4])
					{
						num3++;
						num4++;
					}
					if (num3 == num)
					{
						num2 = i;
						break;
					}
				}
			}
			if (num2 < 0)
			{
				if (!ignoreErrorLog)
				{
					Debug.LogError("APV Index Allocation failed.");
				}
				return false;
			}
			cellUpdateInfo.firstChunkIndex = num2;
			cellUpdateInfo.numberOfChunks = num;
			for (int j = num2; j < num2 + num; j++)
			{
				this.m_IndexChunks[j] = true;
			}
			this.m_NextFreeChunk += Mathf.Max(0, num2 + num - this.m_NextFreeChunk);
			this.m_AvailableChunkCount -= num;
			return true;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00011EB4 File Offset: 0x000100B4
		public void AddBricks(ProbeReferenceVolume.Cell cell, NativeArray<ProbeBrickIndex.Brick> bricks, List<ProbeBrickPool.BrickChunkAlloc> allocations, int allocationSize, int poolWidth, int poolHeight, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			int num = ProbeReferenceVolume.CellSize(7);
			ProbeBrickIndex.g_Cell = cell;
			ProbeBrickIndex.BrickMeta brickMeta = this.m_BrickMetaPool.Get();
			this.m_BricksToVoxels.Add(cell, brickMeta);
			int num2 = 0;
			for (int i = 0; i < allocations.Count; i++)
			{
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = allocations[i];
				int num3 = Mathf.Min(allocationSize, bricks.Length - num2);
				int j = 0;
				while (j < num3)
				{
					ProbeBrickIndex.Brick brick = bricks[num2];
					int num4 = ProbeReferenceVolume.CellSize(brick.subdivisionLevel);
					num = Mathf.Min(num, num4);
					this.MapBrickToVoxels(brick, brickMeta.voxels);
					ProbeBrickIndex.ReservedBrick reservedBrick = default(ProbeBrickIndex.ReservedBrick);
					reservedBrick.brick = brick;
					reservedBrick.flattenedIdx = this.MergeIndex(brickChunkAlloc.flattenIndex(poolWidth, poolHeight), brick.subdivisionLevel);
					brickMeta.bricks.Add(reservedBrick);
					foreach (Vector3Int vector3Int in brickMeta.voxels)
					{
						List<ProbeBrickIndex.VoxelMeta> list;
						if (!this.m_VoxelToBricks.TryGetValue(vector3Int, out list))
						{
							list = this.m_VoxelMetaListPool.Get();
							this.m_VoxelToBricks.Add(vector3Int, list);
						}
						int num5 = list.FindIndex((ProbeBrickIndex.VoxelMeta lhs) => lhs.cell == ProbeBrickIndex.g_Cell);
						ProbeBrickIndex.VoxelMeta voxelMeta;
						if (num5 == -1)
						{
							voxelMeta = this.m_VoxelMetaPool.Get();
							voxelMeta.cell = cell;
							list.Add(voxelMeta);
						}
						else
						{
							voxelMeta = list[num5];
						}
						voxelMeta.brickIndices.Add((ushort)num2);
					}
					j++;
					num2++;
					brickChunkAlloc.x += 4;
				}
			}
			foreach (Vector3Int vector3Int2 in brickMeta.voxels)
			{
				this.UpdateIndexForVoxel(vector3Int2, cellInfo);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000120D4 File Offset: 0x000102D4
		public void RemoveBricks(ProbeReferenceVolume.CellInfo cellInfo)
		{
			if (!this.m_BricksToVoxels.ContainsKey(cellInfo.cell))
			{
				return;
			}
			ProbeBrickIndex.CellIndexUpdateInfo updateInfo = cellInfo.updateInfo;
			ProbeBrickIndex.g_Cell = cellInfo.cell;
			ProbeBrickIndex.BrickMeta brickMeta = this.m_BricksToVoxels[cellInfo.cell];
			foreach (Vector3Int vector3Int in brickMeta.voxels)
			{
				List<ProbeBrickIndex.VoxelMeta> list = this.m_VoxelToBricks[vector3Int];
				int num = list.FindIndex((ProbeBrickIndex.VoxelMeta lhs) => lhs.cell == ProbeBrickIndex.g_Cell);
				if (num >= 0)
				{
					this.m_VoxelMetaPool.Release(list[num]);
					list.RemoveAt(num);
					if (list.Count > 0)
					{
						this.UpdateIndexForVoxel(vector3Int, updateInfo);
					}
					else
					{
						this.ClearVoxel(vector3Int, updateInfo);
						this.m_VoxelMetaListPool.Release(list);
						this.m_VoxelToBricks.Remove(vector3Int);
					}
				}
			}
			this.m_BrickMetaPool.Release(brickMeta);
			this.m_BricksToVoxels.Remove(cellInfo.cell);
			for (int i = updateInfo.firstChunkIndex; i < updateInfo.firstChunkIndex + updateInfo.numberOfChunks; i++)
			{
				this.m_IndexChunks[i] = false;
			}
			this.m_AvailableChunkCount += updateInfo.numberOfChunks;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001224C File Offset: 0x0001044C
		private void UpdateIndexForVoxel(Vector3Int voxel, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			this.ClearVoxel(voxel, cellInfo);
			foreach (ProbeBrickIndex.VoxelMeta voxelMeta in this.m_VoxelToBricks[voxel])
			{
				List<ProbeBrickIndex.ReservedBrick> bricks = this.m_BricksToVoxels[voxelMeta.cell].bricks;
				List<ushort> brickIndices = voxelMeta.brickIndices;
				this.UpdateIndexForVoxel(voxel, bricks, brickIndices, cellInfo);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000122D0 File Offset: 0x000104D0
		private void UpdatePhysicalIndex(Vector3Int brickMin, Vector3Int brickMax, int value, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			brickMin -= cellInfo.cellPositionInBricksAtMaxRes;
			brickMax -= cellInfo.cellPositionInBricksAtMaxRes;
			brickMin /= ProbeReferenceVolume.CellSize(cellInfo.minSubdivInCell);
			brickMax /= ProbeReferenceVolume.CellSize(cellInfo.minSubdivInCell);
			ProbeReferenceVolume.CellSize(ProbeReferenceVolume.instance.GetMaxSubdivision() - 1 - cellInfo.minSubdivInCell);
			Vector3Int vector3Int = cellInfo.minValidBrickIndexForCellAtMaxRes / ProbeReferenceVolume.CellSize(cellInfo.minSubdivInCell);
			Vector3Int vector3Int2 = cellInfo.maxValidBrickIndexForCellAtMaxResPlusOne / ProbeReferenceVolume.CellSize(cellInfo.minSubdivInCell);
			brickMin -= vector3Int;
			brickMax -= vector3Int;
			Vector3Int vector3Int3 = vector3Int2 - vector3Int;
			int num = cellInfo.firstChunkIndex * 243;
			int num2 = num + brickMin.z * (vector3Int3.x * vector3Int3.y) + brickMin.x * vector3Int3.y + brickMin.y;
			int num3 = num + Math.Max(0, brickMax.z - 1) * (vector3Int3.x * vector3Int3.y) + Math.Max(0, brickMax.x - 1) * vector3Int3.y + Math.Max(0, brickMax.y - 1);
			this.m_UpdateMinIndex = Math.Min(this.m_UpdateMinIndex, num2);
			this.m_UpdateMaxIndex = Math.Max(this.m_UpdateMaxIndex, num3);
			for (int i = brickMin.x; i < brickMax.x; i++)
			{
				for (int j = brickMin.z; j < brickMax.z; j++)
				{
					for (int k = brickMin.y; k < brickMax.y; k++)
					{
						int num4 = j * (vector3Int3.x * vector3Int3.y) + i * vector3Int3.y + k;
						int num5 = num + num4;
						this.m_PhysicalIndexBufferData[num5] = value;
					}
				}
			}
			this.m_NeedUpdateIndexComputeBuffer = true;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000124C4 File Offset: 0x000106C4
		private void ClipToIndexSpace(Vector3Int pos, int subdiv, out Vector3Int outMinpos, out Vector3Int outMaxpos, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			int num = ProbeReferenceVolume.CellSize(subdiv);
			Vector3Int vector3Int = cellInfo.cellPositionInBricksAtMaxRes + cellInfo.minValidBrickIndexForCellAtMaxRes;
			Vector3Int vector3Int2 = cellInfo.cellPositionInBricksAtMaxRes + cellInfo.maxValidBrickIndexForCellAtMaxResPlusOne - Vector3Int.one;
			int num2 = pos.x - this.m_CenterRS.x;
			int num3 = pos.y;
			int num4 = pos.z - this.m_CenterRS.z;
			int num5 = num2 + num;
			int num6 = num3 + num;
			int num7 = num4 + num;
			num2 = Mathf.Max(num2, vector3Int.x);
			num3 = Mathf.Max(num3, vector3Int.y);
			num4 = Mathf.Max(num4, vector3Int.z);
			num5 = Mathf.Min(num5, vector3Int2.x);
			num6 = Mathf.Min(num6, vector3Int2.y);
			num7 = Mathf.Min(num7, vector3Int2.z);
			outMinpos = new Vector3Int(num2, num3, num4);
			outMaxpos = new Vector3Int(num5, num6, num7);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000125D0 File Offset: 0x000107D0
		private void UpdateIndexForVoxel(Vector3Int voxel, List<ProbeBrickIndex.ReservedBrick> bricks, List<ushort> indices, ProbeBrickIndex.CellIndexUpdateInfo cellInfo)
		{
			Vector3Int vector3Int;
			Vector3Int vector3Int2;
			this.ClipToIndexSpace(voxel, this.GetVoxelSubdivLevel(), out vector3Int, out vector3Int2, cellInfo);
			foreach (ProbeBrickIndex.ReservedBrick reservedBrick in bricks)
			{
				int num = ProbeReferenceVolume.CellSize(reservedBrick.brick.subdivisionLevel);
				Vector3Int position = reservedBrick.brick.position;
				Vector3Int vector3Int3 = reservedBrick.brick.position + Vector3Int.one * num;
				position.x = Mathf.Max(vector3Int.x, position.x - this.m_CenterRS.x);
				position.y = Mathf.Max(vector3Int.y, position.y);
				position.z = Mathf.Max(vector3Int.z, position.z - this.m_CenterRS.z);
				vector3Int3.x = Mathf.Min(vector3Int2.x, vector3Int3.x - this.m_CenterRS.x);
				vector3Int3.y = Mathf.Min(vector3Int2.y, vector3Int3.y);
				vector3Int3.z = Mathf.Min(vector3Int2.z, vector3Int3.z - this.m_CenterRS.z);
				this.UpdatePhysicalIndex(position, vector3Int3, reservedBrick.flattenedIdx, cellInfo);
			}
		}

		// Token: 0x04000238 RID: 568
		internal const int kMaxSubdivisionLevels = 7;

		// Token: 0x04000239 RID: 569
		internal const int kIndexChunkSize = 243;

		// Token: 0x0400023A RID: 570
		private BitArray m_IndexChunks;

		// Token: 0x0400023B RID: 571
		private int m_IndexInChunks;

		// Token: 0x0400023C RID: 572
		private int m_NextFreeChunk;

		// Token: 0x0400023D RID: 573
		private int m_AvailableChunkCount;

		// Token: 0x0400023E RID: 574
		private ComputeBuffer m_PhysicalIndexBuffer;

		// Token: 0x0400023F RID: 575
		private int[] m_PhysicalIndexBufferData;

		// Token: 0x04000241 RID: 577
		private Vector3Int m_CenterRS;

		// Token: 0x04000242 RID: 578
		private Dictionary<Vector3Int, List<ProbeBrickIndex.VoxelMeta>> m_VoxelToBricks;

		// Token: 0x04000243 RID: 579
		private Dictionary<ProbeReferenceVolume.Cell, ProbeBrickIndex.BrickMeta> m_BricksToVoxels;

		// Token: 0x04000244 RID: 580
		private ObjectPool<ProbeBrickIndex.BrickMeta> m_BrickMetaPool = new ObjectPool<ProbeBrickIndex.BrickMeta>(delegate(ProbeBrickIndex.BrickMeta x)
		{
			x.Clear();
		}, null, false);

		// Token: 0x04000245 RID: 581
		private ObjectPool<List<ProbeBrickIndex.VoxelMeta>> m_VoxelMetaListPool = new ObjectPool<List<ProbeBrickIndex.VoxelMeta>>(delegate(List<ProbeBrickIndex.VoxelMeta> x)
		{
			x.Clear();
		}, null, false);

		// Token: 0x04000246 RID: 582
		private ObjectPool<ProbeBrickIndex.VoxelMeta> m_VoxelMetaPool = new ObjectPool<ProbeBrickIndex.VoxelMeta>(delegate(ProbeBrickIndex.VoxelMeta x)
		{
			x.Clear();
		}, null, false);

		// Token: 0x04000247 RID: 583
		private bool m_NeedUpdateIndexComputeBuffer;

		// Token: 0x04000248 RID: 584
		private int m_UpdateMinIndex = int.MaxValue;

		// Token: 0x04000249 RID: 585
		private int m_UpdateMaxIndex = int.MinValue;

		// Token: 0x0400024A RID: 586
		private static ProbeReferenceVolume.Cell g_Cell;

		// Token: 0x02000194 RID: 404
		[DebuggerDisplay("Brick [{position}, {subdivisionLevel}]")]
		[Serializable]
		public struct Brick : IEquatable<ProbeBrickIndex.Brick>
		{
			// Token: 0x06000AC2 RID: 2754 RVA: 0x0002E242 File Offset: 0x0002C442
			internal Brick(Vector3Int position, int subdivisionLevel)
			{
				this.position = position;
				this.subdivisionLevel = subdivisionLevel;
			}

			// Token: 0x06000AC3 RID: 2755 RVA: 0x0002E252 File Offset: 0x0002C452
			public bool Equals(ProbeBrickIndex.Brick other)
			{
				return this.position == other.position && this.subdivisionLevel == other.subdivisionLevel;
			}

			// Token: 0x04000684 RID: 1668
			public Vector3Int position;

			// Token: 0x04000685 RID: 1669
			public int subdivisionLevel;
		}

		// Token: 0x02000195 RID: 405
		[DebuggerDisplay("Brick [{brick.position}, {brick.subdivisionLevel}], {flattenedIdx}")]
		private struct ReservedBrick
		{
			// Token: 0x04000686 RID: 1670
			public ProbeBrickIndex.Brick brick;

			// Token: 0x04000687 RID: 1671
			public int flattenedIdx;
		}

		// Token: 0x02000196 RID: 406
		private class VoxelMeta
		{
			// Token: 0x06000AC4 RID: 2756 RVA: 0x0002E277 File Offset: 0x0002C477
			public void Clear()
			{
				this.cell = null;
				this.brickIndices.Clear();
			}

			// Token: 0x04000688 RID: 1672
			public ProbeReferenceVolume.Cell cell;

			// Token: 0x04000689 RID: 1673
			public List<ushort> brickIndices = new List<ushort>();
		}

		// Token: 0x02000197 RID: 407
		private class BrickMeta
		{
			// Token: 0x06000AC6 RID: 2758 RVA: 0x0002E29E File Offset: 0x0002C49E
			public void Clear()
			{
				this.voxels.Clear();
				this.bricks.Clear();
			}

			// Token: 0x0400068A RID: 1674
			public HashSet<Vector3Int> voxels = new HashSet<Vector3Int>();

			// Token: 0x0400068B RID: 1675
			public List<ProbeBrickIndex.ReservedBrick> bricks = new List<ProbeBrickIndex.ReservedBrick>();
		}

		// Token: 0x02000198 RID: 408
		public struct CellIndexUpdateInfo
		{
			// Token: 0x0400068C RID: 1676
			public int firstChunkIndex;

			// Token: 0x0400068D RID: 1677
			public int numberOfChunks;

			// Token: 0x0400068E RID: 1678
			public int minSubdivInCell;

			// Token: 0x0400068F RID: 1679
			public Vector3Int minValidBrickIndexForCellAtMaxRes;

			// Token: 0x04000690 RID: 1680
			public Vector3Int maxValidBrickIndexForCellAtMaxResPlusOne;

			// Token: 0x04000691 RID: 1681
			public Vector3Int cellPositionInBricksAtMaxRes;
		}
	}
}
