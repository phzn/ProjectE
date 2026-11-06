using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200009C RID: 156
	public class MMBackgroundColorAttribute : PropertyAttribute
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00011681 File Offset: 0x0000F881
		public MMBackgroundColorAttribute(MMBackgroundAttributeColor color = MMBackgroundAttributeColor.Yellow)
		{
			this.Color = color;
		}

		// Token: 0x040004CD RID: 1229
		public MMBackgroundAttributeColor Color;
	}
}
