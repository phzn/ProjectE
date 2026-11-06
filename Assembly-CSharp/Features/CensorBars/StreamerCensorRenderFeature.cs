using System;
using System.Collections.Generic;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.CensorBars
{
	// Token: 0x020014C0 RID: 5312
	public sealed class StreamerCensorRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x0600A35B RID: 41819 RVA: 0x0043DF1B File Offset: 0x0043C11B
		internal static bool IsCapturing
		{
			get
			{
				return StreamerCensorRenderFeature._captureDepth > 0;
			}
		}

		// Token: 0x0600A35C RID: 41820 RVA: 0x0043DF25 File Offset: 0x0043C125
		public static IDisposable BeginCapture()
		{
			return new StreamerCensorRenderFeature.CaptureScope();
		}

		// Token: 0x0600A35D RID: 41821 RVA: 0x0043DF2C File Offset: 0x0043C12C
		public override void Create()
		{
			StreamerCensorRenderFeature.CensorPass pass = this._pass;
			if (pass != null)
			{
				pass.Dispose();
			}
			this._pass = new StreamerCensorRenderFeature.CensorPass();
		}

		// Token: 0x0600A35E RID: 41822 RVA: 0x0043DF4A File Offset: 0x0043C14A
		protected override void Dispose(bool disposing)
		{
			StreamerCensorRenderFeature.CensorPass pass = this._pass;
			if (pass == null)
			{
				return;
			}
			pass.Dispose();
		}

		// Token: 0x0600A35F RID: 41823 RVA: 0x0043DF5C File Offset: 0x0043C15C
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!GameSettingsService.StreamerMode || renderingData.cameraData.isSceneViewCamera || renderingData.cameraData.isPreviewCamera)
			{
				return;
			}
			if (this._pass != null && this._pass.Collect(renderingData.cameraData.camera))
			{
				renderer.EnqueuePass(this._pass);
			}
		}

		// Token: 0x0400A71B RID: 42779
		private StreamerCensorRenderFeature.CensorPass _pass;

		// Token: 0x0400A71C RID: 42780
		private static int _captureDepth;

		// Token: 0x020014C1 RID: 5313
		private sealed class CaptureScope : IDisposable
		{
			// Token: 0x0600A361 RID: 41825 RVA: 0x0043DFB7 File Offset: 0x0043C1B7
			public CaptureScope()
			{
				StreamerCensorRenderFeature._captureDepth++;
			}

			// Token: 0x0600A362 RID: 41826 RVA: 0x0043DFCB File Offset: 0x0043C1CB
			public void Dispose()
			{
				if (!this._disposed)
				{
					StreamerCensorRenderFeature._captureDepth--;
					this._disposed = true;
				}
			}

			// Token: 0x0400A71D RID: 42781
			private bool _disposed;
		}

		// Token: 0x020014C2 RID: 5314
		private sealed class CensorPass : ScriptableRenderPass
		{
			// Token: 0x0600A363 RID: 41827 RVA: 0x0043DFE8 File Offset: 0x0043C1E8
			public CensorPass()
			{
				base.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
				base.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
				Shader shader = Resources.Load<Shader>("StreamerWorldPixelation");
				if (shader != null)
				{
					this._material = CoreUtils.CreateEngineMaterial(shader);
				}
			}

			// Token: 0x0600A364 RID: 41828 RVA: 0x0043E054 File Offset: 0x0043C254
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData data)
			{
				RenderTextureDescriptor cameraTargetDescriptor = data.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this._source, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_StreamerCensorSource");
				base.ConfigureTarget(data.cameraData.renderer.cameraColorTargetHandle);
			}

			// Token: 0x0600A365 RID: 41829 RVA: 0x0043E0B0 File Offset: 0x0043C2B0
			public override void Execute(ScriptableRenderContext context, ref RenderingData data)
			{
				if (this._material == null)
				{
					return;
				}
				if (this._regions.Count == 0 && this._meshes.Count == 0)
				{
					return;
				}
				RenderTextureDescriptor cameraTargetDescriptor = data.cameraData.cameraTargetDescriptor;
				float num = (float)cameraTargetDescriptor.width;
				float num2 = (float)cameraTargetDescriptor.height;
				RTHandle cameraColorTargetHandle = data.cameraData.renderer.cameraColorTargetHandle;
				CommandBuffer commandBuffer = CommandBufferPool.Get("Streamer pixelation");
				Blitter.BlitCameraTexture(commandBuffer, cameraColorTargetHandle, this._source, 0f, false);
				CoreUtils.SetRenderTarget(commandBuffer, cameraColorTargetHandle, ClearFlag.None, 0, CubemapFace.Unknown, -1);
				commandBuffer.SetGlobalTexture("_StreamerCensorSource", this._source);
				float num3 = Mathf.Max(2f, (float)GameSettingsService.StreamerPixelSize * num2 / 1080f);
				this._properties.SetVector("_CensorGrid", new Vector4(num, num2, num3, 0f));
				foreach (StreamerCensorMesh streamerCensorMesh in this._meshes)
				{
					this._properties.SetMatrix("_CensorWorldToLocal", streamerCensorMesh.Matrix.inverse);
					commandBuffer.DrawMesh(streamerCensorMesh.Mesh, streamerCensorMesh.Matrix, this._material, 0, 1, this._properties);
				}
				foreach (Rect rect in this._regions)
				{
					float num4 = StreamerCensorProjection.BlockPixels(rect, (int)num, (int)num2, (float)GameSettingsService.StreamerPixelSize);
					Vector2 vector = new Vector2(Mathf.Floor(rect.xMin * num / num4) * num4 / num, Mathf.Floor(rect.yMin * num2 / num4) * num4 / num2);
					Vector2 vector2 = new Vector2(Mathf.Ceil(rect.xMax * num / num4) * num4 / num, Mathf.Ceil(rect.yMax * num2 / num4) * num4 / num2);
					this._properties.SetVector("_CensorRect", new Vector4(vector.x, vector.y, vector2.x, vector2.y));
					this._properties.SetVector("_CensorGrid", new Vector4(num, num2, num4, 0f));
					commandBuffer.DrawProcedural(Matrix4x4.identity, this._material, 0, MeshTopology.Triangles, 6, 1, this._properties);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x0600A366 RID: 41830 RVA: 0x0043E364 File Offset: 0x0043C564
			public bool Collect(Camera camera)
			{
				this._regions.Clear();
				this._meshes.Clear();
				foreach (StreamerCensorCharacter streamerCensorCharacter in StreamerCensorCharacter.Active)
				{
					if (streamerCensorCharacter != null && streamerCensorCharacter.isActiveAndEnabled)
					{
						streamerCensorCharacter.Collect(camera, this._meshes);
					}
				}
				foreach (StreamerCensorTarget streamerCensorTarget in StreamerCensorTarget.Active)
				{
					if (streamerCensorTarget != null && streamerCensorTarget.isActiveAndEnabled)
					{
						streamerCensorTarget.Collect(camera, this._regions);
					}
				}
				return this._regions.Count > 0 || this._meshes.Count > 0;
			}

			// Token: 0x0600A367 RID: 41831 RVA: 0x0043E45C File Offset: 0x0043C65C
			public void Dispose()
			{
				RTHandle source = this._source;
				if (source != null)
				{
					source.Release();
				}
				this._source = null;
				CoreUtils.Destroy(this._material);
			}

			// Token: 0x0400A71E RID: 42782
			private readonly List<Rect> _regions = new List<Rect>(64);

			// Token: 0x0400A71F RID: 42783
			private readonly List<StreamerCensorMesh> _meshes = new List<StreamerCensorMesh>(8);

			// Token: 0x0400A720 RID: 42784
			private readonly Material _material;

			// Token: 0x0400A721 RID: 42785
			private RTHandle _source;

			// Token: 0x0400A722 RID: 42786
			private readonly MaterialPropertyBlock _properties = new MaterialPropertyBlock();
		}
	}
}
