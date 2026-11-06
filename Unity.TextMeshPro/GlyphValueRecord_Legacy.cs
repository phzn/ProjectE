using System;
using UnityEngine.TextCore.LowLevel;

namespace TMPro
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public struct GlyphValueRecord_Legacy
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
		internal GlyphValueRecord_Legacy(GlyphValueRecord valueRecord)
		{
			this.xPlacement = valueRecord.xPlacement;
			this.yPlacement = valueRecord.yPlacement;
			this.xAdvance = valueRecord.xAdvance;
			this.yAdvance = valueRecord.yAdvance;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0001C5EC File Offset: 0x0001A7EC
		public static GlyphValueRecord_Legacy operator +(GlyphValueRecord_Legacy a, GlyphValueRecord_Legacy b)
		{
			GlyphValueRecord_Legacy glyphValueRecord_Legacy;
			glyphValueRecord_Legacy.xPlacement = a.xPlacement + b.xPlacement;
			glyphValueRecord_Legacy.yPlacement = a.yPlacement + b.yPlacement;
			glyphValueRecord_Legacy.xAdvance = a.xAdvance + b.xAdvance;
			glyphValueRecord_Legacy.yAdvance = a.yAdvance + b.yAdvance;
			return glyphValueRecord_Legacy;
		}

		// Token: 0x040001DE RID: 478
		public float xPlacement;

		// Token: 0x040001DF RID: 479
		public float yPlacement;

		// Token: 0x040001E0 RID: 480
		public float xAdvance;

		// Token: 0x040001E1 RID: 481
		public float yAdvance;
	}
}
