using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000856 RID: 2134
	public class SwatchSelector : MonoBehaviour
	{
		// Token: 0x06003887 RID: 14471 RVA: 0x0014A176 File Offset: 0x00148376
		public void Init(ColorPickerControl colorPickerControl, OutfitSwatch swatchData, int swatchIndex)
		{
			this.picker = colorPickerControl;
			this.swatchIndex = swatchIndex;
			this.color1.color = swatchData.IconColorTop;
			this.color2.color = swatchData.IconColorBottom;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0014A1A8 File Offset: 0x001483A8
		public void SetSwatch()
		{
			this.picker.SetBaseTexture(this.swatchIndex);
		}

		// Token: 0x04003971 RID: 14705
		private ColorPickerControl picker;

		// Token: 0x04003972 RID: 14706
		public int swatchIndex;

		// Token: 0x04003973 RID: 14707
		public Image color1;

		// Token: 0x04003974 RID: 14708
		public Image color2;
	}
}
