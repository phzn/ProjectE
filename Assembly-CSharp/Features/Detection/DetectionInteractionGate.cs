using System;
using Features.GameOver;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;

namespace Features.Detection
{
	// Token: 0x02001270 RID: 4720
	public static class DetectionInteractionGate
	{
		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x060092D0 RID: 37584 RVA: 0x003C408D File Offset: 0x003C228D
		public static bool IsGameOver
		{
			get
			{
				return GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x060092D1 RID: 37585 RVA: 0x003C40A8 File Offset: 0x003C22A8
		public static bool IsAutonomousDetectionBlocked
		{
			get
			{
				return DetectionInteractionGate.IsBlocked || (SensoryManager.Instance != null && SensoryManager.Instance.IsPaused);
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x060092D2 RID: 37586 RVA: 0x003C40CC File Offset: 0x003C22CC
		public static bool IsBlocked
		{
			get
			{
				if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked || !RunModeService.EncountersEnabled || NPCSimulationPause.IsPaused || TimeFreezeController.IsAnyActive)
				{
					return true;
				}
				SensoryManager instance = SensoryManager.Instance;
				return instance != null && !instance.IsRunModeDetectionEnabled;
			}
		}
	}
}
