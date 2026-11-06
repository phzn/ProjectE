using System;
using Features.Rank;
using Features.RunModes;

namespace Features.Missions
{
	// Token: 0x02000FA1 RID: 4001
	public static class MissionRankPolicy
	{
		// Token: 0x0600786C RID: 30828 RVA: 0x0031255F File Offset: 0x0031075F
		public static bool IsAvailable(MissionObjectiveKind kind)
		{
			return MissionRankPolicy.IsAvailable(kind, RankProgressionService.CurrentRank, RunModeService.CurrentMode, RankProgressionService.IsAdminRankActive, RunModeService.DetectionEnabled, !RunModeService.UnlimitedClothingDurability, RunModeService.EncountersEnabled);
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x00312588 File Offset: 0x00310788
		public static bool IsAvailable(MissionObjectiveKind kind, RankDefinitionSO rank, RunModeKind mode, bool admin, bool detection = true, bool clothingDurability = true, bool encounters = true)
		{
			bool flag = !GuidedRankGameplayPolicy.ShouldBlockAdvancedMechanics(mode, (rank != null) ? rank.rankId : null, admin);
			bool flag2 = admin || (rank != null && rank.allowEmoteWheel);
			bool flag3 = flag2 && (admin || rank.allowMasturbationEmotes);
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				return detection;
			case MissionObjectiveKind.SuccessfulExhibitions:
			case MissionObjectiveKind.SpecificEmoteExhibition:
				return flag2 && detection;
			case MissionObjectiveKind.CarSurfs:
			case MissionObjectiveKind.VehicleDistance:
				return !GuidedRankGameplayPolicy.ShouldBlockCarInteractions(mode, (rank != null) ? rank.rankId : null, admin);
			case MissionObjectiveKind.Escapes:
				return detection && encounters;
			case MissionObjectiveKind.EmotesPerformed:
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.MovingEmoteDistance:
			case MissionObjectiveKind.EmoteNearLandmark:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return flag2;
			case MissionObjectiveKind.Orgasms:
				return flag3;
			case MissionObjectiveKind.TrashcanSearches:
			case MissionObjectiveKind.TrashcanLootItems:
			case MissionObjectiveKind.ItemsCollected:
			case MissionObjectiveKind.ClothesCollected:
			case MissionObjectiveKind.RareLootCollected:
			case MissionObjectiveKind.ClothesWorn:
			case MissionObjectiveKind.SpecificClothingWorn:
			case MissionObjectiveKind.WearableSpecialAction:
				return flag;
			case MissionObjectiveKind.SpecificClothingExhibition:
				return flag && flag2 && detection;
			case MissionObjectiveKind.SpecificClothingEmote:
				return flag && flag2;
			case MissionObjectiveKind.ClothingBroken:
				return flag && clothingDurability && !admin;
			case MissionObjectiveKind.SafeHomeReturns:
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return mode == RunModeKind.Classic || mode == RunModeKind.Guided;
			case MissionObjectiveKind.FlashTopSeenTime:
			case MissionObjectiveKind.FlashBottomSeenTime:
			case MissionObjectiveKind.FlashAnySeenTime:
			case MissionObjectiveKind.FlashBothSeenTime:
				return detection && (admin || (rank != null && (rank.allowLingerieOpenAnimations || rank.allowFlash)));
			case MissionObjectiveKind.StimulationTopSeenTime:
			case MissionObjectiveKind.StimulationBottomSeenTime:
			case MissionObjectiveKind.StimulationAnySeenTime:
				return detection && (admin || (rank != null && rank.allowCoverPose && rank.allowLingerieOpenAnimations));
			case MissionObjectiveKind.BralessSeenTime:
				return detection && (admin || (rank != null && (rank.allowBraRemoval || rank.allowFullUndress)));
			case MissionObjectiveKind.HoldBoobsSeenTime:
				return detection && (admin || (rank != null && rank.allowCoverPose && (rank.allowBraRemoval || rank.allowFullUndress)));
			case MissionObjectiveKind.TowelFlashSeenTime:
				return detection && (admin || (rank != null && rank.allowFlash));
			case MissionObjectiveKind.TowelDropSeen:
				return detection && (admin || (rank != null && rank.allowFullUndress));
			case MissionObjectiveKind.StatuePoseSeenTime:
			case MissionObjectiveKind.StatuePoseAudience:
			case MissionObjectiveKind.PaintedStatueSeenTime:
			case MissionObjectiveKind.StatuePoseCollection:
				return detection;
			case MissionObjectiveKind.StadiumScreenEmotes:
				return flag2;
			case MissionObjectiveKind.SeatedStudentsEmoteAudience:
				return flag2 && detection;
			case MissionObjectiveKind.UniformSkirtSeenTime:
				return detection && (admin || (rank != null && rank.allowFlash));
			case MissionObjectiveKind.KombiniToplessTheft:
			case MissionObjectiveKind.TreadmillToplessTime:
			case MissionObjectiveKind.SubwayToplessRides:
				return admin || (rank != null && (rank.allowBraRemoval || rank.allowFullUndress));
			case MissionObjectiveKind.GymShowerTime:
			case MissionObjectiveKind.NakedSwimDistance:
				return admin || (rank != null && rank.allowFullUndress);
			case MissionObjectiveKind.ShowerFoamSeenTime:
				return detection && (admin || (rank != null && rank.allowFullUndress));
			case MissionObjectiveKind.PoolScreenToplessSeenTime:
				return detection && (admin || (rank != null && (rank.allowBraRemoval || rank.allowFullUndress)));
			case MissionObjectiveKind.GrateWindSeenTime:
				return detection && (admin || (rank != null && (rank.allowLingerieOpenAnimations || rank.allowFlash)));
			case MissionObjectiveKind.NpcPhotosTopless:
				return detection && (admin || (rank != null && (rank.allowBraRemoval || rank.allowFullUndress)));
			case MissionObjectiveKind.NpcPhotosNaked:
				return detection && (admin || (rank != null && rank.allowFullUndress));
			}
			return true;
		}

