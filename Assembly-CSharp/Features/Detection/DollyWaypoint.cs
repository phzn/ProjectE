using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001245 RID: 4677
	[Serializable]
	public class DollyWaypoint
	{
		// Token: 0x0400959E RID: 38302
		[Tooltip("Position offset from target")]
		public Vector3 PositionOffset = new Vector3(0f, 1.5f, -2f);

		// Token: 0x0400959F RID: 38303
		[Tooltip("Time in seconds to reach this waypoint from the previous one")]
		[Range(0.5f, 10f)]
		public float TransitDuration = 2f;

		// Token: 0x040095A0 RID: 38304
		[Tooltip("Optional hold time at this waypoint before moving to next")]
		[Range(0f, 5f)]
		public float HoldDuration;
	}
}
