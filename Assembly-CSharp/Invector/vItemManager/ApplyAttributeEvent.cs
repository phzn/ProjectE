using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005B7 RID: 1463
	[Serializable]
	public class ApplyAttributeEvent
	{
		// Token: 0x04002299 RID: 8857
		[SerializeField]
		public vItemAttributes attribute;

		// Token: 0x0400229A RID: 8858
		[SerializeField]
		public OnApplyAttribute onApplyAttribute;
	}
}
