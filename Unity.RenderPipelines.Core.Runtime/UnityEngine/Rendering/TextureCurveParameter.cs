using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	public class TextureCurveParameter : VolumeParameter<TextureCurve>
	{
		// Token: 0x0600078B RID: 1931 RVA: 0x00024F45 File Offset: 0x00023145
		public TextureCurveParameter(TextureCurve value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00024F4F File Offset: 0x0002314F
		public override void Release()
		{
			this.m_Value.Release();
		}
	}
}
