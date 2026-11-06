using System;
using UnityEngine;

namespace Features.UI.TimedEffects
{
	// Token: 0x02000BC3 RID: 3011
	[Serializable]
	public class TimedEffectData
	{
		// Token: 0x040056E3 RID: 22243
		[Tooltip("Unique identifier for this effect instance (e.g. 'MudBodyPaint', 'Rollerblades')")]
		public string EffectId;

		// Token: 0x040056E4 RID: 22244
		[Tooltip("Display name shown on the HUD entry")]
		public string DisplayName;

		// Token: 0x040056E5 RID: 22245
		[Tooltip("Icon displayed at the center of the radial timer")]
		public Sprite Icon;

		// Token: 0x040056E6 RID: 22246
		[Tooltip("Total duration of the effect in seconds")]
		public float TotalDuration;

		// Token: 0x040056E7 RID: 22247
		public bool ExternalTimeControl;

		// Token: 0x040056E8 RID: 22248
		[Tooltip("Optional accent color for the radial fill and label")]
		public Color AccentColor = new Color(0.91f, 0.36f, 0.49f, 1f);
	}
}
