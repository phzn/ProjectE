using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x0200056F RID: 1391
	public class vItemEnumsList : ScriptableObject
	{
		// Token: 0x040021DD RID: 8669
		[SerializeField]
		[HideInInspector]
		public List<string> itemTypeEnumValues = new List<string>();

		// Token: 0x040021DE RID: 8670
		[SerializeField]
		[HideInInspector]
		public List<string> itemAttributesEnumValues = new List<string>();

		// Token: 0x040021DF RID: 8671
		[SerializeField]
		[HideInInspector]
		public List<string> itemTypeEnumFormats = new List<string>();

		// Token: 0x040021E0 RID: 8672
		[SerializeField]
		[HideInInspector]
		public List<string> itemAttributesEnumFormats = new List<string>();
	}
}
