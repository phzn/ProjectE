using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A8 RID: 168
	[AttributeUsage(AttributeTargets.Field)]
	public class MMInspectorButtonBarAttribute : PropertyAttribute
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000117DC File Offset: 0x0000F9DC
		// (set) Token: 0x0600036D RID: 877 RVA: 0x000117E4 File Offset: 0x0000F9E4
		public string[] Labels { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000117ED File Offset: 0x0000F9ED
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000117F5 File Offset: 0x0000F9F5
		public string[] Methods { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000117FE File Offset: 0x0000F9FE
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00011806 File Offset: 0x0000FA06
		public bool[] OnlyWhenPlaying { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0001180F File Offset: 0x0000FA0F
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00011817 File Offset: 0x0000FA17
		public string[] UssClass { get; set; }

		// Token: 0x06000374 RID: 884 RVA: 0x00011820 File Offset: 0x0000FA20
		public MMInspectorButtonBarAttribute(string[] labels, string[] methods, bool[] onlyWhenPlaying, string[] ussClass)
		{
			this.Labels = labels;
			this.Methods = methods;
			this.OnlyWhenPlaying = onlyWhenPlaying;
			this.UssClass = ussClass;
		}
	}
}
