using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D2 RID: 210
	internal class ShaderData : IDisposable
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x00027C44 File Offset: 0x00025E44
		private ShaderData()
		{
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00027C4C File Offset: 0x00025E4C
		internal static ShaderData instance
		{
			get
			{
				if (ShaderData.m_Instance == null)
				{
					ShaderData.m_Instance = new ShaderData();
				}
				return ShaderData.m_Instance;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00027C64 File Offset: 0x00025E64
		public void Dispose()
		{
			this.DisposeBuffer(ref this.m_LightDataBuffer);
			this.DisposeBuffer(ref this.m_LightIndicesBuffer);
			this.DisposeBuffer(ref this.m_AdditionalLightShadowParamsStructuredBuffer);
			this.DisposeBuffer(ref this.m_AdditionalLightShadowSliceMatricesStructuredBuffer);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00027C96 File Offset: 0x00025E96
		internal ComputeBuffer GetLightDataBuffer(int size)
		{
			return this.GetOrUpdateBuffer<ShaderInput.LightData>(ref this.m_LightDataBuffer, size);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00027CA5 File Offset: 0x00025EA5
		internal ComputeBuffer GetLightIndicesBuffer(int size)
		{
			return this.GetOrUpdateBuffer<int>(ref this.m_LightIndicesBuffer, size);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00027CB4 File Offset: 0x00025EB4
		internal ComputeBuffer GetAdditionalLightShadowParamsStructuredBuffer(int size)
		{
			return this.GetOrUpdateBuffer<Vector4>(ref this.m_AdditionalLightShadowParamsStructuredBuffer, size);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00027CC3 File Offset: 0x00025EC3
		internal ComputeBuffer GetAdditionalLightShadowSliceMatricesStructuredBuffer(int size)
		{
			return this.GetOrUpdateBuffer<Matrix4x4>(ref this.m_AdditionalLightShadowSliceMatricesStructuredBuffer, size);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00027CD2 File Offset: 0x00025ED2
		private ComputeBuffer GetOrUpdateBuffer<T>(ref ComputeBuffer buffer, int size) where T : struct
		{
			if (buffer == null)
			{
				buffer = new ComputeBuffer(size, Marshal.SizeOf<T>());
			}
			else if (size > buffer.count)
			{
				buffer.Dispose();
				buffer = new ComputeBuffer(size, Marshal.SizeOf<T>());
			}
			return buffer;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00027D07 File Offset: 0x00025F07
		private void DisposeBuffer(ref ComputeBuffer buffer)
		{
			if (buffer != null)
			{
				buffer.Dispose();
				buffer = null;
			}
		}

		// Token: 0x04000541 RID: 1345
		private static ShaderData m_Instance;

		// Token: 0x04000542 RID: 1346
		private ComputeBuffer m_LightDataBuffer;

		// Token: 0x04000543 RID: 1347
		private ComputeBuffer m_LightIndicesBuffer;

		// Token: 0x04000544 RID: 1348
		private ComputeBuffer m_AdditionalLightShadowParamsStructuredBuffer;

		// Token: 0x04000545 RID: 1349
		private ComputeBuffer m_AdditionalLightShadowSliceMatricesStructuredBuffer;
	}
}
