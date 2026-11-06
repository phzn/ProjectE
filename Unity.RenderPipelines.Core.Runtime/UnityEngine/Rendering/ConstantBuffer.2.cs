using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering
{
	// Token: 0x02000039 RID: 57
	public class ConstantBuffer<CBType> : ConstantBufferBase where CBType : struct
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000A761 File Offset: 0x00008961
		public ConstantBuffer()
		{
			this.m_GPUConstantBuffer = new ComputeBuffer(1, UnsafeUtility.SizeOf<CBType>(), ComputeBufferType.Constant);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000A792 File Offset: 0x00008992
		public void UpdateData(CommandBuffer cmd, in CBType data)
		{
			this.m_Data[0] = data;
			cmd.SetBufferData(this.m_GPUConstantBuffer, this.m_Data);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000A7B8 File Offset: 0x000089B8
		public void UpdateData(in CBType data)
		{
			this.m_Data[0] = data;
			this.m_GPUConstantBuffer.SetData(this.m_Data);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A7DD File Offset: 0x000089DD
		public void SetGlobal(CommandBuffer cmd, int shaderId)
		{
			this.m_GlobalBindings.Add(shaderId);
			cmd.SetGlobalConstantBuffer(this.m_GPUConstantBuffer, shaderId, 0, this.m_GPUConstantBuffer.stride);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A805 File Offset: 0x00008A05
		public void SetGlobal(int shaderId)
		{
			this.m_GlobalBindings.Add(shaderId);
			Shader.SetGlobalConstantBuffer(shaderId, this.m_GPUConstantBuffer, 0, this.m_GPUConstantBuffer.stride);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A82C File Offset: 0x00008A2C
		public void Set(CommandBuffer cmd, ComputeShader cs, int shaderId)
		{
			cmd.SetComputeConstantBufferParam(cs, shaderId, this.m_GPUConstantBuffer, 0, this.m_GPUConstantBuffer.stride);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A848 File Offset: 0x00008A48
		public void Set(ComputeShader cs, int shaderId)
		{
			cs.SetConstantBuffer(shaderId, this.m_GPUConstantBuffer, 0, this.m_GPUConstantBuffer.stride);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A863 File Offset: 0x00008A63
		public void Set(Material mat, int shaderId)
		{
			mat.SetConstantBuffer(shaderId, this.m_GPUConstantBuffer, 0, this.m_GPUConstantBuffer.stride);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A87E File Offset: 0x00008A7E
		public void PushGlobal(CommandBuffer cmd, in CBType data, int shaderId)
		{
			this.UpdateData(cmd, in data);
			this.SetGlobal(cmd, shaderId);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A890 File Offset: 0x00008A90
		public void PushGlobal(in CBType data, int shaderId)
		{
			this.UpdateData(in data);
			this.SetGlobal(shaderId);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		public override void Release()
		{
			foreach (int num in this.m_GlobalBindings)
			{
				Shader.SetGlobalConstantBuffer(num, null, 0, 0);
			}
			this.m_GlobalBindings.Clear();
			CoreUtils.SafeRelease(this.m_GPUConstantBuffer);
		}

		// Token: 0x0400013E RID: 318
		private HashSet<int> m_GlobalBindings = new HashSet<int>();

		// Token: 0x0400013F RID: 319
		private CBType[] m_Data = new CBType[1];

		// Token: 0x04000140 RID: 320
		private ComputeBuffer m_GPUConstantBuffer;
	}
}
