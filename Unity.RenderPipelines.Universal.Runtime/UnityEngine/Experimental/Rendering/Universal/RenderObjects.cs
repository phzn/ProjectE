using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experimental.Rendering.Universal
{
	// Token: 0x02000009 RID: 9
	[ExcludeFromPreset]
	[Tooltip("Render Objects simplifies the injection of additional render passes by exposing a selection of commonly used settings.")]
	public class RenderObjects : ScriptableRendererFeature
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002CBC File Offset: 0x00000EBC
		public override void Create()
		{
			RenderObjects.FilterSettings filterSettings = this.settings.filterSettings;
			if (this.settings.Event < RenderPassEvent.BeforeRenderingPrePasses)
			{
				this.settings.Event = RenderPassEvent.BeforeRenderingPrePasses;
			}
			this.renderObjectsPass = new RenderObjectsPass(this.settings.passTag, this.settings.Event, filterSettings.PassNames, filterSettings.RenderQueueType, filterSettings.LayerMask, this.settings.cameraSettings);
			switch (this.settings.overrideMode)
			{
			case RenderObjects.RenderObjectsSettings.OverrideMaterialMode.None:
				this.renderObjectsPass.overrideMaterial = null;
				this.renderObjectsPass.overrideShader = null;
				break;
			case RenderObjects.RenderObjectsSettings.OverrideMaterialMode.Material:
				this.renderObjectsPass.overrideMaterial = this.settings.overrideMaterial;
				this.renderObjectsPass.overrideMaterialPassIndex = this.settings.overrideMaterialPassIndex;
				this.renderObjectsPass.overrideShader = null;
				break;
			case RenderObjects.RenderObjectsSettings.OverrideMaterialMode.Shader:
				this.renderObjectsPass.overrideMaterial = null;
				this.renderObjectsPass.overrideShader = this.settings.overrideShader;
				this.renderObjectsPass.overrideShaderPassIndex = this.settings.overrideShaderPassIndex;
				break;
			}
			if (this.settings.overrideDepthState)
			{
				this.renderObjectsPass.SetDetphState(this.settings.enableWrite, this.settings.depthCompareFunction);
			}
			if (this.settings.stencilSettings.overrideStencilState)
			{
				this.renderObjectsPass.SetStencilState(this.settings.stencilSettings.stencilReference, this.settings.stencilSettings.stencilCompareFunction, this.settings.stencilSettings.passOperation, this.settings.stencilSettings.failOperation, this.settings.stencilSettings.zFailOperation);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002E84 File Offset: 0x00001084
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.cameraType == CameraType.Preview || UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData))
			{
				return;
			}
			renderer.EnqueuePass(this.renderObjectsPass);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002EAE File Offset: 0x000010AE
		internal override bool SupportsNativeRenderPass()
		{
			return true;
		}

		// Token: 0x04000021 RID: 33
		public RenderObjects.RenderObjectsSettings settings = new RenderObjects.RenderObjectsSettings();

		// Token: 0x04000022 RID: 34
		private RenderObjectsPass renderObjectsPass;

		// Token: 0x0200014C RID: 332
		[Serializable]
		public class RenderObjectsSettings
		{
			// Token: 0x04000952 RID: 2386
			public string passTag = "RenderObjectsFeature";

			// Token: 0x04000953 RID: 2387
			public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

			// Token: 0x04000954 RID: 2388
			public RenderObjects.FilterSettings filterSettings = new RenderObjects.FilterSettings();

			// Token: 0x04000955 RID: 2389
			public Material overrideMaterial;

			// Token: 0x04000956 RID: 2390
			public int overrideMaterialPassIndex;

			// Token: 0x04000957 RID: 2391
			public Shader overrideShader;

			// Token: 0x04000958 RID: 2392
			public int overrideShaderPassIndex;

			// Token: 0x04000959 RID: 2393
			public RenderObjects.RenderObjectsSettings.OverrideMaterialMode overrideMode = RenderObjects.RenderObjectsSettings.OverrideMaterialMode.Material;

			// Token: 0x0400095A RID: 2394
			public bool overrideDepthState;

			// Token: 0x0400095B RID: 2395
			public CompareFunction depthCompareFunction = CompareFunction.LessEqual;

			// Token: 0x0400095C RID: 2396
			public bool enableWrite = true;

			// Token: 0x0400095D RID: 2397
			public StencilStateData stencilSettings = new StencilStateData();

			// Token: 0x0400095E RID: 2398
			public RenderObjects.CustomCameraSettings cameraSettings = new RenderObjects.CustomCameraSettings();

			// Token: 0x02000207 RID: 519
			public enum OverrideMaterialMode
			{
				// Token: 0x04000CF5 RID: 3317
				None,
				// Token: 0x04000CF6 RID: 3318
				Material,
				// Token: 0x04000CF7 RID: 3319
				Shader
			}
		}

		// Token: 0x0200014D RID: 333
		[Serializable]
		public class FilterSettings
		{
			// Token: 0x06000A4B RID: 2635 RVA: 0x000470AB File Offset: 0x000452AB
			public FilterSettings()
			{
				this.RenderQueueType = RenderQueueType.Opaque;
				this.LayerMask = 0;
			}

			// Token: 0x0400095F RID: 2399
			public RenderQueueType RenderQueueType;

			// Token: 0x04000960 RID: 2400
			public LayerMask LayerMask;

			// Token: 0x04000961 RID: 2401
			public string[] PassNames;
		}

		// Token: 0x0200014E RID: 334
		[Serializable]
		public class CustomCameraSettings
		{
			// Token: 0x04000962 RID: 2402
			public bool overrideCamera;

			// Token: 0x04000963 RID: 2403
			public bool restoreCamera = true;

			// Token: 0x04000964 RID: 2404
			public Vector4 offset;

			// Token: 0x04000965 RID: 2405
			public float cameraFieldOfView = 60f;
		}
	}
}
