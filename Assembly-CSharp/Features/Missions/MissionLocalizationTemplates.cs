using System;

namespace Features.Missions
{
	// Token: 0x02000F96 RID: 3990
	internal static class MissionLocalizationTemplates
	{
		// Token: 0x0600785D RID: 30813 RVA: 0x00311998 File Offset: 0x0030FB98
		internal static string Full(MissionObjectiveKind kind, bool singular)
		{
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				if (!singular)
				{
					return "Be photographed {0} times by NPCs";
				}
				return "Be photographed {0} time by NPCs";
			case MissionObjectiveKind.SuccessfulExhibitions:
				if (!singular)
				{
					return "Be seen by an NPC during {0} emotes";
				}
				return "Be seen by an NPC during {0} emote";
			case MissionObjectiveKind.PoiDiscoveries:
				return "Discover {0} locations";
			case MissionObjectiveKind.CarSurfs:
				if (!singular)
				{
					return "Complete {0} car surfs";
				}
				return "Complete {0} car surf";
			case MissionObjectiveKind.Escapes:
				if (!singular)
				{
					return "Escape attention {0} times";
				}
				return "Escape attention {0} time";
			case MissionObjectiveKind.VehicleDistance:
				return "Travel {0}m with vehicles";
			case MissionObjectiveKind.ScoreEarned:
				return "Earn {0} score";
			case MissionObjectiveKind.EmotesPerformed:
				return "Perform {0} emotes";
			case MissionObjectiveKind.SpecificEmote:
				if (!singular)
				{
					return "Perform {1} {0} times";
				}
				return "Perform {1} {0} time";
			case MissionObjectiveKind.Orgasms:
				if (!singular)
				{
					return "Reach orgasm while masturbating {0} times";
				}
				return "Reach orgasm while masturbating {0} time";
			case MissionObjectiveKind.TrashcanSearches:
				if (!singular)
				{
					return "Loot {0} trashcans";
				}
				return "Loot {0} trashcan";
			case MissionObjectiveKind.TrashcanLootItems:
				return "Recover {0} items from trashcans";
			case MissionObjectiveKind.ItemsCollected:
				if (!singular)
				{
					return "Collect {0} objects";
				}
				return "Collect {0} object";
			case MissionObjectiveKind.ClothesCollected:
				if (!singular)
				{
					return "Collect {0} clothing items";
				}
				return "Collect {0} clothing item";
			case MissionObjectiveKind.RareLootCollected:
				if (!singular)
				{
					return "Find {0} rare or legendary items";
				}
				return "Find {0} rare or legendary item";
			case MissionObjectiveKind.ClothesWorn:
				return "Wear {0} clothing items";
			case MissionObjectiveKind.SpecificClothingWorn:
				return "Put on {1}";
			case MissionObjectiveKind.SpecificClothingExhibition:
				if (!singular)
				{
					return "Be seen by an NPC {0} times wearing {1}";
				}
				return "Be seen by an NPC {0} time wearing {1}";
			case MissionObjectiveKind.SpecificClothingEmote:
				if (!singular)
				{
					return "Perform {0} emotes wearing {1}";
				}
				return "Perform {0} emote wearing {1}";
			case MissionObjectiveKind.WearableSpecialAction:
				if (!singular)
				{
					return "Use {2} with {1} {0} times";
				}
				return "Use {2} with {1} {0} time";
			case MissionObjectiveKind.ClothingBroken:
				return "Wear out {1}";
			case MissionObjectiveKind.MovingEmoteDistance:
				return "Travel {0}m using {1}";
			case MissionObjectiveKind.SpecificEmoteExhibition:
				if (!singular)
				{
					return "Be seen by an NPC during {1} {0} times";
				}
				return "Be seen by an NPC during {1} {0} time";
			case MissionObjectiveKind.WallCoverEntries:
				if (!singular)
				{
					return "Use wall cover {0} times";
				}
				return "Use wall cover {0} time";
			case MissionObjectiveKind.WallCoverTime:
				return "Spend {0}s in wall cover";
			case MissionObjectiveKind.EmoteNearLandmark:
				if (!singular)
				{
					return "Perform {0} emotes near {1}";
				}
				return "Perform {0} emote near {1}";
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				if (!singular)
				{
					return "Perform {1} near {2} {0} times";
				}
				return "Perform {1} near {2} {0} time";
			case MissionObjectiveKind.SafeHomeReturns:
				if (!singular)
				{
					return "Return home safely from {0} Classic runs";
				}
				return "Return home safely to finish this Classic run";
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return "Reach {0:N0} m in one Classic run, then return home safely";
			case MissionObjectiveKind.FirstFlight:
				return "Fly for the first time";
			case MissionObjectiveKind.GhostProtocol:
				return "Ghost Protocol: return home from at least 1,000 m without ever being seen";
			case MissionObjectiveKind.GroupShot:
				return "Group Shot: get photographed by 5 different people within 5 seconds";
			case MissionObjectiveKind.UnicycleDistance:
				return "One Wheel Wonder: ride 500 m on a unicycle";
			case MissionObjectiveKind.MostWantedHome:
				return "Most Wanted, Still Home: get home with the police on your heels";
			case MissionObjectiveKind.GhostScare:
				return "Boo!: scare 5 people away in one run wearing the ghost costume";
			}
			return null;
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x00311C24 File Offset: 0x0030FE24
		internal static bool HasSingular(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
			case MissionObjectiveKind.SuccessfulExhibitions:
			case MissionObjectiveKind.CarSurfs:
			case MissionObjectiveKind.Escapes:
			case MissionObjectiveKind.SpecificEmote:
			case MissionObjectiveKind.Orgasms:
			case MissionObjectiveKind.TrashcanSearches:
			case MissionObjectiveKind.ItemsCollected:
			case MissionObjectiveKind.ClothesCollected:
			case MissionObjectiveKind.RareLootCollected:
			case MissionObjectiveKind.SpecificClothingExhibition:
			case MissionObjectiveKind.SpecificClothingEmote:
			case MissionObjectiveKind.WearableSpecialAction:
			case MissionObjectiveKind.SpecificEmoteExhibition:
			case MissionObjectiveKind.WallCoverEntries:
			case MissionObjectiveKind.EmoteNearLandmark:
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
			case MissionObjectiveKind.SafeHomeReturns:
				return true;
			}
			return false;
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x00311CAC File Offset: 0x0030FEAC
		internal static string RunFull(MissionObjectiveKind kind, bool singular)
		{
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				if (!singular)
				{
					return "Let NPCs see you exposed (not fully dressed) until they photograph or film you, {0} times";
				}
				return "Let an NPC see you exposed (not fully dressed) until they photograph or film you";
			case MissionObjectiveKind.SuccessfulExhibitions:
				if (!singular)
				{
					return "Perform {0} emotes from the emote wheel while an NPC sees you";
				}
				return "Perform an emote from the emote wheel while an NPC sees you";
			case MissionObjectiveKind.CarSurfs:
				if (!singular)
				{
					return "While exposed (not fully dressed), Climb Up on {0} stopped cars and ride each roof for a few seconds";
				}
				return "While exposed (not fully dressed), Climb Up on a stopped car and ride its roof for a few seconds";
			case MissionObjectiveKind.Escapes:
				if (!singular)
				{
					return "Let an NPC's attention marker fill halfway, then break sight or hide until it empties, {0} times";
				}
				return "Let an NPC's attention marker fill halfway, then break sight or hide until it empties";
			case MissionObjectiveKind.VehicleDistance:
				return "Travel {0:N0} m riding vehicles, or with a car via Climb Up or Hide Under";
			case MissionObjectiveKind.ScoreEarned:
				return "Earn {0:N0} score points: get photographed, escape NPCs, discover new places and more";
			case MissionObjectiveKind.EmotesPerformed:
				return "Perform {0} emotes of your choice from the emote wheel";
			case MissionObjectiveKind.SpecificEmote:
				if (!singular)
				{
					return "Perform {1} (emote wheel, gym station or pedestal) {0} times";
				}
				return "Perform {1} (emote wheel, gym station or pedestal) once";
			case MissionObjectiveKind.Orgasms:
				if (!singular)
				{
					return "Use a masturbation emote or a toy until the climax meter fills: reach orgasm {0} times";
				}
				return "Use a masturbation emote or a toy until the climax meter fills: reach orgasm once";
			case MissionObjectiveKind.TrashcanSearches:
				if (!singular)
				{
					return "Hold Search on {0} trashcans until each one drops an item";
				}
				return "Hold Search on a trashcan until it drops an item";
			case MissionObjectiveKind.TrashcanLootItems:
				return "Get {0} items to drop out of trashcans: hold Search until the bar is full";
			case MissionObjectiveKind.ItemsCollected:
				if (!singular)
				{
					return "Pick up {0} items you find (not props you hide behind), e.g. loot from searched trashcans";
				}
				return "Pick up an item you find (not a prop you hide behind), e.g. loot from a searched trashcan";
			case MissionObjectiveKind.ClothesCollected:
				if (!singular)
				{
					return "Search trashcans and pick up {0} different garments from their loot";
				}
				return "Search a trashcan and pick up a garment from its loot";
			case MissionObjectiveKind.RareLootCollected:
				if (!singular)
				{
					return "Pick up {0} Rare (blue) or Legendary (gold) items, e.g. from blue or gold trashcans";
				}
				return "Pick up a Rare (blue) or Legendary (gold) item, e.g. from a blue or gold trashcan";
			case MissionObjectiveKind.SpecificClothingWorn:
				return "Select {1} in your inventory and use Wear";
			case MissionObjectiveKind.SpecificClothingExhibition:
				if (!singular)
				{
					return "Wear {1} and let NPCs see you {0} times";
				}
				return "Wear {1} and let an NPC see you";
			case MissionObjectiveKind.SpecificClothingEmote:
				if (!singular)
				{
					return "Wear {1} and perform {0} emotes of your choice";
				}
				return "Wear {1} and perform an emote of your choice";
			case MissionObjectiveKind.WearableSpecialAction:
				if (!singular)
				{
					return "Wear and select {1} in your inventory, then use {2}, {0} times";
				}
				return "Wear and select {1} in your inventory, then use {2}";
			case MissionObjectiveKind.ClothingBroken:
				return "Keep {1} on until the garment's durability runs out and it breaks";
			case MissionObjectiveKind.MovingEmoteDistance:
				return "Start {1} from the emote wheel and move {0} m while it plays";
			case MissionObjectiveKind.SpecificEmoteExhibition:
				if (!singular)
				{
					return "Perform {1} (emote wheel, gym station or pedestal) while an NPC sees you, {0} times";
				}
				return "Perform {1} (emote wheel, gym station or pedestal) while an NPC sees you";
			case MissionObjectiveKind.WallCoverEntries:
				if (!singular)
				{
					return "Walk up to a wall and use Cover, then Leave and repeat: take wall cover {0} times";
				}
				return "Walk up to a wall and use Cover to take wall cover";
			case MissionObjectiveKind.WallCoverTime:
				return "Spend {0} s in total in wall cover: walk up to a wall and use Cover";
			case MissionObjectiveKind.EmoteNearLandmark:
				if (!singular)
				{
					return "Perform {0} emotes of your choice within a few metres of {1}";
				}
				return "Perform an emote of your choice within a few metres of {1}";
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				if (!singular)
				{
					return "Perform {1} from the emote wheel within a few metres of {2}, {0} times";
				}
				return "Perform {1} from the emote wheel within a few metres of {2}";
			case MissionObjectiveKind.SafeHomeReturns:
				if (!singular)
				{
					return "Finish {0} Classic runs at your front door: get at least 100 m from home, then use Return home";
				}
				return "Get at least 100 m from home (straight line), then end the run with Return home at your front door";
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return "Get {0:N0} m from home (straight line), then end the run with Return home at your front door";
			case MissionObjectiveKind.FlashTopSeenTime:
				return "Open a garment over your breasts (e.g. Expose Breasts) for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.FlashBottomSeenTime:
				return "Open a garment over your pussy (e.g. Expose Pussy) for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.FlashAnySeenTime:
				return "Open a garment over breasts or pussy (e.g. Expose Breasts) for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.FlashBothSeenTime:
				return "Open garments over both breasts and pussy at once for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.StimulationTopSeenTime:
				return "Bare your breasts and use the Stimulation pose on them for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.StimulationBottomSeenTime:
				return "Bare breasts and pussy, use the Stimulation pose and Cover lower body for {0} s while an NPC sees you";
			case MissionObjectiveKind.StimulationAnySeenTime:
				return "Bare your breasts and use the Stimulation pose for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.BralessSeenTime:
				return "Stay topless (bra and top taken off, not just opened) for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.HoldBoobsSeenTime:
				return "Go topless (bra and top off) and use the Hold Boobs pose for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.TowelFlashSeenTime:
				return "Wear and select your towel, use Open Towel and keep it open for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.TowelDropSeen:
				if (!singular)
				{
					return "Wear your towel closed and use Drop Towel while an NPC sees you, {0} times (wear it again in between)";
				}
				return "Wear your towel closed and use Drop Towel while an NPC sees you";
			case MissionObjectiveKind.StatuePoseSeenTime:
				return "Hold statue poses on a pedestal for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.StatuePoseAudience:
				return "Hold one statue pose on a pedestal, without switching, until {0} different NPCs have seen you";
			case MissionObjectiveKind.PaintedStatueSeenTime:
				return "Use the Tint bucket to get Tint Paint, then hold statue poses for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.StatuePoseCollection:
				return "Hold each of the {0} statue poses on a pedestal while an NPC sees you";
			case MissionObjectiveKind.StadiumScreenExposedTime:
				return "Spend {0} s in total exposed (not fully dressed) on the pitch, live on the big screen";
			case MissionObjectiveKind.StadiumScreenEmotes:
				if (!singular)
				{
					return "Perform {0} emotes on the pitch, live on the big screen";
				}
				return "Perform an emote on the pitch, live on the big screen";
			case MissionObjectiveKind.SeatedStudentsEmoteAudience:
				return "Perform an emote while exposed (not fully dressed) so that {0} seated students see you at once";
			case MissionObjectiveKind.LedWallFaceBroadcast:
				return "Put a Camera photo that shows your face on the Aula Magna LED wall with HackMe";
			case MissionObjectiveKind.UniformSkirtSeenTime:
				return "Use Lift Skirt on your uniform skirt for {0} s in total while a seated student sees you";
			case MissionObjectiveKind.KombiniToplessTheft:
				return "Go topless (bra and top off) and steal a vegetable from the Vegetable Crate inside the Kombini";
			case MissionObjectiveKind.TreadmillToplessTime:
				return "Go topless (bra and top off), then spend {0} s in total running on a gym treadmill";
			case MissionObjectiveKind.GymShowerTime:
				return "Get naked (shoes, socks and masks off too) and spend {0} s in total under a running gym shower";
			case MissionObjectiveKind.ShowerFoamSeenTime:
				return "After a gym shower, stay covered in foam for {0} s in total while an NPC outside the gym sees you";
			case MissionObjectiveKind.NakedSwimDistance:
				return "Get naked (shoes, socks and masks off too) and swim {0} m";
			case MissionObjectiveKind.PoolScreenToplessSeenTime:
				return "Go topless in or right by the pool, live on the big screen, for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.GrateWindSeenTime:
				return "Stand on a street grate in a uniform or cheerleader skirt for {0} s in total while an NPC sees you";
			case MissionObjectiveKind.SubwayToplessRides:
				if (!singular)
				{
					return "Ride the subway {0} times, topless from boarding until the doors open at your stop";
				}
				return "Ride the subway topless, from boarding until the doors open at your stop";
			case MissionObjectiveKind.NpcPhotosTopless:
				if (!singular)
				{
					return "Let NPCs see you topless (bra and top off) until they photograph or film you, {0} times";
				}
				return "Let an NPC see you topless (bra and top off) until they photograph or film you";
			case MissionObjectiveKind.NpcPhotosNaked:
				if (!singular)
				{
					return "Let NPCs see you naked (shoes, socks and masks off too) until they photograph or film you, {0} times";
				}
				return "Let an NPC see you naked (shoes, socks and masks off too) until they photograph or film you";
			}
			return null;
		}

