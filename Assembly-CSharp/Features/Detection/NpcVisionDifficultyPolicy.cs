using System;
using Features.RunModes;

namespace Features.Detection
{
	// Token: 0x020012CB RID: 4811
	public static class NpcVisionDifficultyPolicy
	{
		// Token: 0x06009509 RID: 38153 RVA: 0x003D2DF8 File Offset: 0x003D0FF8
		public static float ApplyRadius(float authoredRadius)
		{
			float npcVisionMultiplier = RunModeService.NpcVisionMultiplier;
			if (npcVisionMultiplier != 1f)
			{
				return authoredRadius * npcVisionMultiplier;
			}
			return authoredRadius;
		}
	}
}
