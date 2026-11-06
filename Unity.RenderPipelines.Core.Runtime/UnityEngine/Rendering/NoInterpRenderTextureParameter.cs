using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000111 RID: 273
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpRenderTextureParameter : VolumeParameter<RenderTexture>
	{
		// Token: 0x06000844 RID: 2116 RVA: 0x00027316 File Offset: 0x00025516
		public NoInterpRenderTextureParameter(RenderTexture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00027320 File Offset: 0x00025520
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
