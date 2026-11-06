using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010E8 RID: 4328
	public interface IInventoryInstanceVisualProvider
	{
		// Token: 0x0600833D RID: 33597
		void ConfigureHandInstance(GameObject instance);

		// Token: 0x0600833E RID: 33598
		void ConfigureWorldInstance(GameObject instance);
	}
}
