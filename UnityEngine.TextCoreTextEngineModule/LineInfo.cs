using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000012 RID: 18
	internal struct LineInfo
	{
		// Token: 0x0400007C RID: 124
		internal int controlCharacterCount;

		// Token: 0x0400007D RID: 125
		public int characterCount;

		// Token: 0x0400007E RID: 126
		public int visibleCharacterCount;

		// Token: 0x0400007F RID: 127
		public int spaceCount;

		// Token: 0x04000080 RID: 128
		public int visibleSpaceCount;

		// Token: 0x04000081 RID: 129
		public int wordCount;

		// Token: 0x04000082 RID: 130
		public int firstCharacterIndex;

		// Token: 0x04000083 RID: 131
		public int firstVisibleCharacterIndex;

		// Token: 0x04000084 RID: 132
		public int lastCharacterIndex;

		// Token: 0x04000085 RID: 133
		public int lastVisibleCharacterIndex;

		// Token: 0x04000086 RID: 134
		public float length;

		// Token: 0x04000087 RID: 135
		public float lineHeight;

		// Token: 0x04000088 RID: 136
		public float ascender;

		// Token: 0x04000089 RID: 137
		public float baseline;

		// Token: 0x0400008A RID: 138
		public float descender;

		// Token: 0x0400008B RID: 139
		public float maxAdvance;

		// Token: 0x0400008C RID: 140
		public float width;

		// Token: 0x0400008D RID: 141
		public float marginLeft;

		// Token: 0x0400008E RID: 142
		public float marginRight;

		// Token: 0x0400008F RID: 143
		public TextAlignment alignment;

		// Token: 0x04000090 RID: 144
		public Extents lineExtents;
	}
}
