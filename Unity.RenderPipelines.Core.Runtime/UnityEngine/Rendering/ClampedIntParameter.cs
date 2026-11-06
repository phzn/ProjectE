using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F8 RID: 248
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class ClampedIntParameter : IntParameter
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00026C4C File Offset: 0x00024E4C
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x00026C54 File Offset: 0x00024E54
		public override int value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Clamp(value, this.min, this.max);
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00026C6E File Offset: 0x00024E6E
		public ClampedIntParameter(int value, int min, int max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040004DA RID: 1242
		[NonSerialized]
		public int min;

		// Token: 0x040004DB RID: 1243
		[NonSerialized]
		public int max;
	}
}
