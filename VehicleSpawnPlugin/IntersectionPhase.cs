using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class IntersectionPhase
	{
		// Token: 0x040000C9 RID: 201
		public string phaseName = "Phase";

		// Token: 0x040000CA RID: 202
		public List<ApproachTrigger> greenApproaches = new List<ApproachTrigger>();

		// Token: 0x040000CB RID: 203
		[Range(1f, 60f)]
		public float greenDuration = 6f;

		// Token: 0x040000CC RID: 204
		[Range(0f, 5f)]
		public float yellowDuration = 1.5f;
	}
}
