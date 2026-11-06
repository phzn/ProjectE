using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using Features.Rank;
using UnityEngine;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001484 RID: 5252
	public static class PoiFastTravelProgressService
	{
		// Token: 0x1400011B RID: 283
		// (add) Token: 0x0600A1D8 RID: 41432 RVA: 0x004338B0 File Offset: 0x00431AB0
		// (remove) Token: 0x0600A1D9 RID: 41433 RVA: 0x004338E4 File Offset: 0x00431AE4
		public static event Action Changed;

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600A1DA RID: 41434 RVA: 0x00433917 File Offset: 0x00431B17
		private static string FilePath
		{
			get
			{
				return Path.Combine(GameDataPaths.RuntimeRoot, "poi_fast_travel.json");
			}
		}

		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x0600A1DB RID: 41435 RVA: 0x00433928 File Offset: 0x00431B28
		private static PoiFastTravelProgressData Data
		{
			get
			{
				if (PoiFastTravelProgressService._data == null)
				{
					PoiFastTravelProgressService.Load();
				}
				return PoiFastTravelProgressService._data;
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x0600A1DC RID: 41436 RVA: 0x0043393B File Offset: 0x00431B3B
		public static IReadOnlyList<string> DestinationScenes
		{
			get
			{
				return PoiFastTravelProgressService.KnownScenes;
			}
		}

		// Token: 0x0600A1DD RID: 41437 RVA: 0x00433944 File Offset: 0x00431B44
		public static bool IsKnownDestination(string sceneName)
		{
			if (string.IsNullOrWhiteSpace(sceneName))
			{
				return false;
			}
			for (int i = 0; i < PoiFastTravelProgressService.KnownScenes.Length; i++)
			{
				if (string.Equals(PoiFastTravelProgressService.KnownScenes[i], sceneName, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A1DE RID: 41438 RVA: 0x00433980 File Offset: 0x00431B80
		public static int GetVisitCount(string sceneName)
		{
			PoiVisitProgress poiVisitProgress = PoiFastTravelProgressService.Find(sceneName);
			if (poiVisitProgress == null)
			{
				return 0;
			}
			return Mathf.Clamp(poiVisitProgress.visitCount, 0, 999);
		}

		// Token: 0x0600A1DF RID: 41439 RVA: 0x004339AA File Offset: 0x00431BAA
		public static bool IsDestinationUnlocked(string sceneName)
		{
			return PoiFastTravelProgressService.IsKnownDestination(sceneName) && (RankProgressionService.IsAdminRankActive || CreatorContentUnlocks.IsActive || PoiFastTravelProgressService.GetVisitCount(sceneName) >= 1);
		}

		// Token: 0x0600A1E0 RID: 41440 RVA: 0x004339D4 File Offset: 0x00431BD4
		public static int RecordNaturalVisit(string sceneName, string discoveryId)
		{
			if (!PoiFastTravelProgressService.IsKnownDestination(sceneName))
			{
				return 0;
			}
			if (RankProgressionService.SuppressProgressionRewards || string.IsNullOrWhiteSpace(discoveryId))
			{
				return PoiFastTravelProgressService.GetVisitCount(sceneName);
			}
			PoiVisitProgress poiVisitProgress = PoiFastTravelProgressService.Find(sceneName);
			if (poiVisitProgress == null)
			{
				poiVisitProgress = new PoiVisitProgress
				{
					sceneName = sceneName
				};
				PoiFastTravelProgressService.Data.destinations.Add(poiVisitProgress);
			}
			if (poiVisitProgress.visitCount >= 999 || poiVisitProgress.discoveredInstanceIds.Contains(discoveryId))
			{
				return poiVisitProgress.visitCount;
			}
			poiVisitProgress.discoveredInstanceIds.Add(discoveryId);
			poiVisitProgress.visitCount = Mathf.Clamp(poiVisitProgress.visitCount + 1, 0, 999);
			PoiFastTravelProgressService.Save();
			return poiVisitProgress.visitCount;
		}

		// Token: 0x0600A1E1 RID: 41441 RVA: 0x00433A7C File Offset: 0x00431C7C
		private static PoiVisitProgress Find(string sceneName)
		{
			List<PoiVisitProgress> destinations = PoiFastTravelProgressService.Data.destinations;
			for (int i = 0; i < destinations.Count; i++)
			{
				PoiVisitProgress poiVisitProgress = destinations[i];
				if (poiVisitProgress != null && string.Equals(poiVisitProgress.sceneName, sceneName, StringComparison.Ordinal))
				{
					return poiVisitProgress;
				}
			}
			return null;
		}

		// Token: 0x0600A1E2 RID: 41442 RVA: 0x00433AC4 File Offset: 0x00431CC4
		private static void Load()
		{
			try
			{
				PoiFastTravelProgressService._data = (File.Exists(PoiFastTravelProgressService.FilePath) ? JsonUtility.FromJson<PoiFastTravelProgressData>(File.ReadAllText(PoiFastTravelProgressService.FilePath)) : new PoiFastTravelProgressData());
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Maps] Discovery save could not be read; using a clean state: " + ex.Message);
				PoiFastTravelProgressService._data = new PoiFastTravelProgressData();
			}
			if (PoiFastTravelProgressService._data == null)
			{
				PoiFastTravelProgressService._data = new PoiFastTravelProgressData();
			}
			if (PoiFastTravelProgressService._data.destinations == null)
			{
				PoiFastTravelProgressService._data.destinations = new List<PoiVisitProgress>();
			}
			for (int i = PoiFastTravelProgressService._data.destinations.Count - 1; i >= 0; i--)
			{
				PoiVisitProgress poiVisitProgress = PoiFastTravelProgressService._data.destinations[i];
				if (poiVisitProgress == null || !PoiFastTravelProgressService.IsKnownDestination(poiVisitProgress.sceneName))
				{
					PoiFastTravelProgressService._data.destinations.RemoveAt(i);
				}
				else
				{
					poiVisitProgress.visitCount = Mathf.Clamp(poiVisitProgress.visitCount, 0, 999);
					if (poiVisitProgress.discoveredInstanceIds == null)
					{
						poiVisitProgress.discoveredInstanceIds = new List<string>();
					}
					HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
					poiVisitProgress.discoveredInstanceIds.RemoveAll((string id) => string.IsNullOrWhiteSpace(id) || !seen.Add(id));
					if (poiVisitProgress.discoveredInstanceIds.Count > 999)
					{
						poiVisitProgress.discoveredInstanceIds.RemoveRange(999, poiVisitProgress.discoveredInstanceIds.Count - 999);
					}
					poiVisitProgress.visitCount = Mathf.Max(poiVisitProgress.visitCount, poiVisitProgress.discoveredInstanceIds.Count);
				}
			}
		}

		// Token: 0x0600A1E3 RID: 41443 RVA: 0x00433C58 File Offset: 0x00431E58
		private static void Save()
		{
			try
			{
				GameDataPaths.EnsureRuntimeDirectories();
				File.WriteAllText(PoiFastTravelProgressService.FilePath, JsonUtility.ToJson(PoiFastTravelProgressService.Data, true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Maps] Discovery save could not be written: " + ex.Message);
			}
			Action changed = PoiFastTravelProgressService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x0400A57D RID: 42365
		public const int VisitsRequired = 1;

		// Token: 0x0400A57E RID: 42366
		private const int MaximumStoredVisits = 999;

		// Token: 0x0400A57F RID: 42367
		private const string SaveFileName = "poi_fast_travel.json";

		// Token: 0x0400A580 RID: 42368
		private static readonly string[] KnownScenes = new string[] { "POI - University", "POI - Stadium", "POI - Mall", "POI - Art Gallery", "POI - Pool" };

		// Token: 0x0400A581 RID: 42369
		private static PoiFastTravelProgressData _data;
	}
}
