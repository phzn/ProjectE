using System;

namespace UnityEngine.Networking
{
	// Token: 0x0200000A RID: 10
	public interface IMultipartFormSection
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000065 RID: 101
		string sectionName { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102
		byte[] sectionData { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000067 RID: 103
		string fileName { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104
		string contentType { get; }
	}
}
