using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class MMDebugMenuTabData
	{
		// Token: 0x04000153 RID: 339
		public string Name = "TabName";

		// Token: 0x04000154 RID: 340
		public bool Active = true;

		// Token: 0x04000155 RID: 341
		[MMReorderableAttribute]
		public MMDebugMenuItemList MenuItems;
	}
}
