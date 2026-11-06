using System;

namespace Features.BodyPaintStudio
{
	// Token: 0x02001512 RID: 5394
	[Serializable]
	public sealed class BodyPaintPresetInfo
	{
		// Token: 0x0400ABA0 RID: 43936
		public int schemaVersion;

		// Token: 0x0400ABA1 RID: 43937
		public string uvLayout;

		// Token: 0x0400ABA2 RID: 43938
		public string id;

		// Token: 0x0400ABA3 RID: 43939
		public string name;

		// Token: 0x0400ABA4 RID: 43940
		public string createdUtc;

		// Token: 0x0400ABA5 RID: 43941
		public int bodyWidth;

		// Token: 0x0400ABA6 RID: 43942
		public int bodyHeight;

		// Token: 0x0400ABA7 RID: 43943
		public int headWidth;

		// Token: 0x0400ABA8 RID: 43944
		public int headHeight;

		// Token: 0x0400ABA9 RID: 43945
		public string bodySha256;

		// Token: 0x0400ABAA RID: 43946
		public string headSha256;

		// Token: 0x0400ABAB RID: 43947
		public string previewSha256;
	}
}
