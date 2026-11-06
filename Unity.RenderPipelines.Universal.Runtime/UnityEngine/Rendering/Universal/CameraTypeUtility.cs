using System;
using System.Linq;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E9 RID: 233
	internal static class CameraTypeUtility
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x0002CB70 File Offset: 0x0002AD70
		public static string GetName(this CameraRenderType type)
		{
			int num = (int)type;
			if (num < 0 || num >= CameraTypeUtility.s_CameraTypeNames.Length)
			{
				num = 0;
			}
			return CameraTypeUtility.s_CameraTypeNames[num];
		}

		// Token: 0x040005B5 RID: 1461
		private static string[] s_CameraTypeNames = Enum.GetNames(typeof(CameraRenderType)).ToArray<string>();
	}
}