		// Token: 0x0600786E RID: 30830 RVA: 0x00312920 File Offset: 0x00310B20
		public static string PlaceContext(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.StatuePoseSeenTime:
			case MissionObjectiveKind.StatuePoseAudience:
			case MissionObjectiveKind.PaintedStatueSeenTime:
			case MissionObjectiveKind.StatuePoseCollection:
				return "ArtGallery";
			case MissionObjectiveKind.StadiumScreenExposedTime:
			case MissionObjectiveKind.StadiumScreenEmotes:
				return "Stadium";
			case MissionObjectiveKind.SeatedStudentsEmoteAudience:
			case MissionObjectiveKind.LedWallFaceBroadcast:
			case MissionObjectiveKind.UniformSkirtSeenTime:
				return "University";
			case MissionObjectiveKind.KombiniToplessTheft:
			case MissionObjectiveKind.GrateWindSeenTime:
			case MissionObjectiveKind.SubwayToplessRides:
				return "CITY";
			case MissionObjectiveKind.TreadmillToplessTime:
			case MissionObjectiveKind.GymShowerTime:
			case MissionObjectiveKind.ShowerFoamSeenTime:
				return "Mall";
			case MissionObjectiveKind.NakedSwimDistance:
			case MissionObjectiveKind.PoolScreenToplessSeenTime:
				return "Pool";
			default:
				return null;
			}
		}

		// Token: 0x0600786F RID: 30831 RVA: 0x003129A1 File Offset: 0x00310BA1
		public static bool IsPlaceObjective(MissionObjectiveKind kind)
		{
			return MissionRankPolicy.PlaceContext(kind) != null;
		}

		// Token: 0x06007870 RID: 30832 RVA: 0x003129AC File Offset: 0x00310BAC
		public static bool IsBestAttempt(MissionObjectiveKind kind)
		{
			return kind == MissionObjectiveKind.SafeHomeReturnDistance || kind == MissionObjectiveKind.StatuePoseAudience || kind == MissionObjectiveKind.SeatedStudentsEmoteAudience;
		}

