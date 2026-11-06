using System;
using DA_Assets.CR;
using UnityEngine;

namespace Features.Missions
{
	// Token: 0x02000FA0 RID: 4000
	internal static class MissionPanelStyle
	{
		// Token: 0x0600786A RID: 30826 RVA: 0x001A3764 File Offset: 0x001A1964
		internal static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x04007BC9 RID: 31689
		internal static readonly Color Accent = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04007BCA RID: 31690
		internal static readonly Color Text = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04007BCB RID: 31691
		internal static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x04007BCC RID: 31692
		internal static readonly Color Card = new Color(0.165f, 0.118f, 0.2f, 0.9f);

		// Token: 0x04007BCD RID: 31693
		internal static readonly Color CardHover = new Color(0.243f, 0.165f, 0.298f, 1f);

		// Token: 0x04007BCE RID: 31694
		internal static readonly Color Track = new Color(0.035f, 0.015f, 0.035f, 1f);

		// Token: 0x04007BCF RID: 31695
		internal static readonly Color ScrollTrack = new Color(0.035f, 0.015f, 0.035f, 0.82f);

		// Token: 0x04007BD0 RID: 31696
		internal const float DescriptionSize = 16f;

		// Token: 0x04007BD1 RID: 31697
		internal const float ProgressSize = 12f;

		// Token: 0x04007BD2 RID: 31698
		internal const float RewardSize = 14f;

		// Token: 0x04007BD3 RID: 31699
		internal const float NumberSize = 13f;

		// Token: 0x04007BD4 RID: 31700
		internal const float CardRadius = 14f;

		// Token: 0x04007BD5 RID: 31701
		internal const float RowGap = 14f;

		// Token: 0x04007BD6 RID: 31702
		internal const float MinimumRowHeight = 62f;
	}
}
