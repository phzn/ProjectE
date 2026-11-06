using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200010E RID: 270
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class Texture2DParameter : VolumeParameter<Texture>
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x00027262 File Offset: 0x00025462
		public Texture2DParameter(Texture value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002726C File Offset: 0x0002546C
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
