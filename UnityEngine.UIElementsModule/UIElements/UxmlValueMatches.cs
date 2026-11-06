using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D8 RID: 984
	public class UxmlValueMatches : UxmlTypeRestriction
	{
		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x00079D4E File Offset: 0x00077F4E
		// (set) Token: 0x0600203C RID: 8252 RVA: 0x00079D56 File Offset: 0x00077F56
		public string regex { get; set; }

		// Token: 0x0600203D RID: 8253 RVA: 0x00079D60 File Offset: 0x00077F60
		public override bool Equals(UxmlTypeRestriction other)
		{
			UxmlValueMatches uxmlValueMatches = other as UxmlValueMatches;
			bool flag = uxmlValueMatches == null;
			return !flag && this.regex == uxmlValueMatches.regex;
		}
	}
}
