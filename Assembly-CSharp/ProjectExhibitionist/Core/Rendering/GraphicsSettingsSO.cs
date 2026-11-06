using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000ABB RID: 2747
	[CreateAssetMenu(fileName = "GraphicsSettings", menuName = "Core/Graphics/GraphicsSettings Profile", order = 1)]
	public class GraphicsSettingsSO : ScriptableObject
	{
		// Token: 0x04004F5F RID: 20319
		[Header("Profile Info")]
		public GraphicsSettingsSO.QualityLevel qualityLevel = GraphicsSettingsSO.QualityLevel.Medium;

		// Token: 0x04004F60 RID: 20320
		public string displayName = "Medium";

		// Token: 0x04004F61 RID: 20321
		[TextArea(2, 4)]
		public string description = "Balanced quality and performance";

		// Token: 0x04004F62 RID: 20322
		[Header("Preset Values")]
		public GraphicsSettingsModel model = new GraphicsSettingsModel();

		// Token: 0x02000ABC RID: 2748
		public enum QualityLevel
		{
			// Token: 0x04004F64 RID: 20324
			Low,
			// Token: 0x04004F65 RID: 20325
			Medium,
			// Token: 0x04004F66 RID: 20326
			High,
			// Token: 0x04004F67 RID: 20327
			Ultra
		}
	}
}
