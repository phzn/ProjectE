using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200008F RID: 143
	[ExecuteAlways]
	[AddComponentMenu("Rendering/Probe Volume Touchup")]
	public class ProbeTouchupVolume : MonoBehaviour
	{
		// Token: 0x04000308 RID: 776
		[Range(0.0001f, 2f)]
		public float intensityScale = 1f;

		// Token: 0x04000309 RID: 777
		public bool invalidateProbes;

		// Token: 0x0400030A RID: 778
		public bool overrideDilationThreshold;

		// Token: 0x0400030B RID: 779
		[Range(0f, 0.99f)]
		public float overriddenDilationThreshold = 0.75f;

		// Token: 0x0400030C RID: 780
		public Vector3 size = new Vector3(1f, 1f, 1f);
	}
}
