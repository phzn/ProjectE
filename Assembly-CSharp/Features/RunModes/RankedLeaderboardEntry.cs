using System;

namespace Features.RunModes
{
	// Token: 0x02000D53 RID: 3411
	public sealed class RankedLeaderboardEntry
	{
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x0600643F RID: 25663 RVA: 0x0028291D File Offset: 0x00280B1D
		public string UserId { get; }

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06006440 RID: 25664 RVA: 0x00282925 File Offset: 0x00280B25
		public string DisplayName { get; }

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06006441 RID: 25665 RVA: 0x0028292D File Offset: 0x00280B2D
		public int GlobalRank { get; }

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06006442 RID: 25666 RVA: 0x00282935 File Offset: 0x00280B35
		public int DistanceMeters { get; }

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06006443 RID: 25667 RVA: 0x0028293D File Offset: 0x00280B3D
		public int? Score { get; }

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06006444 RID: 25668 RVA: 0x00282945 File Offset: 0x00280B45
		public bool IsLocalPlayer { get; }

		// Token: 0x06006445 RID: 25669 RVA: 0x00282950 File Offset: 0x00280B50
		public RankedLeaderboardEntry(string userId, string displayName, int globalRank, int distanceMeters, int? score, bool isLocalPlayer)
		{
			this.UserId = userId ?? string.Empty;
			this.DisplayName = ((string.IsNullOrWhiteSpace(displayName) || displayName == "[unknown]") ? "Steam player" : displayName.Replace('\r', ' ').Replace('\n', ' '));
			this.GlobalRank = globalRank;
			this.DistanceMeters = distanceMeters;
			this.Score = score;
			this.IsLocalPlayer = isLocalPlayer;
		}
	}
}