		// Token: 0x06007871 RID: 30833 RVA: 0x003129BF File Offset: 0x00310BBF
		public static bool IsRankSpecial(MissionObjectiveKind kind)
		{
			return MissionRankPolicy.RankSpecialTier(kind) > 0;
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x003129CA File Offset: 0x00310BCA
		public static int RankSpecialTier(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.FlashTopSeenTime:
			case MissionObjectiveKind.FlashBottomSeenTime:
			case MissionObjectiveKind.FlashAnySeenTime:
			case MissionObjectiveKind.FlashBothSeenTime:
			case MissionObjectiveKind.StimulationTopSeenTime:
			case MissionObjectiveKind.StimulationBottomSeenTime:
			case MissionObjectiveKind.StimulationAnySeenTime:
				return 1;
			case MissionObjectiveKind.BralessSeenTime:
			case MissionObjectiveKind.HoldBoobsSeenTime:
				return 2;
			case MissionObjectiveKind.TowelFlashSeenTime:
			case MissionObjectiveKind.TowelDropSeen:
				return 3;
			default:
				return 0;
			}
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x00312A0C File Offset: 0x00310C0C
		public static int RankSpecialFamily(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.FlashTopSeenTime:
			case MissionObjectiveKind.FlashBottomSeenTime:
			case MissionObjectiveKind.FlashAnySeenTime:
			case MissionObjectiveKind.FlashBothSeenTime:
				return 1;
			case MissionObjectiveKind.StimulationTopSeenTime:
			case MissionObjectiveKind.StimulationBottomSeenTime:
			case MissionObjectiveKind.StimulationAnySeenTime:
				return 2;
			case MissionObjectiveKind.BralessSeenTime:
				return 3;
			case MissionObjectiveKind.HoldBoobsSeenTime:
				return 4;
			case MissionObjectiveKind.TowelFlashSeenTime:
				return 5;
			case MissionObjectiveKind.TowelDropSeen:
				return 6;
			default:
				return 0;
			}
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x00312A60 File Offset: 0x00310C60
		public static bool IsSeconds(MissionObjectiveKind kind)
		{
			if (kind != MissionObjectiveKind.WallCoverTime)
			{
				switch (kind)
				{
				case MissionObjectiveKind.StatuePoseSeenTime:
				case MissionObjectiveKind.PaintedStatueSeenTime:
				case MissionObjectiveKind.StadiumScreenExposedTime:
				case MissionObjectiveKind.UniformSkirtSeenTime:
				case MissionObjectiveKind.TreadmillToplessTime:
				case MissionObjectiveKind.GymShowerTime:
				case MissionObjectiveKind.ShowerFoamSeenTime:
				case MissionObjectiveKind.PoolScreenToplessSeenTime:
				case MissionObjectiveKind.GrateWindSeenTime:
					return true;
				}
				return MissionRankPolicy.IsRankSpecial(kind) && kind != MissionObjectiveKind.TowelDropSeen;
			}
			return true;
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x00312AD1 File Offset: 0x00310CD1
		public static bool IsMeters(MissionObjectiveKind kind)
		{
			return kind == MissionObjectiveKind.VehicleDistance || kind == MissionObjectiveKind.MovingEmoteDistance || kind == MissionObjectiveKind.SafeHomeReturnDistance || kind == MissionObjectiveKind.NakedSwimDistance || kind == MissionObjectiveKind.UnicycleDistance;
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x00312AF0 File Offset: 0x00310CF0
		public static MissionCategory CategoryFor(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.PoiDiscoveries:
			case MissionObjectiveKind.SafeHomeReturns:
			case MissionObjectiveKind.SafeHomeReturnDistance:
			case MissionObjectiveKind.FirstFlight:
			case MissionObjectiveKind.NakedSwimDistance:
				return MissionCategory.Exploration;
			case MissionObjectiveKind.CarSurfs:
			case MissionObjectiveKind.VehicleDistance:
			case MissionObjectiveKind.SubwayToplessRides:
				return MissionCategory.Vehicles;
			case MissionObjectiveKind.EmotesPerformed:
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.Orgasms:
			case MissionObjectiveKind.MovingEmoteDistance:
			case MissionObjectiveKind.EmoteNearLandmark:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
			case MissionObjectiveKind.StatuePoseCollection:
			case MissionObjectiveKind.StadiumScreenEmotes:
				return MissionCategory.Emotes;
			case MissionObjectiveKind.TrashcanSearches:
			case MissionObjectiveKind.TrashcanLootItems:
			case MissionObjectiveKind.ItemsCollected:
			case MissionObjectiveKind.ClothesCollected:
			case MissionObjectiveKind.RareLootCollected:
				return MissionCategory.Items;
			case MissionObjectiveKind.ClothesWorn:
			case MissionObjectiveKind.SpecificClothingWorn:
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
			case MissionObjectiveKind.WearableSpecialAction:
			case MissionObjectiveKind.ClothingBroken:
				return MissionCategory.Wardrobe;
			case MissionObjectiveKind.WallCoverEntries:
			case MissionObjectiveKind.WallCoverTime:
				return MissionCategory.Stealth;
			case MissionObjectiveKind.GhostProtocol:
			case MissionObjectiveKind.GroupShot:
			case MissionObjectiveKind.UnicycleDistance:
			case MissionObjectiveKind.MostWantedHome:
			case MissionObjectiveKind.GhostScare:
				return MissionCategory.Secret;
			}
			return MissionCategory.Attention;
		}

		// Token: 0x06007877 RID: 30839 RVA: 0x00312C0C File Offset: 0x00310E0C
		public static MissionVarietyFamily VarietyFamily(MissionObjectiveKind kind)
		{
			if (MissionRankPolicy.IsRankSpecial(kind))
			{
				return MissionVarietyFamily.Exposure;
			}
			if (MissionRankPolicy.IsPlaceObjective(kind))
			{
				return MissionVarietyFamily.Place;
			}
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				break;
			case MissionObjectiveKind.SuccessfulExhibitions:
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificEmoteExhibition:
				return MissionVarietyFamily.Audience;
			case MissionObjectiveKind.PoiDiscoveries:
			case MissionObjectiveKind.ClothesWorn:
				return MissionVarietyFamily.None;
			case MissionObjectiveKind.CarSurfs:
			case MissionObjectiveKind.VehicleDistance:
				return MissionVarietyFamily.Vehicles;
			case MissionObjectiveKind.Escapes:
				return MissionVarietyFamily.Escape;
			case MissionObjectiveKind.ScoreEarned:
				return MissionVarietyFamily.Score;
			case MissionObjectiveKind.EmotesPerformed:
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.SpecificClothingEmote:
			case MissionObjectiveKind.MovingEmoteDistance:
			case MissionObjectiveKind.EmoteNearLandmark:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return MissionVarietyFamily.EmoteActs;
			case MissionObjectiveKind.Orgasms:
				return MissionVarietyFamily.Pleasure;
			case MissionObjectiveKind.TrashcanSearches:
			case MissionObjectiveKind.TrashcanLootItems:
			case MissionObjectiveKind.ItemsCollected:
			case MissionObjectiveKind.ClothesCollected:
			case MissionObjectiveKind.RareLootCollected:
				return MissionVarietyFamily.Loot;
			case MissionObjectiveKind.SpecificClothingWorn:
			case MissionObjectiveKind.WearableSpecialAction:
			case MissionObjectiveKind.ClothingBroken:
				return MissionVarietyFamily.Wardrobe;
			case MissionObjectiveKind.WallCoverEntries:
			case MissionObjectiveKind.WallCoverTime:
				return MissionVarietyFamily.Stealth;
			case MissionObjectiveKind.SafeHomeReturns:
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return MissionVarietyFamily.Home;
			default:
				if (kind - MissionObjectiveKind.NpcPhotosTopless > 1)
				{
					return MissionVarietyFamily.None;
				}
				break;
			}
			return MissionVarietyFamily.Photos;
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x00312CCC File Offset: 0x00310ECC
		public static bool IsEmoteLike(MissionObjectiveKind kind)
		{
			MissionVarietyFamily missionVarietyFamily = MissionRankPolicy.VarietyFamily(kind);
			if (missionVarietyFamily - MissionVarietyFamily.Audience <= 2 || missionVarietyFamily == MissionVarietyFamily.Exposure)
			{
				return true;
			}
			if (kind != MissionObjectiveKind.WearableSpecialAction)
			{
				switch (kind)
				{
				case MissionObjectiveKind.StatuePoseSeenTime:
				case MissionObjectiveKind.StatuePoseAudience:
				case MissionObjectiveKind.PaintedStatueSeenTime:
				case MissionObjectiveKind.StatuePoseCollection:
				case MissionObjectiveKind.StadiumScreenEmotes:
				case MissionObjectiveKind.SeatedStudentsEmoteAudience:
				case MissionObjectiveKind.UniformSkirtSeenTime:
					return true;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x00312D24 File Offset: 0x00310F24
		public static float VarietyWeight(MissionVarietyFamily family)
		{
			switch (family)
			{
			case MissionVarietyFamily.Photos:
				return 2f;
			case MissionVarietyFamily.Pleasure:
			case MissionVarietyFamily.Score:
				return 0.5f;
			case MissionVarietyFamily.Stealth:
				return 0.7f;
			case MissionVarietyFamily.Loot:
				return 1.2f;
			case MissionVarietyFamily.Wardrobe:
			case MissionVarietyFamily.Home:
				return 0.8f;
			}
			return 1f;
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x00312D8A File Offset: 0x00310F8A
		public static string CategoryName(MissionCategory category)
		{
			if (category == MissionCategory.Exploration)
			{
				return "EXPLORATION";
			}
			if (category != MissionCategory.Secret)
			{
				return category.ToString().ToUpperInvariant();
			}
			return "SECRET MISSIONS";
		}

		// Token: 0x04007BD7 RID: 31703
		public static readonly MissionObjectiveKind[] PlaceKinds = new MissionObjectiveKind[]
		{
			MissionObjectiveKind.StatuePoseSeenTime,
			MissionObjectiveKind.StatuePoseAudience,
			MissionObjectiveKind.PaintedStatueSeenTime,
			MissionObjectiveKind.StatuePoseCollection,
			MissionObjectiveKind.StadiumScreenExposedTime,
			MissionObjectiveKind.StadiumScreenEmotes,
			MissionObjectiveKind.SeatedStudentsEmoteAudience,
			MissionObjectiveKind.LedWallFaceBroadcast,
			MissionObjectiveKind.UniformSkirtSeenTime,
			MissionObjectiveKind.KombiniToplessTheft,
			MissionObjectiveKind.TreadmillToplessTime,
			MissionObjectiveKind.GymShowerTime,
			MissionObjectiveKind.ShowerFoamSeenTime,
			MissionObjectiveKind.NakedSwimDistance,
			MissionObjectiveKind.PoolScreenToplessSeenTime,
			MissionObjectiveKind.GrateWindSeenTime,
			MissionObjectiveKind.SubwayToplessRides
		};

		// Token: 0x04007BD8 RID: 31704
		public static readonly MissionObjectiveKind[] RankSpecialKinds = new MissionObjectiveKind[]
		{
			MissionObjectiveKind.FlashTopSeenTime,
			MissionObjectiveKind.FlashBottomSeenTime,
			MissionObjectiveKind.FlashAnySeenTime,
			MissionObjectiveKind.FlashBothSeenTime,
			MissionObjectiveKind.StimulationTopSeenTime,
			MissionObjectiveKind.StimulationBottomSeenTime,
			MissionObjectiveKind.StimulationAnySeenTime,
			MissionObjectiveKind.BralessSeenTime,
			MissionObjectiveKind.HoldBoobsSeenTime,
			MissionObjectiveKind.TowelFlashSeenTime,
			MissionObjectiveKind.TowelDropSeen
		};
	}
}
