using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200010C RID: 268
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class TextureParameter : VolumeParameter<Texture>
	{
		// Token: 0x06000839 RID: 2105 RVA: 0x000271D7 File Offset: 0x000253D7
		public TextureParameter(Texture value, bool overrideState = false)
			: this(value, TextureDimension.Any, overrideState)
		{
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000271E2 File Offset: 0x000253E2
		public TextureParameter(Texture value, TextureDimension dimension, bool overrideState = false)
			: base(value, overrideState)
		{
			this.dimension = dimension;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000271F4 File Offset: 0x000253F4
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.value != null)
			{
				num = 23 * CoreUtils.GetTextureHash(this.value);
			}
			return num;
		}

		// Token: 0x040004F0 RID: 1264
		public TextureDimension dimension;
	}
}
