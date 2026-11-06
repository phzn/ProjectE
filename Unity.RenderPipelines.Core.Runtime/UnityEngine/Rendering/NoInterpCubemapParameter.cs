using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000113 RID: 275
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpCubemapParameter : VolumeParameter<Cubemap>
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0002738E File Offset: 0x0002558E
		public NoInterpCubemapParameter(Cubemap value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00027398 File Offset: 0x00025598
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
