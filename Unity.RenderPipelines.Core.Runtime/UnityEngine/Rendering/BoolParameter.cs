using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F0 RID: 240
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class BoolParameter : VolumeParameter<bool>
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x00026B4D File Offset: 0x00024D4D
		public BoolParameter(bool value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00026B57 File Offset: 0x00024D57
		public BoolParameter(bool value, BoolParameter.DisplayType displayType, bool overrideState = false)
			: base(value, overrideState)
		{
			this.displayType = displayType;
		}

		// Token: 0x040004D5 RID: 1237
		[NonSerialized]
		public BoolParameter.DisplayType displayType;

		// Token: 0x020001D9 RID: 473
		public enum DisplayType
		{
			// Token: 0x040007AC RID: 1964
			Checkbox,
			// Token: 0x040007AD RID: 1965
			EnumPopup
		}
	}
}
