using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D9 RID: 985
	public class UxmlValueBounds : UxmlTypeRestriction
	{
		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00079DA0 File Offset: 0x00077FA0
		// (set) Token: 0x06002040 RID: 8256 RVA: 0x00079DA8 File Offset: 0x00077FA8
		public string min { get; set; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x00079DB1 File Offset: 0x00077FB1
		// (set) Token: 0x06002042 RID: 8258 RVA: 0x00079DB9 File Offset: 0x00077FB9
		public string max { get; set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00079DC2 File Offset: 0x00077FC2
		// (set) Token: 0x06002044 RID: 8260 RVA: 0x00079DCA File Offset: 0x00077FCA
		public bool excludeMin { get; set; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x00079DD3 File Offset: 0x00077FD3
		// (set) Token: 0x06002046 RID: 8262 RVA: 0x00079DDB File Offset: 0x00077FDB
		public bool excludeMax { get; set; }

		// Token: 0x06002047 RID: 8263 RVA: 0x00079DE4 File Offset: 0x00077FE4
		public override bool Equals(UxmlTypeRestriction other)
		{
			UxmlValueBounds uxmlValueBounds = other as UxmlValueBounds;
			bool flag = uxmlValueBounds == null;
			return !flag && (this.min == uxmlValueBounds.min && this.max == uxmlValueBounds.max && this.excludeMin == uxmlValueBounds.excludeMin) && this.excludeMax == uxmlValueBounds.excludeMax;
		}
	}
}
