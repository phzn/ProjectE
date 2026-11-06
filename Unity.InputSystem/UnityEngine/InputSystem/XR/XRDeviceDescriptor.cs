using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x0200006E RID: 110
	[Serializable]
	public class XRDeviceDescriptor
	{
		// Token: 0x06000A2E RID: 2606 RVA: 0x000382AA File Offset: 0x000364AA
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000382B2 File Offset: 0x000364B2
		public static XRDeviceDescriptor FromJson(string json)
		{
			return JsonUtility.FromJson<XRDeviceDescriptor>(json);
		}

		// Token: 0x04000374 RID: 884
		public string deviceName;

		// Token: 0x04000375 RID: 885
		public string manufacturer;

		// Token: 0x04000376 RID: 886
		public string serialNumber;

		// Token: 0x04000377 RID: 887
		public InputDeviceCharacteristics characteristics;

		// Token: 0x04000378 RID: 888
		public int deviceId;

		// Token: 0x04000379 RID: 889
		public List<XRFeatureDescriptor> inputFeatures;
	}
}
