using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200009D RID: 157
	public class MMColorAttribute : PropertyAttribute
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00011690 File Offset: 0x0000F890
		public MMColorAttribute(float red = 1f, float green = 0f, float blue = 0f)
		{
			this.color = new Color(red, green, blue, 1f);
		}

		// Token: 0x040004CE RID: 1230
		public Color color;
	}
}
