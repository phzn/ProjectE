using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200000C RID: 12
	public static class XRSystem
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00003A39 File Offset: 0x00001C39
		public static XRDisplaySubsystem GetActiveDisplay()
		{
			return XRSystem.s_Display;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003A40 File Offset: 0x00001C40
		public static bool displayActive
		{
			get
			{
				return XRSystem.s_Display != null && XRSystem.s_Display.running;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003A58 File Offset: 0x00001C58
		public static bool isHDRDisplayOutputActive
		{
			get
			{
				XRDisplaySubsystem xrdisplaySubsystem = XRSystem.s_Display;
				bool? flag;
				if (xrdisplaySubsystem == null)
				{
					flag = null;
				}
				else
				{
					HDROutputSettings hdrOutputSettings = xrdisplaySubsystem.hdrOutputSettings;
					flag = ((hdrOutputSettings != null) ? new bool?(hdrOutputSettings.active) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003A9F File Offset: 0x00001C9F
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003AA6 File Offset: 0x00001CA6
		public static bool singlePassAllowed { get; set; } = true;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003AAE File Offset: 0x00001CAE
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003AB5 File Offset: 0x00001CB5
		public static FoveatedRenderingCaps foveatedRenderingCaps { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003ABD File Offset: 0x00001CBD
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003AC4 File Offset: 0x00001CC4
		public static bool dumpDebugInfo { get; set; } = false;

		// Token: 0x06000064 RID: 100 RVA: 0x00003ACC File Offset: 0x00001CCC
		public static void Initialize(Func<XRPassCreateInfo, XRPass> passAllocator, Shader occlusionMeshPS, Shader mirrorViewPS)
		{
			if (passAllocator == null)
			{
				throw new ArgumentNullException("passCreator");
			}
			XRSystem.s_PassAllocator = passAllocator;
			XRSystem.RefreshDeviceInfo();
			XRSystem.foveatedRenderingCaps = SystemInfo.foveatedRenderingCaps;
			if (occlusionMeshPS != null && XRSystem.s_OcclusionMeshMaterial == null)
			{
				XRSystem.s_OcclusionMeshMaterial = CoreUtils.CreateEngineMaterial(occlusionMeshPS);
			}
			if (mirrorViewPS != null && XRSystem.s_MirrorViewMaterial == null)
			{
				XRSystem.s_MirrorViewMaterial = CoreUtils.CreateEngineMaterial(mirrorViewPS);
			}
			if (XRGraphicsAutomatedTests.enabled)
			{
				XRSystem.SetLayoutOverride(new Action<XRLayout, Camera>(XRGraphicsAutomatedTests.OverrideLayout));
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003B58 File Offset: 0x00001D58
		public static void SetDisplayMSAASamples(MSAASamples msaaSamples)
		{
			if (XRSystem.s_MSAASamples == msaaSamples)
			{
				return;
			}
			XRSystem.s_MSAASamples = msaaSamples;
			SubsystemManager.GetInstances<XRDisplaySubsystem>(XRSystem.s_DisplayList);
			foreach (XRDisplaySubsystem xrdisplaySubsystem in XRSystem.s_DisplayList)
			{
				xrdisplaySubsystem.SetMSAALevel((int)XRSystem.s_MSAASamples);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public static MSAASamples GetDisplayMSAASamples()
		{
			return XRSystem.s_MSAASamples;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public static void SetRenderScale(float renderScale)
		{
			SubsystemManager.GetInstances<XRDisplaySubsystem>(XRSystem.s_DisplayList);
			foreach (XRDisplaySubsystem xrdisplaySubsystem in XRSystem.s_DisplayList)
			{
				xrdisplaySubsystem.scaleOfAllRenderTargets = renderScale;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003C2C File Offset: 0x00001E2C
		public static XRLayout NewLayout()
		{
			XRSystem.RefreshDeviceInfo();
			if (XRSystem.s_Layout.GetActivePasses().Count > 0)
			{
				Debug.LogWarning("Render Pipeline error : the XR layout still contains active passes. Executing XRSystem.EndLayout() right now.");
				XRSystem.EndLayout();
			}
			return XRSystem.s_Layout;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003C59 File Offset: 0x00001E59
		public static void EndLayout()
		{
			if (XRSystem.dumpDebugInfo)
			{
				XRSystem.s_Layout.LogDebugInfo();
			}
			XRSystem.s_Layout.Clear();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003C76 File Offset: 0x00001E76
		public static void RenderMirrorView(CommandBuffer cmd, Camera camera)
		{
			XRMirrorView.RenderMirrorView(cmd, camera, XRSystem.s_MirrorViewMaterial, XRSystem.s_Display);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003C89 File Offset: 0x00001E89
		public static void Dispose()
		{
			if (XRSystem.s_OcclusionMeshMaterial != null)
			{
				CoreUtils.Destroy(XRSystem.s_OcclusionMeshMaterial);
				XRSystem.s_OcclusionMeshMaterial = null;
			}
			if (XRSystem.s_MirrorViewMaterial != null)
			{
				CoreUtils.Destroy(XRSystem.s_MirrorViewMaterial);
				XRSystem.s_MirrorViewMaterial = null;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003CC5 File Offset: 0x00001EC5
		internal static void SetDisplayZRange(float zNear, float zFar)
		{
			if (XRSystem.s_Display != null)
			{
				XRSystem.s_Display.zNear = zNear;
				XRSystem.s_Display.zFar = zFar;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003CE4 File Offset: 0x00001EE4
		private static void SetLayoutOverride(Action<XRLayout, Camera> action)
		{
			XRSystem.s_LayoutOverride = action;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003CEC File Offset: 0x00001EEC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static void XRSystemInit()
		{
			if (GraphicsSettings.currentRenderPipeline != null)
			{
				XRSystem.RefreshDeviceInfo();
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003D00 File Offset: 0x00001F00
		private static void RefreshDeviceInfo()
		{
			SubsystemManager.GetInstances<XRDisplaySubsystem>(XRSystem.s_DisplayList);
			if (XRSystem.s_DisplayList.Count <= 0)
			{
				XRSystem.s_Display = null;
				return;
			}
			if (XRSystem.s_DisplayList.Count > 1)
			{
				throw new NotImplementedException("Only one XR display is supported!");
			}
			XRSystem.s_Display = XRSystem.s_DisplayList[0];
			XRSystem.s_Display.disableLegacyRenderer = true;
			XRSystem.s_Display.sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
			XRSystem.s_Display.textureLayout = XRDisplaySubsystem.TextureLayout.Texture2DArray;
			TextureXR.maxViews = Math.Max(TextureXR.slices, 2);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003D8C File Offset: 0x00001F8C
		internal static void CreateDefaultLayout(Camera camera)
		{
			if (XRSystem.s_Display == null)
			{
				throw new NullReferenceException("s_Display");
			}
			for (int i = 0; i < XRSystem.s_Display.GetRenderPassCount(); i++)
			{
				XRDisplaySubsystem.XRRenderPass xrrenderPass;
				XRSystem.s_Display.GetRenderPass(i, out xrrenderPass);
				ScriptableCullingParameters scriptableCullingParameters;
				XRSystem.s_Display.GetCullingParameters(camera, xrrenderPass.cullingPassIndex, out scriptableCullingParameters);
				if (XRSystem.CanUseSinglePass(camera, xrrenderPass))
				{
					XRPass xrpass = XRSystem.s_PassAllocator(XRSystem.BuildPass(xrrenderPass, scriptableCullingParameters));
					for (int j = 0; j < xrrenderPass.GetRenderParameterCount(); j++)
					{
						XRDisplaySubsystem.XRRenderParameter xrrenderParameter;
						xrrenderPass.GetRenderParameter(camera, j, out xrrenderParameter);
						xrpass.AddView(XRSystem.BuildView(xrrenderPass, xrrenderParameter));
					}
					XRSystem.s_Layout.AddPass(camera, xrpass);
				}
				else
				{
					for (int k = 0; k < xrrenderPass.GetRenderParameterCount(); k++)
					{
						XRDisplaySubsystem.XRRenderParameter xrrenderParameter2;
						xrrenderPass.GetRenderParameter(camera, k, out xrrenderParameter2);
						XRPass xrpass2 = XRSystem.s_PassAllocator(XRSystem.BuildPass(xrrenderPass, scriptableCullingParameters));
						xrpass2.AddView(XRSystem.BuildView(xrrenderPass, xrrenderParameter2));
						XRSystem.s_Layout.AddPass(camera, xrpass2);
					}
				}
			}
			if (XRSystem.s_LayoutOverride != null)
			{
				XRSystem.s_LayoutOverride(XRSystem.s_Layout, camera);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003EA8 File Offset: 0x000020A8
		internal static void ReconfigurePass(XRPass xrPass, Camera camera)
		{
			if (xrPass.enabled && XRSystem.s_Display != null)
			{
				XRDisplaySubsystem.XRRenderPass xrrenderPass;
				XRSystem.s_Display.GetRenderPass(xrPass.multipassId, out xrrenderPass);
				ScriptableCullingParameters scriptableCullingParameters;
				XRSystem.s_Display.GetCullingParameters(camera, xrrenderPass.cullingPassIndex, out scriptableCullingParameters);
				xrPass.AssignCullingParams(xrrenderPass.cullingPassIndex, scriptableCullingParameters);
				for (int i = 0; i < xrrenderPass.GetRenderParameterCount(); i++)
				{
					XRDisplaySubsystem.XRRenderParameter xrrenderParameter;
					xrrenderPass.GetRenderParameter(camera, i, out xrrenderParameter);
					xrPass.AssignView(i, XRSystem.BuildView(xrrenderPass, xrrenderParameter));
				}
				if (XRSystem.s_LayoutOverride != null)
				{
					XRSystem.s_LayoutOverride(XRSystem.s_Layout, camera);
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003F38 File Offset: 0x00002138
		private static bool CanUseSinglePass(Camera camera, XRDisplaySubsystem.XRRenderPass renderPass)
		{
			if (!XRSystem.singlePassAllowed)
			{
				return false;
			}
			if (renderPass.renderTargetDesc.dimension != TextureDimension.Tex2DArray)
			{
				return false;
			}
			if (renderPass.GetRenderParameterCount() != 2 || renderPass.renderTargetDesc.volumeDepth != 2)
			{
				return false;
			}
			XRDisplaySubsystem.XRRenderParameter xrrenderParameter;
			renderPass.GetRenderParameter(camera, 0, out xrrenderParameter);
			XRDisplaySubsystem.XRRenderParameter xrrenderParameter2;
			renderPass.GetRenderParameter(camera, 1, out xrrenderParameter2);
			return xrrenderParameter.textureArraySlice == 0 && xrrenderParameter2.textureArraySlice == 1 && !(xrrenderParameter.viewport != xrrenderParameter2.viewport);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003FBC File Offset: 0x000021BC
		private static XRView BuildView(XRDisplaySubsystem.XRRenderPass renderPass, XRDisplaySubsystem.XRRenderParameter renderParameter)
		{
			Rect viewport = renderParameter.viewport;
			viewport.x *= (float)renderPass.renderTargetDesc.width;
			viewport.width *= (float)renderPass.renderTargetDesc.width;
			viewport.y *= (float)renderPass.renderTargetDesc.height;
			viewport.height *= (float)renderPass.renderTargetDesc.height;
			Mesh mesh = (XRGraphicsAutomatedTests.running ? null : renderParameter.occlusionMesh);
			return new XRView(renderParameter.projection, renderParameter.view, viewport, mesh, renderParameter.textureArraySlice);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004068 File Offset: 0x00002268
		private static XRPassCreateInfo BuildPass(XRDisplaySubsystem.XRRenderPass xrRenderPass, ScriptableCullingParameters cullingParameters)
		{
			RenderTextureDescriptor renderTargetDesc = xrRenderPass.renderTargetDesc;
			RenderTextureDescriptor renderTextureDescriptor = new RenderTextureDescriptor(renderTargetDesc.width, renderTargetDesc.height, renderTargetDesc.colorFormat, renderTargetDesc.depthBufferBits, renderTargetDesc.mipCount);
			renderTextureDescriptor.dimension = xrRenderPass.renderTargetDesc.dimension;
			renderTextureDescriptor.volumeDepth = xrRenderPass.renderTargetDesc.volumeDepth;
			renderTextureDescriptor.vrUsage = xrRenderPass.renderTargetDesc.vrUsage;
			renderTextureDescriptor.sRGB = xrRenderPass.renderTargetDesc.sRGB;
			return new XRPassCreateInfo
			{
				renderTarget = xrRenderPass.renderTarget,
				renderTargetDesc = renderTextureDescriptor,
				cullingParameters = cullingParameters,
				occlusionMeshMaterial = XRSystem.s_OcclusionMeshMaterial,
				foveatedRenderingInfo = xrRenderPass.foveatedRenderingInfo,
				multipassId = XRSystem.s_Layout.GetActivePasses().Count,
				cullingPassId = xrRenderPass.cullingPassIndex,
				copyDepth = xrRenderPass.shouldFillOutDepth,
				xrSdkRenderPass = xrRenderPass
			};
		}

		// Token: 0x04000043 RID: 67
		private static XRLayout s_Layout = new XRLayout();

		// Token: 0x04000044 RID: 68
		private static Func<XRPassCreateInfo, XRPass> s_PassAllocator = null;

		// Token: 0x04000045 RID: 69
		private static List<XRDisplaySubsystem> s_DisplayList = new List<XRDisplaySubsystem>();

		// Token: 0x04000046 RID: 70
		private static XRDisplaySubsystem s_Display;

		// Token: 0x04000047 RID: 71
		private static MSAASamples s_MSAASamples = MSAASamples.None;

		// Token: 0x04000048 RID: 72
		private static Material s_OcclusionMeshMaterial;

		// Token: 0x04000049 RID: 73
		private static Material s_MirrorViewMaterial;

		// Token: 0x0400004A RID: 74
		private static Action<XRLayout, Camera> s_LayoutOverride = null;

		// Token: 0x0400004B RID: 75
		public static readonly XRPass emptyPass = new XRPass();
	}
}
