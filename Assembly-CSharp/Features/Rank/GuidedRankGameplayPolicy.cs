using System;
using Features.RunModes;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000D9B RID: 3483
	public static class GuidedRankGameplayPolicy
	{
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06006691 RID: 26257 RVA: 0x0028FFA8 File Offset: 0x0028E1A8
		public static bool AdvancedMechanicsBlocked
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return GuidedRankGameplayPolicy.ShouldBlockAdvancedMechanics(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06006692 RID: 26258 RVA: 0x0028FFD3 File Offset: 0x0028E1D3
		public static bool RideableVehiclesBlocked
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return GuidedRankGameplayPolicy.ShouldBlockRideableVehicles(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x0028FFFE File Offset: 0x0028E1FE
		public static bool ShouldBlockRideableVehicles(RunModeKind mode, string rankId, bool isAdminRank)
		{
			return !isAdminRank && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && (rankId != "horny" && rankId != "hornyslut") && rankId != "hornyslut_declared";
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x00290034 File Offset: 0x0028E234
		public static float RideableSpawnMultiplier(RunModeKind mode, string rankId, bool isAdminRank)
		{
			if (mode == RunModeKind.LongWayHome)
			{
				return 0.2f;
			}
			if (GuidedRankGameplayPolicy.ShouldBlockRideableVehicles(mode, rankId, isAdminRank))
			{
				return 0f;
			}
			if (isAdminRank || (mode != RunModeKind.Classic && mode != RunModeKind.Guided) || (!(rankId == "horny") && !(rankId == "hornyslut")))
			{
				return 1f;
			}
			return 0.8f;
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06006695 RID: 26261 RVA: 0x0029008A File Offset: 0x0028E28A
		public static bool CarSpawningBlocked
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return GuidedRankGameplayPolicy.ShouldBlockCarSpawning(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06006696 RID: 26262 RVA: 0x002900B5 File Offset: 0x0028E2B5
		public static bool CarInteractionsBlocked
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return GuidedRankGameplayPolicy.ShouldBlockCarInteractions(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x002900E0 File Offset: 0x0028E2E0
		public static bool ShouldBlockCarSpawning(RunModeKind mode, string rankId, bool isAdminRank)
		{
			return !isAdminRank && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && GuidedRankGameplayPolicy.ShouldBlockCarInteractions(mode, rankId, false);
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x002900F8 File Offset: 0x0028E2F8
		public static bool ShouldBlockCarInteractions(RunModeKind mode, string rankId, bool isAdminRank)
		{
			return !isAdminRank && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && (!string.Equals(rankId, "aroused", StringComparison.Ordinal) && !string.Equals(rankId, "horny", StringComparison.Ordinal) && !string.Equals(rankId, "hornyslut", StringComparison.Ordinal)) && !string.Equals(rankId, "hornyslut_declared", StringComparison.Ordinal);
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x0029014C File Offset: 0x0028E34C
		public static bool ShouldBlockAdvancedMechanics(RunModeKind mode, string rankId, bool isAdminRank)
		{
			return !isAdminRank && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && !string.Equals(rankId, "hornyslut_declared", StringComparison.Ordinal);
		}
	}
}
