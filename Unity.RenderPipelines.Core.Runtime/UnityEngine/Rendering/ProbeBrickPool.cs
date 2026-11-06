using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x02000083 RID: 131
	internal class ProbeBrickPool
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00012758 File Offset: 0x00010958
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00012760 File Offset: 0x00010960
		internal int estimatedVMemCost { get; private set; }

		// Token: 0x06000421 RID: 1057 RVA: 0x0001276C File Offset: 0x0001096C
		internal ProbeBrickPool(ProbeVolumeTextureMemoryBudget memoryBudget, ProbeVolumeSHBands shBands, bool allocateValidityData = true)
		{
			this.m_NextFreeChunk.x = (this.m_NextFreeChunk.y = (this.m_NextFreeChunk.z = 0));
			this.m_SHBands = shBands;
			this.m_ContainsValidity = allocateValidityData;
			this.m_FreeList = new Stack<ProbeBrickPool.BrickChunkAlloc>(256);
			int num;
			int num2;
			int num3;
			this.DerivePoolSizeFromBudget(memoryBudget, out num, out num2, out num3);
			int num4;
			this.m_Pool = ProbeBrickPool.CreateDataLocation(num * num2 * num3, false, shBands, "APV", true, allocateValidityData, out num4);
			this.estimatedVMemCost = num4;
			this.m_AvailableChunkCount = this.m_Pool.width / 512 * (this.m_Pool.height / 4) * (this.m_Pool.depth / 4);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001282B File Offset: 0x00010A2B
		public int GetRemainingChunkCount()
		{
			return this.m_AvailableChunkCount;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00012834 File Offset: 0x00010A34
		internal void EnsureTextureValidity()
		{
			if (this.m_Pool.TexL0_L1rx == null)
			{
				this.m_Pool.Cleanup();
				int num;
				this.m_Pool = ProbeBrickPool.CreateDataLocation(this.m_Pool.width * this.m_Pool.height * this.m_Pool.depth, false, this.m_SHBands, "APV", true, this.m_ContainsValidity, out num);
				this.estimatedVMemCost = num;
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000128A9 File Offset: 0x00010AA9
		internal static int GetChunkSizeInBrickCount()
		{
			return 128;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000128B0 File Offset: 0x00010AB0
		internal static int GetChunkSizeInProbeCount()
		{
			return 8192;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x000128B7 File Offset: 0x00010AB7
		internal int GetPoolWidth()
		{
			return this.m_Pool.width;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000128C4 File Offset: 0x00010AC4
		internal int GetPoolHeight()
		{
			return this.m_Pool.height;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000128D1 File Offset: 0x00010AD1
		internal Vector3Int GetPoolDimensions()
		{
			return new Vector3Int(this.m_Pool.width, this.m_Pool.height, this.m_Pool.depth);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000128FC File Offset: 0x00010AFC
		internal void GetRuntimeResources(ref ProbeReferenceVolume.RuntimeResources rr)
		{
			rr.L0_L1rx = this.m_Pool.TexL0_L1rx as RenderTexture;
			rr.L1_G_ry = this.m_Pool.TexL1_G_ry as RenderTexture;
			rr.L1_B_rz = this.m_Pool.TexL1_B_rz as RenderTexture;
			rr.L2_0 = this.m_Pool.TexL2_0 as RenderTexture;
			rr.L2_1 = this.m_Pool.TexL2_1 as RenderTexture;
			rr.L2_2 = this.m_Pool.TexL2_2 as RenderTexture;
			rr.L2_3 = this.m_Pool.TexL2_3 as RenderTexture;
			rr.Validity = this.m_Pool.TexValidity;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000129B4 File Offset: 0x00010BB4
		internal void Clear()
		{
			this.m_FreeList.Clear();
			this.m_NextFreeChunk.x = (this.m_NextFreeChunk.y = (this.m_NextFreeChunk.z = 0));
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000129F4 File Offset: 0x00010BF4
		internal static int GetChunkCount(int brickCount, int chunkSizeInBricks)
		{
			return (brickCount + chunkSizeInBricks - 1) / chunkSizeInBricks;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00012A0C File Offset: 0x00010C0C
		internal bool Allocate(int numberOfBrickChunks, List<ProbeBrickPool.BrickChunkAlloc> outAllocations, bool ignoreErrorLog)
		{
			while (this.m_FreeList.Count > 0 && numberOfBrickChunks > 0)
			{
				outAllocations.Add(this.m_FreeList.Pop());
				numberOfBrickChunks--;
				this.m_AvailableChunkCount--;
			}
			uint num = 0U;
			while ((ulong)num < (ulong)((long)numberOfBrickChunks))
			{
				if (this.m_NextFreeChunk.z >= this.m_Pool.depth)
				{
					if (!ignoreErrorLog)
					{
						Debug.LogError("Cannot allocate more brick chunks, probe volume brick pool is full.");
					}
					return false;
				}
				outAllocations.Add(this.m_NextFreeChunk);
				this.m_AvailableChunkCount--;
				this.m_NextFreeChunk.x = this.m_NextFreeChunk.x + 512;
				if (this.m_NextFreeChunk.x >= this.m_Pool.width)
				{
					this.m_NextFreeChunk.x = 0;
					this.m_NextFreeChunk.y = this.m_NextFreeChunk.y + 4;
					if (this.m_NextFreeChunk.y >= this.m_Pool.height)
					{
						this.m_NextFreeChunk.y = 0;
						this.m_NextFreeChunk.z = this.m_NextFreeChunk.z + 4;
					}
				}
				num += 1U;
			}
			return true;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00012B24 File Offset: 0x00010D24
		internal void Deallocate(List<ProbeBrickPool.BrickChunkAlloc> allocations)
		{
			this.m_AvailableChunkCount += allocations.Count;
			foreach (ProbeBrickPool.BrickChunkAlloc brickChunkAlloc in allocations)
			{
				this.m_FreeList.Push(brickChunkAlloc);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00012B8C File Offset: 0x00010D8C
		internal void Update(ProbeBrickPool.DataLocation source, List<ProbeBrickPool.BrickChunkAlloc> srcLocations, List<ProbeBrickPool.BrickChunkAlloc> dstLocations, int destStartIndex, ProbeVolumeSHBands bands)
		{
			for (int i = 0; i < srcLocations.Count; i++)
			{
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = srcLocations[i];
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc2 = dstLocations[destStartIndex + i];
				for (int j = 0; j < 4; j++)
				{
					int num = Mathf.Min(512, source.width - brickChunkAlloc.x);
					Graphics.CopyTexture(source.TexL0_L1rx, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL0_L1rx, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
					Graphics.CopyTexture(source.TexL1_G_ry, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL1_G_ry, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
					Graphics.CopyTexture(source.TexL1_B_rz, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL1_B_rz, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
					if (this.m_ContainsValidity)
					{
						Graphics.CopyTexture(source.TexValidity, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexValidity, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
					}
					if (bands == ProbeVolumeSHBands.SphericalHarmonicsL2)
					{
						Graphics.CopyTexture(source.TexL2_0, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL2_0, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
						Graphics.CopyTexture(source.TexL2_1, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL2_1, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
						Graphics.CopyTexture(source.TexL2_2, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL2_2, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
						Graphics.CopyTexture(source.TexL2_3, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexL2_3, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
					}
				}
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00012E18 File Offset: 0x00011018
		internal void UpdateValidity(ProbeBrickPool.DataLocation source, List<ProbeBrickPool.BrickChunkAlloc> srcLocations, List<ProbeBrickPool.BrickChunkAlloc> dstLocations, int destStartIndex)
		{
			for (int i = 0; i < srcLocations.Count; i++)
			{
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = srcLocations[i];
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc2 = dstLocations[destStartIndex + i];
				for (int j = 0; j < 4; j++)
				{
					int num = Mathf.Min(512, source.width - brickChunkAlloc.x);
					Graphics.CopyTexture(source.TexValidity, brickChunkAlloc.z + j, 0, brickChunkAlloc.x, brickChunkAlloc.y, num, 4, this.m_Pool.TexValidity, brickChunkAlloc2.z + j, 0, brickChunkAlloc2.x, brickChunkAlloc2.y);
				}
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00012EB4 File Offset: 0x000110B4
		internal static Vector3Int ProbeCountToDataLocSize(int numProbes)
		{
			int num = numProbes / 64;
			int num2 = 512;
			int num3 = (num + num2 * num2 - 1) / (num2 * num2);
			int num5;
			int num4;
			if (num3 > 1)
			{
				num4 = (num5 = num2);
			}
			else
			{
				num4 = (num + num2 - 1) / num2;
				if (num4 > 1)
				{
					num5 = num2;
				}
				else
				{
					num5 = num;
				}
			}
			num5 *= 4;
			num4 *= 4;
			num3 *= 4;
			return new Vector3Int(num5, num4, num3);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00012F10 File Offset: 0x00011110
		public static Texture CreateDataTexture(int width, int height, int depth, GraphicsFormat format, string name, bool allocateRendertexture, ref int allocatedBytes)
		{
			int num = ((format == GraphicsFormat.R16G16B16A16_SFloat) ? 8 : ((format == GraphicsFormat.R8G8B8A8_UNorm) ? 4 : 1));
			allocatedBytes += width * height * depth * num;
			Texture texture;
			if (allocateRendertexture)
			{
				texture = new RenderTexture(new RenderTextureDescriptor
				{
					width = width,
					height = height,
					volumeDepth = depth,
					graphicsFormat = format,
					mipCount = 1,
					enableRandomWrite = true,
					dimension = TextureDimension.Tex3D,
					msaaSamples = 1
				});
			}
			else
			{
				texture = new Texture3D(width, height, depth, format, TextureCreationFlags.None, 1);
			}
			texture.hideFlags = HideFlags.HideAndDontSave;
			texture.name = name;
			if (allocateRendertexture)
			{
				(texture as RenderTexture).Create();
			}
			return texture;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00012FC0 File Offset: 0x000111C0
		public static ProbeBrickPool.DataLocation CreateDataLocation(int numProbes, bool compressed, ProbeVolumeSHBands bands, string name, bool allocateRendertexture, bool allocateValidityData, out int allocatedBytes)
		{
			Vector3Int vector3Int = ProbeBrickPool.ProbeCountToDataLocSize(numProbes);
			int x = vector3Int.x;
			int y = vector3Int.y;
			int z = vector3Int.z;
			GraphicsFormat graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
			GraphicsFormat graphicsFormat2 = (compressed ? GraphicsFormat.RGBA_BC7_UNorm : GraphicsFormat.R8G8B8A8_UNorm);
			allocatedBytes = 0;
			ProbeBrickPool.DataLocation dataLocation;
			dataLocation.TexL0_L1rx = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat, name + "_TexL0_L1rx", allocateRendertexture, ref allocatedBytes);
			dataLocation.TexL1_G_ry = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL1_G_ry", allocateRendertexture, ref allocatedBytes);
			dataLocation.TexL1_B_rz = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL1_B_rz", allocateRendertexture, ref allocatedBytes);
			if (allocateValidityData)
			{
				dataLocation.TexValidity = ProbeBrickPool.CreateDataTexture(x, y, z, GraphicsFormat.R8_UNorm, name + "_Validity", false, ref allocatedBytes) as Texture3D;
			}
			else
			{
				dataLocation.TexValidity = null;
			}
			if (bands == ProbeVolumeSHBands.SphericalHarmonicsL2)
			{
				dataLocation.TexL2_0 = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL2_0", allocateRendertexture, ref allocatedBytes);
				dataLocation.TexL2_1 = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL2_1", allocateRendertexture, ref allocatedBytes);
				dataLocation.TexL2_2 = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL2_2", allocateRendertexture, ref allocatedBytes);
				dataLocation.TexL2_3 = ProbeBrickPool.CreateDataTexture(x, y, z, graphicsFormat2, name + "_TexL2_3", allocateRendertexture, ref allocatedBytes);
			}
			else
			{
				dataLocation.TexL2_0 = null;
				dataLocation.TexL2_1 = null;
				dataLocation.TexL2_2 = null;
				dataLocation.TexL2_3 = null;
			}
			dataLocation.width = x;
			dataLocation.height = y;
			dataLocation.depth = z;
			return dataLocation;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00013152 File Offset: 0x00011352
		private void DerivePoolSizeFromBudget(ProbeVolumeTextureMemoryBudget memoryBudget, out int width, out int height, out int depth)
		{
			width = (int)memoryBudget;
			height = (int)memoryBudget;
			depth = 4;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001315E File Offset: 0x0001135E
		internal void Cleanup()
		{
			this.m_Pool.Cleanup();
		}

		// Token: 0x0400024B RID: 587
		private const int kProbePoolChunkSizeInBricks = 128;

		// Token: 0x0400024C RID: 588
		internal const int kBrickCellCount = 3;

		// Token: 0x0400024D RID: 589
		internal const int kBrickProbeCountPerDim = 4;

		// Token: 0x0400024E RID: 590
		internal const int kBrickProbeCountTotal = 64;

		// Token: 0x0400024F RID: 591
		internal const int kChunkProbeCountPerDim = 512;

		// Token: 0x04000251 RID: 593
		private const int kMaxPoolWidth = 2048;

		// Token: 0x04000252 RID: 594
		internal ProbeBrickPool.DataLocation m_Pool;

		// Token: 0x04000253 RID: 595
		private ProbeBrickPool.BrickChunkAlloc m_NextFreeChunk;

		// Token: 0x04000254 RID: 596
		private Stack<ProbeBrickPool.BrickChunkAlloc> m_FreeList;

		// Token: 0x04000255 RID: 597
		private int m_AvailableChunkCount;

		// Token: 0x04000256 RID: 598
		private ProbeVolumeSHBands m_SHBands;

		// Token: 0x04000257 RID: 599
		private bool m_ContainsValidity;

		// Token: 0x0200019A RID: 410
		[DebuggerDisplay("Chunk ({x}, {y}, {z})")]
		public struct BrickChunkAlloc
		{
			// Token: 0x06000ACF RID: 2767 RVA: 0x0002E31E File Offset: 0x0002C51E
			internal int flattenIndex(int sx, int sy)
			{
				return this.z * (sx * sy) + this.y * sx + this.x;
			}

			// Token: 0x04000698 RID: 1688
			public int x;

			// Token: 0x04000699 RID: 1689
			public int y;

			// Token: 0x0400069A RID: 1690
			public int z;
		}

		// Token: 0x0200019B RID: 411
		public struct DataLocation
		{
			// Token: 0x06000AD0 RID: 2768 RVA: 0x0002E33C File Offset: 0x0002C53C
			internal void Cleanup()
			{
				CoreUtils.Destroy(this.TexL0_L1rx);
				CoreUtils.Destroy(this.TexL1_G_ry);
				CoreUtils.Destroy(this.TexL1_B_rz);
				CoreUtils.Destroy(this.TexL2_0);
				CoreUtils.Destroy(this.TexL2_1);
				CoreUtils.Destroy(this.TexL2_2);
				CoreUtils.Destroy(this.TexL2_3);
				CoreUtils.Destroy(this.TexValidity);
				this.TexL0_L1rx = null;
				this.TexL1_G_ry = null;
				this.TexL1_B_rz = null;
				this.TexL2_0 = null;
				this.TexL2_1 = null;
				this.TexL2_2 = null;
				this.TexL2_3 = null;
				this.TexValidity = null;
			}

			// Token: 0x0400069B RID: 1691
			internal Texture TexL0_L1rx;

			// Token: 0x0400069C RID: 1692
			internal Texture TexL1_G_ry;

			// Token: 0x0400069D RID: 1693
			internal Texture TexL1_B_rz;

			// Token: 0x0400069E RID: 1694
			internal Texture TexL2_0;

			// Token: 0x0400069F RID: 1695
			internal Texture TexL2_1;

			// Token: 0x040006A0 RID: 1696
			internal Texture TexL2_2;

			// Token: 0x040006A1 RID: 1697
			internal Texture TexL2_3;

			// Token: 0x040006A2 RID: 1698
			internal Texture3D TexValidity;

			// Token: 0x040006A3 RID: 1699
			internal int width;

			// Token: 0x040006A4 RID: 1700
			internal int height;

			// Token: 0x040006A5 RID: 1701
			internal int depth;
		}
	}
}
