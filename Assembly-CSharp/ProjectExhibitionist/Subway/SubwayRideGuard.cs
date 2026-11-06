using System;
using System.Collections;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200093A RID: 2362
	public class SubwayRideGuard : MonoBehaviour
	{
		// Token: 0x06004047 RID: 16455 RVA: 0x0017DE2E File Offset: 0x0017C02E
		private static SubwayRideGuard Ensure()
		{
			if (SubwayRideGuard._instance != null)
			{
				return SubwayRideGuard._instance;
			}
			SubwayRideGuard._instance = new GameObject("SubwayRideGuard").AddComponent<SubwayRideGuard>();
			return SubwayRideGuard._instance;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0017DE5C File Offset: 0x0017C05C
		public static void Begin(SubwayRideDirector director)
		{
			if (director == null)
			{
				return;
			}
			SubwayRideGuard subwayRideGuard = SubwayRideGuard.Ensure();
			if (subwayRideGuard._routine != null)
			{
				subwayRideGuard.StopCoroutine(subwayRideGuard._routine);
			}
			subwayRideGuard._watched = director;
			subwayRideGuard._routine = subwayRideGuard.StartCoroutine(subwayRideGuard.Watch());
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0017DEA8 File Offset: 0x0017C0A8
		public static void End()
		{
			if (SubwayRideGuard._instance == null)
			{
				return;
			}
			if (SubwayRideGuard._instance._routine != null)
			{
				SubwayRideGuard._instance.StopCoroutine(SubwayRideGuard._instance._routine);
			}
			SubwayRideGuard._instance._routine = null;
			SubwayRideGuard._instance._watched = null;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0017DEF9 File Offset: 0x0017C0F9
		private IEnumerator Watch()
		{
			while (this._watched != null && this._watched.isActiveAndEnabled)
			{
				yield return null;
			}
			this._routine = null;
			this._watched = null;
			SubwayScreenFade.Fade(0f, 0.45f);
			SubwayCameraShaker.SetIntensity(0f);
			SubwayTravelService.CompleteTrip();
			Debug.LogWarning("[SubwayRideGuard] il treno della corsa e' sparito a meta' viaggio: riapro lo schermo e rilascio la destinazione senza aggiungere metri artificiali.");
			yield break;
		}

		// Token: 0x04004021 RID: 16417
		private static SubwayRideGuard _instance;

		// Token: 0x04004022 RID: 16418
		private SubwayRideDirector _watched;

		// Token: 0x04004023 RID: 16419
		private Coroutine _routine;
	}
}
