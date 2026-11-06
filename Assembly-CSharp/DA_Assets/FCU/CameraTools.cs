using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002B9 RID: 697
	internal class CameraTools
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x0007F39C File Offset: 0x0007D59C
		internal static void ExcludeBlurFromCulling(Camera camera)
		{
			int num = LayerTools.AddLayer(FcuConfig.BlurredObjectTag);
			camera.cullingMask &= ~(1 << num);
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0007F3C8 File Offset: 0x0007D5C8
		internal static Camera GetOrCreateMainCamera()
		{
			Camera camera = Camera.main;
			if (camera == null)
			{
				camera = new GameObject("MainCamera").AddComponent<Camera>();
				camera.tag = "MainCamera";
			}
			return camera;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0007F400 File Offset: 0x0007D600
		internal static Camera GetOrCreateBackgroundBlurCamera()
		{
			if (!CameraTools.IsTagExists(FcuConfig.BlurCameraTag))
			{
				CameraTools.AddTag(FcuConfig.BlurCameraTag);
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(FcuConfig.BlurCameraTag);
			Camera camera;
			if (gameObject == null)
			{
				GameObject gameObject2 = new GameObject(FcuConfig.BlurCameraTag + "Camera");
				camera = gameObject2.AddComponent<Camera>();
				gameObject2.tag = FcuConfig.BlurCameraTag;
			}
			else
			{
				camera = gameObject.GetComponent<Camera>();
			}
			return camera;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0000D91D File Offset: 0x0000BB1D
		private static bool IsTagExists(string tag)
		{
			return false;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00004F31 File Offset: 0x00003131
		private static void AddTag(string tag)
		{
		}
	}
}
