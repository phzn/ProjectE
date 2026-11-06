using System;
using Features.Detection;
using Features.Rank;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E2E RID: 3630
	public static class UncoveredBottomRequirement
	{
		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06006C54 RID: 27732 RVA: 0x002B553C File Offset: 0x002B373C
		public static string BlockedMessage
		{
			get
			{
				RankProgressionConfigSO config = RankProgressionService.Config;
				if (!(config != null) || string.IsNullOrEmpty(config.masturbationBlockedMessage))
				{
					return "Take your clothes off first";
				}
				return config.masturbationBlockedMessage;
			}
		}

		// Token: 0x06006C55 RID: 27733 RVA: 0x002B5571 File Offset: 0x002B3771
		public static bool TryAllow()
		{
			if (!PlayerExposureState.IsBottomPhysicallyCovered)
			{
				return true;
			}
			BlockedActionFeedback.Show(UncoveredBottomRequirement.BlockedMessage);
			return false;
		}
	}
}
