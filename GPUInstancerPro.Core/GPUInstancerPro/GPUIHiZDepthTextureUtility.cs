using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x02000063 RID: 99
	public static class GPUIHiZDepthTextureUtility
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x00014658 File Offset: 0x00012858
		public static void CopyHiZTextureWithComputeShader(ComputeShader cs, Texture source, Texture destination, int offsetX)
		{
			int num = 0;
			cs.SetTexture(num, GPUIConstants.PROP_source, source);
			cs.SetTexture(num, GPUIConstants.PROP_destination, destination);
			cs.SetInt(GPUIConstants.PROP_offsetX, offsetX);
			cs.SetInt(GPUIConstants.PROP_sourceSizeX, source.width);
			cs.SetInt(GPUIConstants.PROP_sourceSizeY, source.height);
			cs.SetInt(GPUIConstants.PROP_reverseZ, GPUIRuntimeSettings.Instance.ReversedZBuffer ? 1 : 0);
			cs.DispatchXY(num, source.width, source.height);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000146E0 File Offset: 0x000128E0
		public static void CopyHiZTextureWithComputeShader(ComputeShader cs, CommandBuffer commandBuffer, RenderTargetIdentifier sourceIdentifier, int sourceW, int sourceH, RenderTargetIdentifier destinationIdentifier, int offsetX)
		{
			int num = 0;
			commandBuffer.SetComputeTextureParam(cs, num, GPUIConstants.PROP_source, sourceIdentifier, 0, GPUIRuntimeSettings.Instance.IsBuiltInRP ? RenderTextureSubElement.Depth : RenderTextureSubElement.Default);
			commandBuffer.SetComputeTextureParam(cs, num, GPUIConstants.PROP_destination, destinationIdentifier);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_offsetX, offsetX);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeX, sourceW);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeY, sourceH);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_reverseZ, GPUIRuntimeSettings.Instance.ReversedZBuffer ? 1 : 0);
			commandBuffer.DispatchCompute(cs, num, Mathf.CeilToInt((float)sourceW / GPUIConstants.CS_THREAD_COUNT_2D), Mathf.CeilToInt((float)sourceH / GPUIConstants.CS_THREAD_COUNT_2D), 1);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00014788 File Offset: 0x00012988
		public static void CopyHiZTextureArrayWithComputeShader(ComputeShader cs, Texture source, Texture destination, int offsetX, int textureArrayIndex)
		{
			int num = 1;
			cs.SetTexture(num, GPUIConstants.PROP_textureArray, source);
			cs.SetTexture(num, GPUIConstants.PROP_destination, destination);
			cs.SetInt(GPUIConstants.PROP_offsetX, offsetX);
			cs.SetInt(GPUIConstants.PROP_textureArrayIndex, textureArrayIndex);
			cs.SetInt(GPUIConstants.PROP_sourceSizeX, source.width);
			cs.SetInt(GPUIConstants.PROP_sourceSizeY, source.height);
			cs.SetInt(GPUIConstants.PROP_reverseZ, GPUIRuntimeSettings.Instance.ReversedZBuffer ? 1 : 0);
			cs.DispatchXY(num, source.width, source.height);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001481C File Offset: 0x00012A1C
		public static void CopyHiZTextureArrayWithComputeShader(ComputeShader cs, CommandBuffer commandBuffer, RenderTargetIdentifier sourceIdentifier, int sourceW, int sourceH, RenderTargetIdentifier destinationIdentifier, int offsetX, int textureArrayIndex)
		{
			int num = 1;
			commandBuffer.SetComputeTextureParam(cs, num, GPUIConstants.PROP_textureArray, sourceIdentifier);
			commandBuffer.SetComputeTextureParam(cs, num, GPUIConstants.PROP_destination, destinationIdentifier);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_offsetX, offsetX);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeX, sourceW);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeY, sourceH);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_reverseZ, GPUIRuntimeSettings.Instance.ReversedZBuffer ? 1 : 0);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_textureArrayIndex, textureArrayIndex);
			commandBuffer.DispatchCompute(cs, num, Mathf.CeilToInt((float)sourceW / GPUIConstants.CS_THREAD_COUNT_2D), Mathf.CeilToInt((float)sourceH / GPUIConstants.CS_THREAD_COUNT_2D), 1);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000148BF File Offset: 0x00012ABF
		private static void GetMipSize(int width, int height, int sourceMip, int destinationMip, out int destinationW, out int destinationH, out int sourceW, out int sourceH)
		{
			sourceW = Mathf.Max(width >> sourceMip, 1);
			sourceH = Mathf.Max(height >> sourceMip, 1);
			destinationW = Mathf.Max(width >> destinationMip, 1);
			destinationH = Mathf.Max(height >> destinationMip, 1);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00014900 File Offset: 0x00012B00
		public static void ReduceTextureWithComputeShader(ComputeShader cs, Texture source, int sourceW, int sourceH, int sourceMip, int destinationMip)
		{
			int num;
			int num2;
			GPUIHiZDepthTextureUtility.GetMipSize(sourceW, sourceH, sourceMip, destinationMip, out num, out num2, out sourceW, out sourceH);
			int num3 = 0;
			cs.SetTexture(num3, GPUIConstants.PROP_source, source, sourceMip);
			cs.SetTexture(num3, GPUIConstants.PROP_destination, source, destinationMip);
			cs.SetInt(GPUIConstants.PROP_sourceSizeX, sourceW);
			cs.SetInt(GPUIConstants.PROP_sourceSizeY, sourceH);
			cs.SetInt(GPUIConstants.PROP_destinationSizeX, num);
			cs.SetInt(GPUIConstants.PROP_destinationSizeY, num2);
			cs.DispatchXY(num3, num, num2);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001497C File Offset: 0x00012B7C
		public static void ReduceTextureWithComputeShader(ComputeShader cs, CommandBuffer commandBuffer, RenderTargetIdentifier sourceIdentifier, int sourceW, int sourceH, int sourceMip, int destinationMip)
		{
			int num;
			int num2;
			GPUIHiZDepthTextureUtility.GetMipSize(sourceW, sourceH, sourceMip, destinationMip, out num, out num2, out sourceW, out sourceH);
			int num3 = 0;
			commandBuffer.SetComputeTextureParam(cs, num3, GPUIConstants.PROP_source, sourceIdentifier, sourceMip);
			commandBuffer.SetComputeTextureParam(cs, num3, GPUIConstants.PROP_destination, sourceIdentifier, destinationMip);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeX, sourceW);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_sourceSizeY, sourceH);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_destinationSizeX, num);
			commandBuffer.SetComputeIntParam(cs, GPUIConstants.PROP_destinationSizeY, num2);
			commandBuffer.DispatchCompute(cs, num3, Mathf.CeilToInt((float)num / GPUIConstants.CS_THREAD_COUNT_2D), Mathf.CeilToInt((float)num2 / GPUIConstants.CS_THREAD_COUNT_2D), 1);
		}
	}
}