		// Token: 0x06007860 RID: 30816 RVA: 0x00311FF0 File Offset: 0x003101F0
		internal static bool HasRunSingular(MissionObjectiveKind kind)
		{
			if (kind <= MissionObjectiveKind.TowelDropSeen)
			{
				switch (kind)
				{
				case MissionObjectiveKind.NpcPhotos:
				case MissionObjectiveKind.SuccessfulExhibitions:
				case MissionObjectiveKind.CarSurfs:
				case MissionObjectiveKind.Escapes:
				case MissionObjectiveKind.SpecificEmote:
				case MissionObjectiveKind.Orgasms:
				case MissionObjectiveKind.TrashcanSearches:
				case MissionObjectiveKind.ItemsCollected:
				case MissionObjectiveKind.ClothesCollected:
				case MissionObjectiveKind.RareLootCollected:
				case MissionObjectiveKind.SpecificClothingExhibition:
				case MissionObjectiveKind.SpecificClothingEmote:
				case MissionObjectiveKind.WearableSpecialAction:
				case MissionObjectiveKind.SpecificEmoteExhibition:
				case MissionObjectiveKind.WallCoverEntries:
				case MissionObjectiveKind.EmoteNearLandmark:
				case MissionObjectiveKind.SpecificEmoteNearLandmark:
				case MissionObjectiveKind.SafeHomeReturns:
					break;
				case MissionObjectiveKind.PoiDiscoveries:
				case MissionObjectiveKind.VehicleDistance:
				case MissionObjectiveKind.ScoreEarned:
				case MissionObjectiveKind.EmotesPerformed:
				case MissionObjectiveKind.TrashcanLootItems:
				case MissionObjectiveKind.ClothesWorn:
				case MissionObjectiveKind.SpecificClothingWorn:
				case MissionObjectiveKind.ClothingBroken:
				case MissionObjectiveKind.MovingEmoteDistance:
				case MissionObjectiveKind.WallCoverTime:
					return false;
				default:
					if (kind != MissionObjectiveKind.TowelDropSeen)
					{
						return false;
					}
					break;
				}
			}
			else if (kind != MissionObjectiveKind.StadiumScreenEmotes && kind != MissionObjectiveKind.SubwayToplessRides && kind - MissionObjectiveKind.NpcPhotosTopless > 1)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007861 RID: 30817 RVA: 0x00312094 File Offset: 0x00310294
		internal static string Hud(MissionObjectiveKind kind)
		{
			switch (kind)
			{
			case MissionObjectiveKind.NpcPhotos:
				return "Let NPCs see you exposed until they photograph or film you";
			case MissionObjectiveKind.SuccessfulExhibitions:
				return "Perform emotes (emote wheel) while an NPC sees you";
			case MissionObjectiveKind.CarSurfs:
				return "Climb Up on a stopped car and ride its roof while exposed";
			case MissionObjectiveKind.Escapes:
				return "Let an NPC's attention marker fill halfway, then break sight";
			case MissionObjectiveKind.VehicleDistance:
				return "Ride vehicles, or a car via Climb Up or Hide Under";
			case MissionObjectiveKind.ScoreEarned:
				return "Earn score: get photographed, escape NPCs, discover places";
			case MissionObjectiveKind.EmotesPerformed:
				return "Perform emotes of your choice (emote wheel)";
			case MissionObjectiveKind.SpecificEmote:
				return "Perform {0} (emote wheel, gym or pedestal)";
			case MissionObjectiveKind.Orgasms:
				return "Masturbate (emote or toy) until the climax meter fills";
			case MissionObjectiveKind.TrashcanSearches:
				return "Hold Search on trashcans until each one drops an item";
			case MissionObjectiveKind.TrashcanLootItems:
				return "Hold Search on trashcans to the end: every drop counts";
			case MissionObjectiveKind.ItemsCollected:
				return "Pick up items you find, e.g. loot from trashcans";
			case MissionObjectiveKind.ClothesCollected:
				return "Pick up different garments from trashcan loot";
			case MissionObjectiveKind.RareLootCollected:
				return "Pick up Rare (blue) or Legendary (gold) items";
			case MissionObjectiveKind.SpecificClothingWorn:
				return "Wear {0} from your inventory";
			case MissionObjectiveKind.SpecificClothingExhibition:
				return "Wear {0} and let NPCs see you";
			case MissionObjectiveKind.SpecificClothingEmote:
				return "Wear {0} and perform any emote";
			case MissionObjectiveKind.WearableSpecialAction:
				return "Wear {0} and use {1}";
			case MissionObjectiveKind.ClothingBroken:
				return "Keep {0} on until worn out";
			case MissionObjectiveKind.MovingEmoteDistance:
				return "Move while performing {0} (emote wheel)";
			case MissionObjectiveKind.SpecificEmoteExhibition:
				return "Perform {0} while an NPC sees you";
			case MissionObjectiveKind.WallCoverEntries:
				return "Take wall cover: walk up to a wall and use Cover";
			case MissionObjectiveKind.WallCoverTime:
				return "Stay in wall cover: walk up to a wall and use Cover";
			case MissionObjectiveKind.EmoteNearLandmark:
				return "Perform any emote close to {0}";
			case MissionObjectiveKind.SpecificEmoteNearLandmark:
				return "Perform {0} close to {1}";
			case MissionObjectiveKind.SafeHomeReturns:
				return "Get this far from home, then use Return home at your door";
			case MissionObjectiveKind.SafeHomeReturnDistance:
				return "Reach this distance, then use Return home at your door";
			case MissionObjectiveKind.FlashTopSeenTime:
				return "Open a garment over your breasts while an NPC sees you";
			case MissionObjectiveKind.FlashBottomSeenTime:
				return "Open a garment over your pussy while an NPC sees you";
			case MissionObjectiveKind.FlashAnySeenTime:
				return "Open a garment over breasts or pussy while an NPC sees you";
			case MissionObjectiveKind.FlashBothSeenTime:
				return "Open garments over both breasts and pussy while an NPC sees you";
			case MissionObjectiveKind.StimulationTopSeenTime:
				return "Use the Stimulation pose on your breasts while an NPC sees you";
			case MissionObjectiveKind.StimulationBottomSeenTime:
				return "Bare breasts + pussy; Stimulation + Cover lower body in NPC view";
			case MissionObjectiveKind.StimulationAnySeenTime:
				return "Use the Stimulation pose while an NPC sees you";
			case MissionObjectiveKind.BralessSeenTime:
				return "Go topless (bra and top off) while an NPC sees you";
			case MissionObjectiveKind.HoldBoobsSeenTime:
				return "Go topless and use the Hold Boobs pose while an NPC sees you";
			case MissionObjectiveKind.TowelFlashSeenTime:
				return "Use Open Towel and keep it open while an NPC sees you";
			case MissionObjectiveKind.TowelDropSeen:
				return "Use Drop Towel (towel closed) while an NPC sees you";
			case MissionObjectiveKind.StatuePoseSeenTime:
				return "Hold a statue pose on a pedestal while an NPC sees you";
			case MissionObjectiveKind.StatuePoseAudience:
				return "Hold one statue pose while different NPCs see you";
			case MissionObjectiveKind.PaintedStatueSeenTime:
				return "Use Tint, then hold a statue pose while an NPC sees you";
			case MissionObjectiveKind.StatuePoseCollection:
				return "Hold each statue pose on a pedestal while an NPC sees you";
			case MissionObjectiveKind.StadiumScreenExposedTime:
				return "Stay exposed on the pitch, live on the big screen";
			case MissionObjectiveKind.StadiumScreenEmotes:
				return "Perform emotes on the pitch, live on the big screen";
			case MissionObjectiveKind.SeatedStudentsEmoteAudience:
				return "Perform an emote while exposed in front of many seated students";
			case MissionObjectiveKind.LedWallFaceBroadcast:
				return "Use HackMe to put a photo of your face on the LED wall";
			case MissionObjectiveKind.UniformSkirtSeenTime:
				return "Use Lift Skirt on your uniform while a seated student sees you";
			case MissionObjectiveKind.KombiniToplessTheft:
				return "Go topless and steal from the Kombini's Vegetable Crate";
			case MissionObjectiveKind.TreadmillToplessTime:
				return "Go topless and run on a gym treadmill (Start exercise)";
			case MissionObjectiveKind.GymShowerTime:
				return "Get naked and take a shower in the gym";
			case MissionObjectiveKind.ShowerFoamSeenTime:
				return "After a gym shower, stay covered in foam while an NPC sees you";
			case MissionObjectiveKind.NakedSwimDistance:
				return "Get naked and swim in the pool";
			case MissionObjectiveKind.PoolScreenToplessSeenTime:
				return "Go topless in or by the pool while an NPC sees you";
			case MissionObjectiveKind.GrateWindSeenTime:
				return "Stand on a street grate in a skirt while an NPC sees you";
			case MissionObjectiveKind.SubwayToplessRides:
				return "Ride the subway topless, from boarding until the doors open";
			case MissionObjectiveKind.NpcPhotosTopless:
				return "Let NPCs see you topless until they photograph or film you";
			case MissionObjectiveKind.NpcPhotosNaked:
				return "Let NPCs see you naked until they photograph or film you";
			}
			return null;
		}
	}
}
