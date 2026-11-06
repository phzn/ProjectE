using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000037 RID: 55
	public class ConstantBuffer
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0000A5F8 File Offset: 0x000087F8
		public static void PushGlobal<CBType>(CommandBuffer cmd, in CBType data, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(cmd, in data);
			instance.SetGlobal(cmd, shaderId);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A60E File Offset: 0x0000880E
		public static void PushGlobal<CBType>(in CBType data, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(in data);
			instance.SetGlobal(shaderId);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A622 File Offset: 0x00008822
		public static void Push<CBType>(CommandBuffer cmd, in CBType data, ComputeShader cs, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(cmd, in data);
			instance.Set(cmd, cs, shaderId);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A639 File Offset: 0x00008839
		public static void Push<CBType>(in CBType data, ComputeShader cs, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(in data);
			instance.Set(cs, shaderId);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A64E File Offset: 0x0000884E
		public static void Push<CBType>(CommandBuffer cmd, in CBType data, Material mat, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(cmd, in data);
			instance.Set(mat, shaderId);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A664 File Offset: 0x00008864
		public static void Push<CBType>(in CBType data, Material mat, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType> instance = ConstantBufferSingleton<CBType>.instance;
			instance.UpdateData(in data);
			instance.Set(mat, shaderId);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A679 File Offset: 0x00008879
		public static void UpdateData<CBType>(CommandBuffer cmd, in CBType data) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.UpdateData(cmd, in data);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A687 File Offset: 0x00008887
		public static void UpdateData<CBType>(in CBType data) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.UpdateData(in data);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A694 File Offset: 0x00008894
		public static void SetGlobal<CBType>(CommandBuffer cmd, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.SetGlobal(cmd, shaderId);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000A6A2 File Offset: 0x000088A2
		public static void SetGlobal<CBType>(int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.SetGlobal(shaderId);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A6AF File Offset: 0x000088AF
		public static void Set<CBType>(CommandBuffer cmd, ComputeShader cs, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.Set(cmd, cs, shaderId);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A6BE File Offset: 0x000088BE
		public static void Set<CBType>(ComputeShader cs, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.Set(cs, shaderId);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A6CC File Offset: 0x000088CC
		public static void Set<CBType>(Material mat, int shaderId) where CBType : struct
		{
			ConstantBufferSingleton<CBType>.instance.Set(mat, shaderId);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A6DC File Offset: 0x000088DC
		public static void ReleaseAll()
		{
			foreach (ConstantBufferBase constantBufferBase in ConstantBuffer.m_RegisteredConstantBuffers)
			{
				constantBufferBase.Release();
			}
			ConstantBuffer.m_RegisteredConstantBuffers.Clear();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A738 File Offset: 0x00008938
		internal static void Register(ConstantBufferBase cb)
		{
			ConstantBuffer.m_RegisteredConstantBuffers.Add(cb);
		}

		// Token: 0x0400013D RID: 317
		private static List<ConstantBufferBase> m_RegisteredConstantBuffers = new List<ConstantBufferBase>();
	}
}
