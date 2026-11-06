using System;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x0200001B RID: 27
	public static class GPUIPrefabConstants
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00005B91 File Offset: 0x00003D91
		public static string GetPackagesPath()
		{
			if (string.IsNullOrEmpty(GPUIPrefabConstants._packagesPath))
			{
				GPUIPrefabConstants._packagesPath = "Packages/com.gurbu.gpui-pro.prefab/";
			}
			return GPUIPrefabConstants._packagesPath;
		}

		// Token: 0x0400007E RID: 126
		private static string _packagesPath;

		// Token: 0x0400007F RID: 127
		public static readonly string Kw_GPUI_MATERIAL_VARIATION = "GPUI_MATERIAL_VARIATION";
	}
}
