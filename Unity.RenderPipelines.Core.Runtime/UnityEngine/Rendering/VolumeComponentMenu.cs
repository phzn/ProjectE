using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E8 RID: 232
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class VolumeComponentMenu : Attribute
	{
		// Token: 0x060007A8 RID: 1960 RVA: 0x00025A74 File Offset: 0x00023C74
		public VolumeComponentMenu(string menu)
		{
			this.menu = menu;
		}

		// Token: 0x040004C1 RID: 1217
		public readonly string menu;
	}
}
