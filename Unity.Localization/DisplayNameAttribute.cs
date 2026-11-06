using System;

namespace UnityEngine.Localization
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class DisplayNameAttribute : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002280 File Offset: 0x00000480
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002288 File Offset: 0x00000488
		public string Name { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002291 File Offset: 0x00000491
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002299 File Offset: 0x00000499
		public string IconPath { get; set; }

		// Token: 0x06000021 RID: 33 RVA: 0x000022A2 File Offset: 0x000004A2
		public DisplayNameAttribute(string name, string iconPath = null)
		{
			this.Name = name;
			this.IconPath = iconPath;
		}
	}
}
