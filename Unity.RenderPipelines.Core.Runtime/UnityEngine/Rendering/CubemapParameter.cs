using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000112 RID: 274
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class CubemapParameter : VolumeParameter<Texture>
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x00027352 File Offset: 0x00025552
		public CubemapParameter(Texture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002735C File Offset: 0x0002555C
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
