using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000028 RID: 40
	internal interface ILight2DCullResult
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000173 RID: 371
		List<Light2D> visibleLights { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000174 RID: 372
		List<ShadowCasterGroup2D> visibleShadows { get; }

		// Token: 0x06000175 RID: 373
		LightStats GetLightStatsByLayer(int layer);

		// Token: 0x06000176 RID: 374
		bool IsSceneLit();
	}
}
