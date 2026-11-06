using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000763 RID: 1891
	public class StampPlacer : MonoBehaviour, IPlaceStamp
	{
		// Token: 0x060030EE RID: 12526 RVA: 0x001139D8 File Offset: 0x00111BD8
		public void PlaceStamp(MudStamp stamp = null)
		{
			if (stamp)
			{
				stamp.StampSurface();
				return;
			}
			MudStamp mudStamp;
			if (this.Stamp.TryGetComponent<MudStamp>(out mudStamp))
			{
				mudStamp.StampSurface();
			}
		}

		// Token: 0x04003158 RID: 12632
		public GameObject DynamicMudSurface;

		// Token: 0x04003159 RID: 12633
		public GameObject Stamp;
	}
}
