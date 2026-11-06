using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000258 RID: 600
	public struct InternalWiggleProperties
	{
		// Token: 0x0400138E RID: 5006
		public Vector3 returnVector;

		// Token: 0x0400138F RID: 5007
		public Vector3 newValue;

		// Token: 0x04001390 RID: 5008
		public Vector3 initialValue;

		// Token: 0x04001391 RID: 5009
		public Vector3 startValue;

		// Token: 0x04001392 RID: 5010
		public float timeSinceLastChange;

		// Token: 0x04001393 RID: 5011
		public float randomFrequency;

		// Token: 0x04001394 RID: 5012
		public Vector3 randomNoiseFrequency;

		// Token: 0x04001395 RID: 5013
		public Vector3 randomAmplitude;

		// Token: 0x04001396 RID: 5014
		public Vector3 randomNoiseShift;

		// Token: 0x04001397 RID: 5015
		public float timeSinceLastPause;

		// Token: 0x04001398 RID: 5016
		public float pauseDuration;

		// Token: 0x04001399 RID: 5017
		public float noiseElapsedTime;

		// Token: 0x0400139A RID: 5018
		public Vector3 limitedTimeValueSave;

		// Token: 0x0400139B RID: 5019
		public Vector3 remapZero;

		// Token: 0x0400139C RID: 5020
		public Vector3 remapOne;

		// Token: 0x0400139D RID: 5021
		public float curveDirection;

		// Token: 0x0400139E RID: 5022
		public bool ping;
	}
}
