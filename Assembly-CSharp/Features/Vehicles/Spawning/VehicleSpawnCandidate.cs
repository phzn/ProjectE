using System;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B8D RID: 2957
	[Serializable]
	public struct VehicleSpawnCandidate
	{
		// Token: 0x040054D6 RID: 21718
		public GameObject prefab;

		// Token: 0x040054D7 RID: 21719
		[Min(0f)]
		public float weight;
	}
}
