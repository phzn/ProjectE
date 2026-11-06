using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200039C RID: 924
	public class UxmlStyleTraits : UxmlTraits
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x0007764C File Offset: 0x0007584C
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x04000CD9 RID: 3289
		private UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
		{
			name = "name"
		};

		// Token: 0x04000CDA RID: 3290
		private UxmlStringAttributeDescription m_Path = new UxmlStringAttributeDescription
		{
			name = "path"
		};

		// Token: 0x04000CDB RID: 3291
		private UxmlStringAttributeDescription m_Src = new UxmlStringAttributeDescription
		{
			name = "src"
		};
	}
}
