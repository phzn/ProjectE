using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000399 RID: 921
	public class UxmlRootElementTraits : UxmlTraits
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x000774B8 File Offset: 0x000756B8
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield return new UxmlChildElementDescription(typeof(VisualElement));
				yield break;
			}
		}

		// Token: 0x04000CD2 RID: 3282
		protected UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
		{
			name = "name"
		};

		// Token: 0x04000CD3 RID: 3283
		private UxmlStringAttributeDescription m_Class = new UxmlStringAttributeDescription
		{
			name = "class"
		};
	}
}
