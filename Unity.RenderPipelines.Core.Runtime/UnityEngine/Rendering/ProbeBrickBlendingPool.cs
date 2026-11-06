using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000084 RID: 132
	internal class ProbeBrickBlendingPool
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001316B File Offset: 0x0001136B
		internal static bool isSupported
		{
			get
			{
				return ProbeBrickBlendingPool.stateBlendShader != null;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00013178 File Offset: 0x00011378
		internal static void Initialize(in ProbeVolumeSystemParameters parameters)
		{
			ProbeBrickBlendingPool.stateBlendShader = parameters.scenarioBlendingShader;
			ProbeBrickBlendingPool.scenarioBlendingKernel = (ProbeBrickBlendingPool.stateBlendShader ? ProbeBrickBlendingPool.stateBlendShader.FindKernel("BlendScenarios") : (-1));
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x000131A8 File Offset: 0x000113A8
		internal bool isAllocated
		{
			get
			{
				return this.m_State0 != null;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x000131B3 File Offset: 0x000113B3
		internal int estimatedVMemCost
		{
			get
			{
				if (!this.isAllocated)
				{
					return 0;
				}
				return this.m_State0.estimatedVMemCost + this.m_State1.estimatedVMemCost;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000131D6 File Offset: 0x000113D6
		internal int GetPoolWidth()
		{
			return this.m_State0.m_Pool.width;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000131E8 File Offset: 0x000113E8
		internal int GetPoolHeight()
		{
			return this.m_State0.m_Pool.height;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000131FA File Offset: 0x000113FA
		internal int GetPoolDepth()
		{
			return this.m_State0.m_Pool.depth;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001320C File Offset: 0x0001140C
		internal ProbeBrickBlendingPool(ProbeVolumeBlendingTextureMemoryBudget memoryBudget, ProbeVolumeSHBands shBands)
		{
			this.m_MemoryBudget = (ProbeVolumeTextureMemoryBudget)memoryBudget;
			this.m_ShBands = shBands;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00013224 File Offset: 0x00011424
		internal void AllocateResourcesIfNeeded()
		{
			if (this.isAllocated)
			{
				return;
			}
			this.m_State0 = new ProbeBrickPool(this.m_MemoryBudget, this.m_ShBands, false);
			this.m_State1 = new ProbeBrickPool(this.m_MemoryBudget, this.m_ShBands, false);
			int num = this.GetPoolWidth() / 512 * (this.GetPoolHeight() / 4) * (this.GetPoolDepth() / 4);
			this.m_ChunkList = new Vector4[num];
			this.m_MappedChunks = 0;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001329C File Offset: 0x0001149C
		internal void Update(ProbeBrickPool.DataLocation source, List<ProbeBrickPool.BrickChunkAlloc> srcLocations, List<ProbeBrickPool.BrickChunkAlloc> dstLocations, int destStartIndex, ProbeVolumeSHBands bands, int state)
		{
			((state == 0) ? this.m_State0 : this.m_State1).Update(source, srcLocations, dstLocations, destStartIndex, bands);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000132BC File Offset: 0x000114BC
		private static int DivRoundUp(int x, int y)
		{
			return (x + y - 1) / y;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000132C8 File Offset: 0x000114C8
		internal void PerformBlending(CommandBuffer cmd, float factor, ProbeBrickPool dstPool)
		{
			if (this.m_MappedChunks == 0)
			{
				return;
			}
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L0_L1Rx, this.m_State0.m_Pool.TexL0_L1rx);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L1G_L1Ry, this.m_State0.m_Pool.TexL1_G_ry);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L1B_L1Rz, this.m_State0.m_Pool.TexL1_B_rz);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L0_L1Rx, this.m_State1.m_Pool.TexL0_L1rx);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L1G_L1Ry, this.m_State1.m_Pool.TexL1_G_ry);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L1B_L1Rz, this.m_State1.m_Pool.TexL1_B_rz);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L0_L1Rx, dstPool.m_Pool.TexL0_L1rx);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L1G_L1Ry, dstPool.m_Pool.TexL1_G_ry);
			cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L1B_L1Rz, dstPool.m_Pool.TexL1_B_rz);
			if (this.m_ShBands == ProbeVolumeSHBands.SphericalHarmonicsL2)
			{
				ProbeBrickBlendingPool.stateBlendShader.EnableKeyword("PROBE_VOLUMES_L2");
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L2_0, this.m_State0.m_Pool.TexL2_0);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L2_1, this.m_State0.m_Pool.TexL2_1);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L2_2, this.m_State0.m_Pool.TexL2_2);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State0_L2_3, this.m_State0.m_Pool.TexL2_3);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L2_0, this.m_State1.m_Pool.TexL2_0);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L2_1, this.m_State1.m_Pool.TexL2_1);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L2_2, this.m_State1.m_Pool.TexL2_2);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._State1_L2_3, this.m_State1.m_Pool.TexL2_3);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L2_0, dstPool.m_Pool.TexL2_0);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L2_1, dstPool.m_Pool.TexL2_1);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L2_2, dstPool.m_Pool.TexL2_2);
				cmd.SetComputeTextureParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, ProbeBrickBlendingPool._Out_L2_3, dstPool.m_Pool.TexL2_3);
			}
			else
			{
				ProbeBrickBlendingPool.stateBlendShader.DisableKeyword("PROBE_VOLUMES_L2");
			}
			Vector4 vector = new Vector4((float)dstPool.GetPoolWidth(), (float)dstPool.GetPoolHeight(), factor, 0f);
			int num = ProbeBrickBlendingPool.DivRoundUp(512, 4);
			int num2 = ProbeBrickBlendingPool.DivRoundUp(4, 4);
			int num3 = ProbeBrickBlendingPool.DivRoundUp(4, 4);
			cmd.SetComputeVectorArrayParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool._ChunkList, this.m_ChunkList);
			cmd.SetComputeVectorParam(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool._PoolDim_LerpFactor, vector);
			cmd.DispatchCompute(ProbeBrickBlendingPool.stateBlendShader, ProbeBrickBlendingPool.scenarioBlendingKernel, num, num2, num3 * this.m_MappedChunks);
			this.m_MappedChunks = 0;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000136D8 File Offset: 0x000118D8
		internal void BlendChunks(ProbeReferenceVolume.BlendingCellInfo blendingCell, ProbeBrickPool dstPool)
		{
			for (int i = 0; i < blendingCell.chunkList.Count; i++)
			{
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = blendingCell.chunkList[i];
				int num = blendingCell.cellInfo.chunkList[i].flattenIndex(dstPool.GetPoolWidth(), dstPool.GetPoolHeight());
				Vector4[] chunkList = this.m_ChunkList;
				int mappedChunks = this.m_MappedChunks;
				this.m_MappedChunks = mappedChunks + 1;
				chunkList[mappedChunks] = new Vector4((float)brickChunkAlloc.x, (float)brickChunkAlloc.y, (float)brickChunkAlloc.z, (float)num);
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00013769 File Offset: 0x00011969
		internal void Clear()
		{
			ProbeBrickPool state = this.m_State0;
			if (state == null)
			{
				return;
			}
			state.Clear();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001377B File Offset: 0x0001197B
		internal bool Allocate(int numberOfBrickChunks, List<ProbeBrickPool.BrickChunkAlloc> outAllocations)
		{
			this.AllocateResourcesIfNeeded();
			return numberOfBrickChunks <= this.m_State0.GetRemainingChunkCount() && this.m_State0.Allocate(numberOfBrickChunks, outAllocations, false);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x000137A1 File Offset: 0x000119A1
		internal void Deallocate(List<ProbeBrickPool.BrickChunkAlloc> allocations)
		{
			if (allocations.Count == 0)
			{
				return;
			}
			this.m_State0.Deallocate(allocations);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000137B8 File Offset: 0x000119B8
		internal void EnsureTextureValidity()
		{
			if (this.isAllocated)
			{
				this.m_State0.EnsureTextureValidity();
				this.m_State1.EnsureTextureValidity();
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000137D8 File Offset: 0x000119D8
		internal void Cleanup()
		{
			if (this.isAllocated)
			{
				this.m_State0.Cleanup();
				this.m_State1.Cleanup();
			}
		}

		// Token: 0x04000258 RID: 600
		private static ComputeShader stateBlendShader;

		// Token: 0x04000259 RID: 601
		private static int scenarioBlendingKernel = -1;

		// Token: 0x0400025A RID: 602
		private static readonly int _PoolDim_LerpFactor = Shader.PropertyToID("_PoolDim_LerpFactor");

		// Token: 0x0400025B RID: 603
		private static readonly int _ChunkList = Shader.PropertyToID("_ChunkList");

		// Token: 0x0400025C RID: 604
		private static readonly int _State0_L0_L1Rx = Shader.PropertyToID("_State0_L0_L1Rx");

		// Token: 0x0400025D RID: 605
		private static readonly int _State0_L1G_L1Ry = Shader.PropertyToID("_State0_L1G_L1Ry");

		// Token: 0x0400025E RID: 606
		private static readonly int _State0_L1B_L1Rz = Shader.PropertyToID("_State0_L1B_L1Rz");

		// Token: 0x0400025F RID: 607
		private static readonly int _State0_L2_0 = Shader.PropertyToID("_State0_L2_0");

		// Token: 0x04000260 RID: 608
		private static readonly int _State0_L2_1 = Shader.PropertyToID("_State0_L2_1");

		// Token: 0x04000261 RID: 609
		private static readonly int _State0_L2_2 = Shader.PropertyToID("_State0_L2_2");

		// Token: 0x04000262 RID: 610
		private static readonly int _State0_L2_3 = Shader.PropertyToID("_State0_L2_3");

		// Token: 0x04000263 RID: 611
		private static readonly int _State1_L0_L1Rx = Shader.PropertyToID("_State1_L0_L1Rx");

		// Token: 0x04000264 RID: 612
		private static readonly int _State1_L1G_L1Ry = Shader.PropertyToID("_State1_L1G_L1Ry");

		// Token: 0x04000265 RID: 613
		private static readonly int _State1_L1B_L1Rz = Shader.PropertyToID("_State1_L1B_L1Rz");

		// Token: 0x04000266 RID: 614
		private static readonly int _State1_L2_0 = Shader.PropertyToID("_State1_L2_0");

		// Token: 0x04000267 RID: 615
		private static readonly int _State1_L2_1 = Shader.PropertyToID("_State1_L2_1");

		// Token: 0x04000268 RID: 616
		private static readonly int _State1_L2_2 = Shader.PropertyToID("_State1_L2_2");

		// Token: 0x04000269 RID: 617
		private static readonly int _State1_L2_3 = Shader.PropertyToID("_State1_L2_3");

		// Token: 0x0400026A RID: 618
		private static readonly int _Out_L0_L1Rx = Shader.PropertyToID("_Out_L0_L1Rx");

		// Token: 0x0400026B RID: 619
		private static readonly int _Out_L1G_L1Ry = Shader.PropertyToID("_Out_L1G_L1Ry");

		// Token: 0x0400026C RID: 620
		private static readonly int _Out_L1B_L1Rz = Shader.PropertyToID("_Out_L1B_L1Rz");

		// Token: 0x0400026D RID: 621
		private static readonly int _Out_L2_0 = Shader.PropertyToID("_Out_L2_0");

		// Token: 0x0400026E RID: 622
		private static readonly int _Out_L2_1 = Shader.PropertyToID("_Out_L2_1");

		// Token: 0x0400026F RID: 623
		private static readonly int _Out_L2_2 = Shader.PropertyToID("_Out_L2_2");

		// Token: 0x04000270 RID: 624
		private static readonly int _Out_L2_3 = Shader.PropertyToID("_Out_L2_3");

		// Token: 0x04000271 RID: 625
		private Vector4[] m_ChunkList;

		// Token: 0x04000272 RID: 626
		private int m_MappedChunks;

		// Token: 0x04000273 RID: 627
		private ProbeBrickPool m_State0;

		// Token: 0x04000274 RID: 628
		private ProbeBrickPool m_State1;

		// Token: 0x04000275 RID: 629
		private ProbeVolumeTextureMemoryBudget m_MemoryBudget;

		// Token: 0x04000276 RID: 630
		private ProbeVolumeSHBands m_ShBands;
	}
}
