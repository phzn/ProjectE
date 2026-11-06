using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000835 RID: 2101
	[Serializable]
	public class OutfitData
	{
		// Token: 0x04003855 RID: 14421
		public string outfit;

		// Token: 0x04003856 RID: 14422
		public List<Color> colors;

		// Token: 0x04003857 RID: 14423
		public string decal;

		// Token: 0x04003858 RID: 14424
		public List<Color> decalColors;

		// Token: 0x04003859 RID: 14425
		public Vector4 decalScale;

		// Token: 0x0400385A RID: 14426
		public string pattern;

		// Token: 0x0400385B RID: 14427
		public List<Color> patternColors;

		// Token: 0x0400385C RID: 14428
		public Vector4 patternScale;

		// Token: 0x0400385D RID: 14429
		public Color color = Color.white;

		// Token: 0x0400385E RID: 14430
		public int swatch;
	}
}
