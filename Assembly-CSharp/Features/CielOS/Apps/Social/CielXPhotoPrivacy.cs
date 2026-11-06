using System;
using Features.Rank;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001457 RID: 5207
	public static class CielXPhotoPrivacy
	{
		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x0600A0B8 RID: 41144 RVA: 0x00428660 File Offset: 0x00426860
		public static bool CanShowFace
		{
			get
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return CielXPhotoPrivacy.CanShowFaceForRank((currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
			}
		}

		// Token: 0x0600A0B9 RID: 41145 RVA: 0x002DC2C4 File Offset: 0x002DA4C4
		public static bool CanShowFaceForRank(string rankId, bool admin = false)
		{
			return admin || string.Equals(rankId, "hornyslut_declared", StringComparison.Ordinal) || string.Equals(rankId, "admin", StringComparison.Ordinal);
		}
	}
}
