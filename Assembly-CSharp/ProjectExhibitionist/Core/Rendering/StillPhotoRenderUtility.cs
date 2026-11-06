using System;
using Features.CensorBars;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000AC0 RID: 2752
	public static class StillPhotoRenderUtility
	{
		// Token: 0x06004E27 RID: 20007 RVA: 0x001E4A90 File Offset: 0x001E2C90
		public static void Render(Camera camera, RenderTexture destination)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture targetTexture = camera.targetTexture;
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			bool renderPostProcessing = universalAdditionalCameraData.renderPostProcessing;
			AntialiasingMode antialiasing = universalAdditionalCameraData.antialiasing;
			bool allowDynamicResolution = camera.allowDynamicResolution;
			UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
			float num = ((asset != null) ? asset.renderScale : 1f);
			UpscalingFilterSelection upscalingFilterSelection = ((asset != null) ? asset.upscalingFilter : UpscalingFilterSelection.Linear);
			using (StreamerCensorRenderFeature.BeginCapture())
			{
				try
				{
					if (asset != null)
					{
						asset.renderScale = 1f;
						asset.upscalingFilter = UpscalingFilterSelection.Linear;
					}
					if (allowDynamicResolution)
					{
						camera.allowDynamicResolution = false;
					}
					universalAdditionalCameraData.renderPostProcessing = false;
					universalAdditionalCameraData.antialiasing = AntialiasingMode.None;
					RenderTexture.active = destination;
					Color backgroundColor = camera.backgroundColor;
					backgroundColor.a = 1f;
					GL.Clear(true, true, backgroundColor);
					RenderTexture.active = active;
					camera.targetTexture = destination;
					UniversalRenderPipeline.SingleCameraRequest singleCameraRequest = new UniversalRenderPipeline.SingleCameraRequest();
					if (RenderPipeline.SupportsRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, singleCameraRequest))
					{
						singleCameraRequest.destination = destination;
						RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, singleCameraRequest);
					}
					else
					{
						camera.Render();
					}
				}
				finally
				{
					if (asset != null)
					{
						asset.renderScale = num;
						asset.upscalingFilter = upscalingFilterSelection;
					}
					if (camera != null)
					{
						camera.targetTexture = targetTexture;
						if (allowDynamicResolution)
						{
							camera.allowDynamicResolution = true;
						}
					}
					RenderTexture.active = active;
					if (universalAdditionalCameraData != null)
					{
						universalAdditionalCameraData.renderPostProcessing = renderPostProcessing;
						universalAdditionalCameraData.antialiasing = antialiasing;
					}
				}
			}
		}
	}
}
