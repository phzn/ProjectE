using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AF RID: 175
	internal class HDRDebugViewPass : ScriptableRenderPass
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x00020A60 File Offset: 0x0001EC60
		public HDRDebugViewPass(Material mat)
		{
			base.profilingSampler = new ProfilingSampler("HDRDebugViewPass");
			base.renderPassEvent = (RenderPassEvent)1003;
			this.m_PassData = new HDRDebugViewPass.PassData
			{
				material = mat
			};
			base.useNativeRenderPass = false;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00020A9C File Offset: 0x0001EC9C
		public static void ConfigureDescriptor(ref RenderTextureDescriptor descriptor)
		{
			descriptor.useMipMap = false;
			descriptor.autoGenerateMips = false;
			descriptor.useDynamicScale = true;
			descriptor.depthBufferBits = 0;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00020ABC File Offset: 0x0001ECBC
		public static void ConfigureDescriptorForCIEPrepass(ref RenderTextureDescriptor descriptor)
		{
			descriptor.graphicsFormat = GraphicsFormat.R32_SFloat;
			descriptor.width = (descriptor.height = HDRDebugViewPass.ShaderConstants._SizeOfHDRXYMapping);
			descriptor.enableRandomWrite = true;
			descriptor.msaaSamples = 1;
			descriptor.dimension = TextureDimension.Tex2D;
			descriptor.vrUsage = VRTextureUsage.None;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00020B04 File Offset: 0x0001ED04
		internal static Vector4 GetLuminanceParameters(ref CameraData cameraData)
		{
			Vector4 zero = Vector4.zero;
			if (cameraData.isHDROutputActive)
			{
				Tonemapping component = VolumeManager.instance.stack.GetComponent<Tonemapping>();
				UniversalRenderPipeline.GetHDROutputLuminanceParameters(cameraData.hdrDisplayInformation, cameraData.hdrDisplayColorGamut, component, out zero);
			}
			else
			{
				zero.z = 1f;
			}
			return zero;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00020B54 File Offset: 0x0001ED54
		private static void ExecuteCIExyPrepass(CommandBuffer cmd, HDRDebugViewPass.PassData data, RTHandle sourceTexture, RTHandle xyTarget, RTHandle destTexture)
		{
			using (new ProfilingScope(cmd, new ProfilingSampler("Generate HDR DebugView CIExy")))
			{
				CoreUtils.SetRenderTarget(cmd, destTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
				Vector4 vector = new Vector4((float)HDRDebugViewPass.ShaderConstants._SizeOfHDRXYMapping, (float)HDRDebugViewPass.ShaderConstants._SizeOfHDRXYMapping, 0f, 0f);
				cmd.SetRandomWriteTarget(HDRDebugViewPass.ShaderConstants._CIExyUAVIndex, xyTarget);
				data.material.SetVector(HDRDebugViewPass.ShaderConstants._HDRDebugParamsId, vector);
				data.material.SetVector(ShaderPropertyId.hdrOutputLuminanceParams, data.luminanceParameters);
				Vector2 vector2 = (sourceTexture.useScaling ? new Vector2(sourceTexture.rtHandleProperties.rtHandleScale.x, sourceTexture.rtHandleProperties.rtHandleScale.y) : Vector2.one);
				Blitter.BlitTexture(cmd, sourceTexture, vector2, data.material, 0);
				cmd.ClearRandomWriteTargets();
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00020C4C File Offset: 0x0001EE4C
		private static void ExecuteHDRDebugViewFinalPass(CommandBuffer cmd, HDRDebugViewPass.PassData data, RTHandle sourceTexture, RTHandle destination, RTHandle xyTarget)
		{
			using (new ProfilingScope(cmd, new ProfilingSampler("HDR DebugView")))
			{
				if (data.cameraData.isHDROutputActive)
				{
					HDROutputUtils.ConfigureHDROutput(data.material, data.cameraData.hdrDisplayColorGamut, HDROutputUtils.Operation.ColorEncoding);
				}
				data.material.SetTexture(HDRDebugViewPass.ShaderConstants._xyTextureId, xyTarget);
				Vector4 vector = new Vector4((float)HDRDebugViewPass.ShaderConstants._SizeOfHDRXYMapping, (float)HDRDebugViewPass.ShaderConstants._SizeOfHDRXYMapping, 0f, 0f);
				data.material.SetVector(HDRDebugViewPass.ShaderConstants._HDRDebugParamsId, vector);
				data.material.SetVector(ShaderPropertyId.hdrOutputLuminanceParams, data.luminanceParameters);
				data.material.SetInteger(HDRDebugViewPass.ShaderConstants._DebugHDRModeId, (int)data.hdrDebugMode);
				RenderingUtils.FinalBlit(cmd, ref data.cameraData, sourceTexture, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, data.material, 1);
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00020D38 File Offset: 0x0001EF38
		public void Dispose()
		{
			RTHandle ciexyTarget = this.m_CIExyTarget;
			if (ciexyTarget != null)
			{
				ciexyTarget.Release();
			}
			RTHandle passthroughRT = this.m_PassthroughRT;
			if (passthroughRT != null)
			{
				passthroughRT.Release();
			}
			RTHandle cameraTargetHandle = this.m_CameraTargetHandle;
			if (cameraTargetHandle == null)
			{
				return;
			}
			cameraTargetHandle.Release();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00020D6C File Offset: 0x0001EF6C
		public void Setup(ref CameraData cameraData, HDRDebugMode hdrdebugMode)
		{
			RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
			DebugHandler.ConfigureColorDescriptorForDebugScreen(ref cameraTargetDescriptor, cameraData.pixelWidth, cameraData.pixelHeight);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_PassthroughRT, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_HDRDebugDummyRT");
			HDRDebugViewPass.ConfigureDescriptorForCIEPrepass(ref cameraTargetDescriptor);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_CIExyTarget, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_xyBuffer");
			this.m_PassData.hdrDebugMode = hdrdebugMode;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00020DE0 File Offset: 0x0001EFE0
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			this.m_PassData.luminanceParameters = HDRDebugViewPass.GetLuminanceParameters(ref renderingData.cameraData);
			this.m_PassData.cameraData = renderingData.cameraData;
			RTHandle cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
			RenderTargetIdentifier cameraTargetIdentifier = RenderingUtils.GetCameraTargetIdentifier(ref renderingData);
			if (this.m_CameraTargetHandle != cameraTargetIdentifier)
			{
				RTHandle cameraTargetHandle = this.m_CameraTargetHandle;
				if (cameraTargetHandle != null)
				{
					cameraTargetHandle.Release();
				}
				this.m_CameraTargetHandle = RTHandles.Alloc(cameraTargetIdentifier);
			}
			this.m_PassData.material.enabledKeywords = null;
			CoreUtils.SetRenderTarget(commandBuffer, this.m_CIExyTarget, ClearFlag.Color, Color.clear, 0, CubemapFace.Unknown, -1);
			this.ExecutePass(commandBuffer, this.m_PassData, cameraColorTargetHandle, this.m_CIExyTarget);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		private void ExecutePass(CommandBuffer cmd, HDRDebugViewPass.PassData data, RTHandle sourceTexture, RTHandle xyTarget)
		{
			HDRDebugViewPass.ExecuteCIExyPrepass(cmd, data, sourceTexture, xyTarget, this.m_PassthroughRT);
			HDRDebugViewPass.ExecuteHDRDebugViewFinalPass(cmd, data, this.m_PassthroughRT, this.m_CameraTargetHandle, xyTarget);
			data.cameraData.renderer.ConfigureCameraTarget(this.m_CameraTargetHandle, this.m_CameraTargetHandle);
		}

		// Token: 0x0400044F RID: 1103
		private HDRDebugViewPass.PassData m_PassData;

		// Token: 0x04000450 RID: 1104
		private RTHandle m_CIExyTarget;

		// Token: 0x04000451 RID: 1105
		private RTHandle m_PassthroughRT;

		// Token: 0x04000452 RID: 1106
		private RTHandle m_CameraTargetHandle;

		// Token: 0x0200019D RID: 413
		private enum HDRDebugPassId
		{
			// Token: 0x04000ABA RID: 2746
			CIExyPrepass,
			// Token: 0x04000ABB RID: 2747
			DebugViewPass
		}

		// Token: 0x0200019E RID: 414
		private class PassData
		{
			// Token: 0x04000ABC RID: 2748
			internal Material material;

			// Token: 0x04000ABD RID: 2749
			internal HDRDebugMode hdrDebugMode;

			// Token: 0x04000ABE RID: 2750
			internal Vector4 luminanceParameters;

			// Token: 0x04000ABF RID: 2751
			internal CameraData cameraData;
		}

		// Token: 0x0200019F RID: 415
		internal class ShaderConstants
		{
			// Token: 0x04000AC0 RID: 2752
			public static readonly int _DebugHDRModeId = Shader.PropertyToID("_DebugHDRMode");

			// Token: 0x04000AC1 RID: 2753
			public static readonly int _HDRDebugParamsId = Shader.PropertyToID("_HDRDebugParams");

			// Token: 0x04000AC2 RID: 2754
			public static readonly int _xyTextureId = Shader.PropertyToID("_xyBuffer");

			// Token: 0x04000AC3 RID: 2755
			public static readonly int _SizeOfHDRXYMapping = 512;

			// Token: 0x04000AC4 RID: 2756
			public static readonly int _CIExyUAVIndex = 1;
		}
	}
}
