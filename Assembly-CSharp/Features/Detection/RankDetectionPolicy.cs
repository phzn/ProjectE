using System;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012DB RID: 4827
	public static class RankDetectionPolicy
	{
		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x06009594 RID: 38292 RVA: 0x003D6900 File Offset: 0x003D4B00
		public static bool ProgressionTuningActive
		{
			get
			{
				if (Application.isPlaying)
				{
					RunModeKind currentMode = RunModeService.CurrentMode;
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					return RankDetectionPolicy.IsProgressionTuningActive(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
				}
				return false;
			}
		}

		// Token: 0x06009595 RID: 38293 RVA: 0x003D692C File Offset: 0x003D4B2C
		public static bool IsProgressionTuningActive(RunModeKind mode, string rankId, bool isAdminRank)
		{
			return !isAdminRank && (mode == RunModeKind.Classic || mode == RunModeKind.Guided) && (rankId == "shy" || rankId == "curious" || rankId == "aroused" || rankId == "horny" || rankId == "hornyslut");
		}

		// Token: 0x06009596 RID: 38294 RVA: 0x003D6988 File Offset: 0x003D4B88
		public static bool IsWearingStarterUnderwear(ClothingToggleController clothing)
		{
			return clothing != null && !clothing.IsFullyClothed && (clothing.IsWearingVisiblePreset("Outfit.Top", "top2") || clothing.IsWearingVisiblePreset("Outfit.Top", "expose_bra") || clothing.IsWearingVisiblePreset("Outfit.Underwear", "classic_panties") || clothing.IsWearingVisiblePreset("Outfit.Underwear", "expose_panties"));
		}

		// Token: 0x06009597 RID: 38295 RVA: 0x003D69F0 File Offset: 0x003D4BF0
		public static bool IsWearingTowel(ClothingToggleController clothing)
		{
			return clothing != null && clothing.IsWearingVisiblePreset("Outfit.Misc", "towel_bath");
		}

		// Token: 0x06009598 RID: 38296 RVA: 0x003D6A0D File Offset: 0x003D4C0D
		public static float ApplyClothingAttentionFloor(float clothingMultiplier, bool isDecentlyCovered, bool isWearingStarterUnderwear, bool progressionTuningActive)
		{
			if (!progressionTuningActive || !isWearingStarterUnderwear || isDecentlyCovered)
			{
				return clothingMultiplier;
			}
			return Mathf.Max(0.5f, clothingMultiplier);
		}

		// Token: 0x06009599 RID: 38297 RVA: 0x003D6A24 File Offset: 0x003D4C24
		public static float GetAttentionGainMultiplier(RunModeKind mode, string rankId, bool isAdminRank, bool isWearingStarterUnderwear, bool isWearingTowel)
		{
			if (!RankDetectionPolicy.IsProgressionTuningActive(mode, rankId, isAdminRank))
			{
				return 1f;
			}
			if (isWearingStarterUnderwear)
			{
				if (!(rankId == "curious"))
				{
					return 1.3f;
				}
				return 1.1f;
			}
			else
			{
				if (!isWearingTowel || !string.Equals(rankId, "horny", StringComparison.Ordinal))
				{
					return 1f;
				}
				return 1.2f;
			}
		}

		// Token: 0x0600959A RID: 38298 RVA: 0x003D6A7C File Offset: 0x003D4C7C
		public static float GetDirectedFlashAttentionMultiplier(bool exposureActionActive, bool hasLineOfSight, bool progressionTuningActive, Vector3 playerForward, Vector3 playerToObserver, string rankId = null)
		{
			if (!progressionTuningActive || !exposureActionActive || !hasLineOfSight)
			{
				return 1f;
			}
			playerForward.y = 0f;
			playerToObserver.y = 0f;
			if (playerForward.sqrMagnitude < 0.0001f || playerToObserver.sqrMagnitude < 0.0001f)
			{
				return 1f;
			}
			if (Vector3.Dot(playerForward.normalized, playerToObserver.normalized) < 0.5f)
			{
				return 1f;
			}
			if (!(rankId == "curious"))
			{
				return 1.5f;
			}
			return 1.05f;
		}

		// Token: 0x04009AB9 RID: 39609
		public const float ShyMinimumClothingAttentionMultiplier = 0.5f;

		// Token: 0x04009ABA RID: 39610
		public const float UnderwearAttentionGainMultiplier = 1.3f;

		// Token: 0x04009ABB RID: 39611
		public const float TowelAttentionGainMultiplier = 1.2f;

		// Token: 0x04009ABC RID: 39612
		public const float DirectedFlashAttentionGainMultiplier = 1.5f;

		// Token: 0x04009ABD RID: 39613
		public const float CuriousAttentionGainMultiplier = 1.1f;

		// Token: 0x04009ABE RID: 39614
		public const float CuriousDirectedAttentionGainMultiplier = 1.05f;
	}
}
