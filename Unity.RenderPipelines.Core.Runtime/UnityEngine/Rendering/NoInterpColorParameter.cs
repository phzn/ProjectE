using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000105 RID: 261
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpColorParameter : VolumeParameter<Color>
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x00026FF9 File Offset: 0x000251F9
		public NoInterpColorParameter(Color value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00027011 File Offset: 0x00025211
		public NoInterpColorParameter(Color value, bool hdr, bool showAlpha, bool showEyeDropper, bool overrideState = false)
			: base(value, overrideState)
		{
			this.hdr = hdr;
			this.showAlpha = showAlpha;
			this.showEyeDropper = showEyeDropper;
			this.overrideState = overrideState;
		}

		// Token: 0x040004ED RID: 1261
		public bool hdr;

		// Token: 0x040004EE RID: 1262
		[NonSerialized]
		public bool showAlpha = true;

		// Token: 0x040004EF RID: 1263
		[NonSerialized]
		public bool showEyeDropper = true;
	}
}
