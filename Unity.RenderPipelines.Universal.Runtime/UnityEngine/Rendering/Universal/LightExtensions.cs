using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000EC RID: 236
	public static class LightExtensions
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x0002D268 File Offset: 0x0002B468
		public static UniversalAdditionalLightData GetUniversalAdditionalLightData(this Light light)
		{
			GameObject gameObject = light.gameObject;
			UniversalAdditionalLightData universalAdditionalLightData;
			if (!gameObject.TryGetComponent<UniversalAdditionalLightData>(out universalAdditionalLightData))
			{
				universalAdditionalLightData = gameObject.AddComponent<UniversalAdditionalLightData>();
			}
			return universalAdditionalLightData;
		}
	}
}
