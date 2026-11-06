using System;
using UnityEngine;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000943 RID: 2371
	public static class PoiArrivalPresentationRequest
	{
		// Token: 0x06004094 RID: 16532 RVA: 0x0017F73E File Offset: 0x0017D93E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			PoiArrivalPresentationRequest._pendingSceneName = null;
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x0017F746 File Offset: 0x0017D946
		public static void RequestForScene(string sceneName)
		{
			PoiArrivalPresentationRequest._pendingSceneName = (string.IsNullOrWhiteSpace(sceneName) ? null : sceneName);
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x0017F759 File Offset: 0x0017D959
		internal static bool TryConsume(string sceneName)
		{
			if (!string.Equals(PoiArrivalPresentationRequest._pendingSceneName, sceneName, StringComparison.Ordinal))
			{
				return false;
			}
			PoiArrivalPresentationRequest._pendingSceneName = null;
			return true;
		}

		// Token: 0x04004062 RID: 16482
		private static string _pendingSceneName;
	}
}
