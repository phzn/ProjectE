using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000085 RID: 133
	internal class ProbeCellIndices
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00013964 File Offset: 0x00011B64
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001396C File Offset: 0x00011B6C
		internal int estimatedVMemCost { get; private set; }

		// Token: 0x0600044A RID: 1098 RVA: 0x00013975 File Offset: 0x00011B75
		internal Vector3Int GetCellIndexDimension()
		{
			return this.m_CellCount;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001397D File Offset: 0x00011B7D
		internal Vector3Int GetCellMinPosition()
		{
			return this.m_CellMin;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00013985 File Offset: 0x00011B85
		private int GetFlatIndex(Vector3Int normalizedPos)
		{
			return normalizedPos.z * (this.m_CellCount.x * this.m_CellCount.y) + normalizedPos.y * this.m_CellCount.x + normalizedPos.x;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000139C4 File Offset: 0x00011BC4
		internal ProbeCellIndices(Vector3Int cellMin, Vector3Int cellMax, int cellSizeInMinBricks)
		{
			Vector3Int vector3Int = cellMax + Vector3Int.one - cellMin;
			this.m_CellCount = vector3Int;
			this.m_CellMin = cellMin;
			this.m_CellSizeInMinBricks = cellSizeInMinBricks;
			int num = vector3Int.x * vector3Int.y * vector3Int.z;
			int num2 = 3 * num;
			this.m_IndexOfIndicesBuffer = new ComputeBuffer(num, 12);
			this.m_IndexOfIndicesData = new uint[num2];
			this.m_NeedUpdateComputeBuffer = false;
			this.estimatedVMemCost = num * 3 * 4;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00013A48 File Offset: 0x00011C48
		internal int GetFlatIdxForCell(Vector3Int cellPosition)
		{
			Vector3Int vector3Int = cellPosition - this.m_CellMin;
			return this.GetFlatIndex(vector3Int);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00013A6C File Offset: 0x00011C6C
		internal void UpdateCell(int cellFlatIdx, ProbeBrickIndex.CellIndexUpdateInfo cellUpdateInfo)
		{
			int num = ProbeReferenceVolume.CellSize(cellUpdateInfo.minSubdivInCell);
			ProbeCellIndices.IndexMetaData indexMetaData = default(ProbeCellIndices.IndexMetaData);
			indexMetaData.minSubdiv = cellUpdateInfo.minSubdivInCell;
			indexMetaData.minLocalIdx = cellUpdateInfo.minValidBrickIndexForCellAtMaxRes / num;
			indexMetaData.maxLocalIdx = cellUpdateInfo.maxValidBrickIndexForCellAtMaxResPlusOne / num;
			indexMetaData.firstChunkIndex = cellUpdateInfo.firstChunkIndex;
			uint[] array;
			indexMetaData.Pack(out array);
			for (int i = 0; i < 3; i++)
			{
				this.m_IndexOfIndicesData[cellFlatIdx * 3 + i] = array[i];
			}
			this.m_NeedUpdateComputeBuffer = true;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00013AF8 File Offset: 0x00011CF8
		internal void MarkCellAsUnloaded(int cellFlatIdx)
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_IndexOfIndicesData[cellFlatIdx * 3 + i] = uint.MaxValue;
			}
			this.m_NeedUpdateComputeBuffer = true;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00013B25 File Offset: 0x00011D25
		internal void PushComputeData()
		{
			this.m_IndexOfIndicesBuffer.SetData(this.m_IndexOfIndicesData);
			this.m_NeedUpdateComputeBuffer = false;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00013B3F File Offset: 0x00011D3F
		internal void GetRuntimeResources(ref ProbeReferenceVolume.RuntimeResources rr)
		{
			if (this.m_NeedUpdateComputeBuffer)
			{
				this.PushComputeData();
			}
			rr.cellIndices = this.m_IndexOfIndicesBuffer;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00013B5B File Offset: 0x00011D5B
		internal void Cleanup()
		{
			CoreUtils.SafeRelease(this.m_IndexOfIndicesBuffer);
			this.m_IndexOfIndicesBuffer = null;
		}

		// Token: 0x04000277 RID: 631
		private const int kUintPerEntry = 3;

		// Token: 0x04000279 RID: 633
		private ComputeBuffer m_IndexOfIndicesBuffer;

		// Token: 0x0400027A RID: 634
		private uint[] m_IndexOfIndicesData;

		// Token: 0x0400027B RID: 635
		private Vector3Int m_CellCount;

		// Token: 0x0400027C RID: 636
		private Vector3Int m_CellMin;

		// Token: 0x0400027D RID: 637
		private int m_CellSizeInMinBricks;

		// Token: 0x0400027E RID: 638
		private bool m_NeedUpdateComputeBuffer;

		// Token: 0x0200019C RID: 412
		internal struct IndexMetaData
		{
			// Token: 0x06000AD1 RID: 2769 RVA: 0x0002E3DC File Offset: 0x0002C5DC
			internal void Pack(out uint[] vals)
			{
				vals = ProbeCellIndices.IndexMetaData.s_PackedValues;
				for (int i = 0; i < 3; i++)
				{
					vals[i] = 0U;
				}
				vals[0] = (uint)(this.firstChunkIndex & 536870911);
				vals[0] |= (uint)((uint)(this.minSubdiv & 7) << 29);
				vals[1] = (uint)(this.minLocalIdx.x & 1023);
				vals[1] |= (uint)((uint)(this.minLocalIdx.y & 1023) << 10);
				vals[1] |= (uint)((uint)(this.minLocalIdx.z & 1023) << 20);
				vals[2] = (uint)(this.maxLocalIdx.x & 1023);
				vals[2] |= (uint)((uint)(this.maxLocalIdx.y & 1023) << 10);
				vals[2] |= (uint)((uint)(this.maxLocalIdx.z & 1023) << 20);
			}

			// Token: 0x040006A6 RID: 1702
			private static uint[] s_PackedValues = new uint[3];

			// Token: 0x040006A7 RID: 1703
			internal Vector3Int minLocalIdx;

			// Token: 0x040006A8 RID: 1704
			internal Vector3Int maxLocalIdx;

			// Token: 0x040006A9 RID: 1705
			internal int firstChunkIndex;

			// Token: 0x040006AA RID: 1706
			internal int minSubdiv;
		}
	}
}
