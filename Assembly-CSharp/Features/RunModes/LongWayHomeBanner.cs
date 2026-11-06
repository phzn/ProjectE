using System;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CAE RID: 3246
	public sealed class LongWayHomeBanner : MonoBehaviour
	{
		// Token: 0x06005E47 RID: 24135 RVA: 0x00254630 File Offset: 0x00252830
		public static void Show(Transform parent, string title, string subtitle)
		{
			LongWayHomeBanner componentInChildren = parent.GetComponentInChildren<LongWayHomeBanner>(true);
			if (componentInChildren != null)
			{
				componentInChildren.gameObject.SetActive(false);
				Object.Destroy(componentInChildren.gameObject);
			}
			PoiArrivalCatalogSO poiArrivalCatalogSO = Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog");
			float num = 0f;
			PoiArrivalCatalogSO.Zone zone;
			if (poiArrivalCatalogSO != null && poiArrivalCatalogSO.TryGetZone("Game - Current", out zone))
			{
				num = zone.titleHoldDurationOverride;
			}
			PoiArrivalPresentation poiArrivalPresentation = PoiArrivalPresentation.CreateTitleOnly(parent, title, (poiArrivalCatalogSO != null) ? poiArrivalCatalogSO.TitleFont : null, num);
			poiArrivalPresentation.gameObject.AddComponent<LongWayHomeBanner>();
			poiArrivalPresentation.ShowTitleOnly();
		}
	}
}
