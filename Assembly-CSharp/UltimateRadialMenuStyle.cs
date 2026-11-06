using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000164 RID: 356
[CreateAssetMenu(fileName = "Ultimate Radial Menu Style", menuName = "Tank and Healer Studio/Ultimate Radial Menu Style", order = 1)]
public class UltimateRadialMenuStyle : ScriptableObject
{
	// Token: 0x04000B46 RID: 2886
	public int minButtonCount = 3;

	// Token: 0x04000B47 RID: 2887
	public int maxButtonCount = 12;

	// Token: 0x04000B48 RID: 2888
	public List<UltimateRadialMenuStyle.RadialMenuStyle> RadialMenuStyles = new List<UltimateRadialMenuStyle.RadialMenuStyle>();

	// Token: 0x02000165 RID: 357
	[Serializable]
	public class RadialMenuStyle
	{
		// Token: 0x04000B49 RID: 2889
		public int buttonCount;

		// Token: 0x04000B4A RID: 2890
		public Sprite normalSprite;

		// Token: 0x04000B4B RID: 2891
		public Sprite highlightedSprite;

		// Token: 0x04000B4C RID: 2892
		public Sprite pressedSprite;

		// Token: 0x04000B4D RID: 2893
		public Sprite selectedSprite;

		// Token: 0x04000B4E RID: 2894
		public Sprite disabledSprite;
	}
}
