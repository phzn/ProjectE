using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E8 RID: 232
	public static class CameraExtensions
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0002C9C4 File Offset: 0x0002ABC4
		public static UniversalAdditionalCameraData GetUniversalAdditionalCameraData(this Camera camera)
		{
			GameObject gameObject = camera.gameObject;
			UniversalAdditionalCameraData universalAdditionalCameraData;
			if (!gameObject.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
			{
				universalAdditionalCameraData = gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
			return universalAdditionalCameraData;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002C9EA File Offset: 0x0002ABEA
		public static VolumeFrameworkUpdateMode GetVolumeFrameworkUpdateMode(this Camera camera)
		{
			return camera.GetUniversalAdditionalCameraData().volumeFrameworkUpdateMode;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002C9F8 File Offset: 0x0002ABF8
		public static void SetVolumeFrameworkUpdateMode(this Camera camera, VolumeFrameworkUpdateMode mode)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData.volumeFrameworkUpdateMode == mode)
			{
				return;
			}
			bool requiresVolumeFrameworkUpdate = universalAdditionalCameraData.requiresVolumeFrameworkUpdate;
			universalAdditionalCameraData.volumeFrameworkUpdateMode = mode;
			if (requiresVolumeFrameworkUpdate && !universalAdditionalCameraData.requiresVolumeFrameworkUpdate)
			{
				camera.UpdateVolumeStack(universalAdditionalCameraData);
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002CA34 File Offset: 0x0002AC34
		public static void UpdateVolumeStack(this Camera camera)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			camera.UpdateVolumeStack(universalAdditionalCameraData);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002CA50 File Offset: 0x0002AC50
		public static void UpdateVolumeStack(this Camera camera, UniversalAdditionalCameraData cameraData)
		{
			if (cameraData.requiresVolumeFrameworkUpdate)
			{
				return;
			}
			if (cameraData.volumeStack == null)
			{
				cameraData.GetOrCreateVolumeStack();
			}
			LayerMask layerMask;
			Transform transform;
			camera.GetVolumeLayerMaskAndTrigger(cameraData, out layerMask, out transform);
			VolumeManager.instance.Update(cameraData.volumeStack, transform, layerMask);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002CA94 File Offset: 0x0002AC94
		public static void DestroyVolumeStack(this Camera camera)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			camera.DestroyVolumeStack(universalAdditionalCameraData);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002CAAF File Offset: 0x0002ACAF
		public static void DestroyVolumeStack(this Camera camera, UniversalAdditionalCameraData cameraData)
		{
			if (cameraData == null || cameraData.volumeStack == null)
			{
				return;
			}
			cameraData.volumeStack = null;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002CACC File Offset: 0x0002ACCC
		internal static void GetVolumeLayerMaskAndTrigger(this Camera camera, UniversalAdditionalCameraData cameraData, out LayerMask layerMask, out Transform trigger)
		{
			layerMask = 1;
			trigger = camera.transform;
			if (cameraData != null)
			{
				layerMask = cameraData.volumeLayerMask;
				trigger = ((cameraData.volumeTrigger != null) ? cameraData.volumeTrigger : trigger);
				return;
			}
			if (camera.cameraType == CameraType.SceneView)
			{
				Camera main = Camera.main;
				UniversalAdditionalCameraData universalAdditionalCameraData = null;
				if (main != null && main.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
				{
					layerMask = universalAdditionalCameraData.volumeLayerMask;
				}
				trigger = ((universalAdditionalCameraData != null && universalAdditionalCameraData.volumeTrigger != null) ? universalAdditionalCameraData.volumeTrigger : trigger);
			}
		}
	}
}
