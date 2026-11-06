using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B5 RID: 181
	public enum RenderPassEvent
	{
		// Token: 0x040004A1 RID: 1185
		BeforeRendering,
		// Token: 0x040004A2 RID: 1186
		BeforeRenderingShadows = 50,
		// Token: 0x040004A3 RID: 1187
		AfterRenderingShadows = 100,
		// Token: 0x040004A4 RID: 1188
		BeforeRenderingPrePasses = 150,
		// Token: 0x040004A5 RID: 1189
		AfterRenderingPrePasses = 200,
		// Token: 0x040004A6 RID: 1190
		BeforeRenderingGbuffer = 210,
		// Token: 0x040004A7 RID: 1191
		AfterRenderingGbuffer = 220,
		// Token: 0x040004A8 RID: 1192
		BeforeRenderingDeferredLights = 230,
		// Token: 0x040004A9 RID: 1193
		AfterRenderingDeferredLights = 240,
		// Token: 0x040004AA RID: 1194
		BeforeRenderingOpaques = 250,
		// Token: 0x040004AB RID: 1195
		AfterRenderingOpaques = 300,
		// Token: 0x040004AC RID: 1196
		BeforeRenderingSkybox = 350,
		// Token: 0x040004AD RID: 1197
		AfterRenderingSkybox = 400,
		// Token: 0x040004AE RID: 1198
		BeforeRenderingTransparents = 450,
		// Token: 0x040004AF RID: 1199
		AfterRenderingTransparents = 500,
		// Token: 0x040004B0 RID: 1200
		BeforeRenderingPostProcessing = 550,
		// Token: 0x040004B1 RID: 1201
		AfterRenderingPostProcessing = 600,
		// Token: 0x040004B2 RID: 1202
		AfterRendering = 1000
	}
}
