using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A8B RID: 2699
	public static class CharacterPreviewRendering
	{
		// Token: 0x06004CD4 RID: 19668 RVA: 0x001DDA04 File Offset: 0x001DBC04
		public static bool IsTransparentPreview(Camera camera)
		{
			return camera != null && camera.targetTexture != null && (camera.targetTexture.name == "CharacterPreview" || camera.targetTexture.name.StartsWith("CharacterIcon", StringComparison.Ordinal));
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x001DDA5C File Offset: 0x001DBC5C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Register()
		{
			foreach (CharacterPreviewRendering.RenderScope renderScope in CharacterPreviewRendering.Active.Values)
			{
				renderScope.Dispose();
			}
			CharacterPreviewRendering.Active.Clear();
			RenderPipelineManager.beginCameraRendering -= CharacterPreviewRendering.BeginCamera;
			RenderPipelineManager.endCameraRendering -= CharacterPreviewRendering.EndCamera;
			RenderPipelineManager.beginCameraRendering += CharacterPreviewRendering.BeginCamera;
			RenderPipelineManager.endCameraRendering += CharacterPreviewRendering.EndCamera;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x001DDB04 File Offset: 0x001DBD04
		private static void BeginCamera(ScriptableRenderContext context, Camera camera)
		{
			if (CharacterPreviewRendering.IsTransparentPreview(camera) && !CharacterPreviewRendering.Active.ContainsKey(camera))
			{
				CharacterPreviewRendering.Active.Add(camera, new CharacterPreviewRendering.RenderScope(camera));
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x001DDB2C File Offset: 0x001DBD2C
		private static void EndCamera(ScriptableRenderContext context, Camera camera)
		{
			CharacterPreviewRendering.RenderScope renderScope;
			if (!CharacterPreviewRendering.Active.TryGetValue(camera, out renderScope))
			{
				return;
			}
			CharacterPreviewRendering.Active.Remove(camera);
			renderScope.Dispose();
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x001DDB5C File Offset: 0x001DBD5C
		public static void RenderPortrait(Camera camera, RenderTexture destination, bool captureFullTextureQuality = false)
		{
			RenderTexture targetTexture = camera.targetTexture;
			RenderTexture active = RenderTexture.active;
			try
			{
				camera.targetTexture = destination;
				using (new CharacterPreviewRendering.RenderScope(camera))
				{
					camera.Render();
				}
			}
			finally
			{
				camera.targetTexture = targetTexture;
				RenderTexture.active = active;
			}
		}

		// Token: 0x04004E57 RID: 20055
		private static readonly Dictionary<Camera, CharacterPreviewRendering.RenderScope> Active = new Dictionary<Camera, CharacterPreviewRendering.RenderScope>();

		// Token: 0x02000A8C RID: 2700
		public readonly struct RenderScope : IDisposable
		{
			// Token: 0x06004CDA RID: 19674 RVA: 0x001DDBD0 File Offset: 0x001DBDD0
			public RenderScope(Camera camera)
			{
				this._camera = camera;
				this._data = camera.GetUniversalAdditionalCameraData();
				this._pipeline = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
				this._scale = ((this._pipeline != null) ? this._pipeline.renderScale : 1f);
				this._upscale = ((this._pipeline != null) ? this._pipeline.upscalingFilter : UpscalingFilterSelection.Linear);
				this._msaa = ((this._pipeline != null) ? this._pipeline.msaaSampleCount : 1);
				this._hdr = this._pipeline != null && this._pipeline.supportsHDR;
				this._post = this._data.renderPostProcessing;
				this._aa = this._data.antialiasing;
				this._clear = camera.clearFlags;
				this._background = camera.backgroundColor;
				if (this._pipeline != null)
				{
					this._pipeline.renderScale = 1f;
					this._pipeline.upscalingFilter = UpscalingFilterSelection.Linear;
					this._pipeline.msaaSampleCount = 4;
					this._pipeline.supportsHDR = true;
				}
				this._data.renderPostProcessing = false;
				this._data.antialiasing = AntialiasingMode.None;
				camera.clearFlags = CameraClearFlags.Color;
				camera.backgroundColor = new Color(this._background.r, this._background.g, this._background.b, 0f);
			}

			// Token: 0x06004CDB RID: 19675 RVA: 0x001DDD58 File Offset: 0x001DBF58
			public void Dispose()
			{
				if (this._pipeline != null)
				{
					this._pipeline.renderScale = this._scale;
					this._pipeline.upscalingFilter = this._upscale;
					this._pipeline.msaaSampleCount = this._msaa;
					this._pipeline.supportsHDR = this._hdr;
				}
				if (this._data != null)
				{
					this._data.renderPostProcessing = this._post;
					this._data.antialiasing = this._aa;
				}
				if (this._camera != null)
				{
					this._camera.clearFlags = this._clear;
					this._camera.backgroundColor = this._background;
				}
			}

			// Token: 0x04004E58 RID: 20056
			private readonly Camera _camera;

			// Token: 0x04004E59 RID: 20057
			private readonly UniversalAdditionalCameraData _data;

			// Token: 0x04004E5A RID: 20058
			private readonly UniversalRenderPipelineAsset _pipeline;

			// Token: 0x04004E5B RID: 20059
			private readonly float _scale;

			// Token: 0x04004E5C RID: 20060
			private readonly UpscalingFilterSelection _upscale;

			// Token: 0x04004E5D RID: 20061
			private readonly int _msaa;

			// Token: 0x04004E5E RID: 20062
			private readonly bool _post;

			// Token: 0x04004E5F RID: 20063
			private readonly bool _hdr;

			// Token: 0x04004E60 RID: 20064
			private readonly AntialiasingMode _aa;

			// Token: 0x04004E61 RID: 20065
			private readonly CameraClearFlags _clear;

			// Token: 0x04004E62 RID: 20066
			private readonly Color _background;
		}
	}
}
