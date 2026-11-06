using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003A2 RID: 930
	public class UxmlAttributeOverridesTraits : UxmlTraits
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x000778EC File Offset: 0x00075AEC
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x04000CE9 RID: 3305
		internal const string k_ElementNameAttributeName = "element-name";

		// Token: 0x04000CEA RID: 3306
		private UxmlStringAttributeDescription m_ElementName = new UxmlStringAttributeDescription
		{
			name = "element-name",
			use = UxmlAttributeDescription.Use.Required
		};
	}
}
