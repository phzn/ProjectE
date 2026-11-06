using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

namespace GPUInstancerPro
{
	// Token: 0x02000021 RID: 33
	public class GPUIOcclusionCullingData : IDisposable
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000069AD File Offset: 0x00004BAD
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000069B5 File Offset: 0x00004BB5
		public RenderTexture HiZDepthTexture { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000069BE File Offset: 0x00004BBE
		public Vector2Int HiZTextureSize
		{
			get
			{
				return this._occlusionPassData.hiZTextureSize;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000069CB File Offset: 0x00004BCB
		public int HiZMipLevels
		{
			get
			{
				return this._occlusionPassData.hiZMipLevels;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000069D8 File Offset: 0x00004BD8
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000069E0 File Offset: 0x00004BE0
		public Texture CameraDepthTexture { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000069E9 File Offset: 0x00004BE9
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000069F1 File Offset: 0x00004BF1
		public GPUIOcclusionCullingData.GPUIOcclusionCullingMode ActiveCullingMode { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000069FA File Offset: 0x00004BFA
		public bool IsHiZDepthUpdated
		{
			get
			{
				return this.HiZDepthTexture != null && this._isHiZDepthUpdated;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006A12 File Offset: 0x00004C12
		private bool IsDirectCameraDepthAccessRequired
		{
			get
			{
				return this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.DirectTextureAccess || this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferExecutedOnEndRendering;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006A28 File Offset: 0x00004C28
		private bool IsCommandBufferRequired
		{
			get
			{
				return this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferAddedToCamera || this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferExecutedOnEndRendering;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006A40 File Offset: 0x00004C40
		public GPUIOcclusionCullingData(Camera camera, GPUIOcclusionCullingData.GPUIOcclusionCullingMode cullingMode, bool isVRCulling)
		{
			this._activeCamera = camera;
			this._occlusionPassData = new GPUIOcclusionCullingData.GPUIOcclusionPassData
			{
				copyCS = GPUIConstants.CS_HiZTextureCopy,
				reduceCS = GPUIConstants.CS_TextureReduce,
				isVRCulling = isVRCulling,
				isDepth2DArray = GPUIRuntimeSettings.Instance.IsHDRP
			};
			this.Initialize(cullingMode);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006A99 File Offset: 0x00004C99
		public void Initialize(GPUIOcclusionCullingData.GPUIOcclusionCullingMode cullingMode)
		{
			this.Dispose();
			this._activeCamera.depthTextureMode |= DepthTextureMode.Depth;
			this.DetermineOcclusionCullingMode(cullingMode);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006ABB File Offset: 0x00004CBB
		public void Dispose()
		{
			this.DisposeOcclusionCommandBuffer();
			this.DisposeScriptableRenderPass();
			this.DisposeCustomPass();
			this.DisposeHiZDepthTexture();
			this.CameraDepthTexture = null;
			this._isHiZDepthUpdated = false;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006AE3 File Offset: 0x00004CE3
		private void OnHiZTextureSizeChanged()
		{
			this.DisposeOcclusionCommandBuffer();
			this.DisposeScriptableRenderPass();
			this.DisposeHiZDepthTexture();
			this._isHiZDepthUpdated = false;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006AFE File Offset: 0x00004CFE
		private void OnScreenSizeChanged()
		{
			this.DisposeOcclusionCommandBuffer();
			this._isHiZDepthUpdated = false;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006B10 File Offset: 0x00004D10
		private void DetermineOcclusionCullingMode(GPUIOcclusionCullingData.GPUIOcclusionCullingMode cullingMode)
		{
			if (cullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.Auto)
			{
				if (this._occlusionPassData.isVRCulling)
				{
					this.ActiveCullingMode = GPUIOcclusionCullingData.GPUIOcclusionCullingMode.DirectTextureAccess;
					return;
				}
				if (!GPUIRuntimeSettings.Instance.IsBuiltInRP)
				{
					this.ActiveCullingMode = GPUIOcclusionCullingData.GPUIOcclusionCullingMode.DirectTextureAccess;
					return;
				}
				if (this._activeCamera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					this.ActiveCullingMode = GPUIOcclusionCullingData.GPUIOcclusionCullingMode.DirectTextureAccess;
					return;
				}
				this.ActiveCullingMode = GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferAddedToCamera;
				return;
			}
			else if (cullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.URPScriptableRenderPass)
			{
				if (!GPUIRuntimeSettings.Instance.IsURP)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "OcclusionCullingMode.URPScriptableRenderPass is only supported in Universal Render Pipeline! Switching to OcclusionCullingMode.Auto.");
					this.DetermineOcclusionCullingMode(GPUIOcclusionCullingData.GPUIOcclusionCullingMode.Auto);
					return;
				}
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "OcclusionCullingMode.URPScriptableRenderPass is only supported for Unity versions 6000 or higher! Switching to OcclusionCullingMode.Auto.");
				this.DetermineOcclusionCullingMode(GPUIOcclusionCullingData.GPUIOcclusionCullingMode.Auto);
				return;
			}
			else
			{
				if (cullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.HDRPCustomPass && !GPUIRuntimeSettings.Instance.IsHDRP)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "OcclusionCullingMode.HDRPCustomPass is only supported in HDRP! Switching to OcclusionCullingMode.Auto.");
					this.DetermineOcclusionCullingMode(GPUIOcclusionCullingData.GPUIOcclusionCullingMode.Auto);
					return;
				}
				this.ActiveCullingMode = cullingMode;
				return;
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006BE4 File Offset: 0x00004DE4
		private bool CreateHiZDepthTexture(Vector2Int screenSize)
		{
			this.OnHiZTextureSizeChanged();
			this._occlusionPassData.hiZTextureSize = screenSize;
			this._occlusionPassData.hiZMipLevels = this.GetMipLevelCount();
			if (this.HiZTextureSize.x <= 0 || this.HiZTextureSize.y <= 0 || this._occlusionPassData.hiZMipLevels == 0)
			{
				return false;
			}
			this.HiZDepthTexture = new RenderTexture(this.HiZTextureSize.x, this.HiZTextureSize.y, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear)
			{
				name = "GPUI_HiZDepthTexture",
				filterMode = FilterMode.Point,
				useMipMap = true,
				autoGenerateMips = false,
				enableRandomWrite = true,
				hideFlags = HideFlags.HideAndDontSave
			};
			this.HiZDepthTexture.Create();
			this.HiZDepthTexture.GenerateMips();
			this._hiZDepthIdentifier = new RenderTargetIdentifier(this.HiZDepthTexture);
			return true;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006CC8 File Offset: 0x00004EC8
		private void DisposeHiZDepthTexture()
		{
			if (this.HiZDepthTexture != null)
			{
				this.HiZDepthTexture.DestroyRenderTexture();
				this.HiZDepthTexture = null;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000024BC File Offset: 0x000006BC
		private void DisposeScriptableRenderPass()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000024BC File Offset: 0x000006BC
		private void DisposeCustomPass()
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006CEC File Offset: 0x00004EEC
		private Vector2Int GetScreenSize()
		{
			Vector2Int zero = Vector2Int.zero;
			if (this._occlusionPassData.isVRCulling)
			{
				zero.x = XRSettings.eyeTextureWidth;
				zero.y = XRSettings.eyeTextureHeight;
				zero.x *= 2;
			}
			else
			{
				zero.x = this._activeCamera.pixelWidth;
				zero.y = this._activeCamera.pixelHeight;
			}
			UniversalRenderPipelineAsset universalRenderPipelineAsset;
			if (!this._occlusionPassData.isVRCulling && GPUIRuntimeSettings.Instance.IsURP && GPUIRuntimeSettings.TryGetURPAsset(out universalRenderPipelineAsset) && universalRenderPipelineAsset.renderScale != 1f)
			{
				zero.x = Mathf.FloorToInt((float)zero.x * universalRenderPipelineAsset.renderScale);
				zero.y = Mathf.FloorToInt((float)zero.y * universalRenderPipelineAsset.renderScale);
			}
			return zero;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006DC0 File Offset: 0x00004FC0
		private int GetMipLevelCount()
		{
			return 1 + Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(this.HiZTextureSize.x, this.HiZTextureSize.y), 2f));
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006E00 File Offset: 0x00005000
		internal void CheckScreenSize()
		{
			if (this.HiZDepthTexture == null)
			{
				return;
			}
			Vector2Int screenSize = this.GetScreenSize();
			if (screenSize.x != this.HiZTextureSize.x || screenSize.y != this.HiZTextureSize.y)
			{
				if (screenSize.x <= this.HiZDepthTexture.width && screenSize.y <= this.HiZDepthTexture.height)
				{
					this._occlusionPassData.hiZTextureSize = screenSize;
					this._occlusionPassData.hiZMipLevels = this.GetMipLevelCount();
					this.OnScreenSizeChanged();
					this.HiZDepthTexture.ClearRenderTexture(Color.white);
					this.HiZDepthTexture.GenerateMips();
				}
				else
				{
					this.OnHiZTextureSizeChanged();
				}
				this.CameraDepthTexture = null;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006EC8 File Offset: 0x000050C8
		internal void UpdateHiZTexture(ScriptableRenderContext context)
		{
			this._isHiZDepthUpdated = false;
			if (this.IsDirectCameraDepthAccessRequired && this.CameraDepthTexture == null)
			{
				this.DisposeOcclusionCommandBuffer();
				this.CameraDepthTexture = Shader.GetGlobalTexture(GPUIConstants.PROP_CameraDepthTexture);
				if (this.CameraDepthTexture == null || this.CameraDepthTexture.name == "UnityBlack")
				{
					this.CameraDepthTexture = null;
					return;
				}
				this._occlusionPassData.isDepth2DArray = this.CameraDepthTexture.dimension == TextureDimension.Tex2DArray;
			}
			if (this.HiZDepthTexture == null && !this.CreateHiZDepthTexture(this.GetScreenSize()))
			{
				return;
			}
			if (this.IsCommandBufferRequired && this._occlusionCommandBuffer == null)
			{
				this.CreateOcclusionCommandBuffer();
				if (this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferAddedToCamera)
				{
					this._activeCamera.AddCommandBuffer(CameraEvent.AfterDepthTexture, this._occlusionCommandBuffer);
					return;
				}
			}
			switch (this.ActiveCullingMode)
			{
			case GPUIOcclusionCullingData.GPUIOcclusionCullingMode.DirectTextureAccess:
				this.DirectTextureAccessUpdate();
				this._isHiZDepthUpdated = true;
				return;
			case GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferAddedToCamera:
				this._isHiZDepthUpdated = true;
				return;
			case GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferExecutedOnEndRendering:
				if (GPUIRuntimeSettings.Instance.IsBuiltInRP)
				{
					Graphics.ExecuteCommandBuffer(this._occlusionCommandBuffer);
				}
				else
				{
					context.ExecuteCommandBuffer(this._occlusionCommandBuffer);
				}
				this._isHiZDepthUpdated = true;
				return;
			case GPUIOcclusionCullingData.GPUIOcclusionCullingMode.URPScriptableRenderPass:
			case GPUIOcclusionCullingData.GPUIOcclusionCullingMode.HDRPCustomPass:
				this._isHiZDepthUpdated = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000024BC File Offset: 0x000006BC
		internal void UpdateHiZTextureOnBeginRendering(Camera camera, ScriptableRenderContext context)
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000700C File Offset: 0x0000520C
		private void DirectTextureAccessUpdate()
		{
			if (this._occlusionPassData.isVRCulling)
			{
				int num = this.HiZTextureSize.x / 2;
				if (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass)
				{
					if (this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left)
					{
						this.UpdateTextureWithComputeShader(num, 0, 0);
					}
					else if (this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
					{
						this.UpdateTextureWithComputeShader(num, num, 0);
					}
					else if (this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono)
					{
						if (!this._vrMultiPassMono)
						{
							this.UpdateTextureWithComputeShader(num, 0, 0);
							this._vrMultiPassMono = true;
						}
						else
						{
							this.UpdateTextureWithComputeShader(num, num, 0);
							this._vrMultiPassMono = false;
						}
					}
				}
				else if (this._occlusionPassData.isDepth2DArray && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
				{
					this.UpdateTextureWithComputeShader(num, 0, 0);
					this.UpdateTextureWithComputeShader(num, num, 1);
				}
				else
				{
					this.UpdateTextureWithComputeShader(this.HiZTextureSize.x, 0, 0);
				}
			}
			else if (this._occlusionPassData.isDepth2DArray && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced && this._activeCamera.stereoTargetEye == StereoTargetEyeMask.Right)
			{
				this.UpdateTextureWithComputeShader(0, 0, 1);
			}
			else
			{
				this.UpdateTextureWithComputeShader(this.HiZTextureSize.x, 0, 0);
			}
			for (int i = 0; i < this._occlusionPassData.hiZMipLevels - 1; i++)
			{
				GPUIHiZDepthTextureUtility.ReduceTextureWithComputeShader(this._occlusionPassData.reduceCS, this.HiZDepthTexture, this.HiZTextureSize.x, this.HiZTextureSize.y, i, i + 1);
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007190 File Offset: 0x00005390
		private void UpdateTextureWithComputeShader(int sourceWidth, int offset, int textureArrayIndex = 0)
		{
			if (this._occlusionPassData.isDepth2DArray)
			{
				GPUIHiZDepthTextureUtility.CopyHiZTextureArrayWithComputeShader(this._occlusionPassData.copyCS, this.CameraDepthTexture, this.HiZDepthTexture, offset, textureArrayIndex);
				return;
			}
			GPUIHiZDepthTextureUtility.CopyHiZTextureWithComputeShader(this._occlusionPassData.copyCS, this.CameraDepthTexture, this.HiZDepthTexture, offset);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000071E6 File Offset: 0x000053E6
		private void DisposeOcclusionCommandBuffer()
		{
			if (this._occlusionCommandBuffer != null)
			{
				if (this.ActiveCullingMode == GPUIOcclusionCullingData.GPUIOcclusionCullingMode.CommandBufferAddedToCamera)
				{
					this._activeCamera.RemoveCommandBuffer(CameraEvent.AfterDepthTexture, this._occlusionCommandBuffer);
				}
				this._occlusionCommandBuffer.Dispose();
				this._occlusionCommandBuffer = null;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007220 File Offset: 0x00005420
		private void CreateOcclusionCommandBuffer()
		{
			this.DisposeOcclusionCommandBuffer();
			RenderTargetIdentifier renderTargetIdentifier;
			if (GPUIRuntimeSettings.Instance.IsBuiltInRP)
			{
				renderTargetIdentifier = new RenderTargetIdentifier(BuiltinRenderTextureType.Depth);
			}
			else
			{
				renderTargetIdentifier = new RenderTargetIdentifier(this.CameraDepthTexture);
			}
			this._occlusionCommandBuffer = new CommandBuffer();
			this._occlusionCommandBuffer.name = "GPUI.HiZDepthPass";
			this.CreateOcclusionCommandBuffer(this._occlusionCommandBuffer, renderTargetIdentifier);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007280 File Offset: 0x00005480
		private void CreateOcclusionCommandBuffer(CommandBuffer commandBuffer, RenderTargetIdentifier unityDepthIdentifier)
		{
			if (this.HiZDepthTexture == null)
			{
				return;
			}
			if (this._occlusionPassData.isVRCulling)
			{
				int num = this.HiZTextureSize.x / 2;
				if (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass)
				{
					if (this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left || this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono)
					{
						this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, 0, 0);
					}
					else
					{
						this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, num, 0);
					}
					if (this._activeCamera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono)
					{
						this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, num, 0);
					}
				}
				else if (this._occlusionPassData.isDepth2DArray && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.SinglePassInstanced)
				{
					this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, 0, 0);
					this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, num, 1);
				}
				else
				{
					this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, num, 0, 0);
				}
			}
			else
			{
				this.UpdateTextureWithComputeShaderCB(commandBuffer, unityDepthIdentifier, this._hiZDepthIdentifier, this.HiZTextureSize.x, 0, 0);
			}
			for (int i = 0; i < this._occlusionPassData.hiZMipLevels - 1; i++)
			{
				GPUIHiZDepthTextureUtility.ReduceTextureWithComputeShader(this._occlusionPassData.reduceCS, commandBuffer, this._hiZDepthIdentifier, this.HiZTextureSize.x, this.HiZTextureSize.y, i, i + 1);
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000073E0 File Offset: 0x000055E0
		private void UpdateTextureWithComputeShaderCB(CommandBuffer commandBuffer, RenderTargetIdentifier unityDepthIdentifier, RenderTargetIdentifier hiZIdentifier, int sourceWidth, int offset, int textureArrayIndex = 0)
		{
			if (this._occlusionPassData.isDepth2DArray)
			{
				GPUIHiZDepthTextureUtility.CopyHiZTextureArrayWithComputeShader(this._occlusionPassData.copyCS, commandBuffer, unityDepthIdentifier, this.HiZTextureSize.x, this.HiZTextureSize.y, hiZIdentifier, offset, textureArrayIndex);
				return;
			}
			GPUIHiZDepthTextureUtility.CopyHiZTextureWithComputeShader(this._occlusionPassData.copyCS, commandBuffer, unityDepthIdentifier, this.HiZTextureSize.x, this.HiZTextureSize.y, hiZIdentifier, offset);
		}

		// Token: 0x04000095 RID: 149
		private Camera _activeCamera;

		// Token: 0x04000096 RID: 150
		private bool _vrMultiPassMono;

		// Token: 0x04000097 RID: 151
		private bool _isHiZDepthUpdated;

		// Token: 0x04000098 RID: 152
		private GPUIOcclusionCullingData.GPUIOcclusionPassData _occlusionPassData;

		// Token: 0x04000099 RID: 153
		private CommandBuffer _occlusionCommandBuffer;

		// Token: 0x0400009A RID: 154
		private RenderTargetIdentifier _hiZDepthIdentifier;

		// Token: 0x0400009B RID: 155
		private const string GPUI_HiZ_DepthTexture_NAME = "GPUI_HiZDepthTexture";

		// Token: 0x0400009C RID: 156
		private const string GPUI_HiZ_CommandBuffer_NAME = "GPUI.HiZDepthPass";

		// Token: 0x02000022 RID: 34
		public enum GPUIOcclusionCullingMode
		{
			// Token: 0x0400009E RID: 158
			Auto,
			// Token: 0x0400009F RID: 159
			DirectTextureAccess,
			// Token: 0x040000A0 RID: 160
			CommandBufferAddedToCamera,
			// Token: 0x040000A1 RID: 161
			CommandBufferExecutedOnEndRendering,
			// Token: 0x040000A2 RID: 162
			URPScriptableRenderPass,
			// Token: 0x040000A3 RID: 163
			HDRPCustomPass
		}

		// Token: 0x02000023 RID: 35
		private class GPUIOcclusionPassData
		{
			// Token: 0x06000130 RID: 304 RVA: 0x00007460 File Offset: 0x00005660
			public void CopyTo(GPUIOcclusionCullingData.GPUIOcclusionPassData other)
			{
				other.copyCS = this.copyCS;
				other.reduceCS = this.reduceCS;
				other.isVRCulling = this.isVRCulling;
				other.isDepth2DArray = this.isDepth2DArray;
				other.hiZMipLevels = this.hiZMipLevels;
				other.hiZTextureSize = this.hiZTextureSize;
			}

			// Token: 0x040000A4 RID: 164
			public ComputeShader copyCS;

			// Token: 0x040000A5 RID: 165
			public ComputeShader reduceCS;

			// Token: 0x040000A6 RID: 166
			public bool isVRCulling;

			// Token: 0x040000A7 RID: 167
			public bool isDepth2DArray;

			// Token: 0x040000A8 RID: 168
			public int hiZMipLevels;

			// Token: 0x040000A9 RID: 169
			public Vector2Int hiZTextureSize;
		}
	}
}
