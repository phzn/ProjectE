using System;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x0200004E RID: 78
	internal struct BundleFrameData
	{
		// Token: 0x040000BA RID: 186
		public int BundleCode;

		// Token: 0x040000BB RID: 187
		public int ReferenceCount;

		// Token: 0x040000BC RID: 188
		public float PercentComplete;

		// Token: 0x040000BD RID: 189
		public ContentStatus Status;

		// Token: 0x040000BE RID: 190
		public BundleSource Source;

		// Token: 0x040000BF RID: 191
		public BundleOptions LoadingOptions;
	}
}
