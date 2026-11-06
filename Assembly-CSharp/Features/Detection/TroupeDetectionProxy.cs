using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012F7 RID: 4855
	[DisallowMultipleComponent]
	public class TroupeDetectionProxy : MonoBehaviour
	{
		// Token: 0x060096B6 RID: 38582 RVA: 0x003DE0FF File Offset: 0x003DC2FF
		public void Initialize(ReporterTroupe troupe)
		{
			this._troupe = troupe;
		}

		// Token: 0x060096B7 RID: 38583 RVA: 0x003DE108 File Offset: 0x003DC308
		public void HandleDetection(NPCSenses detector)
		{
			if (this._troupe == null)
			{
				Debug.LogWarning("[TroupeDetectionProxy] " + base.name + ": troupe reference è null, detection ignorata.");
				return;
			}
			this._troupe.OnMemberDetected(detector);
		}

		// Token: 0x04009BD0 RID: 39888
		private ReporterTroupe _troupe;
	}
}
