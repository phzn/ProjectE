using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CompassNavigatorPro
{
	// Token: 0x02000003 RID: 3
	public static class CNP2URPCameraSetup
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
		public static void SetupURPCamera(Camera cam, bool renderShadows)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = cam.GetComponent<UniversalAdditionalCameraData>();
			if (universalAdditionalCameraData == null)
			{
				universalAdditionalCameraData = cam.gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
			if (universalAdditionalCameraData != null)
			{
				universalAdditionalCameraData.renderShadows = renderShadows;
			}
		}

		// Token: 0x04000001 RID: 1
		public static bool usesURP = true;
	}
}
