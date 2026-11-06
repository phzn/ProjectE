using System;
using Features.Rank;

namespace Features.Missions
{
	// Token: 0x02000F9F RID: 3999
	public readonly struct MissionCompletion
	{
		// Token: 0x06007869 RID: 30825 RVA: 0x00312470 File Offset: 0x00310670
		public MissionCompletion(MissionState mission, XpGrantOutcome xpOutcome)
		{
			this.Mission = mission;
			this.XpOutcome = xpOutcome;
		}

		// Token: 0x04007BC7 RID: 31687
		public readonly MissionState Mission;

		// Token: 0x04007BC8 RID: 31688
		public readonly XpGrantOutcome XpOutcome;
	}
}
