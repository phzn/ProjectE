using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000079 RID: 121
	internal struct UnsafeJsonValidator
	{
		// Token: 0x04000196 RID: 406
		public JsonTypeStack Stack;

		// Token: 0x04000197 RID: 407
		public int CharBufferPosition;

		// Token: 0x04000198 RID: 408
		public ushort PrevChar;

		// Token: 0x04000199 RID: 409
		public JsonType Expected;

		// Token: 0x0400019A RID: 410
		public JsonType Actual;

		// Token: 0x0400019B RID: 411
		public int LineCount;

		// Token: 0x0400019C RID: 412
		public int LineStart;

		// Token: 0x0400019D RID: 413
		public int CharCount;

		// Token: 0x0400019E RID: 414
		public ushort Char;

		// Token: 0x0400019F RID: 415
		public JsonType PartialTokenType;

		// Token: 0x040001A0 RID: 416
		public int PartialTokenState;
	}
}
