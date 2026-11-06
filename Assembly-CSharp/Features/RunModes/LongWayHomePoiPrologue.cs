using System;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CDC RID: 3292
	public static class LongWayHomePoiPrologue
	{
		// Token: 0x06005FA4 RID: 24484 RVA: 0x0025E21C File Offset: 0x0025C41C
		public static ILongWayHomePoiProloguePresenter GetPresenterForScene(Scene scene)
		{
			ILongWayHomePoiProloguePresenter presenterForScene = LongWayHomeGymShowerSequence.GetPresenterForScene(scene);
			if (presenterForScene != null)
			{
				return presenterForScene;
			}
			ILongWayHomePoiProloguePresenter presenterForScene2 = LongWayHomeUniversitySequence.GetPresenterForScene(scene);
			return presenterForScene2 ?? LongWayHomeStadiumSequence.GetPresenterForScene(scene);
		}
	}
}
