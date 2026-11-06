using System;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x0200015E RID: 350
	public struct VirtualMeshBoneWeight
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x000397EB File Offset: 0x000379EB
		public bool IsValid
		{
			get
			{
				return this.weights[0] >= 1E-06f;
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00039803 File Offset: 0x00037A03
		public VirtualMeshBoneWeight(int4 boneIndices, float4 weights)
		{
			this.boneIndices = boneIndices;
			this.weights = weights;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00039814 File Offset: 0x00037A14
		public int Count
		{
			get
			{
				if (this.weights[3] > 0f)
				{
					return 4;
				}
				if (this.weights[2] > 0f)
				{
					return 3;
				}
				if (this.weights[1] > 0f)
				{
					return 2;
				}
				if (this.weights[0] > 0f)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00039878 File Offset: 0x00037A78
		public void AddWeight(int boneIndex, float weight)
		{
			if (weight < 1E-06f)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				float num2 = this.weights[i];
				if (num2 == 0f)
				{
					break;
				}
				if (this.boneIndices[i] == boneIndex)
				{
					num2 += weight;
					this.weights[i] = num2;
					for (int j = i; j >= 1; j--)
					{
						if (this.weights[j] > this.weights[j - 1])
						{
							num2 = this.weights[j - 1];
							this.weights[j - 1] = this.weights[j];
							this.weights[j] = num2;
							int num3 = this.boneIndices[j - 1];
							this.boneIndices[j - 1] = this.boneIndices[j];
							this.boneIndices[j] = num3;
						}
					}
					return;
				}
				num++;
			}
			for (int k = 0; k < 4; k++)
			{
				float num4 = this.weights[k];
				if (num4 == 0f)
				{
					this.weights[k] = weight;
					this.boneIndices[k] = boneIndex;
					return;
				}
				if (weight > num4)
				{
					for (int l = 2; l >= k; l--)
					{
						this.weights[l + 1] = this.weights[l];
						this.boneIndices[l + 1] = this.boneIndices[l];
					}
					this.weights[k] = weight;
					this.boneIndices[k] = boneIndex;
					return;
				}
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00039A34 File Offset: 0x00037C34
		public void AddWeight(in VirtualMeshBoneWeight bw)
		{
			VirtualMeshBoneWeight virtualMeshBoneWeight = bw;
			if (virtualMeshBoneWeight.IsValid)
			{
				for (int i = 0; i < 4; i++)
				{
					int4 @int = bw.boneIndices;
					int num = @int[i];
					float4 @float = bw.weights;
					this.AddWeight(num, @float[i]);
				}
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00039A84 File Offset: 0x00037C84
		public void AdjustWeight()
		{
			if (!this.IsValid)
			{
				return;
			}
			float num = math.csum(this.weights);
			float num2 = 1f / num;
			this.weights *= num2;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00039AC0 File Offset: 0x00037CC0
		public override string ToString()
		{
			return string.Format("[{0}] w({1})", this.boneIndices, this.weights);
		}

		// Token: 0x04000A8E RID: 2702
		public float4 weights;

		// Token: 0x04000A8F RID: 2703
		public int4 boneIndices;
	}
}
