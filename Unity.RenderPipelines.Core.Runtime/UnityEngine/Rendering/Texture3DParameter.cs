using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200010F RID: 271
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class Texture3DParameter : VolumeParameter<Texture>
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x0002729E File Offset: 0x0002549E
		public Texture3DParameter(Texture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000272A8 File Offset: 0x000254A8
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
