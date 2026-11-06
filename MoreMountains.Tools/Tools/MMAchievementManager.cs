using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000172 RID: 370
	[ExecuteAlways]
	public static class MMAchievementManager
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00028B0A File Offset: 0x00026D0A
		public static List<MMAchievement> AchievementsList
		{
			get
			{
				return MMAchievementManager._achievements;
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00028B14 File Offset: 0x00026D14
		public static void LoadAchievementList(MMAchievementList achievementList)
		{
			MMAchievementManager._achievements = new List<MMAchievement>();
			if (achievementList == null)
			{
				return;
			}
			MMAchievementManager.ListID = achievementList.AchievementsListID;
			foreach (MMAchievement mmachievement in achievementList.Achievements)
			{
				MMAchievementManager._achievements.Add(mmachievement.Copy());
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00028B90 File Offset: 0x00026D90
		public static void UnlockAchievement(string achievementID)
		{
			MMAchievementManager._achievement = MMAchievementManager.AchievementManagerContains(achievementID);
			if (MMAchievementManager._achievement != null)
			{
				MMAchievementManager._achievement.UnlockAchievement();
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00028BAE File Offset: 0x00026DAE
		public static void LockAchievement(string achievementID)
		{
			MMAchievementManager._achievement = MMAchievementManager.AchievementManagerContains(achievementID);
			if (MMAchievementManager._achievement != null)
			{
				MMAchievementManager._achievement.LockAchievement();
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00028BCC File Offset: 0x00026DCC
		public static void AddProgress(string achievementID, int newProgress)
		{
			MMAchievementManager._achievement = MMAchievementManager.AchievementManagerContains(achievementID);
			if (MMAchievementManager._achievement != null)
			{
				MMAchievementManager._achievement.AddProgress(newProgress);
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00028BEB File Offset: 0x00026DEB
		public static void SetProgress(string achievementID, int newProgress)
		{
			MMAchievementManager._achievement = MMAchievementManager.AchievementManagerContains(achievementID);
			if (MMAchievementManager._achievement != null)
			{
				MMAchievementManager._achievement.SetProgress(newProgress);
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00028C0C File Offset: 0x00026E0C
		private static MMAchievement AchievementManagerContains(string searchedID)
		{
			if (MMAchievementManager._achievements.Count == 0)
			{
				return null;
			}
			foreach (MMAchievement mmachievement in MMAchievementManager._achievements)
			{
				if (mmachievement.AchievementID == searchedID)
				{
					return mmachievement;
				}
			}
			return null;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00028C7C File Offset: 0x00026E7C
		public static void ResetAchievements(string listID)
		{
			if (MMAchievementManager._achievements != null)
			{
				foreach (MMAchievement mmachievement in MMAchievementManager._achievements)
				{
					mmachievement.ProgressCurrent = 0;
					mmachievement.UnlockedStatus = false;
				}
			}
			MMAchievementManager.DeterminePath(listID);
			MMSaveLoadManager.DeleteSave(MMAchievementManager.SaveFileName + MMAchievementManager._saveFileExtension, MMAchievementManager._saveFolderName);
			Debug.LogFormat("Achievements Reset", Array.Empty<object>());
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00028D08 File Offset: 0x00026F08
		public static void ResetAllAchievements()
		{
			MMAchievementManager.ResetAchievements(MMAchievementManager.ListID);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00028D14 File Offset: 0x00026F14
		public static void LoadSavedAchievements()
		{
			MMAchievementManager.DeterminePath("");
			MMAchievementManager.ExtractSerializedMMAchievementManager((SerializedMMAchievementManager)MMSaveLoadManager.Load(typeof(SerializedMMAchievementManager), MMAchievementManager.SaveFileName + MMAchievementManager._saveFileExtension, MMAchievementManager._saveFolderName));
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00028D4D File Offset: 0x00026F4D
		public static void SaveAchievements()
		{
			MMAchievementManager.DeterminePath("");
			SerializedMMAchievementManager serializedMMAchievementManager = new SerializedMMAchievementManager();
			MMAchievementManager.FillSerializedMMAchievementManager(serializedMMAchievementManager);
			MMSaveLoadManager.Save(serializedMMAchievementManager, MMAchievementManager.SaveFileName + MMAchievementManager._saveFileExtension, MMAchievementManager._saveFolderName);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00028D80 File Offset: 0x00026F80
		private static void DeterminePath(string specifiedFileName = "")
		{
			string text = ((!string.IsNullOrEmpty(MMAchievementManager.ListID)) ? MMAchievementManager.ListID : MMAchievementManager._defaultFileName);
			if (!string.IsNullOrEmpty(specifiedFileName))
			{
				text = specifiedFileName;
			}
			MMAchievementManager.SaveFileName = text;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00028DB8 File Offset: 0x00026FB8
		public static void FillSerializedMMAchievementManager(SerializedMMAchievementManager serializedAchievements)
		{
			serializedAchievements.Achievements = new SerializedMMAchievement[MMAchievementManager._achievements.Count];
			for (int i = 0; i < MMAchievementManager._achievements.Count<MMAchievement>(); i++)
			{
				SerializedMMAchievement serializedMMAchievement = new SerializedMMAchievement(MMAchievementManager._achievements[i].AchievementID, MMAchievementManager._achievements[i].UnlockedStatus, MMAchievementManager._achievements[i].ProgressCurrent);
				serializedAchievements.Achievements[i] = serializedMMAchievement;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00028E30 File Offset: 0x00027030
		public static void ExtractSerializedMMAchievementManager(SerializedMMAchievementManager serializedAchievements)
		{
			if (serializedAchievements == null)
			{
				return;
			}
			for (int i = 0; i < MMAchievementManager._achievements.Count<MMAchievement>(); i++)
			{
				for (int j = 0; j < serializedAchievements.Achievements.Length; j++)
				{
					if (MMAchievementManager._achievements[i].AchievementID == serializedAchievements.Achievements[j].AchievementID)
					{
						MMAchievementManager._achievements[i].UnlockedStatus = serializedAchievements.Achievements[j].UnlockedStatus;
						MMAchievementManager._achievements[i].ProgressCurrent = serializedAchievements.Achievements[j].ProgressCurrent;
					}
				}
			}
		}

		// Token: 0x04000A56 RID: 2646
		private static List<MMAchievement> _achievements;

		// Token: 0x04000A57 RID: 2647
		private static MMAchievement _achievement = null;

		// Token: 0x04000A58 RID: 2648
		public static string _defaultFileName = "Achievements";

		// Token: 0x04000A59 RID: 2649
		public static string _saveFolderName = "MMAchievements/";

		// Token: 0x04000A5A RID: 2650
		public static string _saveFileExtension = ".achievements";

		// Token: 0x04000A5B RID: 2651
		public static string SaveFileName;

		// Token: 0x04000A5C RID: 2652
		public static string ListID;
	}
}
