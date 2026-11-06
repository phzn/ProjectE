using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000074 RID: 116
	[Obsolete("This variant is obsolete and kept only for not breaking user code. Use IVolumeDebugSettings2 for all new usage.", false)]
	public interface IVolumeDebugSettings
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600039A RID: 922
		// (set) Token: 0x0600039B RID: 923
		int selectedComponent { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600039C RID: 924
		Camera selectedCamera { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600039D RID: 925
		IEnumerable<Camera> cameras { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600039E RID: 926
		// (set) Token: 0x0600039F RID: 927
		int selectedCameraIndex { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003A0 RID: 928
		VolumeStack selectedCameraVolumeStack { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003A1 RID: 929
		LayerMask selectedCameraLayerMask { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003A2 RID: 930
		Vector3 selectedCameraPosition { get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003A3 RID: 931
		// (set) Token: 0x060003A4 RID: 932
		Type selectedComponentType { get; set; }

		// Token: 0x060003A5 RID: 933
		Volume[] GetVolumes();

		// Token: 0x060003A6 RID: 934
		bool VolumeHasInfluence(Volume volume);

		// Token: 0x060003A7 RID: 935
		bool RefreshVolumes(Volume[] newVolumes);

		// Token: 0x060003A8 RID: 936
		float GetVolumeWeight(Volume volume);
	}
}
