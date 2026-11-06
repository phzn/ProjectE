using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000110 RID: 272
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class RenderTextureParameter : VolumeParameter<RenderTexture>
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x000272DA File Offset: 0x000254DA
		public RenderTextureParameter(RenderTexture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000272E4 File Offset: 0x000254E4
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
