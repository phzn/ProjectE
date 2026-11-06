using System;
using Features.Loading;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PoiTravel;

namespace Features.Detection
{
	// Token: 0x02001279 RID: 4729
	public static class DetectionTransitionGate
	{
		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x060092FA RID: 37626 RVA: 0x003C5974 File Offset: 0x003C3B74
		public static bool IsBlocked
		{
			get
			{
				if (LongWayHomeRuntimeController.IsPreparing)
				{
					return true;
				}
				SceneTransitionManager instance = SceneTransitionManager.Instance;
				if (instance != null && instance.IsArrivalProtected)
				{
					return true;
				}
				LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
				if (existingInstance != null && existingInstance.IsLoadingActive)
				{
					return true;
				}
				GameSystemsBootstrapper instance2 = GameSystemsBootstrapper.Instance;
				return (instance2 != null && (!GameSystemsBootstrapper.GameplayReady || !instance2.GetFeatureEnabled(GameSystemsBootstrapper.RuntimeFeature.NPCs))) || PoiArrivalPresentation.IsPresentingInActiveScene;
			}
		}
	}
}
