using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000379 RID: 889
	internal static class UIElementsPackageUtility
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001DFF RID: 7679 RVA: 0x000742F3 File Offset: 0x000724F3
		// (set) Token: 0x06001E00 RID: 7680 RVA: 0x000742FA File Offset: 0x000724FA
		internal static bool IsUIEPackageLoaded { get; private set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x00074302 File Offset: 0x00072502
		// (set) Token: 0x06001E02 RID: 7682 RVA: 0x00074309 File Offset: 0x00072509
		internal static string EditorResourcesBasePath { get; private set; }

		// Token: 0x06001E03 RID: 7683 RVA: 0x00074311 File Offset: 0x00072511
		static UIElementsPackageUtility()
		{
			UIElementsPackageUtility.Refresh();
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0007431A File Offset: 0x0007251A
		internal static void Refresh()
		{
			UIElementsPackageUtility.EditorResourcesBasePath = "";
			UIElementsPackageUtility.IsUIEPackageLoaded = false;
		}
	}
}
