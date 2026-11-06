using System;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000841 RID: 2113
	[Serializable]
	public class StyleMorphValue
	{
		// Token: 0x060037FC RID: 14332 RVA: 0x001479A4 File Offset: 0x00145BA4
		public StyleMorphValue(string partId, string presetId, string controlId, float value)
		{
			this.partId = partId;
			this.presetId = presetId;
			this.controlId = controlId;
			this.value = value;
		}

		// Token: 0x040038EE RID: 14574
		public string partId;

		// Token: 0x040038EF RID: 14575
		public string presetId;

		// Token: 0x040038F0 RID: 14576
		public string controlId;

		// Token: 0x040038F1 RID: 14577
		public float value;
	}
}
