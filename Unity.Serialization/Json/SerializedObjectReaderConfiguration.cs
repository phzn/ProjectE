using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000053 RID: 83
	public struct SerializedObjectReaderConfiguration
	{
		// Token: 0x040000ED RID: 237
		public bool UseReadAsync;

		// Token: 0x040000EE RID: 238
		public int BlockBufferSize;

		// Token: 0x040000EF RID: 239
		public int TokenBufferSize;

		// Token: 0x040000F0 RID: 240
		public int OutputBufferSize;

		// Token: 0x040000F1 RID: 241
		public int NodeBufferSize;

		// Token: 0x040000F2 RID: 242
		public JsonValidationType ValidationType;

		// Token: 0x040000F3 RID: 243
		public bool StripStringEscapeCharacters;

		// Token: 0x040000F4 RID: 244
		public static readonly SerializedObjectReaderConfiguration Default = new SerializedObjectReaderConfiguration
		{
			UseReadAsync = true,
			BlockBufferSize = 4096,
			TokenBufferSize = 1024,
			OutputBufferSize = 4096,
			NodeBufferSize = 128,
			ValidationType = JsonValidationType.Standard,
			StripStringEscapeCharacters = true
		};
	}
}
