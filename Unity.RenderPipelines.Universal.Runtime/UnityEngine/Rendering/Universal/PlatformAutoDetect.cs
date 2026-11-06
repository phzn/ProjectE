using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000104 RID: 260
	internal static class PlatformAutoDetect
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x00034168 File Offset: 0x00032368
		internal static void Initialize()
		{
			PlatformAutoDetect.isXRMobile = false;
			PlatformAutoDetect.isShaderAPIMobileDefined = GraphicsSettings.HasShaderDefine(BuiltinShaderDefine.SHADER_API_MOBILE);
			PlatformAutoDetect.isSwitch = Application.platform == RuntimePlatform.Switch;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0003418A File Offset: 0x0003238A
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00034191 File Offset: 0x00032391
		internal static bool isXRMobile { get; private set; } = false;

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00034199 File Offset: 0x00032399
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x000341A0 File Offset: 0x000323A0
		internal static bool isShaderAPIMobileDefined { get; private set; } = false;

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x000341A8 File Offset: 0x000323A8
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x000341AF File Offset: 0x000323AF
		internal static bool isSwitch { get; private set; } = false;

		// Token: 0x0600081D RID: 2077 RVA: 0x000341B7 File Offset: 0x000323B7
		internal static ShEvalMode ShAutoDetect(ShEvalMode mode)
		{
			if (mode != ShEvalMode.Auto)
			{
				return mode;
			}
			if (PlatformAutoDetect.isXRMobile || PlatformAutoDetect.isShaderAPIMobileDefined || PlatformAutoDetect.isSwitch)
			{
				return ShEvalMode.PerVertex;
			}
			return ShEvalMode.PerPixel;
		}

		// Token: 0x04000799 RID: 1945
		internal static bool isRunningOnPowerVRGPU = SystemInfo.graphicsDeviceName.Contains("PowerVR");
	}
}
