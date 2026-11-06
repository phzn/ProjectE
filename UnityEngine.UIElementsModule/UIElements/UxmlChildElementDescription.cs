using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003C4 RID: 964
	public class UxmlChildElementDescription
	{
		// Token: 0x06001FD6 RID: 8150 RVA: 0x00078F98 File Offset: 0x00077198
		public UxmlChildElementDescription(Type t)
		{
			bool flag = t == null;
			if (flag)
			{
				throw new ArgumentNullException("t");
			}
			this.elementName = t.Name;
			this.elementNamespace = t.Namespace;
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x00078FDE File Offset: 0x000771DE
		// (set) Token: 0x06001FD8 RID: 8152 RVA: 0x00078FE6 File Offset: 0x000771E6
		public string elementName { get; protected set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x00078FEF File Offset: 0x000771EF
		// (set) Token: 0x06001FDA RID: 8154 RVA: 0x00078FF7 File Offset: 0x000771F7
		public string elementNamespace { get; protected set; }
	}
}
