using System;
using Features.Detection;

namespace Features.Inventory
{
	// Token: 0x020010E7 RID: 4327
	public static class GameplayDurationPause
	{
		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x0600833C RID: 33596 RVA: 0x00358D44 File Offset: 0x00356F44
		public static bool IsPaused
		{
			get
			{
				if (WardrobeGameplayContext.IsPresentationOnly(null))
				{
					return true;
				}
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				return instance != null && instance.isActiveAndEnabled && instance.IsEventActive;
			}
		}
	}
}
