using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x0200049F RID: 1183
	public class vImageColorChange : MonoBehaviour
	{
		// Token: 0x06001CFD RID: 7421 RVA: 0x000B2F80 File Offset: 0x000B1180
		public void ChangeColor(int colorIndex)
		{
			if (this.colors.Length != 0 && colorIndex < this.colors.Length)
			{
				this.image.color = this.colors[colorIndex];
			}
		}

		// Token: 0x04001E1C RID: 7708
		public Image image;

		// Token: 0x04001E1D RID: 7709
		public Color[] colors;
	}
}
