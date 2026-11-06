using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000104 RID: 260
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class ColorParameter : VolumeParameter<Color>
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x00026F16 File Offset: 0x00025116
		public ColorParameter(Color value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00026F2E File Offset: 0x0002512E
		public ColorParameter(Color value, bool hdr, bool showAlpha, bool showEyeDropper, bool overrideState = false)
			: base(value, overrideState)
		{
			this.hdr = hdr;
			this.showAlpha = showAlpha;
			this.showEyeDropper = showEyeDropper;
			this.overrideState = overrideState;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00026F68 File Offset: 0x00025168
		public override void Interp(Color from, Color to, float t)
		{
			this.m_Value.r = from.r + (to.r - from.r) * t;
			this.m_Value.g = from.g + (to.g - from.g) * t;
			this.m_Value.b = from.b + (to.b - from.b) * t;
			this.m_Value.a = from.a + (to.a - from.a) * t;
		}

		// Token: 0x040004EA RID: 1258
		[NonSerialized]
		public bool hdr;

		// Token: 0x040004EB RID: 1259
		[NonSerialized]
		public bool showAlpha = true;

		// Token: 0x040004EC RID: 1260
		[NonSerialized]
		public bool showEyeDropper = true;
	}
}
