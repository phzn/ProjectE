using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x02000035 RID: 53
	public class GPUIShaderBuffer : IGPUIDisposable, IDisposable
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000A0AC File Offset: 0x000082AC
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000A0B4 File Offset: 0x000082B4
		public GraphicsBuffer Buffer { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A0BD File Offset: 0x000082BD
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000A0C5 File Offset: 0x000082C5
		public RenderTexture Texture { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A0CE File Offset: 0x000082CE
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000A0D6 File Offset: 0x000082D6
		public int BufferSize { get; private set; }

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public GPUIShaderBuffer(int bufferSize, int stride)
		{
			if ((long)bufferSize > GPUIConstants.MAX_BUFFER_SIZE)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					bufferSize.ToString("#,0"),
					" exceeds maximum allowed buffer size (",
					GPUIConstants.MAX_BUFFER_SIZE.ToString("#,0"),
					")."
				}));
				return;
			}
			if (bufferSize > 0)
			{
				this.Buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, bufferSize, stride);
			}
			this.BufferSize = bufferSize;
			if (GPUIRuntimeSettings.Instance.DisableShaderBuffers)
			{
				int num = Mathf.CeilToInt((float)bufferSize / (float)GPUIConstants.TEXTURE_MAX_SIZE);
				this.Texture = new RenderTexture((num == 1) ? bufferSize : GPUIConstants.TEXTURE_MAX_SIZE, num * Mathf.CeilToInt((float)stride / 16f), 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear)
				{
					isPowerOfTwo = false,
					enableRandomWrite = true,
					filterMode = FilterMode.Point,
					useMipMap = false,
					autoGenerateMips = false,
					useDynamicScale = false,
					wrapMode = TextureWrapMode.Clamp
				};
				this.Texture.Create();
				this._isBufferToTextureFloat4 = stride <= 16;
			}
			this._requestCallbackInternal = new Action<AsyncGPUReadbackRequest>(this.AsyncDataRequestCallback);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A208 File Offset: 0x00008408
		public void ReleaseBuffers()
		{
			this.BufferSize = 0;
			if (this._isDataRequested)
			{
				this._isRequestedDataInvalid = true;
				this._readbackRequest.WaitForCompletion();
			}
			if (this.Buffer != null)
			{
				this.Buffer.Dispose();
			}
			this.Buffer = null;
			this.Texture.DestroyRenderTexture();
			this.Texture = null;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A262 File Offset: 0x00008462
		public void Dispose()
		{
			this.ReleaseBuffers();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A26C File Offset: 0x0000846C
		public void OnDataModified()
		{
			if (this.Buffer == null)
			{
				return;
			}
			if (GPUIRuntimeSettings.Instance.DisableShaderBuffers)
			{
				ComputeShader cs_BufferToTexture = GPUIConstants.CS_BufferToTexture;
				if (this._isBufferToTextureFloat4)
				{
					cs_BufferToTexture.EnableKeyword("GPUI_FLOAT4_BUFFER");
				}
				else
				{
					cs_BufferToTexture.DisableKeyword("GPUI_FLOAT4_BUFFER");
				}
				cs_BufferToTexture.SetBuffer(0, GPUIConstants.PROP_sourceBuffer, this.Buffer);
				cs_BufferToTexture.SetTexture(0, GPUIConstants.PROP_targetTexture, this.Texture);
				cs_BufferToTexture.SetInt(GPUIConstants.PROP_count, this.BufferSize);
				cs_BufferToTexture.SetInt(GPUIConstants.PROP_maxTextureSize, GPUIConstants.TEXTURE_MAX_SIZE);
				cs_BufferToTexture.DispatchX(0, this.Buffer.count);
				this.Texture.IncrementUpdateCount();
			}
			if (this._isDataRequested)
			{
				this._isRequestedDataInvalid = true;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A328 File Offset: 0x00008528
		public void SetBuffer(ComputeShader cs, int kernelIndex, int nameID)
		{
			if (this.Buffer != null)
			{
				cs.SetBuffer(kernelIndex, nameID, this.Buffer);
				return;
			}
			if (this.Texture != null)
			{
				cs.SetTexture(kernelIndex, nameID, this.Texture);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A35D File Offset: 0x0000855D
		public void CompleteAsyncRequests()
		{
			if (this._isDataRequested)
			{
				this._readbackRequest.WaitForCompletion();
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A374 File Offset: 0x00008574
		public void AsyncRequestIntoNativeArray(Action<NativeArray<Matrix4x4>> onSuccessfulReadback)
		{
			if (this.Buffer == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Buffer is not created.");
				return;
			}
			if (this._isDataRequested)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "There is already an active async readback request!");
				return;
			}
			this._requestedData = new NativeArray<Matrix4x4>(this.BufferSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._isDataRequested = true;
			this._requestCallbackExternal = onSuccessfulReadback;
			this._readbackRequest = AsyncGPUReadback.RequestIntoNativeArray<Matrix4x4>(ref this._requestedData, this.Buffer, this._requestCallbackInternal);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A3FC File Offset: 0x000085FC
		private void AsyncDataRequestCallback(AsyncGPUReadbackRequest readbackRequest)
		{
			this._isDataRequested = false;
			if (this._isRequestedDataInvalid)
			{
				this._isRequestedDataInvalid = false;
				if (this._requestedData.IsCreated)
				{
					if (this.BufferSize == 0 || this._requestedData.Length != this.BufferSize || this.Buffer == null)
					{
						this._requestedData.Dispose();
						return;
					}
					this.AsyncRequestIntoNativeArray(this._requestCallbackExternal);
				}
				return;
			}
			if (readbackRequest.hasError)
			{
				if (this._requestedData.IsCreated)
				{
					this._requestedData.Dispose();
				}
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Async data request has encountered an error.");
				return;
			}
			this._requestCallbackExternal(this._requestedData);
		}

		// Token: 0x0400014C RID: 332
		private bool _isBufferToTextureFloat4;

		// Token: 0x0400014D RID: 333
		private bool _isDataRequested;

		// Token: 0x0400014E RID: 334
		private bool _isRequestedDataInvalid;

		// Token: 0x0400014F RID: 335
		private AsyncGPUReadbackRequest _readbackRequest;

		// Token: 0x04000150 RID: 336
		private readonly Action<AsyncGPUReadbackRequest> _requestCallbackInternal;

		// Token: 0x04000151 RID: 337
		private Action<NativeArray<Matrix4x4>> _requestCallbackExternal;

		// Token: 0x04000152 RID: 338
		private NativeArray<Matrix4x4> _requestedData;
	}
}
