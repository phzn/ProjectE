using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001D RID: 29
	internal class PlatformUtilities
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00004B7E File Offset: 0x00002D7E
		internal static bool PlatformUsesMultiThreading(RuntimePlatform platform)
		{
			return platform != RuntimePlatform.WebGLPlayer;
		}
	}
}
