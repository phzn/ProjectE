using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B59 RID: 2905
	[DisallowMultipleComponent]
	public sealed class BlindfoldVision : MonoBehaviour
	{
		// Token: 0x06005272 RID: 21106 RVA: 0x001FF3DA File Offset: 0x001FD5DA
		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += this.BeginCameraRendering;
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x001FF3ED File Offset: 0x001FD5ED
		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.BeginCameraRendering;
			this.ReleaseUiStencils();
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001FF406 File Offset: 0x001FD606
		private void OnDestroy()
		{
			RenderPipelineManager.beginCameraRendering -= this.BeginCameraRendering;
			this.ReleaseUiStencils();
			CoreUtils.Destroy(this._material);
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001FF42A File Offset: 0x001FD62A
		private void Update()
		{
			if (this._uiStencils.Count > 0 && !WardrobeComboRuntime.HasBlind)
			{
				this.ReleaseUiStencils();
			}
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001FF448 File Offset: 0x001FD648
		private void EnsureUiStencils()
		{
			if (this._lastUiScanFrame == Time.frameCount || Time.unscaledTime < this._nextUiScanTime)
			{
				return;
			}
			this._lastUiScanFrame = Time.frameCount;
			this._nextUiScanTime = Time.unscaledTime + 0.25f;
			for (int i = this._uiStencils.Count - 1; i >= 0; i--)
			{
				if (this._uiStencils[i] == null)
				{
					this._uiStencils.RemoveAt(i);
				}
			}
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>())
			{
				if (canvas.renderMode == RenderMode.WorldSpace)
				{
					IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
					for (int k = 0; k < graphicsForCanvas.Count; k++)
					{
						Graphic graphic = graphicsForCanvas[k];
						BlindfoldUiStencil blindfoldUiStencil;
						if (!(graphic == null) && !(graphic.canvas == null) && graphic.canvas.renderMode == RenderMode.WorldSpace && !graphic.TryGetComponent<BlindfoldUiStencil>(out blindfoldUiStencil))
						{
							this._uiStencils.Add(graphic.gameObject.AddComponent<BlindfoldUiStencil>());
						}
					}
				}
			}
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001FF560 File Offset: 0x001FD760
		private void ReleaseUiStencils()
		{
			for (int i = 0; i < this._uiStencils.Count; i++)
			{
				if (this._uiStencils[i] != null)
				{
					CoreUtils.Destroy(this._uiStencils[i]);
				}
			}
			this._uiStencils.Clear();
			this._lastUiScanFrame = -1;
			this._nextUiScanTime = 0f;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001FF5C8 File Offset: 0x001FD7C8
		private void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			Vector3 vector;
			if (camera == null || camera.cameraType != CameraType.Game || camera.targetTexture != null || !camera.CompareTag("MainCamera") || !WardrobeComboRuntime.TryGetBlindVisionCenter(out vector))
			{
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData;
			if (!camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData) || universalAdditionalCameraData.renderType != CameraRenderType.Base || universalAdditionalCameraData.scriptableRenderer == null)
			{
				return;
			}
			if (this._material == null)
			{
				Shader shader = Resources.Load<Shader>("BlindfoldVision");
				if (shader == null || !shader.isSupported)
				{
					return;
				}
				this._material = CoreUtils.CreateEngineMaterial(shader);
				this._pass = new BlindfoldVision.BlindVisionPass(this._material);
			}
			this.EnsureUiStencils();
			this._pass.SetCenter(vector);
			universalAdditionalCameraData.scriptableRenderer.EnqueuePass(this._pass);
		}

		// Token: 0x04005309 RID: 21257
		private Material _material;

		// Token: 0x0400530A RID: 21258
		private BlindfoldVision.BlindVisionPass _pass;

		// Token: 0x0400530B RID: 21259
		private readonly List<BlindfoldUiStencil> _uiStencils = new List<BlindfoldUiStencil>();

		// Token: 0x0400530C RID: 21260
		private int _lastUiScanFrame = -1;

		// Token: 0x0400530D RID: 21261
		private float _nextUiScanTime;

		// Token: 0x02000B5A RID: 2906
		private sealed class BlindVisionPass : ScriptableRenderPass
		{
			// Token: 0x0600527A RID: 21114 RVA: 0x001FF6AC File Offset: 0x001FD8AC
			public BlindVisionPass(Material material)
			{
				this._material = material;
				base.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
				base.ConfigureInput(ScriptableRenderPassInput.Depth);
			}

			// Token: 0x0600527B RID: 21115 RVA: 0x001FF6CD File Offset: 0x001FD8CD
			public void SetCenter(Vector3 center)
			{
				this._center = center;
			}

			// Token: 0x0600527C RID: 21116 RVA: 0x001FF6D6 File Offset: 0x001FD8D6
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				base.ConfigureTarget(renderingData.cameraData.renderer.cameraColorTargetHandle, renderingData.cameraData.renderer.cameraDepthTargetHandle);
			}

			// Token: 0x0600527D RID: 21117 RVA: 0x001FF700 File Offset: 0x001FD900
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				this._material.SetVector(BlindfoldVision.BlindVisionPass.CenterId, new Vector4(this._center.x, this._center.y, this._center.z, 1.5f));
				this._material.SetFloat(BlindfoldVision.BlindVisionPass.OuterRadiusId, 3f);
				CommandBuffer commandBuffer = CommandBufferPool.Get("Blindfold nearby vision");
				CoreUtils.DrawFullScreen(commandBuffer, this._material, null, 0);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x0400530E RID: 21262
			private static readonly int CenterId = Shader.PropertyToID("_BlindCenterAndClearRadius");

			// Token: 0x0400530F RID: 21263
			private static readonly int OuterRadiusId = Shader.PropertyToID("_BlindOuterRadius");

			// Token: 0x04005310 RID: 21264
			private readonly Material _material;

			// Token: 0x04005311 RID: 21265
			private Vector3 _center;
		}
	}
}
