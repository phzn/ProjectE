using System;
using Features.RunModes;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.Rank
{
	// Token: 0x02000DA8 RID: 3496
	public static class RankNightController
	{
		// Token: 0x060066F0 RID: 26352 RVA: 0x00292B00 File Offset: 0x00290D00
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			RankNightController.Release();
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x00292B08 File Offset: 0x00290D08
		public static void Apply(RankDefinitionSO rank, RankProgressionConfigSO config)
		{
			AzureCoreSystem azureCoreSystem = Object.FindObjectOfType<AzureCoreSystem>(true);
			string name = SceneManager.GetActiveScene().name;
			if (name == "CielHome")
			{
				DynamicWorldEnvironmentController.ApplyHomeRank(rank, config);
				return;
			}
			if (DynamicWorldEnvironmentController.IsSupportedWorldScene(name) && !DynamicWorldEnvironmentController.IsEnvironmentReady(azureCoreSystem))
			{
				RankNightController.ConfigureNightVisibility(azureCoreSystem, config);
				return;
			}
			RankNightController.ApplyForScene(rank, config, azureCoreSystem, name, RunModeService.CurrentMode);
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x00292B65 File Offset: 0x00290D65
		public static void ApplyForScene(RankDefinitionSO rank, RankProgressionConfigSO config, AzureCoreSystem azure, string sceneName, RunModeKind mode)
		{
			if (RankNightController.ShouldLockNight(rank, sceneName, mode))
			{
				RankNightController.Apply(rank, config, azure);
				return;
			}
			RankNightController.Release();
			RankNightController.ConfigureNightVisibility(azure, config);
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x00292B87 File Offset: 0x00290D87
		public static bool ShouldLockNight(RankDefinitionSO rank, string sceneName, RunModeKind mode)
		{
			return !(rank == null) && rank.nightOnly && (sceneName == "CielHome" || (mode != RunModeKind.Free && mode != RunModeKind.LongWayHome && DynamicWorldEnvironmentController.IsSupportedWorldScene(sceneName)));
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x00292BBC File Offset: 0x00290DBC
		public static void Apply(RankDefinitionSO rank, RankProgressionConfigSO config, AzureCoreSystem azure)
		{
			if (rank == null || !rank.nightOnly)
			{
				RankNightController.Release();
				RankNightController.ConfigureNightVisibility(azure, config);
				return;
			}
			if (azure == null || azure.timeSystem == null)
			{
				return;
			}
			if (RankNightController._lockedAzure != azure)
			{
				RankNightController.Release();
				RankNightController._lockedAzure = azure;
				RankNightController._previousDayLength = azure.timeSystem.dayLength;
				RankNightController._previousStartTime = azure.timeSystem.startTime;
			}
			RankNightController._lockedHour = RankNightController.ResolveNightHour(config, azure.timeSystem);
			azure.timeSystem.startTime = RankNightController._lockedHour;
			azure.timeSystem.dayLength = 0f;
			azure.timeSystem.timeline = RankNightController._lockedHour;
			RankNightController.ConfigureNightVisibility(azure, config);
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x00292C7C File Offset: 0x00290E7C
		private static float ResolveNightHour(RankProgressionConfigSO config, AzureTimeSystem time)
		{
			float num = ((config != null) ? config.forcedNightHour : 22f);
			float num2 = Mathf.Repeat(time.dawnTime - time.duskTime, 24f);
			float num3 = Mathf.Repeat(time.duskTime + 1f, 24f);
			float num4 = Mathf.Repeat(time.dawnTime - 1f, 24f);
			if (float.IsNaN(num) || float.IsInfinity(num) || !NightVisibilityController.IsNight(num, num3, num4))
			{
				return Mathf.Repeat(time.duskTime + num2 * 0.5f, 24f);
			}
			return Mathf.Repeat(num, 24f);
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x00292D24 File Offset: 0x00290F24
		public static void Release()
		{
			if (RankNightController._lockedAzure != null && RankNightController._lockedAzure.timeSystem != null)
			{
				AzureTimeSystem timeSystem = RankNightController._lockedAzure.timeSystem;
				if (Mathf.Approximately(timeSystem.dayLength, 0f))
				{
					timeSystem.dayLength = RankNightController._previousDayLength;
				}
				if (Mathf.Approximately(timeSystem.startTime, RankNightController._lockedHour))
				{
					timeSystem.startTime = RankNightController._previousStartTime;
				}
			}
			RankNightController._lockedAzure = null;
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x00292D98 File Offset: 0x00290F98
		private static void ConfigureNightVisibility(AzureCoreSystem azure, RankProgressionConfigSO config)
		{
			if (azure == null)
			{
				return;
			}
			(azure.GetComponent<NightVisibilityController>() ?? azure.gameObject.AddComponent<NightVisibilityController>()).Configure(azure, (config != null) ? config.nightVisibilityStartHour : 18.5f, (config != null) ? config.nightVisibilityEndHour : 6f, (config != null) ? config.nightAmbientFillColor : new Color(0.06f, 0.08f, 0.13f, 1f), (config != null) ? config.minimumNightAmbientIntensity : 0.55f, (config != null) ? config.minimumNightDirectionalIntensity : 0.25f);
		}

		// Token: 0x04006996 RID: 27030
		private static AzureCoreSystem _lockedAzure;

		// Token: 0x04006997 RID: 27031
		private static float _previousDayLength;

		// Token: 0x04006998 RID: 27032
		private static float _previousStartTime;

		// Token: 0x04006999 RID: 27033
		private static float _lockedHour;
	}
}
