using System;

namespace CompassNavigatorPro
{
	// Token: 0x020002A3 RID: 675
	public static class MinimapContentsExtensions
	{
		// Token: 0x06001340 RID: 4928 RVA: 0x0007CCEA File Offset: 0x0007AEEA
		public static bool usesTexture(this MiniMapContents contents)
		{
			return contents == MiniMapContents.UITexture || contents == MiniMapContents.Radar;
		}
	}
}
