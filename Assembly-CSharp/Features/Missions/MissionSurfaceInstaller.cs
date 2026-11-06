using System;
using UnityEngine;

namespace Features.Missions
{
	// Token: 0x02000FB6 RID: 4022
	public static class MissionSurfaceInstaller
	{
		// Token: 0x06007971 RID: 31089 RVA: 0x0031976B File Offset: 0x0031796B
		public static void InstallCurrentScene()
		{
			MissionSurfaceInstaller.InstallRunHud();
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x00319774 File Offset: 0x00317974
		private static void InstallRunHud()
		{
			foreach (Canvas canvas in Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!(canvas == null) && !(canvas.name != "HUDCanvas"))
				{
					if (canvas.GetComponent<RunMissionsHUD>() == null)
					{
						canvas.gameObject.AddComponent<RunMissionsHUD>();
					}
					return;
				}
			}
		}
	}
}
