using System;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F5E RID: 3934
	[Serializable]
	public sealed class NippleRotorGripSettings
	{
		// Token: 0x040079B0 RID: 31152
		[Tooltip("Grip point in the LeftHand bone's axes, in Ciel mesh units (multiplied by the body scale).")]
		public Vector3 localPosition = new Vector3(0f, 0.052f, -0.017f);

		// Token: 0x040079B1 RID: 31153
		[Tooltip("Remote rotation relative to the LeftHand bone (degrees). (0,0,90): across the palm, cord end on the thumb side.")]
		public Vector3 localEuler = new Vector3(0f, 0f, 90f);

		// Token: 0x040079B2 RID: 31154
		[Tooltip("Remote world scale = body scale * this.")]
		public float scale = 0.8f;

		// Token: 0x040079B3 RID: 31155
		[Tooltip("+1 or -1: flip it if the fingers open instead of closing around the remote.")]
		public float curlSign = 1f;

		// Token: 0x040079B4 RID: 31156
		[Tooltip("Extra flexion in degrees on top of the animated pose. thumbCurl applies to every thumb segment.")]
		public float thumbCurl = 30f;

		// Token: 0x040079B5 RID: 31157
		[Tooltip("Extra flexion in degrees on top of the animated pose. thumbCurl applies to every thumb segment.")]
		public float proximalCurl = 50f;

		// Token: 0x040079B6 RID: 31158
		[Tooltip("Extra flexion in degrees on top of the animated pose. thumbCurl applies to every thumb segment.")]
		public float intermediateCurl = 65f;

		// Token: 0x040079B7 RID: 31159
		[Tooltip("Extra flexion in degrees on top of the animated pose. thumbCurl applies to every thumb segment.")]
		public float distalCurl = 45f;

		// Token: 0x040079B8 RID: 31160
		[Tooltip("Seconds to blend the finger curl in and out.")]
		public float curlBlendSeconds = 0.25f;
	}
}
