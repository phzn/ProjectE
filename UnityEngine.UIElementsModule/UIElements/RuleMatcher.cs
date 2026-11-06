using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000382 RID: 898
	internal struct RuleMatcher
	{
		// Token: 0x06001E65 RID: 7781 RVA: 0x00075A6F File Offset: 0x00073C6F
		public RuleMatcher(StyleSheet sheet, StyleComplexSelector complexSelector, int styleSheetIndexInStack)
		{
			this.sheet = sheet;
			this.complexSelector = complexSelector;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00075A80 File Offset: 0x00073C80
		public override string ToString()
		{
			return this.complexSelector.ToString();
		}

		// Token: 0x04000CA3 RID: 3235
		public StyleSheet sheet;

		// Token: 0x04000CA4 RID: 3236
		public StyleComplexSelector complexSelector;
	}
}
