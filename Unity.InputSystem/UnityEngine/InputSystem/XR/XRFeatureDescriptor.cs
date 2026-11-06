using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	public struct XRFeatureDescriptor
	{
		// Token: 0x04000370 RID: 880
		public string name;

		// Token: 0x04000371 RID: 881
		public List<UsageHint> usageHints;

		// Token: 0x04000372 RID: 882
		public FeatureType featureType;

		// Token: 0x04000373 RID: 883
		public uint customSize;
	}
}
