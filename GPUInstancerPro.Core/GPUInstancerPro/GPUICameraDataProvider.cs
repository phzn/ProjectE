using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000039 RID: 57
	public class GPUICameraDataProvider : GPUIDataProvider<int, GPUICameraData>
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000BD2E File Offset: 0x00009F2E
		public override void Initialize()
		{
			base.Initialize();
			this._removalQueue = new Queue<int>();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000BD44 File Offset: 0x00009F44
		public override void ReleaseBuffers()
		{
			if (this._dataDict != null)
			{
				foreach (GPUICameraData gpuicameraData in base.Values)
				{
					if (gpuicameraData != null)
					{
						gpuicameraData.ReleaseBuffers();
					}
				}
			}
			base.ReleaseBuffers();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		public override void Dispose()
		{
			this._removalQueue = null;
			base.Dispose();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000BDB7 File Offset: 0x00009FB7
		public override bool AddOrSet(int cameraInstanceID, GPUICameraData value)
		{
			if (base.AddOrSet(cameraInstanceID, value))
			{
				GPUIRenderingSystem.Instance.UpdateCommandBuffers(value, false);
				return true;
			}
			return false;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		public override bool Remove(int cameraInstanceID)
		{
			GPUICameraData gpuicameraData;
			if (base.IsInitialized && this._dataDict.TryGetValue(cameraInstanceID, out gpuicameraData) && gpuicameraData != null)
			{
				gpuicameraData.Dispose();
			}
			return base.Remove(cameraInstanceID);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000BE10 File Offset: 0x0000A010
		internal bool ClearEmptyCameraData()
		{
			bool flag = false;
			foreach (KeyValuePair<int, GPUICameraData> keyValuePair in this._dataDict)
			{
				if (keyValuePair.Value == null || keyValuePair.Value.ActiveCamera == null)
				{
					this._removalQueue.Enqueue(keyValuePair.Key);
					flag = true;
				}
			}
			int num;
			while (this._removalQueue.TryDequeue(out num))
			{
				this.Remove(num);
			}
			return flag;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		internal GPUICameraData AddCamera(Camera camera)
		{
			GPUICamera gpuicamera;
			if (!camera.gameObject.TryGetComponent<GPUICamera>(out gpuicamera))
			{
				gpuicamera = camera.gameObject.AddComponent<GPUICamera>();
			}
			gpuicamera.Initialize();
			this.AddCameraData(gpuicamera._cameraData);
			return gpuicamera._cameraData;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		internal void AddCameraData(GPUICameraData cameraData)
		{
			if (cameraData.ActiveCamera != null && !this.AddOrSet(cameraData.ActiveCamera.GetInstanceIDGPUI(), cameraData))
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not add Camera Data.", cameraData.ActiveCamera.gameObject);
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000BF3A File Offset: 0x0000A13A
		internal void RemoveCamera(Camera camera)
		{
			if (camera != null)
			{
				this.Remove(camera.GetInstanceIDGPUI());
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000BF54 File Offset: 0x0000A154
		internal bool RegisterDefaultCamera()
		{
			Camera camera = null;
			if (GPUIRuntimeSettings.Instance.cameraLoadingType != GPUICameraLoadingType.GPUICameraComponent)
			{
				camera = Camera.main;
				if (camera == null && GPUIRuntimeSettings.Instance.cameraLoadingType == GPUICameraLoadingType.Any)
				{
					Camera[] allCameras = Camera.allCameras;
					if (allCameras.Length != 0)
					{
						camera = allCameras[0];
					}
				}
			}
			if (camera == null)
			{
				return false;
			}
			this.AddCamera(camera);
			return true;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000BFAE File Offset: 0x0000A1AE
		public override bool ContainsValue(GPUICameraData value)
		{
			return base.ContainsValue(value);
		}

		// Token: 0x0400016C RID: 364
		private Queue<int> _removalQueue;
	}
}
