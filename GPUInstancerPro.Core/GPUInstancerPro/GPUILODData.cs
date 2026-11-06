using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class GPUILODData : IGPUIDisposable, IDisposable
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000060F7 File Offset: 0x000042F7
		public GPUILODData()
		{
			this.rendererDataArray = new GPUIRendererData[0];
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000610B File Offset: 0x0000430B
		public int Length
		{
			get
			{
				if (this.rendererDataArray != null)
				{
					return this.rendererDataArray.Length;
				}
				return 0;
			}
		}

		// Token: 0x17000017 RID: 23
		public GPUIRendererData this[int index]
		{
			get
			{
				return this.rendererDataArray[index];
			}
			set
			{
				this.rendererDataArray[index] = value;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006134 File Offset: 0x00004334
		public void Add(GPUIRendererData renderer)
		{
			if (this.rendererDataArray == null)
			{
				this.rendererDataArray = new GPUIRendererData[1];
				this.rendererDataArray[0] = renderer;
				return;
			}
			Array.Resize<GPUIRendererData>(ref this.rendererDataArray, this.Length + 1);
			this.rendererDataArray[this.Length - 1] = renderer;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000024BC File Offset: 0x000006BC
		public void ReleaseBuffers()
		{
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006184 File Offset: 0x00004384
		public void Dispose()
		{
			for (int i = 0; i < this.Length; i++)
			{
				this[i].Dispose();
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000061B0 File Offset: 0x000043B0
		public bool IsShadowCasting()
		{
			for (int i = 0; i < this.Length; i++)
			{
				if (this.rendererDataArray[i].IsShadowCasting)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000061E0 File Offset: 0x000043E0
		internal void CreateCommandBufferArgs(GPUIProfile profile)
		{
			if (this._commandBufferArgs == null)
			{
				this._commandBufferArgs = new List<GraphicsBuffer.IndirectDrawIndexedArgs>();
			}
			else
			{
				this._commandBufferArgs.Clear();
			}
			for (int i = 0; i < this.Length; i++)
			{
				GPUIRendererData gpuirendererData = this[i];
				if (gpuirendererData.rendererMesh != null && gpuirendererData.optionalRendererNo == 0)
				{
					int subMeshCount = gpuirendererData.rendererMesh.subMeshCount;
					for (int j = 0; j < gpuirendererData.rendererMaterials.Length; j++)
					{
						int num = j;
						if (subMeshCount <= num)
						{
							num = subMeshCount - 1;
						}
						this._commandBufferArgs.Add(new GraphicsBuffer.IndirectDrawIndexedArgs
						{
							baseVertexIndex = gpuirendererData.rendererMesh.GetBaseVertex(num),
							indexCountPerInstance = gpuirendererData.rendererMesh.GetIndexCount(num),
							startIndex = gpuirendererData.rendererMesh.GetIndexStart(num),
							instanceCount = 0U,
							startInstance = 0U
						});
					}
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000062D8 File Offset: 0x000044D8
		internal List<GraphicsBuffer.IndirectDrawIndexedArgs> GetOptionalRendererCommandBufferArgs(int optionalRendererNo, GPUIProfile profile)
		{
			if (this._optionalRendererCommandBufferArgs == null)
			{
				this._optionalRendererCommandBufferArgs = new List<GraphicsBuffer.IndirectDrawIndexedArgs>();
			}
			this._optionalRendererCommandBufferArgs.Clear();
			for (int i = 0; i < this.Length; i++)
			{
				GPUIRendererData gpuirendererData = this[i];
				if (gpuirendererData.rendererMesh != null && gpuirendererData.optionalRendererNo == optionalRendererNo)
				{
					int subMeshCount = gpuirendererData.rendererMesh.subMeshCount;
					for (int j = 0; j < gpuirendererData.rendererMaterials.Length; j++)
					{
						int num = j;
						if (subMeshCount <= num)
						{
							num = subMeshCount - 1;
						}
						this._optionalRendererCommandBufferArgs.Add(new GraphicsBuffer.IndirectDrawIndexedArgs
						{
							baseVertexIndex = gpuirendererData.rendererMesh.GetBaseVertex(num),
							indexCountPerInstance = gpuirendererData.rendererMesh.GetIndexCount(num),
							startIndex = gpuirendererData.rendererMesh.GetIndexStart(num),
							instanceCount = 0U,
							startInstance = 0U
						});
					}
				}
			}
			return this._optionalRendererCommandBufferArgs;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000063D2 File Offset: 0x000045D2
		internal List<GraphicsBuffer.IndirectDrawIndexedArgs> GetCommandBufferArgs(GPUIProfile profile)
		{
			if (this._commandBufferArgs == null)
			{
				this.CreateCommandBufferArgs(profile);
			}
			return this._commandBufferArgs;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000063EC File Offset: 0x000045EC
		internal void LoadShaderCommandParams(List<GPUIShaderCommandParams> shaderCommandParams, int instanceDataBufferShiftMultiplier, int lodNo)
		{
			for (int i = 0; i < this.Length; i++)
			{
				GPUIRendererData gpuirendererData = this[i];
				if (gpuirendererData.rendererMesh != null && gpuirendererData.optionalRendererNo == 0)
				{
					for (int j = 0; j < gpuirendererData.rendererMaterials.Length; j++)
					{
						shaderCommandParams.Add(new GPUIShaderCommandParams
						{
							key = lodNo + 10 * i,
							transformOffset = gpuirendererData.transformOffset,
							instanceDataBufferShiftMultiplier = instanceDataBufferShiftMultiplier
						});
					}
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000646C File Offset: 0x0000466C
		internal void LoadShaderCommandParamsForOptionalRenderers(List<GPUIShaderCommandParams> shaderCommandParams, int instanceDataBufferShiftMultiplier, int optionalRendererNo)
		{
			for (int i = 0; i < this.Length; i++)
			{
				GPUIRendererData gpuirendererData = this[i];
				if (gpuirendererData.rendererMesh != null && gpuirendererData.optionalRendererNo == optionalRendererNo)
				{
					for (int j = 0; j < gpuirendererData.rendererMaterials.Length; j++)
					{
						shaderCommandParams.Add(new GPUIShaderCommandParams
						{
							key = 10 * i,
							transformOffset = gpuirendererData.transformOffset,
							instanceDataBufferShiftMultiplier = instanceDataBufferShiftMultiplier
						});
					}
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000064EC File Offset: 0x000046EC
		public void RemoveReplacementMaterials()
		{
			if (this.rendererDataArray == null)
			{
				return;
			}
			GPUIRendererData[] array = this.rendererDataArray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveReplacementMaterials();
			}
		}

		// Token: 0x04000079 RID: 121
		public GPUIRendererData[] rendererDataArray;

		// Token: 0x0400007A RID: 122
		[NonSerialized]
		private List<GraphicsBuffer.IndirectDrawIndexedArgs> _commandBufferArgs;

		// Token: 0x0400007B RID: 123
		[NonSerialized]
		private List<GraphicsBuffer.IndirectDrawIndexedArgs> _optionalRendererCommandBufferArgs;
	}
}
