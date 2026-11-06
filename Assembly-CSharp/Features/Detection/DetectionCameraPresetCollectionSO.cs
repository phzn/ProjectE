using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001247 RID: 4679
	[CreateAssetMenu(fileName = "New Camera Preset Collection", menuName = "Detection/Camera Preset Collection")]
	public class DetectionCameraPresetCollectionSO : ScriptableObject
	{
		// Token: 0x06009184 RID: 37252 RVA: 0x003B9468 File Offset: 0x003B7668
		private void OnValidate()
		{
			if (this.Presets.Count > 5)
			{
				Debug.LogWarning(string.Format("[CameraPresetCollection] '{0}' has {1} presets. Max is 5. Truncating.", base.name, this.Presets.Count));
				this.Presets.RemoveRange(5, this.Presets.Count - 5);
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06009185 RID: 37253 RVA: 0x003B94C1 File Offset: 0x003B76C1
		public int Count
		{
			get
			{
				List<DetectionCameraPresetEntry> presets = this.Presets;
				if (presets == null)
				{
					return 0;
				}
				return presets.Count;
			}
		}

		// Token: 0x06009186 RID: 37254 RVA: 0x003B94D4 File Offset: 0x003B76D4
		public DetectionCameraPresetEntry GetPreset(int index)
		{
			if (this.Presets == null || index < 0 || index >= this.Presets.Count)
			{
				return null;
			}
			return this.Presets[index];
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06009187 RID: 37255 RVA: 0x003B94FE File Offset: 0x003B76FE
		public DetectionCameraPresetEntry DefaultPreset
		{
			get
			{
				if (this.Presets == null || this.Presets.Count <= 0)
				{
					return null;
				}
				return this.Presets[0];
			}
		}

		// Token: 0x040095CF RID: 38351
		[Tooltip("Camera presets (max 5). First entry is the default.")]
		public List<DetectionCameraPresetEntry> Presets = new List<DetectionCameraPresetEntry>();
	}
}
