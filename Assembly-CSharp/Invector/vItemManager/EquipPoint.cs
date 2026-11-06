using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005B5 RID: 1461
	[Serializable]
	public class EquipPoint
	{
		// Token: 0x04002292 RID: 8850
		[SerializeField]
		public string equipPointName;

		// Token: 0x04002293 RID: 8851
		public EquipmentReference equipmentReference = new EquipmentReference();

		// Token: 0x04002294 RID: 8852
		[HideInInspector]
		public vEquipArea area;

		// Token: 0x04002295 RID: 8853
		public vHandler handler = new vHandler();

		// Token: 0x04002296 RID: 8854
		public OnInstantiateItemObjectEvent onInstantiateEquiment = new OnInstantiateItemObjectEvent();
	}
}
