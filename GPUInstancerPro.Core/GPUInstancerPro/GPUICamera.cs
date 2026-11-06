using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000052 RID: 82
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Camera")]
	public class GPUICamera : MonoBehaviour
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000F2E9 File Offset: 0x0000D4E9
		private void Awake()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F2FA File Offset: 0x0000D4FA
		private void OnEnable()
		{
			if (!this._isInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F30A File Offset: 0x0000D50A
		private void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000F314 File Offset: 0x0000D514
		public void Initialize()
		{
			if (this._cameraRef == null)
			{
				this._cameraRef = base.GetComponent<Camera>();
			}
			bool flag = this._cameraData != null;
			if (this._isInitialized && flag)
			{
				return;
			}
			if (flag)
			{
				this.Dispose();
			}
			this._cameraData = new GPUICameraData(this._cameraRef);
			GPUIRenderingSystem.AddCameraData(this._cameraData);
			if (this._enableOcclusionCulling)
			{
				this._cameraData.autoInitializeOcclusionCulling = true;
			}
			this._isInitialized = true;
			this.SetDynamicOcclusionOffsetIntensity(this._dynamicOcclusionOffsetIntensity);
			if (!base.enabled)
			{
				base.enabled = true;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F3AB File Offset: 0x0000D5AB
		public void Dispose()
		{
			this._isInitialized = false;
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.CameraDataProvider.RemoveCamera(this._cameraRef);
			}
			if (this._cameraData != null)
			{
				this._cameraData.Dispose();
			}
			this._cameraData = null;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F3EA File Offset: 0x0000D5EA
		public GPUICameraData GetCameraData()
		{
			return this._cameraData;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000F3F2 File Offset: 0x0000D5F2
		public Camera GetCamera()
		{
			if (this._cameraRef == null)
			{
				this._cameraRef = base.GetComponent<Camera>();
			}
			return this._cameraRef;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000F414 File Offset: 0x0000D614
		public void SetOcclusionCullingEnabled(bool enabled)
		{
			this._enableOcclusionCulling = enabled;
			if (this._cameraData == null)
			{
				return;
			}
			if (enabled)
			{
				this._cameraData.autoInitializeOcclusionCulling = true;
				return;
			}
			this._cameraData.autoInitializeOcclusionCulling = false;
			this._cameraData.DisableOcclusionCulling();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000F44D File Offset: 0x0000D64D
		public void SetDynamicOcclusionOffsetIntensity(float intensity)
		{
			this._dynamicOcclusionOffsetIntensity = intensity;
			if (this._cameraData == null)
			{
				return;
			}
			this._cameraData.SetDynamicOcclusionOffsetIntensity(intensity);
		}

		// Token: 0x0400021E RID: 542
		[SerializeField]
		private bool _enableOcclusionCulling = true;

		// Token: 0x0400021F RID: 543
		[SerializeField]
		[Range(0f, 10f)]
		private float _dynamicOcclusionOffsetIntensity = 1f;

		// Token: 0x04000220 RID: 544
		[NonSerialized]
		internal Camera _cameraRef;

		// Token: 0x04000221 RID: 545
		[NonSerialized]
		internal GPUICameraData _cameraData;

		// Token: 0x04000222 RID: 546
		[NonSerialized]
		private bool _isInitialized;
	}
}
