using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200039F RID: 927
	public class UxmlTemplateTraits : UxmlTraits
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x00077798 File Offset: 0x00075998
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x04000CE1 RID: 3297
		private UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
		{
			name = "name",
			use = UxmlAttributeDescription.Use.Required
		};

		// Token: 0x04000CE2 RID: 3298
		private UxmlStringAttributeDescription m_Path = new UxmlStringAttributeDescription
		{
			name = "path"
		};

		// Token: 0x04000CE3 RID: 3299
		private UxmlStringAttributeDescription m_Src = new UxmlStringAttributeDescription
		{
			name = "src"
		};
	}
}
