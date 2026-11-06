using System;
using System.Collections.Generic;
using Features.Rank;
using Features.RunModes;
using UnityEngine;

namespace Features.Achievements
{
	// Token: 0x0200153C RID: 5436
	public static class AchievementService
	{
		// Token: 0x14000125 RID: 293
		// (add) Token: 0x0600A841 RID: 43073 RVA: 0x00463CB4 File Offset: 0x00461EB4
		// (remove) Token: 0x0600A842 RID: 43074 RVA: 0x00463CE8 File Offset: 0x00461EE8
		public static event Action AvailabilityChanged;

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x0600A843 RID: 43075 RVA: 0x00463D1B File Offset: 0x00461F1B
		public static bool IsAvailable
		{
			get
			{
				return AchievementService._backend != null && AchievementService._backend.IsAvailable;
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x0600A844 RID: 43076 RVA: 0x00463D30 File Offset: 0x00461F30
		public static string PlatformName
		{
			get
			{
				if (!AchievementService.IsAvailable)
				{
					return string.Empty;
				}
				return AchievementService._backend.PlatformName;
			}
		}

		// Token: 0x0600A845 RID: 43077 RVA: 0x00463D4C File Offset: 0x00461F4C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			AchievementService._backend = null;
			AchievementService.AvailabilityChanged = null;
			AchievementService.SessionKnownIds.Clear();
			for (int i = 0; i < AchievementIds.All.Count; i++)
			{
				AchievementService.SessionKnownIds.Add(AchievementIds.All[i]);
			}
		}

		// Token: 0x0600A846 RID: 43078 RVA: 0x00463D9A File Offset: 0x00461F9A
		public static void RegisterBackend(IAchievementBackend backend)
		{
			if (backend == null || AchievementService._backend == backend)
			{
				return;
			}
			AchievementService._backend = backend;
			AchievementService.SyncLocalUnlocks();
			Action availabilityChanged = AchievementService.AvailabilityChanged;
			if (availabilityChanged == null)
			{
				return;
			}
			availabilityChanged();
		}

		// Token: 0x0600A847 RID: 43079 RVA: 0x00463DC2 File Offset: 0x00461FC2
		public static void UnregisterBackend(IAchievementBackend backend)
		{
			if (AchievementService._backend != backend)
			{
				return;
			}
			AchievementService._backend = null;
			Action availabilityChanged = AchievementService.AvailabilityChanged;
			if (availabilityChanged == null)
			{
				return;
			}
			availabilityChanged();
		}

		// Token: 0x0600A848 RID: 43080 RVA: 0x00463DE2 File Offset: 0x00461FE2
		public static void NotifyAvailabilityChanged(IAchievementBackend backend)
		{
			if (AchievementService._backend != backend)
			{
				return;
			}
			AchievementService.SyncLocalUnlocks();
			Action availabilityChanged = AchievementService.AvailabilityChanged;
			if (availabilityChanged == null)
			{
				return;
			}
			availabilityChanged();
		}

		// Token: 0x0600A849 RID: 43081 RVA: 0x00463E04 File Offset: 0x00462004
		public static bool Unlock(string apiName)
		{
			if (RankProgressionService.SuppressProgressionRewards || RunModeService.IsFreeMode)
			{
				return false;
			}
			apiName = AchievementService.NormalizeApiName(apiName);
			if (apiName == null || AchievementIds.IsLegacy(apiName))
			{
				return false;
			}
			AchievementService.SessionKnownIds.Add(apiName);
			bool flag = !AchievementService.IsLocallyUnlocked(apiName);
			if (flag)
			{
				PlayerPrefs.SetInt(AchievementService.LocalKey(apiName), 1);
				PlayerPrefs.Save();
			}
			if (!AchievementService.IsAvailable)
			{
				return flag;
			}
			bool flag2 = AchievementService._backend.TryUnlock(apiName);
			if (!flag2)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[Achievements] ",
					AchievementService._backend.PlatformName,
					" rejected '",
					apiName,
					"'."
				}));
			}
			return flag || flag2;
		}

		// Token: 0x0600A84A RID: 43082 RVA: 0x00463EB4 File Offset: 0x004620B4
		public static bool IsLocallyUnlocked(string apiName)
		{
			apiName = AchievementService.NormalizeApiName(apiName);
			return apiName != null && PlayerPrefs.GetInt(AchievementService.LocalKey(apiName), 0) != 0;
		}

		// Token: 0x0600A84B RID: 43083 RVA: 0x00463ED4 File Offset: 0x004620D4
		private static void SyncLocalUnlocks()
		{
			if (RankProgressionService.SuppressProgressionRewards || RunModeService.IsFreeMode)
			{
				return;
			}
			if (!AchievementService.IsAvailable)
			{
				return;
			}
			foreach (string text in AchievementService.SessionKnownIds)
			{
				if (AchievementService.IsLocallyUnlocked(text))
				{
					AchievementService._backend.TryUnlock(text);
				}
			}
		}

		// Token: 0x0600A84C RID: 43084 RVA: 0x00463F4C File Offset: 0x0046214C
		private static string NormalizeApiName(string apiName)
		{
			if (string.IsNullOrWhiteSpace(apiName))
			{
				return null;
			}
			return apiName.Trim();
		}

		// Token: 0x0600A84D RID: 43085 RVA: 0x00463F5E File Offset: 0x0046215E
		private static string LocalKey(string apiName)
		{
			return "PlatformAchievement.Unlocked." + apiName;
		}

		// Token: 0x0400AD10 RID: 44304
		private const string LocalUnlockPrefix = "PlatformAchievement.Unlocked.";

		// Token: 0x0400AD11 RID: 44305
		private static readonly HashSet<string> SessionKnownIds = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x0400AD12 RID: 44306
		private static IAchievementBackend _backend;
	}
}
