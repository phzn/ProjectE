using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x020003DA RID: 986
	public class UxmlEnumeration : UxmlTypeRestriction
	{
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x00079E50 File Offset: 0x00078050
		// (set) Token: 0x0600204A RID: 8266 RVA: 0x00079E68 File Offset: 0x00078068
		public IEnumerable<string> values
		{
			get
			{
				return this.m_Values;
			}
			set
			{
				this.m_Values = value.ToList<string>();
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00079E78 File Offset: 0x00078078
		public override bool Equals(UxmlTypeRestriction other)
		{
			UxmlEnumeration uxmlEnumeration = other as UxmlEnumeration;
			bool flag = uxmlEnumeration == null;
			return !flag && this.values.All<string>(new Func<string, bool>(uxmlEnumeration.values.Contains<string>)) && this.values.Count<string>() == uxmlEnumeration.values.Count<string>();
		}

		// Token: 0x04000D4C RID: 3404
		private List<string> m_Values = new List<string>();
	}
}
