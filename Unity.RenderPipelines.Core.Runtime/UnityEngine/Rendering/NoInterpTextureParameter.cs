using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200010D RID: 269
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpTextureParameter : VolumeParameter<Texture>
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00027226 File Offset: 0x00025426
		public NoInterpTextureParameter(Texture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00027230 File Offset: 0x00025430
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.value != null)
			{
				num = 23 * CoreUtils.GetTextureHash(this.value);
			}
			return num;
		}
	}
}
