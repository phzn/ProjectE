using System;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004A RID: 74
	public class DrawingSettings : ScriptableObject
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000B7E5 File Offset: 0x000099E5
		public static DrawingSettings.Settings DefaultSettings
		{
			get
			{
				return new DrawingSettings.Settings();
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000B7EC File Offset: 0x000099EC
		public static DrawingSettings GetSettingsAsset()
		{
			return Resources.Load<DrawingSettings>("AstarGizmos");
		}

		// Token: 0x04000128 RID: 296
		public const string SettingsPathCompatibility = "Assets/Settings/ALINE.asset";

		// Token: 0x04000129 RID: 297
		public const string SettingsName = "AstarGizmos";

		// Token: 0x0400012A RID: 298
		public const string SettingsPath = "Assets/Settings/Resources/AstarGizmos.asset";

		// Token: 0x0400012B RID: 299
		[SerializeField]
		private int version;

		// Token: 0x0400012C RID: 300
		public DrawingSettings.Settings settings;

		// Token: 0x0200004B RID: 75
		[Serializable]
		public class Settings
		{
			// Token: 0x0400012D RID: 301
			public float lineOpacity = 1f;

			// Token: 0x0400012E RID: 302
			public float solidOpacity = 0.55f;

			// Token: 0x0400012F RID: 303
			public float textOpacity = 1f;

			// Token: 0x04000130 RID: 304
			public float lineOpacityBehindObjects = 0.12f;

			// Token: 0x04000131 RID: 305
			public float solidOpacityBehindObjects = 0.45f;

			// Token: 0x04000132 RID: 306
			public float textOpacityBehindObjects = 0.9f;

			// Token: 0x04000133 RID: 307
			public float curveResolution = 1f;
		}
	}
}
