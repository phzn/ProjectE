using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003E RID: 62
	public static class ComponentUtility
	{
		// Token: 0x06000273 RID: 627 RVA: 0x0001312B File Offset: 0x0001132B
		public static bool IsUniversalCamera(Camera camera)
		{
			return camera.GetComponent<UniversalAdditionalCameraData>() != null;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00013139 File Offset: 0x00011339
		public static bool IsUniversalLight(Light light)
		{
			return light.GetComponent<UniversalAdditionalLightData>() != null;
		}
	}
}
